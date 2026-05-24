using System;
using UnityEngine;

// Token: 0x0200007F RID: 127
public class RunTrack : MonoSingleton<RunTrack>
{
	// Token: 0x170000F1 RID: 241
	// (get) Token: 0x0600037F RID: 895 RVA: 0x0001B594 File Offset: 0x00019794
	// (set) Token: 0x06000380 RID: 896 RVA: 0x0001B59C File Offset: 0x0001979C
	public BeatTrack track
	{
		get
		{
			return this._track;
		}
		private set
		{
			this._track = value;
		}
	}

	// Token: 0x170000F2 RID: 242
	// (get) Token: 0x06000381 RID: 897 RVA: 0x0001B5A5 File Offset: 0x000197A5
	private Runner runner
	{
		get
		{
			return Runner.instance;
		}
	}

	// Token: 0x170000F3 RID: 243
	// (get) Token: 0x06000382 RID: 898 RVA: 0x0001B5AC File Offset: 0x000197AC
	// (set) Token: 0x06000383 RID: 899 RVA: 0x0001B5B4 File Offset: 0x000197B4
	public RunTrack.State state
	{
		get
		{
			return this._state;
		}
		set
		{
			if (this._state != value)
			{
				this._state = value;
			}
		}
	}

	// Token: 0x170000F4 RID: 244
	// (get) Token: 0x06000384 RID: 900 RVA: 0x0001B5C6 File Offset: 0x000197C6
	public bool playing
	{
		get
		{
			return this.state == RunTrack.State.Playing;
		}
	}

	// Token: 0x170000F5 RID: 245
	// (get) Token: 0x06000385 RID: 901 RVA: 0x0001B5D1 File Offset: 0x000197D1
	public bool playingOrRampingUp
	{
		get
		{
			return this.state == RunTrack.State.Playing || this.state == RunTrack.State.RampingUp;
		}
	}

	// Token: 0x170000F6 RID: 246
	// (get) Token: 0x06000386 RID: 902 RVA: 0x0001B5E7 File Offset: 0x000197E7
	public bool playingOrAboutTo
	{
		get
		{
			return this.state == RunTrack.State.Playing || this.state == RunTrack.State.RampingUp || this.state == RunTrack.State.Scheduled;
		}
	}

	// Token: 0x170000F7 RID: 247
	// (get) Token: 0x06000387 RID: 903 RVA: 0x0001B606 File Offset: 0x00019806
	public bool paused
	{
		get
		{
			return this._pauseReason > RunTrack.PauseReason.None;
		}
	}

	// Token: 0x170000F8 RID: 248
	// (get) Token: 0x06000388 RID: 904 RVA: 0x0001B611 File Offset: 0x00019811
	public bool scheduled
	{
		get
		{
			return this.state == RunTrack.State.Scheduled;
		}
	}

	// Token: 0x170000F9 RID: 249
	// (get) Token: 0x06000389 RID: 905 RVA: 0x0001B61C File Offset: 0x0001981C
	public bool stopped
	{
		get
		{
			return this.state == RunTrack.State.Stopped;
		}
	}

	// Token: 0x170000FA RID: 250
	// (get) Token: 0x0600038A RID: 906 RVA: 0x0001B627 File Offset: 0x00019827
	// (set) Token: 0x0600038B RID: 907 RVA: 0x0001B62F File Offset: 0x0001982F
	public float initialSprintStartTime { get; private set; }

	// Token: 0x170000FB RID: 251
	// (get) Token: 0x0600038C RID: 908 RVA: 0x0001B638 File Offset: 0x00019838
	public bool playerLockedIntoMusicRunStart
	{
		get
		{
			return this.state == RunTrack.State.Scheduled || this.state == RunTrack.State.RampingUp;
		}
	}

	// Token: 0x170000FC RID: 252
	// (get) Token: 0x0600038D RID: 909 RVA: 0x0001B64E File Offset: 0x0001984E
	public float signedTimeSinceMusicStart
	{
		get
		{
			return (float)(AudioSettings.dspTime - this._scheduledStartDspTime);
		}
	}

