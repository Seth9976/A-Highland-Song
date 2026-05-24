using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Windows
{
	// Token: 0x02000287 RID: 647
	public static class CrashReporting
	{
		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x06001C1C RID: 7196
		public static extern string crashReportFolder
		{
			[NativeHeader("PlatformDependent/WinPlayer/Bindings/CrashReportingBindings.h")]
			[ThreadSafe]
			[MethodImpl(4096)]
			get;
		}
	}
}
