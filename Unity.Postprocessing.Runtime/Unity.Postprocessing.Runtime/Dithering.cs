using System;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering.PostProcessing
{
	// Token: 0x02000021 RID: 33
	[Preserve]
	[Serializable]
	internal sealed class Dithering
	{
		// Token: 0x06000046 RID: 70 RVA: 0x00005310 File Offset: 0x00003510
		internal void Render(PostProcessRenderContext context)
		{
			Texture2D[] blueNoise = context.resources.blueNoise64;
			int num = this.m_NoiseTextureIndex + 1;
			this.m_NoiseTextureIndex = num;
			if (num >= blueNoise.Length)
			{
				this.m_NoiseTextureIndex = 0;
			}
			float num2 = (float)this.m_Random.NextDouble();
			float num3 = (float)this.m_Random.NextDouble();
			Texture2D texture2D = blueNoise[this.m_NoiseTextureIndex];
			PropertySheet uberSheet = context.uberSheet;
			uberSheet.properties.SetTexture(ShaderIDs.DitheringTex, texture2D);
			uberSheet.properties.SetVector(ShaderIDs.Dithering_Coords, new Vector4((float)context.screenWidth / (float)texture2D.width, (float)context.screenHeight / (float)texture2D.height, num2, num3));
		}

		// Token: 0x04000085 RID: 133
		private int m_NoiseTextureIndex;

		// Token: 0x04000086 RID: 134
		private Random m_Random = new Random(1234);
	}
}