	// Token: 0x170000FD RID: 253
	// (get) Token: 0x0600038E RID: 910 RVA: 0x0001B65D File Offset: 0x0001985D
	public float currentMusicTime
	{
		get
		{
			return this.currentMusicTimeRawUncalibrated - LatencyCalibrator.latency;
		}
	}

	// Token: 0x170000FE RID: 254
	// (get) Token: 0x0600038F RID: 911 RVA: 0x0001B66C File Offset: 0x0001986C
	private float currentMusicTimeRawUncalibrated
	{
		get
		{
			if (this.state == RunTrack.State.Scheduled)
			{
				double num = this._scheduledStartDspTime - AudioSettings.dspTime;
				return this.source.time - (float)num;
			}
			return (float)(AudioSettings.dspTime - this._trackStartDspTime);
		}
	}

	// Token: 0x170000FF RID: 255
	// (get) Token: 0x06000390 RID: 912 RVA: 0x0001B6AB File Offset: 0x000198AB
	public float scheduledTrackStartTime
	{
		get
		{
			if (this.state != RunTrack.State.Scheduled)
			{
				return 0f;
			}
			return this.source.time;
		}
	}

	// Token: 0x17000100 RID: 256
	// (get) Token: 0x06000391 RID: 913 RVA: 0x0001B6C7 File Offset: 0x000198C7
	public int currentBeatIdx
	{
		get
		{
			return this.track.BeatIndexAtTime(this.currentMusicTime);
		}
	}

	// Token: 0x17000101 RID: 257
	// (get) Token: 0x06000392 RID: 914 RVA: 0x0001B6DA File Offset: 0x000198DA
	public float fractionalBeatIdx
	{
		get
		{
			return this.track.FractionalIndexAtTime(this.currentMusicTime);
		}
	}

	// Token: 0x17000102 RID: 258
	// (get) Token: 0x06000393 RID: 915 RVA: 0x0001B6ED File Offset: 0x000198ED
	public int nearestBeatIndex
	{
		get
		{
			return this.track.NearestBeatIndexAtTime(this.currentMusicTime);
		}
	}

	// Token: 0x17000103 RID: 259
	// (get) Token: 0x06000394 RID: 916 RVA: 0x0001B700 File Offset: 0x00019900
	public float currentBeatDuration
	{
		get
		{
			return this.BeatDurationAtTime(this.currentMusicTime);
		}
	}

	// Token: 0x17000104 RID: 260
	// (get) Token: 0x06000395 RID: 917 RVA: 0x0001B710 File Offset: 0x00019910
	public float timeToNearestObstacle
	{
		get
		{
			if (!this.playingOrRampingUp)
			{
				return float.PositiveInfinity;
			}
			BeatTrack.ObstacleRef obstacleRef = this.FindNearestObstacle();
			if (!obstacleRef.valid)
			{
				return float.PositiveInfinity;
			}
			return obstacleRef.time - this.currentMusicTime;
		}
	}

	// Token: 0x06000396 RID: 918 RVA: 0x0001B74E File Offset: 0x0001994E
	public BeatTrack.ObstacleRef FindNearestObstacle()
	{
		return this.track.FindNearestObstacle(this.currentMusicTime);
	}

	// Token: 0x17000105 RID: 261
	// (get) Token: 0x06000397 RID: 919 RVA: 0x0001B761 File Offset: 0x00019961
	public int currBarIdx
	{
		get
		{
			return this.track.BarIndexAtTime(this.currentMusicTime);
		}
	}

	// Token: 0x06000398 RID: 920 RVA: 0x0001B774 File Offset: 0x00019974
	public float BeatDurationAtTime(float time)
	{
		if (!this.playing)
		{
			return this.settings.defaultBeatDuration;
		}
		return this.track.BeatDurationAtTimeWhenPlaying(time);
	}

