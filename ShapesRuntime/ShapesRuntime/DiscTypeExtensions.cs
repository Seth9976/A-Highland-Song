using System;

namespace Shapes
{
	// Token: 0x02000040 RID: 64
	internal static class DiscTypeExtensions
	{
		// Token: 0x060009F9 RID: 2553 RVA: 0x00023BD6 File Offset: 0x00021DD6
		public static bool HasThickness(this DiscType type)
		{
			return type == DiscType.Ring || type == DiscType.Arc;
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x00023BE2 File Offset: 0x00021DE2
		public static bool HasSector(this DiscType type)
		{
			return type == DiscType.Pie || type == DiscType.Arc;
		}
	}
}
