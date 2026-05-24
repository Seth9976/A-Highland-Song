using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000284 RID: 644
	public struct StyleInt : IStyleValue<int>, IEquatable<StyleInt>
	{
		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x060014F8 RID: 5368 RVA: 0x0005AA8C File Offset: 0x00058C8C
		// (set) Token: 0x060014F9 RID: 5369 RVA: 0x0005AAAF File Offset: 0x00058CAF
		public int value
		{
			get
			{
				return (this.m_Keyword == StyleKeyword.Undefined) ? this.m_Value : 0;
			}
			set
			{
				this.m_Value = value;
				this.m_Keyword = StyleKeyword.Undefined;
			}
		}

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x060014FA RID: 5370 RVA: 0x0005AAC0 File Offset: 0x00058CC0
		// (set) Token: 0x060014FB RID: 5371 RVA: 0x0005AAD8 File Offset: 0x00058CD8
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

		// Token: 0x060014FC RID: 5372 RVA: 0x0005AAE2 File Offset: 0x00058CE2
		public StyleInt(int v)
		{
			this = new StyleInt(v, StyleKeyword.Undefined);
		}

		// Token: 0x060014FD RID: 5373 RVA: 0x0005AAEE File Offset: 0x00058CEE
		public StyleInt(StyleKeyword keyword)
		{
			this = new StyleInt(0, keyword);
		}

		// Token: 0x060014FE RID: 5374 RVA: 0x0005AAFA File Offset: 0x00058CFA
		internal StyleInt(int v, StyleKeyword keyword)
		{
			this.m_Keyword = keyword;
			this.m_Value = v;
		}

		// Token: 0x060014FF RID: 5375 RVA: 0x0005AB0C File Offset: 0x00058D0C
		public static bool operator ==(StyleInt lhs, StyleInt rhs)
		{
			return lhs.m_Keyword == rhs.m_Keyword && lhs.m_Value == rhs.m_Value;
		}

		// Token: 0x06001500 RID: 5376 RVA: 0x0005AB40 File Offset: 0x00058D40
		public static bool operator !=(StyleInt lhs, StyleInt rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06001501 RID: 5377 RVA: 0x0005AB5C File Offset: 0x00058D5C
		public static implicit operator StyleInt(StyleKeyword keyword)
		{
			return new StyleInt(keyword);
		}

		// Token: 0x06001502 RID: 5378 RVA: 0x0005AB74 File Offset: 0x00058D74
		public static implicit operator StyleInt(int v)
		{
			return new StyleInt(v);
		}

		// Token: 0x06001503 RID: 5379 RVA: 0x0005AB8C File Offset: 0x00058D8C
		public bool Equals(StyleInt other)
		{
			return other == this;
		}

		// Token: 0x06001504 RID: 5380 RVA: 0x0005ABAC File Offset: 0x00058DAC
		public override bool Equals(object obj)
		{
			bool flag;
			if (obj is StyleInt)
			{
				StyleInt styleInt = (StyleInt)obj;
				flag = this.Equals(styleInt);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001505 RID: 5381 RVA: 0x0005ABD8 File Offset: 0x00058DD8
		public override int GetHashCode()
		{
			return (this.m_Value * 397) ^ (int)this.m_Keyword;
		}

		// Token: 0x06001506 RID: 5382 RVA: 0x0005AC00 File Offset: 0x00058E00
		public override string ToString()
		{
			return this.DebugString<int>();
		}

		// Token: 0x0400090E RID: 2318
		private int m_Value;

		// Token: 0x0400090F RID: 2319
		private StyleKeyword m_Keyword;
	}
}
