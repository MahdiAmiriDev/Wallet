using System.Data;
using Aspose.Cells;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Yashil.Common.Core.Classes;
using Yashil.Common.SharedKernel;
using Yashil.Common.SharedKernel.Helpers;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Common.SharedKernel.Permission;
using Yashil.Common.Web.Infrastructure.BaseClasses;
using Yashil.Common.Web.Infrastructure.Classes;
using Yashil.Common.Web.Infrastructure.Filters;
using Yashil.Core.Entities;
using YashilPayment.Core.Services;
using YashilPayment.Web.Areas.PaymentAdminApi.ViewModels;

namespace YashilPayment.Web.Areas.PaymentAdminApi.Controllers
{
    public class CardInfoController : BaseController<CardInfo, int, CardInfoListViewModel, CardInfoEditModel, CardInfoSimpleViewModel>
    {
        private readonly IMapper _mapper;
        private readonly ICardInfoService _cardInfoService;
        private readonly ICardAmountHistoryService _cardAmountHistoryService;
        private readonly IYashilLogger<CardInfo> _logger;
        private readonly IUserPrincipal _userPrincipal;
        public CardInfoController(ICardInfoService cardInfoService, ICardAmountHistoryService cardAmountHistoryService, IMapper mapper, IYashilLogger<CardInfo> logger, IPermissionService permissionService, IUserPrincipal userPrincipal) : base(cardInfoService, mapper, logger, permissionService)
        {
            _mapper = mapper;
            _cardInfoService = cardInfoService;
            _cardAmountHistoryService = cardAmountHistoryService;
            _logger = logger;
            _userPrincipal = userPrincipal;
        }
        [HttpGet("GetByCustomFilterForList")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomFilterForList(CustomDataSourceLoadOptions loadOptions, int? cardType, int? personId)
        {
            var cardInfos = _cardInfoService.GetByCustomFilter(cardType, personId);
            return await DataSourceLoader.LoadAsync(cardInfos.ProjectTo<CardInfoListViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }
        [HttpGet("GetByCustomForSelect")]
        [YashilAuthorization]
        public async Task<LoadResult> GetByCustomForSelect(CustomDataSourceLoadOptions loadOptions, int? cardType, int? personId)
        {
            var cardInfos = _cardInfoService.GetByCustomFilter(cardType, personId);
            return await DataSourceLoader.LoadAsync(cardInfos.ProjectTo<CardInfoSimpleViewModel>(_mapper.ConfigurationProvider), loadOptions);
        }

        //بعد از افزودن کارت جدید، سابقه شارژ بن ریالی یا اعتباری را در صورت وجود ذخیره می نماید
        protected override async Task<(bool isSuccess, string message)> AfterInsertAsync(CardInfoEditModel editModel, CardInfo entity)
        {
            //سابقه شارژ بن ریالی ذخیره می شود
            if (entity.CashAmount != 0)
            {
                CardAmountHistory cardAmountHistory = CreateCardAmountHistory(null, entity, false);
                cardAmountHistory.CardInfoId = entity.Id;
                await _cardAmountHistoryService.AddAsync(cardAmountHistory, true);
            }
            //سابقه شارژ اعتبار ذخیره می شود
            if (entity.CreditAmount != 0)
            {
                CardAmountHistory cardAmountHistory = CreateCardAmountHistory(null, entity, true);
                cardAmountHistory.CardInfoId = entity.Id;
                await _cardAmountHistoryService.AddAsync(cardAmountHistory, true);
            }

            return (true, string.Empty);
        }
        //بعد از بروزرسانی کارت، در صورتی که تغییری در بن ریالی یا اعتبار کارت انجام گرفته باشد سابقه مربوطه را ذخیره می کند

        protected override async Task CustomMapBeforeUpdateAsync(CardInfoEditModel editModel, CardInfo entity)
        {
            CardInfo cardInfo = _cardInfoService.Get(entity.Id, true);
            if ((cardInfo.CashAmount != entity.CashAmount))
            {
                CardAmountHistory cardAmountHistory = CreateCardAmountHistory(cardInfo, entity, false);
                await _cardAmountHistoryService.AddAsync(cardAmountHistory);
            }
            if ((cardInfo.CreditAmount != entity.CreditAmount))
            {
                CardAmountHistory cardAmountHistory = CreateCardAmountHistory(cardInfo, entity, true);
                await _cardAmountHistoryService.AddAsync(cardAmountHistory);
            }
            await base.CustomMapBeforeUpdateAsync(editModel, entity);
        }

        //متد کمکی برای ایجاد و مقداردهی شی مربوط به سابقه شارژ
        private CardAmountHistory CreateCardAmountHistory(CardInfo oldEntity, CardInfo newEntity, bool CreditOrCash)
        {
            CardAmountHistory cardAmountHistory = new CardAmountHistory();
            if (CreditOrCash)
            {
                cardAmountHistory.NewAmount = newEntity.CreditAmount;
                if (oldEntity == null)
                {
                    cardAmountHistory.CurrentAmount = 0;
                }
                else
                {
                    cardAmountHistory.CurrentAmount = oldEntity.CreditAmount;
                    cardAmountHistory.CardInfoId = oldEntity.Id;
                }
            }
            else
            {
                cardAmountHistory.NewAmount = newEntity.CashAmount;
                if (oldEntity == null)
                {
                    cardAmountHistory.CurrentAmount = 0;
                }
                else
                {
                    cardAmountHistory.CurrentAmount = oldEntity.CashAmount;
                    cardAmountHistory.CardInfoId = oldEntity.Id;
                }
            }
            cardAmountHistory.CreditOrCash = CreditOrCash;
            return cardAmountHistory;
        }

        //این متد برای کسب و کار مربوط به شارژ بن ریالی یا اعتبار گروهی کارت های یک سازمان مشخص می باشد
        [HttpPost("DoCardCharge")]
        public ResultModel DoCardCharge(CardChargeModel cardChargeModel)
        {
            /*System.Collections.Generic.List<CardInfo> list = _cardInfoService.GetCardInfoByCardTypeAndOrganization(cardChargeModel.CardTypeId, cardChargeModel.OrganizationId);
            int count = list.Count;*/
            int applicationId = _userPrincipal.ApplicationId;
            int organizationId = _userPrincipal.OrganizationId;
            int id = _userPrincipal.Id;
            var resultModel = new ResultModel();
            resultModel.Succeeded = false;
            if (cardChargeModel.Amount > 0)
            {
                _cardInfoService.DoCardCharge(applicationId, id, organizationId, cardChargeModel.OrganizationId, cardChargeModel.CardTypeId, cardChargeModel.Amount, cardChargeModel.IsCredit);
                resultModel.Succeeded = true;
            }
            return resultModel;
        }

        //بارگذاری فایل اکسل حاوی اطلاعات کارت های مربوط به یک سازمان خاص
        [HttpPost("uploadCardInfo")]
        public async Task<IActionResult> AsyncUpload(IFormFile cardInfoFile)
        {
            //تعیین مسیر ذخیره سازی فایل اکسل بارگذاری شده
            string targetLocation = Path.Combine(GlobalConfiguration.WebRootPath, "uploads");
            try
            {
                if (!Directory.Exists(targetLocation))
                    Directory.CreateDirectory(targetLocation);
                await using (var fileStream = System.IO.File.Create(Path.Combine(targetLocation, cardInfoFile.FileName)))
                {
                    await cardInfoFile.CopyToAsync(fileStream);
                }
            }
            catch
            {
                return BadRequest();
            }
            return Ok();
        }

        //افزودن کارت های موجود که در فایل اکسل تعریف شده اند
        [HttpPost("addCardInfo")]
        public async Task<IActionResult> AddCardInfo(CardInfoModel cardInfoModel)
        {
            int applicationId = _userPrincipal.ApplicationId;
            int creatorOrganizationId = _userPrincipal.OrganizationId;
            int createdBy = _userPrincipal.Id;
            string licenseLocation = Path.Combine(GlobalConfiguration.WebRootPath, $"aspose-license{Path.DirectorySeparatorChar}Aspose-License.txt");
            License license = new License();
            license.SetLicense(licenseLocation);
            string fileLocation = Path.Combine(GlobalConfiguration.WebRootPath, $"uploads{Path.DirectorySeparatorChar}{cardInfoModel.Filename}");
            Workbook workbook = new Workbook(fileLocation);
            Worksheet worksheet = workbook.Worksheets[0];
            int column = 0;
            int row = 1;
            int rows = worksheet.Cells.MaxDataRow;
            int columns = worksheet.Cells.MaxDataColumn + 1;
            Aspose.Cells.Range range = worksheet.Cells.CreateRange(row, column, rows, columns);
            DataTable dataTable = range.ExportDataTable();
            int row_count = 1;
            var resultList = new System.Collections.Generic.List<ResultModel>();
            foreach (DataRow dataRow in dataTable.Rows)
            {
                row_count++;
                var result = new ResultModel();
                result.Status = row_count;
                var item = dataRow.ItemArray;
                if (item[0] == null || string.IsNullOrWhiteSpace(item[0].ToString()) ||
                    item[1] == null || string.IsNullOrWhiteSpace(item[1].ToString()) ||
                    item[3] == null || string.IsNullOrWhiteSpace(item[3].ToString()) ||
                    item[5] == null || string.IsNullOrWhiteSpace(item[5].ToString()) ||
                    item[7] == null || string.IsNullOrWhiteSpace(item[7].ToString()))
                {
                    result.Message = "ستون های نام، نام خانوادگی، کدملی، شماره کارت و رمز اول کارت باید تکمیل شوند";
                    result.Succeeded = false;
                    resultList.Add(result);
                    continue;
                }
                CardInfoDto cardInfoDto = new CardInfoDto
                {
                    ApplicationId = applicationId,
                    CreatorOrganizationId = creatorOrganizationId,
                    CreateBy = createdBy,
                    CardTypeId = cardInfoModel.CardTypeId,
                    OrganizationId = cardInfoModel.OrganizationId,
                    Name = item[0].ToString(),
                    Family = item[1].ToString(),
                    Gender = item[2] == null || string.IsNullOrWhiteSpace(item[2].ToString()) ? null : item[2].ToString(),
                    NationalCode = item[3].ToString(),
                    MobileNumber = item[4] == null || string.IsNullOrWhiteSpace(item[4].ToString()) ? null : item[4].ToString(),
                    CardNumber = item[5].ToString(),
                    CVV2 = item[6] == null || string.IsNullOrWhiteSpace(item[6].ToString()) ? null : item[6].ToString(),
                    FirstPassword = item[7].ToString(),
                    SecondPassword = item[8] == null || string.IsNullOrWhiteSpace(item[8].ToString()) ? null : item[8].ToString()
                };
                if (item[9] != null && string.IsNullOrWhiteSpace(item[9].ToString()))
                {
                    cardInfoDto.ExpireDate = Int32.Parse(item[9].ToString());
                }
                else
                {
                    cardInfoDto.ExpireDate = null;
                }
                if (cardInfoDto.ExpireDate != null && cardInfoDto.ExpireDate.ToString().Length != 7)
                {
                    string temp = "14" + cardInfoDto.ExpireDate.ToString();
                    cardInfoDto.ExpireDate = Int32.Parse(temp.Replace("/", ""));
                }
                cardInfoDto.CreditAmount = item[10] == null || string.IsNullOrWhiteSpace(item[10].ToString()) ? 0 : Int32.Parse(item[10].ToString());
                cardInfoDto.CashAmount = item[11] == null || string.IsNullOrWhiteSpace(item[11].ToString()) ? 0 : Int32.Parse(item[11].ToString());
                string pass = item[12] == null || string.IsNullOrWhiteSpace(item[12].ToString()) ? null : item[12].ToString();
                if (pass == null)
                {
                    pass = "1qaz!QAZ";
                }
                CryptographyHelper.CreatePasswordHash(pass, cardInfoDto.NationalCode, out var passwordHash, out var passwordSalt);
                cardInfoDto.Password = passwordHash;
                cardInfoDto.PasswordSalt = passwordSalt;
                result.Message = _cardInfoService.AddCardInfo(cardInfoDto.ApplicationId, cardInfoDto.CreateBy, cardInfoDto.CreatorOrganizationId, cardInfoDto.OrganizationId, cardInfoDto.CardTypeId, cardInfoDto.Name, cardInfoDto.Family, cardInfoDto.Gender, cardInfoDto.NationalCode, cardInfoDto.MobileNumber, cardInfoDto.CardNumber, cardInfoDto.CVV2, cardInfoDto.FirstPassword, cardInfoDto.SecondPassword, cardInfoDto.ExpireDate, cardInfoDto.CreditAmount, cardInfoDto.CashAmount, cardInfoDto.Password, cardInfoDto.PasswordSalt);
                if (!string.IsNullOrEmpty(result.Message))
                {
                    result.Succeeded = false;
                    resultList.Add(result);
                }
            }
            //اگر اعتبارسنجی داده هر کدام از سطرهای موجود در فایل اکسل  با خطا رو به رو شود، یک فایل اکسل جدید شامل شماره سطر و پیغام خطا تولید می شود و برای دانلود در اختیار کاربر قرار می گیرد
            if (resultList.Count > 0)
            {
                var book = new Workbook();
                var sheet = book.Worksheets[0];
                ImportTableOptions imp = new ImportTableOptions
                {
                    InsertRows = true
                };
                sheet.Cells.ImportCustomObjects(resultList, new string[] { "Succeeded", "Message", "Status" }, true, 0, 0, resultList.Count, true, "yyyy/mm/dd", false);
                // book.Worksheets[0].AutoFitColumns();
                MemoryStream ms = new MemoryStream();
                book.Save(ms, SaveFormat.Xlsx);
                ms.Seek(0, SeekOrigin.Begin);
                return File(ms, MimeTypes.GetMimeType("Result.xlsx"), "Result.xlsx");
            }
            return Ok();
        }

    }
}