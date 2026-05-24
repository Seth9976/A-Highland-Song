using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020002BC RID: 700
	public class UxmlRootElementFactory : UxmlFactory<VisualElement, UxmlRootElementTraits>
	{
		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x06001789 RID: 6025 RVA: 0x0005FE3D File Offset: 0x0005E03D
		public override string uxmlName
		{
			get
			{
				return "UXML";
			}
		}

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x0600178A RID: 6026 RVA: 0x0005FE44 File Offset: 0x0005E044
		public override string uxmlQualifiedName
		{
			get
			{
				return this.uxmlNamespace + "." + this.uxmlName;
			}
		}

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x0600178B RID: 6027 RVA: 0x0005FE5C File Offset: 0x0005E05C
		public override string substituteForTypeName
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x0600178C RID: 6028 RVA: 0x0005FE5C File Offset: 0x0005E05C
		public override string substituteForTypeNamespace
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x0600178D RID: 6029 RVA: 0x0005FE5C File Offset: 0x0005E05C
		public override string substituteForTypeQualifiedName
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x0600178E RID: 6030 RVA: 0x0005FE64 File Offset: 0x0005E064
		public override VisualElement Create(IUxmlAttributes bag, CreationContext cc)
		{
			return null;
		}

		// Token: 0x04000A06 RID: 2566
		internal const string k_ElementName = "UXML";
	}
}