	// Token: 0x06000399 RID: 921 RVA: 0x0001B798 File Offset: 0x00019998
	public void ChangeTrack(BeatTrack newTrack, bool forceStopMusic = false, int barIdx = -1)
	{
		if (forceStopMusic && (this.state == RunTrack.State.RampingUp || this.state == RunTrack.State.Playing))
		{
			this.state = RunTrack.State.RampingDown;
			this._musicRunSuccessState = RunTrack.MusicRunSuccessState.AwaitingStart;
		}
		if (newTrack != null && newTrack != this.track)
		{
			this._queuedTrack = newTrack;
		}
		if (barIdx != -1)
		{
			this._queuedBarIdx = barIdx;
		}
	}

	// Token: 0x0600039A RID: 922 RVA: 0x0001B7F4 File Offset: 0x000199F4
	public void SetPaused(bool wantsPause, RunTrack.PauseReason reason)
	{
		if (!this.playingOrAboutTo)
		{
			Debug.LogError("RunTrack.SetPaused should only be called when playing");
			return;
		}
		bool paused = this.paused;
		if (wantsPause)
		{
			this._pauseReason |= reason;
		}
		else
		{
			this._pauseReason &= ~reason;
		}
		if (paused == this.paused)
		{
			return;
		}
		if (this.state == RunTrack.State.Scheduled)
		{
			if (this.paused)
			{
				this._pausedScheduledTimeToStart = this._scheduledStartDspTime - AudioSettings.dspTime;
				this.source.Stop();
				return;
			}
			this._scheduledStartDspTime = AudioSettings.dspTime + this._pausedScheduledTimeToStart;
			this.source.PlayScheduled(this._scheduledStartDspTime);
			this._trackStartDspTime = AudioSettings.dspTime + this._pausedScheduledTimeToStart - (double)this.source.time;
			return;
		}
		else
		{
			if (this.paused)
			{
				this.source.Pause();
				return;
			}
			this._trackStartDspTime = (this._scheduledStartDspTime = AudioSettings.dspTime - (double)this.source.time);
			this.source.UnPause();
			return;
		}
	}

	// Token: 0x0600039B RID: 923 RVA: 0x0001B8F8 File Offset: 0x00019AF8
	public void Clear()
	{
		this.source.volume = 0f;
		this.state = RunTrack.State.Stopped;
		this._musicRunSuccessState = RunTrack.MusicRunSuccessState.AwaitingStart;
		this._hasChosenMusicForCurrentRun = false;
		this._queuedTrack = null;
		this._queuedBarIdx = -1;
		this._pauseReason = RunTrack.PauseReason.None;
		this._flawless = true;
	}

	// Token: 0x0600039C RID: 924 RVA: 0x0001B946 File Offset: 0x00019B46
	private void Start()
	{
		this.Clear();
	}

	// Token: 0x0600039D RID: 925 RVA: 0x0001B94E File Offset: 0x00019B4E
	public void PlayerDidStumble()
	{
		this._flawless = false;
	}

