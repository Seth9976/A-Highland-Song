using System;

namespace UnityEngine
{
	// Token: 0x0200011D RID: 285
	[AttributeUsage(64)]
	public class BeforeRenderOrderAttribute : Attribute
	{
		// Token: 0x170001BD RID: 445
		// (get) Token: 0x060007CF RID: 1999 RVA: 0x0000BB43 File Offset: 0x00009D43
		// (set) Token: 0x060007D0 RID: 2000 RVA: 0x0000BB4B File Offset: 0x00009D4B
		public int order { get; private set; }

		// Token: 0x060007D1 RID: 2001 RVA: 0x0000BB54 File Offset: 0x00009D54
		public BeforeRenderOrderAttribute(int order)
		{
			this.order = order;
		}
	}
}
