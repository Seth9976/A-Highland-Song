using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000005 RID: 5
	[ExecuteAlways]
	[AddComponentMenu("Shapes/Line")]
	public class Line : ShapeRenderer
	{
		// Token: 0x1700002C RID: 44
		public Vector3 this[int i]
		{
			get
			{
				if (i <= 0)
				{
					return this.Start;
				}
				return this.End;
			}
			set
			{
				if (i <= 0)
				{
					this.Start = value;
					return;
				}
				this.End = value;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000064 RID: 100 RVA: 0x00002C78 File Offset: 0x00000E78
		// (set) Token: 0x06000065 RID: 101 RVA: 0x00002C80 File Offset: 0x00000E80
		public LineGeometry Geometry
		{
			get
			{
				return this.geometry;
			}
			set
			{
				this.geometry = value;
				base.SetIntNow(ShapesMaterialUtils.propAlignment, (int)this.geometry);
				base.UpdateMesh(true);
				base.UpdateMaterial();
				base.ApplyProperties();
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000066 RID: 102 RVA: 0x00002CAD File Offset: 0x00000EAD
		// (set) Token: 0x06000067 RID: 103 RVA: 0x00002CB8 File Offset: 0x00000EB8
		public Line.LineColorMode ColorMode
		{
			get
			{
				return this.colorMode;
			}
			set
			{
				int propColorEnd = ShapesMaterialUtils.propColorEnd;
				this.colorMode = value;
				base.SetColorNow(propColorEnd, (value == Line.LineColorMode.Double) ? this.colorEnd : base.Color);
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000068 RID: 104 RVA: 0x00002CEB File Offset: 0x00000EEB
		// (set) Token: 0x06000069 RID: 105 RVA: 0x00002CF4 File Offset: 0x00000EF4
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
				int propColorEnd = ShapesMaterialUtils.propColorEnd;
				this.colorEnd = value;
				base.SetColorNow(propColorEnd, value);
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600006A RID: 106 RVA: 0x00002D2B File Offset: 0x00000F2B
		// (set) Token: 0x0600006B RID: 107 RVA: 0x00002D34 File Offset: 0x00000F34
		public Color ColorStart
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

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600006C RID: 108 RVA: 0x00002D56 File Offset: 0x00000F56
		// (set) Token: 0x0600006D RID: 109 RVA: 0x00002D60 File Offset: 0x00000F60
		public Color ColorEnd
		{
			get
			{
				return this.colorEnd;
			}
			set
			{
				int propColorEnd = ShapesMaterialUtils.propColorEnd;
				this.colorEnd = value;
				base.SetColorNow(propColorEnd, value);
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600006E RID: 110 RVA: 0x00002D82 File Offset: 0x00000F82
		// (set) Token: 0x0600006F RID: 111 RVA: 0x00002D8C File Offset: 0x00000F8C
		public Vector3 Start
		{
			get
			{
				return this.start;
			}
			set
			{
				int propPointStart = ShapesMaterialUtils.propPointStart;
				this.start = value;
				base.SetVector3Now(propPointStart, value);
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000070 RID: 112 RVA: 0x00002DAE File Offset: 0x00000FAE
		// (set) Token: 0x06000071 RID: 113 RVA: 0x00002DB8 File Offset: 0x00000FB8
		public Vector3 End
		{
			get
			{
				return this.end;
			}
			set
			{
				int propPointEnd = ShapesMaterialUtils.propPointEnd;
				this.end = value;
				base.SetVector3Now(propPointEnd, value);
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000072 RID: 114 RVA: 0x00002DDA File Offset: 0x00000FDA
		// (set) Token: 0x06000073 RID: 115 RVA: 0x00002DE4 File Offset: 0x00000FE4
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
				if (this.dashed && this.dashStyle.space == DashSpace.Relative)
				{
					this.SetAllDashValues(true);
				}
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000074 RID: 116 RVA: 0x00002E23 File Offset: 0x00001023
		// (set) Token: 0x06000075 RID: 117 RVA: 0x00002E2C File Offset: 0x0000102C
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

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000076 RID: 118 RVA: 0x00002E4E File Offset: 0x0000104E
		// (set) Token: 0x06000077 RID: 119 RVA: 0x00002E56 File Offset: 0x00001056
		public LineEndCap EndCaps
		{
			get
			{
				return this.endCaps;
			}
			set
			{
				this.endCaps = value;
				base.UpdateMaterial();
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000078 RID: 120 RVA: 0x00002E65 File Offset: 0x00001065
		// (set) Token: 0x06000079 RID: 121 RVA: 0x00002E6D File Offset: 0x0000106D
		public bool MatchDashSpacingToSize
		{
			get
			{
				return this.matchDashSpacingToSize;
			}
			set
			{
				this.matchDashSpacingToSize = value;
				this.SetAllDashValues(true);
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00002E7D File Offset: 0x0000107D
		// (set) Token: 0x0600007B RID: 123 RVA: 0x00002E85 File Offset: 0x00001085
		public bool Dashed
		{
			get
			{
				return this.dashed;
			}
			set
			{
				this.dashed = value;
				this.SetAllDashValues(true);
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600007C RID: 124 RVA: 0x00002E95 File Offset: 0x00001095
		// (set) Token: 0x0600007D RID: 125 RVA: 0x00002EA4 File Offset: 0x000010A4
		public float DashSize
		{
			get
			{
				return this.dashStyle.size;
			}
			set
			{
				this.dashStyle.size = value;
				float netAbsoluteSize = this.dashStyle.GetNetAbsoluteSize(this.dashed, this.thickness);
				if (this.matchDashSpacingToSize)
				{
					base.SetFloat(ShapesMaterialUtils.propDashSpacing, this.GetNetDashSpacing());
				}
				base.SetFloatNow(ShapesMaterialUtils.propDashSize, netAbsoluteSize);
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600007E RID: 126 RVA: 0x00002EFA File Offset: 0x000010FA
		// (set) Token: 0x0600007F RID: 127 RVA: 0x00002F07 File Offset: 0x00001107
		public float DashSpacing
		{
			get
			{
				return this.dashStyle.spacing;
			}
			set
			{
				this.dashStyle.spacing = value;
				base.SetFloatNow(ShapesMaterialUtils.propDashSpacing, this.GetNetDashSpacing());
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00002F26 File Offset: 0x00001126
		// (set) Token: 0x06000081 RID: 129 RVA: 0x00002F34 File Offset: 0x00001134
		public float DashOffset
		{
			get
			{
				return this.dashStyle.offset;
			}
			set
			{
				int propDashOffset = ShapesMaterialUtils.propDashOffset;
				this.dashStyle.offset = value;
				base.SetFloatNow(propDashOffset, value);
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00002F5B File Offset: 0x0000115B
		// (set) Token: 0x06000083 RID: 131 RVA: 0x00002F68 File Offset: 0x00001168
		public DashSpace DashSpace
		{
			get
			{
				return this.dashStyle.space;
			}
			set
			{
				int propDashSpace = ShapesMaterialUtils.propDashSpace;
				this.dashStyle.space = value;
				base.SetInt(propDashSpace, (int)value);
				base.SetFloatNow(ShapesMaterialUtils.propDashSize, this.dashStyle.GetNetAbsoluteSize(this.dashed, this.thickness));
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000084 RID: 132 RVA: 0x00002FB1 File Offset: 0x000011B1
		// (set) Token: 0x06000085 RID: 133 RVA: 0x00002FC0 File Offset: 0x000011C0
		public DashSnapping DashSnap
		{
			get
			{
				return this.dashStyle.snap;
			}
			set
			{
				int propDashSnap = ShapesMaterialUtils.propDashSnap;
				this.dashStyle.snap = value;
				base.SetIntNow(propDashSnap, (int)value);
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000086 RID: 134 RVA: 0x00002FE7 File Offset: 0x000011E7
		// (set) Token: 0x06000087 RID: 135 RVA: 0x00002FF4 File Offset: 0x000011F4
		public DashType DashType
		{
			get
			{
				return this.dashStyle.type;
			}
			set
			{
				int propDashType = ShapesMaterialUtils.propDashType;
				this.dashStyle.type = value;
				base.SetIntNow(propDashType, (int)value);
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000088 RID: 136 RVA: 0x0000301B File Offset: 0x0000121B
		// (set) Token: 0x06000089 RID: 137 RVA: 0x00003028 File Offset: 0x00001228
		public float DashShapeModifier
		{
			get
			{
				return this.dashStyle.shapeModifier;
			}
			set
			{
				int propDashShapeModifier = ShapesMaterialUtils.propDashShapeModifier;
				this.dashStyle.shapeModifier = value;
				base.SetFloatNow(propDashShapeModifier, value);
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x0000304F File Offset: 0x0000124F
		private void SetAllDashValues(bool now)
		{
			base.SetAllDashValues(this.dashStyle, this.Dashed, this.matchDashSpacingToSize, this.thickness, this.Geometry != LineGeometry.Volumetric3D, now);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x0000307C File Offset: 0x0000127C
		private float GetNetDashSpacing()
		{
			return base.GetNetDashSpacing(this.dashStyle, this.dashed, this.matchDashSpacingToSize, this.thickness);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x0000309C File Offset: 0x0000129C
		private protected override void SetAllMaterialProperties()
		{
			base.SetVector3(ShapesMaterialUtils.propPointStart, this.start);
			base.SetVector3(ShapesMaterialUtils.propPointEnd, this.end);
			base.SetFloat(ShapesMaterialUtils.propThickness, this.thickness);
			base.SetInt(ShapesMaterialUtils.propThicknessSpace, (int)this.thicknessSpace);
			base.SetInt(ShapesMaterialUtils.propAlignment, (int)this.geometry);
			base.SetColor(ShapesMaterialUtils.propColorEnd, (this.colorMode == Line.LineColorMode.Double) ? this.colorEnd : base.Color);
			this.SetAllDashValues(false);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00003128 File Offset: 0x00001328
		private protected override Bounds GetBounds_Internal()
		{
			float num = ((this.thicknessSpace == ThicknessSpace.Meters) ? this.thickness : 0f);
			Vector3 vector = (this.start + this.end) / 2f;
			Vector3 vector2 = ShapesMath.Abs(this.start - this.end) + new Vector3(num, num, num);
			return new Bounds(vector, vector2);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00003190 File Offset: 0x00001390
		private protected override Material[] GetMaterials()
		{
			return new Material[] { ShapesMaterialUtils.GetLineMat(this.geometry, this.endCaps)[base.BlendMode] };
		}

		// Token: 0x0600008F RID: 143 RVA: 0x000031B7 File Offset: 0x000013B7
		private protected override Mesh GetInitialMeshAsset()
		{
			return ShapesMeshUtils.GetLineMesh(this.geometry, this.endCaps, this.detailLevel);
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000090 RID: 144 RVA: 0x000031D0 File Offset: 0x000013D0
		internal override bool HasDetailLevels
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000091 RID: 145 RVA: 0x000031D4 File Offset: 0x000013D4
		private protected override void ShapeClampRanges()
		{
			this.thickness = Mathf.Max(0f, this.thickness);
			this.DashSpacing = ((this.DashSpace == DashSpace.FixedCount) ? Mathf.Clamp01(this.DashSpacing) : Mathf.Max(0f, this.DashSpacing));
		}

		// Token: 0x04000018 RID: 24
		[SerializeField]
		private LineGeometry geometry = LineGeometry.Billboard;

		// Token: 0x04000019 RID: 25
		[SerializeField]
		private Line.LineColorMode colorMode;

		// Token: 0x0400001A RID: 26
		[SerializeField]
		[ShapesColorField(true)]
		private Color colorEnd = Color.white;

		// Token: 0x0400001B RID: 27
		[SerializeField]
		private Vector3 start = Vector3.zero;

		// Token: 0x0400001C RID: 28
		[SerializeField]
		private Vector3 end = Vector3.right;

		// Token: 0x0400001D RID: 29
		[SerializeField]
		private float thickness = 0.125f;

		// Token: 0x0400001E RID: 30
		[SerializeField]
		private ThicknessSpace thicknessSpace;

		// Token: 0x0400001F RID: 31
		[SerializeField]
		private LineEndCap endCaps = LineEndCap.Round;

		// Token: 0x04000020 RID: 32
		[SerializeField]
		private bool matchDashSpacingToSize = true;

		// Token: 0x04000021 RID: 33
		[SerializeField]
		private bool dashed;

		// Token: 0x04000022 RID: 34
		[SerializeField]
		private DashStyle dashStyle = DashStyle.DefaultDashStyleLine;

		// Token: 0x02000068 RID: 104
		public enum LineColorMode
		{
			// Token: 0x04000253 RID: 595
			Single,
			// Token: 0x04000254 RID: 596
			Double
		}
	}
}
