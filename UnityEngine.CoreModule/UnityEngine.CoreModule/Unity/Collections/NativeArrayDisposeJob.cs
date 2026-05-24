using System;
using Unity.Jobs;

namespace Unity.Collections
{
	// Token: 0x02000096 RID: 150
	internal struct NativeArrayDisposeJob : IJob
	{
		// Token: 0x060002A8 RID: 680 RVA: 0x000050E9 File Offset: 0x000032E9
		public void Execute()
		{
			this.Data.Dispose();
		}

		// Token: 0x04000231 RID: 561
		internal NativeArrayDispose Data;
	}
}
