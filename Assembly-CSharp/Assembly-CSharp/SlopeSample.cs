using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020001AB RID: 427
public struct SlopeSample
{
	// Token: 0x17000360 RID: 864
	// (get) Token: 0x06000E22 RID: 3618 RVA: 0x0006F42B File Offset: 0x0006D62B
	public Vector3 point3d
	{
		get
		{
			return new Vector3(this.point.x, this.point.y, this.depth);
		}
	}

	// Token: 0x17000361 RID: 865
	// (get) Token: 0x06000E23 RID: 3619 RVA: 0x0006F44E File Offset: 0x0006D64E
	public bool isValid
	{
		get
		{
			return this.slope != null;
		}
	}

	// Token: 0x040010F0 RID: 4336
	public Vector2 point;

	// Token: 0x040010F1 RID: 4337
	public Vector2 clampedPoint;

	// Token: 0x040010F2 RID: 4338
	public float depth;

	// Token: 0x040010F3 RID: 4339
	public Vector2 normal;

	// Token: 0x040010F4 RID: 4340
	public float angle;

	// Token: 0x040010F5 RID: 4341
	[Nullable(1)]
	public Slope slope;

	// Token: 0x040010F6 RID: 4342
	public int i0;

	// Token: 0x040010F7 RID: 4343
	public int i1;

	// Token: 0x040010F8 RID: 4344
	public float t;

	// Token: 0x040010F9 RID: 4345
	public bool outOfRange;
}
