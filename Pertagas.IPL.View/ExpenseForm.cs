using Pertagas.IPL.Common;
using Pertagas.IPL.Domain;
using Pertagas.IPL.Logic;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Pertagas.IPL.View
{
    public partial class ExpenseForm : Form
    {
        private UserInterfaceModes _uiMode = UserInterfaceModes.Viewing;
        private bool _filterActive = false;
        private List<ExpenseDomain> _expenses;
        private List<ExpenseDomain> _displayedExpenses;
        private ExpenseDomain _selectedExpense;
        private BindingSource _bindingSource = new BindingSource();
        private Regex _numericRegex = new Regex("^[0-9]*$");
        private List<Month> _months = MonthUtility.GetMonths();
        private bool _setSelectedExpense = true;

        public ExpenseForm()
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

            SetUIControlsAvailability();
        }

        private void SetUIControlsAvailability()
        {
            addToolStripButton.Enabled = _uiMode == UserInterfaceModes.Viewing;
            editToolStripButton.Enabled = _uiMode == UserInterfaceModes.Viewing;
            deleteToolStripButton.Enabled = _uiMode == UserInterfaceModes.Viewing;
            saveToolStripButton.Enabled = _uiMode != UserInterfaceModes.Viewing;
            cancelToolStripButton.Enabled = _uiMode != UserInterfaceModes.Viewing;

            descriptionTextBox.Enabled = _uiMode != UserInterfaceModes.Viewing;
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

        private void RefreshGrid()
        {
            _bindingSource.DataSource = null;
            expenseDataGridView.DataSource = null;
            expenseDataGridView.AutoGenerateColumns = false;

            _bindingSource.DataSource = _filterActive && _displayedExpenses != null ? _displayedExpenses : _expenses;
            expenseDataGridView.DataSource = _bindingSource;

            if (_selectedExpense != null)
            {
                SelectExpenseOnTheGrid(_selectedExpense.Id);
            }
        }

        private void SelectExpenseOnTheGrid(int expenseId)
        {
            int index = 0;
            foreach (DataGridViewRow row in expenseDataGridView.Rows)
            {
                ExpenseDomain expense = row.DataBoundItem as ExpenseDomain;
                if (expense.Id == _selectedExpense.Id)
                {
                    _selectedExpense = expense;
                    break;
                }

                index++;
            }

            expenseDataGridView.Rows[index].Selected = true;
        }

        private void ExpenseForm_Load(object sender, EventArgs e)
        {
            Size = this.MdiParent.Size;

            List<ExpenseDomain> expenses = null;

            ProgressTrackerForm progressTrackerForm = new ProgressTrackerForm();
            progressTrackerForm.ProcessInformation = "Mengambil data pengeluaran...";

            progressTrackerForm.Task = new ProgressTrackerForm.BackgroundTask(
                () =>
                {
                    expenses = LogicFactory.ExpenseLogic.GetAllExpenses();

                    _expenses = new List<ExpenseDomain>();
                    foreach (ExpenseDomain expense in expenses)
                    {
                        Month month = _months.Find(p => p.Index == expense.Month);
                        if (month != null)
                        {
                            expense.DisplayedMonth = month.Name;
                        }
                        expense.DisplayedAmount = expense.Amount.ToString("#,##0");

                        _expenses.Add(expense);
                    }
                }
                );

            progressTrackerForm.ShowDialog();

            RefreshGrid();
        }

        private void expenseDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if ((_setSelectedExpense && (expenseDataGridView.SelectedRows == null || expenseDataGridView.SelectedRows.Count == 0)) ||
                (!_setSelectedExpense && _selectedExpense == null));
            {
                return;
            }

            if (_setSelectedExpense)
            {
                _selectedExpense = expenseDataGridView.SelectedRows[0].DataBoundItem as ExpenseDomain;
            }

            Month month = _months.Find(p => p.Index == _selectedExpense.Month);
            monthComboBox.SelectedItem = month;

            descriptionTextBox.Text = _selectedExpense.Description;
            yearTextBox.Text = _selectedExpense.Year.ToString();
            amountTextBox.Text = _selectedExpense.Amount.ToString();

            _setSelectedExpense = true;
        }

        private void addToolStripButton_Click(object sender, EventArgs e)
        {
            _uiMode = UserInterfaceModes.Adding;
            SetUIControlsAvailability();

            Month month = _months.Find(p => p.Index == DateTime.Today.Month);
            monthComboBox.SelectedItem = month;

            descriptionTextBox.Text = String.Empty;
            yearTextBox.Text = DateTime.Today.Year.ToString();
            amountTextBox.Text = String.Empty;
        }

        private void editToolStripButton_Click(object sender, EventArgs e)
        {
            if (_selectedExpense == null)
            {
                return;
            }

            _uiMode = UserInterfaceModes.Editing;
            SetUIControlsAvailability();
        }

        private void deleteToolStripButton_Click(object sender, EventArgs e)
        {
            if (_selectedExpense == null)
            {
                return;
            }

            LogicFactory.ExpenseLogic.DeleteExpense(_selectedExpense);

            _expenses.Remove(_selectedExpense);

            _filterActive = false;
            _selectedExpense = null;
            _setSelectedExpense = true;
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
            
            Month month = monthComboBox.SelectedValue as Month;
            int year = Convert.ToInt32(yearTextBox.Text);
            double amount = 0;
            if (!String.IsNullOrEmpty(amountTextBox.Text))
            {
                amount = Convert.ToDouble(amountTextBox.Text);
            }

            if (_uiMode == UserInterfaceModes.Adding)
            {
                ExpenseDomain expense = LogicFactory.ExpenseLogic.AddExpense(descriptionTextBox.Text, month.Index, year, amount);
                expense.DisplayedMonth = month.Name;
                expense.DisplayedAmount = amount.ToString("#,##0");

                _expenses.Add(expense);
                _selectedExpense = expense;
            }
            else if (_uiMode == UserInterfaceModes.Editing)
            {
                _selectedExpense.Description = descriptionTextBox.Text;
                _selectedExpense.Month = month.Index;
                _selectedExpense.Year = year;
                _selectedExpense.Amount = amount;

                ExpenseDomain expense = LogicFactory.ExpenseLogic.UpdateExpense(_selectedExpense);
                expense.DisplayedMonth = month.Name;
                expense.DisplayedAmount = amount.ToString("#,##0");

                int index = _expenses.IndexOf(_selectedExpense);
                _expenses.Remove(_selectedExpense);
                _expenses.Insert(index, expense);

                _selectedExpense = expense;
            }

            _uiMode = UserInterfaceModes.Viewing;
            SetUIControlsAvailability();

            _filterActive = false;
            _setSelectedExpense = false;
            RefreshGrid();    
        }

        private void cancelToolStripButton_Click(object sender, EventArgs e)
        {
            _uiMode = UserInterfaceModes.Viewing;
            SetUIControlsAvailability();

            expenseDataGridView_SelectionChanged(expenseDataGridView, e);
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
            _setSelectedExpense = true;

            ProgressTrackerForm progressTrackerForm = new ProgressTrackerForm();
            progressTrackerForm.ProcessInformation = "Memfilter data pengeluaran...";
            progressTrackerForm.Task = new ProgressTrackerForm.BackgroundTask(
                () =>
                {
                    _displayedExpenses = _expenses.FindAll(p =>
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
            _setSelectedExpense = true;
            RefreshGrid();
        }
    }
}
