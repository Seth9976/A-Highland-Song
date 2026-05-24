using System;

namespace UnityEngine
{
	// Token: 0x020001DA RID: 474
	[AttributeUsage(256, Inherited = true, AllowMultiple = false)]
	public sealed class RangeAttribute : PropertyAttribute
	{
		// Token: 0x060015D2 RID: 5586 RVA: 0x00022FF3 File Offset: 0x000211F3
		public RangeAttribute(float min, float max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x040007B1 RID: 1969
		public readonly float min;

		// Token: 0x040007B2 RID: 1970
		public readonly float max;
	}
}
