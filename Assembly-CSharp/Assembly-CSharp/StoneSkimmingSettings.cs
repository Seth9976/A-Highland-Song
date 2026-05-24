using System;
using UnityEngine;

// Token: 0x020000EF RID: 239
public class StoneSkimmingSettings : ScriptableObject
{
	// Token: 0x040009AF RID: 2479
	public float throwAngle = 90f;

	// Token: 0x040009B0 RID: 2480
	public float minThrowTime = 0.6f;

	// Token: 0x040009B1 RID: 2481
	public Vector2 stoneReleaseLocalPosition = new Vector2(2f, 3f);

	// Token: 0x040009B2 RID: 2482
	public float stoneReleaseTimeNorm = 0.1f;

	// Token: 0x040009B3 RID: 2483
	public AnimationCurve strengthOverTime;

	// Token: 0x040009B4 RID: 2484
	public AnimationCurve initialSpeedOverStrength;

	// Token: 0x040009B5 RID: 2485
	public AnimationCurve initialSpeedMultiplierOverLuck;

	// Token: 0x040009B6 RID: 2486
	[Space]
	public AnimationCurve twinkleStrengthOverStrength;

	// Token: 0x040009B7 RID: 2487
	public AnimationCurve vibrationStrengthOverStrength;

	// Token: 0x040009B8 RID: 2488
	public float vibrationTime;

	// Token: 0x040009B9 RID: 2489
	[Space]
	public float normalizedAnimationTimeFreezePoint = 0.7f;

	// Token: 0x040009BA RID: 2490
	[Space]
	public AnimationCurve skipValueOverSpeed;

	// Token: 0x040009BB RID: 2491
	public AnimationCurve skipValueMultiplierOverLuck;

	// Token: 0x040009BC RID: 2492
	[Space]
	public float minSinkXSpeed = 15f;

	// Token: 0x040009BD RID: 2493
	[Space]
	public float gravity = -50f;

	// Token: 0x040009BE RID: 2494
	public float sinkSpeed = -10f;

	// Token: 0x040009BF RID: 2495
	[Space]
	public float fullScoreNarrativeCommentDistance = 100f;

	// Token: 0x040009C0 RID: 2496
	[Space]
	public AudioClip[] skimClips;

	// Token: 0x040009C1 RID: 2497
	public AudioClip successfulThrowChime;

	// Token: 0x040009C2 RID: 2498
	public AudioClipSet throwSwooshes;
}
