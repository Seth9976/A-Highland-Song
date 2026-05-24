using System;

namespace UnityEngine
{
	// Token: 0x020001DB RID: 475
	[AttributeUsage(256, Inherited = true, AllowMultiple = false)]
	public sealed class MinAttribute : PropertyAttribute
	{
		// Token: 0x060015D3 RID: 5587 RVA: 0x0002300B File Offset: 0x0002120B
		public MinAttribute(float min)
		{
			this.min = min;
		}

		// Token: 0x040007B3 RID: 1971
		public readonly float min;
	}
}
