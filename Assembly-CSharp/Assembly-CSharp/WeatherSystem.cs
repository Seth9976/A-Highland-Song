using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000161 RID: 353
[ExecuteInEditMode]
public class WeatherSystem : MonoBehaviour
{
	// Token: 0x170002BF RID: 703
	// (get) Token: 0x06000BCB RID: 3019 RVA: 0x0005EBEF File Offset: 0x0005CDEF
	public static WeatherSystem instance
	{
		get
		{
			return GSR.WeatherSystem;
		}
	}

	// Token: 0x170002C0 RID: 704
	// (get) Token: 0x06000BCC RID: 3020 RVA: 0x0005EBF6 File Offset: 0x0005CDF6
	public bool isStorm
	{
		get
		{
			return this.currentWeather.HasFlag(WeatherType.Storm);
		}
	}

	// Token: 0x170002C1 RID: 705
	// (get) Token: 0x06000BCD RID: 3021 RVA: 0x0005EC0F File Offset: 0x0005CE0F
	public bool isRaining
	{
		get
		{
			return this.currentWeather.HasFlag(WeatherType.Raining);
		}
	}

	// Token: 0x170002C2 RID: 706
	// (get) Token: 0x06000BCE RID: 3022 RVA: 0x0005EC27 File Offset: 0x0005CE27
	public bool isSnowing
	{
		get
		{
			return this.currentWeather.HasFlag(WeatherType.Snow);
		}
	}

	// Token: 0x170002C3 RID: 707
	// (get) Token: 0x06000BCF RID: 3023 RVA: 0x0005EC40 File Offset: 0x0005CE40
	public bool isFoggy
	{
		get
		{
			return this.currentWeather.HasFlag(WeatherType.Foggy);
		}
	}

	// Token: 0x170002C4 RID: 708
	// (get) Token: 0x06000BD0 RID: 3024 RVA: 0x0005EC5C File Offset: 0x0005CE5C
	// (set) Token: 0x06000BD1 RID: 3025 RVA: 0x0005EC7D File Offset: 0x0005CE7D
	public static bool lightingSettingEnabled
	{
		get
		{
			if (!WeatherSystem._lightingSettingEnabledCached)
			{
				WeatherSystem._lightingSettingEnabled = PlayerPrefsX.GetInt(WeatherSystem.lightingSettingPrefName, 1) != 0;
			}
			return WeatherSystem._lightingSettingEnabled;
		}
		set
		{
			WeatherSystem._lightingSettingEnabled = value;
			PlayerPrefsX.SetInt(WeatherSystem.lightingSettingPrefName, value ? 1 : 0);
			WeatherSystem._lightingSettingEnabled = true;
		}
	}

	// Token: 0x170002C5 RID: 709
	// (get) Token: 0x06000BD2 RID: 3026 RVA: 0x0005EC9C File Offset: 0x0005CE9C
	public float windVelocity
	{
		get
		{
			return this.windSystem.windVelocity;
		}
	}

	// Token: 0x170002C6 RID: 710
	// (get) Token: 0x06000BD3 RID: 3027 RVA: 0x0005ECAC File Offset: 0x0005CEAC
	public float timeToWeatherChange
	{
		get
		{
			if (this._queuedWeather.Count == 0)
			{
				return float.PositiveInfinity;
			}
			return this._queuedWeather.Peek().daysNorm + this.settings.hoursBetweenLevels / 24f - GameClock.instance.daysNorm;
		}
	}

	// Token: 0x170002C7 RID: 711
	// (get) Token: 0x06000BD4 RID: 3028 RVA: 0x0005ECF9 File Offset: 0x0005CEF9
	public WeatherType nextWeather
	{
		get
		{
			if (this._queuedWeather.Count == 0)
			{
				return this.currentWeather;
			}
			return this._queuedWeather.Peek().weatherType;
		}
	}

	// Token: 0x170002C8 RID: 712
	// (get) Token: 0x06000BD5 RID: 3029 RVA: 0x0005ED1F File Offset: 0x0005CF1F
	public Queue<WeatherSystem.QueuedWeather> queuedWeather
	{
		get
		{
			return this._queuedWeather;
		}
	}

