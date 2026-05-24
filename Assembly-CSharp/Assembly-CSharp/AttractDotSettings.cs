using System;
using UnityEngine;

// Token: 0x020000FC RID: 252
public class AttractDotSettings : ScriptableObject
{
	// Token: 0x04000A0C RID: 2572
	public Sprite sprite;

	// Token: 0x04000A0D RID: 2573
	public Color color = Color.white;

	// Token: 0x04000A0E RID: 2574
	public float initialPulseTime = 1f;

	// Token: 0x04000A0F RID: 2575
	public float hideTime = 1f;

	// Token: 0x04000A10 RID: 2576
	public float pulseSpeed = 5f;

	// Token: 0x04000A11 RID: 2577
	public Range pulseRange = new Range(0.8f, 1.2f);

	// Token: 0x04000A12 RID: 2578
	public AnimationCurve popOpenCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
}
