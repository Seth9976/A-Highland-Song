using System;

namespace UnityEngine
{
	// Token: 0x020001D4 RID: 468
	[AttributeUsage(256, Inherited = true, AllowMultiple = false)]
	public abstract class PropertyAttribute : Attribute
	{
		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x060015C9 RID: 5577 RVA: 0x00022F71 File Offset: 0x00021171
		// (set) Token: 0x060015CA RID: 5578 RVA: 0x00022F79 File Offset: 0x00021179
		public int order { get; set; }
	}
}