	// Token: 0x170002C9 RID: 713
	// (get) Token: 0x06000BD6 RID: 3030 RVA: 0x0005ED27 File Offset: 0x0005CF27
	private WindAffectedParticleSystem[] allWeatherParticles
	{
		get
		{
			if (this._allWeatherParticles == null)
			{
				this._allWeatherParticles = base.GetComponentsInChildren<WindAffectedParticleSystem>();
			}
			return this._allWeatherParticles;
		}
	}

	// Token: 0x06000BD7 RID: 3031 RVA: 0x0005ED44 File Offset: 0x0005CF44
	public void BeginGlobalInkOverride(WeatherType overrideType)
	{
		this.weatherInkOverride.weatherType = overrideType;
		this.currentWeather = overrideType;
		this.weatherInkOverride.expiryDaysNorm = GameClock.instance.daysNorm + this.settings.inkGlobalOverrideHours / 24f;
		foreach (CloudGroup cloudGroup in this._localCloudGroups)
		{
			cloudGroup.BeginInkOverride(overrideType, this.settings.inkGlobalOverrideHours / 24f);
		}
	}

	// Token: 0x06000BD8 RID: 3032 RVA: 0x0005EDE4 File Offset: 0x0005CFE4
	public void SetPhotoOverride(WeatherType overrideType)
	{
		this.weatherPhotoOverride.weatherType = overrideType;
		this.currentWeather = overrideType;
		this.weatherPhotoOverride.expiryDaysNorm = GameClock.instance.daysNorm + 1f;
	}

	// Token: 0x06000BD9 RID: 3033 RVA: 0x0005EE21 File Offset: 0x0005D021
	public void EndPhotoOverride(WeatherType originalWeather)
	{
		this.weatherPhotoOverride = default(WeatherOverride);
		this.currentWeather = originalWeather;
	}

	// Token: 0x06000BDA RID: 3034 RVA: 0x0005EE38 File Offset: 0x0005D038
	public void BeginLocalInkOverride(string cloudGroupsName, WeatherType weatherType)
	{
		foreach (CloudGroup cloudGroup in CloudGroup.FindWithInkName(cloudGroupsName))
		{
			cloudGroup.BeginInkOverride(weatherType, 0f);
		}
	}

	// Token: 0x06000BDB RID: 3035 RVA: 0x0005EE90 File Offset: 0x0005D090
	private void OnEnable()
	{
		GameClock.onVisualEffectTimeDidPass = (Action<float>)Delegate.Combine(GameClock.onVisualEffectTimeDidPass, new Action<float>(this.OnGameClockVisualEffectsTimeDidPass));
		if (this.weatherPattern == null)
		{
			this.weatherPattern = this.settings.gameStartWeatherPattern;
		}
		if (this.windSystem != null)
		{
			this.windSystem.ApplyShaderParams();
		}
	}

	// Token: 0x06000BDC RID: 3036 RVA: 0x0005EEEF File Offset: 0x0005D0EF
	private void OnDisable()
	{
		GameClock.onVisualEffectTimeDidPass = (Action<float>)Delegate.Remove(GameClock.onVisualEffectTimeDidPass, new Action<float>(this.OnGameClockVisualEffectsTimeDidPass));
	}

	// Token: 0x06000BDD RID: 3037 RVA: 0x0005EF11 File Offset: 0x0005D111
	private void Start()
	{
		if (Application.isPlaying)
		{
			this.ManualUpdate(true);
			return;
		}
		this.ApplyShaderParams();
	}

	// Token: 0x06000BDE RID: 3038 RVA: 0x0005EF28 File Offset: 0x0005D128
	public void Clear()
	{
		this.currentWeather = (this.futureWeather = WeatherType.Clear);
		this.weatherPattern = this.settings.gameStartWeatherPattern;
		this.weatherInkOverride = default(WeatherOverride);
		this.weatherPhotoOverride = default(WeatherOverride);
		foreach (CloudGroup cloudGroup in MonoInstancer<CloudGroup>.all)
		{
			cloudGroup.ClearInkOverride();
		}
		this.RefreshWeatherPatternState();
		this.UpdateWeather(true);
	}

