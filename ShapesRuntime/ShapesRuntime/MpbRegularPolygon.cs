using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000028 RID: 40
	internal class MpbRegularPolygon : MetaMpb, IFillable
	{
		// Token: 0x1700011A RID: 282
		// (get) Token: 0x0600097A RID: 2426 RVA: 0x00022A5A File Offset: 0x00020C5A
		List<float> IFillable.fillType { get; } = MetaMpb.InitList<float>();

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x0600097B RID: 2427 RVA: 0x00022A62 File Offset: 0x00020C62
		List<float> IFillable.fillSpace { get; } = MetaMpb.InitList<float>();

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x0600097C RID: 2428 RVA: 0x00022A6A File Offset: 0x00020C6A
		List<Vector4> IFillable.fillStart { get; } = MetaMpb.InitList<Vector4>();

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x0600097D RID: 2429 RVA: 0x00022A72 File Offset: 0x00020C72
		List<Vector4> IFillable.fillEnd { get; } = MetaMpb.InitList<Vector4>();

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x0600097E RID: 2430 RVA: 0x00022A7A File Offset: 0x00020C7A
		List<Vector4> IFillable.fillColorEnd { get; } = MetaMpb.InitList<Vector4>();

		// Token: 0x0600097F RID: 2431 RVA: 0x00022A84 File Offset: 0x00020C84
		protected override void TransferShapeProperties()
		{
			base.Transfer(ShapesMaterialUtils.propRadius, this.radius);
			base.Transfer(ShapesMaterialUtils.propRadiusSpace, this.radiusSpace);
			base.Transfer(ShapesMaterialUtils.propAlignment, this.geometry);
			base.Transfer(ShapesMaterialUtils.propSides, this.sides);
			base.Transfer(ShapesMaterialUtils.propAng, this.angle);
			base.Transfer(ShapesMaterialUtils.propRoundness, this.roundness);
			base.Transfer(ShapesMaterialUtils.propHollow, this.hollow);
			base.Transfer(ShapesMaterialUtils.propThicknessSpace, this.thicknessSpace);
			base.Transfer(ShapesMaterialUtils.propThickness, this.thickness);
			base.Transfer(ShapesMaterialUtils.propScaleMode, this.scaleMode);
		}

		// Token: 0x04000125 RID: 293
		internal List<float> radius = MetaMpb.InitList<float>();

		// Token: 0x04000126 RID: 294
		internal List<float> radiusSpace = MetaMpb.InitList<float>();

		// Token: 0x04000127 RID: 295
		internal List<float> geometry = MetaMpb.InitList<float>();

		// Token: 0x04000128 RID: 296
		internal List<float> sides = MetaMpb.InitList<float>();

		// Token: 0x04000129 RID: 297
		internal List<float> angle = MetaMpb.InitList<float>();

		// Token: 0x0400012A RID: 298
		internal List<float> roundness = MetaMpb.InitList<float>();

		// Token: 0x0400012B RID: 299
		internal List<float> hollow = MetaMpb.InitList<float>();

		// Token: 0x0400012C RID: 300
		internal List<float> thicknessSpace = MetaMpb.InitList<float>();

		// Token: 0x0400012D RID: 301
		internal List<float> thickness = MetaMpb.InitList<float>();

		// Token: 0x0400012E RID: 302
		internal List<float> scaleMode = MetaMpb.InitList<float>();
	}
}
