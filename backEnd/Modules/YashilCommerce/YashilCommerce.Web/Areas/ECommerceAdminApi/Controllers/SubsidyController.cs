using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Core.Enums;
using Yashil.Common.Core.Classes;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.EntityFrameworkCore;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Common.Web.Infrastructure.Filters;
using YashilUserManagement.Core.Services;
using System.Text.Json;


namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public class SubsidyController : BaseController<Subsidy, int, SubsidyListViewModel, SubsidyEditModel,
        SubsidySimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly ISubsidyService _subsidyService;
        private readonly IOrderService _orderService;
        private readonly IActivityLogService _activityLogService;
        private readonly IUserPrincipal _userPrincipal;


        public SubsidyController(
            ISubsidyService subsidyService,
            IMapper mapper,
            IYashilLogger<Subsidy> logger,
            IPermissionService permissionService,
            IOrderService orderService,
            IActivityLogService activityLogService,
            IUserPrincipal userPrincipal) : base(subsidyService, mapper, logger,
            permissionService)
        {
            _mapper = mapper;
            _subsidyService = subsidyService;
            _orderService = orderService;
            _activityLogService = activityLogService;
            _userPrincipal = userPrincipal;
        }
        [HttpGet("GetByCustomFilterForList")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int? categoryGroupId, int? subsidyGroupId)
        {
            var refunds = _subsidyService.GetByCustomFilter(categoryGroupId, subsidyGroupId);
            return await DataSourceLoader.LoadAsync(refunds.ProjectTo<SubsidyListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        /// <summary>
        /// جهت تایید یارانه ساتا بوسیله سرویس ساتا
        /// </summary>
        /// <param name="orderViewModel"></param>
        /// <returns></returns>
        [HttpPost("ConfirmSubsidyBySata")]
        [YashilAuthorization(ActionName = "ConfirmSubsidyBySata")]
        public IActionResult ConfirmSubsidyBySata(OrderSimpleViewModel orderViewModel)
        {
            var result = _orderService.ConfirmSubsidyBySata(orderViewModel.Id);
            return Accepted(result);
        }

        /// <summary>
        /// تایید یارانه ساتا
        /// </summary>
        /// <param name="orderId">سفارش</param>
        /// <returns></returns>
        [HttpGet("VerifySataSubsidy")]
        [YashilAuthorization(ActionName = "VerifySataSubsidy")]
        public async Task<ResultStatus<int>> VerifySataSubsidy(int orderId)
        {
            var order = _orderService.Get(orderId, true);
            if (order != null)
            {
                // اگر نوع یارانه ساتا باشد
                // و در حالت ثبت اولیه باشد
                if (order.SubsidyType == (int)SubsidyTypesEnum.Sata && order.SubsidyStatusType == (int)SubsidyStatusTypesEnum.InitialRegistration)
                {
                    try
                    {
                        order.SubsidyStatusType = (int)SubsidyStatusTypesEnum.Confirmed;
                        await _orderService.UpdateAsync(order, order.Id, true);
                        return new ResultStatus<int>
                        {
                            IsSuccess = true,
                            Message = "یارانه تایید شد"
                        };
                    }
                    catch
                    {
                        return new ResultStatus<int>
                        {
                            IsSuccess = false,
                            Message = "خطا در بروزرسانی تایید یارانه"
                        };
                    }

                }
                else
                {
                    return new ResultStatus<int>
                    {
                        IsSuccess = false,
                        Message = "کاربر گرامی سفارش مورد نظر یارانه متعلق به ساتا ندارد"
                    };
                }

            }
            else
            {
                return new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = "سفارش مورد نظر یافت نشد"
                };
            }
        }
        /// <summary>
        /// رد یارانه ساتا
        /// </summary>
        /// <param name="orderId">سفارش</param>
        /// <returns></returns>
        [HttpGet("RejectionSataSubsidy")]
        [YashilAuthorization(ActionName = "RejectionSataSubsidy")]
        public async Task<ResultStatus<int>> RejectionSataSubsidy(int orderId)
        {
            var order = _orderService.Get(orderId, true);
            if (order != null)
            {
                // اگر نوع یارانه ساتا باشد
                // و در حالت ثبت اولیه باشد
                if (order.SubsidyType == (int)SubsidyTypesEnum.Sata && order.SubsidyStatusType == (int)SubsidyStatusTypesEnum.InitialRegistration)
                {
                    try
                    {
                        order.SubsidyStatusType = (int)SubsidyStatusTypesEnum.Rejected;
                        await _orderService.UpdateAsync(order, order.Id, true);
                        return new ResultStatus<int>
                        {
                            IsSuccess = true,
                            Message = "یارانه رد شد"
                        };
                    }
                    catch
                    {
                        return new ResultStatus<int>
                        {
                            IsSuccess = false,
                            Message = "خطا در بروزرسانی رد یارانه"
                        };
                    }

                }
                else
                {
                    return new ResultStatus<int>
                    {
                        IsSuccess = false,
                        Message = "کاربر گرامی سفارش مورد نظر یارانه متعلق به ساتا ندارد"
                    };
                }

            }
            else
            {
                return new ResultStatus<int>
                {
                    IsSuccess = false,
                    Message = "سفارش مورد نظر یافت نشد"
                };
            }
        }

        /// <summary>
        /// تنظیم یارانه قبلی برنامه را غیر فعال می کند 
        /// </summary>
        /// <param name="applicationId">شناسه برنامه</param>
        /// <returns></returns>
        private async Task DeactivateOldSubsidyByApplicationIdAsync(int applicationId)
        {
            await _subsidyService.DeactivateOldSubsidyByApplicationIdAsync(applicationId);
        }
        /// <summary>
        /// در صورتی که برای گروه دسته بندی انتخابی کاربر در گروه یارانه انتخابی کاربر
        /// ، یارانه فعال وجود داشته باشد
        /// به کاربر اجازه ثبت نمی دهد 
        /// </summary>
        /// <param name="editModel"></param>
        /// <returns></returns>
        protected override async Task<string> CustomValidateBeforeInsertAsync(SubsidyEditModel editModel)
        {
            var checkExistsActiveSubsidy = await
                CheckExistCategoryGroupActiveSubsidyBySubsidyGroupIdAsync
                    (editModel.CategoryGroupId, editModel.SubsidyGroupId);
            if (checkExistsActiveSubsidy.isExists && editModel.IsActive)
            {
                return checkExistsActiveSubsidy.message;
            }
            return await base.CustomValidateBeforeInsertAsync(editModel);
        }

        protected override async Task<string> CustomValidateBeforeUpdateAsync(SubsidyEditModel editModel)
        {
            var subsidy = await _subsidyService.FindById(editModel.Id)
                .FirstOrDefaultAsync();
            if (subsidy == null)
            {
                return "کاربر گرامی، رکوردی با شناسه ارسالی یافت نشد.";
            }
            if (subsidy.CategoryGroupId != editModel.CategoryGroupId)
            {
                var checkExistsActiveSubsidy = await
                    CheckExistCategoryGroupActiveSubsidyBySubsidyGroupIdAsync
                        (editModel.CategoryGroupId, editModel.SubsidyGroupId);
                if (checkExistsActiveSubsidy.isExists && editModel.IsActive)
                {
                    return checkExistsActiveSubsidy.message;
                }
            }
            var serializedSubsidy = JsonSerializer.Serialize(subsidy, new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
            await _activityLogService.AddActivityLogAsync(
               _userPrincipal.Id,
               ControllerContext.ActionDescriptor.ControllerName
                   .Replace("Controller", ""),
               ControllerContext.ActionDescriptor.ActionName,
               editModel.Id,
               serializedSubsidy,
               "به روز رسانی یارانه"
           );
            return await base.CustomValidateBeforeUpdateAsync(editModel);
        }
        /// <summary>
        /// در صورتی که برای گروه دسته بندی انتخابی کاربر در گروه یارانه انتخابی کاربر
        /// ، یارانه فعال وجود داشته باشد
        /// </summary>
        /// <param name="categoryGroupId"></param>
        /// <param name="subsidyGroupId"></param>
        /// <returns></returns>
        [NonAction]
        private async Task<(bool isExists, string message)> CheckExistCategoryGroupActiveSubsidyBySubsidyGroupIdAsync(int categoryGroupId, int subsidyGroupId)
        {
            var isExists = await _subsidyService
                .CheckExistCategoryGroupActiveSubsidyAsync
                    (categoryGroupId);
            return isExists ? (true, "کاربر گرامی، برای این گروه دسته بندی قبلا یارانه فعال تعریف شده است.") : (false, "");
        }

    }
}