using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001F3 RID: 499
	public class MouseUpEvent : MouseEventBase<MouseUpEvent>
	{
		// Token: 0x06000F80 RID: 3968 RVA: 0x0003D9A8 File Offset: 0x0003BBA8
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06000F81 RID: 3969 RVA: 0x0003D8E3 File Offset: 0x0003BAE3
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.Bubbles | EventBase.EventPropagation.TricklesDown | EventBase.EventPropagation.Cancellable | EventBase.EventPropagation.SkipDisabledElements;
		}

		// Token: 0x06000F82 RID: 3970 RVA: 0x0003D9B9 File Offset: 0x0003BBB9
		public MouseUpEvent()
		{
			this.LocalInit();
		}

		// Token: 0x06000F83 RID: 3971 RVA: 0x0003D9CC File Offset: 0x0003BBCC
		public new static MouseUpEvent GetPooled(Event systemEvent)
		{
			bool flag = systemEvent != null;
			if (flag)
			{
				PointerDeviceState.ReleaseButton(PointerId.mousePointerId, systemEvent.button);
			}
			return MouseEventBase<MouseUpEvent>.GetPooled(systemEvent);
		}

		// Token: 0x06000F84 RID: 3972 RVA: 0x0003DA00 File Offset: 0x0003BC00
		private static MouseUpEvent MakeFromPointerEvent(IPointerEvent pointerEvent)
		{
			bool flag = pointerEvent != null && pointerEvent.button >= 0;
			if (flag)
			{
				PointerDeviceState.ReleaseButton(PointerId.mousePointerId, pointerEvent.button);
			}
			return MouseEventBase<MouseUpEvent>.GetPooled(pointerEvent);
		}

		// Token: 0x06000F85 RID: 3973 RVA: 0x0003DA44 File Offset: 0x0003BC44
		internal static MouseUpEvent GetPooled(PointerUpEvent pointerEvent)
		{
			return MouseUpEvent.MakeFromPointerEvent(pointerEvent);
		}

		// Token: 0x06000F86 RID: 3974 RVA: 0x0003DA5C File Offset: 0x0003BC5C
		internal static MouseUpEvent GetPooled(PointerMoveEvent pointerEvent)
		{
			return MouseUpEvent.MakeFromPointerEvent(pointerEvent);
		}

		// Token: 0x06000F87 RID: 3975 RVA: 0x0003DA74 File Offset: 0x0003BC74
		internal static MouseUpEvent GetPooled(PointerCancelEvent pointerEvent)
		{
			return MouseUpEvent.MakeFromPointerEvent(pointerEvent);
		}
	}
}
