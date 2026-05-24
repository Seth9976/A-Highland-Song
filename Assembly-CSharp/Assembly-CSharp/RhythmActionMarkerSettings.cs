using System;
using UnityEngine;

// Token: 0x0200007E RID: 126
public class RhythmActionMarkerSettings : ScriptableObject
{
	// Token: 0x040004C4 RID: 1220
	public Sprite baseSprite;

	// Token: 0x040004C5 RID: 1221
	public Sprite specialBaseSprite;

	// Token: 0x040004C6 RID: 1222
	public Color beamColor = Color.white;

	// Token: 0x040004C7 RID: 1223
	public Color specialBeamColor = Color.white;

	// Token: 0x040004C8 RID: 1224
	public Vector3 overallScale = new Vector3(2.5f, 2.5f, 2.5f);

	// Token: 0x040004C9 RID: 1225
	public Vector3 baseSuccessScale = new Vector3(1f, 1f, 1f);

	// Token: 0x040004CA RID: 1226
	public Vector3 beamSuccessScale = new Vector3(1f, 1f, 1f);

	// Token: 0x040004CB RID: 1227
	public Vector3 failScale = new Vector3(0.5f, 0.5f, 0.5f);

	// Token: 0x040004CC RID: 1228
	public float beamSuccessYPos = 2f;

	// Token: 0x040004CD RID: 1229
	public float standardAlpha = 1f;

	// Token: 0x040004CE RID: 1230
	public float successAlpha = 1f;

	// Token: 0x040004CF RID: 1231
	public float successTransitionTime = 0.3f;

	// Token: 0x040004D0 RID: 1232
	public float failTransitionTime = 1f;

	// Token: 0x040004D1 RID: 1233
	public float downwardClunkDist = 0.5f;

	// Token: 0x040004D2 RID: 1234
	public Range beamFadeInDist = new Range(5f, 20f);

	// Token: 0x040004D3 RID: 1235
	public Color flashColor = Color.white;

	// Token: 0x040004D4 RID: 1236
	public float flashDuration = 0.2f;

	// Token: 0x040004D5 RID: 1237
	public AnimationCurve flashScale = AnimationCurve.Linear(0f, 20f, 1f, 100f);

	// Token: 0x040004D6 RID: 1238
	public AnimationCurve flashAlpha = AnimationCurve.Linear(0f, 1f, 1f, 0f);
}
