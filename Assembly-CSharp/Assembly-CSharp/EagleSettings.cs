using System;
using UnityEngine;

// Token: 0x02000186 RID: 390
public class EagleSettings : ScriptableObject
{
	// Token: 0x04000F97 RID: 3991
	public float speed = 100f;

	// Token: 0x04000F98 RID: 3992
	public float splineTangentDist = 50f;

	// Token: 0x04000F99 RID: 3993
	public float arrivalRadius = 20f;

	// Token: 0x04000F9A RID: 3994
	public float catchSpeed = 20f;

	// Token: 0x04000F9B RID: 3995
	public float dropOffSpeed = 50f;

	// Token: 0x04000F9C RID: 3996
	public Vector2 catchOffset = new Vector2(1f, 4f);

	// Token: 0x04000F9D RID: 3997
	public Vector2 playerHangOffset = new Vector2(0f, -4f);

	// Token: 0x04000F9E RID: 3998
	public float slowMoTimeToArrival = 1f;

	// Token: 0x04000F9F RID: 3999
	public float slowMoScalar = 0.25f;

	// Token: 0x04000FA0 RID: 4000
	public float slowMoRecoverTime = 1f;

	// Token: 0x04000FA1 RID: 4001
	public float wingBeatSoundTime = 0.25f;
}
