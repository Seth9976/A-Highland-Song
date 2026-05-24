using System;

namespace UnityEngine.Experimental.GlobalIllumination
{
	// Token: 0x02000460 RID: 1120
	public struct Cookie
	{
		// Token: 0x060027B6 RID: 10166 RVA: 0x00041800 File Offset: 0x0003FA00
		public static Cookie Defaults()
		{
			Cookie cookie;
			cookie.instanceID = 0;
			cookie.scale = 1f;
			cookie.sizes = new Vector2(1f, 1f);
			return cookie;
		}

		// Token: 0x04000EAA RID: 3754
		public int instanceID;

		// Token: 0x04000EAB RID: 3755
		public float scale;

		// Token: 0x04000EAC RID: 3756
		public Vector2 sizes;
	}
}
