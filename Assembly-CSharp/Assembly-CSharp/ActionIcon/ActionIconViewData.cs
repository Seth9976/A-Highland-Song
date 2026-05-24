using System;
using UnityEngine;

namespace ActionIcon
{
	// Token: 0x0200023C RID: 572
	[Serializable]
	public struct ActionIconViewData
	{
		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x0600149E RID: 5278 RVA: 0x0008E74C File Offset: 0x0008C94C
		public static ActionIconViewData blank
		{
			get
			{
				return new ActionIconViewData(ActionIconBackground.blank, null, "", Color.clear, Color.clear, 1f);
			}
		}

		// Token: 0x0600149F RID: 5279 RVA: 0x0008E770 File Offset: 0x0008C970
		public ActionIconViewData(ActionIconViewData data)
		{
			this.background = data.background;
			this.icon = data.icon;
			this.iconText = data.iconText;
			this.iconColor = data.iconColor;
			this.highlightColor = data.highlightColor;
			this.scale = data.scale;
		}

		// Token: 0x060014A0 RID: 5280 RVA: 0x0008E7C5 File Offset: 0x0008C9C5
		public ActionIconViewData(ActionIconBackground background, Sprite icon, string iconText, Color iconColor, Color highlightColor, float scale)
		{
			this.background = background;
			this.icon = icon;
			this.iconText = iconText;
			this.iconColor = iconColor;
			this.highlightColor = highlightColor;
			this.scale = scale;
		}

		// Token: 0x060014A1 RID: 5281 RVA: 0x0008E7F4 File Offset: 0x0008C9F4
		public static ActionIconViewData Lerp(ActionIconViewData start, ActionIconViewData end, float lerp)
		{
			if (lerp <= 0f)
			{
				return new ActionIconViewData(start);
			}
			if (lerp >= 1f)
			{
				return new ActionIconViewData(end);
			}
			return ActionIconViewData.Lerp(start, end, lerp, ActionIconViewData._linear);
		}

		// Token: 0x060014A2 RID: 5282 RVA: 0x0008E824 File Offset: 0x0008CA24
		public static ActionIconViewData Lerp(ActionIconViewData start, ActionIconViewData end, float lerp, AnimationCurve easingCurve)
		{
			ActionIconViewData actionIconViewData = default(ActionIconViewData);
			float num = easingCurve.Evaluate(lerp);
			actionIconViewData.background = ActionIconBackground.Lerp(start.background, end.background, lerp, easingCurve);
			actionIconViewData.icon = ((num <= 0f) ? start.icon : end.icon);
			actionIconViewData.iconText = ((num <= 0f) ? start.iconText : end.iconText);
			actionIconViewData.iconColor = Color.Lerp(start.iconColor, end.iconColor, num);
			actionIconViewData.highlightColor = Color.Lerp(start.highlightColor, end.highlightColor, num);
			actionIconViewData.scale = Mathf.Lerp(start.scale, end.scale, num);
			return actionIconViewData;
		}

		// Token: 0x04001395 RID: 5013
		public ActionIconBackground background;

		// Token: 0x04001396 RID: 5014
		public Sprite icon;

		// Token: 0x04001397 RID: 5015
		public string iconText;

		// Token: 0x04001398 RID: 5016
		public Color iconColor;

		// Token: 0x04001399 RID: 5017
		public Color highlightColor;

		// Token: 0x0400139A RID: 5018
		public float scale;

		// Token: 0x0400139B RID: 5019
		private static AnimationCurve _linear = AnimationCurve.Linear(0f, 0f, 1f, 1f);
	}
}
