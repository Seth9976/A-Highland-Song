using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000112 RID: 274
public class HealthUI : MonoBehaviour
{
	// Token: 0x1700024D RID: 589
	// (get) Token: 0x06000927 RID: 2343 RVA: 0x0004CE0D File Offset: 0x0004B00D
	public SLayout layout
	{
		get
		{
			if (this._layout == null)
			{
				this._layout = base.GetComponent<SLayout>();
			}
			return this._layout;
		}
	}

	// Token: 0x1700024E RID: 590
	// (get) Token: 0x06000928 RID: 2344 RVA: 0x0004CE30 File Offset: 0x0004B030
	public Vector2 topMaxHealthPosInCanvas
	{
		get
		{
			Vector2 size = this._maxHealthBar.size;
			return this._maxHealthBar.ConvertPositionToTarget(new Vector2(0.5f * size.x, size.y), null);
		}
	}

	// Token: 0x1700024F RID: 591
	// (get) Token: 0x06000929 RID: 2345 RVA: 0x0004CE6C File Offset: 0x0004B06C
	public Vector2 topHealthPosInCanvas
	{
		get
		{
			Vector2 size = this._healthBar.size;
			return this._healthBar.ConvertPositionToTarget(new Vector2(0.5f * size.x, size.y), null);
		}
	}

	// Token: 0x17000250 RID: 592
	// (get) Token: 0x0600092A RID: 2346 RVA: 0x0004CEA8 File Offset: 0x0004B0A8
	public Vector2 activeWeatherIconInCanvas
	{
		get
		{
			SLayout slayout = this._weatherIcon ?? this._windIcon;
			if (slayout == null)
			{
				return this.topMaxHealthPosInCanvas;
			}
			Vector2 size = slayout.size;
			return slayout.ConvertPositionToTarget(new Vector2(size.x, 0.5f * size.y), null);
		}
	}

	// Token: 0x0600092B RID: 2347 RVA: 0x0004CEFC File Offset: 0x0004B0FC
	private void Start()
	{
		Runner.onBecameActive = (Action<bool>)Delegate.Combine(Runner.onBecameActive, new Action<bool>(this.OnRunnerBecameActive));
		Health.onHealthChanged = (Action<Health, float>)Delegate.Combine(Health.onHealthChanged, new Action<Health, float>(this.OnChangeHealth));
		Health.onHealthWillDoMajorChange = (Action)Delegate.Combine(Health.onHealthWillDoMajorChange, new Action(this.OnHealthWillDoMajorChange));
		this._healthLossBar.alpha = 0f;
		if (Runner.instance != null)
		{
			this.RefreshImmediate(Runner.instance.health, true);
		}
	}

	// Token: 0x0600092C RID: 2348 RVA: 0x0004CF98 File Offset: 0x0004B198
	private void OnDestroy()
	{
		Runner.onBecameActive = (Action<bool>)Delegate.Remove(Runner.onBecameActive, new Action<bool>(this.OnRunnerBecameActive));
		Health.onHealthChanged = (Action<Health, float>)Delegate.Remove(Health.onHealthChanged, new Action<Health, float>(this.OnChangeHealth));
		Health.onHealthWillDoMajorChange = (Action)Delegate.Remove(Health.onHealthWillDoMajorChange, new Action(this.OnHealthWillDoMajorChange));
	}

	// Token: 0x0600092D RID: 2349 RVA: 0x0004D008 File Offset: 0x0004B208
	private void OnChangeHealth(Health health, float change)
	{
		float num = this.HealthToHeight(health.currentHealth - change);
		float num2 = this.HealthToHeight(health.currentHealth);
		if (num - num2 > 5f)
		{
			this._healthLossInitialDelayRemaining = 2f;
			if (this._healthLossCurrentFullBarHeight < 0f)
			{
				this._healthLossCurrentFullBarHeight = this._healthBar.height;
			}
		}
	}

