using System;
using UnityEngine.Bindings;

namespace UnityEngine.Rendering
{
	// Token: 0x020003B0 RID: 944
	[NativeHeader("Runtime/GfxDevice/GfxDeviceTypes.h")]
	public enum StencilOp
	{
		// Token: 0x04000ACC RID: 2764
		Keep,
		// Token: 0x04000ACD RID: 2765
		Zero,
		// Token: 0x04000ACE RID: 2766
		Replace,
		// Token: 0x04000ACF RID: 2767
		IncrementSaturate,
		// Token: 0x04000AD0 RID: 2768
		DecrementSaturate,
		// Token: 0x04000AD1 RID: 2769
		Invert,
		// Token: 0x04000AD2 RID: 2770
		IncrementWrap,
		// Token: 0x04000AD3 RID: 2771
		DecrementWrap
	}
}
