using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements.StyleSheets;
using UnityEngine.Yoga;

namespace UnityEngine.UIElements
{
	// Token: 0x02000273 RID: 627
	internal class InlineStyleAccess : StyleValueCollection, IStyle
	{
		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x06001365 RID: 4965 RVA: 0x00053459 File Offset: 0x00051659
		// (set) Token: 0x06001366 RID: 4966 RVA: 0x00053461 File Offset: 0x00051661
		private VisualElement ve { get; set; }

		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x06001367 RID: 4967 RVA: 0x0005346A File Offset: 0x0005166A
		public InlineStyleAccess.InlineRule inlineRule
		{
			get
			{
				return this.m_InlineRule;
			}
		}

		// Token: 0x06001368 RID: 4968 RVA: 0x00053472 File Offset: 0x00051672
		public InlineStyleAccess(VisualElement ve)
		{
			this.ve = ve;
		}

		// Token: 0x06001369 RID: 4969 RVA: 0x00053484 File Offset: 0x00051684
		protected override void Finalize()
		{
			try
			{
				StyleValue styleValue = default(StyleValue);
				bool flag = base.TryGetStyleValue(StylePropertyId.BackgroundImage, ref styleValue);
				if (flag)
				{
					bool isAllocated = styleValue.resource.IsAllocated;
					if (isAllocated)
					{
						styleValue.resource.Free();
					}
				}
				bool flag2 = base.TryGetStyleValue(StylePropertyId.UnityFont, ref styleValue);
				if (flag2)
				{
					bool isAllocated2 = styleValue.resource.IsAllocated;
					if (isAllocated2)
					{
						styleValue.resource.Free();
					}
				}
			}
			finally
			{
				base.Finalize();
			}
		}

		// Token: 0x0600136A RID: 4970 RVA: 0x00053518 File Offset: 0x00051718
		public void SetInlineRule(StyleSheet sheet, StyleRule rule)
		{
			this.m_InlineRule.sheet = sheet;
			this.m_InlineRule.rule = rule;
			this.m_InlineRule.propertyIds = StyleSheetCache.GetPropertyIds(rule);
			this.ApplyInlineStyles(this.ve.computedStyle);
		}

		// Token: 0x0600136B RID: 4971 RVA: 0x00053558 File Offset: 0x00051758
		public bool IsValueSet(StylePropertyId id)
		{
			foreach (StyleValue styleValue in this.m_Values)
			{
				bool flag = styleValue.id == id;
				if (flag)
				{
					return true;
				}
			}
			bool flag2 = this.m_ValuesManaged != null;
			if (flag2)
			{
				foreach (StyleValueManaged styleValueManaged in this.m_ValuesManaged)
				{
					bool flag3 = styleValueManaged.id == id;
					if (flag3)
					{
						return true;
					}
				}
			}
			bool flag4;
			if (id != StylePropertyId.TextShadow)
			{
				if (id != StylePropertyId.Cursor)
				{
					switch (id)
					{
					case StylePropertyId.Rotate:
						flag4 = this.m_HasInlineRotate;
						break;
					case StylePropertyId.Scale:
						flag4 = this.m_HasInlineScale;
						break;
					case StylePropertyId.TransformOrigin:
						flag4 = this.m_HasInlineTransformOrigin;
						break;
					case StylePropertyId.Translate:
						flag4 = this.m_HasInlineTranslate;
						break;
					default:
						flag4 = false;
						break;
					}
				}
				else
				{
					flag4 = this.m_HasInlineCursor;
				}
			}
			else
			{
				flag4 = this.m_HasInlineTextShadow;
			}
			return flag4;
		}

		// Token: 0x0600136C RID: 4972 RVA: 0x000536A0 File Offset: 0x000518A0
		public void ApplyInlineStyles(ref ComputedStyle computedStyle)
		{
			VisualElement parent = this.ve.hierarchy.parent;
			ref ComputedStyle ptr;
			if (parent != null)
			{
				ref ComputedStyle computedStyle2 = ref parent.computedStyle;
				ptr = parent.computedStyle;
			}
			else
			{
				ptr = InitialStyle.Get();
			}
			ref ComputedStyle ptr2 = ref ptr;
			bool flag = this.m_InlineRule.sheet != null;
			if (flag)
			{
				InlineStyleAccess.s_StylePropertyReader.SetInlineContext(this.m_InlineRule.sheet, this.m_InlineRule.rule.properties, this.m_InlineRule.propertyIds, 1f);
				computedStyle.ApplyProperties(InlineStyleAccess.s_StylePropertyReader, ref ptr2);
			}
			foreach (StyleValue styleValue in this.m_Values)
			{
				computedStyle.ApplyStyleValue(styleValue, ref ptr2);
			}
			bool flag2 = this.m_ValuesManaged != null;
			if (flag2)
			{
				foreach (StyleValueManaged styleValueManaged in this.m_ValuesManaged)
				{
					computedStyle.ApplyStyleValueManaged(styleValueManaged, ref ptr2);
				}
			}
			bool flag3 = this.ve.style.cursor.keyword != StyleKeyword.Null;
			if (flag3)
			{
				computedStyle.ApplyStyleCursor(this.ve.style.cursor.value);
			}
			bool flag4 = this.ve.style.textShadow.keyword != StyleKeyword.Null;
			if (flag4)
			{
				computedStyle.ApplyStyleTextShadow(this.ve.style.textShadow.value);
			}
			bool hasInlineTransformOrigin = this.m_HasInlineTransformOrigin;
			if (hasInlineTransformOrigin)
			{
				computedStyle.ApplyStyleTransformOrigin(this.ve.style.transformOrigin.value);
			}
			bool hasInlineTranslate = this.m_HasInlineTranslate;
			if (hasInlineTranslate)
			{
				computedStyle.ApplyStyleTranslate(this.ve.style.translate.value);
			}
			bool hasInlineScale = this.m_HasInlineScale;
			if (hasInlineScale)
			{
				computedStyle.ApplyStyleScale(this.ve.style.scale.value);
			}
			bool hasInlineRotate = this.m_HasInlineRotate;
			if (hasInlineRotate)
			{
				computedStyle.ApplyStyleRotate(this.ve.style.rotate.value);
			}
		}

		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x0600136D RID: 4973 RVA: 0x00053928 File Offset: 0x00051B28
		// (set) Token: 0x0600136E RID: 4974 RVA: 0x00053958 File Offset: 0x00051B58
		StyleCursor IStyle.cursor
		{
			get
			{
				StyleCursor styleCursor = default(StyleCursor);
				bool flag = this.TryGetInlineCursor(ref styleCursor);
				StyleCursor styleCursor2;
				if (flag)
				{
					styleCursor2 = styleCursor;
				}
				else
				{
					styleCursor2 = StyleKeyword.Null;
				}
				return styleCursor2;
			}
			set
			{
				bool flag = this.SetInlineCursor(value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles);
				}
			}
		}

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x0600136F RID: 4975 RVA: 0x00053984 File Offset: 0x00051B84
		// (set) Token: 0x06001370 RID: 4976 RVA: 0x000539B4 File Offset: 0x00051BB4
		StyleTextShadow IStyle.textShadow
		{
			get
			{
				StyleTextShadow styleTextShadow = default(StyleTextShadow);
				bool flag = this.TryGetInlineTextShadow(ref styleTextShadow);
				StyleTextShadow styleTextShadow2;
				if (flag)
				{
					styleTextShadow2 = styleTextShadow;
				}
				else
				{
					styleTextShadow2 = StyleKeyword.Null;
				}
				return styleTextShadow2;
			}
			set
			{
				bool flag = this.SetInlineTextShadow(value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x06001371 RID: 4977 RVA: 0x000539E0 File Offset: 0x00051BE0
		private StyleList<T> GetStyleList<T>(StylePropertyId id)
		{
			StyleValueManaged styleValueManaged = default(StyleValueManaged);
			bool flag = this.TryGetStyleValueManaged(id, ref styleValueManaged);
			StyleList<T> styleList;
			if (flag)
			{
				styleList = new StyleList<T>(styleValueManaged.value as List<T>, styleValueManaged.keyword);
			}
			else
			{
				styleList = StyleKeyword.Null;
			}
			return styleList;
		}

		// Token: 0x06001372 RID: 4978 RVA: 0x00053A28 File Offset: 0x00051C28
		private void SetStyleValueManaged(StyleValueManaged value)
		{
			bool flag = this.m_ValuesManaged == null;
			if (flag)
			{
				this.m_ValuesManaged = new List<StyleValueManaged>();
			}
			for (int i = 0; i < this.m_ValuesManaged.Count; i++)
			{
				bool flag2 = this.m_ValuesManaged[i].id == value.id;
				if (flag2)
				{
					bool flag3 = value.keyword == StyleKeyword.Null;
					if (flag3)
					{
						this.m_ValuesManaged.RemoveAt(i);
					}
					else
					{
						this.m_ValuesManaged[i] = value;
					}
					return;
				}
			}
			this.m_ValuesManaged.Add(value);
		}

		// Token: 0x06001373 RID: 4979 RVA: 0x00053AC8 File Offset: 0x00051CC8
		private bool TryGetStyleValueManaged(StylePropertyId id, ref StyleValueManaged value)
		{
			value.id = StylePropertyId.Unknown;
			bool flag = this.m_ValuesManaged == null;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				foreach (StyleValueManaged styleValueManaged in this.m_ValuesManaged)
				{
					bool flag3 = styleValueManaged.id == id;
					if (flag3)
					{
						value = styleValueManaged;
						return true;
					}
				}
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x06001374 RID: 4980 RVA: 0x00053B54 File Offset: 0x00051D54
		// (set) Token: 0x06001375 RID: 4981 RVA: 0x00053B84 File Offset: 0x00051D84
		StyleTransformOrigin IStyle.transformOrigin
		{
			get
			{
				StyleTransformOrigin styleTransformOrigin = default(StyleTransformOrigin);
				bool flag = this.TryGetInlineTransformOrigin(ref styleTransformOrigin);
				StyleTransformOrigin styleTransformOrigin2;
				if (flag)
				{
					styleTransformOrigin2 = styleTransformOrigin;
				}
				else
				{
					styleTransformOrigin2 = StyleKeyword.Null;
				}
				return styleTransformOrigin2;
			}
			set
			{
				bool flag = this.SetInlineTransformOrigin(value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Transform);
				}
			}
		}

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x06001376 RID: 4982 RVA: 0x00053BB0 File Offset: 0x00051DB0
		// (set) Token: 0x06001377 RID: 4983 RVA: 0x00053BE0 File Offset: 0x00051DE0
		StyleTranslate IStyle.translate
		{
			get
			{
				StyleTranslate styleTranslate = default(StyleTranslate);
				bool flag = this.TryGetInlineTranslate(ref styleTranslate);
				StyleTranslate styleTranslate2;
				if (flag)
				{
					styleTranslate2 = styleTranslate;
				}
				else
				{
					styleTranslate2 = StyleKeyword.Null;
				}
				return styleTranslate2;
			}
			set
			{
				bool flag = this.SetInlineTranslate(value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Transform);
				}
			}
		}

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x06001378 RID: 4984 RVA: 0x00053C0C File Offset: 0x00051E0C
		// (set) Token: 0x06001379 RID: 4985 RVA: 0x00053C3C File Offset: 0x00051E3C
		StyleRotate IStyle.rotate
		{
			get
			{
				StyleRotate styleRotate = default(StyleRotate);
				bool flag = this.TryGetInlineRotate(ref styleRotate);
				StyleRotate styleRotate2;
				if (flag)
				{
					styleRotate2 = styleRotate;
				}
				else
				{
					styleRotate2 = StyleKeyword.Null;
				}
				return styleRotate2;
			}
			set
			{
				bool flag = this.SetInlineRotate(value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Transform);
				}
			}
		}

		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x0600137A RID: 4986 RVA: 0x00053C68 File Offset: 0x00051E68
		// (set) Token: 0x0600137B RID: 4987 RVA: 0x00053C98 File Offset: 0x00051E98
		StyleScale IStyle.scale
		{
			get
			{
				StyleScale styleScale = default(StyleScale);
				bool flag = this.TryGetInlineScale(ref styleScale);
				StyleScale styleScale2;
				if (flag)
				{
					styleScale2 = styleScale;
				}
				else
				{
					styleScale2 = StyleKeyword.Null;
				}
				return styleScale2;
			}
			set
			{
				bool flag = this.SetInlineScale(value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles | VersionChangeType.Transform);
				}
			}
		}

