using Aspose.Cells;
using JetBrains.Annotations;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Yashil.Common.Core.Classes;
using Yashil.Common.SharedKernel;
using Yashil.Common.SharedKernel.Helpers;
using Yashil.Common.SharedKernel.Logging;
using Yashil.Core;

namespace Yashil.Infrastructure
{
    public class SqlBulkCopyExcelToDb : ISqlBulkCopyExcelToDb
    {
        private readonly IConfiguration _configuration;
        private readonly IUserPrincipal _userPrincipal;
        private readonly IYashilLogger<SqlBulkCopyExcelToDb> _logger;

        public SqlBulkCopyExcelToDb(IConfiguration configuration, IUserPrincipal userPrincipal, IYashilLogger<SqlBulkCopyExcelToDb> logger)
        {
            _configuration = configuration;
            _userPrincipal = userPrincipal;
            _logger = logger;

        }

        /// <summary>
        /// فایل اکسل را از مسیر داده شده در پارامتر متد  خوانده واز اندیس شیت اکسل (که در پارامتر متد اعلام شده است )دیتا را خوانده و در 
        /// DataTable
        /// میریزد
        /// ستون های جدول را با ستون 
        /// DataTable
        /// مپ میکند و در نهایت به صورت 
        ///  SqlBulk
        ///  در دیتا بیس و در جدول اعلامی در پارامتر درج میکند 
        /// </summary>
        /// <param name="tableName">نام جدول</param>
        /// <param name="filePath">مسیر فایل</param>
        /// <param name="sheetIndex">اندیس شیت</param>
        /// <param name="tableColumns">ستون های جدول</param>
        /// <param name="appDocumentId">شناسه سند</param>
        /// <param name="customColumns">ستون های که در فایل نیستند و بنابر نیاز به جداول یا دیتا ارسال می شوند</param>
        /// <param name="skippedRowCount"></param>
        /// <param name="addCreationDate">تاریخ ایجاد اضافه شود</param>
        /// <returns></returns>
        public async Task<bool> BulkInsertFromExcelOrCsvAsync(
            string tableName,
            string filePath,
            int sheetIndex,
            string[] tableColumns,
            int appDocumentId,
            int skippedRowCount = 1,
            bool addCreationDate = false,
            Dictionary<string, object> customColumns = null,
            int headerRowIndex = 0)
        {
            string fileLocation = Path.Combine(GlobalConfiguration.WebRootPath, filePath);
            var workbook = new Workbook(fileLocation, new TxtLoadOptions
            {
                Separator = ';',
                LoadFilter = new LoadFilter(LoadDataFilterOptions.CellData),
                CheckExcelRestriction = false,
                ConvertNumericData = false,
                ConvertDateTimeData = false
            });

            // var workbook = new Workbook(fileLocation);
            var worksheet = workbook.Worksheets[sheetIndex];
            var rng = worksheet.Cells.MaxDisplayRange;
            
            DataTable excelData;

            if (headerRowIndex == 0)
            {
                // درصورتی که هدر قبلی باشه
                excelData = worksheet.Cells.ExportDataTable(
                    rng.FirstRow,
                    rng.FirstColumn,
                    rng.RowCount,
                    rng.ColumnCount,
                    new ExportTableOptions
                    {
                        CheckMixedValueType = false,
                        ExportColumnName = true,
                        ExportAsString = true
                    });
            }
            else
            {
                excelData = new DataTable();

                // ساخت ستون‌ها از روی Header
                for (int c = 0; c < rng.ColumnCount; c++)
                {
                    var columnName = worksheet.Cells[headerRowIndex, c].StringValue?.Trim();

                    if (string.IsNullOrWhiteSpace(columnName))
                        columnName = $"Column{c + 1}";

                    excelData.Columns.Add(columnName, typeof(object));
                }

                // داده‌ها
                for (int r = headerRowIndex + 1; r < rng.FirstRow + rng.RowCount; r++)
                {
                    var row = excelData.NewRow();

                    for (int c = rng.FirstColumn;
                         c < rng.FirstColumn + rng.ColumnCount;
                         c++)
                    {
                        var cell = worksheet.Cells[r, c];

                        row[c - rng.FirstColumn] =
                            cell.Value ?? DBNull.Value;
                    }

                    excelData.Rows.Add(row);
                }
            }

            if (excelData.Rows.Count - skippedRowCount < 1)
            {
                return false;
            }
            while (excelData.Columns.Count > tableColumns.Length)
            {
                excelData.Columns.RemoveAt(tableColumns.Length);
            }
            excelData.Columns.Add("CreateBy", typeof(int));
            excelData.Columns.Add("AppDocumentId", typeof(int));
            excelData.AsEnumerable().Select(b => b["CreateBy"] = _userPrincipal.Id).ToList();
            excelData.AsEnumerable().Select(b => b["AppDocumentId"] = appDocumentId).ToList();

            if (addCreationDate)
            {
                excelData.Columns.Add("CreationDate", typeof(DateTime));
                excelData.AsEnumerable().Select(b => b["CreationDate"] = DateTime.Now).ToList();
            }
            // اضافه کردن ستون های مورد نیاز به دیتا تیبل
            if (customColumns != null)
            {
                foreach (var column in customColumns)
                {
                    excelData.Columns.Add(column.Key, column.Value.GetType());
                    excelData.AsEnumerable().Select(b => b[column.Key] = column.Value).ToList();
                }
            }
            var connectionString = CryptographyHelper.AesDecrypt(_configuration.GetConnectionString("YashilAppDB"));
            await using var con = new SqlConnection(connectionString);
            using var sqlBulkCopy = new SqlBulkCopy(con);
            try
            {
                //اگر سطون تاریخ ایجاد اضافه شده باشد
                //باید تعداد را منهای 3 کنیم چون 3 ستون به صورت خودکار مقدار دهی شده اند
                var skipColumnsCount = addCreationDate ? 3 : 2;
                if (customColumns != null)
                {
                    skipColumnsCount += customColumns.Count;
                }

                for (var i = 0; i < excelData.Columns.Count - skipColumnsCount; i++)
                {
                    sqlBulkCopy.ColumnMappings.Add(excelData.Columns[i].ColumnName, tableColumns[i]);
                }
                sqlBulkCopy.ColumnMappings.Add("CreateBy", "CreateBy");
                sqlBulkCopy.ColumnMappings.Add("AppDocumentId", "AppDocumentId");

                if (addCreationDate)
                    sqlBulkCopy.ColumnMappings.Add("CreationDate", "CreationDate");
                // اضافه کردن ستون های مورد نیاز به SqlBulk
                if (customColumns != null)
                {
                    foreach (var column in customColumns)
                    {
                        sqlBulkCopy.ColumnMappings.Add(column.Key, column.Key);
                    }
                }
                //Set the database table name.
                sqlBulkCopy.DestinationTableName = tableName;
                con.Open();
                var newRows = excelData.AsEnumerable().Skip(skippedRowCount);

                var excelDataSkippedRowOne = newRows.CopyToDataTable();
                await sqlBulkCopy.WriteToServerAsync(excelDataSkippedRowOne);
            }
            catch (Exception e)
            {
                _logger.InsertLog(LogLevel.Error, (int)Events.UpdateException, tableName, $" بروز خطا در sqlBulkCopy ریختن از اکسل به پایگاه داده", e);
                throw;
            }
            finally
            {
                con.Close();
            }

            return true;

        }