	// Token: 0x06000BDF RID: 3039 RVA: 0x0005EFC0 File Offset: 0x0005D1C0
	public void OnCompleteLoad()
	{
		this.UpdateWeather(true);
		CloudGroup.SetCloudinessStorminessFromWeather(this.currentWeather, this.futureWeather);
		CloudGroup.SetupAll(this.settings.cloudGroupSettings);
		MonoSingleton<Cloudscape>.instance.Refresh(0f, true);
	}

	// Token: 0x06000BE0 RID: 3040 RVA: 0x0005EFFA File Offset: 0x0005D1FA
	public void SetWeatherPatternFromInk(string patternName)
	{
		this.weatherPattern = this.WeatherPatternWithName(patternName);
		this.RefreshWeatherPatternState();
	}

	// Token: 0x06000BE1 RID: 3041 RVA: 0x0005F00F File Offset: 0x0005D20F
	public void RandomiseWeatherPattern()
	{
		this.weatherPattern = this.settings.weatherPatterns.Random<WeatherPattern>();
		this.RefreshWeatherPatternState();
	}

	// Token: 0x06000BE2 RID: 3042 RVA: 0x0005F02D File Offset: 0x0005D22D
	[ContextMenu("Reset weather pattern state")]
	public void RefreshWeatherPatternState()
	{
		this.weatherPatternState.Reset();
		this.ManualUpdate(true);
	}

	// Token: 0x06000BE3 RID: 3043 RVA: 0x0005F041 File Offset: 0x0005D241
	private void Update()
	{
		this.ManualUpdate(false);
	}

	// Token: 0x06000BE4 RID: 3044 RVA: 0x0005F04C File Offset: 0x0005D24C
	private void ManualUpdate(bool isFirstFrame)
	{
		if (Application.isPlaying)
		{
			if (!Game.loaded)
			{
				return;
			}
			this.UpdateWeather(isFirstFrame);
			if (isFirstFrame)
			{
				WindAffectedParticleSystem[] allWeatherParticles = this.allWeatherParticles;
				for (int i = 0; i < allWeatherParticles.Length; i++)
				{
					allWeatherParticles[i].UpdateEmissionRate(true);
				}
			}
			float num = 0.033333335f * this._currentVisualEffectsSpeed;
			if (isFirstFrame)
			{
				num = 0f;
			}
			this.windSystem.ManualUpdate(num);
			if (Time.unscaledTime > this.nextWeatherEffectsUpdateTime)
			{
				this.UpdateLocalWeatherEffects();
				this.nextWeatherEffectsUpdateTime = Time.unscaledTime + 0.113f;
			}
			float currentZ = Level.currentZ;
			float num2 = Mathf.Lerp(this._cloudShadowZRange.min, currentZ, TimeX.Lerping(0.1f));
			if (isFirstFrame)
			{
				num2 = currentZ;
			}
			this._cloudShadowZRange = new Range(num2, num2 + 300f);
			float num3 = Mathf.Lerp(this.settings.cloudShadowSettings.minimumCloudShadow, 1f, WeatherSystem.WeatherTypeToCloudiness(this.currentWeather));
			num3 *= this.cloudShadowScalarDueToCaveDarkness;
			this._cloudinessLevelForShadows = Mathf.MoveTowards(this._cloudinessLevelForShadows, num3, Time.deltaTime);
			this.ApplyShaderParams();
		}
	}

	// Token: 0x06000BE5 RID: 3045 RVA: 0x0005F168 File Offset: 0x0005D368
	private void UpdateWeather(bool initialSetup)
	{
		float daysNorm = GameClock.instance.daysNorm;
		if (initialSetup)
		{
			this._queuedWeather.Clear();
			for (int i = 10; i >= 1; i--)
			{
				float num = (float)i * 0.025f;
				this.RefreshWeatherStateForTime(daysNorm - num);
			}
		}
		this._isInsideCloud = false;
		this._localCloudGroups.Clear();
		Vector2 position = Runner.instance.position;
		int currentIndex = Level.currentIndex;
		foreach (CloudGroup cloudGroup in MonoInstancer<CloudGroup>.all)
		{
			if (cloudGroup.levelIdx == currentIndex && Vector2.Distance(cloudGroup.transform.position, position) < cloudGroup.radius)
			{
				this._localCloudGroups.Add(cloudGroup);
				if (cloudGroup.currentCloudiness == Cloudiness.VeryCloudy && cloudGroup.PointIsInCloud(position, GameCamera.instance.transform.position.z))
				{
					this._isInsideCloud = true;
				}
			}
		}
		this.RefreshWeatherStateForTime(daysNorm);
	}

