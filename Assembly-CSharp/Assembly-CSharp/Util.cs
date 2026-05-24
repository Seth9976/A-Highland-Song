using System;
using UnityEngine;

// Token: 0x020000E2 RID: 226
public static class Util
{
	// Token: 0x06000795 RID: 1941 RVA: 0x00044AFB File Offset: 0x00042CFB
	public static float ClimbAngleFromSurfaceNormal(Vector2 normal, float lookDir)
	{
		if (lookDir < 0f)
		{
			normal.x *= -1f;
		}
		return -Mathf.Atan2(normal.x, normal.y) * 57.29578f;
	}

	// Token: 0x06000796 RID: 1942 RVA: 0x00044B30 File Offset: 0x00042D30
	public static float ClimbAngleFromSurfacePolyEdge(Vector2 edge)
	{
		Vector2 normalized = new Vector2(-edge.y, edge.x).normalized;
		return Mathf.Abs(Mathf.Atan2(normalized.x, normalized.y) * 57.29578f);
	}
}
