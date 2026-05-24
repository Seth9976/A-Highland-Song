using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000011 RID: 17
public static class Build
{
	// Token: 0x06000052 RID: 82 RVA: 0x000062E0 File Offset: 0x000044E0
	public static void BuildAllUnder(Transform transform)
	{
		Build._singleTransform.Clear();
		Build._singleTransform.Add(transform);
		Build.BuildAllUnder(Build._singleTransform);
		Build._singleTransform.Clear();
	}

	// Token: 0x06000053 RID: 83 RVA: 0x0000630C File Offset: 0x0000450C
	public static void BuildAllUnder(List<Transform> transforms)
	{
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		Runner instance = Runner.instance;
		if (instance == null)
		{
			throw new Exception("Need Runner for RunnerSettings's maxGroundAngle to build slopes");
		}
		RunnerSettings settings = instance.settings;
		if (transforms.Count == 0)
		{
			return;
		}
		List<Slope> list = null;
		Build._polys.Clear();
		Build.<>c__DisplayClass5_0 CS$<>8__locals1;
		CS$<>8__locals1.chunk = transforms[0].GetComponent<Chunk>();
		if (CS$<>8__locals1.chunk != null)
		{
			if (transforms.Count > 1)
			{
				Debug.LogWarning("When building a Chunk, expected just one transform to be passed in. Those behind first will be ignored");
			}
			Build._polys.AddRange(CS$<>8__locals1.chunk.polys);
			list = CS$<>8__locals1.chunk.slopes;
		}
		else
		{
			list = new List<Slope>(512);
			if (Application.isPlaying)
			{
				Debug.LogError("Can only Build Chunks in play mode, not general level sections");
				return;
			}
		}
		Build._buildSlopesScratch.Clear();
		for (int i = 0; i < Build._polys.Count; i++)
		{
			Poly poly = Build._polys[i];
			Transform transform = poly.transform;
			Polygon polygon = poly.polygon;
			MusicRun musicRun = poly.musicRun;
			if (musicRun != null)
			{
				Range range = musicRun.range;
			}
			Build.BuildSlope buildSlope = null;
			Build.BuildSlope buildSlope2 = null;
			if (!polygon.isClockwise)
			{
				polygon.vertices = polygon.vertices.Reverse<Vector2>().ToArray<Vector2>();
			}
			for (int j = 0; j < polygon.vertices.Length; j++)
			{
				int num = (j + 1 + polygon.vertices.Length) % polygon.vertices.Length;
				Vector3 vector = transform.TransformPoint(polygon.vertices[j]);
				Vector3 vector2 = transform.TransformPoint(polygon.vertices[num]);
				bool flag = poly.EdgeIsSlideable(j);
				bool flag2 = Poly.EdgeRunnableOrSlidable(vector, vector2, flag, true, poly);
				if (flag2)
				{
					if (buildSlope2 == null || buildSlope2.slideable != flag)
					{
						buildSlope2 = new Build.BuildSlope();
						buildSlope2.depth = transform.position.z;
						buildSlope2.originPoly = Build._polys[i];
						buildSlope2.slideable = flag;
						Build._buildSlopesScratch.Add(buildSlope2);
						if (j == 0)
						{
							buildSlope = buildSlope2;
						}
						buildSlope2.vertices.Add(vector);
					}
					buildSlope2.vertices.Add(vector2);
					poly.SetEdgeIsCollidable(j, false);
				}
				else
				{
					buildSlope2 = null;
					bool flag3 = musicRun == null;
					if (poly.passThroughWalls || poly.sceneryOnly)
					{
						flag3 = false;
					}
					poly.SetEdgeIsCollidable(j, flag3);
				}
				bool flag4 = flag2 && j == polygon.vertices.Length - 1;
				if (buildSlope != null && flag4 && buildSlope != buildSlope2 && buildSlope.slideable == buildSlope2.slideable)
				{
					buildSlope2.vertices.RemoveAt(buildSlope2.vertices.Count - 1);
					buildSlope2.vertices.AddRange(buildSlope.vertices);
					buildSlope.vertices = buildSlope2.vertices;
					int num2 = Build._buildSlopesScratch.Count - 1;
					Build._buildSlopesScratch.RemoveAt(num2);
					buildSlope2 = null;
				}
			}
		}
		float layerIntersectAuthoredSlopesDist = settings.layer.layerIntersectAuthoredSlopesDist;
		for (int k = 0; k < Build._buildSlopesScratch.Count; k++)
		{
			Build.BuildSlope buildSlope3 = Build._buildSlopesScratch[k];
			float z = buildSlope3.originPoly.transform.position.z;
			for (int l = k + 1; l < Build._buildSlopesScratch.Count; l++)
			{
				Build.BuildSlope buildSlope4 = Build._buildSlopesScratch[l];
				float z2 = buildSlope4.originPoly.transform.position.z;
				if (Mathf.Abs(z - z2) <= layerIntersectAuthoredSlopesDist)
				{
					for (int m = 0; m < buildSlope3.vertices.Count - 1; m++)
					{
						for (int n = 0; n < buildSlope4.vertices.Count - 1; n++)
						{
							Vector2 vector3 = buildSlope3.vertices[m];
							Vector2 vector4 = buildSlope3.vertices[m + 1];
							Vector2 vector5 = buildSlope4.vertices[n];
							Vector2 vector6 = buildSlope4.vertices[n + 1];
							Vector2 vector7;
							if (Line.LineIntersectionPoint(new Line(vector3, vector4), new Line(vector5, vector6), out vector7, true, true))
							{
								bool flag5 = false;
								foreach (Poly poly2 in Build._polys)
								{
									if (!(poly2 == buildSlope3.originPoly) && !(poly2 == buildSlope4.originPoly) && Range.Auto(buildSlope3.depth, buildSlope4.depth).Contains(poly2.transform.position.z))
									{
										Vector3 vector8 = poly2.transform.InverseTransformPoint(vector7);
										if (poly2.polygon.ContainsPoint(vector8))
										{
											flag5 = true;
											break;
										}
									}
								}
								if (!flag5 && (((Vector2.Distance(vector7, vector3) >= 0.1f || m != 0) && (Vector2.Distance(vector7, vector4) >= 0.1f || m + 1 != buildSlope3.vertices.Count - 1)) || ((Vector2.Distance(vector7, vector5) >= 0.1f || n != 0) && (Vector2.Distance(vector7, vector6) >= 0.1f || n + 1 != buildSlope4.vertices.Count - 1))))
								{
									Build.BuildSlope buildSlope5 = Build.<BuildAllUnder>g__Slice|5_0(buildSlope3, vector7, m + 1);
									if (buildSlope5.length > 0.2f)
									{
										Build._buildSlopesScratch.Add(buildSlope5);
									}
									Build.BuildSlope buildSlope6 = Build.<BuildAllUnder>g__Slice|5_0(buildSlope4, vector7, n + 1);
									if (buildSlope6.length > 0.2f)
									{
										Build._buildSlopesScratch.Add(buildSlope6);
									}
								}
							}
						}
					}
				}
			}
			foreach (Poly poly3 in Build._polys)
			{
				if (!(buildSlope3.originPoly == poly3))
				{
					float z3 = poly3.transform.position.z;
					Range range2 = settings.layer.layerCollideCurrentRange + z;
					Range range3 = settings.layer.layerCollideNearbyRange + z;
					if (z3 <= range2.max && z3 >= range3.min)
					{
						Transform transform2 = poly3.transform;
						Vector2[] vertices = poly3.polygon.vertices;
						for (int num3 = 0; num3 < buildSlope3.vertices.Count - 1; num3++)
						{
							for (int num4 = 0; num4 < vertices.Length; num4++)
							{
								Vector2 vector9 = buildSlope3.vertices[num3];
								Vector2 vector10 = buildSlope3.vertices[num3 + 1];
								Vector3 vector11 = poly3.PointIdx(num4);
								Vector3 vector12 = poly3.PointIdx((num4 + 1) % vertices.Length);
								Vector2 vector13;
								if (Line.LineIntersectionPoint(new Line(vector9, vector10), new Line(vector11, vector12), out vector13, true, true) && (Vector2.Distance(vector13, vector9) >= 0.1f || num3 != 0) && (Vector2.Distance(vector13, vector10) >= 0.1f || num3 + 1 != buildSlope3.vertices.Count - 1))
								{
									Build.BuildSlope buildSlope7 = Build.<BuildAllUnder>g__Slice|5_0(buildSlope3, vector13, num3 + 1);
									if (buildSlope7.length > 0.2f)
									{
										Build._buildSlopesScratch.Add(buildSlope7);
									}
									if (num3 >= buildSlope3.vertices.Count - 1)
									{
										break;
									}
								}
							}
						}
					}
				}
			}
		}
		if (CS$<>8__locals1.chunk == null)
		{
			for (int num5 = 0; num5 < Build._buildSlopesScratch.Count; num5++)
			{
				Build.BuildSlope buildSlope8 = Build._buildSlopesScratch[num5];
				MusicRun musicRun2 = buildSlope8.originPoly.musicRun;
				if (!(musicRun2 == null))
				{
					Range range4 = musicRun2.range;
					if (range4.Contains(buildSlope8.range))
					{
						buildSlope8.isMusicRun = true;
					}
					else if (range4.Intersects(buildSlope8.range))
					{
						for (int num6 = 0; num6 < buildSlope8.vertices.Count - 1; num6++)
						{
							Vector2 vector14 = buildSlope8.vertices[num6];
							Vector2 vector15 = buildSlope8.vertices[num6 + 1];
							Range range5 = new Range(vector14.x, vector15.x);
							if (range5.Contains(range4.min))
							{
								float num7 = range5.InverseLerp(range4.min);
								Vector2 vector16 = Vector2.Lerp(vector14, vector15, num7);
								if ((num6 != 0 || Vector2.Distance(vector16, vector14) >= 0.1f) && (num6 + 1 != buildSlope8.vertices.Count - 1 || Vector2.Distance(vector16, vector15) >= 0.1f))
								{
									Build.BuildSlope buildSlope9 = Build.<BuildAllUnder>g__Slice|5_0(buildSlope8, vector16, num6 + 1);
									buildSlope8.isMusicRun = false;
									buildSlope9.isMusicRun = true;
									if (buildSlope9.length > 0.2f)
									{
										Build._buildSlopesScratch.Add(buildSlope9);
									}
								}
							}
							else if (range5.Contains(range4.max))
							{
								float num8 = range5.InverseLerp(range4.max);
								Vector2 vector17 = Vector2.Lerp(vector14, vector15, num8);
								if ((num6 != 0 || Vector2.Distance(vector17, vector14) >= 0.1f) && (num6 + 1 != buildSlope8.vertices.Count - 1 || Vector2.Distance(vector17, vector15) >= 0.1f))
								{
									Build.BuildSlope buildSlope10 = Build.<BuildAllUnder>g__Slice|5_0(buildSlope8, vector17, num6 + 1);
									buildSlope8.isMusicRun = true;
									buildSlope10.isMusicRun = false;
									if (buildSlope10.length > 0.2f)
									{
										Build._buildSlopesScratch.Add(buildSlope10);
									}
								}
							}
						}
					}
				}
			}
		}
		Build._buildSlopesScratch.RemoveAll((Build.BuildSlope slope) => slope.length < 0.5f);
		foreach (Build.BuildSlope buildSlope11 in Build._buildSlopesScratch)
		{
			int num9 = buildSlope11.vertices.Count - 1;
			if (buildSlope11.vertices[0].x > buildSlope11.vertices[num9].x)
			{
				Debug.LogError("Slope ran backwards after build, whats wrong? Click for origin poly", buildSlope11.originPoly);
			}
			else
			{
				for (int num10 = 0; num10 < buildSlope11.vertices.Count - 1; num10++)
				{
					Vector2 vector18 = buildSlope11.vertices[num10];
					if (buildSlope11.vertices[num10 + 1].x < vector18.x)
					{
						buildSlope11.vertices.RemoveAt(num10 + 1);
					}
				}
			}
		}
		for (int num11 = Build._buildSlopesScratch.Count - 1; num11 >= 0; num11--)
		{
			Build.BuildSlope buildSlope12 = Build._buildSlopesScratch[num11];
			for (int num12 = 0; num12 < buildSlope12.vertices.Count; num12++)
			{
				Build._slopeVerticesOccluded[num12] = false;
			}
			int num13 = 0;
			foreach (Poly poly4 in Build._polys)
			{
				if (!(buildSlope12.originPoly == poly4))
				{
					float z4 = poly4.transform.position.z;
					if (buildSlope12.depth >= z4 - 0.001f && buildSlope12.depth <= z4 + 40f)
					{
						int num14 = buildSlope12.vertices.Count - 1;
						for (int num15 = 0; num15 < buildSlope12.vertices.Count; num15++)
						{
							Vector2 vector19 = buildSlope12.vertices[num15];
							if (num15 == 0)
							{
								vector19 = Vector2.Lerp(buildSlope12.vertices[num15], buildSlope12.vertices[num15 + 1], 0.1f);
							}
							if (num15 == num14)
							{
								vector19 = Vector2.Lerp(buildSlope12.vertices[num14 - 1], buildSlope12.vertices[num14], 0.9f);
							}
							Vector3 vector20 = poly4.transform.InverseTransformPoint(vector19);
							if (poly4.polygon.ContainsPoint(vector20) && !Build._slopeVerticesOccluded[num15])
							{
								Build._slopeVerticesOccluded[num15] = true;
								num13++;
							}
						}
					}
				}
			}
			if (num13 == buildSlope12.vertices.Count)
			{
				Build._buildSlopesScratch.RemoveAt(num11);
			}
		}
		Build.<BuildAllUnder>g__RecycleOrDeleteSlopes|5_2(list, ref CS$<>8__locals1);
		if (!Application.isPlaying)
		{
			foreach (Poly poly5 in Build._polys)
			{
				poly5.GetComponentsInChildren<Slope>(true, list);
				Build.<BuildAllUnder>g__RecycleOrDeleteSlopes|5_2(list, ref CS$<>8__locals1);
			}
		}
		foreach (Build.BuildSlope buildSlope13 in Build._buildSlopesScratch)
		{
			SlopeFactory.Creation creation = SlopeFactory.Create(buildSlope13.vertices, buildSlope13.originPoly.transform.position.z, CS$<>8__locals1.chunk != null, buildSlope13.originPoly, buildSlope13.autoRunSlopeGroup);
			Slope slope4 = creation.slope;
			slope4.chunk = CS$<>8__locals1.chunk;
			slope4.originPoly = buildSlope13.originPoly;
			slope4.isSlide = buildSlope13.slideable;
			buildSlope13.autoRunSlopeGroup = creation.group;
			list.Add(slope4);
		}
		Build.AutoConnectSlopes(list, 7f);
		if (CS$<>8__locals1.chunk != null)
		{
			Build.ForceConnectChunkGapHintSlopes(CS$<>8__locals1.chunk, list);
		}
		if (!Application.isPlaying && CS$<>8__locals1.chunk == null)
		{
			foreach (Transform transform3 in transforms)
			{
				foreach (MusicRun musicRun3 in transform3.GetComponentsInChildren<MusicRun>())
				{
					foreach (Slope slope2 in musicRun3.slope.leftSlopes)
					{
						slope2.rightSlopes.Remove(musicRun3.slope);
					}
					foreach (Slope slope3 in musicRun3.slope.rightSlopes)
					{
						slope3.leftSlopes.Remove(musicRun3.slope);
					}
					musicRun3.slope.gameObject.SetActive(false);
					bool flag6 = false;
					using (List<Poly.EdgeFlags>.Enumerator enumerator5 = musicRun3.poly.edgeFlags.GetEnumerator())
					{
						while (enumerator5.MoveNext())
						{
							if ((enumerator5.Current & Poly.EdgeFlags.Climbable) > Poly.EdgeFlags.None)
							{
								flag6 = true;
								break;
							}
						}
					}
					if (flag6)
					{
						if (musicRun3.poly.allClimbable)
						{
							Debug.LogError("Music run poly " + musicRun3.name + " has climbable edges. Please un-check 'all climbable' flag and double check that this poly is only used as a music run and has nothing that's meant to be climbable", musicRun3);
						}
						else
						{
							Debug.LogError("Music run poly " + musicRun3.name + " has climbable edges. Please check that this poly is only used as a music run and has nothing that's meant to be climbable", musicRun3);
						}
					}
				}
			}
		}
		if (CS$<>8__locals1.chunk != null)
		{
			CS$<>8__locals1.chunk.startMarker.slope = null;
			CS$<>8__locals1.chunk.startMarker.slope = Build.<BuildAllUnder>g__FindSlopeForMarker|5_3(CS$<>8__locals1.chunk.startMarker, ref CS$<>8__locals1);
			CS$<>8__locals1.chunk.endMarker.slope = null;
			CS$<>8__locals1.chunk.endMarker.slope = Build.<BuildAllUnder>g__FindSlopeForMarker|5_3(CS$<>8__locals1.chunk.endMarker, ref CS$<>8__locals1);
			CS$<>8__locals1.chunk.leftSlope = CS$<>8__locals1.chunk.GetConnectorEast(false).slope;
			CS$<>8__locals1.chunk.rightSlope = CS$<>8__locals1.chunk.GetConnectorEast(true).slope;
			Build.<BuildAllUnder>g__SnapMarkerToSlopeEdge|5_4(CS$<>8__locals1.chunk.startMarker);
			Build.<BuildAllUnder>g__SnapMarkerToSlopeEdge|5_4(CS$<>8__locals1.chunk.endMarker);
		}
		Build._allSplats.Clear();
		foreach (Transform transform4 in transforms)
		{
			transform4.GetComponentsInChildren<Splat>(Build._allSplats);
			foreach (Splat splat in Build._allSplats)
			{
				splat.Refresh();
			}
		}
		Build._allSplats.Clear();
		foreach (Poly poly6 in Build._polys)
		{
			poly6.RefreshClimbables();
		}
		stopwatch.Stop();
	}

