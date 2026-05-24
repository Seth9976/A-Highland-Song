using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000177 RID: 375
	[NativeHeader("Runtime/Graphics/ColorGamut.h")]
	[UsedByNativeCode]
	public enum ColorGamut
	{
		// Token: 0x040004C7 RID: 1223
		sRGB,
		// Token: 0x040004C8 RID: 1224
		Rec709,
		// Token: 0x040004C9 RID: 1225
		Rec2020,
		// Token: 0x040004CA RID: 1226
		DisplayP3,
		// Token: 0x040004CB RID: 1227
		HDR10,
		// Token: 0x040004CC RID: 1228
		DolbyHDR
	}
}
