using System;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering.PostProcessing
{
	// Token: 0x02000020 RID: 32
	[Preserve]
	internal sealed class DepthOfFieldRenderer : PostProcessEffectRenderer<DepthOfField>
	{
		// Token: 0x0600003F RID: 63 RVA: 0x00004D9C File Offset: 0x00002F9C
		public DepthOfFieldRenderer()
		{
			for (int i = 0; i < 2; i++)
			{
				this.m_CoCHistoryTextures[i] = new RenderTexture[2];
				this.m_HistoryPingPong[i] = 0;
			}
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00004DEA File Offset: 0x00002FEA
		public override DepthTextureMode GetCameraFlags()
		{
			return DepthTextureMode.Depth;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00004DED File Offset: 0x00002FED
		private RenderTextureFormat SelectFormat(RenderTextureFormat primary, RenderTextureFormat secondary)
		{
			if (primary.IsSupported())
			{
				return primary;
			}
			if (secondary.IsSupported())
			{
				return secondary;
			}
			return RenderTextureFormat.Default;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00004E04 File Offset: 0x00003004
		private float CalculateMaxCoCRadius(int screenHeight)
		{
			float num = (float)base.settings.kernelSize.value * 4f + 6f;
			return Mathf.Min(0.05f, num / (float)screenHeight);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00004E40 File Offset: 0x00003040
		private RenderTexture CheckHistory(int eye, int id, PostProcessRenderContext context, RenderTextureFormat format)
		{
			RenderTexture renderTexture = this.m_CoCHistoryTextures[eye][id];
			if (this.m_ResetHistory || renderTexture == null || !renderTexture.IsCreated() || renderTexture.width != context.width || renderTexture.height != context.height)
			{
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = context.GetScreenSpaceTemporaryRT(0, format, RenderTextureReadWrite.Linear, 0, 0);
				renderTexture.name = "CoC History, Eye: " + eye.ToString() + ", ID: " + id.ToString();
				renderTexture.filterMode = FilterMode.Bilinear;
				renderTexture.Create();
				this.m_CoCHistoryTextures[eye][id] = renderTexture;
			}
			return renderTexture;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00004EE0 File Offset: 0x000030E0
		public override void Render(PostProcessRenderContext context)
		{
			RenderTextureFormat renderTextureFormat = (context.camera.allowHDR ? RenderTextureFormat.ARGBHalf : RenderTextureFormat.ARGB32);
			RenderTextureFormat renderTextureFormat2 = this.SelectFormat(RenderTextureFormat.R8, RenderTextureFormat.RHalf);
			float num = 0.024f * ((float)context.height / 1080f);
			float num2 = base.settings.focalLength.value / 1000f;
			float num3 = Mathf.Max(base.settings.focusDistance.value, num2);
			float num4 = (float)context.screenWidth / (float)context.screenHeight;
			float num5 = num2 * num2 / (base.settings.aperture.value * (num3 - num2) * num * 2f);
			float num6 = this.CalculateMaxCoCRadius(context.screenHeight);
			PropertySheet propertySheet = context.propertySheets.Get(context.resources.shaders.depthOfField);
			propertySheet.properties.Clear();
			propertySheet.properties.SetFloat(ShaderIDs.Distance, num3);
			propertySheet.properties.SetFloat(ShaderIDs.LensCoeff, num5);
			propertySheet.properties.SetFloat(ShaderIDs.MaxCoC, num6);
			propertySheet.properties.SetFloat(ShaderIDs.RcpMaxCoC, 1f / num6);
			propertySheet.properties.SetFloat(ShaderIDs.RcpAspect, 1f / num4);
			CommandBuffer command = context.command;
			command.BeginSample("DepthOfField");
			context.GetScreenSpaceTemporaryRT(command, ShaderIDs.CoCTex, 0, renderTextureFormat2, RenderTextureReadWrite.Linear, FilterMode.Bilinear, 0, 0);
			command.BlitFullscreenTriangle(BuiltinRenderTextureType.None, ShaderIDs.CoCTex, propertySheet, 0, false, null, false);
			if (context.IsTemporalAntialiasingActive())
			{
				float motionBlending = context.temporalAntialiasing.motionBlending;
				float num7 = (this.m_ResetHistory ? 0f : motionBlending);
				Vector2 jitter = context.temporalAntialiasing.jitter;
				propertySheet.properties.SetVector(ShaderIDs.TaaParams, new Vector3(jitter.x, jitter.y, num7));
				int num8 = this.m_HistoryPingPong[context.xrActiveEye];
				RenderTexture renderTexture = this.CheckHistory(context.xrActiveEye, ++num8 % 2, context, renderTextureFormat2);
				RenderTexture renderTexture2 = this.CheckHistory(context.xrActiveEye, ++num8 % 2, context, renderTextureFormat2);
				this.m_HistoryPingPong[context.xrActiveEye] = (num8 + 1) % 2;
				command.BlitFullscreenTriangle(renderTexture, renderTexture2, propertySheet, 1, false, null, false);
				command.ReleaseTemporaryRT(ShaderIDs.CoCTex);
				command.SetGlobalTexture(ShaderIDs.CoCTex, renderTexture2);
			}
			context.GetScreenSpaceTemporaryRT(command, ShaderIDs.DepthOfFieldTex, 0, renderTextureFormat, RenderTextureReadWrite.Default, FilterMode.Bilinear, context.width / 2, context.height / 2);
			command.BlitFullscreenTriangle(context.source, ShaderIDs.DepthOfFieldTex, propertySheet, 2, false, null, false);
			context.GetScreenSpaceTemporaryRT(command, ShaderIDs.DepthOfFieldTemp, 0, renderTextureFormat, RenderTextureReadWrite.Default, FilterMode.Bilinear, context.width / 2, context.height / 2);
			command.BlitFullscreenTriangle(ShaderIDs.DepthOfFieldTex, ShaderIDs.DepthOfFieldTemp, propertySheet, (int)(3 + base.settings.kernelSize.value), false, null, false);
			command.BlitFullscreenTriangle(ShaderIDs.DepthOfFieldTemp, ShaderIDs.DepthOfFieldTex, propertySheet, 7, false, null, false);
			command.ReleaseTemporaryRT(ShaderIDs.DepthOfFieldTemp);
			if (context.IsDebugOverlayEnabled(DebugOverlay.DepthOfField))
			{
				context.PushDebugOverlay(command, context.source, propertySheet, 9);
			}
			command.BlitFullscreenTriangle(context.source, context.destination, propertySheet, 8, false, null, false);
			command.ReleaseTemporaryRT(ShaderIDs.DepthOfFieldTex);
			if (!context.IsTemporalAntialiasingActive())
			{
				command.ReleaseTemporaryRT(ShaderIDs.CoCTex);
			}
			command.EndSample("DepthOfField");
			this.m_ResetHistory = false;
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000052B8 File Offset: 0x000034B8
		public override void Release()
		{
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < this.m_CoCHistoryTextures[i].Length; j++)
				{
					RenderTexture.ReleaseTemporary(this.m_CoCHistoryTextures[i][j]);
					this.m_CoCHistoryTextures[i][j] = null;
				}
				this.m_HistoryPingPong[i] = 0;
			}
			this.ResetHistory();
		}

		// Token: 0x04000080 RID: 128
		private const int k_NumEyes = 2;

		// Token: 0x04000081 RID: 129
		private const int k_NumCoCHistoryTextures = 2;

		// Token: 0x04000082 RID: 130
		private readonly RenderTexture[][] m_CoCHistoryTextures = new RenderTexture[2][];

		// Token: 0x04000083 RID: 131
		private int[] m_HistoryPingPong = new int[2];

		// Token: 0x04000084 RID: 132
		private const float k_FilmHeight = 0.024f;

		// Token: 0x0200006D RID: 109
		private enum Pass
		{
			// Token: 0x0400025F RID: 607
			CoCCalculation,
			// Token: 0x04000260 RID: 608
			CoCTemporalFilter,
			// Token: 0x04000261 RID: 609
			DownsampleAndPrefilter,
			// Token: 0x04000262 RID: 610
			BokehSmallKernel,
			// Token: 0x04000263 RID: 611
			BokehMediumKernel,
			// Token: 0x04000264 RID: 612
			BokehLargeKernel,
			// Token: 0x04000265 RID: 613
			BokehVeryLargeKernel,
			// Token: 0x04000266 RID: 614
			PostFilter,
			// Token: 0x04000267 RID: 615
			Combine,
			// Token: 0x04000268 RID: 616
			DebugOverlay
		}
	}
}
