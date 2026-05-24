using System;

namespace UnityEngine
{
	// Token: 0x020001D5 RID: 469
	[AttributeUsage(256, Inherited = true, AllowMultiple = true)]
	public class ContextMenuItemAttribute : PropertyAttribute
	{
		// Token: 0x060015CC RID: 5580 RVA: 0x00022F82 File Offset: 0x00021182
		public ContextMenuItemAttribute(string name, string function)
		{
			this.name = name;
			this.function = function;
		}

		// Token: 0x040007AB RID: 1963
		public readonly string name;

		// Token: 0x040007AC RID: 1964
		public readonly string function;
	}
}
