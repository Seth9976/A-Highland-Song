using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000288 RID: 648
	public struct StyleScale : IStyleValue<Scale>, IEquatable<StyleScale>
	{
		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x06001536 RID: 5430 RVA: 0x0005B248 File Offset: 0x00059448
		// (set) Token: 0x06001537 RID: 5431 RVA: 0x0005B273 File Offset: 0x00059473
		public Scale value
		{
			get
			{
				return (this.m_Keyword == StyleKeyword.Undefined) ? this.m_Value : default(Scale);
			}
			set
			{
				this.m_Value = value;
				this.m_Keyword = StyleKeyword.Undefined;
			}
		}

		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x06001538 RID: 5432 RVA: 0x0005B284 File Offset: 0x00059484
		// (set) Token: 0x06001539 RID: 5433 RVA: 0x0005B29C File Offset: 0x0005949C
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

		// Token: 0x0600153A RID: 5434 RVA: 0x0005B2A6 File Offset: 0x000594A6
		public StyleScale(Scale v)
		{
			this = new StyleScale(v, StyleKeyword.Undefined);
		}

		// Token: 0x0600153B RID: 5435 RVA: 0x0005B2B4 File Offset: 0x000594B4
		public StyleScale(StyleKeyword keyword)
		{
			this = new StyleScale(default(Scale), keyword);
		}

		// Token: 0x0600153C RID: 5436 RVA: 0x0005B2D3 File Offset: 0x000594D3
		internal StyleScale(Scale v, StyleKeyword keyword)
		{
			this.m_Keyword = keyword;
			this.m_Value = v;
		}

		// Token: 0x0600153D RID: 5437 RVA: 0x0005B2E4 File Offset: 0x000594E4
		public static bool operator ==(StyleScale lhs, StyleScale rhs)
		{
			return lhs.m_Keyword == rhs.m_Keyword && lhs.m_Value == rhs.m_Value;
		}

		// Token: 0x0600153E RID: 5438 RVA: 0x0005B318 File Offset: 0x00059518
		public static bool operator !=(StyleScale lhs, StyleScale rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x0600153F RID: 5439 RVA: 0x0005B334 File Offset: 0x00059534
		public static implicit operator StyleScale(StyleKeyword keyword)
		{
			return new StyleScale(keyword);
		}

		// Token: 0x06001540 RID: 5440 RVA: 0x0005B34C File Offset: 0x0005954C
		public static implicit operator StyleScale(Scale v)
		{
			return new StyleScale(v);
		}

		// Token: 0x06001541 RID: 5441 RVA: 0x0005B364 File Offset: 0x00059564
		public bool Equals(StyleScale other)
		{
			return other == this;
		}

		// Token: 0x06001542 RID: 5442 RVA: 0x0005B384 File Offset: 0x00059584
		public override bool Equals(object obj)
		{
			bool flag;
			if (obj is StyleScale)
			{
				StyleScale styleScale = (StyleScale)obj;
				flag = this.Equals(styleScale);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001543 RID: 5443 RVA: 0x0005B3B0 File Offset: 0x000595B0
		public override int GetHashCode()
		{
			return (this.m_Value.GetHashCode() * 397) ^ (int)this.m_Keyword;
		}

		// Token: 0x06001544 RID: 5444 RVA: 0x0005B3E4 File Offset: 0x000595E4
		public override string ToString()
		{
			return this.DebugString<Scale>();
		}

		// Token: 0x04000916 RID: 2326
		private Scale m_Value;

		// Token: 0x04000917 RID: 2327
		private StyleKeyword m_Keyword;
	}
}
