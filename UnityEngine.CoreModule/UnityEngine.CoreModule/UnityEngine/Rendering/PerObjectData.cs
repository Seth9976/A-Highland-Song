using System;

namespace UnityEngine.Rendering
{
	// Token: 0x02000400 RID: 1024
	[Flags]
	public enum PerObjectData
	{
		// Token: 0x04000CEE RID: 3310
		None = 0,
		// Token: 0x04000CEF RID: 3311
		LightProbe = 1,
		// Token: 0x04000CF0 RID: 3312
		ReflectionProbes = 2,
		// Token: 0x04000CF1 RID: 3313
		LightProbeProxyVolume = 4,
		// Token: 0x04000CF2 RID: 3314
		Lightmaps = 8,
		// Token: 0x04000CF3 RID: 3315
		LightData = 16,
		// Token: 0x04000CF4 RID: 3316
		MotionVectors = 32,
		// Token: 0x04000CF5 RID: 3317
		LightIndices = 64,
		// Token: 0x04000CF6 RID: 3318
		ReflectionProbeData = 128,
		// Token: 0x04000CF7 RID: 3319
		OcclusionProbe = 256,
		// Token: 0x04000CF8 RID: 3320
		OcclusionProbeProxyVolume = 512,
		// Token: 0x04000CF9 RID: 3321
		ShadowMask = 1024
	}
}
