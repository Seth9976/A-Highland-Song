using System;
using System.Globalization;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Playables
{
	// Token: 0x02000431 RID: 1073
	[NativeHeader("Runtime/Director/Core/FrameRate.h")]
	[UsedByNativeCode("FrameRate")]
	internal struct FrameRate : IEquatable<FrameRate>
	{
		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x06002559 RID: 9561 RVA: 0x0003F06B File Offset: 0x0003D26B
		public bool dropFrame
		{
			get
			{
				return this.m_Rate < 0;
			}
		}

		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x0600255A RID: 9562 RVA: 0x0003F076 File Offset: 0x0003D276
		public double rate
		{
			get
			{
				return this.dropFrame ? ((double)(-(double)this.m_Rate) * 0.999000999000999) : ((double)this.m_Rate);
			}
		}

		// Token: 0x0600255B RID: 9563 RVA: 0x0003F09B File Offset: 0x0003D29B
		public FrameRate(uint frameRate = 0U, bool drop = false)
		{
			this.m_Rate = (int)((drop ? uint.MaxValue : 1U) * frameRate);
		}

		// Token: 0x0600255C RID: 9564 RVA: 0x0003F0B0 File Offset: 0x0003D2B0
		public bool IsValid()
		{
			return this.m_Rate != 0;
		}

		// Token: 0x0600255D RID: 9565 RVA: 0x0003F0CC File Offset: 0x0003D2CC
		public bool Equals(FrameRate other)
		{
			return this.m_Rate == other.m_Rate;
		}

		// Token: 0x0600255E RID: 9566 RVA: 0x0003F0EC File Offset: 0x0003D2EC
		public override bool Equals(object obj)
		{
			return obj is FrameRate && this.Equals((FrameRate)obj);
		}

		// Token: 0x0600255F RID: 9567 RVA: 0x0003F115 File Offset: 0x0003D315
		public static bool operator ==(FrameRate a, FrameRate b)
		{
			return a.Equals(b);
		}

		// Token: 0x06002560 RID: 9568 RVA: 0x0003F11F File Offset: 0x0003D31F
		public static bool operator !=(FrameRate a, FrameRate b)
		{
			return !a.Equals(b);
		}

		// Token: 0x06002561 RID: 9569 RVA: 0x0003F12C File Offset: 0x0003D32C
		public static bool operator <(FrameRate a, FrameRate b)
		{
			return a.rate < b.rate;
		}

		// Token: 0x06002562 RID: 9570 RVA: 0x0003F13E File Offset: 0x0003D33E
		public static bool operator <=(FrameRate a, FrameRate b)
		{
			return a.rate <= b.rate;
		}

		// Token: 0x06002563 RID: 9571 RVA: 0x0003F153 File Offset: 0x0003D353
		public static bool operator >(FrameRate a, FrameRate b)
		{
			return a.rate > b.rate;
		}

		// Token: 0x06002564 RID: 9572 RVA: 0x0003F13E File Offset: 0x0003D33E
		public static bool operator >=(FrameRate a, FrameRate b)
		{
			return a.rate <= b.rate;
		}

		// Token: 0x06002565 RID: 9573 RVA: 0x0003F168 File Offset: 0x0003D368
		public override int GetHashCode()
		{
			return this.m_Rate;
		}

		// Token: 0x06002566 RID: 9574 RVA: 0x0003F180 File Offset: 0x0003D380
		public override string ToString()
		{
			return this.ToString(null, null);
		}

		// Token: 0x06002567 RID: 9575 RVA: 0x0003F19C File Offset: 0x0003D39C
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x06002568 RID: 9576 RVA: 0x0003F1B8 File Offset: 0x0003D3B8
		public string ToString(string format, IFormatProvider formatProvider)
		{
			bool flag = string.IsNullOrEmpty(format);
			if (flag)
			{
				format = (this.dropFrame ? "F2" : "F0");
			}
			bool flag2 = formatProvider == null;
			if (flag2)
			{
				formatProvider = CultureInfo.InvariantCulture.NumberFormat;
			}
			return UnityString.Format("{0} Fps", new object[] { this.rate.ToString(format, formatProvider) });
		}

		// Token: 0x06002569 RID: 9577 RVA: 0x0003F224 File Offset: 0x0003D424
		internal static int FrameRateToInt(FrameRate framerate)
		{
			return framerate.m_Rate;
		}

		// Token: 0x0600256A RID: 9578 RVA: 0x0003F23C File Offset: 0x0003D43C
		internal static FrameRate DoubleToFrameRate(double framerate)
		{
			uint num = (uint)Math.Ceiling(framerate);
			bool flag = num <= 0U;
			FrameRate frameRate;
			if (flag)
			{
				frameRate = new FrameRate(1U, false);
			}
			else
			{
				FrameRate frameRate2 = new FrameRate(num, true);
				bool flag2 = Math.Abs(framerate - frameRate2.rate) < Math.Abs(framerate - num);
				if (flag2)
				{
					frameRate = frameRate2;
				}
				else
				{
					frameRate = new FrameRate(num, false);
				}
			}
			return frameRate;
		}

		// Token: 0x04000DFC RID: 3580
		[Ignore]
		public static readonly FrameRate k_24Fps = new FrameRate(24U, false);

		// Token: 0x04000DFD RID: 3581
		[Ignore]
		public static readonly FrameRate k_23_976Fps = new FrameRate(24U, true);

		// Token: 0x04000DFE RID: 3582
		[Ignore]
		public static readonly FrameRate k_25Fps = new FrameRate(25U, false);

		// Token: 0x04000DFF RID: 3583
		[Ignore]
		public static readonly FrameRate k_30Fps = new FrameRate(30U, false);

		// Token: 0x04000E00 RID: 3584
		[Ignore]
		public static readonly FrameRate k_29_97Fps = new FrameRate(30U, true);

		// Token: 0x04000E01 RID: 3585
		[Ignore]
		public static readonly FrameRate k_50Fps = new FrameRate(50U, false);

		// Token: 0x04000E02 RID: 3586
		[Ignore]
		public static readonly FrameRate k_60Fps = new FrameRate(60U, false);

		// Token: 0x04000E03 RID: 3587
		[Ignore]
		public static readonly FrameRate k_59_94Fps = new FrameRate(60U, true);

		// Token: 0x04000E04 RID: 3588
		[SerializeField]
		private int m_Rate;
	}
}
