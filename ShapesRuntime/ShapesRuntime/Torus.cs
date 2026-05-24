using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x0200000F RID: 15
	[ExecuteAlways]
	[AddComponentMenu("Shapes/Torus")]
	public class Torus : ShapeRenderer
	{
		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060001AC RID: 428 RVA: 0x00005B06 File Offset: 0x00003D06
		// (set) Token: 0x060001AD RID: 429 RVA: 0x00005B10 File Offset: 0x00003D10
		public float Radius
		{
			get
			{
				return this.radius;
			}
			set
			{
				base.SetFloatNow(ShapesMaterialUtils.propRadius, this.radius = Mathf.Max(0f, value));
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060001AE RID: 430 RVA: 0x00005B3C File Offset: 0x00003D3C
		// (set) Token: 0x060001AF RID: 431 RVA: 0x00005B44 File Offset: 0x00003D44
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

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x00005B70 File Offset: 0x00003D70
		// (set) Token: 0x060001B1 RID: 433 RVA: 0x00005B78 File Offset: 0x00003D78
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

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060001B2 RID: 434 RVA: 0x00005B9A File Offset: 0x00003D9A
		// (set) Token: 0x060001B3 RID: 435 RVA: 0x00005BA4 File Offset: 0x00003DA4
		public ThicknessSpace RadiusSpace
		{
			get
			{
				return this.radiusSpace;
			}
			set
			{
				int propThicknessSpace = ShapesMaterialUtils.propThicknessSpace;
				this.radiusSpace = value;
				base.SetIntNow(propThicknessSpace, (int)value);
			}
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00005BC8 File Offset: 0x00003DC8
		private protected override void SetAllMaterialProperties()
		{
			base.SetFloat(ShapesMaterialUtils.propRadius, this.radius);
			base.SetFloat(ShapesMaterialUtils.propThickness, this.thickness);
			base.SetInt(ShapesMaterialUtils.propThicknessSpace, (int)this.thicknessSpace);
			base.SetInt(ShapesMaterialUtils.propRadiusSpace, (int)this.radiusSpace);
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00005C19 File Offset: 0x00003E19
		private protected override void ShapeClampRanges()
		{
			this.radius = Mathf.Max(0f, this.radius);
			this.thickness = Mathf.Max(0f, this.thickness);
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x00005C47 File Offset: 0x00003E47
		internal override bool HasDetailLevels
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00005C4A File Offset: 0x00003E4A
		private protected override Material[] GetMaterials()
		{
			return new Material[] { ShapesMaterialUtils.matTorus[base.BlendMode] };
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00005C65 File Offset: 0x00003E65
		private protected override Mesh GetInitialMeshAsset()
		{
			return ShapesMeshUtils.TorusMesh[(int)this.detailLevel];
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00005C74 File Offset: 0x00003E74
		private protected override Bounds GetBounds_Internal()
		{
			if (this.radiusSpace != ThicknessSpace.Meters)
			{
				return new Bounds(default(Vector3), Vector3.one);
			}
			float num = ((this.thicknessSpace == ThicknessSpace.Meters) ? this.thickness : 0f);
			float num2 = this.radius * 2f + num;
			return new Bounds(Vector3.zero, new Vector3(num2, num2, num));
		}

		// Token: 0x04000067 RID: 103
		[SerializeField]
		private float radius = 1f;

		// Token: 0x04000068 RID: 104
		[SerializeField]
		private float thickness = 0.5f;

		// Token: 0x04000069 RID: 105
		[SerializeField]
		private ThicknessSpace thicknessSpace;

		// Token: 0x0400006A RID: 106
		[SerializeField]
		private ThicknessSpace radiusSpace;
	}
}
