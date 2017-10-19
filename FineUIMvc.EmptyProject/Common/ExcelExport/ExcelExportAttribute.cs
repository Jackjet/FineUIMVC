using System;

namespace FineUIMvc.PumpMVC.Common.ExcelExport
{
    public class ExcelExportAttribute : Attribute
    {
        public ExcelExportAttribute(string description)
        {
            Description = FormatDescription(description);
            Order = 0;
            DateFormat = "yyyy-MM-dd";
        }

        public string Description { get; private set; }
        public string Remark { get; set; }
        public string DateFormat { get; set; }
        public int Order { get; set; }

        private string FormatDescription(string description)
        {
            //TODO:验证Excel中不合法的命名
            return description.Trim();
        }
    }

    public class ExcelExportChildAttribute : Attribute
    {

    }
}
