using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000281 RID: 641
	public struct StyleFloat : IStyleValue<float>, IEquatable<StyleFloat>
	{
		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x060014C4 RID: 5316 RVA: 0x0005A4F0 File Offset: 0x000586F0
		// (set) Token: 0x060014C5 RID: 5317 RVA: 0x0005A517 File Offset: 0x00058717
		public float value
		{
			get
			{
				return (this.m_Keyword == StyleKeyword.Undefined) ? this.m_Value : 0f;
			}
			set
			{
				this.m_Value = value;
				this.m_Keyword = StyleKeyword.Undefined;
			}
		}

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x060014C6 RID: 5318 RVA: 0x0005A528 File Offset: 0x00058728
		// (set) Token: 0x060014C7 RID: 5319 RVA: 0x0005A540 File Offset: 0x00058740
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

		// Token: 0x060014C8 RID: 5320 RVA: 0x0005A54A File Offset: 0x0005874A
		public StyleFloat(float v)
		{
			this = new StyleFloat(v, StyleKeyword.Undefined);
		}

		// Token: 0x060014C9 RID: 5321 RVA: 0x0005A556 File Offset: 0x00058756
		public StyleFloat(StyleKeyword keyword)
		{
			this = new StyleFloat(0f, keyword);
		}

		// Token: 0x060014CA RID: 5322 RVA: 0x0005A566 File Offset: 0x00058766
		internal StyleFloat(float v, StyleKeyword keyword)
		{
			this.m_Keyword = keyword;
			this.m_Value = v;
		}

		// Token: 0x060014CB RID: 5323 RVA: 0x0005A578 File Offset: 0x00058778
		public static bool operator ==(StyleFloat lhs, StyleFloat rhs)
		{
			return lhs.m_Keyword == rhs.m_Keyword && lhs.m_Value == rhs.m_Value;
		}

		// Token: 0x060014CC RID: 5324 RVA: 0x0005A5AC File Offset: 0x000587AC
		public static bool operator !=(StyleFloat lhs, StyleFloat rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x060014CD RID: 5325 RVA: 0x0005A5C8 File Offset: 0x000587C8
		public static implicit operator StyleFloat(StyleKeyword keyword)
		{
			return new StyleFloat(keyword);
		}

		// Token: 0x060014CE RID: 5326 RVA: 0x0005A5E0 File Offset: 0x000587E0
		public static implicit operator StyleFloat(float v)
		{
			return new StyleFloat(v);
		}

		// Token: 0x060014CF RID: 5327 RVA: 0x0005A5F8 File Offset: 0x000587F8
		public bool Equals(StyleFloat other)
		{
			return other == this;
		}

		// Token: 0x060014D0 RID: 5328 RVA: 0x0005A618 File Offset: 0x00058818
		public override bool Equals(object obj)
		{
			bool flag;
			if (obj is StyleFloat)
			{
				StyleFloat styleFloat = (StyleFloat)obj;
				flag = this.Equals(styleFloat);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x060014D1 RID: 5329 RVA: 0x0005A644 File Offset: 0x00058844
		public override int GetHashCode()
		{
			return (this.m_Value.GetHashCode() * 397) ^ (int)this.m_Keyword;
		}

		// Token: 0x060014D2 RID: 5330 RVA: 0x0005A670 File Offset: 0x00058870
		public override string ToString()
		{
			return this.DebugString<float>();
		}

		// Token: 0x04000908 RID: 2312
		private float m_Value;

		// Token: 0x04000909 RID: 2313
		private StyleKeyword m_Keyword;
	}
}
