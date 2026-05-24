using System;
using UnityEngine;

// Token: 0x0200014B RID: 331
public class SettingsScreenSettings : ScriptableObject
{
	// Token: 0x04000D59 RID: 3417
	public float topMargin = 50f;

	// Token: 0x04000D5A RID: 3418
	public float bottomMargin = 50f;

	// Token: 0x04000D5B RID: 3419
	public float subHeadingMarginAbove = 50f;

	// Token: 0x04000D5C RID: 3420
	public float subHeadingHeight = 100f;

	// Token: 0x04000D5D RID: 3421
	public float infoTextMarginAbove = 10f;

	// Token: 0x04000D5E RID: 3422
	public float infoTextHeight = 60f;

	// Token: 0x04000D5F RID: 3423
	public float controlHeight = 100f;

	// Token: 0x04000D60 RID: 3424
	public float marginAboveDoneButton = 100f;

	// Token: 0x04000D61 RID: 3425
	public Range controlPosYRangeOnScreen = new Range(200f, 800f);

	// Token: 0x04000D62 RID: 3426
	public float panelSlideSmoothTime = 0.3f;
}
