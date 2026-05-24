using System;

namespace UnityEngine.Rendering
{
	// Token: 0x020003C2 RID: 962
	[Flags]
	public enum RenderTargetFlags
	{
		// Token: 0x04000B7D RID: 2941
		None = 0,
		// Token: 0x04000B7E RID: 2942
		ReadOnlyDepth = 1,
		// Token: 0x04000B7F RID: 2943
		ReadOnlyStencil = 2,
		// Token: 0x04000B80 RID: 2944
		ReadOnlyDepthStencil = 3
	}
}
