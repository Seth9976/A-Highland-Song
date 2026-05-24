using System;

namespace UnityEngine
{
	// Token: 0x020001D6 RID: 470
	[AttributeUsage(256, Inherited = true, AllowMultiple = false)]
	public class InspectorNameAttribute : PropertyAttribute
	{
		// Token: 0x060015CD RID: 5581 RVA: 0x00022F9A File Offset: 0x0002119A
		public InspectorNameAttribute(string displayName)
		{
			this.displayName = displayName;
		}

		// Token: 0x040007AD RID: 1965
		public readonly string displayName;
	}
}
