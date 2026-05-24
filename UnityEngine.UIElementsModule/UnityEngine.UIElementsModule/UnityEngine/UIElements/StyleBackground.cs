using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200027A RID: 634
	public struct StyleBackground : IStyleValue<Background>, IEquatable<StyleBackground>
	{
		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x0600145A RID: 5210 RVA: 0x0005768C File Offset: 0x0005588C
		// (set) Token: 0x0600145B RID: 5211 RVA: 0x000576B7 File Offset: 0x000558B7
		public Background value
		{
			get
			{
				return (this.m_Keyword == StyleKeyword.Undefined) ? this.m_Value : default(Background);
			}
			set
			{
				this.m_Value = value;
				this.m_Keyword = StyleKeyword.Undefined;
			}
		}

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x0600145C RID: 5212 RVA: 0x000576C8 File Offset: 0x000558C8
		// (set) Token: 0x0600145D RID: 5213 RVA: 0x000576E0 File Offset: 0x000558E0
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

		// Token: 0x0600145E RID: 5214 RVA: 0x000576EA File Offset: 0x000558EA
		public StyleBackground(Background v)
		{
			this = new StyleBackground(v, StyleKeyword.Undefined);
		}

		// Token: 0x0600145F RID: 5215 RVA: 0x000576F6 File Offset: 0x000558F6
		public StyleBackground(Texture2D v)
		{
			this = new StyleBackground(v, StyleKeyword.Undefined);
		}

		// Token: 0x06001460 RID: 5216 RVA: 0x00057702 File Offset: 0x00055902
		public StyleBackground(Sprite v)
		{
			this = new StyleBackground(v, StyleKeyword.Undefined);
		}

		// Token: 0x06001461 RID: 5217 RVA: 0x0005770E File Offset: 0x0005590E
		public StyleBackground(VectorImage v)
		{
			this = new StyleBackground(v, StyleKeyword.Undefined);
		}

		// Token: 0x06001462 RID: 5218 RVA: 0x0005771C File Offset: 0x0005591C
		public StyleBackground(StyleKeyword keyword)
		{
			this = new StyleBackground(default(Background), keyword);
		}

		// Token: 0x06001463 RID: 5219 RVA: 0x0005773B File Offset: 0x0005593B
		internal StyleBackground(Texture2D v, StyleKeyword keyword)
		{
			this = new StyleBackground(Background.FromTexture2D(v), keyword);
		}

		// Token: 0x06001464 RID: 5220 RVA: 0x0005774C File Offset: 0x0005594C
		internal StyleBackground(Sprite v, StyleKeyword keyword)
		{
			this = new StyleBackground(Background.FromSprite(v), keyword);
		}

		// Token: 0x06001465 RID: 5221 RVA: 0x0005775D File Offset: 0x0005595D
		internal StyleBackground(VectorImage v, StyleKeyword keyword)
		{
			this = new StyleBackground(Background.FromVectorImage(v), keyword);
		}

		// Token: 0x06001466 RID: 5222 RVA: 0x0005776E File Offset: 0x0005596E
		internal StyleBackground(Background v, StyleKeyword keyword)
		{
			this.m_Keyword = keyword;
			this.m_Value = v;
		}

		// Token: 0x06001467 RID: 5223 RVA: 0x00057780 File Offset: 0x00055980
		public static bool operator ==(StyleBackground lhs, StyleBackground rhs)
		{
			return lhs.m_Keyword == rhs.m_Keyword && lhs.m_Value == rhs.m_Value;
		}

		// Token: 0x06001468 RID: 5224 RVA: 0x000577B4 File Offset: 0x000559B4
		public static bool operator !=(StyleBackground lhs, StyleBackground rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06001469 RID: 5225 RVA: 0x000577D0 File Offset: 0x000559D0
		public static implicit operator StyleBackground(StyleKeyword keyword)
		{
			return new StyleBackground(keyword);
		}

		// Token: 0x0600146A RID: 5226 RVA: 0x000577E8 File Offset: 0x000559E8
		public static implicit operator StyleBackground(Background v)
		{
			return new StyleBackground(v);
		}

		// Token: 0x0600146B RID: 5227 RVA: 0x00057800 File Offset: 0x00055A00
		public static implicit operator StyleBackground(Texture2D v)
		{
			return new StyleBackground(v);
		}

		// Token: 0x0600146C RID: 5228 RVA: 0x00057818 File Offset: 0x00055A18
		public bool Equals(StyleBackground other)
		{
			return other == this;
		}

		// Token: 0x0600146D RID: 5229 RVA: 0x00057838 File Offset: 0x00055A38
		public override bool Equals(object obj)
		{
			bool flag;
			if (obj is StyleBackground)
			{
				StyleBackground styleBackground = (StyleBackground)obj;
				flag = this.Equals(styleBackground);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x0600146E RID: 5230 RVA: 0x00057864 File Offset: 0x00055A64
		public override int GetHashCode()
		{
			return (this.m_Value.GetHashCode() * 397) ^ (int)this.m_Keyword;
		}

		// Token: 0x0600146F RID: 5231 RVA: 0x00057898 File Offset: 0x00055A98
		public override string ToString()
		{
			return this.DebugString<Background>();
		}

		// Token: 0x040008F7 RID: 2295
		private Background m_Value;

		// Token: 0x040008F8 RID: 2296
		private StyleKeyword m_Keyword;
	}
}
