using System;
using UnityEngine;

// Token: 0x020000B8 RID: 184
public class Health : MonoBehaviour
{
	// Token: 0x17000178 RID: 376
	// (get) Token: 0x060005BF RID: 1471 RVA: 0x0002D0FE File Offset: 0x0002B2FE
	// (set) Token: 0x060005C0 RID: 1472 RVA: 0x0002D10B File Offset: 0x0002B30B
	public static Health.WeatherDifficulty weatherDifficulty
	{
		get
		{
			return (Health.WeatherDifficulty)PlayerPrefsX.GetInt(Health.weatherDifficultyPrefName, 0);
		}
		set
		{
			PlayerPrefsX.SetInt(Health.weatherDifficultyPrefName, (int)value);
		}
	}

	// Token: 0x17000179 RID: 377
	// (get) Token: 0x060005C1 RID: 1473 RVA: 0x0002D118 File Offset: 0x0002B318
	public bool isInvincible
	{
		get
		{
			return this.invincibleReason > Health.InvincibleReason.None;
		}
	}

	// Token: 0x1700017A RID: 378
	// (get) Token: 0x060005C2 RID: 1474 RVA: 0x0002D123 File Offset: 0x0002B323
	private bool protectedFromTemperature
	{
		get
		{
			return CaveRegion.inCave || this.shelterComfort == Health.ShelterComfort.Comfortable || this.shelterComfort == Health.ShelterComfort.VeryComfortable;
		}
	}

	// Token: 0x1700017B RID: 379
	// (get) Token: 0x060005C3 RID: 1475 RVA: 0x0002D142 File Offset: 0x0002B342
	public float normalizedCurrentHealth
	{
		get
		{
			return this.currentHealth / 16f;
		}
	}

	// Token: 0x1700017C RID: 380
	// (get) Token: 0x060005C4 RID: 1476 RVA: 0x0002D150 File Offset: 0x0002B350
	public float normalizedCurrentMaxHealth
	{
		get
		{
			return this.currentMaxHealth / 16f;
		}
	}

	// Token: 0x1700017D RID: 381
	// (get) Token: 0x060005C5 RID: 1477 RVA: 0x0002D160 File Offset: 0x0002B360
	public AvailableHeal availableHeal
	{
		get
		{
			if (this.shelterProtectionStrength == ShelterProtectionStrength.Magical)
			{
				return AvailableHeal.Magical;
			}
			if (this.currentHealth >= this.currentMaxHealth)
			{
				return AvailableHeal.Unnecessary;
			}
			WeatherHealthEffect weatherHealthEffects = this.GetWeatherHealthEffects(false);
			WeatherHealthEffect weatherHealthEffects2 = this.GetWeatherHealthEffects(true);
			if ((weatherHealthEffects2 & WeatherHealthEffect.PreventsHealing) > WeatherHealthEffect.None)
			{
				return AvailableHeal.None;
			}
			AvailableHeal availableHeal = AvailableHeal.Full;
			WeatherHealthEffect weatherHealthEffect = weatherHealthEffects2 & WeatherHealthEffect.TemperatureMask;
			if (weatherHealthEffect == WeatherHealthEffect.Freezing)
			{
				availableHeal = AvailableHeal.VerySlow;
			}
			else if (weatherHealthEffect == WeatherHealthEffect.Cold)
			{
				availableHeal = AvailableHeal.Slow;
			}
			else if (weatherHealthEffect == WeatherHealthEffect.Chilly)
			{
				availableHeal = AvailableHeal.Medium;
			}
			AvailableHeal availableHeal2 = AvailableHeal.Full;
			if (this.shelterProtectionStrength == ShelterProtectionStrength.Partial)
			{
				if ((weatherHealthEffects & (WeatherHealthEffect.Storm | WeatherHealthEffect.Snow)) > WeatherHealthEffect.None)
				{
					availableHeal2 = AvailableHeal.Slow;
				}
				else if ((weatherHealthEffects & (WeatherHealthEffect.StrongWind | WeatherHealthEffect.Rain)) > WeatherHealthEffect.None)
				{
					availableHeal2 = AvailableHeal.Medium;
				}
			}
			if (availableHeal < availableHeal2)
			{
				return availableHeal;
			}
			return availableHeal2;
		}
	}

