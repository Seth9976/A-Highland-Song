using System;
using UnityEngine;

// Token: 0x020000BE RID: 190
public static class PolySimulate
{
	// Token: 0x060005FE RID: 1534 RVA: 0x0002F0A4 File Offset: 0x0002D2A4
	public static PolySimulate.PointResult FindNearestPointOnPoly(Poly poly, Vector2 nearPoint, Range climbAngleRange = default(Range))
	{
		PolySimulate.PointResult pointResult = new PolySimulate.PointResult
		{
			poly = poly
		};
		Vector3 vector = poly.transform.InverseTransformPoint(nearPoint);
		Vector2 vector2 = default(Vector2);
		Vector2 vector3 = default(Vector2);
		float num = float.MaxValue;
		int num2 = -1;
		int vertCount = poly.polygon.VertCount;
		int i = 0;
		while (i < vertCount)
		{
			int num3 = (i + 1) % vertCount;
			Vector2 vector4 = poly.polygon.vertices[i];
			Vector2 vector5 = poly.polygon.vertices[num3];
			if (!(climbAngleRange != Range.zero))
			{
				goto IL_00C2;
			}
			float num4 = Util.ClimbAngleFromSurfacePolyEdge(poly.transform.TransformVector(vector5 - vector4));
			if (climbAngleRange.Contains(num4))
			{
				goto IL_00C2;
			}
			IL_00FE:
			i++;
			continue;
			IL_00C2:
			Vector2 closestPointOnLine = Line.GetClosestPointOnLine(vector4, vector5, vector, true);
			float num5 = Vector2.Distance(closestPointOnLine, vector);
			if (num5 < num)
			{
				num = num5;
				num2 = i;
				vector3 = vector5 - vector4;
				vector2 = closestPointOnLine;
				goto IL_00FE;
			}
			goto IL_00FE;
		}
		pointResult.point = poly.transform.TransformPoint(vector2);
		pointResult.edge = poly.transform.TransformVector(vector3);
		pointResult.normal = new Vector2(-pointResult.edge.y, pointResult.edge.x).normalized;
		pointResult.i0 = num2;
		pointResult.found = num2 >= 0;
		pointResult.climbable = num2 >= 0 && poly.EdgeIsClimbable(num2);
		return pointResult;
	}

	// Token: 0x060005FF RID: 1535 RVA: 0x0002F254 File Offset: 0x0002D454
	private static Vector2 NormalFromTangent(Vector2 tangent, int windingDir)
	{
		Vector2 vector = (float)windingDir * tangent;
		return new Vector2(-vector.y, vector.x).normalized;
	}

