using System.Collections.Generic;

namespace FineUIMvc.PumpMVC.Common.ExcelExport
{
    public interface IExcelExport
    {
        int NumberOfSheet { get; }
        IExcelExport CreateWorkbook();
        IExcelExport CreateWorkbook(string templatePath);
        IExcelExport AddSheet<T>(IList<T> dataList, string sheetName = "");
        IExcelExport AddSheet<T>(IList<T> dataList, bool hasCellBorder, string sheetName = "");
        IExcelExport AddSheetExcludeHeader<T>(IList<T> dataList, bool hasCellBorder = true, string sheetName = "");
        IExcelExport AddStringRowToSheet<T>(List<string> strData, string sheetName = "");
        IExcelExport AddStringRowToSheet(List<string> strData, string sheetName);
        byte[] WriteToBytes();
        string SaveFile(string filePath);
        string SaveFile(string folderPath, string fileNameWithoutSuffix);
    }
}
