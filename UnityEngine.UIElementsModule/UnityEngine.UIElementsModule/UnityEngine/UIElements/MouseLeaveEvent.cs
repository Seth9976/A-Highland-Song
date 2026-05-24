using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001F8 RID: 504
	public class MouseLeaveEvent : MouseEventBase<MouseLeaveEvent>
	{
		// Token: 0x06000F99 RID: 3993 RVA: 0x0003DC0E File Offset: 0x0003BE0E
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06000F9A RID: 3994 RVA: 0x0003DBF1 File Offset: 0x0003BDF1
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.TricklesDown | EventBase.EventPropagation.Cancellable | EventBase.EventPropagation.IgnoreCompositeRoots;
		}

		// Token: 0x06000F9B RID: 3995 RVA: 0x0003DC1F File Offset: 0x0003BE1F
		public MouseLeaveEvent()
		{
			this.LocalInit();
		}
	}
}
