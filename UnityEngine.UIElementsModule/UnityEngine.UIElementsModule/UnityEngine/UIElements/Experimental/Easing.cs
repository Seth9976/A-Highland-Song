using System;

namespace UnityEngine.UIElements.Experimental
{
	// Token: 0x0200037F RID: 895
	public static class Easing
	{
		// Token: 0x06001C5F RID: 7263 RVA: 0x00084F7C File Offset: 0x0008317C
		public static float Step(float t)
		{
			return (float)((t < 0.5f) ? 0 : 1);
		}

		// Token: 0x06001C60 RID: 7264 RVA: 0x00084F9C File Offset: 0x0008319C
		public static float Linear(float t)
		{
			return t;
		}

		// Token: 0x06001C61 RID: 7265 RVA: 0x00084FB0 File Offset: 0x000831B0
		public static float InSine(float t)
		{
			return Mathf.Sin(1.5707964f * (t - 1f)) + 1f;
		}

		// Token: 0x06001C62 RID: 7266 RVA: 0x00084FDC File Offset: 0x000831DC
		public static float OutSine(float t)
		{
			return Mathf.Sin(t * 1.5707964f);
		}

		// Token: 0x06001C63 RID: 7267 RVA: 0x00084FFC File Offset: 0x000831FC
		public static float InOutSine(float t)
		{
			return (Mathf.Sin(3.1415927f * (t - 0.5f)) + 1f) * 0.5f;
		}

		// Token: 0x06001C64 RID: 7268 RVA: 0x0008502C File Offset: 0x0008322C
		public static float InQuad(float t)
		{
			return t * t;
		}

		// Token: 0x06001C65 RID: 7269 RVA: 0x00085044 File Offset: 0x00083244
		public static float OutQuad(float t)
		{
			return t * (2f - t);
		}

		// Token: 0x06001C66 RID: 7270 RVA: 0x00085060 File Offset: 0x00083260
		public static float InOutQuad(float t)
		{
			t *= 2f;
			bool flag = t < 1f;
			float num;
			if (flag)
			{
				num = t * t * 0.5f;
			}
			else
			{
				num = -0.5f * ((t - 1f) * (t - 3f) - 1f);
			}
			return num;
		}

		// Token: 0x06001C67 RID: 7271 RVA: 0x000850B0 File Offset: 0x000832B0
		public static float InCubic(float t)
		{
			return Easing.InPower(t, 3);
		}

		// Token: 0x06001C68 RID: 7272 RVA: 0x000850CC File Offset: 0x000832CC
		public static float OutCubic(float t)
		{
			return Easing.OutPower(t, 3);
		}

		// Token: 0x06001C69 RID: 7273 RVA: 0x000850E8 File Offset: 0x000832E8
		public static float InOutCubic(float t)
		{
			return Easing.InOutPower(t, 3);
		}

		// Token: 0x06001C6A RID: 7274 RVA: 0x00085104 File Offset: 0x00083304
		public static float InPower(float t, int power)
		{
			return Mathf.Pow(t, (float)power);
		}

		// Token: 0x06001C6B RID: 7275 RVA: 0x00085120 File Offset: 0x00083320
		public static float OutPower(float t, int power)
		{
			int num = ((power % 2 == 0) ? (-1) : 1);
			return (float)num * (Mathf.Pow(t - 1f, (float)power) + (float)num);
		}

		// Token: 0x06001C6C RID: 7276 RVA: 0x00085154 File Offset: 0x00083354
		public static float InOutPower(float t, int power)
		{
			t *= 2f;
			bool flag = t < 1f;
			float num;
			if (flag)
			{
				num = Easing.InPower(t, power) * 0.5f;
			}
			else
			{
				int num2 = ((power % 2 == 0) ? (-1) : 1);
				num = (float)num2 * 0.5f * (Mathf.Pow(t - 2f, (float)power) + (float)(num2 * 2));
			}
			return num;
		}

		// Token: 0x06001C6D RID: 7277 RVA: 0x000851B4 File Offset: 0x000833B4
		public static float InBounce(float t)
		{
			return 1f - Easing.OutBounce(1f - t);
		}

		// Token: 0x06001C6E RID: 7278 RVA: 0x000851D8 File Offset: 0x000833D8
		public static float OutBounce(float t)
		{
			bool flag = t < 0.36363637f;
			float num;
			if (flag)
			{
				num = 7.5625f * t * t;
			}
			else
			{
				bool flag2 = t < 0.72727275f;
				if (flag2)
				{
					float num2;
					t = (num2 = t - 0.54545456f);
					num = 7.5625f * num2 * t + 0.75f;
				}
				else
				{
					bool flag3 = t < 0.90909094f;
					if (flag3)
					{
						float num3;
						t = (num3 = t - 0.8181818f);
						num = 7.5625f * num3 * t + 0.9375f;
					}
					else
					{
						float num4;
						t = (num4 = t - 0.95454544f);
						num = 7.5625f * num4 * t + 0.984375f;
					}
				}
			}
			return num;
		}

