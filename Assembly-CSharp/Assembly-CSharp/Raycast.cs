using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020000BF RID: 191
public static class Raycast
{
	// Token: 0x06000601 RID: 1537 RVA: 0x0002F6E0 File Offset: 0x0002D8E0
	public static Raycast.RaycastResult SampleAll(Vector2 start, Vector2 end, bool includeMusicRuns, out SlopeSample sample, Color historyDebugColor = default(Color))
	{
		float maxValue = float.MaxValue;
		Range range = Range.Centered(0f, 10000000f);
		IEnumerable<Slope> enumerable;
		if (!includeMusicRuns)
		{
			IEnumerable<Slope> all = Level.current.slopes.all;
			enumerable = all;
		}
		else
		{
			enumerable = Level.current.slopesAndMusicRunSlopes;
		}
		return Raycast.SampleWithDepthRange(start, end, maxValue, range, enumerable, Level.current.invisibleCollision.all, out sample, historyDebugColor);
	}

	// Token: 0x06000602 RID: 1538 RVA: 0x0002F73C File Offset: 0x0002D93C
	public static Raycast.RaycastResult SampleWithDepthRange(Vector2 start, Vector2 end, float sourceDepth, Range depthRange, out SlopeSample sample, Color historyDebugColor = default(Color))
	{
		return Raycast.SampleWithDepthRange(start, end, sourceDepth, depthRange, Level.current.slopes.Nearby(start, depthRange, 0f, null), Level.current.invisibleCollision.Nearby(start, depthRange, 0f, null), out sample, historyDebugColor);
	}

