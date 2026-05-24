using System;
using UnityEngine;

// Token: 0x02000106 RID: 262
public class ChoicesWidgetSettings : ScriptableObject
{
	// Token: 0x060008CB RID: 2251 RVA: 0x0004A2AC File Offset: 0x000484AC
	public Sprite SpriteForIcon(ChoiceIcon icon)
	{
		foreach (ChoicesWidgetSettings.IconSprite iconSprite in this.icons)
		{
			if (iconSprite.icon == icon)
			{
				return iconSprite.sprite;
			}
		}
		return null;
	}

	// Token: 0x04000A79 RID: 2681
	public float firstChoiceY = 20f;

	// Token: 0x04000A7A RID: 2682
	public float heightPerItemExpanded = 50f;

	// Token: 0x04000A7B RID: 2683
	public float marginToRunOff = 10f;

	// Token: 0x04000A7C RID: 2684
	public float runOffTutorialHeight = 100f;

	// Token: 0x04000A7D RID: 2685
	public float arrowOffsetY;

	// Token: 0x04000A7E RID: 2686
	public float unhighlightedAlpha = 0.7f;

	// Token: 0x04000A7F RID: 2687
	public float delayPerItem = 0.2f;

	// Token: 0x04000A80 RID: 2688
	public float runOffUnhighlightedAlpha = 0.5f;

	// Token: 0x04000A81 RID: 2689
	public float actionIconMaxAlpha = 0.7f;

	// Token: 0x04000A82 RID: 2690
	public float tutorialHighlightPulseSpeed = 1f;

	// Token: 0x04000A83 RID: 2691
	public float tutorialHighlightPulseAmount = 0.5f;

	// Token: 0x04000A84 RID: 2692
	public float attractInitialPulseTime = 1f;

	// Token: 0x04000A85 RID: 2693
	public float attractHideTime = 1f;

	// Token: 0x04000A86 RID: 2694
	public float attractDotPulseSpeed = 5f;

	// Token: 0x04000A87 RID: 2695
	public Range attractDotPulseRange = new Range(0.8f, 1.2f);

	// Token: 0x04000A88 RID: 2696
	public AnimationCurve attractDotPopOpenCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	// Token: 0x04000A89 RID: 2697
	public ChoicesWidgetSettings.IconSprite[] icons;

	// Token: 0x02000329 RID: 809
	[Serializable]
	public struct IconSprite
	{
		// Token: 0x0400180C RID: 6156
		public ChoiceIcon icon;

		// Token: 0x0400180D RID: 6157
		public Sprite sprite;
	}
}
