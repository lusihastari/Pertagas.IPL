using Pertagas.IPL.Common;
using Pertagas.IPL.DataAccess.DAO;
using Pertagas.IPL.Domain;
using System;
using System.Collections.Generic;
using Excel = Microsoft.Office.Interop.Excel;

namespace Pertagas.IPL.Logic
{
    public class IncomeLogic
    {
        public IncomeClusterDomain AddIncomeCluster(ClusterDomain cluster, string occupantName, string addressBlock, string addressNumber,
            string phoneNumber, int month, int year, double amount)
        {
            IncomeClusterDomain incomeCluster = new IncomeClusterDomain();
            incomeCluster.OccupantName = occupantName;
            incomeCluster.ClusterId = cluster.Id;
            incomeCluster.AddressBlock = addressBlock;
            incomeCluster.AddressNumber = addressNumber;
            incomeCluster.PhoneNumber = phoneNumber;
            incomeCluster.Month = month;
            incomeCluster.Year = year;
            incomeCluster.Amount = amount;

            return DaoFactory.IncomeClusterDao.Save(incomeCluster);
        }

        public List<IncomeClusterDomain> GetAllIncomeClusters()
        {
            return DaoFactory.IncomeClusterDao.GetAllIncomeClusters();
        }

        public void DeleteIncomeCluster(IncomeClusterDomain incomeCluster)
        {
            DaoFactory.IncomeClusterDao.Delete(incomeCluster);
        }

        public IncomeClusterDomain UpdateIncomeCluster(IncomeClusterDomain incomeCluster)
        {
            return DaoFactory.IncomeClusterDao.Update(incomeCluster);
        }

        public IncomeDomain AddIncome(IncomeSourceDomain incomeSource, int month, int year, double amount)
        {
            IncomeDomain income = new IncomeDomain();
            income.IncomeSourceId = incomeSource.Id;
            income.Month = month;
            income.Year = year;
            income.Amount = amount;

            return DaoFactory.IncomeDao.Save(income);
        }

        public IncomeDomain UpdateIncome(IncomeDomain income)
        {
            return DaoFactory.IncomeDao.Update(income);
        }

        public void DeleteIncome(IncomeDomain income)
        {
            DaoFactory.IncomeDao.Delete(income);
        }

        public List<IncomeDomain> GetAllIncomes()
        {
            return DaoFactory.IncomeDao.GetAllIncomes();
        }

