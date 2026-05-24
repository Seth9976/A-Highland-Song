using System;
using UnityEngine;

// Token: 0x020001BE RID: 446
[Serializable]
public class TweenProperties<T>
{
	// Token: 0x06000EAF RID: 3759 RVA: 0x00072D48 File Offset: 0x00070F48
	public TweenProperties(T startValue, float tweenTime)
	{
		this.startValue = startValue;
		this.tweenTime = tweenTime;
	}

	// Token: 0x06000EB0 RID: 3760 RVA: 0x00072D5E File Offset: 0x00070F5E
	public TweenProperties(T startValue, T targetValue, float tweenTime)
	{
		this.startValue = startValue;
		this.targetValue = targetValue;
		this.tweenTime = tweenTime;
	}

	// Token: 0x06000EB1 RID: 3761 RVA: 0x00072D7B File Offset: 0x00070F7B
	public TweenProperties(T startValue, T targetValue, float tweenTime, AnimationCurve easingCurve)
	{
		this.startValue = startValue;
		this.targetValue = targetValue;
		this.tweenTime = tweenTime;
		this.easingCurve = easingCurve;
	}

	// Token: 0x0400118B RID: 4491
	public bool setStartValue;

	// Token: 0x0400118C RID: 4492
	public bool setEasingCurve;

	// Token: 0x0400118D RID: 4493
	public T startValue;

	// Token: 0x0400118E RID: 4494
	public T targetValue;

	// Token: 0x0400118F RID: 4495
	public float tweenTime;

	// Token: 0x04001190 RID: 4496
	public AnimationCurve easingCurve;

	// Token: 0x04001191 RID: 4497
	public TypeTween<T>.LerpFunction lerpFunction;
}
