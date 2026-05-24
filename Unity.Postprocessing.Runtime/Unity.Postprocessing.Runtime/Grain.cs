using System;

namespace UnityEngine.Rendering.PostProcessing
{
	// Token: 0x02000024 RID: 36
	[PostProcess(typeof(GrainRenderer), "Unity/Grain", true)]
	[Serializable]
	public sealed class Grain : PostProcessEffectSettings
	{
		// Token: 0x0600004D RID: 77 RVA: 0x0000550F File Offset: 0x0000370F
		public override bool IsEnabledAndSupported(PostProcessRenderContext context)
		{
			return this.enabled.value && this.intensity.value > 0f;
		}

		// Token: 0x0400008B RID: 139
		[Tooltip("Enable the use of colored grain.")]
		public BoolParameter colored = new BoolParameter
		{
			value = true
		};

		// Token: 0x0400008C RID: 140
		[Range(0f, 1f)]
		[Tooltip("Grain strength. Higher values mean more visible grain.")]
		public FloatParameter intensity = new FloatParameter
		{
			value = 0f
		};

		// Token: 0x0400008D RID: 141
		[Range(0.3f, 3f)]
		[Tooltip("Grain particle size.")]
		public FloatParameter size = new FloatParameter
		{
			value = 1f
		};

		// Token: 0x0400008E RID: 142
		[Range(0f, 1f)]
		[DisplayName("Luminance Contribution")]
		[Tooltip("Controls the noise response curve based on scene luminance. Lower values mean less noise in dark areas.")]
		public FloatParameter lumContrib = new FloatParameter
		{
			value = 0.8f
		};
	}
}
