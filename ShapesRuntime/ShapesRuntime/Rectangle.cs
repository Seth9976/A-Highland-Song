using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000009 RID: 9
	[ExecuteAlways]
	[AddComponentMenu("Shapes/Rectangle")]
	public class Rectangle : ShapeRendererFillable
	{
		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x00004298 File Offset: 0x00002498
		public bool IsHollow
		{
			get
			{
				return this.type == Rectangle.RectangleType.HardHollow || this.type == Rectangle.RectangleType.RoundedHollow;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x000042AE File Offset: 0x000024AE
		public bool IsRounded
		{
			get
			{
				return this.type == Rectangle.RectangleType.RoundedSolid || this.type == Rectangle.RectangleType.RoundedHollow;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x000042C4 File Offset: 0x000024C4
		// (set) Token: 0x060000F9 RID: 249 RVA: 0x000042CC File Offset: 0x000024CC
		public RectPivot Pivot
		{
			get
			{
				return this.pivot;
			}
			set
			{
				this.pivot = value;
				this.UpdateRectPositioningNow();
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060000FA RID: 250 RVA: 0x000042DB File Offset: 0x000024DB
		// (set) Token: 0x060000FB RID: 251 RVA: 0x000042E3 File Offset: 0x000024E3
		public float Width
		{
			get
			{
				return this.width;
			}
			set
			{
				this.width = value;
				this.UpdateRectPositioningNow();
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060000FC RID: 252 RVA: 0x000042F2 File Offset: 0x000024F2
		// (set) Token: 0x060000FD RID: 253 RVA: 0x000042FA File Offset: 0x000024FA
		public float Height
		{
			get
			{
				return this.height;
			}
			set
			{
				this.height = value;
				this.UpdateRectPositioningNow();
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060000FE RID: 254 RVA: 0x00004309 File Offset: 0x00002509
		// (set) Token: 0x060000FF RID: 255 RVA: 0x00004311 File Offset: 0x00002511
		public Rectangle.RectangleType Type
		{
			get
			{
				return this.type;
			}
			set
			{
				this.type = value;
				base.UpdateMaterial();
				base.ApplyProperties();
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000100 RID: 256 RVA: 0x00004326 File Offset: 0x00002526
		// (set) Token: 0x06000101 RID: 257 RVA: 0x0000432E File Offset: 0x0000252E
		public Rectangle.RectangleCornerRadiusMode CornerRadiusMode
		{
			get
			{
				return this.cornerRadiusMode;
			}
			set
			{
				this.cornerRadiusMode = value;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000102 RID: 258 RVA: 0x00004337 File Offset: 0x00002537
		// (set) Token: 0x06000103 RID: 259 RVA: 0x0000433F File Offset: 0x0000253F
		[Obsolete("Radius is deprecated, please use CornerRadius instead", true)]
		public float Radius
		{
			get
			{
				return this.CornerRadius;
			}
			set
			{
				this.CornerRadius = value;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000104 RID: 260 RVA: 0x00004348 File Offset: 0x00002548
		// (set) Token: 0x06000105 RID: 261 RVA: 0x00004358 File Offset: 0x00002558
		public float CornerRadius
		{
			get
			{
				return this.cornerRadii.x;
			}
			set
			{
				float num = Mathf.Max(0f, value);
				base.SetVector4Now(ShapesMaterialUtils.propCornerRadii, this.cornerRadii = new Vector4(num, num, num, num));
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000106 RID: 262 RVA: 0x0000438E File Offset: 0x0000258E
		// (set) Token: 0x06000107 RID: 263 RVA: 0x00004398 File Offset: 0x00002598
		public Vector4 CornerRadiii
		{
			get
			{
				return this.cornerRadii;
			}
			set
			{
				base.SetVector4Now(ShapesMaterialUtils.propCornerRadii, this.cornerRadii = new Vector4(Mathf.Max(0f, value.x), Mathf.Max(0f, value.y), Mathf.Max(0f, value.z), Mathf.Max(0f, value.w)));
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000108 RID: 264 RVA: 0x000043FE File Offset: 0x000025FE
		// (set) Token: 0x06000109 RID: 265 RVA: 0x00004408 File Offset: 0x00002608
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

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600010A RID: 266 RVA: 0x00004434 File Offset: 0x00002634
		// (set) Token: 0x0600010B RID: 267 RVA: 0x0000443C File Offset: 0x0000263C
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

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600010C RID: 268 RVA: 0x0000445E File Offset: 0x0000265E
		internal override bool HasDetailLevels
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00004461 File Offset: 0x00002661
		private void UpdateRectPositioningNow()
		{
			base.SetVector4Now(ShapesMaterialUtils.propRect, this.GetPositioningRect());
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00004474 File Offset: 0x00002674
		private void UpdateRectPositioning()
		{
			base.SetVector4(ShapesMaterialUtils.propRect, this.GetPositioningRect());
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00004488 File Offset: 0x00002688
		private Vector4 GetPositioningRect()
		{
			float num = ((this.pivot == RectPivot.Corner) ? 0f : (-this.width / 2f));
			float num2 = ((this.pivot == RectPivot.Corner) ? 0f : (-this.height / 2f));
			return new Vector4(num, num2, this.width, this.height);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x000044E0 File Offset: 0x000026E0
		private protected override void SetAllMaterialProperties()
		{
			if (this.cornerRadiusMode == Rectangle.RectangleCornerRadiusMode.PerCorner)
			{
				base.SetVector4(ShapesMaterialUtils.propCornerRadii, this.cornerRadii);
			}
			else if (this.cornerRadiusMode == Rectangle.RectangleCornerRadiusMode.Uniform)
			{
				base.SetVector4(ShapesMaterialUtils.propCornerRadii, new Vector4(this.CornerRadius, this.CornerRadius, this.CornerRadius, this.CornerRadius));
			}
			this.UpdateRectPositioning();
			base.SetFloat(ShapesMaterialUtils.propThickness, this.thickness);
			base.SetIntNow(ShapesMaterialUtils.propThicknessSpace, (int)this.thicknessSpace);
			base.SetFillProperties();
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00004567 File Offset: 0x00002767
		private protected override Material[] GetMaterials()
		{
			return new Material[] { ShapesMaterialUtils.GetRectMaterial(this.type)[base.BlendMode] };
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00004588 File Offset: 0x00002788
		private protected override Bounds GetBounds_Internal()
		{
			Vector2 vector = new Vector2(this.width, this.height);
			return new Bounds((this.pivot == RectPivot.Center) ? default(Vector2) : (vector / 2f), vector);
		}

		// Token: 0x04000034 RID: 52
		[SerializeField]
		private RectPivot pivot = RectPivot.Center;

		// Token: 0x04000035 RID: 53
		[SerializeField]
		private float width = 1f;

		// Token: 0x04000036 RID: 54
		[SerializeField]
		private float height = 1f;

		// Token: 0x04000037 RID: 55
		[SerializeField]
		private Rectangle.RectangleType type;

		// Token: 0x04000038 RID: 56
		[SerializeField]
		private Rectangle.RectangleCornerRadiusMode cornerRadiusMode;

		// Token: 0x04000039 RID: 57
		[SerializeField]
		private Vector4 cornerRadii = new Vector4(0.25f, 0.25f, 0.25f, 0.25f);

		// Token: 0x0400003A RID: 58
		[SerializeField]
		private float thickness = 0.1f;

		// Token: 0x0400003B RID: 59
		[SerializeField]
		private ThicknessSpace thicknessSpace;

		// Token: 0x0200006B RID: 107
		public enum RectangleType
		{
			// Token: 0x04000261 RID: 609
			HardSolid,
			// Token: 0x04000262 RID: 610
			RoundedSolid,
			// Token: 0x04000263 RID: 611
			HardHollow,
			// Token: 0x04000264 RID: 612
			RoundedHollow
		}

		// Token: 0x0200006C RID: 108
		public enum RectangleCornerRadiusMode
		{
			// Token: 0x04000266 RID: 614
			Uniform,
			// Token: 0x04000267 RID: 615
			PerCorner
		}
	}
}
