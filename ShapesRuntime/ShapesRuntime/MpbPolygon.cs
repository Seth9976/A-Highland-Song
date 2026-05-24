using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000027 RID: 39
	internal class MpbPolygon : MetaMpb, IFillable
	{
		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000973 RID: 2419 RVA: 0x000229F1 File Offset: 0x00020BF1
		List<float> IFillable.fillType { get; } = MetaMpb.InitList<float>();

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000974 RID: 2420 RVA: 0x000229F9 File Offset: 0x00020BF9
		List<float> IFillable.fillSpace { get; } = MetaMpb.InitList<float>();

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000975 RID: 2421 RVA: 0x00022A01 File Offset: 0x00020C01
		List<Vector4> IFillable.fillStart { get; } = MetaMpb.InitList<Vector4>();

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000976 RID: 2422 RVA: 0x00022A09 File Offset: 0x00020C09
		List<Vector4> IFillable.fillEnd { get; } = MetaMpb.InitList<Vector4>();

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000977 RID: 2423 RVA: 0x00022A11 File Offset: 0x00020C11
		List<Vector4> IFillable.fillColorEnd { get; } = MetaMpb.InitList<Vector4>();

		// Token: 0x06000978 RID: 2424 RVA: 0x00022A19 File Offset: 0x00020C19
		protected override void TransferShapeProperties()
		{
		}
	}
}
