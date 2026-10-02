using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Yashil.Core.Interfaces.Services
{
    public interface IExcelBuilder
    {
        /// <summary>
        /// عنوان 
        /// </summary>
        /// <param name="row">موقعیت شروع سطر</param>
        /// <param name="column">موقعیت شروع ستون</param>
        /// <param name="columnCount">تعداد ستون اشغال شده توسط عنوان</param>
        /// <param name="value">متن عنوان</param>
        /// <param name="hideCell">مخفی کردن محتوای سلول مورد نظر</param>
        void AddTitle(int row, int column, int columnCount, string value,bool hideCell = false,bool autoFit = true);
        /// <summary>
        /// هدر هر ستون
        /// </summary>
        /// <param name="headers">مقدار هدر</param>
        /// <param name="rowIndex"></param>
        /// <param name="autoFit">تنظیم اتوماتیک اندازه سلولها</param>
        void AddHeaders(IReadOnlyList<string> headers, int rowIndex = 1,bool autoFit = true);
        /// <summary>
        /// دیتای فایل اکسل
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">دیتا</param>
        /// <param name="propertyNames">نام ویژگی</param>
        /// <param name="textColumns">ستونهایی که باید بصورت text  باشد</param>
        /// <param name="startRow">سطر شروع</param>
        /// <param name="startColumn">ستون شروع</param>
        /// <param name="autoFit">تنظیم اتوماتیک اندازه سلولها</param>
        void ImportData<T>(
            IReadOnlyList<T> source,
            string[] propertyNames,
            IEnumerable<string> textColumns = null,
            int startRow = 2,
            int startColumn = 0
            , bool autoFit = true);

        void AddDataValidation(
            List<string> source,
            string rangeName,
            int endRow,
            int startColumn,
            int validationRangeFirstColumn            
            );

        Stream Build();
    }
}
