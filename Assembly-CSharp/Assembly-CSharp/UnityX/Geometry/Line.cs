using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityX.Geometry
{
	// Token: 0x0200022D RID: 557
	[Serializable]
	public struct Line
	{
		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x06001413 RID: 5139 RVA: 0x0008BFC4 File Offset: 0x0008A1C4
		public Vector2 direction
		{
			get
			{
				return this.vector.normalized;
			}
		}

		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x06001414 RID: 5140 RVA: 0x0008BFDF File Offset: 0x0008A1DF
		public Vector2 vector
		{
			get
			{
				return this.end - this.start;
			}
		}

		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x06001415 RID: 5141 RVA: 0x0008BFF2 File Offset: 0x0008A1F2
		public float length
		{
			get
			{
				return Vector2.Distance(this.start, this.end);
			}
		}

		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x06001416 RID: 5142 RVA: 0x0008C008 File Offset: 0x0008A208
		public float sqrLength
		{
			get
			{
				return (this.start.x - this.end.x) * (this.start.x - this.end.x) + (this.start.y - this.end.y) * (this.start.y - this.end.y);
			}
		}

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x06001417 RID: 5143 RVA: 0x0008C074 File Offset: 0x0008A274
		public float left
		{
			get
			{
				if (this.start.x < this.end.x)
				{
					return this.start.x;
				}
				return this.end.x;
			}
		}

		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x06001418 RID: 5144 RVA: 0x0008C0A5 File Offset: 0x0008A2A5
		public float right
		{
			get
			{
				if (this.start.x > this.end.x)
				{
					return this.start.x;
				}
				return this.end.x;
			}
		}

		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x06001419 RID: 5145 RVA: 0x0008C0D6 File Offset: 0x0008A2D6
		public float top
		{
			get
			{
				if (this.start.y > this.end.y)
				{
					return this.start.y;
				}
				return this.end.y;
			}
		}

		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x0600141A RID: 5146 RVA: 0x0008C107 File Offset: 0x0008A307
		public float bottom
		{
			get
			{
				if (this.start.y < this.end.y)
				{
					return this.start.y;
				}
				return this.end.y;
			}
		}

		// Token: 0x0600141B RID: 5147 RVA: 0x0008C138 File Offset: 0x0008A338
		public Line(Vector2 _start, Vector2 _end)
		{
			this.start = _start;
			this.end = _end;
		}

		// Token: 0x0600141C RID: 5148 RVA: 0x0008C148 File Offset: 0x0008A348
		public Line(Vector2[] _startEnd)
		{
			this.start = _startEnd[0];
			this.end = _startEnd[1];
		}

		// Token: 0x0600141D RID: 5149 RVA: 0x0008C164 File Offset: 0x0008A364
		public void Set(Vector2 _start, Vector2 _end)
		{
			this.start = _start;
			this.end = _end;
		}

		// Token: 0x0600141E RID: 5150 RVA: 0x0008C174 File Offset: 0x0008A374
		public Vector2 AtDistance(float distance)
		{
			return this.start + this.direction * distance;
		}

		// Token: 0x0600141F RID: 5151 RVA: 0x0008C190 File Offset: 0x0008A390
		public override string ToString()
		{
			string text = "Start: ";
			Vector2 vector = this.start;
			string text2 = vector.ToString();
			string text3 = " End: ";
			vector = this.end;
			return text + text2 + text3 + vector.ToString();
		}

		// Token: 0x06001420 RID: 5152 RVA: 0x0008C1D4 File Offset: 0x0008A3D4
		public static Line Add(Line left, Line right)
		{
			return new Line(left.start + right.start, left.end + right.end);
		}

		// Token: 0x06001421 RID: 5153 RVA: 0x0008C1FD File Offset: 0x0008A3FD
		public static Line Subtract(Line left, Line right)
		{
			return new Line(left.start - right.start, left.end - right.end);
		}

		// Token: 0x06001422 RID: 5154 RVA: 0x0008C228 File Offset: 0x0008A428
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			Line line = (Line)obj;
			return line != null && this.Equals(line);
		}

		// Token: 0x06001423 RID: 5155 RVA: 0x0008C252 File Offset: 0x0008A452
		public bool Equals(Line l)
		{
			return l != null && this.start == l.start && this.end == l.end;
		}

		// Token: 0x06001424 RID: 5156 RVA: 0x0008C284 File Offset: 0x0008A484
		public override int GetHashCode()
		{
			return (27 * 31 + this.start.GetHashCode()) * 31 + this.end.GetHashCode();
		}

		// Token: 0x06001425 RID: 5157 RVA: 0x0008C2B2 File Offset: 0x0008A4B2
		public static bool operator ==(Line left, Line right)
		{
			return left == right || (left != null && right != null && left.Equals(right));
		}

		// Token: 0x06001426 RID: 5158 RVA: 0x0008C2DE File Offset: 0x0008A4DE
		public static bool operator !=(Line left, Line right)
		{
			return !(left == right);
		}

		// Token: 0x06001427 RID: 5159 RVA: 0x0008C2EA File Offset: 0x0008A4EA
		public static Line operator +(Line left, Line right)
		{
			return Line.Add(left, right);
		}

		// Token: 0x06001428 RID: 5160 RVA: 0x0008C2F3 File Offset: 0x0008A4F3
		public static Line operator -(Line left, Line right)
		{
			return Line.Subtract(left, right);
		}

		// Token: 0x06001429 RID: 5161 RVA: 0x0008C2FC File Offset: 0x0008A4FC
		public static bool IntersectionCheck(Line line1, Line line2)
		{
			return Line.IntersectionCheck(line1.start, line1.end, line2.start, line2.end);
		}

		// Token: 0x0600142A RID: 5162 RVA: 0x0008C31C File Offset: 0x0008A51C
		public static bool IntersectionCheck(Vector2 line1Start, Vector2 line1End, Vector2 line2Start, Vector2 line2End)
		{
			float num = line1End.x - line1Start.x;
			float num2 = line1End.y - line1Start.y;
			float num3 = line2End.x - line2Start.x;
			float num4 = line2End.y - line2Start.y;
			float num5 = line1Start.x - line2Start.x;
			float num6 = line1Start.y - line2Start.y;
			float num7 = -num3 * num2 + num * num4;
			if (num7 != 0f)
			{
				float num8 = (-num2 * num5 + num * num6) / num7;
				if (num8 >= 0f && num8 <= 1f)
				{
					float num9 = (num3 * num6 - num4 * num5) / num7;
					if (num9 >= 0f && num9 <= 1f)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600142B RID: 5163 RVA: 0x0008C3D8 File Offset: 0x0008A5D8
		public static bool LineIntersectionPoint(Line line1, Line line2, out Vector2 intersectionPoint, bool clampStart = true, bool clampEnd = true)
		{
			float num = line1.end.x - line1.start.x;
			float num2 = line1.end.y - line1.start.y;
			float num3 = line2.end.x - line2.start.x;
			float num4 = line2.end.y - line2.start.y;
			float num5 = line1.start.x - line2.start.x;
			float num6 = line1.start.y - line2.start.y;
			float num7 = (-num2 * num5 + num * num6) / (-num3 * num2 + num * num4);
			if ((!clampStart || num7 >= 0f) && (!clampEnd || num7 <= 1f))
			{
				float num8 = (num3 * num6 - num4 * num5) / (-num3 * num2 + num * num4);
				if ((!clampStart || num8 >= 0f) && (!clampEnd || num8 <= 1f))
				{
					intersectionPoint.x = line1.start.x + num8 * num;
					intersectionPoint.y = line1.start.y + num8 * num2;
					return true;
				}
			}
			intersectionPoint = Vector2.zero;
			return false;
		}

		// Token: 0x0600142C RID: 5164 RVA: 0x0008C50C File Offset: 0x0008A70C
		public bool RayLineIntersect(Vector2 rayOrigin, Vector2 rayDirection, out float distance, bool clamped = true)
		{
			Vector2 vector = this.vector;
			Vector2 vector2 = new Vector2(vector.y, -vector.x);
			float num = Vector2.Dot(rayDirection, vector2);
			if (Mathf.Abs(num) <= 1E-45f)
			{
				distance = float.MaxValue;
				return false;
			}
			Vector2 vector3 = this.start - rayOrigin;
			distance = Vector2.Dot(vector2, vector3) / num;
			float num2 = Vector2.Dot(new Vector2(rayDirection.y, -rayDirection.x), vector3) / num;
			return distance >= 0f && (!clamped || (num2 >= 0f && num2 <= 1f));
		}

		// Token: 0x0600142D RID: 5165 RVA: 0x0008C5AE File Offset: 0x0008A7AE
		public float GetClosestDistanceFromLine(Vector2 p)
		{
			return Vector2.Distance(p, this.GetClosestPointOnLine(p, true));
		}

		// Token: 0x0600142E RID: 5166 RVA: 0x0008C5C0 File Offset: 0x0008A7C0
		public Vector2 GetClosestPointOnLine(Vector2 p, bool clamped = true)
		{
			float normalizedDistanceOnLine = this.GetNormalizedDistanceOnLine(p, clamped);
			return Vector2.LerpUnclamped(this.start, this.end, normalizedDistanceOnLine);
		}

		// Token: 0x0600142F RID: 5167 RVA: 0x0008C5E8 File Offset: 0x0008A7E8
		public float GetNormalizedDistanceOnLine(Vector2 p, bool clamped = true)
		{
			return Line.GetNormalizedDistanceOnLineInternal(this.start, this.end, p, this.sqrLength, clamped);
		}

		// Token: 0x06001430 RID: 5168 RVA: 0x0008C603 File Offset: 0x0008A803
		public static float GetClosestDistanceFromLine(Vector2 start, Vector2 end, Vector2 p, bool clamped = true)
		{
			return Vector2.Distance(p, Line.GetClosestPointOnLine(start, end, p, clamped));
		}

		// Token: 0x06001431 RID: 5169 RVA: 0x0008C614 File Offset: 0x0008A814
		public static Vector2 GetClosestPointOnLine(Vector2 start, Vector2 end, Vector2 p, bool clamped = true)
		{
			float normalizedDistanceOnLine = Line.GetNormalizedDistanceOnLine(start, end, p, clamped);
			return Vector2.LerpUnclamped(start, end, normalizedDistanceOnLine);
		}

		// Token: 0x06001432 RID: 5170 RVA: 0x0008C634 File Offset: 0x0008A834
		public static float GetNormalizedDistanceOnLine(Vector2 start, Vector2 end, Vector2 p, bool clamped = true)
		{
			float num = (start.x - end.x) * (start.x - end.x) + (start.y - end.y) * (start.y - end.y);
			return Line.GetNormalizedDistanceOnLineInternal(start, end, p, num, clamped);
		}

		// Token: 0x06001433 RID: 5171 RVA: 0x0008C684 File Offset: 0x0008A884
		private static float GetNormalizedDistanceOnLineInternal(Vector2 start, Vector2 end, Vector2 p, float sqrLength, bool clamped = true)
		{
			if (sqrLength == 0f)
			{
				return 0f;
			}
			float num = Vector2.Dot(p - start, end - start) / sqrLength;
			if (!clamped)
			{
				return num;
			}
			return Mathf.Clamp01(num);
		}

		// Token: 0x06001434 RID: 5172 RVA: 0x0008C6C1 File Offset: 0x0008A8C1
		public static IEnumerable<Vector2Int> GetCrossedCells(Vector2 pPoint1, Vector2 pPoint2)
		{
			if (pPoint1 != pPoint2)
			{
				Vector2 V = (pPoint2 - pPoint1) / 1f;
				Vector2 U = V.normalized;
				Vector2Int S = new Vector2Int((int)Mathf.Sign(U.x), (int)Mathf.Sign(U.y));
				Vector2 P = pPoint1 / 1f;
				Vector2Int G = new Vector2Int((int)Mathf.Floor(P.x), (int)Mathf.Floor(P.y));
				Vector2 T = new Vector2(Mathf.Abs(1f / U.x), Mathf.Abs(1f / U.y));
				Vector2 vector = new Vector2((S.x > 0) ? (1f - P.x % 1f) : ((S.x < 0) ? (P.x % 1f) : 0f), (S.y > 0) ? (1f - P.y % 1f) : ((S.y < 0) ? (P.y % 1f) : 0f));
				Vector2 M = new Vector2((float.PositiveInfinity == T.x || S.x == 0) ? float.PositiveInfinity : (T.x * vector.x), (float.PositiveInfinity == T.y || S.y == 0) ? float.PositiveInfinity : (T.y * vector.y));
				bool isCanMoveByX = S.x != 0;
				bool isCanMoveByY = S.y != 0;
				while (isCanMoveByX || isCanMoveByY)
				{
					yield return G;
					vector = new Vector2((S.x > 0) ? (Mathf.Floor(P.x) + 1f - P.x) : ((S.x < 0) ? (Mathf.Ceil(P.x) - 1f - P.x) : 0f), (S.y > 0) ? (Mathf.Floor(P.y) + 1f - P.y) : ((S.y < 0) ? (Mathf.Ceil(P.y) - 1f - P.y) : 0f));
					if (Mathf.Abs(V.x) <= Mathf.Abs(vector.x))
					{
						vector.x = V.x;
						isCanMoveByX = false;
					}
					if (Mathf.Abs(V.y) <= Mathf.Abs(vector.y))
					{
						vector.y = V.y;
						isCanMoveByY = false;
					}
					if (M.x <= M.y)
					{
						M.x += T.x;
						G.x += S.x;
						if (isCanMoveByY)
						{
							vector.y = U.y / U.x * vector.x;
						}
					}
					else
					{
						M.y += T.y;
						G.y += S.y;
						if (isCanMoveByX)
						{
							vector.x = U.x / U.y * vector.y;
						}
					}
					V -= vector;
					P += vector;
				}
				V = default(Vector2);
				U = default(Vector2);
				S = default(Vector2Int);
				P = default(Vector2);
				G = default(Vector2Int);
				T = default(Vector2);
				M = default(Vector2);
			}
			yield break;
		}

		// Token: 0x06001435 RID: 5173 RVA: 0x0008C6D8 File Offset: 0x0008A8D8
		public static List<Vector2Int> PointsOnLine(int x0, int y0, int x1, int y1)
		{
			List<Vector2Int> list = new List<Vector2Int>();
			int num = Mathf.Abs(x1 - x0);
			int num2 = Mathf.Abs(y1 - y0);
			int num3 = ((x0 < x1) ? 1 : (-1));
			int num4 = ((y0 < y1) ? 1 : (-1));
			int num5 = num - num2;
			for (;;)
			{
				list.Add(new Vector2Int(x0, y0));
				if (x0 == x1 && y0 == y1)
				{
					break;
				}
				int num6 = num5 * 2;
				if (num6 > -num)
				{
					num5 -= num2;
					x0 += num3;
				}
				if (num6 < num)
				{
					num5 += num;
					y0 += num4;
				}
			}
			return list;
		}

		// Token: 0x06001436 RID: 5174 RVA: 0x0008C750 File Offset: 0x0008A950
		private static void Swap<T>(ref T lhs, ref T rhs)
		{
			T t = lhs;
			lhs = rhs;
			rhs = t;
		}

		// Token: 0x06001437 RID: 5175 RVA: 0x0008C778 File Offset: 0x0008A978
		public static void Plot(int x0, int y0, int x1, int y1, Line.PlotFunction plot)
		{
			bool flag = Mathf.Abs(y1 - y0) > Mathf.Abs(x1 - x0);
			if (flag)
			{
				Line.Swap<int>(ref x0, ref y0);
				Line.Swap<int>(ref x1, ref y1);
			}
			if (x0 > x1)
			{
				Line.Swap<int>(ref x0, ref x1);
				Line.Swap<int>(ref y0, ref y1);
			}
			int num = x1 - x0;
			int num2 = Mathf.Abs(y1 - y0);
			int num3 = num / 2;
			int num4 = ((y0 < y1) ? 1 : (-1));
			int num5 = y0;
			for (int i = x0; i <= x1; i++)
			{
				if (!(flag ? plot(num5, i) : plot(i, num5)))
				{
					return;
				}
				num3 -= num2;
				if (num3 < 0)
				{
					num5 += num4;
					num3 += num;
				}
			}
		}

		// Token: 0x04001323 RID: 4899
		public Vector2 start;

		// Token: 0x04001324 RID: 4900
		public Vector2 end;

		// Token: 0x02000420 RID: 1056
		// (Invoke) Token: 0x0600196B RID: 6507
		public delegate bool PlotFunction(int x, int y);
	}
}
