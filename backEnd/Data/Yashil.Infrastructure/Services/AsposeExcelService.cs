using System;
using Aspose.Cells;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Yashil.Core.Interfaces.Services;
using System.Threading.Tasks;

namespace Yashil.Infrastructure.Services
{
    public class AsposeExcelBuilder : IExcelBuilder
    {
        private readonly Workbook _workbook;
        private readonly Worksheet _worksheet;
        private int _headerColumnCount;

        public AsposeExcelBuilder()
        {
            _workbook = new Workbook();

            _worksheet = _workbook.Worksheets[0];

            _worksheet.DisplayRightToLeft = true;
        }

        public void AddTitle(
          int row,
          int column,
          int columnCount,
          string value,
          bool hideCell = false,
          bool autoFit = true)
        {
            _worksheet.Cells.Merge(
                row,
                column,
                1,
                columnCount);

            var cell = _worksheet.Cells[row, column];

            cell.PutValue(value);

            var style = cell.GetStyle();

            style.HorizontalAlignment = TextAlignmentType.Center;
            style.VerticalAlignment = TextAlignmentType.Center;

            style.Font.IsBold = true;
            style.Font.Size = 14;

            style.BackgroundColor = Color.LightBlue;
            style.ForegroundColor = Color.LightBlue;
            style.Pattern = BackgroundType.Solid;

            if (hideCell)
            {
                style.Custom = ";;;";
            }

            cell.SetStyle(style);

            // ارتفاع همان سطری که Title در آن قرار دارد
            _worksheet.Cells.SetRowHeight(row, 65);

            if (autoFit)
            {
                _worksheet.AutoFitColumns();
            }
        }


        public void AddHeaders(
            IReadOnlyList<string> headers,
            int rowIndex = 1,
            bool autoFit = true)
        {
            _headerColumnCount = headers.Count;

            var style = _workbook.CreateStyle();

            // Header background
            style.BackgroundColor = Color.FromArgb(221, 235, 247);
            style.ForegroundColor = Color.FromArgb(221, 235, 247);
            style.Pattern = BackgroundType.Solid;

            // Font
            style.Font.IsBold = true;
            style.Font.Color = Color.FromArgb(31, 78, 121);

            // Alignment
            style.HorizontalAlignment = TextAlignmentType.Center;
            style.VerticalAlignment = TextAlignmentType.Center;

            // Border
            style.Borders[BorderType.TopBorder].LineStyle =
                CellBorderType.Thin;

            style.Borders[BorderType.BottomBorder].LineStyle =
                CellBorderType.Thin;

            style.Borders[BorderType.LeftBorder].LineStyle =
                CellBorderType.Thin;

            style.Borders[BorderType.RightBorder].LineStyle =
                CellBorderType.Thin;

            // قرار دادن Header ها
            for (var i = 0; i < headers.Count; i++)
            {
                _worksheet.Cells[rowIndex, i]
                    .PutValue(headers[i]);
            }

            // فقط محدوده Header
            var headerRange = _worksheet.Cells.CreateRange(
                rowIndex,
                0,
                1,
                headers.Count);

            headerRange.ApplyStyle(
                style,
                new StyleFlag
                {
                    All = true
                });

            // ارتفاع Header
            _worksheet.Cells.SetRowHeight(rowIndex, 45);

            if (autoFit)
            {
                _worksheet.AutoFitColumns();
            }
        }


        public void ImportData<T>(
              IReadOnlyList<T> source,
              string[] propertyNames,
              IEnumerable<string> textColumns = null,
              int startRow = 2,
              int startColumn = 0,
              bool autoFit = true)
        {
            if (source == null || source.Count == 0)
                return;

            _worksheet.Cells.ImportCustomObjects(
                source.ToList(),
                propertyNames,
                false,
                startRow,
                startColumn,
                source.Count,
                true,
                "yyyy/MM/dd",
                true);

            if (textColumns != null)
            {
                foreach (var columnName in textColumns)
                {
                    var property = typeof(T).GetProperty(columnName);
                    if (property == null)
                        continue;

                    var columnIndex = Array.IndexOf(propertyNames, columnName);
                    if (columnIndex < 0)
                        continue;

                    for (var i = 0; i < source.Count; i++)
                    {
                        var cell = _worksheet.Cells[startRow + i, startColumn + columnIndex];

                        var style = cell.GetStyle();
                        style.Number = 49;          // Text
                                                    // یا:
                                                    // style.SetCustom("@", false);

                        cell.SetStyle(style);

                        // false => رشته را به عدد تبدیل نکن
                        cell.PutValue(property.GetValue(source[i])?.ToString() ?? "", false);
                    }
                }
            }

            // Style
            var dataStyle = _workbook.CreateStyle();
            dataStyle.HorizontalAlignment = TextAlignmentType.Center;
            dataStyle.VerticalAlignment = TextAlignmentType.Center;

            dataStyle.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
            dataStyle.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            dataStyle.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;

            var dataRange = _worksheet.Cells.CreateRange(
                startRow,
                startColumn,
                source.Count,
                propertyNames.Length);

            dataRange.ApplyStyle(dataStyle, new StyleFlag
            {
                HorizontalAlignment = true,
                VerticalAlignment = true,
                BottomBorder = true,
                LeftBorder = true,
                RightBorder = true
            });

            for (var i = 0; i < source.Count; i++)
            {
                _worksheet.Cells.SetRowHeight(startRow + i, 30);
            }

            if (autoFit)
            {
                _worksheet.AutoFitColumns();
            }
        }


        public void AddDataValidation(
            List<string> source,
            string rangeName,
            int endRow,
            int startColumn,
            int validationRangeFirstColumn)
        {
            var sheetNumber = _workbook.Worksheets.Add();

            var validationSheet =
                _workbook.Worksheets[sheetNumber];

            validationSheet.IsVisible = false;

            var range =
                validationSheet.Cells.CreateRange(
                    1,
                    validationRangeFirstColumn,
                    source.Count,
                    1);

            range.Name = rangeName;

            for (int i = 0; i < source.Count; i++)
            {
                range[i, 0].PutValue(source[i]);
            }

            var validations = _worksheet.Validations;

            var area = new CellArea
            {
                StartRow = 2,
                EndRow = endRow,
                StartColumn = startColumn,
                EndColumn = startColumn
            };

            var validation =
                validations[validations.Add(area)];

            validation.Type = ValidationType.List;
            validation.Formula1 = $"={rangeName}";
            validation.InCellDropDown = true;
        }

        public Stream Build()
        {
            var ms = new MemoryStream();

            _workbook.Save(ms, SaveFormat.Xlsx);

            ms.Position = 0;

            return ms;
        }
    }
}
