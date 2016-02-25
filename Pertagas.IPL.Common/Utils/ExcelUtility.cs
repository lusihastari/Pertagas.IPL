using System;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Threading;
using Excel = Microsoft.Office.Interop.Excel;

namespace Pertagas.IPL.Common
{
    public enum HAlignment
    {
        Left,
        Center,
        Right
    }

    public enum VAlignment
    {
        Top,
        Center,
        Bottom
    }

    public enum BorderWeight
    {
        Medium,
        Thick,
        Thin
    }

    public enum RangeProperty
    {
        CellWidth,
        RowHeight,
        HorizontalAlignment,
        VerticalAlignment,
        CellColor
    }

    public static class ExcelUtility
    {
        private static Excel.Application s_application;
        private static Excel.Workbook s_workbook;
        private static Excel.Worksheet s_worksheet;
        private static Excel.Range s_range;
        private static CultureInfo s_oldCI = Thread.CurrentThread.CurrentCulture;

        public static void CreateExcelDocument()
        {
            s_oldCI = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            s_application = new Excel.Application();
            s_application.DisplayAlerts = false;
            s_workbook = s_application.Workbooks.Add(Excel.XlSheetType.xlWorksheet);
            s_worksheet = s_workbook.ActiveSheet as Excel.Worksheet;
        }

