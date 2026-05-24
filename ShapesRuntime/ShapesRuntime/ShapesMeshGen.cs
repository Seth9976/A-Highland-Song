using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000062 RID: 98
	public static class ShapesMeshGen
	{
		// Token: 0x06000A5F RID: 2655 RVA: 0x000257F8 File Offset: 0x000239F8
		private static bool SamePosition(Vector3 a, Vector3 b)
		{
			return Mathf.Max(Mathf.Max(Mathf.Abs(b.x - a.x), Mathf.Abs(b.y - a.y)), Mathf.Abs(b.z - a.z)) < 1E-05f;
		}

		// Token: 0x06000A60 RID: 2656 RVA: 0x0002584C File Offset: 0x00023A4C
		public static void GenPolylineMesh(Mesh mesh, IList<PolylinePoint> path, bool closed, PolylineJoins joins, bool flattenZ, bool useColors)
		{
			ShapesMeshGen.<>c__DisplayClass1_0 CS$<>8__locals1;
			CS$<>8__locals1.path = path;
			mesh.Clear();
			int num = CS$<>8__locals1.path.Count;
			if (num < 2)
			{
				return;
			}
			if (num == 2 && closed)
			{
				closed = false;
			}
			PolylinePoint polylinePoint = CS$<>8__locals1.path[0];
			PolylinePoint polylinePoint2 = CS$<>8__locals1.path[CS$<>8__locals1.path.Count - 1];
			if ((closed || num == 2) && ShapesMeshGen.SamePosition(polylinePoint.point, polylinePoint2.point))
			{
				num--;
				if (num < 2)
				{
					return;
				}
				polylinePoint2 = CS$<>8__locals1.path[CS$<>8__locals1.path.Count - 2];
			}
			bool flag = joins.HasJoinMesh();
			bool flag2 = joins.HasSimpleJoin();
			int num2 = (flag ? 5 : 2);
			int num3 = (flag ? 4 : 2);
			int num4 = num * num2;
			int num5 = num4;
			int num6 = (closed ? num : (num - 1)) * num3 * 3;
			int[] array = null;
			int num7 = 0;
			if (flag)
			{
				num7 = (flag2 ? 3 : 5);
				object obj = (closed ? num : (num - 2));
				int num8 = (flag2 ? 1 : 3);
				object obj2 = obj;
				int num9 = obj2 * num8 * 3;
				int num10 = obj2 * num7;
				num5 += num10;
				array = new int[num9];
			}
			Color[] array2 = (useColors ? new Color[num5] : null);
			Vector3[] array3 = new Vector3[num5];
			CS$<>8__locals1.meshUv0 = new Vector4[num5];
			CS$<>8__locals1.meshUv1Prevs = new Vector3[num5];
			CS$<>8__locals1.meshUv2Nexts = new Vector3[num5];
			CS$<>8__locals1.meshTriangles = new int[num6];
			int num11 = 0;
			int num12 = 0;
			int num13 = 0;
			int num14 = 0;
			int num15 = 0;
			int num16 = 0;
			int num17 = 0;
			int num18 = 0;
			CS$<>8__locals1.triId = 0;
			int num19 = 0;
			ShapesMeshGen.<>c__DisplayClass1_1 CS$<>8__locals2;
			CS$<>8__locals2.i = 0;
			while (CS$<>8__locals2.i < num)
			{
				bool flag3 = CS$<>8__locals2.i == num - 1;
				bool flag4 = CS$<>8__locals2.i == 0;
				bool flag5 = closed || (!flag3 && !flag4);
				ShapesMeshGen.<>c__DisplayClass1_2 CS$<>8__locals3;
				CS$<>8__locals3.uvEndpointValue = (float)((!closed && (flag4 || flag3)) ? (flag4 ? (-1) : 1) : 0);
				Vector3 vector = (flattenZ ? new Vector3(CS$<>8__locals1.path[CS$<>8__locals2.i].point.x, CS$<>8__locals1.path[CS$<>8__locals2.i].point.y, 0f) : CS$<>8__locals1.path[CS$<>8__locals2.i].point);
				Color color = (useColors ? CS$<>8__locals1.path[CS$<>8__locals2.i].color.ColorSpaceAdjusted() : default(Color));
				int num20 = CS$<>8__locals2.i * num2;
				int num21;
				if (flag)
				{
					num21 = num20 + 1;
					num11 = num20 + 2;
					num12 = num20 + 3;
					num13 = num20 + 4;
					array3[num20] = vector;
					array3[num21] = vector;
					array3[num11] = vector;
					array3[num12] = vector;
					array3[num13] = vector;
					if (useColors)
					{
						array2[num20] = color;
						array2[num21] = color;
						array2[num11] = color;
						array2[num12] = color;
						array2[num13] = color;
					}
					if (flag5)
					{
						num14 = (closed ? CS$<>8__locals2.i : (CS$<>8__locals2.i - 1)) * num7 + num4;
						num15 = num14 + 1;
						num16 = num14 + 2;
						num17 = num14 + 3;
						num18 = num14 + 4;
						array3[num14] = vector;
						array3[num15] = vector;
						array3[num16] = vector;
						if (useColors)
						{
							array2[num14] = color;
							array2[num15] = color;
							array2[num16] = color;
						}
						if (!flag2)
						{
							array3[num17] = vector;
							array3[num18] = vector;
							if (useColors)
							{
								array2[num17] = color;
								array2[num18] = color;
							}
						}
					}
				}
				else
				{
					num21 = num20 + 1;
					array3[num20] = vector;
					array3[num21] = vector;
					if (useColors)
					{
						array2[num20] = color;
						array2[num21] = color;
					}
				}
				if (CS$<>8__locals2.i == 0)
				{
					CS$<>8__locals3.prevPos = (closed ? polylinePoint2.point : (polylinePoint.point * 2f - CS$<>8__locals1.path[1].point));
					CS$<>8__locals3.nextPos = CS$<>8__locals1.path[CS$<>8__locals2.i + 1].point;
				}
				else if (CS$<>8__locals2.i == num - 1)
				{
					CS$<>8__locals3.prevPos = CS$<>8__locals1.path[CS$<>8__locals2.i - 1].point;
					CS$<>8__locals3.nextPos = (closed ? polylinePoint.point : (CS$<>8__locals1.path[num - 1].point * 2f - CS$<>8__locals1.path[num - 2].point));
				}
				else
				{
					CS$<>8__locals3.prevPos = CS$<>8__locals1.path[CS$<>8__locals2.i - 1].point;
					CS$<>8__locals3.nextPos = CS$<>8__locals1.path[CS$<>8__locals2.i + 1].point;
				}
				ShapesMeshGen.<GenPolylineMesh>g__SetPrevNext|1_1(num20, ref CS$<>8__locals1, ref CS$<>8__locals3);
				ShapesMeshGen.<GenPolylineMesh>g__SetPrevNext|1_1(num21, ref CS$<>8__locals1, ref CS$<>8__locals3);
				if (flag)
				{
					ShapesMeshGen.<GenPolylineMesh>g__SetPrevNext|1_1(num11, ref CS$<>8__locals1, ref CS$<>8__locals3);
					ShapesMeshGen.<GenPolylineMesh>g__SetPrevNext|1_1(num12, ref CS$<>8__locals1, ref CS$<>8__locals3);
					ShapesMeshGen.<GenPolylineMesh>g__SetPrevNext|1_1(num13, ref CS$<>8__locals1, ref CS$<>8__locals3);
					if (flag5)
					{
						ShapesMeshGen.<GenPolylineMesh>g__SetPrevNext|1_1(num14, ref CS$<>8__locals1, ref CS$<>8__locals3);
						ShapesMeshGen.<GenPolylineMesh>g__SetPrevNext|1_1(num15, ref CS$<>8__locals1, ref CS$<>8__locals3);
						ShapesMeshGen.<GenPolylineMesh>g__SetPrevNext|1_1(num16, ref CS$<>8__locals1, ref CS$<>8__locals3);
						if (!flag2)
						{
							ShapesMeshGen.<GenPolylineMesh>g__SetPrevNext|1_1(num17, ref CS$<>8__locals1, ref CS$<>8__locals3);
							ShapesMeshGen.<GenPolylineMesh>g__SetPrevNext|1_1(num18, ref CS$<>8__locals1, ref CS$<>8__locals3);
						}
					}
				}
				if (flag)
				{
					ShapesMeshGen.<GenPolylineMesh>g__SetUv0|1_0(num20, 0f, 0f, ref CS$<>8__locals1, ref CS$<>8__locals2, ref CS$<>8__locals3);
					ShapesMeshGen.<GenPolylineMesh>g__SetUv0|1_0(num21, -1f, -1f, ref CS$<>8__locals1, ref CS$<>8__locals2, ref CS$<>8__locals3);
					ShapesMeshGen.<GenPolylineMesh>g__SetUv0|1_0(num11, -1f, 1f, ref CS$<>8__locals1, ref CS$<>8__locals2, ref CS$<>8__locals3);
					ShapesMeshGen.<GenPolylineMesh>g__SetUv0|1_0(num12, 1f, -1f, ref CS$<>8__locals1, ref CS$<>8__locals2, ref CS$<>8__locals3);
					ShapesMeshGen.<GenPolylineMesh>g__SetUv0|1_0(num13, 1f, 1f, ref CS$<>8__locals1, ref CS$<>8__locals2, ref CS$<>8__locals3);
					if (flag5)
					{
						ShapesMeshGen.<GenPolylineMesh>g__SetUv0|1_0(num14, 0f, 0f, ref CS$<>8__locals1, ref CS$<>8__locals2, ref CS$<>8__locals3);
						if (flag2)
						{
							ShapesMeshGen.<GenPolylineMesh>g__SetUv0|1_0(num15, 1f, -1f, ref CS$<>8__locals1, ref CS$<>8__locals2, ref CS$<>8__locals3);
							ShapesMeshGen.<GenPolylineMesh>g__SetUv0|1_0(num16, 1f, 1f, ref CS$<>8__locals1, ref CS$<>8__locals2, ref CS$<>8__locals3);
						}
						else
						{
							ShapesMeshGen.<GenPolylineMesh>g__SetUv0|1_0(num15, 1f, -1f, ref CS$<>8__locals1, ref CS$<>8__locals2, ref CS$<>8__locals3);
							ShapesMeshGen.<GenPolylineMesh>g__SetUv0|1_0(num16, -1f, -1f, ref CS$<>8__locals1, ref CS$<>8__locals2, ref CS$<>8__locals3);
							ShapesMeshGen.<GenPolylineMesh>g__SetUv0|1_0(num17, -1f, 1f, ref CS$<>8__locals1, ref CS$<>8__locals2, ref CS$<>8__locals3);
							ShapesMeshGen.<GenPolylineMesh>g__SetUv0|1_0(num18, 1f, 1f, ref CS$<>8__locals1, ref CS$<>8__locals2, ref CS$<>8__locals3);
						}
					}
				}
				else
				{
					ShapesMeshGen.<GenPolylineMesh>g__SetUv0|1_0(num20, -1f, (float)CS$<>8__locals2.i, ref CS$<>8__locals1, ref CS$<>8__locals2, ref CS$<>8__locals3);
					ShapesMeshGen.<GenPolylineMesh>g__SetUv0|1_0(num21, 1f, (float)CS$<>8__locals2.i, ref CS$<>8__locals1, ref CS$<>8__locals2, ref CS$<>8__locals3);
				}
				if (!flag3 || closed)
				{
					if (flag)
					{
						int num22 = num20;
						int num23 = num11;
						int num24 = num13;
						int num25 = (flag3 ? 0 : (num22 + num2));
						int num26 = num25 + 1;
						int num27 = num25 + 3;
						ShapesMeshGen.<GenPolylineMesh>g__AddQuad|1_2(num22, num23, num26, num25, ref CS$<>8__locals1);
						ShapesMeshGen.<GenPolylineMesh>g__AddQuad|1_2(num25, num27, num24, num22, ref CS$<>8__locals1);
						if (flag5)
						{
							array[num19++] = num14;
							array[num19++] = num15;
							array[num19++] = num16;
							if (!flag2)
							{
								array[num19++] = num16;
								array[num19++] = num17;
								array[num19++] = num14;
								array[num19++] = num14;
								array[num19++] = num17;
								array[num19++] = num18;
							}
						}
					}
					else
					{
						int num28 = num20;
						int num29 = num21;
						int num30 = (flag3 ? 0 : (num28 + num2));
						int num31 = num30 + 1;
						ShapesMeshGen.<GenPolylineMesh>g__AddQuad|1_2(num29, num28, num30, num31, ref CS$<>8__locals1);
					}
				}
				int i = CS$<>8__locals2.i;
				CS$<>8__locals2.i = i + 1;
			}
			mesh.vertices = array3;
			mesh.subMeshCount = 2;
			mesh.SetTriangles(CS$<>8__locals1.meshTriangles, 0);
			mesh.SetTriangles(array, 1);
			mesh.SetUVs(0, CS$<>8__locals1.meshUv0);
			mesh.SetUVs(1, CS$<>8__locals1.meshUv1Prevs);
			mesh.SetUVs(2, CS$<>8__locals1.meshUv2Nexts);
			if (useColors)
			{
				mesh.colors = array2;
			}
		}

		// Token: 0x06000A61 RID: 2657 RVA: 0x000260BC File Offset: 0x000242BC
		public static void GenPolygonMesh(Mesh mesh, List<Vector2> path, PolygonTriangulation triangulation)
		{
			ShapesMeshGen.generatingClockwisePolygon = ShapesMath.PolygonSignedArea(path) > 0f;
			float num = (ShapesMeshGen.generatingClockwisePolygon ? 1f : (-1f));
			mesh.Clear();
			int count = path.Count;
			if (count < 2)
			{
				return;
			}
			int num2 = count - 2;
			int[] array = new int[num2 * 3];
			if (triangulation == PolygonTriangulation.FastConvexOnly)
			{
				int num3 = 0;
				for (int i = 0; i < num2; i++)
				{
					array[num3++] = i + 2;
					array[num3++] = i + 1;
					array[num3++] = 0;
				}
			}
			else
			{
				List<ShapesMeshGen.EarClipPoint> list = new List<ShapesMeshGen.EarClipPoint>(count);
				for (int j = 0; j < count; j++)
				{
					list.Add(new ShapesMeshGen.EarClipPoint(j, new Vector2(path[j].x, path[j].y)));
				}
				for (int k = 0; k < count; k++)
				{
					ShapesMeshGen.EarClipPoint earClipPoint = list[k];
					earClipPoint.prev = list[(k + count - 1) % count];
					earClipPoint.next = list[(k + 1) % count];
				}
				int num4 = 0;
				int num5 = 1000000;
				int count2;
				while ((count2 = list.Count) >= 3 && num5-- > 0)
				{
					if (count2 == 3)
					{
						array[num4++] = list[2].vertIndex;
						array[num4++] = list[1].vertIndex;
						array[num4++] = list[0].vertIndex;
						break;
					}
					bool flag = false;
					for (int l = 0; l < count2; l++)
					{
						ShapesMeshGen.EarClipPoint earClipPoint2 = list[l];
						if (earClipPoint2.ReflexState == ShapesMeshGen.ReflexState.Convex)
						{
							bool flag2 = true;
							int num6 = (l + count2 - 1) % count2;
							int num7 = (l + 1) % count2;
							for (int m = 0; m < count2; m++)
							{
								if (m != l && m != num6 && m != num7 && list[m].ReflexState == ShapesMeshGen.ReflexState.Reflex && ShapesMath.PointInsideTriangle(earClipPoint2.next.pt, earClipPoint2.pt, earClipPoint2.prev.pt, list[m].pt, 0f, num * -0.0001f, 0f))
								{
									flag2 = false;
									break;
								}
							}
							if (flag2)
							{
								array[num4++] = earClipPoint2.next.vertIndex;
								array[num4++] = earClipPoint2.vertIndex;
								array[num4++] = earClipPoint2.prev.vertIndex;
								earClipPoint2.next.MarkReflexUnknown();
								earClipPoint2.prev.MarkReflexUnknown();
								ShapesMeshGen.EarClipPoint next = earClipPoint2.next;
								ShapesMeshGen.EarClipPoint prev = earClipPoint2.prev;
								ShapesMeshGen.EarClipPoint prev2 = earClipPoint2.prev;
								ShapesMeshGen.EarClipPoint next2 = earClipPoint2.next;
								next.prev = prev2;
								prev.next = next2;
								list.RemoveAt(l);
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						Debug.LogError("Invalid polygon triangulation - no convex edges found. Your polygon is likely self-intersecting");
						break;
					}
				}
				if (num5 < 1)
				{
					Debug.LogError("Polygon triangulation failed, please report a bug (Shapes/Report Bug) with this exact case included");
				}
			}
			List<Vector3> list2 = new List<Vector3>(count);
			for (int n = 0; n < count; n++)
			{
				list2.Add(path[n]);
			}
			mesh.SetVertices(list2);
			mesh.subMeshCount = 1;
			mesh.SetTriangles(array, 0);
		}

		// Token: 0x06000A62 RID: 2658 RVA: 0x0002640A File Offset: 0x0002460A
		public static void CreateDisc(Mesh mesh, int segmentsPerFullTurn, float radius)
		{
			ShapesMeshGen.GenerateDiscMesh(mesh, segmentsPerFullTurn, false, false, radius, 0f, 0f, 0f);
		}

		// Token: 0x06000A63 RID: 2659 RVA: 0x00026425 File Offset: 0x00024625
		public static void CreateCircleSector(Mesh mesh, int segmentsPerFullTurn, float radius, float angRadiansStart, float angRadiansEnd)
		{
			ShapesMeshGen.GenerateDiscMesh(mesh, segmentsPerFullTurn, true, false, radius, 0f, angRadiansStart, angRadiansEnd);
		}

		// Token: 0x06000A64 RID: 2660 RVA: 0x00026439 File Offset: 0x00024639
		public static void CreateAnnulus(Mesh mesh, int segmentsPerFullTurn, float radius, float radiusInner)
		{
			ShapesMeshGen.GenerateDiscMesh(mesh, segmentsPerFullTurn, true, false, radius, radiusInner, 0f, 0f);
		}

		// Token: 0x06000A65 RID: 2661 RVA: 0x00026450 File Offset: 0x00024650
		public static void CreateAnnulusSector(Mesh mesh, int segmentsPerFullTurn, float radius, float radiusInner, float angRadiansStart, float angRadiansEnd)
		{
			ShapesMeshGen.GenerateDiscMesh(mesh, segmentsPerFullTurn, true, false, radius, radiusInner, angRadiansStart, angRadiansEnd);
		}

		// Token: 0x06000A66 RID: 2662 RVA: 0x00026464 File Offset: 0x00024664
		private static void GenerateDiscMesh(Mesh mesh, int segmentsPerFullTurn, bool hasSector, bool hasInnerRadius, float radius, float radiusInner, float angRadiansStart, float angRadiansEnd)
		{
			float num = (hasSector ? angRadiansStart : 0f);
			float num2 = (hasSector ? angRadiansEnd : 6.2831855f);
			float num3 = Mathf.Abs(num2 - num) / 6.2831855f;
			int num4 = Mathf.Max(1, Mathf.RoundToInt(num3 * (float)segmentsPerFullTurn));
			float num5 = Mathf.Max(radius, radiusInner);
			float num6 = Mathf.Cos(0.5f * Mathf.Abs(num2 - num) / (float)num4) * num5;
			float num7 = num5 * 2f - num6;
			float num8 = (hasInnerRadius ? Mathf.Min(radius, radiusInner) : 0f);
			int num9 = num4 * 2 * 2;
			int num10 = (num4 + 1) * 2;
			ShapesMeshGen.<>c__DisplayClass10_0 CS$<>8__locals1;
			CS$<>8__locals1.triIndices = new int[num9 * 3];
			Vector3[] array = new Vector3[num10];
			Vector3[] array2 = new Vector3[num10];
			for (int i = 0; i < num4 + 1; i++)
			{
				float num11 = (float)i / (float)num4;
				Vector2 vector = ShapesMath.AngToDir(Mathf.Lerp(num, num2, num11));
				int num12 = i * 2;
				int num13 = num12 + 1;
				array[num12] = vector * num7;
				array[num13] = vector * num8;
				array2[num12] = Vector3.forward;
				array2[num13] = Vector3.forward;
			}
			CS$<>8__locals1.tri = 0;
			for (int j = 0; j < num4; j++)
			{
				int num14 = j * 2;
				int num15 = num14 + 1;
				int num16 = num14 + 2;
				int num17 = num14 + 3;
				ShapesMeshGen.<GenerateDiscMesh>g__DblTri|10_0(num14, num17, num16, ref CS$<>8__locals1);
				ShapesMeshGen.<GenerateDiscMesh>g__DblTri|10_0(num14, num15, num17, ref CS$<>8__locals1);
			}
			mesh.vertices = array;
			mesh.normals = array2;
			mesh.triangles = CS$<>8__locals1.triIndices;
			mesh.RecalculateBounds();
		}

		// Token: 0x06000A67 RID: 2663 RVA: 0x00026608 File Offset: 0x00024808
		[CompilerGenerated]
		internal static void <GenPolylineMesh>g__SetUv0|1_0(int id, float x, float y, ref ShapesMeshGen.<>c__DisplayClass1_0 A_3, ref ShapesMeshGen.<>c__DisplayClass1_1 A_4, ref ShapesMeshGen.<>c__DisplayClass1_2 A_5)
		{
			A_3.meshUv0[id] = new Vector4(x, y, A_5.uvEndpointValue, A_3.path[A_4.i].thickness);
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x0002663B File Offset: 0x0002483B
		[CompilerGenerated]
		internal static void <GenPolylineMesh>g__SetPrevNext|1_1(int atIndex, ref ShapesMeshGen.<>c__DisplayClass1_0 A_1, ref ShapesMeshGen.<>c__DisplayClass1_2 A_2)
		{
			A_1.meshUv1Prevs[atIndex] = A_2.prevPos;
			A_1.meshUv2Nexts[atIndex] = A_2.nextPos;
		}

		// Token: 0x06000A69 RID: 2665 RVA: 0x00026664 File Offset: 0x00024864
		[CompilerGenerated]
		internal static void <GenPolylineMesh>g__AddQuad|1_2(int a, int b, int c, int d, ref ShapesMeshGen.<>c__DisplayClass1_0 A_4)
		{
			int[] meshTriangles = A_4.meshTriangles;
			int num = A_4.triId;
			A_4.triId = num + 1;
			meshTriangles[num] = a;
			int[] meshTriangles2 = A_4.meshTriangles;
			num = A_4.triId;
			A_4.triId = num + 1;
			meshTriangles2[num] = b;
			int[] meshTriangles3 = A_4.meshTriangles;
			num = A_4.triId;
			A_4.triId = num + 1;
			meshTriangles3[num] = c;
			int[] meshTriangles4 = A_4.meshTriangles;
			num = A_4.triId;
			A_4.triId = num + 1;
			meshTriangles4[num] = c;
			int[] meshTriangles5 = A_4.meshTriangles;
			num = A_4.triId;
			A_4.triId = num + 1;
			meshTriangles5[num] = d;
			int[] meshTriangles6 = A_4.meshTriangles;
			num = A_4.triId;
			A_4.triId = num + 1;
			meshTriangles6[num] = a;
		}

		// Token: 0x06000A6A RID: 2666 RVA: 0x0002671C File Offset: 0x0002491C
		[CompilerGenerated]
		internal static void <GenerateDiscMesh>g__DblTri|10_0(int a, int b, int c, ref ShapesMeshGen.<>c__DisplayClass10_0 A_3)
		{
			int[] triIndices = A_3.triIndices;
			int num = A_3.tri;
			A_3.tri = num + 1;
			triIndices[num] = a;
			int[] triIndices2 = A_3.triIndices;
			num = A_3.tri;
			A_3.tri = num + 1;
			triIndices2[num] = b;
			int[] triIndices3 = A_3.triIndices;
			num = A_3.tri;
			A_3.tri = num + 1;
			triIndices3[num] = c;
			int[] triIndices4 = A_3.triIndices;
			num = A_3.tri;
			A_3.tri = num + 1;
			triIndices4[num] = c;
			int[] triIndices5 = A_3.triIndices;
			num = A_3.tri;
			A_3.tri = num + 1;
			triIndices5[num] = b;
			int[] triIndices6 = A_3.triIndices;
			num = A_3.tri;
			A_3.tri = num + 1;
			triIndices6[num] = a;
		}

		// Token: 0x0400023D RID: 573
		private static bool generatingClockwisePolygon;

		// Token: 0x02000085 RID: 133
		private enum ReflexState
		{
			// Token: 0x040002E7 RID: 743
			Unknown,
			// Token: 0x040002E8 RID: 744
			Reflex,
			// Token: 0x040002E9 RID: 745
			Convex
		}

		// Token: 0x02000086 RID: 134
		private class EarClipPoint
		{
			// Token: 0x06000ACF RID: 2767 RVA: 0x0002746F File Offset: 0x0002566F
			public EarClipPoint(int vertIndex, Vector2 pt)
			{
				this.vertIndex = vertIndex;
				this.pt = pt;
			}

			// Token: 0x06000AD0 RID: 2768 RVA: 0x00027485 File Offset: 0x00025685
			public void MarkReflexUnknown()
			{
				this.reflex = ShapesMeshGen.ReflexState.Unknown;
			}

			// Token: 0x17000149 RID: 329
			// (get) Token: 0x06000AD1 RID: 2769 RVA: 0x00027490 File Offset: 0x00025690
			public ShapesMeshGen.ReflexState ReflexState
			{
				get
				{
					if (this.reflex == ShapesMeshGen.ReflexState.Unknown)
					{
						Vector2 vector = this.next.pt - this.pt;
						Vector2 vector2 = this.pt - this.prev.pt;
						if (ShapesMeshGen.generatingClockwisePolygon)
						{
							this.reflex = ((ShapesMath.Determinant(vector2, vector) >= -0.001f) ? ShapesMeshGen.ReflexState.Reflex : ShapesMeshGen.ReflexState.Convex);
						}
						else
						{
							this.reflex = ((ShapesMath.Determinant(vector, vector2) >= -0.001f) ? ShapesMeshGen.ReflexState.Reflex : ShapesMeshGen.ReflexState.Convex);
						}
					}
					return this.reflex;
				}
			}

			// Token: 0x040002EA RID: 746
			public int vertIndex;

			// Token: 0x040002EB RID: 747
			public Vector2 pt;

			// Token: 0x040002EC RID: 748
			private ShapesMeshGen.ReflexState reflex;

			// Token: 0x040002ED RID: 749
			public ShapesMeshGen.EarClipPoint prev;

			// Token: 0x040002EE RID: 750
			public ShapesMeshGen.EarClipPoint next;
		}
	}
}
