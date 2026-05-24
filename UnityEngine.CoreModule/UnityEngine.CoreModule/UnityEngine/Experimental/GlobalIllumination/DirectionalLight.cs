using System;

namespace UnityEngine.Experimental.GlobalIllumination
{
	// Token: 0x02000459 RID: 1113
	public struct DirectionalLight
	{
		// Token: 0x04000E60 RID: 3680
		public int instanceID;

		// Token: 0x04000E61 RID: 3681
		public bool shadow;

		// Token: 0x04000E62 RID: 3682
		public LightMode mode;

		// Token: 0x04000E63 RID: 3683
		public Vector3 position;

		// Token: 0x04000E64 RID: 3684
		public Quaternion orientation;

		// Token: 0x04000E65 RID: 3685
		public LinearColor color;

		// Token: 0x04000E66 RID: 3686
		public LinearColor indirectColor;

		// Token: 0x04000E67 RID: 3687
		public float penumbraWidthRadian;

		// Token: 0x04000E68 RID: 3688
		[Obsolete("Directional lights support cookies now. In order to position the cookie projection in the world, a position and full orientation are necessary. Use the position and orientation members instead of the direction parameter.", true)]
		public Vector3 direction;
	}
}