	// Token: 0x060005C6 RID: 1478 RVA: 0x0002D1F0 File Offset: 0x0002B3F0
	public AvailableHeal GetAvailableSleepComfort(string shelterPropName)
	{
		string sleepComfortLevel = Narrative.instance.GetSleepComfortLevel(shelterPropName);
		if (sleepComfortLevel == "NoShelter")
		{
			return AvailableHeal.None;
		}
		if (sleepComfortLevel == "VeryUncomfortable")
		{
			return AvailableHeal.VerySlow;
		}
		if (sleepComfortLevel == "Uncomfortable")
		{
			return AvailableHeal.Slow;
		}
		if (sleepComfortLevel == "Comfortable")
		{
			return AvailableHeal.Medium;
		}
		if (sleepComfortLevel == "VeryComfortable")
		{
			return AvailableHeal.Full;
		}
		if (!(sleepComfortLevel == "Blessed"))
		{
			return AvailableHeal.Unnecessary;
		}
		return AvailableHeal.Magical;
	}

	// Token: 0x1700017E RID: 382
	// (get) Token: 0x060005C7 RID: 1479 RVA: 0x0002D266 File Offset: 0x0002B466
	// (set) Token: 0x060005C8 RID: 1480 RVA: 0x0002D26E File Offset: 0x0002B46E
	public HealthEffect activeHealthEffect { get; private set; }

	// Token: 0x060005C9 RID: 1481 RVA: 0x0002D278 File Offset: 0x0002B478
	public WeatherHealthEffect GetWeatherHealthEffects(bool withProtection)
	{
		WeatherHealthEffect weatherHealthEffect = WeatherHealthEffect.None;
		if (WeatherSystem.instance == null)
		{
			return weatherHealthEffect;
		}
		if (Runner.instance.hidden && withProtection)
		{
			return weatherHealthEffect;
		}
		WeatherHealthEffect weatherHealthEffect2 = WeatherHealthEffect.None;
		WeatherType currentWeather = WeatherSystem.instance.currentWeather;
		if (currentWeather.HasFlag(WeatherType.Snow))
		{
			weatherHealthEffect2 |= WeatherHealthEffect.Snow;
		}
		if (currentWeather.HasFlag(WeatherType.Storm))
		{
			weatherHealthEffect2 |= WeatherHealthEffect.Storm;
		}
		if (currentWeather.HasFlag(WeatherType.Raining))
		{
			weatherHealthEffect2 |= WeatherHealthEffect.Rain;
		}
		if (withProtection)
		{
			if (this.shelterProtectionStrength >= ShelterProtectionStrength.Partial)
			{
				weatherHealthEffect2 = WeatherHealthEffect.None;
			}
			weatherHealthEffect |= weatherHealthEffect2;
		}
		else
		{
			weatherHealthEffect |= weatherHealthEffect2;
		}
		WeatherHealthEffect weatherHealthEffect3 = WeatherSystem.instance.windSystem.effect;
		if (withProtection)
		{
			if (this.shelterProtectionStrength >= ShelterProtectionStrength.Partial)
			{
				weatherHealthEffect3 = WeatherHealthEffect.None;
			}
			weatherHealthEffect |= weatherHealthEffect3;
		}
		else
		{
			weatherHealthEffect |= weatherHealthEffect3;
		}
		if (!withProtection || !this.protectedFromTemperature)
		{
			WeatherHealthEffect weatherHealthEffect4 = MonoSingleton<Temperature>.instance.effect;
			if (weatherHealthEffect2.HasFlag(WeatherHealthEffect.Snow))
			{
				if (weatherHealthEffect4 == WeatherHealthEffect.Cold)
				{
					weatherHealthEffect4 = WeatherHealthEffect.Freezing;
				}
				else if (weatherHealthEffect4 == WeatherHealthEffect.Chilly)
				{
					weatherHealthEffect4 = WeatherHealthEffect.Cold;
				}
				else if (weatherHealthEffect4 == WeatherHealthEffect.None)
				{
					weatherHealthEffect4 = WeatherHealthEffect.Chilly;
				}
			}
			weatherHealthEffect |= weatherHealthEffect4;
		}
		if (GameClock.instance.isNight)
		{
			weatherHealthEffect |= WeatherHealthEffect.Night;
		}
		return weatherHealthEffect;
	}

