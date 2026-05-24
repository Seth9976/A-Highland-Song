using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000024 RID: 36
	internal class MpbDisc : MetaMpb, IDashable
	{
		// Token: 0x17000107 RID: 263
		// (get) Token: 0x0600095F RID: 2399 RVA: 0x00022605 File Offset: 0x00020805
		List<float> IDashable.dashSize { get; } = MetaMpb.InitList<float>();

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000960 RID: 2400 RVA: 0x0002260D File Offset: 0x0002080D
		List<float> IDashable.dashType { get; } = MetaMpb.InitList<float>();

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000961 RID: 2401 RVA: 0x00022615 File Offset: 0x00020815
		List<float> IDashable.dashShapeModifier { get; } = MetaMpb.InitList<float>();

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000962 RID: 2402 RVA: 0x0002261D File Offset: 0x0002081D
		List<float> IDashable.dashSpace { get; } = MetaMpb.InitList<float>();

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000963 RID: 2403 RVA: 0x00022625 File Offset: 0x00020825
		List<float> IDashable.dashSnap { get; } = MetaMpb.InitList<float>();

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000964 RID: 2404 RVA: 0x0002262D File Offset: 0x0002082D
		List<float> IDashable.dashOffset { get; } = MetaMpb.InitList<float>();

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000965 RID: 2405 RVA: 0x00022635 File Offset: 0x00020835
		List<float> IDashable.dashSpacing { get; } = MetaMpb.InitList<float>();

		// Token: 0x06000966 RID: 2406 RVA: 0x00022640 File Offset: 0x00020840
		protected override void TransferShapeProperties()
		{
			base.Transfer(ShapesMaterialUtils.propRadius, this.radius);
			base.Transfer(ShapesMaterialUtils.propRadiusSpace, this.radiusSpace);
			base.Transfer(ShapesMaterialUtils.propAlignment, this.alignment);
			base.Transfer(ShapesMaterialUtils.propThicknessSpace, this.thicknessSpace);
			base.Transfer(ShapesMaterialUtils.propThickness, this.thickness);
			base.Transfer(ShapesMaterialUtils.propScaleMode, this.scaleMode);
			base.Transfer(ShapesMaterialUtils.propAngStart, this.angStart);
			base.Transfer(ShapesMaterialUtils.propAngEnd, this.angEnd);
			base.Transfer(ShapesMaterialUtils.propRoundCaps, this.roundCaps);
			base.Transfer(ShapesMaterialUtils.propColorOuterStart, this.colorOuterStart);
			base.Transfer(ShapesMaterialUtils.propColorInnerEnd, this.colorInnerEnd);
			base.Transfer(ShapesMaterialUtils.propColorOuterEnd, this.colorOuterEnd);
		}

		// Token: 0x040000FB RID: 251
		internal List<float> radius = MetaMpb.InitList<float>();

		// Token: 0x040000FC RID: 252
		internal List<float> radiusSpace = MetaMpb.InitList<float>();

		// Token: 0x040000FD RID: 253
		internal List<float> alignment = MetaMpb.InitList<float>();

		// Token: 0x040000FE RID: 254
		internal List<float> thicknessSpace = MetaMpb.InitList<float>();

		// Token: 0x040000FF RID: 255
		internal List<float> thickness = MetaMpb.InitList<float>();

		// Token: 0x04000100 RID: 256
		internal List<float> scaleMode = MetaMpb.InitList<float>();

		// Token: 0x04000101 RID: 257
		internal List<float> angStart = MetaMpb.InitList<float>();

		// Token: 0x04000102 RID: 258
		internal List<float> angEnd = MetaMpb.InitList<float>();

		// Token: 0x04000103 RID: 259
		internal List<float> roundCaps = MetaMpb.InitList<float>();

		// Token: 0x04000104 RID: 260
		internal List<Vector4> colorOuterStart = MetaMpb.InitList<Vector4>();

		// Token: 0x04000105 RID: 261
		internal List<Vector4> colorInnerEnd = MetaMpb.InitList<Vector4>();

		// Token: 0x04000106 RID: 262
		internal List<Vector4> colorOuterEnd = MetaMpb.InitList<Vector4>();
	}
}
