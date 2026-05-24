using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200004B RID: 75
[ExecuteInEditMode]
public class TimeOfDayEffects : MonoBehaviour
{
	// Token: 0x06000210 RID: 528 RVA: 0x00012250 File Offset: 0x00010450
	public static T EvaluateStops<T>(IList<T> stops, Func<T, float> getHourFunc, Func<T, T, float, T> lerpFunc, float timeOfDayNorm)
	{
		float num = timeOfDayNorm * 24f;
		float num2 = 24f;
		int num3 = -1;
		for (int i = 0; i < stops.Count; i++)
		{
			T t = stops[i];
			float num4;
			for (num4 = getHourFunc(t) - num; num4 > 0f; num4 -= 24f)
			{
			}
			if (-num4 < num2 || num3 == -1)
			{
				num2 = -num4;
				num3 = i;
			}
		}
		int num5 = (num3 + 1) % stops.Count;
		float num6;
		for (num6 = getHourFunc(stops[num3]) - num + 24f; num6 > 0f; num6 -= 24f)
		{
		}
		float num7;
		for (num7 = getHourFunc(stops[num5]) - num - 24f; num7 < 0f; num7 += 24f)
		{
		}
		float num8 = Mathf.InverseLerp(num6, num7, 0f);
		T t2 = stops[num3];
		T t3 = stops[num5];
		return lerpFunc(t2, t3, num8);
	}

	// Token: 0x06000211 RID: 529 RVA: 0x0001234F File Offset: 0x0001054F
	private void OnValidate()
	{
		TimeOfDayEffects.AssertShaderIDsSet();
	}

	// Token: 0x06000212 RID: 530 RVA: 0x00012356 File Offset: 0x00010556
	private void Awake()
	{
		TimeOfDayEffects.AssertShaderIDsSet();
	}

	// Token: 0x06000213 RID: 531 RVA: 0x00012360 File Offset: 0x00010560
	private void Update()
	{
		if (Application.isPlaying)
		{
			this.moon.theta = (this.stars.theta = GameClock.instance.timeOfDayNorm * 360f);
			this.sun.theta = 180f + GameClock.instance.timeOfDayNorm * 360f;
		}
	}

	// Token: 0x06000214 RID: 532 RVA: 0x000123C0 File Offset: 0x000105C0
	private void OnEnable()
	{
		if (Application.isPlaying)
		{
			this.SetImmediate();
			this._mutableSkyboxMaterial = new Material(this.skyboxMaterial);
			this._mutableSkyboxMaterial.hideFlags = HideFlags.DontSave;
			RenderSettings.skybox = this._mutableSkyboxMaterial;
		}
		if (!Application.isPlaying)
		{
			Shader.SetGlobalColor(TimeOfDayEffects._multiplyColourId, Color.white);
			Shader.SetGlobalFloat(TimeOfDayEffects._cloudsCaveMultiplyId, 1f);
			Shader.SetGlobalFloat(TimeOfDayEffects._brightnessId, 1f);
			Shader.SetGlobalColor(TimeOfDayEffects._fogNearColourId, Color.clear);
			Shader.SetGlobalColor(TimeOfDayEffects._fogFarColourId, Color.clear);
			Shader.SetGlobalColor(TimeOfDayEffects._fogHorizonColourId, Color.white);
			Shader.SetGlobalColor(TimeOfDayEffects._cloudsColourId, Color.white);
			Shader.SetGlobalFloat(TimeOfDayEffects._cloudsBrightnessId, 1f);
		}
	}

	// Token: 0x06000215 RID: 533 RVA: 0x00012482 File Offset: 0x00010682
	private void OnDisable()
	{
		if (this._mutableSkyboxMaterial != null)
		{
			RenderSettings.skybox = this.skyboxMaterial;
			Object.Destroy(this._mutableSkyboxMaterial);
		}
	}

