using System;

namespace UnityEngine.Rendering.PostProcessing
{
	// Token: 0x0200003D RID: 61
	[Serializable]
	public sealed class WaveformMonitor : Monitor
	{
		// Token: 0x060000B4 RID: 180 RVA: 0x000091ED File Offset: 0x000073ED
		internal override void OnDisable()
		{
			base.OnDisable();
			if (this.m_Data != null)
			{
				this.m_Data.Release();
			}
			this.m_Data = null;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x0000920F File Offset: 0x0000740F
		internal override bool NeedsHalfRes()
		{
			return true;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00009212 File Offset: 0x00007412
		internal override bool ShaderResourcesAvailable(PostProcessRenderContext context)
		{
			return context.resources.computeShaders.waveform;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x0000922C File Offset: 0x0000742C
		internal override void Render(PostProcessRenderContext context)
		{
			float num = (float)context.width / 2f / ((float)context.height / 2f);
			int num2 = Mathf.FloorToInt((float)this.height * num);
			base.CheckOutput(num2, this.height);
			this.exposure = Mathf.Max(0f, this.exposure);
			int num3 = num2 * this.height;
			if (this.m_Data == null)
			{
				this.m_Data = new ComputeBuffer(num3, 16);
			}
			else if (this.m_Data.count < num3)
			{
				this.m_Data.Release();
				this.m_Data = new ComputeBuffer(num3, 16);
			}
			ComputeShader waveform = context.resources.computeShaders.waveform;
			CommandBuffer command = context.command;
			command.BeginSample("Waveform");
			Vector4 vector = new Vector4((float)num2, (float)this.height, (float)(RuntimeUtilities.isLinearColorSpace ? 1 : 0), 0f);
			int num4 = waveform.FindKernel("KWaveformClear");
			command.SetComputeBufferParam(waveform, num4, "_WaveformBuffer", this.m_Data);
			command.SetComputeVectorParam(waveform, "_Params", vector);
			command.DispatchCompute(waveform, num4, Mathf.CeilToInt((float)num2 / 16f), Mathf.CeilToInt((float)this.height / 16f), 1);
			command.GetTemporaryRT(ShaderIDs.WaveformSource, num2, this.height, 0, FilterMode.Bilinear, context.sourceFormat);
			command.BlitFullscreenTriangle(ShaderIDs.HalfResFinalCopy, ShaderIDs.WaveformSource, false, null, false);
			num4 = waveform.FindKernel("KWaveformGather");
			command.SetComputeBufferParam(waveform, num4, "_WaveformBuffer", this.m_Data);
			command.SetComputeTextureParam(waveform, num4, "_Source", ShaderIDs.WaveformSource);
			command.SetComputeVectorParam(waveform, "_Params", vector);
			command.DispatchCompute(waveform, num4, num2, Mathf.CeilToInt((float)this.height / 256f), 1);
			command.ReleaseTemporaryRT(ShaderIDs.WaveformSource);
			PropertySheet propertySheet = context.propertySheets.Get(context.resources.shaders.waveform);
			propertySheet.properties.SetVector(ShaderIDs.Params, new Vector4((float)num2, (float)this.height, this.exposure, 0f));
			propertySheet.properties.SetBuffer(ShaderIDs.WaveformBuffer, this.m_Data);
			command.BlitFullscreenTriangle(BuiltinRenderTextureType.None, base.output, propertySheet, 0, false, null, false);
			command.EndSample("Waveform");
		}

		// Token: 0x040000F4 RID: 244
		public float exposure = 0.12f;

		// Token: 0x040000F5 RID: 245
		public int height = 256;

		// Token: 0x040000F6 RID: 246
		private ComputeBuffer m_Data;

		// Token: 0x040000F7 RID: 247
		private const int k_ThreadGroupSize = 256;

		// Token: 0x040000F8 RID: 248
		private const int k_ThreadGroupSizeX = 16;

		// Token: 0x040000F9 RID: 249
		private const int k_ThreadGroupSizeY = 16;
	}
}
