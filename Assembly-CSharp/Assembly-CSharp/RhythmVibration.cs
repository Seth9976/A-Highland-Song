using System;
using UnityEngine;

// Token: 0x02000085 RID: 133
public class RhythmVibration : MonoBehaviour
{
	// Token: 0x17000114 RID: 276
	// (get) Token: 0x060003E6 RID: 998 RVA: 0x0001FEDB File Offset: 0x0001E0DB
	private Runner runner
	{
		get
		{
			return Runner.instance;
		}
	}

	// Token: 0x060003E7 RID: 999 RVA: 0x0001FEE4 File Offset: 0x0001E0E4
	private void Update()
	{
		if (this.runner == null || !this.runner.isMusicRunning || MonoSingleton<RunTrack>.instance.paused)
		{
			return;
		}
		RunTrack runTrack = this.runner.runTrack;
		BeatTrack track = runTrack.track;
		float vibrationDelay = this.settings.vibrationDelay;
		int num = track.BeatIndexAtTime(runTrack.currentMusicTime + vibrationDelay);
		if (this.musicRunningNextBeatIndex != num)
		{
			this.musicRunningNextBeatIndex = num;
			BeatTrack.Beat[] beats = track.beats;
			int num2 = this.musicRunningNextBeatIndex;
			float currentMusicTime = runTrack.currentMusicTime;
			if (runTrack.track.HasObstacleAt(this.musicRunningNextBeatIndex))
			{
				InputVibration.DoTimedVibration(TimedVibration.VibrateForSeconds(this.settings.vibrationStrength, this.settings.vibrationDuration));
			}
		}
	}

	// Token: 0x0400051D RID: 1309
	public RhythmVibrationSettings settings;

	// Token: 0x0400051E RID: 1310
	private int musicRunningNextBeatIndex;
}
