using System;

// Token: 0x02000054 RID: 84
public abstract class BaseTimedVibration
{
	// Token: 0x170000B8 RID: 184
	// (get) Token: 0x0600024F RID: 591
	public abstract float maxTime { get; }

	// Token: 0x06000250 RID: 592
	public abstract VibrationMoment Evaluate(float time);
}
