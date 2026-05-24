using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x020001BF RID: 447
	[UsedByNativeCode]
	public struct GradientColorKey
	{
		// Token: 0x060013AB RID: 5035 RVA: 0x0001C4D9 File Offset: 0x0001A6D9
		public GradientColorKey(Color col, float time)
		{
			this.color = col;
			this.time = time;
		}

		// Token: 0x04000740 RID: 1856
		public Color color;

		// Token: 0x04000741 RID: 1857
		public float time;
	}
}
