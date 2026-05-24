using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000023 RID: 35
	internal class MpbRectangle : MetaMpb, IFillable
	{
		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000958 RID: 2392 RVA: 0x000224F5 File Offset: 0x000206F5
		List<float> IFillable.fillType { get; } = MetaMpb.InitList<float>();

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000959 RID: 2393 RVA: 0x000224FD File Offset: 0x000206FD
		List<float> IFillable.fillSpace { get; } = MetaMpb.InitList<float>();

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x0600095A RID: 2394 RVA: 0x00022505 File Offset: 0x00020705
		List<Vector4> IFillable.fillStart { get; } = MetaMpb.InitList<Vector4>();

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x0600095B RID: 2395 RVA: 0x0002250D File Offset: 0x0002070D
		List<Vector4> IFillable.fillEnd { get; } = MetaMpb.InitList<Vector4>();

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x0600095C RID: 2396 RVA: 0x00022515 File Offset: 0x00020715
		List<Vector4> IFillable.fillColorEnd { get; } = MetaMpb.InitList<Vector4>();

		// Token: 0x0600095D RID: 2397 RVA: 0x00022520 File Offset: 0x00020720
		protected override void TransferShapeProperties()
		{
			base.Transfer(ShapesMaterialUtils.propRect, this.rect);
			base.Transfer(ShapesMaterialUtils.propCornerRadii, this.cornerRadii);
			base.Transfer(ShapesMaterialUtils.propThickness, this.thickness);
			base.Transfer(ShapesMaterialUtils.propThicknessSpace, this.thicknessSpace);
			base.Transfer(ShapesMaterialUtils.propScaleMode, this.scaleMode);
		}

		// Token: 0x040000F1 RID: 241
		internal List<Vector4> rect = MetaMpb.InitList<Vector4>();

		// Token: 0x040000F2 RID: 242
		internal List<Vector4> cornerRadii = MetaMpb.InitList<Vector4>();

		// Token: 0x040000F3 RID: 243
		internal List<float> thickness = MetaMpb.InitList<float>();

		// Token: 0x040000F4 RID: 244
		internal List<float> thicknessSpace = MetaMpb.InitList<float>();

		// Token: 0x040000F5 RID: 245
		internal List<float> scaleMode = MetaMpb.InitList<float>();
	}
}
