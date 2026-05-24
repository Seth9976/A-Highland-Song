using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200028B RID: 651
	public struct StyleTransformOrigin : IStyleValue<TransformOrigin>, IEquatable<StyleTransformOrigin>
	{
		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x06001563 RID: 5475 RVA: 0x0005B7B4 File Offset: 0x000599B4
		// (set) Token: 0x06001564 RID: 5476 RVA: 0x0005B7DF File Offset: 0x000599DF
		public TransformOrigin value
		{
			get
			{
				return (this.m_Keyword == StyleKeyword.Undefined) ? this.m_Value : default(TransformOrigin);
			}
			set
			{
				this.m_Value = value;
				this.m_Keyword = StyleKeyword.Undefined;
			}
		}

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x06001565 RID: 5477 RVA: 0x0005B7F0 File Offset: 0x000599F0
		// (set) Token: 0x06001566 RID: 5478 RVA: 0x0005B808 File Offset: 0x00059A08
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

		// Token: 0x06001567 RID: 5479 RVA: 0x0005B812 File Offset: 0x00059A12
		public StyleTransformOrigin(TransformOrigin v)
		{
			this = new StyleTransformOrigin(v, StyleKeyword.Undefined);
		}

		// Token: 0x06001568 RID: 5480 RVA: 0x0005B820 File Offset: 0x00059A20
		public StyleTransformOrigin(StyleKeyword keyword)
		{
			this = new StyleTransformOrigin(default(TransformOrigin), keyword);
		}

		// Token: 0x06001569 RID: 5481 RVA: 0x0005B83F File Offset: 0x00059A3F
		internal StyleTransformOrigin(TransformOrigin v, StyleKeyword keyword)
		{
			this.m_Keyword = keyword;
			this.m_Value = v;
		}

		// Token: 0x0600156A RID: 5482 RVA: 0x0005B850 File Offset: 0x00059A50
		public static bool operator ==(StyleTransformOrigin lhs, StyleTransformOrigin rhs)
		{
			return lhs.m_Keyword == rhs.m_Keyword && lhs.m_Value == rhs.m_Value;
		}

		// Token: 0x0600156B RID: 5483 RVA: 0x0005B884 File Offset: 0x00059A84
		public static bool operator !=(StyleTransformOrigin lhs, StyleTransformOrigin rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x0600156C RID: 5484 RVA: 0x0005B8A0 File Offset: 0x00059AA0
		public static implicit operator StyleTransformOrigin(StyleKeyword keyword)
		{
			return new StyleTransformOrigin(keyword);
		}

		// Token: 0x0600156D RID: 5485 RVA: 0x0005B8B8 File Offset: 0x00059AB8
		public static implicit operator StyleTransformOrigin(TransformOrigin v)
		{
			return new StyleTransformOrigin(v);
		}

		// Token: 0x0600156E RID: 5486 RVA: 0x0005B8D0 File Offset: 0x00059AD0
		public bool Equals(StyleTransformOrigin other)
		{
			return other == this;
		}

		// Token: 0x0600156F RID: 5487 RVA: 0x0005B8F0 File Offset: 0x00059AF0
		public override bool Equals(object obj)
		{
			bool flag;
			if (obj is StyleTransformOrigin)
			{
				StyleTransformOrigin styleTransformOrigin = (StyleTransformOrigin)obj;
				flag = this.Equals(styleTransformOrigin);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001570 RID: 5488 RVA: 0x0005B91C File Offset: 0x00059B1C
		public override int GetHashCode()
		{
			return (this.m_Value.GetHashCode() * 397) ^ (int)this.m_Keyword;
		}

		// Token: 0x06001571 RID: 5489 RVA: 0x0005B950 File Offset: 0x00059B50
		public override string ToString()
		{
			return this.DebugString<TransformOrigin>();
		}

		// Token: 0x0400091C RID: 2332
		private TransformOrigin m_Value;

		// Token: 0x0400091D RID: 2333
		private StyleKeyword m_Keyword;
	}
}
