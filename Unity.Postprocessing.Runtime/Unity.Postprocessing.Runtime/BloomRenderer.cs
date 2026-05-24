using System;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering.PostProcessing
{
	// Token: 0x02000014 RID: 20
	[Preserve]
	internal sealed class BloomRenderer : PostProcessEffectRenderer<Bloom>
	{
		// Token: 0x06000023 RID: 35 RVA: 0x00002A7C File Offset: 0x00000C7C
		public override void Init()
		{
			this.m_Pyramid = new BloomRenderer.Level[16];
			for (int i = 0; i < 16; i++)
			{
				this.m_Pyramid[i] = new BloomRenderer.Level
				{
					down = Shader.PropertyToID("_BloomMipDown" + i.ToString()),
					up = Shader.PropertyToID("_BloomMipUp" + i.ToString())
				};
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002AF4 File Offset: 0x00000CF4
		public override void Render(PostProcessRenderContext context)
		{
			CommandBuffer command = context.command;
			command.BeginSample("BloomPyramid");
			PropertySheet propertySheet = context.propertySheets.Get(context.resources.shaders.bloom);
			propertySheet.properties.SetTexture(ShaderIDs.AutoExposureTex, context.autoExposureTexture);
			float num = Mathf.Clamp(base.settings.anamorphicRatio, -1f, 1f);
			float num2 = ((num < 0f) ? (-num) : 0f);
			float num3 = ((num > 0f) ? num : 0f);
			int num4 = Mathf.FloorToInt((float)context.screenWidth / (2f - num2));
			int num5 = Mathf.FloorToInt((float)context.screenHeight / (2f - num3));
			bool flag = context.stereoActive && context.stereoRenderingMode == PostProcessRenderContext.StereoRenderingMode.SinglePass && context.camera.stereoTargetEye == StereoTargetEyeMask.Both;
			int num6 = (flag ? (num4 * 2) : num4);
			float num7 = Mathf.Log((float)Mathf.Max(num4, num5), 2f) + Mathf.Min(base.settings.diffusion.value, 10f) - 10f;
			int num8 = Mathf.FloorToInt(num7);
			int num9 = Mathf.Clamp(num8, 1, 16);
			float num10 = 0.5f + num7 - (float)num8;
			propertySheet.properties.SetFloat(ShaderIDs.SampleScale, num10);
			float num11 = Mathf.GammaToLinearSpace(base.settings.threshold.value);
			float num12 = num11 * base.settings.softKnee.value + 1E-05f;
			Vector4 vector = new Vector4(num11, num11 - num12, num12 * 2f, 0.25f / num12);
			propertySheet.properties.SetVector(ShaderIDs.Threshold, vector);
			float num13 = Mathf.GammaToLinearSpace(base.settings.clamp.value);
			propertySheet.properties.SetVector(ShaderIDs.Params, new Vector4(num13, 0f, 0f, 0f));
			int num14 = (base.settings.fastMode ? 1 : 0);
			RenderTargetIdentifier renderTargetIdentifier = context.source;
			for (int i = 0; i < num9; i++)
			{
				int down = this.m_Pyramid[i].down;
				int up = this.m_Pyramid[i].up;
				int num15 = ((i == 0) ? num14 : (2 + num14));
				context.GetScreenSpaceTemporaryRT(command, down, 0, context.sourceFormat, RenderTextureReadWrite.Default, FilterMode.Bilinear, num6, num5);
				context.GetScreenSpaceTemporaryRT(command, up, 0, context.sourceFormat, RenderTextureReadWrite.Default, FilterMode.Bilinear, num6, num5);
				command.BlitFullscreenTriangle(renderTargetIdentifier, down, propertySheet, num15, false, null, false);
				renderTargetIdentifier = down;
				num6 = ((flag && num6 / 2 % 2 > 0) ? (1 + num6 / 2) : (num6 / 2));
				num6 = Mathf.Max(num6, 1);
				num5 = Mathf.Max(num5 / 2, 1);
			}
			int num16 = this.m_Pyramid[num9 - 1].down;
			for (int j = num9 - 2; j >= 0; j--)
			{
				int down2 = this.m_Pyramid[j].down;
				int up2 = this.m_Pyramid[j].up;
				command.SetGlobalTexture(ShaderIDs.BloomTex, down2);
				command.BlitFullscreenTriangle(num16, up2, propertySheet, 4 + num14, false, null, false);
				num16 = up2;
			}
			Color linear = base.settings.color.value.linear;
			float num17 = RuntimeUtilities.Exp2(base.settings.intensity.value / 10f) - 1f;
			Vector4 vector2 = new Vector4(num10, num17, base.settings.dirtIntensity.value, (float)num9);
			if (context.IsDebugOverlayEnabled(DebugOverlay.BloomThreshold))
			{
				context.PushDebugOverlay(command, context.source, propertySheet, 6);
			}
			else if (context.IsDebugOverlayEnabled(DebugOverlay.BloomBuffer))
			{
				propertySheet.properties.SetVector(ShaderIDs.ColorIntensity, new Vector4(linear.r, linear.g, linear.b, num17));
				context.PushDebugOverlay(command, this.m_Pyramid[0].up, propertySheet, 7 + num14);
			}
			Texture texture = ((base.settings.dirtTexture.value == null) ? RuntimeUtilities.blackTexture : base.settings.dirtTexture.value);
			float num18 = (float)texture.width / (float)texture.height;
			float num19 = (float)context.screenWidth / (float)context.screenHeight;
			Vector4 vector3 = new Vector4(1f, 1f, 0f, 0f);
			if (num18 > num19)
			{
				vector3.x = num19 / num18;
				vector3.z = (1f - vector3.x) * 0.5f;
			}
			else if (num19 > num18)
			{
				vector3.y = num18 / num19;
				vector3.w = (1f - vector3.y) * 0.5f;
			}
			PropertySheet uberSheet = context.uberSheet;
			if (base.settings.fastMode)
			{
				uberSheet.EnableKeyword("BLOOM_LOW");
			}
			else
			{
				uberSheet.EnableKeyword("BLOOM");
			}
			uberSheet.properties.SetVector(ShaderIDs.Bloom_DirtTileOffset, vector3);
			uberSheet.properties.SetVector(ShaderIDs.Bloom_Settings, vector2);
			uberSheet.properties.SetColor(ShaderIDs.Bloom_Color, linear);
			uberSheet.properties.SetTexture(ShaderIDs.Bloom_DirtTex, texture);
			command.SetGlobalTexture(ShaderIDs.BloomTex, num16);
			for (int k = 0; k < num9; k++)
			{
				if (this.m_Pyramid[k].down != num16)
				{
					command.ReleaseTemporaryRT(this.m_Pyramid[k].down);
				}
				if (this.m_Pyramid[k].up != num16)
				{
					command.ReleaseTemporaryRT(this.m_Pyramid[k].up);
				}
			}
			command.EndSample("BloomPyramid");
			context.bloomBufferNameID = num16;
		}

		// Token: 0x0400003A RID: 58
		private BloomRenderer.Level[] m_Pyramid;

		// Token: 0x0400003B RID: 59
		private const int k_MaxPyramidSize = 16;

		// Token: 0x0200006A RID: 106
		private enum Pass
		{
			// Token: 0x0400024F RID: 591
			Prefilter13,
			// Token: 0x04000250 RID: 592
			Prefilter4,
			// Token: 0x04000251 RID: 593
			Downsample13,
			// Token: 0x04000252 RID: 594
			Downsample4,
			// Token: 0x04000253 RID: 595
			UpsampleTent,
			// Token: 0x04000254 RID: 596
			UpsampleBox,
			// Token: 0x04000255 RID: 597
			DebugOverlayThreshold,
			// Token: 0x04000256 RID: 598
			DebugOverlayTent,
			// Token: 0x04000257 RID: 599
			DebugOverlayBox
		}

		// Token: 0x0200006B RID: 107
		private struct Level
		{
			// Token: 0x04000258 RID: 600
			internal int down;

			// Token: 0x04000259 RID: 601
			internal int up;
		}
	}
}
