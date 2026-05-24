using System;
using UnityEngine;

// Token: 0x02000162 RID: 354
public class WeatherSystemSettings : ScriptableObject
{
	// Token: 0x04000E1C RID: 3612
	public bool useSimpleWeather;

	// Token: 0x04000E1D RID: 3613
	public WeatherPattern gameStartWeatherPattern;

	// Token: 0x04000E1E RID: 3614
	public WeatherPattern[] weatherPatterns;

	// Token: 0x04000E1F RID: 3615
	public float hoursBetweenLevels = 2f;

	// Token: 0x04000E20 RID: 3616
	public float inkGlobalOverrideHours = 3f;

	// Token: 0x04000E21 RID: 3617
	public WeatherSystemSettings.WeatherMapTextureSettings weatherMapTextureSettings;

	// Token: 0x04000E22 RID: 3618
	public WeatherSystemSettings.CloudShadowSettings cloudShadowSettings;

	// Token: 0x04000E23 RID: 3619
	public CloudGroupSettings cloudGroupSettings;

	// Token: 0x04000E24 RID: 3620
	[Space]
	public float offsetPerHour = 0.5f;

	// Token: 0x04000E25 RID: 3621
	[Space]
	public float weatherBoundsDepthPast = 0.25f;

	// Token: 0x04000E26 RID: 3622
	public float weatherBoundsDepthFuture = 3f;

	// Token: 0x02000394 RID: 916
	[Serializable]
	public class WeatherMapTextureSettings
	{
		// Token: 0x04001953 RID: 6483
		public bool applyWeatherMapInEditor = true;

		// Token: 0x04001954 RID: 6484
		public Material blurMaterial;

		// Token: 0x04001955 RID: 6485
		[Range(1f, 32f)]
		public int blurUpscaling = 8;

		// Token: 0x04001956 RID: 6486
		public float blendSpeed = 1f;
	}

	// Token: 0x02000395 RID: 917
	[Serializable]
	public class CloudShadowSettings
	{
		// Token: 0x04001957 RID: 6487
		public Texture2D cloudShadowTexture;

		// Token: 0x04001958 RID: 6488
		public float minimumCloudShadow;
	}
}