	// Token: 0x06000216 RID: 534 RVA: 0x000124A8 File Offset: 0x000106A8
	private void SetImmediate()
	{
		if (!Game.loaded)
		{
			return;
		}
		WeatherType currentWeather = WeatherSystem.instance.currentWeather;
		this.cloudyStrength = 0f;
		if (currentWeather.HasFlag(WeatherType.VeryCloudy))
		{
			this.cloudyStrength = 1f;
		}
		else if (currentWeather.HasFlag(WeatherType.Cloudy))
		{
			this.cloudyStrength = 0.5f;
		}
		this.cloudyStrength = (float)(currentWeather.HasFlag(WeatherType.Cloudy) ? 1 : 0);
		this.rainStrength = (float)(currentWeather.HasFlag(WeatherType.Raining) ? 1 : 0);
		this.stormStrength = (float)(currentWeather.HasFlag(WeatherType.Storm) ? 1 : 0);
		this.snowStrength = (float)(currentWeather.HasFlag(WeatherType.Snow) ? 1 : 0);
		this.foggyStrength = (float)(currentWeather.HasFlag(WeatherType.Foggy) ? 1 : 0);
	}

	// Token: 0x06000217 RID: 535 RVA: 0x000125B0 File Offset: 0x000107B0
	private void LateUpdate()
	{
		if (!Game.loaded)
		{
			return;
		}
		if (TimeOfDayEffects.overrideParamsInEditor)
		{
			return;
		}
		float num = ((GameClock.instance == null) ? 0.5f : GameClock.instance.timeOfDayNorm);
		WeatherType currentWeather = WeatherSystem.instance.currentWeather;
		WeatherType futureWeather = WeatherSystem.instance.futureWeather;
		float z = GameCamera.instance.transform.position.z;
		WeatherType weatherType = currentWeather | futureWeather;
		float num2 = WeatherSystem.WeatherTypeToCloudiness(weatherType);
		this.cloudyStrength = Mathf.MoveTowards(this.cloudyStrength, num2, Time.deltaTime * 0.25f);
		this.rainStrength = Mathf.MoveTowards(this.rainStrength, (float)(weatherType.HasFlag(WeatherType.Raining) ? 1 : 0), Time.deltaTime * 0.25f);
		this.stormStrength = Mathf.MoveTowards(this.stormStrength, (float)(weatherType.HasFlag(WeatherType.Storm) ? 1 : 0), Time.deltaTime * 0.25f);
		this.snowStrength = Mathf.MoveTowards(this.snowStrength, (float)(weatherType.HasFlag(WeatherType.Snow) ? 1 : 0), Time.deltaTime * 0.25f);
		this.foggyStrength = Mathf.MoveTowards(this.foggyStrength, (float)(currentWeather.HasFlag(WeatherType.Foggy) ? 1 : 0), Time.deltaTime * 1f);
		this.currentThemeStop = this.timeOfDaySettings.clear.Evaluate(num);
		if (this.cloudyStrength > 0f)
		{
			this.currentThemeStop = ThemeStop.Lerp(this.currentThemeStop, this.timeOfDaySettings.cloudy.Evaluate(num), this.cloudyStrength);
		}
		if (this.rainStrength > 0f)
		{
			this.currentThemeStop = ThemeStop.Lerp(this.currentThemeStop, this.timeOfDaySettings.raining.Evaluate(num), this.rainStrength);
		}
		if (this.stormStrength > 0f)
		{
			this.currentThemeStop = ThemeStop.Lerp(this.currentThemeStop, this.timeOfDaySettings.storm.Evaluate(num), this.stormStrength);
		}
		if (this.snowStrength > 0f)
		{
			this.currentThemeStop = ThemeStop.Lerp(this.currentThemeStop, this.timeOfDaySettings.snow.Evaluate(num), this.snowStrength);
		}
		if (this.foggyStrength > 0f)
		{
			TimeOfDaySettings.FogStop fogStop = TimeOfDayEffects.EvaluateStops<TimeOfDaySettings.FogStop>(this.timeOfDaySettings.foggyModifierStops, (TimeOfDaySettings.FogStop stop) => stop.hour, new Func<TimeOfDaySettings.FogStop, TimeOfDaySettings.FogStop, float, TimeOfDaySettings.FogStop>(TimeOfDaySettings.FogStop.Lerp), num);
			float a = this.currentThemeStop.fogNearColour.a;
			float a2 = this.currentThemeStop.fogNearColour.a;
			this.currentThemeStop.fogNearColour = Color.Lerp(this.currentThemeStop.fogNearColour, fogStop.fogNearColour, 0.5f * this.foggyStrength);
			this.currentThemeStop.fogNearColour.a = Mathf.Lerp(a, fogStop.fogNearColour.a, this.foggyStrength);
			this.currentThemeStop.fogFarColour = Color.Lerp(this.currentThemeStop.fogFarColour, fogStop.fogFarColour, 0.5f * this.foggyStrength);
			this.currentThemeStop.fogFarColour.a = Mathf.Lerp(a2, fogStop.fogFarColour.a, this.foggyStrength);
			this.currentThemeStop.fogMainRamp = new Range(Mathf.Lerp(this.currentThemeStop.fogMainRamp.min, fogStop.fogMainRamp.min, this.foggyStrength), Mathf.Lerp(this.currentThemeStop.fogMainRamp.max, fogStop.fogMainRamp.max, this.foggyStrength));
		}
		if (TimeOfDayEffects.peakFogTransition > 0f)
		{
			float num3 = GameCamera.instance.cameraProperties.distance - 20f;
			if (num3 < 0f)
			{
				num3 = 0f;
			}
			this.currentThemeStop.fogMainRamp = this.currentThemeStop.fogMainRamp + TimeOfDayEffects.peakFogTransition * num3;
			this.currentThemeStop.fogNearColour.a = Mathf.Lerp(this.currentThemeStop.fogNearColour.a, 0.3f * this.currentThemeStop.fogNearColour.a, TimeOfDayEffects.peakFogTransition);
		}
		this.currentStormCloudMultiplyColour = TimeOfDayEffects.EvaluateStops<TimeOfDaySettings.StormCloudStop>(this.timeOfDaySettings.stormCloudMultiplyColouring, (TimeOfDaySettings.StormCloudStop stop) => stop.hour, new Func<TimeOfDaySettings.StormCloudStop, TimeOfDaySettings.StormCloudStop, float, TimeOfDaySettings.StormCloudStop>(TimeOfDaySettings.StormCloudStop.Lerp), num).colour;
		float num4 = ((CaveRegion.inCaveNorm > this._caveDarknessNorm) ? this.timeOfDaySettings.caveLightingRampUpDuration : this.timeOfDaySettings.caveLightingRampDownDuration);
		this._caveDarknessNorm = Mathf.MoveTowards(this._caveDarknessNorm, CaveRegion.inCaveNorm, Time.deltaTime / num4);
		if (this.shouldOverrideCaveDarkness)
		{
			this._caveDarknessNorm = this.caveDarknessOverride;
		}
		this.currentThemeStop.fogFarColour = Color.Lerp(this.currentThemeStop.fogFarColour, Color.black, this._caveDarknessNorm);
		float num5 = Mathf.InverseLerp(0f, this.timeOfDaySettings.caveLightingEndRampSplitPoint, this._caveDarknessNorm);
		if (num5 < 1f)
		{
			this.currentThemeStop.fogNearColour = Color.Lerp(this.currentThemeStop.fogNearColour, Color.black, num5);
		}
		else
		{
			float num6 = Mathf.InverseLerp(this.timeOfDaySettings.caveLightingEndRampSplitPoint, 1f, this._caveDarknessNorm);
			this.currentThemeStop.fogNearColour = Color.Lerp(Color.black, Color.black.WithAlpha(0f), num6);
		}
		float num7 = Runner.instance.transform.position.z - z;
		this.currentThemeStop.fogMainRamp = new Range(Mathf.Lerp(this.currentThemeStop.fogMainRamp.min, num7 + this.timeOfDaySettings.caveFogRamp.min, num5), Mathf.Lerp(this.currentThemeStop.fogMainRamp.max, num7 + this.timeOfDaySettings.caveFogRamp.max, num5));
		this.currentThemeStop.cloudsColour = Color.Lerp(this.currentThemeStop.cloudsColour, Color.black, this._caveDarknessNorm);
		this.currentThemeStop.cloudsBrightness = Mathf.Lerp(this.currentThemeStop.cloudsBrightness, 0f, this._caveDarknessNorm);
		this.currentThemeStop.sky = SkyColourStop.Lerp(this.currentThemeStop.sky, new SkyColourStop
		{
			top = Color.black,
			mid = Color.black,
			bottom = Color.black
		}, this._caveDarknessNorm);
		this.currentThemeStop.starsOpacity = Mathf.Lerp(this.currentThemeStop.starsOpacity, 0f, this._caveDarknessNorm);
		this.currentThemeStop.brightness = Mathf.Lerp(this.currentThemeStop.brightness, this.timeOfDaySettings.caveBrightness, this._caveDarknessNorm);
		this.currentThemeStop.multiplyColour = Color.Lerp(this.currentThemeStop.multiplyColour, Color.black, this._caveDarknessNorm);
		this.sun.color = (this.moon.color = Color.Lerp(Color.white, Color.black, this._caveDarknessNorm));
		if (Application.isPlaying)
		{
			Color color = Color.white.WithAlpha(this._caveDarknessNorm);
			foreach (CaveLighting caveLighting in MonoInstancer<CaveLighting>.all)
			{
				caveLighting.sprite.color = color;
			}
		}
		WeatherSystem.instance.cloudShadowScalarDueToCaveDarkness = 1f - this._caveDarknessNorm;
		float num8 = Mathf.InverseLerp(50f, 500f, Mathf.Abs(Runner.instance.transform.position.z - 4000f));
		if (num8 < 1f && this.currentThemeStop.brightness > 0.2f)
		{
			this.currentThemeStop.brightness = Mathf.Lerp(this.currentThemeStop.brightness, 0.2f, 1f - num8);
		}
		float num9 = Mathf.InverseLerp(0.16666667f, 0.25f, num);
		float num10 = Mathf.InverseLerp(0.75f, 0.8333333f, num);
		float num11 = Mathf.Min(num9, 1f - num10);
		if (MonoSingleton<BetweenLayersSeparator>.instance != null)
		{
			MonoSingleton<BetweenLayersSeparator>.instance.darknessFadeOutScalar = Mathf.Max(1f - num11, this._caveDarknessNorm);
		}
		this._lookFurtherCloudsTransition = Mathf.MoveTowards(this._lookFurtherCloudsTransition, (float)(Game.instance.lookingFurther ? 1 : 0), Time.deltaTime);
		float num12 = Mathf.Lerp(1f, this.lookFurtherCloudsAlpha, this._lookFurtherCloudsTransition);
		float num13 = Mathf.Lerp(1f, this.lookFurtherFogAlpha, this._lookFurtherCloudsTransition);
		this.currentThemeStop.fogNearColour.a = this.currentThemeStop.fogNearColour.a * num13;
		this.currentThemeStop.fogFarColour.a = this.currentThemeStop.fogFarColour.a * num13;
		Shader.SetGlobalColor(TimeOfDayEffects._multiplyColourId, this.currentThemeStop.multiplyColour);
		Shader.SetGlobalFloat(TimeOfDayEffects._cloudsCaveMultiplyId, 1f - this._caveDarknessNorm);
		Shader.SetGlobalFloat(TimeOfDayEffects._brightnessId, 1f + this.currentThemeStop.brightness);
		Shader.SetGlobalFloat(TimeOfDayEffects._cloudsBrightnessId, this.currentThemeStop.cloudsBrightness);
		Shader.SetGlobalColor(TimeOfDayEffects._cloudsColourId, this.currentThemeStop.cloudsColour.WithAlpha(MonoSingleton<PeakStateController>.instance.peakCloudsAlpha * num12));
		Shader.SetGlobalColor(TimeOfDayEffects._fogNearColourId, this.currentThemeStop.fogNearColour);
		Shader.SetGlobalColor(TimeOfDayEffects._fogFarColourId, this.currentThemeStop.fogFarColour);
		Shader.SetGlobalVector(TimeOfDayEffects._fogParamsId, new Vector4(this.currentThemeStop.fogMainRamp.min, this.currentThemeStop.fogMainRamp.max, 0f, 0f));
		Shader.SetGlobalTexture(TimeOfDayEffects._lightingRenderTextureId, this.lightingRenderTexture);
		Shader.SetGlobalTexture(TimeOfDayEffects._backLightRenderTextureId, this.backlightRenderTexture);
		Color color2 = this.timeOfDaySettings.horizonColour.Evaluate(num);
		Color color3 = this.timeOfDaySettings.horizonColourCloudy.Evaluate(num);
		float num14 = Mathf.Clamp01(this.cloudyStrength * 2f);
		Color color4 = Color.Lerp(color2, color3, num14);
		color4 = Color.Lerp(color4, Color.black, this._caveDarknessNorm);
		Shader.SetGlobalColor(TimeOfDayEffects._fogHorizonColourId, color4);
		if (this._mutableSkyboxMaterial != null)
		{
			this._mutableSkyboxMaterial.SetColor(TimeOfDayEffects._skyboxTopColorId, this.currentThemeStop.sky.top);
			this._mutableSkyboxMaterial.SetColor(TimeOfDayEffects._skyboxMidColorId, this.currentThemeStop.sky.mid);
			this._mutableSkyboxMaterial.SetColor(TimeOfDayEffects._skyboxBottomColorId, this.currentThemeStop.sky.bottom);
			this._mutableSkyboxMaterial.SetFloat(TimeOfDayEffects._skyboxScaleId, 0.7f * this.currentThemeStop.sky.scale);
			this._mutableSkyboxMaterial.SetFloat(TimeOfDayEffects._skyboxOffsetId, this.currentThemeStop.sky.offset);
			this._mutableSkyboxMaterial.SetFloat(TimeOfDayEffects._skyboxFogginessId, this.foggyStrength);
		}
		MonoSingleton<HorizonSoftener>.instance.SetColour(this.currentThemeStop.sky.bottom);
		this.stars.color = new Color(1f, 1f, 1f, this.currentThemeStop.starsOpacity);
		this.sun.backlightFlare.localScale = Vector3.one * this.currentThemeStop.sunBacklightSize;
		this.moon.backlightFlare.localScale = Vector3.one * this.currentThemeStop.moonBacklightSize;
		Color color5 = Color.white;
		color5 = Color.Lerp(color5, this.currentThemeStop.sky.bottom, this.timeOfDaySettings.cloudscapeMultiplySkyLowMix);
		color5 = this.timeOfDaySettings.cloudscapeMultiplyBrightened * color5;
		MonoSingleton<Cloudscape>.instance.multiplyColor = color5;
		Color color6 = this.currentThemeStop.multiplyColour;
		if (this.currentThemeStop.brightness < 0f)
		{
			color6 *= 1f + this.currentThemeStop.brightness;
		}
		foreach (LitParticles litParticles in MonoInstancer<LitParticles>.all)
		{
			float num15 = Mathf.Abs(litParticles.transform.position.z - z);
			float num16 = this.currentThemeStop.fogMainRamp.InverseLerp(num15);
			Color color7 = Color.Lerp(color6, this.currentThemeStop.fogNearColour.WithAlpha(1f), this.currentThemeStop.fogNearColour.a);
			Color color8 = Color.Lerp(color6, this.currentThemeStop.fogFarColour.WithAlpha(1f), this.currentThemeStop.fogFarColour.a);
			Color color9 = Color.Lerp(color7, color8, Mathf.Pow(num16, 0.5f));
			if (num15 < this.currentThemeStop.fogMainRamp.min)
			{
				float num17 = num15 / this.currentThemeStop.fogMainRamp.min;
				color9 = Color.Lerp(color6, color9, num17);
			}
			litParticles.lightingColor = color9;
		}
	}