	// Token: 0x060005CA RID: 1482 RVA: 0x0002D3A4 File Offset: 0x0002B5A4
	public void SetFromInk(float newHealth)
	{
		this.SetHealthInternal(newHealth, DamageType.Ink);
	}

	// Token: 0x1700017F RID: 383
	// (get) Token: 0x060005CB RID: 1483 RVA: 0x0002D3AE File Offset: 0x0002B5AE
	// (set) Token: 0x060005CC RID: 1484 RVA: 0x0002D3B6 File Offset: 0x0002B5B6
	public Health.ShelterComfort shelterComfort { get; private set; }

	// Token: 0x060005CD RID: 1485 RVA: 0x0002D3C0 File Offset: 0x0002B5C0
	public void SetShelterComfortFromInk(string comfortName)
	{
		if (comfortName == "NotResting")
		{
			this.shelterComfort = Health.ShelterComfort.NONE;
			return;
		}
		if (comfortName == "Comfortable")
		{
			this.shelterComfort = Health.ShelterComfort.Comfortable;
			return;
		}
		if (comfortName == "VeryComfortable")
		{
			this.shelterComfort = Health.ShelterComfort.VeryComfortable;
			return;
		}
		if (comfortName == "VeryUncomfortable")
		{
			this.shelterComfort = Health.ShelterComfort.VeryUncomfortable;
			return;
		}
		if (comfortName == "Uncomfortable")
		{
			this.shelterComfort = Health.ShelterComfort.Uncomfortable;
			return;
		}
		if (!(comfortName == "NoShelter"))
		{
			Debug.LogError("Unrecognised ink resting comfort list item: " + comfortName + ". Please make sure RestingComfort LIST in ink is the same as RestingComfort enum in C#");
			return;
		}
		this.shelterComfort = Health.ShelterComfort.NoShelter;
	}

	// Token: 0x17000180 RID: 384
	// (get) Token: 0x060005CE RID: 1486 RVA: 0x0002D464 File Offset: 0x0002B664
	private float _weatherDifficultyModifier
	{
		get
		{
			Health.WeatherDifficulty weatherDifficulty = Health.weatherDifficulty;
			if (weatherDifficulty == Health.WeatherDifficulty.Mild)
			{
				return this._settings.mildDifficultyScalar;
			}
			if (weatherDifficulty == Health.WeatherDifficulty.Harsh)
			{
				return this._settings.harshDifficultyScalar;
			}
			return 1f;
		}
	}

	// Token: 0x060005CF RID: 1487 RVA: 0x0002D49C File Offset: 0x0002B69C
	private float DamageCost(DamageType damageType, Damage damage)
	{
		if (damageType != DamageType.Fall)
		{
			if (damageType == DamageType.LowStaminaHurt)
			{
				return 0.5f * this.weatherBasedDamageScalar;
			}
			if (damageType != DamageType.Weather)
			{
				switch (damage)
				{
				case Damage.None:
					goto IL_00AB;
				case Damage.MinorDamage:
					return 1f;
				case Damage.MajorDamage:
					return 2f;
				case Damage.Death:
					break;
				default:
					goto IL_00C3;
				}
			}
			else
			{
				if (damage == Damage.MinorDamage)
				{
					return 1f * this._weatherDifficultyModifier;
				}
				if (damage == Damage.MajorDamage)
				{
					return 2f * this._weatherDifficultyModifier;
				}
				if (damage != Damage.Death)
				{
					if (damage != Damage.None)
					{
						goto IL_00C3;
					}
					goto IL_00AB;
				}
			}
		}
		else
		{
			switch (damage)
			{
			case Damage.None:
				goto IL_00AB;
			case Damage.MinorDamage:
				return 1f * this.weatherBasedDamageScalar;
			case Damage.MajorDamage:
				return 2f * this.weatherBasedDamageScalar;
			case Damage.Death:
				break;
			default:
				goto IL_00C3;
			}
		}
		return this.currentHealth;
		IL_00AB:
		return 0f;
		IL_00C3:
		return 1f;
	}

