using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x0200000E RID: 14
	[ExecuteAlways]
	[AddComponentMenu("Shapes/Sphere")]
	public class Sphere : ShapeRenderer
	{
		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x000059EF File Offset: 0x00003BEF
		// (set) Token: 0x060001A1 RID: 417 RVA: 0x000059F8 File Offset: 0x00003BF8
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

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x00005A24 File Offset: 0x00003C24
		// (set) Token: 0x060001A3 RID: 419 RVA: 0x00005A2C File Offset: 0x00003C2C
		public ThicknessSpace RadiusSpace
		{
			get
			{
				return this.radiusSpace;
			}
			set
			{
				int propRadiusSpace = ShapesMaterialUtils.propRadiusSpace;
				this.radiusSpace = value;
				base.SetIntNow(propRadiusSpace, (int)value);
			}
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00005A4E File Offset: 0x00003C4E
		private protected override void SetAllMaterialProperties()
		{
			base.SetFloat(ShapesMaterialUtils.propRadius, this.radius);
			base.SetInt(ShapesMaterialUtils.propRadiusSpace, (int)this.radiusSpace);
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x00005A72 File Offset: 0x00003C72
		internal override bool HasDetailLevels
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x00005A75 File Offset: 0x00003C75
		internal override bool HasScaleModes
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00005A78 File Offset: 0x00003C78
		private protected override void ShapeClampRanges()
		{
			this.radius = Mathf.Max(0f, this.radius);
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00005A90 File Offset: 0x00003C90
		private protected override Material[] GetMaterials()
		{
			return new Material[] { ShapesMaterialUtils.matSphere[base.BlendMode] };
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00005AAB File Offset: 0x00003CAB
		private protected override Mesh GetInitialMeshAsset()
		{
			return ShapesMeshUtils.SphereMesh[(int)this.detailLevel];
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00005AB9 File Offset: 0x00003CB9
		private protected override Bounds GetBounds_Internal()
		{
			if (this.radiusSpace != ThicknessSpace.Meters)
			{
				return new Bounds(Vector3.zero, Vector3.one);
			}
			return new Bounds(Vector3.zero, Vector3.one * (this.radius * 2f));
		}

		// Token: 0x04000065 RID: 101
		[SerializeField]
		private float radius = 1f;

		// Token: 0x04000066 RID: 102
		[SerializeField]
		private ThicknessSpace radiusSpace;
	}
}
