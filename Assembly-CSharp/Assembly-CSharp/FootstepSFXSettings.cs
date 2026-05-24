using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000C2 RID: 194
public class FootstepSFXSettings : ScriptableObject
{
	// Token: 0x04000712 RID: 1810
	public float footstepVolume = 1f;

	// Token: 0x04000713 RID: 1811
	public float walkRunThreshold = 3f;

	// Token: 0x04000714 RID: 1812
	public AnimationCurve footstepVolumeOverSlopeAngle;

	// Token: 0x04000715 RID: 1813
	public float runningGameplayFootstepVolumeScale = 1f;

	// Token: 0x04000716 RID: 1814
	public FootstepAudioClipDatabase footstepAudioClipDatabase;

	// Token: 0x04000717 RID: 1815
	[Space]
	public float steppingStoneVolume = 1f;

	// Token: 0x04000718 RID: 1816
	public float runningGameplaySteppingStoneVolumeScale = 1f;

	// Token: 0x04000719 RID: 1817
	public List<AudioClip> steppingStoneClip;
}
