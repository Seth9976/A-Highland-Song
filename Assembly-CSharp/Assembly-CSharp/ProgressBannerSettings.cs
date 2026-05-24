using System;
using UnityEngine;

// Token: 0x0200013B RID: 315
public class ProgressBannerSettings : ScriptableObject
{
	// Token: 0x04000CDC RID: 3292
	public float iconMargin = 20f;

	// Token: 0x04000CDD RID: 3293
	public float transitionInDuration = 1f;

	// Token: 0x04000CDE RID: 3294
	public AnimationCurve transitionInCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	// Token: 0x04000CDF RID: 3295
	public float transitionOutDuration = 1f;

	// Token: 0x04000CE0 RID: 3296
	public float waitTime = 5f;

	// Token: 0x04000CE1 RID: 3297
	public float startScale = 0.8f;

	// Token: 0x04000CE2 RID: 3298
	public float endScale = 1.1f;

	// Token: 0x04000CE3 RID: 3299
	public float glowDuration = 2f;

	// Token: 0x04000CE4 RID: 3300
	public float glowDelay = 0.5f;

	// Token: 0x04000CE5 RID: 3301
	public AnimationCurve glowCurve;

	// Token: 0x04000CE6 RID: 3302
	public float glowMaxPower = 0.35f;

	// Token: 0x04000CE7 RID: 3303
	public float additionalElementFadeTime = 0.5f;

	// Token: 0x04000CE8 RID: 3304
	public float leftIconDelay = 0.5f;

	// Token: 0x04000CE9 RID: 3305
	public float rightIconDelay = 1.5f;

	// Token: 0x04000CEA RID: 3306
	public float underlineDelay = 1f;
}
