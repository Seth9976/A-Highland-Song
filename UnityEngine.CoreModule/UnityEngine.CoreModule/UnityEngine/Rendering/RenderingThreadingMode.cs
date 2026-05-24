using System;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Rendering
{
	// Token: 0x020003DA RID: 986
	[MovedFrom("UnityEngine.Experimental.Rendering")]
	public enum RenderingThreadingMode
	{
		// Token: 0x04000C10 RID: 3088
		Direct,
		// Token: 0x04000C11 RID: 3089
		SingleThreaded,
		// Token: 0x04000C12 RID: 3090
		MultiThreaded,
		// Token: 0x04000C13 RID: 3091
		LegacyJobified,
		// Token: 0x04000C14 RID: 3092
		NativeGraphicsJobs,
		// Token: 0x04000C15 RID: 3093
		NativeGraphicsJobsWithoutRenderThread
	}
}
