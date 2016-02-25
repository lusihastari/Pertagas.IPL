using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Pertagas.IPL.View
{
    public partial class ProgressTrackerForm : Form
    {
        public delegate void BackgroundTask();

        private BackgroundTask _backgroundTask;
        private BackgroundWorker _backgroundWorker = new BackgroundWorker();

        public ProgressTrackerForm()
        {
            InitializeComponent();
            _backgroundWorker.DoWork += _backgroundWorker_DoWork;
            _backgroundWorker.RunWorkerCompleted += _backgroundWorker_RunWorkerCompleted;
            progressBar.Style = ProgressBarStyle.Marquee;
            
        }

        void _backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();
        }

        void _backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            _backgroundTask();
        }

        public BackgroundTask Task
        {
            set { _backgroundTask = value; }
        }

        public string ProcessInformation
        {
            set { progressLabel.Text = value; }
        }

        private void ProgressTrackerForm_Load(object sender, EventArgs e)
        {
            _backgroundWorker.RunWorkerAsync();
        }
    }
}
