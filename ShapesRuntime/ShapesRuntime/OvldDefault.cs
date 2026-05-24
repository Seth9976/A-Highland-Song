using System;

namespace Shapes
{
	// Token: 0x02000015 RID: 21
	[AttributeUsage(AttributeTargets.Parameter)]
	internal class OvldDefault : Attribute
	{
		// Token: 0x06000919 RID: 2329 RVA: 0x00020F8A File Offset: 0x0001F18A
		public OvldDefault(string @default)
		{
			this.@default = @default;
		}

		// Token: 0x0400008B RID: 139
		public string @default;
	}
}
