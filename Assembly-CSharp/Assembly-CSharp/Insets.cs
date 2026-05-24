using System;
using UnityEngine;

// Token: 0x02000215 RID: 533
[Serializable]
public struct Insets
{
	// Token: 0x0600137F RID: 4991 RVA: 0x000891EC File Offset: 0x000873EC
	public Rect ApplyToRect(Rect rect, bool originTopLeft = false)
	{
		if (originTopLeft)
		{
			return new Rect(rect.x + this.left, rect.y + this.top, rect.width - (this.left + this.right), rect.height - (this.top + this.bottom));
		}
		return new Rect(rect.x + this.left, rect.y + this.bottom, rect.width - (this.left + this.right), rect.height - (this.top + this.bottom));
	}

	// Token: 0x06001380 RID: 4992 RVA: 0x00089294 File Offset: 0x00087494
	public Insets Expand(float marginX, float marginY)
	{
		return new Insets
		{
			top = this.top + marginY,
			bottom = this.bottom + marginY,
			left = this.left + marginX,
			right = this.right + marginX
		};
	}

	// Token: 0x040012C0 RID: 4800
	public float top;

	// Token: 0x040012C1 RID: 4801
	public float bottom;

	// Token: 0x040012C2 RID: 4802
	public float left;

	// Token: 0x040012C3 RID: 4803
	public float right;
}