		// Token: 0x06001C6F RID: 7279 RVA: 0x00085278 File Offset: 0x00083478
		public static float InOutBounce(float t)
		{
			bool flag = t < 0.5f;
			float num;
			if (flag)
			{
				num = Easing.InBounce(t * 2f) * 0.5f;
			}
			else
			{
				num = Easing.OutBounce((t - 0.5f) * 2f) * 0.5f + 0.5f;
			}
			return num;
		}

		// Token: 0x06001C70 RID: 7280 RVA: 0x000852CC File Offset: 0x000834CC
		public static float InElastic(float t)
		{
			bool flag = t == 0f;
			float num;
			if (flag)
			{
				num = 0f;
			}
			else
			{
				bool flag2 = t == 1f;
				if (flag2)
				{
					num = 1f;
				}
				else
				{
					float num2 = 0.3f;
					float num3 = num2 / 4f;
					float num4 = Mathf.Pow(2f, 10f * (t -= 1f));
					num = -(num4 * Mathf.Sin((t - num3) * 6.2831855f / num2));
				}
			}
			return num;
		}

		// Token: 0x06001C71 RID: 7281 RVA: 0x00085348 File Offset: 0x00083548
		public static float OutElastic(float t)
		{
			bool flag = t == 0f;
			float num;
			if (flag)
			{
				num = 0f;
			}
			else
			{
				bool flag2 = t == 1f;
				if (flag2)
				{
					num = 1f;
				}
				else
				{
					float num2 = 0.3f;
					float num3 = num2 / 4f;
					num = Mathf.Pow(2f, -10f * t) * Mathf.Sin((t - num3) * 6.2831855f / num2) + 1f;
				}
			}
			return num;
		}

		// Token: 0x06001C72 RID: 7282 RVA: 0x000853BC File Offset: 0x000835BC
		public static float InOutElastic(float t)
		{
			bool flag = t < 0.5f;
			float num;
			if (flag)
			{
				num = Easing.InElastic(t * 2f) * 0.5f;
			}
			else
			{
				num = Easing.OutElastic((t - 0.5f) * 2f) * 0.5f + 0.5f;
			}
			return num;
		}

		// Token: 0x06001C73 RID: 7283 RVA: 0x00085410 File Offset: 0x00083610
		public static float InBack(float t)
		{
			float num = 1.70158f;
			return t * t * ((num + 1f) * t - num);
		}

		// Token: 0x06001C74 RID: 7284 RVA: 0x00085438 File Offset: 0x00083638
		public static float OutBack(float t)
		{
			return 1f - Easing.InBack(1f - t);
		}

		// Token: 0x06001C75 RID: 7285 RVA: 0x0008545C File Offset: 0x0008365C
		public static float InOutBack(float t)
		{
			bool flag = t < 0.5f;
			float num;
			if (flag)
			{
				num = Easing.InBack(t * 2f) * 0.5f;
			}
			else
			{
				num = Easing.OutBack((t - 0.5f) * 2f) * 0.5f + 0.5f;
			}
			return num;
		}

		// Token: 0x06001C76 RID: 7286 RVA: 0x000854B0 File Offset: 0x000836B0
		public static float InBack(float t, float s)
		{
			return t * t * ((s + 1f) * t - s);
		}

		// Token: 0x06001C77 RID: 7287 RVA: 0x000854D4 File Offset: 0x000836D4
		public static float OutBack(float t, float s)
		{
			return 1f - Easing.InBack(1f - t, s);
		}

		// Token: 0x06001C78 RID: 7288 RVA: 0x000854FC File Offset: 0x000836FC
		public static float InOutBack(float t, float s)
		{
			bool flag = t < 0.5f;
			float num;
			if (flag)
			{
				num = Easing.InBack(t * 2f, s) * 0.5f;
			}
			else
			{
				num = Easing.OutBack((t - 0.5f) * 2f, s) * 0.5f + 0.5f;
			}
			return num;
		}

		// Token: 0x06001C79 RID: 7289 RVA: 0x00085554 File Offset: 0x00083754
		public static float InCirc(float t)
		{
			return -(Mathf.Sqrt(1f - t * t) - 1f);
		}

		// Token: 0x06001C7A RID: 7290 RVA: 0x0008557C File Offset: 0x0008377C
		public static float OutCirc(float t)
		{
			t -= 1f;
			return Mathf.Sqrt(1f - t * t);
		}

		// Token: 0x06001C7B RID: 7291 RVA: 0x000855A8 File Offset: 0x000837A8
		public static float InOutCirc(float t)
		{
			t *= 2f;
			bool flag = t < 1f;
			float num;
			if (flag)
			{
				num = -0.5f * (Mathf.Sqrt(1f - t * t) - 1f);
			}
			else
			{
				t -= 2f;
				num = 0.5f * (Mathf.Sqrt(1f - t * t) + 1f);
			}
			return num;
		}

		// Token: 0x04000E59 RID: 3673
		private const float HalfPi = 1.5707964f;
	}
}
