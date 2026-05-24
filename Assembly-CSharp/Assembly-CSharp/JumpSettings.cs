using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020000D5 RID: 213
public class JumpSettings : ScriptableObject
{
	// Token: 0x040008AF RID: 2223
	public float jumpDurationStandard = 0.55f;

	// Token: 0x040008B0 RID: 2224
	public Range jumpHeightRange = new Range(5f, 7f);

	// Token: 0x040008B1 RID: 2225
	public float balancePointJumpHeightScalar = 0.8f;

	// Token: 0x040008B2 RID: 2226
	public float balancePointDurationScalar = 0.9f;

	// Token: 0x040008B3 RID: 2227
	public float balancePointSideDropXSpeed = 6f;

	// Token: 0x040008B4 RID: 2228
	public float wallSlideJumpAwaySpeed = 10f;

	// Token: 0x040008B5 RID: 2229
	public float climbJumpHeightScalar = 0.4f;

	// Token: 0x040008B6 RID: 2230
	public float musicRunningJumpHeightScalar = 0.5f;

	// Token: 0x040008B7 RID: 2231
	public float jumpLandingDurationNorm = 0.2f;

	// Token: 0x040008B8 RID: 2232
	public float minJumpMomentum = 0.7f;

	// Token: 0x040008B9 RID: 2233
	public float maxJumpMomentumBonus = 0.5f;

	// Token: 0x040008BA RID: 2234
	public Range momentumBonusSteepness = new Range(-0.1f, -0.5f);

	// Token: 0x040008BB RID: 2235
	public float jumpQueueGraceTime = 0.2f;

	// Token: 0x040008BC RID: 2236
	public float maxRetroactiveJumpDelay = 0.2f;

	// Token: 0x040008BD RID: 2237
	public float jumpReleaseWindowNorm = 0.8f;

	// Token: 0x040008BE RID: 2238
	public Vector2 jumpReleaseDamping = new Vector2(0.95f, 0.95f);

	// Token: 0x040008BF RID: 2239
	public float fallToJumpGracePeriod = 0.1f;

	// Token: 0x040008C0 RID: 2240
	public float airControl = 1f;

	// Token: 0x040008C1 RID: 2241
	public float balanceSnapLowVelocity = 5f;

	// Token: 0x040008C2 RID: 2242
	public float minBalancePointJumpDurationNorm = 0.5f;

	// Token: 0x040008C3 RID: 2243
	[Nullable(1)]
	public AnimationCurve verticalTimeCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
}
