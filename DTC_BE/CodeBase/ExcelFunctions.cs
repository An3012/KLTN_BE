using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Drawing;

namespace DTC_BE.CodeBase
{
    public class ExcelFunctions
    {
        public static void SetCommonStyles(ExcelRange range, int height = 30, string fontName = "Times New Roman", int fontSize = 12, bool bold = false, bool italic = false, bool hasBorder = false, ExcelHorizontalAlignment horizontalAlignment = ExcelHorizontalAlignment.Center, ExcelVerticalAlignment verticalAlignment = ExcelVerticalAlignment.Center)
        {
            range.Style.HorizontalAlignment = horizontalAlignment;
            range.Style.VerticalAlignment = verticalAlignment;
            range.Style.Font.SetFromFont(fontName, fontSize);
            range.Style.Font.Bold = bold;
            range.Style.Font.Italic = italic;
            range.EntireRow.Height = height;
            range.Style.WrapText = true;
            if (hasBorder)
            {
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Color.SetColor(Color.Black);
                range.Style.Border.Left.Color.SetColor(Color.Black);
                range.Style.Border.Right.Color.SetColor(Color.Black);
                range.Style.Border.Top.Color.SetColor(Color.Black);
            }
        }
        public static void MergeAndSetValue(ExcelWorksheet worksheet, string range, string value)
        {
            worksheet.Cells[range].Merge = true;
            worksheet.Cells[range].Value = value;
        }
    }
}
