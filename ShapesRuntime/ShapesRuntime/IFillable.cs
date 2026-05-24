using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shapes
{
	// Token: 0x0200002A RID: 42
	internal interface IFillable
	{
		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000983 RID: 2435
		List<float> fillType { get; }

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000984 RID: 2436
		List<float> fillSpace { get; }

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000985 RID: 2437
		List<Vector4> fillStart { get; }

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000986 RID: 2438
		List<Vector4> fillEnd { get; }

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000987 RID: 2439
		List<Vector4> fillColorEnd { get; }
	}
}
