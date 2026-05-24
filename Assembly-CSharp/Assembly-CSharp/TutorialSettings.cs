using System;
using UnityEngine;

// Token: 0x02000051 RID: 81
public class TutorialSettings : ScriptableObject
{
	// Token: 0x0400037D RID: 893
	public float marginX = 40f;

	// Token: 0x0400037E RID: 894
	public float marginY = 20f;

	// Token: 0x0400037F RID: 895
	public float lowY = 80f;

	// Token: 0x04000380 RID: 896
	public float highY = 200f;

	// Token: 0x04000381 RID: 897
	public float innerPadding = 10f;

	// Token: 0x04000382 RID: 898
	public float maxTextWidth = 500f;

	// Token: 0x04000383 RID: 899
	public float tooltipMaxTextWidth = 300f;

	// Token: 0x04000384 RID: 900
	public Range musicRunAllowObsPauseRange = new Range(-0.2f, 0.2f);

	// Token: 0x04000385 RID: 901
	public float tooltipXOffsetFromAnchor = 50f;
}
