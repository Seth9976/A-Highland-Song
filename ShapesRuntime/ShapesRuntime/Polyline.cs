using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Shapes
{
	// Token: 0x02000007 RID: 7
	[ExecuteAlways]
	[AddComponentMenu("Shapes/Polyline")]
	public class Polyline : ShapeRenderer
	{
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x000034F3 File Offset: 0x000016F3
		// (set) Token: 0x060000A7 RID: 167 RVA: 0x000034FB File Offset: 0x000016FB
		public PolylineGeometry Geometry
		{
			get
			{
				return this.geometry;
			}
			set
			{
				this.geometry = value;
				base.SetIntNow(ShapesMaterialUtils.propAlignment, (int)this.geometry);
				base.UpdateMaterial();
				base.ApplyProperties();
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x00003521 File Offset: 0x00001721
		// (set) Token: 0x060000A9 RID: 169 RVA: 0x00003529 File Offset: 0x00001729
		public PolylineJoins Joins
		{
			get
			{
				return this.joins;
			}
			set
			{
				this.joins = value;
				this.meshOutOfDate = true;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000AA RID: 170 RVA: 0x00003539 File Offset: 0x00001739
		// (set) Token: 0x060000AB RID: 171 RVA: 0x00003541 File Offset: 0x00001741
		public bool Closed
		{
			get
			{
				return this.closed;
			}
			set
			{
				this.closed = value;
				this.meshOutOfDate = true;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000AC RID: 172 RVA: 0x00003551 File Offset: 0x00001751
		// (set) Token: 0x060000AD RID: 173 RVA: 0x0000355C File Offset: 0x0000175C
		public float Thickness
		{
			get
			{
				return this.thickness;
			}
			set
			{
				int propThickness = ShapesMaterialUtils.propThickness;
				this.thickness = value;
				base.SetFloatNow(propThickness, value);
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000AE RID: 174 RVA: 0x0000357E File Offset: 0x0000177E
		// (set) Token: 0x060000AF RID: 175 RVA: 0x00003588 File Offset: 0x00001788
		public ThicknessSpace ThicknessSpace
		{
			get
			{
				return this.thicknessSpace;
			}
			set
			{
				int propThicknessSpace = ShapesMaterialUtils.propThicknessSpace;
				this.thicknessSpace = value;
				base.SetIntNow(propThicknessSpace, (int)value);
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x000035AA File Offset: 0x000017AA
		public int Count
		{
			get
			{
				return this.points.Count;
			}
		}

		// Token: 0x1700004E RID: 78
		public PolylinePoint this[int i]
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

		// Token: 0x060000B3 RID: 179 RVA: 0x000035DC File Offset: 0x000017DC
		public void SetPointPosition(int index, Vector3 position)
		{
			if (index < 0 || index >= this.Count)
			{
				throw new IndexOutOfRangeException();
			}
			PolylinePoint polylinePoint = this.points[index];
			polylinePoint.point = position;
			this.points[index] = polylinePoint;
			this.meshOutOfDate = true;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00003628 File Offset: 0x00001828
		public void SetPointColor(int index, Color color)
		{
			if (index < 0 || index >= this.Count)
			{
				throw new IndexOutOfRangeException();
			}
			PolylinePoint polylinePoint = this.points[index];
			polylinePoint.color = color;
			this.points[index] = polylinePoint;
			this.meshOutOfDate = true;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00003674 File Offset: 0x00001874
		public void SetPointThickness(int index, float thickness)
		{
			if (index < 0 || index >= this.Count)
			{
				throw new IndexOutOfRangeException();
			}
			PolylinePoint polylinePoint = this.points[index];
			polylinePoint.thickness = thickness;
			this.points[index] = polylinePoint;
			this.meshOutOfDate = true;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000036C0 File Offset: 0x000018C0
		public void SetPoints(IReadOnlyCollection<Vector3> points, IReadOnlyCollection<Color> colors = null)
		{
			this.points.Clear();
			if (colors == null)
			{
				this.AddPoints(points.Select((Vector3 p) => new PolylinePoint(p, Color.white)));
				return;
			}
			if (points.Count != colors.Count)
			{
				throw new ArgumentException("point.Count != color.Count");
			}
			this.AddPoints(points.Zip(colors, (Vector3 p, Color c) => new PolylinePoint(p, c)));
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x0000374C File Offset: 0x0000194C
		public void SetPoints(IReadOnlyCollection<Vector2> points, IReadOnlyCollection<Color> colors = null)
		{
			this.points.Clear();
			if (colors == null)
			{
				this.AddPoints(points.Select((Vector2 p) => new PolylinePoint(p, Color.white)));
				return;
			}
			if (points.Count != colors.Count)
			{
				throw new ArgumentException("point.Count != color.Count");
			}
			this.AddPoints(points.Zip(colors, (Vector2 p, Color c) => new PolylinePoint(p, c)));
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x000037D8 File Offset: 0x000019D8
		public void SetPoints(IEnumerable<PolylinePoint> points)
		{
			this.points.Clear();
			this.AddPoints(points);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x000037EC File Offset: 0x000019EC
		public void AddPoints(IEnumerable<PolylinePoint> points)
		{
			this.points.AddRange(points);
			this.meshOutOfDate = true;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00003801 File Offset: 0x00001A01
		public void AddPoint(Vector3 position)
		{
			this.AddPoint(new PolylinePoint(position));
		}

		// Token: 0x060000BB RID: 187 RVA: 0x0000380F File Offset: 0x00001A0F
		public void AddPoint(Vector3 position, Color color)
		{
			this.AddPoint(new PolylinePoint(position, color));
		}

		// Token: 0x060000BC RID: 188 RVA: 0x0000381E File Offset: 0x00001A1E
		public void AddPoint(Vector3 position, Color color, float thickness)
		{
			this.AddPoint(new PolylinePoint(position, color, thickness));
		}

		// Token: 0x060000BD RID: 189 RVA: 0x0000382E File Offset: 0x00001A2E
		public void AddPoint(Vector3 position, float thickness)
		{
			this.AddPoint(new PolylinePoint(position, Color.white, thickness));
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00003842 File Offset: 0x00001A42
		public void AddPoint(PolylinePoint point)
		{
			this.points.Add(point);
			this.meshOutOfDate = true;
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00003857 File Offset: 0x00001A57
		private protected override bool UseCamOnPreCull
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x0000385A File Offset: 0x00001A5A
		internal override void CamOnPreCull()
		{
			if (this.meshOutOfDate)
			{
				this.meshOutOfDate = false;
				base.UpdateMesh(true);
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00003872 File Offset: 0x00001A72
		private protected override MeshUpdateMode MeshUpdateMode
		{
			get
			{
				return MeshUpdateMode.SelfGenerated;
			}
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00003875 File Offset: 0x00001A75
		private protected override void GenerateMesh()
		{
			ShapesMeshGen.GenPolylineMesh(base.Mesh, this.points, this.closed, this.joins, this.geometry == PolylineGeometry.Flat2D, true);
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x0000389E File Offset: 0x00001A9E
		private protected override void SetAllMaterialProperties()
		{
			base.SetFloat(ShapesMaterialUtils.propThickness, this.thickness);
			base.SetInt(ShapesMaterialUtils.propThicknessSpace, (int)this.thicknessSpace);
			base.SetInt(ShapesMaterialUtils.propAlignment, (int)this.geometry);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x000038D3 File Offset: 0x00001AD3
		private protected override void ShapeClampRanges()
		{
			this.thickness = Mathf.Max(0f, this.thickness);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x000038EC File Offset: 0x00001AEC
		private protected override Material[] GetMaterials()
		{
			if (this.joins.HasJoinMesh())
			{
				return new Material[]
				{
					ShapesMaterialUtils.GetPolylineMat(this.joins)[base.BlendMode],
					ShapesMaterialUtils.GetPolylineJoinsMat(this.joins)[base.BlendMode]
				};
			}
			return new Material[] { ShapesMaterialUtils.GetPolylineMat(this.joins)[base.BlendMode] };
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00003960 File Offset: 0x00001B60
		private protected override Bounds GetBounds_Internal()
		{
			if (this.points.Count < 2)
			{
				return default(Bounds);
			}
			Vector3 vector = Vector3.one * float.MaxValue;
			Vector3 vector2 = Vector3.one * float.MinValue;
			foreach (Vector3 vector3 in this.points.Select((PolylinePoint p) => p.point))
			{
				vector = Vector3.Min(vector, vector3);
				vector2 = Vector3.Max(vector2, vector3);
			}
			if (this.geometry == PolylineGeometry.Flat2D)
			{
				vector.z = (vector2.z = 0f);
			}
			return new Bounds((vector2 + vector) * 0.5f, vector2 - vector + Vector3.one * (this.thickness * 0.5f));
		}

		// Token: 0x04000025 RID: 37
		[FormerlySerializedAs("polyPoints")]
		[SerializeField]
		public List<PolylinePoint> points = new List<PolylinePoint>
		{
			new PolylinePoint(new Vector3(0f, 1f, 0f), Color.white),
			new PolylinePoint(new Vector3(0.8660254f, -0.5f, 0f), Color.white),
			new PolylinePoint(new Vector3(-0.8660254f, -0.5f, 0f), Color.white)
		};

		// Token: 0x04000026 RID: 38
		[SerializeField]
		private PolylineGeometry geometry;

		// Token: 0x04000027 RID: 39
		[SerializeField]
		private PolylineJoins joins = PolylineJoins.Miter;

		// Token: 0x04000028 RID: 40
		[SerializeField]
		private bool closed = true;

		// Token: 0x04000029 RID: 41
		[SerializeField]
		private float thickness = 0.125f;

		// Token: 0x0400002A RID: 42
		[SerializeField]
		private ThicknessSpace thicknessSpace;
	}
}
