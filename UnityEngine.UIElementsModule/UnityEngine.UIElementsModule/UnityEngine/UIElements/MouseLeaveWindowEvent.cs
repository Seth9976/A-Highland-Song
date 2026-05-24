using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001FA RID: 506
	public class MouseLeaveWindowEvent : MouseEventBase<MouseLeaveWindowEvent>
	{
		// Token: 0x06000FA0 RID: 4000 RVA: 0x0003DC9E File Offset: 0x0003BE9E
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06000FA1 RID: 4001 RVA: 0x0003DCAF File Offset: 0x0003BEAF
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.Cancellable;
			((IMouseEventInternal)this).recomputeTopElementUnderMouse = false;
		}

		// Token: 0x06000FA2 RID: 4002 RVA: 0x0003DCC2 File Offset: 0x0003BEC2
		public MouseLeaveWindowEvent()
		{
			this.LocalInit();
		}

		// Token: 0x06000FA3 RID: 4003 RVA: 0x0003DCD4 File Offset: 0x0003BED4
		public new static MouseLeaveWindowEvent GetPooled(Event systemEvent)
		{
			bool flag = systemEvent != null;
			if (flag)
			{
				PointerDeviceState.ReleaseAllButtons(PointerId.mousePointerId);
			}
			return MouseEventBase<MouseLeaveWindowEvent>.GetPooled(systemEvent);
		}

		// Token: 0x06000FA4 RID: 4004 RVA: 0x0003DD00 File Offset: 0x0003BF00
		protected internal override void PostDispatch(IPanel panel)
		{
			EventBase eventBase = ((IMouseEventInternal)this).sourcePointerEvent as EventBase;
			bool flag = eventBase == null;
			if (flag)
			{
				BaseVisualElementPanel baseVisualElementPanel = panel as BaseVisualElementPanel;
				if (baseVisualElementPanel != null)
				{
					baseVisualElementPanel.CommitElementUnderPointers();
				}
			}
			base.PostDispatch(panel);
		}
	}
}
