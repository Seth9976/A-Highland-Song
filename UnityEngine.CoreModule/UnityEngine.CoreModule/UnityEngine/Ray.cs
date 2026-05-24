using System;
using System.Globalization;

namespace UnityEngine
{
	// Token: 0x02000114 RID: 276
	public struct Ray : IFormattable
	{
		// Token: 0x060006F4 RID: 1780 RVA: 0x0000A27F File Offset: 0x0000847F
		public Ray(Vector3 origin, Vector3 direction)
		{
			this.m_Origin = origin;
			this.m_Direction = direction.normalized;
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x060006F5 RID: 1781 RVA: 0x0000A298 File Offset: 0x00008498
		// (set) Token: 0x060006F6 RID: 1782 RVA: 0x0000A2B0 File Offset: 0x000084B0
		public Vector3 origin
		{
			get
			{
				return this.m_Origin;
			}
			set
			{
				this.m_Origin = value;
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x060006F7 RID: 1783 RVA: 0x0000A2BC File Offset: 0x000084BC
		// (set) Token: 0x060006F8 RID: 1784 RVA: 0x0000A2D4 File Offset: 0x000084D4
		public Vector3 direction
		{
			get
			{
				return this.m_Direction;
			}
			set
			{
				this.m_Direction = value.normalized;
			}
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public Vector3 GetPoint(float distance)
		{
			return this.m_Origin + this.m_Direction * distance;
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x0000A310 File Offset: 0x00008510
		public override string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x0000A32C File Offset: 0x0000852C
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x0000A348 File Offset: 0x00008548
		public string ToString(string format, IFormatProvider formatProvider)
		{
			bool flag = string.IsNullOrEmpty(format);
			if (flag)
			{
				format = "F2";
			}
			bool flag2 = formatProvider == null;
			if (flag2)
			{
				formatProvider = CultureInfo.InvariantCulture.NumberFormat;
			}
			return UnityString.Format("Origin: {0}, Dir: {1}", new object[]
			{
				this.m_Origin.ToString(format, formatProvider),
				this.m_Direction.ToString(format, formatProvider)
			});
		}

		// Token: 0x04000390 RID: 912
		private Vector3 m_Origin;

		// Token: 0x04000391 RID: 913
		private Vector3 m_Direction;
	}
}
