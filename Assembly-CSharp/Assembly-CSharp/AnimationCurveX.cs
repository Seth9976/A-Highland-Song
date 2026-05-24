using System;
using UnityEngine;

// Token: 0x020001FE RID: 510
public static class AnimationCurveX
{
	// Token: 0x060012C1 RID: 4801 RVA: 0x000861AC File Offset: 0x000843AC
	public static float GetFirstValue(this AnimationCurve curve)
	{
		if (curve.length > 0)
		{
			return curve[0].value;
		}
		return 0f;
	}

	// Token: 0x060012C2 RID: 4802 RVA: 0x000861D8 File Offset: 0x000843D8
	public static float GetLastValue(this AnimationCurve curve)
	{
		if (curve.length > 0)
		{
			return curve[curve.length - 1].value;
		}
		return 0f;
	}

	// Token: 0x060012C3 RID: 4803 RVA: 0x0008620C File Offset: 0x0008440C
	public static float GetFirstTime(this AnimationCurve curve)
	{
		if (curve.length > 0)
		{
			return curve[0].time;
		}
		return 0f;
	}

	// Token: 0x060012C4 RID: 4804 RVA: 0x00086238 File Offset: 0x00084438
	public static float GetLastTime(this AnimationCurve curve)
	{
		if (curve.length > 0)
		{
			return curve[curve.length - 1].time;
		}
		return 0f;
	}

	// Token: 0x060012C5 RID: 4805 RVA: 0x0008626A File Offset: 0x0008446A
	public static float EvaluateMirrored(this AnimationCurve curve, float time, bool sign = true)
	{
		return curve.Evaluate(Mathf.Abs(time)) * (sign ? Mathf.Sign(time) : 1f);
	}
}
