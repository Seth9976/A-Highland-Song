using System;

namespace UnityEngine.Rendering.PostProcessing
{
	// Token: 0x0200000D RID: 13
	internal interface IAmbientOcclusionMethod
	{
		// Token: 0x0600000C RID: 12
		DepthTextureMode GetCameraFlags();

		// Token: 0x0600000D RID: 13
		void RenderAfterOpaque(PostProcessRenderContext context);

		// Token: 0x0600000E RID: 14
		void RenderAmbientOnly(PostProcessRenderContext context);

		// Token: 0x0600000F RID: 15
		void CompositeAmbientOnly(PostProcessRenderContext context);

		// Token: 0x06000010 RID: 16
		void Release();
	}
}