	// Token: 0x060005D0 RID: 1488 RVA: 0x0002D574 File Offset: 0x0002B774
	public bool ApplyDamage(DamageType damageType, Damage damage)
	{
		if (this.isInvincible && damageType != DamageType.FastResurrection)
		{
			return false;
		}
		float num = this.DamageCost(damageType, damage);
		return num != 0f && this.ApplyDamage(damageType, num);
	}

	// Token: 0x060005D1 RID: 1489 RVA: 0x0002D5AC File Offset: 0x0002B7AC
	public bool ApplyDamage(DamageType damageType, float unitCost)
	{
		if (damageType == DamageType.Weather && this.currentHealth - unitCost < 0.9f)
		{
			unitCost = Mathf.Max(this.currentHealth - 0.9f, 0f);
		}
		if (unitCost >= this.currentHealth)
		{
			unitCost = this.currentHealth;
		}
		bool flag = this.currentHealth > 0f;
		this.SetHealthInternal(this.currentHealth - unitCost, damageType);
		if (unitCost >= 2f)
		{
			GameCamera.instance.cameraShakeState.StartShake(CameraShakeName.Major);
		}
		if (unitCost >= 1f)
		{
			GameCamera.instance.cameraShakeState.StartShake(CameraShakeName.Minor);
		}
		return flag && this.currentHealth == 0f;
	}

	// Token: 0x060005D2 RID: 1490 RVA: 0x0002D654 File Offset: 0x0002B854
	public void ApplyHealing(float healAmount)
	{
		float num = Mathf.Min(this.currentHealth + healAmount, this.currentMaxHealth);
		if (num > this.currentHealth)
		{
			this.SetHealthInternal(num, DamageType.NotDamageOrNone);
		}
	}

	// Token: 0x060005D3 RID: 1491 RVA: 0x0002D686 File Offset: 0x0002B886
	public void RestoreAfterFastDeathReset(float restoreToHealth)
	{
		if (this.currentHealth < restoreToHealth)
		{
			this.causeOfDeath = DamageType.NotDamageOrNone;
			this.SetHealthInternal(restoreToHealth, DamageType.NotDamageOrNone);
		}
	}

	// Token: 0x060005D4 RID: 1492 RVA: 0x0002D6A0 File Offset: 0x0002B8A0
	public void TryMusicRunSuccessHeal(bool forceInEditor = false)
	{
		if (GameClock.instance.dayIdx > this.lastMusicRunHealthBoostDayIdx || forceInEditor)
		{
			if (Health.onHealthWillDoMajorChange != null)
			{
				Health.onHealthWillDoMajorChange();
			}
			this.currentMaxHealth = Mathf.Min(this.currentMaxHealth + this._settings.musicRunningMaxHealthBoost, 16f);
			this.ApplyHealing(this._settings.musicRunningHealthBoost);
			this.lastMusicRunHealthBoostDayIdx = GameClock.instance.dayIdx;
		}
	}

	// Token: 0x060005D5 RID: 1493 RVA: 0x0002D717 File Offset: 0x0002B917
	public void ResetToMax()
	{
		if (this.currentMaxHealth != 16f)
		{
			this.currentMaxHealth = 16f;
		}
		if (this.currentHealth != 16f)
		{
			this.SetHealthInternal(16f, DamageType.NotDamageOrNone);
		}
		this.causeOfDeath = DamageType.NotDamageOrNone;
	}

