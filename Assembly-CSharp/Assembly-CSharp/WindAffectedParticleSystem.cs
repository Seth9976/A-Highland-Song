using System;
using UnityEngine;

// Token: 0x02000163 RID: 355
[RequireComponent(typeof(ParticleSystem))]
public class WindAffectedParticleSystem : MonoBehaviour
{
	// Token: 0x06000BF3 RID: 3059 RVA: 0x0005FA7B File Offset: 0x0005DC7B
	[ContextMenu("Apply Immediate")]
	private void ApplyImmediate()
	{
		this.CacheInitialProperties();
		this.Refresh();
	}

	// Token: 0x06000BF4 RID: 3060 RVA: 0x0005FA8C File Offset: 0x0005DC8C
	private void OnEnable()
	{
		this.CacheInitialProperties();
		this.particleSystem.Clear(true);
		this.weatherEmissionRate = this.GetWeatherEmissionRate();
		GameClock.onVisualEffectTimeDidPass = (Action<float>)Delegate.Combine(GameClock.onVisualEffectTimeDidPass, new Action<float>(this.OnGameClockVisualEffectsTimeDidPass));
		this.Update();
	}

	// Token: 0x06000BF5 RID: 3061 RVA: 0x0005FADD File Offset: 0x0005DCDD
	private void OnDisable()
	{
		GameClock.onVisualEffectTimeDidPass = (Action<float>)Delegate.Remove(GameClock.onVisualEffectTimeDidPass, new Action<float>(this.OnGameClockVisualEffectsTimeDidPass));
	}

	// Token: 0x06000BF6 RID: 3062 RVA: 0x0005FAFF File Offset: 0x0005DCFF
	private void Start()
	{
		this.weatherEmissionRate = this.GetWeatherEmissionRate();
		this.Update();
	}

	// Token: 0x06000BF7 RID: 3063 RVA: 0x0005FB13 File Offset: 0x0005DD13
	private void Update()
	{
		if (!Game.loaded)
		{
			return;
		}
		this.UpdateEmissionRate(false);
	}

	// Token: 0x06000BF8 RID: 3064 RVA: 0x0005FB24 File Offset: 0x0005DD24
	public void UpdateEmissionRate(bool immediate)
	{
		float num = (immediate ? float.MaxValue : (Time.unscaledDeltaTime / 1f));
		if (this.modifiedByWeather)
		{
			float num2 = this.GetWeatherEmissionRate();
			if (num2 != this.weatherEmissionRate)
			{
				this.weatherEmissionRate = Mathf.MoveTowards(this.weatherEmissionRate, num2, num);
			}
		}
		else
		{
			this.weatherEmissionRate = 1f;
		}
		if (this.modifiedByWind)
		{
			this.windEmissionRate = this.GetWindEmissionRate();
		}
		else
		{
			this.windEmissionRate = 1f;
		}
		if (this.modifiedByTimeOfDay)
		{
			float num3 = this.GetTimeEmissionRate();
			if (num3 != this.timeEmissionRate)
			{
				this.timeEmissionRate = Mathf.MoveTowards(this.timeEmissionRate, num3, num);
			}
		}
		else
		{
			this.timeEmissionRate = 1f;
		}
		if (this.modifiedByAltitude)
		{
			float num4 = this.GetAltitudeEmissionRate();
			if (num4 != this.altitudeEmissionRate)
			{
				this.altitudeEmissionRate = Mathf.MoveTowards(this.altitudeEmissionRate, num4, num);
				return;
			}
		}
		else
		{
			this.altitudeEmissionRate = 1f;
		}
	}

	// Token: 0x06000BF9 RID: 3065 RVA: 0x0005FC0F File Offset: 0x0005DE0F
	private void LateUpdate()
	{
		this.Refresh();
	}

	// Token: 0x06000BFA RID: 3066 RVA: 0x0005FC17 File Offset: 0x0005DE17
	private void CacheInitialProperties()
	{
		this.particleSystem = base.GetComponent<ParticleSystem>();
	}

