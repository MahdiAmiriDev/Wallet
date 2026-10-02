using System.Collections.Generic;
using System.Threading.Tasks;
using Aspose.Cells;
using JetBrains.Annotations;

namespace Yashil.Core
{
    public interface ISqlBulkCopyExcelToDb
    {
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
        /// <param name="columns">ستون های جدول</param>
        /// <param name="appDocumentId">شناسه سند</param>
        /// <param name="customColumns">ستون های که در فایل نیستند و بنابر نیاز به جداول یا دیتا ارسال می شوند</param>
        /// <param name="skippedRowCount"></param>
        /// <param name="addCreationDate">تاریخ ایجاد اضافه شود</param>
        /// <returns></returns>
        Task<bool> BulkInsertFromExcelOrCsvAsync(string tableName, string filePath,
            int sheetIndex,
            string[] columns,
            int appDocumentId,
            int skippedRowCount = 1,
            bool addCreationDate = false,
            Dictionary<string, object> customColumns = null,
            int headerRowIndex = 0
            );

        void CreateDataValidation(Workbook mainWorkbook,
            List<string> dataValidationSource,
            string rangeName,
            int endRowInMainWorkbook,
            int startColumnInMainWorkbook,
            int validationRangeFirstColumn,
            int validationRangeFirstRow = 1,
            int validationRangeTotalColumn = 1,
            int startRowInMainWorkbook = 1,
            int? endColumnInMainWorkbook = null);

        /// <summary>
        /// تنظیمات مربوط به لیست آیتم
        /// </summary>
        /// <param name="validations">سورس </param>
        /// <param name="endColumnInMainWorkbook"></param>
        /// <param name="formula1">نام rang که به در فرمول قرار میگیرد</param>
        /// <param name="startRowInMainWorkbook"></param>
        /// <param name="endRowInMainWorkbook"></param>
        /// <param name="startColumnInMainWorkbook"></param>
        void SetDataValidationInMainSheet(
           ValidationCollection validations,
           int startRowInMainWorkbook,
           int endRowInMainWorkbook,
           int startColumnInMainWorkbook,
           int endColumnInMainWorkbook,
           string formula1);

    }
}
