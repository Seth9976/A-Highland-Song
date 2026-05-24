using System;

namespace UnityEngine.Rendering
{
	// Token: 0x02000412 RID: 1042
	[Flags]
	public enum SortingCriteria
	{
		// Token: 0x04000D43 RID: 3395
		None = 0,
		// Token: 0x04000D44 RID: 3396
		SortingLayer = 1,
		// Token: 0x04000D45 RID: 3397
		RenderQueue = 2,
		// Token: 0x04000D46 RID: 3398
		BackToFront = 4,
		// Token: 0x04000D47 RID: 3399
		QuantizedFrontToBack = 8,
		// Token: 0x04000D48 RID: 3400
		OptimizeStateChanges = 16,
		// Token: 0x04000D49 RID: 3401
		CanvasOrder = 32,
		// Token: 0x04000D4A RID: 3402
		RendererPriority = 64,
		// Token: 0x04000D4B RID: 3403
		CommonOpaque = 59,
		// Token: 0x04000D4C RID: 3404
		CommonTransparent = 23
	}
}