        public static void DisplayExcelDocument()
        {
            s_application.DisplayAlerts = true;
            s_application.Visible = true;
            
            if (s_worksheet != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(s_worksheet);
            }

            if (s_workbook != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(s_workbook);
            }

            if (s_application != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(s_application);
            }

            s_worksheet = null;
            s_workbook = null;
            s_application = null;
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public static void CloseExcelDocument()
        {
            s_application.Quit();

            if (s_worksheet != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(s_worksheet);
            }

            if (s_workbook != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(s_workbook);
            }

            if (s_application != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(s_application);
            }

            s_worksheet = null;
            s_workbook = null;
            s_application = null;
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }


        public static void SetCurrentCell(int row, int column)
        {
            SetCurrentCell(row, column, null, null);
        }

        public static void SetCurrentCell(int startRow, int startColumn, int? endRow, int? endColumn)
        {
            s_range = s_worksheet.get_Range(
                GetExcelCellName(startColumn, startRow),
                GetExcelCellName((endColumn.HasValue ? endColumn.Value : startColumn), (endRow.HasValue ? endRow.Value : startRow)));
           
        }

        public static void Write(int startRow, int startColumn, int? endRow, int? endColumn, object content, string numberFormat, string fontName, int? fontSize, bool isBold, bool underlined, bool mergeCells)
        {
            s_range = s_worksheet.get_Range(
                GetExcelCellName(startColumn, startRow),
                GetExcelCellName( (endColumn.HasValue ? endColumn.Value : startColumn), (endRow.HasValue ? endRow.Value : startRow)));
           
            if (content != null)
            {
                s_range.set_Value(Missing.Value, content);
            }

            if (!String.IsNullOrEmpty(numberFormat))
            {
                s_range.NumberFormat = numberFormat;
            }

            if (!String.IsNullOrEmpty(fontName))
            {
                s_range.Font.Name = fontName;
            }

            if (fontSize.HasValue)
            {
                s_range.Font.Size = fontSize.Value;
            }

            s_range.Font.Bold = isBold;
            s_range.Font.Underline = underlined;
            s_range.MergeCells = mergeCells;
            s_range.WrapText = true;
        }

        public static void Write(int row, int column, object content, string numberFormat, string fontName, int fontSize, bool isBold, bool underlined)
        {
            Write(row, column, null, null, content, numberFormat, fontName, fontSize, isBold, underlined, false);
        }

        public static void SetPropertyValue(RangeProperty property, object value)
        {
            if (value == null) return;

            switch (property)
            {
                case RangeProperty.CellWidth: s_range.ColumnWidth = value; break;
                case RangeProperty.RowHeight: s_range.RowHeight = value; break;
                case RangeProperty.HorizontalAlignment: s_range.HorizontalAlignment = ToMsExcelHorizontalAlignment((HAlignment)value); break;
                case RangeProperty.VerticalAlignment: s_range.VerticalAlignment = ToMsExcelVerticalAlignment((VAlignment)value); break;
                case RangeProperty.CellColor: s_range.Interior.Color = ColorTranslator.ToOle((System.Drawing.Color)value); break;
                default: break;                    
            }
        }

        public static void SetBorder(bool left, bool top, bool right, bool bottom)
        {
            SetBorder(left, top, right, bottom, BorderWeight.Medium, BorderWeight.Medium, BorderWeight.Medium, BorderWeight.Medium);
        }

        public static void SetBorder(bool left, bool top, bool right, bool bottom, BorderWeight leftBorderWeight, BorderWeight topBorderWeight, BorderWeight rightBorderWeight, BorderWeight bottomBorderWeight)
        {
            if (left)
            {
                s_range.Columns.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                s_range.Columns.Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = ToMsExcelBorderWeight(leftBorderWeight);
            }

            if (top)
            {
                s_range.Columns.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
                s_range.Columns.Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = ToMsExcelBorderWeight(topBorderWeight);
            }

            if (right)
            {
                s_range.Columns.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                s_range.Columns.Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = ToMsExcelBorderWeight(rightBorderWeight);
            }

            if (bottom)
            {
                s_range.Columns.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                s_range.Columns.Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = ToMsExcelBorderWeight(bottomBorderWeight);
            }
        }

        public static void SetGridVisibility(bool isVisible)
        {
            s_application.ActiveWindow.DisplayGridlines = isVisible;
        }

        private static Excel.XlBorderWeight ToMsExcelBorderWeight(BorderWeight weight)
        {
            switch(weight)
            {
                case BorderWeight.Medium: return Excel.XlBorderWeight.xlMedium;
                case BorderWeight.Thick: return Excel.XlBorderWeight.xlThick;
                case BorderWeight.Thin: return Excel.XlBorderWeight.xlThin;
                default: return Excel.XlBorderWeight.xlMedium;
            }
        }

        private static Excel.XlHAlign ToMsExcelHorizontalAlignment(HAlignment hAlign)
        {
            switch (hAlign)
            {
                case HAlignment.Center:
                    return Excel.XlHAlign.xlHAlignCenter;
                case HAlignment.Left:
                    return Excel.XlHAlign.xlHAlignLeft;
                case HAlignment.Right:
                    return Excel.XlHAlign.xlHAlignRight;
                default:
                    return Excel.XlHAlign.xlHAlignLeft;
            }
        }

        private static Excel.XlVAlign ToMsExcelVerticalAlignment(VAlignment vAlign)
        {
            switch (vAlign)
            {
                case VAlignment.Bottom:
                    return Excel.XlVAlign.xlVAlignBottom;
                case VAlignment.Center:
                    return Excel.XlVAlign.xlVAlignCenter;
                case VAlignment.Top:
                    return Excel.XlVAlign.xlVAlignTop;
                default:
                    return Excel.XlVAlign.xlVAlignBottom;
            }
        }

        public static string GetExcelCellName(int columnIndex, int? rowIndex)
        {
            return GetExcelCellName(columnIndex, rowIndex, false, false);
        }

        public static string GetExcelCellName(int columnIndex, int? rowIndex, bool specificColumn, bool specificRow)
        {
            string sRet = String.Empty;
            while (columnIndex > 25)
            {
                int iAdd = columnIndex % 26;
                columnIndex = (columnIndex - iAdd) / 26 - 1;

                sRet = String.Concat(((char)('A' + iAdd)).ToString(), sRet);
            }
            sRet = String.Concat(((char)('A' + columnIndex)).ToString(), sRet);

            string formula = String.Concat(sRet, (rowIndex.HasValue ? (rowIndex + 1).ToString() : String.Empty));
            if (specificColumn)
            {
                formula = String.Concat("$", sRet, (rowIndex.HasValue ? (rowIndex + 1).ToString() : String.Empty));
            }
            if (specificRow)
            {
                formula = String.Concat(sRet, (rowIndex.HasValue ? String.Concat("$", (rowIndex + 1).ToString()) : String.Empty));
            }
            if (specificRow && specificColumn)
            {
                formula = String.Concat("$", sRet, (rowIndex.HasValue ? String.Concat("$", (rowIndex + 1).ToString()) : String.Empty));
            }

            return formula;
        }
    }
}
