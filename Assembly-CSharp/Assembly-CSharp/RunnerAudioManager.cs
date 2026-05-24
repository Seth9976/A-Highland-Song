using System;
using UnityEngine;

// Token: 0x020000C5 RID: 197
public class RunnerAudioManager : MonoBehaviour
{
	// Token: 0x17000183 RID: 387
	// (get) Token: 0x06000620 RID: 1568 RVA: 0x00030E84 File Offset: 0x0002F084
	private Runner runner
	{
		get
		{
			return Runner.instance;
		}
	}

	// Token: 0x06000621 RID: 1569 RVA: 0x00030E8C File Offset: 0x0002F08C
	private void OnEnable()
	{
		Runner.onJumpStart = (Action)Delegate.Combine(Runner.onJumpStart, new Action(this.OnJumpStart));
		Runner.onJumpEnd = (Action)Delegate.Combine(Runner.onJumpEnd, new Action(this.OnJumpEnd));
	}

	// Token: 0x06000622 RID: 1570 RVA: 0x00030EDC File Offset: 0x0002F0DC
	private void OnDisable()
	{
		Runner.onJumpStart = (Action)Delegate.Remove(Runner.onJumpStart, new Action(this.OnJumpStart));
		Runner.onJumpEnd = (Action)Delegate.Remove(Runner.onJumpEnd, new Action(this.OnJumpEnd));
	}

	// Token: 0x06000623 RID: 1571 RVA: 0x00030F2C File Offset: 0x0002F12C
	private void OnJumpStart()
	{
		if (this.runner.isMusicRunning)
		{
			return;
		}
		if (this.nextBreathTime < 0.4f)
		{
			this.PlayBreathSound(true);
		}
		this.jumpState = RunnerAudioManager.JumpState.Jumping;
		float num = this.jumpSFXSettings.backpackShuffleJumpVolume;
		if (this.runner.isMusicRunning)
		{
			num *= this.jumpSFXSettings.runningGameplayVolumeScale;
		}
		AudioClip audioClip = this.jumpSFXSettings.backpackShuffleJumpClips.Random<AudioClip>();
		if (audioClip != null)
		{
			this.jumpSource.PlayOneShot(audioClip, num);
		}
	}

	// Token: 0x06000624 RID: 1572 RVA: 0x00030FB0 File Offset: 0x0002F1B0
	private void OnJumpEnd()
	{
		this.jumpState = RunnerAudioManager.JumpState.Idle;
	}

