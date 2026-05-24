using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x02000014 RID: 20
public class AudioController : MonoBehaviour
{
	// Token: 0x17000013 RID: 19
	// (get) Token: 0x06000060 RID: 96 RVA: 0x00007F0E File Offset: 0x0000610E
	public static AudioController instance
	{
		get
		{
			return GSR.AudioController;
		}
	}

	// Token: 0x17000014 RID: 20
	// (get) Token: 0x06000061 RID: 97 RVA: 0x00007F18 File Offset: 0x00006118
	public bool runTrackPlaying
	{
		get
		{
			RunTrack instance = MonoSingleton<RunTrack>.instance;
			return instance.state == RunTrack.State.RampingUp || instance.state == RunTrack.State.Playing || (instance.state == RunTrack.State.Scheduled && instance.signedTimeSinceMusicStart > -0.5f);
		}
	}

	// Token: 0x17000015 RID: 21
	// (get) Token: 0x06000062 RID: 98 RVA: 0x00007F57 File Offset: 0x00006157
	public bool stingSourcePlaying
	{
		get
		{
			return this.stingSource.isPlaying;
		}
	}

	// Token: 0x17000016 RID: 22
	// (get) Token: 0x06000063 RID: 99 RVA: 0x00007F64 File Offset: 0x00006164
	public bool narrativeAudioPlaying
	{
		get
		{
			return this._activeSoundEffects.Exists((AudioController.ActiveSoundEffect s) => s.source.outputAudioMixerGroup.name == "Objects");
		}
	}

	// Token: 0x17000017 RID: 23
	// (get) Token: 0x06000064 RID: 100 RVA: 0x00007F90 File Offset: 0x00006190
	public bool vocalisationPlaying
	{
		get
		{
			return this.vocalisationSource.isPlaying;
		}
	}

	// Token: 0x17000018 RID: 24
	// (get) Token: 0x06000065 RID: 101 RVA: 0x00007F9D File Offset: 0x0000619D
	public bool playingFinalJumpMusic
	{
		get
		{
			return this._wantsFinalJumpMusic;
		}
	}

	// Token: 0x17000019 RID: 25
	// (get) Token: 0x06000066 RID: 102 RVA: 0x00007FA5 File Offset: 0x000061A5
	public bool playingOtherEndingMusic
	{
		get
		{
			return this._wantsOtherEndingMusic;
		}
	}

	// Token: 0x1700001A RID: 26
	// (get) Token: 0x06000067 RID: 103 RVA: 0x00007FAD File Offset: 0x000061AD
	public bool playingFinalBlessingMusic
	{
		get
		{
			return this._wantsFinalBlessingMusic;
		}
	}

	// Token: 0x1700001B RID: 27
	// (get) Token: 0x06000068 RID: 104 RVA: 0x00007FB5 File Offset: 0x000061B5
	public bool canSyncMusicForFinalJump
	{
		get
		{
			return this.doubleSyncTrack.playing && !this.doubleSyncTrack.isCrossFading;
		}
	}

	// Token: 0x1700001C RID: 28
	// (get) Token: 0x06000069 RID: 105 RVA: 0x00007FD4 File Offset: 0x000061D4
	public bool anyMusicPlaying
	{
		get
		{
			return this.stingSourcePlaying || this.playingFinalJumpMusic || (this.musicSource.isPlaying && this.musicSource.volume > 0f);
		}
	}

	// Token: 0x1700001D RID: 29
	// (get) Token: 0x0600006A RID: 106 RVA: 0x00008009 File Offset: 0x00006209
	private AudioMixerSnapshot launchSnapshot
	{
		get
		{
			if (this._launchSnapshot == null)
			{
				this._launchSnapshot = this.audioMixer.FindSnapshot("Launch");
			}
			return this._launchSnapshot;
		}
	}

	// Token: 0x1700001E RID: 30
	// (get) Token: 0x0600006B RID: 107 RVA: 0x00008035 File Offset: 0x00006235
	private AudioMixerSnapshot defaultSnapshot
	{
		get
		{
			if (this._defaultSnapshot == null)
			{
				this._defaultSnapshot = this.audioMixer.FindSnapshot("Default");
			}
			return this._defaultSnapshot;
		}
	}

