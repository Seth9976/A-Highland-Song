using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200000F RID: 15
public class PeakStateAudioControllerSettings : ScriptableObject
{
	// Token: 0x04000067 RID: 103
	public List<AudioClip> clickClips;

	// Token: 0x04000068 RID: 104
	public float clickDistanceInterval;

	// Token: 0x04000069 RID: 105
	[Space]
	[Header("Move whoosh effect")]
	public float speedSmoothing;

	// Token: 0x0400006A RID: 106
	public float whooshDamping;

	// Token: 0x0400006B RID: 107
	public AnimationCurve whooshOverMoveSpeed;

	// Token: 0x0400006C RID: 108
	public AnimationCurve whooshOverZoomSpeed;

	// Token: 0x0400006D RID: 109
	public AnimationCurve whooshVolumeOverStrength;

	// Token: 0x0400006E RID: 110
	public AnimationCurve whooshPitchOverStrength;

	// Token: 0x0400006F RID: 111
	[Space]
	[Header("Prop find victory sfx")]
	public List<AudioClip> findPropClips;

	// Token: 0x04000070 RID: 112
	[Space]
	[Header("Prop nearby glimmer")]
	public AnimationCurve glimmerVolumeOverWidgetDistance;

	// Token: 0x04000071 RID: 113
	public AnimationCurve glimmerPanOverSignedWidgetDistanceX;
}
