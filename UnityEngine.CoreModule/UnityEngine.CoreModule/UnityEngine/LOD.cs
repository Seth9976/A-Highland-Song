using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000198 RID: 408
	[UsedByNativeCode]
	public struct LOD
	{
		// Token: 0x06000EF5 RID: 3829 RVA: 0x00012EC9 File Offset: 0x000110C9
		public LOD(float screenRelativeTransitionHeight, Renderer[] renderers)
		{
			this.screenRelativeTransitionHeight = screenRelativeTransitionHeight;
			this.fadeTransitionWidth = 0f;
			this.renderers = renderers;
		}

		// Token: 0x040005A4 RID: 1444
		public float screenRelativeTransitionHeight;

		// Token: 0x040005A5 RID: 1445
		public float fadeTransitionWidth;

		// Token: 0x040005A6 RID: 1446
		public Renderer[] renderers;
	}
}
