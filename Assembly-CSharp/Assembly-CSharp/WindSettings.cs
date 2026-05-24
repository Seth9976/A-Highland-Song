using System;
using UnityEngine;

// Token: 0x02000167 RID: 359
public class WindSettings : ScriptableObject
{
	// Token: 0x04000E4C RID: 3660
	public WindSettings.WindOverride windOverrideMode;

	// Token: 0x04000E4D RID: 3661
	[Range(-1f, 1f)]
	public float windOverrideVelocity;

	// Token: 0x04000E4E RID: 3662
	[Space]
	public Texture2D windShaderEffectNoiseTexture;

	// Token: 0x04000E4F RID: 3663
	public float windChangeSpeed = 0.01f;

	// Token: 0x04000E50 RID: 3664
	public float baseWindMultiplier = 1f;

	// Token: 0x04000E51 RID: 3665
	public float peakMultiplier = 1.6f;

	// Token: 0x04000E52 RID: 3666
	public float ridgeMultiplier = 1.3f;

	// Token: 0x04000E53 RID: 3667
	public AnimationCurve windHeightMultiplier;

	// Token: 0x04000E54 RID: 3668
	public float strongWindSpeed = 0.6f;

	// Token: 0x04000E55 RID: 3669
	public float moderateWindSpeed = 0.35f;

	// Token: 0x0200039A RID: 922
	public enum WindOverride
	{
		// Token: 0x04001969 RID: 6505
		Off,
		// Token: 0x0400196A RID: 6506
		EditorOnly,
		// Token: 0x0400196B RID: 6507
		On
	}
}
