using System;

namespace UnityEngine.Rendering
{
	// Token: 0x02000409 RID: 1033
	[Flags]
	public enum RenderStateMask
	{
		// Token: 0x04000D21 RID: 3361
		Nothing = 0,
		// Token: 0x04000D22 RID: 3362
		Blend = 1,
		// Token: 0x04000D23 RID: 3363
		Raster = 2,
		// Token: 0x04000D24 RID: 3364
		Depth = 4,
		// Token: 0x04000D25 RID: 3365
		Stencil = 8,
		// Token: 0x04000D26 RID: 3366
		Everything = 15
	}
}