	// Token: 0x06000218 RID: 536 RVA: 0x0001335C File Offset: 0x0001155C
	private static void AssertShaderIDsSet()
	{
		if (TimeOfDayEffects._generatedIds)
		{
			return;
		}
		TimeOfDayEffects._multiplyColourId = Shader.PropertyToID("_Global_MultiplyColor");
		TimeOfDayEffects._cloudsCaveMultiplyId = Shader.PropertyToID("_Global_CloudsCaveMultiply");
		TimeOfDayEffects._brightnessId = Shader.PropertyToID("_Global_Brightness");
		TimeOfDayEffects._cloudsBrightnessId = Shader.PropertyToID("_Global_CloudsBrightness");
		TimeOfDayEffects._cloudsColourId = Shader.PropertyToID("_Global_CloudsColor");
		TimeOfDayEffects._fogNearColourId = Shader.PropertyToID("_Global_FogNearColor");
		TimeOfDayEffects._fogFarColourId = Shader.PropertyToID("_Global_FogFarColor");
		TimeOfDayEffects._fogHorizonColourId = Shader.PropertyToID("_Global_HorizonFogColour");
		TimeOfDayEffects._fogParamsId = Shader.PropertyToID("_Global_FogParams");
		TimeOfDayEffects._lightingRenderTextureId = Shader.PropertyToID("_Global_LightingRenderTexture");
		TimeOfDayEffects._backLightRenderTextureId = Shader.PropertyToID("_Global_BackLightRenderTexture");
		TimeOfDayEffects._skyboxTopColorId = Shader.PropertyToID("_TopColor");
		TimeOfDayEffects._skyboxMidColorId = Shader.PropertyToID("_MidColor");
		TimeOfDayEffects._skyboxBottomColorId = Shader.PropertyToID("_BottomColor");
		TimeOfDayEffects._skyboxScaleId = Shader.PropertyToID("_Scale");
		TimeOfDayEffects._skyboxOffsetId = Shader.PropertyToID("_Offset");
		TimeOfDayEffects._skyboxFogginessId = Shader.PropertyToID("_Fogginess");
		TimeOfDayEffects._generatedIds = true;
	}

