using System;
using System.Collections.Generic;

namespace UnityEngine.Assertions.Comparers
{
	// Token: 0x02000488 RID: 1160
	public class FloatComparer : IEqualityComparer<float>
	{
		// Token: 0x0600291D RID: 10525 RVA: 0x00043E3E File Offset: 0x0004203E
		public FloatComparer()
			: this(1E-05f, false)
		{
		}

		// Token: 0x0600291E RID: 10526 RVA: 0x00043E4E File Offset: 0x0004204E
		public FloatComparer(bool relative)
			: this(1E-05f, relative)
		{
		}

		// Token: 0x0600291F RID: 10527 RVA: 0x00043E5E File Offset: 0x0004205E
		public FloatComparer(float error)
			: this(error, false)
		{
		}

		// Token: 0x06002920 RID: 10528 RVA: 0x00043E6A File Offset: 0x0004206A
		public FloatComparer(float error, bool relative)
		{
			this.m_Error = error;
			this.m_Relative = relative;
		}

		// Token: 0x06002921 RID: 10529 RVA: 0x00043E84 File Offset: 0x00042084
		public bool Equals(float a, float b)
		{
			return this.m_Relative ? FloatComparer.AreEqualRelative(a, b, this.m_Error) : FloatComparer.AreEqual(a, b, this.m_Error);
		}

		// Token: 0x06002922 RID: 10530 RVA: 0x00043EBC File Offset: 0x000420BC
		public int GetHashCode(float obj)
		{
			return base.GetHashCode();
		}

		// Token: 0x06002923 RID: 10531 RVA: 0x00043ED4 File Offset: 0x000420D4
		public static bool AreEqual(float expected, float actual, float error)
		{
			return Math.Abs(actual - expected) <= error;
		}

		// Token: 0x06002924 RID: 10532 RVA: 0x00043EF4 File Offset: 0x000420F4
		public static bool AreEqualRelative(float expected, float actual, float error)
		{
			bool flag = expected == actual;
			bool flag2;
			if (flag)
			{
				flag2 = true;
			}
			else
			{
				float num = Math.Abs(expected);
				float num2 = Math.Abs(actual);
				float num3 = Math.Abs((actual - expected) / ((num > num2) ? num : num2));
				flag2 = num3 <= error;
			}
			return flag2;
		}

		// Token: 0x04000F9A RID: 3994
		private readonly float m_Error;

		// Token: 0x04000F9B RID: 3995
		private readonly bool m_Relative;

		// Token: 0x04000F9C RID: 3996
		public static readonly FloatComparer s_ComparerWithDefaultTolerance = new FloatComparer(1E-05f);

		// Token: 0x04000F9D RID: 3997
		public const float kEpsilon = 1E-05f;
	}
}
