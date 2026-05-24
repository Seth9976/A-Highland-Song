using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000021 RID: 33
	internal class MpbQuad : MetaMpb
	{
		// Token: 0x06000954 RID: 2388 RVA: 0x000222D8 File Offset: 0x000204D8
		protected override void TransferShapeProperties()
		{
			base.Transfer(ShapesMaterialUtils.propA, this.a);
			base.Transfer(ShapesMaterialUtils.propB, this.b);
			base.Transfer(ShapesMaterialUtils.propC, this.c);
			base.Transfer(ShapesMaterialUtils.propD, this.d);
			base.Transfer(ShapesMaterialUtils.propColorB, this.colorB);
			base.Transfer(ShapesMaterialUtils.propColorC, this.colorC);
			base.Transfer(ShapesMaterialUtils.propColorD, this.colorD);
		}

		// Token: 0x040000E0 RID: 224
		internal List<Vector4> a = MetaMpb.InitList<Vector4>();

		// Token: 0x040000E1 RID: 225
		internal List<Vector4> b = MetaMpb.InitList<Vector4>();

		// Token: 0x040000E2 RID: 226
		internal List<Vector4> c = MetaMpb.InitList<Vector4>();

		// Token: 0x040000E3 RID: 227
		internal List<Vector4> d = MetaMpb.InitList<Vector4>();

		// Token: 0x040000E4 RID: 228
		internal List<Vector4> colorB = MetaMpb.InitList<Vector4>();

		// Token: 0x040000E5 RID: 229
		internal List<Vector4> colorC = MetaMpb.InitList<Vector4>();

		// Token: 0x040000E6 RID: 230
		internal List<Vector4> colorD = MetaMpb.InitList<Vector4>();
	}
}
