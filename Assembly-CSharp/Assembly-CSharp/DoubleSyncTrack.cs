using System;
using UnityEngine;

// Token: 0x0200000B RID: 11
public class DoubleSyncTrack : MonoBehaviour
{
	// Token: 0x1700000C RID: 12
	// (get) Token: 0x06000038 RID: 56 RVA: 0x000055A7 File Offset: 0x000037A7
	public bool playing
	{
		get
		{
			return this._playing;
		}
	}

	// Token: 0x1700000D RID: 13
	// (get) Token: 0x06000039 RID: 57 RVA: 0x000055AF File Offset: 0x000037AF
	public bool complete
	{
		get
		{
			return this._playing && !this.activeSource.isPlaying;
		}
	}

	// Token: 0x1700000E RID: 14
	// (get) Token: 0x0600003A RID: 58 RVA: 0x000055C9 File Offset: 0x000037C9
	public float timeRemaining
	{
		get
		{
			if (!this.playing)
			{
				return 0f;
			}
			if (this.complete)
			{
				return 0f;
			}
			return this.activeSource.clip.length - this.activeSource.time;
		}
	}

	// Token: 0x1700000F RID: 15
	// (get) Token: 0x0600003B RID: 59 RVA: 0x00005603 File Offset: 0x00003803
	public int currentBarIdx
	{
		get
		{
			if (!this.playing)
			{
				return 0;
			}
			return this.track.BarIndexAtTime(this.activeSource.time);
		}
	}

	// Token: 0x17000010 RID: 16
	// (get) Token: 0x0600003C RID: 60 RVA: 0x00005625 File Offset: 0x00003825
	public bool isCrossFading
	{
		get
		{
			return this._isCrossFading;
		}
	}

	// Token: 0x17000011 RID: 17
	// (get) Token: 0x0600003D RID: 61 RVA: 0x0000562D File Offset: 0x0000382D
	private AudioSource activeSource
	{
		get
		{
			if (!this.playing)
			{
				return this._source1;
			}
			if (this._isCrossFading)
			{
				return this._crossfadeCurrentSource;
			}
			if (!this._source1.isPlaying)
			{
				return this._source2;
			}
			return this._source1;
		}
	}

	// Token: 0x0600003E RID: 62 RVA: 0x00005667 File Offset: 0x00003867
	private void Awake()
	{
		this._source1.clip = this.track.clip;
		this._source2.clip = this.track.clip;
	}

	// Token: 0x0600003F RID: 63 RVA: 0x00005698 File Offset: 0x00003898
	public void Play(int fromBeatIdx = 0)
	{
		if (this.playing)
		{
			return;
		}
		if (fromBeatIdx == 0)
		{
			this._source1.time = 0f;
		}
		else
		{
			this._source1.time = this.track.beats[fromBeatIdx].time;
		}
		this._source1.Play();
		this._source1.volume = this.volume;
		this._playing = true;
	}

	// Token: 0x06000040 RID: 64 RVA: 0x00005708 File Offset: 0x00003908
	public void Stop()
	{
		this._source1.Stop();
		this._source2.Stop();
		this._source1.volume = this.volume;
		this._source2.volume = this.volume;
		this._crossfadeCurrentSource = null;
		this._crossfadeNextSource = null;
		this._isCrossFading = false;
		this._playing = false;
	}

	// Token: 0x06000041 RID: 65 RVA: 0x00005769 File Offset: 0x00003969
	public float MoveSyncToBarIdx(int barIdx, int crossfadeBeats = 2)
	{
		return this.MoveSyncToBeatIdx(this.track.bars[barIdx].firstBeatIdx, crossfadeBeats);
	}