	// Token: 0x0600092E RID: 2350 RVA: 0x0004D062 File Offset: 0x0004B262
	private void OnHealthWillDoMajorChange()
	{
		this._majorHealthChangeAnimStartTime = Time.time;
		this._maxHealthHeightBeforeMajorChange = this._maxHealthBar.height;
		this._healthHeightBeforeMajorChange = this._healthBar.height;
	}

	// Token: 0x0600092F RID: 2351 RVA: 0x0004D091 File Offset: 0x0004B291
	private void RefreshImmediate(Health health, bool immediate)
	{
		if (immediate)
		{
			this._lastIconUpdateTime = -1f;
		}
	}

	// Token: 0x06000930 RID: 2352 RVA: 0x0004D0A4 File Offset: 0x0004B2A4
	private void Update()
	{
		if (!Game.loaded)
		{
			return;
		}
		bool flag = Game.instance.inActiveGameplay && !Game.gameplayPaused && !MonoSingleton<BlackBars>.instance.visible && !GameCamera.instance.playerZoomState.zooming && !Runner.instance.inFinalJump;
		bool flag2 = (Runner.instance.health.invincibleReason & Health.InvincibleReason.InkScene) > Health.InvincibleReason.None;
		bool flag3 = (flag || MonoSingleton<RestStateController>.instance.active) && !flag2 && !MonoSingleton<JournalController>.instance.visible && !MonoSingleton<TitleScreen>.instance.visible && !PhotoMode.visible;
		int targetAlpha = (flag3 ? 1 : 0);
		if ((float)targetAlpha != this.layout.targetGroupAlpha)
		{
			this.layout.Animate(0.4f, delegate
			{
				this.layout.groupAlpha = (float)targetAlpha;
			});
		}
		this.RefreshHealthLayout();
	}

	// Token: 0x06000931 RID: 2353 RVA: 0x0004D194 File Offset: 0x0004B394
	private void RefreshHealthLayout()
	{
		Health health = Runner.instance.health;
		Color weatherFreezeIconColour = this._settings.weatherFreezeIconColour;
		if (health.healthAtRiskForUI > 0f)
		{
			float num = 1f + this._settings.flashAmplitude * Mathf.Sin(this._settings.flashSpeed * Time.time);
			weatherFreezeIconColour = new Color(Mathf.Clamp01(num * weatherFreezeIconColour.r), Mathf.Clamp01(num * weatherFreezeIconColour.g), Mathf.Clamp01(num * weatherFreezeIconColour.b), weatherFreezeIconColour.a);
		}
		this.RefreshHealthBarLayout(weatherFreezeIconColour);
		WeatherHealthEffect weatherHealthEffect = health.GetWeatherHealthEffects(false);
		WeatherHealthEffect weatherHealthEffect2 = health.GetWeatherHealthEffects(true);
		weatherHealthEffect &= ~(WeatherHealthEffect.Chilly | WeatherHealthEffect.Cold | WeatherHealthEffect.Night);
		weatherHealthEffect2 &= ~(WeatherHealthEffect.Chilly | WeatherHealthEffect.Cold | WeatherHealthEffect.Night);
		ValueTuple<WeatherHealthEffect, bool> valueTuple = HealthUI.<RefreshHealthLayout>g__CheckActiveEffects|15_0(weatherHealthEffect, weatherHealthEffect2, WeatherHealthEffect.TemperatureMask);
		WeatherHealthEffect item = valueTuple.Item1;
		bool item2 = valueTuple.Item2;
		ValueTuple<WeatherHealthEffect, bool> valueTuple2 = HealthUI.<RefreshHealthLayout>g__CheckActiveEffects|15_0(weatherHealthEffect, weatherHealthEffect2, WeatherHealthEffect.WeatherMask);
		WeatherHealthEffect item3 = valueTuple2.Item1;
		bool item4 = valueTuple2.Item2;
		ValueTuple<WeatherHealthEffect, bool> valueTuple3 = HealthUI.<RefreshHealthLayout>g__CheckActiveEffects|15_0(weatherHealthEffect, weatherHealthEffect2, WeatherHealthEffect.WindMask);
		WeatherHealthEffect item5 = valueTuple3.Item1;
		bool item6 = valueTuple3.Item2;
		WeatherHealthEffect weatherHealthEffect3 = weatherHealthEffect & WeatherHealthEffect.Night;
		if (this._visibleEffectIcons != weatherHealthEffect && Time.time > this._lastIconUpdateTime + 1f)
		{
			this.RefreshIcon(ref this._temperatureIcon, ref this._temperatureIconEffect, item);
			this.RefreshIcon(ref this._weatherIcon, ref this._weatherIconEffect, item3);
			this.RefreshIcon(ref this._windIcon, ref this._windIconEffect, item5);
			this.RefreshIcon(ref this._nightIcon, ref this._nightIconEffect, weatherHealthEffect3);
			this._lastIconUpdateTime = Time.time;
			this._visibleEffectIcons = weatherHealthEffect;
		}
		float num2 = this._absoluteMaxHealthBar.topY + 10f;
		if (this._temperatureIcon != null)
		{
			this._temperatureIcon.y = num2;
			this._temperatureIcon.color = (item2 ? this._settings.protectedWeatherIconColor : weatherFreezeIconColour);
			num2 += this._temperatureIcon.height + 5f;
		}
		if (this._weatherIcon != null)
		{
			this._weatherIcon.y = num2;
			this._weatherIcon.color = (item4 ? this._settings.protectedWeatherIconColor : weatherFreezeIconColour);
			num2 += this._weatherIcon.height + 5f;
		}
		if (this._windIcon != null)
		{
			this._windIcon.y = num2;
			this._windIcon.color = (item6 ? this._settings.protectedWeatherIconColor : weatherFreezeIconColour);
			num2 += this._windIcon.height + 5f;
		}
		if (this._nightIcon != null)
		{
			this._nightIcon.y = num2;
			this._nightIcon.color = this._settings.weatherFreezeIconColour;
			num2 += this._nightIcon.height + 5f;
		}
	}

