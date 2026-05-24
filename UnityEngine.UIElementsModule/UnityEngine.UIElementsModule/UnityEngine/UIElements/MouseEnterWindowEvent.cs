using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001F9 RID: 505
	public class MouseEnterWindowEvent : MouseEventBase<MouseEnterWindowEvent>
	{
		// Token: 0x06000F9C RID: 3996 RVA: 0x0003DC30 File Offset: 0x0003BE30
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06000F9D RID: 3997 RVA: 0x0003DC41 File Offset: 0x0003BE41
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.Cancellable;
		}

		// Token: 0x06000F9E RID: 3998 RVA: 0x0003DC4C File Offset: 0x0003BE4C
		public MouseEnterWindowEvent()
		{
			this.LocalInit();
		}

		// Token: 0x06000F9F RID: 3999 RVA: 0x0003DC60 File Offset: 0x0003BE60
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
