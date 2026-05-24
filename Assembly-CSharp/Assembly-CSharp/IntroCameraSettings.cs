using System;
using UnityEngine;

// Token: 0x02000024 RID: 36
public class IntroCameraSettings : ScriptableObject
{
	// Token: 0x04000182 RID: 386
	public float maxSpeed = 100f;

	// Token: 0x04000183 RID: 387
	public float initialAccelDuration = 6f;

	// Token: 0x04000184 RID: 388
	public float slowdownDistance = 50f;

	// Token: 0x04000185 RID: 389
	public float slowdownDistanceFast = 20f;

	// Token: 0x04000186 RID: 390
	public float endingSlowdown = 0.3f;

	// Token: 0x04000187 RID: 391
	public float continueExistingGameSpeedScalar = 3f;

	// Token: 0x04000188 RID: 392
	public float initialAccelDurationFast = 3f;

	// Token: 0x04000189 RID: 393
	public Range sidewaysAngleRange = new Range(45f, 90f);

	// Token: 0x0400018A RID: 394
	public float sidewaysSpeedup = 2f;

	// Token: 0x0400018B RID: 395
	public float firstSplineControlDist = 100f;

	// Token: 0x0400018C RID: 396
	public float intermediateSplineControlDist = 100f;

	// Token: 0x0400018D RID: 397
	public float finalSplineControlDist = 100f;

	// Token: 0x0400018E RID: 398
	public float penultimateSplineControlDist = 100f;

	// Token: 0x0400018F RID: 399
	public Vector3 penultimateBezierPointOffset = new Vector3(0f, 20f, -30f);

	// Token: 0x04000190 RID: 400
	public Vector3 penultimateBezierPointDir = new Vector3(0f, -1f, 0f);

	// Token: 0x04000191 RID: 401
	public float speedSmoothTime = 3f;

	// Token: 0x04000192 RID: 402
	public float speedSmoothTimeFast = 1f;

	// Token: 0x04000193 RID: 403
	public float blendOutDistance = 20f;

	// Token: 0x04000194 RID: 404
	public HighlandCameraProperties startProps = HighlandCameraProperties.@default;

	// Token: 0x04000195 RID: 405
	public AnimationCurve easeInOutCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

	// Token: 0x04000196 RID: 406
	public AnimationCurve slowEaseInOutCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
}
