using System;
using System.Collections.Generic;

namespace Shapes
{
	// Token: 0x0200001D RID: 29
	internal class MpbSphere : MetaMpb
	{
		// Token: 0x0600094C RID: 2380 RVA: 0x0002214F File Offset: 0x0002034F
		protected override void TransferShapeProperties()
		{
			base.Transfer(ShapesMaterialUtils.propRadius, this.radius);
			base.Transfer(ShapesMaterialUtils.propRadiusSpace, this.radiusSpace);
		}

		// Token: 0x040000D4 RID: 212
		internal List<float> radius = MetaMpb.InitList<float>();

		// Token: 0x040000D5 RID: 213
		internal List<float> radiusSpace = MetaMpb.InitList<float>();
	}
}
