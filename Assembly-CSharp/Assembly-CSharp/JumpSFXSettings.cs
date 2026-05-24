using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000C3 RID: 195
public class JumpSFXSettings : ScriptableObject
{
	// Token: 0x0400071A RID: 1818
	public float runningGameplayVolumeScale = 1f;

	// Token: 0x0400071B RID: 1819
	[Space]
	public float musicBeatMarkerVolume = 1f;

	// Token: 0x0400071C RID: 1820
	public List<AudioClip> runningLandRewardNoteClips;

	// Token: 0x0400071D RID: 1821
	[Space]
	public float jumpShoutVolume = 1f;

	// Token: 0x0400071E RID: 1822
	public List<MusicGameAudioClip> jumpShoutClips;

	// Token: 0x0400071F RID: 1823
	[Space]
	public float backpackShuffleJumpVolume = 1f;

	// Token: 0x04000720 RID: 1824
	public List<AudioClip> backpackShuffleJumpClips;

	// Token: 0x04000721 RID: 1825
	[Space]
	public float backpackShuffleLandVolume = 1f;

	// Token: 0x04000722 RID: 1826
	public List<MusicGameAudioClip> backpackShuffleLandClips;
}
