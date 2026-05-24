using System;
using UnityEngine;

// Token: 0x0200015C RID: 348
public class LightningEffectsControllerSettings : ScriptableObject
{
	// Token: 0x04000DED RID: 3565
	public float backlightPower = 1.2f;

	// Token: 0x04000DEE RID: 3566
	public float backlightMultiplier = 1.2f;

	// Token: 0x04000DEF RID: 3567
	public Vector2 randomTimeBetweenFlashes = new Vector2(4f, 8f);

	// Token: 0x04000DF0 RID: 3568
	public AnimationCurve[] falloffCurves;
}