	// Token: 0x06000932 RID: 2354 RVA: 0x0004D45E File Offset: 0x0004B65E
	private float HealthToHeight(float health)
	{
		return this._settings.healthBarScale * Mathf.Pow(health, this._settings.healthBarScalePower);
	}

	// Token: 0x06000933 RID: 2355 RVA: 0x0004D480 File Offset: 0x0004B680
	private void RefreshHealthBarLayout(Color blueColourMaybeFlashing)
	{
		Health health = Runner.instance.health;
		float num = 1f;
		if (this._majorHealthChangeAnimStartTime > 0f)
		{
			num = Mathf.InverseLerp(this._majorHealthChangeAnimStartTime + 0.2f, this._majorHealthChangeAnimStartTime + 0.2f + 1f, Time.time);
			if (num >= 1f)
			{
				this._majorHealthChangeAnimStartTime = -1f;
			}
			num = Mathf.SmoothStep(0f, 1f, num);
		}
		float num2 = this._healthBar.y - this._maxHealthBar.y;
		float num3 = this.HealthToHeight(16f) + 2f * num2;
		this._absoluteMaxHealthBar.height = num3;
		float num4 = this.HealthToHeight(health.currentMaxHealth) + 2f * num2;
		float num5 = Mathf.Lerp(this._maxHealthHeightBeforeMajorChange, num4, num);
		this._maxHealthBar.height = num5;
		float num6 = this.HealthToHeight(health.currentHealth);
		float num7 = num6;
		if (this._healthHeightBeforeMajorChange >= 0f)
		{
			if (Mathf.Abs(this._healthHeightBeforeMajorChange - num6) < 5f)
			{
				this._healthHeightBeforeMajorChange = -1f;
			}
			else
			{
				num7 = Mathf.Lerp(this._healthHeightBeforeMajorChange, num6, num);
			}
		}
		this._healthBar.height = num7;
		this._healthBar.fillColor = ((health.availableHeal == AvailableHeal.None) ? this._settings.weatherFreezeHealthBarColour : this._settings.normalHealthBarColour);
		float topY = this._healthBar.topY;
		if (this._healthLossCurrentFullBarHeight >= 0f)
		{
			this._healthLossInitialDelayRemaining -= Time.deltaTime;
			if (this._healthLossInitialDelayRemaining <= 0f)
			{
				this._healthLossInitialDelayRemaining = 0f;
				this._healthLossCurrentFullBarHeight -= 200f * Time.deltaTime;
			}
			if (this._healthLossCurrentFullBarHeight > num5 - num2)
			{
				this._healthLossCurrentFullBarHeight = num5 - num2;
			}
			if (this._healthLossCurrentFullBarHeight <= num6)
			{
				this._healthLossInitialDelayRemaining = -1f;
				this._healthLossCurrentFullBarHeight = -1f;
				this._healthLossBar.Animate(0.2f, delegate
				{
					this._healthLossBar.alpha = 0f;
				});
			}
			else
			{
				float num8 = this._healthBar.y + num6;
				float num9 = this._healthLossCurrentFullBarHeight - num8;
				this._healthLossBar.y = num8;
				this._healthLossBar.height = num9;
				this._healthLossBar.alpha = 1f;
			}
		}
		else
		{
			this._healthLossBar.alpha = Mathf.MoveTowards(this._healthLossBar.alpha, 0f, Time.deltaTime / 0.2f);
		}
		if (health.healthAtRiskForUI > 0f)
		{
			float num10 = this.HealthToHeight(health.currentHealth) - this.HealthToHeight(health.currentHealth - health.healthAtRiskForUI) + num2;
			this._healthRiskBar.y = topY - num10;
			this._healthRiskBar.height = num10;
			this._healthRiskBar.color = blueColourMaybeFlashing;
			return;
		}
		this._healthRiskBar.alpha = 0f;
	}

