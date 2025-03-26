using System.Windows.Forms;

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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            menuStripTab = new ContextMenuStrip(components);
            menuCloseTab = new ToolStripMenuItem();
            menuCloseOtherTabs = new ToolStripMenuItem();
            BtnRefresh = new Button();
            BtnStop = new Button();
            BtnForward = new Button();
            BtnBack = new Button();
            timer1 = new Timer(components);
            BtnDownloads = new Button();
            TxtURL = new TextBox();
            PanelToolbar = new Panel();
            btn_Menu = new Button();
            BtnHome = new Button();
            TabPages = new SharpBrowser.Controls.BrowserTabStrip.BrowserTabStrip();
            tabStrip1 = new SharpBrowser.Controls.BrowserTabStrip.BrowserTabStripItem();
            tabStripAdd = new SharpBrowser.Controls.BrowserTabStrip.BrowserTabStripItem();
            PanelSearch = new Panel();
            BtnNextSearch = new Button();
            BtnPrevSearch = new Button();
            BtnCloseSearch = new Button();
            TxtSearch = new TextBox();
            PanelStatus = new Panel();
            menuStripTab.SuspendLayout();
            PanelToolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)TabPages).BeginInit();
            TabPages.SuspendLayout();
            PanelSearch.SuspendLayout();
            SuspendLayout();
            // 
            // menuStripTab
            // 
            menuStripTab.ImageScalingSize = new System.Drawing.Size(20, 20);
            menuStripTab.Items.AddRange(new ToolStripItem[] { menuCloseTab, menuCloseOtherTabs });
            menuStripTab.Name = "menuStripTab";
            menuStripTab.Size = new System.Drawing.Size(198, 52);
            // 
            // menuCloseTab
            // 
            menuCloseTab.Name = "menuCloseTab";
            menuCloseTab.ShortcutKeys = Keys.Control | Keys.F4;
            menuCloseTab.Size = new System.Drawing.Size(197, 24);
            menuCloseTab.Text = "Close tab";
            menuCloseTab.Click += menuCloseTab_Click;
            // 
            // menuCloseOtherTabs
            // 
            menuCloseOtherTabs.Name = "menuCloseOtherTabs";
            menuCloseOtherTabs.Size = new System.Drawing.Size(197, 24);
            menuCloseOtherTabs.Text = "Close other tabs";
            menuCloseOtherTabs.Click += menuCloseOtherTabs_Click;
            // 
            // BtnRefresh
            // 
            BtnRefresh.BackgroundImage = (System.Drawing.Image)resources.GetObject("BtnRefresh.BackgroundImage");
            BtnRefresh.BackgroundImageLayout = ImageLayout.Zoom;
            BtnRefresh.FlatStyle = FlatStyle.Flat;
            BtnRefresh.ForeColor = System.Drawing.Color.White;
            BtnRefresh.Location = new System.Drawing.Point(114, 9);
            BtnRefresh.Margin = new Padding(3, 4, 3, 4);
            BtnRefresh.Name = "BtnRefresh";
            BtnRefresh.Size = new System.Drawing.Size(36, 34);
            BtnRefresh.TabIndex = 3;
            BtnRefresh.UseVisualStyleBackColor = true;
            BtnRefresh.Click += bRefresh_Click;
            // 
            // BtnStop
            // 
            BtnStop.BackgroundImage = (System.Drawing.Image)resources.GetObject("BtnStop.BackgroundImage");
            BtnStop.BackgroundImageLayout = ImageLayout.Zoom;
            BtnStop.FlatStyle = FlatStyle.Flat;
            BtnStop.ForeColor = System.Drawing.Color.White;
            BtnStop.Location = new System.Drawing.Point(114, 9);
            BtnStop.Margin = new Padding(3, 4, 3, 4);
            BtnStop.Name = "BtnStop";
            BtnStop.Size = new System.Drawing.Size(36, 34);
            BtnStop.TabIndex = 2;
            BtnStop.UseVisualStyleBackColor = true;
            BtnStop.Click += bStop_Click;
            // 
            // BtnForward
            // 
            BtnForward.BackgroundImage = (System.Drawing.Image)resources.GetObject("BtnForward.BackgroundImage");
            BtnForward.BackgroundImageLayout = ImageLayout.Zoom;
            BtnForward.FlatStyle = FlatStyle.Flat;
            BtnForward.ForeColor = System.Drawing.Color.White;
            BtnForward.Location = new System.Drawing.Point(66, 9);
            BtnForward.Margin = new Padding(3, 4, 3, 4);
            BtnForward.Name = "BtnForward";
            BtnForward.Size = new System.Drawing.Size(36, 34);
            BtnForward.TabIndex = 1;
            BtnForward.UseVisualStyleBackColor = true;
            BtnForward.Click += bForward_Click;
            // 
            // BtnBack
            // 
            BtnBack.BackgroundImage = (System.Drawing.Image)resources.GetObject("BtnBack.BackgroundImage");
            BtnBack.BackgroundImageLayout = ImageLayout.Zoom;
            BtnBack.FlatStyle = FlatStyle.Flat;
            BtnBack.ForeColor = System.Drawing.Color.White;
            BtnBack.Location = new System.Drawing.Point(18, 9);
            BtnBack.Margin = new Padding(3, 4, 3, 4);
            BtnBack.Name = "BtnBack";
            BtnBack.Size = new System.Drawing.Size(36, 34);
            BtnBack.TabIndex = 0;
            BtnBack.UseVisualStyleBackColor = true;
            BtnBack.Click += bBack_Click;
            // 
            // timer1
            // 
            timer1.Interval = 50;
            timer1.Tick += timer1_Tick;
            // 
            // BtnDownloads
            // 
            BtnDownloads.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BtnDownloads.BackgroundImage = (System.Drawing.Image)resources.GetObject("BtnDownloads.BackgroundImage");
            BtnDownloads.BackgroundImageLayout = ImageLayout.Zoom;
            BtnDownloads.FlatStyle = FlatStyle.Flat;
            BtnDownloads.ForeColor = System.Drawing.Color.White;
            BtnDownloads.Location = new System.Drawing.Point(741, 9);
            BtnDownloads.Margin = new Padding(3, 4, 3, 4);
            BtnDownloads.Name = "BtnDownloads";
            BtnDownloads.Size = new System.Drawing.Size(36, 34);
            BtnDownloads.TabIndex = 4;
            BtnDownloads.Tag = "Downloads";
            BtnDownloads.UseVisualStyleBackColor = true;
            BtnDownloads.Click += bDownloads_Click;
            // 
            // TxtURL
            // 
            TxtURL.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            TxtURL.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            TxtURL.Location = new System.Drawing.Point(162, 4);
            TxtURL.Margin = new Padding(3, 4, 3, 4);
            TxtURL.Name = "TxtURL";
            TxtURL.Size = new System.Drawing.Size(567, 34);
            TxtURL.TabIndex = 5;
            TxtURL.Click += TxtURL_Click;
            TxtURL.Enter += TxtURL_Enter;
            TxtURL.KeyDown += TxtURL_KeyDown;
            // 
            // PanelToolbar
            // 
            PanelToolbar.BackColor = System.Drawing.Color.FromArgb(247, 247, 247);
            PanelToolbar.Controls.Add(btn_Menu);
            PanelToolbar.Controls.Add(BtnHome);
            PanelToolbar.Controls.Add(BtnRefresh);
            PanelToolbar.Controls.Add(BtnDownloads);
            PanelToolbar.Controls.Add(BtnForward);
            PanelToolbar.Controls.Add(BtnBack);
            PanelToolbar.Controls.Add(BtnStop);
            PanelToolbar.Controls.Add(TxtURL);
            PanelToolbar.Dock = DockStyle.Top;
            PanelToolbar.Location = new System.Drawing.Point(0, 0);
            PanelToolbar.Margin = new Padding(3, 4, 3, 4);
            PanelToolbar.Name = "PanelToolbar";
            PanelToolbar.Size = new System.Drawing.Size(896, 52);
            PanelToolbar.TabIndex = 6;
            // 
            // btn_Menu
            // 
            btn_Menu.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btn_Menu.BackgroundImage = (System.Drawing.Image)resources.GetObject("btn_Menu.BackgroundImage");
            btn_Menu.BackgroundImageLayout = ImageLayout.Zoom;
            btn_Menu.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            btn_Menu.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(224, 224, 224);
            btn_Menu.FlatStyle = FlatStyle.Flat;
            btn_Menu.ForeColor = System.Drawing.Color.White;
            btn_Menu.Location = new System.Drawing.Point(837, 9);
            btn_Menu.Margin = new Padding(3, 4, 3, 4);
            btn_Menu.Name = "btn_Menu";
            btn_Menu.Size = new System.Drawing.Size(36, 34);
            btn_Menu.TabIndex = 7;
            btn_Menu.Tag = "Menu3dot";
            btn_Menu.Text = "...";
            btn_Menu.UseVisualStyleBackColor = true;
            // 
            // BtnHome
            // 
            BtnHome.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BtnHome.BackgroundImage = (System.Drawing.Image)resources.GetObject("BtnHome.BackgroundImage");
            BtnHome.BackgroundImageLayout = ImageLayout.Zoom;
            BtnHome.FlatStyle = FlatStyle.Flat;
            BtnHome.ForeColor = System.Drawing.Color.White;
            BtnHome.Location = new System.Drawing.Point(789, 9);
            BtnHome.Margin = new Padding(3, 4, 3, 4);
            BtnHome.Name = "BtnHome";
            BtnHome.Size = new System.Drawing.Size(36, 34);
            BtnHome.TabIndex = 6;
            BtnHome.Tag = "Home";
            BtnHome.UseVisualStyleBackColor = true;
            BtnHome.Click += BtnHome_Click;
            // 
            // TabPages
            // 
            TabPages.ContextMenuStrip = menuStripTab;
            TabPages.Dock = DockStyle.Fill;
            TabPages.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            TabPages.Items.AddRange(new Controls.BrowserTabStrip.BrowserTabStripItem[] { tabStrip1, tabStripAdd });
            TabPages.Location = new System.Drawing.Point(0, 52);
            TabPages.Name = "TabPages";
            TabPages.Padding = new Padding(1, 49, 1, 1);
            TabPages.SelectedItem = tabStrip1;
            TabPages.Size = new System.Drawing.Size(896, 400);
            TabPages.TabIndex = 4;
            TabPages.Text = "faTabStrip1";
            TabPages.TabStripItemSelectionChanged += OnTabsChanged;
            TabPages.TabStripItemClosed += OnTabClosed;
            TabPages.MouseClick += tabPages_MouseClick;
            // 
            // tabStrip1
            // 
            tabStrip1.Dock = DockStyle.Fill;
            tabStrip1.IsDrawn = true;
            tabStrip1.Location = new System.Drawing.Point(1, 49);
            tabStrip1.Name = "tabStrip1";
            tabStrip1.Selected = true;
            tabStrip1.Size = new System.Drawing.Size(894, 350);
            tabStrip1.TabIndex = 0;
            tabStrip1.Title = "Loading...";
            // 
            // tabStripAdd
            // 
            tabStripAdd.CanClose = false;
            tabStripAdd.Dock = DockStyle.Fill;
            tabStripAdd.IsDrawn = true;
            tabStripAdd.Location = new System.Drawing.Point(0, 0);
            tabStripAdd.Name = "tabStripAdd";
            tabStripAdd.Size = new System.Drawing.Size(931, 601);
            tabStripAdd.TabIndex = 1;
            tabStripAdd.Title = "+";
            // 
            // PanelSearch
            // 
            PanelSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            PanelSearch.BackColor = System.Drawing.Color.White;
            PanelSearch.BorderStyle = BorderStyle.FixedSingle;
            PanelSearch.Controls.Add(BtnNextSearch);
            PanelSearch.Controls.Add(BtnPrevSearch);
            PanelSearch.Controls.Add(BtnCloseSearch);
            PanelSearch.Controls.Add(TxtSearch);
            PanelSearch.Location = new System.Drawing.Point(572, 115);
            PanelSearch.Name = "PanelSearch";
            PanelSearch.Size = new System.Drawing.Size(307, 49);
            PanelSearch.TabIndex = 9;
            PanelSearch.Visible = false;
            // 
            // BtnNextSearch
            // 
            BtnNextSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BtnNextSearch.FlatStyle = FlatStyle.Flat;
            BtnNextSearch.ForeColor = System.Drawing.Color.White;
            BtnNextSearch.Image = (System.Drawing.Image)resources.GetObject("BtnNextSearch.Image");
            BtnNextSearch.Location = new System.Drawing.Point(239, 8);
            BtnNextSearch.Margin = new Padding(3, 4, 3, 4);
            BtnNextSearch.Name = "BtnNextSearch";
            BtnNextSearch.Size = new System.Drawing.Size(25, 30);
            BtnNextSearch.TabIndex = 9;
            BtnNextSearch.Tag = "Find next (Enter)";
            BtnNextSearch.UseVisualStyleBackColor = true;
            BtnNextSearch.Click += BtnNextSearch_Click;
            // 
            // BtnPrevSearch
            // 
            BtnPrevSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BtnPrevSearch.FlatStyle = FlatStyle.Flat;
            BtnPrevSearch.ForeColor = System.Drawing.Color.White;
            BtnPrevSearch.Image = (System.Drawing.Image)resources.GetObject("BtnPrevSearch.Image");
            BtnPrevSearch.Location = new System.Drawing.Point(206, 8);
            BtnPrevSearch.Margin = new Padding(3, 4, 3, 4);
            BtnPrevSearch.Name = "BtnPrevSearch";
            BtnPrevSearch.Size = new System.Drawing.Size(25, 30);
            BtnPrevSearch.TabIndex = 8;
            BtnPrevSearch.Tag = "Find previous (Shift+Enter)";
            BtnPrevSearch.UseVisualStyleBackColor = true;
            BtnPrevSearch.Click += BtnPrevSearch_Click;
            // 
            // BtnCloseSearch
            // 
            BtnCloseSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BtnCloseSearch.FlatStyle = FlatStyle.Flat;
            BtnCloseSearch.ForeColor = System.Drawing.Color.White;
            BtnCloseSearch.Image = (System.Drawing.Image)resources.GetObject("BtnCloseSearch.Image");
            BtnCloseSearch.Location = new System.Drawing.Point(272, 8);
            BtnCloseSearch.Margin = new Padding(3, 4, 3, 4);
            BtnCloseSearch.Name = "BtnCloseSearch";
            BtnCloseSearch.Size = new System.Drawing.Size(25, 30);
            BtnCloseSearch.TabIndex = 7;
            BtnCloseSearch.Tag = "Close (Esc)";
            BtnCloseSearch.UseVisualStyleBackColor = true;
            BtnCloseSearch.Click += BtnClearSearch_Click;
            // 
            // TxtSearch
            // 
            TxtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            TxtSearch.BorderStyle = BorderStyle.None;
            TxtSearch.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            TxtSearch.Location = new System.Drawing.Point(9, 8);
            TxtSearch.Margin = new Padding(3, 4, 3, 4);
            TxtSearch.Name = "TxtSearch";
            TxtSearch.Size = new System.Drawing.Size(181, 31);
            TxtSearch.TabIndex = 6;
            TxtSearch.TextChanged += TxtSearch_TextChanged;
            TxtSearch.KeyDown += TxtSearch_KeyDown;
            // 
            // PanelStatus
            // 
            PanelStatus.Dock = DockStyle.Bottom;
            PanelStatus.Location = new System.Drawing.Point(0, 452);
            PanelStatus.Name = "PanelStatus";
            PanelStatus.Size = new System.Drawing.Size(896, 20);
            PanelStatus.TabIndex = 8;
            // 
            // MainForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new System.Drawing.Size(896, 472);
            Controls.Add(PanelSearch);
            Controls.Add(TabPages);
            Controls.Add(PanelToolbar);
            Controls.Add(PanelStatus);
            Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            Margin = new Padding(4, 5, 4, 5);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Title";
            WindowState = FormWindowState.Maximized;
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            menuStripTab.ResumeLayout(false);
            PanelToolbar.ResumeLayout(false);
            PanelToolbar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)TabPages).EndInit();
            TabPages.ResumeLayout(false);
            PanelSearch.ResumeLayout(false);
            PanelSearch.PerformLayout();
            ResumeLayout(false);
        }


        #endregion

        private SharpBrowser.Controls.BrowserTabStrip.BrowserTabStrip TabPages;
        private SharpBrowser.Controls.BrowserTabStrip.BrowserTabStripItem tabStrip1;
        private SharpBrowser.Controls.BrowserTabStrip.BrowserTabStripItem tabStripAdd;
		private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ContextMenuStrip menuStripTab;
        private System.Windows.Forms.ToolStripMenuItem menuCloseTab;
        private System.Windows.Forms.ToolStripMenuItem menuCloseOtherTabs;
		private System.Windows.Forms.Button BtnForward;
		private System.Windows.Forms.Button BtnBack;
		private System.Windows.Forms.Button BtnStop;
		private System.Windows.Forms.Button BtnRefresh;
		private System.Windows.Forms.Button BtnDownloads;
		private System.Windows.Forms.TextBox TxtURL;
		private System.Windows.Forms.Panel PanelToolbar;
		private System.Windows.Forms.Panel PanelStatus;
		private System.Windows.Forms.Panel PanelSearch;
		private System.Windows.Forms.TextBox TxtSearch;
		private System.Windows.Forms.Button BtnCloseSearch;
		private System.Windows.Forms.Button BtnPrevSearch;
		private System.Windows.Forms.Button BtnNextSearch;
        private System.Windows.Forms.Button BtnHome;
        private System.Windows.Forms.Button btn_Menu;
    }
}

