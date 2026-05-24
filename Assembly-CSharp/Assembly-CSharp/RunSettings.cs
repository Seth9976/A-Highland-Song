using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020000D8 RID: 216
[NullableContext(1)]
[Nullable(0)]
public class RunSettings : ScriptableObject
{
	// Token: 0x040008D9 RID: 2265
	public Range runAngle = new Range(10f, 20f);

	// Token: 0x040008DA RID: 2266
	public Range rotateSpeed = new Range(60f, 180f);

	// Token: 0x040008DB RID: 2267
	public float almostStoppedMomentum = 0.1f;

	// Token: 0x040008DC RID: 2268
	public float stopAtSafeEdgesMomentum = 0.5f;

	// Token: 0x040008DD RID: 2269
	public float reverseGraceTime = 0.3f;

	// Token: 0x040008DE RID: 2270
	public float reverseMinSpeedScalar = 0.5f;

	// Token: 0x040008DF RID: 2271
	public float maxAutoRunOutsideMusicArea = 5f;

	// Token: 0x040008E0 RID: 2272
	public float betweenSlopesYScalar = 0.4f;

	// Token: 0x040008E1 RID: 2273
	public float runningWithLowStaminaInterval = 4.5f;

	// Token: 0x040008E2 RID: 2274
	[Header("Collision")]
	public int bumpCollisionRays = 10;

	// Token: 0x040008E3 RID: 2275
	public Range bumpCollisionHeightRange = new Range(0.2f, 5f);

	// Token: 0x040008E4 RID: 2276
	public float ledgeFeelerHeight = 0.5f;

	// Token: 0x040008E5 RID: 2277
	public float ledgeFeelerLength = 2f;

	// Token: 0x040008E6 RID: 2278
	public float ledgeFeelerPullBack = 2f;

	// Token: 0x040008E7 RID: 2279
	[Header("Speed")]
	public float maxStandardSpeed = 10f;

	// Token: 0x040008E8 RID: 2280
	public float maxStandardMomentum = 0.7f;

	// Token: 0x040008E9 RID: 2281
	public float initialMomentum = 0.25f;

	// Token: 0x040008EA RID: 2282
	public float maxDownhillMomentum = 1f;

	// Token: 0x040008EB RID: 2283
	public float maxUphillMomentum = 0.4f;

	// Token: 0x040008EC RID: 2284
	public AnimationCurve speedCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	// Token: 0x040008ED RID: 2285
	public float uphillSpeedScalar = 1f;

	// Token: 0x040008EE RID: 2286
	public float downhillSpeedScalar = 1.5f;

	// Token: 0x040008EF RID: 2287
	public float scrambleUphillScalar = 0.5f;

	// Token: 0x040008F0 RID: 2288
	public float runSteepDownhillsScalar = 2f;

	// Token: 0x040008F1 RID: 2289
	public float scrambleExpectedSpeedForAnim = 10f;

	// Token: 0x040008F2 RID: 2290
	public float maxSpeedForScambleAnim = 12f;

	// Token: 0x040008F3 RID: 2291
	[Header("Acceleration")]
	public float initialMomentumAccel = 5f;

	// Token: 0x040008F4 RID: 2292
	public float standardMomentumAccel = 1f;

	// Token: 0x040008F5 RID: 2293
	public float downhillMomentumAccel = 2.5f;

	// Token: 0x040008F6 RID: 2294
	public float uphillMomentumAccel = 0.2f;

	// Token: 0x040008F7 RID: 2295
	public float sprintMomentumAccel = 5f;

	// Token: 0x040008F8 RID: 2296
	public float musicRunMomentumAccel = 3f;

	// Token: 0x040008F9 RID: 2297
	public float waterMomentumAccelScalar = 0.3f;

	// Token: 0x040008FA RID: 2298
	public float slowdown = 0.9f;

	// Token: 0x040008FB RID: 2299
	public float overSpeedLimitSlowdown = 0.98f;

	// Token: 0x040008FC RID: 2300
	public float overSpeedLimitSlowdownUphill = 0.9f;

	// Token: 0x040008FD RID: 2301
	public float slowdownAfterLock = 0.9f;

	// Token: 0x040008FE RID: 2302
	public float maxGroundAngle = 65f;

	// Token: 0x040008FF RID: 2303
	public float maxSlideGroundAngle = 75f;

	// Token: 0x04000900 RID: 2304
	public float maxRunGroundAngle = 45f;

	// Token: 0x04000901 RID: 2305
	public float slowdownTriggerSpeedLimit = 0.7f;

	// Token: 0x04000902 RID: 2306
	public float staminaMomentumMaxSlowdown = 0.3f;

	// Token: 0x04000903 RID: 2307
	[Header("Sprint")]
	public float maxSprintMomentum = 2f;

	// Token: 0x04000904 RID: 2308
	public float sprintStartBonus = 1.2f;

	// Token: 0x04000905 RID: 2309
	public float sprintBoostTimer = 0.25f;

	// Token: 0x04000906 RID: 2310
	public float sprintBoost = 1.2f;

	// Token: 0x04000907 RID: 2311
	public float sprintLeanAngle = 15f;

	// Token: 0x04000908 RID: 2312
	public float sprintRotateSpeed = 50f;

	// Token: 0x04000909 RID: 2313
	public Vector2 sprintParticlesVelocity = new Vector2(10f, 5f);

	// Token: 0x0400090A RID: 2314
	public int sprintParticleBurstCount = 5;

	// Token: 0x0400090B RID: 2315
	[Header("Slide")]
	[Info("Sliding max speed is controlled by: maxSlideMomentum, slideDownSpeedRangeByAngle (as scalar to run's max speed)")]
	public float maxSlideMomentum = 2f;

	// Token: 0x0400090C RID: 2316
	public float maxMusicRunMomentum = 1.2f;

	// Token: 0x0400090D RID: 2317
	public float slideMinAngle = 15f;

	// Token: 0x0400090E RID: 2318
	public AnimationCurve slideSpeedCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	// Token: 0x0400090F RID: 2319
	public AnimationCurve slideUpSpeedCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	// Token: 0x04000910 RID: 2320
	public Range slideDownSpeedRangeByAngle;

	// Token: 0x04000911 RID: 2321
	public float slidingSlowdown = 0.95f;

	// Token: 0x04000912 RID: 2322
	public Range slideAccelBySteepness = new Range(1f, 2f);

	// Token: 0x04000913 RID: 2323
	public Range slideDecelBySteepness = new Range(1f, 2f);

	// Token: 0x04000914 RID: 2324
	public float slideDecelUpward = 1f;

	// Token: 0x04000915 RID: 2325
	public float slideInitialMaxMomentumLoss = 0.4f;

	// Token: 0x04000916 RID: 2326
	public float slideMinMomentum = 0.2f;

	// Token: 0x04000917 RID: 2327
	public float slideNaturalMomentum = 0.5f;

	// Token: 0x04000918 RID: 2328
	public float slideBackToRunMomentumThreshold = 0.5f;

	// Token: 0x04000919 RID: 2329
	[Header("Music runing")]
	public int stumbleCountMax = 3;
}
