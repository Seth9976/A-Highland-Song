using System;
using UnityEngine;

// Token: 0x020001EF RID: 495
[Serializable]
public struct Range : IEquatable<Range>
{
	// Token: 0x1700041C RID: 1052
	// (get) Token: 0x060011EA RID: 4586 RVA: 0x00083221 File Offset: 0x00081421
	public float mid
	{
		get
		{
			return 0.5f * (this.min + this.max);
		}
	}

	// Token: 0x1700041D RID: 1053
	// (get) Token: 0x060011EB RID: 4587 RVA: 0x00083236 File Offset: 0x00081436
	public float length
	{
		get
		{
			return this.max - this.min;
		}
	}

	// Token: 0x1700041E RID: 1054
	// (get) Token: 0x060011EC RID: 4588 RVA: 0x00083245 File Offset: 0x00081445
	public Range negated
	{
		get
		{
			return new Range(-this.max, -this.min);
		}
	}

	// Token: 0x060011ED RID: 4589 RVA: 0x0008325A File Offset: 0x0008145A
	public Range(float min, float max)
	{
		this.min = min;
		this.max = max;
	}

	// Token: 0x060011EE RID: 4590 RVA: 0x0008326A File Offset: 0x0008146A
	public static Range Centered(float mid, float width)
	{
		return new Range(mid - 0.5f * width, mid + 0.5f * width);
	}

	// Token: 0x060011EF RID: 4591 RVA: 0x00083283 File Offset: 0x00081483
	public static Range Auto(float x0, float x1)
	{
		if (x0 <= x1)
		{
			return new Range(x0, x1);
		}
		return new Range(x1, x0);
	}

	// Token: 0x060011F0 RID: 4592 RVA: 0x00083298 File Offset: 0x00081498
	public float Random()
	{
		return global::UnityEngine.Random.Range(this.min, this.max);
	}

	// Token: 0x060011F1 RID: 4593 RVA: 0x000832AC File Offset: 0x000814AC
	public float RandomBell(int iterations = 3)
	{
		float num = 0f;
		for (int i = 0; i < iterations; i++)
		{
			num += global::UnityEngine.Random.Range(this.min, this.max);
		}
		return num / (float)iterations;
	}

	// Token: 0x060011F2 RID: 4594 RVA: 0x000832E5 File Offset: 0x000814E5
	public float Lerp(float t)
	{
		return Mathf.Lerp(this.min, this.max, t);
	}

	// Token: 0x060011F3 RID: 4595 RVA: 0x000832F9 File Offset: 0x000814F9
	public float LerpUnclamped(float t)
	{
		return Mathf.LerpUnclamped(this.min, this.max, t);
	}

	// Token: 0x060011F4 RID: 4596 RVA: 0x0008330D File Offset: 0x0008150D
	public float InverseLerp(float val)
	{
		return Mathf.InverseLerp(this.min, this.max, val);
	}

	// Token: 0x060011F5 RID: 4597 RVA: 0x00083321 File Offset: 0x00081521
	public float Clamp(float val)
	{
		return Mathf.Clamp(val, this.min, this.max);
	}

	// Token: 0x060011F6 RID: 4598 RVA: 0x00083335 File Offset: 0x00081535
	public bool Contains(float x)
	{
		return x >= this.min && x <= this.max;
	}

	// Token: 0x060011F7 RID: 4599 RVA: 0x0008334E File Offset: 0x0008154E
	public bool Contains(Range otherRange)
	{
		return otherRange.min >= this.min && otherRange.max <= this.max;
	}

	// Token: 0x060011F8 RID: 4600 RVA: 0x00083371 File Offset: 0x00081571
	public bool Intersects(Range otherRange)
	{
		return otherRange.min <= this.max && otherRange.max >= this.min;
	}

	// Token: 0x060011F9 RID: 4601 RVA: 0x00083394 File Offset: 0x00081594
	public Range Intersection(Range otherRange)
	{
		float num = Math.Max(this.min, otherRange.min);
		float num2 = Math.Min(this.max, otherRange.max);
		return new Range(num, num2);
	}

	// Token: 0x060011FA RID: 4602 RVA: 0x000833CA File Offset: 0x000815CA
	public float GetAmountIncludedByRange(Range otherRange)
	{
		return Mathf.Max(Mathf.Min(otherRange.max, this.max) - Mathf.Max(otherRange.min, this.min), 0f);
	}

	// Token: 0x060011FB RID: 4603 RVA: 0x000833F9 File Offset: 0x000815F9
	public float GetNormalizedAmountIncludedByRange(Range otherRange)
	{
		if (otherRange.length <= 0f)
		{
			return 1f;
		}
		return this.GetAmountIncludedByRange(otherRange) / this.length;
	}

	// Token: 0x060011FC RID: 4604 RVA: 0x00083420 File Offset: 0x00081620
	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		Range range = (Range)obj;
		return range != null && this.Equals(range);
	}

	// Token: 0x060011FD RID: 4605 RVA: 0x0008344A File Offset: 0x0008164A
	public bool Equals(Range p)
	{
		return p != null && this.min == p.min && this.max == p.max;
	}

	// Token: 0x060011FE RID: 4606 RVA: 0x00083474 File Offset: 0x00081674
	public override int GetHashCode()
	{
		return 27 * this.min.GetHashCode() * this.max.GetHashCode();
	}

	// Token: 0x060011FF RID: 4607 RVA: 0x00083490 File Offset: 0x00081690
	public static bool operator ==(Range left, Range right)
	{
		return left == right || (left != null && right != null && left.Equals(right));
	}

	// Token: 0x06001200 RID: 4608 RVA: 0x000834BC File Offset: 0x000816BC
	public static bool operator !=(Range left, Range right)
	{
		return !(left == right);
	}

	// Token: 0x06001201 RID: 4609 RVA: 0x000834C8 File Offset: 0x000816C8
	public static Range operator +(Range left, float x)
	{
		return new Range(left.min + x, left.max + x);
	}

	// Token: 0x06001202 RID: 4610 RVA: 0x000834DF File Offset: 0x000816DF
	public static Range operator -(Range left, float x)
	{
		return new Range(left.min - x, left.max - x);
	}

	// Token: 0x06001203 RID: 4611 RVA: 0x000834F6 File Offset: 0x000816F6
	public static Range operator *(Range left, float x)
	{
		return new Range(left.min * x, left.max * x);
	}

	// Token: 0x06001204 RID: 4612 RVA: 0x0008350D File Offset: 0x0008170D
	public static Range operator /(Range left, float x)
	{
		return new Range(left.min / x, left.max / x);
	}

	// Token: 0x06001205 RID: 4613 RVA: 0x00083524 File Offset: 0x00081724
	public override string ToString()
	{
		return string.Format("[{0:N1} to {1:N1}]", this.min, this.max);
	}

	// Token: 0x04001278 RID: 4728
	public float min;

	// Token: 0x04001279 RID: 4729
	public float max;

	// Token: 0x0400127A RID: 4730
	public static readonly Range infinity = new Range(float.NegativeInfinity, float.PositiveInfinity);

	// Token: 0x0400127B RID: 4731
	public static readonly Range zero = default(Range);
}
