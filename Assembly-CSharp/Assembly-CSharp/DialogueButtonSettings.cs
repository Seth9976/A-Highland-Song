using System;
using UnityEngine;

// Token: 0x0200010D RID: 269
public class DialogueButtonSettings : ScriptableObject
{
	// Token: 0x04000ACA RID: 2762
	public DialogueButtonSettings.Theme primaryTheme;

	// Token: 0x04000ACB RID: 2763
	public DialogueButtonSettings.Theme secondaryTheme;

	// Token: 0x04000ACC RID: 2764
	public DialogueButtonSettings.Theme dangerTheme;

	// Token: 0x04000ACD RID: 2765
	public float pulseSpeed = 6f;

	// Token: 0x04000ACE RID: 2766
	public Range outlineMarginPulseRange = new Range(8f, 14f);

	// Token: 0x04000ACF RID: 2767
	public Range outlinePulseCornerRadius = new Range(8f, 14f);

	// Token: 0x0200032F RID: 815
	[Serializable]
	public class Theme
	{
		// Token: 0x0400181C RID: 6172
		public Color normalColor;

		// Token: 0x0400181D RID: 6173
		public Color highlightedColor1;

		// Token: 0x0400181E RID: 6174
		public Color highlightedColor2;

		// Token: 0x0400181F RID: 6175
		public Color outlineColor;

		// Token: 0x04001820 RID: 6176
		public Color pressedColor;
	}
}