	// Token: 0x06000054 RID: 84 RVA: 0x00007580 File Offset: 0x00005780
	private static void TryConnectSlopes(Slope s0, Slope s1, bool s0Left, bool s1Left, float intersectDist)
	{
		int num = (s0Left ? 0 : (s0.numberOfPoints - 1));
		int num2 = (s1Left ? 0 : (s1.numberOfPoints - 1));
		Vector3 vector = s0.transform.TransformPoint(s0.localPoints[num]);
		Vector3 vector2 = s1.transform.TransformPoint(s1.localPoints[num2]);
		float num3 = Mathf.Abs(vector.z - vector2.z);
		if (Vector2.Distance(vector, vector2) < 0.4f && num3 < intersectDist)
		{
			Vector3 vector3 = vector;
			List<Vector2> localPoints = s0.localPoints;
			localPoints[num] = s0.transform.InverseTransformPoint(vector3);
			s0.localPoints = localPoints;
			List<Vector2> localPoints2 = s1.localPoints;
			localPoints2[num2] = s1.transform.InverseTransformPoint(vector3);
			s1.localPoints = localPoints2;
			if (s0Left)
			{
				s0.connectedLeft = true;
			}
			else
			{
				s0.connectedRight = true;
			}
			if (s1Left)
			{
				s1.connectedLeft = true;
			}
			else
			{
				s1.connectedRight = true;
			}
			List<Slope> list = (s0Left ? s0.leftSlopes : s0.rightSlopes);
			if (!list.Contains(s1))
			{
				list.Add(s1);
			}
			List<Slope> list2 = (s1Left ? s1.leftSlopes : s1.rightSlopes);
			if (!list2.Contains(s0))
			{
				list2.Add(s0);
			}
		}
	}

