using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000025 RID: 37
	internal class MpbLine : MetaMpb, IDashable
	{
		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000968 RID: 2408 RVA: 0x00022800 File Offset: 0x00020A00
		List<float> IDashable.dashSize { get; } = MetaMpb.InitList<float>();

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000969 RID: 2409 RVA: 0x00022808 File Offset: 0x00020A08
		List<float> IDashable.dashType { get; } = MetaMpb.InitList<float>();

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x0600096A RID: 2410 RVA: 0x00022810 File Offset: 0x00020A10
		List<float> IDashable.dashShapeModifier { get; } = MetaMpb.InitList<float>();

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x0600096B RID: 2411 RVA: 0x00022818 File Offset: 0x00020A18
		List<float> IDashable.dashSpace { get; } = MetaMpb.InitList<float>();

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x0600096C RID: 2412 RVA: 0x00022820 File Offset: 0x00020A20
		List<float> IDashable.dashSnap { get; } = MetaMpb.InitList<float>();

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x0600096D RID: 2413 RVA: 0x00022828 File Offset: 0x00020A28
		List<float> IDashable.dashOffset { get; } = MetaMpb.InitList<float>();

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x0600096E RID: 2414 RVA: 0x00022830 File Offset: 0x00020A30
		List<float> IDashable.dashSpacing { get; } = MetaMpb.InitList<float>();

		// Token: 0x0600096F RID: 2415 RVA: 0x00022838 File Offset: 0x00020A38
		protected override void TransferShapeProperties()
		{
			base.Transfer(ShapesMaterialUtils.propColorEnd, this.colorEnd);
			base.Transfer(ShapesMaterialUtils.propPointStart, this.pointStart);
			base.Transfer(ShapesMaterialUtils.propPointEnd, this.pointEnd);
			base.Transfer(ShapesMaterialUtils.propThickness, this.thickness);
			base.Transfer(ShapesMaterialUtils.propAlignment, this.alignment);
			base.Transfer(ShapesMaterialUtils.propThicknessSpace, this.thicknessSpace);
			base.Transfer(ShapesMaterialUtils.propScaleMode, this.scaleMode);
		}

		// Token: 0x0400010E RID: 270
		internal List<Vector4> colorEnd = MetaMpb.InitList<Vector4>();

		// Token: 0x0400010F RID: 271
		internal List<Vector4> pointStart = MetaMpb.InitList<Vector4>();

		// Token: 0x04000110 RID: 272
		internal List<Vector4> pointEnd = MetaMpb.InitList<Vector4>();

		// Token: 0x04000111 RID: 273
		internal List<float> thickness = MetaMpb.InitList<float>();

		// Token: 0x04000112 RID: 274
		internal List<float> alignment = MetaMpb.InitList<float>();

		// Token: 0x04000113 RID: 275
		internal List<float> thicknessSpace = MetaMpb.InitList<float>();

		// Token: 0x04000114 RID: 276
		internal List<float> scaleMode = MetaMpb.InitList<float>();
	}
}
