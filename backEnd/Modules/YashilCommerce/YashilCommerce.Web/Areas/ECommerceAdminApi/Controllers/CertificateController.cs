using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Core.Entities;
using YashilCommerce.Core.Services;
using YashilCommerce.Web.Areas.ECommerceAdminApi.ViewModels;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Microsoft.AspNetCore.Authorization;
using DNTCaptcha.Core;
using Yashil.Common.SharedKernel.Helpers;
using DevExtreme.AspNet.Data.ResponseModel;
using Yashil.Common.Web.Infrastructure.Classes;
using DevExtreme.AspNet.Data;
using AutoMapper.QueryableExtensions;
using Yashil.Common.Web.Infrastructure.Filters;
using Yashil.Infrastructure.Extensions;
using Language = DNTCaptcha.Core.Language;
using System.Collections.Generic;
using System.Linq;

namespace YashilCommerce.Web.Areas.ECommerceAdminApi.Controllers
{
    public class CertificateController : BaseController<Certificate, int, CertificateListViewModel, CertificateEditModel, CertificateSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly ICertificateService _certificateService;
        private readonly IDNTCaptchaValidatorService _validatorService;
        public CertificateController(ICertificateService certificateService,
        IMapper mapper,
        IYashilLogger<Certificate> logger, IDNTCaptchaValidatorService validatorService,
        IPermissionService permissionService) : base(certificateService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _certificateService = certificateService;
            _validatorService = validatorService;
        }

        /// <summary>
        /// اطلاعات گواهینامه را بر اساس شماره گواهینامه دریافت می کند
        /// </summary>
        /// <param name="certificateSearchParam">اطلاعات ارسالی برای جستجوی گواهینامه</param>
        /// <returns></returns>
        [HttpPost("SearchByCertificateNumberAsync")]
        [AllowAnonymous]
        public async Task<CertificateCustomViewModel> SearchByCertificateNumberAsync([FromForm] CertificateSearchParam certificateSearchParam)
        {
            if (string.IsNullOrEmpty(certificateSearchParam.CertificateNumber))
            {
                return new CertificateCustomViewModel()
                {
                    IsSuccess = false,
                    Message = "کاربر گرامی، لطفا شماره گواهینامه را وارد نمایید."
                };
            }
            
            var certificate = await _certificateService.GetByNumberAsync(certificateSearchParam.CertificateNumber);
            if (certificate == null)
            {
                return new CertificateCustomViewModel()
                {
                    IsSuccess = false,
                    Message = "اطلاعاتی با این مشخصات یافت نشد"
                };
            }

            certificate.Name = DecryptData(certificate.Name);
            certificate.LastName = DecryptData(certificate.LastName);
            certificate.FatherName = DecryptData(certificate.FatherName);
            certificate.LatinName = DecryptData(certificate.LatinName);
            certificate.LatinLastName = DecryptData(certificate.LatinLastName);
            certificate.NationalCode = DecryptData(certificate.NationalCode);
            certificate.MobileNumber = DecryptData(certificate.MobileNumber);
            certificate.Tell = DecryptData(certificate.Tell);
            certificate.IdentityNumber = DecryptData(certificate.IdentityNumber);

            var certificateStatisticListViewModel = _mapper.Map<Certificate, CertificateStatisticViewModel>(certificate);
            List<CertificateStatisticViewModel> certificatesStatistic = new() { certificateStatisticListViewModel };

            return new CertificateCustomViewModel()
            {
                CertificatesStatistic = certificatesStatistic,
                IsSuccess = true,
                Message = "بررسی با موفقیت انجام شد"
            };
        }

