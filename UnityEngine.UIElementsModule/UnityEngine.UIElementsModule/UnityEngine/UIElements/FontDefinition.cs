using System;
using UnityEngine.TextCore.Text;

namespace UnityEngine.UIElements
{
	// Token: 0x02000271 RID: 625
	public struct FontDefinition : IEquatable<FontDefinition>
	{
		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x0600134D RID: 4941 RVA: 0x00052DCC File Offset: 0x00050FCC
		// (set) Token: 0x0600134E RID: 4942 RVA: 0x00052DE4 File Offset: 0x00050FE4
		public Font font
		{
			get
			{
				return this.m_Font;
			}
			set
			{
				bool flag = value != null && this.fontAsset != null;
				if (flag)
				{
					throw new InvalidOperationException("Cannot set both Font and FontAsset on FontDefinition");
				}
				this.m_Font = value;
			}
		}

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x0600134F RID: 4943 RVA: 0x00052E20 File Offset: 0x00051020
		// (set) Token: 0x06001350 RID: 4944 RVA: 0x00052E38 File Offset: 0x00051038
		public FontAsset fontAsset
		{
			get
			{
				return this.m_FontAsset;
			}
			set
			{
				bool flag = value != null && this.font != null;
				if (flag)
				{
					throw new InvalidOperationException("Cannot set both Font and FontAsset on FontDefinition");
				}
				this.m_FontAsset = value;
			}
		}

		// Token: 0x06001351 RID: 4945 RVA: 0x00052E74 File Offset: 0x00051074
		public static FontDefinition FromFont(Font f)
		{
			return new FontDefinition
			{
				m_Font = f
			};
		}

		// Token: 0x06001352 RID: 4946 RVA: 0x00052E98 File Offset: 0x00051098
		public static FontDefinition FromSDFFont(FontAsset f)
		{
			return new FontDefinition
			{
				m_FontAsset = f
			};
		}

		// Token: 0x06001353 RID: 4947 RVA: 0x00052EBC File Offset: 0x000510BC
		internal static FontDefinition FromObject(object obj)
		{
			Font font = obj as Font;
			bool flag = font != null;
			FontDefinition fontDefinition;
			if (flag)
			{
				fontDefinition = FontDefinition.FromFont(font);
			}
			else
			{
				FontAsset fontAsset = obj as FontAsset;
				bool flag2 = fontAsset != null;
				if (flag2)
				{
					fontDefinition = FontDefinition.FromSDFFont(fontAsset);
				}
				else
				{
					fontDefinition = default(FontDefinition);
				}
			}
			return fontDefinition;
		}

		// Token: 0x06001354 RID: 4948 RVA: 0x00052F10 File Offset: 0x00051110
		internal bool IsEmpty()
		{
			return this.m_Font == null && this.m_FontAsset == null;
		}

		// Token: 0x06001355 RID: 4949 RVA: 0x00052F40 File Offset: 0x00051140
		public override string ToString()
		{
			bool flag = this.font != null;
			string text;
			if (flag)
			{
				text = string.Format("{0}", this.font);
			}
			else
			{
				text = string.Format("{0}", this.fontAsset);
			}
			return text;
		}

		// Token: 0x06001356 RID: 4950 RVA: 0x00052F88 File Offset: 0x00051188
		public bool Equals(FontDefinition other)
		{
			return object.Equals(this.m_Font, other.m_Font) && object.Equals(this.m_FontAsset, other.m_FontAsset);
		}

		// Token: 0x06001357 RID: 4951 RVA: 0x00052FC4 File Offset: 0x000511C4
		public override bool Equals(object obj)
		{
			bool flag;
			if (obj is FontDefinition)
			{
				FontDefinition fontDefinition = (FontDefinition)obj;
				flag = this.Equals(fontDefinition);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001358 RID: 4952 RVA: 0x00052FF0 File Offset: 0x000511F0
		public override int GetHashCode()
		{
			return (((this.m_Font != null) ? this.m_Font.GetHashCode() : 0) * 397) ^ ((this.m_FontAsset != null) ? this.m_FontAsset.GetHashCode() : 0);
		}

		// Token: 0x06001359 RID: 4953 RVA: 0x00053044 File Offset: 0x00051244
		public static bool operator ==(FontDefinition left, FontDefinition right)
		{
			return left.Equals(right);
		}

		// Token: 0x0600135A RID: 4954 RVA: 0x00053060 File Offset: 0x00051260
		public static bool operator !=(FontDefinition left, FontDefinition right)
		{
			return !left.Equals(right);
		}

		// Token: 0x040008D1 RID: 2257
		private Font m_Font;

		// Token: 0x040008D2 RID: 2258
		private FontAsset m_FontAsset;
	}
}
