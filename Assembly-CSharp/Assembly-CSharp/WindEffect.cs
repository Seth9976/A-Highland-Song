using System;
using UnityEngine;

// Token: 0x02000165 RID: 357
public class WindEffect : MonoBehaviour
{
	// Token: 0x06000C05 RID: 3077 RVA: 0x00060242 File Offset: 0x0005E442
	private void OnEnable()
	{
		this.foreground.CacheInitialProperties();
		this.mid.CacheInitialProperties();
		this.background.CacheInitialProperties();
		this.Refresh();
	}

	// Token: 0x06000C06 RID: 3078 RVA: 0x0006026B File Offset: 0x0005E46B
	private void Update()
	{
		this.strength = Mathf.MoveTowards(this.strength, this.targetStrength, Time.unscaledDeltaTime * 0.1f);
		this.Refresh();
	}

	// Token: 0x06000C07 RID: 3079 RVA: 0x00060298 File Offset: 0x0005E498
	private void Refresh()
	{
		if (this.foreground.particleSystem != null)
		{
			this.foreground.SetStrength(this.strength, WeatherSystem.instance.windVelocity);
			this.foreground.particleSystem.transform.position = GameCamera.instance.transform.position + GameCamera.instance.transform.forward * this.foregroundDistanceFromCamera;
		}
		if (this.mid.particleSystem != null)
		{
			this.mid.SetStrength(this.strength, WeatherSystem.instance.windVelocity);
			this.mid.particleSystem.transform.position = Runner.instance.transform.position + Runner.instance.transform.forward * this.midDistanceFromPlayer;
		}
		if (this.background.particleSystem != null)
		{
			this.background.SetStrength(this.strength, WeatherSystem.instance.windVelocity);
			this.background.particleSystem.transform.position = Runner.instance.transform.position + Runner.instance.transform.forward * this.backgroundDistanceFromPlayer;
		}
	}

	// Token: 0x04000E41 RID: 3649
	public float strength;

	// Token: 0x04000E42 RID: 3650
	public float targetStrength;

	// Token: 0x04000E43 RID: 3651
	public float foregroundDistanceFromCamera = 4f;

	// Token: 0x04000E44 RID: 3652
	public float midDistanceFromPlayer = -4f;

	// Token: 0x04000E45 RID: 3653
	public float backgroundDistanceFromPlayer = 4f;

	// Token: 0x04000E46 RID: 3654
	public WindEffect.ParticleSystemHandler foreground;

	// Token: 0x04000E47 RID: 3655
	public WindEffect.ParticleSystemHandler mid;

	// Token: 0x04000E48 RID: 3656
	public WindEffect.ParticleSystemHandler background;

	// Token: 0x02000398 RID: 920
	[Serializable]
	public class ParticleSystemHandler
	{
		// Token: 0x06001811 RID: 6161 RVA: 0x0009DDE4 File Offset: 0x0009BFE4
		public void CacheInitialProperties()
		{
			this.initialRateOverTime = this.particleSystem.emission.rateOverTimeMultiplier;
		}

		// Token: 0x06001812 RID: 6162 RVA: 0x0009DE0C File Offset: 0x0009C00C
		public void SetStrength(float strength, float windVelocity)
		{
			if (this.particleSystem == null)
			{
				return;
			}
			ParticleSystem.EmissionModule emission = this.particleSystem.emission;
			emission.enabled = strength > 0f;
			emission.rateOverTimeMultiplier = this.initialRateOverTime * strength;
			ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = this.particleSystem.velocityOverLifetime;
			ParticleSystem.MinMaxCurve x = velocityOverLifetime.x;
			x.constantMin = this.velocityXOverWindSpeed.min * windVelocity;
			x.constantMax = this.velocityXOverWindSpeed.max * windVelocity;
			velocityOverLifetime.x = x;
		}

		// Token: 0x04001963 RID: 6499
		public ParticleSystem particleSystem;

		// Token: 0x04001964 RID: 6500
		public Range velocityXOverWindSpeed;

		// Token: 0x04001965 RID: 6501
		private float initialRateOverTime;
	}
}
