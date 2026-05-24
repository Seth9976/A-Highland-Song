using System;

namespace UnityEngine.Rendering
{
	// Token: 0x020003BE RID: 958
	public struct SubMeshDescriptor
	{
		// Token: 0x06001F4B RID: 8011 RVA: 0x00033028 File Offset: 0x00031228
		public SubMeshDescriptor(int indexStart, int indexCount, MeshTopology topology = MeshTopology.Triangles)
		{
			this.indexStart = indexStart;
			this.indexCount = indexCount;
			this.topology = topology;
			this.bounds = default(Bounds);
			this.baseVertex = 0;
			this.firstVertex = 0;
			this.vertexCount = 0;
		}

		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x06001F4C RID: 8012 RVA: 0x00033076 File Offset: 0x00031276
		// (set) Token: 0x06001F4D RID: 8013 RVA: 0x0003307E File Offset: 0x0003127E
		public Bounds bounds { readonly get; set; }

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x06001F4E RID: 8014 RVA: 0x00033087 File Offset: 0x00031287
		// (set) Token: 0x06001F4F RID: 8015 RVA: 0x0003308F File Offset: 0x0003128F
		public MeshTopology topology { readonly get; set; }

		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x06001F50 RID: 8016 RVA: 0x00033098 File Offset: 0x00031298
		// (set) Token: 0x06001F51 RID: 8017 RVA: 0x000330A0 File Offset: 0x000312A0
		public int indexStart { readonly get; set; }

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x06001F52 RID: 8018 RVA: 0x000330A9 File Offset: 0x000312A9
		// (set) Token: 0x06001F53 RID: 8019 RVA: 0x000330B1 File Offset: 0x000312B1
		public int indexCount { readonly get; set; }

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x06001F54 RID: 8020 RVA: 0x000330BA File Offset: 0x000312BA
		// (set) Token: 0x06001F55 RID: 8021 RVA: 0x000330C2 File Offset: 0x000312C2
		public int baseVertex { readonly get; set; }

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x06001F56 RID: 8022 RVA: 0x000330CB File Offset: 0x000312CB
		// (set) Token: 0x06001F57 RID: 8023 RVA: 0x000330D3 File Offset: 0x000312D3
		public int firstVertex { readonly get; set; }

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x06001F58 RID: 8024 RVA: 0x000330DC File Offset: 0x000312DC
		// (set) Token: 0x06001F59 RID: 8025 RVA: 0x000330E4 File Offset: 0x000312E4
		public int vertexCount { readonly get; set; }

		// Token: 0x06001F5A RID: 8026 RVA: 0x000330F0 File Offset: 0x000312F0
		public override string ToString()
		{
			return string.Format("(topo={0} indices={1},{2} vertices={3},{4} basevtx={5} bounds={6})", new object[] { this.topology, this.indexStart, this.indexCount, this.firstVertex, this.vertexCount, this.baseVertex, this.bounds });
		}
	}
}
