using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020002C2 RID: 706
	public class UxmlTemplateFactory : UxmlFactory<VisualElement, UxmlTemplateTraits>
	{
		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x060017AB RID: 6059 RVA: 0x00060138 File Offset: 0x0005E338
		public override string uxmlName
		{
			get
			{
				return "Template";
			}
		}

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x060017AC RID: 6060 RVA: 0x0006013F File Offset: 0x0005E33F
		public override string uxmlQualifiedName
		{
			get
			{
				return this.uxmlNamespace + "." + this.uxmlName;
			}
		}

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x060017AD RID: 6061 RVA: 0x0005FFC3 File Offset: 0x0005E1C3
		public override string substituteForTypeName
		{
			get
			{
				return typeof(VisualElement).Name;
			}
		}

		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x060017AE RID: 6062 RVA: 0x0005FFD4 File Offset: 0x0005E1D4
		public override string substituteForTypeNamespace
		{
			get
			{
				return typeof(VisualElement).Namespace ?? string.Empty;
			}
		}

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x060017AF RID: 6063 RVA: 0x0005FFEE File Offset: 0x0005E1EE
		public override string substituteForTypeQualifiedName
		{
			get
			{
				return typeof(VisualElement).FullName;
			}
		}

		// Token: 0x060017B0 RID: 6064 RVA: 0x00060158 File Offset: 0x0005E358
		public override VisualElement Create(IUxmlAttributes bag, CreationContext cc)
		{
			return null;
		}

		// Token: 0x04000A15 RID: 2581
		internal const string k_ElementName = "Template";
	}
}
