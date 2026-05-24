using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001F4 RID: 500
	public class MouseMoveEvent : MouseEventBase<MouseMoveEvent>
	{
		// Token: 0x06000F88 RID: 3976 RVA: 0x0003DA8C File Offset: 0x0003BC8C
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06000F89 RID: 3977 RVA: 0x0003DA9D File Offset: 0x0003BC9D
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.Bubbles | EventBase.EventPropagation.TricklesDown | EventBase.EventPropagation.Cancellable;
		}

		// Token: 0x06000F8A RID: 3978 RVA: 0x0003DAA8 File Offset: 0x0003BCA8
		public MouseMoveEvent()
		{
			this.LocalInit();
		}

		// Token: 0x06000F8B RID: 3979 RVA: 0x0003DABC File Offset: 0x0003BCBC
		public new static MouseMoveEvent GetPooled(Event systemEvent)
		{
			MouseMoveEvent pooled = MouseEventBase<MouseMoveEvent>.GetPooled(systemEvent);
			pooled.button = 0;
			return pooled;
		}

		// Token: 0x06000F8C RID: 3980 RVA: 0x0003DAE0 File Offset: 0x0003BCE0
		internal static MouseMoveEvent GetPooled(PointerMoveEvent pointerEvent)
		{
			return MouseEventBase<MouseMoveEvent>.GetPooled(pointerEvent);
		}
	}
}
