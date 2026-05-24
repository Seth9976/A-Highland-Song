using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001FF RID: 511
public static class BoundsX
{
	// Token: 0x060012C6 RID: 4806 RVA: 0x0008628C File Offset: 0x0008448C
	public static Bounds CreateEncapsulating(IList<Vector3> vectors)
	{
		if (vectors == null)
		{
			return new Bounds(Vector3.zero, Vector3.zero);
		}
		int count = vectors.Count;
		if (count == 0)
		{
			return new Bounds(Vector3.zero, Vector3.zero);
		}
		Vector3 vector = vectors[0];
		Vector3 vector2 = vectors[0];
		for (int i = 1; i < count; i++)
		{
			Vector3 vector3 = vectors[i];
			if (vector3.x < vector.x)
			{
				vector.x = vector3.x;
			}
			else if (vector3.x > vector2.x)
			{
				vector2.x = vector3.x;
			}
			if (vector3.y < vector.y)
			{
				vector.y = vector3.y;
			}
			else if (vector3.y > vector2.y)
			{
				vector2.y = vector3.y;
			}
			if (vector3.z < vector.z)
			{
				vector.z = vector3.z;
			}
			else if (vector3.z > vector2.z)
			{
				vector2.z = vector3.z;
			}
		}
		Vector3 vector4 = vector2 - vector;
		return new Bounds(vector + vector4 * 0.5f, vector4);
	}

	// Token: 0x060012C7 RID: 4807 RVA: 0x000863CC File Offset: 0x000845CC
	public static Vector3 ClosestPointOnPerimeter(this Bounds bounds, Vector3 point)
	{
		Vector3 min = bounds.min;
		Vector3 max = bounds.max;
		Vector3 center = bounds.center;
		Vector3 size = bounds.size;
		Bounds bounds2 = new Bounds(new Vector3(min.x, center.y, center.z), new Vector3(0f, size.y, size.z));
		Bounds bounds3 = new Bounds(new Vector3(max.x, center.y, center.z), new Vector3(0f, size.y, size.z));
		Bounds bounds4 = new Bounds(new Vector3(center.x, min.y, center.z), new Vector3(size.x, 0f, size.z));
		Bounds bounds5 = new Bounds(new Vector3(center.x, max.y, center.z), new Vector3(size.x, 0f, size.z));
		Bounds bounds6 = new Bounds(new Vector3(center.x, center.y, min.z), new Vector3(size.x, size.y, 0f));
		Bounds bounds7 = new Bounds(new Vector3(center.x, center.y, max.z), new Vector3(size.x, size.y, 0f));
		Vector3 vector = Vector3.zero;
		float num = float.MaxValue;
		Vector3 vector2 = bounds2.ClosestPoint(point);
		float num2 = Vector3.Distance(point, vector2);
		if (num2 < num)
		{
			num = num2;
			vector = vector2;
		}
		Vector3 vector3 = bounds3.ClosestPoint(point);
		float num3 = Vector3.Distance(point, vector3);
		if (num3 < num)
		{
			num = num3;
			vector = vector3;
		}
		Vector3 vector4 = bounds4.ClosestPoint(point);
		float num4 = Vector3.Distance(point, vector4);
		if (num4 < num)
		{
			num = num4;
			vector = vector4;
		}
		Vector3 vector5 = bounds5.ClosestPoint(point);
		float num5 = Vector3.Distance(point, vector5);
		if (num5 < num)
		{
			num = num5;
			vector = vector5;
		}
		Vector3 vector6 = bounds6.ClosestPoint(point);
		float num6 = Vector3.Distance(point, vector6);
		if (num6 < num)
		{
			num = num6;
			vector = vector6;
		}
		Vector3 vector7 = bounds7.ClosestPoint(point);
		float num7 = Vector3.Distance(point, vector7);
		if (num7 < num)
		{
			vector = vector7;
		}
		return vector;
	}

	// Token: 0x060012C8 RID: 4808 RVA: 0x0008660D File Offset: 0x0008480D
	public static float SignedDistanceFromPoint(this Bounds bounds, Vector3 position)
	{
		return (float)(bounds.Contains(position) ? 1 : (-1)) * Vector3.Distance(position, bounds.ClosestPointOnPerimeter(position));
	}
}
