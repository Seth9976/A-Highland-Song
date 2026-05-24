using System;

namespace UnityEngine.Rendering
{
	// Token: 0x020003F3 RID: 1011
	[Flags]
	public enum CullingOptions
	{
		// Token: 0x04000C9F RID: 3231
		None = 0,
		// Token: 0x04000CA0 RID: 3232
		ForceEvenIfCameraIsNotActive = 1,
		// Token: 0x04000CA1 RID: 3233
		OcclusionCull = 2,
		// Token: 0x04000CA2 RID: 3234
		NeedsLighting = 4,
		// Token: 0x04000CA3 RID: 3235
		NeedsReflectionProbes = 8,
		// Token: 0x04000CA4 RID: 3236
		Stereo = 16,
		// Token: 0x04000CA5 RID: 3237
		DisablePerObjectCulling = 32,
		// Token: 0x04000CA6 RID: 3238
		ShadowCasters = 64
	}
}
