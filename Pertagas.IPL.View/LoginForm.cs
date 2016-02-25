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
    public partial class LoginForm : Form
    {
        private bool _loginSuccess = false;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            string errorMessage;
            if (!LogicFactory.UserLogic.Login(userNameTextBox.Text, passwordTextBox.Text, out errorMessage))
            {
                MessageBox.Show(errorMessage, "Login", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                _loginSuccess = true;
                this.Close();
            }
        }

        public bool LoginSuccess
        {
            get { return _loginSuccess; }
        }

        private void LoginForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                loginButton_Click(loginButton, e);
            }
        }
    }
}
