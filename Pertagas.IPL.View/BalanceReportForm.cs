using Pertagas.IPL.Common;
using Pertagas.IPL.Logic;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Pertagas.IPL.View
{
    public partial class BalanceReportForm : Form
    {
        private List<Month> _months = MonthUtility.GetMonths();

        public BalanceReportForm()
        {
            InitializeComponent();

            filterFromMonthComboBox.DataSource = _months;
            filterFromMonthComboBox.DisplayMember = "Name";

            filterToMonthComboBox.DataSource = MonthUtility.GetMonths();
            filterToMonthComboBox.DisplayMember = "Name";

            yearTextBox.Text = DateTime.Now.Year.ToString();

            Month month = _months.Find(p => p.Index == DateTime.Now.Month);
        }

        private void generateReportButton_Click(object sender, EventArgs e)
        {
            Month fromMonth = filterFromMonthComboBox.SelectedItem as Month;
            Month toMonth = filterToMonthComboBox.SelectedItem as Month;

            int year;
            bool includeFromYear = int.TryParse(yearTextBox.Text, out year);

            if (fromMonth.Index > toMonth.Index)
            {
                MessageBox.Show("Bulan sampai tidak boleh lebih lama dari bulan dari!");
                return;
            }

            string message = null;
            ProgressTrackerForm form = new ProgressTrackerForm();
            form.ProcessInformation = "Mencetak laporan...";
            form.Task = new ProgressTrackerForm.BackgroundTask(
                () =>
                {
                    LogicFactory.AccountingLogic.GenerateBalanceReport(fromMonth, toMonth, year, out message);
                });
            form.ShowDialog();

            if (!String.IsNullOrEmpty(message))
            {
                MessageBox.Show("Gagal mencetak laporan!");
                Clipboard.SetText(message);
            }                        
        }
    }
}
