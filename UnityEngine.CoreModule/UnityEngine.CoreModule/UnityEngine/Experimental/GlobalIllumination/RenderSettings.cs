using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Experimental.GlobalIllumination
{
	// Token: 0x02000466 RID: 1126
	[NativeHeader("Runtime/Camera/RenderSettings.h")]
	[StaticAccessor("GetRenderSettings()", StaticAccessorType.Dot)]
	public class RenderSettings
	{
		// Token: 0x170007BE RID: 1982
		// (get) Token: 0x060027DD RID: 10205
		// (set) Token: 0x060027DE RID: 10206
		public static extern bool useRadianceAmbientProbe
		{
			[MethodImpl(4096)]
			get;
			[MethodImpl(4096)]
			set;
		}
	}
}
