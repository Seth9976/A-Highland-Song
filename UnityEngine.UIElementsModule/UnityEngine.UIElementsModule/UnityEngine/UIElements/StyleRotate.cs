using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000287 RID: 647
	public struct StyleRotate : IStyleValue<Rotate>, IEquatable<StyleRotate>
	{
		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x06001527 RID: 5415 RVA: 0x0005B088 File Offset: 0x00059288
		// (set) Token: 0x06001528 RID: 5416 RVA: 0x0005B0B3 File Offset: 0x000592B3
		public Rotate value
		{
			get
			{
				return (this.m_Keyword == StyleKeyword.Undefined) ? this.m_Value : default(Rotate);
			}
			set
			{
				this.m_Value = value;
				this.m_Keyword = StyleKeyword.Undefined;
			}
		}

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x06001529 RID: 5417 RVA: 0x0005B0C4 File Offset: 0x000592C4
		// (set) Token: 0x0600152A RID: 5418 RVA: 0x0005B0DC File Offset: 0x000592DC
		public StyleKeyword keyword
		{
			get
			{
				return this.m_Keyword;
			}
			set
			{
				this.m_Keyword = value;
			}
		}

		// Token: 0x0600152B RID: 5419 RVA: 0x0005B0E6 File Offset: 0x000592E6
		public StyleRotate(Rotate v)
		{
			this = new StyleRotate(v, StyleKeyword.Undefined);
		}

		// Token: 0x0600152C RID: 5420 RVA: 0x0005B0F4 File Offset: 0x000592F4
		public StyleRotate(StyleKeyword keyword)
		{
			this = new StyleRotate(default(Rotate), keyword);
		}

		// Token: 0x0600152D RID: 5421 RVA: 0x0005B113 File Offset: 0x00059313
		internal StyleRotate(Rotate v, StyleKeyword keyword)
		{
			this.m_Keyword = keyword;
			this.m_Value = v;
		}

		// Token: 0x0600152E RID: 5422 RVA: 0x0005B124 File Offset: 0x00059324
		public static bool operator ==(StyleRotate lhs, StyleRotate rhs)
		{
			return lhs.m_Keyword == rhs.m_Keyword && lhs.m_Value == rhs.m_Value;
		}

		// Token: 0x0600152F RID: 5423 RVA: 0x0005B158 File Offset: 0x00059358
		public static bool operator !=(StyleRotate lhs, StyleRotate rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06001530 RID: 5424 RVA: 0x0005B174 File Offset: 0x00059374
		public static implicit operator StyleRotate(StyleKeyword keyword)
		{
			return new StyleRotate(keyword);
		}

		// Token: 0x06001531 RID: 5425 RVA: 0x0005B18C File Offset: 0x0005938C
		public static implicit operator StyleRotate(Rotate v)
		{
			return new StyleRotate(v);
		}

		// Token: 0x06001532 RID: 5426 RVA: 0x0005B1A4 File Offset: 0x000593A4
		public bool Equals(StyleRotate other)
		{
			return other == this;
		}

		// Token: 0x06001533 RID: 5427 RVA: 0x0005B1C4 File Offset: 0x000593C4
		public override bool Equals(object obj)
		{
			bool flag;
			if (obj is StyleRotate)
			{
				StyleRotate styleRotate = (StyleRotate)obj;
				flag = this.Equals(styleRotate);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001534 RID: 5428 RVA: 0x0005B1F0 File Offset: 0x000593F0
		public override int GetHashCode()
		{
			return (this.m_Value.GetHashCode() * 397) ^ (int)this.m_Keyword;
		}

		// Token: 0x06001535 RID: 5429 RVA: 0x0005B224 File Offset: 0x00059424
		public override string ToString()
		{
			return this.DebugString<Rotate>();
		}

		// Token: 0x04000914 RID: 2324
		private Rotate m_Value;

		// Token: 0x04000915 RID: 2325
		private StyleKeyword m_Keyword;
	}
}
