using System;
using UnityEngine;

// Token: 0x0200000D RID: 13
public class FootstepAudioClipSet : ScriptableObject
{
	// Token: 0x0400005A RID: 90
	[Range(0f, 1f)]
	public float volume = 1f;

	// Token: 0x0400005B RID: 91
	[Header("Slow speed footsteps")]
	public AudioClipSet walking;

	// Token: 0x0400005C RID: 92
	[Header("Fast speed footsteps")]
	public AudioClipSet running;

	// Token: 0x0400005D RID: 93
	[Header("On landing a jump")]
	public AudioClipSet landing;

	// Token: 0x0400005E RID: 94
	[Header("Always play this. Handy for body sounds, like rubbing on tall grass.")]
	public AudioClipSet additional;
}