		// Token: 0x0600137C RID: 4988 RVA: 0x00053CC4 File Offset: 0x00051EC4
		private bool SetStyleValue(StylePropertyId id, StyleLength inlineValue)
		{
			StyleValue styleValue = default(StyleValue);
			bool flag = base.TryGetStyleValue(id, ref styleValue);
			if (flag)
			{
				bool flag2 = styleValue.length == inlineValue.value && styleValue.keyword == inlineValue.keyword;
				if (flag2)
				{
					return false;
				}
			}
			else
			{
				bool flag3 = inlineValue.keyword == StyleKeyword.Null;
				if (flag3)
				{
					return false;
				}
			}
			styleValue.id = id;
			styleValue.keyword = inlineValue.keyword;
			styleValue.length = inlineValue.ToLength();
			base.SetStyleValue(styleValue);
			bool flag4 = inlineValue.keyword == StyleKeyword.Null;
			bool flag5;
			if (flag4)
			{
				flag5 = this.RemoveInlineStyle(id);
			}
			else
			{
				this.ApplyStyleValue(styleValue);
				flag5 = true;
			}
			return flag5;
		}

		// Token: 0x0600137D RID: 4989 RVA: 0x00053D80 File Offset: 0x00051F80
		private bool SetStyleValue(StylePropertyId id, StyleFloat inlineValue)
		{
			StyleValue styleValue = default(StyleValue);
			bool flag = base.TryGetStyleValue(id, ref styleValue);
			if (flag)
			{
				bool flag2 = styleValue.number == inlineValue.value && styleValue.keyword == inlineValue.keyword;
				if (flag2)
				{
					return false;
				}
			}
			else
			{
				bool flag3 = inlineValue.keyword == StyleKeyword.Null;
				if (flag3)
				{
					return false;
				}
			}
			styleValue.id = id;
			styleValue.keyword = inlineValue.keyword;
			styleValue.number = inlineValue.value;
			base.SetStyleValue(styleValue);
			bool flag4 = inlineValue.keyword == StyleKeyword.Null;
			bool flag5;
			if (flag4)
			{
				flag5 = this.RemoveInlineStyle(id);
			}
			else
			{
				this.ApplyStyleValue(styleValue);
				flag5 = true;
			}
			return flag5;
		}

		// Token: 0x0600137E RID: 4990 RVA: 0x00053E38 File Offset: 0x00052038
		private bool SetStyleValue(StylePropertyId id, StyleInt inlineValue)
		{
			StyleValue styleValue = default(StyleValue);
			bool flag = base.TryGetStyleValue(id, ref styleValue);
			if (flag)
			{
				bool flag2 = styleValue.number == (float)inlineValue.value && styleValue.keyword == inlineValue.keyword;
				if (flag2)
				{
					return false;
				}
			}
			else
			{
				bool flag3 = inlineValue.keyword == StyleKeyword.Null;
				if (flag3)
				{
					return false;
				}
			}
			styleValue.id = id;
			styleValue.keyword = inlineValue.keyword;
			styleValue.number = (float)inlineValue.value;
			base.SetStyleValue(styleValue);
			bool flag4 = inlineValue.keyword == StyleKeyword.Null;
			bool flag5;
			if (flag4)
			{
				flag5 = this.RemoveInlineStyle(id);
			}
			else
			{
				this.ApplyStyleValue(styleValue);
				flag5 = true;
			}
			return flag5;
		}

		// Token: 0x0600137F RID: 4991 RVA: 0x00053EF4 File Offset: 0x000520F4
		private bool SetStyleValue(StylePropertyId id, StyleColor inlineValue)
		{
			StyleValue styleValue = default(StyleValue);
			bool flag = base.TryGetStyleValue(id, ref styleValue);
			if (flag)
			{
				bool flag2 = styleValue.color == inlineValue.value && styleValue.keyword == inlineValue.keyword;
				if (flag2)
				{
					return false;
				}
			}
			else
			{
				bool flag3 = inlineValue.keyword == StyleKeyword.Null;
				if (flag3)
				{
					return false;
				}
			}
			styleValue.id = id;
			styleValue.keyword = inlineValue.keyword;
			styleValue.color = inlineValue.value;
			base.SetStyleValue(styleValue);
			bool flag4 = inlineValue.keyword == StyleKeyword.Null;
			bool flag5;
			if (flag4)
			{
				flag5 = this.RemoveInlineStyle(id);
			}
			else
			{
				this.ApplyStyleValue(styleValue);
				flag5 = true;
			}
			return flag5;
		}

		// Token: 0x06001380 RID: 4992 RVA: 0x00053FB4 File Offset: 0x000521B4
		private bool SetStyleValue<T>(StylePropertyId id, StyleEnum<T> inlineValue) where T : struct, IConvertible
		{
			StyleValue styleValue = default(StyleValue);
			int num = UnsafeUtility.EnumToInt<T>(inlineValue.value);
			bool flag = base.TryGetStyleValue(id, ref styleValue);
			if (flag)
			{
				bool flag2 = styleValue.number == (float)num && styleValue.keyword == inlineValue.keyword;
				if (flag2)
				{
					return false;
				}
			}
			else
			{
				bool flag3 = inlineValue.keyword == StyleKeyword.Null;
				if (flag3)
				{
					return false;
				}
			}
			styleValue.id = id;
			styleValue.keyword = inlineValue.keyword;
			styleValue.number = (float)num;
			base.SetStyleValue(styleValue);
			bool flag4 = inlineValue.keyword == StyleKeyword.Null;
			bool flag5;
			if (flag4)
			{
				flag5 = this.RemoveInlineStyle(id);
			}
			else
			{
				this.ApplyStyleValue(styleValue);
				flag5 = true;
			}
			return flag5;
		}

		// Token: 0x06001381 RID: 4993 RVA: 0x00054074 File Offset: 0x00052274
		private bool SetStyleValue(StylePropertyId id, StyleBackground inlineValue)
		{
			StyleValue styleValue = default(StyleValue);
			bool flag = base.TryGetStyleValue(id, ref styleValue);
			if (flag)
			{
				VectorImage vectorImage = (styleValue.resource.IsAllocated ? (styleValue.resource.Target as VectorImage) : null);
				Sprite sprite = (styleValue.resource.IsAllocated ? (styleValue.resource.Target as Sprite) : null);
				Texture2D texture2D = (styleValue.resource.IsAllocated ? (styleValue.resource.Target as Texture2D) : null);
				RenderTexture renderTexture = (styleValue.resource.IsAllocated ? (styleValue.resource.Target as RenderTexture) : null);
				bool flag2 = vectorImage == inlineValue.value.vectorImage && texture2D == inlineValue.value.texture && sprite == inlineValue.value.sprite && renderTexture == inlineValue.value.renderTexture && styleValue.keyword == inlineValue.keyword;
				if (flag2)
				{
					return false;
				}
				bool isAllocated = styleValue.resource.IsAllocated;
				if (isAllocated)
				{
					styleValue.resource.Free();
				}
			}
			else
			{
				bool flag3 = inlineValue.keyword == StyleKeyword.Null;
				if (flag3)
				{
					return false;
				}
			}
			styleValue.id = id;
			styleValue.keyword = inlineValue.keyword;
			bool flag4 = inlineValue.value.vectorImage != null;
			if (flag4)
			{
				styleValue.resource = GCHandle.Alloc(inlineValue.value.vectorImage);
			}
			else
			{
				bool flag5 = inlineValue.value.sprite != null;
				if (flag5)
				{
					styleValue.resource = GCHandle.Alloc(inlineValue.value.sprite);
				}
				else
				{
					bool flag6 = inlineValue.value.texture != null;
					if (flag6)
					{
						styleValue.resource = GCHandle.Alloc(inlineValue.value.texture);
					}
					else
					{
						bool flag7 = inlineValue.value.renderTexture != null;
						if (flag7)
						{
							styleValue.resource = GCHandle.Alloc(inlineValue.value.renderTexture);
						}
						else
						{
							styleValue.resource = default(GCHandle);
						}
					}
				}
			}
			base.SetStyleValue(styleValue);
			bool flag8 = inlineValue.keyword == StyleKeyword.Null;
			bool flag9;
			if (flag8)
			{
				flag9 = this.RemoveInlineStyle(id);
			}
			else
			{
				this.ApplyStyleValue(styleValue);
				flag9 = true;
			}
			return flag9;
		}

