using Pertagas.IPL.Common;
using Pertagas.IPL.DataAccess.DAO;
using Pertagas.IPL.Domain;
using System;
using System.Collections.Generic;

namespace Pertagas.IPL.Logic
{
    public class AccountingLogic
    {
        public void GenerateAccountingReport(Month fromMonth, Month toMonth, int year, out string message)
        {
            message = null;
            bool success = true;
            try
            {
                ExcelUtility.CreateExcelDocument();
                ExcelUtility.SetGridVisibility(false);

                ExcelUtility.Write(5, 2, 7, 2, "NO", null, "Tahoma", 12, true, false, true);
                ExcelUtility.SetPropertyValue(RangeProperty.HorizontalAlignment, HAlignment.Center);
                ExcelUtility.SetPropertyValue(RangeProperty.VerticalAlignment, VAlignment.Center);
                ExcelUtility.SetPropertyValue(RangeProperty.CellWidth, 8.33);
                ExcelUtility.SetBorder(true, true, true, true, BorderWeight.Thick, BorderWeight.Thick, BorderWeight.Thick, BorderWeight.Thick);

                ExcelUtility.Write(5, 3, 7, 3, "KETERANGAN", null, "Tahoma", 12, true, false, true);
                ExcelUtility.SetPropertyValue(RangeProperty.HorizontalAlignment, HAlignment.Center);
                ExcelUtility.SetPropertyValue(RangeProperty.VerticalAlignment, VAlignment.Center);
                ExcelUtility.SetPropertyValue(RangeProperty.CellWidth, 98.33);
                ExcelUtility.SetBorder(true, true, true, true, BorderWeight.Thick, BorderWeight.Thick, BorderWeight.Thick, BorderWeight.Thick);

                List<Month> months = MonthUtility.GetMonths();
                int column = 4;
                Dictionary<int, int> columnReferences = new Dictionary<int, int>();
                for (int j = fromMonth.Index; j <= toMonth.Index; j++)
                {
                    columnReferences[j] = column;

                    Month month = months.Find(p => p.Index == j);
                    ExcelUtility.Write(5, column, 7, column, month.Name.ToUpper(), null, "Tahoma", 12, true, false, true);
                    ExcelUtility.SetPropertyValue(RangeProperty.HorizontalAlignment, HAlignment.Center);
                    ExcelUtility.SetPropertyValue(RangeProperty.VerticalAlignment, VAlignment.Center);
                    ExcelUtility.SetPropertyValue(RangeProperty.CellWidth, 16.67);
                    ExcelUtility.SetBorder(true, true, true, true, BorderWeight.Thick, BorderWeight.Thick, BorderWeight.Thick, BorderWeight.Thick);

                    column++;
                }

                ExcelUtility.Write(5, column, 7, column, "JUMLAH", null, "Tahoma", 12, true, false, true);
                ExcelUtility.SetPropertyValue(RangeProperty.HorizontalAlignment, HAlignment.Center);
                ExcelUtility.SetPropertyValue(RangeProperty.VerticalAlignment, VAlignment.Center);
                ExcelUtility.SetPropertyValue(RangeProperty.CellWidth, 16.67);
                ExcelUtility.SetBorder(true, true, true, true, BorderWeight.Thick, BorderWeight.Thick, BorderWeight.Thick, BorderWeight.Thick);

                ExcelUtility.Write(9, 3, "PENDAPATAN CLUSTER", null, "Tahoma", 12, true, true);

                List<ClusterDomain> clusters = DaoFactory.ClusterDao.GetAllClusters();
                List<IncomeClusterDomain> incomeClusters = DaoFactory.IncomeClusterDao.GetIncomeClusters(fromMonth.Index, toMonth.Index, year);
                List<int> clusterIds = new List<int>();
                foreach(IncomeClusterDomain incomeCluster in incomeClusters)
                {
                    if (!clusterIds.Contains(incomeCluster.ClusterId))
                    {
                        clusterIds.Add(incomeCluster.ClusterId);
                    }
                }

                int rowIndex = 10;
                foreach(int clusterId in clusterIds)
                {
                    ClusterDomain cluster = clusters.Find(p => p.Id == clusterId);
                    if (cluster == null)
                    {
                        continue;
                    }

                    List<IncomeClusterDomain> result = incomeClusters.FindAll(p => p.ClusterId == clusterId);
                    if (result == null || result.Count == 0)
                    {
                        continue;
                    }

                    ExcelUtility.Write(rowIndex, 2, (rowIndex - 9).ToString(), "@", "Tahoma", 12, true, false);
                    ExcelUtility.SetPropertyValue(RangeProperty.HorizontalAlignment, HAlignment.Center);

                    ExcelUtility.Write(rowIndex, 3, "IPL WARGA " + cluster.ClusterName, null, "Tahoma", 12, true, false);
                
                    for (int j = fromMonth.Index; j <= toMonth.Index; j++)
                    {
                        List<IncomeClusterDomain> currentMonthIncomeClusters = result.FindAll(p => p.Month == j);
                        if (currentMonthIncomeClusters == null || currentMonthIncomeClusters.Count == 0)
                        {
                            continue;
                        }

                        double amount = 0;
                        foreach (IncomeClusterDomain income in currentMonthIncomeClusters)
                        {
                            amount += income.Amount;
                        }

                        ExcelUtility.Write(rowIndex, columnReferences[j], amount, "#,##0", "Tahoma", 12, true, false);                 
                    }

                    rowIndex++;
                }

                List<IncomeDomain> incomes = DaoFactory.IncomeDao.GetIncomes(fromMonth.Index, toMonth.Index, year);
                List<int> incomeSourceIds = new List<int>();
                foreach(IncomeDomain income in incomes)
                {
                    if (!incomeSourceIds.Contains(income.IncomeSourceId))
                    {
                        incomeSourceIds.Add(income.IncomeSourceId);
                    }
                }

                List<IncomeSourceDomain> incomeSources = DaoFactory.IncomeSourceDao.GetAllIncomeSources();
                foreach(int incomeSourceId in incomeSourceIds)
                {
                    IncomeSourceDomain incomeSource = incomeSources.Find(p => p.Id == incomeSourceId);
                    if (incomeSource == null)
                    {
                        continue;
                    }

                    ExcelUtility.Write(rowIndex, 2, (rowIndex - 9).ToString(), "@", "Tahoma", 12, true, false);
                    ExcelUtility.SetPropertyValue(RangeProperty.HorizontalAlignment, HAlignment.Center);

                    ExcelUtility.Write(rowIndex, 3, incomeSource.Description, null, "Tahoma", 12, true, false);

                    for (int j = fromMonth.Index; j <= toMonth.Index; j++)
                    {
                        List<IncomeDomain> currentMonthIncomes = incomes.FindAll(p => p.Month == j && p.IncomeSourceId == incomeSourceId);
                        if (currentMonthIncomes == null || currentMonthIncomes.Count == 0)
                        {
                            continue;
                        }

                        double amount = 0;
                        foreach (IncomeDomain income in currentMonthIncomes)
                        {
                            amount += income.Amount;
                        }

                        ExcelUtility.Write(rowIndex, columnReferences[j], amount, "#,##0", "Tahoma", 12, true, false);
                    }

                    rowIndex++;
                }

                for (int i = 10; i < rowIndex; i++)
                {
                    ExcelUtility.Write(i, column, "=SUM(" + ExcelUtility.GetExcelCellName(4, i) + ":" + ExcelUtility.GetExcelCellName(column - 1, i), "#,##0", "Tahoma", 12, true, false);
                }
                
                rowIndex++;
                
                ExcelUtility.Write(rowIndex, 3, "SUB TOTAL PENDAPATAN", null, "Tahoma", 12, true, false);
                ExcelUtility.SetPropertyValue(RangeProperty.HorizontalAlignment, HAlignment.Center);

                int columnIndex = 4;
                for (int j = fromMonth.Index; j <= toMonth.Index; j++)
                {
                    ExcelUtility.Write(rowIndex, columnIndex, "=SUM(" + ExcelUtility.GetExcelCellName(columnIndex, 10) + ":" + ExcelUtility.GetExcelCellName(columnIndex, rowIndex - 2), "#,##0", "Tahoma", 12, true, false);
                    columnIndex++;
                }

                ExcelUtility.Write(rowIndex, columnIndex, "=SUM(" + ExcelUtility.GetExcelCellName(columnIndex, 10) + ":" + ExcelUtility.GetExcelCellName(columnIndex, rowIndex - 2), "#,##0", "Tahoma", 12, true, false);

                ExcelUtility.Write(rowIndex + 2, 3, "PENGELUARAN CLUSTER", null, "Tahoma", 12, true, true);

                rowIndex += 4;
                int startExpenseRowIndex = rowIndex;
                List<ExpenseDomain> expenses = DaoFactory.ExpenseDao.GetExpenses(fromMonth.Index, toMonth.Index, year);

                List<ExpenseDomain> condensedExpenses = new List<ExpenseDomain>();
                foreach(ExpenseDomain expense in expenses)
                {
                    ExpenseDomain result = condensedExpenses.Find(p => p.Description.ToLower().Trim() == expense.Description.ToLower().Trim() && p.Month == expense.Month);
                    if (result == null)
                    {
                        condensedExpenses.Add(expense);
                    }
                    else
                    {
                        result.Amount += expense.Amount;
                    }
                }

                Dictionary<string, int> rowReferences = new Dictionary<string, int>();
                foreach (ExpenseDomain expense in condensedExpenses)
                {
                     if (rowReferences.ContainsKey(expense.Description.ToLower().Trim()))
                     {
                         ExcelUtility.Write(rowReferences[expense.Description.ToLower().Trim()], columnReferences[expense.Month], expense.Amount, "#,##0", "Tahoma", 12, true, false);
                     }
                     else
                     {
                         ExcelUtility.Write(rowIndex, 2, (rowIndex - startExpenseRowIndex + 1).ToString(), null, "Tahoma", 12, true, false);
                         ExcelUtility.SetPropertyValue(RangeProperty.HorizontalAlignment, HAlignment.Center);

                         ExcelUtility.Write(rowIndex, 3, expense.Description, null, "Tahoma", 12, true, false);

                         ExcelUtility.Write(rowIndex, columnReferences[expense.Month], expense.Amount, "#,##0", "Tahoma", 12, true, false);

                         rowReferences[expense.Description.ToLower().Trim()] = rowIndex;
                         rowIndex++;
                     }
                }

                for (int i = startExpenseRowIndex; i < rowIndex; i++)
                {
                    ExcelUtility.Write(i, column, "=SUM(" + ExcelUtility.GetExcelCellName(4, i) + ":" + ExcelUtility.GetExcelCellName(column - 1, i), "#,##0", "Tahoma", 12, true, false);
                }

                rowIndex++;

                ExcelUtility.Write(rowIndex, 3, "SUB TOTAL PENGELUARAN", null, "Tahoma", 12, true, false);
                ExcelUtility.SetPropertyValue(RangeProperty.HorizontalAlignment, HAlignment.Center);

                columnIndex = 4;
                for (int j = fromMonth.Index; j <= toMonth.Index; j++)
                {
                    ExcelUtility.Write(rowIndex, columnIndex, "=SUM(" + ExcelUtility.GetExcelCellName(columnIndex, startExpenseRowIndex) + ":" + ExcelUtility.GetExcelCellName(columnIndex, rowIndex - 2), "#,##0", "Tahoma", 12, true, false);
                    columnIndex++;
                }

                ExcelUtility.Write(rowIndex, columnIndex, "=SUM(" + ExcelUtility.GetExcelCellName(columnIndex, startExpenseRowIndex) + ":" + ExcelUtility.GetExcelCellName(columnIndex, rowIndex - 2), "#,##0", "Tahoma", 12, true, false);

                rowIndex++;

                ExcelUtility.Write(rowIndex, 3, "SALDO BULANAN", null, "Tahoma", 12, true, false);
                ExcelUtility.SetPropertyValue(RangeProperty.HorizontalAlignment, HAlignment.Center);

                for (int i = 4; i <= columnIndex; i++)
                {
                    ExcelUtility.Write(rowIndex, i, "=" + ExcelUtility.GetExcelCellName(i, startExpenseRowIndex - 4) + "-" + ExcelUtility.GetExcelCellName(i, rowIndex - 1), "#,##0", "Tahoma", 12, true, false);
                }

                ExcelUtility.Write(0, 2, 0, column, "LAPORAN KEUANGAN", null, "Tahoma", 14, true, false, true);
                ExcelUtility.SetPropertyValue(RangeProperty.HorizontalAlignment, HAlignment.Center);

                ExcelUtility.Write(1, 2, 1, column, "WARGA CLUSTER ARGA PADMA NIRWANA", null, "Tahoma", 16, true, true, true);
                ExcelUtility.SetPropertyValue(RangeProperty.HorizontalAlignment, HAlignment.Center);

                string monthText = fromMonth.Name;
                if (toMonth.Index != fromMonth.Index)
                {
                    monthText += " - " + toMonth.Name;
                }
                ExcelUtility.Write(2, 2, 2, column, "PERIODE BULAN " + monthText + " " + year.ToString(), null, "Tahoma", 14, false, false, true);
                ExcelUtility.SetPropertyValue(RangeProperty.HorizontalAlignment, HAlignment.Center);

                for (int i = 8; i <= rowIndex; i++)
                {
                    for (int j = 2; j <= column; j++)
                    {
                        ExcelUtility.SetCurrentCell(i, j);

                        BorderWeight topWeight = BorderWeight.Thin;
                        BorderWeight bottomWeight = BorderWeight.Thin;
                        if (i == startExpenseRowIndex - 4 || i == rowIndex - 1 || i == rowIndex)
                        {
                            topWeight = BorderWeight.Thick;
                            bottomWeight = BorderWeight.Thick;
                        }
                        else if (i == startExpenseRowIndex - 3)
                        {
                            topWeight = BorderWeight.Thick;
                        }

                        ExcelUtility.SetBorder(true, i > 8, true, true, BorderWeight.Thick, topWeight, BorderWeight.Thick, bottomWeight);
                    }
                }

                ExcelUtility.SetCurrentCell(startExpenseRowIndex - 4, 2, startExpenseRowIndex - 4, column);
                ExcelUtility.SetPropertyValue(RangeProperty.CellColor, System.Drawing.Color.FromArgb(141, 180, 226));

                ExcelUtility.SetCurrentCell(rowIndex - 1, 2, rowIndex - 1, column);
                ExcelUtility.SetPropertyValue(RangeProperty.CellColor, System.Drawing.Color.Yellow);

                ExcelUtility.SetCurrentCell(rowIndex, 2, rowIndex, column);
                ExcelUtility.SetPropertyValue(RangeProperty.CellColor, System.Drawing.Color.FromArgb(141, 180, 226));

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

        public void GenerateBalanceReport(Month fromMonth, Month toMonth, int year, out string message)
        {
            message = null;
            string fontName = "Calibri";
            bool success = true;
            try
            {
                ExcelUtility.CreateExcelDocument();
                ExcelUtility.SetGridVisibility(false);

                ExcelUtility.Write(0, 0, 0, 4, "LAPORAN KEUANGAN", null, fontName, 20, true, false, true);
                ExcelUtility.SetPropertyValue(RangeProperty.HorizontalAlignment, HAlignment.Center);
                ExcelUtility.SetPropertyValue(RangeProperty.RowHeight, 25.8);

                ExcelUtility.Write(1, 0, 1, 4, "WARGA CLUSTER ARGA PADMA NIRWANA", null, fontName, 22, true, true, true);
                ExcelUtility.SetPropertyValue(RangeProperty.HorizontalAlignment, HAlignment.Center);
                ExcelUtility.SetPropertyValue(RangeProperty.RowHeight, 28.8);

                string monthText = fromMonth.Name;
                if (toMonth.Index != fromMonth.Index)
                {
                    monthText += " - " + toMonth.Name;
                }
                ExcelUtility.Write(2, 0, 2, 4, "PERIODE BULAN " + monthText + " " + year.ToString(), null, fontName, 14, true, false, true);
                ExcelUtility.SetPropertyValue(RangeProperty.HorizontalAlignment, HAlignment.Center);
                ExcelUtility.SetPropertyValue(RangeProperty.RowHeight, 18);

                ExcelUtility.Write(3, 4, DateTime.Today, "dd MMM yyyy", fontName, 14, false, false);
                ExcelUtility.SetPropertyValue(RangeProperty.HorizontalAlignment, HAlignment.Right);

                string[] columnNames = new string[] { "NO", "KETERANGAN", "PEMASUKAN", "PENGELUARAN", "SALDO" };
                double[] columnWidths = new double[] { 5.67, 77.11, 20.56, 20.56, 20.56 };
                for (int i = 0; i < columnNames.Length; i++)
                {
                    ExcelUtility.Write(4, i, columnNames[i], null, fontName, 14, true, false);
                    ExcelUtility.SetPropertyValue(RangeProperty.CellWidth, columnWidths[i]);
                    ExcelUtility.SetPropertyValue(RangeProperty.RowHeight, 51);
                    ExcelUtility.SetPropertyValue(RangeProperty.HorizontalAlignment, HAlignment.Center);
                    ExcelUtility.SetPropertyValue(RangeProperty.VerticalAlignment, VAlignment.Center);
                    ExcelUtility.SetPropertyValue(RangeProperty.CellColor, System.Drawing.Color.FromArgb(183, 222, 223));
                }

                ExcelUtility.Write(6, 1, "Saldo akhir tahun " + (year - 1).ToString(), null, fontName, 14, false, false);
                ExcelUtility.Write(6, 4, 0, "#,##0", fontName, 14, true, false);

                List<IncomeClusterDomain> lastYearIncomeCluster = DaoFactory.IncomeClusterDao.GetIncomeClusters(1, 12, (year - 1));

                List<IncomeClusterDomain> incomeClusters = DaoFactory.IncomeClusterDao.GetIncomeClusters(fromMonth.Index, toMonth.Index, year);
                List<ClusterDomain> clusters = DaoFactory.ClusterDao.GetAllClusters();

                List<Month> months = MonthUtility.GetMonths();

                List<int> incomeRows = new List<int>();

                int rowIndex = 7;
                int lastRowIndex = 6;
                int startRowIndex = 7;
                bool collectStartRow = true;                
                for (int i = 0; i < clusters.Count; i++)
                {
                    List<IncomeClusterDomain> lastYearCurrentClusterIncomes = lastYearIncomeCluster.FindAll(p => p.ClusterId == clusters[i].Id);
                    List<IncomeClusterDomain> currentClusterIncomes = incomeClusters.FindAll(p => p.ClusterId == clusters[i].Id);

                    if ((lastYearCurrentClusterIncomes == null || lastYearCurrentClusterIncomes.Count == 0) &&
                        (currentClusterIncomes == null || currentClusterIncomes.Count == 0))
                    {
                        continue;
                    }

                    ExcelUtility.Write(rowIndex++, 1, clusters[i].ClusterName, null, fontName, 14, true, false);

                    if (collectStartRow)
                    {
                        startRowIndex = rowIndex;
                        collectStartRow = false;
                    }
                    
                    double amount = 0;
                    foreach (IncomeClusterDomain incomeCluster in lastYearCurrentClusterIncomes)
                    {
                        amount += incomeCluster.Amount;
                    }

                    ExcelUtility.Write(rowIndex, 0, "1", "@", fontName, 14, false, false);
                    ExcelUtility.Write(rowIndex, 1, "Setoran IPL Cluster " + clusters[i].ClusterName + " Tahun " + (year - 1).ToString(), "@", fontName, 14, false, false);
                    ExcelUtility.Write(rowIndex, 2, amount, "#,##0", fontName, 14, false, false);

                    ExcelUtility.Write(rowIndex, 4, "=" + ExcelUtility.GetExcelCellName(4, lastRowIndex) + "+" + ExcelUtility.GetExcelCellName(2, rowIndex), "#,##0", fontName, 14, false, false);
                    lastRowIndex = rowIndex;
                    rowIndex++;

                    int count = 2;
                    bool printSubTotal = false;
                    for (int month = 1; month <= 12; month++)
                    {
                        List<IncomeClusterDomain> result = currentClusterIncomes.FindAll(p => p.Month == month);
                        if (result == null || result.Count == 0)
                        {
                            continue;
                        }

                        if (!printSubTotal) printSubTotal= true;

                        double totalIncome = 0;
                        foreach(IncomeClusterDomain row in result)
                        {
                            totalIncome += row.Amount;
                        }

                        Month m = months.Find(p => p.Index == month);
                        ExcelUtility.Write(rowIndex, 0, count.ToString(), "@", fontName, 14, false, false);
                        ExcelUtility.Write(rowIndex, 1, "Setoran IPL Cluster " + clusters[i].ClusterName + " Bulan " + m.Name + " Tahun " + year.ToString(), "@", fontName, 14, false, false);
                        ExcelUtility.Write(rowIndex, 2, totalIncome, "#,##0", fontName, 14, false, false);

                        ExcelUtility.Write(rowIndex, 4, "=" + ExcelUtility.GetExcelCellName(4, lastRowIndex) + "+" + ExcelUtility.GetExcelCellName(2, rowIndex), "#,##0", fontName, 14, false, false);
                        lastRowIndex = rowIndex;
                        rowIndex++;
                        count++;
                    }

                    if (printSubTotal)
                    {
                        ExcelUtility.Write(rowIndex, 1, "Sub Total Cluster " + clusters[i].ClusterName, "@", fontName, 14, true, false);
                        ExcelUtility.SetPropertyValue(RangeProperty.HorizontalAlignment, HAlignment.Center);

                        ExcelUtility.Write(rowIndex, 2, "=SUM(" + ExcelUtility.GetExcelCellName(2, startRowIndex) + ":" + ExcelUtility.GetExcelCellName(2, rowIndex - 1) + ")", "#,##0", fontName, 14, true, false);

                        incomeRows.Add(rowIndex);
                        rowIndex++;
                    }
                }

                List<IncomeDomain> incomes = DaoFactory.IncomeDao.GetIncomes(fromMonth.Index, toMonth.Index, year);

                if (incomes != null && incomes.Count > 0)
                {
                    List<IncomeSourceDomain> incomeSources = DaoFactory.IncomeSourceDao.GetAllIncomeSources();
                    Dictionary<int, double> totalIncomes = new Dictionary<int, double>();
                    foreach (IncomeDomain income in incomes)
                    {
                        if (totalIncomes.ContainsKey(income.IncomeSourceId))
                        {
                            totalIncomes[income.IncomeSourceId] += income.Amount;
                        }
                        else
                        {
                            totalIncomes[income.IncomeSourceId] = income.Amount;
                        }
                    }

                    ExcelUtility.Write(rowIndex++, 1, "PEMASUKAN", "@", fontName, 14, true, false);

                    int count = 1;
                    startRowIndex = rowIndex;
                    foreach(KeyValuePair<int, double> pair in totalIncomes)
                    {
                        IncomeSourceDomain source = incomeSources.Find(p => p.Id == pair.Key);

                        ExcelUtility.Write(rowIndex, 0, count.ToString(), "@", fontName, 14, false, false);
                        ExcelUtility.Write(rowIndex, 1, source.Description + " selama tahun " + year.ToString(), "@", fontName, 14, false, false);
                        ExcelUtility.Write(rowIndex, 2, pair.Value, "#,##0", fontName, 14, false, false);

                        ExcelUtility.Write(rowIndex, 4, "=" + ExcelUtility.GetExcelCellName(4, lastRowIndex) + "+" + ExcelUtility.GetExcelCellName(2, rowIndex), "#,##0", fontName, 14, false, false);
                        lastRowIndex = rowIndex;
                        rowIndex++;
                        count++;
                    }

                    ExcelUtility.Write(rowIndex, 1, "Sub Total", null, fontName, 14, true, false);
                    ExcelUtility.SetPropertyValue(RangeProperty.HorizontalAlignment, HAlignment.Center);

                    ExcelUtility.Write(rowIndex, 2, "=SUM(" + ExcelUtility.GetExcelCellName(2, startRowIndex) + ":" + ExcelUtility.GetExcelCellName(2, rowIndex - 1) + ")", "#,##0", fontName, 14, true, false);

                    incomeRows.Add(rowIndex);

                    rowIndex++;
                }

                List<ExpenseDomain> expenses = DaoFactory.ExpenseDao.GetExpenses(fromMonth.Index, toMonth.Index, year);

                int startExpenseRow = 0;
                if (expenses != null && expenses.Count > 0)
                {                    
                    ExcelUtility.Write(rowIndex++, 1, "PENGELUARAN", "@", fontName, 14, true, false);
                 
                    startExpenseRow = rowIndex;
                    int countExpense = 1;
                    for (int month = 1; month <= 12; month++)
                    {
                        List<ExpenseDomain> result = expenses.FindAll(p => p.Month == month);
                        if (result == null || result.Count == 0)
                        {
                            continue;
                        }

                        double totalExpense = 0;
                        foreach (ExpenseDomain row in result)
                        {
                            totalExpense += row.Amount;
                        }

                        Month m = months.Find(p => p.Index == month);
                        ExcelUtility.Write(rowIndex, 0, countExpense.ToString(), "@", fontName, 14, false, false);
                        ExcelUtility.Write(rowIndex, 1, "Pengeluaran selama bulan " + m.Name + " " + year.ToString(), "@", fontName, 14, false, false);
                        ExcelUtility.Write(rowIndex, 3, totalExpense, "#,##0", fontName, 14, false, false);

                        ExcelUtility.Write(rowIndex, 4, "=" + ExcelUtility.GetExcelCellName(4, lastRowIndex) + "-" + ExcelUtility.GetExcelCellName(3, rowIndex), "#,##0", fontName, 14, false, false);
                        lastRowIndex = rowIndex;
                        rowIndex++;
                        countExpense++;
                    }
                }

                rowIndex += 2;

                ExcelUtility.Write(rowIndex, 1, "Saldo", "@", fontName, 14, true, false);
                string[] cellNames = new string[incomeRows.Count];
                for (int x = 0; x < incomeRows.Count; x++)
                {
                    cellNames[x] = ExcelUtility.GetExcelCellName(2, incomeRows[x]);
                }

                ExcelUtility.Write(rowIndex, 2, "=" + String.Join("+", cellNames), "#,##0", fontName, 14, false, false);
                ExcelUtility.Write(rowIndex, 3, "=SUM(" + ExcelUtility.GetExcelCellName(3, startExpenseRow) + ":" + ExcelUtility.GetExcelCellName(3, rowIndex - 3) + ")", "#,##0", fontName, 14, false, false);

                ExcelUtility.Write(rowIndex, 4, "=" + ExcelUtility.GetExcelCellName(2, rowIndex) + "-" + ExcelUtility.GetExcelCellName(3, rowIndex) + "+" + ExcelUtility.GetExcelCellName(3, 6), "#,##0", fontName, 14, false, false);

                rowIndex++;

                ExcelUtility.Write(rowIndex, 1, "Saldo di buku tabungan tahun " + (year - 1).ToString(), "@", fontName, 14, true, false);
                ExcelUtility.Write(rowIndex, 4, "0", "#,##0", fontName, 14, false, false);
                rowIndex++;

                ExcelUtility.Write(rowIndex, 1, "Cash on Hand", "@", fontName, 14, true, false);
                ExcelUtility.Write(rowIndex, 4, "=" + ExcelUtility.GetExcelCellName(4, rowIndex - 2) + "-" + ExcelUtility.GetExcelCellName(4, rowIndex - 1), "#,##0", fontName, 14, false, false);
                rowIndex += 2;

                ExcelUtility.Write(rowIndex, 1, "Dibuat Oleh,", "@", fontName, 14, false, false);
                ExcelUtility.Write(rowIndex + 6, 1, "Bendahara", "@", fontName, 14, false, false);

                ExcelUtility.Write(rowIndex, 3, "Mengetahui,", "@", fontName, 14, false, false);
                ExcelUtility.Write(rowIndex + 6, 3, "Ketua Cluster", "@", fontName, 14, false, false);

                for (int i = 4; i <= rowIndex - 2; i++)
                {
                    for (int j = 0; j <= 4; j++)
                    {
                        BorderWeight topBorderWeight = BorderWeight.Thin;
                        BorderWeight leftBorderWeight = BorderWeight.Thin;
                        BorderWeight rightBorderWeight = BorderWeight.Thin;
                        BorderWeight bottomBorderWeight = BorderWeight.Thin;
                        if (i == 4 || i == rowIndex - 4 || i == rowIndex - 3 || i == rowIndex - 2)
                        {
                            topBorderWeight = BorderWeight.Thick;
                        }
                        if (i == rowIndex - 2)
                        {
                            bottomBorderWeight = BorderWeight.Thick;
                        }

                        if (j == 0)
                        {
                            leftBorderWeight = BorderWeight.Thick;
                        }

                        if (j == 4)
                        {
                            rightBorderWeight = BorderWeight.Thick;
                        }

                        ExcelUtility.SetCurrentCell(i, j);
                        ExcelUtility.SetBorder(true, true, true, true, leftBorderWeight, topBorderWeight, rightBorderWeight, bottomBorderWeight);
                    }
                }

                ExcelUtility.SetCurrentCell(rowIndex - 1, 0, rowIndex + 7, 0);
                ExcelUtility.SetBorder(true, false, false, false, BorderWeight.Thick, BorderWeight.Thick, BorderWeight.Thick, BorderWeight.Thick);

                ExcelUtility.SetCurrentCell(rowIndex - 1, 4, rowIndex + 7, 4);
                ExcelUtility.SetBorder(false, false, true, false, BorderWeight.Thick, BorderWeight.Thick, BorderWeight.Thick, BorderWeight.Thick);

                ExcelUtility.SetCurrentCell(rowIndex + 7, 0, rowIndex + 7, 4);
                ExcelUtility.SetBorder(false, false, false, true, BorderWeight.Thick, BorderWeight.Thick, BorderWeight.Thick, BorderWeight.Thick);
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
