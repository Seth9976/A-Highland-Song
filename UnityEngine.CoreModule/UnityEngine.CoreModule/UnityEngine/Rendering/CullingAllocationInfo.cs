using System;

namespace UnityEngine.Rendering
{
	// Token: 0x020003F8 RID: 1016
	internal struct CullingAllocationInfo
	{
		// Token: 0x04000CC8 RID: 3272
		public unsafe VisibleLight* visibleLightsPtr;

		// Token: 0x04000CC9 RID: 3273
		public unsafe VisibleLight* visibleOffscreenVertexLightsPtr;

		// Token: 0x04000CCA RID: 3274
		public unsafe VisibleReflectionProbe* visibleReflectionProbesPtr;

		// Token: 0x04000CCB RID: 3275
		public int visibleLightCount;

		// Token: 0x04000CCC RID: 3276
		public int visibleOffscreenVertexLightCount;

		// Token: 0x04000CCD RID: 3277
		public int visibleReflectionProbeCount;
	}
}