	// Token: 0x0600039E RID: 926 RVA: 0x0001B958 File Offset: 0x00019B58
	private void ResetAndScheduleMusic()
	{
		this.source.Stop();
		MusicRun currentMusicRun = this.runner.currentMusicRun;
		if (!DebugOptions.opts.dontChooseNewMusic)
		{
			if (currentMusicRun == null)
			{
				Debug.LogError("Expected runner.currentMusicRun to be valid but it was null when starting music");
			}
			else if (!this._hasChosenMusicForCurrentRun)
			{
				float num = currentMusicRun.range.length / Simulate.SignedSpeedOnGround(1f, 0f, false, Runner.instance.settings.run);
				BeatTrackSection beatTrack = MonoSingleton<MusicLibrary>.instance.GetBeatTrack(currentMusicRun.inkName, num);
				this.track = beatTrack.track;
				this.musicStartBarIdx = beatTrack.section.startBarIdx;
				this._hasChosenMusicForCurrentRun = true;
				if (!this.source.isPlaying)
				{
					this.source.clip = this.track.clip;
				}
			}
		}
		if (this.musicStartBarIdx < 0 || this.musicStartBarIdx >= this.track.bars.Length)
		{
			Debug.LogError(string.Format("Music start bar idx was out of range {0}/{1} on track {2}. Resetting to first bar", this.musicStartBarIdx, this.track.bars.Length, this.track.name));
			this.musicStartBarIdx = 0;
			return;
		}
		float time = this.track.beats[this.track.bars[this.musicStartBarIdx].firstBeatIdx].time;
		Simulate.FindOptions standardPredict = Simulate.FindOptions.standardPredict;
		standardPredict.SetDirectionEastwards(this.runner.runningEastwards);
		Simulate.FindResult findResult = Simulate.FindTimeFromTo(this.runner.lastRunPos, this.worldPosToStartMusic, this.runner.direction, standardPredict, this.runner.settings);
		if (!findResult.foundRequestedX)
		{
			Vector3 point3d = this.worldPosToStartMusic.slope.SampleAt(this.worldPosToStartMusic.x, false).point3d;
			Debug.DrawLine(point3d, point3d + 10f * Vector3.up, Color.blue, 20f);
			Debug.DrawLine(findResult.sample.point3d, findResult.sample.point3d + 10f * Vector3.up, Color.red, 20f);
			Debug.LogError(string.Format("Failed to find the edge of the next bar. What's the circumstance? Runner pos={0}, worldPosToStartMusic={1} ({2} from runner), found={3} ({4} from runner). Flipped={5}", new object[]
			{
				this.runner.lastRunPos.x,
				this.worldPosToStartMusic.x,
				this.worldPosToStartMusic.x - this.runner.lastRunPos.x,
				findResult.sample.point.x,
				findResult.sample.point.x - this.runner.lastRunPos.x,
				findResult.flipped
			}));
		}
		float duration = findResult.duration;
		bool flag = true;
		float num2;
		if (this.musicStartBarIdx == 0)
		{
			num2 = time;
			flag = false;
		}
		else
		{
			num2 = 4f * this.BeatDurationAtTime(time);
		}
		if (duration < num2 || time < num2)
		{
			Debug.LogWarning(string.Format("No time for a lead-in: timeToChunkEdge={0}, barAlignedMusicStart={1}, leadIn={2}", duration, time, num2));
			Debug.DrawLine(findResult.sample.point3d, findResult.sample.point3d + 5f * Vector3.up, Color.red);
			this.ScheduleMusicStart(time, duration, flag);
			return;
		}
		Debug.DrawLine(new Vector3(this.worldPosToStartMusic.x, 1000f, 0f), new Vector3(this.worldPosToStartMusic.x, -1000f, 0f), Color.cyan, 10f);
		this.ScheduleMusicStart(time - num2, duration - num2, flag);
	}

	// Token: 0x0600039F RID: 927 RVA: 0x0001BD40 File Offset: 0x00019F40
	private void ScheduleMusicStart(float trackTime, float inDspTime, bool rampUpVolume = true)
	{
		this.source.time = trackTime;
		if (!rampUpVolume)
		{
			this.source.volume = 1f;
		}
		this._scheduledStartDspTime = AudioSettings.dspTime + (double)inDspTime;
		this.source.PlayScheduled(this._scheduledStartDspTime);
		this._trackStartDspTime = AudioSettings.dspTime + (double)inDspTime - (double)trackTime;
	}

