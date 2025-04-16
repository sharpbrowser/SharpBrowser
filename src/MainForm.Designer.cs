using FontAwesome.Sharp;
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
		private void InitializeComponent() {
			components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			TabMenu = new ContextMenuStrip(components);
			TMReload = new IconMenuItem();
			TMClose = new IconMenuItem();
			TMCloseOther = new IconMenuItem();
			BtnRefresh = new IconButton();
			BtnStop = new IconButton();
			BtnForward = new IconButton();
			BtnBack = new IconButton();
			BtnDownloads = new IconButton();
			TxtURL = new TextBox();
			PanelToolbar = new Panel();
			BtnMenu = new IconButton();
			BtnHome = new IconButton();
			TabPages = new SharpBrowser.Controls.BrowserTabStrip.BrowserTabStrip();
			tabStrip1 = new SharpBrowser.Controls.BrowserTabStrip.BrowserTabPage();
			PanelSearch = new Panel();
			BtnNextSearch = new Button();
			BtnPrevSearch = new Button();
			BtnCloseSearch = new Button();
			TxtSearch = new TextBox();
			MainMenu = new ContextMenuStrip(components);
			MMNewTab = new IconMenuItem();
			MMNextTab = new IconMenuItem();
			MMPrevTab = new IconMenuItem();
			toolStripSeparator1 = new ToolStripSeparator();
			MMPrint = new IconMenuItem();
			MMPrintPDF = new IconMenuItem();
			MMDownloads = new IconMenuItem();
			MMFullscreen = new IconMenuItem();
			MMDevTools = new IconMenuItem();
			toolStripSeparator2 = new ToolStripSeparator();
			MMClose = new IconMenuItem();
			MMCloseOther = new IconMenuItem();
			TabMenu.SuspendLayout();
			PanelToolbar.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)TabPages).BeginInit();
			TabPages.SuspendLayout();
			PanelSearch.SuspendLayout();
			MainMenu.SuspendLayout();
			SuspendLayout();
			// 
			// TabMenu
			// 
			TabMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
			TabMenu.Items.AddRange(new ToolStripItem[] { TMReload, TMClose, TMCloseOther });
			TabMenu.Name = "menuStripTab";
			TabMenu.Size = new System.Drawing.Size(173, 82);
			// 
			// TMReload
			// 
			TMReload.IconChar = IconChar.Refresh;
			TMReload.IconColor = System.Drawing.Color.Black;
			TMReload.IconFont = IconFont.Auto;
			TMReload.Name = "TMReload";
			TMReload.Size = new System.Drawing.Size(172, 26);
			TMReload.Text = "Reload tab";
			TMReload.Click += TMReload_Click;
			// 
			// TMClose
			// 
			TMClose.IconChar = IconChar.Close;
			TMClose.IconColor = System.Drawing.Color.Black;
			TMClose.IconFont = IconFont.Auto;
			TMClose.Name = "TMClose";
			TMClose.ShortcutKeyDisplayString = "Ctrl+W";
			TMClose.Size = new System.Drawing.Size(172, 26);
			TMClose.Text = "Close tab";
			TMClose.Click += TMCloseTab_Click;
			// 
			// TMCloseOther
			// 
			TMCloseOther.IconChar = IconChar.Eraser;
			TMCloseOther.IconColor = System.Drawing.Color.Black;
			TMCloseOther.IconFont = IconFont.Auto;
			TMCloseOther.Name = "TMCloseOther";
			TMCloseOther.Size = new System.Drawing.Size(172, 26);
			TMCloseOther.Text = "Close other tabs";
			TMCloseOther.Click += TMCloseOtherTabs_Click;
			// 
			// BtnRefresh
			// 
			BtnRefresh.BackgroundImageLayout = ImageLayout.Zoom;
			BtnRefresh.FlatAppearance.BorderSize = 0;
			BtnRefresh.FlatStyle = FlatStyle.Flat;
			BtnRefresh.ForeColor = System.Drawing.Color.White;
			BtnRefresh.IconChar = IconChar.Refresh;
			BtnRefresh.IconColor = System.Drawing.Color.Black;
			BtnRefresh.IconFont = IconFont.Auto;
			BtnRefresh.IconSize = 30;
			BtnRefresh.Location = new System.Drawing.Point(85, 5);
			BtnRefresh.Margin = new Padding(3, 4, 3, 4);
			BtnRefresh.Name = "BtnRefresh";
			BtnRefresh.Size = new System.Drawing.Size(36, 34);
			BtnRefresh.TabIndex = 3;
			BtnRefresh.UseVisualStyleBackColor = true;
			BtnRefresh.Click += bRefresh_Click;
			// 
			// BtnStop
			// 
			BtnStop.BackgroundImageLayout = ImageLayout.Zoom;
			BtnStop.FlatStyle = FlatStyle.Flat;
			BtnStop.ForeColor = System.Drawing.Color.White;
			BtnStop.IconChar = IconChar.Cancel;
			BtnStop.IconColor = System.Drawing.Color.Black;
			BtnStop.IconFont = IconFont.Auto;
			BtnStop.IconSize = 30;
			BtnStop.Location = new System.Drawing.Point(85, 5);
			BtnStop.Margin = new Padding(3, 4, 3, 4);
			BtnStop.Name = "BtnStop";
			BtnStop.Size = new System.Drawing.Size(36, 34);
			BtnStop.TabIndex = 2;
			BtnStop.UseVisualStyleBackColor = true;
			BtnStop.Click += bStop_Click;
			// 
			// BtnForward
			// 
			BtnForward.BackgroundImageLayout = ImageLayout.Zoom;
			BtnForward.FlatAppearance.BorderSize = 0;
			BtnForward.FlatStyle = FlatStyle.Flat;
			BtnForward.ForeColor = System.Drawing.Color.White;
			BtnForward.IconChar = IconChar.ArrowRight;
			BtnForward.IconColor = System.Drawing.Color.Black;
			BtnForward.IconFont = IconFont.Auto;
			BtnForward.IconSize = 30;
			BtnForward.Location = new System.Drawing.Point(45, 5);
			BtnForward.Margin = new Padding(3, 4, 3, 4);
			BtnForward.Name = "BtnForward";
			BtnForward.Size = new System.Drawing.Size(36, 34);
			BtnForward.TabIndex = 1;
			BtnForward.UseVisualStyleBackColor = true;
			BtnForward.Click += bForward_Click;
			// 
			// BtnBack
			// 
			BtnBack.BackgroundImageLayout = ImageLayout.Zoom;
			BtnBack.FlatAppearance.BorderSize = 0;
			BtnBack.FlatStyle = FlatStyle.Flat;
			BtnBack.ForeColor = System.Drawing.Color.White;
			BtnBack.IconChar = IconChar.ArrowLeft;
			BtnBack.IconColor = System.Drawing.Color.Black;
			BtnBack.IconFont = IconFont.Auto;
			BtnBack.IconSize = 30;
			BtnBack.Location = new System.Drawing.Point(5, 5);
			BtnBack.Margin = new Padding(3, 4, 3, 4);
			BtnBack.Name = "BtnBack";
			BtnBack.Size = new System.Drawing.Size(36, 34);
			BtnBack.TabIndex = 0;
			BtnBack.UseVisualStyleBackColor = true;
			BtnBack.Click += bBack_Click;
			// 
			// BtnDownloads
			// 
			BtnDownloads.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			BtnDownloads.BackgroundImageLayout = ImageLayout.Zoom;
			BtnDownloads.FlatAppearance.BorderSize = 0;
			BtnDownloads.FlatStyle = FlatStyle.Flat;
			BtnDownloads.ForeColor = System.Drawing.Color.White;
			BtnDownloads.IconChar = IconChar.Download;
			BtnDownloads.IconColor = System.Drawing.Color.Black;
			BtnDownloads.IconFont = IconFont.Auto;
			BtnDownloads.IconSize = 30;
			BtnDownloads.Location = new System.Drawing.Point(794, 5);
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
			TxtURL.Location = new System.Drawing.Point(132, 4);
			TxtURL.Margin = new Padding(3, 4, 3, 4);
			TxtURL.Name = "TxtURL";
			TxtURL.Size = new System.Drawing.Size(654, 29);
			TxtURL.TabIndex = 5;
			TxtURL.Click += TxtURL_Click;
			TxtURL.Enter += TxtURL_Enter;
			TxtURL.KeyDown += TxtURL_KeyDown;
			// 
			// PanelToolbar
			// 
			PanelToolbar.BackColor = System.Drawing.Color.FromArgb(247, 247, 247);
			PanelToolbar.Controls.Add(BtnMenu);
			PanelToolbar.Controls.Add(BtnHome);
			PanelToolbar.Controls.Add(BtnDownloads);
			PanelToolbar.Controls.Add(BtnForward);
			PanelToolbar.Controls.Add(BtnBack);
			PanelToolbar.Controls.Add(TxtURL);
			PanelToolbar.Controls.Add(BtnRefresh);
			PanelToolbar.Controls.Add(BtnStop);
			PanelToolbar.Dock = DockStyle.Top;
			PanelToolbar.Location = new System.Drawing.Point(0, 0);
			PanelToolbar.Margin = new Padding(3, 4, 3, 4);
			PanelToolbar.Name = "PanelToolbar";
			PanelToolbar.Size = new System.Drawing.Size(916, 45);
			PanelToolbar.TabIndex = 6;
			// 
			// BtnMenu
			// 
			BtnMenu.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			BtnMenu.BackgroundImageLayout = ImageLayout.Zoom;
			BtnMenu.FlatAppearance.BorderSize = 0;
			BtnMenu.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
			BtnMenu.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(224, 224, 224);
			BtnMenu.FlatStyle = FlatStyle.Flat;
			BtnMenu.ForeColor = System.Drawing.Color.White;
			BtnMenu.IconChar = IconChar.Bars;
			BtnMenu.IconColor = System.Drawing.Color.Black;
			BtnMenu.IconFont = IconFont.Auto;
			BtnMenu.IconSize = 30;
			BtnMenu.Location = new System.Drawing.Point(874, 5);
			BtnMenu.Margin = new Padding(3, 4, 3, 4);
			BtnMenu.Name = "BtnMenu";
			BtnMenu.Size = new System.Drawing.Size(36, 34);
			BtnMenu.TabIndex = 7;
			BtnMenu.Tag = "Menu3dot";
			BtnMenu.Text = "";
			BtnMenu.UseVisualStyleBackColor = true;
			BtnMenu.Click += BtnMenu_Click;
			// 
			// BtnHome
			// 
			BtnHome.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			BtnHome.BackgroundImageLayout = ImageLayout.Zoom;
			BtnHome.FlatAppearance.BorderSize = 0;
			BtnHome.FlatStyle = FlatStyle.Flat;
			BtnHome.ForeColor = System.Drawing.Color.White;
			BtnHome.IconChar = IconChar.House;
			BtnHome.IconColor = System.Drawing.Color.Black;
			BtnHome.IconFont = IconFont.Auto;
			BtnHome.IconSize = 30;
			BtnHome.Location = new System.Drawing.Point(834, 5);
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
			TabPages.ContextMenuStrip = TabMenu;
			TabPages.Dock = DockStyle.Fill;
			TabPages.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			TabPages.Items.AddRange(new Controls.BrowserTabStrip.BrowserTabPage[] { tabStrip1 });
			TabPages.Location = new System.Drawing.Point(0, 45);
			TabPages.Name = "TabPages";
			TabPages.Padding = new Padding(1, 41, 1, 1);
			TabPages.SelectedIndex = 0;
			TabPages.SelectedTab = tabStrip1;
			TabPages.Size = new System.Drawing.Size(916, 427);
			TabPages.TabIndex = 4;
			TabPages.Text = "faTabStrip1";
			TabPages.TabStripItemSelectionChanged += OnTabsChanged;
			TabPages.TabStripItemClosed += OnTabClosed;
			TabPages.TabStripNewTab += OnNewTab;
			// 
			// tabStrip1
			// 
			tabStrip1.Dock = DockStyle.Fill;
			tabStrip1.IsDrawn = true;
			tabStrip1.Location = new System.Drawing.Point(1, 41);
			tabStrip1.Name = "tabStrip1";
			tabStrip1.Selected = true;
			tabStrip1.Size = new System.Drawing.Size(914, 385);
			tabStrip1.TabIndex = 0;
			tabStrip1.Title = "Loading...";
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
			PanelSearch.Location = new System.Drawing.Point(592, 115);
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
			TxtSearch.Size = new System.Drawing.Size(181, 25);
			TxtSearch.TabIndex = 6;
			TxtSearch.TextChanged += TxtSearch_TextChanged;
			TxtSearch.KeyDown += TxtSearch_KeyDown;
			// 
			// MainMenu
			// 
			MainMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
			MainMenu.Items.AddRange(new ToolStripItem[] { MMNewTab, MMNextTab, MMPrevTab, toolStripSeparator1, MMPrint, MMPrintPDF, MMDownloads, MMFullscreen, MMDevTools, toolStripSeparator2, MMClose, MMCloseOther });
			MainMenu.Name = "menuStripTab";
			MainMenu.Size = new System.Drawing.Size(228, 276);
			// 
			// MMNewTab
			// 
			MMNewTab.IconChar = IconChar.Add;
			MMNewTab.IconColor = System.Drawing.Color.Black;
			MMNewTab.IconFont = IconFont.Auto;
			MMNewTab.Name = "MMNewTab";
			MMNewTab.ShortcutKeyDisplayString = "Ctrl+T";
			MMNewTab.Size = new System.Drawing.Size(227, 26);
			MMNewTab.Text = "New tab";
			MMNewTab.Click += MMNewTab_Click;
			// 
			// MMNextTab
			// 
			MMNextTab.IconChar = IconChar.ArrowRight;
			MMNextTab.IconColor = System.Drawing.Color.Black;
			MMNextTab.IconFont = IconFont.Auto;
			MMNextTab.Name = "MMNextTab";
			MMNextTab.ShortcutKeyDisplayString = "Ctrl+Tab";
			MMNextTab.Size = new System.Drawing.Size(227, 26);
			MMNextTab.Text = "Next tab";
			MMNextTab.Click += MMNextTab_Click;
			// 
			// MMPrevTab
			// 
			MMPrevTab.IconChar = IconChar.ArrowLeft;
			MMPrevTab.IconColor = System.Drawing.Color.Black;
			MMPrevTab.IconFont = IconFont.Auto;
			MMPrevTab.Name = "MMPrevTab";
			MMPrevTab.ShortcutKeyDisplayString = "Ctrl+Shift+Tab";
			MMPrevTab.Size = new System.Drawing.Size(227, 26);
			MMPrevTab.Text = "Previous tab";
			MMPrevTab.Click += MMPrevTab_Click;
			// 
			// toolStripSeparator1
			// 
			toolStripSeparator1.Name = "toolStripSeparator1";
			toolStripSeparator1.Size = new System.Drawing.Size(224, 6);
			// 
			// MMPrint
			// 
			MMPrint.IconChar = IconChar.Print;
			MMPrint.IconColor = System.Drawing.Color.Black;
			MMPrint.IconFont = IconFont.Auto;
			MMPrint.Name = "MMPrint";
			MMPrint.ShortcutKeyDisplayString = "Ctrl+P";
			MMPrint.Size = new System.Drawing.Size(227, 26);
			MMPrint.Text = "Print...";
			MMPrint.Click += MMPrint_Click;
			// 
			// MMPrintPDF
			// 
			MMPrintPDF.IconChar = IconChar.FilePdf;
			MMPrintPDF.IconColor = System.Drawing.Color.Black;
			MMPrintPDF.IconFont = IconFont.Auto;
			MMPrintPDF.Name = "MMPrintPDF";
			MMPrintPDF.ShortcutKeyDisplayString = "Ctrl+Shift+P";
			MMPrintPDF.Size = new System.Drawing.Size(227, 26);
			MMPrintPDF.Text = "Print to PDF...";
			MMPrintPDF.Click += MMPrintPDF_Click;
			// 
			// MMDownloads
			// 
			MMDownloads.IconChar = IconChar.Download;
			MMDownloads.IconColor = System.Drawing.Color.Black;
			MMDownloads.IconFont = IconFont.Auto;
			MMDownloads.Name = "MMDownloads";
			MMDownloads.Size = new System.Drawing.Size(227, 26);
			MMDownloads.Text = "Downloads...";
			MMDownloads.Click += MMDownloads_Click;
			// 
			// MMFullscreen
			// 
			MMFullscreen.IconChar = IconChar.Expand;
			MMFullscreen.IconColor = System.Drawing.Color.Black;
			MMFullscreen.IconFont = IconFont.Auto;
			MMFullscreen.Name = "MMFullscreen";
			MMFullscreen.ShortcutKeyDisplayString = "F11";
			MMFullscreen.Size = new System.Drawing.Size(227, 26);
			MMFullscreen.Text = "Fullscreen";
			MMFullscreen.Click += MMFullscreen_Click;
			// 
			// MMDevTools
			// 
			MMDevTools.IconChar = IconChar.Code;
			MMDevTools.IconColor = System.Drawing.Color.Black;
			MMDevTools.IconFont = IconFont.Auto;
			MMDevTools.Name = "MMDevTools";
			MMDevTools.ShortcutKeyDisplayString = "F12";
			MMDevTools.Size = new System.Drawing.Size(227, 26);
			MMDevTools.Text = "Developer tools...";
			MMDevTools.Click += MMDevTools_Click;
			// 
			// toolStripSeparator2
			// 
			toolStripSeparator2.Name = "toolStripSeparator2";
			toolStripSeparator2.Size = new System.Drawing.Size(224, 6);
			// 
			// MMClose
			// 
			MMClose.IconChar = IconChar.Close;
			MMClose.IconColor = System.Drawing.Color.Black;
			MMClose.IconFont = IconFont.Auto;
			MMClose.Name = "MMClose";
			MMClose.ShortcutKeyDisplayString = "Ctrl+W";
			MMClose.Size = new System.Drawing.Size(227, 26);
			MMClose.Text = "Close tab";
			MMClose.Click += MMClose_Click;
			// 
			// MMCloseOther
			// 
			MMCloseOther.IconChar = IconChar.Eraser;
			MMCloseOther.IconColor = System.Drawing.Color.Black;
			MMCloseOther.IconFont = IconFont.Auto;
			MMCloseOther.Name = "MMCloseOther";
			MMCloseOther.Size = new System.Drawing.Size(227, 26);
			MMCloseOther.Text = "Close other tabs";
			MMCloseOther.Click += MMCloseOther_Click;
			// 
			// MainForm
			// 
			AutoScaleMode = AutoScaleMode.None;
			ClientSize = new System.Drawing.Size(916, 472);
			Controls.Add(PanelSearch);
			Controls.Add(TabPages);
			Controls.Add(PanelToolbar);
			Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			Margin = new Padding(4, 5, 4, 5);
			Name = "MainForm";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "Title";
			WindowState = FormWindowState.Maximized;
			FormClosing += MainForm_FormClosing;
			Load += MainForm_Load;
			TabMenu.ResumeLayout(false);
			PanelToolbar.ResumeLayout(false);
			PanelToolbar.PerformLayout();
			((System.ComponentModel.ISupportInitialize)TabPages).EndInit();
			TabPages.ResumeLayout(false);
			PanelSearch.ResumeLayout(false);
			PanelSearch.PerformLayout();
			MainMenu.ResumeLayout(false);
			ResumeLayout(false);
		}


		#endregion

		private SharpBrowser.Controls.BrowserTabStrip.BrowserTabStrip TabPages;
        private SharpBrowser.Controls.BrowserTabStrip.BrowserTabPage tabStrip1;
        private System.Windows.Forms.ContextMenuStrip TabMenu;
		private FontAwesome.Sharp.IconButton BtnForward;
		private FontAwesome.Sharp.IconButton BtnBack;
		private FontAwesome.Sharp.IconButton BtnStop;
		private FontAwesome.Sharp.IconButton BtnRefresh;
		private FontAwesome.Sharp.IconButton BtnDownloads;
		private System.Windows.Forms.TextBox TxtURL;
		private System.Windows.Forms.Panel PanelToolbar;
		private System.Windows.Forms.Panel PanelSearch;
		private System.Windows.Forms.TextBox TxtSearch;
		private System.Windows.Forms.Button BtnCloseSearch;
		private System.Windows.Forms.Button BtnPrevSearch;
		private System.Windows.Forms.Button BtnNextSearch;
        private FontAwesome.Sharp.IconButton BtnHome;
        private FontAwesome.Sharp.IconButton BtnMenu;
		private ContextMenuStrip MainMenu;
		private IconMenuItem MMClose;
		private IconMenuItem MMCloseOther;
		private IconMenuItem MMNewTab;
		private ToolStripSeparator toolStripSeparator1;
		private IconMenuItem TMReload;
        private IconMenuItem TMClose;
        private IconMenuItem TMCloseOther;
		private IconMenuItem MMDownloads;
		private IconMenuItem MMNextTab;
		private IconMenuItem MMPrevTab;
		private ToolStripSeparator toolStripSeparator2;
		private IconMenuItem MMPrint;
		private IconMenuItem MMPrintPDF;
		private IconMenuItem MMDevTools;
		private IconMenuItem MMFullscreen;
	}
}

