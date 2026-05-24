using System;
using UnityEngine.UIElements.StyleSheets;

namespace UnityEngine.UIElements.Experimental
{
	// Token: 0x02000380 RID: 896
	public struct StyleValues
	{
		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x06001C7C RID: 7292 RVA: 0x00085614 File Offset: 0x00083814
		// (set) Token: 0x06001C7D RID: 7293 RVA: 0x0008563E File Offset: 0x0008383E
		public float top
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.Top).value;
			}
			set
			{
				this.SetValue(StylePropertyId.Top, value);
			}
		}

		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x06001C7E RID: 7294 RVA: 0x00085650 File Offset: 0x00083850
		// (set) Token: 0x06001C7F RID: 7295 RVA: 0x0008567A File Offset: 0x0008387A
		public float left
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.Left).value;
			}
			set
			{
				this.SetValue(StylePropertyId.Left, value);
			}
		}

		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x06001C80 RID: 7296 RVA: 0x0008568C File Offset: 0x0008388C
		// (set) Token: 0x06001C81 RID: 7297 RVA: 0x000856B6 File Offset: 0x000838B6
		public float width
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.Width).value;
			}
			set
			{
				this.SetValue(StylePropertyId.Width, value);
			}
		}

		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x06001C82 RID: 7298 RVA: 0x000856C8 File Offset: 0x000838C8
		// (set) Token: 0x06001C83 RID: 7299 RVA: 0x000856F2 File Offset: 0x000838F2
		public float height
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.Height).value;
			}
			set
			{
				this.SetValue(StylePropertyId.Height, value);
			}
		}

		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x06001C84 RID: 7300 RVA: 0x00085704 File Offset: 0x00083904
		// (set) Token: 0x06001C85 RID: 7301 RVA: 0x0008572E File Offset: 0x0008392E
		public float right
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.Right).value;
			}
			set
			{
				this.SetValue(StylePropertyId.Right, value);
			}
		}

		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x06001C86 RID: 7302 RVA: 0x00085740 File Offset: 0x00083940
		// (set) Token: 0x06001C87 RID: 7303 RVA: 0x0008576A File Offset: 0x0008396A
		public float bottom
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.Bottom).value;
			}
			set
			{
				this.SetValue(StylePropertyId.Bottom, value);
			}
		}

		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x06001C88 RID: 7304 RVA: 0x0008577C File Offset: 0x0008397C
		// (set) Token: 0x06001C89 RID: 7305 RVA: 0x000857A6 File Offset: 0x000839A6
		public Color color
		{
			get
			{
				return this.Values().GetStyleColor(StylePropertyId.Color).value;
			}
			set
			{
				this.SetValue(StylePropertyId.Color, value);
			}
		}

		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x06001C8A RID: 7306 RVA: 0x000857B8 File Offset: 0x000839B8
		// (set) Token: 0x06001C8B RID: 7307 RVA: 0x000857E2 File Offset: 0x000839E2
		public Color backgroundColor
		{
			get
			{
				return this.Values().GetStyleColor(StylePropertyId.BackgroundColor).value;
			}
			set
			{
				this.SetValue(StylePropertyId.BackgroundColor, value);
			}
		}

		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x06001C8C RID: 7308 RVA: 0x000857F4 File Offset: 0x000839F4
		// (set) Token: 0x06001C8D RID: 7309 RVA: 0x0008581E File Offset: 0x00083A1E
		public Color unityBackgroundImageTintColor
		{
			get
			{
				return this.Values().GetStyleColor(StylePropertyId.UnityBackgroundImageTintColor).value;
			}
			set
			{
				this.SetValue(StylePropertyId.UnityBackgroundImageTintColor, value);
			}
		}

		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x06001C8E RID: 7310 RVA: 0x00085830 File Offset: 0x00083A30
		// (set) Token: 0x06001C8F RID: 7311 RVA: 0x0008585A File Offset: 0x00083A5A
		public Color borderColor
		{
			get
			{
				return this.Values().GetStyleColor(StylePropertyId.BorderColor).value;
			}
			set
			{
				this.SetValue(StylePropertyId.BorderColor, value);
			}
		}

		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x06001C90 RID: 7312 RVA: 0x0008586C File Offset: 0x00083A6C
		// (set) Token: 0x06001C91 RID: 7313 RVA: 0x00085896 File Offset: 0x00083A96
		public float marginLeft
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.MarginLeft).value;
			}
			set
			{
				this.SetValue(StylePropertyId.MarginLeft, value);
			}
		}

		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x06001C92 RID: 7314 RVA: 0x000858A8 File Offset: 0x00083AA8
		// (set) Token: 0x06001C93 RID: 7315 RVA: 0x000858D2 File Offset: 0x00083AD2
		public float marginTop
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.MarginTop).value;
			}
			set
			{
				this.SetValue(StylePropertyId.MarginTop, value);
			}
		}

		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x06001C94 RID: 7316 RVA: 0x000858E4 File Offset: 0x00083AE4
		// (set) Token: 0x06001C95 RID: 7317 RVA: 0x0008590E File Offset: 0x00083B0E
		public float marginRight
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.MarginRight).value;
			}
			set
			{
				this.SetValue(StylePropertyId.MarginRight, value);
			}
		}

		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x06001C96 RID: 7318 RVA: 0x00085920 File Offset: 0x00083B20
		// (set) Token: 0x06001C97 RID: 7319 RVA: 0x0008594A File Offset: 0x00083B4A
		public float marginBottom
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.MarginBottom).value;
			}
			set
			{
				this.SetValue(StylePropertyId.MarginBottom, value);
			}
		}

		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x06001C98 RID: 7320 RVA: 0x0008595C File Offset: 0x00083B5C
		// (set) Token: 0x06001C99 RID: 7321 RVA: 0x00085986 File Offset: 0x00083B86
		public float paddingLeft
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.PaddingLeft).value;
			}
			set
			{
				this.SetValue(StylePropertyId.PaddingLeft, value);
			}
		}

		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x06001C9A RID: 7322 RVA: 0x00085998 File Offset: 0x00083B98
		// (set) Token: 0x06001C9B RID: 7323 RVA: 0x000859C2 File Offset: 0x00083BC2
		public float paddingTop
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.PaddingTop).value;
			}
			set
			{
				this.SetValue(StylePropertyId.PaddingTop, value);
			}
		}

		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x06001C9C RID: 7324 RVA: 0x000859D4 File Offset: 0x00083BD4
		// (set) Token: 0x06001C9D RID: 7325 RVA: 0x000859FE File Offset: 0x00083BFE
		public float paddingRight
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.PaddingRight).value;
			}
			set
			{
				this.SetValue(StylePropertyId.PaddingRight, value);
			}
		}

		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x06001C9E RID: 7326 RVA: 0x00085A10 File Offset: 0x00083C10
		// (set) Token: 0x06001C9F RID: 7327 RVA: 0x00085A3A File Offset: 0x00083C3A
		public float paddingBottom
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.PaddingBottom).value;
			}
			set
			{
				this.SetValue(StylePropertyId.PaddingBottom, value);
			}
		}

		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x06001CA0 RID: 7328 RVA: 0x00085A4C File Offset: 0x00083C4C
		// (set) Token: 0x06001CA1 RID: 7329 RVA: 0x00085A76 File Offset: 0x00083C76
		public float borderLeftWidth
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.BorderLeftWidth).value;
			}
			set
			{
				this.SetValue(StylePropertyId.BorderLeftWidth, value);
			}
		}

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x06001CA2 RID: 7330 RVA: 0x00085A88 File Offset: 0x00083C88
		// (set) Token: 0x06001CA3 RID: 7331 RVA: 0x00085AB2 File Offset: 0x00083CB2
		public float borderRightWidth
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.BorderRightWidth).value;
			}
			set
			{
				this.SetValue(StylePropertyId.BorderRightWidth, value);
			}
		}

		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x06001CA4 RID: 7332 RVA: 0x00085AC4 File Offset: 0x00083CC4
		// (set) Token: 0x06001CA5 RID: 7333 RVA: 0x00085AEE File Offset: 0x00083CEE
		public float borderTopWidth
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.BorderTopWidth).value;
			}
			set
			{
				this.SetValue(StylePropertyId.BorderTopWidth, value);
			}
		}

		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x06001CA6 RID: 7334 RVA: 0x00085B00 File Offset: 0x00083D00
		// (set) Token: 0x06001CA7 RID: 7335 RVA: 0x00085B2A File Offset: 0x00083D2A
		public float borderBottomWidth
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.BorderBottomWidth).value;
			}
			set
			{
				this.SetValue(StylePropertyId.BorderBottomWidth, value);
			}
		}

		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x06001CA8 RID: 7336 RVA: 0x00085B3C File Offset: 0x00083D3C
		// (set) Token: 0x06001CA9 RID: 7337 RVA: 0x00085B66 File Offset: 0x00083D66
		public float borderTopLeftRadius
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.BorderTopLeftRadius).value;
			}
			set
			{
				this.SetValue(StylePropertyId.BorderTopLeftRadius, value);
			}
		}

		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x06001CAA RID: 7338 RVA: 0x00085B78 File Offset: 0x00083D78
		// (set) Token: 0x06001CAB RID: 7339 RVA: 0x00085BA2 File Offset: 0x00083DA2
		public float borderTopRightRadius
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.BorderTopRightRadius).value;
			}
			set
			{
				this.SetValue(StylePropertyId.BorderTopRightRadius, value);
			}
		}

		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x06001CAC RID: 7340 RVA: 0x00085BB4 File Offset: 0x00083DB4
		// (set) Token: 0x06001CAD RID: 7341 RVA: 0x00085BDE File Offset: 0x00083DDE
		public float borderBottomLeftRadius
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.BorderBottomLeftRadius).value;
			}
			set
			{
				this.SetValue(StylePropertyId.BorderBottomLeftRadius, value);
			}
		}

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x06001CAE RID: 7342 RVA: 0x00085BF0 File Offset: 0x00083DF0
		// (set) Token: 0x06001CAF RID: 7343 RVA: 0x00085C1A File Offset: 0x00083E1A
		public float borderBottomRightRadius
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.BorderBottomRightRadius).value;
			}
			set
			{
				this.SetValue(StylePropertyId.BorderBottomRightRadius, value);
			}
		}

		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x06001CB0 RID: 7344 RVA: 0x00085C2C File Offset: 0x00083E2C
		// (set) Token: 0x06001CB1 RID: 7345 RVA: 0x00085C56 File Offset: 0x00083E56
		public float opacity
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.Opacity).value;
			}
			set
			{
				this.SetValue(StylePropertyId.Opacity, value);
			}
		}

		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x06001CB2 RID: 7346 RVA: 0x00085C68 File Offset: 0x00083E68
		// (set) Token: 0x06001CB3 RID: 7347 RVA: 0x00085C92 File Offset: 0x00083E92
		public float flexGrow
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.FlexGrow).value;
			}
			set
			{
				this.SetValue(StylePropertyId.FlexGrow, value);
			}
		}

		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x06001CB4 RID: 7348 RVA: 0x00085CA4 File Offset: 0x00083EA4
		// (set) Token: 0x06001CB5 RID: 7349 RVA: 0x00085C92 File Offset: 0x00083E92
		public float flexShrink
		{
			get
			{
				return this.Values().GetStyleFloat(StylePropertyId.FlexShrink).value;
			}
			set
			{
				this.SetValue(StylePropertyId.FlexGrow, value);
			}
		}

		// Token: 0x06001CB6 RID: 7350 RVA: 0x00085CD0 File Offset: 0x00083ED0
		internal void SetValue(StylePropertyId id, float value)
		{
			StyleValue styleValue = default(StyleValue);
			styleValue.id = id;
			styleValue.number = value;
			this.Values().SetStyleValue(styleValue);
		}

		// Token: 0x06001CB7 RID: 7351 RVA: 0x00085D04 File Offset: 0x00083F04
		internal void SetValue(StylePropertyId id, Color value)
		{
			StyleValue styleValue = default(StyleValue);
			styleValue.id = id;
			styleValue.color = value;
			this.Values().SetStyleValue(styleValue);
		}

		// Token: 0x06001CB8 RID: 7352 RVA: 0x00085D38 File Offset: 0x00083F38
		internal StyleValueCollection Values()
		{
			bool flag = this.m_StyleValues == null;
			if (flag)
			{
				this.m_StyleValues = new StyleValueCollection();
			}
			return this.m_StyleValues;
		}

		// Token: 0x04000E5A RID: 3674
		internal StyleValueCollection m_StyleValues;
	}
}
