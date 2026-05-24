using System;
using UnityEngine;

// Token: 0x02000020 RID: 32
public class FollowPathCameraSettings : ScriptableObject
{
	// Token: 0x04000118 RID: 280
	[Space]
	public float minDistance = 20f;

	// Token: 0x04000119 RID: 281
	[Space]
	public float durationLocal = 3f;

	// Token: 0x0400011A RID: 282
	public float durationStandard = 6f;

	// Token: 0x0400011B RID: 283
	public float durationLong = 8f;

	// Token: 0x0400011C RID: 284
	public Range localPathZDistRange = new Range(5f, 50f);

	// Token: 0x0400011D RID: 285
	public float maxLocalCamZoomStrength = 0.2f;

	// Token: 0x0400011E RID: 286
	[Space]
	public AnimationCurve strengthOverProgress;

	// Token: 0x0400011F RID: 287
	public AnimationCurve strengthOverProgressLocal;

	// Token: 0x04000120 RID: 288
	public AnimationCurve shearOverProgress;

	// Token: 0x04000121 RID: 289
	public AnimationCurve extraHeightOverProgress;

	// Token: 0x04000122 RID: 290
	public AnimationCurve distanceFromStartOverProgress;
}
