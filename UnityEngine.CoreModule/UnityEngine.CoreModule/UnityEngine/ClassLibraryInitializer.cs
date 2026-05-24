using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x020001FC RID: 508
	internal static class ClassLibraryInitializer
	{
		// Token: 0x0600166A RID: 5738 RVA: 0x00023DF4 File Offset: 0x00021FF4
		[RequiredByNativeCode]
		private static void Init()
		{
			UnityLogWriter.Init();
		}
	}
}
