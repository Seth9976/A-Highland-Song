using System;
using UnityEngine;

// Token: 0x02000151 RID: 337
public class SunClockSettings : ScriptableObject
{
	// Token: 0x04000D9E RID: 3486
	public Gradient backgroundGradient;

	// Token: 0x04000D9F RID: 3487
	public Gradient belowHorizonGradient;

	// Token: 0x04000DA0 RID: 3488
	public Gradient sunGradient;

	// Token: 0x04000DA1 RID: 3489
	public Gradient progressBarGradient;

	// Token: 0x04000DA2 RID: 3490
	public Gradient horizonLineGradient;

	// Token: 0x04000DA3 RID: 3491
	public float sunTopY = 50f;

	// Token: 0x04000DA4 RID: 3492
	public float sunBottomY = -20f;

	// Token: 0x04000DA5 RID: 3493
	public float sunRevolveRadius = 16f;

	// Token: 0x04000DA6 RID: 3494
	public float sunriseAngleNorm;

	// Token: 0x04000DA7 RID: 3495
	public float sunsetAngleNorm;

	// Token: 0x04000DA8 RID: 3496
	public float starsRotation;

	// Token: 0x04000DA9 RID: 3497
	public Range starsFadeInRange = new Range(0.6f, 0.8f);

	// Token: 0x04000DAA RID: 3498
	public float flashStart = 0.8f;

	// Token: 0x04000DAB RID: 3499
	public Color flashColor = Color.red;

	// Token: 0x04000DAC RID: 3500
	public float flashSpeed = 2f;

	// Token: 0x04000DAD RID: 3501
	public Range shadingFadeRange = new Range(0.5f, 0.8f);

	// Token: 0x04000DAE RID: 3502
	public float minShadingAlpha = 0.5f;

	// Token: 0x04000DAF RID: 3503
	public Color nightfallLabelColor = Color.red;

	// Token: 0x04000DB0 RID: 3504
	public float restRingSpeed = 60f;

	// Token: 0x04000DB1 RID: 3505
	public float timeLapseRingSpeed = 150f;

	// Token: 0x04000DB2 RID: 3506
	public float ringAccel = 50f;
}
