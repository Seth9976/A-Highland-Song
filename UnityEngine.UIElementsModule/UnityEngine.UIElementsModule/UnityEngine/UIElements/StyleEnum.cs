using System;
using Unity.Collections.LowLevel.Unsafe;

namespace UnityEngine.UIElements
{
	// Token: 0x02000280 RID: 640
	public struct StyleEnum<T> : IStyleValue<T>, IEquatable<StyleEnum<T>> where T : struct, IConvertible
	{
		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x060014B5 RID: 5301 RVA: 0x0005A338 File Offset: 0x00058538
		// (set) Token: 0x060014B6 RID: 5302 RVA: 0x0005A363 File Offset: 0x00058563
		public T value
		{
			get
			{
				return (this.m_Keyword == StyleKeyword.Undefined) ? this.m_Value : default(T);
			}
			set
			{
				this.m_Value = value;
				this.m_Keyword = StyleKeyword.Undefined;
			}
		}

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x060014B7 RID: 5303 RVA: 0x0005A374 File Offset: 0x00058574
		// (set) Token: 0x060014B8 RID: 5304 RVA: 0x0005A38C File Offset: 0x0005858C
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

		// Token: 0x060014B9 RID: 5305 RVA: 0x0005A396 File Offset: 0x00058596
		public StyleEnum(T v)
		{
			this = new StyleEnum<T>(v, StyleKeyword.Undefined);
		}

		// Token: 0x060014BA RID: 5306 RVA: 0x0005A3A4 File Offset: 0x000585A4
		public StyleEnum(StyleKeyword keyword)
		{
			this = new StyleEnum<T>(default(T), keyword);
		}

		// Token: 0x060014BB RID: 5307 RVA: 0x0005A3C3 File Offset: 0x000585C3
		internal StyleEnum(T v, StyleKeyword keyword)
		{
			this.m_Keyword = keyword;
			this.m_Value = v;
		}

		// Token: 0x060014BC RID: 5308 RVA: 0x0005A3D4 File Offset: 0x000585D4
		public static bool operator ==(StyleEnum<T> lhs, StyleEnum<T> rhs)
		{
			return lhs.m_Keyword == rhs.m_Keyword && UnsafeUtility.EnumEquals<T>(lhs.m_Value, rhs.m_Value);
		}

		// Token: 0x060014BD RID: 5309 RVA: 0x0005A408 File Offset: 0x00058608
		public static bool operator !=(StyleEnum<T> lhs, StyleEnum<T> rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x060014BE RID: 5310 RVA: 0x0005A424 File Offset: 0x00058624
		public static implicit operator StyleEnum<T>(StyleKeyword keyword)
		{
			return new StyleEnum<T>(keyword);
		}

		// Token: 0x060014BF RID: 5311 RVA: 0x0005A43C File Offset: 0x0005863C
		public static implicit operator StyleEnum<T>(T v)
		{
			return new StyleEnum<T>(v);
		}

		// Token: 0x060014C0 RID: 5312 RVA: 0x0005A454 File Offset: 0x00058654
		public bool Equals(StyleEnum<T> other)
		{
			return other == this;
		}

		// Token: 0x060014C1 RID: 5313 RVA: 0x0005A474 File Offset: 0x00058674
		public override bool Equals(object obj)
		{
			bool flag;
			if (obj is StyleEnum<T>)
			{
				StyleEnum<T> styleEnum = (StyleEnum<T>)obj;
				flag = this.Equals(styleEnum);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x060014C2 RID: 5314 RVA: 0x0005A4A0 File Offset: 0x000586A0
		public override int GetHashCode()
		{
			return (UnsafeUtility.EnumToInt<T>(this.m_Value) * 397) ^ (int)this.m_Keyword;
		}

		// Token: 0x060014C3 RID: 5315 RVA: 0x0005A4CC File Offset: 0x000586CC
		public override string ToString()
		{
			return this.DebugString<T>();
		}

		// Token: 0x04000906 RID: 2310
		private T m_Value;

		// Token: 0x04000907 RID: 2311
		private StyleKeyword m_Keyword;
	}
}
