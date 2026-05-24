using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001DD RID: 477
	public class FocusOutEvent : FocusEventBase<FocusOutEvent>
	{
		// Token: 0x06000EF3 RID: 3827 RVA: 0x0003C334 File Offset: 0x0003A534
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06000EF4 RID: 3828 RVA: 0x0003C345 File Offset: 0x0003A545
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.Bubbles | EventBase.EventPropagation.TricklesDown;
		}

		// Token: 0x06000EF5 RID: 3829 RVA: 0x0003C350 File Offset: 0x0003A550
		public FocusOutEvent()
		{
			this.LocalInit();
		}
	}
}
