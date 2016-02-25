using Pertagas.IPL.Common;
using Pertagas.IPL.Domain;
using Pertagas.IPL.Logic;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Pertagas.IPL.View
{
    public partial class IncomeForm : Form
    {
        private UserInterfaceModes _uiMode = UserInterfaceModes.Viewing;
        private bool _filterActive = false;
        private List<IncomeDomain> _incomes;
        private List<IncomeDomain> _displayedIncomes;
        private IncomeDomain _selectedIncome;
        private BindingSource _bindingSource = new BindingSource();
        private Regex _numericRegex = new Regex("^[0-9]*$");
        private List<Month> _months = MonthUtility.GetMonths();
        private List<IncomeSourceDomain> _incomeSources;
        private bool _setSelectedIncome = true;

        public IncomeForm()
        {
            InitializeComponent();
            
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

            _incomeSources = LogicFactory.IncomeSourceLogic.GetAllIncomeSources();
            incomeSourceComboBox.DataSource = _incomeSources;
            incomeSourceComboBox.DisplayMember = "Description";

            SetUIControlsAvailability();
        }

        private void addToolStripButton_Click(object sender, EventArgs e)
        {
            _uiMode = UserInterfaceModes.Adding;
            SetUIControlsAvailability();

            Month month = _months.Find(p => p.Index == DateTime.Today.Month);
            monthComboBox.SelectedItem = month;

            yearTextBox.Text = DateTime.Today.Year.ToString();
            amountTextBox.Text = String.Empty;

            if (_incomeSources != null && _incomeSources.Count > 0)
            {
                incomeSourceComboBox.SelectedIndex = 0;
            }
        }

        private void editToolStripButton_Click(object sender, EventArgs e)
        {
            if (_selectedIncome == null)
            {
                return;
            }

            _uiMode = UserInterfaceModes.Editing;
            SetUIControlsAvailability();
        }

        private void deleteToolStripButton_Click(object sender, EventArgs e)
        {
            if (_selectedIncome == null)
            {
                return;
            }

            LogicFactory.IncomeLogic.DeleteIncome(_selectedIncome);

            _incomes.Remove(_selectedIncome);

            _filterActive = false;
            _selectedIncome = null;
            _setSelectedIncome = true;
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

            IncomeSourceDomain incomeSource = incomeSourceComboBox.SelectedItem as IncomeSourceDomain;
            Month month = monthComboBox.SelectedValue as Month;
            int year = Convert.ToInt32(yearTextBox.Text);
            double amount = 0;
            if (!String.IsNullOrEmpty(amountTextBox.Text))
            {
                amount = Convert.ToDouble(amountTextBox.Text);
            }

            if (_uiMode == UserInterfaceModes.Adding)
            {
                IncomeDomain income = LogicFactory.IncomeLogic.AddIncome(incomeSource, month.Index, year, amount);

                income.DisplayedMonth = month.Name;
                income.DisplayedAmount = amount.ToString("#,##0");
                income.IncomeSourceDescription = incomeSource.Description;

                _incomes.Add(income);

                _selectedIncome = income;
            }
            else if (_uiMode == UserInterfaceModes.Editing)
            {
                _selectedIncome.IncomeSourceId = incomeSource.Id;
                _selectedIncome.Month = month.Index;
                _selectedIncome.Year = year;
                _selectedIncome.Amount = amount;                                

                IncomeDomain income = LogicFactory.IncomeLogic.UpdateIncome(_selectedIncome);
                income.DisplayedMonth = month.Name;
                income.DisplayedAmount = amount.ToString("#,##0");

                int index = _incomes.IndexOf(_selectedIncome);
                _incomes.Remove(_selectedIncome);
                _incomes.Insert(index, income);

                _selectedIncome = income;
            }
       
            _uiMode = UserInterfaceModes.Viewing;
            SetUIControlsAvailability();

            _filterActive = false;
            _setSelectedIncome = false;
            RefreshGrid();            
        }

        private void cancelToolStripButton_Click(object sender, EventArgs e)
        {
            _uiMode = UserInterfaceModes.Viewing;
            SetUIControlsAvailability();

            incomeDataGridView_SelectionChanged(incomeDataGridView, e);
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
            _setSelectedIncome = true;

            ProgressTrackerForm progressTrackerForm = new ProgressTrackerForm();
            progressTrackerForm.ProcessInformation = "Memfilter data pendapatan";

            progressTrackerForm.Task = new ProgressTrackerForm.BackgroundTask(
                () =>
                {
                    _displayedIncomes = _incomes.FindAll(p =>
                        {
                            bool match = true;

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

                            return match;
                        });
                });

            progressTrackerForm.ShowDialog();
            RefreshGrid();
        }

        private void showAllButton_Click(object sender, EventArgs e)
        {
            _filterActive = false;
            _setSelectedIncome = true;
            RefreshGrid();
        }

        private void incomeDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if ((_setSelectedIncome && (incomeDataGridView.SelectedRows == null || incomeDataGridView.SelectedRows.Count == 0)) ||
                (!_setSelectedIncome && _selectedIncome == null))
            {
                return;
            }

            if (_setSelectedIncome)
            {
                _selectedIncome = incomeDataGridView.SelectedRows[0].DataBoundItem as IncomeDomain;
            }

            IncomeSourceDomain incomeSource = _incomeSources.Find(p => p.Id == _selectedIncome.IncomeSourceId);
            incomeSourceComboBox.SelectedItem = incomeSource;

            Month month = _months.Find(p => p.Index == _selectedIncome.Month);
            monthComboBox.SelectedItem = month;

            yearTextBox.Text = _selectedIncome.Year.ToString();
            amountTextBox.Text = _selectedIncome.Amount.ToString();

            _setSelectedIncome = true;
        }

        private void SelectIncomeOnTheGrid(int incomeId)
        {
            int index = 0;
            foreach (DataGridViewRow row in incomeDataGridView.Rows)
            {
                IncomeDomain income = row.DataBoundItem as IncomeDomain;
                if (income.Id == incomeId)
                {
                    _selectedIncome = income;
                    break;
                }

                index++;
            }

            incomeDataGridView.Rows[index].Selected = true;
        }

        private void RefreshGrid()
        {
            _bindingSource.DataSource = null;
            incomeDataGridView.DataSource = null;
            incomeDataGridView.AutoGenerateColumns = false;

            _bindingSource.DataSource = _filterActive && _displayedIncomes != null ? _displayedIncomes : _incomes;
            incomeDataGridView.DataSource = _bindingSource;

            if (_selectedIncome != null)
            {
                SelectIncomeOnTheGrid(_selectedIncome.Id);
            }
        }

        private void SetUIControlsAvailability()
        {
            addToolStripButton.Enabled = _uiMode == UserInterfaceModes.Viewing;
            editToolStripButton.Enabled = _uiMode == UserInterfaceModes.Viewing;
            deleteToolStripButton.Enabled = _uiMode == UserInterfaceModes.Viewing;
            saveToolStripButton.Enabled = _uiMode != UserInterfaceModes.Viewing;
            cancelToolStripButton.Enabled = _uiMode != UserInterfaceModes.Viewing;

            incomeSourceComboBox.Enabled = _uiMode != UserInterfaceModes.Viewing;
            monthComboBox.Enabled = _uiMode != UserInterfaceModes.Viewing;
            yearTextBox.Enabled = _uiMode != UserInterfaceModes.Viewing;
            amountTextBox.Enabled = _uiMode != UserInterfaceModes.Viewing;

            filterFromMonthComboBox.Enabled = _uiMode == UserInterfaceModes.Viewing;
            filterFromYearTextBox.Enabled = _uiMode == UserInterfaceModes.Viewing;
            filterToMonthComboBox.Enabled = _uiMode == UserInterfaceModes.Viewing;
            filterToYearTextBox.Enabled = _uiMode == UserInterfaceModes.Viewing;
            filterButton.Enabled = _uiMode == UserInterfaceModes.Viewing;
            showAllButton.Enabled = _uiMode == UserInterfaceModes.Viewing;
        }

        private void IncomeForm_Load(object sender, EventArgs e)
        {
            Size = this.MdiParent.Size;


            List<IncomeDomain> incomes = null;

            ProgressTrackerForm progressTrackerForm = new ProgressTrackerForm();            
            progressTrackerForm.ProcessInformation = "Mengambil data pendapatan...";
            progressTrackerForm.Task = new ProgressTrackerForm.BackgroundTask(
                () =>
                { 
                    incomes = LogicFactory.IncomeLogic.GetAllIncomes();

                    _incomes = new List<IncomeDomain>();
                    foreach (IncomeDomain income in incomes)
                    {                        
                        Month month = _months.Find(p => p.Index == income.Month);
                        if (month != null)
                        {
                            income.DisplayedMonth = month.Name;
                        }

                        IncomeSourceDomain incomeSource = _incomeSources.Find(p => p.Id == income.IncomeSourceId);
                        if (incomeSource != null)
                        {
                            income.IncomeSourceDescription = incomeSource.Description;
                        }

                        income.DisplayedAmount = income.Amount.ToString("#,##0");

                        _incomes.Add(income);
                    }
                });

            progressTrackerForm.ShowDialog();

            RefreshGrid();
        }
    }
}
