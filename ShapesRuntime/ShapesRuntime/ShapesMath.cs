using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000061 RID: 97
	public static class ShapesMath
	{
		// Token: 0x06000A3D RID: 2621 RVA: 0x0002510A File Offset: 0x0002330A
		public static float Frac(float x)
		{
			return x - Mathf.Floor(x);
		}

		// Token: 0x06000A3E RID: 2622 RVA: 0x00025114 File Offset: 0x00023314
		public static float Eerp(float a, float b, float t)
		{
			return Mathf.Pow(a, 1f - t) * Mathf.Pow(b, t);
		}

		// Token: 0x06000A3F RID: 2623 RVA: 0x0002512B File Offset: 0x0002332B
		public static float SmoothCos01(float x)
		{
			return Mathf.Cos(x * 3.1415927f) * -0.5f + 0.5f;
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x00025145 File Offset: 0x00023345
		public static Vector2 AngToDir(float angRad)
		{
			return new Vector2(Mathf.Cos(angRad), Mathf.Sin(angRad));
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x00025158 File Offset: 0x00023358
		public static float DirToAng(Vector2 dir)
		{
			return Mathf.Atan2(dir.y, dir.x);
		}

		// Token: 0x06000A42 RID: 2626 RVA: 0x0002516B File Offset: 0x0002336B
		public static Vector2 Rotate90CW(Vector2 v)
		{
			return new Vector2(v.y, -v.x);
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x0002517F File Offset: 0x0002337F
		public static Vector2 Rotate90CCW(Vector2 v)
		{
			return new Vector2(-v.y, v.x);
		}

		// Token: 0x06000A44 RID: 2628 RVA: 0x00025194 File Offset: 0x00023394
		public static Vector4 AtLeast0(Vector4 v)
		{
			return new Vector4(Mathf.Max(0f, v.x), Mathf.Max(0f, v.y), Mathf.Max(0f, v.z), Mathf.Max(0f, v.w));
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x000251E6 File Offset: 0x000233E6
		public static float MaxComp(Vector4 v)
		{
			return Mathf.Max(Mathf.Max(Mathf.Max(v.y, v.x), v.z), v.w);
		}

		// Token: 0x06000A46 RID: 2630 RVA: 0x0002520F File Offset: 0x0002340F
		public static bool HasNegativeValues(Vector4 v)
		{
			return v.x < 0f || v.y < 0f || v.z < 0f || v.w < 0f;
		}

		// Token: 0x06000A47 RID: 2631 RVA: 0x00025247 File Offset: 0x00023447
		public static float Determinant(Vector2 a, Vector2 b)
		{
			return a.x * b.y - a.y * b.x;
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x00025264 File Offset: 0x00023464
		public static float Luminance(Color c)
		{
			return c.r * 0.2126f + c.g * 0.7152f + c.b * 0.0722f;
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x0002528C File Offset: 0x0002348C
		public static bool PointInsideTriangle(Vector2 a, Vector2 b, Vector2 c, Vector2 point, float aMargin = 0f, float bMargin = 0f, float cMargin = 0f)
		{
			float num = ShapesMath.Determinant(b - a, point - a);
			float num2 = ShapesMath.Determinant(c - b, point - b);
			float num3 = ShapesMath.Determinant(a - c, point - c);
			bool flag = num < cMargin;
			bool flag2 = num2 < aMargin;
			bool flag3 = num3 < bMargin;
			return flag == flag2 && flag2 == flag3;
		}

		// Token: 0x06000A4A RID: 2634 RVA: 0x000252F0 File Offset: 0x000234F0
		public static float PolygonSignedArea(List<Vector2> pts)
		{
			int count = pts.Count;
			float num = 0f;
			for (int i = 0; i < count; i++)
			{
				Vector2 vector = pts[i];
				Vector2 vector2 = pts[(i + 1) % count];
				num += (vector2.x - vector.x) * (vector2.y + vector.y);
			}
			return num;
		}

		// Token: 0x06000A4B RID: 2635 RVA: 0x0002534C File Offset: 0x0002354C
		public static Vector2 Rotate(Vector2 v, float angRad)
		{
			float num = Mathf.Cos(angRad);
			float num2 = Mathf.Sin(angRad);
			return new Vector2(num * v.x - num2 * v.y, num2 * v.x + num * v.y);
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x0002538E File Offset: 0x0002358E
		private static float DeltaAngleRad(float a, float b)
		{
			return Mathf.Repeat(b - a + 3.1415927f, 6.2831855f) - 3.1415927f;
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x000253AC File Offset: 0x000235AC
		public static float InverseLerpAngleRad(float a, float b, float v)
		{
			float num = ShapesMath.DeltaAngleRad(a, b);
			b = a + num;
			float num2 = a + num * 0.5f;
			v = num2 + ShapesMath.DeltaAngleRad(num2, v);
			return Mathf.InverseLerp(a, b, v);
		}

		// Token: 0x06000A4E RID: 2638 RVA: 0x000253E1 File Offset: 0x000235E1
		private static Vector2 Lerp(Vector2 a, Vector2 b, Vector2 t)
		{
			return new Vector2(Mathf.Lerp(a.x, b.x, t.x), Mathf.Lerp(a.y, b.y, t.y));
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x00025416 File Offset: 0x00023616
		private static Vector2 InverseLerp(Vector2 a, Vector2 b, Vector2 v)
		{
			return (v - a) / (b - a);
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x0002542B File Offset: 0x0002362B
		private static Vector2 Remap(Vector2 iMin, Vector2 iMax, Vector2 oMin, Vector2 oMax, Vector2 value)
		{
			return ShapesMath.Lerp(oMin, oMax, ShapesMath.InverseLerp(iMin, iMax, value));
		}

		// Token: 0x06000A51 RID: 2641 RVA: 0x0002543D File Offset: 0x0002363D
		public static Vector2 Remap(Rect iRect, Rect oRect, Vector2 iPos)
		{
			return ShapesMath.Remap(iRect.min, iRect.max, oRect.min, oRect.max, iPos);
		}

		// Token: 0x06000A52 RID: 2642 RVA: 0x00025461 File Offset: 0x00023661
		public static Vector3 Abs(Vector3 v)
		{
			return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
		}

		// Token: 0x06000A53 RID: 2643 RVA: 0x0002548C File Offset: 0x0002368C
		public static float RandomGaussian(float min = 0f, float max = 1f)
		{
			float num;
			float num3;
			do
			{
				num = 2f * Random.value - 1f;
				float num2 = 2f * Random.value - 1f;
				num3 = num * num + num2 * num2;
			}
			while (num3 >= 1f);
			float num4 = num * Mathf.Sqrt(-2f * Mathf.Log(num3) / num3);
			float num5 = (min + max) / 2f;
			float num6 = (max - num5) / 3f;
			return Mathf.Clamp(num4 * num6 + num5, min, max);
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x00025504 File Offset: 0x00023704
		public static Vector3 GetRandomPerpendicularVector(Vector3 a)
		{
			Vector3 onUnitSphere;
			do
			{
				onUnitSphere = Random.onUnitSphere;
			}
			while (Mathf.Abs(Vector3.Dot(a, onUnitSphere)) > 0.98f);
			return onUnitSphere;
		}

		// Token: 0x06000A55 RID: 2645 RVA: 0x0002552B File Offset: 0x0002372B
		public static IEnumerable<Vector3> GetArcPoints(Vector3 normA, Vector3 normB, Vector3 center, float radius, int count)
		{
			ShapesMath.<>c__DisplayClass25_0 CS$<>8__locals1;
			CS$<>8__locals1.center = center;
			CS$<>8__locals1.radius = radius;
			count = Mathf.Max(2, count);
			yield return ShapesMath.<GetArcPoints>g__DirToPt|25_0(normA, ref CS$<>8__locals1);
			int num2;
			for (int i = 1; i < count - 1; i = num2 + 1)
			{
				float num = (float)i / ((float)count - 1f);
				yield return ShapesMath.<GetArcPoints>g__DirToPt|25_0(Vector3.Slerp(normA, normB, num), ref CS$<>8__locals1);
				num2 = i;
			}
			yield return ShapesMath.<GetArcPoints>g__DirToPt|25_0(normB, ref CS$<>8__locals1);
			yield break;
		}

		// Token: 0x06000A56 RID: 2646 RVA: 0x00025558 File Offset: 0x00023758
		public static IEnumerable<Vector2> GetArcPoints(Vector2 normA, Vector2 normB, Vector2 center, float radius, int count)
		{
			ShapesMath.<>c__DisplayClass26_0 CS$<>8__locals1;
			CS$<>8__locals1.center = center;
			CS$<>8__locals1.radius = radius;
			count = Mathf.Max(2, count);
			yield return ShapesMath.<GetArcPoints>g__DirToPt|26_0(normA, ref CS$<>8__locals1);
			int num2;
			for (int i = 1; i < count - 1; i = num2 + 1)
			{
				float num = (float)i / ((float)count - 1f);
				yield return ShapesMath.<GetArcPoints>g__DirToPt|26_0(Vector3.Slerp(normA, normB, num), ref CS$<>8__locals1);
				num2 = i;
			}
			yield return ShapesMath.<GetArcPoints>g__DirToPt|26_0(normB, ref CS$<>8__locals1);
			yield break;
		}

		// Token: 0x06000A57 RID: 2647 RVA: 0x00025585 File Offset: 0x00023785
		public static IEnumerable<Vector3> CubicBezierPointsSkipFirst(Vector3 a, Vector3 b, Vector3 c, Vector3 d, int count)
		{
			int num2;
			for (int i = 1; i < count - 1; i = num2 + 1)
			{
				float num = (float)i / ((float)count - 1f);
				yield return ShapesMath.CubicBezier(a, b, c, d, num);
				num2 = i;
			}
			yield return d;
			yield break;
		}

		// Token: 0x06000A58 RID: 2648 RVA: 0x000255B2 File Offset: 0x000237B2
		public static IEnumerable<Vector2> CubicBezierPointsSkipFirst(Vector2 a, Vector2 b, Vector2 c, Vector2 d, int count)
		{
			int num2;
			for (int i = 1; i < count - 1; i = num2 + 1)
			{
				float num = (float)i / ((float)count - 1f);
				yield return ShapesMath.CubicBezier(a, b, c, d, num);
				num2 = i;
			}
			yield return d;
			yield break;
		}

		// Token: 0x06000A59 RID: 2649 RVA: 0x000255E0 File Offset: 0x000237E0
		public static Vector3 CubicBezier(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
		{
			if (t <= 0f)
			{
				return a;
			}
			if (t >= 1f)
			{
				return d;
			}
			float num = 1f - t;
			float num2 = num * num;
			float num3 = t * t;
			return a * (num2 * num) + b * (3f * num2 * t) + c * (3f * num * num3) + d * (num3 * t);
		}

		// Token: 0x06000A5A RID: 2650 RVA: 0x00025658 File Offset: 0x00023858
		public static Vector2 CubicBezier(Vector2 a, Vector2 b, Vector2 c, Vector2 d, float t)
		{
			if (t <= 0f)
			{
				return a;
			}
			if (t >= 1f)
			{
				return d;
			}
			float num = 1f - t;
			float num2 = num * num;
			float num3 = t * t;
			return a * (num2 * num) + b * (3f * num2 * t) + c * (3f * num * num3) + d * (num3 * t);
		}

		// Token: 0x06000A5B RID: 2651 RVA: 0x000256D0 File Offset: 0x000238D0
		public static Vector3 CubicBezierDerivative(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
		{
			float num = 1f - t;
			float num2 = num * num;
			float num3 = t * t;
			return a * (-3f * num2) + b * (9f * num3 - 12f * t + 3f) + c * (6f * t - 9f * num3) + d * (3f * num3);
		}

		// Token: 0x06000A5C RID: 2652 RVA: 0x0002574C File Offset: 0x0002394C
		public static float GetApproximateCurveSum(Vector3 a, Vector3 b, Vector3 c, Vector3 d, int vertCount)
		{
			Vector2[] array = new Vector2[vertCount];
			for (int i = 0; i < vertCount; i++)
			{
				float num = (float)i / ((float)vertCount - 1f);
				array[i] = ShapesMath.CubicBezierDerivative(a, b, c, d, num);
			}
			float num2 = 0f;
			for (int j = 0; j < vertCount - 1; j++)
			{
				num2 += Vector2.Angle(array[j], array[j + 1]);
			}
			return num2;
		}

		// Token: 0x06000A5D RID: 2653 RVA: 0x000257C5 File Offset: 0x000239C5
		[CompilerGenerated]
		internal static Vector3 <GetArcPoints>g__DirToPt|25_0(Vector3 dir, ref ShapesMath.<>c__DisplayClass25_0 A_1)
		{
			return A_1.center + dir * A_1.radius;
		}

		// Token: 0x06000A5E RID: 2654 RVA: 0x000257DE File Offset: 0x000239DE
		[CompilerGenerated]
		internal static Vector2 <GetArcPoints>g__DirToPt|26_0(Vector2 dir, ref ShapesMath.<>c__DisplayClass26_0 A_1)
		{
			return A_1.center + dir * A_1.radius;
		}

		// Token: 0x0400023C RID: 572
		public const float TAU = 6.2831855f;
	}
}
