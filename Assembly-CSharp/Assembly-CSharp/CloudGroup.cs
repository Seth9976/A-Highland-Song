using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000179 RID: 377
[ExecuteInEditMode]
public class CloudGroup : MonoInstancer<CloudGroup>
{
	// Token: 0x170002DD RID: 733
	// (get) Token: 0x06000C73 RID: 3187 RVA: 0x000633D1 File Offset: 0x000615D1
	// (set) Token: 0x06000C74 RID: 3188 RVA: 0x000633D9 File Offset: 0x000615D9
	public int levelIdx { get; protected set; }

	// Token: 0x170002DE RID: 734
	// (get) Token: 0x06000C75 RID: 3189 RVA: 0x000633E2 File Offset: 0x000615E2
	public int visibleCloudCount
	{
		get
		{
			return this._activeClouds.Count;
		}
	}

	// Token: 0x06000C76 RID: 3190 RVA: 0x000633F0 File Offset: 0x000615F0
	public static List<CloudGroup> FindWithInkName(string name)
	{
		CloudGroup._cloudGroupsByNameScratch.Clear();
		if (name == null || name.Length == 0)
		{
			return CloudGroup._cloudGroupsByNameScratch;
		}
		foreach (CloudGroup cloudGroup in MonoInstancer<CloudGroup>.all)
		{
			if (cloudGroup.inkName == name)
			{
				CloudGroup._cloudGroupsByNameScratch.Add(cloudGroup);
			}
		}
		return CloudGroup._cloudGroupsByNameScratch;
	}

	// Token: 0x06000C77 RID: 3191 RVA: 0x00063474 File Offset: 0x00061674
	public void BeginInkOverride(WeatherType overrideType, float duration = 0f)
	{
		if (duration == 0f)
		{
			duration = this.inkOverrideExpiryDays;
		}
		this.weatherInkOverride.weatherType = overrideType;
		this.weatherInkOverride.expiryDaysNorm = GameClock.instance.daysNorm + duration;
	}

	// Token: 0x06000C78 RID: 3192 RVA: 0x000634A9 File Offset: 0x000616A9
	public void ClearInkOverride()
	{
		this.weatherInkOverride = default(WeatherOverride);
	}

	// Token: 0x170002DF RID: 735
	// (get) Token: 0x06000C79 RID: 3193 RVA: 0x000634B7 File Offset: 0x000616B7
	public static CloudGroupSettings settings
	{
		get
		{
			if (CloudGroup._settings == null)
			{
				CloudGroup._settings = WeatherSystem.instance.settings.cloudGroupSettings;
			}
			return CloudGroup._settings;
		}
	}

	// Token: 0x06000C7A RID: 3194 RVA: 0x000634E0 File Offset: 0x000616E0
	protected override void OnEnable()
	{
		base.OnEnable();
		this.FindClouds();
		Level level = Level.GetForTransform(base.transform);
		if (level != null)
		{
			this.levelIdx = level.levelIdx;
			return;
		}
		if (base.GetComponent<BackgroundClouds>() == null)
		{
			Debug.LogError("CloudGroup seems to be neither part of a level nor the BackgroundClouds?", this);
			level = Level.GetForTransform(base.transform);
		}
		this.levelIdx = int.MaxValue;
	}

	// Token: 0x06000C7B RID: 3195 RVA: 0x00063545 File Offset: 0x00061745
	protected override void OnDisable()
	{
		base.OnDisable();
	}