	// Token: 0x06000BE6 RID: 3046 RVA: 0x0005F280 File Offset: 0x0005D480
	private void RefreshWeatherStateForTime(float daysNorm)
	{
		this.weatherPatternState.UpdatePeriods(daysNorm, this.weatherPattern);
		WeatherType weatherType = this.weatherPatternState.WeatherTypeAt(daysNorm);
		if (this.settings.useSimpleWeather)
		{
			this.currentWeather = weatherType;
		}
		else
		{
			if (this.futureWeather != weatherType)
			{
				this.futureWeather = weatherType;
				this._queuedWeather.Enqueue(new WeatherSystem.QueuedWeather
				{
					daysNorm = daysNorm,
					weatherType = this.futureWeather
				});
			}
			if (this._queuedWeather.Count > 0 && this.timeToWeatherChange <= 0f)
			{
				this.currentWeather = this.nextWeather;
				this._queuedWeather.Dequeue();
			}
		}
		if (!this._isInsideCloud)
		{
			this.currentWeather &= ~WeatherType.Foggy;
		}
		if (this.weatherInkOverride.active)
		{
			this.currentWeather = this.weatherInkOverride.weatherType;
		}
		foreach (CloudGroup cloudGroup in this._localCloudGroups)
		{
			if (cloudGroup.weatherInkOverride.active)
			{
				this.currentWeather = cloudGroup.weatherInkOverride.weatherType;
			}
		}
		WeatherModifierZone currentWeatherModifierZone = Runner.instance.health.currentWeatherModifierZone;
		if (currentWeatherModifierZone != null && currentWeatherModifierZone.affectsActualWeather && currentWeatherModifierZone.protectionFromWeather == ShelterProtectionStrength.Full)
		{
			this.currentWeather = WeatherType.Clear;
		}
		bool isInsideCloud = this._isInsideCloud;
		if (WeatherSystem.instance.windSystem.windyInkFlagSet)
		{
			this.currentWeather |= WeatherType.Windy;
		}
		else
		{
			this.currentWeather &= ~WeatherType.Windy;
		}
		if (this.settings.useSimpleWeather)
		{
			this.futureWeather = this.currentWeather;
		}
		if (this.weatherPhotoOverride.active)
		{
			this.currentWeather = this.weatherPhotoOverride.weatherType;
		}
	}

	// Token: 0x06000BE7 RID: 3047 RVA: 0x0005F468 File Offset: 0x0005D668
	private void UpdateLocalWeatherEffects()
	{
		if (this.currentWeather.HasFlag(WeatherType.Storm) && !CaveRegion.inCave)
		{
			this.rainAudio.targetStrength = 1f;
		}
		else if (this.currentWeather.HasFlag(WeatherType.Raining) && !CaveRegion.inCave)
		{
			this.rainAudio.targetStrength = 0.5f;
		}
		else
		{
			this.rainAudio.targetStrength = 0f;
		}
		if (this.currentWeather.HasFlag(WeatherType.Storm) && !CaveRegion.inCave && WeatherSystem.lightingSettingEnabled)
		{
			this.lightningEffect.targetStrength = 1f;
			return;
		}
		this.lightningEffect.targetStrength = 0f;
	}

	// Token: 0x06000BE8 RID: 3048 RVA: 0x0005F530 File Offset: 0x0005D730
	private void ApplyShaderParams()
	{
		Shader.SetGlobalTexture("_Global_CloudShadowTexture", this.settings.cloudShadowSettings.cloudShadowTexture);
		Shader.SetGlobalFloat("_Global_Rain", this.rainAudio.strength);
		Shader.SetGlobalVector("_Global_LevelsRange", new Vector2(this._cloudShadowZRange.min, this._cloudShadowZRange.max));
		Shader.SetGlobalFloat("_Global_CloudinessLevelForShadows", this._cloudinessLevelForShadows);
	}