	// Token: 0x060003A0 RID: 928 RVA: 0x0001BD9C File Offset: 0x00019F9C
	private void Update()
	{
		if (this.runner == null)
		{
			return;
		}
		if (this.paused)
		{
			return;
		}
		if (this.source.clip != this.track.clip)
		{
			if (this.source.clip)
			{
				this.source.Stop();
				this.source.volume = 0f;
			}
			if (this.state != RunTrack.State.Stopped)
			{
				this.state = RunTrack.State.Stopped;
				this._musicRunSuccessState = RunTrack.MusicRunSuccessState.AwaitingStart;
			}
			this.source.clip = this.track.clip;
		}
		bool flag = this._queuedTrack != null || this._queuedBarIdx != -1;
		bool flag2 = false;
		if (this.runner.prevMusicRun != null && this.runner.currentMusicRun == null)
		{
			flag2 = !((this.runner.prevPosition.x < this.runner.position.x) ? this.runner.prevMusicRun.musicContinuesEast : this.runner.prevMusicRun.musicContinuesEast);
		}
		bool flag3 = this.runner.inMusicRunningArea && this.runner.momentumAbs >= 1f;
		bool flag4 = !this.runner.inMusicRunningArea && !flag2 && this.runner.momentumAbs >= 1f && (this.state == RunTrack.State.Playing || this.state == RunTrack.State.RampingUp) && this.runner.timeOutsideMusicArea < this.runner.settings.run.maxAutoRunOutsideMusicArea;
		bool flag5 = (this.runner.running || this.runner.sliding || this.runner.balancing) && (flag3 || flag4) && this.state != RunTrack.State.RampingDown && !flag;
		bool flag6 = this.source.clip != null && this.source.time > this.source.clip.length - 0.2f;
		bool flag7 = (this.runner.jumping || this.runner.falling || this.runner.bridgingGap) && (this.state == RunTrack.State.RampingUp || this.state == RunTrack.State.Playing || this.state == RunTrack.State.Scheduled) && !flag;
		bool flag8 = (flag5 || flag7) && !flag6;
		if (this.state == RunTrack.State.Stopped && flag8)
		{
			Range range = this.runner.currentMusicRun.range;
			float num = (this.runner.runningEastwards ? range.max : range.min);
			if (!this.worldPosToStartMusic.isValid)
			{
				flag8 = false;
			}
			else if (Mathf.Abs(num - this.runner.position.x) < this.settings.minimumXSpaceRequireToStart)
			{
				flag8 = false;
			}
			else
			{
				Simulate.FindOptions standardSimulate = Simulate.FindOptions.standardSimulate;
				standardSimulate.SetDirectionEastwards(this.runner.runningEastwards);
				if (Simulate.FindGroundPositionAtTime(this.runner.lastRunPos, this.settings.minimumRunLeadInTimeBeforeObstacle, this.runner.direction, standardSimulate, this.runner.settings).remainingTime > 0f)
				{
					flag8 = false;
				}
			}
		}
		this.readyToSprint = false;
		Game.instance.RemoveTimeScalar(Game.TimeScalar.ReadyToSprint);
		if (flag8)
		{
			if (this.state == RunTrack.State.Stopped)
			{
				this.readyToSprint = true;
				if (GameInput.sprintPressed)
				{
					this.readyToSprint = false;
					this.state = RunTrack.State.Scheduled;
					this.initialSprintStartTime = Time.unscaledTime;
					this.ResetAndScheduleMusic();
					History.Log("MUSIC RUNNING SCHEDULED");
					AudioController.instance.PlayVocalisation(Vocalisation.WoohooStartMusicRun, 0f);
					AudioController.instance.PlaySoundEffect(SoundEffect.Ting);
					AudioController.instance.PlaySoundEffect(SoundEffect.Whoosh);
				}
				else
				{
					Game.instance.SetTimeScalar(Game.TimeScalar.ReadyToSprint, 0.25f);
					History.Log("READY TO SPRINT");
					if (!Narrative.instance.isBusy && !this._hasNarrativeMentionedMusicRunStart)
					{
						Narrative.instance.StartMusicRunning();
						this._hasNarrativeMentionedMusicRunStart = true;
					}
				}
			}
			else if (this.state == RunTrack.State.Scheduled)
			{
				if (AudioSettings.dspTime > this._scheduledStartDspTime)
				{
					this.state = RunTrack.State.RampingUp;
					History.Log("RAMPING UP");
					if (this._musicRunSuccessState != RunTrack.MusicRunSuccessState.Fail)
					{
						this._musicRunSuccessState = RunTrack.MusicRunSuccessState.MusicRunningSucceeding;
					}
				}
			}
			else if (this.state == RunTrack.State.RampingUp)
			{
				this.source.volume = Mathf.MoveTowards(this.source.volume, 1f, Time.deltaTime / this.musicSettings.runMusicRampUpTime);
				if (this.source.volume == 1f)
				{
					this.state = RunTrack.State.Playing;
					History.Log("MUSIC RUN PLAYING");
					if (this._musicRunSuccessState != RunTrack.MusicRunSuccessState.Fail)
					{
						this._musicRunSuccessState = RunTrack.MusicRunSuccessState.MusicRunningSucceeding;
					}
				}
			}
		}
		else
		{
			this._hasNarrativeMentionedMusicRunStart = false;
			if (this.state == RunTrack.State.Playing || this.state == RunTrack.State.RampingUp || this.state == RunTrack.State.Scheduled)
			{
				bool flag9 = !this.runner.inMusicRunningArea;
				if (!Narrative.instance.isBusy && this._musicRunSuccessState == RunTrack.MusicRunSuccessState.MusicRunningSucceeding && flag9)
				{
					Narrative.instance.CompleteMusicRunning(this._flawless);
					if (this._flawless)
					{
						this.runner.TryIncreaseMaxStamina();
					}
					this.runner.health.TryMusicRunSuccessHeal(false);
				}
				this._musicRunSuccessState = ((!this.runner.inMusicRunningArea) ? RunTrack.MusicRunSuccessState.AwaitingStart : RunTrack.MusicRunSuccessState.Fail);
				if (this._musicRunSuccessState == RunTrack.MusicRunSuccessState.Fail)
				{
					this._flawless = false;
				}
				else
				{
					this._flawless = true;
				}
				this.state = RunTrack.State.RampingDown;
			}
			if (this.state == RunTrack.State.RampingDown)
			{
				float num2 = ((this.runner.inMusicRunningArea || this._queuedTrack != null) ? this.musicSettings.runMusicRampDownFastTime : this.musicSettings.runMusicRampDownSlowTime);
				this.source.volume = Mathf.Pow(Mathf.MoveTowards(Mathf.Pow(this.source.volume, 0.5f), 0f, Time.deltaTime / num2), 2f);
				float num3 = 1f - this.source.volume;
				float num4 = this.musicSettings.rampDownReverbDryLevelCurve.Evaluate(num3);
				this.musicSettings.rampDownReverbRoomCurve.Evaluate(num3);
				this.reverbFilter.dryLevel = Mathf.MoveTowards(this.reverbFilter.dryLevel, num4, 10000f * Time.deltaTime / 0.2f);
				this.reverbFilter.room = Mathf.MoveTowards(this.reverbFilter.room, num4, 10000f * Time.deltaTime / 0.2f);
				if (this.source.volume == 0f)
				{
					History.Log("MUSIC RUN RAMP DOWN COMPLETE");
					if (!DebugOptions.opts.resetMusic && this.source.clip != null)
					{
						if (this.currentMusicTime >= this.source.clip.length - 10f)
						{
							this.musicStartBarIdx = 0;
						}
						else
						{
							int num5 = BinarySearch.SearchRoundDown<BeatTrack.Bar>(this.track.bars, this.currentMusicTime, (BeatTrack.Bar bar) => this.track.beats[bar.firstBeatIdx].time);
							this.musicStartBarIdx = Mathf.Max(num5 - this.settings.failureRewindBars, 0);
						}
					}
					this.source.Stop();
					this.state = RunTrack.State.Stopped;
				}
			}
		}
		if (this.state != RunTrack.State.RampingDown)
		{
			float num6 = this.musicSettings.rampDownReverbDryLevelCurve.Evaluate(0f);
			float num7 = this.musicSettings.rampDownReverbRoomCurve.Evaluate(0f);
			this.reverbFilter.dryLevel = Mathf.MoveTowards(this.reverbFilter.dryLevel, num6, 10000f * Time.deltaTime / 0.5f);
			this.reverbFilter.room = Mathf.MoveTowards(this.reverbFilter.room, num7, 10000f * Time.deltaTime / 0.5f);
		}
		if (this.state == RunTrack.State.Stopped && flag)
		{
			if (this._queuedTrack != null)
			{
				this.track = this._queuedTrack;
				this._queuedTrack = null;
			}
			if (this._queuedBarIdx != -1)
			{
				this.musicStartBarIdx = this._queuedBarIdx;
				this._queuedBarIdx = -1;
			}
			else
			{
				this.musicStartBarIdx = 0;
			}
		}
		if (this.state == RunTrack.State.Stopped && Runner.instance.currentMusicRun == null && this._hasChosenMusicForCurrentRun)
		{
			this._hasChosenMusicForCurrentRun = false;
			this._musicRunSuccessState = RunTrack.MusicRunSuccessState.AwaitingStart;
		}
		if (!this.runner.inMusicRunningArea && this.state == RunTrack.State.Stopped)
		{
			this._flawless = true;
		}
		if (this.state == RunTrack.State.Playing)
		{
			float num8 = 1f;
			float num9 = 0.5f;
			if (this.runner.running)
			{
				RhythmActionMarker nextUpcomingMarker = MonoSingleton<TrackBuilder>.instance.nextUpcomingMarker;
				if (nextUpcomingMarker != null)
				{
					float duration = Simulate.FindTimeFromTo(this.runner.trackPosition, new TrackPosition
					{
						x = nextUpcomingMarker.transform.position.x
					}, this.runner.direction, Simulate.FindOptions.standardPredict, this.runner.settings).duration;
					float num10 = MonoSingleton<RunTrack>.instance.currentMusicTime + duration - nextUpcomingMarker.obstacleRef.time;
					float num11 = Mathf.InverseLerp(0.03f, 0.1f, Mathf.Abs(num10));
					if (num10 > 0f)
					{
						num8 = Mathf.Lerp(1f, 1.3f, num11);
					}
					else
					{
						num8 = Mathf.Lerp(1f, 0.8f, num11);
					}
				}
				else
				{
					num8 = 1f;
				}
			}
			else if (this.runner.jumping)
			{
				num9 = 0.1f;
				num8 = 1f;
			}
			float num12 = TimeX.Lerping(num9, Time.unscaledDeltaTime);
			this._musicRunCatchupScalar = Mathf.Lerp(this._musicRunCatchupScalar, num8, num12);
			Game.instance.SetTimeScalar(Game.TimeScalar.MusicRunCatchup, this._musicRunCatchupScalar);
		}
		else
		{
			this._musicRunCatchupScalar = 1f;
			Game.instance.RemoveTimeScalar(Game.TimeScalar.MusicRunCatchup);
		}
		bool inMusicRunningArea = this.runner.inMusicRunningArea;
	}

