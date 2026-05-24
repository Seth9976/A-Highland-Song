using System;
using UnityEngine.Rendering;

namespace UnityEngine.Experimental.GlobalIllumination
{
	// Token: 0x02000458 RID: 1112
	public struct LinearColor
	{
		// Token: 0x170007BA RID: 1978
		// (get) Token: 0x060027AC RID: 10156 RVA: 0x00041514 File Offset: 0x0003F714
		// (set) Token: 0x060027AD RID: 10157 RVA: 0x0004152C File Offset: 0x0003F72C
		public float red
		{
			get
			{
				return this.m_red;
			}
			set
			{
				bool flag = value < 0f || value > 1f;
				if (flag)
				{
					throw new ArgumentOutOfRangeException("Red color (" + value.ToString() + ") must be in range [0;1].");
				}
				this.m_red = value;
			}
		}

		// Token: 0x170007BB RID: 1979
		// (get) Token: 0x060027AE RID: 10158 RVA: 0x00041574 File Offset: 0x0003F774
		// (set) Token: 0x060027AF RID: 10159 RVA: 0x0004158C File Offset: 0x0003F78C
		public float green
		{
			get
			{
				return this.m_green;
			}
			set
			{
				bool flag = value < 0f || value > 1f;
				if (flag)
				{
					throw new ArgumentOutOfRangeException("Green color (" + value.ToString() + ") must be in range [0;1].");
				}
				this.m_green = value;
			}
		}

		// Token: 0x170007BC RID: 1980
		// (get) Token: 0x060027B0 RID: 10160 RVA: 0x000415D4 File Offset: 0x0003F7D4
		// (set) Token: 0x060027B1 RID: 10161 RVA: 0x000415EC File Offset: 0x0003F7EC
		public float blue
		{
			get
			{
				return this.m_blue;
			}
			set
			{
				bool flag = value < 0f || value > 1f;
				if (flag)
				{
					throw new ArgumentOutOfRangeException("Blue color (" + value.ToString() + ") must be in range [0;1].");
				}
				this.m_blue = value;
			}
		}

		// Token: 0x170007BD RID: 1981
		// (get) Token: 0x060027B2 RID: 10162 RVA: 0x00041634 File Offset: 0x0003F834
		// (set) Token: 0x060027B3 RID: 10163 RVA: 0x0004164C File Offset: 0x0003F84C
		public float intensity
		{
			get
			{
				return this.m_intensity;
			}
			set
			{
				bool flag = value < 0f;
				if (flag)
				{
					throw new ArgumentOutOfRangeException("Intensity (" + value.ToString() + ") must be positive.");
				}
				this.m_intensity = value;
			}
		}

		// Token: 0x060027B4 RID: 10164 RVA: 0x0004168C File Offset: 0x0003F88C
		public static LinearColor Convert(Color color, float intensity)
		{
			Color color2 = (GraphicsSettings.lightsUseLinearIntensity ? color.linear.RGBMultiplied(intensity) : color.RGBMultiplied(intensity).linear);
			float maxColorComponent = color2.maxColorComponent;
			bool flag = color2.r < 0f || color2.g < 0f || color2.b < 0f;
			if (flag)
			{
				throw new ArgumentOutOfRangeException(string.Concat(new string[]
				{
					"The input color to be converted must not contain negative values (red: ",
					color2.r.ToString(),
					", green: ",
					color2.g.ToString(),
					", blue: ",
					color2.b.ToString(),
					")."
				}));
			}
			bool flag2 = maxColorComponent <= 1E-20f;
			LinearColor linearColor;
			if (flag2)
			{
				linearColor = LinearColor.Black();
			}
			else
			{
				float num = 1f / color2.maxColorComponent;
				LinearColor linearColor2;
				linearColor2.m_red = color2.r * num;
				linearColor2.m_green = color2.g * num;
				linearColor2.m_blue = color2.b * num;
				linearColor2.m_intensity = maxColorComponent;
				linearColor = linearColor2;
			}
			return linearColor;
		}

		// Token: 0x060027B5 RID: 10165 RVA: 0x000417C0 File Offset: 0x0003F9C0
		public static LinearColor Black()
		{
			LinearColor linearColor;
			linearColor.m_red = (linearColor.m_green = (linearColor.m_blue = (linearColor.m_intensity = 0f)));
			return linearColor;
		}

		// Token: 0x04000E5C RID: 3676
		private float m_red;

		// Token: 0x04000E5D RID: 3677
		private float m_green;

		// Token: 0x04000E5E RID: 3678
		private float m_blue;

		// Token: 0x04000E5F RID: 3679
		private float m_intensity;
	}
}
