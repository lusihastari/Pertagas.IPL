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
    public partial class ClusterForm : Form
    {
        private UserInterfaceModes _uiMode = UserInterfaceModes.Viewing;
        private List<ClusterDomain> _clusters = null;
        private BindingSource _bindingSource = new BindingSource();
        private ClusterDomain _selectedCluster = null;

        public ClusterForm()
        {
            InitializeComponent();

            _clusters = LogicFactory.ClusterLogic.GetAllCluster();
            RefreshGrid();
            SetUIControlsAvailability();
        }

        private void RefreshGrid()
        {
            _bindingSource.DataSource = null;
            clusterDataGridView.DataSource = null;
            clusterDataGridView.AutoGenerateColumns = false;

            _bindingSource.DataSource = _clusters;
            clusterDataGridView.DataSource = _bindingSource;
        }

        private void SetUIControlsAvailability()
        {
            addToolStripButton.Enabled = _uiMode == UserInterfaceModes.Viewing;
            editToolStripButton.Enabled = _uiMode == UserInterfaceModes.Viewing;
            deleteToolStripButton.Enabled = _uiMode == UserInterfaceModes.Viewing;
            saveToolStripButton.Enabled = _uiMode != UserInterfaceModes.Viewing;
            cancelToolStripButton.Enabled = _uiMode != UserInterfaceModes.Viewing;

            clusterNameTextBox.Enabled = _uiMode != UserInterfaceModes.Viewing;            
        }

        private void addToolStripButton_Click(object sender, EventArgs e)
        {
            _uiMode = UserInterfaceModes.Adding;
            SetUIControlsAvailability();

            clusterNameTextBox.Text = String.Empty;
        }

        private void clusterDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (clusterDataGridView.SelectedRows == null || clusterDataGridView.SelectedRows.Count == 0)
            {
                return;
            }

            DataGridViewRow row = clusterDataGridView.SelectedRows[0];
            _selectedCluster = row.DataBoundItem as ClusterDomain;
            if (_selectedCluster != null)
            {
                clusterNameTextBox.Text = _selectedCluster.ClusterName;
            }
        }

        private void editToolStripButton_Click(object sender, EventArgs e)
        {
            if (_selectedCluster == null)
            {
                return;
            }

            _uiMode = UserInterfaceModes.Editing;
            SetUIControlsAvailability();
        }

        private void deleteToolStripButton_Click(object sender, EventArgs e)
        {
            if (_selectedCluster == null)
            {
                return;
            }

            string errorMessage;
            if (!LogicFactory.ClusterLogic.DeleteCluster(_selectedCluster, out errorMessage))
            {
                MessageBox.Show(errorMessage, null, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            _clusters.Remove(_selectedCluster);
            RefreshGrid();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(clusterNameTextBox.Text))
            {
                MessageBox.Show("Nama cluster tidak boleh dikosongkan!", null, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_uiMode == UserInterfaceModes.Adding)
            {
                ClusterDomain cluster = LogicFactory.ClusterLogic.AddCluster(clusterNameTextBox.Text);
                _clusters.Add(cluster);
            }
            else if (_uiMode == UserInterfaceModes.Editing)
            {
                _selectedCluster.ClusterName = clusterNameTextBox.Text;
                LogicFactory.ClusterLogic.UpdateCluster(_selectedCluster);

                ClusterDomain cluster = _clusters.Find(p => p.Id == _selectedCluster.Id);
                _clusters.Remove(cluster);
                _clusters.Add(_selectedCluster);
            }

            RefreshGrid();
            _uiMode = UserInterfaceModes.Viewing;
            SetUIControlsAvailability();
        }

        private void cancelToolStripButton_Click(object sender, EventArgs e)
        {
            _uiMode = UserInterfaceModes.Viewing;
            SetUIControlsAvailability();

            clusterDataGridView_SelectionChanged(clusterDataGridView, e);
        }
    }
}