	// Token: 0x060003A1 RID: 929 RVA: 0x0001C7B8 File Offset: 0x0001A9B8
	private void OnGUI()
	{
		if (DebugOptions.opts.trackBarAndTimeGUI && this.state != RunTrack.State.Stopped)
		{
			GUI.color = Color.black.WithAlpha(this.source.volume);
			float num = 10f;
			float num2 = (float)Screen.width - 80f;
			GUI.Label(new Rect(num2, num, 80f, 30f), string.Format("{0:0.00}s", this.currentMusicTime));
			num += 25f;
			int num3 = Mathf.Max(this.track.BarIndexAtTime(this.currentMusicTime), 0);
			GUI.Label(new Rect(num2, num, 80f, 30f), string.Format("Bar {0}", num3));
		}
	}

	// Token: 0x17000106 RID: 262
	// (get) Token: 0x060003A2 RID: 930 RVA: 0x0001C87B File Offset: 0x0001AA7B
	private AudioSource source
	{
		get
		{
			if (this._source == null)
			{
				this._source = base.GetComponent<AudioSource>();
			}
			return this._source;
		}
	}

	// Token: 0x17000107 RID: 263
	// (get) Token: 0x060003A3 RID: 931 RVA: 0x0001C89D File Offset: 0x0001AA9D
	private AudioReverbFilter reverbFilter
	{
		get
		{
			if (this._reverbFilter == null)
			{
				this._reverbFilter = base.GetComponent<AudioReverbFilter>();
			}
			return this._reverbFilter;
		}
	}