	// Token: 0x04000307 RID: 775
	public static bool overrideParamsInEditor;

	// Token: 0x04000308 RID: 776
	public static float peakFogTransition;

	// Token: 0x04000309 RID: 777
	public float lookFurtherCloudsAlpha = 0.5f;

	// Token: 0x0400030A RID: 778
	public float lookFurtherFogAlpha = 0.7f;

	// Token: 0x0400030B RID: 779
	public CelestialBody moon;

	// Token: 0x0400030C RID: 780
	public CelestialBody sun;

	// Token: 0x0400030D RID: 781
	public CelestialBody stars;

	// Token: 0x0400030E RID: 782
	public TimeOfDaySettings timeOfDaySettings;

	// Token: 0x0400030F RID: 783
	public RenderTexture lightingRenderTexture;

	// Token: 0x04000310 RID: 784
	public RenderTexture backlightRenderTexture;

	// Token: 0x04000311 RID: 785
	public Material skyboxMaterial;

	// Token: 0x04000312 RID: 786
	public ThemeStop currentThemeStop;

	// Token: 0x04000313 RID: 787
	public Color currentStormCloudMultiplyColour;

	// Token: 0x04000314 RID: 788
	public float cloudyStrength;

	// Token: 0x04000315 RID: 789
	public float rainStrength;

