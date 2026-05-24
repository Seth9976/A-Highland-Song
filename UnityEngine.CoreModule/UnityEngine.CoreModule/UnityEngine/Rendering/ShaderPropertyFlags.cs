using System;

namespace UnityEngine.Rendering
{
	// Token: 0x02000426 RID: 1062
	[Flags]
	public enum ShaderPropertyFlags
	{
		// Token: 0x04000DC0 RID: 3520
		None = 0,
		// Token: 0x04000DC1 RID: 3521
		HideInInspector = 1,
		// Token: 0x04000DC2 RID: 3522
		PerRendererData = 2,
		// Token: 0x04000DC3 RID: 3523
		NoScaleOffset = 4,
		// Token: 0x04000DC4 RID: 3524
		Normal = 8,
		// Token: 0x04000DC5 RID: 3525
		HDR = 16,
		// Token: 0x04000DC6 RID: 3526
		Gamma = 32,
		// Token: 0x04000DC7 RID: 3527
		NonModifiableTextureData = 64,
		// Token: 0x04000DC8 RID: 3528
		MainTexture = 128,
		// Token: 0x04000DC9 RID: 3529
		MainColor = 256
	}
}
