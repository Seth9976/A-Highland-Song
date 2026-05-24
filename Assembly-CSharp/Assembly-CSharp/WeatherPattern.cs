using System;
using UnityEngine;

// Token: 0x0200015F RID: 351
[CreateAssetMenu]
public class WeatherPattern : ScriptableObject
{
	// Token: 0x04000DF6 RID: 3574
	public WeatherPattern.Timing cloudyPeriod;

	// Token: 0x04000DF7 RID: 3575
	public WeatherPattern.Timing veryCloudyPeriod;

	// Token: 0x04000DF8 RID: 3576
	public WeatherPattern.Timing rainPeriod;

	// Token: 0x04000DF9 RID: 3577
	public WeatherPattern.Timing stormyPeriod;

	// Token: 0x04000DFA RID: 3578
	public float probabilityThatRainIsSnow;

	// Token: 0x0200038F RID: 911
	[Serializable]
	public struct Timing
	{
		// Token: 0x04001942 RID: 6466
		public Range hoursInactive;

		// Token: 0x04001943 RID: 6467
		public Range hoursActive;

		// Token: 0x04001944 RID: 6468
		public float probability;
	}

	// Token: 0x02000390 RID: 912
	[Serializable]
	public struct Period
	{
		// Token: 0x06001800 RID: 6144 RVA: 0x0009D852 File Offset: 0x0009BA52
		private static float RandomDurationDaysNorm(Range hoursRange)
		{
			return hoursRange.RandomBell(1) / 24f;
		}

		// Token: 0x06001801 RID: 6145 RVA: 0x0009D864 File Offset: 0x0009BA64
		public Period(float now, WeatherPattern.Timing setting)
		{
			this.start = now;
			this.activate = now + WeatherPattern.Period.RandomDurationDaysNorm(setting.hoursInactive);
			this.end = this.activate + WeatherPattern.Period.RandomDurationDaysNorm(setting.hoursActive);
			if (Random.value >= setting.probability)
			{
				this.activate = this.end;
			}
			this.valid = true;
		}

		// Token: 0x06001802 RID: 6146 RVA: 0x0009D8C8 File Offset: 0x0009BAC8
		public Period(float now, WeatherPattern.Timing setting, WeatherPattern.Period dependentPeriod, float endMargin)
		{
			if (!dependentPeriod.isValid)
			{
				this = WeatherPattern.Period.invalid;
				return;
			}
			Range range = setting.hoursInactive;
			if (now > dependentPeriod.activate)
			{
				this.start = now;
				range = setting.hoursInactive;
			}
			else
			{
				this.start = dependentPeriod.activate;
				range = new Range(0.5f * range.min, 0.5f * range.max);
			}
			this.activate = this.start + WeatherPattern.Period.RandomDurationDaysNorm(range);
			this.end = this.activate + WeatherPattern.Period.RandomDurationDaysNorm(setting.hoursActive);
			float num = dependentPeriod.end - endMargin;
			if (this.activate > num)
			{
				this.activate = (this.end = dependentPeriod.end);
			}
			else if (this.end > num)
			{
				this.end = num;
			}
			float num2 = Mathf.Max(0.5f * setting.hoursActive.min / 24f, 0.010416667f);
			if (this.end - this.activate < num2)
			{
				this.activate = (this.end = dependentPeriod.end);
			}
			if (Random.value >= setting.probability)
			{
				this.activate = this.end;
			}
			this.valid = true;
		}

		// Token: 0x06001803 RID: 6147 RVA: 0x0009DA03 File Offset: 0x0009BC03
		public bool IsActive(float time)
		{
			return time > this.activate && time < this.end;
		}

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x06001804 RID: 6148 RVA: 0x0009DA19 File Offset: 0x0009BC19
		public bool isValid
		{
			get
			{
				return this.valid;
			}
		}

		// Token: 0x04001945 RID: 6469
		public float start;

		// Token: 0x04001946 RID: 6470
		public float activate;

		// Token: 0x04001947 RID: 6471
		public float end;

		// Token: 0x04001948 RID: 6472
		public bool valid;

		// Token: 0x04001949 RID: 6473
		private const int randomBellShape = 1;