	// Token: 0x060005D6 RID: 1494 RVA: 0x0002D751 File Offset: 0x0002B951
	public void SetInvincible(bool invincible, Health.InvincibleReason reason)
	{
		if (invincible)
		{
			this.invincibleReason |= reason;
			return;
		}
		this.invincibleReason &= ~reason;
	}

	// Token: 0x060005D7 RID: 1495 RVA: 0x0002D774 File Offset: 0x0002B974
	private void SetHealthInternal(float newHealth, DamageType damageType)
	{
		if (this.currentHealth == newHealth)
		{
			return;
		}
		if (damageType != DamageType.Ink && damageType != DamageType.FastResurrection && this.isInvincible && newHealth < this.currentHealth && this.currentHealth <= this.currentMaxHealth)
		{
			return;
		}
		float num = this.currentHealth;
		this.currentHealth = newHealth;
		if (num > 0f && this.currentHealth == 0f)
		{
			this.causeOfDeath = damageType;
		}
		if (Health.onHealthChanged != null)
		{
			Health.onHealthChanged(this, this.currentHealth - num);
		}
	}

	// Token: 0x060005D8 RID: 1496 RVA: 0x0002D7F8 File Offset: 0x0002B9F8
	public void ApplyFromSave(float currentHealth, float currentMaxHealth, Damage lastWeatherHealthImpact, float weatherRecoverTimeRemaining)
	{
		this.currentHealth = currentHealth;
		this.currentMaxHealth = currentMaxHealth;
		this.healthAtRiskForUI = 0f;
		this.causeOfDeath = DamageType.NotDamageOrNone;
		this.lastWeatherHealthImpact = lastWeatherHealthImpact;
		this.weatherRecoverDaysNormRemaining = weatherRecoverTimeRemaining;
		this._weatherHurtTime = 0f;
		if (Health.onHealthChanged != null)
		{
			Health.onHealthChanged(this, 0f);
		}
	}

	// Token: 0x060005D9 RID: 1497 RVA: 0x0002D858 File Offset: 0x0002BA58
	private void OnEnable()
	{
		this.healthAtRiskForUI = 0f;
		GameClock.onTimeDidPass = (Action<float>)Delegate.Combine(GameClock.onTimeDidPass, new Action<float>(this.OnTimeDidPass));
		GameClock.onCaveTimeDidNotPass = (Action<float>)Delegate.Combine(GameClock.onCaveTimeDidNotPass, new Action<float>(this.OnTimeDidPass));
	}

	// Token: 0x060005DA RID: 1498 RVA: 0x0002D8B0 File Offset: 0x0002BAB0
	private void OnDisable()
	{
		GameClock.onTimeDidPass = (Action<float>)Delegate.Remove(GameClock.onTimeDidPass, new Action<float>(this.OnTimeDidPass));
		GameClock.onCaveTimeDidNotPass = (Action<float>)Delegate.Remove(GameClock.onCaveTimeDidNotPass, new Action<float>(this.OnTimeDidPass));
	}