	// Token: 0x06000600 RID: 1536 RVA: 0x0002F284 File Offset: 0x0002D484
	public static PolySimulate.Result AroundEdge(Poly poly, Vector2 start, Vector2 direction, float maxDistance, PolySimulate.Options opts, PolySimulate.IterateFunc iterateFunc = null)
	{
		PolySimulate.Result result = new PolySimulate.Result
		{
			poly = poly
		};
		PolySimulate.PointResult pointResult = PolySimulate.FindNearestPointOnPoly(poly, start, default(Range));
		if (!pointResult.found)
		{
			return result;
		}
		int num = pointResult.i0;
		int vertexCount = poly.vertexCount;
		int num2 = ((Vector2.Dot(pointResult.edge, direction) < 0f) ? (-1) : 1);
		if (num2 == -1)
		{
			num = (num + 1) % vertexCount;
		}
		Vector2 vector = pointResult.point;
		float num3 = poly.transform.position.z;
		Vector2 vector2 = default(Vector2);
		int num4 = num;
		float num5 = maxDistance;
		bool flag = false;
		bool flag2 = false;
		bool flag3 = true;
		Vector2 vector3;
		float num7;
		float num9;
		SlopeSample slopeSample;
		float num10;
		Raycast.Collision collision;
		float num11;
		for (;;)
		{
			result.position = vector;
			int num6 = (num4 + num2 + vertexCount) % vertexCount;
			vector3 = poly.PointIdx(num6);
			Vector2 vector4 = vector3 - vector;
			if (flag3 && vector4 == Vector2.zero)
			{
				num4 = num6;
			}
			else
			{
				num7 = Vector2.Distance(vector, vector3);
				if (flag3)
				{
					vector2 = vector4;
				}
				if (iterateFunc != null)
				{
					iterateFunc(vector, flag3, false);
				}
				if (opts.preventDirectionChange && Vector2.Dot(vector2, vector4) < 0f)
				{
					break;
				}
				int num8 = num4;
				if (num2 == -1)
				{
					num8 = num6;
				}
				flag2 = poly.EdgeIsUnreachable(num8);
				flag = poly.EdgeIsClimbable(num8);
				if (opts.preventUnreachable && flag2)
				{
					goto Block_11;
				}
				if ((opts.preventClimbable || opts.preventNonClimbable) && ((opts.preventClimbable && flag) || (opts.preventNonClimbable && !flag)))
				{
					goto IL_0186;
				}
				num9 = Util.ClimbAngleFromSurfacePolyEdge((num2 > 0) ? vector4 : (-vector4));
				if (!opts.validAngleRange.Contains(num9))
				{
					goto Block_17;
				}
				if (Application.isPlaying)
				{
					if (Raycast.SampleWithDepthRange(vector, vector3, num3, opts.slopePolyCheckDepthRange, out slopeSample, default(Color)).didHit)
					{
						num10 = Vector2.Distance(vector, slopeSample.point);
						if (num5 > num10)
						{
							goto Block_21;
						}
					}
					collision = Raycast.CollideWallPolygonsVec3(new Vector3(vector.x, vector.y, num3), vector4, opts.slopePolyCheckDepthRange, opts.runnerSettings, false, poly, true, default(Color));
					if (collision.didHit)
					{
						num11 = Vector2.Distance(vector, collision.pos);
						if (num5 > num11)
						{
							num5 -= num11;
							if (opts.allowPolyChange && (collision.climbable || !opts.preventNonClimbable) && collision.normal.x * PolySimulate.NormalFromTangent(vector2, num2).x > 0f)
							{
								result.distanceTravelled += num11;
								vector = collision.pos;
								poly = collision.poly;
								num3 = poly.transform.position.z;
								num4 = collision.i0;
								flag3 = true;
								continue;
							}
							goto IL_035F;
						}
					}
				}
				if (num5 < num7)
				{
					goto Block_27;
				}
				num5 -= num7;
				result.distanceTravelled += num7;
				vector = vector3;
				num4 = num6;
				vector2 = vector4;
				flag3 = false;
			}
		}
		result.endReason = PolySimulate.EndReason.PreventDirectionChange;
		goto IL_040E;
		Block_11:
		result.endReason = PolySimulate.EndReason.PreventUnreachable;
		goto IL_040E;
		IL_0186:
		result.endReason = (flag ? PolySimulate.EndReason.PreventClimbable : PolySimulate.EndReason.PreventNonClimbable);
		goto IL_040E;
		Block_17:
		result.invalidAngle = num9;
		result.endReason = ((num9 < opts.validAngleRange.min) ? PolySimulate.EndReason.AngleTooLow : PolySimulate.EndReason.AngleTooHigh);
		goto IL_040E;
		Block_21:
		num5 -= num10;
		result.distanceTravelled += num10;
		result.position = slopeSample.point;
		result.normal = slopeSample.normal;
		result.endReason = PolySimulate.EndReason.SlopeIntersection;
		result.slopeHit = slopeSample.slope;
		goto IL_040E;
		IL_035F:
		result.distanceTravelled += num11;
		result.position = collision.pos;
		result.endReason = PolySimulate.EndReason.PolyIntersection;
		result.polyHit = collision.poly;
		result.polyHitClimbable = collision.climbable;
		result.normal = collision.normal;
		goto IL_040E;
		Block_27:
		float num12 = num5 / num7;
		result.distanceTravelled = maxDistance;
		result.position = Vector2.Lerp(vector, vector3, num12);
		result.endReason = PolySimulate.EndReason.MaxDistance;
		IL_040E:
		if (iterateFunc != null)
		{
			iterateFunc(result.position, false, true);
		}
		result.normal = PolySimulate.NormalFromTangent(vector2, num2);
		result.climbable = flag;
		result.unreachable = flag2;
		result.poly = poly;
		return result;
	}

