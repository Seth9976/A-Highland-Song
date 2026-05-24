using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001F2 RID: 498
	public class MouseDownEvent : MouseEventBase<MouseDownEvent>
	{
		// Token: 0x06000F79 RID: 3961 RVA: 0x0003D8D2 File Offset: 0x0003BAD2
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06000F7A RID: 3962 RVA: 0x0003D8E3 File Offset: 0x0003BAE3
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.Bubbles | EventBase.EventPropagation.TricklesDown | EventBase.EventPropagation.Cancellable | EventBase.EventPropagation.SkipDisabledElements;
		}

		// Token: 0x06000F7B RID: 3963 RVA: 0x0003D8EF File Offset: 0x0003BAEF
		public MouseDownEvent()
		{
			this.LocalInit();
		}

		// Token: 0x06000F7C RID: 3964 RVA: 0x0003D900 File Offset: 0x0003BB00
		public new static MouseDownEvent GetPooled(Event systemEvent)
		{
			bool flag = systemEvent != null;
			if (flag)
			{
				PointerDeviceState.PressButton(PointerId.mousePointerId, systemEvent.button);
			}
			return MouseEventBase<MouseDownEvent>.GetPooled(systemEvent);
		}

		// Token: 0x06000F7D RID: 3965 RVA: 0x0003D934 File Offset: 0x0003BB34
		private static MouseDownEvent MakeFromPointerEvent(IPointerEvent pointerEvent)
		{
			bool flag = pointerEvent != null && pointerEvent.button >= 0;
			if (flag)
			{
				PointerDeviceState.PressButton(PointerId.mousePointerId, pointerEvent.button);
			}
			return MouseEventBase<MouseDownEvent>.GetPooled(pointerEvent);
		}

		// Token: 0x06000F7E RID: 3966 RVA: 0x0003D978 File Offset: 0x0003BB78
		internal static MouseDownEvent GetPooled(PointerDownEvent pointerEvent)
		{
			return MouseDownEvent.MakeFromPointerEvent(pointerEvent);
		}

		// Token: 0x06000F7F RID: 3967 RVA: 0x0003D990 File Offset: 0x0003BB90
		internal static MouseDownEvent GetPooled(PointerMoveEvent pointerEvent)
		{
			return MouseDownEvent.MakeFromPointerEvent(pointerEvent);
		}
	}
}
