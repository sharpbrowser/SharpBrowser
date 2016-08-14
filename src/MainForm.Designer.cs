namespace SharpBrowser
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.menuStripTab = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.menuCloseTab = new System.Windows.Forms.ToolStripMenuItem();
			this.menuCloseOtherTabs = new System.Windows.Forms.ToolStripMenuItem();
			this.bRefresh = new System.Windows.Forms.Button();
			this.bStop = new System.Windows.Forms.Button();
			this.bForward = new System.Windows.Forms.Button();
			this.bBack = new System.Windows.Forms.Button();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.bDownloads = new System.Windows.Forms.Button();
			this.txtUrl = new System.Windows.Forms.TextBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.tabPages = new FarsiLibrary.Win.FATabStrip();
			this.tabStrip1 = new FarsiLibrary.Win.FATabStripItem();
			this.tabStripAdd = new FarsiLibrary.Win.FATabStripItem();
			this.menuStripTab.SuspendLayout();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tabPages)).BeginInit();
			this.tabPages.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStripTab
			// 
			this.menuStripTab.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuCloseTab,
            this.menuCloseOtherTabs});
			this.menuStripTab.Name = "menuStripTab";
			this.menuStripTab.Size = new System.Drawing.Size(198, 52);
			// 
			// menuCloseTab
			// 
			this.menuCloseTab.Name = "menuCloseTab";
			this.menuCloseTab.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F4)));
			this.menuCloseTab.Size = new System.Drawing.Size(197, 24);
			this.menuCloseTab.Text = "Close tab";
			this.menuCloseTab.Click += new System.EventHandler(this.menuCloseTab_Click);
			// 
			// menuCloseOtherTabs
			// 
			this.menuCloseOtherTabs.Name = "menuCloseOtherTabs";
			this.menuCloseOtherTabs.Size = new System.Drawing.Size(197, 24);
			this.menuCloseOtherTabs.Text = "Close other tabs";
			this.menuCloseOtherTabs.Click += new System.EventHandler(this.menuCloseOtherTabs_Click);
			// 
			// bRefresh
			// 
			this.bRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.bRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.bRefresh.ForeColor = System.Drawing.Color.White;
			this.bRefresh.Image = ((System.Drawing.Image)(resources.GetObject("bRefresh.Image")));
			this.bRefresh.Location = new System.Drawing.Point(854, 2);
			this.bRefresh.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.bRefresh.Name = "bRefresh";
			this.bRefresh.Size = new System.Drawing.Size(35, 35);
			this.bRefresh.TabIndex = 3;
			this.bRefresh.UseVisualStyleBackColor = true;
			this.bRefresh.Click += new System.EventHandler(this.bRefresh_Click);
			// 
			// bStop
			// 
			this.bStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.bStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.bStop.ForeColor = System.Drawing.Color.White;
			this.bStop.Image = ((System.Drawing.Image)(resources.GetObject("bStop.Image")));
			this.bStop.Location = new System.Drawing.Point(854, 2);
			this.bStop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.bStop.Name = "bStop";
			this.bStop.Size = new System.Drawing.Size(35, 35);
			this.bStop.TabIndex = 2;
			this.bStop.UseVisualStyleBackColor = true;
			this.bStop.Click += new System.EventHandler(this.bStop_Click);
			// 
			// bForward
			// 
			this.bForward.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.bForward.ForeColor = System.Drawing.Color.White;
			this.bForward.Image = ((System.Drawing.Image)(resources.GetObject("bForward.Image")));
			this.bForward.Location = new System.Drawing.Point(39, 2);
			this.bForward.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.bForward.Name = "bForward";
			this.bForward.Size = new System.Drawing.Size(35, 35);
			this.bForward.TabIndex = 1;
			this.bForward.UseVisualStyleBackColor = true;
			this.bForward.Click += new System.EventHandler(this.bForward_Click);
			// 
			// bBack
			// 
			this.bBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.bBack.ForeColor = System.Drawing.Color.White;
			this.bBack.Image = ((System.Drawing.Image)(resources.GetObject("bBack.Image")));
			this.bBack.Location = new System.Drawing.Point(2, 2);
			this.bBack.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.bBack.Name = "bBack";
			this.bBack.Size = new System.Drawing.Size(35, 35);
			this.bBack.TabIndex = 0;
			this.bBack.UseVisualStyleBackColor = true;
			this.bBack.Click += new System.EventHandler(this.bBack_Click);
			// 
			// timer1
			// 
			this.timer1.Interval = 50;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// bDownloads
			// 
			this.bDownloads.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.bDownloads.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.bDownloads.ForeColor = System.Drawing.Color.White;
			this.bDownloads.Image = ((System.Drawing.Image)(resources.GetObject("bDownloads.Image")));
			this.bDownloads.Location = new System.Drawing.Point(892, 2);
			this.bDownloads.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.bDownloads.Name = "bDownloads";
			this.bDownloads.Size = new System.Drawing.Size(35, 35);
			this.bDownloads.TabIndex = 4;
			this.bDownloads.UseVisualStyleBackColor = true;
			this.bDownloads.Click += new System.EventHandler(this.bDownloads_Click);
			// 
			// txtUrl
			// 
			this.txtUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtUrl.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtUrl.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtUrl.Location = new System.Drawing.Point(85, 5);
			this.txtUrl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.txtUrl.Name = "txtUrl";
			this.txtUrl.Size = new System.Drawing.Size(763, 31);
			this.txtUrl.TabIndex = 5;
			this.txtUrl.Click += new System.EventHandler(this.txtUrl_Click);
			this.txtUrl.TextChanged += new System.EventHandler(this.txtUrl_TextChanged);
			this.txtUrl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUrl_KeyDown_1);
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.White;
			this.panel1.Controls.Add(this.bRefresh);
			this.panel1.Controls.Add(this.txtUrl);
			this.panel1.Controls.Add(this.bDownloads);
			this.panel1.Controls.Add(this.bForward);
			this.panel1.Controls.Add(this.bBack);
			this.panel1.Controls.Add(this.bStop);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(933, 40);
			this.panel1.TabIndex = 6;
			// 
			// tabPages
			// 
			this.tabPages.ContextMenuStrip = this.menuStripTab;
			this.tabPages.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabPages.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tabPages.Items.AddRange(new FarsiLibrary.Win.FATabStripItem[] {
            this.tabStrip1,
            this.tabStripAdd});
			this.tabPages.Location = new System.Drawing.Point(0, 40);
			this.tabPages.Name = "tabPages";
			this.tabPages.SelectedItem = this.tabStrip1;
			this.tabPages.Size = new System.Drawing.Size(933, 631);
			this.tabPages.TabIndex = 4;
			this.tabPages.Text = "faTabStrip1";
			this.tabPages.TabStripItemSelectionChanged += new FarsiLibrary.Win.TabStripItemChangedHandler(this.tabPages_TabStripItemSelectionChanged);
			// 
			// tabStrip1
			// 
			this.tabStrip1.IsDrawn = true;
			this.tabStrip1.Name = "tabStrip1";
			this.tabStrip1.Selected = true;
			this.tabStrip1.Size = new System.Drawing.Size(931, 601);
			this.tabStrip1.TabIndex = 0;
			this.tabStrip1.Title = "Tab1";
			// 
			// tabStripAdd
			// 
			this.tabStripAdd.CanClose = false;
			this.tabStripAdd.IsDrawn = true;
			this.tabStripAdd.Name = "tabStripAdd";
			this.tabStripAdd.Size = new System.Drawing.Size(931, 601);
			this.tabStripAdd.TabIndex = 1;
			this.tabStripAdd.Title = "+";
			// 
			// MainForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(933, 671);
			this.Controls.Add(this.tabPages);
			this.Controls.Add(this.panel1);
			this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Title";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.menuStripTab.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.tabPages)).EndInit();
			this.tabPages.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

		private FarsiLibrary.Win.FATabStrip tabPages;
        private FarsiLibrary.Win.FATabStripItem tabStrip1;
        private FarsiLibrary.Win.FATabStripItem tabStripAdd;
		private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ContextMenuStrip menuStripTab;
        private System.Windows.Forms.ToolStripMenuItem menuCloseTab;
        private System.Windows.Forms.ToolStripMenuItem menuCloseOtherTabs;
		private System.Windows.Forms.Button bForward;
		private System.Windows.Forms.Button bBack;
		private System.Windows.Forms.Button bStop;
		private System.Windows.Forms.Button bRefresh;
		private System.Windows.Forms.Button bDownloads;
		private System.Windows.Forms.TextBox txtUrl;
		private System.Windows.Forms.Panel panel1;
    }
}

