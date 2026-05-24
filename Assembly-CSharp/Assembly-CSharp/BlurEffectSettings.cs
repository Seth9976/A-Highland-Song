using System;
using UnityEngine;

// Token: 0x020000AC RID: 172
public class BlurEffectSettings : ScriptableObject
{
	// Token: 0x17000165 RID: 357
	// (get) Token: 0x06000582 RID: 1410 RVA: 0x0002BD7D File Offset: 0x00029F7D
	public int renderWidth
	{
		get
		{
			return this.renderWidthDesktop;
		}
	}

	// Token: 0x04000654 RID: 1620
	public int renderWidthDesktop = 1280;

	// Token: 0x04000655 RID: 1621
	[Info("Decreasing this has a tangible impact on GPU performance. Recommend setting as low as you can get away with!")]
	public int renderWidthSwitch = 720;

	// Token: 0x04000656 RID: 1622
	[Range(0f, 0.25f)]
	public float maxBlurSize = 0.02f;
}
