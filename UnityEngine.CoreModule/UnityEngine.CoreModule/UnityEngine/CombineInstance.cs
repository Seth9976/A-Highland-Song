using System;

namespace UnityEngine
{
	// Token: 0x020001A0 RID: 416
	public struct CombineInstance
	{
		// Token: 0x1700035B RID: 859
		// (get) Token: 0x060010AD RID: 4269 RVA: 0x00016070 File Offset: 0x00014270
		// (set) Token: 0x060010AE RID: 4270 RVA: 0x0001608D File Offset: 0x0001428D
		public Mesh mesh
		{
			get
			{
				return Mesh.FromInstanceID(this.m_MeshInstanceID);
			}
			set
			{
				this.m_MeshInstanceID = ((value != null) ? value.GetInstanceID() : 0);
			}
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x060010AF RID: 4271 RVA: 0x000160A8 File Offset: 0x000142A8
		// (set) Token: 0x060010B0 RID: 4272 RVA: 0x000160C0 File Offset: 0x000142C0
		public int subMeshIndex
		{
			get
			{
				return this.m_SubMeshIndex;
			}
			set
			{
				this.m_SubMeshIndex = value;
			}
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x060010B1 RID: 4273 RVA: 0x000160CC File Offset: 0x000142CC
		// (set) Token: 0x060010B2 RID: 4274 RVA: 0x000160E4 File Offset: 0x000142E4
		public Matrix4x4 transform
		{
			get
			{
				return this.m_Transform;
			}
			set
			{
				this.m_Transform = value;
			}
		}

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x060010B3 RID: 4275 RVA: 0x000160F0 File Offset: 0x000142F0
		// (set) Token: 0x060010B4 RID: 4276 RVA: 0x00016108 File Offset: 0x00014308
		public Vector4 lightmapScaleOffset
		{
			get
			{
				return this.m_LightmapScaleOffset;
			}
			set
			{
				this.m_LightmapScaleOffset = value;
			}
		}

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x060010B5 RID: 4277 RVA: 0x00016114 File Offset: 0x00014314
		// (set) Token: 0x060010B6 RID: 4278 RVA: 0x0001612C File Offset: 0x0001432C
		public Vector4 realtimeLightmapScaleOffset
		{
			get
			{
				return this.m_RealtimeLightmapScaleOffset;
			}
			set
			{
				this.m_RealtimeLightmapScaleOffset = value;
			}
		}

		// Token: 0x040005B4 RID: 1460
		private int m_MeshInstanceID;

		// Token: 0x040005B5 RID: 1461
		private int m_SubMeshIndex;

		// Token: 0x040005B6 RID: 1462
		private Matrix4x4 m_Transform;

		// Token: 0x040005B7 RID: 1463
		private Vector4 m_LightmapScaleOffset;

		// Token: 0x040005B8 RID: 1464
		private Vector4 m_RealtimeLightmapScaleOffset;
	}
}
