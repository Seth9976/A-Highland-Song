using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Experimental.Rendering
{
	// Token: 0x02000472 RID: 1138
	[NativeHeader("Runtime/Graphics/GraphicsScriptBindings.h")]
	public static class ExternalGPUProfiler
	{
		// Token: 0x06002829 RID: 10281
		[FreeFunction("ExternalGPUProfilerBindings::BeginGPUCapture")]
		[MethodImpl(4096)]
		public static extern void BeginGPUCapture();

		// Token: 0x0600282A RID: 10282
		[FreeFunction("ExternalGPUProfilerBindings::EndGPUCapture")]
		[MethodImpl(4096)]
		public static extern void EndGPUCapture();

		// Token: 0x0600282B RID: 10283
		[FreeFunction("ExternalGPUProfilerBindings::IsAttached")]
		[MethodImpl(4096)]
		public static extern bool IsAttached();
	}
}
