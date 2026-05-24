using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000285 RID: 645
	public struct StyleLength : IStyleValue<Length>, IEquatable<StyleLength>
	{
		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x06001507 RID: 5383 RVA: 0x0005AC24 File Offset: 0x00058E24
		// (set) Token: 0x06001508 RID: 5384 RVA: 0x0005AC4F File Offset: 0x00058E4F
		public Length value
		{
			get
			{
				return (this.m_Keyword == StyleKeyword.Undefined) ? this.m_Value : default(Length);
			}
			set
			{
				this.m_Value = value;
				this.m_Keyword = StyleKeyword.Undefined;
			}
		}

		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x06001509 RID: 5385 RVA: 0x0005AC60 File Offset: 0x00058E60
		// (set) Token: 0x0600150A RID: 5386 RVA: 0x0005AC78 File Offset: 0x00058E78
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

		// Token: 0x0600150B RID: 5387 RVA: 0x0005AC82 File Offset: 0x00058E82
		public StyleLength(float v)
		{
			this = new StyleLength(new Length(v, LengthUnit.Pixel), StyleKeyword.Undefined);
		}

		// Token: 0x0600150C RID: 5388 RVA: 0x0005AC94 File Offset: 0x00058E94
		public StyleLength(Length v)
		{
			this = new StyleLength(v, StyleKeyword.Undefined);
		}

		// Token: 0x0600150D RID: 5389 RVA: 0x0005ACA0 File Offset: 0x00058EA0
		public StyleLength(StyleKeyword keyword)
		{
			this = new StyleLength(default(Length), keyword);
		}

		// Token: 0x0600150E RID: 5390 RVA: 0x0005ACC0 File Offset: 0x00058EC0
		internal StyleLength(Length v, StyleKeyword keyword)
		{
			this.m_Keyword = keyword;
			this.m_Value = v;
			bool flag = v.IsAuto();
			if (flag)
			{
				this.m_Keyword = StyleKeyword.Auto;
			}
			else
			{
				bool flag2 = v.IsNone();
				if (flag2)
				{
					this.m_Keyword = StyleKeyword.None;
				}
			}
		}

		// Token: 0x0600150F RID: 5391 RVA: 0x0005AD04 File Offset: 0x00058F04
		public static bool operator ==(StyleLength lhs, StyleLength rhs)
		{
			return lhs.m_Keyword == rhs.m_Keyword && lhs.m_Value == rhs.m_Value;
		}

		// Token: 0x06001510 RID: 5392 RVA: 0x0005AD38 File Offset: 0x00058F38
		public static bool operator !=(StyleLength lhs, StyleLength rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06001511 RID: 5393 RVA: 0x0005AD54 File Offset: 0x00058F54
		public static implicit operator StyleLength(StyleKeyword keyword)
		{
			return new StyleLength(keyword);
		}

		// Token: 0x06001512 RID: 5394 RVA: 0x0005AD6C File Offset: 0x00058F6C
		public static implicit operator StyleLength(float v)
		{
			return new StyleLength(v);
		}

		// Token: 0x06001513 RID: 5395 RVA: 0x0005AD84 File Offset: 0x00058F84
		public static implicit operator StyleLength(Length v)
		{
			return new StyleLength(v);
		}

		// Token: 0x06001514 RID: 5396 RVA: 0x0005AD9C File Offset: 0x00058F9C
		public bool Equals(StyleLength other)
		{
			return other == this;
		}

		// Token: 0x06001515 RID: 5397 RVA: 0x0005ADBC File Offset: 0x00058FBC
		public override bool Equals(object obj)
		{
			bool flag;
			if (obj is StyleLength)
			{
				StyleLength styleLength = (StyleLength)obj;
				flag = this.Equals(styleLength);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001516 RID: 5398 RVA: 0x0005ADE8 File Offset: 0x00058FE8
		public override int GetHashCode()
		{
			return (this.m_Value.GetHashCode() * 397) ^ (int)this.m_Keyword;
		}

		// Token: 0x06001517 RID: 5399 RVA: 0x0005AE1C File Offset: 0x0005901C
		public override string ToString()
		{
			return this.DebugString<Length>();
		}

		// Token: 0x04000910 RID: 2320
		private Length m_Value;

		// Token: 0x04000911 RID: 2321
		private StyleKeyword m_Keyword;
	}
}