	// Token: 0x06000BFB RID: 3067 RVA: 0x0005FC28 File Offset: 0x0005DE28
	private void Refresh()
	{
		if (!Game.loaded)
		{
			return;
		}
		ParticleSystem.MainModule main = this.particleSystem.main;
		main.simulationSpeed = this._simulationSpeed;
		ParticleSystem.EmissionModule emission = this.particleSystem.emission;
		ParticleSystem.ShapeModule shape = this.particleSystem.shape;
		if (Application.isPlaying)
		{
			float num = ((shape.scale.x == 0f) ? 1f : shape.scale.x) * ((shape.scale.y == 0f) ? 1f : shape.scale.y);
			float num2 = Mathf.InverseLerp(0f, 0.25f, CaveRegion.inCaveNorm);
			float num3 = 1f - num2;
			emission.rateOverTimeMultiplier = this.emissionRate * (num * this.volumeScaledEmissionRate) * this.weatherEmissionRate * this.windEmissionRate * this.timeEmissionRate * this.altitudeEmissionRate * num3;
			emission.enabled = emission.rateOverTimeMultiplier > 0f;
		}
		if (emission.enabled)
		{
			WeatherSystem instance = WeatherSystem.instance;
			ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = this.particleSystem.velocityOverLifetime;
			velocityOverLifetime.enabled = true;
			ParticleSystem.MinMaxCurve x = velocityOverLifetime.x;
			x.constantMin = this.velocityXOverWindSpeed.min * instance.windVelocity;
			x.constantMax = this.velocityXOverWindSpeed.max * instance.windVelocity;
			velocityOverLifetime.x = x;
			GameCamera instance2 = GameCamera.instance;
			Runner instance3 = Runner.instance;
			if (this.positionAnchor == WindAffectedParticleSystem.PositionAnchor.Camera)
			{
				Ray ray = Raycast.ViewportPointToRay(new Vector2(0.5f, 0.5f));
				base.transform.position = ray.origin + this.distanceFromCamera * ray.direction;
			}
			else if (this.positionAnchor == WindAffectedParticleSystem.PositionAnchor.Player)
			{
				base.transform.position = instance3.transform.position + instance2.transform.forward * this.distanceFromPlayer;
			}
			if (this.positionAnchor != WindAffectedParticleSystem.PositionAnchor.NONE)
			{
				ParticleSystem.MinMaxCurve startLifetime = main.startLifetime;
				float num4 = ((startLifetime.mode == ParticleSystemCurveMode.Constant) ? startLifetime.constant : Mathf.Lerp(startLifetime.constantMin, startLifetime.constantMax, 0.5f));
				Vector3 vector = num4 * new Vector3(Mathf.Lerp(velocityOverLifetime.x.constantMin, velocityOverLifetime.x.constantMax, 0.5f), Mathf.Lerp(velocityOverLifetime.y.constantMin, velocityOverLifetime.y.constantMax, 0.5f), 0f);
				base.transform.position -= this.positionFudge * vector;
				base.transform.position += this.positionFudge * num4 * instance3.velocity;
			}
			Camera camera = instance2.camera;
			shape.shapeType = ParticleSystemShapeType.Box;
			float num5 = camera.GetFrustrumHeightAtDistance(Mathf.Abs(instance2.transform.position.z - base.transform.position.z)) * this.viewportFillScale;
			float num6 = camera.ConvertFrustumHeightToFrustumWidth(num5);
			shape.scale = new Vector3(this.setWidth ? num6 : shape.scale.x, this.setHeight ? num5 : shape.scale.y, shape.scale.z);
		}
	}

	// Token: 0x06000BFC RID: 3068 RVA: 0x0005FFC4 File Offset: 0x0005E1C4
	private float GetWeatherEmissionRate()
	{
		if (!Game.loaded)
		{
			return 0f;
		}
		WeatherType currentWeather = WeatherSystem.instance.currentWeather;
		bool flag = WeatherSystem.AnyWeatherMatches(currentWeather, this.validWeatherTypes);
		bool flag2 = WeatherSystem.AnyWeatherMatches(currentWeather, this.invalidWeatherTypes);
		return (float)((flag && !flag2) ? 1 : 0);
	}

