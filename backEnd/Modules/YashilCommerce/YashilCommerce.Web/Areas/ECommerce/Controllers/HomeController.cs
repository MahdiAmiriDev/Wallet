
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Caching;
using Yashil.Common.Core.Classes;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Core;
using Yashil.Core.CustomViewModels.ECommerce;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using Yashil.Infrastructure.Caching;
using YashilCommerce.Core.Classes;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerce.ViewModels;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using YashilUserManagement.Core.Services;
using Microsoft.Extensions.Logging;
using System;
using YashilDms.Core.Services;
using YashilCommerce.Web.Areas.ECommerce.ViewModels.ResumeModels;
using Microsoft.EntityFrameworkCore;
using YashilCommerce.Core.Enums;

namespace YashilCommerce.Web.Areas.ECommerce.Controllers
{
    public class HomeController : YashilMvcBaseController<object>
    {
        // GET: /<controller>/
        private readonly IAppDocumentService _appDocumentService;
        private readonly ICategoryService _categoryService;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly StoreConfiguration _storeConfiguration;
        private readonly IMapper _mapper;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IAdditionalUserPropService _additionalUserPropService;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IAppConfigService _appConfigService;
        private readonly IHumaxService _humaxService;
        private readonly IProductSalesPlanningService _productSalesPlanningService;
        private readonly ISiteVisitService _siteVisitService;


        public HomeController(ICategoryService categoryService, ApplicationConfiguration applicationConfiguration,
            IStaticCacheManager staticCacheManager,
            IMapper mapper, IYashilLogger<object> logger,
            IPermissionService permissionService,
            StoreConfiguration storeConfiguration,
            IUserPrincipal userPrincipal, IAppConfigService appConfigService,
            IAppDocumentService appDocumentService, IHumaxService humaxService,
            IAdditionalUserPropService additionalUserPropService,
            IProductSalesPlanningService productSalesPlanningService,
            ISiteVisitService siteVisitService) : base(applicationConfiguration, mapper, logger, permissionService)

        {
            _categoryService = categoryService;
            _applicationConfiguration = applicationConfiguration;
            _mapper = mapper;
            _storeConfiguration = storeConfiguration;
            _userPrincipal = userPrincipal;
            _appConfigService = appConfigService;
            _appDocumentService = appDocumentService;
            _humaxService = humaxService;
            _additionalUserPropService = additionalUserPropService;
            _productSalesPlanningService = productSalesPlanningService;
            _siteVisitService = siteVisitService;
            _staticCacheManager = staticCacheManager;
        }

        /// <summary>
        /// صفحه اصلی سایت
        /// </summary>
        /// <param name="id">دسته بندی</param>
        /// <param name="categoryGroupId">گروه دسته بندی</param>
        /// <returns></returns>
        [Route("main/{id?}")]
        [HttpGet]
        public async Task<IActionResult> Main(int? id, int? categoryGroupId = null)
        {
            return await GetContent(id, categoryGroupId);
        }

        private async Task<ViewResult> GetContent(int? categoryId = null, int? categoryGroupId = null)
        {
            var userAdditionalUserProp = await _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id);
            var cacheKey =
                _staticCacheManager.
                    PrepareKeyForDefaultCache(YashilModelCacheDefaults<Category>
                        .FirstLevelCategoriesSimpleViewModelKey,
                        categoryId,
                        categoryGroupId,
                        userAdditionalUserProp.CustomerClubId);

            cacheKey.CacheTime = (int)CacheTime.TwentyFourHour;

            var indexPageViewModel = new IndexPageViewModel
            {
                FirstLevelCategories = await _staticCacheManager.GetAsync(cacheKey, () =>
                {

                    var levelOneUsedCategories =
                        _categoryService.
                            GetRootUsedCategories(
                                categoryId,
                                null,
                                null,
                                null,
                                ApplicationConfiguration.ApplicationId,
                                true,
                                userAdditionalUserProp.CustomerClubId,
                                categoryGroupId);

                    return _mapper.Map<List<CategorySimpleViewModel>>(levelOneUsedCategories);
                }),
                ProductFilter = new ProductFilter()
                {
                    CategoryId = categoryId,
                    CategoryGroupId = categoryGroupId
                }
            };

