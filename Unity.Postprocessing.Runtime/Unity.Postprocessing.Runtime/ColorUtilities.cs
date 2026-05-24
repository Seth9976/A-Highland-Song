using System;

namespace UnityEngine.Rendering.PostProcessing
{
	// Token: 0x0200005A RID: 90
	public static class ColorUtilities
	{
		// Token: 0x060001A3 RID: 419 RVA: 0x0000DBF7 File Offset: 0x0000BDF7
		public static float StandardIlluminantY(float x)
		{
			return 2.87f * x - 3f * x * x - 0.27509508f;
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x0000DC10 File Offset: 0x0000BE10
		public static Vector3 CIExyToLMS(float x, float y)
		{
			float num = 1f;
			float num2 = num * x / y;
			float num3 = num * (1f - x - y) / y;
			float num4 = 0.7328f * num2 + 0.4296f * num - 0.1624f * num3;
			float num5 = -0.7036f * num2 + 1.6975f * num + 0.0061f * num3;
			float num6 = 0.003f * num2 + 0.0136f * num + 0.9834f * num3;
			return new Vector3(num4, num5, num6);
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x0000DC88 File Offset: 0x0000BE88
		public static Vector3 ComputeColorBalance(float temperature, float tint)
		{
			float num = temperature / 60f;
			float num2 = tint / 60f;
			float num3 = 0.31271f - num * ((num < 0f) ? 0.1f : 0.05f);
			float num4 = ColorUtilities.StandardIlluminantY(num3) + num2 * 0.05f;
			Vector3 vector = new Vector3(0.949237f, 1.03542f, 1.08728f);
			Vector3 vector2 = ColorUtilities.CIExyToLMS(num3, num4);
			return new Vector3(vector.x / vector2.x, vector.y / vector2.y, vector.z / vector2.z);
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x0000DD20 File Offset: 0x0000BF20
		public static Vector3 ColorToLift(Vector4 color)
		{
			Vector3 vector = new Vector3(color.x, color.y, color.z);
			float num = vector.x * 0.2126f + vector.y * 0.7152f + vector.z * 0.0722f;
			vector = new Vector3(vector.x - num, vector.y - num, vector.z - num);
			float w = color.w;
			return new Vector3(vector.x + w, vector.y + w, vector.z + w);
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x0000DDB0 File Offset: 0x0000BFB0
		public static Vector3 ColorToInverseGamma(Vector4 color)
		{
			Vector3 vector = new Vector3(color.x, color.y, color.z);
			float num = vector.x * 0.2126f + vector.y * 0.7152f + vector.z * 0.0722f;
			vector = new Vector3(vector.x - num, vector.y - num, vector.z - num);
			float num2 = color.w + 1f;
			return new Vector3(1f / Mathf.Max(vector.x + num2, 0.001f), 1f / Mathf.Max(vector.y + num2, 0.001f), 1f / Mathf.Max(vector.z + num2, 0.001f));
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x0000DE78 File Offset: 0x0000C078
		public static Vector3 ColorToGain(Vector4 color)
		{
			Vector3 vector = new Vector3(color.x, color.y, color.z);
			float num = vector.x * 0.2126f + vector.y * 0.7152f + vector.z * 0.0722f;
			vector = new Vector3(vector.x - num, vector.y - num, vector.z - num);
			float num2 = color.w + 1f;
			return new Vector3(vector.x + num2, vector.y + num2, vector.z + num2);
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x0000DF0E File Offset: 0x0000C10E
		public static float LogCToLinear(float x)
		{
			if (x <= 0.1530537f)
			{
				return (x - 0.092819f) / 5.301883f;
			}
			return (Mathf.Pow(10f, (x - 0.386036f) / 0.244161f) - 0.047996f) / 5.555556f;
		}

		// Token: 0x060001AA RID: 426 RVA: 0x0000DF49 File Offset: 0x0000C149
		public static float LinearToLogC(float x)
		{
			if (x <= 0.011361f)
			{
				return 5.301883f * x + 0.092819f;
			}
			return 0.244161f * Mathf.Log10(5.555556f * x + 0.047996f) + 0.386036f;
		}

		// Token: 0x060001AB RID: 427 RVA: 0x0000DF80 File Offset: 0x0000C180
		public static uint ToHex(Color c)
		{
			return ((uint)(c.a * 255f) << 24) | ((uint)(c.r * 255f) << 16) | ((uint)(c.g * 255f) << 8) | (uint)(c.b * 255f);
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0000DFCC File Offset: 0x0000C1CC
		public static Color ToRGBA(uint hex)
		{
			return new Color(((hex >> 16) & 255U) / 255f, ((hex >> 8) & 255U) / 255f, (hex & 255U) / 255f, ((hex >> 24) & 255U) / 255f);
		}

		// Token: 0x0400018D RID: 397
		private const float logC_cut = 0.011361f;

		// Token: 0x0400018E RID: 398
		private const float logC_a = 5.555556f;

		// Token: 0x0400018F RID: 399
		private const float logC_b = 0.047996f;

		// Token: 0x04000190 RID: 400
		private const float logC_c = 0.244161f;

		// Token: 0x04000191 RID: 401
		private const float logC_d = 0.386036f;

		// Token: 0x04000192 RID: 402
		private const float logC_e = 5.301883f;

		// Token: 0x04000193 RID: 403
		private const float logC_f = 0.092819f;
	}
}
