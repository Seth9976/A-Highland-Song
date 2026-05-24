using System;
using UnityEngine;

// Token: 0x020000B0 RID: 176
public class TwinkleSettings : ScriptableObject
{
	// Token: 0x04000661 RID: 1633
	public AnimationCurve rotationOverTime;

	// Token: 0x04000662 RID: 1634
	public Gradient colourOverTime;

	// Token: 0x04000663 RID: 1635
	public Range duration = new Range(1f, 1f);

	// Token: 0x04000664 RID: 1636
	public Range pauseDuration = new Range(0.1f, 0.1f);
}
