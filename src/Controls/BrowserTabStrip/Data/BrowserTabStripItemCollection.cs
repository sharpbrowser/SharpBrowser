using System;
using System.ComponentModel;

namespace SharpBrowser.Controls.BrowserTabStrip {
	public class BrowserTabStripItemCollection : CollectionWithEvents {
		private int lockUpdate;

		public BrowserTabItem this[int index] {
			get {
				if (index < 0 || base.List.Count - 1 < index) {
					return null;
				}
				return (BrowserTabItem)base.List[index];
			}
			set {
				base.List[index] = value;
			}
		}

		[Browsable(false)]
		public virtual int DrawnCount {
			get {
				int count = base.Count;
				int num = 0;
				if (count == 0) {
					return 0;
				}
				for (int i = 0; i < count; i++) {
					if (this[i].IsDrawn) {
						num++;
					}
				}
				return num;
			}
		}

		[Browsable(false)]
		public virtual int VisibleCount {
			get {
				int count = base.Count;
				int num = 0;
				if (count == 0) {
					return 0;
				}
				for (int i = 0; i < count; i++) {
					if (this[i].Visible) {
						num++;
					}
				}
				return num;
			}
		}

		[Browsable(false)]
		public event CollectionChangeEventHandler CollectionChanged;

		public BrowserTabStripItemCollection() {
			lockUpdate = 0;
		}

		protected virtual void OnCollectionChanged(CollectionChangeEventArgs e) {
			if (this.CollectionChanged != null) {
				this.CollectionChanged(this, e);
			}
		}

		protected virtual void BeginUpdate() {
			lockUpdate++;
		}

		protected virtual void EndUpdate() {
			if (--lockUpdate == 0) {
				OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null));
			}
		}

		public virtual void AddRange(BrowserTabItem[] items) {
			BeginUpdate();
			try {
				foreach (BrowserTabItem value in items) {
					base.List.Add(value);
				}
			}
			finally {
				EndUpdate();
			}
		}

		public virtual void Assign(BrowserTabStripItemCollection collection) {
			BeginUpdate();
			try {
				Clear();
				for (int i = 0; i < collection.Count; i++) {
					BrowserTabItem item = collection[i];
					BrowserTabItem fATabStripItem = new BrowserTabItem();
					fATabStripItem.Assign(item);
					Add(fATabStripItem);
				}
			}
			finally {
				EndUpdate();
			}
		}

		public virtual int Add(BrowserTabItem item) {
			int num = IndexOf(item);
			if (num == -1) {
				num = base.List.Add(item);
			}
			return num;
		}

		public virtual void Remove(BrowserTabItem item) {
			if (base.List.Contains(item)) {
				base.List.Remove(item);
			}
		}

		public virtual BrowserTabItem MoveTo(int newIndex, BrowserTabItem item) {
			int num = base.List.IndexOf(item);
			if (num >= 0) {
				RemoveAt(num);
				Insert(0, item);
				return item;
			}
			return null;
		}

		public virtual int IndexOf(BrowserTabItem item) {
			return base.List.IndexOf(item);
		}

		public virtual bool Contains(BrowserTabItem item) {
			return base.List.Contains(item);
		}

		public virtual void Insert(int index, BrowserTabItem item) {
			if (!Contains(item)) {
				base.List.Insert(index, item);
			}
		}

		protected override void OnInsertComplete(int index, object item) {
			BrowserTabItem fATabStripItem = item as BrowserTabItem;
			fATabStripItem.Changed += OnItem_Changed;
			OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, item));
		}

		protected override void OnRemove(int index, object item) {
			base.OnRemove(index, item);
			BrowserTabItem fATabStripItem = item as BrowserTabItem;
			fATabStripItem.Changed -= OnItem_Changed;
			OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Remove, item));
		}

		protected override void OnClear() {
			if (base.Count == 0) {
				return;
			}
			BeginUpdate();
			try {
				for (int num = base.Count - 1; num >= 0; num--) {
					RemoveAt(num);
				}
			}
			finally {
				EndUpdate();
			}
		}

		protected virtual void OnItem_Changed(object sender, EventArgs e) {
			OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, sender));
		}
	}
}