using SharpBrowser.Controls.BrowserTabStrip.Buttons;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;

namespace SharpBrowser.Controls.BrowserTabStrip {
	[DefaultEvent("TabStripItemSelectionChanged")]
	[DefaultProperty("Items")]
	[ToolboxItem(true)]
	public class BrowserTabStrip : BaseStyledPanel, ISupportInitialize, IDisposable {

		public int TabButton_Height => BrowserTabStyle.TabHeight;

		private BrowserTabItem selectedItem;
		private ContextMenuStrip menu;
		private TabCloseButton closeButton;
		private TabNewButton newTabButton;
		private BrowserTabStripItemCollection items;

		private StringFormat sf;

		private bool isIniting;
		public int MaxTabSize = 200;
		public int AddButtonWidth = 40;

		[RefreshProperties(RefreshProperties.All)]
		[DefaultValue(null)]
		public BrowserTabItem SelectedItem {
			get {
				return selectedItem;
			}
			set {
				if (selectedItem == value) {
					return;
				}
				if (value == null && Items.Count > 0) {
					/*BrowserTabItem fATabStripItem = Items[0];
					if (fATabStripItem.Visible) {
						selectedItem = fATabStripItem;
						selectedItem.Selected = true;
						selectedItem.Dock = DockStyle.Fill;
					}*/
					selectedItem = null;
					return;
				}
				else {
					selectedItem = value;
				}

				foreach (BrowserTabItem item in Items) {
					if (item == selectedItem) {
						SelectItem(item);
						//item.Dock = DockStyle.Fill;
						item.Show();
					}
					else {
						UnSelectItem(item);
						item.Hide();
					}
				}
				Invalidate();
				Refresh();

				/*SelectItem(selectedItem);
				Invalidate();
				if (!selectedItem.IsDrawn) {
					Items.MoveTo(0, selectedItem);
					Invalidate();
				}*/
				OnTabStripItemChanged(new TabStripItemChangedEventArgs(selectedItem, BrowserTabStripItemChangeTypes.SelectionChanged));
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public BrowserTabStripItemCollection Items => items;

		[DefaultValue(typeof(Size), "350,200")]
		public new Size Size {
			get {
				return base.Size;
			}
			set {
				if (!(base.Size == value)) {
					base.Size = value;
					UpdateLayout();
				}
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new ControlCollection Controls => base.Controls;

		public event TabStripItemClosingHandler TabStripItemClosing;
		public event TabStripItemChangedHandler TabStripItemSelectionChanged;
		public event HandledEventHandler MenuItemsLoading;
		public event EventHandler MenuItemsLoaded;
		public event EventHandler TabStripItemClosed;
		public event EventHandler TabStripNewTab;

		public BrowserTabStrip() {
			BeginInit();
			SetStyle(ControlStyles.ContainerControl, value: true);
			SetStyle(ControlStyles.UserPaint, value: true);
			SetStyle(ControlStyles.ResizeRedraw, value: true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, value: true);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, value: true);
			SetStyle(ControlStyles.Selectable, value: true);
			items = new BrowserTabStripItemCollection();
			items.CollectionChanged += OnCollectionChanged;
			base.Size = new Size(350, 200);
			menu = new ContextMenuStrip();
			menu.Renderer = base.ToolStripRenderer;
			menu.ItemClicked += OnMenuItemClicked;
			menu.VisibleChanged += OnMenuVisibleChanged;
			closeButton = new TabCloseButton(base.ToolStripRenderer);
			newTabButton = new TabNewButton(base.ToolStripRenderer);
			sf = new StringFormat();
			EndInit();
			UpdateLayout();
		}

		public HitTestResult HitTest(Point pt) {
			if (newTabButton.IsVisible && newTabButton.Rect.Contains(pt)) {
				return HitTestResult.NewButton;
			}
			if (closeButton.IsVisible && closeButton.Rect.Contains(pt)) {
				return HitTestResult.CloseButton;
			}
			if (GetTabItemByPoint(pt) != null) {
				return HitTestResult.TabItem;
			}
			return HitTestResult.None;
		}

		public void AddTab(BrowserTabItem tabItem) => AddTab(tabItem, autoSelect: false);

		public void AddTab(BrowserTabItem tabItem, bool autoSelect) {

			// add this tab to my collection
			tabItem.Dock = DockStyle.Fill;
			Items.Add(tabItem);

			// select the new tab
			if ((autoSelect && tabItem.Visible) || (tabItem.Visible && Items.DrawnCount < 1)) {
				SelectedItem = tabItem;
				SelectItem(tabItem);
			}
		}

		public void RemoveTab(BrowserTabItem tabItem) {

			// if tab is found in my collection
			int num = Items.IndexOf(tabItem);
			if (num >= 0) {

				// remove the tab
				UnSelectItem(tabItem);
				Items.Remove(tabItem);

				// select last item
				if (Items.Count > 0) {
					if (Items[num] != null) {
						SelectedItem = Items[num];
					}
					else {
						SelectedItem = Items[items.Count - 1];
					}
				}
			}
		}

		public BrowserTabItem GetTabItemByPoint(Point pt) {
			BrowserTabItem result = null;
			bool flag = false;
			for (int i = 0; i < Items.Count; i++) {
				BrowserTabItem fATabStripItem = Items[i];
				if (fATabStripItem.StripRect.Contains(pt) && fATabStripItem.Visible && fATabStripItem.IsDrawn) {
					result = fATabStripItem;
					flag = true;
				}
				if (flag) {
					break;
				}
			}
			return result;
		}

		public virtual void ShowMenu() { }

		internal void UnDrawAll() {
			for (int i = 0; i < Items.Count; i++) {
				Items[i].IsDrawn = false;
			}
		}

		internal void SelectItem(BrowserTabItem tabItem) {
			//tabItem.Dock = DockStyle.Fill;
			tabItem.Visible = true;
			tabItem.Selected = true;
		}

		internal void UnSelectItem(BrowserTabItem tabItem) => tabItem.Selected = false;

		protected internal virtual void OnTabStripItemClosing(TabStripItemClosingEventArgs e) => this.TabStripItemClosing?.Invoke(e);

		protected internal virtual void OnTabStripItemClosed(EventArgs e) {
			this.TabStripItemClosed?.Invoke(this, e);
		}
		protected internal virtual void OnTabStripNewTab(EventArgs e) {
			this.TabStripNewTab?.Invoke(this, e);
		}


		protected virtual void OnMenuItemsLoading(HandledEventArgs e) => this.MenuItemsLoading?.Invoke(this, e);

		protected virtual void OnMenuItemsLoaded(EventArgs e) {
			if (this.MenuItemsLoaded != null) {
				this.MenuItemsLoaded(this, e);
			}
		}

		protected virtual void OnTabStripItemChanged(TabStripItemChangedEventArgs e) {
			this.TabStripItemSelectionChanged?.Invoke(e);
		}

		protected virtual void OnMenuItemsLoad(EventArgs e) {
			menu.RightToLeft = RightToLeft;
			menu.Items.Clear();
			for (int i = 0; i < Items.Count; i++) {
				BrowserTabItem fATabStripItem = Items[i];
				if (fATabStripItem.Visible) {
					ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(fATabStripItem.Title);
					toolStripMenuItem.Tag = fATabStripItem;
					toolStripMenuItem.Image = fATabStripItem.Image;
					menu.Items.Add(toolStripMenuItem);
				}
			}
			OnMenuItemsLoaded(EventArgs.Empty);
		}

		protected override void OnRightToLeftChanged(EventArgs e) {
			base.OnRightToLeftChanged(e);
			UpdateLayout();
			Invalidate();
		}

		protected override void OnMouseDown(MouseEventArgs e) {
			base.OnMouseDown(e);
			HitTestResult hitTestResult = HitTest(e.Location);

			if (hitTestResult == HitTestResult.TabItem) {

				// select the tab if clicked
				BrowserTabItem tabItemByPoint = GetTabItemByPoint(e.Location);
				if (tabItemByPoint != null) {
					SelectedItem = tabItemByPoint;
					Invalidate();
				}

				// close if middle clicked on a tab
				if (e.Button == MouseButtons.Middle) {
					CloseActiveTab();
				}

			}
			else if (hitTestResult == HitTestResult.CloseButton) {
				if (e.Button == MouseButtons.Left) {
					CloseActiveTab();
				}
			}
			else if (hitTestResult == HitTestResult.NewButton) {
				if (e.Button == MouseButtons.Left) {
					OnTabStripNewTab(EventArgs.Empty);
				}
			}
		}

		private void CloseActiveTab() {
			if (SelectedItem != null) {
				TabStripItemClosingEventArgs tabStripItemClosingEventArgs = new TabStripItemClosingEventArgs(SelectedItem);
				OnTabStripItemClosing(tabStripItemClosingEventArgs);
				if (!tabStripItemClosingEventArgs.Cancel && SelectedItem.CanClose) {
					RemoveTab(SelectedItem);
					OnTabStripItemClosed(EventArgs.Empty);
				}
			}
			Invalidate();
		}

		protected override void OnMouseMove(MouseEventArgs e) {
			base.OnMouseMove(e);

			// manually process events for buttons
			closeButton.ProcessRolloverEvents(this, e);
			newTabButton.ProcessRolloverEvents(this, e);

		}
		

		protected override void OnMouseLeave(EventArgs e) {
			base.OnMouseLeave(e);

			// manually process events for buttons
			closeButton.ProcessRolloutEvents(this);
			newTabButton.ProcessRolloutEvents(this);

		}

		protected override void OnSizeChanged(EventArgs e) {
			base.OnSizeChanged(e);
			if (!isIniting) {
				UpdateLayout();
			}
		}

		private void SetDefaultSelected() {
			if (selectedItem == null && Items.Count > 0) {
				SelectedItem = Items[0];
			}
			for (int i = 0; i < Items.Count; i++) {
				BrowserTabItem fATabStripItem = Items[i];
				fATabStripItem.Dock = DockStyle.Fill;
			}
		}

		private void OnMenuItemClicked(object sender, ToolStripItemClickedEventArgs e) {
			BrowserTabItem fATabStripItem2 = (SelectedItem = (BrowserTabItem)e.ClickedItem.Tag);
		}

		private void OnMenuVisibleChanged(object sender, EventArgs e) {
			if (!menu.Visible) {
			}
		}

		protected override void OnPaint(PaintEventArgs e) {
			//SetDefaultSelected();
			Rectangle clientRectangle = base.ClientRectangle;
			clientRectangle.Width--;
			clientRectangle.Height--;
			TabStartX = BrowserTabStyle.TabLeftPadding;
			e.Graphics.DrawRectangle(SystemPens.ControlDark, clientRectangle);
			e.Graphics.FillRectangle(Brushes.White, clientRectangle);
			e.Graphics.FillRectangle(BrowserTabStyle.BackColor, new Rectangle(clientRectangle.X, clientRectangle.Y, clientRectangle.Width, BrowserTabStyle.TabHeight));
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;


			//--------------------------------------------------------
			// DRAW ALL TABS
			for (int i = 0; i < Items.Count; i++) {
				BrowserTabItem fATabStripItem = Items[i];
				if (fATabStripItem.Visible || base.DesignMode) {
					OnCalcTabPage(e.Graphics, fATabStripItem);
					fATabStripItem.IsDrawn = false;
					OnDrawTabButton(e.Graphics, fATabStripItem);
				}
			}

			//--------------------------------------------------------

			if (Items.DrawnCount == 0 || Items.VisibleCount == 0) {
				e.Graphics.DrawLine(Pens.Red, new Point(0, BrowserTabStyle.TabHeight), new Point(base.ClientRectangle.Width, BrowserTabStyle.TabHeight));
			}
			else if (SelectedItem != null && SelectedItem.IsDrawn) {
				var lineColorPen = new Pen(BrowserTabStyle.NormalTabBackColor);

				Point point = new Point((int)SelectedItem.StripRect.Left - TabRadius, BrowserTabStyle.TabHeight);
				e.Graphics.DrawLine(lineColorPen, new Point(0, BrowserTabStyle.TabHeight), point);
				point.X += (int)SelectedItem.StripRect.Width + TabRadius + 2;
				e.Graphics.DrawLine(lineColorPen, point, new Point(base.ClientRectangle.Width, BrowserTabStyle.TabHeight));
			}

			//--------------------------------------------------------
			// DRAW CLOSE BUTTON FOR SELECTED TAB
			if (SelectedItem != null && SelectedItem.CanClose) {
				closeButton.IsVisible = true;
				closeButton.CalcBounds(selectedItem, true);
				closeButton.Draw(e.Graphics);
			}
			else {
				closeButton.IsVisible = false;
			}

			//--------------------------------------------------------
			// DRAW NEW BUTTON
			newTabButton.IsVisible = true;
			newTabButton.CalcBounds(Items[items.Count-1], false);
			newTabButton.Draw(e.Graphics);
			//--------------------------------------------------------

		}

		/// <summary>
		/// ready for future use, (_ [] X) , if we put Tabs and Close/Minimize buttons on TitleBar 
		/// </summary>
		int atRight_ReservedWidth = 250;
		private int TabStartX = BrowserTabStyle.TabLeftPadding;
		private void OnCalcTabPage(Graphics g, BrowserTabItem currentItem) {
			//_ = Font;
			int calcWidth = 0;
			calcWidth = (base.Width - atRight_ReservedWidth - (AddButtonWidth + 20)) / Math.Max(1, (items.Count - 1));
			if (calcWidth > MaxTabSize) {
				calcWidth = MaxTabSize;
			}
			RectangleF rectangleF2 = (currentItem.StripRect = new RectangleF(TabStartX, 3f, calcWidth, BrowserTabStyle.TabHeight));
			TabStartX += calcWidth;
		}



		bool styleNotPill = true;
		int TabRadius = 8;


		/// <summary>
		/// Draws The Tab Header button
		/// </summary>
		private void OnDrawTabButton(Graphics g, BrowserTabItem currentItem) {
			Items.IndexOf(currentItem);
			Font font = Font;

			bool isActiveTab = currentItem == SelectedItem;
			bool is_atRightof_ActiveTab = Items.IndexOf(currentItem) == Items.IndexOf(selectedItem) + 1;

			RectangleF stripRect = currentItem.StripRect;
			var sr = stripRect;
			SolidBrush brush = new SolidBrush((currentItem == SelectedItem) ? BrowserTabStyle.SelectedTabBackColor : BrowserTabStyle.NormalTabBackColor);
			Pen pen = new Pen((currentItem == SelectedItem) ? BrowserTabStyle.SelectedTabBackColor : BrowserTabStyle.NormalTabBackColor);

			//--------------------------------------------------------
			// Calc Rect for the Tab
			//--------------------------------------------------------
			var tabDrawnRect = new RectangleF(sr.Left, 1, sr.Width - 2, sr.Height - 2);
			//tabDrawnRect.Height += 2; // hides bottom Line of Rect.
			tabDrawnRect.Y += 8;
			tabDrawnRect.Height -= 8;
			if (styleNotPill)
				tabDrawnRect.Height += 2;
			//--------------------------------------------------------

			////--Draw Rectange Tabs
			//g.FillRectangle(brush, tabDrawnRect);
			//g.DrawRectangle(SystemPens.ControlDark, tabDrawnRect);

			////--Draw Pill Style Tabs
			//g.FillRoundRectangle(brush, tabDrawnRect,TabRadius);
			//g.DrawRoundRectangle(SystemPens.ControlDark, tabDrawnRect, TabRadius);

			//--------------------------------------------------------
			//// Rounded Chrome Tabs
			//--------------------------------------------------------
			var tabpathNew =
				isActiveTab ?
				tabDrawnRect.CreateTabPath_Roundtop_RoundBottomOut(TabRadius) :
				tabDrawnRect.CreateTabPath_roundAll(TabRadius);

			g.FillPath(brush, tabpathNew);
			//g.DrawPath(SystemPens.ControlDark, tabpathNew);
			//--white color requires more work...
			g.DrawPath(pen, tabpathNew);
			//--draw ,tab seperator line:   | TAB1 | TAB2
			if (!isActiveTab && !is_atRightof_ActiveTab) {
				int margin = 14;
				sr.Y += 2; //move rect down
				g.DrawLine(SystemPens.ControlDark, sr.X, sr.Y + margin, sr.X, sr.Y + sr.Height - margin);
			}

			//g.DrawPath(SystemPens.ControlDark, graphicsPath);
			if (currentItem == SelectedItem) {
				//g.DrawLine(new Pen(brush), sr_left + 19f, sr_height + 2f, sr_left + sr_width - 1f, sr_height + 2f);
			}
			//--------------------------------------------------------

			var ForeColorSel = ForeColor;

			//margin for Tab Text, Vertically Center
			tabDrawnRect.X += 10;
			tabDrawnRect.Width -= 10;
			//sf.LineAlignment = StringAlignment.Center; 

			//--------------------------------------------------------
			// Tab Title
			//--------------------------------------------------------
			if (currentItem == SelectedItem) {
				tabDrawnRect.Width -= 25;// dont overflow ellipsis "..." under the activeTab.CloseButton.
										 //g.DrawRectangle(Pens.Cyan, tabDrawnRect);
				g.DrawString(currentItem.Title, font, new SolidBrush(ForeColorSel), tabDrawnRect, sf);
			}
			else {
				g.DrawString(currentItem.Title, font, new SolidBrush(ForeColorSel), tabDrawnRect, sf);
			}
			//--------------------------------------------------------

			currentItem.IsDrawn = true;
		}


		private void UpdateLayout() {
			//sf.Trimming = StringTrimming.Character;
			sf.Trimming = StringTrimming.EllipsisCharacter;
			sf.FormatFlags = StringFormatFlags.NoWrap;
			//sf.FormatFlags |= StringFormatFlags.DirectionRightToLeft; //this line causes multiline.//is this arabic??
			sf.LineAlignment = StringAlignment.Center;

			base.DockPadding.Top = BrowserTabStyle.TabHeight + 1;
			base.DockPadding.Bottom = 1;
			base.DockPadding.Right = 1;
			base.DockPadding.Left = 1;
		}

		private void OnCollectionChanged(object sender, CollectionChangeEventArgs e) {
			BrowserTabItem fATabStripItem = (BrowserTabItem)e.Element;
			if (e.Action == CollectionChangeAction.Add) {
				Controls.Add(fATabStripItem);
				OnTabStripItemChanged(new TabStripItemChangedEventArgs(fATabStripItem, BrowserTabStripItemChangeTypes.Added));
			}
			else if (e.Action == CollectionChangeAction.Remove) {
				Controls.Remove(fATabStripItem);
				OnTabStripItemChanged(new TabStripItemChangedEventArgs(fATabStripItem, BrowserTabStripItemChangeTypes.Removed));
			}
			else {
				OnTabStripItemChanged(new TabStripItemChangedEventArgs(fATabStripItem, BrowserTabStripItemChangeTypes.Changed));
			}
			UpdateLayout();
			Invalidate();
		}

		public bool ShouldSerializeSelectedItem() => true;
		public bool ShouldSerializeItems() => items.Count > 0;
		public void BeginInit() => isIniting = true;
		public void EndInit() => isIniting = false;

		protected override void Dispose(bool disposing) {
			if (disposing) {
				items.CollectionChanged -= OnCollectionChanged;
				menu.ItemClicked -= OnMenuItemClicked;
				menu.VisibleChanged -= OnMenuVisibleChanged;
				foreach (BrowserTabItem item in items) {
					if (item != null && !item.IsDisposed) {
						item.Dispose();
					}
				}
				if (menu != null && !menu.IsDisposed) {
					menu.Dispose();
				}
				if (sf != null) {
					sf.Dispose();
				}
			}
			base.Dispose(disposing);
		}
	}
}