using System;

namespace UnityEngine
{
	// Token: 0x020001D8 RID: 472
	[AttributeUsage(256, Inherited = true, AllowMultiple = true)]
	public class SpaceAttribute : PropertyAttribute
	{
		// Token: 0x060015CF RID: 5583 RVA: 0x00022FBC File Offset: 0x000211BC
		public SpaceAttribute()
		{
			this.height = 8f;
		}

		// Token: 0x060015D0 RID: 5584 RVA: 0x00022FD1 File Offset: 0x000211D1
		public SpaceAttribute(float height)
		{
			this.height = height;
		}

		// Token: 0x040007AF RID: 1967
		public readonly float height;
	}
}
