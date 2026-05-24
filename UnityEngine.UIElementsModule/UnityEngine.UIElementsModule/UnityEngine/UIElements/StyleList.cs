using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.UIElements
{
	// Token: 0x02000286 RID: 646
	public struct StyleList<T> : IStyleValue<List<T>>, IEquatable<StyleList<T>>
	{
		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x06001518 RID: 5400 RVA: 0x0005AE40 File Offset: 0x00059040
		// (set) Token: 0x06001519 RID: 5401 RVA: 0x0005AE63 File Offset: 0x00059063
		public List<T> value
		{
			get
			{
				return (this.m_Keyword == StyleKeyword.Undefined) ? this.m_Value : null;
			}
			set
			{
				this.m_Value = value;
				this.m_Keyword = StyleKeyword.Undefined;
			}
		}

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x0600151A RID: 5402 RVA: 0x0005AE74 File Offset: 0x00059074
		// (set) Token: 0x0600151B RID: 5403 RVA: 0x0005AE8C File Offset: 0x0005908C
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

		// Token: 0x0600151C RID: 5404 RVA: 0x0005AE96 File Offset: 0x00059096
		public StyleList(List<T> v)
		{
			this = new StyleList<T>(v, StyleKeyword.Undefined);
		}

		// Token: 0x0600151D RID: 5405 RVA: 0x0005AEA2 File Offset: 0x000590A2
		public StyleList(StyleKeyword keyword)
		{
			this = new StyleList<T>(null, keyword);
		}

		// Token: 0x0600151E RID: 5406 RVA: 0x0005AEAE File Offset: 0x000590AE
		internal StyleList(List<T> v, StyleKeyword keyword)
		{
			this.m_Keyword = keyword;
			this.m_Value = v;
		}

		// Token: 0x0600151F RID: 5407 RVA: 0x0005AEC0 File Offset: 0x000590C0
		public static bool operator ==(StyleList<T> lhs, StyleList<T> rhs)
		{
			bool flag = lhs.m_Keyword != rhs.m_Keyword;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				List<T> value = lhs.m_Value;
				List<T> value2 = rhs.m_Value;
				bool flag3 = value == value2;
				if (flag3)
				{
					flag2 = true;
				}
				else
				{
					bool flag4 = value == null || value2 == null;
					flag2 = !flag4 && value.Count == value2.Count && Enumerable.SequenceEqual<T>(value, value2);
				}
			}
			return flag2;
		}

		// Token: 0x06001520 RID: 5408 RVA: 0x0005AF34 File Offset: 0x00059134
		public static bool operator !=(StyleList<T> lhs, StyleList<T> rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06001521 RID: 5409 RVA: 0x0005AF50 File Offset: 0x00059150
		public static implicit operator StyleList<T>(StyleKeyword keyword)
		{
			return new StyleList<T>(keyword);
		}

		// Token: 0x06001522 RID: 5410 RVA: 0x0005AF68 File Offset: 0x00059168
		public static implicit operator StyleList<T>(List<T> v)
		{
			return new StyleList<T>(v);
		}

		// Token: 0x06001523 RID: 5411 RVA: 0x0005AF80 File Offset: 0x00059180
		public bool Equals(StyleList<T> other)
		{
			return other == this;
		}

		// Token: 0x06001524 RID: 5412 RVA: 0x0005AFA0 File Offset: 0x000591A0
		public override bool Equals(object obj)
		{
			bool flag;
			if (obj is StyleList<T>)
			{
				StyleList<T> styleList = (StyleList<T>)obj;
				flag = this.Equals(styleList);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001525 RID: 5413 RVA: 0x0005AFCC File Offset: 0x000591CC
		public override int GetHashCode()
		{
			int num = 0;
			bool flag = this.m_Value != null && this.m_Value.Count > 0;
			if (flag)
			{
				num = EqualityComparer<T>.Default.GetHashCode(this.m_Value[0]);
				for (int i = 1; i < this.m_Value.Count; i++)
				{
					num = (num * 397) ^ EqualityComparer<T>.Default.GetHashCode(this.m_Value[i]);
				}
			}
			return (num * 397) ^ (int)this.m_Keyword;
		}

		// Token: 0x06001526 RID: 5414 RVA: 0x0005B064 File Offset: 0x00059264
		public override string ToString()
		{
			return this.DebugString<List<T>>();
		}

		// Token: 0x04000912 RID: 2322
		private StyleKeyword m_Keyword;

		// Token: 0x04000913 RID: 2323
		private List<T> m_Value;
	}
}
