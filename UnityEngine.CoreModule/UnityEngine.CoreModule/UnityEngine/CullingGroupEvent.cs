using System;

namespace UnityEngine
{
	// Token: 0x02000101 RID: 257
	public struct CullingGroupEvent
	{
		// Token: 0x17000130 RID: 304
		// (get) Token: 0x060005AB RID: 1451 RVA: 0x00007FDC File Offset: 0x000061DC
		public int index
		{
			get
			{
				return this.m_Index;
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060005AC RID: 1452 RVA: 0x00007FF4 File Offset: 0x000061F4
		public bool isVisible
		{
			get
			{
				return (this.m_ThisState & 128) > 0;
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060005AD RID: 1453 RVA: 0x00008018 File Offset: 0x00006218
		public bool wasVisible
		{
			get
			{
				return (this.m_PrevState & 128) > 0;
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060005AE RID: 1454 RVA: 0x0000803C File Offset: 0x0000623C
		public bool hasBecomeVisible
		{
			get
			{
				return this.isVisible && !this.wasVisible;
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060005AF RID: 1455 RVA: 0x00008064 File Offset: 0x00006264
		public bool hasBecomeInvisible
		{
			get
			{
				return !this.isVisible && this.wasVisible;
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060005B0 RID: 1456 RVA: 0x00008088 File Offset: 0x00006288
		public int currentDistance
		{
			get
			{
				return (int)(this.m_ThisState & 127);
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060005B1 RID: 1457 RVA: 0x000080A4 File Offset: 0x000062A4
		public int previousDistance
		{
			get
			{
				return (int)(this.m_PrevState & 127);
			}
		}

		// Token: 0x0400036B RID: 875
		private int m_Index;

		// Token: 0x0400036C RID: 876
		private byte m_PrevState;

		// Token: 0x0400036D RID: 877
		private byte m_ThisState;

		// Token: 0x0400036E RID: 878
		private const byte kIsVisibleMask = 128;

		// Token: 0x0400036F RID: 879
		private const byte kDistanceMask = 127;
	}
}
