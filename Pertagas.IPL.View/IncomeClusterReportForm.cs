using Pertagas.IPL.Common;
using Pertagas.IPL.Domain;
using Pertagas.IPL.Logic;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Pertagas.IPL.View
{
    public partial class IncomeClusterReportForm : Form
    {
        private List<ClusterDomain> _clusters = null;
        private List<Month> _months = MonthUtility.GetMonths();

        public IncomeClusterReportForm()
        {
            InitializeComponent();

            _clusters = LogicFactory.ClusterLogic.GetAllCluster();
            clusterComboBox.DataSource = _clusters;
            clusterComboBox.DisplayMember = "ClusterName";

            filterFromMonthComboBox.DataSource = _months;
            filterFromMonthComboBox.DisplayMember = "Name";

            filterToMonthComboBox.DataSource = MonthUtility.GetMonths();
            filterToMonthComboBox.DisplayMember = "Name";

            filterFromYearTextBox.Text = DateTime.Now.Year.ToString();
            filterToYearTextBox.Text = DateTime.Now.Year.ToString();
        }

        private void generateReportButton_Click(object sender, EventArgs e)
        {
            Month fromMonth = filterFromMonthComboBox.SelectedItem as Month;
            Month toMonth = filterToMonthComboBox.SelectedItem as Month;

            int fromYear;
            bool includeFromYear = int.TryParse(filterFromYearTextBox.Text, out fromYear);

            int toYear;
            bool includeToYear = int.TryParse(filterToYearTextBox.Text, out toYear);

            if (fromYear > toYear)
            {
                MessageBox.Show("Tahun sampai tidak boleh lebih lama dari tahun dari!");
                return;
            }
            else if (fromYear == toYear)
            {
                if (fromMonth.Index > toMonth.Index)
                {
                    MessageBox.Show("Bulan sampai tidak boleh lebih lama dari bulan dari!");
                    return;
                }
            }

            ClusterDomain cluster = clusterComboBox.SelectedItem as ClusterDomain;

            string message = null;
            ProgressTrackerForm form = new ProgressTrackerForm();
            form.ProcessInformation = "Mencetak laporan...";
            form.Task = new ProgressTrackerForm.BackgroundTask(
                () =>
                {
                    LogicFactory.IncomeLogic.GenerateIncomeClusterReport(cluster, fromMonth, fromYear, toMonth, toYear, out message);
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
