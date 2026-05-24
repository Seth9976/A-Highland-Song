using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000168 RID: 360
[Serializable]
public class WindSystem
{
	// Token: 0x170002CB RID: 715
	// (get) Token: 0x06000C0D RID: 3085 RVA: 0x000604B4 File Offset: 0x0005E6B4
	public WeatherHealthEffect effect
	{
		get
		{
			float num = Mathf.Abs(this.windVelocity);
			if (num > this.windSettings.strongWindSpeed)
			{
				return WeatherHealthEffect.StrongWind;
			}
			if (num > this.windSettings.moderateWindSpeed)
			{
				return WeatherHealthEffect.Wind;
			}
			return WeatherHealthEffect.None;
		}
	}

	// Token: 0x170002CC RID: 716
	// (get) Token: 0x06000C0E RID: 3086 RVA: 0x000604EE File Offset: 0x0005E6EE
	public float windVelocity
	{
		get
		{
			return this._windVelocity;
		}
	}

	// Token: 0x170002CD RID: 717
	// (get) Token: 0x06000C0F RID: 3087 RVA: 0x000604F6 File Offset: 0x0005E6F6
	public bool windyInkFlagSet
	{
		get
		{
			return this.timeWindMagnitudeSufficientForInkFlag > 3f;
		}
	}

	// Token: 0x06000C10 RID: 3088 RVA: 0x00060505 File Offset: 0x0005E705
	public void ApplyShaderParams()
	{
		Shader.SetGlobalTexture("_Global_WindNoiseTexture", this.windSettings.windShaderEffectNoiseTexture);
		Shader.SetGlobalFloat("_Global_WindVelocity", this.windVelocity);
		Shader.SetGlobalFloat("_Global_WindOffset", this._windOffset);
	}

	// Token: 0x06000C11 RID: 3089 RVA: 0x0006053C File Offset: 0x0005E73C
	public void ManualUpdate(float dt)
	{
		float num = Mathf.PerlinNoise(GameClock.instance.daysNorm * this.windSettings.windChangeSpeed, Game.instance.playthroughSeed) * this.windSettings.baseWindMultiplier;
		float num2 = this.ApplyPeakAndRidgeMultipliers(num);
		WindModifierZone windModifierZone = null;
		int num3 = int.MinValue;
		foreach (WindModifierZone windModifierZone2 in MonoInstancer<WindModifierZone>.all)
		{
			if (windModifierZone2.layer > num3 && windModifierZone2.bounds.Contains(Runner.instance.transform.position))
			{
				windModifierZone = windModifierZone2;
				num3 = windModifierZone2.layer;
			}
		}
		if (windModifierZone != null && !this.modifierZoneStrengths.ContainsKey(windModifierZone))
		{
			this.modifierZoneStrengths[windModifierZone] = 0f;
		}
		WindSystem.toIterate.Clear();
		WindSystem.toIterate.AddRange(this.modifierZoneStrengths.Keys);
		WindSystem.toRemove.Clear();
		foreach (WindModifierZone windModifierZone3 in WindSystem.toIterate)
		{
			if (windModifierZone3 == windModifierZone)
			{
				this.modifierZoneStrengths[windModifierZone3] = Mathf.Min(1f, this.modifierZoneStrengths[windModifierZone3] + dt);
			}
			else
			{
				float num4 = this.modifierZoneStrengths[windModifierZone3] - Time.deltaTime;
				if (num4 <= 0f)
				{
					WindSystem.toRemove.Add(windModifierZone3);
					continue;
				}
				this.modifierZoneStrengths[windModifierZone3] = num4;
			}
			num2 = Mathf.Lerp(num2, windModifierZone3.GetWindVelocity(), this.modifierZoneStrengths[windModifierZone3]);
		}
		foreach (WindModifierZone windModifierZone4 in WindSystem.toRemove)
		{
			this.modifierZoneStrengths.Remove(windModifierZone4);
		}
		if (this.windSettings.windOverrideMode == WindSettings.WindOverride.On)
		{
			num2 = this.windSettings.windOverrideVelocity;
		}
		this._windVelocity = num2;
		this._windOffset += this._windVelocity * dt;
		if (Mathf.Abs(this._windVelocity) > 0.5f)
		{
			this.timeWindMagnitudeSufficientForInkFlag += dt;
		}
		else
		{
			this.timeWindMagnitudeSufficientForInkFlag = 0f;
		}
		this.ApplyShaderParams();
	}

	// Token: 0x06000C12 RID: 3090 RVA: 0x000607D4 File Offset: 0x0005E9D4
	private float ApplyPeakAndRidgeMultipliers(float windVelocity)
	{
		windVelocity *= this.windSettings.windHeightMultiplier.Evaluate(Runner.instance.position.y);
		if (Runner.instance.isOnRidge)
		{
			if (PropsController.instance.nearbyPeakProp != null)
			{
				windVelocity *= this.windSettings.peakMultiplier;
			}
			else
			{
				windVelocity *= this.windSettings.ridgeMultiplier;
			}
		}
		windVelocity = Mathf.Clamp(windVelocity, -1f, 1f);
		return windVelocity;
	}

	// Token: 0x04000E56 RID: 3670
	public WindSettings windSettings;

	// Token: 0x04000E57 RID: 3671
	[SerializeField]
	private float _windVelocity;

	// Token: 0x04000E58 RID: 3672
	[SerializeField]
	private float _windOffset;

	// Token: 0x04000E59 RID: 3673
	[SerializeField]
	private float timeWindMagnitudeSufficientForInkFlag;

	// Token: 0x04000E5A RID: 3674
	public Dictionary<WindModifierZone, float> modifierZoneStrengths = new Dictionary<WindModifierZone, float>();

	// Token: 0x04000E5B RID: 3675
	private static List<WindModifierZone> toIterate = new List<WindModifierZone>();

	// Token: 0x04000E5C RID: 3676
	private static List<WindModifierZone> toRemove = new List<WindModifierZone>();
}
