using System;

namespace UnityEngine
{
	// Token: 0x0200022E RID: 558
	public sealed class WaitUntil : CustomYieldInstruction
	{
		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x060017ED RID: 6125 RVA: 0x00026DAC File Offset: 0x00024FAC
		public override bool keepWaiting
		{
			get
			{
				return !this.m_Predicate.Invoke();
			}
		}

		// Token: 0x060017EE RID: 6126 RVA: 0x00026DCC File Offset: 0x00024FCC
		public WaitUntil(Func<bool> predicate)
		{
			this.m_Predicate = predicate;
		}

		// Token: 0x04000834 RID: 2100
		private Func<bool> m_Predicate;
	}
}
