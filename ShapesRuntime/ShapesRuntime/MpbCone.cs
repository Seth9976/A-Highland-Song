using System;
using System.Collections.Generic;

namespace Shapes
{
	// Token: 0x0200001E RID: 30
	internal class MpbCone : MetaMpb
	{
		// Token: 0x0600094E RID: 2382 RVA: 0x00022191 File Offset: 0x00020391
		protected override void TransferShapeProperties()
		{
			base.Transfer(ShapesMaterialUtils.propRadius, this.radius);
			base.Transfer(ShapesMaterialUtils.propLength, this.length);
			base.Transfer(ShapesMaterialUtils.propSizeSpace, this.sizeSpace);
		}

		// Token: 0x040000D6 RID: 214
		internal List<float> radius = MetaMpb.InitList<float>();

		// Token: 0x040000D7 RID: 215
		internal List<float> length = MetaMpb.InitList<float>();

		// Token: 0x040000D8 RID: 216
		internal List<float> sizeSpace = MetaMpb.InitList<float>();
	}
}
