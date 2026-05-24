using System;

namespace UnityEngine.Rendering.PostProcessing
{
	// Token: 0x02000026 RID: 38
	[PostProcess(typeof(LensDistortionRenderer), "Unity/Lens Distortion", true)]
	[Serializable]
	public sealed class LensDistortion : PostProcessEffectSettings
	{
		// Token: 0x06000053 RID: 83 RVA: 0x00005824 File Offset: 0x00003A24
		public override bool IsEnabledAndSupported(PostProcessRenderContext context)
		{
			return this.enabled.value && !Mathf.Approximately(this.intensity, 0f) && (this.intensityX > 0f || this.intensityY > 0f) && !context.stereoActive;
		}

		// Token: 0x04000092 RID: 146
		[Range(-100f, 100f)]
		[Tooltip("Total distortion amount.")]
		public FloatParameter intensity = new FloatParameter
		{
			value = 0f
		};

		// Token: 0x04000093 RID: 147
		[Range(0f, 1f)]
		[DisplayName("X Multiplier")]
		[Tooltip("Intensity multiplier on the x-axis. Set it to 0 to disable distortion on this axis.")]
		public FloatParameter intensityX = new FloatParameter
		{
			value = 1f
		};

		// Token: 0x04000094 RID: 148
		[Range(0f, 1f)]
		[DisplayName("Y Multiplier")]
		[Tooltip("Intensity multiplier on the y-axis. Set it to 0 to disable distortion on this axis.")]
		public FloatParameter intensityY = new FloatParameter
		{
			value = 1f
		};

		// Token: 0x04000095 RID: 149
		[Space]
		[Range(-1f, 1f)]
		[Tooltip("Distortion center point (x-axis).")]
		public FloatParameter centerX = new FloatParameter
		{
			value = 0f
		};

		// Token: 0x04000096 RID: 150
		[Range(-1f, 1f)]
		[Tooltip("Distortion center point (y-axis).")]
		public FloatParameter centerY = new FloatParameter
		{
			value = 0f
		};

		// Token: 0x04000097 RID: 151
		[Space]
		[Range(0.01f, 5f)]
		[Tooltip("Global screen scaling.")]
		public FloatParameter scale = new FloatParameter
		{
			value = 1f
		};
	}
}