            return View("Index", indexPageViewModel);
        }




        [Route("/")]
        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> Home()
        {
            await _siteVisitService.AddSiteVisitAsync(HttpContext?.Request?.Path, HttpContext?.Request?.Headers?.UserAgent);

            var homeViewModel = new HomeViewModel
            {
                EmploymentCategoryGroupId = await _appConfigService.GetValueByApplicationIdAsync(
                ApplicationConfigKey.YashilCommerceCategoryGroupEmploymentCategoryGroupId, _applicationConfiguration.ApplicationId),
                IsHekmatCurrentUserCustomerClub = await _additionalUserPropService.CheckIsHekmatCurrentUserCustomerClub(),
                ShowJobOfferListButton = await _appConfigService.GetBooleanValueAsync
                (ApplicationConfigKey.YashilCommerceJobJourneyActiveateShowJobOfferListButton, _applicationConfiguration.ApplicationId)
            };

            return View("Home", homeViewModel);
        }

        /// <summary>
        /// قسمت منوی سایت
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Menu()
        {
            var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(YashilModelCacheDefaults<Category>.AllUsedCategoriesModelKey);
            cacheKey.CacheTime = 240;
            var cachedMenuViewModel = await _staticCacheManager.GetAsync(cacheKey, async () =>
            {
                var applicationCategories = await _categoryService
                    .GetUsedCategoriesAsync(
                        null, null, null, null,
                        null, null, _applicationConfiguration.ApplicationId, false);
                var applicationCategoryList = Mapper.Map<List<CategorySimpleViewModel>>(applicationCategories);
                var menuViewModel = new MenuViewModel { CategoryViewModels = applicationCategoryList, AllowFilterByCity = _storeConfiguration.AllowFilterByCity };
                var rootCategory = applicationCategories.FirstOrDefault(x => !x.ParentId.HasValue);
                if (rootCategory != null)
                {
                    menuViewModel.RootCategoryId = rootCategory.Id;
                    menuViewModel.FirstLevelCategories = applicationCategoryList.Where(x => x.ParentId == rootCategory.Id).ToList();
                }
                return menuViewModel;
            });
            return View(cachedMenuViewModel);
        }
        /// <summary>
        /// صفحه ثبت و ویرایش رزومه
        /// </summary>
        /// <returns></returns>
        [Route("resume")]
        [HttpGet]
        public async Task<ActionResult> Resume()
        {
            if (_userPrincipal.Id <= 0)
            {
                return RedirectToAction("Login", "User");
            }
            var additionalUserProp = await
                _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id);
            var hekmatCustomerClubId =
                await _appConfigService.GetIntValueAsync(ApplicationConfigKey
                    .YashilCommerceCustomerClubsHekmatCustomerClubId);
            // اگر کاربر لاگین کرده در باشگاه طرح باقرالعلوم نباشد نمی تواند رزومه ثبت کند و به صفحه خطا می رود
            if (additionalUserProp.CustomerClubId != hekmatCustomerClubId)
            {
                return RedirectToAction("Index", "Error");
            }
            return View("Resume");
        }

        /// <summary>
        /// صفحه ثبت و ویرایش رزومه
        /// </summary>
        /// <returns></returns>
        [Route("resumefillhelp")]
        [AcceptVerbs("GET", "POST")]
        public async Task<ActionResult> ResumeFillHelp()
        {
            if (_userPrincipal.Id <= 0)
            {
                return Redirect("/User/Login?returnUrl=/resumefillhelp");
            }
            var additionalUserProp = await
                _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id);
            var hekmatCustomerClubId =
                await _appConfigService.GetIntValueAsync(ApplicationConfigKey
                    .YashilCommerceCustomerClubsHekmatCustomerClubId);
            // اگر کاربر لاگین کرده در باشگاه طرح باقرالعلوم نباشد نمی تواند رزومه ثبت کند و به صفحه خطا می رود
            if (additionalUserProp.CustomerClubId != hekmatCustomerClubId)
            {
                return RedirectToAction("Index", "Error");
            }

            var validateUserResumeAndUserAge = await _additionalUserPropService.ValidateUserResumeAndUserAgeAsync(_userPrincipal.Id);

            return View("ResumeFillHelp", new ResumeFillHelpViewModel()
            {
                HasUserAlreadyFilledOutResume = validateUserResumeAndUserAge.isSuccess,
                Message = validateUserResumeAndUserAge.message
            });
        }


        /// <summary>
        /// صفحه شروع آزمون استعدادیابی
        /// </summary>
        /// <returns></returns>
        [Route("TalentExam")]
        [AcceptVerbs("GET", "POST")]
        public async Task<ActionResult> TalentExam()
        {
            if (_userPrincipal.Id <= default(int))
                return Redirect("/User/Login?returnUrl=/TalentExam");

            var hasUserPurchasedEmploymentExam = await _productSalesPlanningService.CheckUserPurchasedEmploymentTestCourse();

            if (hasUserPurchasedEmploymentExam)
            {
                var isSuccess =
                    await _additionalUserPropService.UpdateHasUserPurchasedEmploymentExamAsync(_userPrincipal.Id);

                if (isSuccess)
                {
                    // کاربر جاری آزمون استعداد سنجی را خریده است
                    var userPurchasedEmploymentExam = await CheckHasUserPurchasedEmploymentExamAsync();
                    if (userPurchasedEmploymentExam)
                    {
                        return View(new TalentExamViewModel
                        {
                            HasUserPurchasedExam = true,
                            HasUserTakenExamBefore = false,
                        });
                    }
                }
            }

            var employmentTestProductSalesPlanningId = await _appConfigService.GetValueByApplicationIdAsync(ApplicationConfigKey
                .YashilCommerceEmploymentTestProductSalesPlanningId, _applicationConfiguration.ApplicationId);

            return View(new TalentExamViewModel
            {
                HasUserPurchasedExam = false,
                HasUserTakenExamBefore = false,
                Url = $"/Product?pSpId={employmentTestProductSalesPlanningId}"
            });


        }

        /// <summary>
        /// دریافت لینک ورود به آزمون و بروزرسانی مقدار
        /// ParticipatedInHumaxTest
        /// برای
        /// AdditionalUserProp
        /// </summary>
        /// <returns>Task<IActionResult></returns>
        [HttpGet]
        public async Task<IActionResult> ParticipateInExam()
        {
            if (_userPrincipal.Id <= default(int))
                return RedirectToAction("Login", "User");
            try
            {
                var userPurchasedEmploymentExam = await CheckHasUserPurchasedEmploymentExamAsync();
                if (!userPurchasedEmploymentExam)
                {
                    return View("TalentExam", new TalentExamViewModel
                    {
                        Message = "کاربر گرامی به دلیل عدم خرید دوره آزمون استعداد سنجی، قادر به شرکت در آزمون نمی باشید .",
                        HasUserTakenExamBefore = false,
                        HasUserPurchasedExam = false,
                    });
                }


                var registerInHumaxServiceResult = await _humaxService.DoRegisterAndGetRedirectUrlToHumaxServiceForCurrentUserAsync();


                if (registerInHumaxServiceResult == null || !registerInHumaxServiceResult.Success)
                    return GetHumaxRegisterFailMessage(registerInHumaxServiceResult?.Message);


                return Redirect(registerInHumaxServiceResult.Url);
            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error,
                    new EventId((int)ECommerceEvents.HumaxServiceRegisterError,
                        ECommerceEvents.HumaxServiceRegisterError.ToString()), nameof(ResumeAttachment),
                    "error in upload resume attachment file", e);
                return GetHumaxRegisterFailMessage(null);
            }

        }

        [NonAction]
        private IActionResult GetHumaxRegisterFailMessage(string message)
        {
            return View("TalentExam", new TalentExamViewModel()
            {
                Message = message ?? "بروز خطا به هنگام انجام عملیات لطفا بعد از چند دقیقه مجددا تلاش کنید.",
                HasUserTakenExamBefore = false,
                HasUserPurchasedExam = true,
            });
        }
        /// <summary>
        /// بررسی می کند که کاربر جاری سیستم آزمون استعداد سنجی را خریده است یا خیر
        /// </summary>
        /// <returns></returns>
        [NonAction]
        private async Task<bool> CheckHasUserPurchasedEmploymentExamAsync()
        {
            var additionalUserProp = await _additionalUserPropService.GetUserAdditionalUserPropAsync(_userPrincipal.Id);

            if (additionalUserProp.HasUserPurchasedEmploymentExam.HasValue &&
                additionalUserProp.HasUserPurchasedEmploymentExam.Value)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// این متد عملیات مربوط به آپلود کردن فایل برای رزومه کاربر را انجام می دهد
        /// </summary>
        /// <param name="resumeAttachment">مدل فایل دریافتی از کاربر</param>
        /// <returns></returns>
        [Route("home/UploadAttachments")]
        [AcceptVerbs("POST")]
        public async Task<List<ResultStatus<int>>> UploadAttachments([FromForm] ResumeAttachment resumeAttachment)
        {
            var uploadAttachmentsResult = new List<ResultStatus<int>>();

            var appDocumentCount = await _appDocumentService.GetAppDocumentCountAsync(nameof(Yashil.Core.Entities.User), _userPrincipal.Id, resumeAttachment.DocTypeId);

            if ((appDocumentCount + resumeAttachment.Attachments.Count()) > 15)
            {
                uploadAttachmentsResult.Add(new ResultStatus<int>()
                {
                    IsSuccess = false,
                    Message = "کاربر گرامی ، تعداد فایل های آپلود شده شما نمی تواند بیشتر از 15 فایل باشد "
                });

                return uploadAttachmentsResult;
            }


            if (resumeAttachment.Attachments != null && resumeAttachment.Attachments.Count > 0)
            {
                try
                {
                    var saveDocumentsResult = new List<(bool isSuccess, string fileName, int? id, string message)>();



                    foreach (var attachment in resumeAttachment.Attachments)
                    {
                        var saveDocument = await _appDocumentService.SaveDocumentAsync(null, null,
                             null,
                             null,
                             null,
                             (int)AppEntityEnum.User,
                             resumeAttachment.DocTypeId,
                             null, _userPrincipal.Id, attachment, resumeAttachment.Title);

                        saveDocumentsResult.Add((saveDocument.isSuccess, attachment.FileName, (saveDocument.appDocument != null) ? saveDocument.appDocument.Id : null, saveDocument.message));
                    }

                    foreach (var saveDocumentReslut in saveDocumentsResult)
                    {
                        if (!saveDocumentReslut.isSuccess)
                        {
                            uploadAttachmentsResult.Add(new ResultStatus<int>
                            {
                                IsSuccess = false,
                                Message = saveDocumentReslut.message
                            });
                        }
                        else
                        {
                            uploadAttachmentsResult.Add(new ResultStatus<int>
                            {
                                EntityId = saveDocumentReslut.id.Value,
                                IsSuccess = true
                            });
                        }

                    }

                    return uploadAttachmentsResult;
                }
                catch (Exception e)
                {
                    _logger.InsertLog(LogLevel.Error,
                        new EventId((int)Events.Update,
                            Events.UpdateException.ToString()), nameof(ResumeAttachment),
                        "error in upload resume attachment file", e);
                    uploadAttachmentsResult.Add(new ResultStatus<int>()
                    {
                        IsSuccess = false,
                        Message = "خطا در ذخیره فایل"
                    });
                    return uploadAttachmentsResult;
                }
            }

            uploadAttachmentsResult.Add(new ResultStatus<int>()
            {
                IsSuccess = false,
                Message = "خطا در ذخیره فایل"
            });
            return uploadAttachmentsResult;
        }

        /// <summary>
        /// این متد جهت دریافت لیست مدارک کاربر کاربرد بر اساس شناسه نوع سند کاربرد دارد
        /// </summary>
        /// <param name="docTypeId">شناسه نوع سند</param>
        /// <returns>List<UserCertificateSimpleViewModel></returns>
        [Route("home/GetUserCertificates")]
        [HttpGet]
        public async Task<List<UserCertificateSimpleViewModel>> GetUserCertificates(int? docTypeId)
        {
            if (_userPrincipal.Id <= 0 || !docTypeId.HasValue || docTypeId.Value == 0)
            {
                return null;
            }

            var userCertificates = await _appDocumentService
                .GetObjectDocuments(nameof(Yashil.Core.Entities.User), _userPrincipal.Id, (int)DocumentCategoryEnum.ResumeCertificate, docTypeId.Value)
                .Select(x => new UserCertificateSimpleViewModel
                {
                    Id = x.Id,
                    Title = x.Title
                }).ToListAsync();

            return userCertificates;
        }
        /// <summary>
        /// این متد جهت دریافت فایل مربوط به مدرک روزمه کاربر کاربرد دارد
        /// </summary>
        /// <param name="certificateId">شناسه مدرک</param>
        /// <param name="docTypeId">شناسه نوع سند</param>
        /// <returns>File</returns>
        [Route("home/GetCertificateFile")]
        [HttpGet]
        public IActionResult GetCertificateFile(int? certificateId, int? docTypeId)
        {
            if (!certificateId.HasValue || certificateId.Value == 0 || !docTypeId.HasValue || docTypeId.Value == 0)
                NotFound();

            var appDoc = _appDocumentService.GetFile(certificateId.Value, _userPrincipal.Id, docTypeId.Value);

            if (appDoc?.DocumentFile != null)
            {
                return File(appDoc.DocumentFile, appDoc.ContentType);
            }

            return NotFound();
        }

        /// <summary>
        /// این اکشن متد جهت مشاهده روزمه کاربر کاربرد دارد
        /// </summary>
        /// <returns>ShowResume.cshtml</returns>
        [Route("home/ShowResume")]
        [HttpGet]
        public IActionResult ShowResume()
        {
            if (_userPrincipal.Id <= 0)
                return RedirectToAction("Login", "User");

            return View();
        }

        /// <summary>
        ///   این متد برای تعیین سطح زبان های خارجه کاربرد دارد
        /// </summary>
        /// <returns></returns>
        [Route("PlacementForeignLanguage")]
        [HttpGet]
        public IActionResult PlacementForeignLanguage()
        {
            return View("PlacementForeignLanguage");
        }

        /// <summary>
        ///   این متد برای صفحه طرح اشتغال کاربرد دارد
        /// </summary>
        /// <returns></returns>
        [Route("Employment")]
        [HttpGet]
        public async Task<IActionResult> Employment()
        {
            var employmentViewModel = new EmploymentViewModel
            {
                EmploymentCategoryGroupId = await _appConfigService.GetValueByApplicationIdAsync(
                    ApplicationConfigKey.YashilCommerceCategoryGroupEmploymentCategoryGroupId,
                    _applicationConfiguration.ApplicationId)
            };
            return View("Employment", employmentViewModel);

        }


        /// <summary>
        /// فرم مربوط به لیست نمایندگی های انستیتو
        /// </summary>
        /// <returns></returns>
        [Route("ListofInstituteRepresentatives")]
        [HttpGet]
        public IActionResult ListofInstituteRepresentatives()
        {
            return View();
        }

       

    }
}