        [HttpPost("GetCertificateListByNationalCode")]
        [AllowAnonymous]
        public CertificateCustomViewModel GetCertificateListByNationalCode([FromForm] CertificateSearchParam certificateSearchParam)
        {
            if (string.IsNullOrEmpty(certificateSearchParam.NationalCode))
            {
                return new CertificateCustomViewModel
                {
                    IsSuccess = false,
                    Message = "کاربر گرامی، لطفا شماره کد ملی را وارد نمایید."
                };
            }

            if (!certificateSearchParam.NationalCode.IsValidNationalCode())
            {
                return new CertificateCustomViewModel
                {
                    IsSuccess = false,
                    Message = "کاربر گرامی، کد ملی وارد شده معتبر نمی باشد."
                };
            }

            if (!_validatorService.HasRequestValidCaptchaEntry(Language.Persian, DisplayMode.NumberToWord))
            {
                return new CertificateCustomViewModel
                {
                    IsSuccess = false,
                    Message = "عدد وارد شده با متن همخوانی ندارد"
                };
            }
            var certificatesStatistic =
                _mapper.Map<List<CertificateStatisticViewModel>>(_certificateService.GetByCustomFilter(EncryptData(certificateSearchParam.NationalCode)));
            if (!certificatesStatistic.Any())
            {
                return new CertificateCustomViewModel
                {
                    IsSuccess = false,
                    Message = "کاربر گرامی، برای کد ملی وارد شده گواهینامه ای یافت نشد."
                };
            }
            return new CertificateCustomViewModel
            {
                CertificatesStatistic = certificatesStatistic,
                IsSuccess = true
                 
            };

        }
        /// <summary>
        /// اعتبار سنجی کد ملی و بررسی مقادیر الزامی وارد شده قبل از درج در دیتابیس
        /// </summary>
        /// <param name="editModel"></param>
        /// <returns></returns>
        protected override Task<string> CustomValidateBeforeInsertAsync(CertificateEditModel editModel)
        {
            (bool result, Task<string> value) = Validate(editModel);
            if (!result)
            {
                return value;
            }

            return base.CustomValidateBeforeInsertAsync(editModel);
        }
        /// <summary>
        /// برای رمز کردن اطلاعات مهم این متد را بازنویسی می کنیم
        /// </summary>
        /// <param name="editModel"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected override Task CustomMapBeforeInsertAsync(CertificateEditModel editModel, Certificate entity)
        {
            entity.Name = EncryptData(editModel.Name);
            entity.LastName = EncryptData(editModel.LastName);
            entity.FatherName = EncryptData(editModel.FatherName);
            entity.LatinName = EncryptData(editModel.LatinName);
            entity.LatinLastName = EncryptData(editModel.LatinLastName);
            entity.NationalCode = EncryptData(editModel.NationalCode);
            entity.MobileNumber = EncryptData(editModel.MobileNumber);
            entity.Tell = EncryptData(editModel.Tell);
            entity.IdentityNumber = EncryptData(editModel.IdentityNumber);

            return base.CustomMapBeforeInsertAsync(editModel, entity);
        }

