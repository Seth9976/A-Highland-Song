using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000214 RID: 532
	[NativeHeader("Runtime/Export/Scripting/ScriptingRuntime.h")]
	[VisibleToOtherModules]
	internal class ScriptingRuntime
	{
		// Token: 0x06001754 RID: 5972
		[MethodImpl(4096)]
		public static extern string[] GetAllUserAssemblies();
	}
}
