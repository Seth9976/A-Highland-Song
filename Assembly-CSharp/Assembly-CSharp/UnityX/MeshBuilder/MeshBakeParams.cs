using System;

namespace UnityX.MeshBuilder
{
	// Token: 0x0200022B RID: 555
	[Serializable]
	public struct MeshBakeParams
	{
		// Token: 0x0600140C RID: 5132 RVA: 0x0008BA3C File Offset: 0x00089C3C
		public MeshBakeParams(bool recalculateNormals, bool recalculateBounds)
		{
			this.recalculateNormals = recalculateNormals;
			this.recalculateBounds = recalculateBounds;
		}

		// Token: 0x0400131D RID: 4893
		public bool recalculateNormals;

		// Token: 0x0400131E RID: 4894
		public bool recalculateBounds;
	}
}
