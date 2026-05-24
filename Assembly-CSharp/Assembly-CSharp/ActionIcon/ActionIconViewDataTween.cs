using System;
using UnityEngine;

namespace ActionIcon
{
	// Token: 0x0200023D RID: 573
	[Serializable]
	public class ActionIconViewDataTween : TypeTween<ActionIconViewData>
	{
		// Token: 0x060014A4 RID: 5284 RVA: 0x0008E901 File Offset: 0x0008CB01
		public ActionIconViewDataTween()
		{
		}

		// Token: 0x060014A5 RID: 5285 RVA: 0x0008E909 File Offset: 0x0008CB09
		public ActionIconViewDataTween(ActionIconViewData myStartValue)
			: base(myStartValue)
		{
		}

		// Token: 0x060014A6 RID: 5286 RVA: 0x0008E912 File Offset: 0x0008CB12
		public ActionIconViewDataTween(ActionIconViewData myStartValue, ActionIconViewData myTargetValue, float myLength)
			: base(myStartValue, myTargetValue, myLength)
		{
		}

		// Token: 0x060014A7 RID: 5287 RVA: 0x0008E91D File Offset: 0x0008CB1D
		public ActionIconViewDataTween(ActionIconViewData myStartValue, ActionIconViewData myTargetValue, float myLength, AnimationCurve myLerpCurve)
			: base(myStartValue, myTargetValue, myLength, myLerpCurve)
		{
		}

		// Token: 0x060014A8 RID: 5288 RVA: 0x0008E92A File Offset: 0x0008CB2A
		protected override void SetDefaultLerpFunction()
		{
			this.lerpFunction = (ActionIconViewData start, ActionIconViewData end, float lerp) => ActionIconViewData.Lerp(start, end, lerp, base.easingCurve);
		}

		// Token: 0x060014A9 RID: 5289 RVA: 0x0008E93E File Offset: 0x0008CB3E
		protected override void SetDeltaValue(ActionIconViewData myLastValue, ActionIconViewData myCurrentValue)
		{
		}
	}
}
