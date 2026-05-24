using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x0200002F RID: 47
	public static class DeadZone
	{
		// Token: 0x06000197 RID: 407 RVA: 0x00007100 File Offset: 0x00005300
		public static Vector2 SeparateNotNormalized(float x, float y, float lowerDeadZone, float upperDeadZone)
		{
			float num = upperDeadZone - lowerDeadZone;
			float num2;
			if (x < 0f)
			{
				if (x > -lowerDeadZone)
				{
					num2 = 0f;
				}
				else if (x < -upperDeadZone)
				{
					num2 = -1f;
				}
				else
				{
					num2 = (x + lowerDeadZone) / num;
				}
			}
			else if (x < lowerDeadZone)
			{
				num2 = 0f;
			}
			else if (x > upperDeadZone)
			{
				num2 = 1f;
			}
			else
			{
				num2 = (x - lowerDeadZone) / num;
			}
			float num3;
			if (y < 0f)
			{
				if (y > -lowerDeadZone)
				{
					num3 = 0f;
				}
				else if (y < -upperDeadZone)
				{
					num3 = -1f;
				}
				else
				{
					num3 = (y + lowerDeadZone) / num;
				}
			}
			else if (y < lowerDeadZone)
			{
				num3 = 0f;
			}
			else if (y > upperDeadZone)
			{
				num3 = 1f;
			}
			else
			{
				num3 = (y - lowerDeadZone) / num;
			}
			return new Vector2(num2, num3);
		}

		// Token: 0x06000198 RID: 408 RVA: 0x000071A8 File Offset: 0x000053A8
		public static Vector2 Separate(float x, float y, float lowerDeadZone, float upperDeadZone)
		{
			float num = upperDeadZone - lowerDeadZone;
			float num2;
			if (x < 0f)
			{
				if (x > -lowerDeadZone)
				{
					num2 = 0f;
				}
				else if (x < -upperDeadZone)
				{
					num2 = -1f;
				}
				else
				{
					num2 = (x + lowerDeadZone) / num;
				}
			}
			else if (x < lowerDeadZone)
			{
				num2 = 0f;
			}
			else if (x > upperDeadZone)
			{
				num2 = 1f;
			}
			else
			{
				num2 = (x - lowerDeadZone) / num;
			}
			float num3;
			if (y < 0f)
			{
				if (y > -lowerDeadZone)
				{
					num3 = 0f;
				}
				else if (y < -upperDeadZone)
				{
					num3 = -1f;
				}
				else
				{
					num3 = (y + lowerDeadZone) / num;
				}
			}
			else if (y < lowerDeadZone)
			{
				num3 = 0f;
			}
			else if (y > upperDeadZone)
			{
				num3 = 1f;
			}
			else
			{
				num3 = (y - lowerDeadZone) / num;
			}
			float num4 = (float)Math.Sqrt((double)(num2 * num2 + num3 * num3));
			if (num4 < 1E-05f)
			{
				return Vector2.zero;
			}
			return new Vector2(num2 / num4, num3 / num4);
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00007274 File Offset: 0x00005474
		public static Vector2 Circular(float x, float y, float lowerDeadZone, float upperDeadZone)
		{
			float num = (float)Math.Sqrt((double)(x * x + y * y));
			if (num < lowerDeadZone || num < 1E-05f)
			{
				return Vector2.zero;
			}
			Vector2 vector = new Vector2(x / num, y / num);
			if (num > upperDeadZone)
			{
				return vector;
			}
			return vector * ((num - lowerDeadZone) / (upperDeadZone - lowerDeadZone));
		}
	}
}