	// Token: 0x06000BE9 RID: 3049 RVA: 0x0005F5A6 File Offset: 0x0005D7A6
	public static float WeatherTypeToCloudiness(WeatherType weatherType)
	{
		if ((weatherType & WeatherType.VeryCloudy) > WeatherType.Clear)
		{
			return 1f;
		}
		if ((weatherType & WeatherType.Cloudy) > WeatherType.Clear || (weatherType & WeatherType.Storm) > WeatherType.Clear)
		{
			return 0.5f;
		}
		if ((weatherType & WeatherType.Raining) > WeatherType.Clear)
		{
			return 0.5f;
		}
		if ((weatherType & WeatherType.Snow) > WeatherType.Clear)
		{
			return 0.5f;
		}
		return 0f;
	}

	// Token: 0x06000BEA RID: 3050 RVA: 0x0005F5E8 File Offset: 0x0005D7E8
	private WeatherPattern WeatherPatternWithName(string patternName)
	{
		foreach (WeatherPattern weatherPattern in this.settings.weatherPatterns)
		{
			if (weatherPattern.name == patternName)
			{
				return weatherPattern;
			}
		}
		return null;
	}

	// Token: 0x06000BEB RID: 3051 RVA: 0x0005F624 File Offset: 0x0005D824
	public void LoadSaveState(SaveState.WeatherSystemSaveState saveState)
	{
		this.weatherPatternState = saveState.weatherPatternState;
		this._queuedWeather.Clear();
		foreach (WeatherSystem.QueuedWeather queuedWeather in saveState.queuedWeather)
		{
			this._queuedWeather.Enqueue(queuedWeather);
		}
		CloudGroup.SetCloudinessStorminessFromWeather(this.currentWeather, this.futureWeather);
		this.weatherPattern = this.WeatherPatternWithName(saveState.weatherPatternName);
		if (this.weatherPattern == null)
		{
			Debug.Log("Failed to find named weather pattern from save state: '" + saveState.weatherPatternName + "'. Will randomise based on known weather patterns.");
			this.RandomiseWeatherPattern();
		}
		this.weatherInkOverride = saveState.weatherInkOverride;
		foreach (CloudGroup cloudGroup in MonoInstancer<CloudGroup>.all)
		{
			cloudGroup.weatherInkOverride = default(WeatherOverride);
		}
		foreach (SaveState.CloudGroupSaveState cloudGroupSaveState in saveState.cloudGroupStates)
		{
			List<CloudGroup> list = CloudGroup.FindWithInkName(cloudGroupSaveState.inkName);
			if (list == null)
			{
				Debug.LogError("CloudGroups with ink name '" + cloudGroupSaveState.inkName + "' could not be found when loading a save. Has it changed name or is the scene not currently loaded?");
			}
			else
			{
				foreach (CloudGroup cloudGroup2 in list)
				{
					cloudGroup2.weatherInkOverride = cloudGroupSaveState.weatherOverride;
					cloudGroup2.storminess = cloudGroupSaveState.storminess;
				}
			}
		}
		this.ManualUpdate(true);
	}

