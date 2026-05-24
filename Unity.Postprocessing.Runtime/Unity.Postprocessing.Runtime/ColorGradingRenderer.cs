using System;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering.PostProcessing
{
	// Token: 0x0200001C RID: 28
	[Preserve]
	internal sealed class ColorGradingRenderer : PostProcessEffectRenderer<ColorGrading>
	{
		// Token: 0x0600002F RID: 47 RVA: 0x000038D0 File Offset: 0x00001AD0
		public override void Render(PostProcessRenderContext context)
		{
			GradingMode value = base.settings.gradingMode.value;
			bool flag = SystemInfo.supports3DRenderTextures && SystemInfo.supportsComputeShaders && context.resources.computeShaders.lut3DBaker != null && SystemInfo.graphicsDeviceType != GraphicsDeviceType.OpenGLCore && SystemInfo.graphicsDeviceType != GraphicsDeviceType.OpenGLES3;
			if (value == GradingMode.External)
			{
				this.RenderExternalPipeline3D(context);
				return;
			}
			if (value == GradingMode.HighDefinitionRange && flag)
			{
				this.RenderHDRPipeline3D(context);
				return;
			}
			if (value == GradingMode.HighDefinitionRange)
			{
				this.RenderHDRPipeline2D(context);
				return;
			}
			this.RenderLDRPipeline2D(context);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x0000395C File Offset: 0x00001B5C
		private void RenderExternalPipeline3D(PostProcessRenderContext context)
		{
			Texture value = base.settings.externalLut.value;
			if (value == null)
			{
				return;
			}
			PropertySheet uberSheet = context.uberSheet;
			uberSheet.EnableKeyword("COLOR_GRADING_HDR_3D");
			uberSheet.properties.SetTexture(ShaderIDs.Lut3D, value);
			uberSheet.properties.SetVector(ShaderIDs.Lut3D_Params, new Vector2(1f / (float)value.width, (float)value.width - 1f));
			uberSheet.properties.SetFloat(ShaderIDs.PostExposure, RuntimeUtilities.Exp2(base.settings.postExposure.value));
			context.logLut = value;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00003A08 File Offset: 0x00001C08
		private void RenderHDRPipeline3D(PostProcessRenderContext context)
		{
			this.CheckInternalLogLut();
			ComputeShader lut3DBaker = context.resources.computeShaders.lut3DBaker;
			int num = 0;
			switch (base.settings.tonemapper.value)
			{
			case Tonemapper.None:
				num = lut3DBaker.FindKernel("KGenLut3D_NoTonemap");
				break;
			case Tonemapper.Neutral:
				num = lut3DBaker.FindKernel("KGenLut3D_NeutralTonemap");
				break;
			case Tonemapper.ACES:
				num = lut3DBaker.FindKernel("KGenLut3D_AcesTonemap");
				break;
			case Tonemapper.Custom:
				num = lut3DBaker.FindKernel("KGenLut3D_CustomTonemap");
				break;
			}
			CommandBuffer command = context.command;
			command.SetComputeTextureParam(lut3DBaker, num, "_Output", this.m_InternalLogLut);
			command.SetComputeVectorParam(lut3DBaker, "_Size", new Vector4(33f, 0.03125f, 0f, 0f));
			Vector3 vector = ColorUtilities.ComputeColorBalance(base.settings.temperature.value, base.settings.tint.value);
			command.SetComputeVectorParam(lut3DBaker, "_ColorBalance", vector);
			command.SetComputeVectorParam(lut3DBaker, "_ColorFilter", base.settings.colorFilter.value);
			float num2 = base.settings.hueShift.value / 360f;
			float num3 = base.settings.saturation.value / 100f + 1f;
			float num4 = base.settings.contrast.value / 100f + 1f;
			command.SetComputeVectorParam(lut3DBaker, "_HueSatCon", new Vector4(num2, num3, num4, 0f));
			Vector4 vector2 = new Vector4(base.settings.mixerRedOutRedIn, base.settings.mixerRedOutGreenIn, base.settings.mixerRedOutBlueIn, 0f);
			Vector4 vector3 = new Vector4(base.settings.mixerGreenOutRedIn, base.settings.mixerGreenOutGreenIn, base.settings.mixerGreenOutBlueIn, 0f);
			Vector4 vector4 = new Vector4(base.settings.mixerBlueOutRedIn, base.settings.mixerBlueOutGreenIn, base.settings.mixerBlueOutBlueIn, 0f);
			command.SetComputeVectorParam(lut3DBaker, "_ChannelMixerRed", vector2 / 100f);
			command.SetComputeVectorParam(lut3DBaker, "_ChannelMixerGreen", vector3 / 100f);
			command.SetComputeVectorParam(lut3DBaker, "_ChannelMixerBlue", vector4 / 100f);
			Vector3 vector5 = ColorUtilities.ColorToLift(base.settings.lift.value * 0.2f);
			Vector3 vector6 = ColorUtilities.ColorToGain(base.settings.gain.value * 0.8f);
			Vector3 vector7 = ColorUtilities.ColorToInverseGamma(base.settings.gamma.value * 0.8f);
			command.SetComputeVectorParam(lut3DBaker, "_Lift", new Vector4(vector5.x, vector5.y, vector5.z, 0f));
			command.SetComputeVectorParam(lut3DBaker, "_InvGamma", new Vector4(vector7.x, vector7.y, vector7.z, 0f));
			command.SetComputeVectorParam(lut3DBaker, "_Gain", new Vector4(vector6.x, vector6.y, vector6.z, 0f));
			command.SetComputeTextureParam(lut3DBaker, num, "_Curves", this.GetCurveTexture(true));
			if (base.settings.tonemapper.value == Tonemapper.Custom)
			{
				this.m_HableCurve.Init(base.settings.toneCurveToeStrength.value, base.settings.toneCurveToeLength.value, base.settings.toneCurveShoulderStrength.value, base.settings.toneCurveShoulderLength.value, base.settings.toneCurveShoulderAngle.value, base.settings.toneCurveGamma.value);
				command.SetComputeVectorParam(lut3DBaker, "_CustomToneCurve", this.m_HableCurve.uniforms.curve);
				command.SetComputeVectorParam(lut3DBaker, "_ToeSegmentA", this.m_HableCurve.uniforms.toeSegmentA);
				command.SetComputeVectorParam(lut3DBaker, "_ToeSegmentB", this.m_HableCurve.uniforms.toeSegmentB);
				command.SetComputeVectorParam(lut3DBaker, "_MidSegmentA", this.m_HableCurve.uniforms.midSegmentA);
				command.SetComputeVectorParam(lut3DBaker, "_MidSegmentB", this.m_HableCurve.uniforms.midSegmentB);
				command.SetComputeVectorParam(lut3DBaker, "_ShoSegmentA", this.m_HableCurve.uniforms.shoSegmentA);
				command.SetComputeVectorParam(lut3DBaker, "_ShoSegmentB", this.m_HableCurve.uniforms.shoSegmentB);
			}
			context.command.BeginSample("HdrColorGradingLut3D");
			int num5 = Mathf.CeilToInt(8.25f);
			command.DispatchCompute(lut3DBaker, num, num5, num5, num5);
			context.command.EndSample("HdrColorGradingLut3D");
			RenderTexture internalLogLut = this.m_InternalLogLut;
			PropertySheet uberSheet = context.uberSheet;
			uberSheet.EnableKeyword("COLOR_GRADING_HDR_3D");
			uberSheet.properties.SetTexture(ShaderIDs.Lut3D, internalLogLut);
			uberSheet.properties.SetVector(ShaderIDs.Lut3D_Params, new Vector2(1f / (float)internalLogLut.width, (float)internalLogLut.width - 1f));
			uberSheet.properties.SetFloat(ShaderIDs.PostExposure, RuntimeUtilities.Exp2(base.settings.postExposure.value));
			context.logLut = internalLogLut;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00003FA0 File Offset: 0x000021A0
		private void RenderHDRPipeline2D(PostProcessRenderContext context)
		{
			this.CheckInternalStripLut();
			PropertySheet propertySheet = context.propertySheets.Get(context.resources.shaders.lut2DBaker);
			propertySheet.ClearKeywords();
			propertySheet.properties.SetVector(ShaderIDs.Lut2D_Params, new Vector4(32f, 0.00048828125f, 0.015625f, 1.032258f));
			Vector3 vector = ColorUtilities.ComputeColorBalance(base.settings.temperature.value, base.settings.tint.value);
			propertySheet.properties.SetVector(ShaderIDs.ColorBalance, vector);
			propertySheet.properties.SetVector(ShaderIDs.ColorFilter, base.settings.colorFilter.value);
			float num = base.settings.hueShift.value / 360f;
			float num2 = base.settings.saturation.value / 100f + 1f;
			float num3 = base.settings.contrast.value / 100f + 1f;
			propertySheet.properties.SetVector(ShaderIDs.HueSatCon, new Vector3(num, num2, num3));
			Vector3 vector2 = new Vector3(base.settings.mixerRedOutRedIn, base.settings.mixerRedOutGreenIn, base.settings.mixerRedOutBlueIn);
			Vector3 vector3 = new Vector3(base.settings.mixerGreenOutRedIn, base.settings.mixerGreenOutGreenIn, base.settings.mixerGreenOutBlueIn);
			Vector3 vector4 = new Vector3(base.settings.mixerBlueOutRedIn, base.settings.mixerBlueOutGreenIn, base.settings.mixerBlueOutBlueIn);
			propertySheet.properties.SetVector(ShaderIDs.ChannelMixerRed, vector2 / 100f);
			propertySheet.properties.SetVector(ShaderIDs.ChannelMixerGreen, vector3 / 100f);
			propertySheet.properties.SetVector(ShaderIDs.ChannelMixerBlue, vector4 / 100f);
			Vector3 vector5 = ColorUtilities.ColorToLift(base.settings.lift.value * 0.2f);
			Vector3 vector6 = ColorUtilities.ColorToGain(base.settings.gain.value * 0.8f);
			Vector3 vector7 = ColorUtilities.ColorToInverseGamma(base.settings.gamma.value * 0.8f);
			propertySheet.properties.SetVector(ShaderIDs.Lift, vector5);
			propertySheet.properties.SetVector(ShaderIDs.InvGamma, vector7);
			propertySheet.properties.SetVector(ShaderIDs.Gain, vector6);
			propertySheet.properties.SetTexture(ShaderIDs.Curves, this.GetCurveTexture(true));
			Tonemapper value = base.settings.tonemapper.value;
			if (value == Tonemapper.Custom)
			{
				propertySheet.EnableKeyword("TONEMAPPING_CUSTOM");
				this.m_HableCurve.Init(base.settings.toneCurveToeStrength.value, base.settings.toneCurveToeLength.value, base.settings.toneCurveShoulderStrength.value, base.settings.toneCurveShoulderLength.value, base.settings.toneCurveShoulderAngle.value, base.settings.toneCurveGamma.value);
				propertySheet.properties.SetVector(ShaderIDs.CustomToneCurve, this.m_HableCurve.uniforms.curve);
				propertySheet.properties.SetVector(ShaderIDs.ToeSegmentA, this.m_HableCurve.uniforms.toeSegmentA);
				propertySheet.properties.SetVector(ShaderIDs.ToeSegmentB, this.m_HableCurve.uniforms.toeSegmentB);
				propertySheet.properties.SetVector(ShaderIDs.MidSegmentA, this.m_HableCurve.uniforms.midSegmentA);
				propertySheet.properties.SetVector(ShaderIDs.MidSegmentB, this.m_HableCurve.uniforms.midSegmentB);
				propertySheet.properties.SetVector(ShaderIDs.ShoSegmentA, this.m_HableCurve.uniforms.shoSegmentA);
				propertySheet.properties.SetVector(ShaderIDs.ShoSegmentB, this.m_HableCurve.uniforms.shoSegmentB);
			}
			else if (value == Tonemapper.ACES)
			{
				propertySheet.EnableKeyword("TONEMAPPING_ACES");
			}
			else if (value == Tonemapper.Neutral)
			{
				propertySheet.EnableKeyword("TONEMAPPING_NEUTRAL");
			}
			context.command.BeginSample("HdrColorGradingLut2D");
			context.command.BlitFullscreenTriangle(BuiltinRenderTextureType.None, this.m_InternalLdrLut, propertySheet, 2, false, null, false);
			context.command.EndSample("HdrColorGradingLut2D");
			RenderTexture internalLdrLut = this.m_InternalLdrLut;
			PropertySheet uberSheet = context.uberSheet;
			uberSheet.EnableKeyword("COLOR_GRADING_HDR_2D");
			uberSheet.properties.SetVector(ShaderIDs.Lut2D_Params, new Vector3(1f / (float)internalLdrLut.width, 1f / (float)internalLdrLut.height, (float)internalLdrLut.height - 1f));
			uberSheet.properties.SetTexture(ShaderIDs.Lut2D, internalLdrLut);
			uberSheet.properties.SetFloat(ShaderIDs.PostExposure, RuntimeUtilities.Exp2(base.settings.postExposure.value));
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00004504 File Offset: 0x00002704
		private void RenderLDRPipeline2D(PostProcessRenderContext context)
		{
			this.CheckInternalStripLut();
			PropertySheet propertySheet = context.propertySheets.Get(context.resources.shaders.lut2DBaker);
			propertySheet.ClearKeywords();
			propertySheet.properties.SetVector(ShaderIDs.Lut2D_Params, new Vector4(32f, 0.00048828125f, 0.015625f, 1.032258f));
			Vector3 vector = ColorUtilities.ComputeColorBalance(base.settings.temperature.value, base.settings.tint.value);
			propertySheet.properties.SetVector(ShaderIDs.ColorBalance, vector);
			propertySheet.properties.SetVector(ShaderIDs.ColorFilter, base.settings.colorFilter.value);
			float num = base.settings.hueShift.value / 360f;
			float num2 = base.settings.saturation.value / 100f + 1f;
			float num3 = base.settings.contrast.value / 100f + 1f;
			propertySheet.properties.SetVector(ShaderIDs.HueSatCon, new Vector3(num, num2, num3));
			Vector3 vector2 = new Vector3(base.settings.mixerRedOutRedIn, base.settings.mixerRedOutGreenIn, base.settings.mixerRedOutBlueIn);
			Vector3 vector3 = new Vector3(base.settings.mixerGreenOutRedIn, base.settings.mixerGreenOutGreenIn, base.settings.mixerGreenOutBlueIn);
			Vector3 vector4 = new Vector3(base.settings.mixerBlueOutRedIn, base.settings.mixerBlueOutGreenIn, base.settings.mixerBlueOutBlueIn);
			propertySheet.properties.SetVector(ShaderIDs.ChannelMixerRed, vector2 / 100f);
			propertySheet.properties.SetVector(ShaderIDs.ChannelMixerGreen, vector3 / 100f);
			propertySheet.properties.SetVector(ShaderIDs.ChannelMixerBlue, vector4 / 100f);
			Vector3 vector5 = ColorUtilities.ColorToLift(base.settings.lift.value);
			Vector3 vector6 = ColorUtilities.ColorToGain(base.settings.gain.value);
			Vector3 vector7 = ColorUtilities.ColorToInverseGamma(base.settings.gamma.value);
			propertySheet.properties.SetVector(ShaderIDs.Lift, vector5);
			propertySheet.properties.SetVector(ShaderIDs.InvGamma, vector7);
			propertySheet.properties.SetVector(ShaderIDs.Gain, vector6);
			propertySheet.properties.SetFloat(ShaderIDs.Brightness, (base.settings.brightness.value + 100f) / 100f);
			propertySheet.properties.SetTexture(ShaderIDs.Curves, this.GetCurveTexture(false));
			context.command.BeginSample("LdrColorGradingLut2D");
			Texture value = base.settings.ldrLut.value;
			if (value == null || value.width != value.height * value.height)
			{
				context.command.BlitFullscreenTriangle(BuiltinRenderTextureType.None, this.m_InternalLdrLut, propertySheet, 0, false, null, false);
			}
			else
			{
				propertySheet.properties.SetVector(ShaderIDs.UserLut2D_Params, new Vector4(1f / (float)value.width, 1f / (float)value.height, (float)value.height - 1f, base.settings.ldrLutContribution));
				context.command.BlitFullscreenTriangle(value, this.m_InternalLdrLut, propertySheet, 1, false, null, false);
			}
			context.command.EndSample("LdrColorGradingLut2D");
			RenderTexture internalLdrLut = this.m_InternalLdrLut;
			PropertySheet uberSheet = context.uberSheet;
			uberSheet.EnableKeyword("COLOR_GRADING_LDR_2D");
			uberSheet.properties.SetVector(ShaderIDs.Lut2D_Params, new Vector3(1f / (float)internalLdrLut.width, 1f / (float)internalLdrLut.height, (float)internalLdrLut.height - 1f));
			uberSheet.properties.SetTexture(ShaderIDs.Lut2D, internalLdrLut);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00004970 File Offset: 0x00002B70
		private void CheckInternalLogLut()
		{
			if (this.m_InternalLogLut == null || !this.m_InternalLogLut.IsCreated())
			{
				RuntimeUtilities.Destroy(this.m_InternalLogLut);
				RenderTextureFormat lutFormat = ColorGradingRenderer.GetLutFormat();
				this.m_InternalLogLut = new RenderTexture(33, 33, 0, lutFormat, RenderTextureReadWrite.Linear)
				{
					name = "Color Grading Log Lut",
					dimension = TextureDimension.Tex3D,
					hideFlags = HideFlags.DontSave,
					filterMode = FilterMode.Bilinear,
					wrapMode = TextureWrapMode.Clamp,
					anisoLevel = 0,
					enableRandomWrite = true,
					volumeDepth = 33,
					autoGenerateMips = false,
					useMipMap = false
				};
				this.m_InternalLogLut.Create();
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00004A14 File Offset: 0x00002C14
		private void CheckInternalStripLut()
		{
			if (this.m_InternalLdrLut == null || !this.m_InternalLdrLut.IsCreated())
			{
				RuntimeUtilities.Destroy(this.m_InternalLdrLut);
				RenderTextureFormat lutFormat = ColorGradingRenderer.GetLutFormat();
				this.m_InternalLdrLut = new RenderTexture(1024, 32, 0, lutFormat, RenderTextureReadWrite.Linear)
				{
					name = "Color Grading Strip Lut",
					hideFlags = HideFlags.DontSave,
					filterMode = FilterMode.Bilinear,
					wrapMode = TextureWrapMode.Clamp,
					anisoLevel = 0,
					autoGenerateMips = false,
					useMipMap = false
				};
				this.m_InternalLdrLut.Create();
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00004AA4 File Offset: 0x00002CA4
		private Texture2D GetCurveTexture(bool hdr)
		{
			if (this.m_GradingCurves == null)
			{
				TextureFormat curveFormat = ColorGradingRenderer.GetCurveFormat();
				this.m_GradingCurves = new Texture2D(128, 2, curveFormat, false, true)
				{
					name = "Internal Curves Texture",
					hideFlags = HideFlags.DontSave,
					anisoLevel = 0,
					wrapMode = TextureWrapMode.Clamp,
					filterMode = FilterMode.Bilinear
				};
			}
			Spline value = base.settings.hueVsHueCurve.value;
			Spline value2 = base.settings.hueVsSatCurve.value;
			Spline value3 = base.settings.satVsSatCurve.value;
			Spline value4 = base.settings.lumVsSatCurve.value;
			Spline value5 = base.settings.masterCurve.value;
			Spline value6 = base.settings.redCurve.value;
			Spline value7 = base.settings.greenCurve.value;
			Spline value8 = base.settings.blueCurve.value;
			Color[] pixels = this.m_Pixels;
			for (int i = 0; i < 128; i++)
			{
				float num = value.cachedData[i];
				float num2 = value2.cachedData[i];
				float num3 = value3.cachedData[i];
				float num4 = value4.cachedData[i];
				pixels[i] = new Color(num, num2, num3, num4);
				if (!hdr)
				{
					float num5 = value5.cachedData[i];
					float num6 = value6.cachedData[i];
					float num7 = value7.cachedData[i];
					float num8 = value8.cachedData[i];
					pixels[i + 128] = new Color(num6, num7, num8, num5);
				}
			}
			this.m_GradingCurves.SetPixels(pixels);
			this.m_GradingCurves.Apply(false, false);
			return this.m_GradingCurves;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00004C62 File Offset: 0x00002E62
		private static bool IsRenderTextureFormatSupportedForLinearFiltering(RenderTextureFormat format)
		{
			return SystemInfo.IsFormatSupported(GraphicsFormatUtility.GetGraphicsFormat(format, RenderTextureReadWrite.Linear), FormatUsage.Linear);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00004C74 File Offset: 0x00002E74
		private static RenderTextureFormat GetLutFormat()
		{
			RenderTextureFormat renderTextureFormat = RenderTextureFormat.ARGBHalf;
			if (!ColorGradingRenderer.IsRenderTextureFormatSupportedForLinearFiltering(renderTextureFormat))
			{
				renderTextureFormat = RenderTextureFormat.ARGB2101010;
				if (!ColorGradingRenderer.IsRenderTextureFormatSupportedForLinearFiltering(renderTextureFormat))
				{
					renderTextureFormat = RenderTextureFormat.ARGB32;
				}
			}
			return renderTextureFormat;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00004C98 File Offset: 0x00002E98
		private static TextureFormat GetCurveFormat()
		{
			TextureFormat textureFormat = TextureFormat.RGBAHalf;
			if (!SystemInfo.SupportsTextureFormat(textureFormat))
			{
				textureFormat = TextureFormat.ARGB32;
			}
			return textureFormat;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00004CB3 File Offset: 0x00002EB3
		public override void Release()
		{
			RuntimeUtilities.Destroy(this.m_InternalLdrLut);
			this.m_InternalLdrLut = null;
			RuntimeUtilities.Destroy(this.m_InternalLogLut);
			this.m_InternalLogLut = null;
			RuntimeUtilities.Destroy(this.m_GradingCurves);
			this.m_GradingCurves = null;
		}

		// Token: 0x04000070 RID: 112
		private Texture2D m_GradingCurves;

		// Token: 0x04000071 RID: 113
		private readonly Color[] m_Pixels = new Color[256];

		// Token: 0x04000072 RID: 114
		private RenderTexture m_InternalLdrLut;

		// Token: 0x04000073 RID: 115
		private RenderTexture m_InternalLogLut;

		// Token: 0x04000074 RID: 116
		private const int k_Lut2DSize = 32;

		// Token: 0x04000075 RID: 117
		private const int k_Lut3DSize = 33;

		// Token: 0x04000076 RID: 118
		private readonly HableCurve m_HableCurve = new HableCurve();

		// Token: 0x0200006C RID: 108
		private enum Pass
		{
			// Token: 0x0400025B RID: 603
			LutGenLDRFromScratch,
			// Token: 0x0400025C RID: 604
			LutGenLDR,
			// Token: 0x0400025D RID: 605
			LutGenHDR2D
		}
	}
}
