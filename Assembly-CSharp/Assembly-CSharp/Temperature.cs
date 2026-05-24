using System;
using System.Runtime.CompilerServices;

// Token: 0x02000044 RID: 68
[NullableContext(1)]
[Nullable(new byte[] { 0, 1 })]
public class Temperature : MonoSingleton<Temperature>
{
	// Token: 0x170000AE RID: 174
	// (get) Token: 0x060001FB RID: 507 RVA: 0x000119FB File Offset: 0x0000FBFB
	public bool isCold
	{
		get
		{
			return (this.effect & (WeatherHealthEffect.Cold | WeatherHealthEffect.Freezing)) > WeatherHealthEffect.None;
		}
	}

	// Token: 0x170000AF RID: 175
	// (get) Token: 0x060001FC RID: 508 RVA: 0x00011A0C File Offset: 0x0000FC0C
	public WeatherHealthEffect effect
	{
		get
		{
			float current = this.current;
			if (current < this.settings.freezingTemperature)
			{
				return WeatherHealthEffect.Freezing;
			}
			if (current < this.settings.coldTemperature)
			{
				return WeatherHealthEffect.Cold;
			}
			if (current < this.settings.chillyTemperature)
			{
				return WeatherHealthEffect.Chilly;
			}
			return WeatherHealthEffect.None;
		}
	}

	// Token: 0x170000B0 RID: 176
	// (get) Token: 0x060001FD RID: 509 RVA: 0x00011A52 File Offset: 0x0000FC52
	public float altitudeTemperatureModifier
	{
		get
		{
			return this.settings.additiveTemperatureByHeight.Evaluate(Runner.instance.position.y);
		}
	}

	// Token: 0x170000B1 RID: 177
	// (get) Token: 0x060001FE RID: 510 RVA: 0x00011A73 File Offset: 0x0000FC73
	public float timeTemperatureModifier
	{
		get
		{
			return this.settings.additiveTemperatureByTimeOfDay.Evaluate(GameClock.instance.timeOfDayNorm * 24f);
		}
	}

	// Token: 0x170000B2 RID: 178
	// (get) Token: 0x060001FF RID: 511 RVA: 0x00011A95 File Offset: 0x0000FC95
	public float current
	{
		get
		{
			return this.settings.baseTemperature + this.altitudeTemperatureModifier + this.timeTemperatureModifier;
		}
	}

	// Token: 0x040002CC RID: 716
	[NonSerialized]
	public bool debugOverride;

	// Token: 0x040002CD RID: 717
	[NonSerialized]
	public float debugOverrideTemperature = 15f;

	// Token: 0x040002CE RID: 718
	public TemperatureSettings settings = Presume<TemperatureSettings>.NonNull("C:\\Users\\josep\\Desktop\\highland-song\\Highland Run\\Assets\\Scripts\\Game\\Temperature.cs", 14);
}
