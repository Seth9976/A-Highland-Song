using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

namespace Shapes
{
	// Token: 0x02000017 RID: 23
	internal struct DrawStyle
	{
		// Token: 0x04000099 RID: 153
		private const float DEFAULT_THICKNESS = 0.05f;

		// Token: 0x0400009A RID: 154
		private const ThicknessSpace DEFAULT_THICKNESS_SPACE = ThicknessSpace.Meters;

		// Token: 0x0400009B RID: 155
		public static DrawStyle @default = new DrawStyle
		{
			color = Color.white,
			renderState = new RenderState
			{
				zTest = CompareFunction.LessEqual,
				zOffsetFactor = 0f,
				zOffsetUnits = 0,
				stencilComp = CompareFunction.Always,
				stencilOpPass = StencilOp.Keep,
				stencilRefID = 0,
				stencilReadMask = byte.MaxValue,
				stencilWriteMask = byte.MaxValue
			},
			blendMode = ShapesBlendMode.Transparent,
			scaleMode = ScaleMode.Uniform,
			detailLevel = DetailLevel.Medium,
			lineThickness = 0.05f,
			lineThicknessSpace = ThicknessSpace.Meters,
			lineDashStyle = DashStyle.DefaultDashStyleLine,
			lineEndCaps = LineEndCap.Round,
			lineGeometry = LineGeometry.Billboard,
			polygonTriangulation = PolygonTriangulation.EarClipping,
			polygonShapeFill = new ShapeFill(),
			polylineGeometry = PolylineGeometry.Billboard,
			polylineJoins = PolylineJoins.Round,
			discGeometry = DiscGeometry.Flat2D,
			discRadius = 1f,
			ringThickness = 0.05f,
			ringThicknessSpace = ThicknessSpace.Meters,
			discRadiusSpace = ThicknessSpace.Meters,
			ringDashStyle = DashStyle.DefaultDashStyleRing,
			regularPolygonRadius = 1f,
			regularPolygonSideCount = 6,
			regularPolygonGeometry = RegularPolygonGeometry.Flat2D,
			regularPolygonThickness = 0.05f,
			regularPolygonThicknessSpace = ThicknessSpace.Meters,
			regularPolygonRadiusSpace = ThicknessSpace.Meters,
			regularPolygonShapeFill = new ShapeFill(),
			rectangleThickness = 0.05f,
			rectangleThicknessSpace = ThicknessSpace.Meters,
			rectangleShapeFill = new ShapeFill(),
			triangleThickness = 0.05f,
			triangleThicknessSpace = ThicknessSpace.Meters,
			sphereRadius = 1f,
			sphereRadiusSpace = ThicknessSpace.Meters,
			cuboidSizeSpace = ThicknessSpace.Meters,
			torusThicknessSpace = ThicknessSpace.Meters,
			torusRadiusSpace = ThicknessSpace.Meters,
			coneSizeSpace = ThicknessSpace.Meters,
			font = ShapesAssets.Instance.defaultFont,
			fontSize = 1f,
			textAlign = TextAlign.Center
		};

		// Token: 0x0400009C RID: 156
		public RenderState renderState;

		// Token: 0x0400009D RID: 157
		public Color color;

		// Token: 0x0400009E RID: 158
		public ShapesBlendMode blendMode;

		// Token: 0x0400009F RID: 159
		public ScaleMode scaleMode;

		// Token: 0x040000A0 RID: 160
		public DetailLevel detailLevel;

		// Token: 0x040000A1 RID: 161
		public float lineThickness;

		// Token: 0x040000A2 RID: 162
		public ThicknessSpace lineThicknessSpace;

		// Token: 0x040000A3 RID: 163
		public LineEndCap lineEndCaps;

		// Token: 0x040000A4 RID: 164
		public LineGeometry lineGeometry;

		// Token: 0x040000A5 RID: 165
		public PolygonTriangulation polygonTriangulation;

		// Token: 0x040000A6 RID: 166
		public ShapeFill polygonShapeFill;

		// Token: 0x040000A7 RID: 167
		public DashStyle lineDashStyle;

		// Token: 0x040000A8 RID: 168
		public DashStyle ringDashStyle;

		// Token: 0x040000A9 RID: 169
		public PolylineGeometry polylineGeometry;

		// Token: 0x040000AA RID: 170
		public PolylineJoins polylineJoins;

		// Token: 0x040000AB RID: 171
		public float discRadius;

		// Token: 0x040000AC RID: 172
		public DiscGeometry discGeometry;

		// Token: 0x040000AD RID: 173
		public float ringThickness;

		// Token: 0x040000AE RID: 174
		public ThicknessSpace ringThicknessSpace;

		// Token: 0x040000AF RID: 175
		public ThicknessSpace discRadiusSpace;

		// Token: 0x040000B0 RID: 176
		public float regularPolygonRadius;

		// Token: 0x040000B1 RID: 177
		public int regularPolygonSideCount;

		// Token: 0x040000B2 RID: 178
		public RegularPolygonGeometry regularPolygonGeometry;

		// Token: 0x040000B3 RID: 179
		public float regularPolygonThickness;

		// Token: 0x040000B4 RID: 180
		public ThicknessSpace regularPolygonThicknessSpace;

		// Token: 0x040000B5 RID: 181
		public ThicknessSpace regularPolygonRadiusSpace;

		// Token: 0x040000B6 RID: 182
		public ShapeFill regularPolygonShapeFill;

		// Token: 0x040000B7 RID: 183
		public float rectangleThickness;

		// Token: 0x040000B8 RID: 184
		public ThicknessSpace rectangleThicknessSpace;

		// Token: 0x040000B9 RID: 185
		public ShapeFill rectangleShapeFill;

		// Token: 0x040000BA RID: 186
		public float triangleThickness;

		// Token: 0x040000BB RID: 187
		public ThicknessSpace triangleThicknessSpace;

		// Token: 0x040000BC RID: 188
		public float sphereRadius;

		// Token: 0x040000BD RID: 189
		public ThicknessSpace sphereRadiusSpace;

		// Token: 0x040000BE RID: 190
		public ThicknessSpace cuboidSizeSpace;

		// Token: 0x040000BF RID: 191
		public ThicknessSpace torusThicknessSpace;

		// Token: 0x040000C0 RID: 192
		public ThicknessSpace torusRadiusSpace;

		// Token: 0x040000C1 RID: 193
		public ThicknessSpace coneSizeSpace;

		// Token: 0x040000C2 RID: 194
		public TMP_FontAsset font;

		// Token: 0x040000C3 RID: 195
		public float fontSize;

		// Token: 0x040000C4 RID: 196
		public TextAlign textAlign;
	}
}
