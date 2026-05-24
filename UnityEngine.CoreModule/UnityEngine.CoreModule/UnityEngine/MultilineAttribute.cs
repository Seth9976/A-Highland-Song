using System;

namespace UnityEngine
{
	// Token: 0x020001DC RID: 476
	[AttributeUsage(256, Inherited = true, AllowMultiple = false)]
	public sealed class MultilineAttribute : PropertyAttribute
	{
		// Token: 0x060015D4 RID: 5588 RVA: 0x0002301C File Offset: 0x0002121C
		public MultilineAttribute()
		{
			this.lines = 3;
		}

		// Token: 0x060015D5 RID: 5589 RVA: 0x0002302D File Offset: 0x0002122D
		public MultilineAttribute(int lines)
		{
			this.lines = lines;
		}

		// Token: 0x040007B4 RID: 1972
		public readonly int lines;
	}
}
