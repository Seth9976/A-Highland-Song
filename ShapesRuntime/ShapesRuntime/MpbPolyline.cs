using System;
using System.Collections.Generic;

namespace Shapes
{
	// Token: 0x02000026 RID: 38
	internal class MpbPolyline : MetaMpb
	{
		// Token: 0x06000971 RID: 2417 RVA: 0x0002296C File Offset: 0x00020B6C
		protected override void TransferShapeProperties()
		{
			base.Transfer(ShapesMaterialUtils.propThickness, this.thickness);
			base.Transfer(ShapesMaterialUtils.propThicknessSpace, this.thicknessSpace);
			base.Transfer(ShapesMaterialUtils.propAlignment, this.alignment);
			base.Transfer(ShapesMaterialUtils.propScaleMode, this.scaleMode);
		}

		// Token: 0x0400011C RID: 284
		internal List<float> thickness = MetaMpb.InitList<float>();

		// Token: 0x0400011D RID: 285
		internal List<float> thicknessSpace = MetaMpb.InitList<float>();

		// Token: 0x0400011E RID: 286
		internal List<float> alignment = MetaMpb.InitList<float>();

		// Token: 0x0400011F RID: 287
		internal List<float> scaleMode = MetaMpb.InitList<float>();
	}
}
