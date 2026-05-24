using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x0200002D RID: 45
	public class PolygonPath : PointPath<Vector2>
	{
		// Token: 0x0600099C RID: 2460 RVA: 0x00022D32 File Offset: 0x00020F32
		public void AddPoint(float x, float y)
		{
			base.AddPoint(new Vector2(x, y));
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x00022D41 File Offset: 0x00020F41
		public void BezierTo(Vector2 startTangent, Vector2 endTangent, Vector2 end, int pointCount)
		{
			if (base.CheckCanAddContinuePoint("BezierTo"))
			{
				return;
			}
			base.AddPoints(ShapesMath.CubicBezierPointsSkipFirst(base.LastPoint, startTangent, endTangent, end, pointCount));
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x00022D68 File Offset: 0x00020F68
		public void BezierTo(Vector2 startTangent, Vector2 endTangent, Vector2 end, float pointsPerTurn)
		{
			int num = ShapesConfig.Instance.polylineBezierAngularSumAccuracy * 2 + 1;
			float num2 = ShapesMath.GetApproximateCurveSum(base.LastPoint, startTangent, endTangent, end, num) / 360f;
			int num3 = Mathf.Max(2, Mathf.RoundToInt(num2 * ShapesConfig.Instance.polylineDefaultPointsPerTurn));
			this.BezierTo(startTangent, endTangent, end, num3);
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x00022DD0 File Offset: 0x00020FD0
		public void ArcTo(Vector2 corner, Vector2 next, float radius, float pointsPerTurn)
		{
			if (base.CheckCanAddContinuePoint("ArcTo"))
			{
				return;
			}
			this.AddArcPoints(corner, next, radius, true, 0, pointsPerTurn);
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x00022DED File Offset: 0x00020FED
		public void ArcTo(Vector2 corner, Vector2 next, float radius, int pointCount)
		{
			if (base.CheckCanAddContinuePoint("ArcTo"))
			{
				return;
			}
			this.AddArcPoints(corner, next, radius, false, pointCount, 0f);
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x00022E0E File Offset: 0x0002100E
		public void ArcTo(Vector2 corner, Vector2 next, float radius)
		{
			if (base.CheckCanAddContinuePoint("ArcTo"))
			{
				return;
			}
			this.AddArcPoints(corner, next, radius, true, 0, ShapesConfig.Instance.polylineDefaultPointsPerTurn);
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x00022E33 File Offset: 0x00021033
		public void ArcTo(Vector2 corner, Vector2 next, float radius, float pointsPerTurn, Color color)
		{
			if (base.CheckCanAddContinuePoint("ArcTo"))
			{
				return;
			}
			this.AddArcPoints(corner, next, radius, true, 0, pointsPerTurn);
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x00022E50 File Offset: 0x00021050
		private void AddArcPoints(Vector2 corner, Vector2 next, float radius, bool useDensity, int targetPointCount, float pointsPerTurn)
		{
			if (radius <= 0.0001f)
			{
				base.AddPoint(corner);
				return;
			}
			Vector2 normalized = (corner - base.LastPoint).normalized;
			Vector2 normalized2 = (next - corner).normalized;
			if (Vector2.Dot(normalized, normalized2) > 0.999f)
			{
				base.AddPoint(corner);
				return;
			}
			Vector2 vector = ShapesMath.Rotate90CW(normalized);
			Vector2 vector2 = ShapesMath.Rotate90CW(normalized2);
			Vector2 normalized3 = (vector + vector2).normalized;
			float num = Vector2.Dot(normalized3, vector2);
			Vector2 vector3 = corner + normalized3 * (radius / num);
			if (useDensity)
			{
				targetPointCount = Mathf.RoundToInt(Vector2.Angle(vector, vector2) / 360f * pointsPerTurn);
			}
			base.AddPoints(ShapesMath.GetArcPoints(-vector, -vector2, vector3, radius, targetPointCount));
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x00022F20 File Offset: 0x00021120
		public bool EnsureMeshIsReadyToRender(PolygonTriangulation triangulation, out Mesh outMesh)
		{
			if (!this.meshDirty && triangulation != this.lastUsedTriangulationMode)
			{
				this.meshDirty = true;
			}
			return base.EnsureMeshIsReadyToRender(out outMesh, delegate
			{
				this.TryUpdateMesh(triangulation);
			});
		}

		// Token: 0x060009A5 RID: 2469 RVA: 0x00022F71 File Offset: 0x00021171
		private void TryUpdateMesh(PolygonTriangulation triangulation)
		{
			this.lastUsedTriangulationMode = triangulation;
			ShapesMeshGen.GenPolygonMesh(this.mesh, this.path, triangulation);
		}

		// Token: 0x04000136 RID: 310
		private PolygonTriangulation lastUsedTriangulationMode = PolygonTriangulation.EarClipping;
	}
}