        /// <summary>
        /// اعتبار سنجی کد ملی و بررسی مقادیر الزامی وارد شده قبل از بروزرسانی در دیتابیس
        /// </summary>
        /// <param name="editModel"></param>
        /// <returns></returns>
        protected override Task<string> CustomValidateBeforeUpdateAsync(CertificateEditModel editModel)
        {
            (bool result, Task<string> value) = Validate(editModel);
            if (!result)
            {
                return value;
            }

            return base.CustomValidateBeforeInsertAsync(editModel);
        }
        /// <summary>
        /// برای رمز کردن اطلاعات مهم این متد را بازنویسی می کنیم
        /// </summary>
        /// <param name="editModel"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected override Task CustomMapBeforeUpdateAsync(CertificateEditModel editModel, Certificate entity)
        {

            entity.Name = EncryptData(editModel.Name);
            entity.LastName = EncryptData(editModel.LastName);
            entity.FatherName = EncryptData(editModel.FatherName);
            entity.LatinName = EncryptData(editModel.LatinName);
            entity.LatinLastName = EncryptData(editModel.LatinLastName);
            entity.NationalCode = EncryptData(editModel.NationalCode);
            entity.MobileNumber = EncryptData(editModel.MobileNumber);
            entity.Tell = EncryptData(editModel.Tell);
            entity.IdentityNumber = EncryptData(editModel.IdentityNumber);

            return base.CustomMapBeforeUpdateAsync(editModel, entity);
        }
        /// <summary>
        /// اعتبارسنجی
        /// </summary>
        /// <param name="editModel"></param>
        /// <returns></returns>
        private static (bool result, Task<string> value) Validate(CertificateEditModel editModel)
        {
            if (string.IsNullOrEmpty(editModel.Name))
            {
                return (result: false, value: Task.FromResult("کاربر گرامی، نام الزامی می باشد."));
            }
            if (string.IsNullOrEmpty(editModel.LastName))
            {
                return (result: false, value: Task.FromResult("کاربر گرامی، نام خانوادگی الزامی می باشد."));
            }
            if (string.IsNullOrEmpty(editModel.LastName))
            {
                return (result: false, value: Task.FromResult("کاربر گرامی، نام خانوادگی الزامی می باشد."));
            }
            if (string.IsNullOrEmpty(editModel.NationalCode))
            {
                return (result: false, value: Task.FromResult("کاربر گرامی، کد ملی الزامی می باشد."));
            }
            if (string.IsNullOrEmpty(editModel.CourseTitle))
            {
                return (result: false, value: Task.FromResult("کاربر گرامی، نام دوره الزامی می باشد."));
            }
            if (string.IsNullOrEmpty(editModel.CourseType))
            {
                return (result: false, value: Task.FromResult("کاربر گرامی، نوع دوره الزامی می باشد."));
            }
            if (string.IsNullOrEmpty(editModel.Score))
            {
                return (result: false, value: Task.FromResult("کاربر گرامی، نمره الزامی می باشد."));
            }
            if (!editModel.NationalCode.IsValidNationalCode())
            {
                return (result: false, value: Task.FromResult("کاربر گرامی، کد ملی وارد شده معتبر نمی باشد."));
            }

            return (result: true, value: null);
        }
        /// <summary>
        /// برای خارج کردن از رمز دیتا های رمز شده
        /// وقتی کاربر می خواهد رکوردی را ویرایش کند 
        /// این متد را بازنویسی می کنیم
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected override async Task<CertificateEditModel> GetEntityAndMapToEditModelForEdit(int id)
        {
            var certificate = await base.GetEntityAndMapToEditModelForEdit(id);
            certificate.Name = DecryptData(certificate.Name);
            certificate.LastName = DecryptData(certificate.LastName);
            certificate.FatherName = DecryptData(certificate.FatherName);
            certificate.LatinName = DecryptData(certificate.LatinName);
            certificate.LatinLastName = DecryptData(certificate.LatinLastName);
            certificate.NationalCode = DecryptData(certificate.NationalCode);
            certificate.MobileNumber = DecryptData(certificate.MobileNumber);
            certificate.Tell = DecryptData(certificate.Tell);
            certificate.IdentityNumber = DecryptData(certificate.IdentityNumber);
            return certificate;
        }

        [HttpGet("GetByCustomFilterForList")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, string nationalCode)
        {
            if (!string.IsNullOrEmpty(nationalCode))
            {
                nationalCode = EncryptData(nationalCode);
            }
            var certificate = _certificateService.GetByCustomFilter(nationalCode);
            return await DataSourceLoader.LoadAsync(
                certificate.ProjectTo<CertificateListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        /// <summary>
        /// رمز کردن دیتا
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [NonAction]
        private string EncryptData(string value)
        {
            return !string.IsNullOrWhiteSpace(value) ?
                 CryptographyHelper.AesEncrypt(value) : null;
        }
        /// <summary>
        /// رمز کردن دیتا
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [NonAction]
        private string DecryptData(string value)
        {
            return !string.IsNullOrWhiteSpace(value) ?
                 CryptographyHelper.AesDecrypt(value) : null;
        }
    }
}