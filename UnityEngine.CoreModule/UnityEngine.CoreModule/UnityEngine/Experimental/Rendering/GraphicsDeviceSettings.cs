using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Experimental.Rendering
{
	// Token: 0x02000475 RID: 1141
	public static class GraphicsDeviceSettings
	{
		// Token: 0x170007C4 RID: 1988
		// (get) Token: 0x0600282C RID: 10284
		// (set) Token: 0x0600282D RID: 10285
		[StaticAccessor("GetGfxDevice()", StaticAccessorType.Dot)]
		public static extern WaitForPresentSyncPoint waitForPresentSyncPoint
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}

		// Token: 0x170007C5 RID: 1989
		// (get) Token: 0x0600282E RID: 10286
		// (set) Token: 0x0600282F RID: 10287
		[StaticAccessor("GetGfxDevice()", StaticAccessorType.Dot)]
		public static extern GraphicsJobsSyncPoint graphicsJobsSyncPoint
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}
	}
}