		// Token: 0x06001382 RID: 4994 RVA: 0x0005432C File Offset: 0x0005252C
		private bool SetStyleValue(StylePropertyId id, StyleFontDefinition inlineValue)
		{
			StyleValue styleValue = default(StyleValue);
			bool flag = base.TryGetStyleValue(id, ref styleValue);
			if (flag)
			{
				Font font = (styleValue.resource.IsAllocated ? (styleValue.resource.Target as Font) : null);
				FontAsset fontAsset = (styleValue.resource.IsAllocated ? (styleValue.resource.Target as FontAsset) : null);
				bool flag2 = font == inlineValue.value.font && fontAsset == inlineValue.value.fontAsset && styleValue.keyword == inlineValue.keyword;
				if (flag2)
				{
					return false;
				}
				bool isAllocated = styleValue.resource.IsAllocated;
				if (isAllocated)
				{
					styleValue.resource.Free();
				}
			}
			else
			{
				bool flag3 = inlineValue.keyword == StyleKeyword.Null;
				if (flag3)
				{
					return false;
				}
			}
			styleValue.id = id;
			styleValue.keyword = inlineValue.keyword;
			bool flag4 = inlineValue.value.font != null;
			if (flag4)
			{
				styleValue.resource = GCHandle.Alloc(inlineValue.value.font);
			}
			else
			{
				bool flag5 = inlineValue.value.fontAsset != null;
				if (flag5)
				{
					styleValue.resource = GCHandle.Alloc(inlineValue.value.fontAsset);
				}
				else
				{
					styleValue.resource = default(GCHandle);
				}
			}
			base.SetStyleValue(styleValue);
			bool flag6 = inlineValue.keyword == StyleKeyword.Null;
			bool flag7;
			if (flag6)
			{
				flag7 = this.RemoveInlineStyle(id);
			}
			else
			{
				this.ApplyStyleValue(styleValue);
				flag7 = true;
			}
			return flag7;
		}

		// Token: 0x06001383 RID: 4995 RVA: 0x000544F0 File Offset: 0x000526F0
		private bool SetStyleValue(StylePropertyId id, StyleFont inlineValue)
		{
			StyleValue styleValue = default(StyleValue);
			bool flag = base.TryGetStyleValue(id, ref styleValue);
			if (flag)
			{
				bool isAllocated = styleValue.resource.IsAllocated;
				if (isAllocated)
				{
					Font font = (styleValue.resource.IsAllocated ? (styleValue.resource.Target as Font) : null);
					bool flag2 = font == inlineValue.value && styleValue.keyword == inlineValue.keyword;
					if (flag2)
					{
						return false;
					}
					bool isAllocated2 = styleValue.resource.IsAllocated;
					if (isAllocated2)
					{
						styleValue.resource.Free();
					}
				}
			}
			else
			{
				bool flag3 = inlineValue.keyword == StyleKeyword.Null;
				if (flag3)
				{
					return false;
				}
			}
			styleValue.id = id;
			styleValue.keyword = inlineValue.keyword;
			styleValue.resource = ((inlineValue.value != null) ? GCHandle.Alloc(inlineValue.value) : default(GCHandle));
			base.SetStyleValue(styleValue);
			bool flag4 = inlineValue.keyword == StyleKeyword.Null;
			bool flag5;
			if (flag4)
			{
				flag5 = this.RemoveInlineStyle(id);
			}
			else
			{
				this.ApplyStyleValue(styleValue);
				flag5 = true;
			}
			return flag5;
		}

		// Token: 0x06001384 RID: 4996 RVA: 0x0005462C File Offset: 0x0005282C
		private bool SetStyleValue<T>(StylePropertyId id, StyleList<T> inlineValue)
		{
			StyleValueManaged styleValueManaged = default(StyleValueManaged);
			bool flag = this.TryGetStyleValueManaged(id, ref styleValueManaged);
			if (flag)
			{
				List<T> list = styleValueManaged.value as List<T>;
				bool flag2 = list != null && inlineValue.value != null && Enumerable.SequenceEqual<T>(list, inlineValue.value) && styleValueManaged.keyword == inlineValue.keyword;
				if (flag2)
				{
					return false;
				}
			}
			else
			{
				bool flag3 = inlineValue.keyword == StyleKeyword.Null;
				if (flag3)
				{
					return false;
				}
			}
			styleValueManaged.id = id;
			styleValueManaged.keyword = inlineValue.keyword;
			bool flag4 = inlineValue.value != null;
			if (flag4)
			{
				bool flag5 = styleValueManaged.value == null;
				if (flag5)
				{
					styleValueManaged.value = new List<T>(inlineValue.value);
				}
				else
				{
					List<T> list2 = (List<T>)styleValueManaged.value;
					list2.Clear();
					list2.AddRange(inlineValue.value);
				}
			}
			else
			{
				styleValueManaged.value = null;
			}
			this.SetStyleValueManaged(styleValueManaged);
			bool flag6 = inlineValue.keyword == StyleKeyword.Null;
			bool flag7;
			if (flag6)
			{
				flag7 = this.RemoveInlineStyle(id);
			}
			else
			{
				this.ApplyStyleValue(styleValueManaged);
				flag7 = true;
			}
			return flag7;
		}

		// Token: 0x06001385 RID: 4997 RVA: 0x00054764 File Offset: 0x00052964
		private bool SetInlineCursor(StyleCursor inlineValue)
		{
			StyleCursor styleCursor = default(StyleCursor);
			bool flag = this.TryGetInlineCursor(ref styleCursor);
			if (flag)
			{
				bool flag2 = styleCursor.value == inlineValue.value && styleCursor.keyword == inlineValue.keyword;
				if (flag2)
				{
					return false;
				}
			}
			else
			{
				bool flag3 = inlineValue.keyword == StyleKeyword.Null;
				if (flag3)
				{
					return false;
				}
			}
			styleCursor.value = inlineValue.value;
			styleCursor.keyword = inlineValue.keyword;
			bool flag4 = inlineValue.keyword == StyleKeyword.Null;
			bool flag5;
			if (flag4)
			{
				this.m_HasInlineCursor = false;
				flag5 = this.RemoveInlineStyle(StylePropertyId.Cursor);
			}
			else
			{
				this.m_InlineCursor = styleCursor;
				this.m_HasInlineCursor = true;
				this.ApplyStyleCursor(styleCursor);
				flag5 = true;
			}
			return flag5;
		}

		// Token: 0x06001386 RID: 4998 RVA: 0x00054830 File Offset: 0x00052A30
		private void ApplyStyleCursor(StyleCursor cursor)
		{
			this.ve.computedStyle.ApplyStyleCursor(cursor.value);
			BaseVisualElementPanel elementPanel = this.ve.elementPanel;
			bool flag = ((elementPanel != null) ? elementPanel.GetTopElementUnderPointer(PointerId.mousePointerId) : null) == this.ve;
			if (flag)
			{
				this.ve.elementPanel.cursorManager.SetCursor(cursor.value);
			}
		}

		// Token: 0x06001387 RID: 4999 RVA: 0x0005489C File Offset: 0x00052A9C
		private bool SetInlineTextShadow(StyleTextShadow inlineValue)
		{
			StyleTextShadow styleTextShadow = default(StyleTextShadow);
			bool flag = this.TryGetInlineTextShadow(ref styleTextShadow);
			if (flag)
			{
				bool flag2 = styleTextShadow.value == inlineValue.value && styleTextShadow.keyword == inlineValue.keyword;
				if (flag2)
				{
					return false;
				}
			}
			else
			{
				bool flag3 = inlineValue.keyword == StyleKeyword.Null;
				if (flag3)
				{
					return false;
				}
			}
			styleTextShadow.value = inlineValue.value;
			styleTextShadow.keyword = inlineValue.keyword;
			bool flag4 = inlineValue.keyword == StyleKeyword.Null;
			bool flag5;
			if (flag4)
			{
				this.m_HasInlineTextShadow = false;
				flag5 = this.RemoveInlineStyle(StylePropertyId.TextShadow);
			}
			else
			{
				this.m_InlineTextShadow = styleTextShadow;
				this.m_HasInlineTextShadow = true;
				this.ApplyStyleTextShadow(styleTextShadow);
				flag5 = true;
			}
			return flag5;
		}

		// Token: 0x06001388 RID: 5000 RVA: 0x00054968 File Offset: 0x00052B68
		private void ApplyStyleTextShadow(StyleTextShadow textShadow)
		{
			ComputedTransitionUtils.UpdateComputedTransitions(this.ve.computedStyle);
			bool flag = false;
			ComputedTransitionProperty computedTransitionProperty;
			bool flag2 = this.ve.computedStyle.hasTransition && this.ve.styleInitialized && this.ve.computedStyle.GetTransitionProperty(StylePropertyId.TextShadow, out computedTransitionProperty);
			if (flag2)
			{
				flag = ComputedStyle.StartAnimationInlineTextShadow(this.ve, this.ve.computedStyle, textShadow, computedTransitionProperty.durationMs, computedTransitionProperty.delayMs, computedTransitionProperty.easingCurve);
			}
			else
			{
				this.ve.styleAnimation.CancelAnimation(StylePropertyId.TextShadow);
			}
			bool flag3 = !flag;
			if (flag3)
			{
				this.ve.computedStyle.ApplyStyleTextShadow(textShadow.value);
			}
		}

		// Token: 0x06001389 RID: 5001 RVA: 0x00054A30 File Offset: 0x00052C30
		private bool SetInlineTransformOrigin(StyleTransformOrigin inlineValue)
		{
			StyleTransformOrigin styleTransformOrigin = default(StyleTransformOrigin);
			bool flag = this.TryGetInlineTransformOrigin(ref styleTransformOrigin);
			if (flag)
			{
				bool flag2 = styleTransformOrigin.value == inlineValue.value && styleTransformOrigin.keyword == inlineValue.keyword;
				if (flag2)
				{
					return false;
				}
			}
			else
			{
				bool flag3 = inlineValue.keyword == StyleKeyword.Null;
				if (flag3)
				{
					return false;
				}
			}
			bool flag4 = inlineValue.keyword == StyleKeyword.Null;
			bool flag5;
			if (flag4)
			{
				this.m_HasInlineTransformOrigin = false;
				flag5 = this.RemoveInlineStyle(StylePropertyId.TransformOrigin);
			}
			else
			{
				this.m_InlineTransformOrigin = inlineValue;
				this.m_HasInlineTransformOrigin = true;
				this.ApplyStyleTransformOrigin(inlineValue);
				flag5 = true;
			}
			return flag5;
		}

