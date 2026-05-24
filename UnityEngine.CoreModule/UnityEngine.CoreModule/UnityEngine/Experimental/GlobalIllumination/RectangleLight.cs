using System;

namespace UnityEngine.Experimental.GlobalIllumination
{
	// Token: 0x0200045C RID: 1116
	public struct RectangleLight
	{
		// Token: 0x04000E80 RID: 3712
		public int instanceID;

		// Token: 0x04000E81 RID: 3713
		public bool shadow;

		// Token: 0x04000E82 RID: 3714
		public LightMode mode;

		// Token: 0x04000E83 RID: 3715
		public Vector3 position;

		// Token: 0x04000E84 RID: 3716
		public Quaternion orientation;

		// Token: 0x04000E85 RID: 3717
		public LinearColor color;

		// Token: 0x04000E86 RID: 3718
		public LinearColor indirectColor;

		// Token: 0x04000E87 RID: 3719
		public float range;

		// Token: 0x04000E88 RID: 3720
		public float width;

		// Token: 0x04000E89 RID: 3721
		public float height;

		// Token: 0x04000E8A RID: 3722
		public FalloffType falloff;
	}
}
