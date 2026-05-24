using System;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering.PostProcessing
{
	// Token: 0x0200000E RID: 14
	[Preserve]
	internal sealed class AmbientOcclusionRenderer : PostProcessEffectRenderer<AmbientOcclusion>
	{
		// Token: 0x06000011 RID: 17 RVA: 0x00002359 File Offset: 0x00000559
		public override void Init()
		{
			if (this.m_Methods == null)
			{
				this.m_Methods = new IAmbientOcclusionMethod[]
				{
					new ScalableAO(base.settings),
					new MultiScaleVO(base.settings)
				};
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x0000238C File Offset: 0x0000058C
		public bool IsAmbientOnly(PostProcessRenderContext context)
		{
			Camera camera = context.camera;
			return base.settings.ambientOnly.value && camera.actualRenderingPath == RenderingPath.DeferredShading && camera.allowHDR;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000023C3 File Offset: 0x000005C3
		public IAmbientOcclusionMethod Get()
		{
			return this.m_Methods[(int)base.settings.mode.value];
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000023DC File Offset: 0x000005DC
		public override DepthTextureMode GetCameraFlags()
		{
			return this.Get().GetCameraFlags();
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000023EC File Offset: 0x000005EC
		public override void Release()
		{
			IAmbientOcclusionMethod[] methods = this.m_Methods;
			for (int i = 0; i < methods.Length; i++)
			{
				methods[i].Release();
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002416 File Offset: 0x00000616
		public ScalableAO GetScalableAO()
		{
			return (ScalableAO)this.m_Methods[0];
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002425 File Offset: 0x00000625
		public MultiScaleVO GetMultiScaleVO()
		{
			return (MultiScaleVO)this.m_Methods[1];
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002434 File Offset: 0x00000634
		public override void Render(PostProcessRenderContext context)
		{
		}

		// Token: 0x04000020 RID: 32
		private IAmbientOcclusionMethod[] m_Methods;
	}
}
