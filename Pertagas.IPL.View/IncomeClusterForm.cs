using Pertagas.IPL.Common;
using Pertagas.IPL.Domain;
using Pertagas.IPL.Logic;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Pertagas.IPL.View
{
    public partial class IncomeClusterForm : Form
    {
        private UserInterfaceModes _uiMode = UserInterfaceModes.Viewing;
        private List<ClusterDomain> _clusters = null;
        private List<IncomeClusterDomain> _incomeClusters;
        private List<IncomeClusterDomain> _displayedIncomeClusters;
        private IncomeClusterDomain _selectedIncomeCluster;
        private bool _filterActive = false;
        private BindingSource _bindingSource = new BindingSource();
        private Regex _numericRegex = new Regex("^[0-9]*$");
        private List<Month> _months = MonthUtility.GetMonths();
        private bool _setSelectedIncomeCluster = true;
        private List<AddressBlockDomain> _addressBlocks = null;

        public IncomeClusterForm()
        {
            InitializeComponent();
            SetUIControlsAvailability();

            _clusters = LogicFactory.ClusterLogic.GetAllCluster();
            clusterComboBox.DataSource = _clusters;
            clusterComboBox.DisplayMember = "ClusterName";

            filterClusterComboBox.DataSource = LogicFactory.ClusterLogic.GetAllCluster();
            filterClusterComboBox.DisplayMember = "ClusterName";

            monthComboBox.DataSource = _months;
            monthComboBox.DisplayMember = "Name";

            filterFromMonthComboBox.DataSource = MonthUtility.GetMonths();
            filterFromMonthComboBox.DisplayMember = "Name";

            filterToMonthComboBox.DataSource = MonthUtility.GetMonths();
            filterToMonthComboBox.DisplayMember = "Name";

            filterFromYearTextBox.Text = DateTime.Now.Year.ToString();
            filterToYearTextBox.Text = DateTime.Now.Year.ToString();

            Month month = _months.Find(p => p.Index == DateTime.Now.Month);
            filterFromMonthComboBox.SelectedItem = month;
            filterToMonthComboBox.SelectedItem = month;

            _addressBlocks = LogicFactory.AddressBlockLogic.GetAllAddressBlocks();
            addressBlockComboBox.DataSource = _addressBlocks;
            addressBlockComboBox.DisplayMember = "Block";

            List<AddressBlockDomain> addressBlocks = LogicFactory.AddressBlockLogic.GetAllAddressBlocks();
            filterAddressBlockComboBox.DataSource = addressBlocks;
            filterAddressBlockComboBox.DisplayMember = "Block";
        }

        private void SetUIControlsAvailability()
        {
            addToolStripButton.Enabled = _uiMode == UserInterfaceModes.Viewing;
            editToolStripButton.Enabled = _uiMode == UserInterfaceModes.Viewing;
            deleteToolStripButton.Enabled = _uiMode == UserInterfaceModes.Viewing;
            saveToolStripButton.Enabled = _uiMode != UserInterfaceModes.Viewing;
            cancelToolStripButton.Enabled = _uiMode != UserInterfaceModes.Viewing;

            clusterComboBox.Enabled = _uiMode != UserInterfaceModes.Viewing;
            occupantTextBox.Enabled = _uiMode != UserInterfaceModes.Viewing;
            addressBlockComboBox.Enabled = _uiMode != UserInterfaceModes.Viewing;
            addressNumberTextBox.Enabled = _uiMode != UserInterfaceModes.Viewing;
            phoneNumberTextBox.Enabled = _uiMode != UserInterfaceModes.Viewing;
            monthComboBox.Enabled = _uiMode != UserInterfaceModes.Viewing;
            yearTextBox.Enabled = _uiMode != UserInterfaceModes.Viewing;
            amountTextBox.Enabled = _uiMode != UserInterfaceModes.Viewing;

            filterFromMonthComboBox.Enabled = _uiMode == UserInterfaceModes.Viewing;
            filterFromYearTextBox.Enabled = _uiMode == UserInterfaceModes.Viewing;
            filterToMonthComboBox.Enabled = _uiMode == UserInterfaceModes.Viewing;
            filterToYearTextBox.Enabled = _uiMode == UserInterfaceModes.Viewing;
            filterClusterComboBox.Enabled = _uiMode == UserInterfaceModes.Viewing;
            filterButton.Enabled = _uiMode == UserInterfaceModes.Viewing;
            filterAddressBlockComboBox.Enabled = _uiMode == UserInterfaceModes.Viewing;
            filterAddressNumberTextBox.Enabled = _uiMode == UserInterfaceModes.Viewing;
            filterButton.Enabled = _uiMode == UserInterfaceModes.Viewing;
            showAllButton.Enabled = _uiMode == UserInterfaceModes.Viewing;
        }

        private void RefreshGrid()
        {
            _bindingSource.DataSource = null;
            incomeClusterDataGridView.DataSource = null;
            incomeClusterDataGridView.AutoGenerateColumns = false;

            _bindingSource.DataSource = _filterActive && _displayedIncomeClusters != null ? _displayedIncomeClusters : _incomeClusters;
            incomeClusterDataGridView.DataSource = _bindingSource;

            if (_selectedIncomeCluster != null)
            {
                SelectIncomeClusterOnTheGrid(_selectedIncomeCluster.Id);
            }
        }

        private void SelectIncomeClusterOnTheGrid(int incomeClusterId)
        {
            int index = -1;
            foreach (DataGridViewRow row in incomeClusterDataGridView.Rows)
            {
                index++;
                IncomeClusterDomain incomeCluster = row.DataBoundItem as IncomeClusterDomain;
                if (incomeCluster.Id == _selectedIncomeCluster.Id)
                {
                    _selectedIncomeCluster = incomeCluster;
                    break;
                }
            }

            if (index != -1)
            {
                incomeClusterDataGridView.Rows[index].Selected = true;
            }
        }

        private void IncomeClusterForm_Load(object sender, EventArgs e)
        {
            Size = this.MdiParent.Size;

            ProgressTrackerForm progressTrackerForm = new ProgressTrackerForm();
            progressTrackerForm.ProcessInformation = "Mengambil data pendapatan cluster...";

            progressTrackerForm.Task = new ProgressTrackerForm.BackgroundTask(
                () =>
                {
                    _incomeClusters = new List<IncomeClusterDomain>();
                    List<IncomeClusterDomain> incomeClusters = LogicFactory.IncomeLogic.GetAllIncomeClusters();

                    foreach (IncomeClusterDomain incomeCluster in incomeClusters)
                    {
                        ClusterDomain cluster = _clusters.Find(p => p.Id == incomeCluster.ClusterId);
                        if (cluster != null)
                        {
                            incomeCluster.ClusterName = cluster.ClusterName;
                        }
                        incomeCluster.DisplayedAmount = incomeCluster.Amount.ToString("#,##0");

                        Month month = _months.Find(p => p.Index == incomeCluster.Month);
                        incomeCluster.DisplayedMonth = month.Name;

                        _incomeClusters.Add(incomeCluster);
                    }

                }
                );

            progressTrackerForm.ShowDialog();
            RefreshGrid();
        }

        private void addToolStripButton_Click(object sender, EventArgs e)
        {
            _uiMode = UserInterfaceModes.Adding;
            SetUIControlsAvailability();

            if (_clusters != null && _clusters.Count > 0)
            {
                clusterComboBox.SelectedIndex = 0;
            }
            occupantTextBox.Text = String.Empty;
            addressBlockComboBox.SelectedIndex = 0;
            addressNumberTextBox.Text = String.Empty;
            phoneNumberTextBox.Text = String.Empty;

            Month month = _months.Find(p => p.Index == DateTime.Now.Month);
            monthComboBox.SelectedItem = month;

            yearTextBox.Text = DateTime.Now.Year.ToString();
            amountTextBox.Text = String.Empty;
        }

        private void editToolStripButton_Click(object sender, EventArgs e)
        {
            if (_selectedIncomeCluster == null)
            {
                return;
            }

            _uiMode = UserInterfaceModes.Editing;
            SetUIControlsAvailability();
        }

        private void deleteToolStripButton_Click(object sender, EventArgs e)
        {
            if (_selectedIncomeCluster == null)
            {
                return;
            }

            LogicFactory.IncomeLogic.DeleteIncomeCluster(_selectedIncomeCluster);
            _incomeClusters.Remove(_selectedIncomeCluster);
            _filterActive = false;
            _setSelectedIncomeCluster = true;
            RefreshGrid();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(yearTextBox.Text) || !_numericRegex.IsMatch(yearTextBox.Text))
            {
                MessageBox.Show("Tahun tidak boleh kosong dah harus diisi dengan angka!", null, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!String.IsNullOrEmpty(amountTextBox.Text) && !_numericRegex.IsMatch(amountTextBox.Text))
            {
                MessageBox.Show("Nominal harus diisi dengan angka!", null, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            ClusterDomain cluster = clusterComboBox.SelectedValue as ClusterDomain;
            Month month = monthComboBox.SelectedValue as Month;
            int year = Convert.ToInt32(yearTextBox.Text);
            double amount = 0;
            if (!String.IsNullOrEmpty(amountTextBox.Text))
            {
                amount = Convert.ToDouble(amountTextBox.Text);
            }

            AddressBlockDomain addressBlock = (addressBlockComboBox.SelectedItem as AddressBlockDomain);

            if (_uiMode == UserInterfaceModes.Adding)
            {
                IncomeClusterDomain incomeCluster = LogicFactory.IncomeLogic.AddIncomeCluster(cluster, occupantTextBox.Text, 
                    (addressBlock != null ? addressBlock.Block : String.Empty),
                    addressNumberTextBox.Text, phoneNumberTextBox.Text, month.Index, year, amount);

                incomeCluster.ClusterName = cluster.ClusterName;
                incomeCluster.DisplayedAmount = amount.ToString("#,##0");
                incomeCluster.DisplayedMonth = month.Name;

                _incomeClusters.Add(incomeCluster);
                _selectedIncomeCluster = incomeCluster;
            }
            else if (_uiMode == UserInterfaceModes.Editing)
            {
                IncomeClusterDomain incomeCluster = new IncomeClusterDomain();
                incomeCluster.Id = _selectedIncomeCluster.Id;
                incomeCluster.ClusterId = cluster.Id;
                incomeCluster.ClusterName = cluster.ClusterName;
                incomeCluster.OccupantName = occupantTextBox.Text;

                incomeCluster.AddressBlock = addressBlock != null ? addressBlock.Block : String.Empty;

                incomeCluster.AddressNumber = addressNumberTextBox.Text;
                incomeCluster.PhoneNumber = phoneNumberTextBox.Text;
                incomeCluster.Month = month.Index;
                incomeCluster.Year = year;
                incomeCluster.Amount = amount;
                incomeCluster.DisplayedAmount = amount.ToString("#,##0");
                incomeCluster.DisplayedMonth = month.Name;
                LogicFactory.IncomeLogic.UpdateIncomeCluster(incomeCluster);

                _incomeClusters.Remove(_selectedIncomeCluster);
                _incomeClusters.Add(incomeCluster);

                _selectedIncomeCluster = incomeCluster;
            }

            _setSelectedIncomeCluster = false;
            _filterActive = false;
            _uiMode = UserInterfaceModes.Viewing;
            SetUIControlsAvailability();
            RefreshGrid();
        }

        private void cancelToolStripButton_Click(object sender, EventArgs e)
        {
            _uiMode = UserInterfaceModes.Viewing;
            SetUIControlsAvailability();

            incomeClusterDataGridView_SelectionChanged(incomeClusterDataGridView, e);
        }

        private void filterButton_Click(object sender, EventArgs e)
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

            _filterActive = true;

            ClusterDomain cluster = filterClusterComboBox.SelectedItem as ClusterDomain;
            AddressBlockDomain addressBlock = filterAddressBlockComboBox.SelectedItem as AddressBlockDomain;

            ProgressTrackerForm progressTrackerForm = new ProgressTrackerForm();
            progressTrackerForm.ProcessInformation = "Memfilter data pendapatan cluster...";

            progressTrackerForm.Task = new ProgressTrackerForm.BackgroundTask(
                () =>
                {
                    _displayedIncomeClusters = _incomeClusters.FindAll(p =>
                        {
                            bool match = true;

                            if (cluster != null)
                            {
                                match &= p.ClusterId == cluster.Id;
                            }

                            match &= fromMonth.Index <= p.Month;

                            if (includeFromYear)
                            {
                                match &= fromYear <= p.Year;
                            }

                            match &= toMonth.Index >= p.Month;

                            if (includeToYear)
                            {
                                match &= toYear >= p.Year;
                            }

                            if (addressBlock != null)
                            {
                                match &= (!String.IsNullOrEmpty(p.AddressBlock) && p.AddressBlock.ToLower().Trim() == addressBlock.Block.ToLower().Trim());
                            }

                            if (!String.IsNullOrEmpty(filterAddressNumberTextBox.Text))
                            {
                                match &= (!String.IsNullOrEmpty(p.AddressNumber) && p.AddressNumber.ToLower().Trim() == filterAddressNumberTextBox.Text.ToLower().Trim());
                            }

                            return match;
                        });
                });

            progressTrackerForm.ShowDialog();

            _setSelectedIncomeCluster = true;
            RefreshGrid();
        }

        private void showAllButton_Click(object sender, EventArgs e)
        {
            _setSelectedIncomeCluster = true;
            _filterActive = false;
            RefreshGrid();
        }

        private void incomeClusterDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if ((_setSelectedIncomeCluster && (incomeClusterDataGridView.SelectedRows == null || incomeClusterDataGridView.SelectedRows.Count == 0)) ||
                (!_setSelectedIncomeCluster && _selectedIncomeCluster == null))
            {
                return;
            }

            if (_setSelectedIncomeCluster)
            {
                _selectedIncomeCluster = incomeClusterDataGridView.SelectedRows[0].DataBoundItem as IncomeClusterDomain;
            }

            ClusterDomain cluster = _clusters.Find(p => p.Id == _selectedIncomeCluster.ClusterId);
            if (cluster != null)
            {
                clusterComboBox.SelectedItem = cluster;
            }
            Month month = _months.Find(p => p.Index == _selectedIncomeCluster.Month);
            monthComboBox.SelectedItem = month;

            yearTextBox.Text = _selectedIncomeCluster.Year.ToString();

            AddressBlockDomain addressBlock = _addressBlocks.Find(p => p.Block == _selectedIncomeCluster.AddressBlock);
            if (addressBlock != null)
            {
                addressBlockComboBox.SelectedItem = _selectedIncomeCluster.AddressBlock;
            }
            addressNumberTextBox.Text = _selectedIncomeCluster.AddressNumber;
            phoneNumberTextBox.Text = _selectedIncomeCluster.PhoneNumber;
            occupantTextBox.Text = _selectedIncomeCluster.OccupantName;
            amountTextBox.Text = _selectedIncomeCluster.Amount.ToString();            
        }
    }
}
