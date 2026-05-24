using System;
using System.Runtime.InteropServices;
using UnityEngine.TextCore.Text;

namespace UnityEngine.UIElements
{
	// Token: 0x02000283 RID: 643
	public struct StyleFontDefinition : IStyleValue<FontDefinition>, IEquatable<StyleFontDefinition>
	{
		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x060014E2 RID: 5346 RVA: 0x0005A840 File Offset: 0x00058A40
		// (set) Token: 0x060014E3 RID: 5347 RVA: 0x0005A86B File Offset: 0x00058A6B
		public FontDefinition value
		{
			get
			{
				return (this.m_Keyword == StyleKeyword.Undefined) ? this.m_Value : default(FontDefinition);
			}
			set
			{
				this.m_Value = value;
				this.m_Keyword = StyleKeyword.Undefined;
			}
		}

		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x060014E4 RID: 5348 RVA: 0x0005A87C File Offset: 0x00058A7C
		// (set) Token: 0x060014E5 RID: 5349 RVA: 0x0005A894 File Offset: 0x00058A94
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

		// Token: 0x060014E6 RID: 5350 RVA: 0x0005A89E File Offset: 0x00058A9E
		public StyleFontDefinition(FontDefinition f)
		{
			this = new StyleFontDefinition(f, StyleKeyword.Undefined);
		}

		// Token: 0x060014E7 RID: 5351 RVA: 0x0005A8AA File Offset: 0x00058AAA
		public StyleFontDefinition(FontAsset f)
		{
			this = new StyleFontDefinition(f, StyleKeyword.Undefined);
		}

		// Token: 0x060014E8 RID: 5352 RVA: 0x0005A8B6 File Offset: 0x00058AB6
		public StyleFontDefinition(Font f)
		{
			this = new StyleFontDefinition(f, StyleKeyword.Undefined);
		}

		// Token: 0x060014E9 RID: 5353 RVA: 0x0005A8C4 File Offset: 0x00058AC4
		public StyleFontDefinition(StyleKeyword keyword)
		{
			this = new StyleFontDefinition(default(FontDefinition), keyword);
		}

		// Token: 0x060014EA RID: 5354 RVA: 0x0005A8E3 File Offset: 0x00058AE3
		internal StyleFontDefinition(object obj, StyleKeyword keyword)
		{
			this = new StyleFontDefinition(FontDefinition.FromObject(obj), keyword);
		}

		// Token: 0x060014EB RID: 5355 RVA: 0x0005A8F4 File Offset: 0x00058AF4
		internal StyleFontDefinition(object obj)
		{
			this = new StyleFontDefinition(FontDefinition.FromObject(obj), StyleKeyword.Undefined);
		}

		// Token: 0x060014EC RID: 5356 RVA: 0x0005A905 File Offset: 0x00058B05
		internal StyleFontDefinition(FontAsset f, StyleKeyword keyword)
		{
			this = new StyleFontDefinition(FontDefinition.FromSDFFont(f), keyword);
		}

		// Token: 0x060014ED RID: 5357 RVA: 0x0005A916 File Offset: 0x00058B16
		internal StyleFontDefinition(Font f, StyleKeyword keyword)
		{
			this = new StyleFontDefinition(FontDefinition.FromFont(f), keyword);
		}

		// Token: 0x060014EE RID: 5358 RVA: 0x0005A928 File Offset: 0x00058B28
		internal StyleFontDefinition(GCHandle gcHandle, StyleKeyword keyword)
		{
			this = new StyleFontDefinition(gcHandle.IsAllocated ? FontDefinition.FromObject(gcHandle.Target) : default(FontDefinition), keyword);
		}

		// Token: 0x060014EF RID: 5359 RVA: 0x0005A95E File Offset: 0x00058B5E
		internal StyleFontDefinition(FontDefinition f, StyleKeyword keyword)
		{
			this.m_Keyword = keyword;
			this.m_Value = f;
		}

		// Token: 0x060014F0 RID: 5360 RVA: 0x0005A96F File Offset: 0x00058B6F
		internal StyleFontDefinition(StyleFontDefinition sfd)
		{
			this.m_Keyword = sfd.keyword;
			this.m_Value = sfd.value;
		}

		// Token: 0x060014F1 RID: 5361 RVA: 0x0005A98C File Offset: 0x00058B8C
		public static implicit operator StyleFontDefinition(StyleKeyword keyword)
		{
			return new StyleFontDefinition(keyword);
		}

		// Token: 0x060014F2 RID: 5362 RVA: 0x0005A9A4 File Offset: 0x00058BA4
		public static implicit operator StyleFontDefinition(FontDefinition f)
		{
			return new StyleFontDefinition(f);
		}

		// Token: 0x060014F3 RID: 5363 RVA: 0x0005A9BC File Offset: 0x00058BBC
		public bool Equals(StyleFontDefinition other)
		{
			return this.m_Keyword == other.m_Keyword && this.m_Value.Equals(other.m_Value);
		}

		// Token: 0x060014F4 RID: 5364 RVA: 0x0005A9F0 File Offset: 0x00058BF0
		public override bool Equals(object obj)
		{
			bool flag;
			if (obj is StyleFontDefinition)
			{
				StyleFontDefinition styleFontDefinition = (StyleFontDefinition)obj;
				flag = this.Equals(styleFontDefinition);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x060014F5 RID: 5365 RVA: 0x0005AA1C File Offset: 0x00058C1C
		public override int GetHashCode()
		{
			return (int)((this.m_Keyword * (StyleKeyword)397) ^ (StyleKeyword)this.m_Value.GetHashCode());
		}

		// Token: 0x060014F6 RID: 5366 RVA: 0x0005AA50 File Offset: 0x00058C50
		public static bool operator ==(StyleFontDefinition left, StyleFontDefinition right)
		{
			return left.Equals(right);
		}

		// Token: 0x060014F7 RID: 5367 RVA: 0x0005AA6C File Offset: 0x00058C6C
		public static bool operator !=(StyleFontDefinition left, StyleFontDefinition right)
		{
			return !left.Equals(right);
		}

		// Token: 0x0400090C RID: 2316
		private StyleKeyword m_Keyword;

		// Token: 0x0400090D RID: 2317
		private FontDefinition m_Value;
	}
}
