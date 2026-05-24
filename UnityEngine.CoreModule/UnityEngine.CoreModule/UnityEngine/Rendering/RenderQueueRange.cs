using System;

namespace UnityEngine.Rendering
{
	// Token: 0x02000407 RID: 1031
	public struct RenderQueueRange : IEquatable<RenderQueueRange>
	{
		// Token: 0x06002330 RID: 9008 RVA: 0x0003B3DC File Offset: 0x000395DC
		public RenderQueueRange(int lowerBound, int upperBound)
		{
			bool flag = lowerBound < 0 || lowerBound > 5000;
			if (flag)
			{
				throw new ArgumentOutOfRangeException("lowerBound", lowerBound, string.Format("The lower bound must be at least {0} and at most {1}.", 0, 5000));
			}
			bool flag2 = upperBound < 0 || upperBound > 5000;
			if (flag2)
			{
				throw new ArgumentOutOfRangeException("upperBound", upperBound, string.Format("The upper bound must be at least {0} and at most {1}.", 0, 5000));
			}
			this.m_LowerBound = lowerBound;
			this.m_UpperBound = upperBound;
		}

		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x06002331 RID: 9009 RVA: 0x0003B474 File Offset: 0x00039674
		public static RenderQueueRange all
		{
			get
			{
				return new RenderQueueRange
				{
					m_LowerBound = 0,
					m_UpperBound = 5000
				};
			}
		}

		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x06002332 RID: 9010 RVA: 0x0003B4A0 File Offset: 0x000396A0
		public static RenderQueueRange opaque
		{
			get
			{
				return new RenderQueueRange
				{
					m_LowerBound = 0,
					m_UpperBound = 2500
				};
			}
		}

		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x06002333 RID: 9011 RVA: 0x0003B4CC File Offset: 0x000396CC
		public static RenderQueueRange transparent
		{
			get
			{
				return new RenderQueueRange
				{
					m_LowerBound = 2501,
					m_UpperBound = 5000
				};
			}
		}

		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x06002334 RID: 9012 RVA: 0x0003B4FC File Offset: 0x000396FC
		// (set) Token: 0x06002335 RID: 9013 RVA: 0x0003B514 File Offset: 0x00039714
		public int lowerBound
		{
			get
			{
				return this.m_LowerBound;
			}
			set
			{
				bool flag = value < 0 || value > 5000;
				if (flag)
				{
					throw new ArgumentOutOfRangeException(string.Format("The lower bound must be at least {0} and at most {1}.", 0, 5000));
				}
				this.m_LowerBound = value;
			}
		}

		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x06002336 RID: 9014 RVA: 0x0003B55C File Offset: 0x0003975C
		// (set) Token: 0x06002337 RID: 9015 RVA: 0x0003B574 File Offset: 0x00039774
		public int upperBound
		{
			get
			{
				return this.m_UpperBound;
			}
			set
			{
				bool flag = value < 0 || value > 5000;
				if (flag)
				{
					throw new ArgumentOutOfRangeException(string.Format("The upper bound must be at least {0} and at most {1}.", 0, 5000));
				}
				this.m_UpperBound = value;
			}
		}

		// Token: 0x06002338 RID: 9016 RVA: 0x0003B5BC File Offset: 0x000397BC
		public bool Equals(RenderQueueRange other)
		{
			return this.m_LowerBound == other.m_LowerBound && this.m_UpperBound == other.m_UpperBound;
		}

		// Token: 0x06002339 RID: 9017 RVA: 0x0003B5F0 File Offset: 0x000397F0
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			return !flag && obj is RenderQueueRange && this.Equals((RenderQueueRange)obj);
		}

		// Token: 0x0600233A RID: 9018 RVA: 0x0003B628 File Offset: 0x00039828
		public override int GetHashCode()
		{
			return (this.m_LowerBound * 397) ^ this.m_UpperBound;
		}

		// Token: 0x0600233B RID: 9019 RVA: 0x0003B650 File Offset: 0x00039850
		public static bool operator ==(RenderQueueRange left, RenderQueueRange right)
		{
			return left.Equals(right);
		}

		// Token: 0x0600233C RID: 9020 RVA: 0x0003B66C File Offset: 0x0003986C
		public static bool operator !=(RenderQueueRange left, RenderQueueRange right)
		{
			return !left.Equals(right);
		}

		// Token: 0x04000D14 RID: 3348
		private int m_LowerBound;

		// Token: 0x04000D15 RID: 3349
		private int m_UpperBound;

		// Token: 0x04000D16 RID: 3350
		private const int k_MinimumBound = 0;

		// Token: 0x04000D17 RID: 3351
		public static readonly int minimumBound = 0;

		// Token: 0x04000D18 RID: 3352
		private const int k_MaximumBound = 5000;

		// Token: 0x04000D19 RID: 3353
		public static readonly int maximumBound = 5000;
	}
}
