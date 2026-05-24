using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.Rendering
{
	// Token: 0x0200046D RID: 1133
	[NativeHeader("Runtime/Camera/ReflectionProbes.h")]
	internal class BuiltinRuntimeReflectionSystem : IScriptableRuntimeReflectionSystem, IDisposable
	{
		// Token: 0x06002813 RID: 10259 RVA: 0x00042B6C File Offset: 0x00040D6C
		public bool TickRealtimeProbes()
		{
			return BuiltinRuntimeReflectionSystem.BuiltinUpdate();
		}

		// Token: 0x06002814 RID: 10260 RVA: 0x00042B83 File Offset: 0x00040D83
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06002815 RID: 10261 RVA: 0x00004557 File Offset: 0x00002757
		private void Dispose(bool disposing)
		{
		}

		// Token: 0x06002816 RID: 10262
		[StaticAccessor("GetReflectionProbes()", Type = StaticAccessorType.Dot)]
		[MethodImpl(4096)]
		private static extern bool BuiltinUpdate();

		// Token: 0x06002817 RID: 10263 RVA: 0x00042B90 File Offset: 0x00040D90
		[RequiredByNativeCode]
		private static BuiltinRuntimeReflectionSystem Internal_BuiltinRuntimeReflectionSystem_New()
		{
			return new BuiltinRuntimeReflectionSystem();
		}
	}
}
