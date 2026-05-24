using System;
using UnityEngine;

// Token: 0x020001E0 RID: 480
public class EasingFunction
{
	// Token: 0x060010A8 RID: 4264 RVA: 0x0007B51F File Offset: 0x0007971F
	public static float Linear(float start, float end, float value)
	{
		return Mathf.Lerp(start, end, value);
	}

	// Token: 0x060010A9 RID: 4265 RVA: 0x0007B52C File Offset: 0x0007972C
	public static float Spring(float start, float end, float value)
	{
		value = Mathf.Clamp01(value);
		value = (Mathf.Sin(value * 3.1415927f * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + 1.2f * (1f - value));
		return start + (end - start) * value;
	}

	// Token: 0x060010AA RID: 4266 RVA: 0x0007B590 File Offset: 0x00079790
	public static float EaseInQuad(float start, float end, float value)
	{
		end -= start;
		return end * value * value + start;
	}

	// Token: 0x060010AB RID: 4267 RVA: 0x0007B59E File Offset: 0x0007979E
	public static float EaseOutQuad(float start, float end, float value)
	{
		end -= start;
		return -end * value * (value - 2f) + start;
	}

	// Token: 0x060010AC RID: 4268 RVA: 0x0007B5B4 File Offset: 0x000797B4
	public static float EaseInOutQuad(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end * 0.5f * value * value + start;
		}
		value -= 1f;
		return -end * 0.5f * (value * (value - 2f) - 1f) + start;
	}

	// Token: 0x060010AD RID: 4269 RVA: 0x0007B608 File Offset: 0x00079808
	public static float EaseInCubic(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value + start;
	}

	// Token: 0x060010AE RID: 4270 RVA: 0x0007B618 File Offset: 0x00079818
	public static float EaseOutCubic(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * (value * value * value + 1f) + start;
	}

