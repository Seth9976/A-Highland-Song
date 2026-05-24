using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.U2D
{
	// Token: 0x02000270 RID: 624
	[MovedFrom("UnityEngine.Experimental.U2D")]
	[NativeHeader("Runtime/2D/Common/PixelSnapping.h")]
	public static class PixelPerfectRendering
	{
		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x06001B13 RID: 6931
		// (set) Token: 0x06001B14 RID: 6932
		public static extern float pixelSnapSpacing
		{
			[FreeFunction("GetPixelSnapSpacing")]
			[MethodImpl(4096)]
			get;
			[FreeFunction("SetPixelSnapSpacing")]
			[MethodImpl(4096)]
			set;
		}
	}
}
