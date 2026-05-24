using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shapes
{
	// Token: 0x0200001F RID: 31
	internal class MpbCuboid : MetaMpb
	{
		// Token: 0x06000950 RID: 2384 RVA: 0x000221EF File Offset: 0x000203EF
		protected override void TransferShapeProperties()
		{
			base.Transfer(ShapesMaterialUtils.propSize, this.size);
			base.Transfer(ShapesMaterialUtils.propSizeSpace, this.sizeSpace);
		}

		// Token: 0x040000D9 RID: 217
		internal List<Vector4> size = MetaMpb.InitList<Vector4>();

		// Token: 0x040000DA RID: 218
		internal List<float> sizeSpace = MetaMpb.InitList<float>();
	}
}