	// Token: 0x06000603 RID: 1539 RVA: 0x0002F784 File Offset: 0x0002D984
	public static Raycast.RaycastResult SampleWithDepthRange(Vector2 start, Vector2 end, float sourceDepth, Range depthRange, IEnumerable<Slope> slopes, IEnumerable<InvisibleCollision> invisibleCollisions, out SlopeSample sample, Color historyDebugColor = default(Color))
	{
		Raycast.RaycastResult raycastResult = new Raycast.RaycastResult
		{
			distNorm = float.MaxValue,
			didHit = false
		};
		sample = default(SlopeSample);
		foreach (Slope slope in slopes)
		{
			if (slope == null)
			{
				Debug.LogWarning("There was a null slope in the list when raycasting");
			}
			else
			{
				SlopeSample slopeSample;
				Raycast.RaycastResult raycastResult2 = Raycast.Sample(slope, start, end, out slopeSample);
				if (raycastResult2.didHit && raycastResult2.distNorm < raycastResult.distNorm)
				{
					if (sourceDepth != 3.4028235E+38f)
					{
						Range range = Range.Auto(sourceDepth, slopeSample.depth);
						range.max -= 0.01f;
						range.min += 0.01f;
						Poly poly = Raycast.AnyPolyOccludes(slopeSample.point, range, null, null);
						if (poly)
						{
							Debug.Log(string.Format("Ignoring hit at {0}, sourceDepth={1}, since it's occluded by {2} at Z={3}, which is within range {4}", new object[]
							{
								slopeSample.point,
								sourceDepth,
								poly.name,
								poly.transform.position.z,
								range
							}), poly);
							continue;
						}
					}
					bool flag = false;
					using (IEnumerator<InvisibleCollision> enumerator2 = invisibleCollisions.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (enumerator2.Current.Contains(slopeSample.point3d, -0.2f))
							{
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						raycastResult.distNorm = raycastResult2.distNorm;
						raycastResult.didHit = true;
						sample = slopeSample;
					}
				}
			}
		}
		return raycastResult;
	}

	// Token: 0x06000604 RID: 1540 RVA: 0x0002F974 File Offset: 0x0002DB74
	public static Raycast.RaycastResult Sample(Slope slope, Vector2 start, Vector2 end, out SlopeSample sample)
	{
		sample = default(SlopeSample);
		Vector2 vector = slope.transform.InverseTransformPoint(start);
		Vector2 vector2 = slope.transform.InverseTransformPoint(end);
		for (int i = 0; i < slope.localPoints.Count - 1; i++)
		{
			Vector2 vector3 = slope.localPoints[i];
			Vector2 vector4 = slope.localPoints[i + 1];
			if (Vector3.Cross(vector4 - vector3, vector2 - vector).z <= 0f)
			{
				Raycast.RaycastResult raycastResult = Raycast.IntersectionCheck(vector3, vector4, vector, vector2);
				if (raycastResult.didHit)
				{
					sample = slope.SampleBetweenPointsNorm(i, raycastResult.distNorm);
					return new Raycast.RaycastResult
					{
						didHit = true,
						distNorm = Vector2.Distance(start, sample.point) / Vector2.Distance(start, end)
					};
				}
			}
		}
		return new Raycast.RaycastResult
		{
			didHit = false
		};
	}

	// Token: 0x06000605 RID: 1541 RVA: 0x0002FA84 File Offset: 0x0002DC84
	public static Raycast.RaycastResult IntersectionCheck(Vector2 line1Start, Vector2 line1End, Vector2 line2Start, Vector2 line2End)
	{
		float num = line1End.x - line1Start.x;
		float num2 = line1End.y - line1Start.y;
		float num3 = line2End.x - line2Start.x;
		float num4 = line2End.y - line2Start.y;
		float num5 = line1Start.x - line2Start.x;
		float num6 = line1Start.y - line2Start.y;
		float num7 = (-num2 * num5 + num * num6) / (-num3 * num2 + num * num4);
		if (num7 >= 0f && num7 <= 1f)
		{
			float num8 = (num3 * num6 - num4 * num5) / (-num3 * num2 + num * num4);
			if (num8 >= 0f && num8 <= 1f)
			{
				return new Raycast.RaycastResult
				{
					didHit = true,
					distNorm = num8
				};
			}
		}
		return new Raycast.RaycastResult
		{
			didHit = false
		};
	}

	// Token: 0x06000606 RID: 1542 RVA: 0x0002FB64 File Offset: 0x0002DD64
	public static Raycast.Collision CollideWallPolygonsVec3(Vector3 pos, Vector2 delta, Range depthRange, RunnerSettings settings, bool climbableOnly = false, Poly ignorePoly = null, bool hitCeilings = false, Color historyRayColor = default(Color))
	{
		float num = delta.magnitude;
		Vector3 vector = Vector3.zero;
		Vector2 vector2 = Vector2.up;
		Vector2 normalized = delta.normalized;
		float magnitude = delta.magnitude;
		Raycast.Collision collision = new Raycast.Collision
		{
			pos = pos,
			normal = Vector2.up,
			poly = null
		};
		foreach (Poly poly in Level.current.polys.Nearby(pos, depthRange, magnitude, null))
		{
			if (!poly.passThroughWalls && !poly.sceneryOnly && (!(ignorePoly != null) || !(poly == ignorePoly)))
			{
				Polygon polygon = poly.polygon;
				bool isClockwise = polygon.isClockwise;
				int vertCount = polygon.VertCount;
				for (int i = 0; i < vertCount; i++)
				{
					int num2 = (i + 1) % vertCount;
					if (poly.EdgeIsCollidable(i))
					{
						bool flag = poly.EdgeIsClimbable(i);
						if (!climbableOnly || flag)
						{
							Vector3 vector3 = poly.transform.TransformPoint(polygon.vertices[i]);
							Vector3 vector4 = poly.transform.TransformPoint(polygon.vertices[num2]);
							Line line = new Line(vector3, vector4);
							float num3;
							if (line.RayLineIntersect(pos, normalized, out num3) && num3 < num)
							{
								vector = pos + normalized * num3;
								Vector3 vector5 = (isClockwise ? (vector4 - vector3) : (vector3 - vector4));
								vector2 = new Vector2(-vector5.y, vector5.x).normalized;
								if ((climbableOnly || hitCeilings || vector2.y >= -0.5f) && Vector2.Dot(vector2, normalized) <= 0f)
								{
									Range.Auto(pos.z, poly.transform.position.z);
									if (!Raycast.AnyPolyOccludes(vector, Range.Auto(pos.z, poly.transform.position.z), poly, ignorePoly))
									{
										num = num3;
										collision.normal = vector2;
										collision.pos = vector;
										collision.poly = poly;
										collision.climbable = flag;
										collision.unreachable = poly.EdgeIsUnreachable(i);
										collision.i0 = i;
									}
								}
							}
						}
					}
				}
			}
		}
		return collision;
	}

	// Token: 0x06000607 RID: 1543 RVA: 0x0002FE38 File Offset: 0x0002E038
	public static Raycast.Collision OvalSwept<TPolyOrSlope>(Vector3 circlePos, float rotation, Vector2 size, Vector2 delta, Range depthRange, Component ignorePolyOrSlope = null, bool includeInHistory = false)
	{
		Raycast.Collision collision = default(Raycast.Collision);
		float num = 1f;
		float num2 = 0f;
		int num3 = 0;
		float num4 = 1f;
		float num5 = 0.5f * Mathf.Max(size.x, size.y) + delta.magnitude;
		List<TPolyOrSlope> list2;
		if (typeof(TPolyOrSlope) == typeof(Poly))
		{
			List<Poly> list = Level.current.polys.Nearby(circlePos, depthRange, num5, null);
			list.UpdateAndRemoveIf((Poly poly) => !poly.isActiveAndEnabled || poly == ignorePolyOrSlope || poly.sceneryOnly || poly.passThroughWalls);
			list2 = list as List<TPolyOrSlope>;
		}
		else
		{
			List<Slope> list3 = Level.current.slopes.Nearby(circlePos, depthRange, num5, null);
			list3.UpdateAndRemoveIf((Slope slope) => !slope.isActiveAndEnabled || slope == ignorePolyOrSlope);
			list2 = list3 as List<TPolyOrSlope>;
		}
		Raycast.Collision collision2;
		for (;;)
		{
			collision2 = Raycast.Oval<TPolyOrSlope>(Vector3.Lerp(circlePos, circlePos + delta, num4), rotation, size, list2);
			if (collision2.didHit)
			{
				num = num4;
				collision = collision2;
				if (num4 == 0f)
				{
					break;
				}
				if (num4 == 1f)
				{
					num4 = 0f;
				}
				else
				{
					num4 = 0.5f * (num2 + num);
				}
			}
			else
			{
				num2 = num4;
				if (num4 == 1f)
				{
					return collision2;
				}
				num4 = 0.5f * (num2 + num);
			}
			num3++;
			if (num3 >= 8)
			{
				goto Block_6;
			}
		}
		return collision2;
		Block_6:
		collision.deltaNorm = num2;
		if (includeInHistory)
		{
			circlePos + num2 * delta;
		}
		return collision;
	}

	// Token: 0x06000608 RID: 1544 RVA: 0x0002FFC0 File Offset: 0x0002E1C0
	private static Raycast.Collision Oval<TPolyOrSlope>(Vector3 circlePos, float rotation, Vector2 size, List<TPolyOrSlope> nearbyPolysOrSlopes)
	{
		Raycast.Collision collision = new Raycast.Collision
		{
			pos = circlePos,
			normal = Vector2.up,
			poly = null,
			slope = null,
			i0 = -1
		};
		Matrix4x4 matrix4x = Matrix4x4.TRS(circlePos, Quaternion.AngleAxis(rotation, Vector3.forward), new Vector3(0.5f * size.x, 0.5f * size.y, 1f));
		Matrix4x4 inverse = matrix4x.inverse;
		float maxValue = float.MaxValue;
		bool flag = true;
		List<Poly> list = nearbyPolysOrSlopes as List<Poly>;
		if (typeof(TPolyOrSlope) == typeof(Poly))
		{
			using (List<Poly>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Poly poly = enumerator.Current;
					if (Raycast.CollideVertsPoly(circlePos, poly, matrix4x, inverse, ref maxValue, ref flag, ref collision))
					{
						collision.poly = poly;
						collision.climbable = poly.EdgeIsClimbable(collision.i0);
						collision.unreachable = poly.EdgeIsUnreachable(collision.i0);
						if (!flag)
						{
							break;
						}
					}
				}
				return collision;
			}
		}
		foreach (Slope slope in (nearbyPolysOrSlopes as List<Slope>))
		{
			if (Raycast.CollideVertsSlope(circlePos, slope, matrix4x, inverse, ref maxValue, ref collision))
			{
				collision.slope = slope;
				break;
			}
		}
		return collision;
	}

