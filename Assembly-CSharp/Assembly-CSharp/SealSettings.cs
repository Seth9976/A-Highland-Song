using System;
using UnityEngine;

// Token: 0x020001A4 RID: 420
public class SealSettings : ScriptableObject
{
	// Token: 0x040010AB RID: 4267
	public Range hiddenTime = new Range(4f, 12f);

	// Token: 0x040010AC RID: 4268
	public float diveVsBobProbability = 0.5f;

	// Token: 0x040010AD RID: 4269
	public float turnProbability = 0.1f;

	// Token: 0x040010AE RID: 4270
	public float hideProbability = 0.1f;

	// Token: 0x040010AF RID: 4271
	public Range turnedTime = new Range(0.1f, 2f);

	// Token: 0x040010B0 RID: 4272
	public FrameAnimation appearAnim;

	// Token: 0x040010B1 RID: 4273
	public FrameAnimation bobbingAnim;

	// Token: 0x040010B2 RID: 4274
	public FrameAnimation turnAnim;

	// Token: 0x040010B3 RID: 4275
	public FrameAnimation turnedAnim;

	// Token: 0x040010B4 RID: 4276
	public FrameAnimation hideAnim;

	// Token: 0x040010B5 RID: 4277
	public FrameAnimation divingAnim;

	// Token: 0x040010B6 RID: 4278
	public Color colorRange1 = Color.white;

	// Token: 0x040010B7 RID: 4279
	public Color colorRange2 = Color.white;

	// Token: 0x040010B8 RID: 4280
	public float diveDuration = 1f;

	// Token: 0x040010B9 RID: 4281
	public AnimationCurve diveAnimXCurve;

	// Token: 0x040010BA RID: 4282
	public AnimationCurve diveAnimYCurve;

	// Token: 0x040010BB RID: 4283
	public AnimationCurve diveAnimRotationCurve;

	// Token: 0x040010BC RID: 4284
	public Range diveXOffsetRange;

	// Token: 0x040010BD RID: 4285
	public Range diveYOffsetRange;

	// Token: 0x040010BE RID: 4286
	public Range driveRotationOffsetRange;

	// Token: 0x040010BF RID: 4287
	public float rippleOffsetZ = 1f;
}