	// Token: 0x06000C7C RID: 3196 RVA: 0x0006354D File Offset: 0x0006174D
	public void SetActiveWithFade(bool wantsActive)
	{
		if (!wantsActive && base.gameObject.activeSelf)
		{
			this._fadingOut = true;
			return;
		}
		if (wantsActive)
		{
			this._fadingOut = false;
			if (!base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x06000C7D RID: 3197 RVA: 0x0006358C File Offset: 0x0006178C
	public void FindClouds()
	{
		this._allClouds = base.GetComponentsInChildren<Cloud>();
		for (int i = 0; i < this._allClouds.Length; i++)
		{
			Cloud cloud = this._allClouds[i];
			cloud.basePosition = cloud.transform.localPosition;
			cloud.baseScale = cloud.transform.localScale;
		}
		this._activeClouds = new List<Cloud>(this._allClouds.Length);
		this._inactiveClouds = new List<Cloud>(this._allClouds.Length);
		this._inactiveClouds.AddRange(this._allClouds);
	}

	// Token: 0x06000C7E RID: 3198 RVA: 0x00063618 File Offset: 0x00061818
	public static void SetupAll(CloudGroupSettings settings)
	{
		foreach (CloudGroup cloudGroup in MonoInstancer<CloudGroup>.all)
		{
			cloudGroup._activeClouds.Clear();
			cloudGroup._inactiveClouds.Clear();
			cloudGroup._inactiveClouds.AddRange(cloudGroup._allClouds);
			foreach (Cloud cloud in cloudGroup._allClouds)
			{
				cloud.visible = false;
				cloud.tint.a = 0f;
				cloud.UpdateVisibility();
				cloudGroup._allHiddenDueToVisibility = true;
			}
		}
		CloudGroup.UpdateAll(0f, 0f, true, settings);
	}

	// Token: 0x06000C7F RID: 3199 RVA: 0x000636D8 File Offset: 0x000618D8
	public static void SetCloudinessStorminessFromWeather(WeatherType currentWeather, WeatherType futureWeather)
	{
		foreach (CloudGroup cloudGroup in MonoInstancer<CloudGroup>.all)
		{
			WeatherType weatherType = ((cloudGroup.levelIdx > Level.currentIndex) ? futureWeather : currentWeather);
			if (cloudGroup.weatherInkOverride.active)
			{
				weatherType = cloudGroup.weatherInkOverride.weatherType;
			}
			Cloudiness cloudiness;
			if ((weatherType & WeatherType.VeryCloudy) > WeatherType.Clear)
			{
				cloudiness = Cloudiness.VeryCloudy;
			}
			else if ((weatherType & WeatherType.Cloudy) > WeatherType.Clear || (weatherType & WeatherType.Storm) > WeatherType.Clear || (weatherType & WeatherType.Raining) > WeatherType.Clear)
			{
				cloudiness = Cloudiness.CloudyRainy;
			}
			else
			{
				cloudiness = Cloudiness.Clear;
			}
			cloudGroup.currentCloudiness = cloudiness;
			if ((weatherType & WeatherType.Storm) > WeatherType.Clear)
			{
				cloudGroup.targetStorminess = 1f;
			}
			else if ((weatherType & WeatherType.Raining) > WeatherType.Clear)
			{
				cloudGroup.targetStorminess = 0.6f;
			}
			else if ((weatherType & WeatherType.VeryCloudy) > WeatherType.Clear)
			{
				cloudGroup.targetStorminess = 0.4f;
			}
			else
			{
				cloudGroup.targetStorminess = 0f;
			}
		}
	}

	// Token: 0x06000C80 RID: 3200 RVA: 0x000637C8 File Offset: 0x000619C8
	public bool PointIsInCloud(Vector2 point, float cameraZ)
	{
		foreach (Cloud cloud in this._activeClouds)
		{
			if (cloud.color.a * cloud.fadeAlpha > 0.5f && cloud.meshRenderer.enabled)
			{
				if (!cloud.ContainsPoint(point))
				{
					return false;
				}
				float z = cloud.transform.position.z;
				Range range = new Range(z - 50f, z + 20f);
				if (!range.Contains(cameraZ))
				{
					return false;
				}
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000C81 RID: 3201 RVA: 0x00063890 File Offset: 0x00061A90
	public static void UpdateAll(float visualEffectsTimeStep, float windVelocity, bool initialSetup, CloudGroupSettings settings)
	{
		CloudGroup._cloudGroupCache.Clear();
		CloudGroup._cloudGroupCache.AddRange(MonoInstancer<CloudGroup>.all);
		foreach (CloudGroup cloudGroup in CloudGroup._cloudGroupCache)
		{
			cloudGroup.UpdateAndAnimate(visualEffectsTimeStep, windVelocity, initialSetup);
		}
		CloudGroup._cloudGroupCache.Clear();
	}

	// Token: 0x06000C82 RID: 3202 RVA: 0x00063908 File Offset: 0x00061B08
	public void UpdateAndAnimate(float visualEffectsTimeStep, float windVelocity, bool initialSetup)
	{
		float deltaTime = Time.deltaTime;
		float num = this.storminess;
		if (initialSetup)
		{
			this.storminess = this.targetStorminess;
		}
		else
		{
			this.storminess = Mathf.MoveTowards(this.storminess, this.targetStorminess, visualEffectsTimeStep);
		}
		if (Runner.instance.isMusicRunning && num == this.storminess)
		{
			return;
		}
		Color currentStormCloudMultiplyColour = GSR.timeOfDayEffects.currentStormCloudMultiplyColour;
		float fadeDurationNorm = CloudGroup.settings.fadeDurationNorm;
		float baseAnimSpeed = CloudGroup.settings.baseAnimSpeed;
		float num2 = 1f;
		if (this.visibility != CloudGroup.Visibility.All)
		{
			if (this.visibility == CloudGroup.Visibility.Near)
			{
				num2 = (float)((this.levelIdx == Level.currentIndex) ? 1 : 0);
			}
			else
			{
				num2 = (float)((GameCamera.instance.transform.position.z < base.transform.position.z - 200f) ? 1 : 0);
			}
			if (!Application.isPlaying && CloudGroup.hasEditorVisibilityOverride)
			{
				num2 = (float)(((CloudGroup.editorVisibilityOverrde & this.visibility) > (CloudGroup.Visibility)0) ? 1 : 0);
			}
		}
		if (this._fadingOut)
		{
			num2 = 0f;
		}
		if (initialSetup)
		{
			this.visibilityFade = num2;
		}
		else
		{
			this.visibilityFade = Mathf.MoveTowards(this.visibilityFade, num2, deltaTime);
		}
		bool allHiddenDueToVisibility = this._allHiddenDueToVisibility;
		if (this._allHiddenDueToVisibility)
		{
			if (this.visibilityFade == 0f)
			{
				if (this._fadingOut)
				{
					this._fadingOut = false;
					base.gameObject.SetActive(false);
				}
				return;
			}
			this._allHiddenDueToVisibility = false;
		}
		float num3 = 0f;
		float num4 = this.usageProportionClear;
		float num5 = this.usageProportionCloudy;
		float num6 = this.usageProportionVeryCloudy;
		if (DebugOptions.opts.reduceCloudUsageTest)
		{
			num4 *= 0.8f;
			num5 = Mathf.Lerp(num5, num4, 0.4f);
			num6 = Mathf.Lerp(num6, num4, 0.4f);
		}
		if (this.currentCloudiness == Cloudiness.Clear)
		{
			num3 = num4;
		}
		else if (this.currentCloudiness == Cloudiness.CloudyRainy)
		{
			num3 = num5;
		}
		else if (this.currentCloudiness == Cloudiness.VeryCloudy)
		{
			num3 = num6;
		}
		int num7 = Mathf.CeilToInt(num3 * (float)this._allClouds.Length);
		for (int i = this._activeClouds.Count - 1; i >= 0; i--)
		{
			Cloud cloud = this._activeClouds[i];
			float num8 = baseAnimSpeed * windVelocity * visualEffectsTimeStep * cloud.driftSpeed * cloud.speedVariationScalar;
			cloud.posNorm += num8;
			cloud.transform.localPosition = cloud.currentCycleOffset + Mathf.Lerp(-1f, 1f, cloud.posNorm) * cloud.cachedAnimationExtent + cloud.basePosition;
			float num9 = 1f;
			if (cloud.posNorm < fadeDurationNorm)
			{
				num9 = Mathf.InverseLerp(0f, fadeDurationNorm, cloud.posNorm);
			}
			if (cloud.posNorm > 1f - fadeDurationNorm)
			{
				num9 = Mathf.InverseLerp(1f, 1f - fadeDurationNorm, cloud.posNorm);
			}
			int num10 = (cloud.earlyFadeOut ? 0 : 1);
			if (initialSetup)
			{
				cloud.fadeAlpha = (float)num10;
			}
			else
			{
				cloud.fadeAlpha = Mathf.MoveTowards(cloud.fadeAlpha, (float)num10, deltaTime);
			}
			Color color = Color.Lerp(Color.white, currentStormCloudMultiplyColour, this.storminess);
			color.a = num9 * this.visibilityFade;
			cloud.tint = color;
			cloud.RefreshPropertyBlockColorOnly();
			cloud.UpdateVisibility();
			if (cloud.posNorm > 1f || cloud.posNorm < 0f || cloud.fadeAlpha == 0f)
			{
				this._inactiveClouds.Add(cloud);
				this._activeClouds.RemoveAt(i);
			}
		}
		int num11 = this._activeClouds.Count - num7;
		if (num11 > 0)
		{
			this._activeClouds.Sort((Cloud c1, Cloud c2) => c1.posNorm.CompareTo(c2.posNorm));
			for (int j = 0; j < num11; j++)
			{
				int num12 = this._activeClouds.Count - j - 1;
				Cloud cloud2 = this._activeClouds[num12];
				if (initialSetup)
				{
					cloud2.fadeAlpha = 0f;
					cloud2.UpdateVisibility();
					this._inactiveClouds.Add(cloud2);
					this._activeClouds.RemoveAt(num12);
				}
				else
				{
					cloud2.earlyFadeOut = true;
				}
			}
		}
		int num13 = num7 - this._activeClouds.Count;
		int num14 = num7 - this._prevTargetCloudVisibleCount - 1;
		for (int k = 0; k < num13; k++)
		{
			int num15 = this._inactiveClouds.RandomIndex<Cloud>();
			Cloud cloud3 = this._inactiveClouds[num15];
			int num16 = this._inactiveClouds.Count - 1;
			if (num15 < num16)
			{
				this._inactiveClouds[num15] = this._inactiveClouds[num16];
			}
			this._inactiveClouds.RemoveAt(num16);
			cloud3.visible = true;
			cloud3.earlyFadeOut = false;
			cloud3.tint.a = 0f;
			this._activeClouds.Add(cloud3);
			if (initialSetup || allHiddenDueToVisibility || k < num14)
			{
				cloud3.posNorm = Random.value;
				cloud3.fadeAlpha = 0f;
			}
			else
			{
				cloud3.posNorm = (float)((windVelocity > 0f) ? 0 : 1);
				cloud3.fadeAlpha = 1f;
			}
			cloud3.currentCycleOffset = (float)Random.Range(-1, 1) * cloud3.xVariation * cloud3.cachedAnimationExtent;
			cloud3.speedVariationScalar = Random.Range(0.9f, 1.1f);
			float value = Random.value;
			float num18;
			if (value < 0.5f)
			{
				float num17 = 1f - Mathf.InverseLerp(0f, 0.5f, value);
				num18 = Mathf.Lerp(1f, 1f / (1f + cloud3.scaleVariation), num17);
			}
			else
			{
				float num19 = Mathf.InverseLerp(0.5f, 1f, value);
				num18 = Mathf.Lerp(1f, 1f + cloud3.scaleVariation, num19);
			}
			cloud3.transform.localScale = Vector3.Scale(cloud3.baseScale, num18 * Vector3.one);
			cloud3.RefreshPropertyBlockColorOnly();
			cloud3.UpdateVisibility();
		}
		if (this._activeClouds.Count == 0 && num13 == 0 && this.visibilityFade == 0f)
		{
			this._allHiddenDueToVisibility = true;
		}
		this._prevTargetCloudVisibleCount = num7;
	}

	// Token: 0x06000C83 RID: 3203 RVA: 0x00063F78 File Offset: 0x00062178
	public void ResetToBasePosition()
	{
		for (int i = 0; i < this._allClouds.Length; i++)
		{
			Cloud cloud = this._allClouds[i];
			cloud.transform.localPosition = cloud.basePosition;
			cloud.transform.localScale = cloud.baseScale;
			cloud.tint = Color.white;
			cloud.fadeAlpha = 1f;
			cloud.RefreshPropertyBlockColorOnly();
		}
	}

	// Token: 0x04000EFC RID: 3836
	[EnumButtonGroup]
	public CloudGroup.Visibility visibility = CloudGroup.Visibility.Near;

	// Token: 0x04000EFD RID: 3837
	[Info("0 means current level only, 1 means visible on current and next level, 2 means current and next 2 levels. Otherwise clouds will be hidden.")]
	public int levelVisibilityRange = 2;

	// Token: 0x04000EFE RID: 3838
	[Range(0f, 1f)]
	public float usageProportionClear = 0.1f;

	// Token: 0x04000EFF RID: 3839
	[Range(0f, 1f)]
	public float usageProportionCloudy = 0.5f;

	// Token: 0x04000F00 RID: 3840
	[Range(0f, 1f)]
	public float usageProportionVeryCloudy = 1f;

	// Token: 0x04000F01 RID: 3841
	public static bool hasEditorVisibilityOverride = false;

	// Token: 0x04000F02 RID: 3842
	public static CloudGroup.Visibility editorVisibilityOverrde = CloudGroup.Visibility.All;

	// Token: 0x04000F03 RID: 3843
	[NonSerialized]
	public Cloudiness currentCloudiness;

	// Token: 0x04000F04 RID: 3844
	[Range(0f, 1f)]
	[NonSerialized]
	public float storminess;

	// Token: 0x04000F05 RID: 3845
	[Range(0f, 1f)]
	[NonSerialized]
	public float targetStorminess;

	// Token: 0x04000F06 RID: 3846
	[Range(0f, 1f)]
	[NonSerialized]
	public float visibilityFade = 1f;

	// Token: 0x04000F08 RID: 3848
	public string inkName;

	// Token: 0x04000F09 RID: 3849
	[HideInInspector]
	public float radius = 100f;

	// Token: 0x04000F0A RID: 3850
	public float inkOverrideExpiryDays = 0.25f;

	// Token: 0x04000F0B RID: 3851
	public WeatherOverride weatherInkOverride;

	// Token: 0x04000F0C RID: 3852
	private static List<CloudGroup> _cloudGroupsByNameScratch = new List<CloudGroup>(8);

	// Token: 0x04000F0D RID: 3853
	private static CloudGroupSettings _settings;

	// Token: 0x04000F0E RID: 3854
	public const int cloudGroupsLayer = 13;

	// Token: 0x04000F0F RID: 3855
	private static List<CloudGroup> _cloudGroupCache = new List<CloudGroup>(32);

	// Token: 0x04000F10 RID: 3856
	private Cloud[] _allClouds;

	// Token: 0x04000F11 RID: 3857
	private List<Cloud> _activeClouds;

	// Token: 0x04000F12 RID: 3858
	private List<Cloud> _inactiveClouds;

	// Token: 0x04000F13 RID: 3859
	private bool _allHiddenDueToVisibility;

	// Token: 0x04000F14 RID: 3860
	private int _prevTargetCloudVisibleCount;

	// Token: 0x04000F15 RID: 3861
	private bool _fadingOut;

	// Token: 0x020003A0 RID: 928
	[Flags]
	public enum Visibility
	{
		// Token: 0x04001977 RID: 6519
		Near = 1,
		// Token: 0x04001978 RID: 6520
		Far = 2,
		// Token: 0x04001979 RID: 6521
		All = 3
	}
}
