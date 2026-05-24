using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000063 RID: 99
	internal static class ShapesMeshUtils
	{
		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000A6B RID: 2667 RVA: 0x000267BF File Offset: 0x000249BF
		public static Mesh[] QuadMesh
		{
			get
			{
				return ShapesAssets.Instance.meshQuad;
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000A6C RID: 2668 RVA: 0x000267CB File Offset: 0x000249CB
		public static Mesh[] TriangleMesh
		{
			get
			{
				return ShapesAssets.Instance.meshTriangle;
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000A6D RID: 2669 RVA: 0x000267D7 File Offset: 0x000249D7
		public static Mesh[] SphereMesh
		{
			get
			{
				return ShapesAssets.Instance.meshSphere;
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000A6E RID: 2670 RVA: 0x000267E3 File Offset: 0x000249E3
		public static Mesh[] CuboidMesh
		{
			get
			{
				return ShapesAssets.Instance.meshCube;
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000A6F RID: 2671 RVA: 0x000267EF File Offset: 0x000249EF
		public static Mesh[] TorusMesh
		{
			get
			{
				return ShapesAssets.Instance.meshTorus;
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000A70 RID: 2672 RVA: 0x000267FB File Offset: 0x000249FB
		public static Mesh[] ConeMesh
		{
			get
			{
				return ShapesAssets.Instance.meshCone;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000A71 RID: 2673 RVA: 0x00026807 File Offset: 0x00024A07
		public static Mesh[] ConeMeshUncapped
		{
			get
			{
				return ShapesAssets.Instance.meshConeUncapped;
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000A72 RID: 2674 RVA: 0x00026813 File Offset: 0x00024A13
		public static Mesh[] CylinderMesh
		{
			get
			{
				return ShapesAssets.Instance.meshCylinder;
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000A73 RID: 2675 RVA: 0x0002681F File Offset: 0x00024A1F
		public static Mesh[] CapsuleMesh
		{
			get
			{
				return ShapesAssets.Instance.meshCapsule;
			}
		}

		// Token: 0x06000A74 RID: 2676 RVA: 0x0002682B File Offset: 0x00024A2B
		private static Mesh EnsureValidMeshBounds(Mesh mesh, Bounds bounds)
		{
			mesh.hideFlags = HideFlags.HideInInspector;
			mesh.bounds = bounds;
			return mesh;
		}

		// Token: 0x06000A75 RID: 2677 RVA: 0x0002683C File Offset: 0x00024A3C
		public static Mesh GetLineMesh(LineGeometry geometry, LineEndCap endCaps, DetailLevel detail)
		{
			if (geometry <= LineGeometry.Billboard)
			{
				return ShapesMeshUtils.QuadMesh[0];
			}
			if (geometry != LineGeometry.Volumetric3D)
			{
				return null;
			}
			if (endCaps != LineEndCap.Round)
			{
				return ShapesMeshUtils.CylinderMesh[(int)detail];
			}
			return ShapesMeshUtils.CapsuleMesh[(int)detail];
		}

		// Token: 0x0400023E RID: 574
		private static Mesh quadMesh;

		// Token: 0x0400023F RID: 575
		private static Mesh triangleMesh;

		// Token: 0x04000240 RID: 576
		private static Mesh sphereMesh;

		// Token: 0x04000241 RID: 577
		private static Mesh cuboidMesh;

		// Token: 0x04000242 RID: 578
		private static Mesh torusMesh;

		// Token: 0x04000243 RID: 579
		private static Mesh coneMesh;

		// Token: 0x04000244 RID: 580
		private static Mesh coneMeshUncapped;

		// Token: 0x04000245 RID: 581
		private static Mesh cylinderMesh;

		// Token: 0x04000246 RID: 582
		private static Mesh capsuleMesh;
	}
}
