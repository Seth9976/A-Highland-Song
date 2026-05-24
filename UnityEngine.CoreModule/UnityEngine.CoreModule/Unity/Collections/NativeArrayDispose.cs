using System;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Collections
{
	// Token: 0x02000095 RID: 149
	[NativeContainer]
	internal struct NativeArrayDispose
	{
		// Token: 0x060002A7 RID: 679 RVA: 0x000050D4 File Offset: 0x000032D4
		public void Dispose()
		{
			UnsafeUtility.Free(this.m_Buffer, this.m_AllocatorLabel);
		}

		// Token: 0x0400022F RID: 559
		[NativeDisableUnsafePtrRestriction]
		internal unsafe void* m_Buffer;

		// Token: 0x04000230 RID: 560
		internal Allocator m_AllocatorLabel;
	}
}
