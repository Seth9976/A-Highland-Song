using System;
using UnityEngine;

// Token: 0x020000F5 RID: 245
public static class Parabola
{
	// Token: 0x060007E3 RID: 2019 RVA: 0x0004617A File Offset: 0x0004437A
	public static float CalcJumpSpeedYFromHeight(float jumpHeight, float stepDiff, float duration)
	{
		return 2f * (jumpHeight + Mathf.Sqrt(jumpHeight * (jumpHeight - stepDiff))) / duration;
	}

	// Token: 0x060007E4 RID: 2020 RVA: 0x00046190 File Offset: 0x00044390
	public static float CalcGravity(float initialSpeedY, float apexHeight)
	{
		return -(initialSpeedY * initialSpeedY) / (2f * apexHeight);
	}

	// Token: 0x060007E5 RID: 2021 RVA: 0x0004619E File Offset: 0x0004439E
	public static float CalcJumpSpeedYFromGravity(float gravity, float stepDiff, float duration)
	{
		return (-(0.5f * gravity * duration * duration) + stepDiff) / duration;
	}

	// Token: 0x060007E6 RID: 2022 RVA: 0x000461B0 File Offset: 0x000443B0
	public static float CalcJumpHeightFromInitialSpeed(float initialSpeedY, float gravity)
	{
		return initialSpeedY * initialSpeedY / (2f * -gravity);
	}

	// Token: 0x060007E7 RID: 2023 RVA: 0x000461C0 File Offset: 0x000443C0
	public static float CalcJumpDurationFromDropHeight(float dropHeight, float initialYSpeed, float gravity)
	{
		if (dropHeight < 0f)
		{
			Debug.LogError("Expected dropHeight to be positive (aka start.y - target.y)");
			return 0.2f;
		}
		float num = -dropHeight;
		float num2 = Mathf.Sqrt(initialYSpeed * initialYSpeed - 4f * (0.5f * gravity * -num));
		float num3 = (-initialYSpeed + num2) / gravity;
		if (num3 < 0f)
		{
			num3 = (-initialYSpeed - num2) / gravity;
		}
		return num3;
	}
}