		// Token: 0x0600138A RID: 5002 RVA: 0x00054ADC File Offset: 0x00052CDC
		private void ApplyStyleTransformOrigin(StyleTransformOrigin transformOrigin)
		{
			ComputedTransitionUtils.UpdateComputedTransitions(this.ve.computedStyle);
			bool flag = false;
			ComputedTransitionProperty computedTransitionProperty;
			bool flag2 = this.ve.computedStyle.hasTransition && this.ve.styleInitialized && this.ve.computedStyle.GetTransitionProperty(StylePropertyId.TransformOrigin, out computedTransitionProperty);
			if (flag2)
			{
				flag = ComputedStyle.StartAnimationInlineTransformOrigin(this.ve, this.ve.computedStyle, transformOrigin, computedTransitionProperty.durationMs, computedTransitionProperty.delayMs, computedTransitionProperty.easingCurve);
			}
			else
			{
				this.ve.styleAnimation.CancelAnimation(StylePropertyId.TransformOrigin);
			}
			bool flag3 = !flag;
			if (flag3)
			{
				this.ve.computedStyle.ApplyStyleTransformOrigin(transformOrigin.value);
			}
		}

		// Token: 0x0600138B RID: 5003 RVA: 0x00054BA4 File Offset: 0x00052DA4
		private bool SetInlineTranslate(StyleTranslate inlineValue)
		{
			StyleTranslate styleTranslate = default(StyleTranslate);
			bool flag = this.TryGetInlineTranslate(ref styleTranslate);
			if (flag)
			{
				bool flag2 = styleTranslate.value == inlineValue.value && styleTranslate.keyword == inlineValue.keyword;
				if (flag2)
				{
					return false;
				}
			}
			else
			{
				bool flag3 = inlineValue.keyword == StyleKeyword.Null;
				if (flag3)
				{
					return false;
				}
			}
			bool flag4 = inlineValue.keyword == StyleKeyword.Null;
			bool flag5;
			if (flag4)
			{
				this.m_HasInlineTranslate = false;
				flag5 = this.RemoveInlineStyle(StylePropertyId.Translate);
			}
			else
			{
				this.m_InlineTranslateOperation = inlineValue;
				this.m_HasInlineTranslate = true;
				this.ApplyStyleTranslate(inlineValue);
				flag5 = true;
			}
			return flag5;
		}

		// Token: 0x0600138C RID: 5004 RVA: 0x00054C50 File Offset: 0x00052E50
		private void ApplyStyleTranslate(StyleTranslate translate)
		{
			ComputedTransitionUtils.UpdateComputedTransitions(this.ve.computedStyle);
			bool flag = false;
			ComputedTransitionProperty computedTransitionProperty;
			bool flag2 = this.ve.computedStyle.hasTransition && this.ve.styleInitialized && this.ve.computedStyle.GetTransitionProperty(StylePropertyId.Translate, out computedTransitionProperty);
			if (flag2)
			{
				flag = ComputedStyle.StartAnimationInlineTranslate(this.ve, this.ve.computedStyle, translate, computedTransitionProperty.durationMs, computedTransitionProperty.delayMs, computedTransitionProperty.easingCurve);
			}
			else
			{
				this.ve.styleAnimation.CancelAnimation(StylePropertyId.Translate);
			}
			bool flag3 = !flag;
			if (flag3)
			{
				this.ve.computedStyle.ApplyStyleTranslate(translate.value);
			}
		}

		// Token: 0x0600138D RID: 5005 RVA: 0x00054D18 File Offset: 0x00052F18
		private bool SetInlineScale(StyleScale inlineValue)
		{
			StyleScale styleScale = default(StyleScale);
			bool flag = this.TryGetInlineScale(ref styleScale);
			if (flag)
			{
				bool flag2 = styleScale.value == inlineValue.value && styleScale.keyword == inlineValue.keyword;
				if (flag2)
				{
					return false;
				}
			}
			else
			{
				bool flag3 = inlineValue.keyword == StyleKeyword.Null;
				if (flag3)
				{
					return false;
				}
			}
			bool flag4 = inlineValue.keyword == StyleKeyword.Null;
			bool flag5;
			if (flag4)
			{
				this.m_HasInlineScale = false;
				flag5 = this.RemoveInlineStyle(StylePropertyId.Scale);
			}
			else
			{
				this.m_InlineScale = inlineValue;
				this.m_HasInlineScale = true;
				this.ApplyStyleScale(inlineValue);
				flag5 = true;
			}
			return flag5;
		}

		// Token: 0x0600138E RID: 5006 RVA: 0x00054DC4 File Offset: 0x00052FC4
		private void ApplyStyleScale(StyleScale scale)
		{
			ComputedTransitionUtils.UpdateComputedTransitions(this.ve.computedStyle);
			bool flag = false;
			ComputedTransitionProperty computedTransitionProperty;
			bool flag2 = this.ve.computedStyle.hasTransition && this.ve.styleInitialized && this.ve.computedStyle.GetTransitionProperty(StylePropertyId.Scale, out computedTransitionProperty);
			if (flag2)
			{
				flag = ComputedStyle.StartAnimationInlineScale(this.ve, this.ve.computedStyle, scale, computedTransitionProperty.durationMs, computedTransitionProperty.delayMs, computedTransitionProperty.easingCurve);
			}
			else
			{
				this.ve.styleAnimation.CancelAnimation(StylePropertyId.Scale);
			}
			bool flag3 = !flag;
			if (flag3)
			{
				this.ve.computedStyle.ApplyStyleScale(scale.value);
			}
		}

		// Token: 0x0600138F RID: 5007 RVA: 0x00054E8C File Offset: 0x0005308C
		private bool SetInlineRotate(StyleRotate inlineValue)
		{
			StyleRotate styleRotate = default(StyleRotate);
			bool flag = this.TryGetInlineRotate(ref styleRotate);
			if (flag)
			{
				bool flag2 = styleRotate.value == inlineValue.value && styleRotate.keyword == inlineValue.keyword;
				if (flag2)
				{
					return false;
				}
			}
			else
			{
				bool flag3 = inlineValue.keyword == StyleKeyword.Null;
				if (flag3)
				{
					return false;
				}
			}
			bool flag4 = inlineValue.keyword == StyleKeyword.Null;
			bool flag5;
			if (flag4)
			{
				this.m_HasInlineRotate = false;
				flag5 = this.RemoveInlineStyle(StylePropertyId.Rotate);
			}
			else
			{
				this.m_InlineRotateOperation = inlineValue;
				this.m_HasInlineRotate = true;
				this.ApplyStyleRotate(inlineValue);
				flag5 = true;
			}
			return flag5;
		}

		// Token: 0x06001390 RID: 5008 RVA: 0x00054F38 File Offset: 0x00053138
		private void ApplyStyleRotate(StyleRotate rotate)
		{
			VisualElement parent = this.ve.hierarchy.parent;
			if (parent != null)
			{
				ref ComputedStyle computedStyle = ref parent.computedStyle;
				ref ComputedStyle computedStyle2 = ref parent.computedStyle;
			}
			else
			{
				InitialStyle.Get();
			}
			ComputedTransitionUtils.UpdateComputedTransitions(this.ve.computedStyle);
			bool flag = false;
			ComputedTransitionProperty computedTransitionProperty;
			bool flag2 = this.ve.computedStyle.hasTransition && this.ve.styleInitialized && this.ve.computedStyle.GetTransitionProperty(StylePropertyId.Rotate, out computedTransitionProperty);
			if (flag2)
			{
				flag = ComputedStyle.StartAnimationInlineRotate(this.ve, this.ve.computedStyle, rotate, computedTransitionProperty.durationMs, computedTransitionProperty.delayMs, computedTransitionProperty.easingCurve);
			}
			else
			{
				this.ve.styleAnimation.CancelAnimation(StylePropertyId.Rotate);
			}
			bool flag3 = !flag;
			if (flag3)
			{
				this.ve.computedStyle.ApplyStyleRotate(rotate.value);
			}
		}

		// Token: 0x06001391 RID: 5009 RVA: 0x00055034 File Offset: 0x00053234
		private void ApplyStyleValue(StyleValue value)
		{
			VisualElement parent = this.ve.hierarchy.parent;
			ref ComputedStyle ptr;
			if (parent != null)
			{
				ref ComputedStyle computedStyle = ref parent.computedStyle;
				ptr = parent.computedStyle;
			}
			else
			{
				ptr = InitialStyle.Get();
			}
			ref ComputedStyle ptr2 = ref ptr;
			bool flag = false;
			bool flag2 = StylePropertyUtil.IsAnimatable(value.id);
			if (flag2)
			{
				ComputedTransitionUtils.UpdateComputedTransitions(this.ve.computedStyle);
				ComputedTransitionProperty computedTransitionProperty;
				bool flag3 = this.ve.computedStyle.hasTransition && this.ve.styleInitialized && this.ve.computedStyle.GetTransitionProperty(value.id, out computedTransitionProperty);
				if (flag3)
				{
					flag = ComputedStyle.StartAnimationInline(this.ve, value.id, this.ve.computedStyle, value, computedTransitionProperty.durationMs, computedTransitionProperty.delayMs, computedTransitionProperty.easingCurve);
				}
				else
				{
					this.ve.styleAnimation.CancelAnimation(value.id);
				}
			}
			bool flag4 = !flag;
			if (flag4)
			{
				this.ve.computedStyle.ApplyStyleValue(value, ref ptr2);
			}
		}

		// Token: 0x06001392 RID: 5010 RVA: 0x00055148 File Offset: 0x00053348
		private void ApplyStyleValue(StyleValueManaged value)
		{
			VisualElement parent = this.ve.hierarchy.parent;
			ref ComputedStyle ptr;
			if (parent != null)
			{
				ref ComputedStyle computedStyle = ref parent.computedStyle;
				ptr = parent.computedStyle;
			}
			else
			{
				ptr = InitialStyle.Get();
			}
			ref ComputedStyle ptr2 = ref ptr;
			this.ve.computedStyle.ApplyStyleValueManaged(value, ref ptr2);
		}

