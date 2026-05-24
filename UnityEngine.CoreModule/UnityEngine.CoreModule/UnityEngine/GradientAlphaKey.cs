using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x020001C0 RID: 448
	[UsedByNativeCode]
	public struct GradientAlphaKey
	{
		// Token: 0x060013AC RID: 5036 RVA: 0x0001C4EA File Offset: 0x0001A6EA
		public GradientAlphaKey(float alpha, float time)
		{
			this.alpha = alpha;
			this.time = time;
		}

		// Token: 0x04000742 RID: 1858
		public float alpha;

		// Token: 0x04000743 RID: 1859
		public float time;
	}
}
