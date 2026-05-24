using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Windows.WebCam
{
	// Token: 0x020002B1 RID: 689
	[StaticAccessor("WebCam::GetInstance()", StaticAccessorType.Dot)]
	[NativeHeader("PlatformDependent/Win/Webcam/WebCam.h")]
	[MovedFrom("UnityEngine.XR.WSA.WebCam")]
	public class WebCam
	{
		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x06001CFD RID: 7421
		public static extern WebCamMode Mode
		{
			[NativeName("GetWebCamMode")]
			[NativeConditional("(PLATFORM_WIN || PLATFORM_WINRT) && !PLATFORM_XBOXONE")]
			[MethodImpl(4096)]
			get;
		}
	}
}
