using Pertagas.IPL.Common;
using Pertagas.IPL.Domain;
using Pertagas.IPL.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Pertagas.IPL.View
{
    public partial class AddressBlockForm : Form
    {
        private UserInterfaceModes _uiMode = UserInterfaceModes.Viewing;
        private List<AddressBlockDomain> _addressBlocks = null;
        private AddressBlockDomain _selectedBlock = null;
        private BindingSource _bindingSource = new BindingSource();

        public AddressBlockForm()
        {
            InitializeComponent();

            _addressBlocks = LogicFactory.AddressBlockLogic.GetAllAddressBlocks();
            RefreshGrid();
            SetUIControlsAvailability();
        }

        private void RefreshGrid()
        {
            _bindingSource.DataSource = null;
            addressBlockDataGridView.DataSource = null;
            addressBlockDataGridView.AutoGenerateColumns = false;

            _bindingSource.DataSource = _addressBlocks;
            addressBlockDataGridView.DataSource = _bindingSource;
        }

        private void SetUIControlsAvailability()
        {
            addToolStripButton.Enabled = _uiMode == UserInterfaceModes.Viewing;
            editToolStripButton.Enabled = _uiMode == UserInterfaceModes.Viewing;
            deleteToolStripButton.Enabled = _uiMode == UserInterfaceModes.Viewing;
            saveToolStripButton.Enabled = _uiMode != UserInterfaceModes.Viewing;
            cancelToolStripButton.Enabled = _uiMode != UserInterfaceModes.Viewing;

            addressBlockTextBox.Enabled = _uiMode != UserInterfaceModes.Viewing;            
        }

        private void addToolStripButton_Click(object sender, EventArgs e)
        {
            _uiMode = UserInterfaceModes.Adding;
            SetUIControlsAvailability();

            addressBlockTextBox.Text = String.Empty;
        }

        private void clusterDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (addressBlockDataGridView.SelectedRows == null || addressBlockDataGridView.SelectedRows.Count == 0)
            {
                return;
            }

            DataGridViewRow row = addressBlockDataGridView.SelectedRows[0];
            _selectedBlock = row.DataBoundItem as AddressBlockDomain;
            if (_selectedBlock != null)
            {
                addressBlockTextBox.Text = _selectedBlock.Block;
            }
        }

        private void editToolStripButton_Click(object sender, EventArgs e)
        {
            if (_selectedBlock == null)
            {
                return;
            }

            _uiMode = UserInterfaceModes.Editing;
            SetUIControlsAvailability();
        }

        private void deleteToolStripButton_Click(object sender, EventArgs e)
        {
            if (_selectedBlock == null)
            {
                return;
            }

            LogicFactory.AddressBlockLogic.DeleteBlock(_selectedBlock.Block);

            _addressBlocks.Remove(_selectedBlock);
            RefreshGrid();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(addressBlockTextBox.Text))
            {
                MessageBox.Show("Blok tidak boleh dikosongkan!", null, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_uiMode == UserInterfaceModes.Adding)
            {
                AddressBlockDomain addressBlock = LogicFactory.AddressBlockLogic.AddBlock(addressBlockTextBox.Text);
                _addressBlocks.Add(addressBlock);
            }
            else if (_uiMode == UserInterfaceModes.Editing)
            {
                _selectedBlock.Block = addressBlockTextBox.Text;
                LogicFactory.AddressBlockLogic.UpdateBlock(_selectedBlock);                
            }

            RefreshGrid();
            _uiMode = UserInterfaceModes.Viewing;
            SetUIControlsAvailability();
        }

        private void cancelToolStripButton_Click(object sender, EventArgs e)
        {
            _uiMode = UserInterfaceModes.Viewing;
            SetUIControlsAvailability();

            clusterDataGridView_SelectionChanged(addressBlockDataGridView, e);
        }
    }
}
