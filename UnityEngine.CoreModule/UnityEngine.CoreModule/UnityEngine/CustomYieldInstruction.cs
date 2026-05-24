using System;
using System.Collections;

namespace UnityEngine
{
	// Token: 0x02000200 RID: 512
	public abstract class CustomYieldInstruction : IEnumerator
	{
		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x060016A2 RID: 5794
		public abstract bool keepWaiting { get; }

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x060016A3 RID: 5795 RVA: 0x000242F0 File Offset: 0x000224F0
		public object Current
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060016A4 RID: 5796 RVA: 0x00024304 File Offset: 0x00022504
		public bool MoveNext()
		{
			return this.keepWaiting;
		}

		// Token: 0x060016A5 RID: 5797 RVA: 0x00004557 File Offset: 0x00002757
		public virtual void Reset()
		{
		}
	}
}
