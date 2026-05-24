using System;

namespace UnityEngine.Rendering.PostProcessing
{
	// Token: 0x02000030 RID: 48
	[PostProcess(typeof(ScreenSpaceReflectionsRenderer), "Unity/Screen-space reflections", true)]
	[Serializable]
	public sealed class ScreenSpaceReflections : PostProcessEffectSettings
	{
		// Token: 0x0600007E RID: 126 RVA: 0x000076E4 File Offset: 0x000058E4
		public override bool IsEnabledAndSupported(PostProcessRenderContext context)
		{
			return this.enabled && context.camera.actualRenderingPath == RenderingPath.DeferredShading && SystemInfo.supportsMotionVectors && SystemInfo.supportsComputeShaders && SystemInfo.copyTextureSupport > CopyTextureSupport.None && context.resources.shaders.screenSpaceReflections && context.resources.shaders.screenSpaceReflections.isSupported && context.resources.computeShaders.gaussianDownsample;
		}

		// Token: 0x040000B8 RID: 184
		[Tooltip("Choose a quality preset, or use \"Custom\" to create your own custom preset. Don't use a preset higher than \"Medium\" if you desire good performance on consoles.")]
		public ScreenSpaceReflectionPresetParameter preset = new ScreenSpaceReflectionPresetParameter
		{
			value = ScreenSpaceReflectionPreset.Medium
		};

		// Token: 0x040000B9 RID: 185
		[Range(0f, 256f)]
		[Tooltip("Maximum number of steps in the raymarching pass. Higher values mean more reflections.")]
		public IntParameter maximumIterationCount = new IntParameter
		{
			value = 16
		};

		// Token: 0x040000BA RID: 186
		[Tooltip("Changes the size of the SSR buffer. Downsample it to maximize performances or supersample it for higher quality results with reduced performance.")]
		public ScreenSpaceReflectionResolutionParameter resolution = new ScreenSpaceReflectionResolutionParameter
		{
			value = ScreenSpaceReflectionResolution.Downsampled
		};

		// Token: 0x040000BB RID: 187
		[Range(1f, 64f)]
		[Tooltip("Ray thickness. Lower values are more expensive but allow the effect to detect smaller details.")]
		public FloatParameter thickness = new FloatParameter
		{
			value = 8f
		};

		// Token: 0x040000BC RID: 188
		[Tooltip("Maximum distance to traverse after which it will stop drawing reflections.")]
		public FloatParameter maximumMarchDistance = new FloatParameter
		{
			value = 100f
		};

		// Token: 0x040000BD RID: 189
		[Range(0f, 1f)]
		[Tooltip("Fades reflections close to the near planes.")]
		public FloatParameter distanceFade = new FloatParameter
		{
			value = 0.5f
		};

		// Token: 0x040000BE RID: 190
		[Range(0f, 1f)]
		[Tooltip("Fades reflections close to the screen edges.")]
		public FloatParameter vignette = new FloatParameter
		{
			value = 0.5f
		};
	}
}
