using System;

namespace UnityEngine
{
	// Token: 0x02000210 RID: 528
	public struct RangeInt
	{
		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x06001744 RID: 5956 RVA: 0x00025668 File Offset: 0x00023868
		public int end
		{
			get
			{
				return this.start + this.length;
			}
		}

		// Token: 0x06001745 RID: 5957 RVA: 0x00025687 File Offset: 0x00023887
		public RangeInt(int start, int length)
		{
			this.start = start;
			this.length = length;
		}

		// Token: 0x040007F8 RID: 2040
		public int start;

		// Token: 0x040007F9 RID: 2041
		public int length;
	}
}