	// Token: 0x06000BEC RID: 3052 RVA: 0x0005F7F8 File Offset: 0x0005D9F8
	public void UpdateSaveState(SaveState.WeatherSystemSaveState saveState)
	{
		saveState.weatherPatternState = this.weatherPatternState;
		saveState.queuedWeather.Clear();
		foreach (WeatherSystem.QueuedWeather queuedWeather in this._queuedWeather)
		{
			saveState.queuedWeather.Add(queuedWeather);
		}
		saveState.weatherPatternName = this.weatherPattern.name;
		saveState.weatherInkOverride = this.weatherInkOverride;
		saveState.cloudGroupStates.Clear();
		using (List<CloudGroup>.Enumerator enumerator2 = MonoInstancer<CloudGroup>.all.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				CloudGroup cloudGroup = enumerator2.Current;
				if (cloudGroup.weatherInkOverride.active && !saveState.cloudGroupStates.Exists((SaveState.CloudGroupSaveState cloudState) => cloudState.inkName == cloudGroup.inkName))
				{
					saveState.cloudGroupStates.Add(new SaveState.CloudGroupSaveState
					{
						inkName = cloudGroup.inkName,
						weatherOverride = cloudGroup.weatherInkOverride,
						storminess = cloudGroup.storminess
					});
				}
			}
		}
	}

	// Token: 0x06000BED RID: 3053 RVA: 0x0005F950 File Offset: 0x0005DB50
	public static bool IsWeatherValid(WeatherType currentWeather, WeatherType testWeather)
	{
		int num = (int)(~(int)testWeather);
		return (currentWeather & (WeatherType)num) == WeatherType.Clear;
	}

	// Token: 0x06000BEE RID: 3054 RVA: 0x0005F966 File Offset: 0x0005DB66
	public static bool AnyWeatherMatches(WeatherType currentWeather, WeatherType testWeather)
	{
		return (currentWeather & testWeather) > WeatherType.Clear;
	}

	// Token: 0x06000BEF RID: 3055 RVA: 0x0005F970 File Offset: 0x0005DB70
	private void OnGameClockVisualEffectsTimeDidPass(float visualEffectTimePassed)
	{
		this._currentVisualEffectsSpeed = visualEffectTimePassed;
		CloudGroup.SetCloudinessStorminessFromWeather(this.currentWeather, this.futureWeather);
		CloudGroup.UpdateAll(0.016666668f * visualEffectTimePassed, this.windVelocity, false, this.settings.cloudGroupSettings);
		MonoSingleton<Cloudscape>.instance.Refresh(visualEffectTimePassed, false);
	}

	// Token: 0x04000E05 RID: 3589
	public static Action<WeatherType, WeatherType> onWeatherDidChange;

	// Token: 0x04000E06 RID: 3590
	public static string lightingSettingPrefName = "vibrationEnabled";

	// Token: 0x04000E07 RID: 3591
	private static bool _lightingSettingEnabled = true;

	// Token: 0x04000E08 RID: 3592
	private static bool _lightingSettingEnabledCached;

	// Token: 0x04000E09 RID: 3593
	[NonSerialized]
	public WeatherType currentWeather;

	// Token: 0x04000E0A RID: 3594
	[NonSerialized]
	public WeatherType futureWeather;

	// Token: 0x04000E0B RID: 3595
	public WeatherPattern weatherPattern;

	// Token: 0x04000E0C RID: 3596
	public WeatherSystemSettings settings;

	// Token: 0x04000E0D RID: 3597
	public WindSystem windSystem;

	// Token: 0x04000E0E RID: 3598
	private WindAffectedParticleSystem[] _allWeatherParticles;

	// Token: 0x04000E0F RID: 3599
	public RainAudio rainAudio;

	// Token: 0x04000E10 RID: 3600
	public LightningEffectsController lightningEffect;

	// Token: 0x04000E11 RID: 3601
	public float cloudShadowScalarDueToCaveDarkness = 1f;

	// Token: 0x04000E12 RID: 3602
	private float nextWeatherEffectsUpdateTime;

	// Token: 0x04000E13 RID: 3603
	public WeatherOverride weatherInkOverride;

	// Token: 0x04000E14 RID: 3604
	public WeatherOverride weatherPhotoOverride;

	// Token: 0x04000E15 RID: 3605
	private Queue<WeatherSystem.QueuedWeather> _queuedWeather = new Queue<WeatherSystem.QueuedWeather>();

	// Token: 0x04000E16 RID: 3606
	[NonSerialized]
	public WeatherPattern.State weatherPatternState = new WeatherPattern.State();

	// Token: 0x04000E17 RID: 3607
	private List<CloudGroup> _localCloudGroups = new List<CloudGroup>();

	// Token: 0x04000E18 RID: 3608
	private bool _isInsideCloud;

	// Token: 0x04000E19 RID: 3609
	private float _cloudinessLevelForShadows = 0.5f;

	// Token: 0x04000E1A RID: 3610
	private Range _cloudShadowZRange = new Range(0f, 300f);

	// Token: 0x04000E1B RID: 3611
	private float _currentVisualEffectsSpeed = 1f;

	// Token: 0x02000392 RID: 914
	[Serializable]
	public struct QueuedWeather
	{
		// Token: 0x04001950 RID: 6480
		public float daysNorm;

		// Token: 0x04001951 RID: 6481
		public WeatherType weatherType;
	}
}