	// Token: 0x06000625 RID: 1573 RVA: 0x00030FBC File Offset: 0x0002F1BC
	private void Update()
	{
		if (this.runner.dead)
		{
			this.scramblingRockfallSource.volume = 0f;
			return;
		}
		if (this.jumpState == RunnerAudioManager.JumpState.Jumping && (this.runner.jump.targetSlope != null || this.runner.jump.targetBalancePoint != null))
		{
			float num = this.runner.jump.expectedDuration - this.runner.stateTimer;
			if (num < 0.3f)
			{
				MusicGameAudioClip musicGameAudioClip = this.jumpSFXSettings.backpackShuffleLandClips.Random<MusicGameAudioClip>();
				float num2 = num - musicGameAudioClip.clipPeakTime;
				if (num2 < 0f)
				{
					Debug.LogWarning("Jump land clip plays in the past! Skipping this jump. You should raise the maxTimeTillLandToPlay variable.");
				}
				else
				{
					float num3 = this.jumpSFXSettings.backpackShuffleJumpVolume;
					if (this.runner.isMusicRunning)
					{
						num3 *= this.jumpSFXSettings.runningGameplayVolumeScale;
					}
					this.jumpSource.clip = musicGameAudioClip.audioClip;
					this.jumpSource.volume = num3;
					this.jumpSource.PlayDelayed(num2);
				}
				this.jumpState = RunnerAudioManager.JumpState.HandledJump;
			}
		}
		if (!this.runner.isMusicRunning)
		{
			if (this.runner.climbing)
			{
				float num4 = Mathf.Abs(45f - Mathf.Abs(this.runner.climbingAngle));
				float num5 = Mathf.InverseLerp(35f, 10f, num4);
				num5 = Mathf.Clamp01(Mathf.Pow(num5, 1.6f) * 1.25f - 0.25f);
				int num6 = (Game.gameplayPaused ? 0 : 1);
				this.scramblingRockfallVolume = Mathf.MoveTowards(this.scramblingRockfallVolume, Mathf.Abs(this.runner.climbSpeed) * 0.7f * num5 * (float)num6, Time.unscaledDeltaTime * 1.5f);
			}
			else if (this.runner.sliding && !Game.gameplayPaused)
			{
				this.scramblingRockfallVolume = Mathf.MoveTowards(this.scramblingRockfallVolume, 1f, Time.unscaledDeltaTime * 3f);
			}
			else
			{
				this.scramblingRockfallVolume = Mathf.MoveTowards(this.scramblingRockfallVolume, 0f, Time.unscaledDeltaTime * 0.6f);
			}
			this.scramblingRockfallSource.volume = this.scramblingRockfallVolume;
			if (this.runner.catchingBreath)
			{
				this.exhaustion = Mathf.MoveTowards(this.exhaustion, this.breathingSFXSettings.targetExhaustionOverStaminaRecovering.Evaluate(this.runner.stamina), this.breathingSFXSettings.exhaustionDeltaOverStaminaRecovering.Evaluate(this.runner.stamina) * Time.deltaTime);
			}
			else if (this.runner.running && this.runner.speed > 0f)
			{
				this.exhaustion = Mathf.MoveTowards(this.exhaustion, this.breathingSFXSettings.targetExhaustionOverStaminaRunning.Evaluate(this.runner.stamina), this.breathingSFXSettings.exhaustionDeltaOverStaminaRunning.Evaluate(this.runner.stamina) * Time.deltaTime);
			}
			else if (this.runner.climbing)
			{
				this.exhaustion = Mathf.MoveTowards(this.exhaustion, this.breathingSFXSettings.targetExhaustionOverStaminaClimbing.Evaluate(this.runner.stamina), this.breathingSFXSettings.exhaustionDeltaOverStaminaClimbing.Evaluate(this.runner.stamina) * Time.deltaTime);
			}
			else
			{
				this.exhaustion = Mathf.MoveTowards(this.exhaustion, this.breathingSFXSettings.targetExhaustionOverStaminaIdle.Evaluate(this.runner.stamina), this.breathingSFXSettings.exhaustionDeltaOverStaminaIdle.Evaluate(this.runner.stamina) * Time.deltaTime);
			}
			this.exhaustion = Mathf.Clamp01(this.exhaustion);
			if (Time.time > this.nextBreathTime)
			{
				this.PlayBreathSound(false);
				return;
			}
		}
		else if (!MonoSingleton<RunTrack>.instance.paused)
		{
			RunTrack runTrack = this.runner.runTrack;
			BeatTrack track = runTrack.track;
			float num7 = 0.4f * runTrack.currentBeatDuration;
			int num8 = track.BeatIndexAtTime(runTrack.currentMusicTime + num7);
			if (this.musicRunningNextBeatIndex != num8)
			{
				this.musicRunningNextBeatIndex = num8;
				float num9 = track.beats[this.musicRunningNextBeatIndex].time - runTrack.currentMusicTime;
				int num10 = runTrack.track.BeatIndexInBar(this.musicRunningNextBeatIndex);
				bool flag = runTrack.track.HasObstacleAt(this.musicRunningNextBeatIndex);
				if (flag)
				{
					AudioSource audioSource = this.audioSourcePrototype.Instantiate<AudioSource>(null);
					audioSource.clip = this.jumpSFXSettings.runningLandRewardNoteClips.Random<AudioClip>();
					audioSource.volume = this.jumpSFXSettings.musicBeatMarkerVolume;
					audioSource.PlayScheduled(AudioSettings.dspTime + (double)num9 - (double)LatencyCalibrator.latency);
					audioSource.GetComponent<Prototype>().ReturnToPool(num9 + audioSource.clip.length + 1f);
					audioSource.gameObject.name = "Music Running Marker";
				}
				if (num10 == 0 && flag)
				{
					float num11 = num9 - LatencyCalibrator.latency - 0.2f;
					AudioController.instance.PlayVocalisation(Vocalisation.JumpHuh, num11);
					this.musicRunningNextBreathVolumeMultiplier = 1.25f;
					return;
				}
				if (num10 % 2 == 1)
				{
					MusicGameAudioClip breathingClip = this.GetBreathingClip(this.exhaustion);
					float num12 = this.breathingSFXSettings.runningGameplayVolume * this.musicRunningNextBreathVolumeMultiplier;
					if (breathingClip != null && num12 > 0f)
					{
						AudioSource audioSource2 = this.audioSourcePrototype.Instantiate<AudioSource>(null);
						audioSource2.clip = breathingClip.audioClip;
						audioSource2.volume = num12;
						audioSource2.PlayScheduled(AudioSettings.dspTime + (double)num9 - (double)LatencyCalibrator.latency - (double)breathingClip.clipPeakTime);
						audioSource2.GetComponent<Prototype>().ReturnToPool(num9 - breathingClip.clipPeakTime + audioSource2.clip.length + 1f);
						audioSource2.gameObject.name = "Music Running Breath";
						this.nextBreathTime = Time.time + 1.5f * this.breathingSFXSettings.breathTimeOverExhaustion.Evaluate(this.exhaustion);
						if (this.onBreatheOut != null)
						{
							this.onBreatheOut(this.exhaustion);
						}
					}
					this.musicRunningNextBreathVolumeMultiplier = 0.75f;
				}
			}
		}
	}

