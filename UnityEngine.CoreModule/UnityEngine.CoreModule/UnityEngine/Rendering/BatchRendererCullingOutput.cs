using System;
using Unity.Jobs;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering
{
	// Token: 0x020003E9 RID: 1001
	[UsedByNativeCode]
	[NativeHeader("Runtime/Camera/BatchRendererGroup.h")]
	internal struct BatchRendererCullingOutput
	{
		// Token: 0x04000C57 RID: 3159
		public JobHandle cullingJobsFence;

		// Token: 0x04000C58 RID: 3160
		public Matrix4x4 cullingMatrix;

		// Token: 0x04000C59 RID: 3161
		public unsafe Plane* cullingPlanes;

		// Token: 0x04000C5A RID: 3162
		public unsafe BatchVisibility* batchVisibility;

		// Token: 0x04000C5B RID: 3163
		public unsafe int* visibleIndices;

		// Token: 0x04000C5C RID: 3164
		public unsafe int* visibleIndicesY;

		// Token: 0x04000C5D RID: 3165
		public int cullingPlanesCount;

		// Token: 0x04000C5E RID: 3166
		public int batchVisibilityCount;

		// Token: 0x04000C5F RID: 3167
		public int visibleIndicesCount;

		// Token: 0x04000C60 RID: 3168
		public float nearPlane;
	}
}
