using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200027C RID: 636
	public struct StyleCursor : IStyleValue<Cursor>, IEquatable<StyleCursor>
	{
		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x06001481 RID: 5249 RVA: 0x00057AA8 File Offset: 0x00055CA8
		// (set) Token: 0x06001482 RID: 5250 RVA: 0x00057AD3 File Offset: 0x00055CD3
		public Cursor value
		{
			get
			{
				return (this.m_Keyword == StyleKeyword.Undefined) ? this.m_Value : default(Cursor);
			}
			set
			{
				this.m_Value = value;
				this.m_Keyword = StyleKeyword.Undefined;
			}
		}

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x06001483 RID: 5251 RVA: 0x00057AE4 File Offset: 0x00055CE4
		// (set) Token: 0x06001484 RID: 5252 RVA: 0x00057AFC File Offset: 0x00055CFC
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

		// Token: 0x06001485 RID: 5253 RVA: 0x00057B06 File Offset: 0x00055D06
		public StyleCursor(Cursor v)
		{
			this = new StyleCursor(v, StyleKeyword.Undefined);
		}

		// Token: 0x06001486 RID: 5254 RVA: 0x00057B14 File Offset: 0x00055D14
		public StyleCursor(StyleKeyword keyword)
		{
			this = new StyleCursor(default(Cursor), keyword);
		}

		// Token: 0x06001487 RID: 5255 RVA: 0x00057B33 File Offset: 0x00055D33
		internal StyleCursor(Cursor v, StyleKeyword keyword)
		{
			this.m_Keyword = keyword;
			this.m_Value = v;
		}

		// Token: 0x06001488 RID: 5256 RVA: 0x00057B44 File Offset: 0x00055D44
		public static bool operator ==(StyleCursor lhs, StyleCursor rhs)
		{
			return lhs.m_Keyword == rhs.m_Keyword && lhs.m_Value == rhs.m_Value;
		}

		// Token: 0x06001489 RID: 5257 RVA: 0x00057B78 File Offset: 0x00055D78
		public static bool operator !=(StyleCursor lhs, StyleCursor rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x0600148A RID: 5258 RVA: 0x00057B94 File Offset: 0x00055D94
		public static implicit operator StyleCursor(StyleKeyword keyword)
		{
			return new StyleCursor(keyword);
		}

		// Token: 0x0600148B RID: 5259 RVA: 0x00057BAC File Offset: 0x00055DAC
		public static implicit operator StyleCursor(Cursor v)
		{
			return new StyleCursor(v);
		}

		// Token: 0x0600148C RID: 5260 RVA: 0x00057BC4 File Offset: 0x00055DC4
		public bool Equals(StyleCursor other)
		{
			return other == this;
		}

		// Token: 0x0600148D RID: 5261 RVA: 0x00057BE4 File Offset: 0x00055DE4
		public override bool Equals(object obj)
		{
			bool flag;
			if (obj is StyleCursor)
			{
				StyleCursor styleCursor = (StyleCursor)obj;
				flag = this.Equals(styleCursor);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x0600148E RID: 5262 RVA: 0x00057C10 File Offset: 0x00055E10
		public override int GetHashCode()
		{
			return (this.m_Value.GetHashCode() * 397) ^ (int)this.m_Keyword;
		}

		// Token: 0x0600148F RID: 5263 RVA: 0x00057C44 File Offset: 0x00055E44
		public override string ToString()
		{
			return this.DebugString<Cursor>();
		}

		// Token: 0x040008FB RID: 2299
		private Cursor m_Value;

		// Token: 0x040008FC RID: 2300
		private StyleKeyword m_Keyword;
	}
}
