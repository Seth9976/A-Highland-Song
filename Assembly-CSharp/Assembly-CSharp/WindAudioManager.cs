using System;
using UnityEngine;

// Token: 0x02000164 RID: 356
public class WindAudioManager : MonoBehaviour
{
	// Token: 0x06000C02 RID: 3074 RVA: 0x00060134 File Offset: 0x0005E334
	private void Awake()
	{
		WindAudioManager.WindAudioSource[] array = this.windAudio;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].audioSource.volume = 0f;
		}
	}

	// Token: 0x06000C03 RID: 3075 RVA: 0x00060168 File Offset: 0x0005E368
	private void Update()
	{
		if (AudioController.instance == null || WeatherSystem.instance == null)
		{
			return;
		}
		float num = ((AudioController.instance.runTrackPlaying || AudioController.instance.stingSourcePlaying || AudioController.instance.narrativeAudioPlaying) ? 0.25f : 1f);
		if (!Game.loaded)
		{
			num = 0f;
		}
		else
		{
			WeatherModifierZone currentWeatherModifierZone = Runner.instance.health.currentWeatherModifierZone;
			if (currentWeatherModifierZone != null && (currentWeatherModifierZone.protectionFromWeather != ShelterProtectionStrength.None || currentWeatherModifierZone.protectionFromWind != ShelterProtectionStrength.None))
			{
				num *= 0.2f;
			}
		}
		float num2 = Mathf.Abs(WeatherSystem.instance.windSystem.windVelocity);
		WindAudioManager.WindAudioSource[] array = this.windAudio;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Update(num2, num);
		}
	}

	// Token: 0x04000E40 RID: 3648
	public WindAudioManager.WindAudioSource[] windAudio;

	// Token: 0x02000397 RID: 919
	[Serializable]
	public class WindAudioSource
	{
		// Token: 0x0600180E RID: 6158 RVA: 0x0009DCCC File Offset: 0x0009BECC
		public void Update(float windSpeed, float targetVolumeModifier)
		{
			if (this.isCaveAmbience)
			{
				this.windSpeedVolume = 1f;
			}
			else
			{
				this.windSpeedVolume = WindAudioManager.WindAudioSource.DoubleInverseLerp(this.silentRange.min, this.fullVolumeRange.min, this.fullVolumeRange.max, this.silentRange.max, windSpeed);
			}
			float num = CaveRegion.inCaveNorm;
			if (!this.isCaveAmbience)
			{
				num = 1f - num;
			}
			float num2 = this.windSpeedVolume * targetVolumeModifier * this.masterVolume * num;
			float num3 = 1f;
			if (Game.instance.inTitleScreenAndIntroState)
			{
				num3 = 5f;
			}
			this.audioSource.volume = Mathf.MoveTowards(this.audioSource.volume, num2, Time.unscaledDeltaTime / num3);
		}

		// Token: 0x0600180F RID: 6159 RVA: 0x0009DD89 File Offset: 0x0009BF89
		private static float DoubleInverseLerp(float a, float b, float c, float d, float value)
		{
			if (value >= d)
			{
				return 0f;
			}
			if (value >= c)
			{
				return Mathf.InverseLerp(d, c, value);
			}
			if (value >= b)
			{
				return 1f;
			}
			if (value >= a)
			{
				return Mathf.InverseLerp(a, b, value);
			}
			return 0f;
		}

		// Token: 0x0400195D RID: 6493
		public float masterVolume = 1f;

		// Token: 0x0400195E RID: 6494
		public Range silentRange;

		// Token: 0x0400195F RID: 6495
		public Range fullVolumeRange;

		// Token: 0x04001960 RID: 6496
		public bool isCaveAmbience;

		// Token: 0x04001961 RID: 6497
		public AudioSource audioSource;

		// Token: 0x04001962 RID: 6498
		[Disable]
		public float windSpeedVolume = 1f;
	}
}
