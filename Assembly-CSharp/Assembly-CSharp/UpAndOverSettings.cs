using System;
using UnityEngine;

// Token: 0x020000DB RID: 219
public class UpAndOverSettings : ScriptableObject
{
	// Token: 0x04000930 RID: 2352
	[Header("From standing")]
	public Range xRangeGround = new Range(2f, 5f);

	// Token: 0x04000931 RID: 2353
	public Range yRangeGround = new Range(0.1f, 6f);

	// Token: 0x04000932 RID: 2354
	[Header("From running/sliding, automatic climb")]
	public Range xRangeGroundAuto = new Range(2f, 5f);

	// Token: 0x04000933 RID: 2355
	public Range yRangeGroundAuto = new Range(0.1f, 6f);

	// Token: 0x04000934 RID: 2356
	[Header("From climb")]
	public Range xRangeClimb = new Range(1f, 2f);

	// Token: 0x04000935 RID: 2357
	public Range yRangeClimb = new Range(-1f, 2f);

	// Token: 0x04000936 RID: 2358
	[Header("While jumping/falling")]
	public Range xRangeJumpFall = new Range(1f, 2f);

	// Token: 0x04000937 RID: 2359
	public Range yRangeJumpFall = new Range(-1f, 2f);

	// Token: 0x04000938 RID: 2360
	public float xRangeBackScalar = 0.7f;

	// Token: 0x04000939 RID: 2361
	public float autoClimbMaxHeight = 3f;
}
