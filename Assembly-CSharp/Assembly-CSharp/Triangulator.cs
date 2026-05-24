using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001F8 RID: 504
public static class Triangulator
{
	// Token: 0x060012A7 RID: 4775 RVA: 0x000859CC File Offset: 0x00083BCC
	public static void GenerateIndices(IList<Vector2> points, List<int> outputIndices)
	{
		int count = points.Count;
		if (count < 3)
		{
			return;
		}
		Triangulator._indicesScratch.Clear();
		if (Triangulator.SignedArea(points) > 0f)
		{
			for (int i = 0; i < count; i++)
			{
				Triangulator._indicesScratch.Add(i);
			}
		}
		else
		{
			for (int j = 0; j < count; j++)
			{
				Triangulator._indicesScratch.Add(count - 1 - j);
			}
		}
		int k = count;
		int num = 2 * k;
		int num2 = 0;
		int num3 = k - 1;
		while (k > 2)
		{
			if (num-- <= 0)
			{
				return;
			}
			int num4 = num3;
			if (k <= num4)
			{
				num4 = 0;
			}
			num3 = num4 + 1;
			if (k <= num3)
			{
				num3 = 0;
			}
			int num5 = num3 + 1;
			if (k <= num5)
			{
				num5 = 0;
			}
			if (Triangulator.Snip(points, num4, num3, num5, k))
			{
				int num6 = Triangulator._indicesScratch[num4];
				int num7 = Triangulator._indicesScratch[num3];
				int num8 = Triangulator._indicesScratch[num5];
				outputIndices.Add(num6);
				outputIndices.Add(num7);
				outputIndices.Add(num8);
				num2++;
				int num9 = num3;
				for (int l = num3 + 1; l < k; l++)
				{
					Triangulator._indicesScratch[num9] = Triangulator._indicesScratch[l];
					num9++;
				}
				k--;
				num = 2 * k;
			}
		}
		outputIndices.Reverse();
	}

	// Token: 0x060012A8 RID: 4776 RVA: 0x00085B20 File Offset: 0x00083D20
	public static void GenerateIndices(IList<Vector3> points, List<int> outputIndices)
	{
		int count = points.Count;
		if (count < 3)
		{
			return;
		}
		Triangulator._indicesScratch.Clear();
		if (Triangulator.SignedArea(points) > 0f)
		{
			for (int i = 0; i < count; i++)
			{
				Triangulator._indicesScratch.Add(i);
			}
		}
		else
		{
			for (int j = 0; j < count; j++)
			{
				Triangulator._indicesScratch.Add(count - 1 - j);
			}
		}
		int k = count;
		int num = 2 * k;
		int num2 = 0;
		int num3 = k - 1;
		while (k > 2)
		{
			if (num-- <= 0)
			{
				return;
			}
			int num4 = num3;
			if (k <= num4)
			{
				num4 = 0;
			}
			num3 = num4 + 1;
			if (k <= num3)
			{
				num3 = 0;
			}
			int num5 = num3 + 1;
			if (k <= num5)
			{
				num5 = 0;
			}
			if (Triangulator.Snip(points, num4, num3, num5, k))
			{
				int num6 = Triangulator._indicesScratch[num4];
				int num7 = Triangulator._indicesScratch[num3];
				int num8 = Triangulator._indicesScratch[num5];
				outputIndices.Add(num6);
				outputIndices.Add(num7);
				outputIndices.Add(num8);
				num2++;
				int num9 = num3;
				for (int l = num3 + 1; l < k; l++)
				{
					Triangulator._indicesScratch[num9] = Triangulator._indicesScratch[l];
					num9++;
				}
				k--;
				num = 2 * k;
			}
		}
		outputIndices.Reverse();
	}

	// Token: 0x060012A9 RID: 4777 RVA: 0x00085C74 File Offset: 0x00083E74
	public static float SignedArea(IList<Vector3> points)
	{
		int count = points.Count;
		float num = 0f;
		int num2 = count - 1;
		int i = 0;
		while (i < count)
		{
			Vector2 vector = points[num2];
			Vector2 vector2 = points[i];
			num += vector.x * vector2.y - vector2.x * vector.y;
			num2 = i++;
		}
		return num * 0.5f;
	}

