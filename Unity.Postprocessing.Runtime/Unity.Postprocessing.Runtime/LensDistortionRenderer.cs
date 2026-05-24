using System;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering.PostProcessing
{
	// Token: 0x02000027 RID: 39
	[Preserve]
	internal sealed class LensDistortionRenderer : PostProcessEffectRenderer<LensDistortion>
	{
		// Token: 0x06000055 RID: 85 RVA: 0x0000591C File Offset: 0x00003B1C
		public override void Render(PostProcessRenderContext context)
		{
			PropertySheet uberSheet = context.uberSheet;
			float num = 1.6f * Math.Max(Mathf.Abs(base.settings.intensity.value), 1f);
			float num2 = 0.017453292f * Math.Min(160f, num);
			float num3 = 2f * Mathf.Tan(num2 * 0.5f);
			Vector4 vector = new Vector4(base.settings.centerX.value, base.settings.centerY.value, Mathf.Max(base.settings.intensityX.value, 0.0001f), Mathf.Max(base.settings.intensityY.value, 0.0001f));
			Vector4 vector2 = new Vector4((base.settings.intensity.value >= 0f) ? num2 : (1f / num2), num3, 1f / base.settings.scale.value, base.settings.intensity.value);
			uberSheet.EnableKeyword("DISTORT");
			uberSheet.properties.SetVector(ShaderIDs.Distortion_CenterScale, vector);
			uberSheet.properties.SetVector(ShaderIDs.Distortion_Amount, vector2);
		}
	}
}
