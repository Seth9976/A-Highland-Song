using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000224 RID: 548
	public class TooltipEvent : EventBase<TooltipEvent>
	{
		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06001074 RID: 4212 RVA: 0x0004022A File Offset: 0x0003E42A
		// (set) Token: 0x06001075 RID: 4213 RVA: 0x00040232 File Offset: 0x0003E432
		public string tooltip { get; set; }

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x06001076 RID: 4214 RVA: 0x0004023B File Offset: 0x0003E43B
		// (set) Token: 0x06001077 RID: 4215 RVA: 0x00040243 File Offset: 0x0003E443
		public Rect rect { get; set; }

		// Token: 0x06001078 RID: 4216 RVA: 0x0004024C File Offset: 0x0003E44C
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06001079 RID: 4217 RVA: 0x00040260 File Offset: 0x0003E460
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.Bubbles | EventBase.EventPropagation.TricklesDown;
			this.rect = default(Rect);
			this.tooltip = string.Empty;
			base.ignoreCompositeRoots = true;
		}

		// Token: 0x0600107A RID: 4218 RVA: 0x0004029C File Offset: 0x0003E49C
		internal static TooltipEvent GetPooled(string tooltip, Rect rect)
		{
			TooltipEvent pooled = EventBase<TooltipEvent>.GetPooled();
			pooled.tooltip = tooltip;
			pooled.rect = rect;
			return pooled;
		}

		// Token: 0x0600107B RID: 4219 RVA: 0x000402C5 File Offset: 0x0003E4C5
		public TooltipEvent()
		{
			this.LocalInit();
		}
	}
}
