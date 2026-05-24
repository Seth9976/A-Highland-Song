using System;

namespace Shapes
{
	// Token: 0x0200003B RID: 59
	public static class DashTypeExtensions
	{
		// Token: 0x060009F7 RID: 2551 RVA: 0x00023BC5 File Offset: 0x00021DC5
		public static bool HasModifier(this DashType type)
		{
			return type == DashType.Angled;
		}
	}
}
