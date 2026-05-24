using System;

namespace Shapes
{
	// Token: 0x02000054 RID: 84
	public static class ShapeFillExtensions
	{
		// Token: 0x06000A15 RID: 2581 RVA: 0x000241A1 File Offset: 0x000223A1
		internal static int GetShaderFillModeInt(this ShapeFill fill)
		{
			if (fill == null)
			{
				return -1;
			}
			return (int)fill.type;
		}
	}
}
