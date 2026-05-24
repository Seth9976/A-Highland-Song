using System;
using UnityEngine;

// Token: 0x020001EC RID: 492
[Serializable]
public struct Line3D
{
	// Token: 0x1700040E RID: 1038
	// (get) Token: 0x0600116F RID: 4463 RVA: 0x00080D70 File Offset: 0x0007EF70
	public Vector3 direction
	{
		get
		{
			return Vector3.Normalize(this.end - this.start);
		}
	}

	// Token: 0x1700040F RID: 1039
	// (get) Token: 0x06001170 RID: 4464 RVA: 0x00080D88 File Offset: 0x0007EF88
	public Vector3 vector
	{
		get
		{
			return this.end - this.start;
		}
	}

	// Token: 0x17000410 RID: 1040
	// (get) Token: 0x06001171 RID: 4465 RVA: 0x00080D9B File Offset: 0x0007EF9B
	public float length
	{
		get
		{
			return Vector3.Distance(this.start, this.end);
		}
	}

	// Token: 0x17000411 RID: 1041
	// (get) Token: 0x06001172 RID: 4466 RVA: 0x00080DAE File Offset: 0x0007EFAE
	public float sqrLength
	{
		get
		{
			return Line3D.SqrDistance(this.start, this.end);
		}
	}

	// Token: 0x06001173 RID: 4467 RVA: 0x00080DC1 File Offset: 0x0007EFC1
	public Line3D(Vector3 _start, Vector3 _end)
	{
		this.start = _start;
		this.end = _end;
	}

	// Token: 0x06001174 RID: 4468 RVA: 0x00080DD1 File Offset: 0x0007EFD1
	public Line3D(Vector3[] _startEnd)
	{
		this.start = _startEnd[0];
		this.end = _startEnd[1];
	}

	// Token: 0x06001175 RID: 4469 RVA: 0x00080DED File Offset: 0x0007EFED
	public void Set(Vector3 _start, Vector3 _end)
	{
		this.start = _start;
		this.end = _end;
	}

	// Token: 0x06001176 RID: 4470 RVA: 0x00080DFD File Offset: 0x0007EFFD
	public Vector3 AtDistance(float distance)
	{
		return this.start + this.direction * distance;
	}

	// Token: 0x06001177 RID: 4471 RVA: 0x00080E18 File Offset: 0x0007F018
	public override string ToString()
	{
		string text = "Start: ";
		Vector3 vector = this.start;
		string text2 = vector.ToString();
		string text3 = " End: ";
		vector = this.end;
		return text + text2 + text3 + vector.ToString();
	}

	// Token: 0x06001178 RID: 4472 RVA: 0x00080E5C File Offset: 0x0007F05C
	public static Line3D Add(Line3D left, Line3D right)
	{
		return new Line3D(left.start + right.start, left.end + right.end);
	}

	// Token: 0x06001179 RID: 4473 RVA: 0x00080E85 File Offset: 0x0007F085
	public static Line3D Subtract(Line3D left, Line3D right)
	{
		return new Line3D(left.start - right.start, left.end - right.end);
	}

