using System;
using UnityEngine;

// Token: 0x02000146 RID: 326
public class SettingControlSettings : ScriptableObject
{
	// Token: 0x04000D1D RID: 3357
	public Color labelColor = Color.gray;

	// Token: 0x04000D1E RID: 3358
	public Color buttonLabelNormalColor = Color.white;

	// Token: 0x04000D1F RID: 3359
	public Color buttonLabelHighlightColor = Color.white.WithAlpha(0.6f);

	// Token: 0x04000D20 RID: 3360
	public Color highlightColor = Color.white;

	// Token: 0x04000D21 RID: 3361
	public Color buttonBackgroundColor = Color.gray;

	// Token: 0x04000D22 RID: 3362
	public int maxLabelCharactersPerLine = 16;

	// Token: 0x04000D23 RID: 3363
	public float sliderSmoothTime = 0.3f;
}
