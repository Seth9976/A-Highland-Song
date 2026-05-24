using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000010 RID: 16
	[ExecuteAlways]
	[AddComponentMenu("Shapes/Triangle")]
	public class Triangle : ShapeRenderer
	{
		// Token: 0x170000AB RID: 171
		public Vector3 this[int index]
		{
			get
			{
				switch (index)
				{
				case 0:
					return this.A;
				case 1:
					return this.B;
				case 2:
					return this.C;
				default:
					throw new IndexOutOfRangeException(string.Format("Triangle only has four vertices, 0 to 2, you tried to access element {0}", index));
				}
			}
			set
			{
				switch (index)
				{
				case 0:
					this.A = value;
					return;
				case 1:
					this.B = value;
					return;
				case 2:
					this.C = value;
					return;
				default:
					throw new IndexOutOfRangeException(string.Format("Triangle only has four vertices, 0 to 2, you tried to set element {0}", index));
				}
			}
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00005D82 File Offset: 0x00003F82
		public Vector3 GetTriangleVertex(int index)
		{
			return this[index];
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00005D8C File Offset: 0x00003F8C
		public Vector3 SetTriangleVertex(int index, Vector3 value)
		{
			this[index] = value;
			return value;
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00005DA4 File Offset: 0x00003FA4
		public Color GetTriangleColor(int index)
		{
			switch (index)
			{
			case 0:
				return this.Color;
			case 1:
				return this.ColorB;
			case 2:
				return this.ColorC;
			default:
				throw new IndexOutOfRangeException(string.Format("Triangle only has four vertices, 0 to 2, you tried to access element {0}", index));
			}
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00005DE4 File Offset: 0x00003FE4
		public void SetTriangleColor(int index, Color color)
		{
			switch (index)
			{
			case 0:
				this.Color = color;
				return;
			case 1:
				this.ColorB = color;
				return;
			case 2:
				this.ColorC = color;
				return;
			default:
				throw new IndexOutOfRangeException(string.Format("Triangle only has four vertices, 0 to 3, you tried to set element {0}", index));
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x00005E32 File Offset: 0x00004032
		// (set) Token: 0x060001C2 RID: 450 RVA: 0x00005E3A File Offset: 0x0000403A
		public Triangle.TriangleColorMode ColorMode
		{
			get
			{
				return this.colorMode;
			}
			set
			{
				this.colorMode = value;
				base.ApplyProperties();
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x00005E49 File Offset: 0x00004049
		// (set) Token: 0x060001C4 RID: 452 RVA: 0x00005E54 File Offset: 0x00004054
		public Vector3 A
		{
			get
			{
				return this.a;
			}
			set
			{
				int propA = ShapesMaterialUtils.propA;
				this.a = value;
				base.SetVector3Now(propA, value);
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x00005E76 File Offset: 0x00004076
		// (set) Token: 0x060001C6 RID: 454 RVA: 0x00005E80 File Offset: 0x00004080
		public Vector3 B
		{
			get
			{
				return this.b;
			}
			set
			{
				int propB = ShapesMaterialUtils.propB;
				this.b = value;
				base.SetVector3Now(propB, value);
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x00005EA2 File Offset: 0x000040A2
		// (set) Token: 0x060001C8 RID: 456 RVA: 0x00005EAC File Offset: 0x000040AC
		public Vector3 C
		{
			get
			{
				return this.c;
			}
			set
			{
				int propC = ShapesMaterialUtils.propC;
				this.c = value;
				base.SetVector3Now(propC, value);
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x00005ECE File Offset: 0x000040CE
		// (set) Token: 0x060001CA RID: 458 RVA: 0x00005ED8 File Offset: 0x000040D8
		public bool Hollow
		{
			get
			{
				return this.hollow;
			}
			set
			{
				int propHollow = ShapesMaterialUtils.propHollow;
				this.hollow = value;
				base.SetIntNow(propHollow, value.AsInt());
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060001CB RID: 459 RVA: 0x00005EFF File Offset: 0x000040FF
		// (set) Token: 0x060001CC RID: 460 RVA: 0x00005F08 File Offset: 0x00004108
		public float Thickness
		{
			get
			{
				return this.thickness;
			}
			set
			{
				base.SetFloatNow(ShapesMaterialUtils.propThickness, this.thickness = Mathf.Max(0f, value));
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060001CD RID: 461 RVA: 0x00005F34 File Offset: 0x00004134
		// (set) Token: 0x060001CE RID: 462 RVA: 0x00005F3C File Offset: 0x0000413C
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

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060001CF RID: 463 RVA: 0x00005F5E File Offset: 0x0000415E
		// (set) Token: 0x060001D0 RID: 464 RVA: 0x00005F68 File Offset: 0x00004168
		public float Roundness
		{
			get
			{
				return this.roundness;
			}
			set
			{
				base.SetFloatNow(ShapesMaterialUtils.propRoundness, this.roundness = Mathf.Clamp01(value));
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x00005F8F File Offset: 0x0000418F
		// (set) Token: 0x060001D2 RID: 466 RVA: 0x00005F98 File Offset: 0x00004198
		public override Color Color
		{
			get
			{
				return this.color;
			}
			set
			{
				int propColor = ShapesMaterialUtils.propColor;
				this.color = value;
				base.SetColor(propColor, value);
				int propColorB = ShapesMaterialUtils.propColorB;
				this.colorB = value;
				base.SetColor(propColorB, value);
				int propColorC = ShapesMaterialUtils.propColorC;
				this.colorC = value;
				base.SetColorNow(propColorC, value);
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x00005FE4 File Offset: 0x000041E4
		// (set) Token: 0x060001D4 RID: 468 RVA: 0x00005FEC File Offset: 0x000041EC
		public Color ColorA
		{
			get
			{
				return this.color;
			}
			set
			{
				int propColor = ShapesMaterialUtils.propColor;
				this.color = value;
				base.SetColorNow(propColor, value);
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x0000600E File Offset: 0x0000420E
		// (set) Token: 0x060001D6 RID: 470 RVA: 0x00006018 File Offset: 0x00004218
		public Color ColorB
		{
			get
			{
				return this.colorB;
			}
			set
			{
				int propColorB = ShapesMaterialUtils.propColorB;
				this.colorB = value;
				base.SetColorNow(propColorB, value);
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x0000603A File Offset: 0x0000423A
		// (set) Token: 0x060001D8 RID: 472 RVA: 0x00006044 File Offset: 0x00004244
		public Color ColorC
		{
			get
			{
				return this.colorC;
			}
			set
			{
				int propColorC = ShapesMaterialUtils.propColorC;
				this.colorC = value;
				base.SetColorNow(propColorC, value);
			}
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00006068 File Offset: 0x00004268
		private protected override void SetAllMaterialProperties()
		{
			base.SetVector3(ShapesMaterialUtils.propA, this.a);
			base.SetVector3(ShapesMaterialUtils.propB, this.b);
			base.SetVector3(ShapesMaterialUtils.propC, this.c);
			if (this.colorMode == Triangle.TriangleColorMode.Single)
			{
				base.SetColor(ShapesMaterialUtils.propColorB, this.Color);
				base.SetColor(ShapesMaterialUtils.propColorC, this.Color);
			}
			else
			{
				base.SetColor(ShapesMaterialUtils.propColorB, this.colorB);
				base.SetColor(ShapesMaterialUtils.propColorC, this.colorC);
			}
			base.SetFloat(ShapesMaterialUtils.propRoundness, this.roundness);
			base.SetFloat(ShapesMaterialUtils.propThickness, this.thickness);
			base.SetFloat(ShapesMaterialUtils.propThicknessSpace, (float)this.thicknessSpace);
			base.SetFloat(ShapesMaterialUtils.propHollow, (float)this.hollow.AsInt());
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060001DA RID: 474 RVA: 0x00006141 File Offset: 0x00004341
		internal override bool HasDetailLevels
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060001DB RID: 475 RVA: 0x00006144 File Offset: 0x00004344
		private protected override Mesh GetInitialMeshAsset()
		{
			return ShapesMeshUtils.TriangleMesh[0];
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000614D File Offset: 0x0000434D
		private protected override Material[] GetMaterials()
		{
			return new Material[] { ShapesMaterialUtils.matTriangle[base.BlendMode] };
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00006168 File Offset: 0x00004368
		private protected override Bounds GetBounds_Internal()
		{
			Vector3 vector = Vector3.Min(Vector3.Min(this.a, this.b), this.c);
			Vector3 vector2 = Vector3.Max(Vector3.Max(this.a, this.b), this.c);
			return new Bounds((vector + vector2) / 2f, ShapesMath.Abs(vector2 - vector));
		}

		// Token: 0x0400006B RID: 107
		[SerializeField]
		private Triangle.TriangleColorMode colorMode;

		// Token: 0x0400006C RID: 108
		[SerializeField]
		private Vector3 a = Vector3.zero;

		// Token: 0x0400006D RID: 109
		[SerializeField]
		private Vector3 b = Vector3.up;

		// Token: 0x0400006E RID: 110
		[SerializeField]
		private Vector3 c = Vector3.right;

		// Token: 0x0400006F RID: 111
		[SerializeField]
		private bool hollow;

		// Token: 0x04000070 RID: 112
		[SerializeField]
		private float thickness = 0.5f;

		// Token: 0x04000071 RID: 113
		[SerializeField]
		private ThicknessSpace thicknessSpace;

		// Token: 0x04000072 RID: 114
		[SerializeField]
		[Range(0f, 1f)]
		private float roundness;

		// Token: 0x04000073 RID: 115
		[SerializeField]
		[ShapesColorField(true)]
		private Color colorB = Color.white;

		// Token: 0x04000074 RID: 116
		[SerializeField]
		[ShapesColorField(true)]
		private Color colorC = Color.white;

		// Token: 0x0200006F RID: 111
		public enum TriangleColorMode
		{
			// Token: 0x0400026D RID: 621
			Single,
			// Token: 0x0400026E RID: 622
			PerCorner
		}
	}
}
