using System;
using UnityEngine.Bindings;

namespace UnityEngine.U2D
{
	// Token: 0x02000272 RID: 626
	[VisibleToOtherModules]
	internal struct SpriteChannelInfo
	{
		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x06001B23 RID: 6947 RVA: 0x0002B7E8 File Offset: 0x000299E8
		// (set) Token: 0x06001B24 RID: 6948 RVA: 0x0002B805 File Offset: 0x00029A05
		public unsafe void* buffer
		{
			get
			{
				return (void*)this.m_Buffer;
			}
			set
			{
				this.m_Buffer = (IntPtr)value;
			}
		}

		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x06001B25 RID: 6949 RVA: 0x0002B814 File Offset: 0x00029A14
		// (set) Token: 0x06001B26 RID: 6950 RVA: 0x0002B82C File Offset: 0x00029A2C
		public int count
		{
			get
			{
				return this.m_Count;
			}
			set
			{
				this.m_Count = value;
			}
		}

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x06001B27 RID: 6951 RVA: 0x0002B838 File Offset: 0x00029A38
		// (set) Token: 0x06001B28 RID: 6952 RVA: 0x0002B850 File Offset: 0x00029A50
		public int offset
		{
			get
			{
				return this.m_Offset;
			}
			set
			{
				this.m_Offset = value;
			}
		}

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x06001B29 RID: 6953 RVA: 0x0002B85C File Offset: 0x00029A5C
		// (set) Token: 0x06001B2A RID: 6954 RVA: 0x0002B874 File Offset: 0x00029A74
		public int stride
		{
			get
			{
				return this.m_Stride;
			}
			set
			{
				this.m_Stride = value;
			}
		}

		// Token: 0x040008F3 RID: 2291
		[NativeName("buffer")]
		private IntPtr m_Buffer;

		// Token: 0x040008F4 RID: 2292
		[NativeName("count")]
		private int m_Count;

		// Token: 0x040008F5 RID: 2293
		[NativeName("offset")]
		private int m_Offset;

		// Token: 0x040008F6 RID: 2294
		[NativeName("stride")]
		private int m_Stride;
	}
}
