using System;
using UnityEngine.Bindings;

namespace UnityEngine.Rendering
{
	// Token: 0x020003AD RID: 941
	[NativeHeader("Runtime/GfxDevice/GfxDeviceTypes.h")]
	public enum CompareFunction
	{
		// Token: 0x04000AB8 RID: 2744
		Disabled,
		// Token: 0x04000AB9 RID: 2745
		Never,
		// Token: 0x04000ABA RID: 2746
		Less,
		// Token: 0x04000ABB RID: 2747
		Equal,
		// Token: 0x04000ABC RID: 2748
		LessEqual,
		// Token: 0x04000ABD RID: 2749
		Greater,
		// Token: 0x04000ABE RID: 2750
		NotEqual,
		// Token: 0x04000ABF RID: 2751
		GreaterEqual,
		// Token: 0x04000AC0 RID: 2752
		Always
	}
}