	// Token: 0x06000934 RID: 2356 RVA: 0x0004D780 File Offset: 0x0004B980
	private void RefreshIcon(ref SLayout icon, ref WeatherHealthEffect currentEffect, WeatherHealthEffect wantedEffect)
	{
		if ((wantedEffect & WeatherHealthEffect.StrongWind) > WeatherHealthEffect.None)
		{
			wantedEffect = WeatherHealthEffect.StrongWind;
		}
		if ((wantedEffect & WeatherHealthEffect.Freezing) > WeatherHealthEffect.None)
		{
			wantedEffect = WeatherHealthEffect.Freezing;
		}
		if ((wantedEffect & WeatherHealthEffect.Cold) > WeatherHealthEffect.None)
		{
			wantedEffect = WeatherHealthEffect.Cold;
		}
		if ((wantedEffect & WeatherHealthEffect.Storm) > WeatherHealthEffect.None)
		{
			wantedEffect = WeatherHealthEffect.Storm;
		}
		if ((wantedEffect == WeatherHealthEffect.None || currentEffect != wantedEffect) && icon != null)
		{
			SLayout iconTemp2 = icon;
			iconTemp2.Animate(0.2f, delegate
			{
				iconTemp2.groupAlpha = 0f;
			}).Then(delegate
			{
				iconTemp2.GetComponent<Prototype>().ReturnToPool();
			});
			icon = null;
		}
		if (wantedEffect != currentEffect && wantedEffect != WeatherHealthEffect.None)
		{
			icon = this._iconPrototype.Instantiate<SLayout>(null);
			icon.groupAlpha = 0f;
			foreach (HealthUISettings.Icon icon2 in this._settings.icons)
			{
				if (icon2.weatherEffect == wantedEffect)
				{
					icon.image.sprite = icon2.sprite;
					icon.image.SetNativeSize();
					break;
				}
			}
			if (icon.image.sprite == null)
			{
				Debug.LogWarning("Icon for weather effect " + wantedEffect.ToString() + " not found");
			}
			SLayout iconTemp = icon;
			icon.CancelAnimations();
			icon.Animate(0.2f, delegate
			{
				iconTemp.groupAlpha = 1f;
			});
		}
		currentEffect = wantedEffect;
	}

