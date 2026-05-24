using System;
using UnityEngine;
using UnityX.MeshBuilder;

// Token: 0x0200006A RID: 106
public class PlaneMeshGenerator : MonoBehaviour
{
	// Token: 0x060002F6 RID: 758 RVA: 0x00018528 File Offset: 0x00016728
	[ContextMenu("Generate 2x2")]
	private void GeneratePrivate2x2()
	{
		this.mesh = PlaneMeshGenerator.Generate(new Vector2Int(2, 2));
	}

	// Token: 0x060002F7 RID: 759 RVA: 0x0001853C File Offset: 0x0001673C
	[ContextMenu("Generate 5x5")]
	private void GeneratePrivate5x5()
	{
		this.mesh = PlaneMeshGenerator.Generate(new Vector2Int(5, 5));
	}

	// Token: 0x060002F8 RID: 760 RVA: 0x00018550 File Offset: 0x00016750
	[ContextMenu("Generate 4x2")]
	private void GeneratePrivate4x2()
	{
		this.mesh = PlaneMeshGenerator.Generate(new Vector2Int(4, 2));
	}

	// Token: 0x060002F9 RID: 761 RVA: 0x00018564 File Offset: 0x00016764
	public static Mesh Generate(Vector2Int verts)
	{
		Mesh mesh = new Mesh();
		mesh.name = "Subdivided Plane " + verts.x.ToString() + "x" + verts.y.ToString();
		Vector2 vector = new Vector2(1f / (float)verts.x, 1f / (float)verts.y);
		Vector3[] array = new Vector3[(verts.x + 1) * (verts.y + 1)];
		Vector2[] array2 = new Vector2[array.Length];
		Vector4[] array3 = new Vector4[array.Length];
		Vector4 vector2 = new Vector4(1f, 0f, 0f, -1f);
		int num = 0;
		for (int i = 0; i <= verts.y; i++)
		{
			int j = 0;
			while (j <= verts.x)
			{
				array[num] = new Vector3((float)j * vector.x - 0.5f, (float)i * vector.y - 0.5f, 0f);
				array2[num] = new Vector2((float)j / (float)verts.x, (float)i / (float)verts.y);
				array3[num] = vector2;
				j++;
				num++;
			}
		}
		mesh.vertices = array;
		mesh.uv = array2;
		mesh.tangents = array3;
		int[] array4 = new int[verts.x * verts.y * 6];
		int num2 = 0;
		int num3 = 0;
		int k = 0;
		while (k < verts.y)
		{
			int l = 0;
			while (l < verts.x)
			{
				array4[num2] = num3;
				array4[num2 + 3] = (array4[num2 + 2] = num3 + 1);
				array4[num2 + 4] = (array4[num2 + 1] = num3 + verts.x + 1);
				array4[num2 + 5] = num3 + verts.x + 2;
				l++;
				num2 += 6;
				num3++;
			}
			k++;
			num3++;
		}
		mesh.triangles = array4;
		if (PlaneMeshGenerator.mbp.recalculateBounds)
		{
			mesh.RecalculateBounds();
		}
		if (PlaneMeshGenerator.mbp.recalculateNormals)
		{
			mesh.RecalculateNormals();
		}
		return mesh;
	}

	// Token: 0x04000433 RID: 1075
	[AssetSaver]
	public Mesh mesh;

	// Token: 0x04000434 RID: 1076
	public static MeshBakeParams mbp = new MeshBakeParams(true, true);
}