	// Token: 0x0600117A RID: 4474 RVA: 0x00080EB0 File Offset: 0x0007F0B0
	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		Line3D line3D = (Line3D)obj;
		return line3D != null && ((this.start == line3D.start && this.end == line3D.end) || (this.start == line3D.end && this.end == line3D.start));
	}

	// Token: 0x0600117B RID: 4475 RVA: 0x00080F24 File Offset: 0x0007F124
	public bool Equals(Line3D l)
	{
		return l != null && ((this.start == l.start && this.end == l.end) || (this.start == l.end && this.end == l.start));
	}

	// Token: 0x0600117C RID: 4476 RVA: 0x00080F89 File Offset: 0x0007F189
	public override int GetHashCode()
	{
		return (27 * 31 + this.start.GetHashCode()) * 31 + this.end.GetHashCode();
	}

	// Token: 0x0600117D RID: 4477 RVA: 0x00080FB8 File Offset: 0x0007F1B8
	public static bool operator ==(Line3D left, Line3D right)
	{
		return left == right || (left != null && right != null && ((left.start == right.start && left.end == right.end) || (left.start == right.end && left.end == right.start)));
	}

	// Token: 0x0600117E RID: 4478 RVA: 0x00081035 File Offset: 0x0007F235
	public static bool operator !=(Line3D left, Line3D right)
	{
		return !(left == right);
	}

	// Token: 0x0600117F RID: 4479 RVA: 0x00081041 File Offset: 0x0007F241
	public static Line3D operator +(Line3D left, Line3D right)
	{
		return Line3D.Add(left, right);
	}

	// Token: 0x06001180 RID: 4480 RVA: 0x0008104A File Offset: 0x0007F24A
	public static Line3D operator -(Line3D left, Line3D right)
	{
		return Line3D.Subtract(left, right);
	}

	// Token: 0x06001181 RID: 4481 RVA: 0x00081054 File Offset: 0x0007F254
	public static bool LineIntersectionPoint(Line3D line1, Line3D line2, out Vector3 intersectionPoint)
	{
		float num = line1.end.y - line1.start.y;
		float num2 = line1.start.x - line1.end.x;
		float num3 = num * line1.start.x + num2 * line1.start.y;
		float num4 = line2.end.y - line2.start.y;
		float num5 = line2.start.x - line2.end.x;
		float num6 = num4 * line2.start.x + num5 * line2.start.y;
		float num7 = num * num5 - num4 * num2;
		if (num7 == 0f)
		{
			intersectionPoint = Vector3.zero;
			return false;
		}
		intersectionPoint = new Vector3((num5 * num3 - num2 * num6) / num7, (num * num6 - num4 * num3) / num7);
		return true;
	}

	// Token: 0x06001182 RID: 4482 RVA: 0x0008113F File Offset: 0x0007F33F
	public float GetClosestSqrDistanceFromLine(Vector3 p, bool clamped = true)
	{
		return Line3D.SqrDistance(p, this.GetClosestPointOnLine(p, clamped));
	}

	// Token: 0x06001183 RID: 4483 RVA: 0x0008114F File Offset: 0x0007F34F
	public float GetClosestDistanceFromLine(Vector3 p, bool clamped = true)
	{
		return Vector3.Distance(p, this.GetClosestPointOnLine(p, clamped));
	}

	// Token: 0x06001184 RID: 4484 RVA: 0x00081160 File Offset: 0x0007F360
	public Vector3 GetClosestPointOnLine(Vector3 p, bool clamped = true)
	{
		float normalizedDistanceOnLine = this.GetNormalizedDistanceOnLine(p, clamped);
		return Vector3.LerpUnclamped(this.start, this.end, normalizedDistanceOnLine);
	}

	// Token: 0x06001185 RID: 4485 RVA: 0x00081188 File Offset: 0x0007F388
	public float GetNormalizedDistanceOnLine(Vector3 p, bool clamped = true)
	{
		return Line3D.GetNormalizedDistanceOnLineInternal(this.start, this.end, p, this.sqrLength, clamped);
	}

	// Token: 0x06001186 RID: 4486 RVA: 0x000811A3 File Offset: 0x0007F3A3
	public float GetDistanceOnLine(Vector3 p, bool clamped = true)
	{
		return this.GetNormalizedDistanceOnLine(p, clamped) * this.length;
	}

	// Token: 0x06001187 RID: 4487 RVA: 0x000811B4 File Offset: 0x0007F3B4
	public static float GetClosestDistanceFromLine(Vector3 start, Vector3 end, Vector3 p)
	{
		return Vector3.Distance(p, Line3D.GetClosestPointOnLine(start, end, p, true));
	}

	// Token: 0x06001188 RID: 4488 RVA: 0x000811C8 File Offset: 0x0007F3C8
	public static Vector3 GetClosestPointOnLine(Vector3 start, Vector3 end, Vector3 p, bool clamped = true)
	{
		float normalizedDistanceOnLine = Line3D.GetNormalizedDistanceOnLine(start, end, p, clamped);
		return Vector3.LerpUnclamped(start, end, normalizedDistanceOnLine);
	}

	// Token: 0x06001189 RID: 4489 RVA: 0x000811E8 File Offset: 0x0007F3E8
	public static float GetNormalizedDistanceOnLine(Vector3 start, Vector3 end, Vector3 p, bool clamped = true)
	{
		float num = Line3D.SqrDistance(start, end);
		return Line3D.GetNormalizedDistanceOnLineInternal(start, end, p, num, clamped);
	}

	// Token: 0x0600118A RID: 4490 RVA: 0x00081207 File Offset: 0x0007F407
	public static float GetDistanceOnLine(Vector3 start, Vector3 end, Vector3 p, bool clamped = true)
	{
		return Line3D.GetNormalizedDistanceOnLine(start, end, p, clamped) * Vector3.Distance(start, end);
	}

	// Token: 0x0600118B RID: 4491 RVA: 0x0008121C File Offset: 0x0007F41C
	private static float GetNormalizedDistanceOnLineInternal(Vector3 start, Vector3 end, Vector3 p, float sqrLength, bool clamped = true)
	{
		if (sqrLength == 0f)
		{
			return 0f;
		}
		float num = Vector3.Dot(p - start, end - start) / sqrLength;
		if (!clamped)
		{
			return num;
		}
		return Mathf.Clamp01(num);
	}

	// Token: 0x0600118C RID: 4492 RVA: 0x0008125C File Offset: 0x0007F45C
	private static float SqrDistance(Vector3 a, Vector3 b)
	{
		return (a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y) + (a.z - b.z) * (a.z - b.z);
	}

	// Token: 0x04001271 RID: 4721
	public Vector3 start;

	// Token: 0x04001272 RID: 4722
	public Vector3 end;
}
