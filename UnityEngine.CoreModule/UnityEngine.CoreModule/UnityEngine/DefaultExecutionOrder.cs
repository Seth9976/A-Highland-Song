using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x020001F7 RID: 503
	[UsedByNativeCode]
	[AttributeUsage(4)]
	public class DefaultExecutionOrder : Attribute
	{
		// Token: 0x06001662 RID: 5730 RVA: 0x00023DC9 File Offset: 0x00021FC9
		public DefaultExecutionOrder(int order)
		{
			this.m_Order = order;
		}

		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x06001663 RID: 5731 RVA: 0x00023DDC File Offset: 0x00021FDC
		public int order
		{
			get
			{
				return this.m_Order;
			}
		}

		// Token: 0x040007D9 RID: 2009
		private int m_Order;
	}
}
