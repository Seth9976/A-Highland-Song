using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Rendering
{
	// Token: 0x0200039E RID: 926
	[NativeHeader("Runtime/Graphics/GraphicsScriptBindings.h")]
	public class PIX
	{
		// Token: 0x06001F46 RID: 8006
		[FreeFunction("PIX::BeginGPUCapture")]
		[MethodImpl(4096)]
		public static extern void BeginGPUCapture();

		// Token: 0x06001F47 RID: 8007
		[FreeFunction("PIX::EndGPUCapture")]
		[MethodImpl(4096)]
		public static extern void EndGPUCapture();

		// Token: 0x06001F48 RID: 8008
		[FreeFunction("PIX::IsAttached")]
		[MethodImpl(4096)]
		public static extern bool IsAttached();
	}
}
