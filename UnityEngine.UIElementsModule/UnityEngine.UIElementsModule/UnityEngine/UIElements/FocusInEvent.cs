using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001DF RID: 479
	public class FocusInEvent : FocusEventBase<FocusInEvent>
	{
		// Token: 0x06000EF8 RID: 3832 RVA: 0x0003C39F File Offset: 0x0003A59F
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06000EF9 RID: 3833 RVA: 0x0003C345 File Offset: 0x0003A545
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.Bubbles | EventBase.EventPropagation.TricklesDown;
		}

		// Token: 0x06000EFA RID: 3834 RVA: 0x0003C3B0 File Offset: 0x0003A5B0
		public FocusInEvent()
		{
			this.LocalInit();
		}
	}
}
