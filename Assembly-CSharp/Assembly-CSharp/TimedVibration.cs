using System;

// Token: 0x02000058 RID: 88
[Serializable]
public class TimedVibration : BaseTimedVibration
{
	// Token: 0x170000BA RID: 186
	// (get) Token: 0x0600026B RID: 619 RVA: 0x0001508E File Offset: 0x0001328E
	public override float maxTime
	{
		get
		{
			return this.length;
		}
	}

	// Token: 0x0600026C RID: 620 RVA: 0x00015096 File Offset: 0x00013296
	public void X()
	{
	}

	// Token: 0x0600026D RID: 621 RVA: 0x00015098 File Offset: 0x00013298
	public override VibrationMoment Evaluate(float time)
	{
		VibrationMoment zero = VibrationMoment.zero;
		zero.leftStrength = this.leftStrength;
		zero.rightStrength = this.rightStrength;
		return zero;
	}

	// Token: 0x0600026E RID: 622 RVA: 0x000150C8 File Offset: 0x000132C8
	public static TimedVibration VibrateForSeconds(float strength, float time)
	{
		return new TimedVibration
		{
			rightStrength = strength,
			leftStrength = strength,
			length = time
		};
	}

	// Token: 0x04000396 RID: 918
	public float length;

	// Token: 0x04000397 RID: 919
	public float largeMotorStrength;

	// Token: 0x04000398 RID: 920
	public float smallMotorStrength;

	// Token: 0x04000399 RID: 921
	public float leftStrength;

	// Token: 0x0400039A RID: 922
	public float rightStrength;
}
