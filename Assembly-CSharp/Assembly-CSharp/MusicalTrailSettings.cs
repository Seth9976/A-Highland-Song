using System;
using UnityEngine;

// Token: 0x020000D7 RID: 215
public class MusicalTrailSettings : ScriptableObject
{
	// Token: 0x040008CC RID: 2252
	public float trailWavesAmplitude = 1f;

	// Token: 0x040008CD RID: 2253
	public float trailWavesFrequency = 1f;

	// Token: 0x040008CE RID: 2254
	public Range alphaRange = new Range(0.5f, 1f);

	// Token: 0x040008CF RID: 2255
	public float alphaChangeFrequency = 1f;

	// Token: 0x040008D0 RID: 2256
	public float noiseContrast = 2f;

	// Token: 0x040008D1 RID: 2257
	public float noiseMid = 0.7f;

	// Token: 0x040008D2 RID: 2258
	public AnimationCurve successNoteScaleOverTime = AnimationCurve.Constant(0f, 1f, 1f);

	// Token: 0x040008D3 RID: 2259
	public Range successNoteAngle = new Range(0f, 30f);

	// Token: 0x040008D4 RID: 2260
	public int successPoofParticleCount = 10;

	// Token: 0x040008D5 RID: 2261
	public Color successNoteNailedColor = Color.yellow;

	// Token: 0x040008D6 RID: 2262
	public Color successNoteNailedSpecialColor = Color.yellow;

	// Token: 0x040008D7 RID: 2263
	public Color successNoteColor = Color.white;

	// Token: 0x040008D8 RID: 2264
	public Color successNoteSpecialColor = Color.white;
}