	// Token: 0x06000055 RID: 85 RVA: 0x000076E4 File Offset: 0x000058E4
	public static void ConnectCrossReferencedSlopes(List<LevelSection> levelSections)
	{
		Build._allCrossReferencedSlopes.Clear();
		foreach (LevelSection levelSection in levelSections)
		{
			Build._allCrossReferencedSlopes.AddRange(levelSection.crossReferencedSlopes);
			levelSection.crossReferencedSlopesHaveBeenConnected = true;
		}
		Build.TryConnectAllSlopes(Build._allCrossReferencedSlopes, 7f);
		Build._allCrossReferencedSlopes.Clear();
	}

	// Token: 0x06000056 RID: 86 RVA: 0x00007768 File Offset: 0x00005968
	private static void TryConnectAllSlopes(List<Slope> slopes, float autoConnectDistance = 7f)
	{
		for (int i = 0; i < slopes.Count; i++)
		{
			Slope slope = slopes[i];
			for (int j = i + 1; j < slopes.Count; j++)
			{
				Slope slope2 = slopes[j];
				Build.TryConnectSlopes(slope, slope2, false, false, 1.5f);
				Build.TryConnectSlopes(slope, slope2, false, true, 1.5f);
				Build.TryConnectSlopes(slope, slope2, true, false, 1.5f);
				Build.TryConnectSlopes(slope, slope2, true, true, 1.5f);
			}
		}
		for (int k = 0; k < slopes.Count; k++)
		{
			Slope slope3 = slopes[k];
			if (slope3.rightSlopes.Count <= 0)
			{
				Vector3 rightPoint = slope3.rightPoint;
				float z = slope3.transform.position.z;
				float num = autoConnectDistance;
				Slope slope4 = null;
				for (int l = 0; l < slopes.Count; l++)
				{
					if (k != l)
					{
						Slope slope5 = slopes[l];
						if (slope5.leftSlopes.Count <= 0 && Mathf.Abs(slope5.transform.position.z - z) <= 1.5f)
						{
							Vector3 leftPoint = slope5.leftPoint;
							float num2 = Vector2.Distance(rightPoint, leftPoint);
							if (num2 < num)
							{
								num = num2;
								slope4 = slope5;
							}
						}
					}
				}
				if (slope4)
				{
					slope3.rightSlopes.Add(slope4);
					slope4.leftSlopes.Add(slope3);
				}
			}
		}
	}

