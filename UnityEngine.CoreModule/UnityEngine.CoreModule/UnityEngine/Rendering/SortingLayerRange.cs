using System;

namespace UnityEngine.Rendering
{
	// Token: 0x02000413 RID: 1043
	public struct SortingLayerRange : IEquatable<SortingLayerRange>
	{
		// Token: 0x060023F0 RID: 9200 RVA: 0x0003CB7E File Offset: 0x0003AD7E
		public SortingLayerRange(short lowerBound, short upperBound)
		{
			this.m_LowerBound = lowerBound;
			this.m_UpperBound = upperBound;
		}

		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x060023F1 RID: 9201 RVA: 0x0003CB90 File Offset: 0x0003AD90
		// (set) Token: 0x060023F2 RID: 9202 RVA: 0x0003CBA8 File Offset: 0x0003ADA8
		public short lowerBound
		{
			get
			{
				return this.m_LowerBound;
			}
			set
			{
				this.m_LowerBound = value;
			}
		}

		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x060023F3 RID: 9203 RVA: 0x0003CBB4 File Offset: 0x0003ADB4
		// (set) Token: 0x060023F4 RID: 9204 RVA: 0x0003CBCC File Offset: 0x0003ADCC
		public short upperBound
		{
			get
			{
				return this.m_UpperBound;
			}
			set
			{
				this.m_UpperBound = value;
			}
		}

		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x060023F5 RID: 9205 RVA: 0x0003CBD8 File Offset: 0x0003ADD8
		public static SortingLayerRange all
		{
			get
			{
				return new SortingLayerRange
				{
					m_LowerBound = short.MinValue,
					m_UpperBound = short.MaxValue
				};
			}
		}

		// Token: 0x060023F6 RID: 9206 RVA: 0x0003CC08 File Offset: 0x0003AE08
		public bool Equals(SortingLayerRange other)
		{
			return this.m_LowerBound == other.m_LowerBound && this.m_UpperBound == other.m_UpperBound;
		}

		// Token: 0x060023F7 RID: 9207 RVA: 0x0003CC3C File Offset: 0x0003AE3C
		public override bool Equals(object obj)
		{
			bool flag = !(obj is SortingLayerRange);
			return !flag && this.Equals((SortingLayerRange)obj);
		}

		// Token: 0x060023F8 RID: 9208 RVA: 0x0003CC70 File Offset: 0x0003AE70
		public static bool operator !=(SortingLayerRange lhs, SortingLayerRange rhs)
		{
			return !lhs.Equals(rhs);
		}

		// Token: 0x060023F9 RID: 9209 RVA: 0x0003CC90 File Offset: 0x0003AE90
		public static bool operator ==(SortingLayerRange lhs, SortingLayerRange rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x060023FA RID: 9210 RVA: 0x0003CCAC File Offset: 0x0003AEAC
		public override int GetHashCode()
		{
			return ((int)this.m_UpperBound << 16) | ((int)this.m_LowerBound & 65535);
		}

		// Token: 0x04000D4D RID: 3405
		private short m_LowerBound;

		// Token: 0x04000D4E RID: 3406
		private short m_UpperBound;
	}
}
