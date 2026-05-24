using System;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.LowLevel
{
	// Token: 0x020002EC RID: 748
	[MovedFrom("UnityEngine.Experimental.LowLevel")]
	public struct PlayerLoopSystem
	{
		// Token: 0x06001E77 RID: 7799 RVA: 0x00031682 File Offset: 0x0002F882
		public override string ToString()
		{
			return this.type.Name;
		}

		// Token: 0x040009F9 RID: 2553
		public Type type;

		// Token: 0x040009FA RID: 2554
		public PlayerLoopSystem[] subSystemList;

		// Token: 0x040009FB RID: 2555
		public PlayerLoopSystem.UpdateFunction updateDelegate;

		// Token: 0x040009FC RID: 2556
		public IntPtr updateFunction;

		// Token: 0x040009FD RID: 2557
		public IntPtr loopConditionFunction;

		// Token: 0x020002ED RID: 749
		// (Invoke) Token: 0x06001E79 RID: 7801
		public delegate void UpdateFunction();
	}
}
