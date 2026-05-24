using System;
using UnityEngine;

// Token: 0x020000A5 RID: 165
public class AnimatedTextViewSettings : ScriptableObject
{
	// Token: 0x0400061B RID: 1563
	public float maxWidth = 1200f;

	// Token: 0x0400061C RID: 1564
	public float lineHeight = 50f;

	// Token: 0x0400061D RID: 1565
	public float spaceWidth = 30f;

	// Token: 0x0400061E RID: 1566
	public float fadeTimePerWord = 0.4f;

	// Token: 0x0400061F RID: 1567
	public float timeBetweenWords = 0.1f;

	// Token: 0x04000620 RID: 1568
	public float fadeOutTime = 0.5f;

	// Token: 0x04000621 RID: 1569
	public float backgroundOpacity = 1f;

	// Token: 0x04000622 RID: 1570
	public float punctuationPause = 0.1f;

	// Token: 0x04000623 RID: 1571
	public float terminatorPause = 0.1f;

	// Token: 0x04000624 RID: 1572
	public float skipTimeMinSeconds = 0.6f;

	// Token: 0x04000625 RID: 1573
	public float skipTimeMinProportionOfFadeIn = 0.6f;
}
