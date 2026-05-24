using System;
using UnityEngine;

// Token: 0x0200020D RID: 525
public static class RectX
{
	// Token: 0x06001362 RID: 4962 RVA: 0x0008895C File Offset: 0x00086B5C
	public static Rect Inset(this Rect rect, float pixels)
	{
		return new Rect(rect.x + pixels, rect.y + pixels, rect.width - 2f * pixels, rect.height - 2f * pixels);
	}

	// Token: 0x06001363 RID: 4963 RVA: 0x00088993 File Offset: 0x00086B93
	public static Rect Expand(this Rect rect, float pixels)
	{
		return new Rect(rect.x - pixels, rect.y - pixels, rect.width + 2f * pixels, rect.height + 2f * pixels);
	}

	// Token: 0x06001364 RID: 4964 RVA: 0x000889CA File Offset: 0x00086BCA
	public static Rect CreateFromCenter(Vector2 centerPosition, Vector2 size)
	{
		return RectX.CreateFromCenter(centerPosition.x, centerPosition.y, size.x, size.y);
	}

	// Token: 0x06001365 RID: 4965 RVA: 0x000889E9 File Offset: 0x00086BE9
	public static Rect CreateFromCenter(float centerX, float centerY, float sizeX, float sizeY)
	{
		return new Rect(centerX - sizeX * 0.5f, centerY - sizeY * 0.5f, sizeX, sizeY);
	}

	// Token: 0x06001366 RID: 4966 RVA: 0x00088A04 File Offset: 0x00086C04
	public static bool Intersects(Rect r1, Rect r2)
	{
		return r1.xMax >= r2.x && r1.x <= r2.xMax && r1.yMax >= r2.y && r1.y <= r2.yMax;
	}

	// Token: 0x06001367 RID: 4967 RVA: 0x00088A58 File Offset: 0x00086C58
	public static Rect Intersect(Rect r1, Rect r2)
	{
		if (r1.xMax < r2.xMin || r1.xMin > r2.xMax || r1.yMax < r2.yMin || r1.yMin > r2.yMax)
		{
			return Rect.zero;
		}
		float num = Mathf.Max(r1.xMin, r2.xMin);
		float num2 = Mathf.Min(r1.xMax, r2.xMax);
		float num3 = Mathf.Max(r1.yMin, r2.yMin);
		float num4 = Mathf.Min(r1.yMax, r2.yMax);
		return Rect.MinMaxRect(num, num3, num2, num4);
	}

	// Token: 0x06001368 RID: 4968 RVA: 0x00088B04 File Offset: 0x00086D04
	public static Vector2 SplatVector(Vector2 rectSize, Vector2 vector)
	{
		if (vector == Vector2.zero || rectSize.x == 0f || rectSize.y == 0f)
		{
			return Vector2.zero;
		}
		float num = Mathf.Abs(vector.x / vector.y);
		float num2 = rectSize.x / rectSize.y;
		float num3;
		if (num > num2)
		{
			num3 = Mathf.Abs(0.5f * rectSize.x / vector.x);
		}
		else
		{
			num3 = Mathf.Abs(0.5f * rectSize.y / vector.y);
		}
		return num3 * vector;
	}

	// Token: 0x06001369 RID: 4969 RVA: 0x00088B9C File Offset: 0x00086D9C
	public static Vector2 GetNearestPointInPerimeter(this Rect r, Vector2 point)
	{
		point.x = Mathf.Clamp(point.x, r.xMin, r.xMax);
		point.y = Mathf.Clamp(point.y, r.yMin, r.yMax);
		float num = Mathf.Abs(point.x - r.xMin);
		float num2 = Mathf.Abs(point.x - r.xMax);
		float num3 = Mathf.Abs(point.y - r.yMin);
		float num4 = Mathf.Abs(point.y - r.yMax);
		float num5 = Mathf.Min(new float[] { num, num2, num3, num4 });
		if (num5 == num3)
		{
			return new Vector2(point.x, r.yMin);
		}
		if (num5 == num4)
		{
			return new Vector2(point.x, r.yMax);
		}
		if (num5 == num)
		{
			return new Vector2(r.xMin, point.y);
		}
		return new Vector2(r.xMax, point.y);
	}
}
