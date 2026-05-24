using System;
using UnityEngine;

// Token: 0x020000D9 RID: 217
public class StaminaSettings : ScriptableObject
{
	// Token: 0x0400091A RID: 2330
	public float climbTimeLimit = 10f;

	// Token: 0x0400091B RID: 2331
	public float staminaRestoreSpeed = 1f;

	// Token: 0x0400091C RID: 2332
	public float overhangScalar = 1.5f;

	// Token: 0x0400091D RID: 2333
	public float climbSprintScalar = 1.3f;

	// Token: 0x0400091E RID: 2334
	public float overhangStartAngle = 100f;

	// Token: 0x0400091F RID: 2335
	public float sprintInitialCost = 0.1f;

	// Token: 0x04000920 RID: 2336
	public float sprintCost = 1f;

	// Token: 0x04000921 RID: 2337
	public float staminaEffectOfHardLanding = 0.35f;

	// Token: 0x04000922 RID: 2338
	public StaminaSettings.Vignette vignette = new StaminaSettings.Vignette
	{
		fadeTime = 0.5f,
		flashPeriod = 0.7f,
		flashPeriodFast = 0.4f,
		flashAlpha = new Range(0.4f, 0.8f),
		flashAlphaFast = new Range(0.7f, 1f)
	};

	// Token: 0x02000303 RID: 771
	[Serializable]
	public struct Vignette
	{
		// Token: 0x04001770 RID: 6000
		public float fadeTime;

		// Token: 0x04001771 RID: 6001
		public float fadeMasterAlpha;

		// Token: 0x04001772 RID: 6002
		public float flashPeriod;

		// Token: 0x04001773 RID: 6003
		public float flashPeriodFast;

		// Token: 0x04001774 RID: 6004
		public Range flashAlpha;

		// Token: 0x04001775 RID: 6005
		public Range flashAlphaFast;
	}
}
