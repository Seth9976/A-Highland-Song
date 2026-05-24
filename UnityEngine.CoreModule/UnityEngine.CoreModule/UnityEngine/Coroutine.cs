using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x020001FE RID: 510
	[NativeHeader("Runtime/Mono/Coroutine.h")]
	[RequiredByNativeCode]
	[StructLayout(0)]
	public sealed class Coroutine : YieldInstruction
	{
		// Token: 0x0600169B RID: 5787 RVA: 0x000241F4 File Offset: 0x000223F4
		private Coroutine()
		{
		}

		// Token: 0x0600169C RID: 5788 RVA: 0x00024200 File Offset: 0x00022400
		~Coroutine()
		{
			Coroutine.ReleaseCoroutine(this.m_Ptr);
		}

		// Token: 0x0600169D RID: 5789
		[FreeFunction("Coroutine::CleanupCoroutineGC", true)]
		[MethodImpl(4096)]
		private static extern void ReleaseCoroutine(IntPtr ptr);

		// Token: 0x040007DC RID: 2012
		internal IntPtr m_Ptr;
	}
}
