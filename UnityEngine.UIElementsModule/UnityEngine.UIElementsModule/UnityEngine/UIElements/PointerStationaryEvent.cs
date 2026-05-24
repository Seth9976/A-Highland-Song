using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000219 RID: 537
	public sealed class PointerStationaryEvent : PointerEventBase<PointerStationaryEvent>
	{
		// Token: 0x06001055 RID: 4181 RVA: 0x0003FD74 File Offset: 0x0003DF74
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06001056 RID: 4182 RVA: 0x0003FD85 File Offset: 0x0003DF85
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.Bubbles | EventBase.EventPropagation.TricklesDown | EventBase.EventPropagation.Cancellable;
			((IPointerEventInternal)this).triggeredByOS = true;
			((IPointerEventInternal)this).recomputeTopElementUnderPointer = true;
		}

		// Token: 0x06001057 RID: 4183 RVA: 0x0003FDA0 File Offset: 0x0003DFA0
		public PointerStationaryEvent()
		{
			this.LocalInit();
		}
	}
}
