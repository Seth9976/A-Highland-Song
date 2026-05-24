using System;
using UnityEngine;

// Token: 0x0200001F RID: 31
public class CaughtCameraSettings : ScriptableObject
{
	// Token: 0x0400010C RID: 268
	public float startDistance = 20f;

	// Token: 0x0400010D RID: 269
	public float zoomedOutDistance = 400f;

	// Token: 0x0400010E RID: 270
	public float catchDistance = 50f;

	// Token: 0x0400010F RID: 271
	public float eagleToPlayerLerpTime = 0.5f;

	// Token: 0x04000110 RID: 272
	public float ealgeToPlayerInitialPause = 0.5f;

	// Token: 0x04000111 RID: 273
	public Range approachDistRange = new Range(10f, 100f);

	// Token: 0x04000112 RID: 274
	public float zoomedOutForCarryDistance = 1000f;

	// Token: 0x04000113 RID: 275
	public float flyDistForZoomOutAfterPickup = 300f;

	// Token: 0x04000114 RID: 276
	public float flyDistForZoomInBeforeDropOff = 1500f;

	// Token: 0x04000115 RID: 277
	public float zoomedInForDropOffDistance = 50f;

	// Token: 0x04000116 RID: 278
	public float zoomBackInCurvePower = 2f;

	// Token: 0x04000117 RID: 279
	public Range cutToZoomedInDuringFlightTimeRange = new Range(5f, 15f);
}
