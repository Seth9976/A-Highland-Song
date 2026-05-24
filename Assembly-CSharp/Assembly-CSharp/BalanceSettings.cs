using System;
using UnityEngine;

// Token: 0x020000CF RID: 207
public class BalanceSettings : ScriptableObject
{
	// Token: 0x04000847 RID: 2119
	public float balanceMaxTimeMusic = 0.3f;

	// Token: 0x04000848 RID: 2120
	public float balanceMaxTime = 3f;

	// Token: 0x04000849 RID: 2121
	public Range balanceDropGainAngleRange = new Range(-40f, 40f);

	// Token: 0x0400084A RID: 2122
	public float balancePointMinDist = 0.5f;

	// Token: 0x0400084B RID: 2123
	public float balanceExtraDistMargin = 2f;

	// Token: 0x0400084C RID: 2124
	public float balanceDistDurationScalar = 0.7f;

	// Token: 0x0400084D RID: 2125
	public float maxRadiusOvalYScale = 0.7f;

	// Token: 0x0400084E RID: 2126
	public Range balanceWallClimbYRange = new Range(1f, 10f);

	// Token: 0x0400084F RID: 2127
	public Range balanceWallClimbXRange = new Range(0f, 5f);

	// Token: 0x04000850 RID: 2128
	public float balanceWallClimbDistDurationScalar = 0.3f;

	// Token: 0x04000851 RID: 2129
	public Vector2 fallVelocity = new Vector2(10f, 0f);
}
