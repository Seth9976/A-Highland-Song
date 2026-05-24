using System;

namespace UnityEngine.Profiling.Memory.Experimental
{
	// Token: 0x0200027D RID: 637
	[Flags]
	public enum CaptureFlags : uint
	{
		// Token: 0x04000915 RID: 2325
		ManagedObjects = 1U,
		// Token: 0x04000916 RID: 2326
		NativeObjects = 2U,
		// Token: 0x04000917 RID: 2327
		NativeAllocations = 4U,
		// Token: 0x04000918 RID: 2328
		NativeAllocationSites = 8U,
		// Token: 0x04000919 RID: 2329
		NativeStackTraces = 16U
	}
}
