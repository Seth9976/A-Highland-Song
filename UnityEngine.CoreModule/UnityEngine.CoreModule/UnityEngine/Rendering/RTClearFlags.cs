using System;

namespace UnityEngine.Rendering
{
	// Token: 0x020003D8 RID: 984
	[Flags]
	public enum RTClearFlags
	{
		// Token: 0x04000C02 RID: 3074
		None = 0,
		// Token: 0x04000C03 RID: 3075
		Color = 1,
		// Token: 0x04000C04 RID: 3076
		Depth = 2,
		// Token: 0x04000C05 RID: 3077
		Stencil = 4,
		// Token: 0x04000C06 RID: 3078
		All = 7,
		// Token: 0x04000C07 RID: 3079
		DepthStencil = 6,
		// Token: 0x04000C08 RID: 3080
		ColorDepth = 3,
		// Token: 0x04000C09 RID: 3081
		ColorStencil = 5
	}
}
