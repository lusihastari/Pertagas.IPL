namespace Pertagas.IPL.View
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clusterIncomeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.incomeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.expenseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.adminToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clusterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.incomeSourceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.laporanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.incomeClusterReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.accountingReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.balanceReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addressBlockToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.adminToolStripMenuItem,
            this.laporanToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(282, 28);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clusterIncomeToolStripMenuItem,
            this.incomeToolStripMenuItem,
            this.expenseToolStripMenuItem,
            this.toolStripSeparator1,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // clusterIncomeToolStripMenuItem
            // 
            this.clusterIncomeToolStripMenuItem.Name = "clusterIncomeToolStripMenuItem";
            this.clusterIncomeToolStripMenuItem.Size = new System.Drawing.Size(201, 24);
            this.clusterIncomeToolStripMenuItem.Text = "Pemasukan Cluster";
            this.clusterIncomeToolStripMenuItem.Click += new System.EventHandler(this.clusterIncomeToolStripMenuItem_Click);
            // 
            // incomeToolStripMenuItem
            // 
            this.incomeToolStripMenuItem.Name = "incomeToolStripMenuItem";
            this.incomeToolStripMenuItem.Size = new System.Drawing.Size(201, 24);
            this.incomeToolStripMenuItem.Text = "Pemasukan";
            this.incomeToolStripMenuItem.Click += new System.EventHandler(this.incomeToolStripMenuItem_Click);
            // 
            // expenseToolStripMenuItem
            // 
            this.expenseToolStripMenuItem.Name = "expenseToolStripMenuItem";
            this.expenseToolStripMenuItem.Size = new System.Drawing.Size(201, 24);
            this.expenseToolStripMenuItem.Text = "Pengeluaran";
            this.expenseToolStripMenuItem.Click += new System.EventHandler(this.expenseToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(198, 6);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(201, 24);
            this.closeToolStripMenuItem.Text = "Tutup";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.tutupToolStripMenuItem_Click);
            // 
            // adminToolStripMenuItem
            // 
            this.adminToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.userToolStripMenuItem,
            this.clusterToolStripMenuItem,
            this.incomeSourceToolStripMenuItem,
            this.addressBlockToolStripMenuItem});
            this.adminToolStripMenuItem.Name = "adminToolStripMenuItem";
            this.adminToolStripMenuItem.Size = new System.Drawing.Size(65, 24);
            this.adminToolStripMenuItem.Text = "Admin";
            // 
            // userToolStripMenuItem
            // 
            this.userToolStripMenuItem.Name = "userToolStripMenuItem";
            this.userToolStripMenuItem.Size = new System.Drawing.Size(192, 24);
            this.userToolStripMenuItem.Text = "Pengguna";
            this.userToolStripMenuItem.Click += new System.EventHandler(this.userToolStripMenuItem_Click);
            // 
            // clusterToolStripMenuItem
            // 
            this.clusterToolStripMenuItem.Name = "clusterToolStripMenuItem";
            this.clusterToolStripMenuItem.Size = new System.Drawing.Size(192, 24);
            this.clusterToolStripMenuItem.Text = "Cluster";
            this.clusterToolStripMenuItem.Click += new System.EventHandler(this.clusterToolStripMenuItem_Click);
            // 
            // incomeSourceToolStripMenuItem
            // 
            this.incomeSourceToolStripMenuItem.Name = "incomeSourceToolStripMenuItem";
            this.incomeSourceToolStripMenuItem.Size = new System.Drawing.Size(192, 24);
            this.incomeSourceToolStripMenuItem.Text = "Jenis Pendapatan";
            this.incomeSourceToolStripMenuItem.Click += new System.EventHandler(this.incomeSourceToolStripMenuItem_Click);
            // 
            // laporanToolStripMenuItem
            // 
            this.laporanToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.incomeClusterReportToolStripMenuItem,
            this.accountingReportToolStripMenuItem,
            this.balanceReportToolStripMenuItem});
            this.laporanToolStripMenuItem.Name = "laporanToolStripMenuItem";
            this.laporanToolStripMenuItem.Size = new System.Drawing.Size(75, 24);
            this.laporanToolStripMenuItem.Text = "Laporan";
            // 
            // incomeClusterReportToolStripMenuItem
            // 
            this.incomeClusterReportToolStripMenuItem.Name = "incomeClusterReportToolStripMenuItem";
            this.incomeClusterReportToolStripMenuItem.Size = new System.Drawing.Size(201, 24);
            this.incomeClusterReportToolStripMenuItem.Text = "Pemasukan Cluster";
            this.incomeClusterReportToolStripMenuItem.Click += new System.EventHandler(this.incomeClusterReportToolStripMenuItem_Click);
            // 
            // accountingReportToolStripMenuItem
            // 
            this.accountingReportToolStripMenuItem.Name = "accountingReportToolStripMenuItem";
            this.accountingReportToolStripMenuItem.Size = new System.Drawing.Size(201, 24);
            this.accountingReportToolStripMenuItem.Text = "Keuangan";
            this.accountingReportToolStripMenuItem.Click += new System.EventHandler(this.accountingReportToolStripMenuItem_Click);
            // 
            // balanceReportToolStripMenuItem
            // 
            this.balanceReportToolStripMenuItem.Name = "balanceReportToolStripMenuItem";
            this.balanceReportToolStripMenuItem.Size = new System.Drawing.Size(201, 24);
            this.balanceReportToolStripMenuItem.Text = "Neraca Keuangan";
            this.balanceReportToolStripMenuItem.Click += new System.EventHandler(this.balanceReportToolStripMenuItem_Click);
            // 
            // addressBlockToolStripMenuItem
            // 
            this.addressBlockToolStripMenuItem.Name = "addressBlockToolStripMenuItem";
            this.addressBlockToolStripMenuItem.Size = new System.Drawing.Size(192, 24);
            this.addressBlockToolStripMenuItem.Text = "Alamat Blok";
            this.addressBlockToolStripMenuItem.Click += new System.EventHandler(this.addressBlockToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Sistem Iuran Pengelolaan Lingkungan";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem adminToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clusterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem incomeSourceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem incomeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem expenseToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem laporanToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem userToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clusterIncomeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem incomeClusterReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem accountingReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem balanceReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addressBlockToolStripMenuItem;
    }
}