		// Token: 0x06001393 RID: 5011 RVA: 0x00055198 File Offset: 0x00053398
		private bool RemoveInlineStyle(StylePropertyId id)
		{
			long matchingRulesHash = this.ve.computedStyle.matchingRulesHash;
			bool flag = matchingRulesHash == 0L;
			bool flag2;
			if (flag)
			{
				this.ApplyFromComputedStyle(id, InitialStyle.Get());
				flag2 = true;
			}
			else
			{
				ComputedStyle computedStyle;
				bool flag3 = StyleCache.TryGetValue(matchingRulesHash, out computedStyle);
				if (flag3)
				{
					this.ApplyFromComputedStyle(id, ref computedStyle);
					flag2 = true;
				}
				else
				{
					flag2 = false;
				}
			}
			return flag2;
		}

		// Token: 0x06001394 RID: 5012 RVA: 0x000551F4 File Offset: 0x000533F4
		private void ApplyFromComputedStyle(StylePropertyId id, ref ComputedStyle newStyle)
		{
			bool flag = false;
			bool flag2 = StylePropertyUtil.IsAnimatable(id);
			if (flag2)
			{
				ComputedTransitionUtils.UpdateComputedTransitions(this.ve.computedStyle);
				ComputedTransitionProperty computedTransitionProperty;
				bool flag3 = this.ve.computedStyle.hasTransition && this.ve.styleInitialized && this.ve.computedStyle.GetTransitionProperty(id, out computedTransitionProperty);
				if (flag3)
				{
					flag = ComputedStyle.StartAnimation(this.ve, id, this.ve.computedStyle, ref newStyle, computedTransitionProperty.durationMs, computedTransitionProperty.delayMs, computedTransitionProperty.easingCurve);
				}
				else
				{
					this.ve.styleAnimation.CancelAnimation(id);
				}
			}
			bool flag4 = !flag;
			if (flag4)
			{
				this.ve.computedStyle.ApplyFromComputedStyle(id, ref newStyle);
			}
		}

