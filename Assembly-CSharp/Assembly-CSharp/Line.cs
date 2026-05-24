using System;
using UnityEngine;

// Token: 0x02000209 RID: 521
[Serializable]
public struct Line
{
	// Token: 0x17000444 RID: 1092
	// (get) Token: 0x06001332 RID: 4914 RVA: 0x0008801C File Offset: 0x0008621C
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

	// Token: 0x17000445 RID: 1093
	// (get) Token: 0x06001333 RID: 4915 RVA: 0x0008804D File Offset: 0x0008624D
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

	// Token: 0x17000446 RID: 1094
	// (get) Token: 0x06001334 RID: 4916 RVA: 0x0008807E File Offset: 0x0008627E
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

	// Token: 0x17000447 RID: 1095
	// (get) Token: 0x06001335 RID: 4917 RVA: 0x000880AF File Offset: 0x000862AF
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

	// Token: 0x17000448 RID: 1096
	// (get) Token: 0x06001336 RID: 4918 RVA: 0x000880E0 File Offset: 0x000862E0
	public Vector2 direction
	{
		get
		{
			return this.vector.normalized;
		}
	}

	// Token: 0x17000449 RID: 1097
	// (get) Token: 0x06001337 RID: 4919 RVA: 0x000880FB File Offset: 0x000862FB
	public Vector2 vector
	{
		get
		{
			return this.end - this.start;
		}
	}

	// Token: 0x1700044A RID: 1098
	// (get) Token: 0x06001338 RID: 4920 RVA: 0x0008810E File Offset: 0x0008630E
	public float length
	{
		get
		{
			return Vector2.Distance(this.start, this.end);
		}
	}

	// Token: 0x1700044B RID: 1099
	// (get) Token: 0x06001339 RID: 4921 RVA: 0x00088121 File Offset: 0x00086321
	public float sqrLength
	{
		get
		{
			return Line.SqrDistance(this.start, this.end);
		}
	}

	// Token: 0x0600133A RID: 4922 RVA: 0x00088134 File Offset: 0x00086334
	private static float SqrDistance(Vector2 a, Vector2 b)
	{
		return (a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y);
	}

	// Token: 0x0600133B RID: 4923 RVA: 0x0008816D File Offset: 0x0008636D
	public Line(Vector2 _start, Vector2 _end)
	{
		this.start = _start;
		this.end = _end;
	}

	// Token: 0x0600133C RID: 4924 RVA: 0x0008817D File Offset: 0x0008637D
	public Line(Vector2[] _startEnd)
	{
		this.start = _startEnd[0];
		this.end = _startEnd[1];
	}

	// Token: 0x0600133D RID: 4925 RVA: 0x00088199 File Offset: 0x00086399
	public void Set(Vector2 _start, Vector2 _end)
	{
		this.start = _start;
		this.end = _end;
	}

	// Token: 0x0600133E RID: 4926 RVA: 0x000881A9 File Offset: 0x000863A9
	public Vector2 AtDistance(float distance)
	{
		return this.start + this.direction * distance;
	}

	// Token: 0x0600133F RID: 4927 RVA: 0x000881C4 File Offset: 0x000863C4
	public override string ToString()
	{
		string text = "Start: ";
		Vector2 vector = this.start;
		string text2 = vector.ToString();
		string text3 = " End: ";
		vector = this.end;
		return text + text2 + text3 + vector.ToString();
	}

	// Token: 0x06001340 RID: 4928 RVA: 0x00088208 File Offset: 0x00086408
	public static Line Add(Line left, Line right)
	{
		return new Line(left.start + right.start, left.end + right.end);
	}

	// Token: 0x06001341 RID: 4929 RVA: 0x00088231 File Offset: 0x00086431
	public static Line Subtract(Line left, Line right)
	{
		return new Line(left.start - right.start, left.end - right.end);
	}

