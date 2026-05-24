using System;
using InControl;
using UnityEngine;

// Token: 0x02000059 RID: 89
public class TimedVibrationCurve : BaseTimedVibration
{
	// Token: 0x170000BB RID: 187
	// (get) Token: 0x06000270 RID: 624 RVA: 0x000150F9 File Offset: 0x000132F9
	public override float maxTime
	{
		get
		{
			return Mathf.Max(this.leftStrengthOverTime.GetLastTime(), this.rightStrengthOverTime.GetLastTime());
		}
	}

	// Token: 0x06000271 RID: 625 RVA: 0x00015118 File Offset: 0x00013318
	public override VibrationMoment Evaluate(float time)
	{
		return new VibrationMoment
		{
			leftStrength = this.leftStrengthOverTime.Evaluate(time),
			rightStrength = this.rightStrengthOverTime.Evaluate(time)
		};
	}

	// Token: 0x06000272 RID: 626 RVA: 0x00015154 File Offset: 0x00013354
	public static TimedVibrationCurve VibrateForSeconds(AnimationCurve strengthOverTime)
	{
		return new TimedVibrationCurve
		{
			rightStrengthOverTime = strengthOverTime,
			leftStrengthOverTime = strengthOverTime
		};
	}

	// Token: 0x0400039B RID: 923
	public InputDevice inputDevice;

	// Token: 0x0400039C RID: 924
	public AnimationCurve leftStrengthOverTime;

	// Token: 0x0400039D RID: 925
	public AnimationCurve rightStrengthOverTime;
}
