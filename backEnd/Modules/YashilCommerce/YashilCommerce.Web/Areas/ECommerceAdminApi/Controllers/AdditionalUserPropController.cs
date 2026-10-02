using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Yashil.Common.Core.Classes;
using Yashil.Common.SharedKernel.Helpers;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Common.Web.Infrastructure.Filters;
using Yashil.Core.CustomViewModels;
using Yashil.Core.Entities;
using Yashil.Infrastructure.Extensions;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public class AdditionalUserPropController : BaseController<AdditionalUserProp, int, AdditionalUserPropListViewModel,
        AdditionalUserPropEditModel, AdditionalUserPropSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IAdditionalUserPropService _additionalUserPropService;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IYashilLogger<AdditionalUserProp> _yashilLogger;

        public AdditionalUserPropController(IAdditionalUserPropService additionalUserPropService, IMapper mapper, IUserPrincipal userPrincipal, IYashilLogger<AdditionalUserProp> logger, IPermissionService permissionService)
            : base(additionalUserPropService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _userPrincipal = userPrincipal;
            _additionalUserPropService = additionalUserPropService;
            _yashilLogger = logger;
        }

        [HttpGet("GetCurrentUserAdditionalProp")]
        public Task<CurrentUserAdditionalPropViewModel> GetCurrentUserAdditionalProp()
        {
            var additionalUserProp = _additionalUserPropService.FindByUserId(_userPrincipal.Id);
            //return _mapper.Map<AdditionalUserProp, CurrentUserAdditionalPropViewModel>(additionalUserProp);
            return additionalUserProp.ProjectTo<CurrentUserAdditionalPropViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }
        /// <summary>
        /// دریافت اطلاعات کاربر لاگین شده بر اساس اپیلیکیشن 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCurrentUserAdditionalPropInfo")]
        public Task<AdditionalUserPropInfoViewModel> GetCurrentAdditionalPropInfo()
        {
            var additionalUserProp = _additionalUserPropService.GetByCustomFilter(_userPrincipal.Id, _userPrincipal.ApplicationId);
            return additionalUserProp.ProjectTo<AdditionalUserPropInfoViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// درج رزومه برای کاربر
        /// رزومه به صورت
        /// json ذخیره می شود
        /// </summary>
        /// <param name="resumeEditModel"></param>
        /// <returns></returns>
        [HttpPost("AddUserResume")]
        [YashilAuthorization]
        public async Task<IActionResult> AddUserResumeAsync(ResumeEditModel resumeEditModel)
        {
            try
            {
                List<string> messages = new();

                if (string.IsNullOrEmpty(resumeEditModel.UserResume))
                {
                    messages.Add("بروز خطا در عملیات ذخیره سازی");

                    return Accepted(new ResumeResponseModel(false, messages));
                }

                resumeEditModel.Id = _userPrincipal.Id;
                var additionalUserProp = await _additionalUserPropService.FindByUserId(_userPrincipal.Id).FirstOrDefaultAsync();
                if (additionalUserProp == null)
                {
                    messages.Add("کاربری با این شناسه وجود ندارد");
                    return Accepted(new ResumeResponseModel(false, messages));
                }

                ModelState.Clear();

                var resume = _additionalUserPropService.DeserializeResumeStringToResumeModel(resumeEditModel.UserResume);

                if (resume == null)
                {
                    messages.Add("کاربر گرامی، به هنگام ذخیره سازی رزومه شما خطای رخ داد لطفا با مدیر سیستم تماس بگیرید");
                    return Accepted(new ResumeResponseModel(false, messages));
                }

                if (!TryValidateModel(resume))
                {
                    foreach (var item in ModelState)
                    {
                        if (item.Value.ValidationState == ModelValidationState.Invalid)
                        {
                            messages.Add(string.Join(",", item.Value.Errors.Select(error => error.ErrorMessage)));
                        }
                    }

                    return Accepted(new ResumeResponseModel()
                    {
                        EntityId = resumeEditModel.Id,
                        IsSuccess = false,
                        Message = messages
                    });

                }


                var stringResume = _additionalUserPropService.SerializeResumeToJson(resume);



                if (string.IsNullOrEmpty(stringResume))
                {
                    messages.Add("کاربر گرامی، به هنگام ذخیره سازی رزومه شما خطای رخ داد لطفا با مدیر سیستم تماس بگیرید");
                    return Accepted(new ResumeResponseModel(false, messages));
                }

                additionalUserProp.Resume = CryptographyHelper.AesEncrypt(stringResume);

                await _additionalUserPropService.UpdateAsync(additionalUserProp, additionalUserProp.Id, true);

                if (_additionalUserPropService.HasUserFillOutResumeEnough(resume))
                {
                    await _additionalUserPropService.UpdateIsResumeFillOutEnoughAsync(_userPrincipal.Id);
                }

                messages.Add("اطلاعات با موفقیت ثبت شد");

                return Accepted(new ResumeResponseModel()
                {
                    EntityId = resumeEditModel.Id,
                    IsSuccess = true,
                    Message = messages
                });
            }
            catch (Exception e)
            {
                _yashilLogger.InsertLog(LogLevel.Error, (int)Events.UpdateException, nameof(AddUserResumeAsync),
                    "بروز خطا به هنگام ذخیره سازی رزومه کاربر", e);

                return Accepted(new ResumeResponseModel()
                {
                    EntityId = resumeEditModel.Id,
                    IsSuccess = false,
                    Message = new List<string>() { "بروز خطا به هنگام ذخیره سازی رزومه کاربر" }
                });
            }
        }
        /// <summary>
        /// دریافت رزومه کاربر
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUserResume")]
        [YashilAuthorization]
        public async Task<ResumeEditModel> GetUserResumeAsync()
        {
            var userResume = await _additionalUserPropService.FindByUserId(_userPrincipal.Id)
                .Select(x => x.Resume)
                .FirstOrDefaultAsync();

            try
            {
                return new ResumeEditModel
                {
                    Id = _userPrincipal.Id,
                    UserResume = string.IsNullOrEmpty(userResume) ? "" : CryptographyHelper.AesDecrypt(userResume)
                };

            }
            catch (Exception e)
            {
                _yashilLogger.InsertLog(LogLevel.Error, (int)Events.GetEntityException, nameof(GetUserResumeAsync),
                    "بروز خطا به هنگام دریافت رزومه کاربر", e);

                return new ResumeEditModel
                {
                    Id = _userPrincipal.Id,
                    UserResume = ""
                };
            }

        }

        /// <summary>
        /// این متد برای دریافت درصد میزان تکمیل بودن
        /// رزومه کاربر استفاده دارد
        /// در صورتی که کاربر لاگین نباشد 0 برمیگردانیم
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetResumeCompletionPercentage")]
        public async Task<int> GetCurrentUserResumeCompletionPercentageAsync()
        {
            if (_userPrincipal.Id <= 0)
                return 0;

            return await _additionalUserPropService.GetResumeCompletionPercentageByUserIdAsync(_userPrincipal.Id);
        }

        /// <summary>
        /// بررسی می کند که آیا باشگاه مشتریان کاربر جاری حکمتی می باشد یا خیر 
        /// </summary>
        /// <returns></returns>
        [HttpGet("CheckIsCurrentUserHekmatCustomerClub")]
        public async Task<bool> CheckIsCurrentUserHekmatCustomerClubAsync()
        {
            return await _additionalUserPropService.CheckIsHekmatCurrentUserCustomerClub();
        }


        #region AssigneAddtionalUserPropToItemGroup

        /// <summary>
        /// دریافت کاربرانی که گروه بندی نشده اند
        /// </summary>
        /// <param name="loadOptions"></param>
        /// <param name="id">شناسه گروه</param>
        /// <param name="companyId"></param>
        /// <param name="representationId"></param>
        /// <param name="categoryId"></param>
        /// <param name="userGroupId"></param>
        /// <returns></returns>
        [HttpGet("GetAdditionalUserPropsNotAssignedToItemGroupAsync")]
        [YashilAuthorization(ActionName = "GetEntities")]
        public async Task<LoadResult> GetAdditionalUserPropsNotAssignedToItemGroupAsync(
            CustomDataSourceLoadOptions loadOptions,
            int id,
            int? companyId,
            int? representationId,
            int? categoryId,
            int? userGroupId)
        {
            var users = await _additionalUserPropService.GetAdditionalUserPropNotAssignedToGroupAsync(
                id,
                companyId,
                representationId,
                categoryId,
                userGroupId);
            return await DataSourceLoader.LoadAsync(
                users.ProjectTo<AdditionalUserPropCustomListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        [HttpGet("GetAdditionalUserPropsAssignedToItemGroupAsync")]
        [YashilAuthorization(ActionName = "GetEntities")]
        public async Task<LoadResult> GetAdditionalUsersPropAssignedToItemGroupAsync(
            CustomDataSourceLoadOptions loadOptions, int id)
        {
            var users = _additionalUserPropService.GetAdditionalUsersPropAssignedToGroup(id);
            return await DataSourceLoader.LoadAsync(
                users.ProjectTo<AdditionalUserPropCustomListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        /// <summary>
        /// این متد در فرم تخصیص دانشجو به دوره کاربرد دارد
        /// این متد برای تخصیص دانشجو به دوره و خارج کردن  دانشجو از دوره کاربرد دارد و در زمان کلیک بر روی دکمه های فلش
        /// بین باکس های سمت چپ و راست فرم فراخوانی می شود
        /// </summary>
        /// <param name="assignableListEditModel"></param>
        /// <returns></returns>
        [HttpPost("AssignSelectedItemsToItemGroup")]
        [YashilAuthorization(ActionName = "UpdateEntity")]
        public async Task<bool> AssignSelectedItemsToItemGroup(AssignableListEditModel assignableListEditModel)
        {
            return await _additionalUserPropService.AssignSelectedItemsToGroupAsync(
                new List<int>(assignableListEditModel.SelectedItems),
                assignableListEditModel.GroupId,
                assignableListEditModel.Assign);
        }
        #endregion

        /// <summary>
        /// این متد جهت بروزرسانی باشگاه مشتری کاربر به نوع حکمت کاربرد دارد
        /// </summary>
        /// <param name="hekmatCardNumber"></param>
        /// <returns></returns>
        [HttpGet("updateUserCutomerClubToHekmat")]
        public async Task<ResultStatus<int>> UpdateUserCutomerClubToHekmatAsync(string hekmatCardNumber)
        {
            if (string.IsNullOrEmpty(hekmatCardNumber) || !hekmatCardNumber.IsValidValidateHekmatCard())
                return new ResultStatus<int>(false, "کاربر گرامی، شماره کارت وارد شده صحیح نمی باشد.");

            try
            {
                await _additionalUserPropService.UpdateCurrentUserCustomerClubToHekmatAsync(hekmatCardNumber);

            }
            catch (System.Exception e)
            {
                _yashilLogger.InsertLog(Microsoft.Extensions.Logging.LogLevel.Error, (int)Events.InsertException, nameof(AdditionalUserProp),
                    "بروز خطا به هنگام تغییر باشگاه مشتری کاربر به حمت", e);
                return new ResultStatus<int>(false, "بروز خطا به هنگام انجام عملیات");
            }

            return new ResultStatus<int>(true, "کاربر گرامی، عملیات ثبت شماره کارت با موفقیت انجام شد.");
        }
    }
}