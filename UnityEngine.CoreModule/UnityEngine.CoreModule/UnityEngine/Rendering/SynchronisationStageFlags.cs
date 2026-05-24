using System;

namespace UnityEngine.Rendering
{
	// Token: 0x020003DD RID: 989
	public enum SynchronisationStageFlags
	{
		// Token: 0x04000C23 RID: 3107
		VertexProcessing = 1,
		// Token: 0x04000C24 RID: 3108
		PixelProcessing,
		// Token: 0x04000C25 RID: 3109
		ComputeProcessing = 4,
		// Token: 0x04000C26 RID: 3110
		AllGPUOperations = 7
	}
}
