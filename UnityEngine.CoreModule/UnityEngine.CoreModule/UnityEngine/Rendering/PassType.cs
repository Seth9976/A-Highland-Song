using System;

namespace UnityEngine.Rendering
{
	// Token: 0x020003B9 RID: 953
	public enum PassType
	{
		// Token: 0x04000B2B RID: 2859
		Normal,
		// Token: 0x04000B2C RID: 2860
		Vertex,
		// Token: 0x04000B2D RID: 2861
		VertexLM,
		// Token: 0x04000B2E RID: 2862
		[Obsolete("VertexLMRGBM PassType is obsolete. Please use VertexLM PassType together with DecodeLightmap shader function.")]
		VertexLMRGBM,
		// Token: 0x04000B2F RID: 2863
		ForwardBase,
		// Token: 0x04000B30 RID: 2864
		ForwardAdd,
		// Token: 0x04000B31 RID: 2865
		LightPrePassBase,
		// Token: 0x04000B32 RID: 2866
		LightPrePassFinal,
		// Token: 0x04000B33 RID: 2867
		ShadowCaster,
		// Token: 0x04000B34 RID: 2868
		Deferred = 10,
		// Token: 0x04000B35 RID: 2869
		Meta,
		// Token: 0x04000B36 RID: 2870
		MotionVectors,
		// Token: 0x04000B37 RID: 2871
		ScriptableRenderPipeline,
		// Token: 0x04000B38 RID: 2872
		ScriptableRenderPipelineDefaultUnlit,
		// Token: 0x04000B39 RID: 2873
		GrabPass
	}
}
