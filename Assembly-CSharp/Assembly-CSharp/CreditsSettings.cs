using System;
using UnityEngine;

// Token: 0x0200010A RID: 266
public class CreditsSettings : ScriptableObject
{
	// Token: 0x04000AAA RID: 2730
	public float initialPause = 1f;

	// Token: 0x04000AAB RID: 2731
	public float scrollSpeed = 1f;

	// Token: 0x04000AAC RID: 2732
	public float charactersRunTime = 2f;

	// Token: 0x04000AAD RID: 2733
	public float fadeInTime = 2f;

	// Token: 0x04000AAE RID: 2734
	public float fadeOutTime = 4f;

	// Token: 0x04000AAF RID: 2735
	public float fastFadeInTime = 1f;

	// Token: 0x04000AB0 RID: 2736
	public float fastFadeOutTime = 1f;

	// Token: 0x04000AB1 RID: 2737
	public float charactersRunSpeed = 600f;

	// Token: 0x04000AB2 RID: 2738
	public Range backgroundFadeTimeRange = new Range(5f, 10f);

	// Token: 0x04000AB3 RID: 2739
	public float backgroundInitialAlpha = 0.5f;
}
