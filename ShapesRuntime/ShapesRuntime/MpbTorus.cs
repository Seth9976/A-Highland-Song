using System;
using System.Collections.Generic;

namespace Shapes
{
	// Token: 0x02000020 RID: 32
	internal class MpbTorus : MetaMpb
	{
		// Token: 0x06000952 RID: 2386 RVA: 0x00022234 File Offset: 0x00020434
		protected override void TransferShapeProperties()
		{
			base.Transfer(ShapesMaterialUtils.propRadius, this.radius);
			base.Transfer(ShapesMaterialUtils.propThickness, this.thickness);
			base.Transfer(ShapesMaterialUtils.propRadiusSpace, this.spaceRadius);
			base.Transfer(ShapesMaterialUtils.propThicknessSpace, this.spaceThickness);
			base.Transfer(ShapesMaterialUtils.propScaleMode, this.scaleMode);
		}

		// Token: 0x040000DB RID: 219
		internal List<float> radius = MetaMpb.InitList<float>();

		// Token: 0x040000DC RID: 220
		internal List<float> thickness = MetaMpb.InitList<float>();

		// Token: 0x040000DD RID: 221
		internal List<float> spaceRadius = MetaMpb.InitList<float>();

		// Token: 0x040000DE RID: 222
		internal List<float> spaceThickness = MetaMpb.InitList<float>();

		// Token: 0x040000DF RID: 223
		internal List<float> scaleMode = MetaMpb.InitList<float>();
	}
}
