using System;
using UnityEngine;

// Token: 0x020000E1 RID: 225
public class TorchSettings : ScriptableObject
{
	// Token: 0x04000958 RID: 2392
	public float batteryHours = 16f;

	// Token: 0x04000959 RID: 2393
	public float gameStartIntialBatteryHours = 4f;

	// Token: 0x0400095A RID: 2394
	public AnimationCurve alphaByBatteryLevel = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	// Token: 0x0400095B RID: 2395
	public float minimumScaleLowBattery = 0.5f;

	// Token: 0x0400095C RID: 2396
	public float flickerNoiseActiveScale = 0.5f;

	// Token: 0x0400095D RID: 2397
	public float flickerNoiseActiveThreshold = 0.5f;

	// Token: 0x0400095E RID: 2398
	public float flickerNoiseActiveOnScale = 10f;

	// Token: 0x0400095F RID: 2399
	public float flickerBatteryLevel = 0.2f;

	// Token: 0x04000960 RID: 2400
	public float flickeringAmplitudeOffset;
}