	// Token: 0x060005DB RID: 1499 RVA: 0x0002D900 File Offset: 0x0002BB00
	private void OnTimeDidPass(float daysNormDelta)
	{
		if (Game.instance.isPathFollowing)
		{
			return;
		}
		if (GameCamera.instance.introCameraState.active)
		{
			return;
		}
		Runner instance = Runner.instance;
		if (instance == null || instance.dead)
		{
			return;
		}
		AvailableHeal availableHeal = this.availableHeal;
		HealthEffect healthEffect = new HealthEffect
		{
			healthPerDay = 0f,
			maxHealthPerDay = 0f
		};
		WeatherHealthEffect weatherHealthEffects = this.GetWeatherHealthEffects(true);
		Damage damage = Damage.None;
		if ((weatherHealthEffects & (WeatherHealthEffect.Freezing | WeatherHealthEffect.Storm | WeatherHealthEffect.Snow)) > WeatherHealthEffect.None)
		{
			this.weatherBasedDamageScalar = this._settings.badWeatherDamageScalar;
			damage = Damage.MajorDamage;
		}
		else if ((weatherHealthEffects & (WeatherHealthEffect.StrongWind | WeatherHealthEffect.Rain)) > WeatherHealthEffect.None)
		{
			this.weatherBasedDamageScalar = this._settings.harshWeatherDamageScalar;
			damage = Damage.MinorDamage;
		}
		else
		{
			this.weatherBasedDamageScalar = 1f;
		}
		this.weatherBasedDamageScalar *= this._weatherDifficultyModifier;
		if (damage > this.lastWeatherHealthImpact && this._weatherHurtTime == 0f)
		{
			this._weatherHurtTime = this._settings.weatherHurtDelayTime;
		}
		else if (damage == Damage.None && this._weatherHurtTime > 0f)
		{
			this._weatherHurtTime = 0f;
		}
		if (this._weatherHurtTime > 0f)
		{
			this._weatherHurtTime -= Time.deltaTime;
			Damage damage2 = Damage.None;
			if ((weatherHealthEffects & (WeatherHealthEffect.Freezing | WeatherHealthEffect.Storm | WeatherHealthEffect.Snow)) > WeatherHealthEffect.None)
			{
				damage2 = ((this.lastWeatherHealthImpact == Damage.None) ? Damage.MajorDamage : Damage.MinorDamage);
			}
			else if ((weatherHealthEffects & (WeatherHealthEffect.StrongWind | WeatherHealthEffect.Rain)) > WeatherHealthEffect.None)
			{
				damage2 = Damage.MinorDamage;
			}
			if (this._weatherHurtTime <= 0f)
			{
				instance.health.ApplyDamage(DamageType.Weather, damage2);
				this.healthAtRiskForUI = 0f;
				this.lastWeatherHealthImpact = damage;
				this.weatherRecoverDaysNormRemaining = this._settings.weatherRecoverTimeDaysNorm;
				this._weatherHurtTime = 0f;
			}
			else
			{
				float num = 1f - this._weatherHurtTime / this._settings.weatherHurtDelayTime;
				this.healthAtRiskForUI = num * this.DamageCost(DamageType.Weather, damage2);
			}
		}
		else
		{
			this.healthAtRiskForUI = 0f;
		}
		if (this.weatherRecoverDaysNormRemaining > 0f && damage == Damage.None)
		{
			this.weatherRecoverDaysNormRemaining -= daysNormDelta;
			if (this.weatherRecoverDaysNormRemaining < 0f)
			{
				this.weatherRecoverDaysNormRemaining = 0f;
				this.lastWeatherHealthImpact = Damage.None;
			}
		}
		if (MonoSingleton<RestStateController>.instance.restingOrSitting && availableHeal != AvailableHeal.None)
		{
			if (availableHeal == AvailableHeal.VerySlow)
			{
				healthEffect.healthPerDay += this._settings.restingHealthEffect.healthPerDay * this._settings.verySlowHealingScalar;
			}
			else if (availableHeal == AvailableHeal.Slow)
			{
				healthEffect.healthPerDay += this._settings.restingHealthEffect.healthPerDay * this._settings.slowHealingScalar;
			}
			else if (availableHeal == AvailableHeal.Medium)
			{
				healthEffect.healthPerDay += this._settings.restingHealthEffect.healthPerDay * this._settings.mediumHealingScalar;
			}
			else
			{
				healthEffect.healthPerDay += this._settings.restingHealthEffect.healthPerDay * 1f;
			}
			if (availableHeal == AvailableHeal.Magical)
			{
				healthEffect.maxHealthPerDay = this._settings.maxHealthRefreshEffectInBlessedPeakWeatherModifierZone.maxHealthPerDay;
			}
		}
		else if (instance.position.y < 0f && ((instance.running && instance.stoppedBasically) || instance.resting || instance.sitting || instance.stoneSkimming))
		{
			healthEffect += this._settings.waterHealthEffect;
		}
		switch (this.shelterComfort)
		{
		case Health.ShelterComfort.Comfortable:
			healthEffect = this._settings.comfortableShelter;
			break;
		case Health.ShelterComfort.VeryComfortable:
			healthEffect = this._settings.veryComfortableShelter;
			break;
		case Health.ShelterComfort.Uncomfortable:
			healthEffect = this._settings.uncomfortableShelter;
			break;
		case Health.ShelterComfort.VeryUncomfortable:
			healthEffect = this._settings.veryUncomfortableShelter;
			break;
		case Health.ShelterComfort.NoShelter:
			healthEffect = this._settings.noShelterAtAll;
			break;
		}
		if (healthEffect.healthPerDay < 0f)
		{
			if (Health.weatherDifficulty == Health.WeatherDifficulty.Mild)
			{
				healthEffect.healthPerDay *= this._settings.mildDifficultyScalar;
			}
			else if (Health.weatherDifficulty == Health.WeatherDifficulty.Harsh)
			{
				healthEffect.healthPerDay *= this._settings.harshDifficultyScalar;
			}
		}
		if (healthEffect.maxHealthPerDay < 0f)
		{
			if (Health.weatherDifficulty == Health.WeatherDifficulty.Mild)
			{
				healthEffect.maxHealthPerDay *= this._settings.mildDifficultyScalar;
			}
			else if (Health.weatherDifficulty == Health.WeatherDifficulty.Harsh)
			{
				healthEffect.maxHealthPerDay *= this._settings.harshDifficultyScalar;
			}
		}
		float num2 = this.currentMaxHealth + healthEffect.maxHealthPerDay * daysNormDelta;
		num2 = Mathf.Clamp(num2, this._settings.minimumMaxHealth, 16f);
		this.currentMaxHealth = num2;
		float num3 = this.currentHealth + healthEffect.healthPerDay * daysNormDelta;
		num3 = Mathf.Clamp(num3, 0f, this.currentMaxHealth);
		this.SetHealthInternal(num3, DamageType.Environment);
		this.activeHealthEffect = healthEffect;
	}

