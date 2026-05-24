using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Diagnostics
{
	// Token: 0x0200044D RID: 1101
	[NativeHeader("Runtime/Export/Diagnostics/DiagnosticsUtils.bindings.h")]
	public static class Utils
	{
		// Token: 0x060026DD RID: 9949
		[FreeFunction("DiagnosticsUtils_Bindings::ForceCrash", ThrowsException = true)]
		[MethodImpl(4096)]
		public static extern void ForceCrash(ForcedCrashCategory crashCategory);

		// Token: 0x060026DE RID: 9950
		[FreeFunction("DiagnosticsUtils_Bindings::NativeAssert")]
		[MethodImpl(4096)]
		public static extern void NativeAssert(string message);

		// Token: 0x060026DF RID: 9951
		[FreeFunction("DiagnosticsUtils_Bindings::NativeError")]
		[MethodImpl(4096)]
		public static extern void NativeError(string message);

		// Token: 0x060026E0 RID: 9952
		[FreeFunction("DiagnosticsUtils_Bindings::NativeWarning")]
		[MethodImpl(4096)]
		public static extern void NativeWarning(string message);
	}
}
