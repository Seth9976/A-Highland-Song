using System;

namespace UnityEngine.Rendering
{
	// Token: 0x020003B7 RID: 951
	[Flags]
	public enum ShadowMapPass
	{
		// Token: 0x04000B04 RID: 2820
		PointlightPositiveX = 1,
		// Token: 0x04000B05 RID: 2821
		PointlightNegativeX = 2,
		// Token: 0x04000B06 RID: 2822
		PointlightPositiveY = 4,
		// Token: 0x04000B07 RID: 2823
		PointlightNegativeY = 8,
		// Token: 0x04000B08 RID: 2824
		PointlightPositiveZ = 16,
		// Token: 0x04000B09 RID: 2825
		PointlightNegativeZ = 32,
		// Token: 0x04000B0A RID: 2826
		DirectionalCascade0 = 64,
		// Token: 0x04000B0B RID: 2827
		DirectionalCascade1 = 128,
		// Token: 0x04000B0C RID: 2828
		DirectionalCascade2 = 256,
		// Token: 0x04000B0D RID: 2829
		DirectionalCascade3 = 512,
		// Token: 0x04000B0E RID: 2830
		Spotlight = 1024,
		// Token: 0x04000B0F RID: 2831
		Pointlight = 63,
		// Token: 0x04000B10 RID: 2832
		Directional = 960,
		// Token: 0x04000B11 RID: 2833
		All = 2047
	}
}
