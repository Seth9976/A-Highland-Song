using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x020002E7 RID: 743
	public class UxmlFactory<TCreatedType, TTraits> : IUxmlFactory where TCreatedType : VisualElement, new() where TTraits : UxmlTraits, new()
	{
		// Token: 0x06001876 RID: 6262 RVA: 0x00061AA8 File Offset: 0x0005FCA8
		protected UxmlFactory()
		{
			this.m_Traits = new TTraits();
		}

		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x06001877 RID: 6263 RVA: 0x00061AC0 File Offset: 0x0005FCC0
		public virtual string uxmlName
		{
			get
			{
				return typeof(TCreatedType).Name;
			}
		}

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x06001878 RID: 6264 RVA: 0x00061AE4 File Offset: 0x0005FCE4
		public virtual string uxmlNamespace
		{
			get
			{
				return typeof(TCreatedType).Namespace ?? string.Empty;
			}
		}

		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x06001879 RID: 6265 RVA: 0x00061B10 File Offset: 0x0005FD10
		public virtual string uxmlQualifiedName
		{
			get
			{
				return typeof(TCreatedType).FullName;
			}
		}

		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x0600187A RID: 6266 RVA: 0x00061B34 File Offset: 0x0005FD34
		public bool canHaveAnyAttribute
		{
			get
			{
				return this.m_Traits.canHaveAnyAttribute;
			}
		}

		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x0600187B RID: 6267 RVA: 0x00061B58 File Offset: 0x0005FD58
		public virtual IEnumerable<UxmlAttributeDescription> uxmlAttributesDescription
		{
			get
			{
				return this.m_Traits.uxmlAttributesDescription;
			}
		}

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x0600187C RID: 6268 RVA: 0x00061B7C File Offset: 0x0005FD7C
		public virtual IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
		{
			get
			{
				return this.m_Traits.uxmlChildElementsDescription;
			}
		}

		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x0600187D RID: 6269 RVA: 0x00061BA0 File Offset: 0x0005FDA0
		public virtual string substituteForTypeName
		{
			get
			{
				bool flag = typeof(TCreatedType) == typeof(VisualElement);
				string text;
				if (flag)
				{
					text = string.Empty;
				}
				else
				{
					text = typeof(VisualElement).Name;
				}
				return text;
			}
		}

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x0600187E RID: 6270 RVA: 0x00061BE4 File Offset: 0x0005FDE4
		public virtual string substituteForTypeNamespace
		{
			get
			{
				bool flag = typeof(TCreatedType) == typeof(VisualElement);
				string text;
				if (flag)
				{
					text = string.Empty;
				}
				else
				{
					text = typeof(VisualElement).Namespace ?? string.Empty;
				}
				return text;
			}
		}

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x0600187F RID: 6271 RVA: 0x00061C34 File Offset: 0x0005FE34
		public virtual string substituteForTypeQualifiedName
		{
			get
			{
				bool flag = typeof(TCreatedType) == typeof(VisualElement);
				string text;
				if (flag)
				{
					text = string.Empty;
				}
				else
				{
					text = typeof(VisualElement).FullName;
				}
				return text;
			}
		}

		// Token: 0x06001880 RID: 6272 RVA: 0x00061C7C File Offset: 0x0005FE7C
		public virtual bool AcceptsAttributeBag(IUxmlAttributes bag, CreationContext cc)
		{
			return true;
		}

		// Token: 0x06001881 RID: 6273 RVA: 0x00061C90 File Offset: 0x0005FE90
		public virtual VisualElement Create(IUxmlAttributes bag, CreationContext cc)
		{
			TCreatedType tcreatedType = new TCreatedType();
			this.m_Traits.Init(tcreatedType, bag, cc);
			return tcreatedType;
		}

		// Token: 0x04000A6C RID: 2668
		internal TTraits m_Traits;
	}
}