	// Token: 0x060005DC RID: 1500 RVA: 0x0002DDB8 File Offset: 0x0002BFB8
	private void Update()
	{
		if (!Game.loaded)
		{
			return;
		}
		this.SetInvincible(MonoSingleton<PeakStateController>.instance.active, Health.InvincibleReason.PeakState);
		this.SetInvincible(Runner.instance.playerControlDisabled > PlayerControlDisableReason.None, Health.InvincibleReason.NoPlayerControl);
		this.SetInvincible(Game.gameplayPaused, Health.InvincibleReason.GameplayPaused);
		this.SetInvincible(DebugOptions.opts.godMode, Health.InvincibleReason.DebugOption);
		this.RefreshShelterProtectionStrength();
	}

	// Token: 0x060005DD RID: 1501 RVA: 0x0002DE18 File Offset: 0x0002C018
	public void RefreshShelterProtectionStrength()
	{
		this.currentWeatherModifierZone = null;
		this.shelterProtectionStrength = ShelterProtectionStrength.None;
		if (CaveRegion.inCave)
		{
			this.shelterProtectionStrength = ShelterProtectionStrength.Full;
			return;
		}
		Vector3 physicalPosition3d = Runner.instance.physicalPosition3d;
		float num = float.MaxValue;
		foreach (WeatherModifierZone weatherModifierZone in Level.current.weatherModifierZones.Nearby(physicalPosition3d, new Range(-1000000f, 1000000f), 0f, null))
		{
			if (weatherModifierZone.enabled && weatherModifierZone.ContainsPoint(physicalPosition3d))
			{
				if (weatherModifierZone.protectionFromTemperature > this.shelterProtectionStrength)
				{
					this.shelterProtectionStrength = weatherModifierZone.protectionFromTemperature;
				}
				if (weatherModifierZone.protectionFromWind > this.shelterProtectionStrength)
				{
					this.shelterProtectionStrength = weatherModifierZone.protectionFromWind;
				}
				if (weatherModifierZone.protectionFromWeather > this.shelterProtectionStrength)
				{
					this.shelterProtectionStrength = weatherModifierZone.protectionFromWeather;
				}
				if (weatherModifierZone.provideMaxHealthRefreshOnRestingWithin)
				{
					this.shelterProtectionStrength = ShelterProtectionStrength.Magical;
				}
				float area = weatherModifierZone.area;
				if (area < num)
				{
					num = area;
					this.currentWeatherModifierZone = weatherModifierZone;
				}
			}
		}
		if (this.shelterComfort == Health.ShelterComfort.Comfortable || this.shelterComfort == Health.ShelterComfort.VeryComfortable)
		{
			this.shelterProtectionStrength = ShelterProtectionStrength.Full;
		}
	}

