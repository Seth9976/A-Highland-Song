using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000060 RID: 96
	internal static class ShapesMaterialUtils
	{
		// Token: 0x06000A34 RID: 2612 RVA: 0x00024977 File Offset: 0x00022B77
		public static ShapesMaterials GetDiscMaterial(bool hollow, bool sector)
		{
			if (hollow)
			{
				if (!sector)
				{
					return ShapesMaterialUtils.matRing;
				}
				return ShapesMaterialUtils.matRingSector;
			}
			else
			{
				if (!sector)
				{
					return ShapesMaterialUtils.matDisc;
				}
				return ShapesMaterialUtils.matCircleSector;
			}
		}

		// Token: 0x06000A35 RID: 2613 RVA: 0x0002499C File Offset: 0x00022B9C
		public static ShapesMaterials GetDiscMaterial(DiscType type)
		{
			ShapesMaterialUtils.<>c__DisplayClass75_0 CS$<>8__locals1;
			CS$<>8__locals1.type = type;
			return ShapesMaterialUtils.<GetDiscMaterial>g__Load|75_0(ref CS$<>8__locals1);
		}

		// Token: 0x06000A36 RID: 2614 RVA: 0x000249B8 File Offset: 0x00022BB8
		public static ShapesMaterials GetRectMaterial(bool hollow, bool rounded)
		{
			if (hollow)
			{
				if (!rounded)
				{
					return ShapesMaterialUtils.matRectBorder;
				}
				return ShapesMaterialUtils.matRectBorderRounded;
			}
			else
			{
				if (!rounded)
				{
					return ShapesMaterialUtils.matRectSimple;
				}
				return ShapesMaterialUtils.matRectRounded;
			}
		}

		// Token: 0x06000A37 RID: 2615 RVA: 0x000249DA File Offset: 0x00022BDA
		public static ShapesMaterials GetRectMaterial(Rectangle.RectangleType type)
		{
			switch (type)
			{
			case Rectangle.RectangleType.HardSolid:
				return ShapesMaterialUtils.matRectSimple;
			case Rectangle.RectangleType.RoundedSolid:
				return ShapesMaterialUtils.matRectRounded;
			case Rectangle.RectangleType.HardHollow:
				return ShapesMaterialUtils.matRectBorder;
			case Rectangle.RectangleType.RoundedHollow:
				return ShapesMaterialUtils.matRectBorderRounded;
			default:
				return null;
			}
		}

		// Token: 0x06000A38 RID: 2616 RVA: 0x00024A0D File Offset: 0x00022C0D
		public static ShapesMaterials GetPolylineMat(PolylineJoins join)
		{
			return ShapesMaterialUtils.matsPolyline[(int)join];
		}

		// Token: 0x06000A39 RID: 2617 RVA: 0x00024A16 File Offset: 0x00022C16
		public static ShapesMaterials GetPolylineJoinsMat(PolylineJoins join)
		{
			return ShapesMaterialUtils.matsPolylineJoin[(int)join];
		}

		// Token: 0x06000A3A RID: 2618 RVA: 0x00024A1F File Offset: 0x00022C1F
		public static ShapesMaterials GetLineMat(LineGeometry geometry, LineEndCap cap)
		{
			if (geometry <= LineGeometry.Billboard)
			{
				return ShapesMaterialUtils.matsLine[(int)cap];
			}
			if (geometry != LineGeometry.Volumetric3D)
			{
				throw new ArgumentOutOfRangeException("geometry", geometry, null);
			}
			return ShapesMaterialUtils.matsLine3D[(int)cap];
		}

		// Token: 0x06000A3C RID: 2620 RVA: 0x000250AC File Offset: 0x000232AC
		[CompilerGenerated]
		internal static ShapesMaterials <GetDiscMaterial>g__Load|75_0(ref ShapesMaterialUtils.<>c__DisplayClass75_0 A_0)
		{
			switch (A_0.type)
			{
			case DiscType.Disc:
				return ShapesMaterialUtils.matDisc;
			case DiscType.Pie:
				return ShapesMaterialUtils.matCircleSector;
			case DiscType.Ring:
				return ShapesMaterialUtils.matRing;
			case DiscType.Arc:
				return ShapesMaterialUtils.matRingSector;
			default:
				throw new IndexOutOfRangeException(string.Format("Failed to get disc material, invalid enum index of {0} ", (int)A_0.type));
			}
		}

		// Token: 0x040001F2 RID: 498
		public static readonly int propZTest = Shader.PropertyToID("_ZTest");

		// Token: 0x040001F3 RID: 499
		public static readonly int propZTestTMP = Shader.PropertyToID("unity_GUIZTestMode");

		// Token: 0x040001F4 RID: 500
		public static readonly int propZOffsetFactor = Shader.PropertyToID("_ZOffsetFactor");

		// Token: 0x040001F5 RID: 501
		public static readonly int propZOffsetUnits = Shader.PropertyToID("_ZOffsetUnits");

		// Token: 0x040001F6 RID: 502
		public static readonly int propStencilComp = Shader.PropertyToID("_StencilComp");

		// Token: 0x040001F7 RID: 503
		public static readonly int propStencilOpPass = Shader.PropertyToID("_StencilOpPass");

		// Token: 0x040001F8 RID: 504
		public static readonly int propStencilID = Shader.PropertyToID("_StencilID");

		// Token: 0x040001F9 RID: 505
		public static readonly int propStencilIDTMP = Shader.PropertyToID("_Stencil");

		// Token: 0x040001FA RID: 506
		public static readonly int propStencilReadMask = Shader.PropertyToID("_StencilReadMask");

		// Token: 0x040001FB RID: 507
		public static readonly int propStencilWriteMask = Shader.PropertyToID("_StencilWriteMask");

		// Token: 0x040001FC RID: 508
		public static readonly int propBaseColor = Shader.PropertyToID("_BaseColor");

		// Token: 0x040001FD RID: 509
		public static readonly int propColor = Shader.PropertyToID("_Color");

		// Token: 0x040001FE RID: 510
		public static readonly int propScaleMode = Shader.PropertyToID("_ScaleMode");

		// Token: 0x040001FF RID: 511
		public static readonly int propColorEnd = Shader.PropertyToID("_ColorEnd");

		// Token: 0x04000200 RID: 512
		public static readonly int propColorOuterStart = Shader.PropertyToID("_ColorOuterStart");

		// Token: 0x04000201 RID: 513
		public static readonly int propColorInnerEnd = Shader.PropertyToID("_ColorInnerEnd");

		// Token: 0x04000202 RID: 514
		public static readonly int propColorOuterEnd = Shader.PropertyToID("_ColorOuterEnd");

		// Token: 0x04000203 RID: 515
		public static readonly int propColorB = Shader.PropertyToID("_ColorB");

		// Token: 0x04000204 RID: 516
		public static readonly int propColorC = Shader.PropertyToID("_ColorC");

		// Token: 0x04000205 RID: 517
		public static readonly int propColorD = Shader.PropertyToID("_ColorD");

		// Token: 0x04000206 RID: 518
		public static readonly int propPointStart = Shader.PropertyToID("_PointStart");

		// Token: 0x04000207 RID: 519
		public static readonly int propPointEnd = Shader.PropertyToID("_PointEnd");

		// Token: 0x04000208 RID: 520
		public static readonly int propA = Shader.PropertyToID("_A");

		// Token: 0x04000209 RID: 521
		public static readonly int propB = Shader.PropertyToID("_B");

		// Token: 0x0400020A RID: 522
		public static readonly int propC = Shader.PropertyToID("_C");

		// Token: 0x0400020B RID: 523
		public static readonly int propD = Shader.PropertyToID("_D");

		// Token: 0x0400020C RID: 524
		public static readonly int propRect = Shader.PropertyToID("_Rect");

		// Token: 0x0400020D RID: 525
		public static readonly int propRadius = Shader.PropertyToID("_Radius");

		// Token: 0x0400020E RID: 526
		public static readonly int propCornerRadii = Shader.PropertyToID("_CornerRadii");

		// Token: 0x0400020F RID: 527
		public static readonly int propLength = Shader.PropertyToID("_Length");

		// Token: 0x04000210 RID: 528
		public static readonly int propHollow = Shader.PropertyToID("_Hollow");

		// Token: 0x04000211 RID: 529
		public static readonly int propSides = Shader.PropertyToID("_Sides");

		// Token: 0x04000212 RID: 530
		public static readonly int propAng = Shader.PropertyToID("_Angle");

		// Token: 0x04000213 RID: 531
		public static readonly int propRoundness = Shader.PropertyToID("_Roundness");

		// Token: 0x04000214 RID: 532
		public static readonly int propAngStart = Shader.PropertyToID("_AngleStart");

		// Token: 0x04000215 RID: 533
		public static readonly int propAngEnd = Shader.PropertyToID("_AngleEnd");

		// Token: 0x04000216 RID: 534
		public static readonly int propRoundCaps = Shader.PropertyToID("_RoundCaps");

		// Token: 0x04000217 RID: 535
		public static readonly int propThickness = Shader.PropertyToID("_Thickness");

		// Token: 0x04000218 RID: 536
		public static readonly int propThicknessSpace = Shader.PropertyToID("_ThicknessSpace");

		// Token: 0x04000219 RID: 537
		public static readonly int propRadiusSpace = Shader.PropertyToID("_RadiusSpace");

		// Token: 0x0400021A RID: 538
		public static readonly int propDashSize = Shader.PropertyToID("_DashSize");

		// Token: 0x0400021B RID: 539
		public static readonly int propDashOffset = Shader.PropertyToID("_DashOffset");

		// Token: 0x0400021C RID: 540
		public static readonly int propDashSpacing = Shader.PropertyToID("_DashSpacing");

		// Token: 0x0400021D RID: 541
		public static readonly int propDashType = Shader.PropertyToID("_DashType");

		// Token: 0x0400021E RID: 542
		public static readonly int propDashSpace = Shader.PropertyToID("_DashSpace");

		// Token: 0x0400021F RID: 543
		public static readonly int propDashSnap = Shader.PropertyToID("_DashSnap");

		// Token: 0x04000220 RID: 544
		public static readonly int propDashShapeModifier = Shader.PropertyToID("_DashShapeModifier");

		// Token: 0x04000221 RID: 545
		public static readonly int propSize = Shader.PropertyToID("_Size");

		// Token: 0x04000222 RID: 546
		public static readonly int propSizeSpace = Shader.PropertyToID("_SizeSpace");

		// Token: 0x04000223 RID: 547
		public static readonly int propAlignment = Shader.PropertyToID("_Alignment");

		// Token: 0x04000224 RID: 548
		public static readonly int propFillType = Shader.PropertyToID("_FillType");

		// Token: 0x04000225 RID: 549
		public static readonly int propFillStart = Shader.PropertyToID("_FillStart");

		// Token: 0x04000226 RID: 550
		public static readonly int propFillEnd = Shader.PropertyToID("_FillEnd");

		// Token: 0x04000227 RID: 551
		public static readonly int propFillSpace = Shader.PropertyToID("_FillSpace");

		// Token: 0x04000228 RID: 552
		private static readonly ShapesMaterials matDisc = new ShapesMaterials("Disc", Array.Empty<string>());

		// Token: 0x04000229 RID: 553
		private static readonly ShapesMaterials matCircleSector = new ShapesMaterials("Disc", new string[] { "SECTOR" });

		// Token: 0x0400022A RID: 554
		private static readonly ShapesMaterials matRing = new ShapesMaterials("Disc", new string[] { "INNER_RADIUS" });

		// Token: 0x0400022B RID: 555
		private static readonly ShapesMaterials matRingSector = new ShapesMaterials("Disc", new string[] { "INNER_RADIUS", "SECTOR" });

		// Token: 0x0400022C RID: 556
		private static readonly ShapesMaterials matRectSimple = new ShapesMaterials("Rect", Array.Empty<string>());

		// Token: 0x0400022D RID: 557
		private static readonly ShapesMaterials matRectRounded = new ShapesMaterials("Rect", new string[] { "CORNER_RADIUS" });

		// Token: 0x0400022E RID: 558
		private static readonly ShapesMaterials matRectBorder = new ShapesMaterials("Rect", new string[] { "BORDERED" });

		// Token: 0x0400022F RID: 559
		private static readonly ShapesMaterials matRectBorderRounded = new ShapesMaterials("Rect", new string[] { "CORNER_RADIUS", "BORDERED" });

		// Token: 0x04000230 RID: 560
		public static readonly ShapesMaterials matTriangle = new ShapesMaterials("Triangle", Array.Empty<string>());

		// Token: 0x04000231 RID: 561
		public static readonly ShapesMaterials matQuad = new ShapesMaterials("Quad", Array.Empty<string>());

		// Token: 0x04000232 RID: 562
		public static readonly ShapesMaterials matSphere = new ShapesMaterials("Sphere", Array.Empty<string>());

		// Token: 0x04000233 RID: 563
		public static readonly ShapesMaterials matCone = new ShapesMaterials("Cone", Array.Empty<string>());

		// Token: 0x04000234 RID: 564
		public static readonly ShapesMaterials matCuboid = new ShapesMaterials("Cuboid", Array.Empty<string>());

		// Token: 0x04000235 RID: 565
		public static readonly ShapesMaterials matTorus = new ShapesMaterials("Torus", Array.Empty<string>());

		// Token: 0x04000236 RID: 566
		public static readonly ShapesMaterials matPolygon = new ShapesMaterials("Polygon", Array.Empty<string>());

		// Token: 0x04000237 RID: 567
		public static readonly ShapesMaterials matRegularPolygon = new ShapesMaterials("Regular Polygon", Array.Empty<string>());

		// Token: 0x04000238 RID: 568
		private static readonly ShapesMaterials[] matsLine = new ShapesMaterials[]
		{
			new ShapesMaterials("Line 2D", Array.Empty<string>()),
			new ShapesMaterials("Line 2D", new string[] { "CAP_SQUARE" }),
			new ShapesMaterials("Line 2D", new string[] { "CAP_ROUND" })
		};

		// Token: 0x04000239 RID: 569
		private static readonly ShapesMaterials[] matsLine3D = new ShapesMaterials[]
		{
			new ShapesMaterials("Line 3D", Array.Empty<string>()),
			new ShapesMaterials("Line 3D", new string[] { "CAP_SQUARE" }),
			new ShapesMaterials("Line 3D", new string[] { "CAP_ROUND" })
		};

		// Token: 0x0400023A RID: 570
		private static readonly ShapesMaterials[] matsPolyline = new ShapesMaterials[]
		{
			new ShapesMaterials("Polyline 2D", Array.Empty<string>()),
			new ShapesMaterials("Polyline 2D", new string[] { "JOIN_MITER" }),
			new ShapesMaterials("Polyline 2D", new string[] { "JOIN_ROUND" }),
			new ShapesMaterials("Polyline 2D", new string[] { "JOIN_BEVEL" })
		};

		// Token: 0x0400023B RID: 571
		private static readonly ShapesMaterials[] matsPolylineJoin = new ShapesMaterials[]
		{
			new ShapesMaterials("Polyline 2D", new string[] { "IS_JOIN_MESH" }),
			new ShapesMaterials("Polyline 2D", new string[] { "IS_JOIN_MESH", "JOIN_MITER" }),
			new ShapesMaterials("Polyline 2D", new string[] { "IS_JOIN_MESH", "JOIN_ROUND" }),
			new ShapesMaterials("Polyline 2D", new string[] { "IS_JOIN_MESH", "JOIN_BEVEL" })
		};
	}
}
