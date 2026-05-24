using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x0200000A RID: 10
	[ExecuteAlways]
	[AddComponentMenu("Shapes/RegularPolygon")]
	public class RegularPolygon : ShapeRendererFillable
	{
		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000114 RID: 276 RVA: 0x00004632 File Offset: 0x00002832
		// (set) Token: 0x06000115 RID: 277 RVA: 0x0000463C File Offset: 0x0000283C
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

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000116 RID: 278 RVA: 0x00004663 File Offset: 0x00002863
		// (set) Token: 0x06000117 RID: 279 RVA: 0x0000466C File Offset: 0x0000286C
		public int Sides
		{
			get
			{
				return this.sides;
			}
			set
			{
				base.SetIntNow(ShapesMaterialUtils.propSides, this.sides = Mathf.Max(3, value));
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000118 RID: 280 RVA: 0x00004694 File Offset: 0x00002894
		// (set) Token: 0x06000119 RID: 281 RVA: 0x0000469C File Offset: 0x0000289C
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

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600011A RID: 282 RVA: 0x000046C3 File Offset: 0x000028C3
		// (set) Token: 0x0600011B RID: 283 RVA: 0x000046CC File Offset: 0x000028CC
		public float Angle
		{
			get
			{
				return this.angle;
			}
			set
			{
				int propAng = ShapesMaterialUtils.propAng;
				this.angle = value;
				base.SetFloatNow(propAng, value);
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600011C RID: 284 RVA: 0x000046EE File Offset: 0x000028EE
		// (set) Token: 0x0600011D RID: 285 RVA: 0x000046F8 File Offset: 0x000028F8
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

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x0600011E RID: 286 RVA: 0x00004724 File Offset: 0x00002924
		// (set) Token: 0x0600011F RID: 287 RVA: 0x0000472C File Offset: 0x0000292C
		public RegularPolygonGeometry Geometry
		{
			get
			{
				return this.geometry;
			}
			set
			{
				int propAlignment = ShapesMaterialUtils.propAlignment;
				this.geometry = value;
				base.SetIntNow(propAlignment, (int)value);
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000120 RID: 288 RVA: 0x0000474E File Offset: 0x0000294E
		// (set) Token: 0x06000121 RID: 289 RVA: 0x00004758 File Offset: 0x00002958
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

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000122 RID: 290 RVA: 0x0000477A File Offset: 0x0000297A
		// (set) Token: 0x06000123 RID: 291 RVA: 0x00004784 File Offset: 0x00002984
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

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000124 RID: 292 RVA: 0x000047B0 File Offset: 0x000029B0
		// (set) Token: 0x06000125 RID: 293 RVA: 0x000047B8 File Offset: 0x000029B8
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

		// Token: 0x06000126 RID: 294 RVA: 0x000047DC File Offset: 0x000029DC
		private protected override void SetAllMaterialProperties()
		{
			base.SetFillProperties();
			base.SetIntNow(ShapesMaterialUtils.propHollow, this.hollow.AsInt());
			base.SetInt(ShapesMaterialUtils.propAlignment, (int)this.geometry);
			base.SetFloat(ShapesMaterialUtils.propRadius, this.radius);
			base.SetInt(ShapesMaterialUtils.propRadiusSpace, (int)this.radiusSpace);
			base.SetFloat(ShapesMaterialUtils.propThickness, this.thickness);
			base.SetInt(ShapesMaterialUtils.propThicknessSpace, (int)this.thicknessSpace);
			base.SetFloat(ShapesMaterialUtils.propAng, this.angle);
			base.SetFloat(ShapesMaterialUtils.propSides, (float)this.sides);
			base.SetFloat(ShapesMaterialUtils.propRoundness, this.roundness);
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000127 RID: 295 RVA: 0x0000488E File Offset: 0x00002A8E
		internal override bool HasDetailLevels
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00004891 File Offset: 0x00002A91
		private protected override Material[] GetMaterials()
		{
			return new Material[] { ShapesMaterialUtils.matRegularPolygon[base.BlendMode] };
		}

		// Token: 0x06000129 RID: 297 RVA: 0x000048AC File Offset: 0x00002AAC
		private protected override Bounds GetBounds_Internal()
		{
			if (this.radiusSpace != ThicknessSpace.Meters)
			{
				return new Bounds(Vector3.zero, Vector3.one);
			}
			float num = ((this.thicknessSpace == ThicknessSpace.Meters) ? (this.thickness * 0.5f) : 0f);
			float num2 = (this.hollow ? (this.radius + num) : this.radius) * 2f;
			return new Bounds(Vector3.zero, new Vector3(num2, num2, 0f));
		}

		// Token: 0x0400003C RID: 60
		[SerializeField]
		private bool hollow;

		// Token: 0x0400003D RID: 61
		[SerializeField]
		private int sides = 3;

		// Token: 0x0400003E RID: 62
		[SerializeField]
		[Range(0f, 1f)]
		private float roundness;

		// Token: 0x0400003F RID: 63
		[SerializeField]
		private float angle = 1.5707964f;

		// Token: 0x04000040 RID: 64
		[SerializeField]
		private float radius = 1f;

		// Token: 0x04000041 RID: 65
		[SerializeField]
		private AngularUnit angUnitInput = AngularUnit.Degrees;

		// Token: 0x04000042 RID: 66
		[SerializeField]
		private RegularPolygonGeometry geometry;

		// Token: 0x04000043 RID: 67
		[SerializeField]
		private ThicknessSpace radiusSpace;

		// Token: 0x04000044 RID: 68
		[SerializeField]
		private float thickness = 0.5f;

		// Token: 0x04000045 RID: 69
		[SerializeField]
		private ThicknessSpace thicknessSpace;
	}
}
