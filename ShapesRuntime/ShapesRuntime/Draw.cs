using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

namespace Shapes
{
	// Token: 0x02000013 RID: 19
	public static class Draw
	{
		// Token: 0x060001EC RID: 492 RVA: 0x0000639C File Offset: 0x0000459C
		public static DrawCommand Command(Camera cam, CameraEvent cameraEvent = CameraEvent.BeforeImageEffects)
		{
			return ObjectPool<DrawCommand>.Alloc().Initialize(cam, cameraEvent);
		}

		// Token: 0x060001ED RID: 493 RVA: 0x000063AC File Offset: 0x000045AC
		[OvldGenCallTarget]
		private static void Line_Internal([OvldDefault("LineEndCaps")] LineEndCap endCaps, [OvldDefault("LineThicknessSpace")] ThicknessSpace thicknessSpace, Vector3 start, Vector3 end, [OvldDefault("Color")] Color colorStart, [OvldDefault("Color")] Color colorEnd, [OvldDefault("LineThickness")] float thickness, [OvldDefault("LineDashStyle")] DashStyle dashStyle = null)
		{
			using (new IMDrawer(Draw.mpbLine, ShapesMaterialUtils.GetLineMat(Draw.LineGeometry, endCaps)[Draw.BlendMode], ShapesMeshUtils.GetLineMesh(Draw.LineGeometry, endCaps, Draw.DetailLevel), 0, false))
			{
				MetaMpb.ApplyDashSettings<MpbLine>(Draw.mpbLine, dashStyle, thickness);
				Draw.mpbLine.color.Add(colorStart.ColorSpaceAdjusted());
				Draw.mpbLine.colorEnd.Add(colorEnd.ColorSpaceAdjusted());
				Draw.mpbLine.pointStart.Add(start);
				Draw.mpbLine.pointEnd.Add(end);
				Draw.mpbLine.thickness.Add(thickness);
				Draw.mpbLine.alignment.Add((float)Draw.LineGeometry);
				Draw.mpbLine.thicknessSpace.Add((float)thicknessSpace);
				Draw.mpbLine.scaleMode.Add((float)Draw.ScaleMode);
			}
		}

		// Token: 0x060001EE RID: 494 RVA: 0x000064C8 File Offset: 0x000046C8
		[OvldGenCallTarget]
		private static void Polyline_Internal(PolylinePath path, [OvldDefault("false")] bool closed, [OvldDefault("PolylineGeometry")] PolylineGeometry geometry, [OvldDefault("PolylineJoins")] PolylineJoins joins, [OvldDefault("LineThickness")] float thickness, [OvldDefault("LineThicknessSpace")] ThicknessSpace thicknessSpace, [OvldDefault("Color")] Color color)
		{
			Draw.<>c__DisplayClass5_0 CS$<>8__locals1;
			CS$<>8__locals1.thickness = thickness;
			CS$<>8__locals1.thicknessSpace = thicknessSpace;
			CS$<>8__locals1.color = color;
			CS$<>8__locals1.geometry = geometry;
			Mesh mesh;
			if (!path.EnsureMeshIsReadyToRender(closed, joins, out mesh))
			{
				return;
			}
			int count = path.Count;
			if (count == 0)
			{
				Debug.LogWarning("Tried to draw polyline with no points");
				return;
			}
			if (count != 1)
			{
				if (DrawCommand.IsAddingDrawCommandsToBuffer)
				{
					path.lastCommandUsedIn = DrawCommand.CurrentWritingCommandBuffer;
				}
				using (new IMDrawer(Draw.mpbPolyline, ShapesMaterialUtils.GetPolylineMat(joins)[Draw.BlendMode], mesh, 0, false))
				{
					Draw.<Polyline_Internal>g__ApplyToMpb|5_0(Draw.mpbPolyline, ref CS$<>8__locals1);
				}
				if (joins.HasJoinMesh())
				{
					using (new IMDrawer(Draw.mpbPolylineJoins, ShapesMaterialUtils.GetPolylineJoinsMat(joins)[Draw.BlendMode], mesh, 1, false))
					{
						Draw.<Polyline_Internal>g__ApplyToMpb|5_0(Draw.mpbPolylineJoins, ref CS$<>8__locals1);
					}
				}
				return;
			}
			Debug.LogWarning("Tried to draw polyline with only one point");
		}

		// Token: 0x060001EF RID: 495 RVA: 0x000065D8 File Offset: 0x000047D8
		[OvldGenCallTarget]
		private static void Polygon_Internal(PolygonPath path, [OvldDefault("PolygonTriangulation")] PolygonTriangulation triangulation, [OvldDefault("Color")] Color color, [OvldDefault("PolygonShapeFill")] ShapeFill fill)
		{
			Mesh mesh;
			if (!path.EnsureMeshIsReadyToRender(triangulation, out mesh))
			{
				return;
			}
			switch (path.Count)
			{
			case 0:
				Debug.LogWarning("Tried to draw polygon with no points");
				return;
			case 1:
				Debug.LogWarning("Tried to draw polygon with only one point");
				return;
			case 2:
				Debug.LogWarning("Tried to draw polygon with only two points");
				return;
			default:
				if (DrawCommand.IsAddingDrawCommandsToBuffer)
				{
					path.lastCommandUsedIn = DrawCommand.CurrentWritingCommandBuffer;
				}
				using (new IMDrawer(Draw.mpbPolygon, ShapesMaterialUtils.matPolygon[Draw.BlendMode], mesh, 0, false))
				{
					MetaMpb.ApplyColorOrFill<MpbPolygon>(Draw.mpbPolygon, fill, color);
				}
				return;
			}
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000668C File Offset: 0x0000488C
		[OvldGenCallTarget]
		private static void Disc_Internal([OvldDefault("DiscRadius")] float radius, [OvldDefault("Color")] Color colorInnerStart, [OvldDefault("Color")] Color colorOuterStart, [OvldDefault("Color")] Color colorInnerEnd, [OvldDefault("Color")] Color colorOuterEnd)
		{
			Draw.DiscCore(false, false, radius, 0f, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, null, 0f, 0f, ArcEndCap.None);
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x000066B8 File Offset: 0x000048B8
		[OvldGenCallTarget]
		private static void Ring_Internal([OvldDefault("DiscRadius")] float radius, [OvldDefault("RingThickness")] float thickness, [OvldDefault("Color")] Color colorInnerStart, [OvldDefault("Color")] Color colorOuterStart, [OvldDefault("Color")] Color colorInnerEnd, [OvldDefault("Color")] Color colorOuterEnd, [OvldDefault("RingDashStyle")] DashStyle dashStyle = null)
		{
			Draw.DiscCore(true, false, radius, thickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, dashStyle, 0f, 0f, ArcEndCap.None);
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x000066E4 File Offset: 0x000048E4
		[OvldGenCallTarget]
		private static void Pie_Internal([OvldDefault("DiscRadius")] float radius, [OvldDefault("Color")] Color colorInnerStart, [OvldDefault("Color")] Color colorOuterStart, [OvldDefault("Color")] Color colorInnerEnd, [OvldDefault("Color")] Color colorOuterEnd, float angleRadStart, float angleRadEnd)
		{
			Draw.DiscCore(false, true, radius, 0f, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, null, angleRadStart, angleRadEnd, ArcEndCap.None);
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000670C File Offset: 0x0000490C
		[OvldGenCallTarget]
		private static void Arc_Internal([OvldDefault("DiscRadius")] float radius, [OvldDefault("RingThickness")] float thickness, [OvldDefault("Color")] Color colorInnerStart, [OvldDefault("Color")] Color colorOuterStart, [OvldDefault("Color")] Color colorInnerEnd, [OvldDefault("Color")] Color colorOuterEnd, float angleRadStart, float angleRadEnd, [OvldDefault("ArcEndCap.None")] ArcEndCap endCaps, [OvldDefault("RingDashStyle")] DashStyle dashStyle = null)
		{
			Draw.DiscCore(true, true, radius, thickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, dashStyle, angleRadStart, angleRadEnd, endCaps);
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x00006730 File Offset: 0x00004930
		private static void DiscCore(bool hollow, bool sector, float radius, float thickness, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd, DashStyle dashStyle = null, float angleRadStart = 0f, float angleRadEnd = 0f, ArcEndCap arcEndCaps = ArcEndCap.None)
		{
			if (sector && Mathf.Abs(angleRadEnd - angleRadStart) < 0.0001f)
			{
				return;
			}
			using (new IMDrawer(Draw.mpbDisc, ShapesMaterialUtils.GetDiscMaterial(hollow, sector)[Draw.BlendMode], ShapesMeshUtils.QuadMesh[0], 0, false))
			{
				MetaMpb.ApplyDashSettings<MpbDisc>(Draw.mpbDisc, dashStyle, thickness);
				Draw.mpbDisc.radius.Add(radius);
				Draw.mpbDisc.radiusSpace.Add((float)Draw.DiscRadiusSpace);
				Draw.mpbDisc.alignment.Add((float)Draw.DiscGeometry);
				Draw.mpbDisc.thicknessSpace.Add((float)Draw.RingThicknessSpace);
				Draw.mpbDisc.thickness.Add(thickness);
				Draw.mpbDisc.scaleMode.Add((float)Draw.ScaleMode);
				Draw.mpbDisc.angStart.Add(angleRadStart);
				Draw.mpbDisc.angEnd.Add(angleRadEnd);
				Draw.mpbDisc.roundCaps.Add((float)arcEndCaps);
				Draw.mpbDisc.color.Add(colorInnerStart.ColorSpaceAdjusted());
				Draw.mpbDisc.colorOuterStart.Add(colorOuterStart.ColorSpaceAdjusted());
				Draw.mpbDisc.colorInnerEnd.Add(colorInnerEnd.ColorSpaceAdjusted());
				Draw.mpbDisc.colorOuterEnd.Add(colorOuterEnd.ColorSpaceAdjusted());
			}
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x000068C8 File Offset: 0x00004AC8
		[OvldGenCallTarget]
		private static void RegularPolygon_Internal([OvldDefault("RegularPolygonSideCount")] int sideCount, [OvldDefault("RegularPolygonRadius")] float radius, [OvldDefault("RegularPolygonThickness")] float thickness, [OvldDefault("Color")] Color color, bool hollow, [OvldDefault("0f")] float roundness, [OvldDefault("0f")] float angle, [OvldDefault("PolygonShapeFill")] ShapeFill fill)
		{
			using (new IMDrawer(Draw.mpbRegularPolygon, ShapesMaterialUtils.matRegularPolygon[Draw.BlendMode], ShapesMeshUtils.QuadMesh[0], 0, false))
			{
				MetaMpb.ApplyColorOrFill<MpbRegularPolygon>(Draw.mpbRegularPolygon, fill, color);
				Draw.mpbRegularPolygon.radius.Add(radius);
				Draw.mpbRegularPolygon.radiusSpace.Add((float)Draw.RegularPolygonRadiusSpace);
				Draw.mpbRegularPolygon.geometry.Add((float)Draw.RegularPolygonGeometry);
				Draw.mpbRegularPolygon.sides.Add((float)Mathf.Max(3, sideCount));
				Draw.mpbRegularPolygon.angle.Add(angle);
				Draw.mpbRegularPolygon.roundness.Add(roundness);
				Draw.mpbRegularPolygon.hollow.Add((float)hollow.AsInt());
				Draw.mpbRegularPolygon.thicknessSpace.Add((float)Draw.RegularPolygonThicknessSpace);
				Draw.mpbRegularPolygon.thickness.Add(thickness);
				Draw.mpbRegularPolygon.scaleMode.Add((float)Draw.ScaleMode);
			}
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x000069EC File Offset: 0x00004BEC
		[OvldGenCallTarget]
		private static void Rectangle_Internal([OvldDefault("BlendMode")] ShapesBlendMode blendMode, [OvldDefault("false")] bool hollow, Rect rect, [OvldDefault("Color")] Color color, [OvldDefault("RectangleThickness")] float thickness, [OvldDefault("default")] Vector4 cornerRadii, [OvldDefault("PolygonShapeFill")] ShapeFill fill)
		{
			bool flag = ShapesMath.MaxComp(cornerRadii) >= 0.0001f;
			if (rect.width < 0f)
			{
				rect.x -= (rect.width *= -1f);
			}
			if (rect.height < 0f)
			{
				rect.y -= (rect.height *= -1f);
			}
			if (hollow && thickness * 2f >= Mathf.Min(rect.width, rect.height))
			{
				hollow = false;
			}
			using (new IMDrawer(Draw.mpbRectangle, ShapesMaterialUtils.GetRectMaterial(hollow, flag)[blendMode], ShapesMeshUtils.QuadMesh[0], 0, false))
			{
				MetaMpb.ApplyColorOrFill<MpbRectangle>(Draw.mpbRectangle, fill, color);
				Draw.mpbRectangle.rect.Add(rect.ToVector4());
				Draw.mpbRectangle.cornerRadii.Add(cornerRadii);
				Draw.mpbRectangle.thickness.Add(thickness);
				Draw.mpbRectangle.thicknessSpace.Add((float)Draw.RegularPolygonThicknessSpace);
				Draw.mpbRectangle.scaleMode.Add((float)Draw.ScaleMode);
			}
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x00006B44 File Offset: 0x00004D44
		[OvldGenCallTarget]
		private static void Triangle_Internal(Vector3 a, Vector3 b, Vector3 c, bool hollow, [OvldDefault("TriangleThickness")] float thickness, [OvldDefault("0f")] float roundness, [OvldDefault("Color")] Color colorA, [OvldDefault("Color")] Color colorB, [OvldDefault("Color")] Color colorC)
		{
			using (new IMDrawer(Draw.mpbTriangle, ShapesMaterialUtils.matTriangle[Draw.BlendMode], ShapesMeshUtils.TriangleMesh[0], 0, false))
			{
				Draw.mpbTriangle.a.Add(a);
				Draw.mpbTriangle.b.Add(b);
				Draw.mpbTriangle.c.Add(c);
				Draw.mpbTriangle.color.Add(colorA.ColorSpaceAdjusted());
				Draw.mpbTriangle.colorB.Add(colorB.ColorSpaceAdjusted());
				Draw.mpbTriangle.colorC.Add(colorC.ColorSpaceAdjusted());
				Draw.mpbTriangle.roundness.Add(roundness);
				Draw.mpbTriangle.hollow.Add((float)hollow.AsInt());
				Draw.mpbTriangle.thicknessSpace.Add((float)Draw.RegularPolygonThicknessSpace);
				Draw.mpbTriangle.thickness.Add(thickness);
				Draw.mpbTriangle.scaleMode.Add((float)Draw.ScaleMode);
			}
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x00006C88 File Offset: 0x00004E88
		[OvldGenCallTarget]
		private static void Quad_Internal(Vector3 a, Vector3 b, Vector3 c, [OvldDefault("a + ( c - b )")] Vector3 d, [OvldDefault("Color")] Color colorA, [OvldDefault("Color")] Color colorB, [OvldDefault("Color")] Color colorC, [OvldDefault("Color")] Color colorD)
		{
			using (new IMDrawer(Draw.mpbQuad, ShapesMaterialUtils.matQuad[Draw.BlendMode], ShapesMeshUtils.QuadMesh[0], 0, false))
			{
				Draw.mpbQuad.a.Add(a);
				Draw.mpbQuad.b.Add(b);
				Draw.mpbQuad.c.Add(c);
				Draw.mpbQuad.d.Add(d);
				Draw.mpbQuad.color.Add(colorA.ColorSpaceAdjusted());
				Draw.mpbQuad.colorB.Add(colorB.ColorSpaceAdjusted());
				Draw.mpbQuad.colorC.Add(colorC.ColorSpaceAdjusted());
				Draw.mpbQuad.colorD.Add(colorD.ColorSpaceAdjusted());
			}
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x00006D9C File Offset: 0x00004F9C
		[OvldGenCallTarget]
		private static void Sphere_Internal([OvldDefault("SphereRadius")] float radius, [OvldDefault("Color")] Color color)
		{
			using (new IMDrawer(Draw.metaMpbSphere, ShapesMaterialUtils.matSphere[Draw.BlendMode], ShapesMeshUtils.SphereMesh[(int)Draw.DetailLevel], 0, false))
			{
				Draw.metaMpbSphere.color.Add(color.ColorSpaceAdjusted());
				Draw.metaMpbSphere.radius.Add(radius);
				Draw.metaMpbSphere.radiusSpace.Add((float)Draw.SphereRadiusSpace);
			}
		}

		// Token: 0x060001FA RID: 506 RVA: 0x00006E30 File Offset: 0x00005030
		[OvldGenCallTarget]
		private static void Cone_Internal(float radius, float length, [OvldDefault("true")] bool fillCap, [OvldDefault("Color")] Color color)
		{
			Mesh mesh = (fillCap ? ShapesMeshUtils.ConeMesh[(int)Draw.DetailLevel] : ShapesMeshUtils.ConeMeshUncapped[(int)Draw.DetailLevel]);
			using (new IMDrawer(Draw.mpbCone, ShapesMaterialUtils.matCone[Draw.BlendMode], mesh, 0, false))
			{
				Draw.mpbCone.color.Add(color.ColorSpaceAdjusted());
				Draw.mpbCone.radius.Add(radius);
				Draw.mpbCone.length.Add(length);
				Draw.mpbCone.sizeSpace.Add((float)Draw.ConeSizeSpace);
			}
		}

		// Token: 0x060001FB RID: 507 RVA: 0x00006EE8 File Offset: 0x000050E8
		[OvldGenCallTarget]
		private static void Cuboid_Internal(Vector3 size, [OvldDefault("Color")] Color color)
		{
			using (new IMDrawer(Draw.mpbCuboid, ShapesMaterialUtils.matCuboid[Draw.BlendMode], ShapesMeshUtils.CuboidMesh[0], 0, false))
			{
				Draw.mpbCuboid.color.Add(color.ColorSpaceAdjusted());
				Draw.mpbCuboid.size.Add(size);
				Draw.mpbCuboid.sizeSpace.Add((float)Draw.CuboidSizeSpace);
			}
		}

		// Token: 0x060001FC RID: 508 RVA: 0x00006F80 File Offset: 0x00005180
		[OvldGenCallTarget]
		private static void Torus_Internal(float radius, float thickness, [OvldDefault("Color")] Color color)
		{
			if (thickness < 0.0001f)
			{
				return;
			}
			if (radius < 1E-05f)
			{
				ThicknessSpace sphereRadiusSpace = Draw.SphereRadiusSpace;
				Draw.SphereRadiusSpace = Draw.TorusThicknessSpace;
				Draw.Sphere(thickness / 2f, color);
				Draw.SphereRadiusSpace = sphereRadiusSpace;
				return;
			}
			using (new IMDrawer(Draw.mpbTorus, ShapesMaterialUtils.matTorus[Draw.BlendMode], ShapesMeshUtils.TorusMesh[(int)Draw.DetailLevel], 0, false))
			{
				Draw.mpbTorus.color.Add(color.ColorSpaceAdjusted());
				Draw.mpbTorus.radius.Add(radius);
				Draw.mpbTorus.thickness.Add(thickness);
				Draw.mpbTorus.spaceRadius.Add((float)Draw.TorusRadiusSpace);
				Draw.mpbTorus.spaceThickness.Add((float)Draw.TorusThicknessSpace);
				Draw.mpbTorus.scaleMode.Add((float)Draw.ScaleMode);
			}
		}

		// Token: 0x060001FD RID: 509 RVA: 0x00007084 File Offset: 0x00005284
		[OvldGenCallTarget]
		private static void Text_Internal(string content, [OvldDefault("Font")] TMP_FontAsset font, [OvldDefault("FontSize")] float fontSize, [OvldDefault("TextAlign")] TextAlign align, [OvldDefault("Color")] Color color)
		{
			TextMeshPro tmp = ShapesTextDrawer.Instance.tmp;
			tmp.font = font;
			tmp.color = color;
			tmp.fontSize = fontSize;
			tmp.text = content;
			tmp.alignment = align.GetTMPAlignment();
			tmp.rectTransform.pivot = align.GetPivot();
			tmp.transform.position = Draw.Matrix.GetColumn(3);
			tmp.rectTransform.rotation = Draw.Matrix.rotation;
			tmp.ForceMeshUpdate(false, false);
			using (new IMDrawer(Draw.mpbText, font.material, tmp.mesh, 0, true))
			{
			}
		}

		// Token: 0x060001FE RID: 510 RVA: 0x00007150 File Offset: 0x00005350
		public static void Line(Vector3 start, Vector3 end)
		{
			Draw.Line_Internal(Draw.LineEndCaps, Draw.LineThicknessSpace, start, end, Draw.Color, Draw.Color, Draw.LineThickness, null);
		}

		// Token: 0x060001FF RID: 511 RVA: 0x00007173 File Offset: 0x00005373
		public static void Line(Vector3 start, Vector3 end, Color color)
		{
			Draw.Line_Internal(Draw.LineEndCaps, Draw.LineThicknessSpace, start, end, color, color, Draw.LineThickness, null);
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0000718E File Offset: 0x0000538E
		public static void Line(Vector3 start, Vector3 end, Color colorStart, Color colorEnd)
		{
			Draw.Line_Internal(Draw.LineEndCaps, Draw.LineThicknessSpace, start, end, colorStart, colorEnd, Draw.LineThickness, null);
		}

		// Token: 0x06000201 RID: 513 RVA: 0x000071A9 File Offset: 0x000053A9
		public static void Line(Vector3 start, Vector3 end, float thickness)
		{
			Draw.Line_Internal(Draw.LineEndCaps, Draw.LineThicknessSpace, start, end, Draw.Color, Draw.Color, thickness, null);
		}

		// Token: 0x06000202 RID: 514 RVA: 0x000071C8 File Offset: 0x000053C8
		public static void Line(Vector3 start, Vector3 end, float thickness, Color color)
		{
			Draw.Line_Internal(Draw.LineEndCaps, Draw.LineThicknessSpace, start, end, color, color, thickness, null);
		}

		// Token: 0x06000203 RID: 515 RVA: 0x000071DF File Offset: 0x000053DF
		public static void Line(Vector3 start, Vector3 end, float thickness, Color colorStart, Color colorEnd)
		{
			Draw.Line_Internal(Draw.LineEndCaps, Draw.LineThicknessSpace, start, end, colorStart, colorEnd, thickness, null);
		}

		// Token: 0x06000204 RID: 516 RVA: 0x000071F7 File Offset: 0x000053F7
		public static void Line(Vector3 start, Vector3 end, LineEndCap endCaps)
		{
			Draw.Line_Internal(endCaps, Draw.LineThicknessSpace, start, end, Draw.Color, Draw.Color, Draw.LineThickness, null);
		}

		// Token: 0x06000205 RID: 517 RVA: 0x00007216 File Offset: 0x00005416
		public static void Line(Vector3 start, Vector3 end, LineEndCap endCaps, Color color)
		{
			Draw.Line_Internal(endCaps, Draw.LineThicknessSpace, start, end, color, color, Draw.LineThickness, null);
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0000722D File Offset: 0x0000542D
		public static void Line(Vector3 start, Vector3 end, LineEndCap endCaps, Color colorStart, Color colorEnd)
		{
			Draw.Line_Internal(endCaps, Draw.LineThicknessSpace, start, end, colorStart, colorEnd, Draw.LineThickness, null);
		}

		// Token: 0x06000207 RID: 519 RVA: 0x00007245 File Offset: 0x00005445
		public static void Line(Vector3 start, Vector3 end, float thickness, LineEndCap endCaps)
		{
			Draw.Line_Internal(endCaps, Draw.LineThicknessSpace, start, end, Draw.Color, Draw.Color, thickness, null);
		}

		// Token: 0x06000208 RID: 520 RVA: 0x00007260 File Offset: 0x00005460
		public static void Line(Vector3 start, Vector3 end, float thickness, LineEndCap endCaps, Color color)
		{
			Draw.Line_Internal(endCaps, Draw.LineThicknessSpace, start, end, color, color, thickness, null);
		}

		// Token: 0x06000209 RID: 521 RVA: 0x00007275 File Offset: 0x00005475
		public static void Line(Vector3 start, Vector3 end, float thickness, LineEndCap endCaps, Color colorStart, Color colorEnd)
		{
			Draw.Line_Internal(endCaps, Draw.LineThicknessSpace, start, end, colorStart, colorEnd, thickness, null);
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000728A File Offset: 0x0000548A
		public static void LineDashed(Vector3 start, Vector3 end)
		{
			Draw.Line_Internal(Draw.LineEndCaps, Draw.LineThicknessSpace, start, end, Draw.Color, Draw.Color, Draw.LineThickness, Draw.LineDashStyle);
		}

		// Token: 0x0600020B RID: 523 RVA: 0x000072B1 File Offset: 0x000054B1
		public static void LineDashed(Vector3 start, Vector3 end, Color color)
		{
			Draw.Line_Internal(Draw.LineEndCaps, Draw.LineThicknessSpace, start, end, color, color, Draw.LineThickness, Draw.LineDashStyle);
		}

		// Token: 0x0600020C RID: 524 RVA: 0x000072D0 File Offset: 0x000054D0
		public static void LineDashed(Vector3 start, Vector3 end, Color colorStart, Color colorEnd)
		{
			Draw.Line_Internal(Draw.LineEndCaps, Draw.LineThicknessSpace, start, end, colorStart, colorEnd, Draw.LineThickness, Draw.LineDashStyle);
		}

		// Token: 0x0600020D RID: 525 RVA: 0x000072EF File Offset: 0x000054EF
		public static void LineDashed(Vector3 start, Vector3 end, float thickness)
		{
			Draw.Line_Internal(Draw.LineEndCaps, Draw.LineThicknessSpace, start, end, Draw.Color, Draw.Color, thickness, Draw.LineDashStyle);
		}

		// Token: 0x0600020E RID: 526 RVA: 0x00007312 File Offset: 0x00005512
		public static void LineDashed(Vector3 start, Vector3 end, float thickness, Color color)
		{
			Draw.Line_Internal(Draw.LineEndCaps, Draw.LineThicknessSpace, start, end, color, color, thickness, Draw.LineDashStyle);
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000732D File Offset: 0x0000552D
		public static void LineDashed(Vector3 start, Vector3 end, float thickness, Color colorStart, Color colorEnd)
		{
			Draw.Line_Internal(Draw.LineEndCaps, Draw.LineThicknessSpace, start, end, colorStart, colorEnd, thickness, Draw.LineDashStyle);
		}

		// Token: 0x06000210 RID: 528 RVA: 0x00007349 File Offset: 0x00005549
		public static void LineDashed(Vector3 start, Vector3 end, LineEndCap endCaps)
		{
			Draw.Line_Internal(endCaps, Draw.LineThicknessSpace, start, end, Draw.Color, Draw.Color, Draw.LineThickness, Draw.LineDashStyle);
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000736C File Offset: 0x0000556C
		public static void LineDashed(Vector3 start, Vector3 end, LineEndCap endCaps, Color color)
		{
			Draw.Line_Internal(endCaps, Draw.LineThicknessSpace, start, end, color, color, Draw.LineThickness, Draw.LineDashStyle);
		}

		// Token: 0x06000212 RID: 530 RVA: 0x00007387 File Offset: 0x00005587
		public static void LineDashed(Vector3 start, Vector3 end, LineEndCap endCaps, Color colorStart, Color colorEnd)
		{
			Draw.Line_Internal(endCaps, Draw.LineThicknessSpace, start, end, colorStart, colorEnd, Draw.LineThickness, Draw.LineDashStyle);
		}

		// Token: 0x06000213 RID: 531 RVA: 0x000073A3 File Offset: 0x000055A3
		public static void LineDashed(Vector3 start, Vector3 end, float thickness, LineEndCap endCaps)
		{
			Draw.Line_Internal(endCaps, Draw.LineThicknessSpace, start, end, Draw.Color, Draw.Color, thickness, Draw.LineDashStyle);
		}

		// Token: 0x06000214 RID: 532 RVA: 0x000073C2 File Offset: 0x000055C2
		public static void LineDashed(Vector3 start, Vector3 end, float thickness, LineEndCap endCaps, Color color)
		{
			Draw.Line_Internal(endCaps, Draw.LineThicknessSpace, start, end, color, color, thickness, Draw.LineDashStyle);
		}

		// Token: 0x06000215 RID: 533 RVA: 0x000073DB File Offset: 0x000055DB
		public static void LineDashed(Vector3 start, Vector3 end, float thickness, LineEndCap endCaps, Color colorStart, Color colorEnd)
		{
			Draw.Line_Internal(endCaps, Draw.LineThicknessSpace, start, end, colorStart, colorEnd, thickness, Draw.LineDashStyle);
		}

		// Token: 0x06000216 RID: 534 RVA: 0x000073F4 File Offset: 0x000055F4
		public static void LineDashed(Vector3 start, Vector3 end, DashStyle dashStyle)
		{
			Draw.Line_Internal(Draw.LineEndCaps, Draw.LineThicknessSpace, start, end, Draw.Color, Draw.Color, Draw.LineThickness, dashStyle);
		}

		// Token: 0x06000217 RID: 535 RVA: 0x00007417 File Offset: 0x00005617
		public static void LineDashed(Vector3 start, Vector3 end, DashStyle dashStyle, Color color)
		{
			Draw.Line_Internal(Draw.LineEndCaps, Draw.LineThicknessSpace, start, end, color, color, Draw.LineThickness, dashStyle);
		}

		// Token: 0x06000218 RID: 536 RVA: 0x00007432 File Offset: 0x00005632
		public static void LineDashed(Vector3 start, Vector3 end, DashStyle dashStyle, Color colorStart, Color colorEnd)
		{
			Draw.Line_Internal(Draw.LineEndCaps, Draw.LineThicknessSpace, start, end, colorStart, colorEnd, Draw.LineThickness, dashStyle);
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000744E File Offset: 0x0000564E
		public static void LineDashed(Vector3 start, Vector3 end, DashStyle dashStyle, float thickness)
		{
			Draw.Line_Internal(Draw.LineEndCaps, Draw.LineThicknessSpace, start, end, Draw.Color, Draw.Color, thickness, dashStyle);
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000746D File Offset: 0x0000566D
		public static void LineDashed(Vector3 start, Vector3 end, DashStyle dashStyle, float thickness, Color color)
		{
			Draw.Line_Internal(Draw.LineEndCaps, Draw.LineThicknessSpace, start, end, color, color, thickness, dashStyle);
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00007486 File Offset: 0x00005686
		public static void LineDashed(Vector3 start, Vector3 end, DashStyle dashStyle, float thickness, Color colorStart, Color colorEnd)
		{
			Draw.Line_Internal(Draw.LineEndCaps, Draw.LineThicknessSpace, start, end, colorStart, colorEnd, thickness, dashStyle);
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000749F File Offset: 0x0000569F
		public static void LineDashed(Vector3 start, Vector3 end, DashStyle dashStyle, LineEndCap endCaps)
		{
			Draw.Line_Internal(endCaps, Draw.LineThicknessSpace, start, end, Draw.Color, Draw.Color, Draw.LineThickness, dashStyle);
		}

		// Token: 0x0600021D RID: 541 RVA: 0x000074BE File Offset: 0x000056BE
		public static void LineDashed(Vector3 start, Vector3 end, DashStyle dashStyle, LineEndCap endCaps, Color color)
		{
			Draw.Line_Internal(endCaps, Draw.LineThicknessSpace, start, end, color, color, Draw.LineThickness, dashStyle);
		}

		// Token: 0x0600021E RID: 542 RVA: 0x000074D7 File Offset: 0x000056D7
		public static void LineDashed(Vector3 start, Vector3 end, DashStyle dashStyle, LineEndCap endCaps, Color colorStart, Color colorEnd)
		{
			Draw.Line_Internal(endCaps, Draw.LineThicknessSpace, start, end, colorStart, colorEnd, Draw.LineThickness, dashStyle);
		}

		// Token: 0x0600021F RID: 543 RVA: 0x000074F0 File Offset: 0x000056F0
		public static void LineDashed(Vector3 start, Vector3 end, DashStyle dashStyle, float thickness, LineEndCap endCaps)
		{
			Draw.Line_Internal(endCaps, Draw.LineThicknessSpace, start, end, Draw.Color, Draw.Color, thickness, dashStyle);
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000750C File Offset: 0x0000570C
		public static void LineDashed(Vector3 start, Vector3 end, DashStyle dashStyle, float thickness, LineEndCap endCaps, Color color)
		{
			Draw.Line_Internal(endCaps, Draw.LineThicknessSpace, start, end, color, color, thickness, dashStyle);
		}

		// Token: 0x06000221 RID: 545 RVA: 0x00007522 File Offset: 0x00005722
		public static void LineDashed(Vector3 start, Vector3 end, DashStyle dashStyle, float thickness, LineEndCap endCaps, Color colorStart, Color colorEnd)
		{
			Draw.Line_Internal(endCaps, Draw.LineThicknessSpace, start, end, colorStart, colorEnd, thickness, dashStyle);
		}

		// Token: 0x06000222 RID: 546 RVA: 0x00007538 File Offset: 0x00005738
		public static void Polyline(PolylinePath path)
		{
			Draw.Polyline_Internal(path, false, Draw.PolylineGeometry, Draw.PolylineJoins, Draw.LineThickness, Draw.LineThicknessSpace, Draw.Color);
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000755A File Offset: 0x0000575A
		public static void Polyline(PolylinePath path, bool closed)
		{
			Draw.Polyline_Internal(path, closed, Draw.PolylineGeometry, Draw.PolylineJoins, Draw.LineThickness, Draw.LineThicknessSpace, Draw.Color);
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000757C File Offset: 0x0000577C
		public static void Polyline(PolylinePath path, float thickness)
		{
			Draw.Polyline_Internal(path, false, Draw.PolylineGeometry, Draw.PolylineJoins, thickness, Draw.LineThicknessSpace, Draw.Color);
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000759A File Offset: 0x0000579A
		public static void Polyline(PolylinePath path, bool closed, float thickness)
		{
			Draw.Polyline_Internal(path, closed, Draw.PolylineGeometry, Draw.PolylineJoins, thickness, Draw.LineThicknessSpace, Draw.Color);
		}

		// Token: 0x06000226 RID: 550 RVA: 0x000075B8 File Offset: 0x000057B8
		public static void Polyline(PolylinePath path, PolylineJoins joins)
		{
			Draw.Polyline_Internal(path, false, Draw.PolylineGeometry, joins, Draw.LineThickness, Draw.LineThicknessSpace, Draw.Color);
		}

		// Token: 0x06000227 RID: 551 RVA: 0x000075D6 File Offset: 0x000057D6
		public static void Polyline(PolylinePath path, bool closed, PolylineJoins joins)
		{
			Draw.Polyline_Internal(path, closed, Draw.PolylineGeometry, joins, Draw.LineThickness, Draw.LineThicknessSpace, Draw.Color);
		}

		// Token: 0x06000228 RID: 552 RVA: 0x000075F4 File Offset: 0x000057F4
		public static void Polyline(PolylinePath path, float thickness, PolylineJoins joins)
		{
			Draw.Polyline_Internal(path, false, Draw.PolylineGeometry, joins, thickness, Draw.LineThicknessSpace, Draw.Color);
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000760E File Offset: 0x0000580E
		public static void Polyline(PolylinePath path, bool closed, float thickness, PolylineJoins joins)
		{
			Draw.Polyline_Internal(path, closed, Draw.PolylineGeometry, joins, thickness, Draw.LineThicknessSpace, Draw.Color);
		}

		// Token: 0x0600022A RID: 554 RVA: 0x00007628 File Offset: 0x00005828
		public static void Polyline(PolylinePath path, Color color)
		{
			Draw.Polyline_Internal(path, false, Draw.PolylineGeometry, Draw.PolylineJoins, Draw.LineThickness, Draw.LineThicknessSpace, color);
		}

		// Token: 0x0600022B RID: 555 RVA: 0x00007646 File Offset: 0x00005846
		public static void Polyline(PolylinePath path, bool closed, Color color)
		{
			Draw.Polyline_Internal(path, closed, Draw.PolylineGeometry, Draw.PolylineJoins, Draw.LineThickness, Draw.LineThicknessSpace, color);
		}

		// Token: 0x0600022C RID: 556 RVA: 0x00007664 File Offset: 0x00005864
		public static void Polyline(PolylinePath path, float thickness, Color color)
		{
			Draw.Polyline_Internal(path, false, Draw.PolylineGeometry, Draw.PolylineJoins, thickness, Draw.LineThicknessSpace, color);
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000767E File Offset: 0x0000587E
		public static void Polyline(PolylinePath path, bool closed, float thickness, Color color)
		{
			Draw.Polyline_Internal(path, closed, Draw.PolylineGeometry, Draw.PolylineJoins, thickness, Draw.LineThicknessSpace, color);
		}

		// Token: 0x0600022E RID: 558 RVA: 0x00007698 File Offset: 0x00005898
		public static void Polyline(PolylinePath path, PolylineJoins joins, Color color)
		{
			Draw.Polyline_Internal(path, false, Draw.PolylineGeometry, joins, Draw.LineThickness, Draw.LineThicknessSpace, color);
		}

		// Token: 0x0600022F RID: 559 RVA: 0x000076B2 File Offset: 0x000058B2
		public static void Polyline(PolylinePath path, bool closed, PolylineJoins joins, Color color)
		{
			Draw.Polyline_Internal(path, closed, Draw.PolylineGeometry, joins, Draw.LineThickness, Draw.LineThicknessSpace, color);
		}

		// Token: 0x06000230 RID: 560 RVA: 0x000076CC File Offset: 0x000058CC
		public static void Polyline(PolylinePath path, float thickness, PolylineJoins joins, Color color)
		{
			Draw.Polyline_Internal(path, false, Draw.PolylineGeometry, joins, thickness, Draw.LineThicknessSpace, color);
		}

		// Token: 0x06000231 RID: 561 RVA: 0x000076E2 File Offset: 0x000058E2
		public static void Polyline(PolylinePath path, bool closed, float thickness, PolylineJoins joins, Color color)
		{
			Draw.Polyline_Internal(path, closed, Draw.PolylineGeometry, joins, thickness, Draw.LineThicknessSpace, color);
		}

		// Token: 0x06000232 RID: 562 RVA: 0x000076F9 File Offset: 0x000058F9
		public static void Polygon(PolygonPath path)
		{
			Draw.Polygon_Internal(path, Draw.PolygonTriangulation, Draw.Color, null);
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000770C File Offset: 0x0000590C
		public static void Polygon(PolygonPath path, Color color)
		{
			Draw.Polygon_Internal(path, Draw.PolygonTriangulation, color, null);
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000771B File Offset: 0x0000591B
		public static void Polygon(PolygonPath path, PolygonTriangulation triangulation)
		{
			Draw.Polygon_Internal(path, triangulation, Draw.Color, null);
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000772A File Offset: 0x0000592A
		public static void Polygon(PolygonPath path, PolygonTriangulation triangulation, Color color)
		{
			Draw.Polygon_Internal(path, triangulation, color, null);
		}

		// Token: 0x06000236 RID: 566 RVA: 0x00007735 File Offset: 0x00005935
		public static void PolygonFill(PolygonPath path)
		{
			Draw.Polygon_Internal(path, Draw.PolygonTriangulation, Draw.Color, Draw.PolygonShapeFill);
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000774C File Offset: 0x0000594C
		public static void PolygonFill(PolygonPath path, ShapeFill fill)
		{
			Draw.Polygon_Internal(path, Draw.PolygonTriangulation, Draw.Color, fill);
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000775F File Offset: 0x0000595F
		public static void PolygonFill(PolygonPath path, PolygonTriangulation triangulation)
		{
			Draw.Polygon_Internal(path, triangulation, Draw.Color, Draw.PolygonShapeFill);
		}

		// Token: 0x06000239 RID: 569 RVA: 0x00007772 File Offset: 0x00005972
		public static void PolygonFill(PolygonPath path, PolygonTriangulation triangulation, ShapeFill fill)
		{
			Draw.Polygon_Internal(path, triangulation, Draw.Color, fill);
		}

		// Token: 0x0600023A RID: 570 RVA: 0x00007781 File Offset: 0x00005981
		public static void PolygonFillLinear(PolygonPath path, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.Polygon_Internal(path, Draw.PolygonTriangulation, Draw.Color, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000779F File Offset: 0x0000599F
		public static void PolygonFillLinear(PolygonPath path, PolygonTriangulation triangulation, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.Polygon_Internal(path, triangulation, Draw.Color, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
		}

		// Token: 0x0600023C RID: 572 RVA: 0x000077BA File Offset: 0x000059BA
		public static void PolygonFillRadial(PolygonPath path, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.Polygon_Internal(path, Draw.PolygonTriangulation, Draw.Color, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
		}

		// Token: 0x0600023D RID: 573 RVA: 0x000077D8 File Offset: 0x000059D8
		public static void PolygonFillRadial(PolygonPath path, PolygonTriangulation triangulation, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.Polygon_Internal(path, triangulation, Draw.Color, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
		}

		// Token: 0x0600023E RID: 574 RVA: 0x000077F3 File Offset: 0x000059F3
		public static void RegularPolygon(Vector3 pos)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000782A File Offset: 0x00005A2A
		public static void RegularPolygon(Vector3 pos, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, color, false, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000785D File Offset: 0x00005A5D
		public static void RegularPolygon(Vector3 pos, float radius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000241 RID: 577 RVA: 0x00007890 File Offset: 0x00005A90
		public static void RegularPolygon(Vector3 pos, float radius, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, color, false, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000242 RID: 578 RVA: 0x000078BF File Offset: 0x00005ABF
		public static void RegularPolygon(Vector3 pos, float radius, float angle)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, angle, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000243 RID: 579 RVA: 0x000078EE File Offset: 0x00005AEE
		public static void RegularPolygon(Vector3 pos, float radius, float angle, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, color, false, 0f, angle, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000244 RID: 580 RVA: 0x00007919 File Offset: 0x00005B19
		public static void RegularPolygon(Vector3 pos, float radius, float angle, float roundness)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, roundness, angle, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000245 RID: 581 RVA: 0x00007944 File Offset: 0x00005B44
		public static void RegularPolygon(Vector3 pos, float radius, float angle, float roundness, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, color, false, roundness, angle, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000796C File Offset: 0x00005B6C
		public static void RegularPolygon(Vector3 pos, int sideCount)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0000799F File Offset: 0x00005B9F
		public static void RegularPolygon(Vector3 pos, int sideCount, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, color, false, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000248 RID: 584 RVA: 0x000079CE File Offset: 0x00005BCE
		public static void RegularPolygon(Vector3 pos, int sideCount, float radius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000249 RID: 585 RVA: 0x000079FD File Offset: 0x00005BFD
		public static void RegularPolygon(Vector3 pos, int sideCount, float radius, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, color, false, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600024A RID: 586 RVA: 0x00007A28 File Offset: 0x00005C28
		public static void RegularPolygon(Vector3 pos, int sideCount, float radius, float angle)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, angle, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600024B RID: 587 RVA: 0x00007A53 File Offset: 0x00005C53
		public static void RegularPolygon(Vector3 pos, int sideCount, float radius, float angle, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, color, false, 0f, angle, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600024C RID: 588 RVA: 0x00007A7B File Offset: 0x00005C7B
		public static void RegularPolygon(Vector3 pos, int sideCount, float radius, float angle, float roundness)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, roundness, angle, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600024D RID: 589 RVA: 0x00007AA3 File Offset: 0x00005CA3
		public static void RegularPolygon(Vector3 pos, int sideCount, float radius, float angle, float roundness, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, color, false, roundness, angle, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600024E RID: 590 RVA: 0x00007AC8 File Offset: 0x00005CC8
		public static void RegularPolygon(Vector3 pos, Vector3 normal)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600024F RID: 591 RVA: 0x00007B24 File Offset: 0x00005D24
		public static void RegularPolygon(Vector3 pos, Vector3 normal, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, color, false, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000250 RID: 592 RVA: 0x00007B7C File Offset: 0x00005D7C
		public static void RegularPolygon(Vector3 pos, Vector3 normal, float radius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000251 RID: 593 RVA: 0x00007BD4 File Offset: 0x00005DD4
		public static void RegularPolygon(Vector3 pos, Vector3 normal, float radius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, color, false, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000252 RID: 594 RVA: 0x00007C28 File Offset: 0x00005E28
		public static void RegularPolygon(Vector3 pos, Vector3 normal, float radius, float angle)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, angle, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000253 RID: 595 RVA: 0x00007C7C File Offset: 0x00005E7C
		public static void RegularPolygon(Vector3 pos, Vector3 normal, float radius, float angle, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, color, false, 0f, angle, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000254 RID: 596 RVA: 0x00007CD0 File Offset: 0x00005ED0
		public static void RegularPolygon(Vector3 pos, Vector3 normal, float radius, float angle, float roundness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, roundness, angle, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000255 RID: 597 RVA: 0x00007D24 File Offset: 0x00005F24
		public static void RegularPolygon(Vector3 pos, Vector3 normal, float radius, float angle, float roundness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, color, false, roundness, angle, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000256 RID: 598 RVA: 0x00007D74 File Offset: 0x00005F74
		public static void RegularPolygon(Vector3 pos, Vector3 normal, int sideCount)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000257 RID: 599 RVA: 0x00007DCC File Offset: 0x00005FCC
		public static void RegularPolygon(Vector3 pos, Vector3 normal, int sideCount, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, color, false, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000258 RID: 600 RVA: 0x00007E20 File Offset: 0x00006020
		public static void RegularPolygon(Vector3 pos, Vector3 normal, int sideCount, float radius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000259 RID: 601 RVA: 0x00007E74 File Offset: 0x00006074
		public static void RegularPolygon(Vector3 pos, Vector3 normal, int sideCount, float radius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, color, false, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600025A RID: 602 RVA: 0x00007EC8 File Offset: 0x000060C8
		public static void RegularPolygon(Vector3 pos, Vector3 normal, int sideCount, float radius, float angle)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, angle, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600025B RID: 603 RVA: 0x00007F1C File Offset: 0x0000611C
		public static void RegularPolygon(Vector3 pos, Vector3 normal, int sideCount, float radius, float angle, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, color, false, 0f, angle, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600025C RID: 604 RVA: 0x00007F6C File Offset: 0x0000616C
		public static void RegularPolygon(Vector3 pos, Vector3 normal, int sideCount, float radius, float angle, float roundness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, roundness, angle, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600025D RID: 605 RVA: 0x00007FBA File Offset: 0x000061BA
		public static void RegularPolygon(Vector3 pos, Vector3 normal, int sideCount, float radius, float angle, float roundness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, color, false, roundness, angle, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600025E RID: 606 RVA: 0x00007FFC File Offset: 0x000061FC
		public static void RegularPolygon(Vector3 pos, Quaternion rot)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600025F RID: 607 RVA: 0x00008054 File Offset: 0x00006254
		public static void RegularPolygon(Vector3 pos, Quaternion rot, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, color, false, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000260 RID: 608 RVA: 0x000080A8 File Offset: 0x000062A8
		public static void RegularPolygon(Vector3 pos, Quaternion rot, float radius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000261 RID: 609 RVA: 0x000080FC File Offset: 0x000062FC
		public static void RegularPolygon(Vector3 pos, Quaternion rot, float radius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, color, false, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000814C File Offset: 0x0000634C
		public static void RegularPolygon(Vector3 pos, Quaternion rot, float radius, float angle)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, angle, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000819C File Offset: 0x0000639C
		public static void RegularPolygon(Vector3 pos, Quaternion rot, float radius, float angle, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, color, false, 0f, angle, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000264 RID: 612 RVA: 0x000081E8 File Offset: 0x000063E8
		public static void RegularPolygon(Vector3 pos, Quaternion rot, float radius, float angle, float roundness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, roundness, angle, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000265 RID: 613 RVA: 0x00008234 File Offset: 0x00006434
		public static void RegularPolygon(Vector3 pos, Quaternion rot, float radius, float angle, float roundness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, color, false, roundness, angle, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000266 RID: 614 RVA: 0x00008274 File Offset: 0x00006474
		public static void RegularPolygon(Vector3 pos, Quaternion rot, int sideCount)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000267 RID: 615 RVA: 0x000082C8 File Offset: 0x000064C8
		public static void RegularPolygon(Vector3 pos, Quaternion rot, int sideCount, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, color, false, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000268 RID: 616 RVA: 0x00008318 File Offset: 0x00006518
		public static void RegularPolygon(Vector3 pos, Quaternion rot, int sideCount, float radius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000269 RID: 617 RVA: 0x00008368 File Offset: 0x00006568
		public static void RegularPolygon(Vector3 pos, Quaternion rot, int sideCount, float radius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, color, false, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600026A RID: 618 RVA: 0x000083B4 File Offset: 0x000065B4
		public static void RegularPolygon(Vector3 pos, Quaternion rot, int sideCount, float radius, float angle)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, angle, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600026B RID: 619 RVA: 0x00008400 File Offset: 0x00006600
		public static void RegularPolygon(Vector3 pos, Quaternion rot, int sideCount, float radius, float angle, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, color, false, 0f, angle, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000843E File Offset: 0x0000663E
		public static void RegularPolygon(Vector3 pos, Quaternion rot, int sideCount, float radius, float angle, float roundness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, roundness, angle, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000847C File Offset: 0x0000667C
		public static void RegularPolygon(Vector3 pos, Quaternion rot, int sideCount, float radius, float angle, float roundness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, color, false, roundness, angle, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600026E RID: 622 RVA: 0x000084B7 File Offset: 0x000066B7
		public static void RegularPolygon()
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, null);
		}

		// Token: 0x0600026F RID: 623 RVA: 0x000084DE File Offset: 0x000066DE
		public static void RegularPolygon(Color color)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, color, false, 0f, 0f, null);
		}

		// Token: 0x06000270 RID: 624 RVA: 0x00008501 File Offset: 0x00006701
		public static void RegularPolygon(float radius)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, null);
		}

		// Token: 0x06000271 RID: 625 RVA: 0x00008524 File Offset: 0x00006724
		public static void RegularPolygon(float radius, Color color)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, color, false, 0f, 0f, null);
		}

		// Token: 0x06000272 RID: 626 RVA: 0x00008543 File Offset: 0x00006743
		public static void RegularPolygon(float radius, float angle)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, angle, null);
		}

		// Token: 0x06000273 RID: 627 RVA: 0x00008562 File Offset: 0x00006762
		public static void RegularPolygon(float radius, float angle, Color color)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, color, false, 0f, angle, null);
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000857D File Offset: 0x0000677D
		public static void RegularPolygon(float radius, float angle, float roundness)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, roundness, angle, null);
		}

		// Token: 0x06000275 RID: 629 RVA: 0x00008598 File Offset: 0x00006798
		public static void RegularPolygon(float radius, float angle, float roundness, Color color)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, color, false, roundness, angle, null);
		}

		// Token: 0x06000276 RID: 630 RVA: 0x000085AF File Offset: 0x000067AF
		public static void RegularPolygon(int sideCount)
		{
			Draw.RegularPolygon_Internal(sideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, null);
		}

		// Token: 0x06000277 RID: 631 RVA: 0x000085D2 File Offset: 0x000067D2
		public static void RegularPolygon(int sideCount, Color color)
		{
			Draw.RegularPolygon_Internal(sideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, color, false, 0f, 0f, null);
		}

		// Token: 0x06000278 RID: 632 RVA: 0x000085F1 File Offset: 0x000067F1
		public static void RegularPolygon(int sideCount, float radius)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, null);
		}

		// Token: 0x06000279 RID: 633 RVA: 0x00008610 File Offset: 0x00006810
		public static void RegularPolygon(int sideCount, float radius, Color color)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, color, false, 0f, 0f, null);
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000862B File Offset: 0x0000682B
		public static void RegularPolygon(int sideCount, float radius, float angle)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, angle, null);
		}

		// Token: 0x0600027B RID: 635 RVA: 0x00008646 File Offset: 0x00006846
		public static void RegularPolygon(int sideCount, float radius, float angle, Color color)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, color, false, 0f, angle, null);
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000865D File Offset: 0x0000685D
		public static void RegularPolygon(int sideCount, float radius, float angle, float roundness)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, roundness, angle, null);
		}

		// Token: 0x0600027D RID: 637 RVA: 0x00008674 File Offset: 0x00006874
		public static void RegularPolygon(int sideCount, float radius, float angle, float roundness, Color color)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, color, false, roundness, angle, null);
		}

		// Token: 0x0600027E RID: 638 RVA: 0x00008688 File Offset: 0x00006888
		public static void RegularPolygonHollow(Vector3 pos)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600027F RID: 639 RVA: 0x000086BF File Offset: 0x000068BF
		public static void RegularPolygonHollow(Vector3 pos, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, color, true, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000280 RID: 640 RVA: 0x000086F2 File Offset: 0x000068F2
		public static void RegularPolygonHollow(Vector3 pos, float radius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000281 RID: 641 RVA: 0x00008725 File Offset: 0x00006925
		public static void RegularPolygonHollow(Vector3 pos, float radius, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, color, true, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000282 RID: 642 RVA: 0x00008754 File Offset: 0x00006954
		public static void RegularPolygonHollow(Vector3 pos, float radius, float thickness)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000283 RID: 643 RVA: 0x00008783 File Offset: 0x00006983
		public static void RegularPolygonHollow(Vector3 pos, float radius, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, color, true, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000284 RID: 644 RVA: 0x000087AE File Offset: 0x000069AE
		public static void RegularPolygonHollow(Vector3 pos, float radius, float thickness, float angle)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, 0f, angle, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000285 RID: 645 RVA: 0x000087D9 File Offset: 0x000069D9
		public static void RegularPolygonHollow(Vector3 pos, float radius, float thickness, float angle, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, color, true, 0f, angle, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000286 RID: 646 RVA: 0x00008801 File Offset: 0x00006A01
		public static void RegularPolygonHollow(Vector3 pos, float radius, float thickness, float angle, float roundness)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, roundness, angle, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000287 RID: 647 RVA: 0x00008829 File Offset: 0x00006A29
		public static void RegularPolygonHollow(Vector3 pos, float radius, float thickness, float angle, float roundness, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, color, true, roundness, angle, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000884E File Offset: 0x00006A4E
		public static void RegularPolygonHollow(Vector3 pos, int sideCount)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000289 RID: 649 RVA: 0x00008881 File Offset: 0x00006A81
		public static void RegularPolygonHollow(Vector3 pos, int sideCount, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, color, true, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600028A RID: 650 RVA: 0x000088B0 File Offset: 0x00006AB0
		public static void RegularPolygonHollow(Vector3 pos, int sideCount, float radius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600028B RID: 651 RVA: 0x000088DF File Offset: 0x00006ADF
		public static void RegularPolygonHollow(Vector3 pos, int sideCount, float radius, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, color, true, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000890A File Offset: 0x00006B0A
		public static void RegularPolygonHollow(Vector3 pos, int sideCount, float radius, float thickness)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600028D RID: 653 RVA: 0x00008935 File Offset: 0x00006B35
		public static void RegularPolygonHollow(Vector3 pos, int sideCount, float radius, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, color, true, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000895D File Offset: 0x00006B5D
		public static void RegularPolygonHollow(Vector3 pos, int sideCount, float radius, float thickness, float angle)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, 0f, angle, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600028F RID: 655 RVA: 0x00008985 File Offset: 0x00006B85
		public static void RegularPolygonHollow(Vector3 pos, int sideCount, float radius, float thickness, float angle, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, color, true, 0f, angle, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000290 RID: 656 RVA: 0x000089AA File Offset: 0x00006BAA
		public static void RegularPolygonHollow(Vector3 pos, int sideCount, float radius, float thickness, float angle, float roundness)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, roundness, angle, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000291 RID: 657 RVA: 0x000089CF File Offset: 0x00006BCF
		public static void RegularPolygonHollow(Vector3 pos, int sideCount, float radius, float thickness, float angle, float roundness, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, color, true, roundness, angle, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000292 RID: 658 RVA: 0x000089F4 File Offset: 0x00006BF4
		public static void RegularPolygonHollow(Vector3 pos, Vector3 normal)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000293 RID: 659 RVA: 0x00008A50 File Offset: 0x00006C50
		public static void RegularPolygonHollow(Vector3 pos, Vector3 normal, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, color, true, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000294 RID: 660 RVA: 0x00008AA8 File Offset: 0x00006CA8
		public static void RegularPolygonHollow(Vector3 pos, Vector3 normal, float radius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000295 RID: 661 RVA: 0x00008B00 File Offset: 0x00006D00
		public static void RegularPolygonHollow(Vector3 pos, Vector3 normal, float radius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, color, true, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000296 RID: 662 RVA: 0x00008B54 File Offset: 0x00006D54
		public static void RegularPolygonHollow(Vector3 pos, Vector3 normal, float radius, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000297 RID: 663 RVA: 0x00008BA8 File Offset: 0x00006DA8
		public static void RegularPolygonHollow(Vector3 pos, Vector3 normal, float radius, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, color, true, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000298 RID: 664 RVA: 0x00008BFC File Offset: 0x00006DFC
		public static void RegularPolygonHollow(Vector3 pos, Vector3 normal, float radius, float thickness, float angle)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, 0f, angle, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000299 RID: 665 RVA: 0x00008C50 File Offset: 0x00006E50
		public static void RegularPolygonHollow(Vector3 pos, Vector3 normal, float radius, float thickness, float angle, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, color, true, 0f, angle, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600029A RID: 666 RVA: 0x00008CA0 File Offset: 0x00006EA0
		public static void RegularPolygonHollow(Vector3 pos, Vector3 normal, float radius, float thickness, float angle, float roundness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, roundness, angle, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600029B RID: 667 RVA: 0x00008CEE File Offset: 0x00006EEE
		public static void RegularPolygonHollow(Vector3 pos, Vector3 normal, float radius, float thickness, float angle, float roundness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, color, true, roundness, angle, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600029C RID: 668 RVA: 0x00008D30 File Offset: 0x00006F30
		public static void RegularPolygonHollow(Vector3 pos, Vector3 normal, int sideCount)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600029D RID: 669 RVA: 0x00008D88 File Offset: 0x00006F88
		public static void RegularPolygonHollow(Vector3 pos, Vector3 normal, int sideCount, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, color, true, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600029E RID: 670 RVA: 0x00008DDC File Offset: 0x00006FDC
		public static void RegularPolygonHollow(Vector3 pos, Vector3 normal, int sideCount, float radius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600029F RID: 671 RVA: 0x00008E30 File Offset: 0x00007030
		public static void RegularPolygonHollow(Vector3 pos, Vector3 normal, int sideCount, float radius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, color, true, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x00008E84 File Offset: 0x00007084
		public static void RegularPolygonHollow(Vector3 pos, Vector3 normal, int sideCount, float radius, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x00008ED8 File Offset: 0x000070D8
		public static void RegularPolygonHollow(Vector3 pos, Vector3 normal, int sideCount, float radius, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, color, true, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x00008F28 File Offset: 0x00007128
		public static void RegularPolygonHollow(Vector3 pos, Vector3 normal, int sideCount, float radius, float thickness, float angle)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, 0f, angle, null);
			Draw.PopMatrix();
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x00008F76 File Offset: 0x00007176
		public static void RegularPolygonHollow(Vector3 pos, Vector3 normal, int sideCount, float radius, float thickness, float angle, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, color, true, 0f, angle, null);
			Draw.PopMatrix();
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x00008FB6 File Offset: 0x000071B6
		public static void RegularPolygonHollow(Vector3 pos, Vector3 normal, int sideCount, float radius, float thickness, float angle, float roundness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, roundness, angle, null);
			Draw.PopMatrix();
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x00008FF6 File Offset: 0x000071F6
		public static void RegularPolygonHollow(Vector3 pos, Vector3 normal, int sideCount, float radius, float thickness, float angle, float roundness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, color, true, roundness, angle, null);
			Draw.PopMatrix();
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x00009034 File Offset: 0x00007234
		public static void RegularPolygonHollow(Vector3 pos, Quaternion rot)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000908C File Offset: 0x0000728C
		public static void RegularPolygonHollow(Vector3 pos, Quaternion rot, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, color, true, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x000090E0 File Offset: 0x000072E0
		public static void RegularPolygonHollow(Vector3 pos, Quaternion rot, float radius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x00009134 File Offset: 0x00007334
		public static void RegularPolygonHollow(Vector3 pos, Quaternion rot, float radius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, color, true, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x060002AA RID: 682 RVA: 0x00009184 File Offset: 0x00007384
		public static void RegularPolygonHollow(Vector3 pos, Quaternion rot, float radius, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x060002AB RID: 683 RVA: 0x000091D4 File Offset: 0x000073D4
		public static void RegularPolygonHollow(Vector3 pos, Quaternion rot, float radius, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, color, true, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x060002AC RID: 684 RVA: 0x00009220 File Offset: 0x00007420
		public static void RegularPolygonHollow(Vector3 pos, Quaternion rot, float radius, float thickness, float angle)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, 0f, angle, null);
			Draw.PopMatrix();
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000926C File Offset: 0x0000746C
		public static void RegularPolygonHollow(Vector3 pos, Quaternion rot, float radius, float thickness, float angle, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, color, true, 0f, angle, null);
			Draw.PopMatrix();
		}

		// Token: 0x060002AE RID: 686 RVA: 0x000092AA File Offset: 0x000074AA
		public static void RegularPolygonHollow(Vector3 pos, Quaternion rot, float radius, float thickness, float angle, float roundness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, roundness, angle, null);
			Draw.PopMatrix();
		}

		// Token: 0x060002AF RID: 687 RVA: 0x000092E8 File Offset: 0x000074E8
		public static void RegularPolygonHollow(Vector3 pos, Quaternion rot, float radius, float thickness, float angle, float roundness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, color, true, roundness, angle, null);
			Draw.PopMatrix();
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x00009324 File Offset: 0x00007524
		public static void RegularPolygonHollow(Vector3 pos, Quaternion rot, int sideCount)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x00009378 File Offset: 0x00007578
		public static void RegularPolygonHollow(Vector3 pos, Quaternion rot, int sideCount, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, color, true, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x000093C8 File Offset: 0x000075C8
		public static void RegularPolygonHollow(Vector3 pos, Quaternion rot, int sideCount, float radius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x00009418 File Offset: 0x00007618
		public static void RegularPolygonHollow(Vector3 pos, Quaternion rot, int sideCount, float radius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, color, true, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x00009464 File Offset: 0x00007664
		public static void RegularPolygonHollow(Vector3 pos, Quaternion rot, int sideCount, float radius, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x000094B0 File Offset: 0x000076B0
		public static void RegularPolygonHollow(Vector3 pos, Quaternion rot, int sideCount, float radius, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, color, true, 0f, 0f, null);
			Draw.PopMatrix();
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x000094EE File Offset: 0x000076EE
		public static void RegularPolygonHollow(Vector3 pos, Quaternion rot, int sideCount, float radius, float thickness, float angle)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, 0f, angle, null);
			Draw.PopMatrix();
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000952C File Offset: 0x0000772C
		public static void RegularPolygonHollow(Vector3 pos, Quaternion rot, int sideCount, float radius, float thickness, float angle, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, color, true, 0f, angle, null);
			Draw.PopMatrix();
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x00009567 File Offset: 0x00007767
		public static void RegularPolygonHollow(Vector3 pos, Quaternion rot, int sideCount, float radius, float thickness, float angle, float roundness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, roundness, angle, null);
			Draw.PopMatrix();
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x000095A2 File Offset: 0x000077A2
		public static void RegularPolygonHollow(Vector3 pos, Quaternion rot, int sideCount, float radius, float thickness, float angle, float roundness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, color, true, roundness, angle, null);
			Draw.PopMatrix();
		}

		// Token: 0x060002BA RID: 698 RVA: 0x000095DA File Offset: 0x000077DA
		public static void RegularPolygonHollow()
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, null);
		}

		// Token: 0x060002BB RID: 699 RVA: 0x00009601 File Offset: 0x00007801
		public static void RegularPolygonHollow(Color color)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, color, true, 0f, 0f, null);
		}

		// Token: 0x060002BC RID: 700 RVA: 0x00009624 File Offset: 0x00007824
		public static void RegularPolygonHollow(float radius)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, null);
		}

		// Token: 0x060002BD RID: 701 RVA: 0x00009647 File Offset: 0x00007847
		public static void RegularPolygonHollow(float radius, Color color)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, color, true, 0f, 0f, null);
		}

		// Token: 0x060002BE RID: 702 RVA: 0x00009666 File Offset: 0x00007866
		public static void RegularPolygonHollow(float radius, float thickness)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, 0f, 0f, null);
		}

		// Token: 0x060002BF RID: 703 RVA: 0x00009685 File Offset: 0x00007885
		public static void RegularPolygonHollow(float radius, float thickness, Color color)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, color, true, 0f, 0f, null);
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x000096A0 File Offset: 0x000078A0
		public static void RegularPolygonHollow(float radius, float thickness, float angle)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, 0f, angle, null);
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x000096BB File Offset: 0x000078BB
		public static void RegularPolygonHollow(float radius, float thickness, float angle, Color color)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, color, true, 0f, angle, null);
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x000096D2 File Offset: 0x000078D2
		public static void RegularPolygonHollow(float radius, float thickness, float angle, float roundness)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, roundness, angle, null);
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x000096E9 File Offset: 0x000078E9
		public static void RegularPolygonHollow(float radius, float thickness, float angle, float roundness, Color color)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, color, true, roundness, angle, null);
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x000096FD File Offset: 0x000078FD
		public static void RegularPolygonHollow(int sideCount)
		{
			Draw.RegularPolygon_Internal(sideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, null);
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x00009720 File Offset: 0x00007920
		public static void RegularPolygonHollow(int sideCount, Color color)
		{
			Draw.RegularPolygon_Internal(sideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, color, true, 0f, 0f, null);
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000973F File Offset: 0x0000793F
		public static void RegularPolygonHollow(int sideCount, float radius)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, null);
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000975E File Offset: 0x0000795E
		public static void RegularPolygonHollow(int sideCount, float radius, Color color)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, color, true, 0f, 0f, null);
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x00009779 File Offset: 0x00007979
		public static void RegularPolygonHollow(int sideCount, float radius, float thickness)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, 0f, 0f, null);
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x00009794 File Offset: 0x00007994
		public static void RegularPolygonHollow(int sideCount, float radius, float thickness, Color color)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, color, true, 0f, 0f, null);
		}

		// Token: 0x060002CA RID: 714 RVA: 0x000097AB File Offset: 0x000079AB
		public static void RegularPolygonHollow(int sideCount, float radius, float thickness, float angle)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, 0f, angle, null);
		}

		// Token: 0x060002CB RID: 715 RVA: 0x000097C2 File Offset: 0x000079C2
		public static void RegularPolygonHollow(int sideCount, float radius, float thickness, float angle, Color color)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, color, true, 0f, angle, null);
		}

		// Token: 0x060002CC RID: 716 RVA: 0x000097D6 File Offset: 0x000079D6
		public static void RegularPolygonHollow(int sideCount, float radius, float thickness, float angle, float roundness)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, roundness, angle, null);
		}

		// Token: 0x060002CD RID: 717 RVA: 0x000097EA File Offset: 0x000079EA
		public static void RegularPolygonHollow(int sideCount, float radius, float thickness, float angle, float roundness, Color color)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, color, true, roundness, angle, null);
		}

		// Token: 0x060002CE RID: 718 RVA: 0x000097FB File Offset: 0x000079FB
		public static void RegularPolygonFill(Vector3 pos)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060002CF RID: 719 RVA: 0x00009836 File Offset: 0x00007A36
		public static void RegularPolygonFill(Vector3 pos, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, fill);
			Draw.PopMatrix();
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000986D File Offset: 0x00007A6D
		public static void RegularPolygonFill(Vector3 pos, float radius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x000098A4 File Offset: 0x00007AA4
		public static void RegularPolygonFill(Vector3 pos, float radius, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, fill);
			Draw.PopMatrix();
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x000098D7 File Offset: 0x00007AD7
		public static void RegularPolygonFill(Vector3 pos, float radius, float angle)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, angle, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000990A File Offset: 0x00007B0A
		public static void RegularPolygonFill(Vector3 pos, float radius, float angle, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, angle, fill);
			Draw.PopMatrix();
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x00009939 File Offset: 0x00007B39
		public static void RegularPolygonFill(Vector3 pos, float radius, float angle, float roundness)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, roundness, angle, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x00009968 File Offset: 0x00007B68
		public static void RegularPolygonFill(Vector3 pos, float radius, float angle, float roundness, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, roundness, angle, fill);
			Draw.PopMatrix();
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x00009994 File Offset: 0x00007B94
		public static void RegularPolygonFill(Vector3 pos, int sideCount)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x000099CB File Offset: 0x00007BCB
		public static void RegularPolygonFill(Vector3 pos, int sideCount, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, fill);
			Draw.PopMatrix();
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x000099FE File Offset: 0x00007BFE
		public static void RegularPolygonFill(Vector3 pos, int sideCount, float radius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x00009A31 File Offset: 0x00007C31
		public static void RegularPolygonFill(Vector3 pos, int sideCount, float radius, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, fill);
			Draw.PopMatrix();
		}

		// Token: 0x060002DA RID: 730 RVA: 0x00009A60 File Offset: 0x00007C60
		public static void RegularPolygonFill(Vector3 pos, int sideCount, float radius, float angle)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, angle, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060002DB RID: 731 RVA: 0x00009A8F File Offset: 0x00007C8F
		public static void RegularPolygonFill(Vector3 pos, int sideCount, float radius, float angle, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, angle, fill);
			Draw.PopMatrix();
		}

		// Token: 0x060002DC RID: 732 RVA: 0x00009ABB File Offset: 0x00007CBB
		public static void RegularPolygonFill(Vector3 pos, int sideCount, float radius, float angle, float roundness)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, roundness, angle, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060002DD RID: 733 RVA: 0x00009AE7 File Offset: 0x00007CE7
		public static void RegularPolygonFill(Vector3 pos, int sideCount, float radius, float angle, float roundness, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, roundness, angle, fill);
			Draw.PopMatrix();
		}

		// Token: 0x060002DE RID: 734 RVA: 0x00009B10 File Offset: 0x00007D10
		public static void RegularPolygonFill(Vector3 pos, Vector3 normal)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060002DF RID: 735 RVA: 0x00009B70 File Offset: 0x00007D70
		public static void RegularPolygonFill(Vector3 pos, Vector3 normal, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, fill);
			Draw.PopMatrix();
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x00009BCC File Offset: 0x00007DCC
		public static void RegularPolygonFill(Vector3 pos, Vector3 normal, float radius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x00009C28 File Offset: 0x00007E28
		public static void RegularPolygonFill(Vector3 pos, Vector3 normal, float radius, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, fill);
			Draw.PopMatrix();
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x00009C80 File Offset: 0x00007E80
		public static void RegularPolygonFill(Vector3 pos, Vector3 normal, float radius, float angle)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, angle, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x00009CD8 File Offset: 0x00007ED8
		public static void RegularPolygonFill(Vector3 pos, Vector3 normal, float radius, float angle, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, angle, fill);
			Draw.PopMatrix();
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x00009D30 File Offset: 0x00007F30
		public static void RegularPolygonFill(Vector3 pos, Vector3 normal, float radius, float angle, float roundness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, roundness, angle, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x00009D88 File Offset: 0x00007F88
		public static void RegularPolygonFill(Vector3 pos, Vector3 normal, float radius, float angle, float roundness, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, roundness, angle, fill);
			Draw.PopMatrix();
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x00009DDC File Offset: 0x00007FDC
		public static void RegularPolygonFill(Vector3 pos, Vector3 normal, int sideCount)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x00009E38 File Offset: 0x00008038
		public static void RegularPolygonFill(Vector3 pos, Vector3 normal, int sideCount, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, fill);
			Draw.PopMatrix();
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x00009E90 File Offset: 0x00008090
		public static void RegularPolygonFill(Vector3 pos, Vector3 normal, int sideCount, float radius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x00009EE8 File Offset: 0x000080E8
		public static void RegularPolygonFill(Vector3 pos, Vector3 normal, int sideCount, float radius, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, fill);
			Draw.PopMatrix();
		}

		// Token: 0x060002EA RID: 746 RVA: 0x00009F40 File Offset: 0x00008140
		public static void RegularPolygonFill(Vector3 pos, Vector3 normal, int sideCount, float radius, float angle)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, angle, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060002EB RID: 747 RVA: 0x00009F98 File Offset: 0x00008198
		public static void RegularPolygonFill(Vector3 pos, Vector3 normal, int sideCount, float radius, float angle, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, angle, fill);
			Draw.PopMatrix();
		}

		// Token: 0x060002EC RID: 748 RVA: 0x00009FEC File Offset: 0x000081EC
		public static void RegularPolygonFill(Vector3 pos, Vector3 normal, int sideCount, float radius, float angle, float roundness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, roundness, angle, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000A040 File Offset: 0x00008240
		public static void RegularPolygonFill(Vector3 pos, Vector3 normal, int sideCount, float radius, float angle, float roundness, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, roundness, angle, fill);
			Draw.PopMatrix();
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0000A090 File Offset: 0x00008290
		public static void RegularPolygonFill(Vector3 pos, Quaternion rot)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0000A0EC File Offset: 0x000082EC
		public static void RegularPolygonFill(Vector3 pos, Quaternion rot, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, fill);
			Draw.PopMatrix();
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000A144 File Offset: 0x00008344
		public static void RegularPolygonFill(Vector3 pos, Quaternion rot, float radius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000A19C File Offset: 0x0000839C
		public static void RegularPolygonFill(Vector3 pos, Quaternion rot, float radius, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, fill);
			Draw.PopMatrix();
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000A1F0 File Offset: 0x000083F0
		public static void RegularPolygonFill(Vector3 pos, Quaternion rot, float radius, float angle)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, angle, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000A244 File Offset: 0x00008444
		public static void RegularPolygonFill(Vector3 pos, Quaternion rot, float radius, float angle, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, angle, fill);
			Draw.PopMatrix();
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000A294 File Offset: 0x00008494
		public static void RegularPolygonFill(Vector3 pos, Quaternion rot, float radius, float angle, float roundness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, roundness, angle, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public static void RegularPolygonFill(Vector3 pos, Quaternion rot, float radius, float angle, float roundness, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, roundness, angle, fill);
			Draw.PopMatrix();
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000A334 File Offset: 0x00008534
		public static void RegularPolygonFill(Vector3 pos, Quaternion rot, int sideCount)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000A38C File Offset: 0x0000858C
		public static void RegularPolygonFill(Vector3 pos, Quaternion rot, int sideCount, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, fill);
			Draw.PopMatrix();
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000A3E0 File Offset: 0x000085E0
		public static void RegularPolygonFill(Vector3 pos, Quaternion rot, int sideCount, float radius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000A434 File Offset: 0x00008634
		public static void RegularPolygonFill(Vector3 pos, Quaternion rot, int sideCount, float radius, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, fill);
			Draw.PopMatrix();
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000A484 File Offset: 0x00008684
		public static void RegularPolygonFill(Vector3 pos, Quaternion rot, int sideCount, float radius, float angle)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, angle, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000A4D4 File Offset: 0x000086D4
		public static void RegularPolygonFill(Vector3 pos, Quaternion rot, int sideCount, float radius, float angle, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, angle, fill);
			Draw.PopMatrix();
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000A524 File Offset: 0x00008724
		public static void RegularPolygonFill(Vector3 pos, Quaternion rot, int sideCount, float radius, float angle, float roundness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, roundness, angle, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000A571 File Offset: 0x00008771
		public static void RegularPolygonFill(Vector3 pos, Quaternion rot, int sideCount, float radius, float angle, float roundness, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, roundness, angle, fill);
			Draw.PopMatrix();
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000A5B0 File Offset: 0x000087B0
		public static void RegularPolygonFill()
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, Draw.PolygonShapeFill);
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000A5DB File Offset: 0x000087DB
		public static void RegularPolygonFill(ShapeFill fill)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, fill);
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000A602 File Offset: 0x00008802
		public static void RegularPolygonFill(float radius)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, Draw.PolygonShapeFill);
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000A629 File Offset: 0x00008829
		public static void RegularPolygonFill(float radius, ShapeFill fill)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, fill);
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0000A64C File Offset: 0x0000884C
		public static void RegularPolygonFill(float radius, float angle)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, angle, Draw.PolygonShapeFill);
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0000A66F File Offset: 0x0000886F
		public static void RegularPolygonFill(float radius, float angle, ShapeFill fill)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, angle, fill);
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0000A68E File Offset: 0x0000888E
		public static void RegularPolygonFill(float radius, float angle, float roundness)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, roundness, angle, Draw.PolygonShapeFill);
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0000A6AD File Offset: 0x000088AD
		public static void RegularPolygonFill(float radius, float angle, float roundness, ShapeFill fill)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, roundness, angle, fill);
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0000A6C8 File Offset: 0x000088C8
		public static void RegularPolygonFill(int sideCount)
		{
			Draw.RegularPolygon_Internal(sideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, Draw.PolygonShapeFill);
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0000A6EF File Offset: 0x000088EF
		public static void RegularPolygonFill(int sideCount, ShapeFill fill)
		{
			Draw.RegularPolygon_Internal(sideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, fill);
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0000A712 File Offset: 0x00008912
		public static void RegularPolygonFill(int sideCount, float radius)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, Draw.PolygonShapeFill);
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0000A735 File Offset: 0x00008935
		public static void RegularPolygonFill(int sideCount, float radius, ShapeFill fill)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, fill);
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000A754 File Offset: 0x00008954
		public static void RegularPolygonFill(int sideCount, float radius, float angle)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, angle, Draw.PolygonShapeFill);
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000A773 File Offset: 0x00008973
		public static void RegularPolygonFill(int sideCount, float radius, float angle, ShapeFill fill)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, angle, fill);
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000A78E File Offset: 0x0000898E
		public static void RegularPolygonFill(int sideCount, float radius, float angle, float roundness)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, roundness, angle, Draw.PolygonShapeFill);
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0000A7A9 File Offset: 0x000089A9
		public static void RegularPolygonFill(int sideCount, float radius, float angle, float roundness, ShapeFill fill)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, roundness, angle, fill);
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0000A7C1 File Offset: 0x000089C1
		public static void RegularPolygonHollowFill(Vector3 pos)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000A7FC File Offset: 0x000089FC
		public static void RegularPolygonHollowFill(Vector3 pos, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0000A833 File Offset: 0x00008A33
		public static void RegularPolygonHollowFill(Vector3 pos, float radius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000311 RID: 785 RVA: 0x0000A86A File Offset: 0x00008A6A
		public static void RegularPolygonHollowFill(Vector3 pos, float radius, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0000A89D File Offset: 0x00008A9D
		public static void RegularPolygonHollowFill(Vector3 pos, float radius, float thickness)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, 0f, 0f, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0000A8D0 File Offset: 0x00008AD0
		public static void RegularPolygonHollowFill(Vector3 pos, float radius, float thickness, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, 0f, 0f, fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000314 RID: 788 RVA: 0x0000A8FF File Offset: 0x00008AFF
		public static void RegularPolygonHollowFill(Vector3 pos, float radius, float thickness, float angle)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, 0f, angle, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000315 RID: 789 RVA: 0x0000A92E File Offset: 0x00008B2E
		public static void RegularPolygonHollowFill(Vector3 pos, float radius, float thickness, float angle, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, 0f, angle, fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000316 RID: 790 RVA: 0x0000A95A File Offset: 0x00008B5A
		public static void RegularPolygonHollowFill(Vector3 pos, float radius, float thickness, float angle, float roundness)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, roundness, angle, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000317 RID: 791 RVA: 0x0000A986 File Offset: 0x00008B86
		public static void RegularPolygonHollowFill(Vector3 pos, float radius, float thickness, float angle, float roundness, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, roundness, angle, fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000318 RID: 792 RVA: 0x0000A9AF File Offset: 0x00008BAF
		public static void RegularPolygonHollowFill(Vector3 pos, int sideCount)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000319 RID: 793 RVA: 0x0000A9E6 File Offset: 0x00008BE6
		public static void RegularPolygonHollowFill(Vector3 pos, int sideCount, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, fill);
			Draw.PopMatrix();
		}

		// Token: 0x0600031A RID: 794 RVA: 0x0000AA19 File Offset: 0x00008C19
		public static void RegularPolygonHollowFill(Vector3 pos, int sideCount, float radius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0000AA4C File Offset: 0x00008C4C
		public static void RegularPolygonHollowFill(Vector3 pos, int sideCount, float radius, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, fill);
			Draw.PopMatrix();
		}

		// Token: 0x0600031C RID: 796 RVA: 0x0000AA7B File Offset: 0x00008C7B
		public static void RegularPolygonHollowFill(Vector3 pos, int sideCount, float radius, float thickness)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, 0f, 0f, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x0600031D RID: 797 RVA: 0x0000AAAA File Offset: 0x00008CAA
		public static void RegularPolygonHollowFill(Vector3 pos, int sideCount, float radius, float thickness, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, 0f, 0f, fill);
			Draw.PopMatrix();
		}

		// Token: 0x0600031E RID: 798 RVA: 0x0000AAD6 File Offset: 0x00008CD6
		public static void RegularPolygonHollowFill(Vector3 pos, int sideCount, float radius, float thickness, float angle)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, 0f, angle, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x0600031F RID: 799 RVA: 0x0000AB02 File Offset: 0x00008D02
		public static void RegularPolygonHollowFill(Vector3 pos, int sideCount, float radius, float thickness, float angle, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, 0f, angle, fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000320 RID: 800 RVA: 0x0000AB2B File Offset: 0x00008D2B
		public static void RegularPolygonHollowFill(Vector3 pos, int sideCount, float radius, float thickness, float angle, float roundness)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, roundness, angle, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000321 RID: 801 RVA: 0x0000AB54 File Offset: 0x00008D54
		public static void RegularPolygonHollowFill(Vector3 pos, int sideCount, float radius, float thickness, float angle, float roundness, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, roundness, angle, fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000322 RID: 802 RVA: 0x0000AB7C File Offset: 0x00008D7C
		public static void RegularPolygonHollowFill(Vector3 pos, Vector3 normal)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0000ABDC File Offset: 0x00008DDC
		public static void RegularPolygonHollowFill(Vector3 pos, Vector3 normal, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0000AC38 File Offset: 0x00008E38
		public static void RegularPolygonHollowFill(Vector3 pos, Vector3 normal, float radius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0000AC94 File Offset: 0x00008E94
		public static void RegularPolygonHollowFill(Vector3 pos, Vector3 normal, float radius, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0000ACEC File Offset: 0x00008EEC
		public static void RegularPolygonHollowFill(Vector3 pos, Vector3 normal, float radius, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, 0f, 0f, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0000AD44 File Offset: 0x00008F44
		public static void RegularPolygonHollowFill(Vector3 pos, Vector3 normal, float radius, float thickness, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, 0f, 0f, fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0000AD9C File Offset: 0x00008F9C
		public static void RegularPolygonHollowFill(Vector3 pos, Vector3 normal, float radius, float thickness, float angle)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, 0f, angle, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0000ADF4 File Offset: 0x00008FF4
		public static void RegularPolygonHollowFill(Vector3 pos, Vector3 normal, float radius, float thickness, float angle, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, 0f, angle, fill);
			Draw.PopMatrix();
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0000AE48 File Offset: 0x00009048
		public static void RegularPolygonHollowFill(Vector3 pos, Vector3 normal, float radius, float thickness, float angle, float roundness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, roundness, angle, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000AE9C File Offset: 0x0000909C
		public static void RegularPolygonHollowFill(Vector3 pos, Vector3 normal, float radius, float thickness, float angle, float roundness, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, roundness, angle, fill);
			Draw.PopMatrix();
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0000AEEC File Offset: 0x000090EC
		public static void RegularPolygonHollowFill(Vector3 pos, Vector3 normal, int sideCount)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0000AF48 File Offset: 0x00009148
		public static void RegularPolygonHollowFill(Vector3 pos, Vector3 normal, int sideCount, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, fill);
			Draw.PopMatrix();
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0000AFA0 File Offset: 0x000091A0
		public static void RegularPolygonHollowFill(Vector3 pos, Vector3 normal, int sideCount, float radius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0000AFF8 File Offset: 0x000091F8
		public static void RegularPolygonHollowFill(Vector3 pos, Vector3 normal, int sideCount, float radius, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0000B050 File Offset: 0x00009250
		public static void RegularPolygonHollowFill(Vector3 pos, Vector3 normal, int sideCount, float radius, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, 0f, 0f, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000331 RID: 817 RVA: 0x0000B0A8 File Offset: 0x000092A8
		public static void RegularPolygonHollowFill(Vector3 pos, Vector3 normal, int sideCount, float radius, float thickness, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, 0f, 0f, fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000332 RID: 818 RVA: 0x0000B0FC File Offset: 0x000092FC
		public static void RegularPolygonHollowFill(Vector3 pos, Vector3 normal, int sideCount, float radius, float thickness, float angle)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, 0f, angle, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000333 RID: 819 RVA: 0x0000B150 File Offset: 0x00009350
		public static void RegularPolygonHollowFill(Vector3 pos, Vector3 normal, int sideCount, float radius, float thickness, float angle, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, 0f, angle, fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000334 RID: 820 RVA: 0x0000B1A0 File Offset: 0x000093A0
		public static void RegularPolygonHollowFill(Vector3 pos, Vector3 normal, int sideCount, float radius, float thickness, float angle, float roundness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, roundness, angle, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000335 RID: 821 RVA: 0x0000B1F0 File Offset: 0x000093F0
		public static void RegularPolygonHollowFill(Vector3 pos, Vector3 normal, int sideCount, float radius, float thickness, float angle, float roundness, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, roundness, angle, fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0000B23C File Offset: 0x0000943C
		public static void RegularPolygonHollowFill(Vector3 pos, Quaternion rot)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000337 RID: 823 RVA: 0x0000B298 File Offset: 0x00009498
		public static void RegularPolygonHollowFill(Vector3 pos, Quaternion rot, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000338 RID: 824 RVA: 0x0000B2F0 File Offset: 0x000094F0
		public static void RegularPolygonHollowFill(Vector3 pos, Quaternion rot, float radius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000339 RID: 825 RVA: 0x0000B348 File Offset: 0x00009548
		public static void RegularPolygonHollowFill(Vector3 pos, Quaternion rot, float radius, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, fill);
			Draw.PopMatrix();
		}

		// Token: 0x0600033A RID: 826 RVA: 0x0000B39C File Offset: 0x0000959C
		public static void RegularPolygonHollowFill(Vector3 pos, Quaternion rot, float radius, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, 0f, 0f, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x0600033B RID: 827 RVA: 0x0000B3F0 File Offset: 0x000095F0
		public static void RegularPolygonHollowFill(Vector3 pos, Quaternion rot, float radius, float thickness, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, 0f, 0f, fill);
			Draw.PopMatrix();
		}

		// Token: 0x0600033C RID: 828 RVA: 0x0000B440 File Offset: 0x00009640
		public static void RegularPolygonHollowFill(Vector3 pos, Quaternion rot, float radius, float thickness, float angle)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, 0f, angle, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x0600033D RID: 829 RVA: 0x0000B490 File Offset: 0x00009690
		public static void RegularPolygonHollowFill(Vector3 pos, Quaternion rot, float radius, float thickness, float angle, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, 0f, angle, fill);
			Draw.PopMatrix();
		}

		// Token: 0x0600033E RID: 830 RVA: 0x0000B4E0 File Offset: 0x000096E0
		public static void RegularPolygonHollowFill(Vector3 pos, Quaternion rot, float radius, float thickness, float angle, float roundness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, roundness, angle, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x0600033F RID: 831 RVA: 0x0000B52D File Offset: 0x0000972D
		public static void RegularPolygonHollowFill(Vector3 pos, Quaternion rot, float radius, float thickness, float angle, float roundness, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, roundness, angle, fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000340 RID: 832 RVA: 0x0000B56C File Offset: 0x0000976C
		public static void RegularPolygonHollowFill(Vector3 pos, Quaternion rot, int sideCount)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000341 RID: 833 RVA: 0x0000B5C4 File Offset: 0x000097C4
		public static void RegularPolygonHollowFill(Vector3 pos, Quaternion rot, int sideCount, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000342 RID: 834 RVA: 0x0000B618 File Offset: 0x00009818
		public static void RegularPolygonHollowFill(Vector3 pos, Quaternion rot, int sideCount, float radius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000343 RID: 835 RVA: 0x0000B66C File Offset: 0x0000986C
		public static void RegularPolygonHollowFill(Vector3 pos, Quaternion rot, int sideCount, float radius, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000B6BC File Offset: 0x000098BC
		public static void RegularPolygonHollowFill(Vector3 pos, Quaternion rot, int sideCount, float radius, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, 0f, 0f, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0000B70C File Offset: 0x0000990C
		public static void RegularPolygonHollowFill(Vector3 pos, Quaternion rot, int sideCount, float radius, float thickness, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, 0f, 0f, fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000346 RID: 838 RVA: 0x0000B75C File Offset: 0x0000995C
		public static void RegularPolygonHollowFill(Vector3 pos, Quaternion rot, int sideCount, float radius, float thickness, float angle)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, 0f, angle, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0000B7A9 File Offset: 0x000099A9
		public static void RegularPolygonHollowFill(Vector3 pos, Quaternion rot, int sideCount, float radius, float thickness, float angle, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, 0f, angle, fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0000B7E8 File Offset: 0x000099E8
		public static void RegularPolygonHollowFill(Vector3 pos, Quaternion rot, int sideCount, float radius, float thickness, float angle, float roundness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, roundness, angle, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000349 RID: 841 RVA: 0x0000B827 File Offset: 0x00009A27
		public static void RegularPolygonHollowFill(Vector3 pos, Quaternion rot, int sideCount, float radius, float thickness, float angle, float roundness, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, roundness, angle, fill);
			Draw.PopMatrix();
		}

		// Token: 0x0600034A RID: 842 RVA: 0x0000B863 File Offset: 0x00009A63
		public static void RegularPolygonHollowFill()
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, Draw.PolygonShapeFill);
		}

		// Token: 0x0600034B RID: 843 RVA: 0x0000B88E File Offset: 0x00009A8E
		public static void RegularPolygonHollowFill(ShapeFill fill)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, fill);
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0000B8B5 File Offset: 0x00009AB5
		public static void RegularPolygonHollowFill(float radius)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, Draw.PolygonShapeFill);
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0000B8DC File Offset: 0x00009ADC
		public static void RegularPolygonHollowFill(float radius, ShapeFill fill)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, fill);
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0000B8FF File Offset: 0x00009AFF
		public static void RegularPolygonHollowFill(float radius, float thickness)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, 0f, 0f, Draw.PolygonShapeFill);
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0000B922 File Offset: 0x00009B22
		public static void RegularPolygonHollowFill(float radius, float thickness, ShapeFill fill)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, 0f, 0f, fill);
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0000B941 File Offset: 0x00009B41
		public static void RegularPolygonHollowFill(float radius, float thickness, float angle)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, 0f, angle, Draw.PolygonShapeFill);
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0000B960 File Offset: 0x00009B60
		public static void RegularPolygonHollowFill(float radius, float thickness, float angle, ShapeFill fill)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, 0f, angle, fill);
		}

		// Token: 0x06000352 RID: 850 RVA: 0x0000B97B File Offset: 0x00009B7B
		public static void RegularPolygonHollowFill(float radius, float thickness, float angle, float roundness)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, roundness, angle, Draw.PolygonShapeFill);
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0000B996 File Offset: 0x00009B96
		public static void RegularPolygonHollowFill(float radius, float thickness, float angle, float roundness, ShapeFill fill)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, roundness, angle, fill);
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0000B9AE File Offset: 0x00009BAE
		public static void RegularPolygonHollowFill(int sideCount)
		{
			Draw.RegularPolygon_Internal(sideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, Draw.PolygonShapeFill);
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0000B9D5 File Offset: 0x00009BD5
		public static void RegularPolygonHollowFill(int sideCount, ShapeFill fill)
		{
			Draw.RegularPolygon_Internal(sideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, fill);
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0000B9F8 File Offset: 0x00009BF8
		public static void RegularPolygonHollowFill(int sideCount, float radius)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, Draw.PolygonShapeFill);
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0000BA1B File Offset: 0x00009C1B
		public static void RegularPolygonHollowFill(int sideCount, float radius, ShapeFill fill)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, fill);
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0000BA3A File Offset: 0x00009C3A
		public static void RegularPolygonHollowFill(int sideCount, float radius, float thickness)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, 0f, 0f, Draw.PolygonShapeFill);
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0000BA59 File Offset: 0x00009C59
		public static void RegularPolygonHollowFill(int sideCount, float radius, float thickness, ShapeFill fill)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, 0f, 0f, fill);
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0000BA74 File Offset: 0x00009C74
		public static void RegularPolygonHollowFill(int sideCount, float radius, float thickness, float angle)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, 0f, angle, Draw.PolygonShapeFill);
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0000BA8F File Offset: 0x00009C8F
		public static void RegularPolygonHollowFill(int sideCount, float radius, float thickness, float angle, ShapeFill fill)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, 0f, angle, fill);
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0000BAA7 File Offset: 0x00009CA7
		public static void RegularPolygonHollowFill(int sideCount, float radius, float thickness, float angle, float roundness)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, roundness, angle, Draw.PolygonShapeFill);
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0000BABF File Offset: 0x00009CBF
		public static void RegularPolygonHollowFill(int sideCount, float radius, float thickness, float angle, float roundness, ShapeFill fill)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, roundness, angle, fill);
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0000BAD4 File Offset: 0x00009CD4
		public static void RegularPolygonFillLinear(Vector3 pos, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0000BB24 File Offset: 0x00009D24
		public static void RegularPolygonFillLinear(Vector3 pos, float radius, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000BB70 File Offset: 0x00009D70
		public static void RegularPolygonFillLinear(Vector3 pos, float radius, float angle, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, angle, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0000BBB8 File Offset: 0x00009DB8
		public static void RegularPolygonFillLinear(Vector3 pos, float radius, float angle, float roundness, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, roundness, angle, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0000BBFC File Offset: 0x00009DFC
		public static void RegularPolygonFillLinear(Vector3 pos, int sideCount, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0000BC48 File Offset: 0x00009E48
		public static void RegularPolygonFillLinear(Vector3 pos, int sideCount, float radius, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0000BC90 File Offset: 0x00009E90
		public static void RegularPolygonFillLinear(Vector3 pos, int sideCount, float radius, float angle, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, angle, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0000BCD4 File Offset: 0x00009ED4
		public static void RegularPolygonFillLinear(Vector3 pos, int sideCount, float radius, float angle, float roundness, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, roundness, angle, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0000BD18 File Offset: 0x00009F18
		public static void RegularPolygonFillLinear(Vector3 pos, Vector3 normal, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0000BD80 File Offset: 0x00009F80
		public static void RegularPolygonFillLinear(Vector3 pos, Vector3 normal, float radius, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x06000368 RID: 872 RVA: 0x0000BDE8 File Offset: 0x00009FE8
		public static void RegularPolygonFillLinear(Vector3 pos, Vector3 normal, float radius, float angle, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, angle, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0000BE4C File Offset: 0x0000A04C
		public static void RegularPolygonFillLinear(Vector3 pos, Vector3 normal, float radius, float angle, float roundness, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, roundness, angle, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0000BEAC File Offset: 0x0000A0AC
		public static void RegularPolygonFillLinear(Vector3 pos, Vector3 normal, int sideCount, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0000BF14 File Offset: 0x0000A114
		public static void RegularPolygonFillLinear(Vector3 pos, Vector3 normal, int sideCount, float radius, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0000BF78 File Offset: 0x0000A178
		public static void RegularPolygonFillLinear(Vector3 pos, Vector3 normal, int sideCount, float radius, float angle, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, angle, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0000BFD8 File Offset: 0x0000A1D8
		public static void RegularPolygonFillLinear(Vector3 pos, Vector3 normal, int sideCount, float radius, float angle, float roundness, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, roundness, angle, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x0600036E RID: 878 RVA: 0x0000C034 File Offset: 0x0000A234
		public static void RegularPolygonFillLinear(Vector3 pos, Quaternion rot, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x0600036F RID: 879 RVA: 0x0000C098 File Offset: 0x0000A298
		public static void RegularPolygonFillLinear(Vector3 pos, Quaternion rot, float radius, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x06000370 RID: 880 RVA: 0x0000C0F8 File Offset: 0x0000A2F8
		public static void RegularPolygonFillLinear(Vector3 pos, Quaternion rot, float radius, float angle, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, angle, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x06000371 RID: 881 RVA: 0x0000C158 File Offset: 0x0000A358
		public static void RegularPolygonFillLinear(Vector3 pos, Quaternion rot, float radius, float angle, float roundness, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, roundness, angle, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x06000372 RID: 882 RVA: 0x0000C1B4 File Offset: 0x0000A3B4
		public static void RegularPolygonFillLinear(Vector3 pos, Quaternion rot, int sideCount, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x06000373 RID: 883 RVA: 0x0000C214 File Offset: 0x0000A414
		public static void RegularPolygonFillLinear(Vector3 pos, Quaternion rot, int sideCount, float radius, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0000C274 File Offset: 0x0000A474
		public static void RegularPolygonFillLinear(Vector3 pos, Quaternion rot, int sideCount, float radius, float angle, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, angle, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0000C2D0 File Offset: 0x0000A4D0
		public static void RegularPolygonFillLinear(Vector3 pos, Quaternion rot, int sideCount, float radius, float angle, float roundness, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, roundness, angle, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0000C328 File Offset: 0x0000A528
		public static void RegularPolygonFillLinear(Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0000C364 File Offset: 0x0000A564
		public static void RegularPolygonFillLinear(float radius, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
		}

		// Token: 0x06000378 RID: 888 RVA: 0x0000C3A0 File Offset: 0x0000A5A0
		public static void RegularPolygonFillLinear(float radius, float angle, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, angle, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
		}

		// Token: 0x06000379 RID: 889 RVA: 0x0000C3D8 File Offset: 0x0000A5D8
		public static void RegularPolygonFillLinear(float radius, float angle, float roundness, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, roundness, angle, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0000C40C File Offset: 0x0000A60C
		public static void RegularPolygonFillLinear(int sideCount, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.RegularPolygon_Internal(sideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0000C448 File Offset: 0x0000A648
		public static void RegularPolygonFillLinear(int sideCount, float radius, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
		}

		// Token: 0x0600037C RID: 892 RVA: 0x0000C480 File Offset: 0x0000A680
		public static void RegularPolygonFillLinear(int sideCount, float radius, float angle, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, angle, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
		}

		// Token: 0x0600037D RID: 893 RVA: 0x0000C4B4 File Offset: 0x0000A6B4
		public static void RegularPolygonFillLinear(int sideCount, float radius, float angle, float roundness, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, roundness, angle, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
		}

		// Token: 0x0600037E RID: 894 RVA: 0x0000C4E4 File Offset: 0x0000A6E4
		public static void RegularPolygonHollowFillLinear(Vector3 pos, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x0600037F RID: 895 RVA: 0x0000C534 File Offset: 0x0000A734
		public static void RegularPolygonHollowFillLinear(Vector3 pos, float radius, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x06000380 RID: 896 RVA: 0x0000C580 File Offset: 0x0000A780
		public static void RegularPolygonHollowFillLinear(Vector3 pos, float radius, float thickness, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, 0f, 0f, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x06000381 RID: 897 RVA: 0x0000C5C8 File Offset: 0x0000A7C8
		public static void RegularPolygonHollowFillLinear(Vector3 pos, float radius, float thickness, float angle, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, 0f, angle, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0000C60C File Offset: 0x0000A80C
		public static void RegularPolygonHollowFillLinear(Vector3 pos, float radius, float thickness, float angle, float roundness, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, roundness, angle, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0000C650 File Offset: 0x0000A850
		public static void RegularPolygonHollowFillLinear(Vector3 pos, int sideCount, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0000C69C File Offset: 0x0000A89C
		public static void RegularPolygonHollowFillLinear(Vector3 pos, int sideCount, float radius, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x06000385 RID: 901 RVA: 0x0000C6E4 File Offset: 0x0000A8E4
		public static void RegularPolygonHollowFillLinear(Vector3 pos, int sideCount, float radius, float thickness, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, 0f, 0f, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x06000386 RID: 902 RVA: 0x0000C728 File Offset: 0x0000A928
		public static void RegularPolygonHollowFillLinear(Vector3 pos, int sideCount, float radius, float thickness, float angle, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, 0f, angle, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x06000387 RID: 903 RVA: 0x0000C76C File Offset: 0x0000A96C
		public static void RegularPolygonHollowFillLinear(Vector3 pos, int sideCount, float radius, float thickness, float angle, float roundness, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, roundness, angle, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0000C7AC File Offset: 0x0000A9AC
		public static void RegularPolygonHollowFillLinear(Vector3 pos, Vector3 normal, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x06000389 RID: 905 RVA: 0x0000C814 File Offset: 0x0000AA14
		public static void RegularPolygonHollowFillLinear(Vector3 pos, Vector3 normal, float radius, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0000C87C File Offset: 0x0000AA7C
		public static void RegularPolygonHollowFillLinear(Vector3 pos, Vector3 normal, float radius, float thickness, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, 0f, 0f, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x0600038B RID: 907 RVA: 0x0000C8E0 File Offset: 0x0000AAE0
		public static void RegularPolygonHollowFillLinear(Vector3 pos, Vector3 normal, float radius, float thickness, float angle, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, 0f, angle, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x0600038C RID: 908 RVA: 0x0000C940 File Offset: 0x0000AB40
		public static void RegularPolygonHollowFillLinear(Vector3 pos, Vector3 normal, float radius, float thickness, float angle, float roundness, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, roundness, angle, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x0600038D RID: 909 RVA: 0x0000C99C File Offset: 0x0000AB9C
		public static void RegularPolygonHollowFillLinear(Vector3 pos, Vector3 normal, int sideCount, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x0600038E RID: 910 RVA: 0x0000CA04 File Offset: 0x0000AC04
		public static void RegularPolygonHollowFillLinear(Vector3 pos, Vector3 normal, int sideCount, float radius, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x0600038F RID: 911 RVA: 0x0000CA68 File Offset: 0x0000AC68
		public static void RegularPolygonHollowFillLinear(Vector3 pos, Vector3 normal, int sideCount, float radius, float thickness, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, 0f, 0f, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x06000390 RID: 912 RVA: 0x0000CAC8 File Offset: 0x0000ACC8
		public static void RegularPolygonHollowFillLinear(Vector3 pos, Vector3 normal, int sideCount, float radius, float thickness, float angle, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, 0f, angle, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x06000391 RID: 913 RVA: 0x0000CB24 File Offset: 0x0000AD24
		public static void RegularPolygonHollowFillLinear(Vector3 pos, Vector3 normal, int sideCount, float radius, float thickness, float angle, float roundness, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, roundness, angle, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x06000392 RID: 914 RVA: 0x0000CB80 File Offset: 0x0000AD80
		public static void RegularPolygonHollowFillLinear(Vector3 pos, Quaternion rot, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x06000393 RID: 915 RVA: 0x0000CBE4 File Offset: 0x0000ADE4
		public static void RegularPolygonHollowFillLinear(Vector3 pos, Quaternion rot, float radius, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x06000394 RID: 916 RVA: 0x0000CC44 File Offset: 0x0000AE44
		public static void RegularPolygonHollowFillLinear(Vector3 pos, Quaternion rot, float radius, float thickness, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, 0f, 0f, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x06000395 RID: 917 RVA: 0x0000CCA4 File Offset: 0x0000AEA4
		public static void RegularPolygonHollowFillLinear(Vector3 pos, Quaternion rot, float radius, float thickness, float angle, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, 0f, angle, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x06000396 RID: 918 RVA: 0x0000CD00 File Offset: 0x0000AF00
		public static void RegularPolygonHollowFillLinear(Vector3 pos, Quaternion rot, float radius, float thickness, float angle, float roundness, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, roundness, angle, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x06000397 RID: 919 RVA: 0x0000CD58 File Offset: 0x0000AF58
		public static void RegularPolygonHollowFillLinear(Vector3 pos, Quaternion rot, int sideCount, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x06000398 RID: 920 RVA: 0x0000CDB8 File Offset: 0x0000AFB8
		public static void RegularPolygonHollowFillLinear(Vector3 pos, Quaternion rot, int sideCount, float radius, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x06000399 RID: 921 RVA: 0x0000CE18 File Offset: 0x0000B018
		public static void RegularPolygonHollowFillLinear(Vector3 pos, Quaternion rot, int sideCount, float radius, float thickness, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, 0f, 0f, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x0600039A RID: 922 RVA: 0x0000CE74 File Offset: 0x0000B074
		public static void RegularPolygonHollowFillLinear(Vector3 pos, Quaternion rot, int sideCount, float radius, float thickness, float angle, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, 0f, angle, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x0600039B RID: 923 RVA: 0x0000CECC File Offset: 0x0000B0CC
		public static void RegularPolygonHollowFillLinear(Vector3 pos, Quaternion rot, int sideCount, float radius, float thickness, float angle, float roundness, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, roundness, angle, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x0600039C RID: 924 RVA: 0x0000CF20 File Offset: 0x0000B120
		public static void RegularPolygonHollowFillLinear(Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
		}

		// Token: 0x0600039D RID: 925 RVA: 0x0000CF5C File Offset: 0x0000B15C
		public static void RegularPolygonHollowFillLinear(float radius, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0000CF98 File Offset: 0x0000B198
		public static void RegularPolygonHollowFillLinear(float radius, float thickness, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, 0f, 0f, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
		}

		// Token: 0x0600039F RID: 927 RVA: 0x0000CFD0 File Offset: 0x0000B1D0
		public static void RegularPolygonHollowFillLinear(float radius, float thickness, float angle, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, 0f, angle, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x0000D004 File Offset: 0x0000B204
		public static void RegularPolygonHollowFillLinear(float radius, float thickness, float angle, float roundness, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, roundness, angle, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x0000D034 File Offset: 0x0000B234
		public static void RegularPolygonHollowFillLinear(int sideCount, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.RegularPolygon_Internal(sideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x0000D070 File Offset: 0x0000B270
		public static void RegularPolygonHollowFillLinear(int sideCount, float radius, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x0000D0A8 File Offset: 0x0000B2A8
		public static void RegularPolygonHollowFillLinear(int sideCount, float radius, float thickness, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, 0f, 0f, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0000D0DC File Offset: 0x0000B2DC
		public static void RegularPolygonHollowFillLinear(int sideCount, float radius, float thickness, float angle, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, 0f, angle, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x0000D10C File Offset: 0x0000B30C
		public static void RegularPolygonHollowFillLinear(int sideCount, float radius, float thickness, float angle, float roundness, Vector3 fillStart, Vector3 fillEnd, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, roundness, angle, ShapeFill.CreateLinear(fillStart, fillEnd, fillColorStart, fillColorEnd, fillSpace));
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0000D13C File Offset: 0x0000B33C
		public static void RegularPolygonFillRadial(Vector3 pos, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x0000D18C File Offset: 0x0000B38C
		public static void RegularPolygonFillRadial(Vector3 pos, float radius, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0000D1D8 File Offset: 0x0000B3D8
		public static void RegularPolygonFillRadial(Vector3 pos, float radius, float angle, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, angle, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0000D220 File Offset: 0x0000B420
		public static void RegularPolygonFillRadial(Vector3 pos, float radius, float angle, float roundness, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, roundness, angle, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0000D264 File Offset: 0x0000B464
		public static void RegularPolygonFillRadial(Vector3 pos, int sideCount, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0000D2B0 File Offset: 0x0000B4B0
		public static void RegularPolygonFillRadial(Vector3 pos, int sideCount, float radius, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0000D2F8 File Offset: 0x0000B4F8
		public static void RegularPolygonFillRadial(Vector3 pos, int sideCount, float radius, float angle, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, angle, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0000D33C File Offset: 0x0000B53C
		public static void RegularPolygonFillRadial(Vector3 pos, int sideCount, float radius, float angle, float roundness, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, roundness, angle, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003AE RID: 942 RVA: 0x0000D380 File Offset: 0x0000B580
		public static void RegularPolygonFillRadial(Vector3 pos, Vector3 normal, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003AF RID: 943 RVA: 0x0000D3E8 File Offset: 0x0000B5E8
		public static void RegularPolygonFillRadial(Vector3 pos, Vector3 normal, float radius, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x0000D450 File Offset: 0x0000B650
		public static void RegularPolygonFillRadial(Vector3 pos, Vector3 normal, float radius, float angle, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, angle, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x0000D4B4 File Offset: 0x0000B6B4
		public static void RegularPolygonFillRadial(Vector3 pos, Vector3 normal, float radius, float angle, float roundness, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, roundness, angle, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x0000D514 File Offset: 0x0000B714
		public static void RegularPolygonFillRadial(Vector3 pos, Vector3 normal, int sideCount, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x0000D57C File Offset: 0x0000B77C
		public static void RegularPolygonFillRadial(Vector3 pos, Vector3 normal, int sideCount, float radius, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x0000D5E0 File Offset: 0x0000B7E0
		public static void RegularPolygonFillRadial(Vector3 pos, Vector3 normal, int sideCount, float radius, float angle, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, angle, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x0000D640 File Offset: 0x0000B840
		public static void RegularPolygonFillRadial(Vector3 pos, Vector3 normal, int sideCount, float radius, float angle, float roundness, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, roundness, angle, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x0000D69C File Offset: 0x0000B89C
		public static void RegularPolygonFillRadial(Vector3 pos, Quaternion rot, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x0000D700 File Offset: 0x0000B900
		public static void RegularPolygonFillRadial(Vector3 pos, Quaternion rot, float radius, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x0000D760 File Offset: 0x0000B960
		public static void RegularPolygonFillRadial(Vector3 pos, Quaternion rot, float radius, float angle, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, angle, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0000D7C0 File Offset: 0x0000B9C0
		public static void RegularPolygonFillRadial(Vector3 pos, Quaternion rot, float radius, float angle, float roundness, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, roundness, angle, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003BA RID: 954 RVA: 0x0000D81C File Offset: 0x0000BA1C
		public static void RegularPolygonFillRadial(Vector3 pos, Quaternion rot, int sideCount, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003BB RID: 955 RVA: 0x0000D87C File Offset: 0x0000BA7C
		public static void RegularPolygonFillRadial(Vector3 pos, Quaternion rot, int sideCount, float radius, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003BC RID: 956 RVA: 0x0000D8DC File Offset: 0x0000BADC
		public static void RegularPolygonFillRadial(Vector3 pos, Quaternion rot, int sideCount, float radius, float angle, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, angle, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0000D938 File Offset: 0x0000BB38
		public static void RegularPolygonFillRadial(Vector3 pos, Quaternion rot, int sideCount, float radius, float angle, float roundness, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, roundness, angle, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0000D990 File Offset: 0x0000BB90
		public static void RegularPolygonFillRadial(Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0000D9CC File Offset: 0x0000BBCC
		public static void RegularPolygonFillRadial(float radius, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x0000DA08 File Offset: 0x0000BC08
		public static void RegularPolygonFillRadial(float radius, float angle, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, angle, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x0000DA40 File Offset: 0x0000BC40
		public static void RegularPolygonFillRadial(float radius, float angle, float roundness, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, roundness, angle, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0000DA74 File Offset: 0x0000BC74
		public static void RegularPolygonFillRadial(int sideCount, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.RegularPolygon_Internal(sideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0000DAB0 File Offset: 0x0000BCB0
		public static void RegularPolygonFillRadial(int sideCount, float radius, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, 0f, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0000DAE8 File Offset: 0x0000BCE8
		public static void RegularPolygonFillRadial(int sideCount, float radius, float angle, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, 0f, angle, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x0000DB1C File Offset: 0x0000BD1C
		public static void RegularPolygonFillRadial(int sideCount, float radius, float angle, float roundness, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, false, roundness, angle, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x0000DB4C File Offset: 0x0000BD4C
		public static void RegularPolygonHollowFillRadial(Vector3 pos, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x0000DB9C File Offset: 0x0000BD9C
		public static void RegularPolygonHollowFillRadial(Vector3 pos, float radius, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0000DBE8 File Offset: 0x0000BDE8
		public static void RegularPolygonHollowFillRadial(Vector3 pos, float radius, float thickness, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, 0f, 0f, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0000DC30 File Offset: 0x0000BE30
		public static void RegularPolygonHollowFillRadial(Vector3 pos, float radius, float thickness, float angle, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, 0f, angle, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0000DC74 File Offset: 0x0000BE74
		public static void RegularPolygonHollowFillRadial(Vector3 pos, float radius, float thickness, float angle, float roundness, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, roundness, angle, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003CB RID: 971 RVA: 0x0000DCB8 File Offset: 0x0000BEB8
		public static void RegularPolygonHollowFillRadial(Vector3 pos, int sideCount, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003CC RID: 972 RVA: 0x0000DD04 File Offset: 0x0000BF04
		public static void RegularPolygonHollowFillRadial(Vector3 pos, int sideCount, float radius, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0000DD4C File Offset: 0x0000BF4C
		public static void RegularPolygonHollowFillRadial(Vector3 pos, int sideCount, float radius, float thickness, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, 0f, 0f, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003CE RID: 974 RVA: 0x0000DD90 File Offset: 0x0000BF90
		public static void RegularPolygonHollowFillRadial(Vector3 pos, int sideCount, float radius, float thickness, float angle, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, 0f, angle, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003CF RID: 975 RVA: 0x0000DDD4 File Offset: 0x0000BFD4
		public static void RegularPolygonHollowFillRadial(Vector3 pos, int sideCount, float radius, float thickness, float angle, float roundness, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, roundness, angle, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x0000DE14 File Offset: 0x0000C014
		public static void RegularPolygonHollowFillRadial(Vector3 pos, Vector3 normal, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0000DE7C File Offset: 0x0000C07C
		public static void RegularPolygonHollowFillRadial(Vector3 pos, Vector3 normal, float radius, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x0000DEE4 File Offset: 0x0000C0E4
		public static void RegularPolygonHollowFillRadial(Vector3 pos, Vector3 normal, float radius, float thickness, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, 0f, 0f, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x0000DF48 File Offset: 0x0000C148
		public static void RegularPolygonHollowFillRadial(Vector3 pos, Vector3 normal, float radius, float thickness, float angle, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, 0f, angle, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x0000DFA8 File Offset: 0x0000C1A8
		public static void RegularPolygonHollowFillRadial(Vector3 pos, Vector3 normal, float radius, float thickness, float angle, float roundness, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, roundness, angle, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x0000E004 File Offset: 0x0000C204
		public static void RegularPolygonHollowFillRadial(Vector3 pos, Vector3 normal, int sideCount, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0000E06C File Offset: 0x0000C26C
		public static void RegularPolygonHollowFillRadial(Vector3 pos, Vector3 normal, int sideCount, float radius, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x0000E0D0 File Offset: 0x0000C2D0
		public static void RegularPolygonHollowFillRadial(Vector3 pos, Vector3 normal, int sideCount, float radius, float thickness, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, 0f, 0f, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x0000E130 File Offset: 0x0000C330
		public static void RegularPolygonHollowFillRadial(Vector3 pos, Vector3 normal, int sideCount, float radius, float thickness, float angle, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, 0f, angle, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0000E18C File Offset: 0x0000C38C
		public static void RegularPolygonHollowFillRadial(Vector3 pos, Vector3 normal, int sideCount, float radius, float thickness, float angle, float roundness, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, roundness, angle, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003DA RID: 986 RVA: 0x0000E1E8 File Offset: 0x0000C3E8
		public static void RegularPolygonHollowFillRadial(Vector3 pos, Quaternion rot, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003DB RID: 987 RVA: 0x0000E24C File Offset: 0x0000C44C
		public static void RegularPolygonHollowFillRadial(Vector3 pos, Quaternion rot, float radius, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003DC RID: 988 RVA: 0x0000E2AC File Offset: 0x0000C4AC
		public static void RegularPolygonHollowFillRadial(Vector3 pos, Quaternion rot, float radius, float thickness, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, 0f, 0f, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003DD RID: 989 RVA: 0x0000E30C File Offset: 0x0000C50C
		public static void RegularPolygonHollowFillRadial(Vector3 pos, Quaternion rot, float radius, float thickness, float angle, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, 0f, angle, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003DE RID: 990 RVA: 0x0000E368 File Offset: 0x0000C568
		public static void RegularPolygonHollowFillRadial(Vector3 pos, Quaternion rot, float radius, float thickness, float angle, float roundness, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, roundness, angle, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003DF RID: 991 RVA: 0x0000E3C0 File Offset: 0x0000C5C0
		public static void RegularPolygonHollowFillRadial(Vector3 pos, Quaternion rot, int sideCount, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x0000E420 File Offset: 0x0000C620
		public static void RegularPolygonHollowFillRadial(Vector3 pos, Quaternion rot, int sideCount, float radius, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0000E480 File Offset: 0x0000C680
		public static void RegularPolygonHollowFillRadial(Vector3 pos, Quaternion rot, int sideCount, float radius, float thickness, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, 0f, 0f, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x0000E4DC File Offset: 0x0000C6DC
		public static void RegularPolygonHollowFillRadial(Vector3 pos, Quaternion rot, int sideCount, float radius, float thickness, float angle, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, 0f, angle, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0000E534 File Offset: 0x0000C734
		public static void RegularPolygonHollowFillRadial(Vector3 pos, Quaternion rot, int sideCount, float radius, float thickness, float angle, float roundness, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, roundness, angle, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
			Draw.PopMatrix();
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0000E588 File Offset: 0x0000C788
		public static void RegularPolygonHollowFillRadial(Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0000E5C4 File Offset: 0x0000C7C4
		public static void RegularPolygonHollowFillRadial(float radius, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0000E600 File Offset: 0x0000C800
		public static void RegularPolygonHollowFillRadial(float radius, float thickness, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, 0f, 0f, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0000E638 File Offset: 0x0000C838
		public static void RegularPolygonHollowFillRadial(float radius, float thickness, float angle, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, 0f, angle, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x0000E66C File Offset: 0x0000C86C
		public static void RegularPolygonHollowFillRadial(float radius, float thickness, float angle, float roundness, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.RegularPolygon_Internal(Draw.RegularPolygonSideCount, radius, thickness, Draw.Color, true, roundness, angle, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0000E69C File Offset: 0x0000C89C
		public static void RegularPolygonHollowFillRadial(int sideCount, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.RegularPolygon_Internal(sideCount, Draw.RegularPolygonRadius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0000E6D8 File Offset: 0x0000C8D8
		public static void RegularPolygonHollowFillRadial(int sideCount, float radius, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, Draw.RegularPolygonThickness, Draw.Color, true, 0f, 0f, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x0000E710 File Offset: 0x0000C910
		public static void RegularPolygonHollowFillRadial(int sideCount, float radius, float thickness, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, 0f, 0f, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0000E744 File Offset: 0x0000C944
		public static void RegularPolygonHollowFillRadial(int sideCount, float radius, float thickness, float angle, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, 0f, angle, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x0000E774 File Offset: 0x0000C974
		public static void RegularPolygonHollowFillRadial(int sideCount, float radius, float thickness, float angle, float roundness, Vector3 fillOrigin, float fillRadius, Color fillColorStart, Color fillColorEnd, FillSpace fillSpace = FillSpace.Local)
		{
			Draw.RegularPolygon_Internal(sideCount, radius, thickness, Draw.Color, true, roundness, angle, ShapeFill.CreateRadial(fillOrigin, fillRadius, fillColorStart, fillColorEnd, fillSpace));
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x0000E7A1 File Offset: 0x0000C9A1
		public static void Disc(Vector3 pos)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Disc_Internal(Draw.DiscRadius, Draw.Color, Draw.Color, Draw.Color, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x0000E7D1 File Offset: 0x0000C9D1
		public static void Disc(Vector3 pos, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Disc_Internal(Draw.DiscRadius, color, color, color, color);
			Draw.PopMatrix();
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0000E7F1 File Offset: 0x0000C9F1
		public static void Disc(Vector3 pos, float radius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Disc_Internal(radius, Draw.Color, Draw.Color, Draw.Color, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x0000E81D File Offset: 0x0000CA1D
		public static void Disc(Vector3 pos, float radius, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Disc_Internal(radius, color, color, color, color);
			Draw.PopMatrix();
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x0000E83C File Offset: 0x0000CA3C
		public static void Disc(Vector3 pos, Vector3 normal)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Disc_Internal(Draw.DiscRadius, Draw.Color, Draw.Color, Draw.Color, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x0000E891 File Offset: 0x0000CA91
		public static void Disc(Vector3 pos, Vector3 normal, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Disc_Internal(Draw.DiscRadius, color, color, color, color);
			Draw.PopMatrix();
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x0000E8CC File Offset: 0x0000CACC
		public static void Disc(Vector3 pos, Vector3 normal, float radius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Disc_Internal(radius, Draw.Color, Draw.Color, Draw.Color, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0000E91D File Offset: 0x0000CB1D
		public static void Disc(Vector3 pos, Vector3 normal, float radius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Disc_Internal(radius, color, color, color, color);
			Draw.PopMatrix();
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x0000E954 File Offset: 0x0000CB54
		public static void Disc(Vector3 pos, Quaternion rot)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Disc_Internal(Draw.DiscRadius, Draw.Color, Draw.Color, Draw.Color, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x0000E9A4 File Offset: 0x0000CBA4
		public static void Disc(Vector3 pos, Quaternion rot, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Disc_Internal(Draw.DiscRadius, color, color, color, color);
			Draw.PopMatrix();
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0000E9DC File Offset: 0x0000CBDC
		public static void Disc(Vector3 pos, Quaternion rot, float radius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Disc_Internal(radius, Draw.Color, Draw.Color, Draw.Color, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0000EA28 File Offset: 0x0000CC28
		public static void Disc(Vector3 pos, Quaternion rot, float radius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Disc_Internal(radius, color, color, color, color);
			Draw.PopMatrix();
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x0000EA59 File Offset: 0x0000CC59
		public static void Disc()
		{
			Draw.Disc_Internal(Draw.DiscRadius, Draw.Color, Draw.Color, Draw.Color, Draw.Color);
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x0000EA79 File Offset: 0x0000CC79
		public static void Disc(Color color)
		{
			Draw.Disc_Internal(Draw.DiscRadius, color, color, color, color);
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0000EA89 File Offset: 0x0000CC89
		public static void Disc(float radius)
		{
			Draw.Disc_Internal(radius, Draw.Color, Draw.Color, Draw.Color, Draw.Color);
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x0000EAA5 File Offset: 0x0000CCA5
		public static void Disc(float radius, Color color)
		{
			Draw.Disc_Internal(radius, color, color, color, color);
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x0000EAB1 File Offset: 0x0000CCB1
		public static void DiscGradientRadial(Vector3 pos, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Disc_Internal(Draw.DiscRadius, colorInner, colorOuter, colorInner, colorOuter);
			Draw.PopMatrix();
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x0000EAD1 File Offset: 0x0000CCD1
		public static void DiscGradientRadial(Vector3 pos, float radius, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Disc_Internal(radius, colorInner, colorOuter, colorInner, colorOuter);
			Draw.PopMatrix();
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0000EAED File Offset: 0x0000CCED
		public static void DiscGradientRadial(Vector3 pos, Vector3 normal, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Disc_Internal(Draw.DiscRadius, colorInner, colorOuter, colorInner, colorOuter);
			Draw.PopMatrix();
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x0000EB27 File Offset: 0x0000CD27
		public static void DiscGradientRadial(Vector3 pos, Vector3 normal, float radius, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Disc_Internal(radius, colorInner, colorOuter, colorInner, colorOuter);
			Draw.PopMatrix();
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x0000EB5F File Offset: 0x0000CD5F
		public static void DiscGradientRadial(Vector3 pos, Quaternion rot, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Disc_Internal(Draw.DiscRadius, colorInner, colorOuter, colorInner, colorOuter);
			Draw.PopMatrix();
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x0000EB94 File Offset: 0x0000CD94
		public static void DiscGradientRadial(Vector3 pos, Quaternion rot, float radius, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Disc_Internal(radius, colorInner, colorOuter, colorInner, colorOuter);
			Draw.PopMatrix();
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x0000EBC7 File Offset: 0x0000CDC7
		public static void DiscGradientRadial(Color colorInner, Color colorOuter)
		{
			Draw.Disc_Internal(Draw.DiscRadius, colorInner, colorOuter, colorInner, colorOuter);
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x0000EBD7 File Offset: 0x0000CDD7
		public static void DiscGradientRadial(float radius, Color colorInner, Color colorOuter)
		{
			Draw.Disc_Internal(radius, colorInner, colorOuter, colorInner, colorOuter);
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x0000EBE3 File Offset: 0x0000CDE3
		public static void DiscGradientAngular(Vector3 pos, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Disc_Internal(Draw.DiscRadius, colorStart, colorStart, colorEnd, colorEnd);
			Draw.PopMatrix();
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x0000EC03 File Offset: 0x0000CE03
		public static void DiscGradientAngular(Vector3 pos, float radius, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Disc_Internal(radius, colorStart, colorStart, colorEnd, colorEnd);
			Draw.PopMatrix();
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x0000EC1F File Offset: 0x0000CE1F
		public static void DiscGradientAngular(Vector3 pos, Vector3 normal, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Disc_Internal(Draw.DiscRadius, colorStart, colorStart, colorEnd, colorEnd);
			Draw.PopMatrix();
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x0000EC59 File Offset: 0x0000CE59
		public static void DiscGradientAngular(Vector3 pos, Vector3 normal, float radius, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Disc_Internal(radius, colorStart, colorStart, colorEnd, colorEnd);
			Draw.PopMatrix();
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x0000EC91 File Offset: 0x0000CE91
		public static void DiscGradientAngular(Vector3 pos, Quaternion rot, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Disc_Internal(Draw.DiscRadius, colorStart, colorStart, colorEnd, colorEnd);
			Draw.PopMatrix();
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x0000ECC6 File Offset: 0x0000CEC6
		public static void DiscGradientAngular(Vector3 pos, Quaternion rot, float radius, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Disc_Internal(radius, colorStart, colorStart, colorEnd, colorEnd);
			Draw.PopMatrix();
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x0000ECF9 File Offset: 0x0000CEF9
		public static void DiscGradientAngular(Color colorStart, Color colorEnd)
		{
			Draw.Disc_Internal(Draw.DiscRadius, colorStart, colorStart, colorEnd, colorEnd);
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x0000ED09 File Offset: 0x0000CF09
		public static void DiscGradientAngular(float radius, Color colorStart, Color colorEnd)
		{
			Draw.Disc_Internal(radius, colorStart, colorStart, colorEnd, colorEnd);
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x0000ED15 File Offset: 0x0000CF15
		public static void DiscGradientBilinear(Vector3 pos, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Disc_Internal(Draw.DiscRadius, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd);
			Draw.PopMatrix();
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x0000ED36 File Offset: 0x0000CF36
		public static void DiscGradientBilinear(Vector3 pos, float radius, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Disc_Internal(radius, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd);
			Draw.PopMatrix();
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0000ED54 File Offset: 0x0000CF54
		public static void DiscGradientBilinear(Vector3 pos, Vector3 normal, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Disc_Internal(Draw.DiscRadius, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd);
			Draw.PopMatrix();
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x0000ED90 File Offset: 0x0000CF90
		public static void DiscGradientBilinear(Vector3 pos, Vector3 normal, float radius, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Disc_Internal(radius, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd);
			Draw.PopMatrix();
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x0000EDC9 File Offset: 0x0000CFC9
		public static void DiscGradientBilinear(Vector3 pos, Quaternion rot, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Disc_Internal(Draw.DiscRadius, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd);
			Draw.PopMatrix();
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x0000EE00 File Offset: 0x0000D000
		public static void DiscGradientBilinear(Vector3 pos, Quaternion rot, float radius, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Disc_Internal(radius, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd);
			Draw.PopMatrix();
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x0000EE34 File Offset: 0x0000D034
		public static void DiscGradientBilinear(Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.Disc_Internal(Draw.DiscRadius, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd);
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x0000EE44 File Offset: 0x0000D044
		public static void DiscGradientBilinear(float radius, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.Disc_Internal(radius, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd);
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x0000EE51 File Offset: 0x0000D051
		public static void Ring(Vector3 pos)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x0000EE87 File Offset: 0x0000D087
		public static void Ring(Vector3 pos, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, color, color, color, color, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x0000EEAD File Offset: 0x0000D0AD
		public static void Ring(Vector3 pos, float radius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(radius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x0000EEDF File Offset: 0x0000D0DF
		public static void Ring(Vector3 pos, float radius, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(radius, Draw.RingThickness, color, color, color, color, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x0000EF01 File Offset: 0x0000D101
		public static void Ring(Vector3 pos, float radius, float thickness)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(radius, thickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x0000EF2F File Offset: 0x0000D12F
		public static void Ring(Vector3 pos, float radius, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(radius, thickness, color, color, color, color, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x0000EF50 File Offset: 0x0000D150
		public static void Ring(Vector3 pos, Vector3 normal)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x0000EFAB File Offset: 0x0000D1AB
		public static void Ring(Vector3 pos, Vector3 normal, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, color, color, color, color, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x0000EFEC File Offset: 0x0000D1EC
		public static void Ring(Vector3 pos, Vector3 normal, float radius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(radius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x0000F043 File Offset: 0x0000D243
		public static void Ring(Vector3 pos, Vector3 normal, float radius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(radius, Draw.RingThickness, color, color, color, color, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x0000F080 File Offset: 0x0000D280
		public static void Ring(Vector3 pos, Vector3 normal, float radius, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(radius, thickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x0000F0D3 File Offset: 0x0000D2D3
		public static void Ring(Vector3 pos, Vector3 normal, float radius, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(radius, thickness, color, color, color, color, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x0000F110 File Offset: 0x0000D310
		public static void Ring(Vector3 pos, Quaternion rot)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x0000F166 File Offset: 0x0000D366
		public static void Ring(Vector3 pos, Quaternion rot, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, color, color, color, color, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x0000F1A4 File Offset: 0x0000D3A4
		public static void Ring(Vector3 pos, Quaternion rot, float radius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(radius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x0000F1F6 File Offset: 0x0000D3F6
		public static void Ring(Vector3 pos, Quaternion rot, float radius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(radius, Draw.RingThickness, color, color, color, color, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x0000F230 File Offset: 0x0000D430
		public static void Ring(Vector3 pos, Quaternion rot, float radius, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(radius, thickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x0000F27E File Offset: 0x0000D47E
		public static void Ring(Vector3 pos, Quaternion rot, float radius, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(radius, thickness, color, color, color, color, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x0000F2B5 File Offset: 0x0000D4B5
		public static void Ring()
		{
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, null);
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x0000F2DB File Offset: 0x0000D4DB
		public static void Ring(Color color)
		{
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, color, color, color, color, null);
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0000F2F1 File Offset: 0x0000D4F1
		public static void Ring(float radius)
		{
			Draw.Ring_Internal(radius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, null);
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x0000F313 File Offset: 0x0000D513
		public static void Ring(float radius, Color color)
		{
			Draw.Ring_Internal(radius, Draw.RingThickness, color, color, color, color, null);
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x0000F325 File Offset: 0x0000D525
		public static void Ring(float radius, float thickness)
		{
			Draw.Ring_Internal(radius, thickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, null);
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x0000F343 File Offset: 0x0000D543
		public static void Ring(float radius, float thickness, Color color)
		{
			Draw.Ring_Internal(radius, thickness, color, color, color, color, null);
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x0000F351 File Offset: 0x0000D551
		public static void RingDashed(Vector3 pos)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x0000F38B File Offset: 0x0000D58B
		public static void RingDashed(Vector3 pos, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, color, color, color, color, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x0000F3B5 File Offset: 0x0000D5B5
		public static void RingDashed(Vector3 pos, float radius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(radius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x0000F3EB File Offset: 0x0000D5EB
		public static void RingDashed(Vector3 pos, float radius, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(radius, Draw.RingThickness, color, color, color, color, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x0000F411 File Offset: 0x0000D611
		public static void RingDashed(Vector3 pos, float radius, float thickness)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(radius, thickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x0000F443 File Offset: 0x0000D643
		public static void RingDashed(Vector3 pos, float radius, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(radius, thickness, color, color, color, color, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x0000F465 File Offset: 0x0000D665
		public static void RingDashed(Vector3 pos, DashStyle dashStyle)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x0000F49B File Offset: 0x0000D69B
		public static void RingDashed(Vector3 pos, DashStyle dashStyle, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, color, color, color, color, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x0000F4C1 File Offset: 0x0000D6C1
		public static void RingDashed(Vector3 pos, DashStyle dashStyle, float radius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(radius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x0000F4F3 File Offset: 0x0000D6F3
		public static void RingDashed(Vector3 pos, DashStyle dashStyle, float radius, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(radius, Draw.RingThickness, color, color, color, color, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x0000F515 File Offset: 0x0000D715
		public static void RingDashed(Vector3 pos, DashStyle dashStyle, float radius, float thickness)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(radius, thickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x0000F543 File Offset: 0x0000D743
		public static void RingDashed(Vector3 pos, DashStyle dashStyle, float radius, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(radius, thickness, color, color, color, color, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x0000F568 File Offset: 0x0000D768
		public static void RingDashed(Vector3 pos, Vector3 normal)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x0000F5C8 File Offset: 0x0000D7C8
		public static void RingDashed(Vector3 pos, Vector3 normal, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, color, color, color, color, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x0000F618 File Offset: 0x0000D818
		public static void RingDashed(Vector3 pos, Vector3 normal, float radius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(radius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x0000F673 File Offset: 0x0000D873
		public static void RingDashed(Vector3 pos, Vector3 normal, float radius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(radius, Draw.RingThickness, color, color, color, color, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x0000F6B4 File Offset: 0x0000D8B4
		public static void RingDashed(Vector3 pos, Vector3 normal, float radius, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(radius, thickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x0000F70B File Offset: 0x0000D90B
		public static void RingDashed(Vector3 pos, Vector3 normal, float radius, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(radius, thickness, color, color, color, color, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x0000F74C File Offset: 0x0000D94C
		public static void RingDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x0000F7A7 File Offset: 0x0000D9A7
		public static void RingDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, color, color, color, color, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x0000F7E8 File Offset: 0x0000D9E8
		public static void RingDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(radius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x0000F83F File Offset: 0x0000DA3F
		public static void RingDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(radius, Draw.RingThickness, color, color, color, color, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x0000F880 File Offset: 0x0000DA80
		public static void RingDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(radius, thickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x0000F8D4 File Offset: 0x0000DAD4
		public static void RingDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(radius, thickness, color, color, color, color, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x0000F914 File Offset: 0x0000DB14
		public static void RingDashed(Vector3 pos, Quaternion rot)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x0000F96E File Offset: 0x0000DB6E
		public static void RingDashed(Vector3 pos, Quaternion rot, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, color, color, color, color, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x0000F9B0 File Offset: 0x0000DBB0
		public static void RingDashed(Vector3 pos, Quaternion rot, float radius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(radius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x0000FA06 File Offset: 0x0000DC06
		public static void RingDashed(Vector3 pos, Quaternion rot, float radius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(radius, Draw.RingThickness, color, color, color, color, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x0000FA44 File Offset: 0x0000DC44
		public static void RingDashed(Vector3 pos, Quaternion rot, float radius, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(radius, thickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x0000FA96 File Offset: 0x0000DC96
		public static void RingDashed(Vector3 pos, Quaternion rot, float radius, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(radius, thickness, color, color, color, color, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x0000FAD4 File Offset: 0x0000DCD4
		public static void RingDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x0000FB2A File Offset: 0x0000DD2A
		public static void RingDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, color, color, color, color, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x0000FB68 File Offset: 0x0000DD68
		public static void RingDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(radius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x0000FBBA File Offset: 0x0000DDBA
		public static void RingDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(radius, Draw.RingThickness, color, color, color, color, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x0000FBF8 File Offset: 0x0000DDF8
		public static void RingDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(radius, thickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x0000FC47 File Offset: 0x0000DE47
		public static void RingDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(radius, thickness, color, color, color, color, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x0000FC7F File Offset: 0x0000DE7F
		public static void RingDashed()
		{
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, Draw.RingDashStyle);
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x0000FCA9 File Offset: 0x0000DEA9
		public static void RingDashed(Color color)
		{
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, color, color, color, color, Draw.RingDashStyle);
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x0000FCC3 File Offset: 0x0000DEC3
		public static void RingDashed(float radius)
		{
			Draw.Ring_Internal(radius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, Draw.RingDashStyle);
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x0000FCE9 File Offset: 0x0000DEE9
		public static void RingDashed(float radius, Color color)
		{
			Draw.Ring_Internal(radius, Draw.RingThickness, color, color, color, color, Draw.RingDashStyle);
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x0000FCFF File Offset: 0x0000DEFF
		public static void RingDashed(float radius, float thickness)
		{
			Draw.Ring_Internal(radius, thickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, Draw.RingDashStyle);
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x0000FD21 File Offset: 0x0000DF21
		public static void RingDashed(float radius, float thickness, Color color)
		{
			Draw.Ring_Internal(radius, thickness, color, color, color, color, Draw.RingDashStyle);
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x0000FD33 File Offset: 0x0000DF33
		public static void RingDashed(DashStyle dashStyle)
		{
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, dashStyle);
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x0000FD59 File Offset: 0x0000DF59
		public static void RingDashed(DashStyle dashStyle, Color color)
		{
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, color, color, color, color, dashStyle);
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x0000FD6F File Offset: 0x0000DF6F
		public static void RingDashed(DashStyle dashStyle, float radius)
		{
			Draw.Ring_Internal(radius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, dashStyle);
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x0000FD91 File Offset: 0x0000DF91
		public static void RingDashed(DashStyle dashStyle, float radius, Color color)
		{
			Draw.Ring_Internal(radius, Draw.RingThickness, color, color, color, color, dashStyle);
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x0000FDA3 File Offset: 0x0000DFA3
		public static void RingDashed(DashStyle dashStyle, float radius, float thickness)
		{
			Draw.Ring_Internal(radius, thickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, dashStyle);
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x0000FDC1 File Offset: 0x0000DFC1
		public static void RingDashed(DashStyle dashStyle, float radius, float thickness, Color color)
		{
			Draw.Ring_Internal(radius, thickness, color, color, color, color, dashStyle);
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x0000FDCF File Offset: 0x0000DFCF
		public static void RingGradientRadial(Vector3 pos, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x0000FDF5 File Offset: 0x0000DFF5
		public static void RingGradientRadial(Vector3 pos, float radius, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(radius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x0000FE17 File Offset: 0x0000E017
		public static void RingGradientRadial(Vector3 pos, float radius, float thickness, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(radius, thickness, colorInner, colorOuter, colorInner, colorOuter, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x0000FE37 File Offset: 0x0000E037
		public static void RingGradientRadial(Vector3 pos, Vector3 normal, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x0000FE77 File Offset: 0x0000E077
		public static void RingGradientRadial(Vector3 pos, Vector3 normal, float radius, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(radius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x0000FEB5 File Offset: 0x0000E0B5
		public static void RingGradientRadial(Vector3 pos, Vector3 normal, float radius, float thickness, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(radius, thickness, colorInner, colorOuter, colorInner, colorOuter, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x0000FEF1 File Offset: 0x0000E0F1
		public static void RingGradientRadial(Vector3 pos, Quaternion rot, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x0000FF2C File Offset: 0x0000E12C
		public static void RingGradientRadial(Vector3 pos, Quaternion rot, float radius, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(radius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x0000FF65 File Offset: 0x0000E165
		public static void RingGradientRadial(Vector3 pos, Quaternion rot, float radius, float thickness, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(radius, thickness, colorInner, colorOuter, colorInner, colorOuter, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x0000FF9C File Offset: 0x0000E19C
		public static void RingGradientRadial(Color colorInner, Color colorOuter)
		{
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, null);
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x0000FFB2 File Offset: 0x0000E1B2
		public static void RingGradientRadial(float radius, Color colorInner, Color colorOuter)
		{
			Draw.Ring_Internal(radius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, null);
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x0000FFC4 File Offset: 0x0000E1C4
		public static void RingGradientRadial(float radius, float thickness, Color colorInner, Color colorOuter)
		{
			Draw.Ring_Internal(radius, thickness, colorInner, colorOuter, colorInner, colorOuter, null);
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x0000FFD2 File Offset: 0x0000E1D2
		public static void RingGradientRadialDashed(Vector3 pos, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x0000FFFC File Offset: 0x0000E1FC
		public static void RingGradientRadialDashed(Vector3 pos, float radius, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(radius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x00010022 File Offset: 0x0000E222
		public static void RingGradientRadialDashed(Vector3 pos, float radius, float thickness, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(radius, thickness, colorInner, colorOuter, colorInner, colorOuter, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x00010046 File Offset: 0x0000E246
		public static void RingGradientRadialDashed(Vector3 pos, DashStyle dashStyle, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x0001006C File Offset: 0x0000E26C
		public static void RingGradientRadialDashed(Vector3 pos, DashStyle dashStyle, float radius, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(radius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x00010090 File Offset: 0x0000E290
		public static void RingGradientRadialDashed(Vector3 pos, DashStyle dashStyle, float radius, float thickness, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(radius, thickness, colorInner, colorOuter, colorInner, colorOuter, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x000100B4 File Offset: 0x0000E2B4
		public static void RingGradientRadialDashed(Vector3 pos, Vector3 normal, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00010104 File Offset: 0x0000E304
		public static void RingGradientRadialDashed(Vector3 pos, Vector3 normal, float radius, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(radius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x00010151 File Offset: 0x0000E351
		public static void RingGradientRadialDashed(Vector3 pos, Vector3 normal, float radius, float thickness, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(radius, thickness, colorInner, colorOuter, colorInner, colorOuter, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x00010194 File Offset: 0x0000E394
		public static void RingGradientRadialDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x000101E1 File Offset: 0x0000E3E1
		public static void RingGradientRadialDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(radius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x00010221 File Offset: 0x0000E421
		public static void RingGradientRadialDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float thickness, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(radius, thickness, colorInner, colorOuter, colorInner, colorOuter, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x0001025E File Offset: 0x0000E45E
		public static void RingGradientRadialDashed(Vector3 pos, Quaternion rot, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x0001029D File Offset: 0x0000E49D
		public static void RingGradientRadialDashed(Vector3 pos, Quaternion rot, float radius, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(radius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x000102DA File Offset: 0x0000E4DA
		public static void RingGradientRadialDashed(Vector3 pos, Quaternion rot, float radius, float thickness, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(radius, thickness, colorInner, colorOuter, colorInner, colorOuter, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x00010315 File Offset: 0x0000E515
		public static void RingGradientRadialDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x00010352 File Offset: 0x0000E552
		public static void RingGradientRadialDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(radius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x0001038D File Offset: 0x0000E58D
		public static void RingGradientRadialDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float thickness, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(radius, thickness, colorInner, colorOuter, colorInner, colorOuter, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x000103C5 File Offset: 0x0000E5C5
		public static void RingGradientRadialDashed(Color colorInner, Color colorOuter)
		{
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, Draw.RingDashStyle);
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x000103DF File Offset: 0x0000E5DF
		public static void RingGradientRadialDashed(float radius, Color colorInner, Color colorOuter)
		{
			Draw.Ring_Internal(radius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, Draw.RingDashStyle);
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x000103F5 File Offset: 0x0000E5F5
		public static void RingGradientRadialDashed(float radius, float thickness, Color colorInner, Color colorOuter)
		{
			Draw.Ring_Internal(radius, thickness, colorInner, colorOuter, colorInner, colorOuter, Draw.RingDashStyle);
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x00010407 File Offset: 0x0000E607
		public static void RingGradientRadialDashed(DashStyle dashStyle, Color colorInner, Color colorOuter)
		{
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, dashStyle);
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x0001041D File Offset: 0x0000E61D
		public static void RingGradientRadialDashed(DashStyle dashStyle, float radius, Color colorInner, Color colorOuter)
		{
			Draw.Ring_Internal(radius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, dashStyle);
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x0001042F File Offset: 0x0000E62F
		public static void RingGradientRadialDashed(DashStyle dashStyle, float radius, float thickness, Color colorInner, Color colorOuter)
		{
			Draw.Ring_Internal(radius, thickness, colorInner, colorOuter, colorInner, colorOuter, dashStyle);
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x0001043F File Offset: 0x0000E63F
		public static void RingGradientAngular(Vector3 pos, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x00010465 File Offset: 0x0000E665
		public static void RingGradientAngular(Vector3 pos, float radius, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(radius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x00010487 File Offset: 0x0000E687
		public static void RingGradientAngular(Vector3 pos, float radius, float thickness, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(radius, thickness, colorStart, colorStart, colorEnd, colorEnd, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x000104A7 File Offset: 0x0000E6A7
		public static void RingGradientAngular(Vector3 pos, Vector3 normal, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x000104E7 File Offset: 0x0000E6E7
		public static void RingGradientAngular(Vector3 pos, Vector3 normal, float radius, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(radius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x00010525 File Offset: 0x0000E725
		public static void RingGradientAngular(Vector3 pos, Vector3 normal, float radius, float thickness, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(radius, thickness, colorStart, colorStart, colorEnd, colorEnd, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x00010561 File Offset: 0x0000E761
		public static void RingGradientAngular(Vector3 pos, Quaternion rot, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x0001059C File Offset: 0x0000E79C
		public static void RingGradientAngular(Vector3 pos, Quaternion rot, float radius, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(radius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x000105D5 File Offset: 0x0000E7D5
		public static void RingGradientAngular(Vector3 pos, Quaternion rot, float radius, float thickness, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(radius, thickness, colorStart, colorStart, colorEnd, colorEnd, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x0001060C File Offset: 0x0000E80C
		public static void RingGradientAngular(Color colorStart, Color colorEnd)
		{
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, null);
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x00010622 File Offset: 0x0000E822
		public static void RingGradientAngular(float radius, Color colorStart, Color colorEnd)
		{
			Draw.Ring_Internal(radius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, null);
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x00010634 File Offset: 0x0000E834
		public static void RingGradientAngular(float radius, float thickness, Color colorStart, Color colorEnd)
		{
			Draw.Ring_Internal(radius, thickness, colorStart, colorStart, colorEnd, colorEnd, null);
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x00010642 File Offset: 0x0000E842
		public static void RingGradientAngularDashed(Vector3 pos, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x0001066C File Offset: 0x0000E86C
		public static void RingGradientAngularDashed(Vector3 pos, float radius, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(radius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x00010692 File Offset: 0x0000E892
		public static void RingGradientAngularDashed(Vector3 pos, float radius, float thickness, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(radius, thickness, colorStart, colorStart, colorEnd, colorEnd, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x000106B6 File Offset: 0x0000E8B6
		public static void RingGradientAngularDashed(Vector3 pos, DashStyle dashStyle, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x000106DC File Offset: 0x0000E8DC
		public static void RingGradientAngularDashed(Vector3 pos, DashStyle dashStyle, float radius, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(radius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x00010700 File Offset: 0x0000E900
		public static void RingGradientAngularDashed(Vector3 pos, DashStyle dashStyle, float radius, float thickness, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(radius, thickness, colorStart, colorStart, colorEnd, colorEnd, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x00010724 File Offset: 0x0000E924
		public static void RingGradientAngularDashed(Vector3 pos, Vector3 normal, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x00010774 File Offset: 0x0000E974
		public static void RingGradientAngularDashed(Vector3 pos, Vector3 normal, float radius, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(radius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x000107C1 File Offset: 0x0000E9C1
		public static void RingGradientAngularDashed(Vector3 pos, Vector3 normal, float radius, float thickness, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(radius, thickness, colorStart, colorStart, colorEnd, colorEnd, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x00010804 File Offset: 0x0000EA04
		public static void RingGradientAngularDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x00010851 File Offset: 0x0000EA51
		public static void RingGradientAngularDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(radius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x00010891 File Offset: 0x0000EA91
		public static void RingGradientAngularDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float thickness, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(radius, thickness, colorStart, colorStart, colorEnd, colorEnd, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x000108CE File Offset: 0x0000EACE
		public static void RingGradientAngularDashed(Vector3 pos, Quaternion rot, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x0001090D File Offset: 0x0000EB0D
		public static void RingGradientAngularDashed(Vector3 pos, Quaternion rot, float radius, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(radius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x0001094A File Offset: 0x0000EB4A
		public static void RingGradientAngularDashed(Vector3 pos, Quaternion rot, float radius, float thickness, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(radius, thickness, colorStart, colorStart, colorEnd, colorEnd, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x00010985 File Offset: 0x0000EB85
		public static void RingGradientAngularDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x000109C2 File Offset: 0x0000EBC2
		public static void RingGradientAngularDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(radius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x000109FD File Offset: 0x0000EBFD
		public static void RingGradientAngularDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float thickness, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(radius, thickness, colorStart, colorStart, colorEnd, colorEnd, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x00010A35 File Offset: 0x0000EC35
		public static void RingGradientAngularDashed(Color colorStart, Color colorEnd)
		{
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, Draw.RingDashStyle);
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x00010A4F File Offset: 0x0000EC4F
		public static void RingGradientAngularDashed(float radius, Color colorStart, Color colorEnd)
		{
			Draw.Ring_Internal(radius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, Draw.RingDashStyle);
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x00010A65 File Offset: 0x0000EC65
		public static void RingGradientAngularDashed(float radius, float thickness, Color colorStart, Color colorEnd)
		{
			Draw.Ring_Internal(radius, thickness, colorStart, colorStart, colorEnd, colorEnd, Draw.RingDashStyle);
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x00010A77 File Offset: 0x0000EC77
		public static void RingGradientAngularDashed(DashStyle dashStyle, Color colorStart, Color colorEnd)
		{
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, dashStyle);
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x00010A8D File Offset: 0x0000EC8D
		public static void RingGradientAngularDashed(DashStyle dashStyle, float radius, Color colorStart, Color colorEnd)
		{
			Draw.Ring_Internal(radius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, dashStyle);
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x00010A9F File Offset: 0x0000EC9F
		public static void RingGradientAngularDashed(DashStyle dashStyle, float radius, float thickness, Color colorStart, Color colorEnd)
		{
			Draw.Ring_Internal(radius, thickness, colorStart, colorStart, colorEnd, colorEnd, dashStyle);
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x00010AAF File Offset: 0x0000ECAF
		public static void RingGradientBilinear(Vector3 pos, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, null);
			Draw.PopMatrix();
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x00010AD6 File Offset: 0x0000ECD6
		public static void RingGradientBilinear(Vector3 pos, float radius, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(radius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, null);
			Draw.PopMatrix();
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x00010AFA File Offset: 0x0000ECFA
		public static void RingGradientBilinear(Vector3 pos, float radius, float thickness, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(radius, thickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, null);
			Draw.PopMatrix();
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x00010B1C File Offset: 0x0000ED1C
		public static void RingGradientBilinear(Vector3 pos, Vector3 normal, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, null);
			Draw.PopMatrix();
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x00010B69 File Offset: 0x0000ED69
		public static void RingGradientBilinear(Vector3 pos, Vector3 normal, float radius, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(radius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, null);
			Draw.PopMatrix();
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x00010BA8 File Offset: 0x0000EDA8
		public static void RingGradientBilinear(Vector3 pos, Vector3 normal, float radius, float thickness, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(radius, thickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, null);
			Draw.PopMatrix();
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x00010BE4 File Offset: 0x0000EDE4
		public static void RingGradientBilinear(Vector3 pos, Quaternion rot, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, null);
			Draw.PopMatrix();
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x00010C21 File Offset: 0x0000EE21
		public static void RingGradientBilinear(Vector3 pos, Quaternion rot, float radius, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(radius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, null);
			Draw.PopMatrix();
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x00010C5B File Offset: 0x0000EE5B
		public static void RingGradientBilinear(Vector3 pos, Quaternion rot, float radius, float thickness, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(radius, thickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, null);
			Draw.PopMatrix();
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x00010C92 File Offset: 0x0000EE92
		public static void RingGradientBilinear(Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, null);
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x00010CA8 File Offset: 0x0000EEA8
		public static void RingGradientBilinear(float radius, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.Ring_Internal(radius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, null);
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x00010CBB File Offset: 0x0000EEBB
		public static void RingGradientBilinear(float radius, float thickness, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.Ring_Internal(radius, thickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, null);
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x00010CCB File Offset: 0x0000EECB
		public static void RingGradientBilinearDashed(Vector3 pos, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x00010CF6 File Offset: 0x0000EEF6
		public static void RingGradientBilinearDashed(Vector3 pos, float radius, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(radius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x00010D1E File Offset: 0x0000EF1E
		public static void RingGradientBilinearDashed(Vector3 pos, float radius, float thickness, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(radius, thickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x00010D43 File Offset: 0x0000EF43
		public static void RingGradientBilinearDashed(Vector3 pos, DashStyle dashStyle, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x00010D6B File Offset: 0x0000EF6B
		public static void RingGradientBilinearDashed(Vector3 pos, DashStyle dashStyle, float radius, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(radius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x00010D90 File Offset: 0x0000EF90
		public static void RingGradientBilinearDashed(Vector3 pos, DashStyle dashStyle, float radius, float thickness, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Ring_Internal(radius, thickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x00010DB4 File Offset: 0x0000EFB4
		public static void RingGradientBilinearDashed(Vector3 pos, Vector3 normal, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x00010E08 File Offset: 0x0000F008
		public static void RingGradientBilinearDashed(Vector3 pos, Vector3 normal, float radius, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(radius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x00010E56 File Offset: 0x0000F056
		public static void RingGradientBilinearDashed(Vector3 pos, Vector3 normal, float radius, float thickness, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(radius, thickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x00010E98 File Offset: 0x0000F098
		public static void RingGradientBilinearDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x00010EE6 File Offset: 0x0000F0E6
		public static void RingGradientBilinearDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(radius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x00010F26 File Offset: 0x0000F126
		public static void RingGradientBilinearDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float thickness, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Ring_Internal(radius, thickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x00010F64 File Offset: 0x0000F164
		public static void RingGradientBilinearDashed(Vector3 pos, Quaternion rot, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x00010FB0 File Offset: 0x0000F1B0
		public static void RingGradientBilinearDashed(Vector3 pos, Quaternion rot, float radius, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(radius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x00010FEE File Offset: 0x0000F1EE
		public static void RingGradientBilinearDashed(Vector3 pos, Quaternion rot, float radius, float thickness, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(radius, thickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x00011029 File Offset: 0x0000F229
		public static void RingGradientBilinearDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x00011067 File Offset: 0x0000F267
		public static void RingGradientBilinearDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(radius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x000110A2 File Offset: 0x0000F2A2
		public static void RingGradientBilinearDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float thickness, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Ring_Internal(radius, thickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x000110DA File Offset: 0x0000F2DA
		public static void RingGradientBilinearDashed(Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, Draw.RingDashStyle);
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x000110F4 File Offset: 0x0000F2F4
		public static void RingGradientBilinearDashed(float radius, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.Ring_Internal(radius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, Draw.RingDashStyle);
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x0001110B File Offset: 0x0000F30B
		public static void RingGradientBilinearDashed(float radius, float thickness, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.Ring_Internal(radius, thickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, Draw.RingDashStyle);
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x0001111F File Offset: 0x0000F31F
		public static void RingGradientBilinearDashed(DashStyle dashStyle, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.Ring_Internal(Draw.DiscRadius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, dashStyle);
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x00011136 File Offset: 0x0000F336
		public static void RingGradientBilinearDashed(DashStyle dashStyle, float radius, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.Ring_Internal(radius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, dashStyle);
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x0001114A File Offset: 0x0000F34A
		public static void RingGradientBilinearDashed(DashStyle dashStyle, float radius, float thickness, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.Ring_Internal(radius, thickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, dashStyle);
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x0001115B File Offset: 0x0000F35B
		public static void Pie(Vector3 pos, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Pie_Internal(Draw.DiscRadius, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd);
			Draw.PopMatrix();
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x0001118D File Offset: 0x0000F38D
		public static void Pie(Vector3 pos, float angleRadStart, float angleRadEnd, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Pie_Internal(Draw.DiscRadius, color, color, color, color, angleRadStart, angleRadEnd);
			Draw.PopMatrix();
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x000111AF File Offset: 0x0000F3AF
		public static void Pie(Vector3 pos, float radius, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Pie_Internal(radius, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd);
			Draw.PopMatrix();
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x000111DD File Offset: 0x0000F3DD
		public static void Pie(Vector3 pos, float radius, float angleRadStart, float angleRadEnd, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Pie_Internal(radius, color, color, color, color, angleRadStart, angleRadEnd);
			Draw.PopMatrix();
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x00011200 File Offset: 0x0000F400
		public static void Pie(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Pie_Internal(Draw.DiscRadius, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd);
			Draw.PopMatrix();
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x00011257 File Offset: 0x0000F457
		public static void Pie(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Pie_Internal(Draw.DiscRadius, color, color, color, color, angleRadStart, angleRadEnd);
			Draw.PopMatrix();
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x00011298 File Offset: 0x0000F498
		public static void Pie(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Pie_Internal(radius, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd);
			Draw.PopMatrix();
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x000112EC File Offset: 0x0000F4EC
		public static void Pie(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Pie_Internal(radius, color, color, color, color, angleRadStart, angleRadEnd);
			Draw.PopMatrix();
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x0001132C File Offset: 0x0000F52C
		public static void Pie(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Pie_Internal(Draw.DiscRadius, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd);
			Draw.PopMatrix();
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x0001137E File Offset: 0x0000F57E
		public static void Pie(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Pie_Internal(Draw.DiscRadius, color, color, color, color, angleRadStart, angleRadEnd);
			Draw.PopMatrix();
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x000113BC File Offset: 0x0000F5BC
		public static void Pie(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Pie_Internal(radius, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd);
			Draw.PopMatrix();
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x0001140B File Offset: 0x0000F60B
		public static void Pie(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Pie_Internal(radius, color, color, color, color, angleRadStart, angleRadEnd);
			Draw.PopMatrix();
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x00011443 File Offset: 0x0000F643
		public static void Pie(float angleRadStart, float angleRadEnd)
		{
			Draw.Pie_Internal(Draw.DiscRadius, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd);
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x00011465 File Offset: 0x0000F665
		public static void Pie(float angleRadStart, float angleRadEnd, Color color)
		{
			Draw.Pie_Internal(Draw.DiscRadius, color, color, color, color, angleRadStart, angleRadEnd);
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x00011477 File Offset: 0x0000F677
		public static void Pie(float radius, float angleRadStart, float angleRadEnd)
		{
			Draw.Pie_Internal(radius, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd);
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x00011495 File Offset: 0x0000F695
		public static void Pie(float radius, float angleRadStart, float angleRadEnd, Color color)
		{
			Draw.Pie_Internal(radius, color, color, color, color, angleRadStart, angleRadEnd);
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x000114A3 File Offset: 0x0000F6A3
		public static void PieGradientRadial(Vector3 pos, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Pie_Internal(Draw.DiscRadius, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd);
			Draw.PopMatrix();
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x000114C7 File Offset: 0x0000F6C7
		public static void PieGradientRadial(Vector3 pos, float radius, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Pie_Internal(radius, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd);
			Draw.PopMatrix();
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x000114E9 File Offset: 0x0000F6E9
		public static void PieGradientRadial(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Pie_Internal(Draw.DiscRadius, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd);
			Draw.PopMatrix();
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x00011529 File Offset: 0x0000F729
		public static void PieGradientRadial(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Pie_Internal(radius, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd);
			Draw.PopMatrix();
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x00011566 File Offset: 0x0000F766
		public static void PieGradientRadial(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Pie_Internal(Draw.DiscRadius, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd);
			Draw.PopMatrix();
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x000115A1 File Offset: 0x0000F7A1
		public static void PieGradientRadial(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Pie_Internal(radius, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd);
			Draw.PopMatrix();
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x000115D9 File Offset: 0x0000F7D9
		public static void PieGradientRadial(float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
			Draw.Pie_Internal(Draw.DiscRadius, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd);
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x000115EB File Offset: 0x0000F7EB
		public static void PieGradientRadial(float radius, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
			Draw.Pie_Internal(radius, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd);
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x000115FB File Offset: 0x0000F7FB
		public static void PieGradientAngular(Vector3 pos, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Pie_Internal(Draw.DiscRadius, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd);
			Draw.PopMatrix();
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x0001161F File Offset: 0x0000F81F
		public static void PieGradientAngular(Vector3 pos, float radius, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Pie_Internal(radius, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd);
			Draw.PopMatrix();
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x00011641 File Offset: 0x0000F841
		public static void PieGradientAngular(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Pie_Internal(Draw.DiscRadius, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd);
			Draw.PopMatrix();
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x00011681 File Offset: 0x0000F881
		public static void PieGradientAngular(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Pie_Internal(radius, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd);
			Draw.PopMatrix();
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x000116BE File Offset: 0x0000F8BE
		public static void PieGradientAngular(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Pie_Internal(Draw.DiscRadius, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd);
			Draw.PopMatrix();
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x000116F9 File Offset: 0x0000F8F9
		public static void PieGradientAngular(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Pie_Internal(radius, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd);
			Draw.PopMatrix();
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x00011731 File Offset: 0x0000F931
		public static void PieGradientAngular(float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
			Draw.Pie_Internal(Draw.DiscRadius, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd);
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x00011743 File Offset: 0x0000F943
		public static void PieGradientAngular(float radius, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
			Draw.Pie_Internal(radius, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd);
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x00011753 File Offset: 0x0000F953
		public static void PieGradientBilinear(Vector3 pos, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Pie_Internal(Draw.DiscRadius, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd);
			Draw.PopMatrix();
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x00011778 File Offset: 0x0000F978
		public static void PieGradientBilinear(Vector3 pos, float radius, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Pie_Internal(radius, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd);
			Draw.PopMatrix();
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x0001179A File Offset: 0x0000F99A
		public static void PieGradientBilinear(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Pie_Internal(Draw.DiscRadius, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd);
			Draw.PopMatrix();
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x000117DA File Offset: 0x0000F9DA
		public static void PieGradientBilinear(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Pie_Internal(radius, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd);
			Draw.PopMatrix();
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x00011817 File Offset: 0x0000FA17
		public static void PieGradientBilinear(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Pie_Internal(Draw.DiscRadius, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd);
			Draw.PopMatrix();
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x00011852 File Offset: 0x0000FA52
		public static void PieGradientBilinear(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Pie_Internal(radius, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd);
			Draw.PopMatrix();
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x0001188A File Offset: 0x0000FA8A
		public static void PieGradientBilinear(float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.Pie_Internal(Draw.DiscRadius, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd);
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x0001189E File Offset: 0x0000FA9E
		public static void PieGradientBilinear(float radius, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.Pie_Internal(radius, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd);
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x000118B0 File Offset: 0x0000FAB0
		public static void Arc(Vector3 pos, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, ArcEndCap.None, null);
			Draw.PopMatrix();
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x000118F4 File Offset: 0x0000FAF4
		public static void Arc(Vector3 pos, float angleRadStart, float angleRadEnd, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, color, color, color, color, angleRadStart, angleRadEnd, ArcEndCap.None, null);
			Draw.PopMatrix();
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x00011928 File Offset: 0x0000FB28
		public static void Arc(Vector3 pos, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, endCaps, null);
			Draw.PopMatrix();
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x0001196C File Offset: 0x0000FB6C
		public static void Arc(Vector3 pos, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, color, color, color, color, angleRadStart, angleRadEnd, endCaps, null);
			Draw.PopMatrix();
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x000119A4 File Offset: 0x0000FBA4
		public static void Arc(Vector3 pos, float radius, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, ArcEndCap.None, null);
			Draw.PopMatrix();
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x000119E4 File Offset: 0x0000FBE4
		public static void Arc(Vector3 pos, float radius, float angleRadStart, float angleRadEnd, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, Draw.RingThickness, color, color, color, color, angleRadStart, angleRadEnd, ArcEndCap.None, null);
			Draw.PopMatrix();
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x00011A18 File Offset: 0x0000FC18
		public static void Arc(Vector3 pos, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, endCaps, null);
			Draw.PopMatrix();
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x00011A5C File Offset: 0x0000FC5C
		public static void Arc(Vector3 pos, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, Draw.RingThickness, color, color, color, color, angleRadStart, angleRadEnd, endCaps, null);
			Draw.PopMatrix();
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x00011A94 File Offset: 0x0000FC94
		public static void Arc(Vector3 pos, float radius, float thickness, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, thickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, ArcEndCap.None, null);
			Draw.PopMatrix();
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x00011AD4 File Offset: 0x0000FCD4
		public static void Arc(Vector3 pos, float radius, float thickness, float angleRadStart, float angleRadEnd, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, thickness, color, color, color, color, angleRadStart, angleRadEnd, ArcEndCap.None, null);
			Draw.PopMatrix();
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x00011B08 File Offset: 0x0000FD08
		public static void Arc(Vector3 pos, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, thickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, endCaps, null);
			Draw.PopMatrix();
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x00011B48 File Offset: 0x0000FD48
		public static void Arc(Vector3 pos, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, thickness, color, color, color, color, angleRadStart, angleRadEnd, endCaps, null);
			Draw.PopMatrix();
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x00011B7C File Offset: 0x0000FD7C
		public static void Arc(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, ArcEndCap.None, null);
			Draw.PopMatrix();
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x00011BDC File Offset: 0x0000FDDC
		public static void Arc(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, color, color, color, color, angleRadStart, angleRadEnd, ArcEndCap.None, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x00011C30 File Offset: 0x0000FE30
		public static void Arc(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, endCaps, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x00011C90 File Offset: 0x0000FE90
		public static void Arc(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, color, color, color, color, angleRadStart, angleRadEnd, endCaps, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x00011CE4 File Offset: 0x0000FEE4
		public static void Arc(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, ArcEndCap.None, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x00011D40 File Offset: 0x0000FF40
		public static void Arc(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, color, color, color, color, angleRadStart, angleRadEnd, ArcEndCap.None, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x00011D90 File Offset: 0x0000FF90
		public static void Arc(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, endCaps, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x00011DEC File Offset: 0x0000FFEC
		public static void Arc(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, color, color, color, color, angleRadStart, angleRadEnd, endCaps, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x00011E3C File Offset: 0x0001003C
		public static void Arc(Vector3 pos, Vector3 normal, float radius, float thickness, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, thickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, ArcEndCap.None, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x00011E94 File Offset: 0x00010094
		public static void Arc(Vector3 pos, Vector3 normal, float radius, float thickness, float angleRadStart, float angleRadEnd, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, thickness, color, color, color, color, angleRadStart, angleRadEnd, ArcEndCap.None, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x00011EE0 File Offset: 0x000100E0
		public static void Arc(Vector3 pos, Vector3 normal, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, thickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, endCaps, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x00011F3C File Offset: 0x0001013C
		public static void Arc(Vector3 pos, Vector3 normal, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, thickness, color, color, color, color, angleRadStart, angleRadEnd, endCaps, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x00011F8C File Offset: 0x0001018C
		public static void Arc(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, ArcEndCap.None, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x00011FE8 File Offset: 0x000101E8
		public static void Arc(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, color, color, color, color, angleRadStart, angleRadEnd, ArcEndCap.None, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x00012038 File Offset: 0x00010238
		public static void Arc(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, endCaps, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x00012094 File Offset: 0x00010294
		public static void Arc(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, color, color, color, color, angleRadStart, angleRadEnd, endCaps, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x000120E4 File Offset: 0x000102E4
		public static void Arc(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, ArcEndCap.None, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x0001213C File Offset: 0x0001033C
		public static void Arc(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, color, color, color, color, angleRadStart, angleRadEnd, ArcEndCap.None, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x00012188 File Offset: 0x00010388
		public static void Arc(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, endCaps, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x000121E0 File Offset: 0x000103E0
		public static void Arc(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, color, color, color, color, angleRadStart, angleRadEnd, endCaps, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x0001222C File Offset: 0x0001042C
		public static void Arc(Vector3 pos, Quaternion rot, float radius, float thickness, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, thickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, ArcEndCap.None, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x00012280 File Offset: 0x00010480
		public static void Arc(Vector3 pos, Quaternion rot, float radius, float thickness, float angleRadStart, float angleRadEnd, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, thickness, color, color, color, color, angleRadStart, angleRadEnd, ArcEndCap.None, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x000122C8 File Offset: 0x000104C8
		public static void Arc(Vector3 pos, Quaternion rot, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, thickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, endCaps, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x0001231C File Offset: 0x0001051C
		public static void Arc(Vector3 pos, Quaternion rot, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, thickness, color, color, color, color, angleRadStart, angleRadEnd, endCaps, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x00012364 File Offset: 0x00010564
		public static void Arc(float angleRadStart, float angleRadEnd)
		{
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, ArcEndCap.None, null);
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x00012398 File Offset: 0x00010598
		public static void Arc(float angleRadStart, float angleRadEnd, Color color)
		{
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, color, color, color, color, angleRadStart, angleRadEnd, ArcEndCap.None, null);
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x000123BC File Offset: 0x000105BC
		public static void Arc(float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, endCaps, null);
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x000123F0 File Offset: 0x000105F0
		public static void Arc(float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, color, color, color, color, angleRadStart, angleRadEnd, endCaps, null);
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x00012414 File Offset: 0x00010614
		public static void Arc(float radius, float angleRadStart, float angleRadEnd)
		{
			Draw.Arc_Internal(radius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, ArcEndCap.None, null);
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x00012444 File Offset: 0x00010644
		public static void Arc(float radius, float angleRadStart, float angleRadEnd, Color color)
		{
			Draw.Arc_Internal(radius, Draw.RingThickness, color, color, color, color, angleRadStart, angleRadEnd, ArcEndCap.None, null);
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x00012464 File Offset: 0x00010664
		public static void Arc(float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
			Draw.Arc_Internal(radius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, endCaps, null);
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x00012494 File Offset: 0x00010694
		public static void Arc(float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
			Draw.Arc_Internal(radius, Draw.RingThickness, color, color, color, color, angleRadStart, angleRadEnd, endCaps, null);
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x000124B8 File Offset: 0x000106B8
		public static void Arc(float radius, float thickness, float angleRadStart, float angleRadEnd)
		{
			Draw.Arc_Internal(radius, thickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, ArcEndCap.None, null);
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x000124E4 File Offset: 0x000106E4
		public static void Arc(float radius, float thickness, float angleRadStart, float angleRadEnd, Color color)
		{
			Draw.Arc_Internal(radius, thickness, color, color, color, color, angleRadStart, angleRadEnd, ArcEndCap.None, null);
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x00012504 File Offset: 0x00010704
		public static void Arc(float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
			Draw.Arc_Internal(radius, thickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, endCaps, null);
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x00012534 File Offset: 0x00010734
		public static void Arc(float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
			Draw.Arc_Internal(radius, thickness, color, color, color, color, angleRadStart, angleRadEnd, endCaps, null);
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x00012558 File Offset: 0x00010758
		public static void ArcDashed(Vector3 pos, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x000125A0 File Offset: 0x000107A0
		public static void ArcDashed(Vector3 pos, float angleRadStart, float angleRadEnd, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, color, color, color, color, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x000125D8 File Offset: 0x000107D8
		public static void ArcDashed(Vector3 pos, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x00012620 File Offset: 0x00010820
		public static void ArcDashed(Vector3 pos, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, color, color, color, color, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x0001265C File Offset: 0x0001085C
		public static void ArcDashed(Vector3 pos, float radius, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x000126A0 File Offset: 0x000108A0
		public static void ArcDashed(Vector3 pos, float radius, float angleRadStart, float angleRadEnd, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, Draw.RingThickness, color, color, color, color, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x000126D8 File Offset: 0x000108D8
		public static void ArcDashed(Vector3 pos, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x00012720 File Offset: 0x00010920
		public static void ArcDashed(Vector3 pos, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, Draw.RingThickness, color, color, color, color, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x0001275C File Offset: 0x0001095C
		public static void ArcDashed(Vector3 pos, float radius, float thickness, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, thickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x000127A0 File Offset: 0x000109A0
		public static void ArcDashed(Vector3 pos, float radius, float thickness, float angleRadStart, float angleRadEnd, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, thickness, color, color, color, color, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x000127D8 File Offset: 0x000109D8
		public static void ArcDashed(Vector3 pos, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, thickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x0001281C File Offset: 0x00010A1C
		public static void ArcDashed(Vector3 pos, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, thickness, color, color, color, color, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x00012854 File Offset: 0x00010A54
		public static void ArcDashed(Vector3 pos, DashStyle dashStyle, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x00012898 File Offset: 0x00010A98
		public static void ArcDashed(Vector3 pos, DashStyle dashStyle, float angleRadStart, float angleRadEnd, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, color, color, color, color, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x000128D0 File Offset: 0x00010AD0
		public static void ArcDashed(Vector3 pos, DashStyle dashStyle, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, endCaps, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x00012918 File Offset: 0x00010B18
		public static void ArcDashed(Vector3 pos, DashStyle dashStyle, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, color, color, color, color, angleRadStart, angleRadEnd, endCaps, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x00012954 File Offset: 0x00010B54
		public static void ArcDashed(Vector3 pos, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x00012998 File Offset: 0x00010B98
		public static void ArcDashed(Vector3 pos, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, Draw.RingThickness, color, color, color, color, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x000129D0 File Offset: 0x00010BD0
		public static void ArcDashed(Vector3 pos, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, endCaps, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x00012A14 File Offset: 0x00010C14
		public static void ArcDashed(Vector3 pos, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, Draw.RingThickness, color, color, color, color, angleRadStart, angleRadEnd, endCaps, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x00012A4C File Offset: 0x00010C4C
		public static void ArcDashed(Vector3 pos, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, thickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x00012A8C File Offset: 0x00010C8C
		public static void ArcDashed(Vector3 pos, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, thickness, color, color, color, color, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x00012AC0 File Offset: 0x00010CC0
		public static void ArcDashed(Vector3 pos, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, thickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, endCaps, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x00012B00 File Offset: 0x00010D00
		public static void ArcDashed(Vector3 pos, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, thickness, color, color, color, color, angleRadStart, angleRadEnd, endCaps, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x00012B34 File Offset: 0x00010D34
		public static void ArcDashed(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x00012B98 File Offset: 0x00010D98
		public static void ArcDashed(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, color, color, color, color, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x00012BF0 File Offset: 0x00010DF0
		public static void ArcDashed(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x00012C54 File Offset: 0x00010E54
		public static void ArcDashed(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, color, color, color, color, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x00012CAC File Offset: 0x00010EAC
		public static void ArcDashed(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x00012D0C File Offset: 0x00010F0C
		public static void ArcDashed(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, color, color, color, color, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x00012D60 File Offset: 0x00010F60
		public static void ArcDashed(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x00012DC0 File Offset: 0x00010FC0
		public static void ArcDashed(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, color, color, color, color, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x00012E14 File Offset: 0x00011014
		public static void ArcDashed(Vector3 pos, Vector3 normal, float radius, float thickness, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, thickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x00012E70 File Offset: 0x00011070
		public static void ArcDashed(Vector3 pos, Vector3 normal, float radius, float thickness, float angleRadStart, float angleRadEnd, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, thickness, color, color, color, color, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x00012EC0 File Offset: 0x000110C0
		public static void ArcDashed(Vector3 pos, Vector3 normal, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, thickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x00012F20 File Offset: 0x00011120
		public static void ArcDashed(Vector3 pos, Vector3 normal, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, thickness, color, color, color, color, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x00012F74 File Offset: 0x00011174
		public static void ArcDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x00012FD4 File Offset: 0x000111D4
		public static void ArcDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float angleRadStart, float angleRadEnd, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, color, color, color, color, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x00013028 File Offset: 0x00011228
		public static void ArcDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, endCaps, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x00013088 File Offset: 0x00011288
		public static void ArcDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, color, color, color, color, angleRadStart, angleRadEnd, endCaps, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x000130DC File Offset: 0x000112DC
		public static void ArcDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x00013138 File Offset: 0x00011338
		public static void ArcDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, color, color, color, color, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x00013188 File Offset: 0x00011388
		public static void ArcDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, endCaps, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x000131E8 File Offset: 0x000113E8
		public static void ArcDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, color, color, color, color, angleRadStart, angleRadEnd, endCaps, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x0001323C File Offset: 0x0001143C
		public static void ArcDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, thickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x00013298 File Offset: 0x00011498
		public static void ArcDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, thickness, color, color, color, color, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x000132E8 File Offset: 0x000114E8
		public static void ArcDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, thickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, endCaps, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x00013344 File Offset: 0x00011544
		public static void ArcDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, thickness, color, color, color, color, angleRadStart, angleRadEnd, endCaps, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x00013394 File Offset: 0x00011594
		public static void ArcDashed(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x000133F4 File Offset: 0x000115F4
		public static void ArcDashed(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, color, color, color, color, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x00013448 File Offset: 0x00011648
		public static void ArcDashed(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x000134A8 File Offset: 0x000116A8
		public static void ArcDashed(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, color, color, color, color, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x000134FC File Offset: 0x000116FC
		public static void ArcDashed(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x00013558 File Offset: 0x00011758
		public static void ArcDashed(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, color, color, color, color, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x000135A8 File Offset: 0x000117A8
		public static void ArcDashed(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x00013604 File Offset: 0x00011804
		public static void ArcDashed(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, color, color, color, color, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x00013654 File Offset: 0x00011854
		public static void ArcDashed(Vector3 pos, Quaternion rot, float radius, float thickness, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, thickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x000136AC File Offset: 0x000118AC
		public static void ArcDashed(Vector3 pos, Quaternion rot, float radius, float thickness, float angleRadStart, float angleRadEnd, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, thickness, color, color, color, color, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x000136F8 File Offset: 0x000118F8
		public static void ArcDashed(Vector3 pos, Quaternion rot, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, thickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x00013750 File Offset: 0x00011950
		public static void ArcDashed(Vector3 pos, Quaternion rot, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, thickness, color, color, color, color, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x0001379C File Offset: 0x0001199C
		public static void ArcDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x000137F8 File Offset: 0x000119F8
		public static void ArcDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float angleRadStart, float angleRadEnd, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, color, color, color, color, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x00013848 File Offset: 0x00011A48
		public static void ArcDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, endCaps, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x000138A4 File Offset: 0x00011AA4
		public static void ArcDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, color, color, color, color, angleRadStart, angleRadEnd, endCaps, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x000138F4 File Offset: 0x00011AF4
		public static void ArcDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x0001394C File Offset: 0x00011B4C
		public static void ArcDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, color, color, color, color, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x00013998 File Offset: 0x00011B98
		public static void ArcDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, endCaps, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x000139F0 File Offset: 0x00011BF0
		public static void ArcDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, color, color, color, color, angleRadStart, angleRadEnd, endCaps, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x00013A3C File Offset: 0x00011C3C
		public static void ArcDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, thickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x00013A90 File Offset: 0x00011C90
		public static void ArcDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, thickness, color, color, color, color, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x00013AD8 File Offset: 0x00011CD8
		public static void ArcDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, thickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, endCaps, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x00013B30 File Offset: 0x00011D30
		public static void ArcDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, thickness, color, color, color, color, angleRadStart, angleRadEnd, endCaps, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x00013B7C File Offset: 0x00011D7C
		public static void ArcDashed(float angleRadStart, float angleRadEnd)
		{
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x00013BB4 File Offset: 0x00011DB4
		public static void ArcDashed(float angleRadStart, float angleRadEnd, Color color)
		{
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, color, color, color, color, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x00013BDC File Offset: 0x00011DDC
		public static void ArcDashed(float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x00013C14 File Offset: 0x00011E14
		public static void ArcDashed(float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, color, color, color, color, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x00013C3C File Offset: 0x00011E3C
		public static void ArcDashed(float radius, float angleRadStart, float angleRadEnd)
		{
			Draw.Arc_Internal(radius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x00013C70 File Offset: 0x00011E70
		public static void ArcDashed(float radius, float angleRadStart, float angleRadEnd, Color color)
		{
			Draw.Arc_Internal(radius, Draw.RingThickness, color, color, color, color, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x00013C94 File Offset: 0x00011E94
		public static void ArcDashed(float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
			Draw.Arc_Internal(radius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x00013CC8 File Offset: 0x00011EC8
		public static void ArcDashed(float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
			Draw.Arc_Internal(radius, Draw.RingThickness, color, color, color, color, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x00013CF0 File Offset: 0x00011EF0
		public static void ArcDashed(float radius, float thickness, float angleRadStart, float angleRadEnd)
		{
			Draw.Arc_Internal(radius, thickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x00013D20 File Offset: 0x00011F20
		public static void ArcDashed(float radius, float thickness, float angleRadStart, float angleRadEnd, Color color)
		{
			Draw.Arc_Internal(radius, thickness, color, color, color, color, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x00013D44 File Offset: 0x00011F44
		public static void ArcDashed(float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
			Draw.Arc_Internal(radius, thickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x00013D78 File Offset: 0x00011F78
		public static void ArcDashed(float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
			Draw.Arc_Internal(radius, thickness, color, color, color, color, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x00013DA0 File Offset: 0x00011FA0
		public static void ArcDashed(DashStyle dashStyle, float angleRadStart, float angleRadEnd)
		{
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x00013DD4 File Offset: 0x00011FD4
		public static void ArcDashed(DashStyle dashStyle, float angleRadStart, float angleRadEnd, Color color)
		{
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, color, color, color, color, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x00013DF8 File Offset: 0x00011FF8
		public static void ArcDashed(DashStyle dashStyle, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, endCaps, dashStyle);
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x00013E2C File Offset: 0x0001202C
		public static void ArcDashed(DashStyle dashStyle, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, color, color, color, color, angleRadStart, angleRadEnd, endCaps, dashStyle);
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x00013E54 File Offset: 0x00012054
		public static void ArcDashed(DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd)
		{
			Draw.Arc_Internal(radius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x00013E84 File Offset: 0x00012084
		public static void ArcDashed(DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, Color color)
		{
			Draw.Arc_Internal(radius, Draw.RingThickness, color, color, color, color, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x00013EA8 File Offset: 0x000120A8
		public static void ArcDashed(DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
			Draw.Arc_Internal(radius, Draw.RingThickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, endCaps, dashStyle);
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x00013EDC File Offset: 0x000120DC
		public static void ArcDashed(DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
			Draw.Arc_Internal(radius, Draw.RingThickness, color, color, color, color, angleRadStart, angleRadEnd, endCaps, dashStyle);
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x00013F04 File Offset: 0x00012104
		public static void ArcDashed(DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd)
		{
			Draw.Arc_Internal(radius, thickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x00013F34 File Offset: 0x00012134
		public static void ArcDashed(DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, Color color)
		{
			Draw.Arc_Internal(radius, thickness, color, color, color, color, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x00013F58 File Offset: 0x00012158
		public static void ArcDashed(DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps)
		{
			Draw.Arc_Internal(radius, thickness, Draw.Color, Draw.Color, Draw.Color, Draw.Color, angleRadStart, angleRadEnd, endCaps, dashStyle);
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x00013F88 File Offset: 0x00012188
		public static void ArcDashed(DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color color)
		{
			Draw.Arc_Internal(radius, thickness, color, color, color, color, angleRadStart, angleRadEnd, endCaps, dashStyle);
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x00013FAC File Offset: 0x000121AC
		public static void ArcGradientRadial(Vector3 pos, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, ArcEndCap.None, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x00013FE4 File Offset: 0x000121E4
		public static void ArcGradientRadial(Vector3 pos, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, endCaps, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x0001401C File Offset: 0x0001221C
		public static void ArcGradientRadial(Vector3 pos, float radius, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, ArcEndCap.None, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x00014050 File Offset: 0x00012250
		public static void ArcGradientRadial(Vector3 pos, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, endCaps, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x00014088 File Offset: 0x00012288
		public static void ArcGradientRadial(Vector3 pos, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, thickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, ArcEndCap.None, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x000140BC File Offset: 0x000122BC
		public static void ArcGradientRadial(Vector3 pos, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, thickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, endCaps, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x000140F0 File Offset: 0x000122F0
		public static void ArcGradientRadial(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, ArcEndCap.None, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x00014144 File Offset: 0x00012344
		public static void ArcGradientRadial(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, endCaps, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x00014198 File Offset: 0x00012398
		public static void ArcGradientRadial(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, ArcEndCap.None, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x000141E8 File Offset: 0x000123E8
		public static void ArcGradientRadial(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, endCaps, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x00014238 File Offset: 0x00012438
		public static void ArcGradientRadial(Vector3 pos, Vector3 normal, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, thickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, ArcEndCap.None, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x00014284 File Offset: 0x00012484
		public static void ArcGradientRadial(Vector3 pos, Vector3 normal, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, thickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, endCaps, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x000142D4 File Offset: 0x000124D4
		public static void ArcGradientRadial(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, ArcEndCap.None, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x00014324 File Offset: 0x00012524
		public static void ArcGradientRadial(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, endCaps, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x00014374 File Offset: 0x00012574
		public static void ArcGradientRadial(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, ArcEndCap.None, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x000143C0 File Offset: 0x000125C0
		public static void ArcGradientRadial(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, endCaps, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x0001440C File Offset: 0x0001260C
		public static void ArcGradientRadial(Vector3 pos, Quaternion rot, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, thickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, ArcEndCap.None, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x00014454 File Offset: 0x00012654
		public static void ArcGradientRadial(Vector3 pos, Quaternion rot, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, thickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, endCaps, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x0001449C File Offset: 0x0001269C
		public static void ArcGradientRadial(float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, ArcEndCap.None, null);
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x000144C0 File Offset: 0x000126C0
		public static void ArcGradientRadial(float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, endCaps, null);
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x000144E8 File Offset: 0x000126E8
		public static void ArcGradientRadial(float radius, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
			Draw.Arc_Internal(radius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, ArcEndCap.None, null);
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x0001450C File Offset: 0x0001270C
		public static void ArcGradientRadial(float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
			Draw.Arc_Internal(radius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, endCaps, null);
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x00014530 File Offset: 0x00012730
		public static void ArcGradientRadial(float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
			Draw.Arc_Internal(radius, thickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, ArcEndCap.None, null);
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x00014550 File Offset: 0x00012750
		public static void ArcGradientRadial(float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
			Draw.Arc_Internal(radius, thickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, endCaps, null);
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x00014574 File Offset: 0x00012774
		public static void ArcGradientRadialDashed(Vector3 pos, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x000145B0 File Offset: 0x000127B0
		public static void ArcGradientRadialDashed(Vector3 pos, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x000145EC File Offset: 0x000127EC
		public static void ArcGradientRadialDashed(Vector3 pos, float radius, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x00014624 File Offset: 0x00012824
		public static void ArcGradientRadialDashed(Vector3 pos, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x00014660 File Offset: 0x00012860
		public static void ArcGradientRadialDashed(Vector3 pos, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, thickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x00014698 File Offset: 0x00012898
		public static void ArcGradientRadialDashed(Vector3 pos, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, thickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x000146D0 File Offset: 0x000128D0
		public static void ArcGradientRadialDashed(Vector3 pos, DashStyle dashStyle, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x00014708 File Offset: 0x00012908
		public static void ArcGradientRadialDashed(Vector3 pos, DashStyle dashStyle, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, endCaps, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x00014744 File Offset: 0x00012944
		public static void ArcGradientRadialDashed(Vector3 pos, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x0001477C File Offset: 0x0001297C
		public static void ArcGradientRadialDashed(Vector3 pos, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, endCaps, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x000147B4 File Offset: 0x000129B4
		public static void ArcGradientRadialDashed(Vector3 pos, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, thickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x000147E8 File Offset: 0x000129E8
		public static void ArcGradientRadialDashed(Vector3 pos, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, thickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, endCaps, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x0001481C File Offset: 0x00012A1C
		public static void ArcGradientRadialDashed(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x00014874 File Offset: 0x00012A74
		public static void ArcGradientRadialDashed(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x000148CC File Offset: 0x00012ACC
		public static void ArcGradientRadialDashed(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x00014920 File Offset: 0x00012B20
		public static void ArcGradientRadialDashed(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x00014974 File Offset: 0x00012B74
		public static void ArcGradientRadialDashed(Vector3 pos, Vector3 normal, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, thickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x000149C4 File Offset: 0x00012BC4
		public static void ArcGradientRadialDashed(Vector3 pos, Vector3 normal, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, thickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x00014A18 File Offset: 0x00012C18
		public static void ArcGradientRadialDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x00014A6C File Offset: 0x00012C6C
		public static void ArcGradientRadialDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, endCaps, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x00014AC0 File Offset: 0x00012CC0
		public static void ArcGradientRadialDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x00014B10 File Offset: 0x00012D10
		public static void ArcGradientRadialDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, endCaps, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x00014B64 File Offset: 0x00012D64
		public static void ArcGradientRadialDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, thickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x00014BB4 File Offset: 0x00012DB4
		public static void ArcGradientRadialDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, thickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, endCaps, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x00014C04 File Offset: 0x00012E04
		public static void ArcGradientRadialDashed(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x00014C58 File Offset: 0x00012E58
		public static void ArcGradientRadialDashed(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x00014CAC File Offset: 0x00012EAC
		public static void ArcGradientRadialDashed(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x00014CFC File Offset: 0x00012EFC
		public static void ArcGradientRadialDashed(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x00014D4C File Offset: 0x00012F4C
		public static void ArcGradientRadialDashed(Vector3 pos, Quaternion rot, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, thickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x00014D98 File Offset: 0x00012F98
		public static void ArcGradientRadialDashed(Vector3 pos, Quaternion rot, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, thickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x00014DE4 File Offset: 0x00012FE4
		public static void ArcGradientRadialDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x00014E34 File Offset: 0x00013034
		public static void ArcGradientRadialDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, endCaps, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x00014E84 File Offset: 0x00013084
		public static void ArcGradientRadialDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x00014ED0 File Offset: 0x000130D0
		public static void ArcGradientRadialDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, endCaps, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x00014F1C File Offset: 0x0001311C
		public static void ArcGradientRadialDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, thickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x00014F64 File Offset: 0x00013164
		public static void ArcGradientRadialDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, thickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, endCaps, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x00014FB0 File Offset: 0x000131B0
		public static void ArcGradientRadialDashed(float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x00014FD8 File Offset: 0x000131D8
		public static void ArcGradientRadialDashed(float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x00015004 File Offset: 0x00013204
		public static void ArcGradientRadialDashed(float radius, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
			Draw.Arc_Internal(radius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x0001502C File Offset: 0x0001322C
		public static void ArcGradientRadialDashed(float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
			Draw.Arc_Internal(radius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x00015054 File Offset: 0x00013254
		public static void ArcGradientRadialDashed(float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
			Draw.Arc_Internal(radius, thickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x00015078 File Offset: 0x00013278
		public static void ArcGradientRadialDashed(float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
			Draw.Arc_Internal(radius, thickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x000150A0 File Offset: 0x000132A0
		public static void ArcGradientRadialDashed(DashStyle dashStyle, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x000150C8 File Offset: 0x000132C8
		public static void ArcGradientRadialDashed(DashStyle dashStyle, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, endCaps, dashStyle);
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x000150F0 File Offset: 0x000132F0
		public static void ArcGradientRadialDashed(DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
			Draw.Arc_Internal(radius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x00015114 File Offset: 0x00013314
		public static void ArcGradientRadialDashed(DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
			Draw.Arc_Internal(radius, Draw.RingThickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, endCaps, dashStyle);
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x0001513C File Offset: 0x0001333C
		public static void ArcGradientRadialDashed(DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorInner, Color colorOuter)
		{
			Draw.Arc_Internal(radius, thickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x00015160 File Offset: 0x00013360
		public static void ArcGradientRadialDashed(DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInner, Color colorOuter)
		{
			Draw.Arc_Internal(radius, thickness, colorInner, colorOuter, colorInner, colorOuter, angleRadStart, angleRadEnd, endCaps, dashStyle);
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x00015184 File Offset: 0x00013384
		public static void ArcGradientAngular(Vector3 pos, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, ArcEndCap.None, null);
			Draw.PopMatrix();
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x000151BC File Offset: 0x000133BC
		public static void ArcGradientAngular(Vector3 pos, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, endCaps, null);
			Draw.PopMatrix();
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x000151F4 File Offset: 0x000133F4
		public static void ArcGradientAngular(Vector3 pos, float radius, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, ArcEndCap.None, null);
			Draw.PopMatrix();
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x00015228 File Offset: 0x00013428
		public static void ArcGradientAngular(Vector3 pos, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, endCaps, null);
			Draw.PopMatrix();
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x00015260 File Offset: 0x00013460
		public static void ArcGradientAngular(Vector3 pos, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, thickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, ArcEndCap.None, null);
			Draw.PopMatrix();
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x00015294 File Offset: 0x00013494
		public static void ArcGradientAngular(Vector3 pos, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, thickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, endCaps, null);
			Draw.PopMatrix();
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x000152C8 File Offset: 0x000134C8
		public static void ArcGradientAngular(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, ArcEndCap.None, null);
			Draw.PopMatrix();
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x0001531C File Offset: 0x0001351C
		public static void ArcGradientAngular(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, endCaps, null);
			Draw.PopMatrix();
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x00015370 File Offset: 0x00013570
		public static void ArcGradientAngular(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, ArcEndCap.None, null);
			Draw.PopMatrix();
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x000153C0 File Offset: 0x000135C0
		public static void ArcGradientAngular(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, endCaps, null);
			Draw.PopMatrix();
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x00015410 File Offset: 0x00013610
		public static void ArcGradientAngular(Vector3 pos, Vector3 normal, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, thickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, ArcEndCap.None, null);
			Draw.PopMatrix();
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x0001545C File Offset: 0x0001365C
		public static void ArcGradientAngular(Vector3 pos, Vector3 normal, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, thickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, endCaps, null);
			Draw.PopMatrix();
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x000154AC File Offset: 0x000136AC
		public static void ArcGradientAngular(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, ArcEndCap.None, null);
			Draw.PopMatrix();
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x000154FC File Offset: 0x000136FC
		public static void ArcGradientAngular(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, endCaps, null);
			Draw.PopMatrix();
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x0001554C File Offset: 0x0001374C
		public static void ArcGradientAngular(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, ArcEndCap.None, null);
			Draw.PopMatrix();
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x00015598 File Offset: 0x00013798
		public static void ArcGradientAngular(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, endCaps, null);
			Draw.PopMatrix();
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x000155E4 File Offset: 0x000137E4
		public static void ArcGradientAngular(Vector3 pos, Quaternion rot, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, thickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, ArcEndCap.None, null);
			Draw.PopMatrix();
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x0001562C File Offset: 0x0001382C
		public static void ArcGradientAngular(Vector3 pos, Quaternion rot, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, thickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, endCaps, null);
			Draw.PopMatrix();
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x00015674 File Offset: 0x00013874
		public static void ArcGradientAngular(float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, ArcEndCap.None, null);
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x00015698 File Offset: 0x00013898
		public static void ArcGradientAngular(float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, endCaps, null);
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x000156C0 File Offset: 0x000138C0
		public static void ArcGradientAngular(float radius, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
			Draw.Arc_Internal(radius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, ArcEndCap.None, null);
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x000156E4 File Offset: 0x000138E4
		public static void ArcGradientAngular(float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
			Draw.Arc_Internal(radius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, endCaps, null);
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x00015708 File Offset: 0x00013908
		public static void ArcGradientAngular(float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
			Draw.Arc_Internal(radius, thickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, ArcEndCap.None, null);
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x00015728 File Offset: 0x00013928
		public static void ArcGradientAngular(float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
			Draw.Arc_Internal(radius, thickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, endCaps, null);
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x0001574C File Offset: 0x0001394C
		public static void ArcGradientAngularDashed(Vector3 pos, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x00015788 File Offset: 0x00013988
		public static void ArcGradientAngularDashed(Vector3 pos, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x000157C4 File Offset: 0x000139C4
		public static void ArcGradientAngularDashed(Vector3 pos, float radius, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x000157FC File Offset: 0x000139FC
		public static void ArcGradientAngularDashed(Vector3 pos, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x00015838 File Offset: 0x00013A38
		public static void ArcGradientAngularDashed(Vector3 pos, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, thickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x00015870 File Offset: 0x00013A70
		public static void ArcGradientAngularDashed(Vector3 pos, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, thickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x000158A8 File Offset: 0x00013AA8
		public static void ArcGradientAngularDashed(Vector3 pos, DashStyle dashStyle, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x000158E0 File Offset: 0x00013AE0
		public static void ArcGradientAngularDashed(Vector3 pos, DashStyle dashStyle, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, endCaps, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x0001591C File Offset: 0x00013B1C
		public static void ArcGradientAngularDashed(Vector3 pos, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x00015954 File Offset: 0x00013B54
		public static void ArcGradientAngularDashed(Vector3 pos, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, endCaps, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x0001598C File Offset: 0x00013B8C
		public static void ArcGradientAngularDashed(Vector3 pos, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, thickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x000159C0 File Offset: 0x00013BC0
		public static void ArcGradientAngularDashed(Vector3 pos, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, thickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, endCaps, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x000159F4 File Offset: 0x00013BF4
		public static void ArcGradientAngularDashed(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x00015A4C File Offset: 0x00013C4C
		public static void ArcGradientAngularDashed(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x00015AA4 File Offset: 0x00013CA4
		public static void ArcGradientAngularDashed(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x00015AF8 File Offset: 0x00013CF8
		public static void ArcGradientAngularDashed(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x00015B4C File Offset: 0x00013D4C
		public static void ArcGradientAngularDashed(Vector3 pos, Vector3 normal, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, thickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x00015B9C File Offset: 0x00013D9C
		public static void ArcGradientAngularDashed(Vector3 pos, Vector3 normal, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, thickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x00015BF0 File Offset: 0x00013DF0
		public static void ArcGradientAngularDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x00015C44 File Offset: 0x00013E44
		public static void ArcGradientAngularDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, endCaps, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x00015C98 File Offset: 0x00013E98
		public static void ArcGradientAngularDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x00015CE8 File Offset: 0x00013EE8
		public static void ArcGradientAngularDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, endCaps, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x00015D3C File Offset: 0x00013F3C
		public static void ArcGradientAngularDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, thickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x00015D8C File Offset: 0x00013F8C
		public static void ArcGradientAngularDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, thickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, endCaps, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x00015DDC File Offset: 0x00013FDC
		public static void ArcGradientAngularDashed(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x00015E30 File Offset: 0x00014030
		public static void ArcGradientAngularDashed(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x00015E84 File Offset: 0x00014084
		public static void ArcGradientAngularDashed(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x00015ED4 File Offset: 0x000140D4
		public static void ArcGradientAngularDashed(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x00015F24 File Offset: 0x00014124
		public static void ArcGradientAngularDashed(Vector3 pos, Quaternion rot, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, thickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x00015F70 File Offset: 0x00014170
		public static void ArcGradientAngularDashed(Vector3 pos, Quaternion rot, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, thickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x00015FBC File Offset: 0x000141BC
		public static void ArcGradientAngularDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x0001600C File Offset: 0x0001420C
		public static void ArcGradientAngularDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, endCaps, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x0001605C File Offset: 0x0001425C
		public static void ArcGradientAngularDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x000160A8 File Offset: 0x000142A8
		public static void ArcGradientAngularDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, endCaps, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x000160F4 File Offset: 0x000142F4
		public static void ArcGradientAngularDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, thickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x0001613C File Offset: 0x0001433C
		public static void ArcGradientAngularDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, thickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, endCaps, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x00016188 File Offset: 0x00014388
		public static void ArcGradientAngularDashed(float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x000161B0 File Offset: 0x000143B0
		public static void ArcGradientAngularDashed(float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x000161DC File Offset: 0x000143DC
		public static void ArcGradientAngularDashed(float radius, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
			Draw.Arc_Internal(radius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x00016204 File Offset: 0x00014404
		public static void ArcGradientAngularDashed(float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
			Draw.Arc_Internal(radius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x0001622C File Offset: 0x0001442C
		public static void ArcGradientAngularDashed(float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
			Draw.Arc_Internal(radius, thickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x00016250 File Offset: 0x00014450
		public static void ArcGradientAngularDashed(float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
			Draw.Arc_Internal(radius, thickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x00016278 File Offset: 0x00014478
		public static void ArcGradientAngularDashed(DashStyle dashStyle, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x000162A0 File Offset: 0x000144A0
		public static void ArcGradientAngularDashed(DashStyle dashStyle, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, endCaps, dashStyle);
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x000162C8 File Offset: 0x000144C8
		public static void ArcGradientAngularDashed(DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
			Draw.Arc_Internal(radius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x000162EC File Offset: 0x000144EC
		public static void ArcGradientAngularDashed(DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
			Draw.Arc_Internal(radius, Draw.RingThickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, endCaps, dashStyle);
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x00016314 File Offset: 0x00014514
		public static void ArcGradientAngularDashed(DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorStart, Color colorEnd)
		{
			Draw.Arc_Internal(radius, thickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x00016338 File Offset: 0x00014538
		public static void ArcGradientAngularDashed(DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorStart, Color colorEnd)
		{
			Draw.Arc_Internal(radius, thickness, colorStart, colorStart, colorEnd, colorEnd, angleRadStart, angleRadEnd, endCaps, dashStyle);
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x0001635C File Offset: 0x0001455C
		public static void ArcGradientBilinear(Vector3 pos, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, ArcEndCap.None, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x00016394 File Offset: 0x00014594
		public static void ArcGradientBilinear(Vector3 pos, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, endCaps, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x000163CC File Offset: 0x000145CC
		public static void ArcGradientBilinear(Vector3 pos, float radius, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, ArcEndCap.None, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x00016400 File Offset: 0x00014600
		public static void ArcGradientBilinear(Vector3 pos, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, endCaps, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x00016438 File Offset: 0x00014638
		public static void ArcGradientBilinear(Vector3 pos, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, thickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, ArcEndCap.None, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x0001646C File Offset: 0x0001466C
		public static void ArcGradientBilinear(Vector3 pos, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, thickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, endCaps, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x000164A0 File Offset: 0x000146A0
		public static void ArcGradientBilinear(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, ArcEndCap.None, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x000164F4 File Offset: 0x000146F4
		public static void ArcGradientBilinear(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, endCaps, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x00016548 File Offset: 0x00014748
		public static void ArcGradientBilinear(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, ArcEndCap.None, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x00016598 File Offset: 0x00014798
		public static void ArcGradientBilinear(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, endCaps, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x000165E8 File Offset: 0x000147E8
		public static void ArcGradientBilinear(Vector3 pos, Vector3 normal, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, thickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, ArcEndCap.None, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x00016634 File Offset: 0x00014834
		public static void ArcGradientBilinear(Vector3 pos, Vector3 normal, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, thickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, endCaps, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x00016684 File Offset: 0x00014884
		public static void ArcGradientBilinear(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, ArcEndCap.None, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x000166D4 File Offset: 0x000148D4
		public static void ArcGradientBilinear(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, endCaps, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x00016724 File Offset: 0x00014924
		public static void ArcGradientBilinear(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, ArcEndCap.None, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x00016770 File Offset: 0x00014970
		public static void ArcGradientBilinear(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, endCaps, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x000167BC File Offset: 0x000149BC
		public static void ArcGradientBilinear(Vector3 pos, Quaternion rot, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, thickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, ArcEndCap.None, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x00016804 File Offset: 0x00014A04
		public static void ArcGradientBilinear(Vector3 pos, Quaternion rot, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, thickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, endCaps, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x0001684C File Offset: 0x00014A4C
		public static void ArcGradientBilinear(float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, ArcEndCap.None, null);
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x00016874 File Offset: 0x00014A74
		public static void ArcGradientBilinear(float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, endCaps, null);
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x0001689C File Offset: 0x00014A9C
		public static void ArcGradientBilinear(float radius, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.Arc_Internal(radius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, ArcEndCap.None, null);
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x000168C0 File Offset: 0x00014AC0
		public static void ArcGradientBilinear(float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.Arc_Internal(radius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, endCaps, null);
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x000168E4 File Offset: 0x00014AE4
		public static void ArcGradientBilinear(float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.Arc_Internal(radius, thickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, ArcEndCap.None, null);
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x00016904 File Offset: 0x00014B04
		public static void ArcGradientBilinear(float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.Arc_Internal(radius, thickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, endCaps, null);
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x00016928 File Offset: 0x00014B28
		public static void ArcGradientBilinearDashed(Vector3 pos, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x00016964 File Offset: 0x00014B64
		public static void ArcGradientBilinearDashed(Vector3 pos, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x000169A0 File Offset: 0x00014BA0
		public static void ArcGradientBilinearDashed(Vector3 pos, float radius, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x000169D8 File Offset: 0x00014BD8
		public static void ArcGradientBilinearDashed(Vector3 pos, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x00016A14 File Offset: 0x00014C14
		public static void ArcGradientBilinearDashed(Vector3 pos, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, thickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x00016A4C File Offset: 0x00014C4C
		public static void ArcGradientBilinearDashed(Vector3 pos, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, thickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x00016A84 File Offset: 0x00014C84
		public static void ArcGradientBilinearDashed(Vector3 pos, DashStyle dashStyle, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x00016ABC File Offset: 0x00014CBC
		public static void ArcGradientBilinearDashed(Vector3 pos, DashStyle dashStyle, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, endCaps, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x00016AF8 File Offset: 0x00014CF8
		public static void ArcGradientBilinearDashed(Vector3 pos, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x00016B30 File Offset: 0x00014D30
		public static void ArcGradientBilinearDashed(Vector3 pos, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, endCaps, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x00016B68 File Offset: 0x00014D68
		public static void ArcGradientBilinearDashed(Vector3 pos, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, thickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x00016B9C File Offset: 0x00014D9C
		public static void ArcGradientBilinearDashed(Vector3 pos, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Arc_Internal(radius, thickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, endCaps, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x00016BD0 File Offset: 0x00014DD0
		public static void ArcGradientBilinearDashed(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x00016C28 File Offset: 0x00014E28
		public static void ArcGradientBilinearDashed(Vector3 pos, Vector3 normal, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x00016C80 File Offset: 0x00014E80
		public static void ArcGradientBilinearDashed(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x00016CD4 File Offset: 0x00014ED4
		public static void ArcGradientBilinearDashed(Vector3 pos, Vector3 normal, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x00016D28 File Offset: 0x00014F28
		public static void ArcGradientBilinearDashed(Vector3 pos, Vector3 normal, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, thickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x00016D78 File Offset: 0x00014F78
		public static void ArcGradientBilinearDashed(Vector3 pos, Vector3 normal, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, thickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x00016DCC File Offset: 0x00014FCC
		public static void ArcGradientBilinearDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x00016E20 File Offset: 0x00015020
		public static void ArcGradientBilinearDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, endCaps, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x00016E74 File Offset: 0x00015074
		public static void ArcGradientBilinearDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x00016EC4 File Offset: 0x000150C4
		public static void ArcGradientBilinearDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, endCaps, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x00016F18 File Offset: 0x00015118
		public static void ArcGradientBilinearDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, thickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x00016F68 File Offset: 0x00015168
		public static void ArcGradientBilinearDashed(Vector3 pos, Vector3 normal, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Arc_Internal(radius, thickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, endCaps, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x00016FB8 File Offset: 0x000151B8
		public static void ArcGradientBilinearDashed(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x0001700C File Offset: 0x0001520C
		public static void ArcGradientBilinearDashed(Vector3 pos, Quaternion rot, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x00017060 File Offset: 0x00015260
		public static void ArcGradientBilinearDashed(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x000170B0 File Offset: 0x000152B0
		public static void ArcGradientBilinearDashed(Vector3 pos, Quaternion rot, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x00017100 File Offset: 0x00015300
		public static void ArcGradientBilinearDashed(Vector3 pos, Quaternion rot, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, thickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x0001714C File Offset: 0x0001534C
		public static void ArcGradientBilinearDashed(Vector3 pos, Quaternion rot, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, thickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x00017198 File Offset: 0x00015398
		public static void ArcGradientBilinearDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x000171E8 File Offset: 0x000153E8
		public static void ArcGradientBilinearDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, endCaps, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x00017238 File Offset: 0x00015438
		public static void ArcGradientBilinearDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x00017284 File Offset: 0x00015484
		public static void ArcGradientBilinearDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, endCaps, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x000172D0 File Offset: 0x000154D0
		public static void ArcGradientBilinearDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, thickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x00017318 File Offset: 0x00015518
		public static void ArcGradientBilinearDashed(Vector3 pos, Quaternion rot, DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Arc_Internal(radius, thickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, endCaps, dashStyle);
			Draw.PopMatrix();
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x00017364 File Offset: 0x00015564
		public static void ArcGradientBilinearDashed(float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x00017390 File Offset: 0x00015590
		public static void ArcGradientBilinearDashed(float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x000173BC File Offset: 0x000155BC
		public static void ArcGradientBilinearDashed(float radius, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.Arc_Internal(radius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x000173E4 File Offset: 0x000155E4
		public static void ArcGradientBilinearDashed(float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.Arc_Internal(radius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x0001740C File Offset: 0x0001560C
		public static void ArcGradientBilinearDashed(float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.Arc_Internal(radius, thickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, ArcEndCap.None, Draw.RingDashStyle);
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x00017430 File Offset: 0x00015630
		public static void ArcGradientBilinearDashed(float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.Arc_Internal(radius, thickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, endCaps, Draw.RingDashStyle);
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x00017458 File Offset: 0x00015658
		public static void ArcGradientBilinearDashed(DashStyle dashStyle, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x00017480 File Offset: 0x00015680
		public static void ArcGradientBilinearDashed(DashStyle dashStyle, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.Arc_Internal(Draw.DiscRadius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, endCaps, dashStyle);
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x000174A8 File Offset: 0x000156A8
		public static void ArcGradientBilinearDashed(DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.Arc_Internal(radius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x000174CC File Offset: 0x000156CC
		public static void ArcGradientBilinearDashed(DashStyle dashStyle, float radius, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.Arc_Internal(radius, Draw.RingThickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, endCaps, dashStyle);
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x000174F4 File Offset: 0x000156F4
		public static void ArcGradientBilinearDashed(DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.Arc_Internal(radius, thickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, ArcEndCap.None, dashStyle);
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x00017518 File Offset: 0x00015718
		public static void ArcGradientBilinearDashed(DashStyle dashStyle, float radius, float thickness, float angleRadStart, float angleRadEnd, ArcEndCap endCaps, Color colorInnerStart, Color colorOuterStart, Color colorInnerEnd, Color colorOuterEnd)
		{
			Draw.Arc_Internal(radius, thickness, colorInnerStart, colorOuterStart, colorInnerEnd, colorOuterEnd, angleRadStart, angleRadEnd, endCaps, dashStyle);
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x0001753C File Offset: 0x0001573C
		public static void Rectangle(Vector3 pos, Rect rect)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, Draw.Color, Draw.RectangleThickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x0001757C File Offset: 0x0001577C
		public static void Rectangle(Vector3 pos, Rect rect, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, color, Draw.RectangleThickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x000175B8 File Offset: 0x000157B8
		public static void Rectangle(Vector3 pos, Rect rect, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, Draw.Color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x000175F8 File Offset: 0x000157F8
		public static void Rectangle(Vector3 pos, Rect rect, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x00017631 File Offset: 0x00015831
		public static void Rectangle(Vector3 pos, Rect rect, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, Draw.Color, Draw.RectangleThickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x0001765B File Offset: 0x0001585B
		public static void Rectangle(Vector3 pos, Rect rect, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, color, Draw.RectangleThickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x00017684 File Offset: 0x00015884
		public static void Rectangle(Vector3 pos, Vector3 normal, Rect rect)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, Draw.Color, Draw.RectangleThickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x000176DC File Offset: 0x000158DC
		public static void Rectangle(Vector3 pos, Vector3 normal, Rect rect, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, color, Draw.RectangleThickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x00017730 File Offset: 0x00015930
		public static void Rectangle(Vector3 pos, Vector3 normal, Rect rect, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, Draw.Color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x00017788 File Offset: 0x00015988
		public static void Rectangle(Vector3 pos, Vector3 normal, Rect rect, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x000177DC File Offset: 0x000159DC
		public static void Rectangle(Vector3 pos, Vector3 normal, Rect rect, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, Draw.Color, Draw.RectangleThickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x0001782C File Offset: 0x00015A2C
		public static void Rectangle(Vector3 pos, Vector3 normal, Rect rect, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, color, Draw.RectangleThickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x00017878 File Offset: 0x00015A78
		public static void Rectangle(Vector3 pos, Quaternion rot, Rect rect)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, Draw.Color, Draw.RectangleThickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x000178CC File Offset: 0x00015ACC
		public static void Rectangle(Vector3 pos, Quaternion rot, Rect rect, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, color, Draw.RectangleThickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x0001791C File Offset: 0x00015B1C
		public static void Rectangle(Vector3 pos, Quaternion rot, Rect rect, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, Draw.Color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x00017970 File Offset: 0x00015B70
		public static void Rectangle(Vector3 pos, Quaternion rot, Rect rect, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x000179BF File Offset: 0x00015BBF
		public static void Rectangle(Vector3 pos, Quaternion rot, Rect rect, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, Draw.Color, Draw.RectangleThickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x000179FE File Offset: 0x00015BFE
		public static void Rectangle(Vector3 pos, Quaternion rot, Rect rect, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, color, Draw.RectangleThickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x00017A3C File Offset: 0x00015C3C
		public static void Rectangle(Vector3 pos, Vector2 size)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), Draw.Color, Draw.RectangleThickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x00017A80 File Offset: 0x00015C80
		public static void Rectangle(Vector3 pos, Vector2 size, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), color, Draw.RectangleThickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x00017AC0 File Offset: 0x00015CC0
		public static void Rectangle(Vector3 pos, Vector2 size, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), Draw.Color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x00017B04 File Offset: 0x00015D04
		public static void Rectangle(Vector3 pos, Vector2 size, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x00017B43 File Offset: 0x00015D43
		public static void Rectangle(Vector3 pos, Vector2 size, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), Draw.Color, Draw.RectangleThickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x00017B73 File Offset: 0x00015D73
		public static void Rectangle(Vector3 pos, Vector2 size, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), color, Draw.RectangleThickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x00017BA0 File Offset: 0x00015DA0
		public static void Rectangle(Vector3 pos, float width, float height)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), Draw.Color, Draw.RectangleThickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x00017BE4 File Offset: 0x00015DE4
		public static void Rectangle(Vector3 pos, float width, float height, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), color, Draw.RectangleThickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x00017C24 File Offset: 0x00015E24
		public static void Rectangle(Vector3 pos, float width, float height, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), Draw.Color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x00017C68 File Offset: 0x00015E68
		public static void Rectangle(Vector3 pos, float width, float height, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x00017CA9 File Offset: 0x00015EA9
		public static void Rectangle(Vector3 pos, float width, float height, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), Draw.Color, Draw.RectangleThickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x00017CDA File Offset: 0x00015EDA
		public static void Rectangle(Vector3 pos, float width, float height, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), color, Draw.RectangleThickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x00017D08 File Offset: 0x00015F08
		public static void Rectangle(Vector3 pos, Vector3 normal, Vector2 size)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), Draw.Color, Draw.RectangleThickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x00017D68 File Offset: 0x00015F68
		public static void Rectangle(Vector3 pos, Vector3 normal, Vector2 size, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), color, Draw.RectangleThickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x00017DC4 File Offset: 0x00015FC4
		public static void Rectangle(Vector3 pos, Vector3 normal, Vector2 size, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), Draw.Color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x00017E24 File Offset: 0x00016024
		public static void Rectangle(Vector3 pos, Vector3 normal, Vector2 size, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x00017E80 File Offset: 0x00016080
		public static void Rectangle(Vector3 pos, Vector3 normal, Vector2 size, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), Draw.Color, Draw.RectangleThickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x00017ED8 File Offset: 0x000160D8
		public static void Rectangle(Vector3 pos, Vector3 normal, Vector2 size, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), color, Draw.RectangleThickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x00017F2C File Offset: 0x0001612C
		public static void Rectangle(Vector3 pos, Vector3 normal, float width, float height)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), Draw.Color, Draw.RectangleThickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x00017F8C File Offset: 0x0001618C
		public static void Rectangle(Vector3 pos, Vector3 normal, float width, float height, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), color, Draw.RectangleThickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x00017FE8 File Offset: 0x000161E8
		public static void Rectangle(Vector3 pos, Vector3 normal, float width, float height, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), Draw.Color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x0001804C File Offset: 0x0001624C
		public static void Rectangle(Vector3 pos, Vector3 normal, float width, float height, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x000180AC File Offset: 0x000162AC
		public static void Rectangle(Vector3 pos, Vector3 normal, float width, float height, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), Draw.Color, Draw.RectangleThickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x00018104 File Offset: 0x00016304
		public static void Rectangle(Vector3 pos, Vector3 normal, float width, float height, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), color, Draw.RectangleThickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x00018158 File Offset: 0x00016358
		public static void Rectangle(Vector3 pos, Quaternion rot, Vector2 size)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), Draw.Color, Draw.RectangleThickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x000181B0 File Offset: 0x000163B0
		public static void Rectangle(Vector3 pos, Quaternion rot, Vector2 size, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), color, Draw.RectangleThickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x00018204 File Offset: 0x00016404
		public static void Rectangle(Vector3 pos, Quaternion rot, Vector2 size, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), Draw.Color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x0001825C File Offset: 0x0001645C
		public static void Rectangle(Vector3 pos, Quaternion rot, Vector2 size, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x000182B4 File Offset: 0x000164B4
		public static void Rectangle(Vector3 pos, Quaternion rot, Vector2 size, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), Draw.Color, Draw.RectangleThickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x00018304 File Offset: 0x00016504
		public static void Rectangle(Vector3 pos, Quaternion rot, Vector2 size, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), color, Draw.RectangleThickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x00018354 File Offset: 0x00016554
		public static void Rectangle(Vector3 pos, Quaternion rot, float width, float height)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), Draw.Color, Draw.RectangleThickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x000183B0 File Offset: 0x000165B0
		public static void Rectangle(Vector3 pos, Quaternion rot, float width, float height, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), color, Draw.RectangleThickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x00018408 File Offset: 0x00016608
		public static void Rectangle(Vector3 pos, Quaternion rot, float width, float height, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), Draw.Color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x00018468 File Offset: 0x00016668
		public static void Rectangle(Vector3 pos, Quaternion rot, float width, float height, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x000184C4 File Offset: 0x000166C4
		public static void Rectangle(Vector3 pos, Quaternion rot, float width, float height, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), Draw.Color, Draw.RectangleThickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x00018518 File Offset: 0x00016718
		public static void Rectangle(Vector3 pos, Quaternion rot, float width, float height, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), color, Draw.RectangleThickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x00018568 File Offset: 0x00016768
		public static void Rectangle(Rect rect)
		{
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, Draw.Color, Draw.RectangleThickness, default(Vector4), null);
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x00018598 File Offset: 0x00016798
		public static void Rectangle(Rect rect, Color color)
		{
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, color, Draw.RectangleThickness, default(Vector4), null);
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x000185C4 File Offset: 0x000167C4
		public static void Rectangle(Rect rect, float cornerRadius)
		{
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, Draw.Color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x000185F4 File Offset: 0x000167F4
		public static void Rectangle(Rect rect, float cornerRadius, Color color)
		{
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x0001861D File Offset: 0x0001681D
		public static void Rectangle(Rect rect, Vector4 cornerRadii)
		{
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, Draw.Color, Draw.RectangleThickness, cornerRadii, null);
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x00018637 File Offset: 0x00016837
		public static void Rectangle(Rect rect, Vector4 cornerRadii, Color color)
		{
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, color, Draw.RectangleThickness, cornerRadii, null);
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x00018650 File Offset: 0x00016850
		public static void Rectangle(Vector3 pos, Vector2 size, RectPivot pivot)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), Draw.Color, Draw.RectangleThickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x00018694 File Offset: 0x00016894
		public static void Rectangle(Vector3 pos, Vector2 size, RectPivot pivot, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), color, Draw.RectangleThickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x000186D4 File Offset: 0x000168D4
		public static void Rectangle(Vector3 pos, Vector2 size, RectPivot pivot, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), Draw.Color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x00018718 File Offset: 0x00016918
		public static void Rectangle(Vector3 pos, Vector2 size, RectPivot pivot, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x00018758 File Offset: 0x00016958
		public static void Rectangle(Vector3 pos, Vector2 size, RectPivot pivot, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), Draw.Color, Draw.RectangleThickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x00018788 File Offset: 0x00016988
		public static void Rectangle(Vector3 pos, Vector2 size, RectPivot pivot, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), color, Draw.RectangleThickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x000187B8 File Offset: 0x000169B8
		public static void Rectangle(Vector3 pos, float width, float height, RectPivot pivot)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), Draw.Color, Draw.RectangleThickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x000187FC File Offset: 0x000169FC
		public static void Rectangle(Vector3 pos, float width, float height, RectPivot pivot, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), color, Draw.RectangleThickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x00018840 File Offset: 0x00016A40
		public static void Rectangle(Vector3 pos, float width, float height, RectPivot pivot, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), Draw.Color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x00018888 File Offset: 0x00016A88
		public static void Rectangle(Vector3 pos, float width, float height, RectPivot pivot, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x000188CD File Offset: 0x00016ACD
		public static void Rectangle(Vector3 pos, float width, float height, RectPivot pivot, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), Draw.Color, Draw.RectangleThickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x000188FF File Offset: 0x00016AFF
		public static void Rectangle(Vector3 pos, float width, float height, RectPivot pivot, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), color, Draw.RectangleThickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x00018930 File Offset: 0x00016B30
		public static void Rectangle(Vector3 pos, Vector3 normal, Vector2 size, RectPivot pivot)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), Draw.Color, Draw.RectangleThickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x00018990 File Offset: 0x00016B90
		public static void Rectangle(Vector3 pos, Vector3 normal, Vector2 size, RectPivot pivot, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), color, Draw.RectangleThickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x000189EC File Offset: 0x00016BEC
		public static void Rectangle(Vector3 pos, Vector3 normal, Vector2 size, RectPivot pivot, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), Draw.Color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x00018A50 File Offset: 0x00016C50
		public static void Rectangle(Vector3 pos, Vector3 normal, Vector2 size, RectPivot pivot, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x00018AB0 File Offset: 0x00016CB0
		public static void Rectangle(Vector3 pos, Vector3 normal, Vector2 size, RectPivot pivot, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), Draw.Color, Draw.RectangleThickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x00018B08 File Offset: 0x00016D08
		public static void Rectangle(Vector3 pos, Vector3 normal, Vector2 size, RectPivot pivot, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), color, Draw.RectangleThickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x00018B5C File Offset: 0x00016D5C
		public static void Rectangle(Vector3 pos, Vector3 normal, float width, float height, RectPivot pivot)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), Draw.Color, Draw.RectangleThickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x00018BBC File Offset: 0x00016DBC
		public static void Rectangle(Vector3 pos, Vector3 normal, float width, float height, RectPivot pivot, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), color, Draw.RectangleThickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x00018C18 File Offset: 0x00016E18
		public static void Rectangle(Vector3 pos, Vector3 normal, float width, float height, RectPivot pivot, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), Draw.Color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x00018C7C File Offset: 0x00016E7C
		public static void Rectangle(Vector3 pos, Vector3 normal, float width, float height, RectPivot pivot, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x00018CDC File Offset: 0x00016EDC
		public static void Rectangle(Vector3 pos, Vector3 normal, float width, float height, RectPivot pivot, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), Draw.Color, Draw.RectangleThickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x00018D34 File Offset: 0x00016F34
		public static void Rectangle(Vector3 pos, Vector3 normal, float width, float height, RectPivot pivot, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), color, Draw.RectangleThickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x00018D8C File Offset: 0x00016F8C
		public static void Rectangle(Vector3 pos, Quaternion rot, Vector2 size, RectPivot pivot)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), Draw.Color, Draw.RectangleThickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x00018DE4 File Offset: 0x00016FE4
		public static void Rectangle(Vector3 pos, Quaternion rot, Vector2 size, RectPivot pivot, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), color, Draw.RectangleThickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x00018E3C File Offset: 0x0001703C
		public static void Rectangle(Vector3 pos, Quaternion rot, Vector2 size, RectPivot pivot, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), Draw.Color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x00018E98 File Offset: 0x00017098
		public static void Rectangle(Vector3 pos, Quaternion rot, Vector2 size, RectPivot pivot, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x00018EF4 File Offset: 0x000170F4
		public static void Rectangle(Vector3 pos, Quaternion rot, Vector2 size, RectPivot pivot, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), Draw.Color, Draw.RectangleThickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x00018F48 File Offset: 0x00017148
		public static void Rectangle(Vector3 pos, Quaternion rot, Vector2 size, RectPivot pivot, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), color, Draw.RectangleThickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x00018F98 File Offset: 0x00017198
		public static void Rectangle(Vector3 pos, Quaternion rot, float width, float height, RectPivot pivot)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), Draw.Color, Draw.RectangleThickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x00018FF4 File Offset: 0x000171F4
		public static void Rectangle(Vector3 pos, Quaternion rot, float width, float height, RectPivot pivot, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), color, Draw.RectangleThickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x0001904C File Offset: 0x0001724C
		public static void Rectangle(Vector3 pos, Quaternion rot, float width, float height, RectPivot pivot, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), Draw.Color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x000190AC File Offset: 0x000172AC
		public static void Rectangle(Vector3 pos, Quaternion rot, float width, float height, RectPivot pivot, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x00019108 File Offset: 0x00017308
		public static void Rectangle(Vector3 pos, Quaternion rot, float width, float height, RectPivot pivot, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), Draw.Color, Draw.RectangleThickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x0001915C File Offset: 0x0001735C
		public static void Rectangle(Vector3 pos, Quaternion rot, float width, float height, RectPivot pivot, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), color, Draw.RectangleThickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x000191AC File Offset: 0x000173AC
		public static void RectangleBorder(Vector3 pos, Rect rect, float thickness)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, Draw.Color, thickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x000191E8 File Offset: 0x000173E8
		public static void RectangleBorder(Vector3 pos, Rect rect, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, color, thickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x00019220 File Offset: 0x00017420
		public static void RectangleBorder(Vector3 pos, Rect rect, float thickness, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x0001925C File Offset: 0x0001745C
		public static void RectangleBorder(Vector3 pos, Rect rect, float thickness, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x00019292 File Offset: 0x00017492
		public static void RectangleBorder(Vector3 pos, Rect rect, float thickness, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, Draw.Color, thickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x000192B8 File Offset: 0x000174B8
		public static void RectangleBorder(Vector3 pos, Rect rect, float thickness, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, color, thickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x000192DC File Offset: 0x000174DC
		public static void RectangleBorder(Vector3 pos, Vector3 normal, Rect rect, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, Draw.Color, thickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x00019330 File Offset: 0x00017530
		public static void RectangleBorder(Vector3 pos, Vector3 normal, Rect rect, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, color, thickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x00019380 File Offset: 0x00017580
		public static void RectangleBorder(Vector3 pos, Vector3 normal, Rect rect, float thickness, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x000193D8 File Offset: 0x000175D8
		public static void RectangleBorder(Vector3 pos, Vector3 normal, Rect rect, float thickness, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x0001942C File Offset: 0x0001762C
		public static void RectangleBorder(Vector3 pos, Vector3 normal, Rect rect, float thickness, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, Draw.Color, thickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x00019478 File Offset: 0x00017678
		public static void RectangleBorder(Vector3 pos, Vector3 normal, Rect rect, float thickness, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, color, thickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x000194B8 File Offset: 0x000176B8
		public static void RectangleBorder(Vector3 pos, Quaternion rot, Rect rect, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, Draw.Color, thickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x00019508 File Offset: 0x00017708
		public static void RectangleBorder(Vector3 pos, Quaternion rot, Rect rect, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, color, thickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x00019554 File Offset: 0x00017754
		public static void RectangleBorder(Vector3 pos, Quaternion rot, Rect rect, float thickness, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x000195A8 File Offset: 0x000177A8
		public static void RectangleBorder(Vector3 pos, Quaternion rot, Rect rect, float thickness, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x000195F7 File Offset: 0x000177F7
		public static void RectangleBorder(Vector3 pos, Quaternion rot, Rect rect, float thickness, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, Draw.Color, thickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x00019633 File Offset: 0x00017833
		public static void RectangleBorder(Vector3 pos, Quaternion rot, Rect rect, float thickness, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, color, thickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x0001966C File Offset: 0x0001786C
		public static void RectangleBorder(Vector3 pos, Vector2 size, float thickness)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), Draw.Color, thickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x000196AC File Offset: 0x000178AC
		public static void RectangleBorder(Vector3 pos, Vector2 size, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), color, thickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x000196E8 File Offset: 0x000178E8
		public static void RectangleBorder(Vector3 pos, Vector2 size, float thickness, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x00019728 File Offset: 0x00017928
		public static void RectangleBorder(Vector3 pos, Vector2 size, float thickness, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x00019764 File Offset: 0x00017964
		public static void RectangleBorder(Vector3 pos, Vector2 size, float thickness, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), Draw.Color, thickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x00019790 File Offset: 0x00017990
		public static void RectangleBorder(Vector3 pos, Vector2 size, float thickness, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), color, thickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x000197BC File Offset: 0x000179BC
		public static void RectangleBorder(Vector3 pos, float width, float height, float thickness)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), Draw.Color, thickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x000197FC File Offset: 0x000179FC
		public static void RectangleBorder(Vector3 pos, float width, float height, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), color, thickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x0001983C File Offset: 0x00017A3C
		public static void RectangleBorder(Vector3 pos, float width, float height, float thickness, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x00019880 File Offset: 0x00017A80
		public static void RectangleBorder(Vector3 pos, float width, float height, float thickness, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x000198C1 File Offset: 0x00017AC1
		public static void RectangleBorder(Vector3 pos, float width, float height, float thickness, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), Draw.Color, thickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x000198EF File Offset: 0x00017AEF
		public static void RectangleBorder(Vector3 pos, float width, float height, float thickness, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), color, thickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x0001991C File Offset: 0x00017B1C
		public static void RectangleBorder(Vector3 pos, Vector3 normal, Vector2 size, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), Draw.Color, thickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x00019978 File Offset: 0x00017B78
		public static void RectangleBorder(Vector3 pos, Vector3 normal, Vector2 size, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), color, thickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x000199D0 File Offset: 0x00017BD0
		public static void RectangleBorder(Vector3 pos, Vector3 normal, Vector2 size, float thickness, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x00019A30 File Offset: 0x00017C30
		public static void RectangleBorder(Vector3 pos, Vector3 normal, Vector2 size, float thickness, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x00019A8C File Offset: 0x00017C8C
		public static void RectangleBorder(Vector3 pos, Vector3 normal, Vector2 size, float thickness, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), Draw.Color, thickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x00019AE0 File Offset: 0x00017CE0
		public static void RectangleBorder(Vector3 pos, Vector3 normal, Vector2 size, float thickness, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), color, thickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x00019B30 File Offset: 0x00017D30
		public static void RectangleBorder(Vector3 pos, Vector3 normal, float width, float height, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), Draw.Color, thickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x00019B8C File Offset: 0x00017D8C
		public static void RectangleBorder(Vector3 pos, Vector3 normal, float width, float height, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), color, thickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x00019BE4 File Offset: 0x00017DE4
		public static void RectangleBorder(Vector3 pos, Vector3 normal, float width, float height, float thickness, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x00019C44 File Offset: 0x00017E44
		public static void RectangleBorder(Vector3 pos, Vector3 normal, float width, float height, float thickness, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x00019CA0 File Offset: 0x00017EA0
		public static void RectangleBorder(Vector3 pos, Vector3 normal, float width, float height, float thickness, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), Draw.Color, thickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x00019CF4 File Offset: 0x00017EF4
		public static void RectangleBorder(Vector3 pos, Vector3 normal, float width, float height, float thickness, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), color, thickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x00019D48 File Offset: 0x00017F48
		public static void RectangleBorder(Vector3 pos, Quaternion rot, Vector2 size, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), Draw.Color, thickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x00019D9C File Offset: 0x00017F9C
		public static void RectangleBorder(Vector3 pos, Quaternion rot, Vector2 size, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), color, thickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x00019DF0 File Offset: 0x00017FF0
		public static void RectangleBorder(Vector3 pos, Quaternion rot, Vector2 size, float thickness, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x00019E48 File Offset: 0x00018048
		public static void RectangleBorder(Vector3 pos, Quaternion rot, Vector2 size, float thickness, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x00019EA0 File Offset: 0x000180A0
		public static void RectangleBorder(Vector3 pos, Quaternion rot, Vector2 size, float thickness, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), Draw.Color, thickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x00019EED File Offset: 0x000180ED
		public static void RectangleBorder(Vector3 pos, Quaternion rot, Vector2 size, float thickness, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), color, thickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x00019F2C File Offset: 0x0001812C
		public static void RectangleBorder(Vector3 pos, Quaternion rot, float width, float height, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), Draw.Color, thickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x00019F84 File Offset: 0x00018184
		public static void RectangleBorder(Vector3 pos, Quaternion rot, float width, float height, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), color, thickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x00019FD8 File Offset: 0x000181D8
		public static void RectangleBorder(Vector3 pos, Quaternion rot, float width, float height, float thickness, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x0001A034 File Offset: 0x00018234
		public static void RectangleBorder(Vector3 pos, Quaternion rot, float width, float height, float thickness, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x0001A08C File Offset: 0x0001828C
		public static void RectangleBorder(Vector3 pos, Quaternion rot, float width, float height, float thickness, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), Draw.Color, thickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x0001A0DC File Offset: 0x000182DC
		public static void RectangleBorder(Vector3 pos, Quaternion rot, float width, float height, float thickness, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), color, thickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x0001A128 File Offset: 0x00018328
		public static void RectangleBorder(Rect rect, float thickness)
		{
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, Draw.Color, thickness, default(Vector4), null);
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x0001A154 File Offset: 0x00018354
		public static void RectangleBorder(Rect rect, float thickness, Color color)
		{
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, color, thickness, default(Vector4), null);
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x0001A17C File Offset: 0x0001837C
		public static void RectangleBorder(Rect rect, float thickness, float cornerRadius)
		{
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x0001A1A8 File Offset: 0x000183A8
		public static void RectangleBorder(Rect rect, float thickness, float cornerRadius, Color color)
		{
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x0001A1CD File Offset: 0x000183CD
		public static void RectangleBorder(Rect rect, float thickness, Vector4 cornerRadii)
		{
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, Draw.Color, thickness, cornerRadii, null);
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x0001A1E3 File Offset: 0x000183E3
		public static void RectangleBorder(Rect rect, float thickness, Vector4 cornerRadii, Color color)
		{
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, color, thickness, cornerRadii, null);
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x0001A1F8 File Offset: 0x000183F8
		public static void RectangleBorder(Vector3 pos, Vector2 size, RectPivot pivot, float thickness)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), Draw.Color, thickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x0001A238 File Offset: 0x00018438
		public static void RectangleBorder(Vector3 pos, Vector2 size, RectPivot pivot, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), color, thickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x0001A274 File Offset: 0x00018474
		public static void RectangleBorder(Vector3 pos, Vector2 size, RectPivot pivot, float thickness, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x0001A2B8 File Offset: 0x000184B8
		public static void RectangleBorder(Vector3 pos, Vector2 size, RectPivot pivot, float thickness, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x0001A2F8 File Offset: 0x000184F8
		public static void RectangleBorder(Vector3 pos, Vector2 size, RectPivot pivot, float thickness, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), Draw.Color, thickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x0001A325 File Offset: 0x00018525
		public static void RectangleBorder(Vector3 pos, Vector2 size, RectPivot pivot, float thickness, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), color, thickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x0001A350 File Offset: 0x00018550
		public static void RectangleBorder(Vector3 pos, float width, float height, RectPivot pivot, float thickness)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), Draw.Color, thickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x0001A394 File Offset: 0x00018594
		public static void RectangleBorder(Vector3 pos, float width, float height, RectPivot pivot, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), color, thickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x0001A3D4 File Offset: 0x000185D4
		public static void RectangleBorder(Vector3 pos, float width, float height, RectPivot pivot, float thickness, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x0001A41C File Offset: 0x0001861C
		public static void RectangleBorder(Vector3 pos, float width, float height, RectPivot pivot, float thickness, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x0001A45E File Offset: 0x0001865E
		public static void RectangleBorder(Vector3 pos, float width, float height, RectPivot pivot, float thickness, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), Draw.Color, thickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x0001A48D File Offset: 0x0001868D
		public static void RectangleBorder(Vector3 pos, float width, float height, RectPivot pivot, float thickness, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), color, thickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x0001A4BC File Offset: 0x000186BC
		public static void RectangleBorder(Vector3 pos, Vector3 normal, Vector2 size, RectPivot pivot, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), Draw.Color, thickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x0001A518 File Offset: 0x00018718
		public static void RectangleBorder(Vector3 pos, Vector3 normal, Vector2 size, RectPivot pivot, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), color, thickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x0001A570 File Offset: 0x00018770
		public static void RectangleBorder(Vector3 pos, Vector3 normal, Vector2 size, RectPivot pivot, float thickness, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x0001A5D0 File Offset: 0x000187D0
		public static void RectangleBorder(Vector3 pos, Vector3 normal, Vector2 size, RectPivot pivot, float thickness, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x0001A62C File Offset: 0x0001882C
		public static void RectangleBorder(Vector3 pos, Vector3 normal, Vector2 size, RectPivot pivot, float thickness, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), Draw.Color, thickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x0001A680 File Offset: 0x00018880
		public static void RectangleBorder(Vector3 pos, Vector3 normal, Vector2 size, RectPivot pivot, float thickness, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), color, thickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x0001A6D0 File Offset: 0x000188D0
		public static void RectangleBorder(Vector3 pos, Vector3 normal, float width, float height, RectPivot pivot, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), Draw.Color, thickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x0001A72C File Offset: 0x0001892C
		public static void RectangleBorder(Vector3 pos, Vector3 normal, float width, float height, RectPivot pivot, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), color, thickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x0001A788 File Offset: 0x00018988
		public static void RectangleBorder(Vector3 pos, Vector3 normal, float width, float height, RectPivot pivot, float thickness, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x0001A7E8 File Offset: 0x000189E8
		public static void RectangleBorder(Vector3 pos, Vector3 normal, float width, float height, RectPivot pivot, float thickness, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x0001A848 File Offset: 0x00018A48
		public static void RectangleBorder(Vector3 pos, Vector3 normal, float width, float height, RectPivot pivot, float thickness, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), Draw.Color, thickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x0001A8A0 File Offset: 0x00018AA0
		public static void RectangleBorder(Vector3 pos, Vector3 normal, float width, float height, RectPivot pivot, float thickness, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), color, thickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x0001A8F4 File Offset: 0x00018AF4
		public static void RectangleBorder(Vector3 pos, Quaternion rot, Vector2 size, RectPivot pivot, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), Draw.Color, thickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x0001A94C File Offset: 0x00018B4C
		public static void RectangleBorder(Vector3 pos, Quaternion rot, Vector2 size, RectPivot pivot, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), color, thickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x0001A9A0 File Offset: 0x00018BA0
		public static void RectangleBorder(Vector3 pos, Quaternion rot, Vector2 size, RectPivot pivot, float thickness, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x0001A9FC File Offset: 0x00018BFC
		public static void RectangleBorder(Vector3 pos, Quaternion rot, Vector2 size, RectPivot pivot, float thickness, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x0001AA54 File Offset: 0x00018C54
		public static void RectangleBorder(Vector3 pos, Quaternion rot, Vector2 size, RectPivot pivot, float thickness, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), Draw.Color, thickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x0001AAA2 File Offset: 0x00018CA2
		public static void RectangleBorder(Vector3 pos, Quaternion rot, Vector2 size, RectPivot pivot, float thickness, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), color, thickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x0001AAE4 File Offset: 0x00018CE4
		public static void RectangleBorder(Vector3 pos, Quaternion rot, float width, float height, RectPivot pivot, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), Draw.Color, thickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x0001AB3C File Offset: 0x00018D3C
		public static void RectangleBorder(Vector3 pos, Quaternion rot, float width, float height, RectPivot pivot, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), color, thickness, default(Vector4), null);
			Draw.PopMatrix();
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x0001AB90 File Offset: 0x00018D90
		public static void RectangleBorder(Vector3 pos, Quaternion rot, float width, float height, RectPivot pivot, float thickness, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x0001ABEC File Offset: 0x00018DEC
		public static void RectangleBorder(Vector3 pos, Quaternion rot, float width, float height, RectPivot pivot, float thickness, float cornerRadius, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), null);
			Draw.PopMatrix();
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x0001AC44 File Offset: 0x00018E44
		public static void RectangleBorder(Vector3 pos, Quaternion rot, float width, float height, RectPivot pivot, float thickness, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), Draw.Color, thickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x0001AC94 File Offset: 0x00018E94
		public static void RectangleBorder(Vector3 pos, Quaternion rot, float width, float height, RectPivot pivot, float thickness, Vector4 cornerRadii, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), color, thickness, cornerRadii, null);
			Draw.PopMatrix();
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x0001ACE4 File Offset: 0x00018EE4
		public static void RectangleFill(Vector3 pos, Rect rect)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, Draw.Color, Draw.RectangleThickness, default(Vector4), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x0001AD28 File Offset: 0x00018F28
		public static void RectangleFill(Vector3 pos, Rect rect, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, Draw.Color, Draw.RectangleThickness, default(Vector4), fill);
			Draw.PopMatrix();
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x0001AD68 File Offset: 0x00018F68
		public static void RectangleFill(Vector3 pos, Rect rect, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, Draw.Color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x0001ADAC File Offset: 0x00018FAC
		public static void RectangleFill(Vector3 pos, Rect rect, float cornerRadius, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, Draw.Color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), fill);
			Draw.PopMatrix();
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x0001ADE9 File Offset: 0x00018FE9
		public static void RectangleFill(Vector3 pos, Rect rect, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, Draw.Color, Draw.RectangleThickness, cornerRadii, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x0001AE17 File Offset: 0x00019017
		public static void RectangleFill(Vector3 pos, Rect rect, Vector4 cornerRadii, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, Draw.Color, Draw.RectangleThickness, cornerRadii, fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x0001AE44 File Offset: 0x00019044
		public static void RectangleFill(Vector3 pos, Vector3 normal, Rect rect)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, Draw.Color, Draw.RectangleThickness, default(Vector4), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x0001AEA0 File Offset: 0x000190A0
		public static void RectangleFill(Vector3 pos, Vector3 normal, Rect rect, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, Draw.Color, Draw.RectangleThickness, default(Vector4), fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x0001AEF8 File Offset: 0x000190F8
		public static void RectangleFill(Vector3 pos, Vector3 normal, Rect rect, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, Draw.Color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x0001AF54 File Offset: 0x00019154
		public static void RectangleFill(Vector3 pos, Vector3 normal, Rect rect, float cornerRadius, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, Draw.Color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x0001AFAC File Offset: 0x000191AC
		public static void RectangleFill(Vector3 pos, Vector3 normal, Rect rect, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, Draw.Color, Draw.RectangleThickness, cornerRadii, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x0001B000 File Offset: 0x00019200
		public static void RectangleFill(Vector3 pos, Vector3 normal, Rect rect, Vector4 cornerRadii, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, Draw.Color, Draw.RectangleThickness, cornerRadii, fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x0001B050 File Offset: 0x00019250
		public static void RectangleFill(Vector3 pos, Quaternion rot, Rect rect)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, Draw.Color, Draw.RectangleThickness, default(Vector4), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x0001B0A8 File Offset: 0x000192A8
		public static void RectangleFill(Vector3 pos, Quaternion rot, Rect rect, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, Draw.Color, Draw.RectangleThickness, default(Vector4), fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x0001B0FC File Offset: 0x000192FC
		public static void RectangleFill(Vector3 pos, Quaternion rot, Rect rect, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, Draw.Color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x0001B154 File Offset: 0x00019354
		public static void RectangleFill(Vector3 pos, Quaternion rot, Rect rect, float cornerRadius, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, Draw.Color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), fill);
			Draw.PopMatrix();
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x0001B1A8 File Offset: 0x000193A8
		public static void RectangleFill(Vector3 pos, Quaternion rot, Rect rect, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, Draw.Color, Draw.RectangleThickness, cornerRadii, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x0001B1F6 File Offset: 0x000193F6
		public static void RectangleFill(Vector3 pos, Quaternion rot, Rect rect, Vector4 cornerRadii, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, Draw.Color, Draw.RectangleThickness, cornerRadii, fill);
			Draw.PopMatrix();
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x0001B238 File Offset: 0x00019438
		public static void RectangleFill(Vector3 pos, Vector2 size)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), Draw.Color, Draw.RectangleThickness, default(Vector4), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x0001B280 File Offset: 0x00019480
		public static void RectangleFill(Vector3 pos, Vector2 size, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), Draw.Color, Draw.RectangleThickness, default(Vector4), fill);
			Draw.PopMatrix();
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x0001B2C4 File Offset: 0x000194C4
		public static void RectangleFill(Vector3 pos, Vector2 size, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), Draw.Color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x0001B30C File Offset: 0x0001950C
		public static void RectangleFill(Vector3 pos, Vector2 size, float cornerRadius, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), Draw.Color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x0001B34F File Offset: 0x0001954F
		public static void RectangleFill(Vector3 pos, Vector2 size, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), Draw.Color, Draw.RectangleThickness, cornerRadii, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x0001B383 File Offset: 0x00019583
		public static void RectangleFill(Vector3 pos, Vector2 size, Vector4 cornerRadii, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), Draw.Color, Draw.RectangleThickness, cornerRadii, fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x0001B3B4 File Offset: 0x000195B4
		public static void RectangleFill(Vector3 pos, float width, float height)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), Draw.Color, Draw.RectangleThickness, default(Vector4), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x0001B3FC File Offset: 0x000195FC
		public static void RectangleFill(Vector3 pos, float width, float height, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), Draw.Color, Draw.RectangleThickness, default(Vector4), fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x0001B440 File Offset: 0x00019640
		public static void RectangleFill(Vector3 pos, float width, float height, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), Draw.Color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x0001B488 File Offset: 0x00019688
		public static void RectangleFill(Vector3 pos, float width, float height, float cornerRadius, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), Draw.Color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x0001B4CD File Offset: 0x000196CD
		public static void RectangleFill(Vector3 pos, float width, float height, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), Draw.Color, Draw.RectangleThickness, cornerRadii, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x0001B502 File Offset: 0x00019702
		public static void RectangleFill(Vector3 pos, float width, float height, Vector4 cornerRadii, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), Draw.Color, Draw.RectangleThickness, cornerRadii, fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x0001B534 File Offset: 0x00019734
		public static void RectangleFill(Vector3 pos, Vector3 normal, Vector2 size)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), Draw.Color, Draw.RectangleThickness, default(Vector4), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x0001B598 File Offset: 0x00019798
		public static void RectangleFill(Vector3 pos, Vector3 normal, Vector2 size, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), Draw.Color, Draw.RectangleThickness, default(Vector4), fill);
			Draw.PopMatrix();
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x0001B5F8 File Offset: 0x000197F8
		public static void RectangleFill(Vector3 pos, Vector3 normal, Vector2 size, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), Draw.Color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x0001B65C File Offset: 0x0001985C
		public static void RectangleFill(Vector3 pos, Vector3 normal, Vector2 size, float cornerRadius, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), Draw.Color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), fill);
			Draw.PopMatrix();
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x0001B6BC File Offset: 0x000198BC
		public static void RectangleFill(Vector3 pos, Vector3 normal, Vector2 size, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), Draw.Color, Draw.RectangleThickness, cornerRadii, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x0001B718 File Offset: 0x00019918
		public static void RectangleFill(Vector3 pos, Vector3 normal, Vector2 size, Vector4 cornerRadii, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), Draw.Color, Draw.RectangleThickness, cornerRadii, fill);
			Draw.PopMatrix();
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x0001B770 File Offset: 0x00019970
		public static void RectangleFill(Vector3 pos, Vector3 normal, float width, float height)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), Draw.Color, Draw.RectangleThickness, default(Vector4), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x0001B7D4 File Offset: 0x000199D4
		public static void RectangleFill(Vector3 pos, Vector3 normal, float width, float height, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), Draw.Color, Draw.RectangleThickness, default(Vector4), fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x0001B834 File Offset: 0x00019A34
		public static void RectangleFill(Vector3 pos, Vector3 normal, float width, float height, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), Draw.Color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x0001B89C File Offset: 0x00019A9C
		public static void RectangleFill(Vector3 pos, Vector3 normal, float width, float height, float cornerRadius, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), Draw.Color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x0001B900 File Offset: 0x00019B00
		public static void RectangleFill(Vector3 pos, Vector3 normal, float width, float height, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), Draw.Color, Draw.RectangleThickness, cornerRadii, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x0001B95C File Offset: 0x00019B5C
		public static void RectangleFill(Vector3 pos, Vector3 normal, float width, float height, Vector4 cornerRadii, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), Draw.Color, Draw.RectangleThickness, cornerRadii, fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x0001B9B4 File Offset: 0x00019BB4
		public static void RectangleFill(Vector3 pos, Quaternion rot, Vector2 size)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), Draw.Color, Draw.RectangleThickness, default(Vector4), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x0001BA10 File Offset: 0x00019C10
		public static void RectangleFill(Vector3 pos, Quaternion rot, Vector2 size, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), Draw.Color, Draw.RectangleThickness, default(Vector4), fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x0001BA68 File Offset: 0x00019C68
		public static void RectangleFill(Vector3 pos, Quaternion rot, Vector2 size, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), Draw.Color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x0001BAC4 File Offset: 0x00019CC4
		public static void RectangleFill(Vector3 pos, Quaternion rot, Vector2 size, float cornerRadius, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), Draw.Color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x0001BB20 File Offset: 0x00019D20
		public static void RectangleFill(Vector3 pos, Quaternion rot, Vector2 size, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), Draw.Color, Draw.RectangleThickness, cornerRadii, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x0001BB74 File Offset: 0x00019D74
		public static void RectangleFill(Vector3 pos, Quaternion rot, Vector2 size, Vector4 cornerRadii, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(size), Draw.Color, Draw.RectangleThickness, cornerRadii, fill);
			Draw.PopMatrix();
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x0001BBC8 File Offset: 0x00019DC8
		public static void RectangleFill(Vector3 pos, Quaternion rot, float width, float height)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), Draw.Color, Draw.RectangleThickness, default(Vector4), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x0001BC28 File Offset: 0x00019E28
		public static void RectangleFill(Vector3 pos, Quaternion rot, float width, float height, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), Draw.Color, Draw.RectangleThickness, default(Vector4), fill);
			Draw.PopMatrix();
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x0001BC84 File Offset: 0x00019E84
		public static void RectangleFill(Vector3 pos, Quaternion rot, float width, float height, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), Draw.Color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x0001BCE8 File Offset: 0x00019EE8
		public static void RectangleFill(Vector3 pos, Quaternion rot, float width, float height, float cornerRadius, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), Draw.Color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), fill);
			Draw.PopMatrix();
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x0001BD48 File Offset: 0x00019F48
		public static void RectangleFill(Vector3 pos, Quaternion rot, float width, float height, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), Draw.Color, Draw.RectangleThickness, cornerRadii, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x0001BDA0 File Offset: 0x00019FA0
		public static void RectangleFill(Vector3 pos, Quaternion rot, float width, float height, Vector4 cornerRadii, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, RectPivot.Center.GetRect(width, height), Draw.Color, Draw.RectangleThickness, cornerRadii, fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x0001BDF4 File Offset: 0x00019FF4
		public static void RectangleFill(Rect rect)
		{
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, Draw.Color, Draw.RectangleThickness, default(Vector4), Draw.PolygonShapeFill);
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x0001BE28 File Offset: 0x0001A028
		public static void RectangleFill(Rect rect, ShapeFill fill)
		{
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, Draw.Color, Draw.RectangleThickness, default(Vector4), fill);
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x0001BE58 File Offset: 0x0001A058
		public static void RectangleFill(Rect rect, float cornerRadius)
		{
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, Draw.Color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), Draw.PolygonShapeFill);
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x0001BE8C File Offset: 0x0001A08C
		public static void RectangleFill(Rect rect, float cornerRadius, ShapeFill fill)
		{
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, Draw.Color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), fill);
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x0001BEB9 File Offset: 0x0001A0B9
		public static void RectangleFill(Rect rect, Vector4 cornerRadii)
		{
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, Draw.Color, Draw.RectangleThickness, cornerRadii, Draw.PolygonShapeFill);
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x0001BED7 File Offset: 0x0001A0D7
		public static void RectangleFill(Rect rect, Vector4 cornerRadii, ShapeFill fill)
		{
			Draw.Rectangle_Internal(Draw.BlendMode, false, rect, Draw.Color, Draw.RectangleThickness, cornerRadii, fill);
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x0001BEF4 File Offset: 0x0001A0F4
		public static void RectangleFill(Vector3 pos, Vector2 size, RectPivot pivot)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), Draw.Color, Draw.RectangleThickness, default(Vector4), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x0001BF3C File Offset: 0x0001A13C
		public static void RectangleFill(Vector3 pos, Vector2 size, RectPivot pivot, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), Draw.Color, Draw.RectangleThickness, default(Vector4), fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x0001BF80 File Offset: 0x0001A180
		public static void RectangleFill(Vector3 pos, Vector2 size, RectPivot pivot, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), Draw.Color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x0001BFC8 File Offset: 0x0001A1C8
		public static void RectangleFill(Vector3 pos, Vector2 size, RectPivot pivot, float cornerRadius, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), Draw.Color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), fill);
			Draw.PopMatrix();
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x0001C00C File Offset: 0x0001A20C
		public static void RectangleFill(Vector3 pos, Vector2 size, RectPivot pivot, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), Draw.Color, Draw.RectangleThickness, cornerRadii, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x0001C040 File Offset: 0x0001A240
		public static void RectangleFill(Vector3 pos, Vector2 size, RectPivot pivot, Vector4 cornerRadii, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), Draw.Color, Draw.RectangleThickness, cornerRadii, fill);
			Draw.PopMatrix();
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x0001C074 File Offset: 0x0001A274
		public static void RectangleFill(Vector3 pos, float width, float height, RectPivot pivot)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), Draw.Color, Draw.RectangleThickness, default(Vector4), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x0001C0BC File Offset: 0x0001A2BC
		public static void RectangleFill(Vector3 pos, float width, float height, RectPivot pivot, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), Draw.Color, Draw.RectangleThickness, default(Vector4), fill);
			Draw.PopMatrix();
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x0001C104 File Offset: 0x0001A304
		public static void RectangleFill(Vector3 pos, float width, float height, RectPivot pivot, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), Draw.Color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x0001C150 File Offset: 0x0001A350
		public static void RectangleFill(Vector3 pos, float width, float height, RectPivot pivot, float cornerRadius, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), Draw.Color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x0001C199 File Offset: 0x0001A399
		public static void RectangleFill(Vector3 pos, float width, float height, RectPivot pivot, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), Draw.Color, Draw.RectangleThickness, cornerRadii, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x0001C1CF File Offset: 0x0001A3CF
		public static void RectangleFill(Vector3 pos, float width, float height, RectPivot pivot, Vector4 cornerRadii, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), Draw.Color, Draw.RectangleThickness, cornerRadii, fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x0001C204 File Offset: 0x0001A404
		public static void RectangleFill(Vector3 pos, Vector3 normal, Vector2 size, RectPivot pivot)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), Draw.Color, Draw.RectangleThickness, default(Vector4), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x0001C268 File Offset: 0x0001A468
		public static void RectangleFill(Vector3 pos, Vector3 normal, Vector2 size, RectPivot pivot, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), Draw.Color, Draw.RectangleThickness, default(Vector4), fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x0001C2C8 File Offset: 0x0001A4C8
		public static void RectangleFill(Vector3 pos, Vector3 normal, Vector2 size, RectPivot pivot, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), Draw.Color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x0001C330 File Offset: 0x0001A530
		public static void RectangleFill(Vector3 pos, Vector3 normal, Vector2 size, RectPivot pivot, float cornerRadius, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), Draw.Color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x0001C394 File Offset: 0x0001A594
		public static void RectangleFill(Vector3 pos, Vector3 normal, Vector2 size, RectPivot pivot, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), Draw.Color, Draw.RectangleThickness, cornerRadii, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x0001C3F0 File Offset: 0x0001A5F0
		public static void RectangleFill(Vector3 pos, Vector3 normal, Vector2 size, RectPivot pivot, Vector4 cornerRadii, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), Draw.Color, Draw.RectangleThickness, cornerRadii, fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x0001C448 File Offset: 0x0001A648
		public static void RectangleFill(Vector3 pos, Vector3 normal, float width, float height, RectPivot pivot)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), Draw.Color, Draw.RectangleThickness, default(Vector4), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x0001C4AC File Offset: 0x0001A6AC
		public static void RectangleFill(Vector3 pos, Vector3 normal, float width, float height, RectPivot pivot, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), Draw.Color, Draw.RectangleThickness, default(Vector4), fill);
			Draw.PopMatrix();
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x0001C50C File Offset: 0x0001A70C
		public static void RectangleFill(Vector3 pos, Vector3 normal, float width, float height, RectPivot pivot, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), Draw.Color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x0001C574 File Offset: 0x0001A774
		public static void RectangleFill(Vector3 pos, Vector3 normal, float width, float height, RectPivot pivot, float cornerRadius, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), Draw.Color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), fill);
			Draw.PopMatrix();
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x0001C5D8 File Offset: 0x0001A7D8
		public static void RectangleFill(Vector3 pos, Vector3 normal, float width, float height, RectPivot pivot, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), Draw.Color, Draw.RectangleThickness, cornerRadii, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x0001C634 File Offset: 0x0001A834
		public static void RectangleFill(Vector3 pos, Vector3 normal, float width, float height, RectPivot pivot, Vector4 cornerRadii, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), Draw.Color, Draw.RectangleThickness, cornerRadii, fill);
			Draw.PopMatrix();
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x0001C690 File Offset: 0x0001A890
		public static void RectangleFill(Vector3 pos, Quaternion rot, Vector2 size, RectPivot pivot)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), Draw.Color, Draw.RectangleThickness, default(Vector4), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x0001C6EC File Offset: 0x0001A8EC
		public static void RectangleFill(Vector3 pos, Quaternion rot, Vector2 size, RectPivot pivot, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), Draw.Color, Draw.RectangleThickness, default(Vector4), fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x0001C748 File Offset: 0x0001A948
		public static void RectangleFill(Vector3 pos, Quaternion rot, Vector2 size, RectPivot pivot, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), Draw.Color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x0001C7A8 File Offset: 0x0001A9A8
		public static void RectangleFill(Vector3 pos, Quaternion rot, Vector2 size, RectPivot pivot, float cornerRadius, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), Draw.Color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x0001C808 File Offset: 0x0001AA08
		public static void RectangleFill(Vector3 pos, Quaternion rot, Vector2 size, RectPivot pivot, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), Draw.Color, Draw.RectangleThickness, cornerRadii, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x0001C860 File Offset: 0x0001AA60
		public static void RectangleFill(Vector3 pos, Quaternion rot, Vector2 size, RectPivot pivot, Vector4 cornerRadii, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(size), Draw.Color, Draw.RectangleThickness, cornerRadii, fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x0001C8B4 File Offset: 0x0001AAB4
		public static void RectangleFill(Vector3 pos, Quaternion rot, float width, float height, RectPivot pivot)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), Draw.Color, Draw.RectangleThickness, default(Vector4), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x0001C914 File Offset: 0x0001AB14
		public static void RectangleFill(Vector3 pos, Quaternion rot, float width, float height, RectPivot pivot, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), Draw.Color, Draw.RectangleThickness, default(Vector4), fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x0001C970 File Offset: 0x0001AB70
		public static void RectangleFill(Vector3 pos, Quaternion rot, float width, float height, RectPivot pivot, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), Draw.Color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x0001C9D4 File Offset: 0x0001ABD4
		public static void RectangleFill(Vector3 pos, Quaternion rot, float width, float height, RectPivot pivot, float cornerRadius, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), Draw.Color, Draw.RectangleThickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x0001CA34 File Offset: 0x0001AC34
		public static void RectangleFill(Vector3 pos, Quaternion rot, float width, float height, RectPivot pivot, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), Draw.Color, Draw.RectangleThickness, cornerRadii, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x0001CA8C File Offset: 0x0001AC8C
		public static void RectangleFill(Vector3 pos, Quaternion rot, float width, float height, RectPivot pivot, Vector4 cornerRadii, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, false, pivot.GetRect(width, height), Draw.Color, Draw.RectangleThickness, cornerRadii, fill);
			Draw.PopMatrix();
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x0001CAE0 File Offset: 0x0001ACE0
		public static void RectangleBorderFill(Vector3 pos, Rect rect, float thickness)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, Draw.Color, thickness, default(Vector4), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x0001CB20 File Offset: 0x0001AD20
		public static void RectangleBorderFill(Vector3 pos, Rect rect, float thickness, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, Draw.Color, thickness, default(Vector4), fill);
			Draw.PopMatrix();
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x0001CB5C File Offset: 0x0001AD5C
		public static void RectangleBorderFill(Vector3 pos, Rect rect, float thickness, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x0001CB9C File Offset: 0x0001AD9C
		public static void RectangleBorderFill(Vector3 pos, Rect rect, float thickness, float cornerRadius, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), fill);
			Draw.PopMatrix();
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x0001CBD6 File Offset: 0x0001ADD6
		public static void RectangleBorderFill(Vector3 pos, Rect rect, float thickness, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, Draw.Color, thickness, cornerRadii, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x0001CC00 File Offset: 0x0001AE00
		public static void RectangleBorderFill(Vector3 pos, Rect rect, float thickness, Vector4 cornerRadii, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, Draw.Color, thickness, cornerRadii, fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x0001CC28 File Offset: 0x0001AE28
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, Rect rect, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, Draw.Color, thickness, default(Vector4), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x0001CC80 File Offset: 0x0001AE80
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, Rect rect, float thickness, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, Draw.Color, thickness, default(Vector4), fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x0001CCD4 File Offset: 0x0001AED4
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, Rect rect, float thickness, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x0001CD30 File Offset: 0x0001AF30
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, Rect rect, float thickness, float cornerRadius, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x0001CD88 File Offset: 0x0001AF88
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, Rect rect, float thickness, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, Draw.Color, thickness, cornerRadii, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x0001CDD8 File Offset: 0x0001AFD8
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, Rect rect, float thickness, Vector4 cornerRadii, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, Draw.Color, thickness, cornerRadii, fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x0001CE28 File Offset: 0x0001B028
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, Rect rect, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, Draw.Color, thickness, default(Vector4), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x0001CE7C File Offset: 0x0001B07C
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, Rect rect, float thickness, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, Draw.Color, thickness, default(Vector4), fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x0001CECC File Offset: 0x0001B0CC
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, Rect rect, float thickness, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x0001CF24 File Offset: 0x0001B124
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, Rect rect, float thickness, float cornerRadius, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), fill);
			Draw.PopMatrix();
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x0001CF77 File Offset: 0x0001B177
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, Rect rect, float thickness, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, Draw.Color, thickness, cornerRadii, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x0001CFB7 File Offset: 0x0001B1B7
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, Rect rect, float thickness, Vector4 cornerRadii, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, Draw.Color, thickness, cornerRadii, fill);
			Draw.PopMatrix();
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x0001CFF4 File Offset: 0x0001B1F4
		public static void RectangleBorderFill(Vector3 pos, Vector2 size, float thickness)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), Draw.Color, thickness, default(Vector4), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x0001D038 File Offset: 0x0001B238
		public static void RectangleBorderFill(Vector3 pos, Vector2 size, float thickness, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), Draw.Color, thickness, default(Vector4), fill);
			Draw.PopMatrix();
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x0001D078 File Offset: 0x0001B278
		public static void RectangleBorderFill(Vector3 pos, Vector2 size, float thickness, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x0001D0BC File Offset: 0x0001B2BC
		public static void RectangleBorderFill(Vector3 pos, Vector2 size, float thickness, float cornerRadius, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x0001D0FC File Offset: 0x0001B2FC
		public static void RectangleBorderFill(Vector3 pos, Vector2 size, float thickness, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), Draw.Color, thickness, cornerRadii, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x0001D12C File Offset: 0x0001B32C
		public static void RectangleBorderFill(Vector3 pos, Vector2 size, float thickness, Vector4 cornerRadii, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), Draw.Color, thickness, cornerRadii, fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x0001D15C File Offset: 0x0001B35C
		public static void RectangleBorderFill(Vector3 pos, float width, float height, float thickness)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), Draw.Color, thickness, default(Vector4), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x0001D1A0 File Offset: 0x0001B3A0
		public static void RectangleBorderFill(Vector3 pos, float width, float height, float thickness, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), Draw.Color, thickness, default(Vector4), fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x0001D1E4 File Offset: 0x0001B3E4
		public static void RectangleBorderFill(Vector3 pos, float width, float height, float thickness, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x0001D22C File Offset: 0x0001B42C
		public static void RectangleBorderFill(Vector3 pos, float width, float height, float thickness, float cornerRadius, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x0001D271 File Offset: 0x0001B471
		public static void RectangleBorderFill(Vector3 pos, float width, float height, float thickness, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), Draw.Color, thickness, cornerRadii, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x0001D2A3 File Offset: 0x0001B4A3
		public static void RectangleBorderFill(Vector3 pos, float width, float height, float thickness, Vector4 cornerRadii, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), Draw.Color, thickness, cornerRadii, fill);
			Draw.PopMatrix();
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x0001D2D4 File Offset: 0x0001B4D4
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, Vector2 size, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), Draw.Color, thickness, default(Vector4), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x0001D334 File Offset: 0x0001B534
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, Vector2 size, float thickness, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), Draw.Color, thickness, default(Vector4), fill);
			Draw.PopMatrix();
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x0001D390 File Offset: 0x0001B590
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, Vector2 size, float thickness, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x0001D3F4 File Offset: 0x0001B5F4
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, Vector2 size, float thickness, float cornerRadius, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), fill);
			Draw.PopMatrix();
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x0001D454 File Offset: 0x0001B654
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, Vector2 size, float thickness, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), Draw.Color, thickness, cornerRadii, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x0001D4AC File Offset: 0x0001B6AC
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, Vector2 size, float thickness, Vector4 cornerRadii, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), Draw.Color, thickness, cornerRadii, fill);
			Draw.PopMatrix();
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x0001D500 File Offset: 0x0001B700
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, float width, float height, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), Draw.Color, thickness, default(Vector4), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x0001D560 File Offset: 0x0001B760
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, float width, float height, float thickness, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), Draw.Color, thickness, default(Vector4), fill);
			Draw.PopMatrix();
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x0001D5BC File Offset: 0x0001B7BC
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, float width, float height, float thickness, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060007A1 RID: 1953 RVA: 0x0001D620 File Offset: 0x0001B820
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, float width, float height, float thickness, float cornerRadius, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), fill);
			Draw.PopMatrix();
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x0001D680 File Offset: 0x0001B880
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, float width, float height, float thickness, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), Draw.Color, thickness, cornerRadii, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x0001D6D8 File Offset: 0x0001B8D8
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, float width, float height, float thickness, Vector4 cornerRadii, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), Draw.Color, thickness, cornerRadii, fill);
			Draw.PopMatrix();
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x0001D730 File Offset: 0x0001B930
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, Vector2 size, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), Draw.Color, thickness, default(Vector4), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x0001D788 File Offset: 0x0001B988
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, Vector2 size, float thickness, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), Draw.Color, thickness, default(Vector4), fill);
			Draw.PopMatrix();
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x0001D7E0 File Offset: 0x0001B9E0
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, Vector2 size, float thickness, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x0001D83C File Offset: 0x0001BA3C
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, Vector2 size, float thickness, float cornerRadius, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), fill);
			Draw.PopMatrix();
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x0001D898 File Offset: 0x0001BA98
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, Vector2 size, float thickness, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), Draw.Color, thickness, cornerRadii, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060007A9 RID: 1961 RVA: 0x0001D8EC File Offset: 0x0001BAEC
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, Vector2 size, float thickness, Vector4 cornerRadii, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(size), Draw.Color, thickness, cornerRadii, fill);
			Draw.PopMatrix();
		}

		// Token: 0x060007AA RID: 1962 RVA: 0x0001D93C File Offset: 0x0001BB3C
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, float width, float height, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), Draw.Color, thickness, default(Vector4), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060007AB RID: 1963 RVA: 0x0001D998 File Offset: 0x0001BB98
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, float width, float height, float thickness, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), Draw.Color, thickness, default(Vector4), fill);
			Draw.PopMatrix();
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x0001D9F0 File Offset: 0x0001BBF0
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, float width, float height, float thickness, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060007AD RID: 1965 RVA: 0x0001DA50 File Offset: 0x0001BC50
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, float width, float height, float thickness, float cornerRadius, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), fill);
			Draw.PopMatrix();
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x0001DAAC File Offset: 0x0001BCAC
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, float width, float height, float thickness, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), Draw.Color, thickness, cornerRadii, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x0001DB00 File Offset: 0x0001BD00
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, float width, float height, float thickness, Vector4 cornerRadii, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, RectPivot.Center.GetRect(width, height), Draw.Color, thickness, cornerRadii, fill);
			Draw.PopMatrix();
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x0001DB50 File Offset: 0x0001BD50
		public static void RectangleBorderFill(Rect rect, float thickness)
		{
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, Draw.Color, thickness, default(Vector4), Draw.PolygonShapeFill);
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x0001DB80 File Offset: 0x0001BD80
		public static void RectangleBorderFill(Rect rect, float thickness, ShapeFill fill)
		{
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, Draw.Color, thickness, default(Vector4), fill);
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x0001DBAC File Offset: 0x0001BDAC
		public static void RectangleBorderFill(Rect rect, float thickness, float cornerRadius)
		{
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), Draw.PolygonShapeFill);
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x0001DBDC File Offset: 0x0001BDDC
		public static void RectangleBorderFill(Rect rect, float thickness, float cornerRadius, ShapeFill fill)
		{
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), fill);
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x0001DC05 File Offset: 0x0001BE05
		public static void RectangleBorderFill(Rect rect, float thickness, Vector4 cornerRadii)
		{
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, Draw.Color, thickness, cornerRadii, Draw.PolygonShapeFill);
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x0001DC1F File Offset: 0x0001BE1F
		public static void RectangleBorderFill(Rect rect, float thickness, Vector4 cornerRadii, ShapeFill fill)
		{
			Draw.Rectangle_Internal(Draw.BlendMode, true, rect, Draw.Color, thickness, cornerRadii, fill);
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x0001DC38 File Offset: 0x0001BE38
		public static void RectangleBorderFill(Vector3 pos, Vector2 size, RectPivot pivot, float thickness)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), Draw.Color, thickness, default(Vector4), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x0001DC7C File Offset: 0x0001BE7C
		public static void RectangleBorderFill(Vector3 pos, Vector2 size, RectPivot pivot, float thickness, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), Draw.Color, thickness, default(Vector4), fill);
			Draw.PopMatrix();
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x0001DCBC File Offset: 0x0001BEBC
		public static void RectangleBorderFill(Vector3 pos, Vector2 size, RectPivot pivot, float thickness, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x0001DD04 File Offset: 0x0001BF04
		public static void RectangleBorderFill(Vector3 pos, Vector2 size, RectPivot pivot, float thickness, float cornerRadius, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), fill);
			Draw.PopMatrix();
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x0001DD48 File Offset: 0x0001BF48
		public static void RectangleBorderFill(Vector3 pos, Vector2 size, RectPivot pivot, float thickness, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), Draw.Color, thickness, cornerRadii, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x0001DD79 File Offset: 0x0001BF79
		public static void RectangleBorderFill(Vector3 pos, Vector2 size, RectPivot pivot, float thickness, Vector4 cornerRadii, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), Draw.Color, thickness, cornerRadii, fill);
			Draw.PopMatrix();
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x0001DDA8 File Offset: 0x0001BFA8
		public static void RectangleBorderFill(Vector3 pos, float width, float height, RectPivot pivot, float thickness)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), Draw.Color, thickness, default(Vector4), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x0001DDF0 File Offset: 0x0001BFF0
		public static void RectangleBorderFill(Vector3 pos, float width, float height, RectPivot pivot, float thickness, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), Draw.Color, thickness, default(Vector4), fill);
			Draw.PopMatrix();
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x0001DE34 File Offset: 0x0001C034
		public static void RectangleBorderFill(Vector3 pos, float width, float height, RectPivot pivot, float thickness, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x0001DE80 File Offset: 0x0001C080
		public static void RectangleBorderFill(Vector3 pos, float width, float height, RectPivot pivot, float thickness, float cornerRadius, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), fill);
			Draw.PopMatrix();
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x0001DEC6 File Offset: 0x0001C0C6
		public static void RectangleBorderFill(Vector3 pos, float width, float height, RectPivot pivot, float thickness, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), Draw.Color, thickness, cornerRadii, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x0001DEF9 File Offset: 0x0001C0F9
		public static void RectangleBorderFill(Vector3 pos, float width, float height, RectPivot pivot, float thickness, Vector4 cornerRadii, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), Draw.Color, thickness, cornerRadii, fill);
			Draw.PopMatrix();
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x0001DF2C File Offset: 0x0001C12C
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, Vector2 size, RectPivot pivot, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), Draw.Color, thickness, default(Vector4), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x0001DF8C File Offset: 0x0001C18C
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, Vector2 size, RectPivot pivot, float thickness, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), Draw.Color, thickness, default(Vector4), fill);
			Draw.PopMatrix();
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x0001DFE8 File Offset: 0x0001C1E8
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, Vector2 size, RectPivot pivot, float thickness, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x0001E04C File Offset: 0x0001C24C
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, Vector2 size, RectPivot pivot, float thickness, float cornerRadius, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), fill);
			Draw.PopMatrix();
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x0001E0AC File Offset: 0x0001C2AC
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, Vector2 size, RectPivot pivot, float thickness, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), Draw.Color, thickness, cornerRadii, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x0001E104 File Offset: 0x0001C304
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, Vector2 size, RectPivot pivot, float thickness, Vector4 cornerRadii, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), Draw.Color, thickness, cornerRadii, fill);
			Draw.PopMatrix();
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x0001E158 File Offset: 0x0001C358
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, float width, float height, RectPivot pivot, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), Draw.Color, thickness, default(Vector4), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x0001E1B8 File Offset: 0x0001C3B8
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, float width, float height, RectPivot pivot, float thickness, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), Draw.Color, thickness, default(Vector4), fill);
			Draw.PopMatrix();
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x0001E218 File Offset: 0x0001C418
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, float width, float height, RectPivot pivot, float thickness, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x0001E27C File Offset: 0x0001C47C
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, float width, float height, RectPivot pivot, float thickness, float cornerRadius, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), fill);
			Draw.PopMatrix();
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x0001E2E0 File Offset: 0x0001C4E0
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, float width, float height, RectPivot pivot, float thickness, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), Draw.Color, thickness, cornerRadii, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x0001E33C File Offset: 0x0001C53C
		public static void RectangleBorderFill(Vector3 pos, Vector3 normal, float width, float height, RectPivot pivot, float thickness, Vector4 cornerRadii, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), Draw.Color, thickness, cornerRadii, fill);
			Draw.PopMatrix();
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x0001E394 File Offset: 0x0001C594
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, Vector2 size, RectPivot pivot, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), Draw.Color, thickness, default(Vector4), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x0001E3F0 File Offset: 0x0001C5F0
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, Vector2 size, RectPivot pivot, float thickness, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), Draw.Color, thickness, default(Vector4), fill);
			Draw.PopMatrix();
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x0001E448 File Offset: 0x0001C648
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, Vector2 size, RectPivot pivot, float thickness, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x0001E4A8 File Offset: 0x0001C6A8
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, Vector2 size, RectPivot pivot, float thickness, float cornerRadius, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), fill);
			Draw.PopMatrix();
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x0001E504 File Offset: 0x0001C704
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, Vector2 size, RectPivot pivot, float thickness, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), Draw.Color, thickness, cornerRadii, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x0001E558 File Offset: 0x0001C758
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, Vector2 size, RectPivot pivot, float thickness, Vector4 cornerRadii, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(size), Draw.Color, thickness, cornerRadii, fill);
			Draw.PopMatrix();
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x0001E5A8 File Offset: 0x0001C7A8
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, float width, float height, RectPivot pivot, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), Draw.Color, thickness, default(Vector4), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x0001E604 File Offset: 0x0001C804
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, float width, float height, RectPivot pivot, float thickness, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), Draw.Color, thickness, default(Vector4), fill);
			Draw.PopMatrix();
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x0001E65C File Offset: 0x0001C85C
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, float width, float height, RectPivot pivot, float thickness, float cornerRadius)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x0001E6BC File Offset: 0x0001C8BC
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, float width, float height, RectPivot pivot, float thickness, float cornerRadius, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), Draw.Color, thickness, new Vector4(cornerRadius, cornerRadius, cornerRadius, cornerRadius), fill);
			Draw.PopMatrix();
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x0001E718 File Offset: 0x0001C918
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, float width, float height, RectPivot pivot, float thickness, Vector4 cornerRadii)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), Draw.Color, thickness, cornerRadii, Draw.PolygonShapeFill);
			Draw.PopMatrix();
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x0001E76C File Offset: 0x0001C96C
		public static void RectangleBorderFill(Vector3 pos, Quaternion rot, float width, float height, RectPivot pivot, float thickness, Vector4 cornerRadii, ShapeFill fill)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Rectangle_Internal(Draw.BlendMode, true, pivot.GetRect(width, height), Draw.Color, thickness, cornerRadii, fill);
			Draw.PopMatrix();
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x0001E7C0 File Offset: 0x0001C9C0
		public static void Triangle(Vector3 a, Vector3 b, Vector3 c)
		{
			Draw.Triangle_Internal(a, b, c, false, Draw.TriangleThickness, 0f, Draw.Color, Draw.Color, Draw.Color);
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x0001E7F0 File Offset: 0x0001C9F0
		public static void Triangle(Vector3 a, Vector3 b, Vector3 c, Color color)
		{
			Draw.Triangle_Internal(a, b, c, false, Draw.TriangleThickness, 0f, color, color, color);
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x0001E814 File Offset: 0x0001CA14
		public static void Triangle(Vector3 a, Vector3 b, Vector3 c, Color colorA, Color colorB, Color colorC)
		{
			Draw.Triangle_Internal(a, b, c, false, Draw.TriangleThickness, 0f, colorA, colorB, colorC);
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x0001E83C File Offset: 0x0001CA3C
		public static void Triangle(Vector3 a, Vector3 b, Vector3 c, float roundness)
		{
			Draw.Triangle_Internal(a, b, c, false, Draw.TriangleThickness, roundness, Draw.Color, Draw.Color, Draw.Color);
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x0001E868 File Offset: 0x0001CA68
		public static void Triangle(Vector3 a, Vector3 b, Vector3 c, float roundness, Color color)
		{
			Draw.Triangle_Internal(a, b, c, false, Draw.TriangleThickness, roundness, color, color, color);
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x0001E88C File Offset: 0x0001CA8C
		public static void Triangle(Vector3 a, Vector3 b, Vector3 c, float roundness, Color colorA, Color colorB, Color colorC)
		{
			Draw.Triangle_Internal(a, b, c, false, Draw.TriangleThickness, roundness, colorA, colorB, colorC);
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x0001E8B0 File Offset: 0x0001CAB0
		public static void TriangleHollow(Vector3 a, Vector3 b, Vector3 c)
		{
			Draw.Triangle_Internal(a, b, c, true, Draw.TriangleThickness, 0f, Draw.Color, Draw.Color, Draw.Color);
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x0001E8E0 File Offset: 0x0001CAE0
		public static void TriangleHollow(Vector3 a, Vector3 b, Vector3 c, Color color)
		{
			Draw.Triangle_Internal(a, b, c, true, Draw.TriangleThickness, 0f, color, color, color);
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x0001E904 File Offset: 0x0001CB04
		public static void TriangleHollow(Vector3 a, Vector3 b, Vector3 c, Color colorA, Color colorB, Color colorC)
		{
			Draw.Triangle_Internal(a, b, c, true, Draw.TriangleThickness, 0f, colorA, colorB, colorC);
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x0001E92C File Offset: 0x0001CB2C
		public static void TriangleHollow(Vector3 a, Vector3 b, Vector3 c, float thickness)
		{
			Draw.Triangle_Internal(a, b, c, true, thickness, 0f, Draw.Color, Draw.Color, Draw.Color);
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x0001E958 File Offset: 0x0001CB58
		public static void TriangleHollow(Vector3 a, Vector3 b, Vector3 c, float thickness, Color color)
		{
			Draw.Triangle_Internal(a, b, c, true, thickness, 0f, color, color, color);
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x0001E97C File Offset: 0x0001CB7C
		public static void TriangleHollow(Vector3 a, Vector3 b, Vector3 c, float thickness, Color colorA, Color colorB, Color colorC)
		{
			Draw.Triangle_Internal(a, b, c, true, thickness, 0f, colorA, colorB, colorC);
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x0001E9A0 File Offset: 0x0001CBA0
		public static void TriangleHollow(Vector3 a, Vector3 b, Vector3 c, float thickness, float roundness)
		{
			Draw.Triangle_Internal(a, b, c, true, thickness, roundness, Draw.Color, Draw.Color, Draw.Color);
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x0001E9C8 File Offset: 0x0001CBC8
		public static void TriangleHollow(Vector3 a, Vector3 b, Vector3 c, float thickness, float roundness, Color color)
		{
			Draw.Triangle_Internal(a, b, c, true, thickness, roundness, color, color, color);
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x0001E9E8 File Offset: 0x0001CBE8
		public static void TriangleHollow(Vector3 a, Vector3 b, Vector3 c, float thickness, float roundness, Color colorA, Color colorB, Color colorC)
		{
			Draw.Triangle_Internal(a, b, c, true, thickness, roundness, colorA, colorB, colorC);
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x0001EA07 File Offset: 0x0001CC07
		public static void Quad(Vector3 a, Vector3 b, Vector3 c)
		{
			Draw.Quad_Internal(a, b, c, a + (c - b), Draw.Color, Draw.Color, Draw.Color, Draw.Color);
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x0001EA32 File Offset: 0x0001CC32
		public static void Quad(Vector3 a, Vector3 b, Vector3 c, Color color)
		{
			Draw.Quad_Internal(a, b, c, a + (c - b), color, color, color, color);
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x0001EA4D File Offset: 0x0001CC4D
		public static void Quad(Vector3 a, Vector3 b, Vector3 c, Color colorA, Color colorB, Color colorC, Color colorD)
		{
			Draw.Quad_Internal(a, b, c, a + (c - b), colorA, colorB, colorC, colorD);
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x0001EA6B File Offset: 0x0001CC6B
		public static void Quad(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
		{
			Draw.Quad_Internal(a, b, c, d, Draw.Color, Draw.Color, Draw.Color, Draw.Color);
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x0001EA8A File Offset: 0x0001CC8A
		public static void Quad(Vector3 a, Vector3 b, Vector3 c, Vector3 d, Color color)
		{
			Draw.Quad_Internal(a, b, c, d, color, color, color, color);
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x0001EA9D File Offset: 0x0001CC9D
		public static void Quad(Vector3 a, Vector3 b, Vector3 c, Vector3 d, Color colorA, Color colorB, Color colorC, Color colorD)
		{
			Draw.Quad_Internal(a, b, c, d, colorA, colorB, colorC, colorD);
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x0001EAB0 File Offset: 0x0001CCB0
		public static void Sphere(Vector3 pos)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Sphere_Internal(Draw.SphereRadius, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x0001EAD1 File Offset: 0x0001CCD1
		public static void Sphere(Vector3 pos, float radius)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Sphere_Internal(radius, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x0001EAEE File Offset: 0x0001CCEE
		public static void Sphere(Vector3 pos, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Sphere_Internal(Draw.SphereRadius, color);
			Draw.PopMatrix();
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x0001EB0B File Offset: 0x0001CD0B
		public static void Sphere(Vector3 pos, float radius, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Sphere_Internal(radius, color);
			Draw.PopMatrix();
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x0001EB24 File Offset: 0x0001CD24
		public static void Sphere()
		{
			Draw.Sphere_Internal(Draw.SphereRadius, Draw.Color);
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x0001EB35 File Offset: 0x0001CD35
		public static void Sphere(float radius)
		{
			Draw.Sphere_Internal(radius, Draw.Color);
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x0001EB42 File Offset: 0x0001CD42
		public static void Sphere(Color color)
		{
			Draw.Sphere_Internal(Draw.SphereRadius, color);
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x0001EB4F File Offset: 0x0001CD4F
		public static void Sphere(float radius, Color color)
		{
			Draw.Sphere_Internal(radius, color);
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x0001EB58 File Offset: 0x0001CD58
		public static void Cuboid(Vector3 pos, Vector3 size)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Cuboid_Internal(size, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x0001EB75 File Offset: 0x0001CD75
		public static void Cuboid(Vector3 pos, Vector3 size, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Cuboid_Internal(size, color);
			Draw.PopMatrix();
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x0001EB8E File Offset: 0x0001CD8E
		public static void Cuboid(Vector3 pos, Vector3 normal, Vector3 size)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Cuboid_Internal(size, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x0001EBC5 File Offset: 0x0001CDC5
		public static void Cuboid(Vector3 pos, Vector3 normal, Vector3 size, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Cuboid_Internal(size, color);
			Draw.PopMatrix();
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x0001EBF8 File Offset: 0x0001CDF8
		public static void Cuboid(Vector3 pos, Quaternion rot, Vector3 size)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Cuboid_Internal(size, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x0001EC2A File Offset: 0x0001CE2A
		public static void Cuboid(Vector3 pos, Quaternion rot, Vector3 size, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Cuboid_Internal(size, color);
			Draw.PopMatrix();
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x0001EC58 File Offset: 0x0001CE58
		public static void Cuboid(Vector3 size)
		{
			Draw.Cuboid_Internal(size, Draw.Color);
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x0001EC65 File Offset: 0x0001CE65
		public static void Cuboid(Vector3 size, Color color)
		{
			Draw.Cuboid_Internal(size, color);
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x0001EC6E File Offset: 0x0001CE6E
		public static void Cube(Vector3 pos, float size)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Cuboid_Internal(new Vector3(size, size, size), Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x0001EC92 File Offset: 0x0001CE92
		public static void Cube(Vector3 pos, float size, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Cuboid_Internal(new Vector3(size, size, size), color);
			Draw.PopMatrix();
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x0001ECB2 File Offset: 0x0001CEB2
		public static void Cube(Vector3 pos, Vector3 normal, float size)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Cuboid_Internal(new Vector3(size, size, size), Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x0001ECF0 File Offset: 0x0001CEF0
		public static void Cube(Vector3 pos, Vector3 normal, float size, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Cuboid_Internal(new Vector3(size, size, size), color);
			Draw.PopMatrix();
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x0001ED2A File Offset: 0x0001CF2A
		public static void Cube(Vector3 pos, Quaternion rot, float size)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Cuboid_Internal(new Vector3(size, size, size), Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x0001ED63 File Offset: 0x0001CF63
		public static void Cube(Vector3 pos, Quaternion rot, float size, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Cuboid_Internal(new Vector3(size, size, size), color);
			Draw.PopMatrix();
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x0001ED98 File Offset: 0x0001CF98
		public static void Cube(float size)
		{
			Draw.Cuboid_Internal(new Vector3(size, size, size), Draw.Color);
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x0001EDAC File Offset: 0x0001CFAC
		public static void Cube(float size, Color color)
		{
			Draw.Cuboid_Internal(new Vector3(size, size, size), color);
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x0001EDBC File Offset: 0x0001CFBC
		public static void Cone(Vector3 pos, float radius, float length)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Cone_Internal(radius, length, true, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x0001EDDB File Offset: 0x0001CFDB
		public static void Cone(Vector3 pos, float radius, float length, bool fillCap)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Cone_Internal(radius, length, fillCap, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x0001EDFA File Offset: 0x0001CFFA
		public static void Cone(Vector3 pos, float radius, float length, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Cone_Internal(radius, length, true, color);
			Draw.PopMatrix();
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x0001EE15 File Offset: 0x0001D015
		public static void Cone(Vector3 pos, float radius, float length, bool fillCap, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Cone_Internal(radius, length, fillCap, color);
			Draw.PopMatrix();
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x0001EE31 File Offset: 0x0001D031
		public static void Cone(Vector3 pos, Vector3 normal, float radius, float length)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Cone_Internal(radius, length, true, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x0001EE6A File Offset: 0x0001D06A
		public static void Cone(Vector3 pos, Vector3 normal, float radius, float length, bool fillCap)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Cone_Internal(radius, length, fillCap, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x0001EEA4 File Offset: 0x0001D0A4
		public static void Cone(Vector3 pos, Vector3 normal, float radius, float length, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Cone_Internal(radius, length, true, color);
			Draw.PopMatrix();
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x0001EEDA File Offset: 0x0001D0DA
		public static void Cone(Vector3 pos, Vector3 normal, float radius, float length, bool fillCap, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Cone_Internal(radius, length, fillCap, color);
			Draw.PopMatrix();
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x0001EF11 File Offset: 0x0001D111
		public static void Cone(Vector3 pos, Quaternion rot, float radius, float length)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Cone_Internal(radius, length, true, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x0001EF45 File Offset: 0x0001D145
		public static void Cone(Vector3 pos, Quaternion rot, float radius, float length, bool fillCap)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Cone_Internal(radius, length, fillCap, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x0001EF7A File Offset: 0x0001D17A
		public static void Cone(Vector3 pos, Quaternion rot, float radius, float length, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Cone_Internal(radius, length, true, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x0001EFAB File Offset: 0x0001D1AB
		public static void Cone(Vector3 pos, Quaternion rot, float radius, float length, bool fillCap, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Cone_Internal(radius, length, fillCap, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x0001EFDD File Offset: 0x0001D1DD
		public static void Cone(float radius, float length)
		{
			Draw.Cone_Internal(radius, length, true, Draw.Color);
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x0001EFEC File Offset: 0x0001D1EC
		public static void Cone(float radius, float length, bool fillCap)
		{
			Draw.Cone_Internal(radius, length, fillCap, Draw.Color);
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x0001EFFB File Offset: 0x0001D1FB
		public static void Cone(float radius, float length, Color color)
		{
			Draw.Cone_Internal(radius, length, true, color);
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x0001F006 File Offset: 0x0001D206
		public static void Cone(float radius, float length, bool fillCap, Color color)
		{
			Draw.Cone_Internal(radius, length, fillCap, color);
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x0001F011 File Offset: 0x0001D211
		public static void Torus(Vector3 pos, float radius, float thickness)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Torus_Internal(radius, thickness, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x0001F02F File Offset: 0x0001D22F
		public static void Torus(Vector3 pos, float radius, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Torus_Internal(radius, thickness, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x0001F049 File Offset: 0x0001D249
		public static void Torus(Vector3 pos, Vector3 normal, float radius, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Torus_Internal(radius, thickness, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x0001F081 File Offset: 0x0001D281
		public static void Torus(Vector3 pos, Vector3 normal, float radius, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Torus_Internal(radius, thickness, color);
			Draw.PopMatrix();
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x0001F0B6 File Offset: 0x0001D2B6
		public static void Torus(Vector3 pos, Quaternion rot, float radius, float thickness)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Torus_Internal(radius, thickness, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x0001F0E9 File Offset: 0x0001D2E9
		public static void Torus(Vector3 pos, Quaternion rot, float radius, float thickness, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Torus_Internal(radius, thickness, color);
			Draw.PopMatrix();
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x0001F119 File Offset: 0x0001D319
		public static void Torus(float radius, float thickness)
		{
			Draw.Torus_Internal(radius, thickness, Draw.Color);
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x0001F127 File Offset: 0x0001D327
		public static void Torus(float radius, float thickness, Color color)
		{
			Draw.Torus_Internal(radius, thickness, color);
		}

		// Token: 0x0600081F RID: 2079 RVA: 0x0001F131 File Offset: 0x0001D331
		public static void Text(Vector3 pos, string content)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(content, Draw.Font, Draw.FontSize, Draw.TextAlign, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x0001F15D File Offset: 0x0001D35D
		public static void Text(Vector3 pos, string content, TextAlign align)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(content, Draw.Font, Draw.FontSize, align, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x0001F185 File Offset: 0x0001D385
		public static void Text(Vector3 pos, string content, float fontSize)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(content, Draw.Font, fontSize, Draw.TextAlign, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x0001F1AD File Offset: 0x0001D3AD
		public static void Text(Vector3 pos, string content, TextAlign align, float fontSize)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(content, Draw.Font, fontSize, align, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x0001F1D1 File Offset: 0x0001D3D1
		public static void Text(Vector3 pos, string content, TMP_FontAsset font)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(content, font, Draw.FontSize, Draw.TextAlign, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x0001F1F9 File Offset: 0x0001D3F9
		public static void Text(Vector3 pos, string content, TextAlign align, TMP_FontAsset font)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(content, font, Draw.FontSize, align, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x0001F21D File Offset: 0x0001D41D
		public static void Text(Vector3 pos, string content, float fontSize, TMP_FontAsset font)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(content, font, fontSize, Draw.TextAlign, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x0001F241 File Offset: 0x0001D441
		public static void Text(Vector3 pos, string content, TextAlign align, float fontSize, TMP_FontAsset font)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(content, font, fontSize, align, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x0001F262 File Offset: 0x0001D462
		public static void Text(Vector3 pos, string content, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(content, Draw.Font, Draw.FontSize, Draw.TextAlign, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x0001F28A File Offset: 0x0001D48A
		public static void Text(Vector3 pos, string content, TextAlign align, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(content, Draw.Font, Draw.FontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x0001F2AE File Offset: 0x0001D4AE
		public static void Text(Vector3 pos, string content, float fontSize, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(content, Draw.Font, fontSize, Draw.TextAlign, color);
			Draw.PopMatrix();
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x0001F2D2 File Offset: 0x0001D4D2
		public static void Text(Vector3 pos, string content, TextAlign align, float fontSize, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(content, Draw.Font, fontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x0001F2F3 File Offset: 0x0001D4F3
		public static void Text(Vector3 pos, string content, TMP_FontAsset font, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(content, font, Draw.FontSize, Draw.TextAlign, color);
			Draw.PopMatrix();
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x0001F317 File Offset: 0x0001D517
		public static void Text(Vector3 pos, string content, TextAlign align, TMP_FontAsset font, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(content, font, Draw.FontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x0001F338 File Offset: 0x0001D538
		public static void Text(Vector3 pos, string content, float fontSize, TMP_FontAsset font, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(content, font, fontSize, Draw.TextAlign, color);
			Draw.PopMatrix();
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x0001F359 File Offset: 0x0001D559
		public static void Text(Vector3 pos, string content, TextAlign align, float fontSize, TMP_FontAsset font, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Text_Internal(content, font, fontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x0001F378 File Offset: 0x0001D578
		public static void Text(Vector3 pos, Vector3 normal, string content)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Text_Internal(content, Draw.Font, Draw.FontSize, Draw.TextAlign, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x0001F3CC File Offset: 0x0001D5CC
		public static void Text(Vector3 pos, Vector3 normal, string content, TextAlign align)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Text_Internal(content, Draw.Font, Draw.FontSize, align, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x0001F41C File Offset: 0x0001D61C
		public static void Text(Vector3 pos, Vector3 normal, string content, float fontSize)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Text_Internal(content, Draw.Font, fontSize, Draw.TextAlign, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x0001F469 File Offset: 0x0001D669
		public static void Text(Vector3 pos, Vector3 normal, string content, TextAlign align, float fontSize)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Text_Internal(content, Draw.Font, fontSize, align, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x0001F4A8 File Offset: 0x0001D6A8
		public static void Text(Vector3 pos, Vector3 normal, string content, TMP_FontAsset font)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Text_Internal(content, font, Draw.FontSize, Draw.TextAlign, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x0001F4F5 File Offset: 0x0001D6F5
		public static void Text(Vector3 pos, Vector3 normal, string content, TextAlign align, TMP_FontAsset font)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Text_Internal(content, font, Draw.FontSize, align, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x0001F534 File Offset: 0x0001D734
		public static void Text(Vector3 pos, Vector3 normal, string content, float fontSize, TMP_FontAsset font)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Text_Internal(content, font, fontSize, Draw.TextAlign, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x0001F573 File Offset: 0x0001D773
		public static void Text(Vector3 pos, Vector3 normal, string content, TextAlign align, float fontSize, TMP_FontAsset font)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Text_Internal(content, font, fontSize, align, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x0001F5B0 File Offset: 0x0001D7B0
		public static void Text(Vector3 pos, Vector3 normal, string content, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Text_Internal(content, Draw.Font, Draw.FontSize, Draw.TextAlign, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x0001F5FD File Offset: 0x0001D7FD
		public static void Text(Vector3 pos, Vector3 normal, string content, TextAlign align, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Text_Internal(content, Draw.Font, Draw.FontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x0001F63C File Offset: 0x0001D83C
		public static void Text(Vector3 pos, Vector3 normal, string content, float fontSize, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Text_Internal(content, Draw.Font, fontSize, Draw.TextAlign, color);
			Draw.PopMatrix();
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x0001F67B File Offset: 0x0001D87B
		public static void Text(Vector3 pos, Vector3 normal, string content, TextAlign align, float fontSize, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Text_Internal(content, Draw.Font, fontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x0001F6B7 File Offset: 0x0001D8B7
		public static void Text(Vector3 pos, Vector3 normal, string content, TMP_FontAsset font, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Text_Internal(content, font, Draw.FontSize, Draw.TextAlign, color);
			Draw.PopMatrix();
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x0001F6F6 File Offset: 0x0001D8F6
		public static void Text(Vector3 pos, Vector3 normal, string content, TextAlign align, TMP_FontAsset font, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Text_Internal(content, font, Draw.FontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x0001F732 File Offset: 0x0001D932
		public static void Text(Vector3 pos, Vector3 normal, string content, float fontSize, TMP_FontAsset font, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Text_Internal(content, font, fontSize, Draw.TextAlign, color);
			Draw.PopMatrix();
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x0001F76E File Offset: 0x0001D96E
		public static void Text(Vector3 pos, Vector3 normal, string content, TextAlign align, float fontSize, TMP_FontAsset font, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, Quaternion.LookRotation(normal), Vector3.one);
			Draw.Text_Internal(content, font, fontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x0001F7A8 File Offset: 0x0001D9A8
		public static void Text(Vector3 pos, Quaternion rot, string content)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(content, Draw.Font, Draw.FontSize, Draw.TextAlign, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x0001F7F4 File Offset: 0x0001D9F4
		public static void Text(Vector3 pos, Quaternion rot, string content, TextAlign align)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(content, Draw.Font, Draw.FontSize, align, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x0001F831 File Offset: 0x0001DA31
		public static void Text(Vector3 pos, Quaternion rot, string content, float fontSize)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(content, Draw.Font, fontSize, Draw.TextAlign, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x0001F86E File Offset: 0x0001DA6E
		public static void Text(Vector3 pos, Quaternion rot, string content, TextAlign align, float fontSize)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(content, Draw.Font, fontSize, align, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x0001F8A8 File Offset: 0x0001DAA8
		public static void Text(Vector3 pos, Quaternion rot, string content, TMP_FontAsset font)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(content, font, Draw.FontSize, Draw.TextAlign, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x0001F8E5 File Offset: 0x0001DAE5
		public static void Text(Vector3 pos, Quaternion rot, string content, TextAlign align, TMP_FontAsset font)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(content, font, Draw.FontSize, align, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x0001F91F File Offset: 0x0001DB1F
		public static void Text(Vector3 pos, Quaternion rot, string content, float fontSize, TMP_FontAsset font)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(content, font, fontSize, Draw.TextAlign, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x0001F959 File Offset: 0x0001DB59
		public static void Text(Vector3 pos, Quaternion rot, string content, TextAlign align, float fontSize, TMP_FontAsset font)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(content, font, fontSize, align, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x0001F990 File Offset: 0x0001DB90
		public static void Text(Vector3 pos, Quaternion rot, string content, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(content, Draw.Font, Draw.FontSize, Draw.TextAlign, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x0001F9CD File Offset: 0x0001DBCD
		public static void Text(Vector3 pos, Quaternion rot, string content, TextAlign align, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(content, Draw.Font, Draw.FontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x0001FA07 File Offset: 0x0001DC07
		public static void Text(Vector3 pos, Quaternion rot, string content, float fontSize, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(content, Draw.Font, fontSize, Draw.TextAlign, color);
			Draw.PopMatrix();
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x0001FA41 File Offset: 0x0001DC41
		public static void Text(Vector3 pos, Quaternion rot, string content, TextAlign align, float fontSize, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(content, Draw.Font, fontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x0001FA78 File Offset: 0x0001DC78
		public static void Text(Vector3 pos, Quaternion rot, string content, TMP_FontAsset font, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(content, font, Draw.FontSize, Draw.TextAlign, color);
			Draw.PopMatrix();
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x0001FAB2 File Offset: 0x0001DCB2
		public static void Text(Vector3 pos, Quaternion rot, string content, TextAlign align, TMP_FontAsset font, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(content, font, Draw.FontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x0001FAE9 File Offset: 0x0001DCE9
		public static void Text(Vector3 pos, Quaternion rot, string content, float fontSize, TMP_FontAsset font, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(content, font, fontSize, Draw.TextAlign, color);
			Draw.PopMatrix();
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x0001FB20 File Offset: 0x0001DD20
		public static void Text(Vector3 pos, Quaternion rot, string content, TextAlign align, float fontSize, TMP_FontAsset font, Color color)
		{
			Draw.PushMatrix();
			Draw.Matrix *= Matrix4x4.TRS(pos, rot, Vector3.one);
			Draw.Text_Internal(content, font, fontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x0001FB54 File Offset: 0x0001DD54
		public static void Text(Vector3 pos, float angle, string content)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rotate(angle);
			Draw.Text_Internal(content, Draw.Font, Draw.FontSize, Draw.TextAlign, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x0001FB86 File Offset: 0x0001DD86
		public static void Text(Vector3 pos, float angle, string content, TextAlign align)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rotate(angle);
			Draw.Text_Internal(content, Draw.Font, Draw.FontSize, align, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x0001FBB4 File Offset: 0x0001DDB4
		public static void Text(Vector3 pos, float angle, string content, float fontSize)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rotate(angle);
			Draw.Text_Internal(content, Draw.Font, fontSize, Draw.TextAlign, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x0001FBE2 File Offset: 0x0001DDE2
		public static void Text(Vector3 pos, float angle, string content, TextAlign align, float fontSize)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rotate(angle);
			Draw.Text_Internal(content, Draw.Font, fontSize, align, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x0001FC0D File Offset: 0x0001DE0D
		public static void Text(Vector3 pos, float angle, string content, TMP_FontAsset font)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rotate(angle);
			Draw.Text_Internal(content, font, Draw.FontSize, Draw.TextAlign, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x0001FC3B File Offset: 0x0001DE3B
		public static void Text(Vector3 pos, float angle, string content, TextAlign align, TMP_FontAsset font)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rotate(angle);
			Draw.Text_Internal(content, font, Draw.FontSize, align, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x0001FC66 File Offset: 0x0001DE66
		public static void Text(Vector3 pos, float angle, string content, float fontSize, TMP_FontAsset font)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rotate(angle);
			Draw.Text_Internal(content, font, fontSize, Draw.TextAlign, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x0001FC91 File Offset: 0x0001DE91
		public static void Text(Vector3 pos, float angle, string content, TextAlign align, float fontSize, TMP_FontAsset font)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rotate(angle);
			Draw.Text_Internal(content, font, fontSize, align, Draw.Color);
			Draw.PopMatrix();
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x0001FCB9 File Offset: 0x0001DEB9
		public static void Text(Vector3 pos, float angle, string content, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rotate(angle);
			Draw.Text_Internal(content, Draw.Font, Draw.FontSize, Draw.TextAlign, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x0001FCE7 File Offset: 0x0001DEE7
		public static void Text(Vector3 pos, float angle, string content, TextAlign align, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rotate(angle);
			Draw.Text_Internal(content, Draw.Font, Draw.FontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x06000859 RID: 2137 RVA: 0x0001FD12 File Offset: 0x0001DF12
		public static void Text(Vector3 pos, float angle, string content, float fontSize, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rotate(angle);
			Draw.Text_Internal(content, Draw.Font, fontSize, Draw.TextAlign, color);
			Draw.PopMatrix();
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x0001FD3D File Offset: 0x0001DF3D
		public static void Text(Vector3 pos, float angle, string content, TextAlign align, float fontSize, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rotate(angle);
			Draw.Text_Internal(content, Draw.Font, fontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x0001FD65 File Offset: 0x0001DF65
		public static void Text(Vector3 pos, float angle, string content, TMP_FontAsset font, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rotate(angle);
			Draw.Text_Internal(content, font, Draw.FontSize, Draw.TextAlign, color);
			Draw.PopMatrix();
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x0001FD90 File Offset: 0x0001DF90
		public static void Text(Vector3 pos, float angle, string content, TextAlign align, TMP_FontAsset font, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rotate(angle);
			Draw.Text_Internal(content, font, Draw.FontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x0001FDB8 File Offset: 0x0001DFB8
		public static void Text(Vector3 pos, float angle, string content, float fontSize, TMP_FontAsset font, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rotate(angle);
			Draw.Text_Internal(content, font, fontSize, Draw.TextAlign, color);
			Draw.PopMatrix();
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x0001FDE0 File Offset: 0x0001DFE0
		public static void Text(Vector3 pos, float angle, string content, TextAlign align, float fontSize, TMP_FontAsset font, Color color)
		{
			Draw.PushMatrix();
			Draw.Translate(pos);
			Draw.Rotate(angle);
			Draw.Text_Internal(content, font, fontSize, align, color);
			Draw.PopMatrix();
		}

		// Token: 0x0600085F RID: 2143 RVA: 0x0001FE05 File Offset: 0x0001E005
		public static void Text(string content)
		{
			Draw.Text_Internal(content, Draw.Font, Draw.FontSize, Draw.TextAlign, Draw.Color);
		}

		// Token: 0x06000860 RID: 2144 RVA: 0x0001FE21 File Offset: 0x0001E021
		public static void Text(string content, TextAlign align)
		{
			Draw.Text_Internal(content, Draw.Font, Draw.FontSize, align, Draw.Color);
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x0001FE39 File Offset: 0x0001E039
		public static void Text(string content, float fontSize)
		{
			Draw.Text_Internal(content, Draw.Font, fontSize, Draw.TextAlign, Draw.Color);
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x0001FE51 File Offset: 0x0001E051
		public static void Text(string content, TextAlign align, float fontSize)
		{
			Draw.Text_Internal(content, Draw.Font, fontSize, align, Draw.Color);
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x0001FE65 File Offset: 0x0001E065
		public static void Text(string content, TMP_FontAsset font)
		{
			Draw.Text_Internal(content, font, Draw.FontSize, Draw.TextAlign, Draw.Color);
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x0001FE7D File Offset: 0x0001E07D
		public static void Text(string content, TextAlign align, TMP_FontAsset font)
		{
			Draw.Text_Internal(content, font, Draw.FontSize, align, Draw.Color);
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x0001FE91 File Offset: 0x0001E091
		public static void Text(string content, float fontSize, TMP_FontAsset font)
		{
			Draw.Text_Internal(content, font, fontSize, Draw.TextAlign, Draw.Color);
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x0001FEA5 File Offset: 0x0001E0A5
		public static void Text(string content, TextAlign align, float fontSize, TMP_FontAsset font)
		{
			Draw.Text_Internal(content, font, fontSize, align, Draw.Color);
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x0001FEB5 File Offset: 0x0001E0B5
		public static void Text(string content, Color color)
		{
			Draw.Text_Internal(content, Draw.Font, Draw.FontSize, Draw.TextAlign, color);
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x0001FECD File Offset: 0x0001E0CD
		public static void Text(string content, TextAlign align, Color color)
		{
			Draw.Text_Internal(content, Draw.Font, Draw.FontSize, align, color);
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x0001FEE1 File Offset: 0x0001E0E1
		public static void Text(string content, float fontSize, Color color)
		{
			Draw.Text_Internal(content, Draw.Font, fontSize, Draw.TextAlign, color);
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x0001FEF5 File Offset: 0x0001E0F5
		public static void Text(string content, TextAlign align, float fontSize, Color color)
		{
			Draw.Text_Internal(content, Draw.Font, fontSize, align, color);
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x0001FF05 File Offset: 0x0001E105
		public static void Text(string content, TMP_FontAsset font, Color color)
		{
			Draw.Text_Internal(content, font, Draw.FontSize, Draw.TextAlign, color);
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x0001FF19 File Offset: 0x0001E119
		public static void Text(string content, TextAlign align, TMP_FontAsset font, Color color)
		{
			Draw.Text_Internal(content, font, Draw.FontSize, align, color);
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x0001FF29 File Offset: 0x0001E129
		public static void Text(string content, float fontSize, TMP_FontAsset font, Color color)
		{
			Draw.Text_Internal(content, font, fontSize, Draw.TextAlign, color);
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x0001FF39 File Offset: 0x0001E139
		public static void Text(string content, TextAlign align, float fontSize, TMP_FontAsset font, Color color)
		{
			Draw.Text_Internal(content, font, fontSize, align, color);
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x0001FF48 File Offset: 0x0001E148
		static Draw()
		{
			Draw.ResetAllDrawStates();
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x0001FFF0 File Offset: 0x0001E1F0
		public static void ResetAllDrawStates()
		{
			Draw.ResetMatrix();
			Draw.ResetStyle();
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000871 RID: 2161 RVA: 0x0001FFFC File Offset: 0x0001E1FC
		public static StateStack Scope
		{
			get
			{
				return new StateStack(Draw.style, Draw.matrix);
			}
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x0002000D File Offset: 0x0001E20D
		public static void Push()
		{
			StateStack.Push(Draw.style, Draw.matrix);
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x0002001E File Offset: 0x0001E21E
		public static void Pop()
		{
			StateStack.Pop();
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000874 RID: 2164 RVA: 0x00020025 File Offset: 0x0001E225
		// (set) Token: 0x06000875 RID: 2165 RVA: 0x0002002C File Offset: 0x0001E22C
		public static Matrix4x4 Matrix
		{
			get
			{
				return Draw.matrix;
			}
			set
			{
				Draw.matrix = value;
			}
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x00020034 File Offset: 0x0001E234
		public static void ResetMatrix()
		{
			Draw.matrix = Matrix4x4.identity;
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000877 RID: 2167 RVA: 0x00020040 File Offset: 0x0001E240
		public static MatrixStack MatrixScope
		{
			get
			{
				return new MatrixStack(Draw.Matrix);
			}
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x0002004C File Offset: 0x0001E24C
		public static void PushMatrix()
		{
			MatrixStack.Push(Draw.Matrix);
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x00020058 File Offset: 0x0001E258
		public static void PopMatrix()
		{
			MatrixStack.Pop();
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x0002005F File Offset: 0x0001E25F
		public static void ApplyMatrix(Matrix4x4 matrix)
		{
			Draw.Matrix *= matrix;
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x0600087B RID: 2171 RVA: 0x00020071 File Offset: 0x0001E271
		// (set) Token: 0x0600087C RID: 2172 RVA: 0x00020096 File Offset: 0x0001E296
		public static Vector3 Position
		{
			get
			{
				return new Vector3(Draw.matrix.m03, Draw.matrix.m13, Draw.matrix.m23);
			}
			set
			{
				Draw.matrix.m03 = value.x;
				Draw.matrix.m13 = value.y;
				Draw.matrix.m23 = value.z;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600087D RID: 2173 RVA: 0x000200C8 File Offset: 0x0001E2C8
		// (set) Token: 0x0600087E RID: 2174 RVA: 0x000200E3 File Offset: 0x0001E2E3
		public static Vector2 Position2D
		{
			get
			{
				return new Vector2(Draw.matrix.m03, Draw.matrix.m13);
			}
			set
			{
				Draw.matrix.m03 = value.x;
				Draw.matrix.m13 = value.y;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600087F RID: 2175 RVA: 0x00020105 File Offset: 0x0001E305
		// (set) Token: 0x06000880 RID: 2176 RVA: 0x0002010C File Offset: 0x0001E30C
		[Obsolete("Please use Draw.Position instead (I done messed up, did a typo, I'm sorry~)", true)]
		public static Vector3 Postition
		{
			get
			{
				return Draw.Position;
			}
			set
			{
				Draw.Position = value;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000881 RID: 2177 RVA: 0x00020114 File Offset: 0x0001E314
		// (set) Token: 0x06000882 RID: 2178 RVA: 0x0002011B File Offset: 0x0001E31B
		[Obsolete("Please use Draw.Position2D instead (I done messed up, did a typo, I'm sorry~)", true)]
		public static Vector2 Postition2D
		{
			get
			{
				return Draw.Position2D;
			}
			set
			{
				Draw.Position2D = value;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000883 RID: 2179 RVA: 0x00020123 File Offset: 0x0001E323
		// (set) Token: 0x06000884 RID: 2180 RVA: 0x0002012F File Offset: 0x0001E32F
		public static Quaternion Rotation
		{
			get
			{
				return Draw.matrix.rotation;
			}
			set
			{
				Draw.MtxSetRotationKeepScale(ref Draw.matrix, value);
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000885 RID: 2181 RVA: 0x0002013C File Offset: 0x0001E33C
		// (set) Token: 0x06000886 RID: 2182 RVA: 0x0002014D File Offset: 0x0001E34D
		public static float Angle2D
		{
			get
			{
				return ShapesMath.DirToAng(Draw.RightBasis);
			}
			set
			{
				Draw.MtxRotateZLhs(ref Draw.matrix, value - Draw.Angle2D);
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000887 RID: 2183 RVA: 0x00020160 File Offset: 0x0001E360
		public static Vector3 Right
		{
			get
			{
				return Draw.RightBasis.normalized;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000888 RID: 2184 RVA: 0x0002017C File Offset: 0x0001E37C
		public static Vector3 Up
		{
			get
			{
				return Draw.UpBasis.normalized;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000889 RID: 2185 RVA: 0x00020198 File Offset: 0x0001E398
		public static Vector3 Forward
		{
			get
			{
				return Draw.ForwardBasis.normalized;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600088A RID: 2186 RVA: 0x000201B2 File Offset: 0x0001E3B2
		public static Vector3 RightBasis
		{
			get
			{
				return Draw.matrix.GetColumn(0);
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x0600088B RID: 2187 RVA: 0x000201C4 File Offset: 0x0001E3C4
		public static Vector3 UpBasis
		{
			get
			{
				return Draw.matrix.GetColumn(1);
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x0600088C RID: 2188 RVA: 0x000201D6 File Offset: 0x0001E3D6
		public static Vector3 ForwardBasis
		{
			get
			{
				return Draw.matrix.GetColumn(2);
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x0600088D RID: 2189 RVA: 0x000201E8 File Offset: 0x0001E3E8
		// (set) Token: 0x0600088E RID: 2190 RVA: 0x00020224 File Offset: 0x0001E424
		public static Vector3 LocalScale
		{
			get
			{
				return new Vector3(Draw.RightBasis.magnitude, Draw.UpBasis.magnitude, Draw.ForwardBasis.magnitude);
			}
			set
			{
				float num = value.x / Draw.RightBasis.magnitude;
				float num2 = value.y / Draw.UpBasis.magnitude;
				float num3 = value.z / Draw.ForwardBasis.magnitude;
				Draw.Scale(num, num2, num3);
			}
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x00020276 File Offset: 0x0001E476
		public static void Translate(float x, float y)
		{
			Draw.MtxTranslateXY(ref Draw.matrix, (double)x, (double)y);
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x00020286 File Offset: 0x0001E486
		public static void Translate(float x, float y, float z)
		{
			Draw.MtxTranslateXYZ(ref Draw.matrix, (double)x, (double)y, (double)z);
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x00020298 File Offset: 0x0001E498
		public static void Translate(Vector2 displacement)
		{
			Draw.Translate(displacement.x, displacement.y);
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x000202AB File Offset: 0x0001E4AB
		public static void Translate(Vector3 displacement)
		{
			Draw.Translate(displacement.x, displacement.y, displacement.z);
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x000202C4 File Offset: 0x0001E4C4
		public static void Rotate(float angle)
		{
			Draw.MtxRotateZ(ref Draw.matrix, angle);
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x000202D1 File Offset: 0x0001E4D1
		public static void Rotate(float x, float y, float z)
		{
			Draw.Rotate(Quaternion.Euler(x * 57.29578f, y * 57.29578f, z * 57.29578f));
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x000202F2 File Offset: 0x0001E4F2
		public static void Rotate(float angle, Vector3 axis)
		{
			Draw.Rotate(Quaternion.AngleAxis(angle * 57.29578f, axis));
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x00020306 File Offset: 0x0001E506
		public static void Rotate(Quaternion rotation)
		{
			Draw.matrix *= Matrix4x4.Rotate(rotation);
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x0002031D File Offset: 0x0001E51D
		public static void Scale(float uniformScale)
		{
			Draw.MtxScaleXYZ(ref Draw.matrix, (double)uniformScale, (double)uniformScale, (double)uniformScale);
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x0002032F File Offset: 0x0001E52F
		public static void Scale(float x, float y)
		{
			Draw.MtxScaleXY(ref Draw.matrix, (double)x, (double)y);
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x0002033F File Offset: 0x0001E53F
		public static void Scale(float x, float y, float z)
		{
			Draw.MtxScaleXYZ(ref Draw.matrix, (double)x, (double)y, (double)z);
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x00020351 File Offset: 0x0001E551
		public static void Scale(Vector2 scale)
		{
			Draw.Scale(scale.x, scale.y);
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x00020364 File Offset: 0x0001E564
		public static void Scale(Vector3 scale)
		{
			Draw.Scale(scale.x, scale.y, scale.z);
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x0002037D File Offset: 0x0001E57D
		public static void SetMatrix(Matrix4x4 matrix)
		{
			Draw.Matrix = matrix;
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x00020385 File Offset: 0x0001E585
		public static void SetMatrix(Vector3 position, Quaternion rotation, Vector3 scale)
		{
			Draw.Matrix = Matrix4x4.TRS(position, rotation, scale);
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x00020394 File Offset: 0x0001E594
		public static void SetMatrix(Transform transform)
		{
			Draw.Matrix = transform.localToWorldMatrix;
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x000203A4 File Offset: 0x0001E5A4
		private static void MtxSetRotationKeepScale(ref Matrix4x4 m, Quaternion rotation)
		{
			Matrix4x4 matrix4x = Matrix4x4.Rotate(rotation);
			float magnitude = m.GetColumn(0).magnitude;
			float magnitude2 = m.GetColumn(1).magnitude;
			float magnitude3 = m.GetColumn(2).magnitude;
			m.m00 = matrix4x.m00 * magnitude;
			m.m10 = matrix4x.m10 * magnitude;
			m.m20 = matrix4x.m20 * magnitude;
			m.m01 = matrix4x.m01 * magnitude2;
			m.m11 = matrix4x.m11 * magnitude2;
			m.m21 = matrix4x.m21 * magnitude2;
			m.m02 = matrix4x.m02 * magnitude3;
			m.m12 = matrix4x.m12 * magnitude3;
			m.m22 = matrix4x.m22 * magnitude3;
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x00020478 File Offset: 0x0001E678
		private static void MtxRotateZLhs(ref Matrix4x4 rhs, float a)
		{
			double num = Math.Cos((double)a);
			double num2 = Math.Sin((double)a);
			double num3 = (double)rhs.m00;
			double num4 = (double)rhs.m01;
			double num5 = (double)rhs.m02;
			double num6 = (double)rhs.m03;
			rhs.m00 = (float)(num * num3 - num2 * (double)rhs.m10);
			rhs.m01 = (float)(num * num4 - num2 * (double)rhs.m11);
			rhs.m02 = (float)(num * num5 - num2 * (double)rhs.m12);
			rhs.m03 = (float)(num * num6 - num2 * (double)rhs.m13);
			rhs.m10 = (float)(num2 * num3 + num * (double)rhs.m10);
			rhs.m11 = (float)(num2 * num4 + num * (double)rhs.m11);
			rhs.m12 = (float)(num2 * num5 + num * (double)rhs.m12);
			rhs.m13 = (float)(num2 * num6 + num * (double)rhs.m13);
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x0002055C File Offset: 0x0001E75C
		private static void MtxTranslateXYZ(ref Matrix4x4 lhs, double x, double y, double z)
		{
			lhs.m03 = (float)((double)lhs.m00 * x + (double)lhs.m01 * y + (double)lhs.m02 * z + (double)lhs.m03);
			lhs.m13 = (float)((double)lhs.m10 * x + (double)lhs.m11 * y + (double)lhs.m12 * z + (double)lhs.m13);
			lhs.m23 = (float)((double)lhs.m20 * x + (double)lhs.m21 * y + (double)lhs.m22 * z + (double)lhs.m23);
		}

		// Token: 0x060008A2 RID: 2210 RVA: 0x000205F0 File Offset: 0x0001E7F0
		private static void MtxTranslateXY(ref Matrix4x4 lhs, double x, double y)
		{
			lhs.m03 = (float)((double)lhs.m00 * x + (double)lhs.m01 * y + (double)lhs.m03);
			lhs.m13 = (float)((double)lhs.m10 * x + (double)lhs.m11 * y + (double)lhs.m13);
			lhs.m23 = (float)((double)lhs.m20 * x + (double)lhs.m21 * y + (double)lhs.m23);
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x00020664 File Offset: 0x0001E864
		private static void MtxRotateZ(ref Matrix4x4 lhs, float a)
		{
			double num = Math.Cos((double)a);
			double num2 = Math.Sin((double)a);
			float m = lhs.m00;
			float m2 = lhs.m01;
			float m3 = lhs.m10;
			float m4 = lhs.m11;
			float m5 = lhs.m20;
			float m6 = lhs.m21;
			lhs.m00 = (float)((double)m * num + (double)m2 * num2);
			lhs.m01 = (float)((double)m * -(float)num2 + (double)m2 * num);
			lhs.m10 = (float)((double)m3 * num + (double)m4 * num2);
			lhs.m11 = (float)((double)m3 * -(float)num2 + (double)m4 * num);
			lhs.m20 = (float)((double)m5 * num + (double)m6 * num2);
			lhs.m21 = (float)((double)m5 * -(float)num2 + (double)m6 * num);
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x0002071C File Offset: 0x0001E91C
		private static void MtxScaleXYZ(ref Matrix4x4 m, double x, double y, double z)
		{
			m.m00 = (float)((double)m.m00 * x);
			m.m10 = (float)((double)m.m10 * x);
			m.m20 = (float)((double)m.m20 * x);
			m.m01 = (float)((double)m.m01 * y);
			m.m11 = (float)((double)m.m11 * y);
			m.m21 = (float)((double)m.m21 * y);
			m.m02 = (float)((double)m.m02 * z);
			m.m12 = (float)((double)m.m12 * z);
			m.m22 = (float)((double)m.m22 * z);
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x000207BC File Offset: 0x0001E9BC
		private static void MtxScaleXY(ref Matrix4x4 m, double x, double y)
		{
			m.m00 = (float)((double)m.m00 * x);
			m.m10 = (float)((double)m.m10 * x);
			m.m20 = (float)((double)m.m20 * x);
			m.m01 = (float)((double)m.m01 * y);
			m.m11 = (float)((double)m.m11 * y);
			m.m21 = (float)((double)m.m21 * y);
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x00020829 File Offset: 0x0001EA29
		private static void MtxResetToXYZ(out Matrix4x4 m, float x, float y, float z)
		{
			m = Matrix4x4.identity;
			m.m03 = x;
			m.m13 = y;
			m.m23 = z;
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x0002084B File Offset: 0x0001EA4B
		private static void MtxResetToXY(out Matrix4x4 m, float x, float y)
		{
			m = Matrix4x4.identity;
			m.m03 = x;
			m.m13 = y;
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x00020866 File Offset: 0x0001EA66
		private static void MtxResetToPosXYatAngle(out Matrix4x4 lhs, float x, float y, float a)
		{
			Draw.MtxResetToXY(out lhs, x, y);
			Draw.MtxResetScaleSetAngleZ(ref lhs, a);
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x00020877 File Offset: 0x0001EA77
		private static void MtxResetToPosXYatDirection(out Matrix4x4 lhs, float x, float y, Vector2 dir)
		{
			Draw.MtxResetToXY(out lhs, x, y);
			Draw.MtxResetScaleSetDirX(ref lhs, dir);
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x00020888 File Offset: 0x0001EA88
		private static void MtxResetScaleSetAngleZ(ref Matrix4x4 lhs, float a)
		{
			float num = Mathf.Cos(a);
			float num2 = Mathf.Sin(a);
			lhs.m00 = num;
			lhs.m10 = num2;
			lhs.m20 = 0f;
			lhs.m01 = -num2;
			lhs.m11 = num;
			lhs.m21 = 0f;
			lhs.m02 = 0f;
			lhs.m12 = 0f;
			lhs.m22 = 1f;
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x000208F8 File Offset: 0x0001EAF8
		private static void MtxResetScaleSetDirX(ref Matrix4x4 lhs, Vector2 dir)
		{
			dir.Normalize();
			lhs.m00 = dir.x;
			lhs.m10 = dir.y;
			lhs.m20 = 0f;
			lhs.m01 = -dir.y;
			lhs.m11 = dir.x;
			lhs.m21 = 0f;
			lhs.m02 = 0f;
			lhs.m12 = 0f;
			lhs.m22 = 1f;
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x00020974 File Offset: 0x0001EB74
		public static void ResetStyle()
		{
			Draw.style = DrawStyle.@default;
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060008AD RID: 2221 RVA: 0x00020980 File Offset: 0x0001EB80
		public static StyleStack StyleScope
		{
			get
			{
				return new StyleStack(Draw.style);
			}
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x0002098C File Offset: 0x0001EB8C
		public static void PushStyle()
		{
			StyleStack.Push(Draw.style);
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x00020998 File Offset: 0x0001EB98
		public static void PopStyle()
		{
			StyleStack.Pop();
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060008B0 RID: 2224 RVA: 0x0002099F File Offset: 0x0001EB9F
		public static ColorStack ColorScope
		{
			get
			{
				return new ColorStack(Draw.style.color);
			}
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x000209B0 File Offset: 0x0001EBB0
		public static void PushColor()
		{
			ColorStack.Push(Draw.style.color);
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x000209C1 File Offset: 0x0001EBC1
		public static void PopColor()
		{
			ColorStack.Pop();
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060008B3 RID: 2227 RVA: 0x000209C8 File Offset: 0x0001EBC8
		// (set) Token: 0x060008B4 RID: 2228 RVA: 0x000209D9 File Offset: 0x0001EBD9
		public static CompareFunction ZTest
		{
			get
			{
				return Draw.style.renderState.zTest;
			}
			set
			{
				Draw.style.renderState.zTest = value;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060008B5 RID: 2229 RVA: 0x000209EB File Offset: 0x0001EBEB
		// (set) Token: 0x060008B6 RID: 2230 RVA: 0x000209FC File Offset: 0x0001EBFC
		public static float ZOffsetFactor
		{
			get
			{
				return Draw.style.renderState.zOffsetFactor;
			}
			set
			{
				Draw.style.renderState.zOffsetFactor = value;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060008B7 RID: 2231 RVA: 0x00020A0E File Offset: 0x0001EC0E
		// (set) Token: 0x060008B8 RID: 2232 RVA: 0x00020A1F File Offset: 0x0001EC1F
		public static int ZOffsetUnits
		{
			get
			{
				return Draw.style.renderState.zOffsetUnits;
			}
			set
			{
				Draw.style.renderState.zOffsetUnits = value;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060008B9 RID: 2233 RVA: 0x00020A31 File Offset: 0x0001EC31
		// (set) Token: 0x060008BA RID: 2234 RVA: 0x00020A42 File Offset: 0x0001EC42
		public static CompareFunction StencilComp
		{
			get
			{
				return Draw.style.renderState.stencilComp;
			}
			set
			{
				Draw.style.renderState.stencilComp = value;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060008BB RID: 2235 RVA: 0x00020A54 File Offset: 0x0001EC54
		// (set) Token: 0x060008BC RID: 2236 RVA: 0x00020A65 File Offset: 0x0001EC65
		public static StencilOp StencilOpPass
		{
			get
			{
				return Draw.style.renderState.stencilOpPass;
			}
			set
			{
				Draw.style.renderState.stencilOpPass = value;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060008BD RID: 2237 RVA: 0x00020A77 File Offset: 0x0001EC77
		// (set) Token: 0x060008BE RID: 2238 RVA: 0x00020A88 File Offset: 0x0001EC88
		public static byte StencilRefID
		{
			get
			{
				return Draw.style.renderState.stencilRefID;
			}
			set
			{
				Draw.style.renderState.stencilRefID = value;
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060008BF RID: 2239 RVA: 0x00020A9A File Offset: 0x0001EC9A
		// (set) Token: 0x060008C0 RID: 2240 RVA: 0x00020AAB File Offset: 0x0001ECAB
		public static byte StencilReadMask
		{
			get
			{
				return Draw.style.renderState.stencilReadMask;
			}
			set
			{
				Draw.style.renderState.stencilReadMask = value;
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060008C1 RID: 2241 RVA: 0x00020ABD File Offset: 0x0001ECBD
		// (set) Token: 0x060008C2 RID: 2242 RVA: 0x00020ACE File Offset: 0x0001ECCE
		public static byte StencilWriteMask
		{
			get
			{
				return Draw.style.renderState.stencilWriteMask;
			}
			set
			{
				Draw.style.renderState.stencilWriteMask = value;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060008C3 RID: 2243 RVA: 0x00020AE0 File Offset: 0x0001ECE0
		// (set) Token: 0x060008C4 RID: 2244 RVA: 0x00020AEC File Offset: 0x0001ECEC
		public static Color Color
		{
			get
			{
				return Draw.style.color;
			}
			set
			{
				Draw.style.color = value;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060008C5 RID: 2245 RVA: 0x00020AF9 File Offset: 0x0001ECF9
		// (set) Token: 0x060008C6 RID: 2246 RVA: 0x00020B08 File Offset: 0x0001ED08
		public static float Opacity
		{
			get
			{
				return Draw.Color.a;
			}
			set
			{
				Color color = Draw.Color;
				color.a = value;
				Draw.Color = color;
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060008C7 RID: 2247 RVA: 0x00020B29 File Offset: 0x0001ED29
		// (set) Token: 0x060008C8 RID: 2248 RVA: 0x00020B35 File Offset: 0x0001ED35
		public static ShapesBlendMode BlendMode
		{
			get
			{
				return Draw.style.blendMode;
			}
			set
			{
				Draw.style.blendMode = value;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060008C9 RID: 2249 RVA: 0x00020B42 File Offset: 0x0001ED42
		// (set) Token: 0x060008CA RID: 2250 RVA: 0x00020B4E File Offset: 0x0001ED4E
		public static ScaleMode ScaleMode
		{
			get
			{
				return Draw.style.scaleMode;
			}
			set
			{
				Draw.style.scaleMode = value;
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060008CB RID: 2251 RVA: 0x00020B5B File Offset: 0x0001ED5B
		// (set) Token: 0x060008CC RID: 2252 RVA: 0x00020B67 File Offset: 0x0001ED67
		public static DetailLevel DetailLevel
		{
			get
			{
				return Draw.style.detailLevel;
			}
			set
			{
				Draw.style.detailLevel = value;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060008CD RID: 2253 RVA: 0x00020B74 File Offset: 0x0001ED74
		// (set) Token: 0x060008CE RID: 2254 RVA: 0x00020B80 File Offset: 0x0001ED80
		public static float LineThickness
		{
			get
			{
				return Draw.style.lineThickness;
			}
			set
			{
				Draw.style.lineThickness = value;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060008CF RID: 2255 RVA: 0x00020B8D File Offset: 0x0001ED8D
		// (set) Token: 0x060008D0 RID: 2256 RVA: 0x00020B99 File Offset: 0x0001ED99
		public static ThicknessSpace LineThicknessSpace
		{
			get
			{
				return Draw.style.lineThicknessSpace;
			}
			set
			{
				Draw.style.lineThicknessSpace = value;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060008D1 RID: 2257 RVA: 0x00020BA6 File Offset: 0x0001EDA6
		// (set) Token: 0x060008D2 RID: 2258 RVA: 0x00020BB2 File Offset: 0x0001EDB2
		public static LineEndCap LineEndCaps
		{
			get
			{
				return Draw.style.lineEndCaps;
			}
			set
			{
				Draw.style.lineEndCaps = value;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060008D3 RID: 2259 RVA: 0x00020BBF File Offset: 0x0001EDBF
		// (set) Token: 0x060008D4 RID: 2260 RVA: 0x00020BCB File Offset: 0x0001EDCB
		public static LineGeometry LineGeometry
		{
			get
			{
				return Draw.style.lineGeometry;
			}
			set
			{
				Draw.style.lineGeometry = value;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060008D5 RID: 2261 RVA: 0x00020BD8 File Offset: 0x0001EDD8
		// (set) Token: 0x060008D6 RID: 2262 RVA: 0x00020BE4 File Offset: 0x0001EDE4
		public static PolygonTriangulation PolygonTriangulation
		{
			get
			{
				return Draw.style.polygonTriangulation;
			}
			set
			{
				Draw.style.polygonTriangulation = value;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060008D7 RID: 2263 RVA: 0x00020BF1 File Offset: 0x0001EDF1
		// (set) Token: 0x060008D8 RID: 2264 RVA: 0x00020BFD File Offset: 0x0001EDFD
		public static ShapeFill PolygonShapeFill
		{
			get
			{
				return Draw.style.polygonShapeFill;
			}
			set
			{
				Draw.style.polygonShapeFill = value;
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060008D9 RID: 2265 RVA: 0x00020C0A File Offset: 0x0001EE0A
		// (set) Token: 0x060008DA RID: 2266 RVA: 0x00020C16 File Offset: 0x0001EE16
		public static DashStyle LineDashStyle
		{
			get
			{
				return Draw.style.lineDashStyle;
			}
			set
			{
				Draw.style.lineDashStyle = value;
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060008DB RID: 2267 RVA: 0x00020C23 File Offset: 0x0001EE23
		// (set) Token: 0x060008DC RID: 2268 RVA: 0x00020C2F File Offset: 0x0001EE2F
		public static DashStyle RingDashStyle
		{
			get
			{
				return Draw.style.ringDashStyle;
			}
			set
			{
				Draw.style.ringDashStyle = value;
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060008DD RID: 2269 RVA: 0x00020C3C File Offset: 0x0001EE3C
		// (set) Token: 0x060008DE RID: 2270 RVA: 0x00020C48 File Offset: 0x0001EE48
		[Obsolete("please use Draw.LineDashStyle.UniformSize or Draw.LineDashStyle.size instead", false)]
		public static float LineDashSize
		{
			get
			{
				return Draw.LineDashStyle.UniformSize;
			}
			set
			{
				Draw.LineDashStyle.UniformSize = value;
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060008DF RID: 2271 RVA: 0x00020C55 File Offset: 0x0001EE55
		// (set) Token: 0x060008E0 RID: 2272 RVA: 0x00020C61 File Offset: 0x0001EE61
		public static PolylineGeometry PolylineGeometry
		{
			get
			{
				return Draw.style.polylineGeometry;
			}
			set
			{
				Draw.style.polylineGeometry = value;
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060008E1 RID: 2273 RVA: 0x00020C6E File Offset: 0x0001EE6E
		// (set) Token: 0x060008E2 RID: 2274 RVA: 0x00020C7A File Offset: 0x0001EE7A
		public static PolylineJoins PolylineJoins
		{
			get
			{
				return Draw.style.polylineJoins;
			}
			set
			{
				Draw.style.polylineJoins = value;
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060008E3 RID: 2275 RVA: 0x00020C87 File Offset: 0x0001EE87
		// (set) Token: 0x060008E4 RID: 2276 RVA: 0x00020C93 File Offset: 0x0001EE93
		public static float DiscRadius
		{
			get
			{
				return Draw.style.discRadius;
			}
			set
			{
				Draw.style.discRadius = value;
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060008E5 RID: 2277 RVA: 0x00020CA0 File Offset: 0x0001EEA0
		// (set) Token: 0x060008E6 RID: 2278 RVA: 0x00020CAC File Offset: 0x0001EEAC
		public static DiscGeometry DiscGeometry
		{
			get
			{
				return Draw.style.discGeometry;
			}
			set
			{
				Draw.style.discGeometry = value;
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060008E7 RID: 2279 RVA: 0x00020CB9 File Offset: 0x0001EEB9
		// (set) Token: 0x060008E8 RID: 2280 RVA: 0x00020CC5 File Offset: 0x0001EEC5
		public static float RingThickness
		{
			get
			{
				return Draw.style.ringThickness;
			}
			set
			{
				Draw.style.ringThickness = value;
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060008E9 RID: 2281 RVA: 0x00020CD2 File Offset: 0x0001EED2
		// (set) Token: 0x060008EA RID: 2282 RVA: 0x00020CDE File Offset: 0x0001EEDE
		public static ThicknessSpace RingThicknessSpace
		{
			get
			{
				return Draw.style.ringThicknessSpace;
			}
			set
			{
				Draw.style.ringThicknessSpace = value;
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060008EB RID: 2283 RVA: 0x00020CEB File Offset: 0x0001EEEB
		// (set) Token: 0x060008EC RID: 2284 RVA: 0x00020CF7 File Offset: 0x0001EEF7
		public static ThicknessSpace DiscRadiusSpace
		{
			get
			{
				return Draw.style.discRadiusSpace;
			}
			set
			{
				Draw.style.discRadiusSpace = value;
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060008ED RID: 2285 RVA: 0x00020D04 File Offset: 0x0001EF04
		// (set) Token: 0x060008EE RID: 2286 RVA: 0x00020D10 File Offset: 0x0001EF10
		public static float RegularPolygonRadius
		{
			get
			{
				return Draw.style.regularPolygonRadius;
			}
			set
			{
				Draw.style.regularPolygonRadius = value;
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060008EF RID: 2287 RVA: 0x00020D1D File Offset: 0x0001EF1D
		// (set) Token: 0x060008F0 RID: 2288 RVA: 0x00020D29 File Offset: 0x0001EF29
		public static int RegularPolygonSideCount
		{
			get
			{
				return Draw.style.regularPolygonSideCount;
			}
			set
			{
				Draw.style.regularPolygonSideCount = value;
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060008F1 RID: 2289 RVA: 0x00020D36 File Offset: 0x0001EF36
		// (set) Token: 0x060008F2 RID: 2290 RVA: 0x00020D42 File Offset: 0x0001EF42
		public static RegularPolygonGeometry RegularPolygonGeometry
		{
			get
			{
				return Draw.style.regularPolygonGeometry;
			}
			set
			{
				Draw.style.regularPolygonGeometry = value;
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060008F3 RID: 2291 RVA: 0x00020D4F File Offset: 0x0001EF4F
		// (set) Token: 0x060008F4 RID: 2292 RVA: 0x00020D5B File Offset: 0x0001EF5B
		public static float RegularPolygonThickness
		{
			get
			{
				return Draw.style.regularPolygonThickness;
			}
			set
			{
				Draw.style.regularPolygonThickness = value;
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060008F5 RID: 2293 RVA: 0x00020D68 File Offset: 0x0001EF68
		// (set) Token: 0x060008F6 RID: 2294 RVA: 0x00020D74 File Offset: 0x0001EF74
		public static ThicknessSpace RegularPolygonThicknessSpace
		{
			get
			{
				return Draw.style.regularPolygonThicknessSpace;
			}
			set
			{
				Draw.style.regularPolygonThicknessSpace = value;
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060008F7 RID: 2295 RVA: 0x00020D81 File Offset: 0x0001EF81
		// (set) Token: 0x060008F8 RID: 2296 RVA: 0x00020D8D File Offset: 0x0001EF8D
		public static ThicknessSpace RegularPolygonRadiusSpace
		{
			get
			{
				return Draw.style.regularPolygonRadiusSpace;
			}
			set
			{
				Draw.style.regularPolygonRadiusSpace = value;
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060008F9 RID: 2297 RVA: 0x00020D9A File Offset: 0x0001EF9A
		// (set) Token: 0x060008FA RID: 2298 RVA: 0x00020DA6 File Offset: 0x0001EFA6
		public static ShapeFill RegularPolygonShapeFill
		{
			get
			{
				return Draw.style.regularPolygonShapeFill;
			}
			set
			{
				Draw.style.regularPolygonShapeFill = value;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060008FB RID: 2299 RVA: 0x00020DB3 File Offset: 0x0001EFB3
		// (set) Token: 0x060008FC RID: 2300 RVA: 0x00020DBF File Offset: 0x0001EFBF
		public static float RectangleThickness
		{
			get
			{
				return Draw.style.rectangleThickness;
			}
			set
			{
				Draw.style.rectangleThickness = value;
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060008FD RID: 2301 RVA: 0x00020DCC File Offset: 0x0001EFCC
		// (set) Token: 0x060008FE RID: 2302 RVA: 0x00020DD8 File Offset: 0x0001EFD8
		public static ThicknessSpace RectangleThicknessSpace
		{
			get
			{
				return Draw.style.rectangleThicknessSpace;
			}
			set
			{
				Draw.style.rectangleThicknessSpace = value;
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060008FF RID: 2303 RVA: 0x00020DE5 File Offset: 0x0001EFE5
		// (set) Token: 0x06000900 RID: 2304 RVA: 0x00020DF1 File Offset: 0x0001EFF1
		public static ShapeFill RectangleShapeFill
		{
			get
			{
				return Draw.style.rectangleShapeFill;
			}
			set
			{
				Draw.style.rectangleShapeFill = value;
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000901 RID: 2305 RVA: 0x00020DFE File Offset: 0x0001EFFE
		// (set) Token: 0x06000902 RID: 2306 RVA: 0x00020E0A File Offset: 0x0001F00A
		public static float TriangleThickness
		{
			get
			{
				return Draw.style.triangleThickness;
			}
			set
			{
				Draw.style.triangleThickness = value;
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000903 RID: 2307 RVA: 0x00020E17 File Offset: 0x0001F017
		// (set) Token: 0x06000904 RID: 2308 RVA: 0x00020E23 File Offset: 0x0001F023
		public static ThicknessSpace TriangleThicknessSpace
		{
			get
			{
				return Draw.style.triangleThicknessSpace;
			}
			set
			{
				Draw.style.triangleThicknessSpace = value;
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000905 RID: 2309 RVA: 0x00020E30 File Offset: 0x0001F030
		// (set) Token: 0x06000906 RID: 2310 RVA: 0x00020E3C File Offset: 0x0001F03C
		public static float SphereRadius
		{
			get
			{
				return Draw.style.sphereRadius;
			}
			set
			{
				Draw.style.sphereRadius = value;
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000907 RID: 2311 RVA: 0x00020E49 File Offset: 0x0001F049
		// (set) Token: 0x06000908 RID: 2312 RVA: 0x00020E55 File Offset: 0x0001F055
		public static ThicknessSpace SphereRadiusSpace
		{
			get
			{
				return Draw.style.sphereRadiusSpace;
			}
			set
			{
				Draw.style.sphereRadiusSpace = value;
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000909 RID: 2313 RVA: 0x00020E62 File Offset: 0x0001F062
		// (set) Token: 0x0600090A RID: 2314 RVA: 0x00020E6E File Offset: 0x0001F06E
		public static ThicknessSpace CuboidSizeSpace
		{
			get
			{
				return Draw.style.cuboidSizeSpace;
			}
			set
			{
				Draw.style.cuboidSizeSpace = value;
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x0600090B RID: 2315 RVA: 0x00020E7B File Offset: 0x0001F07B
		// (set) Token: 0x0600090C RID: 2316 RVA: 0x00020E87 File Offset: 0x0001F087
		public static ThicknessSpace TorusThicknessSpace
		{
			get
			{
				return Draw.style.torusThicknessSpace;
			}
			set
			{
				Draw.style.torusThicknessSpace = value;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x0600090D RID: 2317 RVA: 0x00020E94 File Offset: 0x0001F094
		// (set) Token: 0x0600090E RID: 2318 RVA: 0x00020EA0 File Offset: 0x0001F0A0
		public static ThicknessSpace TorusRadiusSpace
		{
			get
			{
				return Draw.style.torusRadiusSpace;
			}
			set
			{
				Draw.style.torusRadiusSpace = value;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x0600090F RID: 2319 RVA: 0x00020EAD File Offset: 0x0001F0AD
		// (set) Token: 0x06000910 RID: 2320 RVA: 0x00020EB9 File Offset: 0x0001F0B9
		public static ThicknessSpace ConeSizeSpace
		{
			get
			{
				return Draw.style.coneSizeSpace;
			}
			set
			{
				Draw.style.coneSizeSpace = value;
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000911 RID: 2321 RVA: 0x00020EC6 File Offset: 0x0001F0C6
		// (set) Token: 0x06000912 RID: 2322 RVA: 0x00020ED2 File Offset: 0x0001F0D2
		public static TMP_FontAsset Font
		{
			get
			{
				return Draw.style.font;
			}
			set
			{
				Draw.style.font = value;
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000913 RID: 2323 RVA: 0x00020EDF File Offset: 0x0001F0DF
		// (set) Token: 0x06000914 RID: 2324 RVA: 0x00020EEB File Offset: 0x0001F0EB
		public static float FontSize
		{
			get
			{
				return Draw.style.fontSize;
			}
			set
			{
				Draw.style.fontSize = value;
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000915 RID: 2325 RVA: 0x00020EF8 File Offset: 0x0001F0F8
		// (set) Token: 0x06000916 RID: 2326 RVA: 0x00020F04 File Offset: 0x0001F104
		public static TextAlign TextAlign
		{
			get
			{
				return Draw.style.textAlign;
			}
			set
			{
				Draw.style.textAlign = value;
			}
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x00020F14 File Offset: 0x0001F114
		[CompilerGenerated]
		internal static void <Polyline_Internal>g__ApplyToMpb|5_0(MpbPolyline mpb, ref Draw.<>c__DisplayClass5_0 A_1)
		{
			mpb.thickness.Add(A_1.thickness);
			mpb.thicknessSpace.Add((float)A_1.thicknessSpace);
			mpb.color.Add(A_1.color.ColorSpaceAdjusted());
			mpb.alignment.Add((float)A_1.geometry);
			mpb.scaleMode.Add((float)Draw.ScaleMode);
		}

		// Token: 0x0400007B RID: 123
		private static MpbLine mpbLine = new MpbLine();

		// Token: 0x0400007C RID: 124
		private static MpbPolyline mpbPolyline = new MpbPolyline();

		// Token: 0x0400007D RID: 125
		private static MpbPolyline mpbPolylineJoins = new MpbPolyline();

		// Token: 0x0400007E RID: 126
		private static MpbPolygon mpbPolygon = new MpbPolygon();

		// Token: 0x0400007F RID: 127
		private static readonly MpbDisc mpbDisc = new MpbDisc();

		// Token: 0x04000080 RID: 128
		private static readonly MpbRegularPolygon mpbRegularPolygon = new MpbRegularPolygon();

		// Token: 0x04000081 RID: 129
		private static readonly MpbRectangle mpbRectangle = new MpbRectangle();

		// Token: 0x04000082 RID: 130
		private static MpbTriangle mpbTriangle = new MpbTriangle();

		// Token: 0x04000083 RID: 131
		private static MpbQuad mpbQuad = new MpbQuad();

		// Token: 0x04000084 RID: 132
		private static readonly MpbSphere metaMpbSphere = new MpbSphere();

		// Token: 0x04000085 RID: 133
		private static readonly MpbCone mpbCone = new MpbCone();

		// Token: 0x04000086 RID: 134
		private static readonly MpbCuboid mpbCuboid = new MpbCuboid();

		// Token: 0x04000087 RID: 135
		private static MpbTorus mpbTorus = new MpbTorus();

		// Token: 0x04000088 RID: 136
		private static MpbText mpbText = new MpbText();

		// Token: 0x04000089 RID: 137
		private static Matrix4x4 matrix = Matrix4x4.identity;

		// Token: 0x0400008A RID: 138
		internal static DrawStyle style;
	}
}