	// Token: 0x06000609 RID: 1545 RVA: 0x00030150 File Offset: 0x0002E350
	private static bool CollideVertsPoly(Vector3 circlePos, Poly poly, Matrix4x4 circleLocalToWorldMtx, Matrix4x4 circleWorldToLocalMtx, ref float nearestPolyDist, ref bool nearestIsBehind, ref Raycast.Collision collision)
	{
		Transform transform = poly.transform;
		Matrix4x4 matrix4x = circleWorldToLocalMtx * transform.localToWorldMatrix;
		bool flag = false;
		Vector2[] vertices = poly.polygon.vertices;
		int num = vertices.Length;
		for (int i = 0; i < num; i++)
		{
			int num2 = (i + 1) % num;
			if (poly.EdgeIsCollidable(i))
			{
				Vector3 vector = matrix4x.MultiplyPoint3x4(vertices[i]);
				Vector3 vector2 = matrix4x.MultiplyPoint3x4(vertices[num2]);
				Line line = new Line(vector, vector2);
				Vector2 closestPointOnLine = line.GetClosestPointOnLine(Vector2.zero, true);
				if (closestPointOnLine.sqrMagnitude <= 1f)
				{
					Vector3 vector3 = circleLocalToWorldMtx.MultiplyPoint3x4(closestPointOnLine);
					float num3 = Vector2.Distance(vector3, circlePos);
					if (num3 <= nearestPolyDist || nearestIsBehind)
					{
						Vector3 vector4 = poly.PointIdx(i);
						Vector3 vector5 = poly.PointIdx(num2) - vector4;
						Vector2 normalized = new Vector2(-vector5.y, vector5.x).normalized;
						bool flag2 = Vector2.Dot(vector3 - circlePos, normalized) > 0f;
						if ((!flag2 || nearestIsBehind) && (flag2 != nearestIsBehind || num3 <= nearestPolyDist))
						{
							nearestIsBehind = flag2;
							nearestPolyDist = num3;
							collision.normal = normalized;
							collision.pos = vector3;
							collision.i0 = i;
							collision.isBehind = flag2;
							if (!flag2)
							{
								return true;
							}
							flag = true;
						}
					}
				}
			}
		}
		return flag;
	}