	// Token: 0x04000316 RID: 790
	public float stormStrength;

	// Token: 0x04000317 RID: 791
	public float snowStrength;

	// Token: 0x04000318 RID: 792
	public float foggyStrength;

	// Token: 0x04000319 RID: 793
	[Range(0f, 1f)]
	public float caveDarknessOverride = -1f;

	// Token: 0x0400031A RID: 794
	public bool shouldOverrideCaveDarkness;

	// Token: 0x0400031B RID: 795
	private Material _mutableSkyboxMaterial;

	// Token: 0x0400031C RID: 796
	private bool _wasDebugIsolateClear;

	// Token: 0x0400031D RID: 797
	private bool _wasDebugIsolateCloudy;

	// Token: 0x0400031E RID: 798
	private bool _wasDebugIsolateRaining;

	// Token: 0x0400031F RID: 799
	private bool _wasDebugIsolateStorm;

	// Token: 0x04000320 RID: 800
	private bool _wasDebugIsolateSnow;

	// Token: 0x04000321 RID: 801
	private float _caveDarknessNorm;

	// Token: 0x04000322 RID: 802
	private float _lookFurtherCloudsTransition;

	// Token: 0x04000323 RID: 803
	public static int _multiplyColourId;

	// Token: 0x04000324 RID: 804
	public static int _brightnessId;

