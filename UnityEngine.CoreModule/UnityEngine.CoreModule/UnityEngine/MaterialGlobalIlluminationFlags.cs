using System;

namespace UnityEngine
{
	// Token: 0x02000185 RID: 389
	[Flags]
	public enum MaterialGlobalIlluminationFlags
	{
		// Token: 0x04000570 RID: 1392
		None = 0,
		// Token: 0x04000571 RID: 1393
		RealtimeEmissive = 1,
		// Token: 0x04000572 RID: 1394
		BakedEmissive = 2,
		// Token: 0x04000573 RID: 1395
		EmissiveIsBlack = 4,
		// Token: 0x04000574 RID: 1396
		AnyEmissive = 3
	}
}