	// Token: 0x060012AA RID: 4778 RVA: 0x00085CE5 File Offset: 0x00083EE5
	public static float Area(IList<Vector3> points)
	{
		return Mathf.Abs(Triangulator.SignedArea(points));
	}

	// Token: 0x060012AB RID: 4779 RVA: 0x00085CF4 File Offset: 0x00083EF4
	public static float SignedArea(IList<Vector2> points)
	{
		int count = points.Count;
		float num = 0f;
		int num2 = count - 1;
		int i = 0;
		while (i < count)
		{
			Vector2 vector = points[num2];
			Vector2 vector2 = points[i];
			num += vector.x * vector2.y - vector2.x * vector.y;
			num2 = i++;
		}
		return num * 0.5f;
	}

	// Token: 0x060012AC RID: 4780 RVA: 0x00085D5B File Offset: 0x00083F5B
	public static float Area(IList<Vector2> points)
	{
		return Mathf.Abs(Triangulator.SignedArea(points));
	}

	// Token: 0x060012AD RID: 4781 RVA: 0x00085D68 File Offset: 0x00083F68
	private static bool Snip(IList<Vector2> points, int u, int v, int w, int n)
	{
		Vector2 vector = points[Triangulator._indicesScratch[u]];
		Vector2 vector2 = points[Triangulator._indicesScratch[v]];
		Vector2 vector3 = points[Triangulator._indicesScratch[w]];
		if (Mathf.Epsilon > (vector2.x - vector.x) * (vector3.y - vector.y) - (vector2.y - vector.y) * (vector3.x - vector.x))
		{
			return false;
		}
		for (int i = 0; i < n; i++)
		{
			if (i != u && i != v && i != w)
			{
				Vector2 vector4 = points[Triangulator._indicesScratch[i]];
				if (Triangulator.InsideTriangle(vector, vector2, vector3, vector4))
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x060012AE RID: 4782 RVA: 0x00085E28 File Offset: 0x00084028
	private static bool Snip(IList<Vector3> points, int u, int v, int w, int n)
	{
		Vector2 vector = points[Triangulator._indicesScratch[u]];
		Vector2 vector2 = points[Triangulator._indicesScratch[v]];
		Vector2 vector3 = points[Triangulator._indicesScratch[w]];
		if (Mathf.Epsilon > (vector2.x - vector.x) * (vector3.y - vector.y) - (vector2.y - vector.y) * (vector3.x - vector.x))
		{
			return false;
		}
		for (int i = 0; i < n; i++)
		{
			if (i != u && i != v && i != w)
			{
				Vector2 vector4 = points[Triangulator._indicesScratch[i]];
				if (Triangulator.InsideTriangle(vector, vector2, vector3, vector4))
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x060012AF RID: 4783 RVA: 0x00085EFC File Offset: 0x000840FC
	private static bool InsideTriangle(Vector2 A, Vector2 B, Vector2 C, Vector2 P)
	{
		float num = C.x - B.x;
		float num2 = C.y - B.y;
		float num3 = A.x - C.x;
		float num4 = A.y - C.y;
		float num5 = B.x - A.x;
		float num6 = B.y - A.y;
		float num7 = P.x - A.x;
		float num8 = P.y - A.y;
		float num9 = P.x - B.x;
		float num10 = P.y - B.y;
		float num11 = P.x - C.x;
		float num12 = P.y - C.y;
		float num13 = num * num10 - num2 * num9;
		float num14 = num5 * num8 - num6 * num7;
		float num15 = num3 * num12 - num4 * num11;
		return num13 >= 0f && num15 >= 0f && num14 >= 0f;
	}

	// Token: 0x0400129E RID: 4766
	private static List<int> _indicesScratch = new List<int>(256);
}