	// Token: 0x04000325 RID: 805
	public static int _cloudsBrightnessId;

	// Token: 0x04000326 RID: 806
	public static int _cloudsColourId;

	// Token: 0x04000327 RID: 807
	public static int _cloudsCaveMultiplyId;

	// Token: 0x04000328 RID: 808
	private static int _skyboxTopColorId;

	// Token: 0x04000329 RID: 809
	private static int _skyboxMidColorId;

	// Token: 0x0400032A RID: 810
	private static int _skyboxBottomColorId;

	// Token: 0x0400032B RID: 811
	private static int _skyboxScaleId;

	// Token: 0x0400032C RID: 812
	private static int _skyboxOffsetId;

	// Token: 0x0400032D RID: 813
	private static int _skyboxFogginessId;

	// Token: 0x0400032E RID: 814
	public static int _fogNearColourId;

	// Token: 0x0400032F RID: 815
	public static int _fogFarColourId;

	// Token: 0x04000330 RID: 816
	public static int _fogHorizonColourId;

	// Token: 0x04000331 RID: 817
	public static int _fogParamsId;

	// Token: 0x04000332 RID: 818
	public static int _lightingRenderTextureId;

	// Token: 0x04000333 RID: 819
	public static int _backLightRenderTextureId;

	// Token: 0x04000334 RID: 820
	private static bool _generatedIds;
}
