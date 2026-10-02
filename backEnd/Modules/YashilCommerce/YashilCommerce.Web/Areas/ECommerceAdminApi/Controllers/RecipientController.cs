using System;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.DynamicLinq;
using Yashil.Common.Core.Classes;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Common.Web.Infrastructure.Filters;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerce.ViewModels;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using YashilUserManagement.Core.Services;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public class RecipientController : BaseController<Recipient, int, RecipientListViewModel, RecipientEditModel, RecipientSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IRecipientService _recipientService;
        public IPermissionService PermissionService1 { get; }
        private readonly IUserPrincipal _userPrincipal;
        private readonly IAddressService _addressService;


        public RecipientController(
            IRecipientService recipientService,
            IMapper mapper,
            IYashilLogger<Recipient> logger,
            IPermissionService permissionService,
            IUserPrincipal userPrincipal,
            IAddressService addressService) : base(recipientService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _recipientService = recipientService;
            PermissionService1 = permissionService;
            _userPrincipal = userPrincipal;
            _addressService = addressService;
        }
        [HttpGet("GetByCustomFilterForList")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int? addressId, int? membershipType)
        {
            var recipients = _recipientService.GetByCustomFilter(_userPrincipal.Id, addressId, membershipType);
            return await DataSourceLoader.LoadAsync(recipients.ProjectTo<RecipientListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [HttpGet("GetByCustomForSelect")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions, int? addressId, int? membershipType)
        {
            var recipients = _recipientService.GetByCustomFilter(_userPrincipal.Id, addressId, membershipType);
            return await DataSourceLoader.LoadAsync(recipients.ProjectTo<RecipientSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        /// <summary>
        /// این متد در هنگام ویرایش یا درج recipient از سمت پنل عمومی فراخوانی میشود
        /// </summary>
        /// <param name="editModel"></param>
        /// <returns></returns>
        [HttpPost("AddOrUpdateRecipient")]
        public async Task<ResultStatus<int>> AddOrUpdateRecipient(RecipientPresentationEditModel editModel)
        {
            if (!ModelState.IsValid)
            {
                return new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = "لطفا ورودی های را کنترل کنید"
                };
            }

            //اگر رابطه تحویل گیرنده با صاحب کارت حکمت برابر صفر باشد 
            //مقداری غیر قابل پذیرش است برای کلید خارجی این فیلد پس برابر نال می کنیم
            //ممکن است در حالتی باشیم که کاربر غیر حکمتی است که آن را در سرویس چک میکنیم اگر غیر حکمتی باشد
            //مقدار نال مجاز است و در غیر این صورت حکمتی باشد باید مقداری صحییحی وارد شده باشد که در سرویس بررسی می شود
            if (editModel.RelationshipWithBuyerType.HasValue && editModel.RelationshipWithBuyerType.Value == 0)
                editModel.RelationshipWithBuyerType = null;

            var canConvertBirthDateStrToInt = Int32.TryParse(editModel.BirthDateStr, out var birthDateInt);
            if (!canConvertBirthDateStrToInt)
            {
                return new ResultStatus<int>(false, "لطفا تاریخ تولد خود را به درستی وارد کنید");
            }

            editModel.BirthDate = birthDateInt;
            var recipient = _mapper.Map<Recipient>(editModel);
            var address = _mapper.Map<Address>(editModel);
            var (isSuccess, message, recipientId) =
                await _recipientService.AddOrUpdateRecipientAsync(recipient, address, editModel.HasCardNumber);
            if (!isSuccess)
            {
                return new ResultStatus<int>(false, message);
            }
            return new ResultStatus<int>
            {
                IsSuccess = true,
                Message = String.Empty,
                EntityId = recipientId.Value
            };
        }

        protected override async Task CustomMapBeforeUpdateAsync(RecipientEditModel editModel, Recipient entity)
        {
            if (entity.Address != null)
            {
                await _addressService.UpdateAsync(entity.Address, entity.Address.Id);
            }

            await base.CustomMapBeforeUpdateAsync(editModel, entity);
        }

        protected override async Task<string> CustomValidateBeforeDeleteAsync(int id)
        {
            var recipientQuery = _recipientService.GetUserRecipient(id);
            if (!await recipientQuery.AnyAsync())
            {
                return "کاربر گرامی شما مجاز به انجام این عملیات نمی باشید.";
            }

            return await base.CustomValidateBeforeDeleteAsync(id);
        }
    }
}