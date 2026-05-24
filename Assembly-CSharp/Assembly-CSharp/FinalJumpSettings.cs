using System;
using UnityEngine;

// Token: 0x020000D4 RID: 212
public class FinalJumpSettings : ScriptableObject
{
	// Token: 0x0400089E RID: 2206
	public Vector2 jumpStartVelocity = new Vector2(20f, 20f);

	// Token: 0x0400089F RID: 2207
	public float gravity = -5f;

	// Token: 0x040008A0 RID: 2208
	public float xDamping = 0.9f;

	// Token: 0x040008A1 RID: 2209
	public float pauseBeforeLaunchTime = 0.1f;

	// Token: 0x040008A2 RID: 2210
	public float launchTimeScalar = 0.5f;

	// Token: 0x040008A3 RID: 2211
	public float flyTimeScalar = 0.1f;

	// Token: 0x040008A4 RID: 2212
	public float removeTimeScalarTime = 0.6f;

	// Token: 0x040008A5 RID: 2213
	public float leftRightToCancelHoldTime = 0.6f;

	// Token: 0x040008A6 RID: 2214
	public int runUpStartFrameIdx = 4;

	// Token: 0x040008A7 RID: 2215
	public int launchFrameIdx = 8;

	// Token: 0x040008A8 RID: 2216
	public Vector2 launchRunVelocity = Vector2.right;

	// Token: 0x040008A9 RID: 2217
	public float rotationOffsetStart = 30f;

	// Token: 0x040008AA RID: 2218
	public float rotationOffsetEnd = 10f;

	// Token: 0x040008AB RID: 2219
	public float rotationOffsetDuration = 0.3f;

	// Token: 0x040008AC RID: 2220
	public int firstPreRotatedFrameIdx = 29;

	// Token: 0x040008AD RID: 2221
	public Quaternion finalFramesRotationOffset;

	// Token: 0x040008AE RID: 2222
	public Vector3 finalFramesPositionOffset;
}
