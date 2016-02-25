using Pertagas.IPL.Common;
using Pertagas.IPL.Domain;
using Pertagas.IPL.Logic;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Pertagas.IPL.View
{
    public partial class UserForm : Form
    {
        private UserInterfaceModes _uiMode = UserInterfaceModes.Viewing;
        private List<UserDomain> _users = null;
        private BindingSource _bindingSource = new BindingSource();
        private UserDomain _selectedUser = null;
        private bool _passwordChanged = false;

        public UserForm()
        {
            InitializeComponent();
            userDataGridView.ReadOnly = true;
        }

        private void UserForm_Load(object sender, EventArgs e)
        {
            SetUIControlsAvailability();
            _users = LogicFactory.UserLogic.GetAllUsers();
            RefreshGrid();
        }

        private void SetUIControlsAvailability()
        {
            addToolStripButton.Enabled = _uiMode == UserInterfaceModes.Viewing;
            editToolStripButton.Enabled = _uiMode == UserInterfaceModes.Viewing;
            deleteToolStripButton.Enabled = _uiMode == UserInterfaceModes.Viewing;
            saveToolStripButton.Enabled = _uiMode != UserInterfaceModes.Viewing;
            cancelToolStripButton.Enabled = _uiMode != UserInterfaceModes.Viewing;

            firstNameTextBox.Enabled = _uiMode != UserInterfaceModes.Viewing;
            lastNameTextBox.Enabled = _uiMode != UserInterfaceModes.Viewing;
            userNameTextBox.Enabled = _uiMode == UserInterfaceModes.Adding;
            passwordTextBox.Enabled = _uiMode != UserInterfaceModes.Viewing;
        }

        private void RefreshGrid()
        {
            _bindingSource.DataSource = null;
            userDataGridView.DataSource = null;
            userDataGridView.AutoGenerateColumns = false;

            _bindingSource.DataSource = _users;
            userDataGridView.DataSource = _bindingSource;
        }

        private void addToolStripButton_Click(object sender, EventArgs e)
        {
            _uiMode = UserInterfaceModes.Adding;
            SetUIControlsAvailability();

            firstNameTextBox.Text = String.Empty;
            lastNameTextBox.Text = String.Empty;
            userNameTextBox.Text = String.Empty;
            passwordTextBox.Text = String.Empty;
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(userNameTextBox.Text))
            {
                MessageBox.Show("Nama Login tidak boleh dikosongkan!");
                return;
            }

            if (_uiMode == UserInterfaceModes.Adding)
            {
                string message;
                UserDomain newUser = LogicFactory.UserLogic.AddUser(firstNameTextBox.Text, lastNameTextBox.Text, userNameTextBox.Text, passwordTextBox.Text, out message);
                if (newUser == null)
                {
                    MessageBox.Show(message, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _users.Add(newUser);
            }
            else if (_uiMode == UserInterfaceModes.Editing)
            {
                _selectedUser.FirstName = firstNameTextBox.Text;
                _selectedUser.Lastname = lastNameTextBox.Text;
                if (_passwordChanged)
                {
                    _selectedUser.Password = passwordTextBox.Text;
                }
                _selectedUser = LogicFactory.UserLogic.UpdateUser(_selectedUser, _passwordChanged);
                
                UserDomain user = _users.Find(p => p.Id == _selectedUser.Id);
                _users.Remove(user);
                _users.Add(_selectedUser);
            }

            _uiMode = UserInterfaceModes.Viewing;
            SetUIControlsAvailability();

            RefreshGrid();
        }

        private void cancelToolStripButton_Click(object sender, EventArgs e)
        {
            _uiMode = UserInterfaceModes.Viewing;
            SetUIControlsAvailability();

            userDataGridView_SelectionChanged(userDataGridView, e);
        }

        private void userDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (userDataGridView.SelectedRows == null || userDataGridView.SelectedRows.Count == 0)
            {
                return;
            }

            DataGridViewRow row = userDataGridView.SelectedRows[0];
            _selectedUser = row.DataBoundItem as UserDomain;
            if (_selectedUser != null)
            {
                firstNameTextBox.Text = _selectedUser.FirstName;
                lastNameTextBox.Text = _selectedUser.Lastname;
                userNameTextBox.Text = _selectedUser.Username;
                passwordTextBox.Text = _selectedUser.Password;
            }
        }

        private void editToolStripButton_Click(object sender, EventArgs e)
        {
            if (_selectedUser == null)
            {
                return;
            }

            _uiMode = UserInterfaceModes.Editing;
            SetUIControlsAvailability();
        }

        private void deleteToolStripButton_Click(object sender, EventArgs e)
        {
            if (_selectedUser == null || SecurityManager.CurrentUser.Id == _selectedUser.Id)
            {
                return;
            }

            string errorMessage;
            if (!LogicFactory.UserLogic.DeleteUser(_selectedUser, out errorMessage))
            {
                MessageBox.Show(errorMessage, null, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            _users.Remove(_selectedUser);
            RefreshGrid();
        }

        private void passwordTextBox_TextChanged(object sender, EventArgs e)
        {
            _passwordChanged = true;
        }
    }
}
