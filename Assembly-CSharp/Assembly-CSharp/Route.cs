using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200003B RID: 59
[Serializable]
public class Route
{
	// Token: 0x040002A1 RID: 673
	public List<Vector2> points = new List<Vector2>();

	// Token: 0x040002A2 RID: 674
	public float z;

	// Token: 0x040002A3 RID: 675
	public float flattenedZ;

	// Token: 0x040002A4 RID: 676
	public bool isSlide;

	// Token: 0x040002A5 RID: 677
	public Slope sourceSlope;

	// Token: 0x040002A6 RID: 678
	public Poly sourcePoly;
}
