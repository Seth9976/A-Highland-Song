using System;
using System.ComponentModel;

namespace Shapes
{
	// Token: 0x02000056 RID: 86
	public enum ShapesBlendMode
	{
		// Token: 0x040001B2 RID: 434
		[Description("Opaque")]
		Opaque,
		// Token: 0x040001B3 RID: 435
		[Description("Transparent_")]
		Transparent,
		// Token: 0x040001B4 RID: 436
		[Description("Linear Dodge (Additive)")]
		Additive,
		// Token: 0x040001B5 RID: 437
		[Description("Color Dodge")]
		ColorDodge = 9,
		// Token: 0x040001B6 RID: 438
		[Description("Screen")]
		Screen = 4,
		// Token: 0x040001B7 RID: 439
		[Description("Lighten_")]
		Lighten = 7,
		// Token: 0x040001B8 RID: 440
		[Description("Linear Burn")]
		LinearBurn = 6,
		// Token: 0x040001B9 RID: 441
		[Description("Color Burn")]
		ColorBurn = 10,
		// Token: 0x040001BA RID: 442
		[Description("Multiply")]
		Multiplicative = 3,
		// Token: 0x040001BB RID: 443
		[Description("Darken_")]
		Darken = 8,
		// Token: 0x040001BC RID: 444
		[Description("Subtract")]
		Subtractive = 5
	}
}