	// Token: 0x020002E6 RID: 742
	public struct Result
	{
		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x0600165C RID: 5724 RVA: 0x000985FE File Offset: 0x000967FE
		public Vector3 position3d
		{
			get
			{
				return new Vector3(this.position.x, this.position.y, this.poly.transform.position.z);
			}
		}

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x0600165D RID: 5725 RVA: 0x00098630 File Offset: 0x00096830
		public bool success
		{
			get
			{
				return this.endReason == PolySimulate.EndReason.MaxDistance;
			}
		}

		// Token: 0x040016A8 RID: 5800
		public Poly poly;

		// Token: 0x040016A9 RID: 5801
		public Vector2 position;

		// Token: 0x040016AA RID: 5802
		public Vector2 normal;

		// Token: 0x040016AB RID: 5803
		public bool climbable;

		// Token: 0x040016AC RID: 5804
		public bool unreachable;

		// Token: 0x040016AD RID: 5805
		public float distanceTravelled;

		// Token: 0x040016AE RID: 5806
		public PolySimulate.EndReason endReason;

		// Token: 0x040016AF RID: 5807
		public float invalidAngle;

		// Token: 0x040016B0 RID: 5808
		public Slope slopeHit;

		// Token: 0x040016B1 RID: 5809
		public Poly polyHit;

		// Token: 0x040016B2 RID: 5810
		public bool polyHitClimbable;
	}

	// Token: 0x020002E7 RID: 743
	public enum EndReason
	{
		// Token: 0x040016B4 RID: 5812
		Unknown,
		// Token: 0x040016B5 RID: 5813
		MaxDistance,
		// Token: 0x040016B6 RID: 5814
		PreventClimbable,
		// Token: 0x040016B7 RID: 5815
		PreventNonClimbable,
		// Token: 0x040016B8 RID: 5816
		PreventUnreachable,
		// Token: 0x040016B9 RID: 5817
		PreventDirectionChange,
		// Token: 0x040016BA RID: 5818
		SlopeIntersection,
		// Token: 0x040016BB RID: 5819
		PolyIntersection,
		// Token: 0x040016BC RID: 5820
		AngleTooLow,
		// Token: 0x040016BD RID: 5821
		AngleTooHigh
	}

	// Token: 0x020002E8 RID: 744
	[Serializable]
	public struct Options
	{
		// Token: 0x040016BE RID: 5822
		public bool preventClimbable;

		// Token: 0x040016BF RID: 5823
		public bool preventNonClimbable;

		// Token: 0x040016C0 RID: 5824
		public bool preventDirectionChange;

		// Token: 0x040016C1 RID: 5825
		public bool preventUnreachable;

		// Token: 0x040016C2 RID: 5826
		public bool allowPolyChange;

		// Token: 0x040016C3 RID: 5827
		public Range validAngleRange;

		// Token: 0x040016C4 RID: 5828
		public Range slopePolyCheckDepthRange;

		// Token: 0x040016C5 RID: 5829
		public RunnerSettings runnerSettings;
	}

	// Token: 0x020002E9 RID: 745
	// (Invoke) Token: 0x0600165F RID: 5727
	public delegate void IterateFunc(Vector2 pos, bool isFirst, bool isLast);

	// Token: 0x020002EA RID: 746
	public struct PointResult
	{
		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x06001662 RID: 5730 RVA: 0x0009863B File Offset: 0x0009683B
		public Vector3 point3d
		{
			get
			{
				return new Vector3(this.point.x, this.point.y, this.poly.transform.position.z);
			}
		}

		// Token: 0x040016C6 RID: 5830
		public Poly poly;

		// Token: 0x040016C7 RID: 5831
		public Vector2 point;

		// Token: 0x040016C8 RID: 5832
		public Vector2 normal;

		// Token: 0x040016C9 RID: 5833
		public int i0;

		// Token: 0x040016CA RID: 5834
		public Vector2 edge;

		// Token: 0x040016CB RID: 5835
		public bool found;

		// Token: 0x040016CC RID: 5836
		public bool climbable;
	}
}