	// Token: 0x040006AD RID: 1709
	public static Action<Health, float> onHealthChanged;

	// Token: 0x040006AE RID: 1710
	public static Action onHealthWillDoMajorChange;

	// Token: 0x040006AF RID: 1711
	public static string weatherDifficultyPrefName = "weatherDifficulty";

	// Token: 0x040006B0 RID: 1712
	public const float absoluteMaxHealth = 16f;

	// Token: 0x040006B1 RID: 1713
	public float currentMaxHealth = 16f;

	// Token: 0x040006B2 RID: 1714
	public float currentHealth = 16f;

	// Token: 0x040006B3 RID: 1715
	public float healthAtRiskForUI;

	// Token: 0x040006B4 RID: 1716
	public int lastMusicRunHealthBoostDayIdx = -1;

	// Token: 0x040006B5 RID: 1717
	public Damage lastWeatherHealthImpact;

	// Token: 0x040006B6 RID: 1718
	public float weatherRecoverDaysNormRemaining;

	// Token: 0x040006B7 RID: 1719
	public DamageType causeOfDeath = DamageType.NotDamageOrNone;

	// Token: 0x040006B8 RID: 1720
	public Health.InvincibleReason invincibleReason;

	// Token: 0x040006B9 RID: 1721
	[Disable]
	public ShelterProtectionStrength shelterProtectionStrength;

	// Token: 0x040006BA RID: 1722
	[Disable]
	public WeatherModifierZone currentWeatherModifierZone;

	// Token: 0x040006BB RID: 1723
	private float weatherBasedDamageScalar = 1f;

	// Token: 0x040006BE RID: 1726
	private float _weatherHurtTime;

	// Token: 0x040006BF RID: 1727
	[SerializeField]
	private HealthSettings _settings = Presume<HealthSettings>.NonNull("C:\\Users\\josep\\Desktop\\highland-song\\Highland Run\\Assets\\Scripts\\Runner\\Health.cs", 761);

	// Token: 0x020002D8 RID: 728
	public enum WeatherDifficulty
	{
		// Token: 0x04001666 RID: 5734
		Mild,
		// Token: 0x04001667 RID: 5735
		Moderate,
		// Token: 0x04001668 RID: 5736
		Harsh
	}

	// Token: 0x020002D9 RID: 729
	[Flags]
	public enum InvincibleReason
	{
		// Token: 0x0400166A RID: 5738
		None = 0,
		// Token: 0x0400166B RID: 5739
		InkScene = 1,
		// Token: 0x0400166C RID: 5740
		PeakState = 2,
		// Token: 0x0400166D RID: 5741
		NoPlayerControl = 4,
		// Token: 0x0400166E RID: 5742
		GameplayPaused = 8,
		// Token: 0x0400166F RID: 5743
		DebugOption = 16,
		// Token: 0x04001670 RID: 5744
		TrailerAndScreenshotWindow = 16
	}

	// Token: 0x020002DA RID: 730
	public enum ShelterComfort
	{
		// Token: 0x04001672 RID: 5746
		NONE,
		// Token: 0x04001673 RID: 5747
		Comfortable,
		// Token: 0x04001674 RID: 5748
		VeryComfortable,
		// Token: 0x04001675 RID: 5749
		Uncomfortable,
		// Token: 0x04001676 RID: 5750
		VeryUncomfortable,
		// Token: 0x04001677 RID: 5751
		NoShelter
	}
}
