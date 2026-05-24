using System;

namespace UnityEngine.Rendering.PostProcessing
{
	// Token: 0x02000039 RID: 57
	[Serializable]
	public sealed class LightMeterMonitor : Monitor
	{
		// Token: 0x060000A2 RID: 162 RVA: 0x00008C8E File Offset: 0x00006E8E
		internal override bool ShaderResourcesAvailable(PostProcessRenderContext context)
		{
			return context.resources.shaders.lightMeter && context.resources.shaders.lightMeter.isSupported;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00008CC0 File Offset: 0x00006EC0
		internal override void Render(PostProcessRenderContext context)
		{
			base.CheckOutput(this.width, this.height);
			LogHistogram logHistogram = context.logHistogram;
			PropertySheet propertySheet = context.propertySheets.Get(context.resources.shaders.lightMeter);
			propertySheet.ClearKeywords();
			propertySheet.properties.SetBuffer(ShaderIDs.HistogramBuffer, logHistogram.data);
			Vector4 histogramScaleOffsetRes = logHistogram.GetHistogramScaleOffsetRes(context);
			histogramScaleOffsetRes.z = 1f / (float)this.width;
			histogramScaleOffsetRes.w = 1f / (float)this.height;
			propertySheet.properties.SetVector(ShaderIDs.ScaleOffsetRes, histogramScaleOffsetRes);
			if (context.logLut != null && this.showCurves)
			{
				propertySheet.EnableKeyword("COLOR_GRADING_HDR");
				propertySheet.properties.SetTexture(ShaderIDs.Lut3D, context.logLut);
			}
			AutoExposure autoExposure = context.autoExposure;
			if (autoExposure != null)
			{
				float num = autoExposure.filtering.value.x;
				float num2 = autoExposure.filtering.value.y;
				num2 = Mathf.Clamp(num2, 1.01f, 99f);
				num = Mathf.Clamp(num, 1f, num2 - 0.01f);
				Vector4 vector = new Vector4(num * 0.01f, num2 * 0.01f, RuntimeUtilities.Exp2(autoExposure.minLuminance.value), RuntimeUtilities.Exp2(autoExposure.maxLuminance.value));
				propertySheet.EnableKeyword("AUTO_EXPOSURE");
				propertySheet.properties.SetVector(ShaderIDs.Params, vector);
			}
			CommandBuffer command = context.command;
			command.BeginSample("LightMeter");
			command.BlitFullscreenTriangle(BuiltinRenderTextureType.None, base.output, propertySheet, 0, false, null, false);
			command.EndSample("LightMeter");
		}

		// Token: 0x040000E5 RID: 229
		public int width = 512;

		// Token: 0x040000E6 RID: 230
		public int height = 256;

		// Token: 0x040000E7 RID: 231
		public bool showCurves = true;
	}
}
