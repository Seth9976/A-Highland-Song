using System;

namespace UnityEngine.Experimental.GlobalIllumination
{
	// Token: 0x0200045D RID: 1117
	public struct DiscLight
	{
		// Token: 0x04000E8B RID: 3723
		public int instanceID;

		// Token: 0x04000E8C RID: 3724
		public bool shadow;

		// Token: 0x04000E8D RID: 3725
		public LightMode mode;

		// Token: 0x04000E8E RID: 3726
		public Vector3 position;

		// Token: 0x04000E8F RID: 3727
		public Quaternion orientation;

		// Token: 0x04000E90 RID: 3728
		public LinearColor color;

		// Token: 0x04000E91 RID: 3729
		public LinearColor indirectColor;

		// Token: 0x04000E92 RID: 3730
		public float range;

		// Token: 0x04000E93 RID: 3731
		public float radius;

		// Token: 0x04000E94 RID: 3732
		public FalloffType falloff;
	}
}
