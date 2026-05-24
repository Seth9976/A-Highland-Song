using System;

namespace UnityEngine.Experimental.GlobalIllumination
{
	// Token: 0x0200045A RID: 1114
	public struct PointLight
	{
		// Token: 0x04000E69 RID: 3689
		public int instanceID;

		// Token: 0x04000E6A RID: 3690
		public bool shadow;

		// Token: 0x04000E6B RID: 3691
		public LightMode mode;

		// Token: 0x04000E6C RID: 3692
		public Vector3 position;

		// Token: 0x04000E6D RID: 3693
		public Quaternion orientation;

		// Token: 0x04000E6E RID: 3694
		public LinearColor color;

		// Token: 0x04000E6F RID: 3695
		public LinearColor indirectColor;

		// Token: 0x04000E70 RID: 3696
		public float range;

		// Token: 0x04000E71 RID: 3697
		public float sphereRadius;

		// Token: 0x04000E72 RID: 3698
		public FalloffType falloff;
	}
}
