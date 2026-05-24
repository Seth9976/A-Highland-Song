using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Shapes
{
	// Token: 0x02000006 RID: 6
	[ExecuteAlways]
	[AddComponentMenu("Shapes/Polygon")]
	public class Polygon : ShapeRendererFillable
	{
		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000093 RID: 147 RVA: 0x00003283 File Offset: 0x00001483
		// (set) Token: 0x06000094 RID: 148 RVA: 0x0000328B File Offset: 0x0000148B
		public PolygonTriangulation Triangulation
		{
			get
			{
				return this.triangulation;
			}
			set
			{
				this.triangulation = value;
				this.meshOutOfDate = true;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000095 RID: 149 RVA: 0x0000329B File Offset: 0x0000149B
		public int Count
		{
			get
			{
				return this.points.Count;
			}
		}

		// Token: 0x17000043 RID: 67
		public Vector2 this[int i]
		{
			get
			{
				return this.points[i];
			}
			set
			{
				this.points[i] = value;
				this.meshOutOfDate = true;
			}
		}

		// Token: 0x06000098 RID: 152 RVA: 0x000032CC File Offset: 0x000014CC
		public void SetPointPosition(int index, Vector2 position)
		{
			if (index < 0 || index >= this.Count)
			{
				throw new IndexOutOfRangeException();
			}
			this.points[index] = position;
			this.meshOutOfDate = true;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x000032F5 File Offset: 0x000014F5
		public void SetPoints(IEnumerable<Vector2> points)
		{
			this.points.Clear();
			this.AddPoints(points);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00003309 File Offset: 0x00001509
		public void AddPoints(IEnumerable<Vector2> points)
		{
			this.points.AddRange(points);
			this.meshOutOfDate = true;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x0000331E File Offset: 0x0000151E
		public void AddPoint(Vector2 point)
		{
			this.points.Add(point);
			this.meshOutOfDate = true;
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600009C RID: 156 RVA: 0x00003333 File Offset: 0x00001533
		private protected override bool UseCamOnPreCull
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00003336 File Offset: 0x00001536
		internal override void CamOnPreCull()
		{
			if (this.meshOutOfDate)
			{
				this.meshOutOfDate = false;
				base.UpdateMesh(true);
			}
		}

		// Token: 0x0600009E RID: 158 RVA: 0x0000334E File Offset: 0x0000154E
		private protected override void SetAllMaterialProperties()
		{
			base.SetFillProperties();
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600009F RID: 159 RVA: 0x00003356 File Offset: 0x00001556
		internal override bool HasScaleModes
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x00003359 File Offset: 0x00001559
		internal override bool HasDetailLevels
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x0000335C File Offset: 0x0000155C
		private protected override Material[] GetMaterials()
		{
			return new Material[] { ShapesMaterialUtils.matPolygon[base.BlendMode] };
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x00003377 File Offset: 0x00001577
		private protected override MeshUpdateMode MeshUpdateMode
		{
			get
			{
				return MeshUpdateMode.SelfGenerated;
			}
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x0000337A File Offset: 0x0000157A
		private protected override void GenerateMesh()
		{
			ShapesMeshGen.GenPolygonMesh(base.Mesh, this.points, this.triangulation);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00003394 File Offset: 0x00001594
		private protected override Bounds GetBounds_Internal()
		{
			if (this.points.Count < 2)
			{
				return default(Bounds);
			}
			Vector3 vector = Vector3.one * float.MaxValue;
			Vector3 vector2 = Vector3.one * float.MinValue;
			foreach (Vector2 vector3 in this.points)
			{
				Vector3 vector4 = vector3;
				vector = Vector3.Min(vector, vector4);
				vector2 = Vector3.Max(vector2, vector4);
			}
			return new Bounds((vector2 + vector) * 0.5f, vector2 - vector);
		}

		// Token: 0x04000023 RID: 35
		[FormerlySerializedAs("polyPoints")]
		[SerializeField]
		public List<Vector2> points = new List<Vector2>
		{
			new Vector2(1f, 0f),
			new Vector2(0.5f, 0.86602545f),
			new Vector2(-0.5f, 0.8660254f),
			new Vector2(-1f, 0f),
			new Vector2(-0.5f, -0.86602545f),
			new Vector2(0.5f, -0.86602545f)
		};

		// Token: 0x04000024 RID: 36
		[SerializeField]
		private PolygonTriangulation triangulation = PolygonTriangulation.EarClipping;
	}
}
