using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000289 RID: 649
	public struct StyleTranslate : IStyleValue<Translate>, IEquatable<StyleTranslate>
	{
		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x06001545 RID: 5445 RVA: 0x0005B408 File Offset: 0x00059608
		// (set) Token: 0x06001546 RID: 5446 RVA: 0x0005B433 File Offset: 0x00059633
		public Translate value
		{
			get
			{
				return (this.m_Keyword == StyleKeyword.Undefined) ? this.m_Value : default(Translate);
			}
			set
			{
				this.m_Value = value;
				this.m_Keyword = StyleKeyword.Undefined;
			}
		}

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x06001547 RID: 5447 RVA: 0x0005B444 File Offset: 0x00059644
		// (set) Token: 0x06001548 RID: 5448 RVA: 0x0005B45C File Offset: 0x0005965C
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

		// Token: 0x06001549 RID: 5449 RVA: 0x0005B466 File Offset: 0x00059666
		public StyleTranslate(Translate v)
		{
			this = new StyleTranslate(v, StyleKeyword.Undefined);
		}

		// Token: 0x0600154A RID: 5450 RVA: 0x0005B474 File Offset: 0x00059674
		public StyleTranslate(StyleKeyword keyword)
		{
			this = new StyleTranslate(default(Translate), keyword);
		}

		// Token: 0x0600154B RID: 5451 RVA: 0x0005B493 File Offset: 0x00059693
		internal StyleTranslate(Translate v, StyleKeyword keyword)
		{
			this.m_Keyword = keyword;
			this.m_Value = v;
		}

		// Token: 0x0600154C RID: 5452 RVA: 0x0005B4A4 File Offset: 0x000596A4
		public static bool operator ==(StyleTranslate lhs, StyleTranslate rhs)
		{
			return lhs.m_Keyword == rhs.m_Keyword && lhs.m_Value == rhs.m_Value;
		}

		// Token: 0x0600154D RID: 5453 RVA: 0x0005B4D8 File Offset: 0x000596D8
		public static bool operator !=(StyleTranslate lhs, StyleTranslate rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x0600154E RID: 5454 RVA: 0x0005B4F4 File Offset: 0x000596F4
		public static implicit operator StyleTranslate(StyleKeyword keyword)
		{
			return new StyleTranslate(keyword);
		}

		// Token: 0x0600154F RID: 5455 RVA: 0x0005B50C File Offset: 0x0005970C
		public static implicit operator StyleTranslate(Translate v)
		{
			return new StyleTranslate(v);
		}

		// Token: 0x06001550 RID: 5456 RVA: 0x0005B524 File Offset: 0x00059724
		public bool Equals(StyleTranslate other)
		{
			return other == this;
		}

		// Token: 0x06001551 RID: 5457 RVA: 0x0005B544 File Offset: 0x00059744
		public override bool Equals(object obj)
		{
			bool flag;
			if (obj is StyleTranslate)
			{
				StyleTranslate styleTranslate = (StyleTranslate)obj;
				flag = this.Equals(styleTranslate);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001552 RID: 5458 RVA: 0x0005B570 File Offset: 0x00059770
		public override int GetHashCode()
		{
			return (this.m_Value.GetHashCode() * 397) ^ (int)this.m_Keyword;
		}

		// Token: 0x06001553 RID: 5459 RVA: 0x0005B5A4 File Offset: 0x000597A4
		public override string ToString()
		{
			return this.DebugString<Translate>();
		}

		// Token: 0x04000918 RID: 2328
		private Translate m_Value;

		// Token: 0x04000919 RID: 2329
		private StyleKeyword m_Keyword;
	}
}
