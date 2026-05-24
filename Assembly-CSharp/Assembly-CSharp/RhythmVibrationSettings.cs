using System;
using UnityEngine;

// Token: 0x02000086 RID: 134
public class RhythmVibrationSettings : ScriptableObject
{
	// Token: 0x0400051F RID: 1311
	public float vibrationStrength;

	// Token: 0x04000520 RID: 1312
	[Range(0f, 0.5f)]
	public float vibrationDuration;

	// Token: 0x04000521 RID: 1313
	[Range(-0.1f, 0f)]
	public float vibrationDelay;
}
