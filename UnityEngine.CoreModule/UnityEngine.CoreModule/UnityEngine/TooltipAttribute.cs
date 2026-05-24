using System;

namespace UnityEngine
{
	// Token: 0x020001D7 RID: 471
	[AttributeUsage(32767, Inherited = true, AllowMultiple = false)]
	public class TooltipAttribute : PropertyAttribute
	{
		// Token: 0x060015CE RID: 5582 RVA: 0x00022FAB File Offset: 0x000211AB
		public TooltipAttribute(string tooltip)
		{
			this.tooltip = tooltip;
		}

		// Token: 0x040007AE RID: 1966
		public readonly string tooltip;
	}
}
