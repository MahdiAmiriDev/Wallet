using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Yashil.Common.Core.Classes;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Core;
using Yashil.Core.Enums;
using YashilDms.Core.Services;
using YashilPayment.Core.Classes;
using YashilUserManagement.Core.Services;

namespace YashilPayment.Web.Areas.Payment.Controllers
{
    public class YashilHekmatTerminalController : YashilMvcBaseController<YashilHekmatTerminalController>
    {
        private readonly IAppDocumentService _appDocumentService;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly IAppConfigService _appConfigService;
        public YashilHekmatTerminalController(ApplicationConfiguration applicationConfiguration, IMapper mapper, IYashilLogger<YashilHekmatTerminalController> logger, IPermissionService permissionService, IAppDocumentService appDocumentService, IAppConfigService appConfigService) : base(applicationConfiguration, mapper, logger, permissionService)
        {
            _applicationConfiguration = applicationConfiguration;
            _appDocumentService = appDocumentService;
            _appConfigService = appConfigService;
        }
        /// <summary>
        /// فایل لوگو مربوط به ترمینال بر اساس شناسه ترمینال 
        /// </summary>
        /// <param name="terminalId">شناسه ترمینال</param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<IActionResult> GetYashilHekmatTerminalIcon(int terminalId)
        {
            var appDoc = _appDocumentService.GetFile(terminalId, (int)DocumentTypeEnum.YashilHekmatTerminalIcon);
            if (appDoc?.DocumentFile != null)
            {
                return File(appDoc.DocumentFile, appDoc.ContentType);
            }

            var defaultLogoUrl = await _appConfigService
                .GetStringValueByApplicationIdAsync(
                    ApplicationConfigKey.YashilPaymentYashilHekmatTerminalDefaultLogoName,
                    _applicationConfiguration.ApplicationId);
            return File(defaultLogoUrl, "image/x-png");
        }
    }
}
