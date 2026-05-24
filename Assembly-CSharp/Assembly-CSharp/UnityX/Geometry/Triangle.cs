using System;
using UnityEngine;

namespace UnityX.Geometry
{
	// Token: 0x0200022E RID: 558
	[Serializable]
	public struct Triangle
	{
		// Token: 0x06001438 RID: 5176 RVA: 0x0008C824 File Offset: 0x0008AA24
		public Triangle(Vector2 a, Vector2 b, Vector2 c)
		{
			this.a = a;
			this.b = b;
			this.c = c;
			this.ab = new Line(a, b);
			this.bc = new Line(b, c);
			this.ca = new Line(c, a);
			float sqrLength = this.ab.sqrLength;
			float sqrLength2 = this.bc.sqrLength;
			float sqrLength3 = this.ca.sqrLength;
			if (sqrLength2 > sqrLength && sqrLength2 > sqrLength3)
			{
				this.@base = this.bc.length;
				this.height = this.bc.GetClosestDistanceFromLine(a);
			}
			else if (sqrLength3 > sqrLength && sqrLength3 > sqrLength2)
			{
				this.@base = this.ca.length;
				this.height = this.ca.GetClosestDistanceFromLine(b);
			}
			else
			{
				this.@base = this.ab.length;
				this.height = this.ab.GetClosestDistanceFromLine(c);
			}
			this.area = this.height * this.@base * 0.5f;
		}

		// Token: 0x06001439 RID: 5177 RVA: 0x0008C927 File Offset: 0x0008AB27
		public bool ContainsPoint(Vector2 p)
		{
			return Triangle.ContainsPoint(this.a, this.b, this.c, p);
		}

		// Token: 0x0600143A RID: 5178 RVA: 0x0008C944 File Offset: 0x0008AB44
		public static bool ContainsPoint(Vector2 a, Vector2 b, Vector2 c, Vector2 p)
		{
			float num = c.x - b.x;
			float num2 = c.y - b.y;
			float num3 = a.x - c.x;
			float num4 = a.y - c.y;
			float num5 = b.x - a.x;
			float num6 = b.y - a.y;
			float num7 = p.x - a.x;
			float num8 = p.y - a.y;
			float num9 = p.x - b.x;
			float num10 = p.y - b.y;
			float num11 = p.x - c.x;
			float num12 = p.y - c.y;
			float num13 = num * num10 - num2 * num9;
			float num14 = num5 * num8 - num6 * num7;
			float num15 = num3 * num12 - num4 * num11;
			return num13 >= 0f && num15 >= 0f && num14 >= 0f;
		}

		// Token: 0x0600143B RID: 5179 RVA: 0x0008CA3C File Offset: 0x0008AC3C
		public Vector2 RandomPoint()
		{
			float num = Mathf.Sqrt(Random.Range(0f, 1f));
			float num2 = Random.Range(0f, 1f);
			float num3 = 1f - num;
			float num4 = num * (1f - num2);
			float num5 = num2 * num;
			return num3 * this.a + num4 * this.b + num5 * this.c;
		}

		// Token: 0x04001325 RID: 4901
		public Vector2 a;

		// Token: 0x04001326 RID: 4902
		public Vector2 b;

		// Token: 0x04001327 RID: 4903
		public Vector2 c;

		// Token: 0x04001328 RID: 4904
		public Line ab;

		// Token: 0x04001329 RID: 4905
		public Line bc;

		// Token: 0x0400132A RID: 4906
		public Line ca;

		// Token: 0x0400132B RID: 4907
		public float @base;

		// Token: 0x0400132C RID: 4908
		public float height;

		// Token: 0x0400132D RID: 4909
		public float area;
	}
}