	// Token: 0x1700001F RID: 31
	// (get) Token: 0x0600006C RID: 108 RVA: 0x00008061 File Offset: 0x00006261
	private AudioMixerSnapshot caveSnapshot
	{
		get
		{
			if (this._caveSnapshot == null)
			{
				this._caveSnapshot = this.audioMixer.FindSnapshot("Cave");
			}
			return this._caveSnapshot;
		}
	}

	// Token: 0x17000020 RID: 32
	// (get) Token: 0x0600006D RID: 109 RVA: 0x0000808D File Offset: 0x0000628D
	private AudioMixerSnapshot deathSnapshot
	{
		get
		{
			if (this._deathSnapshot == null)
			{
				this._deathSnapshot = this.audioMixer.FindSnapshot("Death");
			}
			return this._deathSnapshot;
		}
	}

	// Token: 0x0600006E RID: 110 RVA: 0x000080BC File Offset: 0x000062BC
	public void Clear(bool allowEndingMusicToContinue)
	{
		if (allowEndingMusicToContinue && (this._wantsFinalJumpMusic || this._wantsOtherEndingMusic || this._wantsFinalBlessingMusic))
		{
			return;
		}
		this._wantsFinalJumpMusic = false;
		this._wantsOtherEndingMusic = false;
		this._wantsFinalBlessingMusic = false;
		this.deathAudioWantedByInk = false;
		this._inkAllowingAmbientMusic = true;
		this._clearing = true;
	}

	// Token: 0x0600006F RID: 111 RVA: 0x00008110 File Offset: 0x00006310
	public void PlaySting(Sting sting, int specificIdx = -1)
	{
		if (this.stingSource.isPlaying && (this.stingSource.time < 0.9f * this.stingSource.clip.length || sting == this._lastStingPlayed))
		{
			return;
		}
		if (this.playingFinalJumpMusic || this.playingOtherEndingMusic)
		{
			return;
		}
		if (specificIdx == -1)
		{
			int num = this.BumpCycleIndex(ref this._stingPlayCycleIndices, (int)sting, 7);
			this.stingSource.clip = this.stings.GetStingClip(sting, num);
		}
		else
		{
			this.stingSource.clip = this.stings.GetStingClip(sting, specificIdx);
		}
		this.stingSource.loop = false;
		this.stingSource.Play();
		this._lastStingPlayed = sting;
	}

	// Token: 0x06000070 RID: 112 RVA: 0x000081CB File Offset: 0x000063CB
	public bool IsPlayingSting(Sting sting)
	{
		return this._lastStingPlayed == sting && this.stingSource.isPlaying;
	}

	// Token: 0x06000071 RID: 113 RVA: 0x000081E4 File Offset: 0x000063E4
	private int BumpCycleIndex(ref int[] indices, int soundIdx, int totalSounds)
	{
		if (indices == null || indices.Length != totalSounds)
		{
			indices = new int[totalSounds];
			for (int i = 0; i < totalSounds; i++)
			{
				indices[i] = Random.Range(0, 100);
			}
		}
		indices[soundIdx]++;
		return indices[soundIdx];
	}

	// Token: 0x06000072 RID: 114 RVA: 0x0000822D File Offset: 0x0000642D
	public IEnumerator PlayNarrativeAudioWithName(string name)
	{
		ResourceRequest req = Resources.LoadAsync<AudioClip>(name);
		yield return req;
		AudioClip audioClip = req.asset as AudioClip;
		if (audioClip == null)
		{
			Debug.LogError("Could not find audio file called " + name + " in a Resources that was being played via an '>>> AUDIO' ink command");
			yield break;
		}
		AudioSource audioSource = this._narrativeSourcePrototype.Instantiate<AudioSource>(null);
		audioSource.clip = audioClip;
		audioSource.Play();
		this._activeSoundEffects.Add(new AudioController.ActiveSoundEffect
		{
			soundEffect = SoundEffect.None,
			source = audioSource
		});
		yield break;
	}

	// Token: 0x06000073 RID: 115 RVA: 0x00008243 File Offset: 0x00006443
	public void SetAmbientMusicAllowedByInk(bool allowAmbient)
	{
		this._inkAllowingAmbientMusic = allowAmbient;
	}

