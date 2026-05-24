using System;

namespace UnityEngine
{
	// Token: 0x020000FF RID: 255
	public struct BoundingSphere
	{
		// Token: 0x060005A9 RID: 1449 RVA: 0x00007F9E File Offset: 0x0000619E
		public BoundingSphere(Vector3 pos, float rad)
		{
			this.position = pos;
			this.radius = rad;
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x00007FAF File Offset: 0x000061AF
		public BoundingSphere(Vector4 packedSphere)
		{
			this.position = new Vector3(packedSphere.x, packedSphere.y, packedSphere.z);
			this.radius = packedSphere.w;
		}

		// Token: 0x04000365 RID: 869
		public Vector3 position;

		// Token: 0x04000366 RID: 870
		public float radius;
	}
}
