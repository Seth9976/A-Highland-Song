using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200021C RID: 540
	public sealed class ClickEvent : PointerEventBase<ClickEvent>
	{
		// Token: 0x06001060 RID: 4192 RVA: 0x0003FF64 File Offset: 0x0003E164
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06001061 RID: 4193 RVA: 0x0003D8E3 File Offset: 0x0003BAE3
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.Bubbles | EventBase.EventPropagation.TricklesDown | EventBase.EventPropagation.Cancellable | EventBase.EventPropagation.SkipDisabledElements;
		}

		// Token: 0x06001062 RID: 4194 RVA: 0x0003FF75 File Offset: 0x0003E175
		public ClickEvent()
		{
			this.LocalInit();
		}

		// Token: 0x06001063 RID: 4195 RVA: 0x0003FF88 File Offset: 0x0003E188
		internal static ClickEvent GetPooled(PointerUpEvent pointerEvent, int clickCount)
		{
			ClickEvent pooled = PointerEventBase<ClickEvent>.GetPooled(pointerEvent);
			pooled.clickCount = clickCount;
			return pooled;
		}
	}
}
