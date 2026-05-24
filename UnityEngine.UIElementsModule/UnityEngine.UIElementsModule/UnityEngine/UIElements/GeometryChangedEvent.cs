using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001EB RID: 491
	public class GeometryChangedEvent : EventBase<GeometryChangedEvent>
	{
		// Token: 0x06000F2B RID: 3883 RVA: 0x0003CC28 File Offset: 0x0003AE28
		public static GeometryChangedEvent GetPooled(Rect oldRect, Rect newRect)
		{
			GeometryChangedEvent pooled = EventBase<GeometryChangedEvent>.GetPooled();
			pooled.oldRect = oldRect;
			pooled.newRect = newRect;
			return pooled;
		}

		// Token: 0x06000F2C RID: 3884 RVA: 0x0003CC51 File Offset: 0x0003AE51
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06000F2D RID: 3885 RVA: 0x0003CC62 File Offset: 0x0003AE62
		private void LocalInit()
		{
			this.oldRect = Rect.zero;
			this.newRect = Rect.zero;
			this.layoutPass = 0;
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06000F2E RID: 3886 RVA: 0x0003CC85 File Offset: 0x0003AE85
		// (set) Token: 0x06000F2F RID: 3887 RVA: 0x0003CC8D File Offset: 0x0003AE8D
		public Rect oldRect { get; private set; }

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06000F30 RID: 3888 RVA: 0x0003CC96 File Offset: 0x0003AE96
		// (set) Token: 0x06000F31 RID: 3889 RVA: 0x0003CC9E File Offset: 0x0003AE9E
		public Rect newRect { get; private set; }

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06000F32 RID: 3890 RVA: 0x0003CCA7 File Offset: 0x0003AEA7
		// (set) Token: 0x06000F33 RID: 3891 RVA: 0x0003CCAF File Offset: 0x0003AEAF
		internal int layoutPass { get; set; }

		// Token: 0x06000F34 RID: 3892 RVA: 0x0003CCB8 File Offset: 0x0003AEB8
		public GeometryChangedEvent()
		{
			this.LocalInit();
		}
	}
}
