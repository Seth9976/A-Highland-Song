using System;
using UnityEngine;

// Token: 0x020001F3 RID: 499
public static class Bezier
{
	// Token: 0x06001296 RID: 4758 RVA: 0x0008530C File Offset: 0x0008350C
	public static Vector2 GetPoint(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
	{
		if (t <= 0f)
		{
			return p0;
		}
		if (t >= 1f)
		{
			return p3;
		}
		float num = t * t;
		float num2 = 1f - t;
		float num3 = num2 * num2;
		float num4 = num3 * num2;
		float num5 = 3f * num3 * t;
		float num6 = 3f * num2 * num;
		float num7 = num * t;
		return new Vector2(num4 * p0.x + num5 * p1.x + num6 * p2.x + num7 * p3.x, num4 * p0.y + num5 * p1.y + num6 * p2.y + num7 * p3.y);
	}

	// Token: 0x06001297 RID: 4759 RVA: 0x000853B4 File Offset: 0x000835B4
	public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
	{
		if (t <= 0f)
		{
			return p0;
		}
		if (t >= 1f)
		{
			return p3;
		}
		float num = t * t;
		float num2 = 1f - t;
		float num3 = num2 * num2;
		float num4 = num3 * num2;
		float num5 = 3f * num3 * t;
		float num6 = 3f * num2 * num;
		float num7 = num * t;
		return new Vector3(num4 * p0.x + num5 * p1.x + num6 * p2.x + num7 * p3.x, num4 * p0.y + num5 * p1.y + num6 * p2.y + num7 * p3.y, num4 * p0.z + num5 * p1.z + num6 * p2.z + num7 * p3.z);
	}

	// Token: 0x06001298 RID: 4760 RVA: 0x00085484 File Offset: 0x00083684
	public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
	{
		t = Mathf.Clamp01(t);
		float num = 1f - t;
		float num2 = 3f * num * num;
		float num3 = 6f * num * t;
		float num4 = 3f * t * t;
		return new Vector3(num2 * (p1.x - p0.x) + num3 * (p2.x - p1.x) + num4 * (p3.x - p2.x), num2 * (p1.y - p0.y) + num3 * (p2.y - p1.y) + num4 * (p3.y - p2.y), num2 * (p1.z - p0.z) + num3 * (p2.z - p1.z) + num4 * (p3.z - p2.z));
	}

	// Token: 0x06001299 RID: 4761 RVA: 0x00085558 File Offset: 0x00083758
	public static float GetRoughLength(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
	{
		float magnitude = (p3 - p0).magnitude;
		return ((p0 - p1).magnitude + (p2 - p1).magnitude + (p3 - p2).magnitude + magnitude) / 2f;
	}
}
