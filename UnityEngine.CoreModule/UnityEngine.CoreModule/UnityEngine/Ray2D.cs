using System;
using System.Globalization;

namespace UnityEngine
{
	// Token: 0x02000115 RID: 277
	public struct Ray2D : IFormattable
	{
		// Token: 0x060006FD RID: 1789 RVA: 0x0000A3AF File Offset: 0x000085AF
		public Ray2D(Vector2 origin, Vector2 direction)
		{
			this.m_Origin = origin;
			this.m_Direction = direction.normalized;
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x060006FE RID: 1790 RVA: 0x0000A3C8 File Offset: 0x000085C8
		// (set) Token: 0x060006FF RID: 1791 RVA: 0x0000A3E0 File Offset: 0x000085E0
		public Vector2 origin
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

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000700 RID: 1792 RVA: 0x0000A3EC File Offset: 0x000085EC
		// (set) Token: 0x06000701 RID: 1793 RVA: 0x0000A404 File Offset: 0x00008604
		public Vector2 direction
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

		// Token: 0x06000702 RID: 1794 RVA: 0x0000A414 File Offset: 0x00008614
		public Vector2 GetPoint(float distance)
		{
			return this.m_Origin + this.m_Direction * distance;
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x0000A440 File Offset: 0x00008640
		public override string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x0000A45C File Offset: 0x0000865C
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x0000A478 File Offset: 0x00008678
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

		// Token: 0x04000392 RID: 914
		private Vector2 m_Origin;

		// Token: 0x04000393 RID: 915
		private Vector2 m_Direction;
	}
}
