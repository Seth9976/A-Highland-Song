using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000137 RID: 311
	[RequiredByNativeCode]
	public struct Resolution
	{
		// Token: 0x1700020D RID: 525
		// (get) Token: 0x060009E0 RID: 2528 RVA: 0x0000ED80 File Offset: 0x0000CF80
		// (set) Token: 0x060009E1 RID: 2529 RVA: 0x0000ED98 File Offset: 0x0000CF98
		public int width
		{
			get
			{
				return this.m_Width;
			}
			set
			{
				this.m_Width = value;
			}
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x060009E2 RID: 2530 RVA: 0x0000EDA4 File Offset: 0x0000CFA4
		// (set) Token: 0x060009E3 RID: 2531 RVA: 0x0000EDBC File Offset: 0x0000CFBC
		public int height
		{
			get
			{
				return this.m_Height;
			}
			set
			{
				this.m_Height = value;
			}
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x060009E4 RID: 2532 RVA: 0x0000EDC8 File Offset: 0x0000CFC8
		// (set) Token: 0x060009E5 RID: 2533 RVA: 0x0000EDE0 File Offset: 0x0000CFE0
		public int refreshRate
		{
			get
			{
				return this.m_RefreshRate;
			}
			set
			{
				this.m_RefreshRate = value;
			}
		}

		// Token: 0x060009E6 RID: 2534 RVA: 0x0000EDEC File Offset: 0x0000CFEC
		public override string ToString()
		{
			return UnityString.Format("{0} x {1} @ {2}Hz", new object[] { this.m_Width, this.m_Height, this.m_RefreshRate });
		}

		// Token: 0x040003E3 RID: 995
		private int m_Width;

		// Token: 0x040003E4 RID: 996
		private int m_Height;

		// Token: 0x040003E5 RID: 997
		private int m_RefreshRate;
	}
}
