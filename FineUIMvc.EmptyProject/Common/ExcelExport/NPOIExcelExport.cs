using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace FineUIMvc.PumpMVC.Common.ExcelExport
{
    public class NPOIExcelExport : IExcelExport
    {
        private int _firstDataRowIndexOfSheet;
        private IWorkbook _workbook;
        private int _sheetCount;
        private readonly string  _suffixLower = ".xlsx";

        #region Implement

        public int NumberOfSheet
        {
            get { return _sheetCount; }
        }

        public IExcelExport CreateWorkbook()
        {
            _workbook = new XSSFWorkbook();

            return this;
        }

        public IExcelExport CreateWorkbook(string templatePath)
        {
            if (string.IsNullOrEmpty(templatePath) || !File.Exists(templatePath))
            {
                throw new ArgumentException("Tempalte path is invalid.", "templatePath");
            }
            if (Path.GetExtension(templatePath).ToLower() != _suffixLower)
            {
                throw new ArgumentException("Only support .xlsx file.", "templatePath");
            }

            try
            {
                using (var file = new FileStream(templatePath, FileMode.Open, FileAccess.Read))
                {
                    _workbook = new XSSFWorkbook(file);
                }

                _sheetCount = _workbook.NumberOfSheets;
            }
            catch
            {
                throw new ArgumentException("Tempalte file is invalid.", "templatePath");
            }

            return this;
        }

        public IExcelExport AddSheet<T>(IList<T> dataList, string sheetName = "")
        {
            return AddSheet(dataList, true, sheetName);
        }

        public IExcelExport AddSheet<T>(IList<T> dataList, bool hasCellBorder, string sheetName = "")
        {
            var sheet = GetOrCreateSheet<T>(sheetName);

            int headerRowCount = WriteHeaderToSheet<T>(sheet, _firstDataRowIndexOfSheet, hasCellBorder);
            WriteDataToSheet(sheet, dataList, _firstDataRowIndexOfSheet + headerRowCount, hasCellBorder);

            return this;
        }

        public IExcelExport AddSheetExcludeHeader<T>(IList<T> dataList, bool hasCellBorder = false, string sheetName = "")
        {
            var sheet = GetOrCreateSheet<T>(sheetName);

            WriteDataToSheet(sheet, dataList, _firstDataRowIndexOfSheet, hasCellBorder);

            return this;
        }

        public IExcelExport AddStringRowToSheet<T>(List<string> strData, string sheetName = "")
        {
            var sheet = GetOrCreateSheet<T>(sheetName);
            WriteStringRowToSheet(sheet, _firstDataRowIndexOfSheet, strData);
            return this;
        }

        public IExcelExport AddStringRowToSheet(List<string> strData, string sheetName)
        {
            var sheet = GetOrCreateSheet(sheetName);
            WriteStringRowToSheet(sheet, _firstDataRowIndexOfSheet, strData);
            return this;
        }

        public byte[] WriteToBytes()
        {
            using (var ms = new MemoryStream())
            {
                _workbook.Write(ms);
                return ms.ToArray();
            }
        }

        public string SaveFile(string filePath)
        {
            File.WriteAllBytes(filePath, WriteToBytes());

            return filePath;
        }

        public string SaveFile(string folderPath, string fileNameWithoutSuffix)
        {
            string saveToPath = Path.Combine(folderPath, fileNameWithoutSuffix + _suffixLower);

            return SaveFile(saveToPath);
        }

        #endregion Implement

        #region private methods

        private string GetSheetNameBy<T>()
        {
            var type = typeof(T);
            var tableNameAttr = (ExcelExportAttribute) Attribute.GetCustomAttribute(type, typeof (ExcelExportAttribute));

            var sheetName = tableNameAttr == null ? type.Name : tableNameAttr.Description;
            if (string.IsNullOrEmpty(sheetName))
            {
                sheetName = Guid.NewGuid().ToString();
            }

            return sheetName;
        }

        private ISheet GetOrCreateSheet<T>(string sheetName = "")
        {
            if (string.IsNullOrWhiteSpace(sheetName))
            {
                sheetName = GetSheetNameBy<T>();
            }

            return GetOrCreateSheet(sheetName);
        }

        private ISheet GetOrCreateSheet(string sheetName)
        {
            if (string.IsNullOrWhiteSpace(sheetName))
            {
                throw new Exception("sheet name can not be empty");
            }

            var sheet = _workbook.GetSheet(sheetName);
            if (sheet == null)
            {
                sheet = _workbook.CreateSheet(sheetName);
                sheet.DefaultColumnWidth = 15; //almost the same
                sheet.DefaultRowHeight = 300; //actual height multiply 20

                _firstDataRowIndexOfSheet = 0;
                _sheetCount++;
            }
            else
            {
                _firstDataRowIndexOfSheet = GetFirstBlankRowIndex(sheet);
            }

            return sheet;
        }

        //If the first cell is empty, data row will start from the index.
        private int GetFirstBlankRowIndex(ISheet sheet)
        {
            var rowIndex = 0;
            var rows = sheet.GetRowEnumerator();
            while (rows.MoveNext())
            {
                var firstCell = ((IRow)rows.Current).GetCell(0);
                if (firstCell == null || (firstCell.CellType == CellType.Blank && !firstCell.IsMergedCell))
                {
                    break;
                }
                rowIndex++;
            }

            return rowIndex;
        }

        /// <returns>返回表头使用的行数</returns>
        private int WriteHeaderToSheet<T>(ISheet sheet, int startSheetRowIndex, bool hasCellBorder)
        {
            var headerCellStyle = sheet.Workbook.CreateCellStyle();
            headerCellStyle.Alignment = HorizontalAlignment.Center;
            headerCellStyle.VerticalAlignment = VerticalAlignment.Center;
            headerCellStyle.WrapText = true;
            if (hasCellBorder)
            {
                headerCellStyle.BorderTop = BorderStyle.Thin;
                headerCellStyle.BorderRight = BorderStyle.Thin;
                headerCellStyle.BorderBottom = BorderStyle.Thin;
                headerCellStyle.BorderLeft = BorderStyle.Thin;
            }

            var type = typeof (T);
            var props = type.GetProperties().Where(p => p.GetCustomAttributes(true).Any(pa => pa is ExcelExportAttribute)).ToList();

            var headerRow = sheet.CreateRow(startSheetRowIndex);

            for (int i = 0; i < props.Count; i++)
            {
                var pi = props[i];
                var columnAttr = (ExcelExportAttribute) pi.GetCustomAttributes(true).First(pa => pa is ExcelExportAttribute);
                var headerCell = headerRow.CreateCell(i);
                if (!string.IsNullOrEmpty(columnAttr.Remark))
                {
                    if (headerCell.CellComment == null)
                    {
                        var part = sheet.CreateDrawingPatriarch();
                        headerCell.CellComment = part.CreateCellComment(new XSSFClientAnchor());
                    }
                    headerCell.CellComment.String = new XSSFRichTextString(columnAttr.Remark);//TODO:批注效果不理想
                }
                headerCell.CellStyle = headerCellStyle;
                headerCell.SetCellValue(columnAttr.Description);
            }

            return 1;
        }

        private void WriteDataToSheet<T>(ISheet sheet, IList<T> dataList, int startSheetRowIndex,bool hasCellBorder)
        {
            if (dataList == null || dataList.Count == 0)
            {
                return;
            }

            var type = typeof (T);
            var props = type.GetProperties()
                .Where(p => p.GetCustomAttributes(true).Any(pa => pa is ExcelExportAttribute))
                .ToList();

            var cellStyle = sheet.Workbook.CreateCellStyle();
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            //cellStyle.WrapText = true;

            if (hasCellBorder)
            {
                cellStyle.BorderTop = BorderStyle.Thin;
                cellStyle.BorderRight = BorderStyle.Thin;
                cellStyle.BorderBottom = BorderStyle.Thin;
                cellStyle.BorderLeft = BorderStyle.Thin;
            }

            for (int i = 0; i < dataList.Count; i++)
            {
                var row = sheet.CreateRow(startSheetRowIndex + i);
                var data = dataList[i];
                for (int j = 0; j < props.Count; j++)
                {
                    var pi = props[j];
                    var piType = pi.PropertyType.Name;
                    var piValue = pi.GetValue(data, null);
                    string piValueStr = piValue == null ? string.Empty : piValue.ToString();
                    if (piType == "Decimal" || piType == "Int32" || piType == "Double")
                    {
                        double d;
                        double.TryParse(piValueStr, out d);
                        var cell = row.CreateCell(j, CellType.Numeric);
                        cell.CellStyle = cellStyle;
                        cell.SetCellValue(d);
                    }
                    else
                    {
                        var cell = row.CreateCell(j, CellType.String);
                        cell.CellStyle = cellStyle;
                        cell.SetCellValue(piValueStr);
                    }
                }
            }
        }

        private void WriteStringRowToSheet(ISheet sheet, int startSheetRowIndex, List<string> strData)
        {
            if (strData == null || !strData.Any()) return;

            for (int i = 0; i < strData.Count; i++)
            {
                var row = sheet.CreateRow(startSheetRowIndex + i);
                row.CreateCell(0).SetCellValue(strData[i]);
            }
        }

        #endregion private methods
    }
}
