using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x020001AA RID: 426
	[UsedByNativeCode]
	[Serializable]
	public struct CustomRenderTextureUpdateZone
	{
		// Token: 0x040005C2 RID: 1474
		public Vector3 updateZoneCenter;

		// Token: 0x040005C3 RID: 1475
		public Vector3 updateZoneSize;

		// Token: 0x040005C4 RID: 1476
		public float rotation;

		// Token: 0x040005C5 RID: 1477
		public int passIndex;

		// Token: 0x040005C6 RID: 1478
		public bool needSwap;
	}
}
