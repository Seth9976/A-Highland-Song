using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x0200004F RID: 79
	public static class RectPivotExtensions
	{
		// Token: 0x06000A0D RID: 2573 RVA: 0x00023E12 File Offset: 0x00022012
		public static Rect GetRect(this RectPivot pivot, Vector2 size)
		{
			return pivot.GetRect(size.x, size.y);
		}

		// Token: 0x06000A0E RID: 2574 RVA: 0x00023E26 File Offset: 0x00022026
		public static Rect GetRect(this RectPivot pivot, float w, float h)
		{
			if (pivot != RectPivot.Corner)
			{
				return new Rect(-w / 2f, -h / 2f, w, h);
			}
			return new Rect(0f, 0f, w, h);
		}
	}
}
