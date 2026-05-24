using System;
using UnityEngine.Bindings;

namespace UnityEngine.Rendering
{
	// Token: 0x020003AC RID: 940
	[NativeHeader("Runtime/GfxDevice/GfxDeviceTypes.h")]
	public enum BlendOp
	{
		// Token: 0x04000A93 RID: 2707
		Add,
		// Token: 0x04000A94 RID: 2708
		Subtract,
		// Token: 0x04000A95 RID: 2709
		ReverseSubtract,
		// Token: 0x04000A96 RID: 2710
		Min,
		// Token: 0x04000A97 RID: 2711
		Max,
		// Token: 0x04000A98 RID: 2712
		LogicalClear,
		// Token: 0x04000A99 RID: 2713
		LogicalSet,
		// Token: 0x04000A9A RID: 2714
		LogicalCopy,
		// Token: 0x04000A9B RID: 2715
		LogicalCopyInverted,
		// Token: 0x04000A9C RID: 2716
		LogicalNoop,
		// Token: 0x04000A9D RID: 2717
		LogicalInvert,
		// Token: 0x04000A9E RID: 2718
		LogicalAnd,
		// Token: 0x04000A9F RID: 2719
		LogicalNand,
		// Token: 0x04000AA0 RID: 2720
		LogicalOr,
		// Token: 0x04000AA1 RID: 2721
		LogicalNor,
		// Token: 0x04000AA2 RID: 2722
		LogicalXor,
		// Token: 0x04000AA3 RID: 2723
		LogicalEquivalence,
		// Token: 0x04000AA4 RID: 2724
		LogicalAndReverse,
		// Token: 0x04000AA5 RID: 2725
		LogicalAndInverted,
		// Token: 0x04000AA6 RID: 2726
		LogicalOrReverse,
		// Token: 0x04000AA7 RID: 2727
		LogicalOrInverted,
		// Token: 0x04000AA8 RID: 2728
		Multiply,
		// Token: 0x04000AA9 RID: 2729
		Screen,
		// Token: 0x04000AAA RID: 2730
		Overlay,
		// Token: 0x04000AAB RID: 2731
		Darken,
		// Token: 0x04000AAC RID: 2732
		Lighten,
		// Token: 0x04000AAD RID: 2733
		ColorDodge,
		// Token: 0x04000AAE RID: 2734
		ColorBurn,
		// Token: 0x04000AAF RID: 2735
		HardLight,
		// Token: 0x04000AB0 RID: 2736
		SoftLight,
		// Token: 0x04000AB1 RID: 2737
		Difference,
		// Token: 0x04000AB2 RID: 2738
		Exclusion,
		// Token: 0x04000AB3 RID: 2739
		HSLHue,
		// Token: 0x04000AB4 RID: 2740
		HSLSaturation,
		// Token: 0x04000AB5 RID: 2741
		HSLColor,
		// Token: 0x04000AB6 RID: 2742
		HSLLuminosity
	}
}
