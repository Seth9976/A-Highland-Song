using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000257 RID: 599
	[NativeHeader("Runtime/Profiler/PerformanceTools/FrameDebugger.h")]
	[StaticAccessor("FrameDebugger", StaticAccessorType.DoubleColon)]
	public static class FrameDebugger
	{
		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x060019F3 RID: 6643 RVA: 0x00029FD2 File Offset: 0x000281D2
		public static bool enabled
		{
			get
			{
				return FrameDebugger.IsLocalEnabled() || FrameDebugger.IsRemoteEnabled();
			}
		}

		// Token: 0x060019F4 RID: 6644
		[MethodImpl(4096)]
		internal static extern bool IsLocalEnabled();

		// Token: 0x060019F5 RID: 6645
		[MethodImpl(4096)]
		internal static extern bool IsRemoteEnabled();
	}
}
