using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200021D RID: 541
	public sealed class PointerEnterEvent : PointerEventBase<PointerEnterEvent>
	{
		// Token: 0x06001064 RID: 4196 RVA: 0x0003FFAA File Offset: 0x0003E1AA
		protected override void Init()
		{
			base.Init();
			this.LocalInit();
		}

		// Token: 0x06001065 RID: 4197 RVA: 0x0003FFBB File Offset: 0x0003E1BB
		private void LocalInit()
		{
			base.propagation = EventBase.EventPropagation.TricklesDown | EventBase.EventPropagation.IgnoreCompositeRoots;
		}

		// Token: 0x06001066 RID: 4198 RVA: 0x0003FFC7 File Offset: 0x0003E1C7
		public PointerEnterEvent()
		{
			this.LocalInit();
		}
	}
}