        /// <summary>
        /// برای ساخت  لیست انتخابی در شیت اصلی اکسل از این متد استفاده میشود
        /// ابتدا یک شیت جدید برای اکسل تعریف میکنیم تا سورس لیست انتخابی در آن قرار گیرید
        /// سپس رنج لیست انتخابی را با محل قرار گیری آن در شیت جدید تعریف می کنیم و رنج را با سورس لیست انتخابی مقدار دهی میکنیم
        /// سپس محل قرار گیری لیست انتخابی در شیت اصلی اکسل را مشخص می کنیم
        /// </summary>
        /// <param name="mainWorkbook">اکسل اصلی</param>
        /// <param name="dataValidationSource">سورس مربوط به لیست انتخابی</param>
        /// <param name="rangeName">نام رنج مربوط به لیست انتخابی</param>
        /// <param name="validationRangeTotalColumn"></param>
        /// <param name="startRowInMainWorkbook">شروع سطر مربوط محل قرار گیری رنج در شیت اصلی</param>
        /// <param name="endRowInMainWorkbook">پایان سطر مربوط محل قرار گیری رنج در شیت اصلی</param>
        /// <param name="startColumnInMainWorkbook">شروع ستون مربوط محل قرار گیری رنج در شیت اصلی</param>
        /// <param name="endColumnInMainWorkbook">پایان ستون مربوط محل قرار گیری رنج در شیت اصلی</param>
        /// <param name="validationRangeFirstColumn"></param>
        /// <param name="validationRangeFirstRow"></param>
        public void CreateDataValidation(Workbook mainWorkbook,
            List<string> dataValidationSource,
            string rangeName,
            int endRowInMainWorkbook,
            int startColumnInMainWorkbook,
            int validationRangeFirstColumn,
            int validationRangeFirstRow = 1,
            int validationRangeTotalColumn = 1,
            int startRowInMainWorkbook = 1,
            int? endColumnInMainWorkbook = null)
        {
            endColumnInMainWorkbook ??= startColumnInMainWorkbook;
            int totalRow = dataValidationSource.Count;

            var mainWorksheet = mainWorkbook.Worksheets[0];
            //ساخت شیت اکسل برای آیتم های لیست باکس در شیت اصلی محصولات مرکز
            var sheetNumber = mainWorkbook.Worksheets.Add();
            Worksheet validationSourceSheet = mainWorkbook.Worksheets[sheetNumber];
            validationSourceSheet.IsVisible = false;
            validationSourceSheet.Protect(ProtectionType.All, "aspose", null);
            // ساخت رنج برای لیست انتخابی
            var range = validationSourceSheet.Cells.CreateRange(validationRangeFirstRow, validationRangeFirstColumn,
                totalRow, validationRangeTotalColumn);
            range.Name = rangeName;
            for (var i = 0; i < dataValidationSource.Count; i++)
            {
                range[i, 0].PutValue(dataValidationSource[i]);
            }

            // قرار دادن لیست انتخابی در شیت اصلی اکسل
            ValidationCollection dataValidation = mainWorksheet.Validations;
            SetDataValidationInMainSheet(
                dataValidation,
                startRowInMainWorkbook,
                endRowInMainWorkbook,
                startColumnInMainWorkbook,
                endColumnInMainWorkbook.Value,
                rangeName);
        }
        /// <summary>
        /// تنظیمات مربوط به لیست آیتم
        /// </summary>
        /// <param name="validations">سورس </param>
        /// <param name="endColumnInMainWorkbook"></param>
        /// <param name="formula1">نام rang که به در فرمول قرار میگیرد</param>
        /// <param name="startRowInMainWorkbook"></param>
        /// <param name="endRowInMainWorkbook"></param>
        /// <param name="startColumnInMainWorkbook"></param>
        public void SetDataValidationInMainSheet(
            ValidationCollection validations,
            int startRowInMainWorkbook,
            int endRowInMainWorkbook,
            int startColumnInMainWorkbook,
            int endColumnInMainWorkbook,
            string formula1)
        {
            // Create Cell Area
            var ca = new CellArea
            {
                StartRow = startRowInMainWorkbook,
                EndRow = endRowInMainWorkbook,
                StartColumn = startColumnInMainWorkbook,
                EndColumn = endColumnInMainWorkbook
            };
            // Create a new validation to the validations list.
            var validation = validations[validations.Add(ca)];

            // Set the validation type.
            validation.Type = ValidationType.List;

            // Set the operator.
            validation.Operator = OperatorType.None;

            // Set the in cell drop down.
            validation.InCellDropDown = true;

            // Set the formula1.
            validation.Formula1 = $"={formula1}";

            // Enable it to show error.
            validation.ShowError = true;

            // Set the alert type severity level.
            validation.AlertStyle = ValidationAlertType.Stop;

            // Set the error title.
            validation.ErrorTitle = "خطا";

            // Set the error message.
            validation.ErrorMessage = "کاربر گرامی لطفا از آیتم های لیست شده یک مورد را انتخاب نمایید";


            // Specify the validation area.
            CellArea area;
            area.StartRow = startRowInMainWorkbook;
            area.EndRow = endRowInMainWorkbook;
            area.StartColumn = startColumnInMainWorkbook;
            area.EndColumn = endColumnInMainWorkbook;

            // Add the validation area.
            validation.AddArea(area);
        }


    }
}
