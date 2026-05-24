using System;
using UnityEngine.Bindings;

namespace UnityEngine.Rendering
{
	// Token: 0x020003AB RID: 939
	[NativeHeader("Runtime/GfxDevice/GfxDeviceTypes.h")]
	public enum BlendMode
	{
		// Token: 0x04000A87 RID: 2695
		Zero,
		// Token: 0x04000A88 RID: 2696
		One,
		// Token: 0x04000A89 RID: 2697
		DstColor,
		// Token: 0x04000A8A RID: 2698
		SrcColor,
		// Token: 0x04000A8B RID: 2699
		OneMinusDstColor,
		// Token: 0x04000A8C RID: 2700
		SrcAlpha,
		// Token: 0x04000A8D RID: 2701
		OneMinusSrcColor,
		// Token: 0x04000A8E RID: 2702
		DstAlpha,
		// Token: 0x04000A8F RID: 2703
		OneMinusDstAlpha,
		// Token: 0x04000A90 RID: 2704
		SrcAlphaSaturate,
		// Token: 0x04000A91 RID: 2705
		OneMinusSrcAlpha
	}
}
