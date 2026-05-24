using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000022 RID: 34
	internal class MpbTriangle : MetaMpb
	{
		// Token: 0x06000956 RID: 2390 RVA: 0x000223BC File Offset: 0x000205BC
		protected override void TransferShapeProperties()
		{
			base.Transfer(ShapesMaterialUtils.propA, this.a);
			base.Transfer(ShapesMaterialUtils.propB, this.b);
			base.Transfer(ShapesMaterialUtils.propC, this.c);
			base.Transfer(ShapesMaterialUtils.propColorB, this.colorB);
			base.Transfer(ShapesMaterialUtils.propColorC, this.colorC);
			base.Transfer(ShapesMaterialUtils.propRoundness, this.roundness);
			base.Transfer(ShapesMaterialUtils.propHollow, this.hollow);
			base.Transfer(ShapesMaterialUtils.propThicknessSpace, this.thicknessSpace);
			base.Transfer(ShapesMaterialUtils.propThickness, this.thickness);
			base.Transfer(ShapesMaterialUtils.propScaleMode, this.scaleMode);
		}

		// Token: 0x040000E7 RID: 231
		internal List<Vector4> a = MetaMpb.InitList<Vector4>();

		// Token: 0x040000E8 RID: 232
		internal List<Vector4> b = MetaMpb.InitList<Vector4>();

		// Token: 0x040000E9 RID: 233
		internal List<Vector4> c = MetaMpb.InitList<Vector4>();

		// Token: 0x040000EA RID: 234
		internal List<Vector4> colorB = MetaMpb.InitList<Vector4>();

		// Token: 0x040000EB RID: 235
		internal List<Vector4> colorC = MetaMpb.InitList<Vector4>();

		// Token: 0x040000EC RID: 236
		internal List<float> roundness = MetaMpb.InitList<float>();

		// Token: 0x040000ED RID: 237
		internal List<float> hollow = MetaMpb.InitList<float>();

		// Token: 0x040000EE RID: 238
		internal List<float> thicknessSpace = MetaMpb.InitList<float>();

		// Token: 0x040000EF RID: 239
		internal List<float> thickness = MetaMpb.InitList<float>();

		// Token: 0x040000F0 RID: 240
		internal List<float> scaleMode = MetaMpb.InitList<float>();
	}
}