	// Token: 0x06000626 RID: 1574 RVA: 0x00031600 File Offset: 0x0002F800
	private void PlayBreathSound(bool isJump = false)
	{
		AudioClip audioClip = this.GetBreathingClip(this.exhaustion).audioClip;
		float num = this.breathingSFXSettings.breathVolumeOverExhaustion.Evaluate(this.exhaustion);
		if (isJump)
		{
			num = Mathf.Clamp01(num * 1.5f);
		}
		if (this.runner.isMusicRunning)
		{
			num *= this.breathingSFXSettings.runningGameplayVolume;
		}
		if (audioClip != null && num > 0f && !AudioController.instance.vocalisationPlaying)
		{
			this.breathingSource.PlayOneShot(audioClip, num);
			this.nextBreathTime = Time.time + (isJump ? 1.5f : 1f) * this.breathingSFXSettings.breathTimeOverExhaustion.Evaluate(this.exhaustion);
			if (this.onBreatheOut != null)
			{
				this.onBreatheOut(this.exhaustion);
			}
		}
	}

	// Token: 0x06000627 RID: 1575 RVA: 0x000316D8 File Offset: 0x0002F8D8
	private MusicGameAudioClip GetBreathingClip(float exhaustion)
	{
		if (exhaustion > 0.7f)
		{
			return this.breathingSFXSettings.heaviestBreathingClips.Random<MusicGameAudioClip>();
		}
		if (exhaustion > 0.5f)
		{
			return this.breathingSFXSettings.heavyBreathingClips.Random<MusicGameAudioClip>();
		}
		if (exhaustion > 0.3f)
		{
			return this.breathingSFXSettings.regularBreathingClips.Random<MusicGameAudioClip>();
		}
		return this.breathingSFXSettings.lightBreathingClips.Random<MusicGameAudioClip>();
	}

	// Token: 0x04000725 RID: 1829
	public BreathingSFXSettings breathingSFXSettings;

	// Token: 0x04000726 RID: 1830
	public JumpSFXSettings jumpSFXSettings;

	// Token: 0x04000727 RID: 1831
	public Prototype audioSourcePrototype;

	// Token: 0x04000728 RID: 1832
	public AudioSource jumpSource;

	// Token: 0x04000729 RID: 1833
	[Space]
	public float scramblingRockfallVolume;

	// Token: 0x0400072A RID: 1834
	public AudioSource scramblingRockfallSource;

	// Token: 0x0400072B RID: 1835
	[Space]
	public float exhaustion;

	// Token: 0x0400072C RID: 1836
	public float nextBreathTime;

	// Token: 0x0400072D RID: 1837
	public AudioSource breathingSource;

	// Token: 0x0400072E RID: 1838
	public Action<float> onBreatheOut;

	// Token: 0x0400072F RID: 1839
	public RunnerAudioManager.JumpState jumpState;

	// Token: 0x04000730 RID: 1840
	private int musicRunningNextBeatIndex;

	// Token: 0x04000731 RID: 1841
	private float musicRunningNextBreathVolumeMultiplier = 1f;

	// Token: 0x020002EE RID: 750
	public enum JumpState
	{
		// Token: 0x040016DA RID: 5850
		Idle,
		// Token: 0x040016DB RID: 5851
		Jumping,
		// Token: 0x040016DC RID: 5852
		HandledJump
	}
}
