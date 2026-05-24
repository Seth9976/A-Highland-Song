using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x0200002F RID: 47
	internal struct ShapeDrawState
	{
		// Token: 0x060009D1 RID: 2513 RVA: 0x00023677 File Offset: 0x00021877
		internal bool CompatibleWith(ShapeDrawState other)
		{
			return this.mesh == other.mesh && this.submesh == other.submesh && this.mat == other.mat;
		}

		// Token: 0x04000139 RID: 313
		public Mesh mesh;

		// Token: 0x0400013A RID: 314
		public Material mat;

		// Token: 0x0400013B RID: 315
		public int submesh;
	}
}
