using System;

namespace UnityEngine
{
	// Token: 0x020001F0 RID: 496
	[AttributeUsage(4, AllowMultiple = false, Inherited = false)]
	public sealed class CreateAssetMenuAttribute : Attribute
	{
		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x06001651 RID: 5713 RVA: 0x00023D08 File Offset: 0x00021F08
		// (set) Token: 0x06001652 RID: 5714 RVA: 0x00023D10 File Offset: 0x00021F10
		public string menuName { get; set; }

		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x06001653 RID: 5715 RVA: 0x00023D19 File Offset: 0x00021F19
		// (set) Token: 0x06001654 RID: 5716 RVA: 0x00023D21 File Offset: 0x00021F21
		public string fileName { get; set; }

		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x06001655 RID: 5717 RVA: 0x00023D2A File Offset: 0x00021F2A
		// (set) Token: 0x06001656 RID: 5718 RVA: 0x00023D32 File Offset: 0x00021F32
		public int order { get; set; }
	}
}