	// Token: 0x06000074 RID: 116 RVA: 0x0000824C File Offset: 0x0000644C
	public void PlayVocalisation(Vocalisation voc, float delay = 0f)
	{
		if (Runner.instance == null || Runner.instance.dead)
		{
			return;
		}
		if (MonoSingleton<RestStateController>.instance.resting || GameClock.instance.isWaitingForTimeToPass)
		{
			return;
		}
		if (this._activeVocalisation != Vocalisation.None && this.vocalisations.vocalisations[(int)this._activeVocalisation].priority > this.vocalisations.vocalisations[(int)voc].priority)
		{
			return;
		}
		this.vocalisationSource.clip = this.vocalisations.GetVolcalisationClip(voc);
		this.vocalisationSource.loop = false;
		if (delay == 0f)
		{
			this.vocalisationSource.Play();
		}
		else
		{
			this.vocalisationSource.PlayScheduled(AudioSettings.dspTime + (double)delay);
		}
		this._activeVocalisation = voc;
	}

	// Token: 0x06000075 RID: 117 RVA: 0x0000831C File Offset: 0x0000651C
	public void PlayUI(UISound sound)
	{
		AudioClip clip = this.uiSounds.GetClip(sound);
		if (clip != null)
		{
			this.uiSource.clip = clip;
			this.uiSource.Play();
			return;
		}
		Debug.LogWarning(string.Format("Tried to play UISound.{0} but no matching AudioClip was found in UISounds ScriptableObject file", sound));
	}

	// Token: 0x06000076 RID: 118 RVA: 0x0000836C File Offset: 0x0000656C
	public void PlaySoundEffect(SoundEffect sound)
	{
		int num = this.BumpCycleIndex(ref this._soundEffectCycleIndices, (int)sound, 8);
		AudioClip clip = this.soundEffects.GetClip(sound, num);
		if (clip != null)
		{
			AudioSource audioSource = this._soundEffectSourcePrototype.Instantiate<AudioSource>(null);
			audioSource.clip = clip;
			audioSource.Play();
			this._activeSoundEffects.Add(new AudioController.ActiveSoundEffect
			{
				soundEffect = sound,
				source = audioSource
			});
			return;
		}
		Debug.LogWarning(string.Format("Tried to play SoundEffect.{0} but no matching AudioClip was found in SoundEffects ScriptableObject file", sound));
	}