	// Token: 0x06000935 RID: 2357 RVA: 0x0004D904 File Offset: 0x0004BB04
	private void OnRunnerBecameActive(bool active)
	{
		if (active)
		{
			this.RefreshImmediate(Runner.instance.health, true);
		}
	}

	// Token: 0x06000937 RID: 2359 RVA: 0x0004D974 File Offset: 0x0004BB74
	[CompilerGenerated]
	internal static ValueTuple<WeatherHealthEffect, bool> <RefreshHealthLayout>g__CheckActiveEffects|15_0(WeatherHealthEffect rawEffects, WeatherHealthEffect protectedEffects, WeatherHealthEffect mask)
	{
		bool flag = false;
		WeatherHealthEffect weatherHealthEffect = rawEffects & mask;
		WeatherHealthEffect weatherHealthEffect2 = protectedEffects & mask;
		WeatherHealthEffect weatherHealthEffect3 = weatherHealthEffect;
		if (weatherHealthEffect2 != weatherHealthEffect)
		{
			if (weatherHealthEffect2 == WeatherHealthEffect.None)
			{
				flag = true;
			}
			else
			{
				weatherHealthEffect3 = weatherHealthEffect2;
			}
		}
		return new ValueTuple<WeatherHealthEffect, bool>(weatherHealthEffect3, flag);
	}

	// Token: 0x04000AFB RID: 2811
	private SLayout _layout;

	// Token: 0x04000AFC RID: 2812
	private WeatherHealthEffect _temperatureIconEffect;

	// Token: 0x04000AFD RID: 2813
	private WeatherHealthEffect _weatherIconEffect;

	// Token: 0x04000AFE RID: 2814
	private WeatherHealthEffect _windIconEffect;

	// Token: 0x04000AFF RID: 2815
	private WeatherHealthEffect _nightIconEffect;

	// Token: 0x04000B00 RID: 2816
	private SLayout _temperatureIcon;

	// Token: 0x04000B01 RID: 2817
	private SLayout _weatherIcon;

	// Token: 0x04000B02 RID: 2818
	private SLayout _windIcon;

	// Token: 0x04000B03 RID: 2819
	private SLayout _nightIcon;

	// Token: 0x04000B04 RID: 2820
	private WeatherHealthEffect _visibleEffectIcons;

	// Token: 0x04000B05 RID: 2821
	private float _lastIconUpdateTime = -1f;

	// Token: 0x04000B06 RID: 2822
	private float _healthLossInitialDelayRemaining = -1f;

	// Token: 0x04000B07 RID: 2823
	private float _healthLossCurrentFullBarHeight = -1f;

	// Token: 0x04000B08 RID: 2824
	private float _majorHealthChangeAnimStartTime = -1f;

	// Token: 0x04000B09 RID: 2825
	private float _maxHealthHeightBeforeMajorChange = -1f;

	// Token: 0x04000B0A RID: 2826
	private float _healthHeightBeforeMajorChange = -1f;

	// Token: 0x04000B0B RID: 2827
	[SerializeField]
	private SLayout _absoluteMaxHealthBar;

	// Token: 0x04000B0C RID: 2828
	[SerializeField]
	private SLayout _maxHealthBar;

	// Token: 0x04000B0D RID: 2829
	[SerializeField]
	private SLayout _maxHealthRiskBar;

	// Token: 0x04000B0E RID: 2830
	[SerializeField]
	private SLayout _healthRiskBar;

	// Token: 0x04000B0F RID: 2831
	[SerializeField]
	private SLayout _healthBar;

	// Token: 0x04000B10 RID: 2832
	[SerializeField]
	private SLayout _healthLossBar;

	// Token: 0x04000B11 RID: 2833
	[SerializeField]
	private Prototype _iconPrototype;

	// Token: 0x04000B12 RID: 2834
	[SerializeField]
	private HealthUISettings _settings;

	// Token: 0x04000B13 RID: 2835
	private const float majorHealthAnimDelay = 0.2f;

	// Token: 0x04000B14 RID: 2836
	private const float majorHealthAnimDuration = 1f;
}
