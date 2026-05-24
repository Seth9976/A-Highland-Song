using System;
using UnityEngine;

// Token: 0x0200006B RID: 107
public struct Alignment
{
	// Token: 0x170000CC RID: 204
	// (get) Token: 0x060002FC RID: 764 RVA: 0x000187BB File Offset: 0x000169BB
	public Vector2 vector
	{
		get
		{
			return this.right - this.left;
		}
	}

	// Token: 0x04000435 RID: 1077
	public Vector2 left;

	// Token: 0x04000436 RID: 1078
	public Vector2 right;
}
