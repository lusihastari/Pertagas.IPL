using Pertagas.IPL.Common;
using Pertagas.IPL.Domain;
using Pertagas.IPL.Logic;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Pertagas.IPL.View
{
    public partial class IncomeSourceForm : Form
    {
        private UserInterfaceModes _uiMode = UserInterfaceModes.Viewing;
        private List<IncomeSourceDomain> _incomeSources = null;
        private BindingSource _bindingSource = new BindingSource();
        private IncomeSourceDomain _selectedIncomeSource = null;

        public IncomeSourceForm()
        {
            InitializeComponent();

            _incomeSources = LogicFactory.IncomeSourceLogic.GetAllIncomeSources();
            RefreshGrid();
            SetUIControlsAvailability();
        }

        private void RefreshGrid()
        {
            _bindingSource.DataSource = null;
            incomeSourceDataGridView.DataSource = null;
            incomeSourceDataGridView.AutoGenerateColumns = false;

            _bindingSource.DataSource = _incomeSources;
            incomeSourceDataGridView.DataSource = _bindingSource;
        }

        private void SetUIControlsAvailability()
        {
            addToolStripButton.Enabled = _uiMode == UserInterfaceModes.Viewing;
            editToolStripButton.Enabled = _uiMode == UserInterfaceModes.Viewing;
            deleteToolStripButton.Enabled = _uiMode == UserInterfaceModes.Viewing;
            saveToolStripButton.Enabled = _uiMode != UserInterfaceModes.Viewing;
            cancelToolStripButton.Enabled = _uiMode != UserInterfaceModes.Viewing;

            incomeSourceDescriptionTextBox.Enabled = _uiMode != UserInterfaceModes.Viewing;            
        }

        private void addToolStripButton_Click(object sender, EventArgs e)
        {
            _uiMode = UserInterfaceModes.Adding;
            SetUIControlsAvailability();

            incomeSourceDescriptionTextBox.Text = String.Empty;
        }

        private void clusterDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (incomeSourceDataGridView.SelectedRows == null || incomeSourceDataGridView.SelectedRows.Count == 0)
            {
                return;
            }

            DataGridViewRow row = incomeSourceDataGridView.SelectedRows[0];
            _selectedIncomeSource = row.DataBoundItem as IncomeSourceDomain;
            if (_selectedIncomeSource != null)
            {
                incomeSourceDescriptionTextBox.Text = _selectedIncomeSource.Description;
            }
        }

        private void editToolStripButton_Click(object sender, EventArgs e)
        {
            if (_selectedIncomeSource == null)
            {
                return;
            }

            _uiMode = UserInterfaceModes.Editing;
            SetUIControlsAvailability();
        }

        private void deleteToolStripButton_Click(object sender, EventArgs e)
        {
            if (_selectedIncomeSource == null)
            {
                return;
            }

            string errorMessage;
            if (!LogicFactory.IncomeSourceLogic.DeleteIncomeSource(_selectedIncomeSource, out errorMessage))
            {
                MessageBox.Show(errorMessage, null, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            _incomeSources.Remove(_selectedIncomeSource);
            RefreshGrid();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(incomeSourceDescriptionTextBox.Text))
            {
                MessageBox.Show("Nama cluster tidak boleh dikosongkan!", null, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_uiMode == UserInterfaceModes.Adding)
            {
                IncomeSourceDomain incomeSource = LogicFactory.IncomeSourceLogic.AddIncomeSource(incomeSourceDescriptionTextBox.Text);
                _incomeSources.Add(incomeSource);
            }
            else if (_uiMode == UserInterfaceModes.Editing)
            {
                _selectedIncomeSource.Description = incomeSourceDescriptionTextBox.Text;
                LogicFactory.IncomeSourceLogic.UpdateIncomeSource(_selectedIncomeSource);

                IncomeSourceDomain incomeSource = _incomeSources.Find(p => p.Id == _selectedIncomeSource.Id);
                _incomeSources.Remove(incomeSource);
                _incomeSources.Add(_selectedIncomeSource);
            }

            RefreshGrid();
            _uiMode = UserInterfaceModes.Viewing;
            SetUIControlsAvailability();
        }

        private void cancelToolStripButton_Click(object sender, EventArgs e)
        {
            _uiMode = UserInterfaceModes.Viewing;
            SetUIControlsAvailability();

            clusterDataGridView_SelectionChanged(incomeSourceDataGridView, e);
        }
    }
}
