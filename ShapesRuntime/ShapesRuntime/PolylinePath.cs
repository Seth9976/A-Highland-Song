using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Shapes
{
	// Token: 0x0200002E RID: 46
	public class PolylinePath : PointPath<PolylinePoint>
	{
		// Token: 0x060009A7 RID: 2471 RVA: 0x00022F9C File Offset: 0x0002119C
		public void SetPoint(int index, Vector3 point)
		{
			PolylinePoint polylinePoint = this.path[index];
			polylinePoint.point = point;
			base.SetPoint(index, polylinePoint);
		}

		// Token: 0x060009A8 RID: 2472 RVA: 0x00022FC8 File Offset: 0x000211C8
		public void SetPoint(int index, Vector2 point)
		{
			PolylinePoint polylinePoint = this.path[index];
			polylinePoint.point = point;
			base.SetPoint(index, polylinePoint);
		}

		// Token: 0x060009A9 RID: 2473 RVA: 0x00022FF8 File Offset: 0x000211F8
		public void SetColor(int index, Color color)
		{
			PolylinePoint polylinePoint = this.path[index];
			polylinePoint.color = color;
			base.SetPoint(index, polylinePoint);
		}

		// Token: 0x060009AA RID: 2474 RVA: 0x00023022 File Offset: 0x00021222
		public void AddPoint(float x, float y)
		{
			base.AddPoint(new PolylinePoint(new Vector3(x, y, 0f), Color.white));
		}

		// Token: 0x060009AB RID: 2475 RVA: 0x00023040 File Offset: 0x00021240
		public void AddPoint(float x, float y, float z)
		{
			base.AddPoint(new PolylinePoint(new Vector3(x, y, z), Color.white));
		}

		// Token: 0x060009AC RID: 2476 RVA: 0x0002305A File Offset: 0x0002125A
		public void AddPoint(float x, float y, Color color)
		{
			base.AddPoint(new PolylinePoint(new Vector3(x, y, 0f), color));
		}

		// Token: 0x060009AD RID: 2477 RVA: 0x00023074 File Offset: 0x00021274
		public void AddPoint(float x, float y, float z, Color color)
		{
			base.AddPoint(new PolylinePoint(new Vector3(x, y, z), color));
		}

		// Token: 0x060009AE RID: 2478 RVA: 0x0002308B File Offset: 0x0002128B
		public void AddPoint(Vector3 pos)
		{
			base.AddPoint(new PolylinePoint(pos, Color.white));
		}

		// Token: 0x060009AF RID: 2479 RVA: 0x0002309E File Offset: 0x0002129E
		public void AddPoint(Vector3 pos, Color color)
		{
			base.AddPoint(new PolylinePoint(pos, color));
		}

		// Token: 0x060009B0 RID: 2480 RVA: 0x000230AD File Offset: 0x000212AD
		public void AddPoint(Vector3 pos, float thickness)
		{
			base.AddPoint(new PolylinePoint(pos, Color.white, thickness));
		}

		// Token: 0x060009B1 RID: 2481 RVA: 0x000230C1 File Offset: 0x000212C1
		public void AddPoint(Vector3 pos, float thickness, Color color)
		{
			base.AddPoint(new PolylinePoint(pos, color, thickness));
		}

		// Token: 0x060009B2 RID: 2482 RVA: 0x000230D1 File Offset: 0x000212D1
		public void AddPoint(Vector2 pos)
		{
			base.AddPoint(new PolylinePoint(pos, Color.white));
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x000230E4 File Offset: 0x000212E4
		public void AddPoint(Vector2 pos, Color color)
		{
			base.AddPoint(new PolylinePoint(pos, color));
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x000230F3 File Offset: 0x000212F3
		public void AddPoint(Vector2 pos, float thickness)
		{
			base.AddPoint(new PolylinePoint(pos, Color.white, thickness));
		}

		// Token: 0x060009B5 RID: 2485 RVA: 0x00023107 File Offset: 0x00021307
		public void AddPoint(Vector2 pos, float thickness, Color color)
		{
			base.AddPoint(new PolylinePoint(pos, color, thickness));
		}

		// Token: 0x060009B6 RID: 2486 RVA: 0x00023117 File Offset: 0x00021317
		public void AddPoints(IEnumerable<Vector3> pts)
		{
			base.AddPoints(pts.Select((Vector3 point) => new PolylinePoint(point, Color.white)));
		}

		// Token: 0x060009B7 RID: 2487 RVA: 0x00023144 File Offset: 0x00021344
		public void AddPoints(params Vector3[] pts)
		{
			base.AddPoints(pts.Select((Vector3 point) => new PolylinePoint(point, Color.white)));
		}

		// Token: 0x060009B8 RID: 2488 RVA: 0x00023171 File Offset: 0x00021371
		public void AddPoints(IEnumerable<Vector2> pts)
		{
			base.AddPoints(pts.Select((Vector2 point) => new PolylinePoint(point, Color.white)));
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x0002319E File Offset: 0x0002139E
		public void AddPoints(params Vector2[] pts)
		{
			base.AddPoints(pts.Select((Vector2 point) => new PolylinePoint(point, Color.white)));
		}

		// Token: 0x060009BA RID: 2490 RVA: 0x000231CC File Offset: 0x000213CC
		public void AddPoints(IEnumerable<Vector3> pts, Color color)
		{
			base.AddPoints(pts.Select((Vector3 point) => new PolylinePoint(point, color)));
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x00023200 File Offset: 0x00021400
		public void AddPoints(IEnumerable<Vector2> pts, Color color)
		{
			base.AddPoints(pts.Select((Vector2 point) => new PolylinePoint(point, color)));
		}

		// Token: 0x060009BC RID: 2492 RVA: 0x00023232 File Offset: 0x00021432
		public void AddPoints(IEnumerable<Vector3> pts, IEnumerable<Color> colors)
		{
			base.AddPoints(pts.Zip(colors, (Vector3 p, Color c) => new PolylinePoint(p, c)));
		}

		// Token: 0x060009BD RID: 2493 RVA: 0x00023260 File Offset: 0x00021460
		public void AddPoints(IEnumerable<Vector2> pts, IEnumerable<Color> colors)
		{
			base.AddPoints(pts.Zip(colors, (Vector2 p, Color c) => new PolylinePoint(p, c)));
		}

		// Token: 0x060009BE RID: 2494 RVA: 0x0002328E File Offset: 0x0002148E
		public void AddPoints(IEnumerable<Vector3> pts, IEnumerable<float> thicknesses)
		{
			base.AddPoints(pts.Zip(thicknesses, (Vector3 p, float t) => new PolylinePoint(p, Color.white, t)));
		}

		// Token: 0x060009BF RID: 2495 RVA: 0x000232BC File Offset: 0x000214BC
		public void AddPoints(IEnumerable<Vector2> pts, IEnumerable<float> thicknesses)
		{
			base.AddPoints(pts.Zip(thicknesses, (Vector2 p, float t) => new PolylinePoint(p, Color.white, t)));
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x000232EA File Offset: 0x000214EA
		public void AddPoints(IEnumerable<Vector3> pts, IEnumerable<float> thicknesses, IEnumerable<Color> colors)
		{
			base.AddPoints(pts.Zip(colors, thicknesses, (Vector3 p, Color c, float t) => new PolylinePoint(p, c, t)));
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x00023319 File Offset: 0x00021519
		public void AddPoints(IEnumerable<Vector2> pts, IEnumerable<float> thicknesses, IEnumerable<Color> colors)
		{
			base.AddPoints(pts.Zip(colors, thicknesses, (Vector2 p, Color c, float t) => new PolylinePoint(p, c, t)));
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x00023348 File Offset: 0x00021548
		public void BezierTo(Vector3 startTangent, Vector3 endTangent, Vector3 end, int pointCount)
		{
			this.BezierTo(startTangent, endTangent, end, pointCount, Color.white);
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x0002335A File Offset: 0x0002155A
		public void BezierTo(Vector3 startTangent, Vector3 endTangent, Vector3 end, int pointCount, Color color)
		{
			if (base.CheckCanAddContinuePoint("BezierTo"))
			{
				return;
			}
			this.AddPoints(ShapesMath.CubicBezierPointsSkipFirst(base.LastPoint.point, startTangent, endTangent, end, pointCount), color);
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x00023387 File Offset: 0x00021587
		public void BezierTo(Vector3 startTangent, Vector3 endTangent, Vector3 end)
		{
			this.BezierTo(startTangent, endTangent, end, ShapesConfig.Instance.polylineDefaultPointsPerTurn, Color.white);
		}

		// Token: 0x060009C5 RID: 2501 RVA: 0x000233A1 File Offset: 0x000215A1
		public void BezierTo(Vector3 startTangent, Vector3 endTangent, Vector3 end, Color color)
		{
			this.BezierTo(startTangent, endTangent, end, ShapesConfig.Instance.polylineDefaultPointsPerTurn, color);
		}

		// Token: 0x060009C6 RID: 2502 RVA: 0x000233B8 File Offset: 0x000215B8
		public void BezierTo(Vector3 startTangent, Vector3 endTangent, Vector3 end, float pointsPerTurn)
		{
			this.BezierTo(startTangent, endTangent, end, pointsPerTurn, Color.white);
		}

		// Token: 0x060009C7 RID: 2503 RVA: 0x000233CC File Offset: 0x000215CC
		public void BezierTo(Vector3 startTangent, Vector3 endTangent, Vector3 end, float pointsPerTurn, Color color)
		{
			int num = ShapesConfig.Instance.polylineBezierAngularSumAccuracy * 2 + 1;
			float num2 = ShapesMath.GetApproximateCurveSum(base.LastPoint.point, startTangent, endTangent, end, num) / 360f;
			int num3 = Mathf.Max(2, Mathf.RoundToInt(num2 * pointsPerTurn));
			this.BezierTo(startTangent, endTangent, end, num3, color);
		}

		// Token: 0x060009C8 RID: 2504 RVA: 0x0002341F File Offset: 0x0002161F
		public void ArcTo(Vector3 corner, Vector3 next, float radius, int pointCount)
		{
			if (base.CheckCanAddContinuePoint("ArcTo"))
			{
				return;
			}
			this.AddArcPoints(corner, next, radius, false, pointCount, 0f, Color.white);
		}

		// Token: 0x060009C9 RID: 2505 RVA: 0x00023445 File Offset: 0x00021645
		public void ArcTo(Vector3 corner, Vector3 next, float radius)
		{
			if (base.CheckCanAddContinuePoint("ArcTo"))
			{
				return;
			}
			this.AddArcPoints(corner, next, radius, true, 0, ShapesConfig.Instance.polylineDefaultPointsPerTurn, Color.white);
		}

		// Token: 0x060009CA RID: 2506 RVA: 0x0002346F File Offset: 0x0002166F
		public void ArcTo(Vector3 corner, Vector3 next, float radius, float pointsPerTurn)
		{
			if (base.CheckCanAddContinuePoint("ArcTo"))
			{
				return;
			}
			this.AddArcPoints(corner, next, radius, true, 0, pointsPerTurn, Color.white);
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x00023491 File Offset: 0x00021691
		public void ArcTo(Vector3 corner, Vector3 next, float radius, int pointCount, Color color)
		{
			if (base.CheckCanAddContinuePoint("ArcTo"))
			{
				return;
			}
			this.AddArcPoints(corner, next, radius, false, pointCount, 0f, color);
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x000234B4 File Offset: 0x000216B4
		public void ArcTo(Vector3 corner, Vector3 next, float radius, Color color)
		{
			if (base.CheckCanAddContinuePoint("ArcTo"))
			{
				return;
			}
			this.AddArcPoints(corner, next, radius, true, 0, ShapesConfig.Instance.polylineDefaultPointsPerTurn, color);
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x000234DB File Offset: 0x000216DB
		public void ArcTo(Vector3 corner, Vector3 next, float radius, float pointsPerTurn, Color color)
		{
			if (base.CheckCanAddContinuePoint("ArcTo"))
			{
				return;
			}
			this.AddArcPoints(corner, next, radius, true, 0, pointsPerTurn, color);
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x000234FC File Offset: 0x000216FC
		private void AddArcPoints(Vector3 corner, Vector3 next, float radius, bool useDensity, int targetPointCount, float pointsPerTurn, Color color)
		{
			if (radius <= 0.0001f)
			{
				this.AddPoint(corner, color);
				return;
			}
			Vector3 normalized = (corner - base.LastPoint.point).normalized;
			Vector3 normalized2 = (next - corner).normalized;
			Vector3 vector = Vector3.Cross(normalized, normalized2);
			if (vector.TaxicabMagnitude() <= 0.001f)
			{
				this.AddPoint(corner, color);
				return;
			}
			Vector3 normalized3 = vector.normalized;
			Vector3 vector2 = Vector3.Cross(normalized3, normalized);
			Vector3 vector3 = Vector3.Cross(normalized3, normalized2);
			Vector3 normalized4 = (vector2 + vector3).normalized;
			float num = Vector3.Dot(normalized4, vector3);
			Vector3 vector4 = corner + normalized4 * (radius / num);
			if (useDensity)
			{
				targetPointCount = Mathf.RoundToInt(Vector3.Angle(vector2, vector3) / 360f * pointsPerTurn);
			}
			this.AddPoints(ShapesMath.GetArcPoints(-vector2, -vector3, vector4, radius, targetPointCount), color);
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x000235EC File Offset: 0x000217EC
		public bool EnsureMeshIsReadyToRender(bool closed, PolylineJoins renderJoins, out Mesh outMesh)
		{
			if (!this.meshDirty && (renderJoins != this.lastUsedJoins || closed != this.lastUsedClosed))
			{
				this.meshDirty = true;
			}
			return base.EnsureMeshIsReadyToRender(out outMesh, delegate
			{
				this.TryUpdateMesh(closed, renderJoins);
			});
		}

		// Token: 0x060009D0 RID: 2512 RVA: 0x00023652 File Offset: 0x00021852
		private void TryUpdateMesh(bool closed, PolylineJoins joins)
		{
			this.lastUsedClosed = closed;
			this.lastUsedJoins = joins;
			ShapesMeshGen.GenPolylineMesh(this.mesh, this.path, closed, joins, false, true);
		}

		// Token: 0x04000137 RID: 311
		private bool lastUsedClosed;

		// Token: 0x04000138 RID: 312
		private PolylineJoins lastUsedJoins = PolylineJoins.Miter;
	}
}
