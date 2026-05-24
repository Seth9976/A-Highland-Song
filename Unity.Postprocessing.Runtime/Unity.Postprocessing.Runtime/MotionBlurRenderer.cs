using System;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering.PostProcessing
{
	// Token: 0x02000029 RID: 41
	[Preserve]
	internal sealed class MotionBlurRenderer : PostProcessEffectRenderer<MotionBlur>
	{
		// Token: 0x06000059 RID: 89 RVA: 0x00005AC6 File Offset: 0x00003CC6
		public override DepthTextureMode GetCameraFlags()
		{
			return DepthTextureMode.Depth | DepthTextureMode.MotionVectors;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00005ACC File Offset: 0x00003CCC
		private void CreateTemporaryRT(PostProcessRenderContext context, int nameID, int width, int height, RenderTextureFormat RTFormat)
		{
			CommandBuffer command = context.command;
			RenderTextureDescriptor descriptor = context.GetDescriptor(0, RTFormat, RenderTextureReadWrite.Linear);
			descriptor.width = width;
			descriptor.height = height;
			command.GetTemporaryRT(nameID, descriptor, FilterMode.Point);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00005B04 File Offset: 0x00003D04
		public override void Render(PostProcessRenderContext context)
		{
			CommandBuffer command = context.command;
			if (this.m_ResetHistory)
			{
				command.BlitFullscreenTriangle(context.source, context.destination, false, null, false);
				this.m_ResetHistory = false;
				return;
			}
			RenderTextureFormat renderTextureFormat = RenderTextureFormat.RGHalf;
			RenderTextureFormat renderTextureFormat2 = (RenderTextureFormat.ARGB2101010.IsSupported() ? RenderTextureFormat.ARGB2101010 : RenderTextureFormat.ARGB32);
			PropertySheet propertySheet = context.propertySheets.Get(context.resources.shaders.motionBlur);
			command.BeginSample("MotionBlur");
			int num = (int)(5f * (float)context.height / 100f);
			int num2 = ((num - 1) / 8 + 1) * 8;
			float num3 = base.settings.shutterAngle / 360f;
			propertySheet.properties.SetFloat(ShaderIDs.VelocityScale, num3);
			propertySheet.properties.SetFloat(ShaderIDs.MaxBlurRadius, (float)num);
			propertySheet.properties.SetFloat(ShaderIDs.RcpMaxBlurRadius, 1f / (float)num);
			int velocityTex = ShaderIDs.VelocityTex;
			this.CreateTemporaryRT(context, velocityTex, context.width, context.height, renderTextureFormat2);
			command.BlitFullscreenTriangle(BuiltinRenderTextureType.None, velocityTex, propertySheet, 0, false, null, false);
			int tile2RT = ShaderIDs.Tile2RT;
			this.CreateTemporaryRT(context, tile2RT, context.width / 2, context.height / 2, renderTextureFormat);
			command.BlitFullscreenTriangle(velocityTex, tile2RT, propertySheet, 1, false, null, false);
			int tile4RT = ShaderIDs.Tile4RT;
			this.CreateTemporaryRT(context, tile4RT, context.width / 4, context.height / 4, renderTextureFormat);
			command.BlitFullscreenTriangle(tile2RT, tile4RT, propertySheet, 2, false, null, false);
			command.ReleaseTemporaryRT(tile2RT);
			int tile8RT = ShaderIDs.Tile8RT;
			this.CreateTemporaryRT(context, tile8RT, context.width / 8, context.height / 8, renderTextureFormat);
			command.BlitFullscreenTriangle(tile4RT, tile8RT, propertySheet, 2, false, null, false);
			command.ReleaseTemporaryRT(tile4RT);
			Vector2 vector = Vector2.one * ((float)num2 / 8f - 1f) * -0.5f;
			propertySheet.properties.SetVector(ShaderIDs.TileMaxOffs, vector);
			propertySheet.properties.SetFloat(ShaderIDs.TileMaxLoop, (float)((int)((float)num2 / 8f)));
			int tileVRT = ShaderIDs.TileVRT;
			this.CreateTemporaryRT(context, tileVRT, context.width / num2, context.height / num2, renderTextureFormat);
			command.BlitFullscreenTriangle(tile8RT, tileVRT, propertySheet, 3, false, null, false);
			command.ReleaseTemporaryRT(tile8RT);
			int neighborMaxTex = ShaderIDs.NeighborMaxTex;
			this.CreateTemporaryRT(context, neighborMaxTex, context.width / num2, context.height / num2, renderTextureFormat);
			command.BlitFullscreenTriangle(tileVRT, neighborMaxTex, propertySheet, 4, false, null, false);
			command.ReleaseTemporaryRT(tileVRT);
			propertySheet.properties.SetFloat(ShaderIDs.LoopCount, (float)Mathf.Clamp(base.settings.sampleCount / 2, 1, 64));
			command.BlitFullscreenTriangle(context.source, context.destination, propertySheet, 5, false, null, false);
			command.ReleaseTemporaryRT(velocityTex);
			command.ReleaseTemporaryRT(neighborMaxTex);
			command.EndSample("MotionBlur");
		}

		// Token: 0x0200006E RID: 110
		private enum Pass
		{
			// Token: 0x0400026A RID: 618
			VelocitySetup,
			// Token: 0x0400026B RID: 619
			TileMax1,
			// Token: 0x0400026C RID: 620
			TileMax2,
			// Token: 0x0400026D RID: 621
			TileMaxV,
			// Token: 0x0400026E RID: 622
			NeighborMax,
			// Token: 0x0400026F RID: 623
			Reconstruction
		}
	}
}
