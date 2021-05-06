using System.Collections;
using System.ComponentModel;

namespace SharpBrowser.BrowserTabStrip {
	public abstract class CollectionWithEvents : CollectionBase {
		private int _suspendCount;

		[Browsable(false)]
		public bool IsSuspended => _suspendCount > 0;

		[Browsable(false)]
		public event CollectionClear Clearing;

		[Browsable(false)]
		public event CollectionClear Cleared;

		[Browsable(false)]
		public event CollectionChange Inserting;

		[Browsable(false)]
		public event CollectionChange Inserted;

		[Browsable(false)]
		public event CollectionChange Removing;

		[Browsable(false)]
		public event CollectionChange Removed;

		public CollectionWithEvents() {
			_suspendCount = 0;
		}

		public void SuspendEvents() {
			_suspendCount++;
		}

		public void ResumeEvents() {
			_suspendCount--;
		}

		protected override void OnClear() {
			if (!IsSuspended && this.Clearing != null) {
				this.Clearing();
			}
		}

		protected override void OnClearComplete() {
			if (!IsSuspended && this.Cleared != null) {
				this.Cleared();
			}
		}

		protected override void OnInsert(int index, object value) {
			if (!IsSuspended && this.Inserting != null) {
				this.Inserting(index, value);
			}
		}

		protected override void OnInsertComplete(int index, object value) {
			if (!IsSuspended && this.Inserted != null) {
				this.Inserted(index, value);
			}
		}

		protected override void OnRemove(int index, object value) {
			if (!IsSuspended && this.Removing != null) {
				this.Removing(index, value);
			}
		}

		protected override void OnRemoveComplete(int index, object value) {
			if (!IsSuspended && this.Removed != null) {
				this.Removed(index, value);
			}
		}

		protected int IndexOf(object value) {
			return base.List.IndexOf(value);
		}
	}
}