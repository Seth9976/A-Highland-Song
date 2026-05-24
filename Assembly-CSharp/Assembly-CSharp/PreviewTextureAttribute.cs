using System;
using UnityEngine;

// Token: 0x02000220 RID: 544
public class PreviewTextureAttribute : PropertyAttribute
{
	// Token: 0x0600139D RID: 5021 RVA: 0x00089AC4 File Offset: 0x00087CC4
	public PreviewTextureAttribute()
	{
	}

	// Token: 0x0600139E RID: 5022 RVA: 0x00089AE3 File Offset: 0x00087CE3
	public PreviewTextureAttribute(int size, ScaleMode scaleMode = ScaleMode.ScaleToFit)
	{
		this.width = size;
		this.height = size;
		this.scaleMode = scaleMode;
	}

	// Token: 0x0600139F RID: 5023 RVA: 0x00089B17 File Offset: 0x00087D17
	public PreviewTextureAttribute(int width, int height, ScaleMode scaleMode = ScaleMode.ScaleToFit)
	{
		this.width = width;
		this.height = height;
		this.scaleMode = scaleMode;
	}

	// Token: 0x040012D0 RID: 4816
	public ScaleMode scaleMode = ScaleMode.ScaleToFit;

	// Token: 0x040012D1 RID: 4817
	public int width = 32;

	// Token: 0x040012D2 RID: 4818
	public int height = 32;
}