	// Token: 0x06000042 RID: 66 RVA: 0x00005788 File Offset: 0x00003988
	public float MoveSyncToBeatIdx(int beatIdx, int crossfadeBeats = 2)
	{
		if (!this.playing)
		{
			this._source1.time = this.track.beats[beatIdx].time;
			this.Play(0);
			return 0f;
		}
		if (this._isCrossFading)
		{
			Debug.LogError("DoubleSyncTrack couldn't start a crossfade because a crossfade was already active");
			return 0f;
		}
		this._crossfadeCurrentSource = this.activeSource;
		this._crossfadeNextSource = ((this._crossfadeCurrentSource == this._source1) ? this._source2 : this._source1);
		int num = this.track.BeatIndexAtTime(this._crossfadeCurrentSource.time) + 1;
		int num2 = this.track.BarIndexAtTime(this._crossfadeCurrentSource.time) + 1;
		if (this.track.bars[num2].firstBeatIdx < num + crossfadeBeats)
		{
			num2++;
		}
		int firstBeatIdx = this.track.bars[num2].firstBeatIdx;
		this._currentFadeOut = new DoubleSyncTrack.FadeTimes
		{
			start = this.track.beats[firstBeatIdx - crossfadeBeats].time,
			end = this.track.beats[firstBeatIdx].time
		};
		this._nextFadeIn = new DoubleSyncTrack.FadeTimes
		{
			start = this.track.beats[beatIdx - crossfadeBeats].time,
			end = this.track.beats[beatIdx].time
		};
		float num3 = this._currentFadeOut.end - this._crossfadeCurrentSource.time;
		float num4 = num3 - (this._nextFadeIn.end - this._nextFadeIn.start);
		this._crossfadeNextSource.time = this._nextFadeIn.start;
		this._crossfadeNextSource.PlayScheduled(AudioSettings.dspTime + (double)num4);
		this._crossfadeNextSource.volume = 0f;
		this._isCrossFading = true;
		return num3;
	}

	// Token: 0x06000043 RID: 67 RVA: 0x00005987 File Offset: 0x00003B87
	private void SetCrossfadeVolume(AudioSource source, float vol)
	{
		source.volume = this.volume * Mathf.Pow(vol, 0.5f);
	}

	// Token: 0x06000044 RID: 68 RVA: 0x000059A4 File Offset: 0x00003BA4
	private void Update()
	{
		if (this._isCrossFading)
		{
			this.SetCrossfadeVolume(this._crossfadeCurrentSource, 1f - Mathf.InverseLerp(this._currentFadeOut.start, this._currentFadeOut.end, this._crossfadeCurrentSource.time));
			if (this._crossfadeNextSource.isPlaying)
			{
				this.SetCrossfadeVolume(this._crossfadeNextSource, Mathf.InverseLerp(this._nextFadeIn.start, this._nextFadeIn.end, this._crossfadeNextSource.time));
			}
			if (!this.playing || (this._crossfadeCurrentSource.time > this._currentFadeOut.end && this._crossfadeNextSource.time > this._nextFadeIn.end))
			{
				this._crossfadeCurrentSource.Stop();
				this._crossfadeCurrentSource = null;
				this._crossfadeNextSource = null;
				this._isCrossFading = false;
				return;
			}
		}
		else if (this.playing)
		{
			this.activeSource.volume = this.volume;
		}
	}

	// Token: 0x04000043 RID: 67
	public BeatTrack track;

	// Token: 0x04000044 RID: 68
	public int crossfadeBeats = 2;

	// Token: 0x04000045 RID: 69
	public float volume = 1f;

	// Token: 0x04000046 RID: 70
	private DoubleSyncTrack.FadeTimes _currentFadeOut;

	// Token: 0x04000047 RID: 71
	private DoubleSyncTrack.FadeTimes _nextFadeIn;

	// Token: 0x04000048 RID: 72
	private AudioSource _crossfadeCurrentSource;

	// Token: 0x04000049 RID: 73
	private AudioSource _crossfadeNextSource;

	// Token: 0x0400004A RID: 74
	private bool _isCrossFading;

	// Token: 0x0400004B RID: 75
	private bool _playing;

	// Token: 0x0400004C RID: 76
	[SerializeField]
	private AudioSource _source1;

	// Token: 0x0400004D RID: 77
	[SerializeField]
	private AudioSource _source2;

	// Token: 0x02000244 RID: 580
	private struct FadeTimes
	{
		// Token: 0x040013BD RID: 5053
		public float start;

		// Token: 0x040013BE RID: 5054
		public float end;
	}
}