	// Token: 0x06000077 RID: 119 RVA: 0x000083F4 File Offset: 0x000065F4
	public void PlayWorldSound(SoundEffect sound, Vector3 pos, int maxOfType = -1)
	{
		int num = 0;
		if (maxOfType != -1)
		{
			using (List<AudioController.ActiveSoundEffect>.Enumerator enumerator = this._activeSoundEffects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.soundEffect == sound)
					{
						num++;
					}
				}
			}
			if (num >= maxOfType)
			{
				return;
			}
		}
		int num2 = this.BumpCycleIndex(ref this._soundEffectCycleIndices, (int)sound, 8);
		AudioClip clip = this.soundEffects.GetClip(sound, num2);
		if (clip != null)
		{
			AudioSource audioSource = this._worldEffectSourcePrototype.Instantiate<AudioSource>(null);
			audioSource.transform.position = pos;
			audioSource.clip = clip;
			audioSource.Play();
			this._activeSoundEffects.Add(new AudioController.ActiveSoundEffect
			{
				soundEffect = sound,
				source = audioSource
			});
			return;
		}
		Debug.LogWarning(string.Format("Tried to play SoundEffect.{0} but no matching AudioClip was found in SoundEffects ScriptableObject file", sound));
	}

	// Token: 0x06000078 RID: 120 RVA: 0x000084E4 File Offset: 0x000066E4
	public void StartFinalJumpMusic()
	{
		this._wantsFinalJumpMusic = true;
	}

	// Token: 0x06000079 RID: 121 RVA: 0x000084ED File Offset: 0x000066ED
	public void StartEndingMusic()
	{
		this._wantsOtherEndingMusic = true;
		this._wantsFinalBlessingMusic = false;
	}

	// Token: 0x0600007A RID: 122 RVA: 0x000084FD File Offset: 0x000066FD
	public void StartBlessingMusic()
	{
		this._wantsFinalBlessingMusic = true;
		this._wantsOtherEndingMusic = false;
	}

	// Token: 0x0600007B RID: 123 RVA: 0x00008510 File Offset: 0x00006710
	public void SyncMusicToFinalLoop()
	{
		if (!this._wantsFinalJumpMusic)
		{
			Debug.LogError("Can't call SyncMusicToFinalLoop until final music is already playing! ");
			return;
		}
		if (!this.doubleSyncTrack.playing)
		{
			Debug.LogError("Expected doubleSyncTrack to be playing but it wasn't?");
			return;
		}
		if (this._doubleSyncTrackIsInFinalLoop)
		{
			return;
		}
		int currentBarIdx = this.doubleSyncTrack.currentBarIdx;
		if (currentBarIdx < this.settings.finalJumpLoopStartBarIdx || currentBarIdx > this.settings.finalJumpLoopEndBarIdx)
		{
			if (this.doubleSyncTrack.isCrossFading)
			{
				Debug.LogError("DoubleSyncTrack couldn't sync to final loop because it was already cross fading");
				return;
			}
			this.doubleSyncTrack.MoveSyncToBarIdx(this.settings.finalJumpLoopStartBarIdx, this.settings.syncToFinalLoopCrossfadeBeats);
			this._doubleSyncTrackIsInFinalLoop = true;
		}
	}

	// Token: 0x0600007C RID: 124 RVA: 0x000085BC File Offset: 0x000067BC
	public float SyncMusicForFinalJump()
	{
		if (!this._wantsFinalJumpMusic)
		{
			Debug.LogError("Can't call SyncMusicToFinalLoop until final music is already playing! ");
			return 0f;
		}
		if (!this.doubleSyncTrack.playing)
		{
			Debug.LogError("Expected doubleSyncTrack to be playing but it wasn't?");
			return 0f;
		}
		if (this.doubleSyncTrack.isCrossFading)
		{
			Debug.LogError("DoubleSyncTrack couldn't sync for final jump because it was already cross fading");
			return 0f;
		}
		if (!this._doubleSyncTrackIsInFinalLoop)
		{
			Debug.LogError("SyncMusicForFinalJump was called, but music wasn't in the final loop, so this might not sound so good.");
		}
		this._doubleSyncTrackIsInFinalLoop = false;
		return this.doubleSyncTrack.MoveSyncToBarIdx(this.settings.finalJumpBarIdx, this.settings.syncToJumpCrossfadeBeats) + this.doubleSyncTrack.track.BarDuration(this.settings.finalJumpBarIdx);
	}

	// Token: 0x0600007D RID: 125 RVA: 0x00008674 File Offset: 0x00006874
	private void Update()
	{
		if (this._currentSnapshot == null)
		{
			this._currentSnapshot = this.launchSnapshot;
		}
		if (!Game.loaded || MonoSingleton<TitleScreen>.instance.visible || MonoSingleton<SplashSequence>.instance.visible || (Game.instance.inTitleScreenAndIntroState && !Game.instance.showingIntro))
		{
			if (this._currentSnapshot != this.launchSnapshot)
			{
				this.launchSnapshot.TransitionTo(2f);
				this._currentSnapshot = this.launchSnapshot;
			}
		}
		else if (this.deathAudioWantedByInk)
		{
			if (this._currentSnapshot != this.deathSnapshot)
			{
				this.deathSnapshot.TransitionTo(2f);
				this._currentSnapshot = this.deathSnapshot;
			}
		}
		else if (CaveRegion.inCave)
		{
			if (this._currentSnapshot != this.caveSnapshot)
			{
				this.caveSnapshot.TransitionTo(1f);
				this._currentSnapshot = this.caveSnapshot;
			}
		}
		else if (this._currentSnapshot != this.defaultSnapshot)
		{
			int num = ((this._currentSnapshot == this.launchSnapshot) ? 3 : 1);
			this._currentSnapshot = this.defaultSnapshot;
			this.defaultSnapshot.TransitionTo((float)num);
		}
		MusicType musicType = MusicType.None;
		if (Level.currentIndex == 8 && !WorldManager.instance.loading && Game.loaded && !this._wantsFinalBlessingMusic)
		{
			this._wantsOtherEndingMusic = true;
		}
		if ((Game.instance.inActiveGameplay || Game.instance.inPeakState || (Game.instance.isPathFollowing && !Game.instance.isPathFollowingBetweenLevels)) && !CaveRegion.inCave && this._inkAllowingAmbientMusic && !GameClock.instance.isNight)
		{
			musicType = MusicType.Ambient;
		}
		if (Game.instance.inTitleScreenAndIntroState)
		{
			musicType = MusicType.TitleScreen;
			if (!this.musicSource.isPlaying && this._wantsOtherEndingMusic)
			{
				this._wantsOtherEndingMusic = false;
			}
			if (!this.musicSource.isPlaying && this._wantsFinalBlessingMusic)
			{
				this._wantsFinalBlessingMusic = false;
			}
			if (this.doubleSyncTrack.complete && this._wantsFinalJumpMusic)
			{
				this._wantsFinalJumpMusic = false;
			}
		}
		bool flag = this._playingMusicType == MusicType.Intro && Game.instance.startedFreshNewGame && this.musicSource.isPlaying;
		if (Game.instance.showingIntro || flag)
		{
			this._wantsFinalJumpMusic = false;
			this._wantsOtherEndingMusic = false;
			this._wantsFinalBlessingMusic = false;
			if (Game.instance.showingIntro && !Game.instance.startedFreshNewGame)
			{
				musicType = MusicType.None;
			}
			else
			{
				musicType = MusicType.Intro;
			}
		}
		if (this._wantsOtherEndingMusic)
		{
			musicType = MusicType.OtherEnding;
		}
		else if (this._wantsFinalBlessingMusic)
		{
			musicType = MusicType.Fantasia;
		}
		if (this.runTrackPlaying || this.stingSourcePlaying || MonoSingleton<LatencyCalibrator>.instance == null || MonoSingleton<LatencyCalibrator>.instance.visible || SplashSequence.preventTitleMusic)
		{
			musicType = MusicType.None;
		}
		if (this._wantsFinalJumpMusic)
		{
			musicType = MusicType.FinalJumpSyncTrack;
		}
		if (this._clearing)
		{
			musicType = MusicType.None;
		}
		float num2 = 1f;
		if ((musicType != this._playingMusicType && this._playingMusicType != MusicType.None) || this._clearing)
		{
			num2 = 0f;
		}
		else if (musicType == MusicType.Ambient)
		{
			num2 = this.settings.ambientMusicVolume;
		}
		else if (musicType == MusicType.FinalJumpSyncTrack && CaveRegion.inCave)
		{
			num2 = 0f;
		}
		else if (musicType == MusicType.OtherEnding && CaveRegion.inCave)
		{
			num2 = 0f;
		}
		else if (musicType == MusicType.Fantasia && CaveRegion.inCave)
		{
			num2 = 0f;
		}
		float num3 = this.settings.standardMusicRampTime;
		if (musicType == MusicType.Ambient)
		{
			if (num2 > 0f)
			{
				num3 = this.settings.ambientRampUpTime;
			}
			else
			{
				num3 = this.settings.ambientRampDownTime;
			}
		}
		else if (this._playingMusicType == MusicType.Intro && num2 == 0f)
		{
			num3 = this.settings.introRampDownTime;
		}
		else if (musicType == MusicType.None && this.stingSourcePlaying)
		{
			num3 = this.settings.quickRampDownTimeForSting;
		}
		if (!this.musicSource.isPlaying && musicType != MusicType.None && musicType != MusicType.FinalJumpSyncTrack && num2 > 0f)
		{
			switch (musicType)
			{
			case MusicType.Ambient:
				if (this._currentAmbientClip != null)
				{
					this.musicSource.clip = this._currentAmbientClip;
					this.musicSource.time = this._currentAmbientClipTime;
				}
				else
				{
					this.musicSource.clip = this.settings.ambientMusicClips.Random<AudioClip>();
					this.musicSource.time = 0f;
					num3 = 0f;
				}
				break;
			case MusicType.TitleScreen:
				this.musicSource.clip = this.settings.titleScreenMusic;
				num3 = 0f;
				break;
			case MusicType.Intro:
				this.musicSource.clip = this.settings.introMusic;
				num3 = 0f;
				break;
			case MusicType.OtherEnding:
				this.musicSource.clip = this.settings.otherEndingMusic;
				num3 = 0f;
				break;
			case MusicType.Fantasia:
				this.musicSource.clip = this.settings.fantasiaMusic;
				num3 = 0f;
				break;
			}
			this.musicSource.Play();
			this._playingMusicType = musicType;
		}
		if (num3 >= 0f && this.musicSource.clip != null && this.musicSource.isPlaying)
		{
			if (num3 == 0f)
			{
				this.musicSource.volume = num2;
			}
			else
			{
				this.musicSource.volume = Mathf.MoveTowards(this.musicSource.volume, num2, Time.unscaledDeltaTime / num3);
			}
			if (this.musicSource.volume == 0f && num2 == 0f)
			{
				this._playingMusicType = MusicType.None;
				this.musicSource.Pause();
			}
		}
		else if (!this.musicSource.loop && !this.musicSource.isPlaying && this._playingMusicType != MusicType.None)
		{
			this._playingMusicType = MusicType.None;
			this.musicSource.volume = 1f;
		}
		if (this._clearing && (this.musicSource.clip == null || !this.musicSource.isPlaying || this.musicSource.volume == 0f))
		{
			this._clearing = false;
		}
		if (musicType == MusicType.Ambient && this._playingMusicType == MusicType.Ambient && num2 > 0f)
		{
			this._currentAmbientClip = this.musicSource.clip;
			this._currentAmbientClipTime = this.musicSource.time;
		}
		if (this._playingMusicType != MusicType.Ambient && (Game.instance.isPathFollowingBetweenLevels || GameClock.instance.isNight || (this._currentAmbientClip != null && this._currentAmbientClipTime > this._currentAmbientClip.length - 20f)))
		{
			this._currentAmbientClip = null;
			this._currentAmbientClipTime = 0f;
		}
		if (this._activeVocalisation != Vocalisation.None && !this.vocalisationPlaying)
		{
			this._activeVocalisation = Vocalisation.None;
		}
		if (musicType == MusicType.FinalJumpSyncTrack && !this.musicSource.isPlaying && !this.doubleSyncTrack.playing)
		{
			this.doubleSyncTrack.volume = 1f;
			this.doubleSyncTrack.Play(0);
			this._doubleSyncTrackIsInFinalLoop = false;
		}
		else if (musicType != MusicType.FinalJumpSyncTrack && this.doubleSyncTrack.playing)
		{
			this.doubleSyncTrack.volume = Mathf.MoveTowards(this.doubleSyncTrack.volume, 0f, Time.unscaledDeltaTime / num3);
			if (this.doubleSyncTrack.volume == 0f)
			{
				this.doubleSyncTrack.Stop();
			}
		}
		if (this.doubleSyncTrack.playing && this._doubleSyncTrackIsInFinalLoop)
		{
			if (this.doubleSyncTrack.currentBarIdx == this.settings.finalJumpLoopEndBarIdx && !this.doubleSyncTrack.isCrossFading)
			{
				this.doubleSyncTrack.MoveSyncToBarIdx(this.settings.finalJumpLoopStartBarIdx, this.settings.syncResetLoopCrossfadeBeats);
			}
		}
		else if (this.doubleSyncTrack.playing && !this._doubleSyncTrackIsInFinalLoop && this.doubleSyncTrack.currentBarIdx >= this.settings.finalJumpLoopStartBarIdx && this.doubleSyncTrack.currentBarIdx < this.settings.finalJumpLoopEndBarIdx)
		{
			this._doubleSyncTrackIsInFinalLoop = true;
		}
		else if (musicType == MusicType.FinalJumpSyncTrack && this.doubleSyncTrack.playing && !this._doubleSyncTrackIsInFinalLoop)
		{
			if (CaveRegion.inCave)
			{
				this.doubleSyncTrack.volume = Mathf.MoveTowards(this.doubleSyncTrack.volume, 0f, Time.unscaledDeltaTime / num3);
			}
			else
			{
				this.doubleSyncTrack.volume = Mathf.MoveTowards(this.doubleSyncTrack.volume, 1f, Time.unscaledDeltaTime / num3);
			}
		}
		this._activeSoundEffects.UpdateAndRemoveIf(delegate(AudioController.ActiveSoundEffect sfx)
		{
			if (!sfx.source.isPlaying)
			{
				sfx.source.clip = null;
				sfx.source.GetComponent<Prototype>().ReturnToPool();
				return true;
			}
			return false;
		});
	}

	// Token: 0x0600007E RID: 126 RVA: 0x00008F18 File Offset: 0x00007118
	private bool FadeOutAndStop(AudioSource source)
	{
		if (!source.isPlaying)
		{
			return true;
		}
		source.volume = Mathf.MoveTowards(source.volume, 0f, Time.unscaledDeltaTime);
		if (source.volume == 0f)
		{
			source.Stop();
			source.volume = 1f;
			return true;
		}
		return false;
	}

	// Token: 0x04000084 RID: 132
	public AudioMixer audioMixer;

	// Token: 0x04000085 RID: 133
	public AudioSource musicSource;

	// Token: 0x04000086 RID: 134
	public AudioSource stingSource;

	// Token: 0x04000087 RID: 135
	public AudioSource vocalisationSource;

	// Token: 0x04000088 RID: 136
	public AudioSource uiSource;

	// Token: 0x04000089 RID: 137
	public MusicSettings settings;

	// Token: 0x0400008A RID: 138
	public Vocalisations vocalisations;

	// Token: 0x0400008B RID: 139
	public UISounds uiSounds;

	// Token: 0x0400008C RID: 140
	public SoundEffects soundEffects;

	// Token: 0x0400008D RID: 141
	public Stings stings;

	// Token: 0x0400008E RID: 142
	public DoubleSyncTrack doubleSyncTrack;

	// Token: 0x0400008F RID: 143
	public bool deathAudioWantedByInk;

	// Token: 0x04000090 RID: 144
	private AudioMixerSnapshot _launchSnapshot;

	// Token: 0x04000091 RID: 145
	private AudioMixerSnapshot _defaultSnapshot;

	// Token: 0x04000092 RID: 146
	private AudioMixerSnapshot _caveSnapshot;

	// Token: 0x04000093 RID: 147
	private AudioMixerSnapshot _deathSnapshot;

	// Token: 0x04000094 RID: 148
	private Vocalisation _activeVocalisation;

	// Token: 0x04000095 RID: 149
	private Sting _lastStingPlayed;

	// Token: 0x04000096 RID: 150
	private MusicType _playingMusicType;

	// Token: 0x04000097 RID: 151
	private AudioClip _currentAmbientClip;

	// Token: 0x04000098 RID: 152
	private float _currentAmbientClipTime;

	// Token: 0x04000099 RID: 153
	private bool _doubleSyncTrackIsInFinalLoop;

	// Token: 0x0400009A RID: 154
	private bool _wantsFinalJumpMusic;

	// Token: 0x0400009B RID: 155
	private bool _wantsOtherEndingMusic;

	// Token: 0x0400009C RID: 156
	private bool _wantsFinalBlessingMusic;

	// Token: 0x0400009D RID: 157
	private bool _clearing;

	// Token: 0x0400009E RID: 158
	private bool _inkAllowingAmbientMusic = true;

	// Token: 0x0400009F RID: 159
	private int[] _stingPlayCycleIndices;

	// Token: 0x040000A0 RID: 160
	private int[] _soundEffectCycleIndices;

	// Token: 0x040000A1 RID: 161
	private AudioMixerSnapshot _currentSnapshot;

	// Token: 0x040000A2 RID: 162
	private List<AudioController.ActiveSoundEffect> _activeSoundEffects = new List<AudioController.ActiveSoundEffect>();

	// Token: 0x040000A3 RID: 163
	[SerializeField]
	private Prototype _soundEffectSourcePrototype;

	// Token: 0x040000A4 RID: 164
	[SerializeField]
	private Prototype _worldEffectSourcePrototype;

	// Token: 0x040000A5 RID: 165
	[SerializeField]
	private Prototype _narrativeSourcePrototype;

	// Token: 0x0200024A RID: 586
	private struct ActiveSoundEffect
	{
		// Token: 0x040013CF RID: 5071
		public SoundEffect soundEffect;

		// Token: 0x040013D0 RID: 5072
		public AudioSource source;
	}
}
