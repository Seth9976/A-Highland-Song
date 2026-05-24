using System;
using UnityEngine;

// Token: 0x02000087 RID: 135
[Serializable]
public class DialogueBubbleSettings : ScriptableObject
{
	// Token: 0x04000522 RID: 1314
	public float worldSpaceOffset = 1f;

	// Token: 0x04000523 RID: 1315
	public float screenSpaceOffset = 70f;

	// Token: 0x04000524 RID: 1316
	public float fadeTimePerWord = 0.1f;

	// Token: 0x04000525 RID: 1317
	public float timeBetweenWords = 0.05f;

	// Token: 0x04000526 RID: 1318
	public float backgroundFadeTime = 0.4f;

	// Token: 0x04000527 RID: 1319
	public float shadowMaxAlpha = 0.7f;

	// Token: 0x04000528 RID: 1320
	public float marginToTailX = 20f;

	// Token: 0x04000529 RID: 1321
	public float marginToTailY = 15f;

	// Token: 0x0400052A RID: 1322
	public float tailAngleOffset = 180f;

	// Token: 0x0400052B RID: 1323
	public float tailSize = 100f;

	// Token: 0x0400052C RID: 1324
	public Range tailFlipAngleRange = new Range(20f, 175f);

	// Token: 0x0400052D RID: 1325
	public float rotateSmoothTime = 0.2f;

	// Token: 0x0400052E RID: 1326
	public float smoothTime = 0.1f;
}
