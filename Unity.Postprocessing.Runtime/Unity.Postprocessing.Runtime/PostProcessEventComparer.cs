using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering.PostProcessing
{
	// Token: 0x02000053 RID: 83
	internal struct PostProcessEventComparer : IEqualityComparer<PostProcessEvent>
	{
		// Token: 0x0600011A RID: 282 RVA: 0x0000A702 File Offset: 0x00008902
		public bool Equals(PostProcessEvent x, PostProcessEvent y)
		{
			return x == y;
		}

		// Token: 0x0600011B RID: 283 RVA: 0x0000A708 File Offset: 0x00008908
		public int GetHashCode(PostProcessEvent obj)
		{
			return (int)obj;
		}
	}
}