	// Token: 0x040004D7 RID: 1239
	public int musicStartBarIdx;

	// Token: 0x040004D8 RID: 1240
	public TrackPosition worldPosToStartMusic = TrackPosition.none;

	// Token: 0x040004D9 RID: 1241
	[SerializeField]
	private BeatTrack _track = Presume<BeatTrack>.NonNull("C:\\Users\\josep\\Desktop\\highland-song\\Highland Run\\Assets\\Scripts\\MusicRunning\\RunTrack.cs", 33);

	// Token: 0x040004DA RID: 1242
	public RunTrackSettings settings = Presume<RunTrackSettings>.NonNull("C:\\Users\\josep\\Desktop\\highland-song\\Highland Run\\Assets\\Scripts\\MusicRunning\\RunTrack.cs", 34);

	// Token: 0x040004DB RID: 1243
	public MusicSettings musicSettings = Presume<MusicSettings>.NonNull("C:\\Users\\josep\\Desktop\\highland-song\\Highland Run\\Assets\\Scripts\\MusicRunning\\RunTrack.cs", 35);

	// Token: 0x040004DC RID: 1244
	public bool readyToSprint;

	// Token: 0x040004DD RID: 1245
	private RunTrack.State _state;

	// Token: 0x040004DF RID: 1247
	private AudioSource _source;