		// Token: 0x0400194A RID: 6474
		public static WeatherPattern.Period invalid = new WeatherPattern.Period
		{
			valid = false,
			start = -1f,
			activate = -1f,
			end = -1f
		};
	}

	// Token: 0x02000391 RID: 913
	[Serializable]
	public class State
	{
		// Token: 0x06001806 RID: 6150 RVA: 0x0009DA6B File Offset: 0x0009BC6B
		public void Reset()
		{
			this.currentCloudyPeriod = WeatherPattern.Period.invalid;
			this.currentVeryCloudyPeriod = WeatherPattern.Period.invalid;
			this.currentRainPeriod = WeatherPattern.Period.invalid;
			this.currentStormPeriod = WeatherPattern.Period.invalid;
		}

		// Token: 0x06001807 RID: 6151 RVA: 0x0009DA9C File Offset: 0x0009BC9C
		public WeatherType WeatherTypeAt(float daysNorm)
		{
			WeatherType weatherType = WeatherType.Clear;
			if (this.currentCloudyPeriod.IsActive(daysNorm))
			{
				weatherType |= WeatherType.Cloudy;
			}
			if (this.currentVeryCloudyPeriod.IsActive(daysNorm))
			{
				weatherType |= WeatherType.VeryCloudy;
			}
			if (this.currentRainPeriod.IsActive(daysNorm))
			{
				weatherType |= WeatherType.Raining;
			}
			if (this.currentStormPeriod.IsActive(daysNorm))
			{
				weatherType |= WeatherType.Storm;
			}
			if ((weatherType & WeatherType.Raining) > WeatherType.Clear && this.rainIsSnow)
			{
				weatherType &= ~WeatherType.Raining;
				weatherType |= WeatherType.Snow;
			}
			return weatherType;
		}

		// Token: 0x06001808 RID: 6152 RVA: 0x0009DB10 File Offset: 0x0009BD10
		public void UpdatePeriods(float daysNorm, WeatherPattern weatherPattern)
		{
			if (!this.currentCloudyPeriod.isValid || daysNorm > this.currentCloudyPeriod.end)
			{
				this.currentCloudyPeriod = new WeatherPattern.Period(daysNorm, weatherPattern.cloudyPeriod);
				this.currentVeryCloudyPeriod = WeatherPattern.Period.invalid;
				this.currentRainPeriod = WeatherPattern.Period.invalid;
			}
			if ((!this.currentVeryCloudyPeriod.isValid || daysNorm > this.currentVeryCloudyPeriod.end) && this.currentCloudyPeriod.isValid)
			{
				this.currentVeryCloudyPeriod = new WeatherPattern.Period(daysNorm, weatherPattern.veryCloudyPeriod, this.currentCloudyPeriod, 0.010416667f);
			}
			if ((!this.currentRainPeriod.isValid || daysNorm > this.currentRainPeriod.end) && this.currentCloudyPeriod.isValid)
			{
				this.currentRainPeriod = new WeatherPattern.Period(daysNorm, weatherPattern.rainPeriod, this.currentCloudyPeriod, 0.010416667f);
				this.currentStormPeriod = WeatherPattern.Period.invalid;
				this.rainIsSnow = Random.value < weatherPattern.probabilityThatRainIsSnow;
			}
			if ((!this.currentStormPeriod.isValid || daysNorm > this.currentStormPeriod.end) && this.currentRainPeriod.isValid)
			{
				this.currentStormPeriod = new WeatherPattern.Period(daysNorm, weatherPattern.stormyPeriod, this.currentRainPeriod, 0f);
			}
		}

		// Token: 0x0400194B RID: 6475
		public WeatherPattern.Period currentCloudyPeriod = WeatherPattern.Period.invalid;

		// Token: 0x0400194C RID: 6476
		public WeatherPattern.Period currentVeryCloudyPeriod = WeatherPattern.Period.invalid;

		// Token: 0x0400194D RID: 6477
		public WeatherPattern.Period currentRainPeriod = WeatherPattern.Period.invalid;

		// Token: 0x0400194E RID: 6478
		public WeatherPattern.Period currentStormPeriod = WeatherPattern.Period.invalid;

		// Token: 0x0400194F RID: 6479
		public bool rainIsSnow;
	}
}
