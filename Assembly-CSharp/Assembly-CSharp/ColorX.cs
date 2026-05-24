using System;
using UnityEngine;

// Token: 0x02000202 RID: 514
public static class ColorX
{
	// Token: 0x060012F0 RID: 4848 RVA: 0x00086FE5 File Offset: 0x000851E5
	public static Color WithAlpha(this Color color, float alpha)
	{
		color.a = alpha;
		return color;
	}
}