        public void GenerateIncomeClusterReport(ClusterDomain cluster, Month fromMonth, int fromYear, Month toMonth, int toYear, out string message)
        {
            message = null;
            bool success = true;
            try
            {
                ExcelUtility.CreateExcelDocument();
                ExcelUtility.SetGridVisibility(false);

                ExcelUtility.Write(4, 1, 5, 1, "No.", null, "Arial", 12, false, false, true);
                ExcelUtility.SetPropertyValue(RangeProperty.HorizontalAlignment, HAlignment.Center);
                ExcelUtility.SetPropertyValue(RangeProperty.VerticalAlignment, VAlignment.Center);
                ExcelUtility.SetPropertyValue(RangeProperty.CellWidth, 5.89);
                ExcelUtility.SetBorder(true, true, true, true, BorderWeight.Thick, BorderWeight.Thick, BorderWeight.Thin, BorderWeight.Thin);

                ExcelUtility.Write(4, 2, 5, 2, "Nama Pemilik/Penghuni Rumah", null, "Arial", 12, true, false, true);
                ExcelUtility.SetPropertyValue(RangeProperty.HorizontalAlignment, HAlignment.Center);
                ExcelUtility.SetPropertyValue(RangeProperty.VerticalAlignment, VAlignment.Center);
                ExcelUtility.SetPropertyValue(RangeProperty.CellWidth, 37.22);
                ExcelUtility.SetBorder(true, true, true, true, BorderWeight.Thin, BorderWeight.Thick, BorderWeight.Thin, BorderWeight.Thin);

                ExcelUtility.Write(4, 3, 4, 4, "Alamat", null, "Arial", 12, true, false, true);
                ExcelUtility.SetPropertyValue(RangeProperty.HorizontalAlignment, HAlignment.Center);
                ExcelUtility.SetPropertyValue(RangeProperty.VerticalAlignment, VAlignment.Center);
                ExcelUtility.SetBorder(true, true, true, true, BorderWeight.Thin, BorderWeight.Thick, BorderWeight.Thin, BorderWeight.Thin);

                ExcelUtility.Write(5, 3, 5, 3, "Blok", null, "Arial", 12, true, false, true);
                ExcelUtility.SetPropertyValue(RangeProperty.HorizontalAlignment, HAlignment.Center);
                ExcelUtility.SetPropertyValue(RangeProperty.VerticalAlignment, VAlignment.Center);
                ExcelUtility.SetPropertyValue(RangeProperty.CellWidth, 5.78);
                ExcelUtility.SetBorder(true, true, true, true, BorderWeight.Thin, BorderWeight.Thin, BorderWeight.Thin, BorderWeight.Thin);

                ExcelUtility.Write(5, 4, 5, 4, "No.", null, "Arial", 12, true, false, true);
                ExcelUtility.SetPropertyValue(RangeProperty.HorizontalAlignment, HAlignment.Center);
                ExcelUtility.SetPropertyValue(RangeProperty.VerticalAlignment, VAlignment.Center);
                ExcelUtility.SetPropertyValue(RangeProperty.CellWidth, 5.78);
                ExcelUtility.SetBorder(true, true, true, true, BorderWeight.Thin, BorderWeight.Thin, BorderWeight.Thin, BorderWeight.Thin);

                ExcelUtility.Write(4, 5, 5, 5, "No. Telpon", null, "Arial", 12, true, false, true);
                ExcelUtility.SetPropertyValue(RangeProperty.HorizontalAlignment, HAlignment.Center);
                ExcelUtility.SetPropertyValue(RangeProperty.VerticalAlignment, VAlignment.Center);
                ExcelUtility.SetPropertyValue(RangeProperty.CellWidth, 11.89);
                ExcelUtility.SetBorder(true, true, true, true, BorderWeight.Thin, BorderWeight.Thick, BorderWeight.Thick, BorderWeight.Thin);

                List<Month> months = MonthUtility.GetMonths();
                int column = 6;
                Dictionary<int, Dictionary<int, int>> columnReferences = new Dictionary<int, Dictionary<int, int>>();
                for (int i = fromYear; i <= toYear; i++)
                {
                    columnReferences[i] = new Dictionary<int, int>();
                    for (int j = fromMonth.Index; j <= toMonth.Index; j++)
                    {
                        columnReferences[i][j] = column;

                        Month month = months.Find(p => p.Index == j);                        
                        ExcelUtility.Write(5, column, month.ShortName.ToUpper(), null, "Arial", 12, true, false);
                        ExcelUtility.SetPropertyValue(RangeProperty.HorizontalAlignment, HAlignment.Center);
                        ExcelUtility.SetPropertyValue(RangeProperty.VerticalAlignment, VAlignment.Center);
                        ExcelUtility.SetPropertyValue(RangeProperty.CellWidth, 11.33);
                        ExcelUtility.SetBorder(j > fromMonth.Index, true, j < toMonth.Index, true, BorderWeight.Thin, BorderWeight.Thin, BorderWeight.Thin, BorderWeight.Thin);

                        ExcelUtility.SetCurrentCell(4, column);
                        ExcelUtility.SetBorder(false, true, false, true, BorderWeight.Thin, BorderWeight.Thick, BorderWeight.Thin, BorderWeight.Thin);

                        column++;
                    }
                }

                ExcelUtility.Write(4, column, 5, column, "TOTAL", null, "Arial", 12, true, false, true);
                ExcelUtility.SetPropertyValue(RangeProperty.HorizontalAlignment, HAlignment.Center);
                ExcelUtility.SetPropertyValue(RangeProperty.VerticalAlignment, VAlignment.Center);
                ExcelUtility.SetPropertyValue(RangeProperty.CellWidth, 11.89);
                ExcelUtility.SetBorder(true, true, true, true, BorderWeight.Thick, BorderWeight.Thick, BorderWeight.Thick, BorderWeight.Thin);

                List<IncomeClusterDomain> incomeClusters = DaoFactory.IncomeClusterDao.GetIncomeClusters(cluster, fromMonth.Index, fromYear, toMonth.Index, toYear);                
                incomeClusters.Sort((p1, p2) =>
                    {
                        if (p1.AddressBlock.CompareTo(p2.AddressBlock) != 0)
                        {
                            return p1.AddressBlock.CompareTo(p2.AddressBlock);
                        }
                        else
                        {
                            return p1.AddressNumber.CompareTo(p2.AddressNumber);
                        }
                    });

                Dictionary<string, int> detailsIndices = new Dictionary<string,int>();                
                int currentRow = 6;
                for (int i = 0; i < incomeClusters.Count; i++)
                {
                    IncomeClusterDomain incomeCluster = incomeClusters[i];
                    string index = string.Join(";", new string[] { incomeCluster.OccupantName, incomeCluster.AddressBlock, incomeCluster.AddressNumber });

                    if (detailsIndices.ContainsKey(index))
                    {
                        ExcelUtility.Write(detailsIndices[index], columnReferences[incomeCluster.Year][incomeCluster.Month], incomeCluster.Amount, "#,##0", "Arial", 12, true, false);                        
                    }
                    else
                    {
                        ExcelUtility.Write(currentRow, 1, (currentRow - 5).ToString(), null, "Arial", 12, true, false);
                        ExcelUtility.SetPropertyValue(RangeProperty.HorizontalAlignment, HAlignment.Center);
                        ExcelUtility.SetPropertyValue(RangeProperty.VerticalAlignment, VAlignment.Center);
                        ExcelUtility.SetBorder(true, true, true, true, BorderWeight.Thick, BorderWeight.Thin, BorderWeight.Thin, BorderWeight.Thin);

                        ExcelUtility.Write(currentRow, 2, incomeCluster.OccupantName, null, "Arial", 12, true, false);
                        ExcelUtility.SetBorder(true, true, true, true, BorderWeight.Thin, BorderWeight.Thin, BorderWeight.Thin, BorderWeight.Thin);

                        ExcelUtility.Write(currentRow, 3, incomeCluster.AddressBlock, null, "Arial", 12, true, false);
                        ExcelUtility.SetBorder(true, true, true, true, BorderWeight.Thin, BorderWeight.Thin, BorderWeight.Thin, BorderWeight.Thin);
                        ExcelUtility.SetPropertyValue(RangeProperty.HorizontalAlignment, HAlignment.Center);

                        ExcelUtility.Write(currentRow, 4, incomeCluster.AddressNumber, null, "Arial", 12, true, false);
                        ExcelUtility.SetBorder(true, true, true, true, BorderWeight.Thin, BorderWeight.Thin, BorderWeight.Thin, BorderWeight.Thin);
                        ExcelUtility.SetPropertyValue(RangeProperty.HorizontalAlignment, HAlignment.Center);

                        ExcelUtility.Write(currentRow, 5, incomeCluster.PhoneNumber, null, "Arial", 12, true, false);
                        ExcelUtility.SetBorder(true, true, true, true, BorderWeight.Thin, BorderWeight.Thin, BorderWeight.Thick, BorderWeight.Thin);

                        ExcelUtility.Write(currentRow, columnReferences[incomeCluster.Year][incomeCluster.Month], incomeCluster.Amount, "#,##0", "Arial", 12, true, false);
                       
                        detailsIndices[index] = currentRow;
                        currentRow++;
                    }
                }

                for (int rowIndex = 6; rowIndex < currentRow; rowIndex++)
                {
                    int columnIndex = 6;
                    for (int i = fromYear; i <= toYear; i++)
                    {
                        for (int j = fromMonth.Index; j <= toMonth.Index; j++)
                        {
                            ExcelUtility.SetCurrentCell(rowIndex, columnIndex);
                            ExcelUtility.SetBorder(columnIndex > 6, true, true, columnIndex < column, BorderWeight.Thin, BorderWeight.Thin, BorderWeight.Thick, BorderWeight.Thin);
                            columnIndex++;
                        }
                    }
                }

                foreach (KeyValuePair<string, int> detailsIndex in detailsIndices)
                {
                    ExcelUtility.Write(detailsIndex.Value, column, "=SUM(" + ExcelUtility.GetExcelCellName(6, detailsIndex.Value) + ":" + ExcelUtility.GetExcelCellName(column - 1, detailsIndex.Value), "#,##0", "Arial", 12, true, false);
                    ExcelUtility.SetBorder(true, true, true, true, BorderWeight.Thick, BorderWeight.Thin, BorderWeight.Thick, BorderWeight.Thin);
                }

                for (int columnIndex = 1; columnIndex <= column; columnIndex++)
                {
                    ExcelUtility.SetCurrentCell(currentRow, columnIndex);

                    BorderWeight leftBorderWeight = BorderWeight.Thin;
                    if (columnIndex == 1 || columnIndex == 6 || columnIndex == column)
                    {
                        leftBorderWeight = BorderWeight.Thick;
                    }

                    ExcelUtility.SetBorder(true, false, true, true, leftBorderWeight, BorderWeight.Thin, columnIndex == column ? BorderWeight.Thick : BorderWeight.Thin, BorderWeight.Thick);
                }

                for (int columnIndex = 1; columnIndex <= column; columnIndex++)
                {
                    ExcelUtility.SetCurrentCell(currentRow + 1, columnIndex);

                    BorderWeight leftBorderWeight = BorderWeight.Thick;
                    if (columnIndex == 2 || columnIndex == 3 || columnIndex == 4 || columnIndex == 5)
                    {
                        leftBorderWeight = BorderWeight.Thin;
                    }

                    ExcelUtility.SetBorder(true, false, columnIndex == column, true, leftBorderWeight, BorderWeight.Thin, BorderWeight.Thick, BorderWeight.Thick);
                }

                ExcelUtility.Write(currentRow + 1, 5, "TOTAL", null, "Arial", 12, true, false);
                ExcelUtility.SetPropertyValue(RangeProperty.HorizontalAlignment, HAlignment.Center);
                ExcelUtility.SetPropertyValue(RangeProperty.VerticalAlignment, VAlignment.Center);

                for (int i = 6; i <= column; i++)
                {
                    ExcelUtility.Write(currentRow + 1, i, "=SUM(" + ExcelUtility.GetExcelCellName(i, 6) + ":" + ExcelUtility.GetExcelCellName(i, currentRow - 1), "#,##0", "Arial", 12, true, false);                        
                }

                ExcelUtility.Write(0, 1, 0, column, "YAYASAN WARGA ARGA PADMA NIRWANA BOGOR", null, "Arial", 12, true, false, true);
                ExcelUtility.SetPropertyValue(RangeProperty.HorizontalAlignment, HAlignment.Center);
                ExcelUtility.SetPropertyValue(RangeProperty.RowHeight, 23.3);

                ExcelUtility.Write(1, 1, 1, column, "Rekapitulasi Iuran Pengelolaan Lingkungan ( I P L )", null, "Arial", 12, true, false, true);
                ExcelUtility.SetPropertyValue(RangeProperty.HorizontalAlignment, HAlignment.Center);
                ExcelUtility.SetPropertyValue(RangeProperty.RowHeight, 23.3);

                ExcelUtility.Write(2, 1, 2, column, "Cluster: " + cluster.ClusterName, null, "Arial", 12, true, false, true);
                ExcelUtility.SetPropertyValue(RangeProperty.HorizontalAlignment, HAlignment.Center);
                ExcelUtility.SetPropertyValue(RangeProperty.RowHeight, 23.3);

                ExcelUtility.Write(3, column, DateTime.Today.ToString("dd MMMM yyyy"), null, "Arial", 12, true, false);
                ExcelUtility.SetPropertyValue(RangeProperty.HorizontalAlignment, HAlignment.Right);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                success = false;
            }
            finally
            {
                if (success)
                {
                    ExcelUtility.DisplayExcelDocument();
                }
                else
                {
                    ExcelUtility.CloseExcelDocument();
                }
            }
        }
    }
}
