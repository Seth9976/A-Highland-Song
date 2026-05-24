using System;
using UnityEngine;

// Token: 0x02000135 RID: 309
public class PeakClimbedBannerSettings : ScriptableObject
{
	// Token: 0x04000C92 RID: 3218
	public float fadeInTime = 1f;

	// Token: 0x04000C93 RID: 3219
	public float fadeOutTime = 1f;

	// Token: 0x04000C94 RID: 3220
	public float totalTime = 6f;

	// Token: 0x04000C95 RID: 3221
	public float startScale = 0.7f;

	// Token: 0x04000C96 RID: 3222
	public AnimationCurve scaleCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	// Token: 0x04000C97 RID: 3223
	public float glowStartScale = 0.5f;

	// Token: 0x04000C98 RID: 3224
	public float glowFadeInTime = 0.5f;

	// Token: 0x04000C99 RID: 3225
	public float glowVisibleScale = 1f;

	// Token: 0x04000C9A RID: 3226
	public float glowDelay = 1f;

	// Token: 0x04000C9B RID: 3227
	public float glowVisibleTime = 0.5f;

	// Token: 0x04000C9C RID: 3228
	public float glowEndScale = 0.5f;

	// Token: 0x04000C9D RID: 3229
	public float glowFadeOutTime = 2f;

	// Token: 0x04000C9E RID: 3230
	public PeakIconDatabase peakIconDatabase;
}
