using System;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Yashil.Common.Core.Classes;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.Attributes;
using Yashil.Core;
using Yashil.Core.Entities;
using Yashil.Core.Enums;
using YashilDms.Core.Services;

namespace YashilCommerce.Web.Areas.ECommerce.Controllers
{
    [YashilAppActions("ChangeProductSalesPlanningPublishState")]
    public class ProductSalesPlanningController : YashilMvcBaseController<ProductSalesPlanning>
    {
        private readonly IAppDocumentService _appDocumentService;

        public ProductSalesPlanningController(ApplicationConfiguration applicationConfiguration,
            IMapper mapper, IYashilLogger<ProductSalesPlanning> logger,
            IPermissionService permissionService, IAppDocumentService appDocumentService) : base(applicationConfiguration, mapper, logger, permissionService)
        {
            _appDocumentService = appDocumentService;
        }

        /// <summary>
        /// متد دانلود اتچمنت برنامه ریزی دوره
        /// این متد در صفحه جزئئیات دوره پنل عمومی مورد استفاده قرار میگیرد
        /// </summary>
        /// <param name="productSalesPlanningId"></param>
        /// <param name="appDocumentId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult GetProductSalesPlanningAttachment(int? productSalesPlanningId, int? appDocumentId)
        {
            if (productSalesPlanningId == null || productSalesPlanningId == 0 || appDocumentId == null || appDocumentId == 0)
            {
                throw new Exception("parameter is not set correctly");
            }

            var appDoc = _appDocumentService.GetFile(appDocumentId.Value, productSalesPlanningId.Value, (int)DocumentTypeEnum.ProductSalesPlanningPdfAttachments);
            if (appDoc?.DocumentFile != null)
            {
                return File(appDoc.DocumentFile, appDoc.ContentType, appDoc.OrginalName);

            }
            return NotFound("کاربر گرامی فایل مورد نظر یافت نشد");
        }



        /// <summary>
        /// متد دریافت تگ های مرتبط به محصولات
        /// این متد در صفحه جزئئیات محصول  استفاده می شود
        /// </summary>
        /// <param name="productSalesPlanningId"></param>
        /// <returns>ProductTag</returns>
        [AllowAnonymous]
        public IActionResult GetProductTag(int? productSalesPlanningId)
        {
            return ViewComponent("ProductTag", new { productSalesPlanningId });
        }

    }
}
