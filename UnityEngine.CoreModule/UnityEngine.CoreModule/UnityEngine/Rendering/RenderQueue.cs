using System;

namespace UnityEngine.Rendering
{
	// Token: 0x020003A7 RID: 935
	public enum RenderQueue
	{
		// Token: 0x04000A73 RID: 2675
		Background = 1000,
		// Token: 0x04000A74 RID: 2676
		Geometry = 2000,
		// Token: 0x04000A75 RID: 2677
		AlphaTest = 2450,
		// Token: 0x04000A76 RID: 2678
		GeometryLast = 2500,
		// Token: 0x04000A77 RID: 2679
		Transparent = 3000,
		// Token: 0x04000A78 RID: 2680
		Overlay = 4000
	}
}
