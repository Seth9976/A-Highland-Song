using System;
using System.Collections.Generic;

namespace Shapes
{
	// Token: 0x0200002B RID: 43
	internal interface IDashable
	{
		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000988 RID: 2440
		List<float> dashSize { get; }

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000989 RID: 2441
		List<float> dashType { get; }

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x0600098A RID: 2442
		List<float> dashShapeModifier { get; }

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x0600098B RID: 2443
		List<float> dashSpace { get; }

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x0600098C RID: 2444
		List<float> dashSnap { get; }

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x0600098D RID: 2445
		List<float> dashOffset { get; }

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x0600098E RID: 2446
		List<float> dashSpacing { get; }
	}
}