	// Token: 0x06001342 RID: 4930 RVA: 0x0008825C File Offset: 0x0008645C
	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		Line line = (Line)obj;
		return line != null && this.Equals(line);
	}

	// Token: 0x06001343 RID: 4931 RVA: 0x00088286 File Offset: 0x00086486
	public bool Equals(Line l)
	{
		return l != null && this.start == l.start && this.end == l.end;
	}

	// Token: 0x06001344 RID: 4932 RVA: 0x000882B8 File Offset: 0x000864B8
	public override int GetHashCode()
	{
		return 27 * this.start.GetHashCode() * this.end.GetHashCode();
	}

	// Token: 0x06001345 RID: 4933 RVA: 0x000882E0 File Offset: 0x000864E0
	public static bool operator ==(Line left, Line right)
	{
		return left == right || (left != null && right != null && left.Equals(right));
	}

	// Token: 0x06001346 RID: 4934 RVA: 0x0008830C File Offset: 0x0008650C
	public static bool operator !=(Line left, Line right)
	{
		return !(left == right);
	}

	// Token: 0x06001347 RID: 4935 RVA: 0x00088318 File Offset: 0x00086518
	public static Line operator +(Line left, Line right)
	{
		return Line.Add(left, right);
	}

	// Token: 0x06001348 RID: 4936 RVA: 0x00088321 File Offset: 0x00086521
	public static Line operator -(Line left, Line right)
	{
		return Line.Subtract(left, right);
	}

	// Token: 0x06001349 RID: 4937 RVA: 0x0008832A File Offset: 0x0008652A
	public static bool IntersectionCheck(Line line1, Line line2)
	{
		return Line.IntersectionCheck(line1.start, line1.end, line2.start, line2.end);
	}

	// Token: 0x0600134A RID: 4938 RVA: 0x0008834C File Offset: 0x0008654C
	public static bool IntersectionCheck(Vector2 line1Start, Vector2 line1End, Vector2 line2Start, Vector2 line2End)
	{
		float num = line1End.x - line1Start.x;
		float num2 = line1End.y - line1Start.y;
		float num3 = line2End.x - line2Start.x;
		float num4 = line2End.y - line2Start.y;
		float num5 = line1Start.x - line2Start.x;
		float num6 = line1Start.y - line2Start.y;
		float num7 = (-num2 * num5 + num * num6) / (-num3 * num2 + num * num4);
		if (num7 >= 0f && num7 <= 1f)
		{
			float num8 = (num3 * num6 - num4 * num5) / (-num3 * num2 + num * num4);
			if (num8 >= 0f && num8 <= 1f)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600134B RID: 4939 RVA: 0x00088400 File Offset: 0x00086600
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

	// Token: 0x0600134C RID: 4940 RVA: 0x00088534 File Offset: 0x00086734
	public bool RayLineIntersect(Vector2 rayOrigin, Vector2 rayDirection, out float distance)
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
		return distance >= 0f && num2 >= 0f && num2 <= 1f;
	}

	// Token: 0x0600134D RID: 4941 RVA: 0x000885CE File Offset: 0x000867CE
	public float GetClosestDistanceFromLine(Vector2 p)
	{
		return Vector2.Distance(p, this.GetClosestPointOnLine(p, true));
	}

	// Token: 0x0600134E RID: 4942 RVA: 0x000885E0 File Offset: 0x000867E0
	public Vector2 GetClosestPointOnLine(Vector2 p, bool clamped = true)
	{
		float normalizedDistanceOnLine = this.GetNormalizedDistanceOnLine(p, clamped);
		return Vector3.LerpUnclamped(this.start, this.end, normalizedDistanceOnLine);
	}

	// Token: 0x0600134F RID: 4943 RVA: 0x00088617 File Offset: 0x00086817
	public float GetNormalizedDistanceOnLine(Vector2 p, bool clamped = true)
	{
		return Line.GetNormalizedDistanceOnLineInternal(this.start, this.end, p, this.sqrLength, clamped);
	}

	// Token: 0x06001350 RID: 4944 RVA: 0x00088632 File Offset: 0x00086832
	public static float GetClosestDistanceFromLine(Vector2 start, Vector2 end, Vector2 p)
	{
		return Vector2.Distance(p, Line.GetClosestPointOnLine(start, end, p, true));
	}

	// Token: 0x06001351 RID: 4945 RVA: 0x00088644 File Offset: 0x00086844
	public static Vector2 GetClosestPointOnLine(Vector2 start, Vector2 end, Vector2 p, bool clamped = true)
	{
		float normalizedDistanceOnLine = Line.GetNormalizedDistanceOnLine(start, end, p, clamped);
		return Vector2.LerpUnclamped(start, end, normalizedDistanceOnLine);
	}

	// Token: 0x06001352 RID: 4946 RVA: 0x00088664 File Offset: 0x00086864
	public static float GetNormalizedDistanceOnLine(Vector2 start, Vector2 end, Vector2 p, bool clamped = true)
	{
		float num = Line.SqrDistance(start, end);
		return Line.GetNormalizedDistanceOnLineInternal(start, end, p, num, clamped);
	}

	// Token: 0x06001353 RID: 4947 RVA: 0x00088684 File Offset: 0x00086884
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

	// Token: 0x040012AC RID: 4780
	public Vector2 start;

	// Token: 0x040012AD RID: 4781
	public Vector2 end;
}
