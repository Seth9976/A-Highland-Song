using System;
using UnityEngine;

// Token: 0x02000075 RID: 117
public class MusicSettings : ScriptableObject
{
	// Token: 0x04000484 RID: 1156
	[Header("Run")]
	public float runMusicRampUpTime = 2f;

	// Token: 0x04000485 RID: 1157
	public float runMusicRampDownFastTime = 0.5f;

	// Token: 0x04000486 RID: 1158
	public float runMusicRampDownSlowTime = 2f;

	// Token: 0x04000487 RID: 1159
	public AnimationCurve rampDownReverbRoomCurve;

	// Token: 0x04000488 RID: 1160
	public AnimationCurve rampDownReverbDryLevelCurve;

	// Token: 0x04000489 RID: 1161
	[Header("Music")]
	public float standardMusicRampTime = 2f;

	// Token: 0x0400048A RID: 1162
	public float introRampDownTime = 5f;

	// Token: 0x0400048B RID: 1163
	public float quickRampDownTimeForSting = 0.5f;

	// Token: 0x0400048C RID: 1164
	public AudioClip titleScreenMusic;

	// Token: 0x0400048D RID: 1165
	public AudioClip introMusic;

	// Token: 0x0400048E RID: 1166
	public AudioClip otherEndingMusic;

	// Token: 0x0400048F RID: 1167
	public AudioClip fantasiaMusic;

	// Token: 0x04000490 RID: 1168
	public AudioClip[] ambientMusicClips;

	// Token: 0x04000491 RID: 1169
	public float ambientMusicVolume = 0.8f;

	// Token: 0x04000492 RID: 1170
	public float ambientRampUpTime;

	// Token: 0x04000493 RID: 1171
	public float ambientRampDownTime;

	// Token: 0x04000494 RID: 1172
	[Header("Final jump music sync")]
	public int finalJumpBarIdx;

	// Token: 0x04000495 RID: 1173
	public int finalJumpLoopStartBarIdx;

	// Token: 0x04000496 RID: 1174
	public int finalJumpLoopEndBarIdx;

	// Token: 0x04000497 RID: 1175
	public int syncToFinalLoopCrossfadeBeats = 1;

	// Token: 0x04000498 RID: 1176
	public int syncToJumpCrossfadeBeats = 1;

	// Token: 0x04000499 RID: 1177
	public int syncResetLoopCrossfadeBeats = 1;
}
