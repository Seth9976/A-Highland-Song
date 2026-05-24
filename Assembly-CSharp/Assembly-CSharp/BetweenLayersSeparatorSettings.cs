using System;
using UnityEngine;

// Token: 0x0200016C RID: 364
public class BetweenLayersSeparatorSettings : ScriptableObject
{
	// Token: 0x04000E6A RID: 3690
	public float separatorZOffset = 1.5f;

	// Token: 0x04000E6B RID: 3691
	public float separatorCrossFadeDuration = 2f;

	// Token: 0x04000E6C RID: 3692
	public Color separatorColor = Color.white.WithAlpha(0.5f);

	// Token: 0x04000E6D RID: 3693
	public float lerpSpeed2d = 0.01f;

	// Token: 0x04000E6E RID: 3694
	public float minCameraDistance = 50f;

	// Token: 0x04000E6F RID: 3695
	public float maxCameraDistance = 100f;
}