	// Token: 0x040004E0 RID: 1248
	private AudioReverbFilter _reverbFilter;

	// Token: 0x040004E1 RID: 1249
	private double _trackStartDspTime;

	// Token: 0x040004E2 RID: 1250
	private double _scheduledStartDspTime;

	// Token: 0x040004E3 RID: 1251
	private BeatTrack _queuedTrack;

	// Token: 0x040004E4 RID: 1252
	private int _queuedBarIdx = -1;

	// Token: 0x040004E5 RID: 1253
	private RunTrack.PauseReason _pauseReason;

	// Token: 0x040004E6 RID: 1254
	private double _pausedScheduledTimeToStart;

	// Token: 0x040004E7 RID: 1255
	private bool _hasChosenMusicForCurrentRun;

	// Token: 0x040004E8 RID: 1256
	private bool _hasNarrativeMentionedMusicRunStart;

	// Token: 0x040004E9 RID: 1257
	private float _musicRunCatchupScalar;

	// Token: 0x040004EA RID: 1258
	private RunTrack.MusicRunSuccessState _musicRunSuccessState;

	// Token: 0x040004EB RID: 1259
	[SerializeField]
	private bool _flawless = true;

	// Token: 0x02000299 RID: 665
	public enum State
	{
		// Token: 0x0400153B RID: 5435
		Stopped,
		// Token: 0x0400153C RID: 5436
		Scheduled,
		// Token: 0x0400153D RID: 5437
		RampingUp,
		// Token: 0x0400153E RID: 5438
		RampingDown,
		// Token: 0x0400153F RID: 5439
		Playing
	}

	// Token: 0x0200029A RID: 666
	[Flags]
	public enum PauseReason
	{
		// Token: 0x04001541 RID: 5441
		None = 0,
		// Token: 0x04001542 RID: 5442
		Tutorial = 1,
		// Token: 0x04001543 RID: 5443
		Feedback = 2,
		// Token: 0x04001544 RID: 5444
		Journal = 4,
		// Token: 0x04001545 RID: 5445
		PhotoMode = 8
	}

	// Token: 0x0200029B RID: 667
	private enum MusicRunSuccessState
	{
		// Token: 0x04001547 RID: 5447
		AwaitingStart,
		// Token: 0x04001548 RID: 5448
		MusicRunningSucceeding,
		// Token: 0x04001549 RID: 5449
		Fail
	}
}
