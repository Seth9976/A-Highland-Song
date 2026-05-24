using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020000E8 RID: 232
	internal class VisualElementFocusChangeTarget : FocusChangeDirection
	{
		// Token: 0x06000748 RID: 1864 RVA: 0x0001A698 File Offset: 0x00018898
		public static VisualElementFocusChangeTarget GetPooled(Focusable target)
		{
			VisualElementFocusChangeTarget visualElementFocusChangeTarget = VisualElementFocusChangeTarget.Pool.Get();
			visualElementFocusChangeTarget.target = target;
			return visualElementFocusChangeTarget;
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x0001A6BE File Offset: 0x000188BE
		protected override void Dispose()
		{
			this.target = null;
			VisualElementFocusChangeTarget.Pool.Release(this);
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x0001A6D5 File Offset: 0x000188D5
		internal override void ApplyTo(FocusController focusController, Focusable f)
		{
			f.Focus();
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x0001A6DF File Offset: 0x000188DF
		public VisualElementFocusChangeTarget()
			: base(FocusChangeDirection.unspecified)
		{
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x0600074C RID: 1868 RVA: 0x0001A6F3 File Offset: 0x000188F3
		// (set) Token: 0x0600074D RID: 1869 RVA: 0x0001A6FB File Offset: 0x000188FB
		public Focusable target { get; private set; }

		// Token: 0x040002F3 RID: 755
		private static readonly ObjectPool<VisualElementFocusChangeTarget> Pool = new ObjectPool<VisualElementFocusChangeTarget>(100);
	}
}
