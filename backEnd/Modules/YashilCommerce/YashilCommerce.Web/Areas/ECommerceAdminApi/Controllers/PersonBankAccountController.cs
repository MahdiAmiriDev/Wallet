using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quartz.Util;
using Yashil.Common.Core.Classes;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Common.Web.Infrastructure.Filters;
using Yashil.Core.CustomDomainModels.Commerce;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using Yashil.Infrastructure.Extensions;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using YashilUserManagement.Core.Services;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public class PersonBankAccountController : BaseController<PersonBankAccount, int, PersonBankAccountListViewModel, PersonBankAccountEditModel, PersonBankAccountSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IPersonBankAccountService _personBankAccountService;
        private readonly IFactorService _factorService;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IPersonService _personService;
        private readonly IRefundDetailService _refundDetailService;

        public PersonBankAccountController(IPersonBankAccountService personBankAccountService, IMapper mapper,
            IYashilLogger<PersonBankAccount> logger, IPermissionService permissionService,
            IFactorService factorService, IRefundDetailService refundDetailService,
            IUserPrincipal userPrincipal, IPersonService personService)
            : base(personBankAccountService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _factorService = factorService;
            _userPrincipal = userPrincipal;
            _personService = personService;
            _personBankAccountService = personBankAccountService;
            _refundDetailService = refundDetailService;
        }
        [HttpGet("GetByCustomFilterForList")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int? personId, int? bankType, int? representationId)
        {
            var personBankAccounts = _personBankAccountService.GetByCustomFilter(personId, bankType, representationId);
            return await DataSourceLoader.LoadAsync(personBankAccounts.ProjectTo<PersonBankAccountListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [HttpGet("GetByCustomForSelect")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions, int? personId, int? bankType, int? representationId)
        {
            var personBankAccounts = _personBankAccountService.GetByCustomFilter(personId, bankType, representationId);
            return await DataSourceLoader.LoadAsync(personBankAccounts.ProjectTo<PersonBankAccountSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        protected override async Task CustomMapBeforeInsertAsync(PersonBankAccountEditModel editModel, PersonBankAccount entity)
        {
            if (!editModel.PersonId.HasValue || editModel.PersonId == 0)
            {
                var person = new Person
                {
                    Name = editModel.Name,
                    LastName = editModel.LastName,
                    MobileNumber = editModel.MobileNumber,
                    NationalCode = editModel.NationalCode,
                    CreateBy = _userPrincipal.Id,
                    CreationDate = DateTime.Now,
                    CreatorOrganizationId = _userPrincipal.OrganizationId,
                    ApplicationId = _userPrincipal.ApplicationId,
                    Gender = (int)GenderEnum.Man
                };
                await _personService.AddAsync(person);
                person.PersonBankAccount.Add(entity);
            }

            await base.CustomMapBeforeInsertAsync(editModel, entity);
        }

        protected override async Task<string> CustomValidateBeforeInsertAsync(PersonBankAccountEditModel editModel)
        {
            var message = ValidateCardAndSheba(editModel);
            if (!string.IsNullOrEmpty(message))
            {
                return message;
            }
            return await base.CustomValidateBeforeInsertAsync(editModel);
        }
        protected override async Task<string> CustomValidateBeforeUpdateAsync(PersonBankAccountEditModel editModel)
        {
            var existAnyFactorByPersonBankAccountId = await
                _factorService.CheckExistAnyFactorByPersonBankAccountIdAsync(editModel.Id);
            var personBankAccountInfo = await 
                _personBankAccountService.FindById(editModel.Id)
                    .Select(x => new { x.ShebaNumber , x.AccountNumber})
                    .FirstOrDefaultAsync();

            if (existAnyFactorByPersonBankAccountId &&
                !string.IsNullOrEmpty(personBankAccountInfo.ShebaNumber))
            {
                return "کاربر گرامی این اطلاعات حساب در  حال استفاده در فاکتور ها می باشد و امکان ویرایش آن وجود ندارد";
            }

            //اگر شناسه اطلاعات بانکی کاربر در فاکتوری استفاده شده باشد و 
            //کاربر اقدام به ویرایش شماره حساب خود کند جلوی این کار را می گیریم
            if (existAnyFactorByPersonBankAccountId && personBankAccountInfo.AccountNumber != editModel.AccountNumber)
                return "کاربر گرامی، اطلاعات حساب شما در حال استفاده در فاکتور ها می باشد و به همین دلیل امکان ویرایش شماره حساب وجود ندارد.";

            var existAnyRefundDetailByPersonBankAccountId = _refundDetailService.CheckExistAnyRefundDetailByPersonBankAccountId(editModel.Id);
            if (existAnyRefundDetailByPersonBankAccountId)
            {
                return "کاربر گرامی این اطلاعات حساب در حال استفاده در عودت وجه می باشد و امکان ویرایش آن وجود ندارد.";
            }
            if (editModel.PersonId == 0)
            {
                if (string.IsNullOrWhiteSpace(editModel.NationalCode) || string.IsNullOrWhiteSpace(editModel.Name) || string.IsNullOrWhiteSpace(editModel.LastName))
                {
                    return "لطفا اطلاعات شخص را وارد کنید";
                }
            }
            var message = ValidateCardAndSheba(editModel);
            if (!string.IsNullOrEmpty(message))
            {
                return message;
            }
            return await base.CustomValidateBeforeUpdateAsync(editModel);
        }
        private string ValidateCardAndSheba(PersonBankAccountEditModel editModel)
        {
            if (!editModel.CardNumber.IsNullOrWhiteSpace() && !editModel.CardNumber.IsValidCardNumber())
            {
                return "کاربر گرامی شماره کارت نامعتبر است";
            }
            if (editModel.ShebaNumber.IsNullOrWhiteSpace())
            {
                return "کاربر گرامی لطفا شماره شبا را تکمیل نمایید.";
            }
            if (!editModel.ShebaNumber.IsNullOrWhiteSpace() && !editModel.ShebaNumber.IsValidSheba())
            {
                return "کاربر گرامی شماره شبا نامعتبر است";
            }

            return null;
        }

        [HttpGet("GetPersonBankAccountInformation")]
        [YashilAuthorization(ActionName = "PersonBankAccountInformation")]
        public async Task<PersonBankAccountCustomDomainModel> GetPersonBankAccountInformation(string cardNumber)
        {
            return await _personBankAccountService.GetByCardNumberAsync(cardNumber);
        }

        protected override Task<PersonBankAccountEditModel> GetEntityAndMapToEditModelForEdit(int id)
        {
            var personBankAccount = _personBankAccountService.GetIncludePerson(id);
            return personBankAccount.ProjectTo<PersonBankAccountEditModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }
    }
}