	// Token: 0x0600060A RID: 1546 RVA: 0x000302F0 File Offset: 0x0002E4F0
	private static bool CollideVertsSlope(Vector3 circlePos, Slope slope, Matrix4x4 circleLocalToWorldMtx, Matrix4x4 circleWorldToLocalMtx, ref float nearestPolyDist, ref Raycast.Collision collision)
	{
		Transform transform = slope.transform;
		Matrix4x4 matrix4x = circleWorldToLocalMtx * transform.localToWorldMatrix;
		List<Vector2> localPoints = slope.localPoints;
		int count = localPoints.Count;
		for (int i = 0; i < count; i++)
		{
			int num = (i + 1) % count;
			Vector3 vector = matrix4x.MultiplyPoint3x4(localPoints[i]);
			Vector3 vector2 = matrix4x.MultiplyPoint3x4(localPoints[num]);
			Line line = new Line(vector, vector2);
			Vector2 closestPointOnLine = line.GetClosestPointOnLine(Vector2.zero, true);
			if (closestPointOnLine.sqrMagnitude <= 1f)
			{
				Vector3 vector3 = circleLocalToWorldMtx.MultiplyPoint3x4(closestPointOnLine);
				float num2 = Vector2.Distance(vector3, circlePos);
				if (num2 <= nearestPolyDist)
				{
					nearestPolyDist = num2;
					Vector3 vector4 = slope.PointIdx(i);
					Vector3 vector5 = slope.PointIdx(num) - vector4;
					Vector2 normalized = new Vector2(-vector5.y, vector5.x).normalized;
					collision.normal = normalized;
					collision.pos = vector3;
					collision.i0 = i;
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600060B RID: 1547 RVA: 0x0003042C File Offset: 0x0002E62C
	public static Poly AnyPolyOccludes(Vector2 point, Range betweenRange, Poly ignoring = null, Poly ignoring2 = null)
	{
		Level.current.polys.Nearby(point, betweenRange, 0f, Raycast.occlusionPolysScratchList);
		foreach (Poly poly in Raycast.occlusionPolysScratchList)
		{
			if (!(poly == ignoring) && !(poly == ignoring2) && !poly.isCutawayWall && !poly.sceneryOnly)
			{
				Vector2 vector = poly.transform.InverseTransformPoint(point);
				if (poly.polygon.ContainsPoint(vector))
				{
					return poly;
				}
			}
		}
		return null;
	}

	// Token: 0x0600060C RID: 1548 RVA: 0x000304E4 File Offset: 0x0002E6E4
	public static Vector3 RayIntersectionWithZPlane(Ray ray, float z)
	{
		float num = (z - ray.origin.z) / ray.direction.z;
		return ray.origin + num * ray.direction;
	}

	// Token: 0x0600060D RID: 1549 RVA: 0x00030528 File Offset: 0x0002E728
	[return: TupleElementNames(new string[] { "pos", "radius" })]
	public static ValueTuple<Vector2, float> GetNearbyParamsForRay(Level level, Ray ray, Range bucketZRange)
	{
		Vector3 vector = Raycast.RayIntersectionWithZPlane(ray, bucketZRange.min);
		Vector3 vector2 = Raycast.RayIntersectionWithZPlane(ray, bucketZRange.max);
		Vector3 vector3 = 0.5f * (vector + vector2);
		float num = 0.5f * Mathf.Abs(vector2.x - vector.x);
		return new ValueTuple<Vector2, float>(vector3, num);
	}

	// Token: 0x0600060E RID: 1550 RVA: 0x00030584 File Offset: 0x0002E784
	public static Poly RayHitAnyPolyAnyLevel(Ray ray, float maxZ, int minLevel, Poly ignore = null)
	{
		Range range = new Range(ray.origin.z, maxZ);
		foreach (Level level in Level.activeLevels)
		{
			if (level.levelIdx >= minLevel && level.polys.zRange.min <= maxZ)
			{
				ValueTuple<Vector2, float> nearbyParamsForRay = Raycast.GetNearbyParamsForRay(level, ray, level.polys.zRange);
				foreach (Poly poly in level.polys.Nearby(nearbyParamsForRay.Item1, range, nearbyParamsForRay.Item2, null))
				{
					if (!(poly == ignore))
					{
						Vector3 vector = Raycast.RayIntersectionWithZPlane(ray, poly.transform.position.z);
						Vector2 vector2 = poly.transform.InverseTransformPoint(vector);
						if (poly.polygon.ContainsPoint(vector2))
						{
							return poly;
						}
					}
				}
			}
		}
		return null;
	}

	// Token: 0x0600060F RID: 1551 RVA: 0x000306C0 File Offset: 0x0002E8C0
	public static bool Poly(Poly poly, Vector2 start, Vector2 end, out Vector2 hitPos)
	{
		if (poly.passThroughWalls)
		{
			Debug.LogWarning("Should you really be raycasting directly against a poly with passThroughWalls = true?");
		}
		if (poly.sceneryOnly)
		{
			Debug.LogWarning("Should you really be raycasting directly against a poly with sceneryOnlyOrBackgroundLOD = true?");
		}
		Vector2 vector = end - start;
		float magnitude = vector.magnitude;
		Vector2 vector2 = vector / magnitude;
		Polygon polygon = poly.polygon;
		bool isClockwise = polygon.isClockwise;
		int vertCount = polygon.VertCount;
		for (int i = 0; i < vertCount; i++)
		{
			if (poly.EdgeIsCollidable(i))
			{
				int num = (i + 1) % vertCount;
				Vector3 vector3 = poly.transform.TransformPoint(polygon.vertices[i]);
				Vector3 vector4 = poly.transform.TransformPoint(polygon.vertices[num]);
				Line line = new Line(vector3, vector4);
				Vector3 vector5 = (isClockwise ? (vector4 - vector3) : (vector3 - vector4));
				float num2;
				if (line.RayLineIntersect(start, vector2, out num2) && num2 < magnitude && Vector2.Dot(new Vector2(-vector5.y, vector5.x).normalized, vector2) <= 0f)
				{
					hitPos = start + vector2 * num2;
					return true;
				}
			}
		}
		hitPos = Vector2.zero;
		return false;
	}

	// Token: 0x06000610 RID: 1552 RVA: 0x00030818 File Offset: 0x0002EA18
	public static bool InvisibleCollision(Vector2 position, Vector2 nextPosition, Range range, out Vector2 collisionPoint, bool checkInside = false)
	{
		collisionPoint = position;
		float num = Vector2.Distance(position, nextPosition);
		using (List<InvisibleCollision>.Enumerator enumerator = Level.current.invisibleCollision.Nearby(position, Range.infinity, num, null).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Intersect(position, nextPosition, range, checkInside, out collisionPoint))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06000611 RID: 1553 RVA: 0x00030898 File Offset: 0x0002EA98
	public static Ray ViewportPointToRay(Vector2 viewportPosNorm)
	{
		Vector2 vector = new Vector2(Mathf.LerpUnclamped(-1f, 1f, viewportPosNorm.x), Mathf.LerpUnclamped(-1f, 1f, viewportPosNorm.y));
		Camera camera = GameCamera.instance.camera;
		Matrix4x4 inverse = (camera.projectionMatrix * camera.transform.worldToLocalMatrix).inverse;
		Vector3 vector2 = inverse.MultiplyPoint(vector);
		Vector3 vector3 = inverse.MultiplyPoint(vector + 1f * Vector3.forward) - vector2;
		vector3.z *= -1f;
		return new Ray(vector2, vector3.normalized);
	}

	// Token: 0x06000612 RID: 1554 RVA: 0x00030958 File Offset: 0x0002EB58
	public static SlopeSample FindBestNearbySlopeSample(Level level, Vector3 searchPos, bool includeMusicRuns = false, float zChangeWeight = 3f)
	{
		SlopeSample slopeSample = default(SlopeSample);
		float num = float.MaxValue;
		List<Slope> list = level.slopes.Nearby(searchPos, Range.infinity, 50f, null);
		if (includeMusicRuns)
		{
			foreach (MusicRun musicRun in level.musicRuns)
			{
				list.Add(musicRun.slope);
			}
		}
		foreach (Slope slope in list)
		{
			SlopeSample slopeSample2 = slope.SampleAt(searchPos.x, false);
			if (slopeSample2.outOfRange)
			{
				if (slopeSample2.t < 0.5f)
				{
					slopeSample2.point = slopeSample2.slope.leftPoint;
				}
				else
				{
					slopeSample2.point = slopeSample2.slope.rightPoint;
				}
			}
			if (!Raycast.AnyPolyOccludes(slopeSample2.point, new Range(-3.4028235E+38f, slopeSample2.depth), slopeSample2.slope.originPoly, null))
			{
				Vector3 vector = slopeSample2.point3d - searchPos;
				if (vector.y > 0f)
				{
					vector.y *= 3f;
				}
				float num2 = Mathf.Abs(vector.y);
				vector.y -= Mathf.Sign(vector.y) * Mathf.Min(num2, 5f);
				vector.z *= zChangeWeight;
				float sqrMagnitude = vector.sqrMagnitude;
				if (sqrMagnitude < num)
				{
					num = sqrMagnitude;
					slopeSample = slopeSample2;
				}
			}
		}
		return slopeSample;
	}

	// Token: 0x040006FE RID: 1790
	public static Color historySampleRayColor = Color.yellow;

	// Token: 0x040006FF RID: 1791
	private static List<Poly> occlusionPolysScratchList = new List<Poly>();

	// Token: 0x020002EB RID: 747
	public struct RaycastResult
	{
		// Token: 0x040016CD RID: 5837
		public bool didHit;

		// Token: 0x040016CE RID: 5838
		public float distNorm;
	}

	// Token: 0x020002EC RID: 748
	public struct Collision
	{
		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x06001663 RID: 5731 RVA: 0x00098670 File Offset: 0x00096870
		public Vector3 pos3d
		{
			get
			{
				return new Vector3(this.pos.x, this.pos.y, (this.poly != null) ? this.poly.transform.position.z : this.slope.transform.position.z);
			}
		}

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x06001664 RID: 5732 RVA: 0x000986D2 File Offset: 0x000968D2
		public bool didHit
		{
			get
			{
				return this.poly != null || this.slope != null;
			}
		}

		// Token: 0x040016CF RID: 5839
		public Vector2 pos;

		// Token: 0x040016D0 RID: 5840
		public Vector2 normal;

		// Token: 0x040016D1 RID: 5841
		public bool isBehind;

		// Token: 0x040016D2 RID: 5842
		public float deltaNorm;

		// Token: 0x040016D3 RID: 5843
		public Poly poly;

		// Token: 0x040016D4 RID: 5844
		public Slope slope;

		// Token: 0x040016D5 RID: 5845
		public int i0;

		// Token: 0x040016D6 RID: 5846
		public bool climbable;

		// Token: 0x040016D7 RID: 5847
		public bool unreachable;
	}
}
