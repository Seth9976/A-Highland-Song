using System;
using UnityEngine;

// Token: 0x02000102 RID: 258
public class ButtonPromptSettings : ScriptableObject
{
	// Token: 0x17000230 RID: 560
	// (get) Token: 0x060008A1 RID: 2209 RVA: 0x0004956A File Offset: 0x0004776A
	public float outerHeight
	{
		get
		{
			return this.height + 2f * this.verticalMargin;
		}
	}

	// Token: 0x04000A41 RID: 2625
	public float height = 57f;

	// Token: 0x04000A42 RID: 2626
	public float heightSwitch = 57f;

	// Token: 0x04000A43 RID: 2627
	public float padding = 10f;

	// Token: 0x04000A44 RID: 2628
	public float horizontalMargin = 6f;

	// Token: 0x04000A45 RID: 2629
	public float verticalMargin = 6f;

	// Token: 0x04000A46 RID: 2630
	public Color primaryColor = new Color(0f, 0.77254903f, 0.67058825f);

	// Token: 0x04000A47 RID: 2631
	public Color secondaryColor = new Color(0.2901961f, 0.2901961f, 0.2901961f);

	// Token: 0x04000A48 RID: 2632
	public Color secondaryColorSwitch = new Color(0.2901961f, 0.2901961f, 0.2901961f);

	// Token: 0x04000A49 RID: 2633
	public Color outlineColor = Color.gray;

	// Token: 0x04000A4A RID: 2634
	public Color secondaryLabelColor = Color.white;

	// Token: 0x04000A4B RID: 2635
	public Color highlightColor = Color.gray;

	// Token: 0x04000A4C RID: 2636
	public Color pressedColor = Color.gray;

	// Token: 0x04000A4D RID: 2637
	public Sprite backgroundSprite;

	// Token: 0x04000A4E RID: 2638
	public Sprite outlineBackgroundSprite;

	// Token: 0x04000A4F RID: 2639
	public Sprite glowSprite;

	// Token: 0x04000A50 RID: 2640
	public float glowOpacity = 0.5f;
}
