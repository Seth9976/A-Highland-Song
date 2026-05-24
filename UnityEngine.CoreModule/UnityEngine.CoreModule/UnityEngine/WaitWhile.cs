using System;

namespace UnityEngine
{
	// Token: 0x0200022F RID: 559
	public sealed class WaitWhile : CustomYieldInstruction
	{
		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x060017EF RID: 6127 RVA: 0x00026DE0 File Offset: 0x00024FE0
		public override bool keepWaiting
		{
			get
			{
				return this.m_Predicate.Invoke();
			}
		}

		// Token: 0x060017F0 RID: 6128 RVA: 0x00026DFD File Offset: 0x00024FFD
		public WaitWhile(Func<bool> predicate)
		{
			this.m_Predicate = predicate;
		}

		// Token: 0x04000835 RID: 2101
		private Func<bool> m_Predicate;
	}
}