	// Token: 0x06000BFD RID: 3069 RVA: 0x0006000C File Offset: 0x0005E20C
	private float GetWindEmissionRate()
	{
		return this.windSpeedEmissionRateMultiplier.Evaluate(Mathf.Abs(WeatherSystem.instance.windVelocity));
	}

	// Token: 0x06000BFE RID: 3070 RVA: 0x00060028 File Offset: 0x0005E228
	private float GetTimeEmissionRate()
	{
		return (float)(this.validTimesOfDay.IsSetAtTime(GameClock.instance.hourOfDay) ? 1 : 0);
	}

	// Token: 0x06000BFF RID: 3071 RVA: 0x00060046 File Offset: 0x0005E246
	private float GetAltitudeEmissionRate()
	{
		return this.altitudeEmissionRateMultiplier.Evaluate(Runner.instance.transform.position.y);
	}

	// Token: 0x06000C00 RID: 3072 RVA: 0x00060067 File Offset: 0x0005E267
	private void OnGameClockVisualEffectsTimeDidPass(float visualEffectsDelta)
	{
		this._simulationSpeed = Mathf.Lerp(this._simulationSpeed, visualEffectsDelta, TimeX.Lerping(0.2f, Time.unscaledDeltaTime));
	}

	// Token: 0x04000E27 RID: 3623
	private ParticleSystem particleSystem;

	// Token: 0x04000E28 RID: 3624
	public WindAffectedParticleSystem.PositionAnchor positionAnchor;

	// Token: 0x04000E29 RID: 3625
	public float distanceFromCamera = 4f;

	// Token: 0x04000E2A RID: 3626
	public float distanceFromPlayer = 4f;

	// Token: 0x04000E2B RID: 3627
	public float emissionRate = 1f;

	// Token: 0x04000E2C RID: 3628
	public float volumeScaledEmissionRate = 1f;

	// Token: 0x04000E2D RID: 3629
	public bool modifiedByWeather;

	// Token: 0x04000E2E RID: 3630
	public WeatherType validWeatherTypes;

	// Token: 0x04000E2F RID: 3631
	public WeatherType invalidWeatherTypes;

	// Token: 0x04000E30 RID: 3632
	public float weatherEmissionRate = 1f;

	// Token: 0x04000E31 RID: 3633
	public bool modifiedByTimeOfDay;

	// Token: 0x04000E32 RID: 3634
	public TimeOfDayPicker validTimesOfDay = TimeOfDayPicker.day;

	// Token: 0x04000E33 RID: 3635
	public float timeEmissionRate = 1f;

	// Token: 0x04000E34 RID: 3636
	public bool modifiedByWind;

	// Token: 0x04000E35 RID: 3637
	public AnimationCurve windSpeedEmissionRateMultiplier;

	// Token: 0x04000E36 RID: 3638
	public float windEmissionRate = 1f;

	// Token: 0x04000E37 RID: 3639
	public bool modifiedByAltitude;

	// Token: 0x04000E38 RID: 3640
	public AnimationCurve altitudeEmissionRateMultiplier;

	// Token: 0x04000E39 RID: 3641
	public float altitudeEmissionRate = 1f;

	// Token: 0x04000E3A RID: 3642
	public Range velocityXOverWindSpeed;

	// Token: 0x04000E3B RID: 3643
	public float viewportFillScale = 1.25f;

	// Token: 0x04000E3C RID: 3644
	public bool setWidth = true;

	// Token: 0x04000E3D RID: 3645
	public bool setHeight = true;

	// Token: 0x04000E3E RID: 3646
	public float positionFudge = 0.6f;

	// Token: 0x04000E3F RID: 3647
	private float _simulationSpeed = 1f;

	// Token: 0x02000396 RID: 918
	public enum PositionAnchor
	{
		// Token: 0x0400195A RID: 6490
		NONE,
		// Token: 0x0400195B RID: 6491
		Camera,
		// Token: 0x0400195C RID: 6492
		Player
	}
}
