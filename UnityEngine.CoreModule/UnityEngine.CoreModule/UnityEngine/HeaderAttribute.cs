using System;

namespace UnityEngine
{
	// Token: 0x020001D9 RID: 473
	[AttributeUsage(256, Inherited = true, AllowMultiple = true)]
	public class HeaderAttribute : PropertyAttribute
	{
		// Token: 0x060015D1 RID: 5585 RVA: 0x00022FE2 File Offset: 0x000211E2
		public HeaderAttribute(string header)
		{
			this.header = header;
		}

		// Token: 0x040007B0 RID: 1968
		public readonly string header;
	}
}
