using System;
using UnityEngine;

// Token: 0x02000210 RID: 528
public static class TimeX
{
	// Token: 0x0600136C RID: 4972 RVA: 0x00088CD2 File Offset: 0x00086ED2
	public static float Damping(float damping)
	{
		return TimeX.Damping(damping, Time.deltaTime);
	}

	// Token: 0x0600136D RID: 4973 RVA: 0x00088CE0 File Offset: 0x00086EE0
	public static float Damping(float damping, float deltaTime)
	{
		float num = 0.033333335f;
		float num2 = deltaTime / num;
		return Mathf.Pow(damping, num2);
	}

	// Token: 0x0600136E RID: 4974 RVA: 0x00088CFE File Offset: 0x00086EFE
	public static float Lerping(float lerping)
	{
		return TimeX.Lerping(lerping, Time.deltaTime);
	}

	// Token: 0x0600136F RID: 4975 RVA: 0x00088D0B File Offset: 0x00086F0B
	public static float Lerping(float lerping, float deltaTime)
	{
		return 1f - TimeX.Damping(1f - lerping, deltaTime);
	}

	// Token: 0x040012B0 RID: 4784
	public const float kDampingLerpingExpectedFramerate = 30f;

	// Token: 0x040012B1 RID: 4785
	public const float targetFrameRateDeltaTime = 0.033333335f;
}