	// Token: 0x06000057 RID: 87 RVA: 0x000078E0 File Offset: 0x00005AE0
	private static bool TryAddSceneCrossReference(Slope s0, Slope s1, bool direct)
	{
		if (!Application.isPlaying)
		{
			LevelSection component = s0.transform.root.GetComponent<LevelSection>();
			LevelSection component2 = s1.transform.root.GetComponent<LevelSection>();
			if (component != component2)
			{
				if (!component.crossReferencedSlopes.Contains(s0))
				{
					component.crossReferencedSlopes.Add(s0);
				}
				if (!component2.crossReferencedSlopes.Contains(s1))
				{
					component2.crossReferencedSlopes.Add(s1);
				}
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000058 RID: 88 RVA: 0x00007958 File Offset: 0x00005B58
	public static void AutoConnectSlopes(List<Slope> allSlopes, float autoConnectDistance = 7f)
	{
		foreach (Slope slope in allSlopes)
		{
			slope.ReverseIfNecessary();
		}
		foreach (Slope slope2 in allSlopes)
		{
			slope2.leftSlopes.Clear();
			slope2.rightSlopes.Clear();
		}
		Range layerCollideNearbyRange = Runner.instance.settings.layer.layerCollideNearbyRange;
		foreach (Slope slope3 in allSlopes)
		{
			slope3.connectedLeft = false;
			slope3.connectedRight = false;
		}
		Build.TryConnectAllSlopes(allSlopes, autoConnectDistance);
	}

	// Token: 0x06000059 RID: 89 RVA: 0x00007A4C File Offset: 0x00005C4C
	private static void ForceConnectChunkGapHintSlopes(Chunk chunk, List<Slope> slopes)
	{
		foreach (ChunkGapHint chunkGapHint in chunk.gapHints)
		{
			Vector2 vector = chunkGapHint.transform.position;
			Slope slope = null;
			float num = float.MaxValue;
			Slope slope2 = null;
			float num2 = float.MaxValue;
			chunkGapHint.foundLeftSlope = null;
			chunkGapHint.foundRightSlope = null;
			chunkGapHint.leftHintWasRequired = false;
			chunkGapHint.rightHintWasRequired = false;
			foreach (Slope slope3 in slopes)
			{
				Vector3 rightPoint = slope3.rightPoint;
				if (rightPoint.x < vector.x)
				{
					float num3 = Vector2.Distance(vector, rightPoint);
					if (slope == null || num3 < num)
					{
						slope = slope3;
						num = num3;
					}
				}
				else
				{
					Vector3 leftPoint = slope3.leftPoint;
					if (leftPoint.x > vector.x)
					{
						float num4 = Vector2.Distance(vector, leftPoint);
						if (slope2 == null || num4 < num2)
						{
							slope2 = slope3;
							num2 = num4;
						}
					}
				}
			}
			if (slope != null && slope2 != null)
			{
				if (!slope.rightSlopes.Contains(slope2))
				{
					slope.rightSlopes.Add(slope2);
					chunkGapHint.leftHintWasRequired = true;
				}
				if (!slope2.leftSlopes.Contains(slope))
				{
					slope2.leftSlopes.Add(slope);
					chunkGapHint.rightHintWasRequired = true;
				}
				chunkGapHint.foundLeftSlope = slope;
				chunkGapHint.foundRightSlope = slope2;
			}
			else if (slope == null && slope2 == null)
			{
				Debug.LogError("ChunkGapHint in " + chunk.name + " couldn't find ANY slopes to connect to!", chunkGapHint);
			}
			else if (slope == null)
			{
				Debug.LogError("ChunkGapHint in " + chunk.name + " couldn't find a valid leftSlope to connect to", chunkGapHint);
			}
			else if (slope2 == null)
			{
				Debug.LogError("ChunkGapHint in " + chunk.name + " couldn't find a valid rightSlope to connect to", chunkGapHint);
			}
		}
	}

	// Token: 0x0600005B RID: 91 RVA: 0x00007CD0 File Offset: 0x00005ED0
	[CompilerGenerated]
	internal static Build.BuildSlope <BuildAllUnder>g__Slice|5_0(Build.BuildSlope existing, Vector2 intersectionPoint, int startIdx)
	{
		Build.BuildSlope buildSlope = new Build.BuildSlope();
		buildSlope.depth = existing.depth;
		buildSlope.originPoly = existing.originPoly;
		buildSlope.slideable = existing.slideable;
		buildSlope.vertices.Add(intersectionPoint);
		for (int i = startIdx; i < existing.vertices.Count; i++)
		{
			Vector2 vector = existing.vertices[i];
			if (i != startIdx || Vector2.Distance(intersectionPoint, vector) >= 0.05f)
			{
				buildSlope.vertices.Add(vector);
			}
		}
		existing.vertices.RemoveRange(startIdx, existing.vertices.Count - startIdx);
		int num = startIdx - 1;
		if (Vector2.Distance(intersectionPoint, existing.vertices[num]) < 0.05f)
		{
			existing.vertices.RemoveAt(num);
		}
		existing.vertices.Add(intersectionPoint);
		return buildSlope;
	}

	// Token: 0x0600005C RID: 92 RVA: 0x00007DA4 File Offset: 0x00005FA4
	[CompilerGenerated]
	internal static void <BuildAllUnder>g__RecycleOrDeleteSlopes|5_2(List<Slope> slopes, ref Build.<>c__DisplayClass5_0 A_1)
	{
		if (A_1.chunk != null)
		{
			A_1.chunk.RecycleSlopesAndRemoveRefs();
			return;
		}
		foreach (Slope slope in slopes)
		{
			Object.DestroyImmediate(slope.gameObject, true);
		}
		slopes.Clear();
	}

	// Token: 0x0600005D RID: 93 RVA: 0x00007E18 File Offset: 0x00006018
	[CompilerGenerated]
	internal static Slope <BuildAllUnder>g__FindSlopeForMarker|5_3(ChunkStartEnd marker, ref Build.<>c__DisplayClass5_0 A_1)
	{
		Slope slope = null;
		Vector3 point = marker.point;
		float num = float.MaxValue;
		foreach (Slope slope2 in A_1.chunk.slopes)
		{
			float num2 = Vector2.Distance(slope2.SampleAt(point.x, false).clampedPoint, point);
			if (num2 < num)
			{
				num = num2;
				slope = slope2;
			}
		}
		return slope;
	}

	// Token: 0x0600005E RID: 94 RVA: 0x00007EA8 File Offset: 0x000060A8
	[CompilerGenerated]
	internal static void <BuildAllUnder>g__SnapMarkerToSlopeEdge|5_4(ChunkStartEnd marker)
	{
		bool flag = marker.isStart ^ !marker.rightwards;
		if (marker.slope == null)
		{
			Debug.LogError("Marker had no slope", marker);
			return;
		}
		marker.transform.position = (flag ? marker.slope.leftPoint : marker.slope.rightPoint);
	}

	// Token: 0x04000073 RID: 115
	private static List<Build.BuildSlope> _buildSlopesScratch = new List<Build.BuildSlope>(128);

	// Token: 0x04000074 RID: 116
	private static List<Poly> _polys = new List<Poly>(2048);

	// Token: 0x04000075 RID: 117
	private static List<Transform> _singleTransform = new List<Transform>(1);

	// Token: 0x04000076 RID: 118
	private const float depthIntersectDist = 1.5f;

	// Token: 0x04000077 RID: 119
	private static List<Splat> _allSplats = new List<Splat>(64);

	// Token: 0x04000078 RID: 120
	private static List<Slope> _allCrossReferencedSlopes = new List<Slope>();

	// Token: 0x04000079 RID: 121
	private static List<ValueTuple<Slideable, Poly>> _allSlideables = new List<ValueTuple<Slideable, Poly>>();

	// Token: 0x0400007A RID: 122
	private static bool[] _slopeVerticesOccluded = new bool[512];

	// Token: 0x02000247 RID: 583
	private class BuildSlope
	{
		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x060014B1 RID: 5297 RVA: 0x0008E9B8 File Offset: 0x0008CBB8
		public float length
		{
			get
			{
				float num = 0f;
				for (int i = 0; i < this.vertices.Count - 1; i++)
				{
					num += Vector2.Distance(this.vertices[i], this.vertices[i + 1]);
				}
				return num;
			}
		}

		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x060014B2 RID: 5298 RVA: 0x0008EA06 File Offset: 0x0008CC06
		public Range range
		{
			get
			{
				return new Range(this.vertices[0].x, this.vertices[this.vertices.Count - 1].x);
			}
		}

		// Token: 0x040013C6 RID: 5062
		public List<Vector2> vertices = new List<Vector2>();

		// Token: 0x040013C7 RID: 5063
		public float depth;

		// Token: 0x040013C8 RID: 5064
		public Poly originPoly;

		// Token: 0x040013C9 RID: 5065
		public Transform autoRunSlopeGroup;

		// Token: 0x040013CA RID: 5066
		public bool isMusicRun;

		// Token: 0x040013CB RID: 5067
		public bool slideable;
	}
}
