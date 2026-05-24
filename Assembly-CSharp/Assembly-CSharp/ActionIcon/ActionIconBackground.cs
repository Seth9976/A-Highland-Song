using System;
using UnityEngine;

namespace ActionIcon
{
	// Token: 0x02000235 RID: 565
	[Serializable]
	public struct ActionIconBackground
	{
		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x0600144D RID: 5197 RVA: 0x0008CB79 File Offset: 0x0008AD79
		public static ActionIconBackground blank
		{
			get
			{
				return new ActionIconBackground(null, null, Vector2.zero, Vector2.one);
			}
		}

		// Token: 0x0600144E RID: 5198 RVA: 0x0008CB8C File Offset: 0x0008AD8C
		public ActionIconBackground(Sprite backgroundSprite, Sprite highlightSprite, Vector2 textAnchorMin, Vector2 textAnchorMax)
		{
			this.size = Vector2.zero;
			this.backgroundSprite = backgroundSprite;
			this.highlightSprite = highlightSprite;
			this.textAnchorMin = textAnchorMin;
			this.textAnchorMax = textAnchorMax;
		}

		// Token: 0x0600144F RID: 5199 RVA: 0x0008CBB8 File Offset: 0x0008ADB8
		public static ActionIconBackground Lerp(ActionIconBackground start, ActionIconBackground end, float lerp, AnimationCurve easingCurve)
		{
			ActionIconBackground actionIconBackground = default(ActionIconBackground);
			float num = easingCurve.Evaluate(lerp);
			actionIconBackground.backgroundSprite = ((num <= 0f) ? start.backgroundSprite : end.backgroundSprite);
			actionIconBackground.highlightSprite = ((num <= 0f) ? start.highlightSprite : end.highlightSprite);
			actionIconBackground.textAnchorMin = Vector2.Lerp(start.textAnchorMin, end.textAnchorMin, num);
			actionIconBackground.textAnchorMax = Vector2.Lerp(start.textAnchorMax, end.textAnchorMax, num);
			return actionIconBackground;
		}

		// Token: 0x04001348 RID: 4936
		public Vector2 size;

		// Token: 0x04001349 RID: 4937
		public Sprite backgroundSprite;

		// Token: 0x0400134A RID: 4938
		public Sprite highlightSprite;

		// Token: 0x0400134B RID: 4939
		public Vector2 textAnchorMin;

		// Token: 0x0400134C RID: 4940
		public Vector2 textAnchorMax;
	}
}
