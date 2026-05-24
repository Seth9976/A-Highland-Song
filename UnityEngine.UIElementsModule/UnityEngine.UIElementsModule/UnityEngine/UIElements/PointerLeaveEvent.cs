using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200021E RID: 542
	public sealed class PointerLeaveEvent : PointerEventBase<PointerLeaveEvent>
	{
		// Token: 0x06001067 RID: 4199 RVA: 0x0003FFD8 File Offset: 0x0003E1D8
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06001068 RID: 4200 RVA: 0x0003FFBB File Offset: 0x0003E1BB
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.TricklesDown | EventBase.EventPropagation.IgnoreCompositeRoots;
		}

		// Token: 0x06001069 RID: 4201 RVA: 0x0003FFE9 File Offset: 0x0003E1E9
		public PointerLeaveEvent()
		{
			this.LocalInit();
		}
	}
}