		// Token: 0x06001395 RID: 5013 RVA: 0x000552C0 File Offset: 0x000534C0
		public bool TryGetInlineCursor(ref StyleCursor value)
		{
			bool hasInlineCursor = this.m_HasInlineCursor;
			bool flag;
			if (hasInlineCursor)
			{
				value = this.m_InlineCursor;
				flag = true;
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001396 RID: 5014 RVA: 0x000552F0 File Offset: 0x000534F0
		public bool TryGetInlineTextShadow(ref StyleTextShadow value)
		{
			bool hasInlineTextShadow = this.m_HasInlineTextShadow;
			bool flag;
			if (hasInlineTextShadow)
			{
				value = this.m_InlineTextShadow;
				flag = true;
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001397 RID: 5015 RVA: 0x00055320 File Offset: 0x00053520
		public bool TryGetInlineTransformOrigin(ref StyleTransformOrigin value)
		{
			bool hasInlineTransformOrigin = this.m_HasInlineTransformOrigin;
			bool flag;
			if (hasInlineTransformOrigin)
			{
				value = this.m_InlineTransformOrigin;
				flag = true;
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001398 RID: 5016 RVA: 0x00055350 File Offset: 0x00053550
		public bool TryGetInlineTranslate(ref StyleTranslate value)
		{
			bool hasInlineTranslate = this.m_HasInlineTranslate;
			bool flag;
			if (hasInlineTranslate)
			{
				value = this.m_InlineTranslateOperation;
				flag = true;
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001399 RID: 5017 RVA: 0x00055380 File Offset: 0x00053580
		public bool TryGetInlineRotate(ref StyleRotate value)
		{
			bool hasInlineRotate = this.m_HasInlineRotate;
			bool flag;
			if (hasInlineRotate)
			{
				value = this.m_InlineRotateOperation;
				flag = true;
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x0600139A RID: 5018 RVA: 0x000553B0 File Offset: 0x000535B0
		public bool TryGetInlineScale(ref StyleScale value)
		{
			bool hasInlineScale = this.m_HasInlineScale;
			bool flag;
			if (hasInlineScale)
			{
				value = this.m_InlineScale;
				flag = true;
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x0600139B RID: 5019 RVA: 0x000553E0 File Offset: 0x000535E0
		// (set) Token: 0x0600139C RID: 5020 RVA: 0x00055414 File Offset: 0x00053614
		StyleEnum<Align> IStyle.alignContent
		{
			get
			{
				StyleInt styleInt = base.GetStyleInt(StylePropertyId.AlignContent);
				return new StyleEnum<Align>((Align)styleInt.value, styleInt.keyword);
			}
			set
			{
				bool flag = this.SetStyleValue<Align>(StylePropertyId.AlignContent, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.AlignContent = (YogaAlign)this.ve.computedStyle.alignContent;
				}
			}
		}

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x0600139D RID: 5021 RVA: 0x00055464 File Offset: 0x00053664
		// (set) Token: 0x0600139E RID: 5022 RVA: 0x00055498 File Offset: 0x00053698
		StyleEnum<Align> IStyle.alignItems
		{
			get
			{
				StyleInt styleInt = base.GetStyleInt(StylePropertyId.AlignItems);
				return new StyleEnum<Align>((Align)styleInt.value, styleInt.keyword);
			}
			set
			{
				bool flag = this.SetStyleValue<Align>(StylePropertyId.AlignItems, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.AlignItems = (YogaAlign)this.ve.computedStyle.alignItems;
				}
			}
		}

		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x0600139F RID: 5023 RVA: 0x000554E8 File Offset: 0x000536E8
		// (set) Token: 0x060013A0 RID: 5024 RVA: 0x0005551C File Offset: 0x0005371C
		StyleEnum<Align> IStyle.alignSelf
		{
			get
			{
				StyleInt styleInt = base.GetStyleInt(StylePropertyId.AlignSelf);
				return new StyleEnum<Align>((Align)styleInt.value, styleInt.keyword);
			}
			set
			{
				bool flag = this.SetStyleValue<Align>(StylePropertyId.AlignSelf, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.AlignSelf = (YogaAlign)this.ve.computedStyle.alignSelf;
				}
			}
		}

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x060013A1 RID: 5025 RVA: 0x0005556C File Offset: 0x0005376C
		// (set) Token: 0x060013A2 RID: 5026 RVA: 0x0005558C File Offset: 0x0005378C
		StyleColor IStyle.backgroundColor
		{
			get
			{
				return base.GetStyleColor(StylePropertyId.BackgroundColor);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.BackgroundColor, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Color);
				}
			}
		}

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x060013A3 RID: 5027 RVA: 0x000555C0 File Offset: 0x000537C0
		// (set) Token: 0x060013A4 RID: 5028 RVA: 0x000555E0 File Offset: 0x000537E0
		StyleBackground IStyle.backgroundImage
		{
			get
			{
				return base.GetStyleBackground(StylePropertyId.BackgroundImage);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.BackgroundImage, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x060013A5 RID: 5029 RVA: 0x00055614 File Offset: 0x00053814
		// (set) Token: 0x060013A6 RID: 5030 RVA: 0x00055634 File Offset: 0x00053834
		StyleColor IStyle.borderBottomColor
		{
			get
			{
				return base.GetStyleColor(StylePropertyId.BorderBottomColor);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.BorderBottomColor, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Color);
				}
			}
		}

		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x060013A7 RID: 5031 RVA: 0x00055668 File Offset: 0x00053868
		// (set) Token: 0x060013A8 RID: 5032 RVA: 0x00055688 File Offset: 0x00053888
		StyleLength IStyle.borderBottomLeftRadius
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.BorderBottomLeftRadius);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.BorderBottomLeftRadius, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.BorderRadius | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x060013A9 RID: 5033 RVA: 0x000556BC File Offset: 0x000538BC
		// (set) Token: 0x060013AA RID: 5034 RVA: 0x000556DC File Offset: 0x000538DC
		StyleLength IStyle.borderBottomRightRadius
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.BorderBottomRightRadius);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.BorderBottomRightRadius, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.BorderRadius | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x060013AB RID: 5035 RVA: 0x00055710 File Offset: 0x00053910
		// (set) Token: 0x060013AC RID: 5036 RVA: 0x00055730 File Offset: 0x00053930
		StyleFloat IStyle.borderBottomWidth
		{
			get
			{
				return base.GetStyleFloat(StylePropertyId.BorderBottomWidth);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.BorderBottomWidth, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles | VersionChangeType.BorderWidth | VersionChangeType.Repaint);
					this.ve.yogaNode.BorderBottomWidth = this.ve.computedStyle.borderBottomWidth;
				}
			}
		}

		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x060013AD RID: 5037 RVA: 0x00055784 File Offset: 0x00053984
		// (set) Token: 0x060013AE RID: 5038 RVA: 0x000557A4 File Offset: 0x000539A4
		StyleColor IStyle.borderLeftColor
		{
			get
			{
				return base.GetStyleColor(StylePropertyId.BorderLeftColor);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.BorderLeftColor, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Color);
				}
			}
		}

		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x060013AF RID: 5039 RVA: 0x000557D8 File Offset: 0x000539D8
		// (set) Token: 0x060013B0 RID: 5040 RVA: 0x000557F8 File Offset: 0x000539F8
		StyleFloat IStyle.borderLeftWidth
		{
			get
			{
				return base.GetStyleFloat(StylePropertyId.BorderLeftWidth);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.BorderLeftWidth, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles | VersionChangeType.BorderWidth | VersionChangeType.Repaint);
					this.ve.yogaNode.BorderLeftWidth = this.ve.computedStyle.borderLeftWidth;
				}
			}
		}

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x060013B1 RID: 5041 RVA: 0x0005584C File Offset: 0x00053A4C
		// (set) Token: 0x060013B2 RID: 5042 RVA: 0x0005586C File Offset: 0x00053A6C
		StyleColor IStyle.borderRightColor
		{
			get
			{
				return base.GetStyleColor(StylePropertyId.BorderRightColor);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.BorderRightColor, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Color);
				}
			}
		}

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x060013B3 RID: 5043 RVA: 0x000558A0 File Offset: 0x00053AA0
		// (set) Token: 0x060013B4 RID: 5044 RVA: 0x000558C0 File Offset: 0x00053AC0
		StyleFloat IStyle.borderRightWidth
		{
			get
			{
				return base.GetStyleFloat(StylePropertyId.BorderRightWidth);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.BorderRightWidth, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles | VersionChangeType.BorderWidth | VersionChangeType.Repaint);
					this.ve.yogaNode.BorderRightWidth = this.ve.computedStyle.borderRightWidth;
				}
			}
		}

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x060013B5 RID: 5045 RVA: 0x00055914 File Offset: 0x00053B14
		// (set) Token: 0x060013B6 RID: 5046 RVA: 0x00055934 File Offset: 0x00053B34
		StyleColor IStyle.borderTopColor
		{
			get
			{
				return base.GetStyleColor(StylePropertyId.BorderTopColor);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.BorderTopColor, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Color);
				}
			}
		}

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x060013B7 RID: 5047 RVA: 0x00055968 File Offset: 0x00053B68
		// (set) Token: 0x060013B8 RID: 5048 RVA: 0x00055988 File Offset: 0x00053B88
		StyleLength IStyle.borderTopLeftRadius
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.BorderTopLeftRadius);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.BorderTopLeftRadius, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.BorderRadius | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x060013B9 RID: 5049 RVA: 0x000559BC File Offset: 0x00053BBC
		// (set) Token: 0x060013BA RID: 5050 RVA: 0x000559DC File Offset: 0x00053BDC
		StyleLength IStyle.borderTopRightRadius
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.BorderTopRightRadius);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.BorderTopRightRadius, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.BorderRadius | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x060013BB RID: 5051 RVA: 0x00055A10 File Offset: 0x00053C10
		// (set) Token: 0x060013BC RID: 5052 RVA: 0x00055A30 File Offset: 0x00053C30
		StyleFloat IStyle.borderTopWidth
		{
			get
			{
				return base.GetStyleFloat(StylePropertyId.BorderTopWidth);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.BorderTopWidth, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles | VersionChangeType.BorderWidth | VersionChangeType.Repaint);
					this.ve.yogaNode.BorderTopWidth = this.ve.computedStyle.borderTopWidth;
				}
			}
		}

		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x060013BD RID: 5053 RVA: 0x00055A84 File Offset: 0x00053C84
		// (set) Token: 0x060013BE RID: 5054 RVA: 0x00055AA4 File Offset: 0x00053CA4
		StyleLength IStyle.bottom
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.Bottom);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.Bottom, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.Bottom = this.ve.computedStyle.bottom.ToYogaValue();
				}
			}
		}

		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x060013BF RID: 5055 RVA: 0x00055AF8 File Offset: 0x00053CF8
		// (set) Token: 0x060013C0 RID: 5056 RVA: 0x00055B18 File Offset: 0x00053D18
		StyleColor IStyle.color
		{
			get
			{
				return base.GetStyleColor(StylePropertyId.Color);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.Color, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.StyleSheet | VersionChangeType.Styles | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x060013C1 RID: 5057 RVA: 0x00055B4C File Offset: 0x00053D4C
		// (set) Token: 0x060013C2 RID: 5058 RVA: 0x00055B80 File Offset: 0x00053D80
		StyleEnum<DisplayStyle> IStyle.display
		{
			get
			{
				StyleInt styleInt = base.GetStyleInt(StylePropertyId.Display);
				return new StyleEnum<DisplayStyle>((DisplayStyle)styleInt.value, styleInt.keyword);
			}
			set
			{
				bool flag = this.SetStyleValue<DisplayStyle>(StylePropertyId.Display, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles | VersionChangeType.Repaint);
					this.ve.yogaNode.Display = (YogaDisplay)this.ve.computedStyle.display;
				}
			}
		}

		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x060013C3 RID: 5059 RVA: 0x00055BD4 File Offset: 0x00053DD4
		// (set) Token: 0x060013C4 RID: 5060 RVA: 0x00055BF4 File Offset: 0x00053DF4
		StyleLength IStyle.flexBasis
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.FlexBasis);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.FlexBasis, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.FlexBasis = this.ve.computedStyle.flexBasis.ToYogaValue();
				}
			}
		}

		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x060013C5 RID: 5061 RVA: 0x00055C48 File Offset: 0x00053E48
		// (set) Token: 0x060013C6 RID: 5062 RVA: 0x00055C7C File Offset: 0x00053E7C
		StyleEnum<FlexDirection> IStyle.flexDirection
		{
			get
			{
				StyleInt styleInt = base.GetStyleInt(StylePropertyId.FlexDirection);
				return new StyleEnum<FlexDirection>((FlexDirection)styleInt.value, styleInt.keyword);
			}
			set
			{
				bool flag = this.SetStyleValue<FlexDirection>(StylePropertyId.FlexDirection, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.FlexDirection = (YogaFlexDirection)this.ve.computedStyle.flexDirection;
				}
			}
		}

		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x060013C7 RID: 5063 RVA: 0x00055CCC File Offset: 0x00053ECC
		// (set) Token: 0x060013C8 RID: 5064 RVA: 0x00055CEC File Offset: 0x00053EEC
		StyleFloat IStyle.flexGrow
		{
			get
			{
				return base.GetStyleFloat(StylePropertyId.FlexGrow);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.FlexGrow, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.FlexGrow = this.ve.computedStyle.flexGrow;
				}
			}
		}

		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x060013C9 RID: 5065 RVA: 0x00055D3C File Offset: 0x00053F3C
		// (set) Token: 0x060013CA RID: 5066 RVA: 0x00055D5C File Offset: 0x00053F5C
		StyleFloat IStyle.flexShrink
		{
			get
			{
				return base.GetStyleFloat(StylePropertyId.FlexShrink);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.FlexShrink, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.FlexShrink = this.ve.computedStyle.flexShrink;
				}
			}
		}

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x060013CB RID: 5067 RVA: 0x00055DAC File Offset: 0x00053FAC
		// (set) Token: 0x060013CC RID: 5068 RVA: 0x00055DE0 File Offset: 0x00053FE0
		StyleEnum<Wrap> IStyle.flexWrap
		{
			get
			{
				StyleInt styleInt = base.GetStyleInt(StylePropertyId.FlexWrap);
				return new StyleEnum<Wrap>((Wrap)styleInt.value, styleInt.keyword);
			}
			set
			{
				bool flag = this.SetStyleValue<Wrap>(StylePropertyId.FlexWrap, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.Wrap = (YogaWrap)this.ve.computedStyle.flexWrap;
				}
			}
		}

		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x060013CD RID: 5069 RVA: 0x00055E30 File Offset: 0x00054030
		// (set) Token: 0x060013CE RID: 5070 RVA: 0x00055E50 File Offset: 0x00054050
		StyleLength IStyle.fontSize
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.FontSize);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.FontSize, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.StyleSheet | VersionChangeType.Styles);
				}
			}
		}

		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x060013CF RID: 5071 RVA: 0x00055E80 File Offset: 0x00054080
		// (set) Token: 0x060013D0 RID: 5072 RVA: 0x00055EA0 File Offset: 0x000540A0
		StyleLength IStyle.height
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.Height);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.Height, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.Height = this.ve.computedStyle.height.ToYogaValue();
				}
			}
		}

		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x060013D1 RID: 5073 RVA: 0x00055EF4 File Offset: 0x000540F4
		// (set) Token: 0x060013D2 RID: 5074 RVA: 0x00055F28 File Offset: 0x00054128
		StyleEnum<Justify> IStyle.justifyContent
		{
			get
			{
				StyleInt styleInt = base.GetStyleInt(StylePropertyId.JustifyContent);
				return new StyleEnum<Justify>((Justify)styleInt.value, styleInt.keyword);
			}
			set
			{
				bool flag = this.SetStyleValue<Justify>(StylePropertyId.JustifyContent, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.JustifyContent = (YogaJustify)this.ve.computedStyle.justifyContent;
				}
			}
		}

		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x060013D3 RID: 5075 RVA: 0x00055F78 File Offset: 0x00054178
		// (set) Token: 0x060013D4 RID: 5076 RVA: 0x00055F98 File Offset: 0x00054198
		StyleLength IStyle.left
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.Left);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.Left, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.Left = this.ve.computedStyle.left.ToYogaValue();
				}
			}
		}

		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x060013D5 RID: 5077 RVA: 0x00055FEC File Offset: 0x000541EC
		// (set) Token: 0x060013D6 RID: 5078 RVA: 0x0005600C File Offset: 0x0005420C
		StyleLength IStyle.letterSpacing
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.LetterSpacing);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.LetterSpacing, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.StyleSheet | VersionChangeType.Styles | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x060013D7 RID: 5079 RVA: 0x00056040 File Offset: 0x00054240
		// (set) Token: 0x060013D8 RID: 5080 RVA: 0x00056060 File Offset: 0x00054260
		StyleLength IStyle.marginBottom
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.MarginBottom);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.MarginBottom, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.MarginBottom = this.ve.computedStyle.marginBottom.ToYogaValue();
				}
			}
		}

		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x060013D9 RID: 5081 RVA: 0x000560B4 File Offset: 0x000542B4
		// (set) Token: 0x060013DA RID: 5082 RVA: 0x000560D4 File Offset: 0x000542D4
		StyleLength IStyle.marginLeft
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.MarginLeft);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.MarginLeft, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.MarginLeft = this.ve.computedStyle.marginLeft.ToYogaValue();
				}
			}
		}

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x060013DB RID: 5083 RVA: 0x00056128 File Offset: 0x00054328
		// (set) Token: 0x060013DC RID: 5084 RVA: 0x00056148 File Offset: 0x00054348
		StyleLength IStyle.marginRight
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.MarginRight);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.MarginRight, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.MarginRight = this.ve.computedStyle.marginRight.ToYogaValue();
				}
			}
		}

		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x060013DD RID: 5085 RVA: 0x0005619C File Offset: 0x0005439C
		// (set) Token: 0x060013DE RID: 5086 RVA: 0x000561BC File Offset: 0x000543BC
		StyleLength IStyle.marginTop
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.MarginTop);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.MarginTop, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.MarginTop = this.ve.computedStyle.marginTop.ToYogaValue();
				}
			}
		}

		// Token: 0x1700049E RID: 1182
		// (get) Token: 0x060013DF RID: 5087 RVA: 0x00056210 File Offset: 0x00054410
		// (set) Token: 0x060013E0 RID: 5088 RVA: 0x00056230 File Offset: 0x00054430
		StyleLength IStyle.maxHeight
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.MaxHeight);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.MaxHeight, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.MaxHeight = this.ve.computedStyle.maxHeight.ToYogaValue();
				}
			}
		}

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x060013E1 RID: 5089 RVA: 0x00056284 File Offset: 0x00054484
		// (set) Token: 0x060013E2 RID: 5090 RVA: 0x000562A4 File Offset: 0x000544A4
		StyleLength IStyle.maxWidth
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.MaxWidth);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.MaxWidth, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.MaxWidth = this.ve.computedStyle.maxWidth.ToYogaValue();
				}
			}
		}

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x060013E3 RID: 5091 RVA: 0x000562F8 File Offset: 0x000544F8
		// (set) Token: 0x060013E4 RID: 5092 RVA: 0x00056318 File Offset: 0x00054518
		StyleLength IStyle.minHeight
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.MinHeight);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.MinHeight, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.MinHeight = this.ve.computedStyle.minHeight.ToYogaValue();
				}
			}
		}

		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x060013E5 RID: 5093 RVA: 0x0005636C File Offset: 0x0005456C
		// (set) Token: 0x060013E6 RID: 5094 RVA: 0x0005638C File Offset: 0x0005458C
		StyleLength IStyle.minWidth
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.MinWidth);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.MinWidth, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.MinWidth = this.ve.computedStyle.minWidth.ToYogaValue();
				}
			}
		}

		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x060013E7 RID: 5095 RVA: 0x000563E0 File Offset: 0x000545E0
		// (set) Token: 0x060013E8 RID: 5096 RVA: 0x00056400 File Offset: 0x00054600
		StyleFloat IStyle.opacity
		{
			get
			{
				return base.GetStyleFloat(StylePropertyId.Opacity);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.Opacity, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Opacity);
				}
			}
		}

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x060013E9 RID: 5097 RVA: 0x00056434 File Offset: 0x00054634
		// (set) Token: 0x060013EA RID: 5098 RVA: 0x00056468 File Offset: 0x00054668
		StyleEnum<Overflow> IStyle.overflow
		{
			get
			{
				StyleInt styleInt = base.GetStyleInt(StylePropertyId.Overflow);
				return new StyleEnum<Overflow>((Overflow)styleInt.value, styleInt.keyword);
			}
			set
			{
				StyleEnum<OverflowInternal> styleEnum = new StyleEnum<OverflowInternal>((OverflowInternal)value.value, value.keyword);
				bool flag = this.SetStyleValue<OverflowInternal>(StylePropertyId.Overflow, styleEnum);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles | VersionChangeType.Overflow);
					this.ve.yogaNode.Overflow = (YogaOverflow)this.ve.computedStyle.overflow;
				}
			}
		}

		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x060013EB RID: 5099 RVA: 0x000564CC File Offset: 0x000546CC
		// (set) Token: 0x060013EC RID: 5100 RVA: 0x000564EC File Offset: 0x000546EC
		StyleLength IStyle.paddingBottom
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.PaddingBottom);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.PaddingBottom, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.PaddingBottom = this.ve.computedStyle.paddingBottom.ToYogaValue();
				}
			}
		}

		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x060013ED RID: 5101 RVA: 0x00056540 File Offset: 0x00054740
		// (set) Token: 0x060013EE RID: 5102 RVA: 0x00056560 File Offset: 0x00054760
		StyleLength IStyle.paddingLeft
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.PaddingLeft);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.PaddingLeft, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.PaddingLeft = this.ve.computedStyle.paddingLeft.ToYogaValue();
				}
			}
		}

		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x060013EF RID: 5103 RVA: 0x000565B4 File Offset: 0x000547B4
		// (set) Token: 0x060013F0 RID: 5104 RVA: 0x000565D4 File Offset: 0x000547D4
		StyleLength IStyle.paddingRight
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.PaddingRight);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.PaddingRight, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.PaddingRight = this.ve.computedStyle.paddingRight.ToYogaValue();
				}
			}
		}

		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x060013F1 RID: 5105 RVA: 0x00056628 File Offset: 0x00054828
		// (set) Token: 0x060013F2 RID: 5106 RVA: 0x00056648 File Offset: 0x00054848
		StyleLength IStyle.paddingTop
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.PaddingTop);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.PaddingTop, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.PaddingTop = this.ve.computedStyle.paddingTop.ToYogaValue();
				}
			}
		}

		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x060013F3 RID: 5107 RVA: 0x0005669C File Offset: 0x0005489C
		// (set) Token: 0x060013F4 RID: 5108 RVA: 0x000566D0 File Offset: 0x000548D0
		StyleEnum<Position> IStyle.position
		{
			get
			{
				StyleInt styleInt = base.GetStyleInt(StylePropertyId.Position);
				return new StyleEnum<Position>((Position)styleInt.value, styleInt.keyword);
			}
			set
			{
				bool flag = this.SetStyleValue<Position>(StylePropertyId.Position, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.PositionType = (YogaPositionType)this.ve.computedStyle.position;
				}
			}
		}

		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x060013F5 RID: 5109 RVA: 0x00056720 File Offset: 0x00054920
		// (set) Token: 0x060013F6 RID: 5110 RVA: 0x00056740 File Offset: 0x00054940
		StyleLength IStyle.right
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.Right);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.Right, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.Right = this.ve.computedStyle.right.ToYogaValue();
				}
			}
		}

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x060013F7 RID: 5111 RVA: 0x00056794 File Offset: 0x00054994
		// (set) Token: 0x060013F8 RID: 5112 RVA: 0x000567C8 File Offset: 0x000549C8
		StyleEnum<TextOverflow> IStyle.textOverflow
		{
			get
			{
				StyleInt styleInt = base.GetStyleInt(StylePropertyId.TextOverflow);
				return new StyleEnum<TextOverflow>((TextOverflow)styleInt.value, styleInt.keyword);
			}
			set
			{
				bool flag = this.SetStyleValue<TextOverflow>(StylePropertyId.TextOverflow, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x060013F9 RID: 5113 RVA: 0x000567FC File Offset: 0x000549FC
		// (set) Token: 0x060013FA RID: 5114 RVA: 0x0005681C File Offset: 0x00054A1C
		StyleLength IStyle.top
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.Top);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.Top, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.Top = this.ve.computedStyle.top.ToYogaValue();
				}
			}
		}

		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x060013FB RID: 5115 RVA: 0x00056870 File Offset: 0x00054A70
		// (set) Token: 0x060013FC RID: 5116 RVA: 0x00056890 File Offset: 0x00054A90
		StyleList<TimeValue> IStyle.transitionDelay
		{
			get
			{
				return this.GetStyleList<TimeValue>(StylePropertyId.TransitionDelay);
			}
			set
			{
				bool flag = this.SetStyleValue<TimeValue>(StylePropertyId.TransitionDelay, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.TransitionProperty);
				}
			}
		}

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x060013FD RID: 5117 RVA: 0x000568C4 File Offset: 0x00054AC4
		// (set) Token: 0x060013FE RID: 5118 RVA: 0x000568E4 File Offset: 0x00054AE4
		StyleList<TimeValue> IStyle.transitionDuration
		{
			get
			{
				return this.GetStyleList<TimeValue>(StylePropertyId.TransitionDuration);
			}
			set
			{
				bool flag = this.SetStyleValue<TimeValue>(StylePropertyId.TransitionDuration, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.TransitionProperty);
				}
			}
		}

		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x060013FF RID: 5119 RVA: 0x00056918 File Offset: 0x00054B18
		// (set) Token: 0x06001400 RID: 5120 RVA: 0x00056938 File Offset: 0x00054B38
		StyleList<StylePropertyName> IStyle.transitionProperty
		{
			get
			{
				return this.GetStyleList<StylePropertyName>(StylePropertyId.TransitionProperty);
			}
			set
			{
				bool flag = this.SetStyleValue<StylePropertyName>(StylePropertyId.TransitionProperty, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.TransitionProperty);
				}
			}
		}

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x06001401 RID: 5121 RVA: 0x0005696C File Offset: 0x00054B6C
		// (set) Token: 0x06001402 RID: 5122 RVA: 0x0005698C File Offset: 0x00054B8C
		StyleList<EasingFunction> IStyle.transitionTimingFunction
		{
			get
			{
				return this.GetStyleList<EasingFunction>(StylePropertyId.TransitionTimingFunction);
			}
			set
			{
				bool flag = this.SetStyleValue<EasingFunction>(StylePropertyId.TransitionTimingFunction, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles);
				}
			}
		}

		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x06001403 RID: 5123 RVA: 0x000569BC File Offset: 0x00054BBC
		// (set) Token: 0x06001404 RID: 5124 RVA: 0x000569DC File Offset: 0x00054BDC
		StyleColor IStyle.unityBackgroundImageTintColor
		{
			get
			{
				return base.GetStyleColor(StylePropertyId.UnityBackgroundImageTintColor);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.UnityBackgroundImageTintColor, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Color);
				}
			}
		}

		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x06001405 RID: 5125 RVA: 0x00056A10 File Offset: 0x00054C10
		// (set) Token: 0x06001406 RID: 5126 RVA: 0x00056A44 File Offset: 0x00054C44
		StyleEnum<ScaleMode> IStyle.unityBackgroundScaleMode
		{
			get
			{
				StyleInt styleInt = base.GetStyleInt(StylePropertyId.UnityBackgroundScaleMode);
				return new StyleEnum<ScaleMode>((ScaleMode)styleInt.value, styleInt.keyword);
			}
			set
			{
				bool flag = this.SetStyleValue<ScaleMode>(StylePropertyId.UnityBackgroundScaleMode, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x06001407 RID: 5127 RVA: 0x00056A78 File Offset: 0x00054C78
		// (set) Token: 0x06001408 RID: 5128 RVA: 0x00056A98 File Offset: 0x00054C98
		StyleFont IStyle.unityFont
		{
			get
			{
				return base.GetStyleFont(StylePropertyId.UnityFont);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.UnityFont, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.StyleSheet | VersionChangeType.Styles | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x06001409 RID: 5129 RVA: 0x00056ACC File Offset: 0x00054CCC
		// (set) Token: 0x0600140A RID: 5130 RVA: 0x00056AEC File Offset: 0x00054CEC
		StyleFontDefinition IStyle.unityFontDefinition
		{
			get
			{
				return base.GetStyleFontDefinition(StylePropertyId.UnityFontDefinition);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.UnityFontDefinition, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.StyleSheet | VersionChangeType.Styles | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x0600140B RID: 5131 RVA: 0x00056B20 File Offset: 0x00054D20
		// (set) Token: 0x0600140C RID: 5132 RVA: 0x00056B54 File Offset: 0x00054D54
		StyleEnum<FontStyle> IStyle.unityFontStyleAndWeight
		{
			get
			{
				StyleInt styleInt = base.GetStyleInt(StylePropertyId.UnityFontStyleAndWeight);
				return new StyleEnum<FontStyle>((FontStyle)styleInt.value, styleInt.keyword);
			}
			set
			{
				bool flag = this.SetStyleValue<FontStyle>(StylePropertyId.UnityFontStyleAndWeight, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.StyleSheet | VersionChangeType.Styles | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x0600140D RID: 5133 RVA: 0x00056B88 File Offset: 0x00054D88
		// (set) Token: 0x0600140E RID: 5134 RVA: 0x00056BBC File Offset: 0x00054DBC
		StyleEnum<OverflowClipBox> IStyle.unityOverflowClipBox
		{
			get
			{
				StyleInt styleInt = base.GetStyleInt(StylePropertyId.UnityOverflowClipBox);
				return new StyleEnum<OverflowClipBox>((OverflowClipBox)styleInt.value, styleInt.keyword);
			}
			set
			{
				bool flag = this.SetStyleValue<OverflowClipBox>(StylePropertyId.UnityOverflowClipBox, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x0600140F RID: 5135 RVA: 0x00056BF0 File Offset: 0x00054DF0
		// (set) Token: 0x06001410 RID: 5136 RVA: 0x00056C10 File Offset: 0x00054E10
		StyleLength IStyle.unityParagraphSpacing
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.UnityParagraphSpacing);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.UnityParagraphSpacing, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.StyleSheet | VersionChangeType.Styles | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x06001411 RID: 5137 RVA: 0x00056C44 File Offset: 0x00054E44
		// (set) Token: 0x06001412 RID: 5138 RVA: 0x00056C64 File Offset: 0x00054E64
		StyleInt IStyle.unitySliceBottom
		{
			get
			{
				return base.GetStyleInt(StylePropertyId.UnitySliceBottom);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.UnitySliceBottom, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x06001413 RID: 5139 RVA: 0x00056C98 File Offset: 0x00054E98
		// (set) Token: 0x06001414 RID: 5140 RVA: 0x00056CB8 File Offset: 0x00054EB8
		StyleInt IStyle.unitySliceLeft
		{
			get
			{
				return base.GetStyleInt(StylePropertyId.UnitySliceLeft);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.UnitySliceLeft, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x06001415 RID: 5141 RVA: 0x00056CEC File Offset: 0x00054EEC
		// (set) Token: 0x06001416 RID: 5142 RVA: 0x00056D0C File Offset: 0x00054F0C
		StyleInt IStyle.unitySliceRight
		{
			get
			{
				return base.GetStyleInt(StylePropertyId.UnitySliceRight);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.UnitySliceRight, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x06001417 RID: 5143 RVA: 0x00056D40 File Offset: 0x00054F40
		// (set) Token: 0x06001418 RID: 5144 RVA: 0x00056D60 File Offset: 0x00054F60
		StyleInt IStyle.unitySliceTop
		{
			get
			{
				return base.GetStyleInt(StylePropertyId.UnitySliceTop);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.UnitySliceTop, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x06001419 RID: 5145 RVA: 0x00056D94 File Offset: 0x00054F94
		// (set) Token: 0x0600141A RID: 5146 RVA: 0x00056DC8 File Offset: 0x00054FC8
		StyleEnum<TextAnchor> IStyle.unityTextAlign
		{
			get
			{
				StyleInt styleInt = base.GetStyleInt(StylePropertyId.UnityTextAlign);
				return new StyleEnum<TextAnchor>((TextAnchor)styleInt.value, styleInt.keyword);
			}
			set
			{
				bool flag = this.SetStyleValue<TextAnchor>(StylePropertyId.UnityTextAlign, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.StyleSheet | VersionChangeType.Styles | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x0600141B RID: 5147 RVA: 0x00056DFC File Offset: 0x00054FFC
		// (set) Token: 0x0600141C RID: 5148 RVA: 0x00056E1C File Offset: 0x0005501C
		StyleColor IStyle.unityTextOutlineColor
		{
			get
			{
				return base.GetStyleColor(StylePropertyId.UnityTextOutlineColor);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.UnityTextOutlineColor, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.StyleSheet | VersionChangeType.Styles | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x0600141D RID: 5149 RVA: 0x00056E50 File Offset: 0x00055050
		// (set) Token: 0x0600141E RID: 5150 RVA: 0x00056E70 File Offset: 0x00055070
		StyleFloat IStyle.unityTextOutlineWidth
		{
			get
			{
				return base.GetStyleFloat(StylePropertyId.UnityTextOutlineWidth);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.UnityTextOutlineWidth, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.StyleSheet | VersionChangeType.Styles | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x0600141F RID: 5151 RVA: 0x00056EA4 File Offset: 0x000550A4
		// (set) Token: 0x06001420 RID: 5152 RVA: 0x00056ED8 File Offset: 0x000550D8
		StyleEnum<TextOverflowPosition> IStyle.unityTextOverflowPosition
		{
			get
			{
				StyleInt styleInt = base.GetStyleInt(StylePropertyId.UnityTextOverflowPosition);
				return new StyleEnum<TextOverflowPosition>((TextOverflowPosition)styleInt.value, styleInt.keyword);
			}
			set
			{
				bool flag = this.SetStyleValue<TextOverflowPosition>(StylePropertyId.UnityTextOverflowPosition, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Styles | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x06001421 RID: 5153 RVA: 0x00056F0C File Offset: 0x0005510C
		// (set) Token: 0x06001422 RID: 5154 RVA: 0x00056F40 File Offset: 0x00055140
		StyleEnum<Visibility> IStyle.visibility
		{
			get
			{
				StyleInt styleInt = base.GetStyleInt(StylePropertyId.Visibility);
				return new StyleEnum<Visibility>((Visibility)styleInt.value, styleInt.keyword);
			}
			set
			{
				bool flag = this.SetStyleValue<Visibility>(StylePropertyId.Visibility, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.StyleSheet | VersionChangeType.Styles | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x06001423 RID: 5155 RVA: 0x00056F74 File Offset: 0x00055174
		// (set) Token: 0x06001424 RID: 5156 RVA: 0x00056FA8 File Offset: 0x000551A8
		StyleEnum<WhiteSpace> IStyle.whiteSpace
		{
			get
			{
				StyleInt styleInt = base.GetStyleInt(StylePropertyId.WhiteSpace);
				return new StyleEnum<WhiteSpace>((WhiteSpace)styleInt.value, styleInt.keyword);
			}
			set
			{
				bool flag = this.SetStyleValue<WhiteSpace>(StylePropertyId.WhiteSpace, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.StyleSheet | VersionChangeType.Styles);
				}
			}
		}

		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x06001425 RID: 5157 RVA: 0x00056FD8 File Offset: 0x000551D8
		// (set) Token: 0x06001426 RID: 5158 RVA: 0x00056FF8 File Offset: 0x000551F8
		StyleLength IStyle.width
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.Width);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.Width, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.Styles);
					this.ve.yogaNode.Width = this.ve.computedStyle.width.ToYogaValue();
				}
			}
		}

		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x06001427 RID: 5159 RVA: 0x0005704C File Offset: 0x0005524C
		// (set) Token: 0x06001428 RID: 5160 RVA: 0x0005706C File Offset: 0x0005526C
		StyleLength IStyle.wordSpacing
		{
			get
			{
				return base.GetStyleLength(StylePropertyId.WordSpacing);
			}
			set
			{
				bool flag = this.SetStyleValue(StylePropertyId.WordSpacing, value);
				if (flag)
				{
					this.ve.IncrementVersion(VersionChangeType.Layout | VersionChangeType.StyleSheet | VersionChangeType.Styles | VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x040008D4 RID: 2260
		private static StylePropertyReader s_StylePropertyReader = new StylePropertyReader();

		// Token: 0x040008D5 RID: 2261
		private List<StyleValueManaged> m_ValuesManaged;

		// Token: 0x040008D7 RID: 2263
		private bool m_HasInlineCursor;

		// Token: 0x040008D8 RID: 2264
		private StyleCursor m_InlineCursor;

		// Token: 0x040008D9 RID: 2265
		private bool m_HasInlineTextShadow;

		// Token: 0x040008DA RID: 2266
		private StyleTextShadow m_InlineTextShadow;

		// Token: 0x040008DB RID: 2267
		private bool m_HasInlineTransformOrigin;

		// Token: 0x040008DC RID: 2268
		private StyleTransformOrigin m_InlineTransformOrigin;

		// Token: 0x040008DD RID: 2269
		private bool m_HasInlineTranslate;

		// Token: 0x040008DE RID: 2270
		private StyleTranslate m_InlineTranslateOperation;

		// Token: 0x040008DF RID: 2271
		private bool m_HasInlineRotate;

		// Token: 0x040008E0 RID: 2272
		private StyleRotate m_InlineRotateOperation;

		// Token: 0x040008E1 RID: 2273
		private bool m_HasInlineScale;

		// Token: 0x040008E2 RID: 2274
		private StyleScale m_InlineScale;

		// Token: 0x040008E3 RID: 2275
		private InlineStyleAccess.InlineRule m_InlineRule;

		// Token: 0x02000274 RID: 628
		internal struct InlineRule
		{
			// Token: 0x170004C3 RID: 1219
			// (get) Token: 0x0600142A RID: 5162 RVA: 0x000570A9 File Offset: 0x000552A9
			public StyleProperty[] properties
			{
				get
				{
					return this.rule.properties;
				}
			}

			// Token: 0x040008E4 RID: 2276
			public StyleSheet sheet;

			// Token: 0x040008E5 RID: 2277
			public StyleRule rule;

			// Token: 0x040008E6 RID: 2278
			public StylePropertyId[] propertyIds;
		}
	}
}
