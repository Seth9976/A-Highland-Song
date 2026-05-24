using System;

namespace UnityEngine.Rendering
{
	// Token: 0x020003A1 RID: 929
	[Flags]
	public enum MeshUpdateFlags
	{
		// Token: 0x04000A42 RID: 2626
		Default = 0,
		// Token: 0x04000A43 RID: 2627
		DontValidateIndices = 1,
		// Token: 0x04000A44 RID: 2628
		DontResetBoneBounds = 2,
		// Token: 0x04000A45 RID: 2629
		DontNotifyMeshUsers = 4,
		// Token: 0x04000A46 RID: 2630
		DontRecalculateBounds = 8
	}
}
