using System;
using UnityEngine;

// Token: 0x020001A7 RID: 423
public class SkiLiftSettings : ScriptableObject
{
	// Token: 0x040010CF RID: 4303
	public float linearSpeed = 10f;

	// Token: 0x040010D0 RID: 4304
	public float acceleration = 1f;

	// Token: 0x040010D1 RID: 4305
	public int chairsPerLine = 6;

	// Token: 0x040010D2 RID: 4306
	public float fadeStartDist = 10f;

	// Token: 0x040010D3 RID: 4307
	[Range(0f, 1f)]
	public float startOffset = 0.7f;

	// Token: 0x040010D4 RID: 4308
	public float maxJumpX = 15f;

	// Token: 0x040010D5 RID: 4309
	public float maxJumpY = 10f;

	// Token: 0x040010D6 RID: 4310
	public Range chairsMaxSwingAngle = new Range(20f, 40f);

	// Token: 0x040010D7 RID: 4311
	public float swingDamping = 0.98f;

	// Token: 0x040010D8 RID: 4312
	public float swingSpeed = 2f;
}
