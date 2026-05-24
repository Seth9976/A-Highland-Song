using System;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.Rendering
{
	// Token: 0x02000471 RID: 1137
	[RequiredByNativeCode]
	internal class ScriptableRuntimeReflectionSystemWrapper
	{
		// Token: 0x170007C3 RID: 1987
		// (get) Token: 0x06002825 RID: 10277 RVA: 0x00042CF3 File Offset: 0x00040EF3
		// (set) Token: 0x06002826 RID: 10278 RVA: 0x00042CFB File Offset: 0x00040EFB
		internal IScriptableRuntimeReflectionSystem implementation { get; set; }

		// Token: 0x06002827 RID: 10279 RVA: 0x00042D04 File Offset: 0x00040F04
		[RequiredByNativeCode]
		private void Internal_ScriptableRuntimeReflectionSystemWrapper_TickRealtimeProbes(out bool result)
		{
			result = this.implementation != null && this.implementation.TickRealtimeProbes();
		}
	}
}
