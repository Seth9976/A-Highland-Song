using System;

namespace UnityEngine.Rendering
{
	// Token: 0x0200039D RID: 925
	[Obsolete("GPUFence has been deprecated. Use GraphicsFence instead (UnityUpgradable) -> GraphicsFence", false)]
	public struct GPUFence
	{
		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x06001F45 RID: 8005 RVA: 0x00032FF0 File Offset: 0x000311F0
		public bool passed
		{
			get
			{
				return true;
			}
		}
	}
}