	// Token: 0x060010AF RID: 4271 RVA: 0x0007B638 File Offset: 0x00079838
	public static float EaseInOutCubic(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end * 0.5f * value * value * value + start;
		}
		value -= 2f;
		return end * 0.5f * (value * value * value + 2f) + start;
	}

	// Token: 0x060010B0 RID: 4272 RVA: 0x0007B689 File Offset: 0x00079889
	public static float EaseInQuart(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value * value + start;
	}

	// Token: 0x060010B1 RID: 4273 RVA: 0x0007B69B File Offset: 0x0007989B
	public static float EaseOutQuart(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return -end * (value * value * value * value - 1f) + start;
	}

	// Token: 0x060010B2 RID: 4274 RVA: 0x0007B6C0 File Offset: 0x000798C0
	public static float EaseInOutQuart(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end * 0.5f * value * value * value * value + start;
		}
		value -= 2f;
		return -end * 0.5f * (value * value * value * value - 2f) + start;
	}

	// Token: 0x060010B3 RID: 4275 RVA: 0x0007B716 File Offset: 0x00079916
	public static float EaseInQuint(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value * value * value + start;
	}

	// Token: 0x060010B4 RID: 4276 RVA: 0x0007B72A File Offset: 0x0007992A
	public static float EaseOutQuint(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * (value * value * value * value * value + 1f) + start;
	}

	// Token: 0x060010B5 RID: 4277 RVA: 0x0007B750 File Offset: 0x00079950
	public static float EaseInOutQuint(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end * 0.5f * value * value * value * value * value + start;
		}
		value -= 2f;
		return end * 0.5f * (value * value * value * value * value + 2f) + start;
	}

	// Token: 0x060010B6 RID: 4278 RVA: 0x0007B7A9 File Offset: 0x000799A9
	public static float EaseInSine(float start, float end, float value)
	{
		end -= start;
		return -end * Mathf.Cos(value * 1.5707964f) + end + start;
	}

	// Token: 0x060010B7 RID: 4279 RVA: 0x0007B7C3 File Offset: 0x000799C3
	public static float EaseOutSine(float start, float end, float value)
	{
		end -= start;
		return end * Mathf.Sin(value * 1.5707964f) + start;
	}

	// Token: 0x060010B8 RID: 4280 RVA: 0x0007B7DA File Offset: 0x000799DA
	public static float EaseInOutSine(float start, float end, float value)
	{
		end -= start;
		return -end * 0.5f * (Mathf.Cos(3.1415927f * value) - 1f) + start;
	}

	// Token: 0x060010B9 RID: 4281 RVA: 0x0007B7FE File Offset: 0x000799FE
	public static float EaseInExpo(float start, float end, float value)
	{
		end -= start;
		return end * Mathf.Pow(2f, 10f * (value - 1f)) + start;
	}

	// Token: 0x060010BA RID: 4282 RVA: 0x0007B820 File Offset: 0x00079A20
	public static float EaseOutExpo(float start, float end, float value)
	{
		end -= start;
		return end * (-Mathf.Pow(2f, -10f * value) + 1f) + start;
	}

	// Token: 0x060010BB RID: 4283 RVA: 0x0007B844 File Offset: 0x00079A44
	public static float EaseInOutExpo(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end * 0.5f * Mathf.Pow(2f, 10f * (value - 1f)) + start;
		}
		value -= 1f;
		return end * 0.5f * (-Mathf.Pow(2f, -10f * value) + 2f) + start;
	}

	// Token: 0x060010BC RID: 4284 RVA: 0x0007B8B4 File Offset: 0x00079AB4
	public static float EaseInCirc(float start, float end, float value)
	{
		end -= start;
		return -end * (Mathf.Sqrt(1f - value * value) - 1f) + start;
	}

	// Token: 0x060010BD RID: 4285 RVA: 0x0007B8D4 File Offset: 0x00079AD4
	public static float EaseOutCirc(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * Mathf.Sqrt(1f - value * value) + start;
	}

	// Token: 0x060010BE RID: 4286 RVA: 0x0007B8F8 File Offset: 0x00079AF8
	public static float EaseInOutCirc(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return -end * 0.5f * (Mathf.Sqrt(1f - value * value) - 1f) + start;
		}
		value -= 2f;
		return end * 0.5f * (Mathf.Sqrt(1f - value * value) + 1f) + start;
	}

	// Token: 0x060010BF RID: 4287 RVA: 0x0007B964 File Offset: 0x00079B64
	public static float EaseInBounce(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		return end - EasingFunction.EaseOutBounce(0f, end, num - value) + start;
	}

	// Token: 0x060010C0 RID: 4288 RVA: 0x0007B990 File Offset: 0x00079B90
	public static float EaseOutBounce(float start, float end, float value)
	{
		value /= 1f;
		end -= start;
		if (value < 0.36363637f)
		{
			return end * (7.5625f * value * value) + start;
		}
		if (value < 0.72727275f)
		{
			value -= 0.54545456f;
			return end * (7.5625f * value * value + 0.75f) + start;
		}
		if ((double)value < 0.9090909090909091)
		{
			value -= 0.8181818f;
			return end * (7.5625f * value * value + 0.9375f) + start;
		}
		value -= 0.95454544f;
		return end * (7.5625f * value * value + 0.984375f) + start;
	}

	// Token: 0x060010C1 RID: 4289 RVA: 0x0007BA2C File Offset: 0x00079C2C
	public static float EaseInOutBounce(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		if (value < num * 0.5f)
		{
			return EasingFunction.EaseInBounce(0f, end, value * 2f) * 0.5f + start;
		}
		return EasingFunction.EaseOutBounce(0f, end, value * 2f - num) * 0.5f + end * 0.5f + start;
	}

	// Token: 0x060010C2 RID: 4290 RVA: 0x0007BA90 File Offset: 0x00079C90
	public static float EaseInBack(float start, float end, float value)
	{
		end -= start;
		value /= 1f;
		float num = 1.70158f;
		return end * value * value * ((num + 1f) * value - num) + start;
	}

	// Token: 0x060010C3 RID: 4291 RVA: 0x0007BAC4 File Offset: 0x00079CC4
	public static float EaseOutBack(float start, float end, float value)
	{
		float num = 1.70158f;
		end -= start;
		value -= 1f;
		return end * (value * value * ((num + 1f) * value + num) + 1f) + start;
	}

	// Token: 0x060010C4 RID: 4292 RVA: 0x0007BB00 File Offset: 0x00079D00
	public static float EaseInOutBack(float start, float end, float value)
	{
		float num = 1.70158f;
		end -= start;
		value /= 0.5f;
		if (value < 1f)
		{
			num *= 1.525f;
			return end * 0.5f * (value * value * ((num + 1f) * value - num)) + start;
		}
		value -= 2f;
		num *= 1.525f;
		return end * 0.5f * (value * value * ((num + 1f) * value + num) + 2f) + start;
	}

	// Token: 0x060010C5 RID: 4293 RVA: 0x0007BB7C File Offset: 0x00079D7C
	public static float EaseInElastic(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= num) == 1f)
		{
			return start + end;
		}
		float num4;
		if (num3 == 0f || num3 < Mathf.Abs(end))
		{
			num3 = end;
			num4 = num2 / 4f;
		}
		else
		{
			num4 = num2 / 6.2831855f * Mathf.Asin(end / num3);
		}
		return -(num3 * Mathf.Pow(2f, 10f * (value -= 1f)) * Mathf.Sin((value * num - num4) * 6.2831855f / num2)) + start;
	}

	// Token: 0x060010C6 RID: 4294 RVA: 0x0007BC20 File Offset: 0x00079E20
	public static float EaseOutElastic(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= num) == 1f)
		{
			return start + end;
		}
		float num4;
		if (num3 == 0f || num3 < Mathf.Abs(end))
		{
			num3 = end;
			num4 = num2 * 0.25f;
		}
		else
		{
			num4 = num2 / 6.2831855f * Mathf.Asin(end / num3);
		}
		return num3 * Mathf.Pow(2f, -10f * value) * Mathf.Sin((value * num - num4) * 6.2831855f / num2) + end + start;
	}

	// Token: 0x060010C7 RID: 4295 RVA: 0x0007BCBC File Offset: 0x00079EBC
	public static float EaseInOutElastic(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= num * 0.5f) == 2f)
		{
			return start + end;
		}
		float num4;
		if (num3 == 0f || num3 < Mathf.Abs(end))
		{
			num3 = end;
			num4 = num2 / 4f;
		}
		else
		{
			num4 = num2 / 6.2831855f * Mathf.Asin(end / num3);
		}
		if (value < 1f)
		{
			return -0.5f * (num3 * Mathf.Pow(2f, 10f * (value -= 1f)) * Mathf.Sin((value * num - num4) * 6.2831855f / num2)) + start;
		}
		return num3 * Mathf.Pow(2f, -10f * (value -= 1f)) * Mathf.Sin((value * num - num4) * 6.2831855f / num2) * 0.5f + end + start;
	}

	// Token: 0x060010C8 RID: 4296 RVA: 0x0007BDAA File Offset: 0x00079FAA
	public static float LinearD(float start, float end, float value)
	{
		return end - start;
	}

	// Token: 0x060010C9 RID: 4297 RVA: 0x0007BDAF File Offset: 0x00079FAF
	public static float EaseInQuadD(float start, float end, float value)
	{
		return 2f * (end - start) * value;
	}

	// Token: 0x060010CA RID: 4298 RVA: 0x0007BDBC File Offset: 0x00079FBC
	public static float EaseOutQuadD(float start, float end, float value)
	{
		end -= start;
		return -end * value - end * (value - 2f);
	}

	// Token: 0x060010CB RID: 4299 RVA: 0x0007BDD1 File Offset: 0x00079FD1
	public static float EaseInOutQuadD(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end * value;
		}
		value -= 1f;
		return end * (1f - value);
	}

	// Token: 0x060010CC RID: 4300 RVA: 0x0007BDFF File Offset: 0x00079FFF
	public static float EaseInCubicD(float start, float end, float value)
	{
		return 3f * (end - start) * value * value;
	}

	// Token: 0x060010CD RID: 4301 RVA: 0x0007BE0E File Offset: 0x0007A00E
	public static float EaseOutCubicD(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return 3f * end * value * value;
	}

	// Token: 0x060010CE RID: 4302 RVA: 0x0007BE29 File Offset: 0x0007A029
	public static float EaseInOutCubicD(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return 1.5f * end * value * value;
		}
		value -= 2f;
		return 1.5f * end * value * value;
	}

	// Token: 0x060010CF RID: 4303 RVA: 0x0007BE61 File Offset: 0x0007A061
	public static float EaseInQuartD(float start, float end, float value)
	{
		return 4f * (end - start) * value * value * value;
	}

	// Token: 0x060010D0 RID: 4304 RVA: 0x0007BE72 File Offset: 0x0007A072
	public static float EaseOutQuartD(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return -4f * end * value * value * value;
	}

	// Token: 0x060010D1 RID: 4305 RVA: 0x0007BE8F File Offset: 0x0007A08F
	public static float EaseInOutQuartD(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return 2f * end * value * value * value;
		}
		value -= 2f;
		return -2f * end * value * value * value;
	}

	// Token: 0x060010D2 RID: 4306 RVA: 0x0007BECB File Offset: 0x0007A0CB
	public static float EaseInQuintD(float start, float end, float value)
	{
		return 5f * (end - start) * value * value * value * value;
	}

	// Token: 0x060010D3 RID: 4307 RVA: 0x0007BEDE File Offset: 0x0007A0DE
	public static float EaseOutQuintD(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return 5f * end * value * value * value * value;
	}

	// Token: 0x060010D4 RID: 4308 RVA: 0x0007BEFD File Offset: 0x0007A0FD
	public static float EaseInOutQuintD(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return 2.5f * end * value * value * value * value;
		}
		value -= 2f;
		return 2.5f * end * value * value * value * value;
	}

	// Token: 0x060010D5 RID: 4309 RVA: 0x0007BF3D File Offset: 0x0007A13D
	public static float EaseInSineD(float start, float end, float value)
	{
		return (end - start) * 0.5f * 3.1415927f * Mathf.Sin(1.5707964f * value);
	}

	// Token: 0x060010D6 RID: 4310 RVA: 0x0007BF5B File Offset: 0x0007A15B
	public static float EaseOutSineD(float start, float end, float value)
	{
		end -= start;
		return 1.5707964f * end * Mathf.Cos(value * 1.5707964f);
	}

	// Token: 0x060010D7 RID: 4311 RVA: 0x0007BF76 File Offset: 0x0007A176
	public static float EaseInOutSineD(float start, float end, float value)
	{
		end -= start;
		return end * 0.5f * 3.1415927f * Mathf.Cos(3.1415927f * value);
	}

	// Token: 0x060010D8 RID: 4312 RVA: 0x0007BF97 File Offset: 0x0007A197
	public static float EaseInExpoD(float start, float end, float value)
	{
		return 6.931472f * (end - start) * Mathf.Pow(2f, 10f * (value - 1f));
	}

	// Token: 0x060010D9 RID: 4313 RVA: 0x0007BFBA File Offset: 0x0007A1BA
	public static float EaseOutExpoD(float start, float end, float value)
	{
		end -= start;
		return 3.465736f * end * Mathf.Pow(2f, 1f - 10f * value);
	}

	// Token: 0x060010DA RID: 4314 RVA: 0x0007BFE0 File Offset: 0x0007A1E0
	public static float EaseInOutExpoD(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return 3.465736f * end * Mathf.Pow(2f, 10f * (value - 1f));
		}
		value -= 1f;
		return 3.465736f * end / Mathf.Pow(2f, 10f * value);
	}

	// Token: 0x060010DB RID: 4315 RVA: 0x0007C045 File Offset: 0x0007A245
	public static float EaseInCircD(float start, float end, float value)
	{
		return (end - start) * value / Mathf.Sqrt(1f - value * value);
	}

	// Token: 0x060010DC RID: 4316 RVA: 0x0007C05B File Offset: 0x0007A25B
	public static float EaseOutCircD(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return -end * value / Mathf.Sqrt(1f - value * value);
	}

	// Token: 0x060010DD RID: 4317 RVA: 0x0007C080 File Offset: 0x0007A280
	public static float EaseInOutCircD(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end * value / (2f * Mathf.Sqrt(1f - value * value));
		}
		value -= 2f;
		return -end * value / (2f * Mathf.Sqrt(1f - value * value));
	}

	// Token: 0x060010DE RID: 4318 RVA: 0x0007C0E0 File Offset: 0x0007A2E0
	public static float EaseInBounceD(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		return EasingFunction.EaseOutBounceD(0f, end, num - value);
	}

	// Token: 0x060010DF RID: 4319 RVA: 0x0007C108 File Offset: 0x0007A308
	public static float EaseOutBounceD(float start, float end, float value)
	{
		value /= 1f;
		end -= start;
		if (value < 0.36363637f)
		{
			return 2f * end * 7.5625f * value;
		}
		if (value < 0.72727275f)
		{
			value -= 0.54545456f;
			return 2f * end * 7.5625f * value;
		}
		if ((double)value < 0.9090909090909091)
		{
			value -= 0.8181818f;
			return 2f * end * 7.5625f * value;
		}
		value -= 0.95454544f;
		return 2f * end * 7.5625f * value;
	}

	// Token: 0x060010E0 RID: 4320 RVA: 0x0007C19C File Offset: 0x0007A39C
	public static float EaseInOutBounceD(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		if (value < num * 0.5f)
		{
			return EasingFunction.EaseInBounceD(0f, end, value * 2f) * 0.5f;
		}
		return EasingFunction.EaseOutBounceD(0f, end, value * 2f - num) * 0.5f;
	}

	// Token: 0x060010E1 RID: 4321 RVA: 0x0007C1F4 File Offset: 0x0007A3F4
	public static float EaseInBackD(float start, float end, float value)
	{
		float num = 1.70158f;
		return 3f * (num + 1f) * (end - start) * value * value - 2f * num * (end - start) * value;
	}

	// Token: 0x060010E2 RID: 4322 RVA: 0x0007C22C File Offset: 0x0007A42C
	public static float EaseOutBackD(float start, float end, float value)
	{
		float num = 1.70158f;
		end -= start;
		value -= 1f;
		return end * ((num + 1f) * value * value + 2f * value * ((num + 1f) * value + num));
	}

	// Token: 0x060010E3 RID: 4323 RVA: 0x0007C270 File Offset: 0x0007A470
	public static float EaseInOutBackD(float start, float end, float value)
	{
		float num = 1.70158f;
		end -= start;
		value /= 0.5f;
		if (value < 1f)
		{
			num *= 1.525f;
			return 0.5f * end * (num + 1f) * value * value + end * value * ((num + 1f) * value - num);
		}
		value -= 2f;
		num *= 1.525f;
		return 0.5f * end * ((num + 1f) * value * value + 2f * value * ((num + 1f) * value + num));
	}

	// Token: 0x060010E4 RID: 4324 RVA: 0x0007C300 File Offset: 0x0007A500
	public static float EaseInElasticD(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		float num4;
		if (num3 == 0f || num3 < Mathf.Abs(end))
		{
			num3 = end;
			num4 = num2 / 4f;
		}
		else
		{
			num4 = num2 / 6.2831855f * Mathf.Asin(end / num3);
		}
		float num5 = 6.2831855f;
		return -num3 * num * num5 * Mathf.Cos(num5 * (num * (value - 1f) - num4) / num2) / num2 - 3.465736f * num3 * Mathf.Sin(num5 * (num * (value - 1f) - num4) / num2) * Mathf.Pow(2f, 10f * (value - 1f) + 1f);
	}

	// Token: 0x060010E5 RID: 4325 RVA: 0x0007C3B8 File Offset: 0x0007A5B8
	public static float EaseOutElasticD(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		float num4;
		if (num3 == 0f || num3 < Mathf.Abs(end))
		{
			num3 = end;
			num4 = num2 * 0.25f;
		}
		else
		{
			num4 = num2 / 6.2831855f * Mathf.Asin(end / num3);
		}
		return num3 * 3.1415927f * num * Mathf.Pow(2f, 1f - 10f * value) * Mathf.Cos(6.2831855f * (num * value - num4) / num2) / num2 - 3.465736f * num3 * Mathf.Pow(2f, 1f - 10f * value) * Mathf.Sin(6.2831855f * (num * value - num4) / num2);
	}

	// Token: 0x060010E6 RID: 4326 RVA: 0x0007C478 File Offset: 0x0007A678
	public static float EaseInOutElasticD(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		float num4;
		if (num3 == 0f || num3 < Mathf.Abs(end))
		{
			num3 = end;
			num4 = num2 / 4f;
		}
		else
		{
			num4 = num2 / 6.2831855f * Mathf.Asin(end / num3);
		}
		if (value < 1f)
		{
			value -= 1f;
			return -3.465736f * num3 * Mathf.Pow(2f, 10f * value) * Mathf.Sin(6.2831855f * (num * value - 2f) / num2) - num3 * 3.1415927f * num * Mathf.Pow(2f, 10f * value) * Mathf.Cos(6.2831855f * (num * value - num4) / num2) / num2;
		}
		value -= 1f;
		return num3 * 3.1415927f * num * Mathf.Cos(6.2831855f * (num * value - num4) / num2) / (num2 * Mathf.Pow(2f, 10f * value)) - 3.465736f * num3 * Mathf.Sin(6.2831855f * (num * value - num4) / num2) / Mathf.Pow(2f, 10f * value);
	}

	// Token: 0x060010E7 RID: 4327 RVA: 0x0007C5A8 File Offset: 0x0007A7A8
	public static float SpringD(float start, float end, float value)
	{
		value = Mathf.Clamp01(value);
		end -= start;
		return end * (6f * (1f - value) / 5f + 1f) * (-2.2f * Mathf.Pow(1f - value, 1.2f) * Mathf.Sin(3.1415927f * value * (2.5f * value * value * value + 0.2f)) + Mathf.Pow(1f - value, 2.2f) * (3.1415927f * (2.5f * value * value * value + 0.2f) + 23.561945f * value * value * value) * Mathf.Cos(3.1415927f * value * (2.5f * value * value * value + 0.2f)) + 1f) - 6f * end * (Mathf.Pow(1f - value, 2.2f) * Mathf.Sin(3.1415927f * value * (2.5f * value * value * value + 0.2f)) + value / 5f);
	}

	// Token: 0x060010E8 RID: 4328 RVA: 0x0007C6B0 File Offset: 0x0007A8B0
	public static EasingFunction.Function GetEasingFunction(EasingFunction.Ease easingFunction)
	{
		if (easingFunction == EasingFunction.Ease.EaseInQuad)
		{
			return new EasingFunction.Function(EasingFunction.EaseInQuad);
		}
		if (easingFunction == EasingFunction.Ease.EaseOutQuad)
		{
			return new EasingFunction.Function(EasingFunction.EaseOutQuad);
		}
		if (easingFunction == EasingFunction.Ease.EaseInOutQuad)
		{
			return new EasingFunction.Function(EasingFunction.EaseInOutQuad);
		}
		if (easingFunction == EasingFunction.Ease.EaseInCubic)
		{
			return new EasingFunction.Function(EasingFunction.EaseInCubic);
		}
		if (easingFunction == EasingFunction.Ease.EaseOutCubic)
		{
			return new EasingFunction.Function(EasingFunction.EaseOutCubic);
		}
		if (easingFunction == EasingFunction.Ease.EaseInOutCubic)
		{
			return new EasingFunction.Function(EasingFunction.EaseInOutCubic);
		}
		if (easingFunction == EasingFunction.Ease.EaseInQuart)
		{
			return new EasingFunction.Function(EasingFunction.EaseInQuart);
		}
		if (easingFunction == EasingFunction.Ease.EaseOutQuart)
		{
			return new EasingFunction.Function(EasingFunction.EaseOutQuart);
		}
		if (easingFunction == EasingFunction.Ease.EaseInOutQuart)
		{
			return new EasingFunction.Function(EasingFunction.EaseInOutQuart);
		}
		if (easingFunction == EasingFunction.Ease.EaseInQuint)
		{
			return new EasingFunction.Function(EasingFunction.EaseInQuint);
		}
		if (easingFunction == EasingFunction.Ease.EaseOutQuint)
		{
			return new EasingFunction.Function(EasingFunction.EaseOutQuint);
		}
		if (easingFunction == EasingFunction.Ease.EaseInOutQuint)
		{
			return new EasingFunction.Function(EasingFunction.EaseInOutQuint);
		}
		if (easingFunction == EasingFunction.Ease.EaseInSine)
		{
			return new EasingFunction.Function(EasingFunction.EaseInSine);
		}
		if (easingFunction == EasingFunction.Ease.EaseOutSine)
		{
			return new EasingFunction.Function(EasingFunction.EaseOutSine);
		}
		if (easingFunction == EasingFunction.Ease.EaseInOutSine)
		{
			return new EasingFunction.Function(EasingFunction.EaseInOutSine);
		}
		if (easingFunction == EasingFunction.Ease.EaseInExpo)
		{
			return new EasingFunction.Function(EasingFunction.EaseInExpo);
		}
		if (easingFunction == EasingFunction.Ease.EaseOutExpo)
		{
			return new EasingFunction.Function(EasingFunction.EaseOutExpo);
		}
		if (easingFunction == EasingFunction.Ease.EaseInOutExpo)
		{
			return new EasingFunction.Function(EasingFunction.EaseInOutExpo);
		}
		if (easingFunction == EasingFunction.Ease.EaseInCirc)
		{
			return new EasingFunction.Function(EasingFunction.EaseInCirc);
		}
		if (easingFunction == EasingFunction.Ease.EaseOutCirc)
		{
			return new EasingFunction.Function(EasingFunction.EaseOutCirc);
		}
		if (easingFunction == EasingFunction.Ease.EaseInOutCirc)
		{
			return new EasingFunction.Function(EasingFunction.EaseInOutCirc);
		}
		if (easingFunction == EasingFunction.Ease.Linear)
		{
			return new EasingFunction.Function(EasingFunction.Linear);
		}
		if (easingFunction == EasingFunction.Ease.Spring)
		{
			return new EasingFunction.Function(EasingFunction.Spring);
		}
		if (easingFunction == EasingFunction.Ease.EaseInBounce)
		{
			return new EasingFunction.Function(EasingFunction.EaseInBounce);
		}
		if (easingFunction == EasingFunction.Ease.EaseOutBounce)
		{
			return new EasingFunction.Function(EasingFunction.EaseOutBounce);
		}
		if (easingFunction == EasingFunction.Ease.EaseInOutBounce)
		{
			return new EasingFunction.Function(EasingFunction.EaseInOutBounce);
		}
		if (easingFunction == EasingFunction.Ease.EaseInBack)
		{
			return new EasingFunction.Function(EasingFunction.EaseInBack);
		}
		if (easingFunction == EasingFunction.Ease.EaseOutBack)
		{
			return new EasingFunction.Function(EasingFunction.EaseOutBack);
		}
		if (easingFunction == EasingFunction.Ease.EaseInOutBack)
		{
			return new EasingFunction.Function(EasingFunction.EaseInOutBack);
		}
		if (easingFunction == EasingFunction.Ease.EaseInElastic)
		{
			return new EasingFunction.Function(EasingFunction.EaseInElastic);
		}
		if (easingFunction == EasingFunction.Ease.EaseOutElastic)
		{
			return new EasingFunction.Function(EasingFunction.EaseOutElastic);
		}
		if (easingFunction == EasingFunction.Ease.EaseInOutElastic)
		{
			return new EasingFunction.Function(EasingFunction.EaseInOutElastic);
		}
		return null;
	}

	// Token: 0x060010E9 RID: 4329 RVA: 0x0007C8F4 File Offset: 0x0007AAF4
	public static EasingFunction.Function GetEasingFunctionDerivative(EasingFunction.Ease easingFunction)
	{
		if (easingFunction == EasingFunction.Ease.EaseInQuad)
		{
			return new EasingFunction.Function(EasingFunction.EaseInQuadD);
		}
		if (easingFunction == EasingFunction.Ease.EaseOutQuad)
		{
			return new EasingFunction.Function(EasingFunction.EaseOutQuadD);
		}
		if (easingFunction == EasingFunction.Ease.EaseInOutQuad)
		{
			return new EasingFunction.Function(EasingFunction.EaseInOutQuadD);
		}
		if (easingFunction == EasingFunction.Ease.EaseInCubic)
		{
			return new EasingFunction.Function(EasingFunction.EaseInCubicD);
		}
		if (easingFunction == EasingFunction.Ease.EaseOutCubic)
		{
			return new EasingFunction.Function(EasingFunction.EaseOutCubicD);
		}
		if (easingFunction == EasingFunction.Ease.EaseInOutCubic)
		{
			return new EasingFunction.Function(EasingFunction.EaseInOutCubicD);
		}
		if (easingFunction == EasingFunction.Ease.EaseInQuart)
		{
			return new EasingFunction.Function(EasingFunction.EaseInQuartD);
		}
		if (easingFunction == EasingFunction.Ease.EaseOutQuart)
		{
			return new EasingFunction.Function(EasingFunction.EaseOutQuartD);
		}
		if (easingFunction == EasingFunction.Ease.EaseInOutQuart)
		{
			return new EasingFunction.Function(EasingFunction.EaseInOutQuartD);
		}
		if (easingFunction == EasingFunction.Ease.EaseInQuint)
		{
			return new EasingFunction.Function(EasingFunction.EaseInQuintD);
		}
		if (easingFunction == EasingFunction.Ease.EaseOutQuint)
		{
			return new EasingFunction.Function(EasingFunction.EaseOutQuintD);
		}
		if (easingFunction == EasingFunction.Ease.EaseInOutQuint)
		{
			return new EasingFunction.Function(EasingFunction.EaseInOutQuintD);
		}
		if (easingFunction == EasingFunction.Ease.EaseInSine)
		{
			return new EasingFunction.Function(EasingFunction.EaseInSineD);
		}
		if (easingFunction == EasingFunction.Ease.EaseOutSine)
		{
			return new EasingFunction.Function(EasingFunction.EaseOutSineD);
		}
		if (easingFunction == EasingFunction.Ease.EaseInOutSine)
		{
			return new EasingFunction.Function(EasingFunction.EaseInOutSineD);
		}
		if (easingFunction == EasingFunction.Ease.EaseInExpo)
		{
			return new EasingFunction.Function(EasingFunction.EaseInExpoD);
		}
		if (easingFunction == EasingFunction.Ease.EaseOutExpo)
		{
			return new EasingFunction.Function(EasingFunction.EaseOutExpoD);
		}
		if (easingFunction == EasingFunction.Ease.EaseInOutExpo)
		{
			return new EasingFunction.Function(EasingFunction.EaseInOutExpoD);
		}
		if (easingFunction == EasingFunction.Ease.EaseInCirc)
		{
			return new EasingFunction.Function(EasingFunction.EaseInCircD);
		}
		if (easingFunction == EasingFunction.Ease.EaseOutCirc)
		{
			return new EasingFunction.Function(EasingFunction.EaseOutCircD);
		}
		if (easingFunction == EasingFunction.Ease.EaseInOutCirc)
		{
			return new EasingFunction.Function(EasingFunction.EaseInOutCircD);
		}
		if (easingFunction == EasingFunction.Ease.Linear)
		{
			return new EasingFunction.Function(EasingFunction.LinearD);
		}
		if (easingFunction == EasingFunction.Ease.Spring)
		{
			return new EasingFunction.Function(EasingFunction.SpringD);
		}
		if (easingFunction == EasingFunction.Ease.EaseInBounce)
		{
			return new EasingFunction.Function(EasingFunction.EaseInBounceD);
		}
		if (easingFunction == EasingFunction.Ease.EaseOutBounce)
		{
			return new EasingFunction.Function(EasingFunction.EaseOutBounceD);
		}
		if (easingFunction == EasingFunction.Ease.EaseInOutBounce)
		{
			return new EasingFunction.Function(EasingFunction.EaseInOutBounceD);
		}
		if (easingFunction == EasingFunction.Ease.EaseInBack)
		{
			return new EasingFunction.Function(EasingFunction.EaseInBackD);
		}
		if (easingFunction == EasingFunction.Ease.EaseOutBack)
		{
			return new EasingFunction.Function(EasingFunction.EaseOutBackD);
		}
		if (easingFunction == EasingFunction.Ease.EaseInOutBack)
		{
			return new EasingFunction.Function(EasingFunction.EaseInOutBackD);
		}
		if (easingFunction == EasingFunction.Ease.EaseInElastic)
		{
			return new EasingFunction.Function(EasingFunction.EaseInElasticD);
		}
		if (easingFunction == EasingFunction.Ease.EaseOutElastic)
		{
			return new EasingFunction.Function(EasingFunction.EaseOutElasticD);
		}
		if (easingFunction == EasingFunction.Ease.EaseInOutElastic)
		{
			return new EasingFunction.Function(EasingFunction.EaseInOutElasticD);
		}
		return null;
	}

	// Token: 0x04001253 RID: 4691
	private const float NATURAL_LOG_OF_2 = 0.6931472f;

	// Token: 0x020003F0 RID: 1008
	public enum Ease
	{
		// Token: 0x04001A68 RID: 6760
		EaseInQuad,
		// Token: 0x04001A69 RID: 6761
		EaseOutQuad,
		// Token: 0x04001A6A RID: 6762
		EaseInOutQuad,
		// Token: 0x04001A6B RID: 6763
		EaseInCubic,
		// Token: 0x04001A6C RID: 6764
		EaseOutCubic,
		// Token: 0x04001A6D RID: 6765
		EaseInOutCubic,
		// Token: 0x04001A6E RID: 6766
		EaseInQuart,
		// Token: 0x04001A6F RID: 6767
		EaseOutQuart,
		// Token: 0x04001A70 RID: 6768
		EaseInOutQuart,
		// Token: 0x04001A71 RID: 6769
		EaseInQuint,
		// Token: 0x04001A72 RID: 6770
		EaseOutQuint,
		// Token: 0x04001A73 RID: 6771
		EaseInOutQuint,
		// Token: 0x04001A74 RID: 6772
		EaseInSine,
		// Token: 0x04001A75 RID: 6773
		EaseOutSine,
		// Token: 0x04001A76 RID: 6774
		EaseInOutSine,
		// Token: 0x04001A77 RID: 6775
		EaseInExpo,
		// Token: 0x04001A78 RID: 6776
		EaseOutExpo,
		// Token: 0x04001A79 RID: 6777
		EaseInOutExpo,
		// Token: 0x04001A7A RID: 6778
		EaseInCirc,
		// Token: 0x04001A7B RID: 6779
		EaseOutCirc,
		// Token: 0x04001A7C RID: 6780
		EaseInOutCirc,
		// Token: 0x04001A7D RID: 6781
		Linear,
		// Token: 0x04001A7E RID: 6782
		Spring,
		// Token: 0x04001A7F RID: 6783
		EaseInBounce,
		// Token: 0x04001A80 RID: 6784
		EaseOutBounce,
		// Token: 0x04001A81 RID: 6785
		EaseInOutBounce,
		// Token: 0x04001A82 RID: 6786
		EaseInBack,
		// Token: 0x04001A83 RID: 6787
		EaseOutBack,
		// Token: 0x04001A84 RID: 6788
		EaseInOutBack,
		// Token: 0x04001A85 RID: 6789
		EaseInElastic,
		// Token: 0x04001A86 RID: 6790
		EaseOutElastic,
		// Token: 0x04001A87 RID: 6791
		EaseInOutElastic
	}

	// Token: 0x020003F1 RID: 1009
	// (Invoke) Token: 0x060018B2 RID: 6322
	public delegate float Function(float s, float e, float v);
}
