using Pertagas.IPL.Logic;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Pertagas.IPL.View
{
    public partial class MainForm : Form
    {
        private bool _loggedIn = false;

        public MainForm()
        {
            InitializeComponent();
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileInfo fileInfo = new FileInfo(assembly.Location);
            string databaseFilePath = Path.Combine(fileInfo.DirectoryName, "IPL.sqlite");
            
            string appDataPertagasPath = Path.Combine( Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Pertagas");
            if (!Directory.Exists(appDataPertagasPath))
            {
                Directory.CreateDirectory(appDataPertagasPath);      
            }

            if (!File.Exists(Path.Combine(appDataPertagasPath, "IPL.sqlite")))
            {
                File.Copy(databaseFilePath, Path.Combine(appDataPertagasPath, "IPL.sqlite"));
            } 
            
            databaseFilePath = Path.Combine(appDataPertagasPath, "IPL.sqlite");

            ProgressTrackerForm trackerForm = new ProgressTrackerForm();
            trackerForm.ProcessInformation = "Mengkonfigurasi database...";
            trackerForm.Task = new ProgressTrackerForm.BackgroundTask(
                () =>
                {
                    LogicFactory.Initialize(databaseFilePath);
                });
            trackerForm.ShowDialog();
            this.FormClosed += MainForm_FormClosed;
        }

        void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            LogicFactory.Terminate();
        }

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            if (!_loggedIn)
            {
                adminToolStripMenuItem.Enabled = false;
                laporanToolStripMenuItem.Enabled = false;
                incomeToolStripMenuItem.Enabled = false;
                expenseToolStripMenuItem.Enabled = false;
                clusterIncomeToolStripMenuItem.Enabled = false;

                LoginForm loginForm = new LoginForm();
                loginForm.MdiParent = this;
                loginForm.Show();
                loginForm.FormClosed += loginForm_FormClosed;
            }
        }

        void loginForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            LoginForm loginForm = sender as LoginForm;
            
            _loggedIn = loginForm.LoginSuccess;
            adminToolStripMenuItem.Enabled = loginForm.LoginSuccess;
            laporanToolStripMenuItem.Enabled = loginForm.LoginSuccess;
            incomeToolStripMenuItem.Enabled = loginForm.LoginSuccess;
            expenseToolStripMenuItem.Enabled = loginForm.LoginSuccess;
            clusterIncomeToolStripMenuItem.Enabled = loginForm.LoginSuccess;
        }

        private void tutupToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            LogicFactory.Terminate();
            this.Close();
        }

        private void clusterToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            ClusterForm clusterForm = new ClusterForm();            
            clusterForm.ShowDialog();
        }

        private void userToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            UserForm userForm = new UserForm();
            userForm.ShowDialog();
        }

        private void incomeSourceToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            IncomeSourceForm incomeSourceForm = new IncomeSourceForm();
            incomeSourceForm.ShowDialog();
        }

        private void clusterIncomeToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            IncomeClusterForm incomeClusterForm = new IncomeClusterForm();
            incomeClusterForm.MdiParent = this;
            incomeClusterForm.Show();
        }

        private void incomeClusterReportToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            IncomeClusterReportForm incomeClusterReportForm = new IncomeClusterReportForm();
            incomeClusterReportForm.ShowDialog();
        }

        private void incomeToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            IncomeForm incomeForm = new IncomeForm();
            incomeForm.MdiParent = this;
            incomeForm.Show();
        }

        private void expenseToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            ExpenseForm expenseForm = new ExpenseForm();
            expenseForm.MdiParent = this;
            expenseForm.Show();
        }

        private void accountingReportToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            AccountingReportForm incomeReportForm = new AccountingReportForm();
            incomeReportForm.ShowDialog();
        }

        private void balanceReportToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            BalanceReportForm balanceReportForm = new BalanceReportForm();
            balanceReportForm.ShowDialog();
        }

        private void addressBlockToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            AddressBlockForm addressBlockForm = new AddressBlockForm();
            addressBlockForm.ShowDialog();
        }
    }
}
