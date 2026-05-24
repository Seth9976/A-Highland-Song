using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityX.MeshBuilder
{
	// Token: 0x0200022C RID: 556
	public class MeshBuilder
	{
		// Token: 0x0600140D RID: 5133 RVA: 0x0008BA4C File Offset: 0x00089C4C
		public void Clear()
		{
			this.verts.Clear();
			this.tris.Clear();
			this.uvs.Clear();
			this.colors.Clear();
		}

		// Token: 0x0600140E RID: 5134 RVA: 0x0008BA7C File Offset: 0x00089C7C
		public void ToMesh(Mesh mesh, MeshBakeParams bakeParams)
		{
			if (mesh.vertexCount != this.verts.Count)
			{
				mesh.Clear();
			}
			mesh.SetVertices(this.verts);
			mesh.SetTriangles(this.tris, 0);
			mesh.SetUVs(0, this.uvs);
			mesh.SetColors(this.colors);
			if (bakeParams.recalculateNormals)
			{
				mesh.RecalculateNormals();
			}
			if (bakeParams.recalculateBounds)
			{
				mesh.RecalculateBounds();
			}
		}

		// Token: 0x0600140F RID: 5135 RVA: 0x0008BAF0 File Offset: 0x00089CF0
		public Mesh ToMesh(MeshBakeParams bakeParams)
		{
			Mesh mesh = new Mesh();
			mesh.name = "MeshBuilder mesh";
			this.ToMesh(mesh, bakeParams);
			return mesh;
		}

		// Token: 0x06001410 RID: 5136 RVA: 0x0008BB18 File Offset: 0x00089D18
		public void AddPlane(AddPlaneParams input)
		{
			if (input.front)
			{
				this.verts.Add(input.topLeft);
				this.verts.Add(input.topRight);
				this.verts.Add(input.bottomLeft);
				this.verts.Add(input.topRight);
				this.verts.Add(input.bottomRight);
				this.verts.Add(input.bottomLeft);
				int count = this.tris.Count;
				for (int i = 0; i < 6; i++)
				{
					this.tris.Add(i + count);
				}
				this.uvs.Add(input.uvTopLeft);
				this.uvs.Add(input.uvTopRight);
				this.uvs.Add(input.uvBottomLeft);
				this.uvs.Add(input.uvTopRight);
				this.uvs.Add(input.uvBottomRight);
				this.uvs.Add(input.uvBottomLeft);
				this.colors.Add(input.colorTopLeft);
				this.colors.Add(input.colorTopRight);
				this.colors.Add(input.colorBottomLeft);
				this.colors.Add(input.colorTopRight);
				this.colors.Add(input.colorBottomRight);
				this.colors.Add(input.colorBottomLeft);
			}
			if (input.back)
			{
				this.verts.Add(input.bottomLeft);
				this.verts.Add(input.topRight);
				this.verts.Add(input.topLeft);
				this.verts.Add(input.bottomLeft);
				this.verts.Add(input.bottomRight);
				this.verts.Add(input.topRight);
				int count2 = this.tris.Count;
				for (int j = 0; j < 6; j++)
				{
					this.tris.Add(j + count2);
				}
				this.uvs.Add(input.uvBottomLeft);
				this.uvs.Add(input.uvTopRight);
				this.uvs.Add(input.uvTopLeft);
				this.uvs.Add(input.uvBottomLeft);
				this.uvs.Add(input.uvBottomRight);
				this.uvs.Add(input.uvTopRight);
				this.colors.Add(input.colorBottomLeft);
				this.colors.Add(input.colorTopRight);
				this.colors.Add(input.colorTopLeft);
				this.colors.Add(input.colorBottomLeft);
				this.colors.Add(input.colorBottomRight);
				this.colors.Add(input.colorTopRight);
			}
		}

		// Token: 0x06001411 RID: 5137 RVA: 0x0008BDEC File Offset: 0x00089FEC
		public void AddTriangle(AddTriangleParams input)
		{
			if (input.front)
			{
				this.verts.Add(input.topLeft);
				this.verts.Add(input.topRight);
				this.verts.Add(input.bottom);
				int count = this.tris.Count;
				for (int i = 0; i < 3; i++)
				{
					this.tris.Add(i + count);
				}
				this.uvs.Add(input.uvTopLeft);
				this.uvs.Add(input.uvTopRight);
				this.uvs.Add(input.uvBottom);
				this.colors.Add(input.colorTopLeft);
				this.colors.Add(input.colorTopRight);
				this.colors.Add(input.colorBottom);
			}
			if (input.back)
			{
				this.verts.Add(input.bottom);
				this.verts.Add(input.topRight);
				this.verts.Add(input.topLeft);
				int count2 = this.tris.Count;
				for (int j = 0; j < 3; j++)
				{
					this.tris.Add(j + count2);
				}
				this.uvs.Add(input.uvBottom);
				this.uvs.Add(input.uvTopRight);
				this.uvs.Add(input.uvTopLeft);
				this.colors.Add(input.colorBottom);
				this.colors.Add(input.colorTopRight);
				this.colors.Add(input.colorTopLeft);
			}
		}

		// Token: 0x0400131F RID: 4895
		public List<Vector3> verts = new List<Vector3>();

		// Token: 0x04001320 RID: 4896
		public List<int> tris = new List<int>();

		// Token: 0x04001321 RID: 4897
		public List<Vector2> uvs = new List<Vector2>();

		// Token: 0x04001322 RID: 4898
		public List<Color> colors = new List<Color>();
	}
}
