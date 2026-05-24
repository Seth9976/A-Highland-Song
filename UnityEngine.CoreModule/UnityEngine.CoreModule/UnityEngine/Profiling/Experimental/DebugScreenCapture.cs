using System;
using Unity.Collections;

namespace UnityEngine.Profiling.Experimental
{
	// Token: 0x0200027C RID: 636
	public struct DebugScreenCapture
	{
		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x06001BBA RID: 7098 RVA: 0x0002C81F File Offset: 0x0002AA1F
		// (set) Token: 0x06001BBB RID: 7099 RVA: 0x0002C827 File Offset: 0x0002AA27
		public NativeArray<byte> rawImageDataReference { readonly get; set; }

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x06001BBC RID: 7100 RVA: 0x0002C830 File Offset: 0x0002AA30
		// (set) Token: 0x06001BBD RID: 7101 RVA: 0x0002C838 File Offset: 0x0002AA38
		public TextureFormat imageFormat { readonly get; set; }

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x06001BBE RID: 7102 RVA: 0x0002C841 File Offset: 0x0002AA41
		// (set) Token: 0x06001BBF RID: 7103 RVA: 0x0002C849 File Offset: 0x0002AA49
		public int width { readonly get; set; }

		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x06001BC0 RID: 7104 RVA: 0x0002C852 File Offset: 0x0002AA52
		// (set) Token: 0x06001BC1 RID: 7105 RVA: 0x0002C85A File Offset: 0x0002AA5A
		public int height { readonly get; set; }
	}
}
