using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SharpBrowser.BrowserTabStrip {
	[DefaultEvent("TabStripItemSelectionChanged")]
	[DefaultProperty("Items")]
	[ToolboxItem(true)]
	public class BrowserTabStrip : BaseStyledPanel, ISupportInitialize, IDisposable {
		private const int TEXT_LEFT_MARGIN = 15;

		private const int TEXT_RIGHT_MARGIN = 10;

		private const int DEF_HEADER_HEIGHT = 28;

		private const int DEF_BUTTON_HEIGHT = 28;

		private const int DEF_GLYPH_WIDTH = 40;

		private int DEF_START_POS = 10;

		private Rectangle stripButtonRect = Rectangle.Empty;

		private BrowserTabStripItem selectedItem;

		private ContextMenuStrip menu;

		private BrowserTabStripCloseButton closeButton;

		private BrowserTabStripItemCollection items;

		private StringFormat sf;

		private static Font defaultFont = new Font("Tahoma", 8.25f, FontStyle.Regular);

		private bool isIniting;

		private bool menuOpen;

		public int MaxTabSize = 200;

		public int AddButtonWidth = 40;

		[RefreshProperties(RefreshProperties.All)]
		[DefaultValue(null)]
		public BrowserTabStripItem SelectedItem {
			get {
				return selectedItem;
			}
			set {
				if (selectedItem == value) {
					return;
				}
				if (value == null && Items.Count > 0) {
					BrowserTabStripItem fATabStripItem = Items[0];
					if (fATabStripItem.Visible) {
						selectedItem = fATabStripItem;
						selectedItem.Selected = true;
						selectedItem.Dock = DockStyle.Fill;
					}
				}
				else {
					selectedItem = value;
				}
				foreach (BrowserTabStripItem item in Items) {
					if (item == selectedItem) {
						SelectItem(item);
						item.Dock = DockStyle.Fill;
						item.Show();
					}
					else {
						UnSelectItem(item);
						item.Hide();
					}
				}
				SelectItem(selectedItem);
				Invalidate();
				if (!selectedItem.IsDrawn) {
					Items.MoveTo(0, selectedItem);
					Invalidate();
				}
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
			closeButton = new BrowserTabStripCloseButton(base.ToolStripRenderer);
			Font = defaultFont;
			sf = new StringFormat();
			EndInit();
			UpdateLayout();
		}

		public HitTestResult HitTest(Point pt) {
			if (closeButton.IsVisible && closeButton.Rect.Contains(pt)) {
				return HitTestResult.CloseButton;
			}
			if (GetTabItemByPoint(pt) != null) {
				return HitTestResult.TabItem;
			}
			return HitTestResult.None;
		}

		public void AddTab(BrowserTabStripItem tabItem) {
			AddTab(tabItem, autoSelect: false);
		}

		public void AddTab(BrowserTabStripItem tabItem, bool autoSelect) {
			tabItem.Dock = DockStyle.Fill;
			Items.Add(tabItem);
			if ((autoSelect && tabItem.Visible) || (tabItem.Visible && Items.DrawnCount < 1)) {
				SelectedItem = tabItem;
				SelectItem(tabItem);
			}
		}

		public void RemoveTab(BrowserTabStripItem tabItem) {
			int num = Items.IndexOf(tabItem);
			if (num >= 0) {
				UnSelectItem(tabItem);
				Items.Remove(tabItem);
			}
			if (Items.Count > 0) {
				if (Items[num - 1] != null) {
					SelectedItem = Items[num - 1];
				}
				else {
					SelectedItem = Items.FirstVisible;
				}
			}
		}

		public BrowserTabStripItem GetTabItemByPoint(Point pt) {
			BrowserTabStripItem result = null;
			bool flag = false;
			for (int i = 0; i < Items.Count; i++) {
				BrowserTabStripItem fATabStripItem = Items[i];
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

		public virtual void ShowMenu() {
		}

		internal void UnDrawAll() {
			for (int i = 0; i < Items.Count; i++) {
				Items[i].IsDrawn = false;
			}
		}

		internal void SelectItem(BrowserTabStripItem tabItem) {
			tabItem.Dock = DockStyle.Fill;
			tabItem.Visible = true;
			tabItem.Selected = true;
		}

		internal void UnSelectItem(BrowserTabStripItem tabItem) {
			tabItem.Selected = false;
		}

		protected internal virtual void OnTabStripItemClosing(TabStripItemClosingEventArgs e) {
			if (this.TabStripItemClosing != null) {
				this.TabStripItemClosing(e);
			}
		}

		protected internal virtual void OnTabStripItemClosed(EventArgs e) {
			selectedItem = null;
			if (this.TabStripItemClosed != null) {
				this.TabStripItemClosed(this, e);
			}
		}

		protected virtual void OnMenuItemsLoading(HandledEventArgs e) {
			if (this.MenuItemsLoading != null) {
				this.MenuItemsLoading(this, e);
			}
		}

		protected virtual void OnMenuItemsLoaded(EventArgs e) {
			if (this.MenuItemsLoaded != null) {
				this.MenuItemsLoaded(this, e);
			}
		}

		protected virtual void OnTabStripItemChanged(TabStripItemChangedEventArgs e) {
			if (this.TabStripItemSelectionChanged != null) {
				this.TabStripItemSelectionChanged(e);
			}
		}

		protected virtual void OnMenuItemsLoad(EventArgs e) {
			menu.RightToLeft = RightToLeft;
			menu.Items.Clear();
			for (int i = 0; i < Items.Count; i++) {
				BrowserTabStripItem fATabStripItem = Items[i];
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

		protected override void OnPaint(PaintEventArgs e) {
			SetDefaultSelected();
			Rectangle clientRectangle = base.ClientRectangle;
			clientRectangle.Width--;
			clientRectangle.Height--;
			DEF_START_POS = 10;
			e.Graphics.DrawRectangle(SystemPens.ControlDark, clientRectangle);
			e.Graphics.FillRectangle(Brushes.White, clientRectangle);
			e.Graphics.FillRectangle(SystemBrushes.GradientInactiveCaption, new Rectangle(clientRectangle.X, clientRectangle.Y, clientRectangle.Width, 28));
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			for (int i = 0; i < Items.Count; i++) {
				BrowserTabStripItem fATabStripItem = Items[i];
				if (fATabStripItem.Visible || base.DesignMode) {
					OnCalcTabPage(e.Graphics, fATabStripItem);
					fATabStripItem.IsDrawn = false;
					OnDrawTabButton(e.Graphics, fATabStripItem);
				}
			}
			if (selectedItem != null) {
				OnDrawTabButton(e.Graphics, selectedItem);
			}
			if (Items.DrawnCount == 0 || Items.VisibleCount == 0) {
				e.Graphics.DrawLine(SystemPens.ControlDark, new Point(0, 28), new Point(base.ClientRectangle.Width, 28));
			}
			else if (SelectedItem != null && SelectedItem.IsDrawn) {
				int num = (int)(SelectedItem.StripRect.Height / 4f);
				Point point = new Point((int)SelectedItem.StripRect.Left - num, 28);
				e.Graphics.DrawLine(SystemPens.ControlDark, new Point(0, 28), point);
				point.X += (int)SelectedItem.StripRect.Width + num * 2;
				e.Graphics.DrawLine(SystemPens.ControlDark, point, new Point(base.ClientRectangle.Width, 28));
			}
			if (SelectedItem != null && SelectedItem.CanClose) {
				closeButton.IsVisible = true;
				closeButton.CalcBounds(selectedItem);
				closeButton.Draw(e.Graphics);
			}
			else {
				closeButton.IsVisible = false;
			}
		}

		protected override void OnMouseDown(MouseEventArgs e) {
			base.OnMouseDown(e);
			HitTestResult hitTestResult = HitTest(e.Location);
			if (hitTestResult == HitTestResult.TabItem) {
				BrowserTabStripItem tabItemByPoint = GetTabItemByPoint(e.Location);
				if (tabItemByPoint != null) {
					SelectedItem = tabItemByPoint;
					Invalidate();
				}
			}
			else {
				if (e.Button != MouseButtons.Left || hitTestResult != 0) {
					return;
				}
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
		}

		protected override void OnMouseMove(MouseEventArgs e) {
			base.OnMouseMove(e);
			if (closeButton.IsVisible) {
				if (closeButton.Rect.Contains(e.Location)) {
					closeButton.IsMouseOver = true;
					Invalidate(closeButton.RedrawRect);
				}
				else if (closeButton.IsMouseOver) {
					closeButton.IsMouseOver = false;
					Invalidate(closeButton.RedrawRect);
				}
			}
		}

		protected override void OnMouseLeave(EventArgs e) {
			base.OnMouseLeave(e);
			closeButton.IsMouseOver = false;
			if (closeButton.IsVisible) {
				Invalidate(closeButton.RedrawRect);
			}
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
				BrowserTabStripItem fATabStripItem = Items[i];
				fATabStripItem.Dock = DockStyle.Fill;
			}
		}

		private void OnMenuItemClicked(object sender, ToolStripItemClickedEventArgs e) {
			BrowserTabStripItem fATabStripItem2 = (SelectedItem = (BrowserTabStripItem)e.ClickedItem.Tag);
		}

		private void OnMenuVisibleChanged(object sender, EventArgs e) {
			if (!menu.Visible) {
				menuOpen = false;
			}
		}

		private void OnCalcTabPage(Graphics g, BrowserTabStripItem currentItem) {
			_ = Font;
			int num = 0;
			if (currentItem.Title == "+") {
				num = AddButtonWidth;
			}
			else {
				num = (base.Width - (AddButtonWidth + 20)) / (items.Count - 1);
				if (num > MaxTabSize) {
					num = MaxTabSize;
				}
			}
			RectangleF rectangleF2 = (currentItem.StripRect = new RectangleF(DEF_START_POS, 3f, num, 28f));
			DEF_START_POS += num;
		}

		private SizeF MeasureTabWidth(Graphics g, BrowserTabStripItem currentItem, Font currentFont) {
			SizeF result = g.MeasureString(currentItem.Title, currentFont, new SizeF(200f, 28f), sf);
			result.Width += 25f;
			return result;
		}

		private void OnDrawTabButton(Graphics g, BrowserTabStripItem currentItem) {
			Items.IndexOf(currentItem);
			Font font = Font;
			RectangleF stripRect = currentItem.StripRect;
			GraphicsPath graphicsPath = new GraphicsPath();
			float left = stripRect.Left;
			float right = stripRect.Right;
			float num = 3f;
			float num2 = stripRect.Bottom - 1f;
			float num3 = stripRect.Width;
			float num4 = stripRect.Height;
			float num5 = num4 / 4f;
			graphicsPath.AddLine(left - num5, num2, left + num5, num);
			graphicsPath.AddLine(right - num5, num, right + num5, num2);
			graphicsPath.CloseFigure();
			SolidBrush brush = new SolidBrush((currentItem == SelectedItem) ? Color.White : SystemColors.GradientInactiveCaption);
			g.FillPath(brush, graphicsPath);
			g.DrawPath(SystemPens.ControlDark, graphicsPath);
			if (currentItem == SelectedItem) {
				g.DrawLine(new Pen(brush), left - 9f, num4 + 2f, left + num3 - 1f, num4 + 2f);
			}
			PointF location = new PointF(left + 15f, 5f);
			RectangleF layoutRectangle = stripRect;
			layoutRectangle.Location = location;
			layoutRectangle.Width = num3 - (layoutRectangle.Left - left) - 4f;
			if (currentItem == selectedItem) {
				layoutRectangle.Width -= 15f;
			}
			layoutRectangle.Height = 23f;
			if (currentItem == SelectedItem) {
				g.DrawString(currentItem.Title, font, new SolidBrush(ForeColor), layoutRectangle, sf);
			}
			else {
				g.DrawString(currentItem.Title, font, new SolidBrush(ForeColor), layoutRectangle, sf);
			}
			currentItem.IsDrawn = true;
		}

		private void UpdateLayout() {
			sf.Trimming = StringTrimming.EllipsisCharacter;
			sf.FormatFlags |= StringFormatFlags.NoWrap;
			sf.FormatFlags &= StringFormatFlags.DirectionRightToLeft;
			stripButtonRect = new Rectangle(0, 0, base.ClientSize.Width - 40 - 2, 10);
			base.DockPadding.Top = 29;
			base.DockPadding.Bottom = 1;
			base.DockPadding.Right = 1;
			base.DockPadding.Left = 1;
		}

		private void OnCollectionChanged(object sender, CollectionChangeEventArgs e) {
			BrowserTabStripItem fATabStripItem = (BrowserTabStripItem)e.Element;
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

		public bool ShouldSerializeFont() {
			if (Font != null) {
				return !Font.Equals(defaultFont);
			}
			return false;
		}

		public bool ShouldSerializeSelectedItem() {
			return true;
		}

		public bool ShouldSerializeItems() {
			return items.Count > 0;
		}

		public new void ResetFont() {
			Font = defaultFont;
		}

		public void BeginInit() {
			isIniting = true;
		}

		public void EndInit() {
			isIniting = false;
		}

		protected override void Dispose(bool disposing) {
			if (disposing) {
				items.CollectionChanged -= OnCollectionChanged;
				menu.ItemClicked -= OnMenuItemClicked;
				menu.VisibleChanged -= OnMenuVisibleChanged;
				foreach (BrowserTabStripItem item in items) {
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