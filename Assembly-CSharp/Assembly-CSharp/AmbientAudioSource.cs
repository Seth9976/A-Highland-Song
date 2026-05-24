using System;
using UnityEngine;

// Token: 0x02000008 RID: 8
[RequireComponent(typeof(AudioSource))]
public class AmbientAudioSource : MonoBehaviour
{
	// Token: 0x17000008 RID: 8
	// (get) Token: 0x0600002B RID: 43 RVA: 0x00005172 File Offset: 0x00003372
	private AudioSource audioSource
	{
		get
		{
			return base.GetComponent<AudioSource>();
		}
	}

	// Token: 0x17000009 RID: 9
	// (get) Token: 0x0600002C RID: 44 RVA: 0x0000517A File Offset: 0x0000337A
	public Region region
	{
		get
		{
			return base.GetComponent<Region>();
		}
	}

	// Token: 0x1700000A RID: 10
	// (get) Token: 0x0600002D RID: 45 RVA: 0x00005184 File Offset: 0x00003384
	private Bounds bounds
	{
		get
		{
			if (!this.hasBounds)
			{
				int num = Level.DepthToIndex(base.transform.position.z);
				return new Bounds(new Vector3(0f, 0f, Level.IndexToDepth(num)), new Vector3(100000f, 100000f, 400f));
			}
			return new Bounds(base.transform.position, this.boundsSize);
		}
	}

	// Token: 0x0600002E RID: 46 RVA: 0x000051F4 File Offset: 0x000033F4
	private void OnEnable()
	{
		this.audioSource.volume = 0f;
		this.Refresh();
		if (this.audioSource.loop && this.audioSource.clip != null)
		{
			this.audioSource.time = Random.Range(0f, this.audioSource.clip.length);
		}
	}

	// Token: 0x0600002F RID: 47 RVA: 0x0000525C File Offset: 0x0000345C
	private void Update()
	{
		this.Refresh();
	}

	// Token: 0x06000030 RID: 48 RVA: 0x00005264 File Offset: 0x00003464
	private void Refresh()
	{
		if (!Game.loaded)
		{
			return;
		}
		bool flag = WeatherSystem.IsWeatherValid(WeatherSystem.instance.currentWeather, this.validWeatherTypes);
		bool flag2 = this.validTimesOfDay.IsSetAtTime(GameClock.instance.hourOfDay);
		bool flag3 = this.bounds.Contains(GameCamera.instance.cameraProperties.targetPoint);
		float num = (float)((flag && flag2 && flag3) ? 1 : 0);
		int num2 = (Runner.instance.isMusicRunning ? 0 : 1);
		this.musicRunVolumeModifier = Mathf.MoveTowards(this.musicRunVolumeModifier, (float)num2, Time.deltaTime * 0.4f);
		float num3 = num * this.musicRunVolumeModifier * this.masterVolume;
		float num4 = this.volumeSmoothTime;
		if (this.soundsBadWithMusic && AudioController.instance.anyMusicPlaying)
		{
			num3 = 0f;
			num4 = 0.4f;
		}
		if (num4 <= 0f)
		{
			this.audioSource.volume = num3;
		}
		else
		{
			this.audioSource.volume = Mathf.MoveTowards(this.audioSource.volume, num3, Time.unscaledDeltaTime / num4);
		}
		if (this.audioSource.volume > 0f && !this.audioSource.isPlaying)
		{
			this.audioSource.Play();
			return;
		}
		if (this.audioSource.volume == 0f && this.audioSource.isPlaying)
		{
			this.audioSource.Stop();
		}
	}

	// Token: 0x06000031 RID: 49 RVA: 0x000053C8 File Offset: 0x000035C8
	private void OnDrawGizmosSelected()
	{
		if (this.hasBounds)
		{
			Gizmos.color = Color.white;
			Gizmos.DrawWireCube(this.bounds.center, this.bounds.size);
		}
	}

	// Token: 0x04000037 RID: 55
	[Range(0f, 1f)]
	public float masterVolume = 1f;

	// Token: 0x04000038 RID: 56
	public float volumeSmoothTime = 4f;

	// Token: 0x04000039 RID: 57
	[EnumFlagsButtonGroup]
	public WeatherType validWeatherTypes = WeatherType.AllAny;

	// Token: 0x0400003A RID: 58
	public TimeOfDayPicker validTimesOfDay = TimeOfDayPicker.all;

	// Token: 0x0400003B RID: 59
	public float musicRunVolumeModifier = 0.3f;

	// Token: 0x0400003C RID: 60
	public bool hasBounds = true;

	// Token: 0x0400003D RID: 61
	public Vector3 boundsSize = new Vector3(30f, 30f, 30f);

	// Token: 0x0400003E RID: 62
	public bool soundsBadWithMusic;
}
