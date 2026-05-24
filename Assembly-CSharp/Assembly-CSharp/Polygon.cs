using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityX.Geometry;

// Token: 0x020001ED RID: 493
[Serializable]
public class Polygon
{
	// Token: 0x17000412 RID: 1042
	// (get) Token: 0x0600118D RID: 4493 RVA: 0x000812BC File Offset: 0x0007F4BC
	// (set) Token: 0x0600118E RID: 4494 RVA: 0x000812C4 File Offset: 0x0007F4C4
	public Vector2[] vertices
	{
		get
		{
			return this._vertices;
		}
		set
		{
			this._vertices = value;
		}
	}

	// Token: 0x0600118F RID: 4495 RVA: 0x000812CD File Offset: 0x0007F4CD
	public void CopyVertices(IList<Vector2> newVertices)
	{
		if (this._vertices == null || this._vertices.Length != newVertices.Count)
		{
			this._vertices = newVertices.ToArray<Vector2>();
			return;
		}
		newVertices.CopyTo(this._vertices, 0);
	}

	// Token: 0x17000413 RID: 1043
	// (get) Token: 0x06001190 RID: 4496 RVA: 0x00081304 File Offset: 0x0007F504
	public Vector2 center
	{
		get
		{
			if (this._vertices == null || this._vertices.Length == 0)
			{
				return Vector2.zero;
			}
			Vector2 vector = Vector2.zero;
			for (int i = 0; i < this._vertices.Length; i++)
			{
				vector += this._vertices[i];
			}
			vector.x /= (float)this._vertices.Length;
			vector.y /= (float)this._vertices.Length;
			return vector;
		}
	}

	// Token: 0x17000414 RID: 1044
	// (get) Token: 0x06001191 RID: 4497 RVA: 0x0008137D File Offset: 0x0007F57D
	public bool isClockwise
	{
		get
		{
			return this.GetIsClockwise();
		}
	}

	// Token: 0x06001192 RID: 4498 RVA: 0x00081385 File Offset: 0x0007F585
	public void FlipWindingOrder()
	{
		Array.Reverse<Vector2>(this._vertices);
	}

	// Token: 0x17000415 RID: 1045
	// (get) Token: 0x06001193 RID: 4499 RVA: 0x00081394 File Offset: 0x0007F594
	private Vector2 centroid
	{
		get
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			for (int i = 0; i < this._vertices.Length - 1; i++)
			{
				Vector2 vector = this._vertices[i];
				Vector2 vector2 = ((i == this._vertices.Length) ? this._vertices[0] : this._vertices[i + 1]);
				float num4 = vector.x * vector2.y - vector2.x * vector.y;
				num2 += (vector.x + vector2.x) * num4;
				num3 += (vector.y + vector2.y) * num4;
				num += num4 * 3f;
			}
			if (num == 0f)
			{
				return this._vertices[0];
			}
			return new Vector2(num2 / num, num3 / num);
		}
	}

	// Token: 0x06001194 RID: 4500 RVA: 0x0008147C File Offset: 0x0007F67C
	public Vector2 GetRandomPointInPolygon()
	{
		if (this.vertices.Length < 3 || this.GetArea() == 0f)
		{
			return Vector2.zero;
		}
		Polygon.tris.Clear();
		Triangulator.GenerateIndices(this.vertices, Polygon.tris);
		float num = 0f;
		for (int i = 0; i < Polygon.tris.Count; i += 3)
		{
			Triangle triangle = new Triangle(this.vertices[Polygon.tris[i]], this.vertices[Polygon.tris[i + 1]], this.vertices[Polygon.tris[i + 2]]);
			num += triangle.area;
		}
		if (num == 0f)
		{
			Debug.LogWarning("Polygon has no area!");
			return Vector2.zero;
		}
		float num2 = Random.Range(0f, num);
		Triangle triangle2 = default(Triangle);
		for (int j = 0; j < Polygon.tris.Count; j += 3)
		{
			triangle2 = new Triangle(this.vertices[Polygon.tris[j]], this.vertices[Polygon.tris[j + 1]], this.vertices[Polygon.tris[j + 2]]);
			float area = triangle2.area;
			if (num2 < area)
			{
				return triangle2.RandomPoint();
			}
			num2 -= area;
		}
		return triangle2.RandomPoint();
	}

	// Token: 0x06001195 RID: 4501 RVA: 0x000815E9 File Offset: 0x0007F7E9
	private Polygon.QuadCell MakeQuadCell(float x, float y, float halfHeight)
	{
		return this.MakeQuadCell(new Vector2(x, y), halfHeight);
	}

	// Token: 0x06001196 RID: 4502 RVA: 0x000815FC File Offset: 0x0007F7FC
	private Polygon.QuadCell MakeQuadCell(Vector2 centre, float halfHeight)
	{
		Polygon.QuadCell quadCell = default(Polygon.QuadCell);
		quadCell.centre = centre;
		quadCell.halfSize = halfHeight;
		quadCell.distanceFromPoly = this.MinSignedDistanceFromPointToPolygon(quadCell.centre);
		quadCell.maxDistanceFromPoly = quadCell.distanceFromPoly + quadCell.halfSize * Mathf.Sqrt(2f);
		return quadCell;
	}

	// Token: 0x06001197 RID: 4503 RVA: 0x00081654 File Offset: 0x0007F854
	public float MinSignedDistanceFromPointToPolygon(Vector2 point)
	{
		float num = float.MaxValue;
		for (int i = 0; i < this._vertices.Length; i++)
		{
			num = Mathf.Min(num, this.PointToLineSegmentSquaredDistance(point, this._vertices[i], (i == this._vertices.Length - 1) ? this._vertices[0] : this._vertices[i + 1]));
		}
		return (float)(this.ContainsPoint(point) ? 1 : (-1)) * Mathf.Sqrt(num);
	}

	// Token: 0x06001198 RID: 4504 RVA: 0x000816D4 File Offset: 0x0007F8D4
	private float PointToLineSegmentSquaredDistance(Vector2 point, Vector2 a, Vector2 b)
	{
		float num = a.x;
		float num2 = a.y;
		float num3 = b.x - a.x;
		float num4 = b.y - a.y;
		if (num3 != 0f || num4 != 0f)
		{
			float num5 = ((point.x - a.x) * num3 + (point.y - a.y) * num4) / (num3 * num3 + num4 * num4);
			if (num5 > 1f)
			{
				num = b.x;
				num2 = b.y;
			}
			else if (num5 > 0f)
			{
				num += num3 * num5;
				num2 += num4 * num5;
			}
		}
		num3 = point.x - num;
		num4 = point.y - num2;
		return num3 * num3 + num4 * num4;
	}

	// Token: 0x17000416 RID: 1046
	// (get) Token: 0x06001199 RID: 4505 RVA: 0x0008178D File Offset: 0x0007F98D
	public Vector2 poleOfInaccessibility
	{
		get
		{
			if (this._poleOfInaccessibility.magnitude == 0f)
			{
				this._poleOfInaccessibility = this.computePoleOfInaccesibility();
			}
			return this._poleOfInaccessibility;
		}
	}

	// Token: 0x0600119A RID: 4506 RVA: 0x000817B4 File Offset: 0x0007F9B4
	private Vector2 computePoleOfInaccesibility()
	{
		if (this._vertices.Length == 0)
		{
			return new Vector2(0f, 0f);
		}
		float num = float.MaxValue;
		float num2 = float.MaxValue;
		float num3 = float.MaxValue;
		float num4 = float.MaxValue;
		for (int i = 0; i < this._vertices.Length; i++)
		{
			Vector2 vector = this._vertices[i];
			if (i == 0 || vector[0] < num)
			{
				num = vector[0];
			}
			if (i == 0 || vector[1] < num2)
			{
				num2 = vector[1];
			}
			if (i == 0 || vector[0] > num3)
			{
				num3 = vector[0];
			}
			if (i == 0 || vector[1] > num4)
			{
				num4 = vector[1];
			}
		}
		float num5 = num3 - num;
		float num6 = num4 - num2;
		float num7 = Mathf.Min(num5, num6);
		float num8 = num7 / 2f;
		float num9 = Mathf.Min(1f, num8 / 50f);
		if (num7 == 0f)
		{
			return new Vector2(num + num5 / 2f, num2 + num6 / 2f);
		}
		List<Polygon.QuadCell> list = new List<Polygon.QuadCell>();
		for (float num10 = num; num10 < num3; num10 += num7)
		{
			for (float num11 = num2; num11 < num4; num11 += num7)
			{
				list.Add(this.MakeQuadCell(num10 + num8, num11 + num8, num8));
			}
		}
		Polygon.QuadCell quadCell = this.MakeQuadCell(this.centroid, 0f);
		Polygon.QuadCell quadCell2 = this.MakeQuadCell(num + num5 / 2f, num2 + num6 / 2f, 0f);
		if (quadCell2.distanceFromPoly > quadCell.distanceFromPoly)
		{
			quadCell = quadCell2;
		}
		int num12 = list.Count;
		while (list.Count > 0 && num12 < 1000)
		{
			int num13 = -1;
			for (int j = 0; j < list.Count; j++)
			{
				if (num13 == -1 || list[j].maxDistanceFromPoly > list[num13].maxDistanceFromPoly)
				{
					num13 = j;
				}
			}
			Polygon.QuadCell quadCell3 = list[num13];
			list.RemoveAt(num13);
			if (quadCell3.distanceFromPoly > quadCell.distanceFromPoly)
			{
				quadCell = quadCell3;
			}
			if (quadCell3.maxDistanceFromPoly - quadCell.distanceFromPoly > num9)
			{
				num8 = quadCell3.halfSize / 2f;
				list.Add(this.MakeQuadCell(quadCell3.centre.x - num8, quadCell3.centre.y - num8, num8));
				list.Add(this.MakeQuadCell(quadCell3.centre.x + num8, quadCell3.centre.y - num8, num8));
				list.Add(this.MakeQuadCell(quadCell3.centre.x - num8, quadCell3.centre.y + num8, num8));
				list.Add(this.MakeQuadCell(quadCell3.centre.x + num8, quadCell3.centre.y + num8, num8));
				num12 += 4;
			}
		}
		return quadCell.centre;
	}

	// Token: 0x17000417 RID: 1047
	// (get) Token: 0x0600119B RID: 4507 RVA: 0x00081ACD File Offset: 0x0007FCCD
	private bool verticesIsEmpty
	{
		get
		{
			return this._vertices.Length == 0;
		}
	}

	// Token: 0x17000418 RID: 1048
	// (get) Token: 0x0600119C RID: 4508 RVA: 0x00081AD9 File Offset: 0x0007FCD9
	public int VertCount
	{
		get
		{
			return this._vertices.Length;
		}
	}

	// Token: 0x17000419 RID: 1049
	// (get) Token: 0x0600119D RID: 4509 RVA: 0x00081AE3 File Offset: 0x0007FCE3
	public bool connected
	{
		get
		{
			return !this.verticesIsEmpty && this._vertices[0] == this._vertices[this._vertices.Length - 1];
		}
	}

	// Token: 0x0600119E RID: 4510 RVA: 0x00081B15 File Offset: 0x0007FD15
	public bool IsValid()
	{
		if (this.verticesIsEmpty)
		{
			return false;
		}
		if (this.connected)
		{
			return this._vertices.Length >= 3;
		}
		return this._vertices.Length >= 2;
	}

	// Token: 0x1700041A RID: 1050
	// (get) Token: 0x0600119F RID: 4511 RVA: 0x00081B48 File Offset: 0x0007FD48
	public static Polygon square
	{
		get
		{
			return new Polygon(new Vector2[]
			{
				new Vector2(-0.5f, -0.5f),
				new Vector2(0.5f, -0.5f),
				new Vector2(0.5f, 0.5f),
				new Vector2(-0.5f, 0.5f)
			});
		}
	}

	// Token: 0x060011A0 RID: 4512 RVA: 0x00081BB8 File Offset: 0x0007FDB8
	public Polygon(params Vector2[] _vertices)
	{
		this.vertices = _vertices;
	}

	// Token: 0x060011A1 RID: 4513 RVA: 0x00081BDC File Offset: 0x0007FDDC
	public Polygon(Polygon _polygon)
	{
		this.vertices = (Vector2[])_polygon.vertices.Clone();
	}

	// Token: 0x060011A2 RID: 4514 RVA: 0x00081C10 File Offset: 0x0007FE10
	public Rect GetRect()
	{
		Rect rect = default(Rect);
		if (!this.IsValid())
		{
			return rect;
		}
		rect = new Rect(this._vertices[0].x, this._vertices[0].y, 0f, 0f);
		for (int i = 1; i < this._vertices.Length; i++)
		{
			Vector2 vector = this._vertices[i];
			rect.xMin = Mathf.Min(rect.xMin, vector.x);
			rect.xMax = Mathf.Max(rect.xMax, vector.x);
			rect.yMin = Mathf.Min(rect.yMin, vector.y);
			rect.yMax = Mathf.Max(rect.yMax, vector.y);
		}
		return rect;
	}

	// Token: 0x060011A3 RID: 4515 RVA: 0x00081CE8 File Offset: 0x0007FEE8
	public void Move(Vector2 vector)
	{
		if (vector == Vector2.zero)
		{
			return;
		}
		for (int i = 0; i < this._vertices.Length; i++)
		{
			this._vertices[i] = this._vertices[i] + vector;
		}
	}

	// Token: 0x060011A4 RID: 4516 RVA: 0x00081D34 File Offset: 0x0007FF34
	public static Polygon Scale(Polygon _polygonDefinition, Polygon _scaleModifier)
	{
		Polygon polygon = new Polygon(_polygonDefinition);
		if (polygon.vertices.Length > _scaleModifier.vertices.Length)
		{
			Debug.Log(string.Concat(new string[]
			{
				"Cannot Scale PolygonDefinition because the input modifier does not have enough vertices. It has ",
				_scaleModifier.vertices.Length.ToString(),
				". It requires at least ",
				_polygonDefinition.vertices.Length.ToString(),
				"."
			}));
		}
		for (int i = 0; i < _polygonDefinition.vertices.Length; i++)
		{
			Vector2.Scale(polygon.vertices[i], _scaleModifier.vertices[i]);
		}
		return polygon;
	}

	// Token: 0x060011A5 RID: 4517 RVA: 0x00081DDC File Offset: 0x0007FFDC
	public static Polygon Scale(Polygon _polygonDefinition, Vector2 _scaleModifier)
	{
		if (_scaleModifier == Vector2.one)
		{
			return _polygonDefinition;
		}
		Polygon polygon = new Polygon(_polygonDefinition);
		for (int i = 0; i < _polygonDefinition.vertices.Length; i++)
		{
			Vector2.Scale(polygon.vertices[i], _scaleModifier);
		}
		return polygon;
	}

	// Token: 0x060011A6 RID: 4518 RVA: 0x00081E28 File Offset: 0x00080028
	public static Polygon Scale(Polygon _polygonDefinition, float _scaleModifier)
	{
		if (_scaleModifier == 1f)
		{
			return _polygonDefinition;
		}
		Polygon polygon = new Polygon(_polygonDefinition);
		for (int i = 0; i < _polygonDefinition.vertices.Length; i++)
		{
			polygon.vertices[i] = _polygonDefinition.vertices[i] * _scaleModifier;
		}
		return polygon;
	}

	// Token: 0x060011A7 RID: 4519 RVA: 0x00081E78 File Offset: 0x00080078
	public void Expand(float _expansion)
	{
		if (_expansion == 0f)
		{
			return;
		}
		for (int i = 0; i < this.vertices.Length; i++)
		{
			this.vertices[i] = this.vertices[i].normalized * (this.vertices[i].magnitude + _expansion);
		}
	}

	// Token: 0x060011A8 RID: 4520 RVA: 0x00081ED8 File Offset: 0x000800D8
	public float GetTotalLength()
	{
		float num = 0f;
		foreach (global::Line line in this.GetLines())
		{
			num += line.length;
		}
		return num;
	}

	// Token: 0x060011A9 RID: 4521 RVA: 0x00081F30 File Offset: 0x00080130
	public Vector2 GetRegularEdgePosition(float normalizedEdgeLength)
	{
		normalizedEdgeLength %= 1f;
		float num = this.GetTotalLength() * normalizedEdgeLength;
		float num2 = 0f;
		foreach (global::Line line in this.GetLines())
		{
			float num3 = num2 + line.length;
			if (num3 > num)
			{
				float num4 = Mathf.InverseLerp(num2, num3, num);
				return Vector2.Lerp(line.start, line.end, num4);
			}
			num2 = num3;
		}
		return this.vertices.Last<Vector2>();
	}

	// Token: 0x060011AA RID: 4522 RVA: 0x00081FD4 File Offset: 0x000801D4
	public Vector2 GetVertex(int edgeIndex)
	{
		return this.vertices[(int)this.GetRepeatingVertexIndex((float)edgeIndex)];
	}

	// Token: 0x060011AB RID: 4523 RVA: 0x00081FEC File Offset: 0x000801EC
	public global::Line GetEdge(float edgeDistance)
	{
		int num = Mathf.FloorToInt(edgeDistance);
		int num2 = num + 1;
		return this.GetEdge(num, num2);
	}

	// Token: 0x060011AC RID: 4524 RVA: 0x0008200C File Offset: 0x0008020C
	public global::Line GetEdge(int i = 0, int j = 1)
	{
		return new global::Line(this.GetVertex(i), this.GetVertex(j));
	}

	// Token: 0x060011AD RID: 4525 RVA: 0x00082021 File Offset: 0x00080221
	public float GetEdgeLength(int i = 0, int j = 1)
	{
		return Vector2.Distance(this.GetVertex(i), this.GetVertex(j));
	}

	// Token: 0x060011AE RID: 4526 RVA: 0x00082038 File Offset: 0x00080238
	public Vector2 GetEdgeTangentAtEdgeIndex(int edgeIndex)
	{
		return (this.GetVertex(edgeIndex + 1) - this.GetVertex(edgeIndex)).normalized;
	}

	// Token: 0x060011AF RID: 4527 RVA: 0x00082062 File Offset: 0x00080262
	public Vector2 GetEdgeNormalAtEdgeIndex(int edgeIndex)
	{
		return this.GetEdgeNormalAtEdgeIndex(edgeIndex, this.GetIsClockwise());
	}

	// Token: 0x060011B0 RID: 4528 RVA: 0x00082074 File Offset: 0x00080274
	private Vector2 GetEdgeNormalAtEdgeIndex(int edgeIndex, bool isClockwise)
	{
		Vector2 edgeTangentAtEdgeIndex = this.GetEdgeTangentAtEdgeIndex(edgeIndex);
		Vector2 vector = new Vector2(-edgeTangentAtEdgeIndex.y, edgeTangentAtEdgeIndex.x);
		if (!isClockwise)
		{
			vector *= -1f;
		}
		return vector;
	}

	// Token: 0x060011B1 RID: 4529 RVA: 0x000820B0 File Offset: 0x000802B0
	public static Vector2 GetEdgeNormalAtEdgeIndex(Vector2 tangent, bool isClockwise)
	{
		Vector2 vector = new Vector2(-tangent.y, tangent.x);
		if (!isClockwise)
		{
			vector *= -1f;
		}
		return vector;
	}

	// Token: 0x060011B2 RID: 4530 RVA: 0x000820E4 File Offset: 0x000802E4
	public Vector2 GetVertexNormal(int index)
	{
		bool isClockwise = this.GetIsClockwise();
		Vector2 edgeNormalAtEdgeIndex = this.GetEdgeNormalAtEdgeIndex(index - 1, isClockwise);
		Vector2 edgeNormalAtEdgeIndex2 = this.GetEdgeNormalAtEdgeIndex(index, isClockwise);
		return Vector2.Lerp(edgeNormalAtEdgeIndex, edgeNormalAtEdgeIndex2, 0.5f).normalized;
	}

	// Token: 0x060011B3 RID: 4531 RVA: 0x0008211E File Offset: 0x0008031E
	public bool GetIsClockwise()
	{
		return Polygon.GetIsClockwise(this._vertices);
	}

	// Token: 0x060011B4 RID: 4532 RVA: 0x0008212C File Offset: 0x0008032C
	public static bool GetIsClockwise(Vector2[] _vertices)
	{
		float num = 0f;
		Vector2 vector = _vertices[_vertices.Length - 1];
		Vector2 vector2 = _vertices[0];
		num += (vector2.x - vector.x) * (vector2.y + vector.y);
		for (int i = 1; i < _vertices.Length; i++)
		{
			vector = vector2;
			vector2 = _vertices[i];
			num += (vector2.x - vector.x) * (vector2.y + vector.y);
		}
		return num >= 0f;
	}

	// Token: 0x060011B5 RID: 4533 RVA: 0x000821B2 File Offset: 0x000803B2
	public IEnumerable<global::Line> GetLines()
	{
		int num;
		for (int i = 0; i < this._vertices.Length - 1; i = num + 1)
		{
			yield return new global::Line(this._vertices[i], this._vertices[i + 1]);
			num = i;
		}
		yield return new global::Line(this._vertices[this._vertices.Length - 1], this._vertices[0]);
		yield break;
	}

	// Token: 0x060011B6 RID: 4534 RVA: 0x000821C2 File Offset: 0x000803C2
	private static Vector2 PointInPolyFromIndex(Polygon poly, int index)
	{
		return poly.vertices[index % poly.vertices.Length];
	}

	// Token: 0x060011B7 RID: 4535 RVA: 0x000821D9 File Offset: 0x000803D9
	private static int GetIndexInPolyAtPoint(Polygon poly, Vector2 point)
	{
		return Polygon.GetIndexInPolyLyingOnLineBetween(poly, new Vector2(point.x - 0.001f, point.y), new Vector2(point.x + 0.001f, point.y));
	}

	// Token: 0x060011B8 RID: 4536 RVA: 0x00082210 File Offset: 0x00080410
	private static int GetIndexInPolyLyingOnLineBetween(Polygon poly, Vector2 pointA, Vector2 pointB)
	{
		global::Line line = new global::Line(pointA, pointB);
		int num = -1;
		for (int i = 0; i < poly.vertices.Length; i++)
		{
			Vector2 vector = poly.vertices[i];
			if (Vector2.Distance(vector, pointA) > 1.0000001E-06f && line.GetClosestDistanceFromLine(vector) < 0.001f && (num == -1 || (num > -1 && Vector2.Distance(vector, pointA) < Vector2.Distance(poly.vertices[num], pointA))))
			{
				num = i;
			}
		}
		return num;
	}

	// Token: 0x060011B9 RID: 4537 RVA: 0x0008228C File Offset: 0x0008048C
	private Polygon AddVert(int indexToAddAfter, Vector2 pointToAdd)
	{
		List<Vector2> list = new List<Vector2>(this.vertices);
		if (indexToAddAfter == this.vertices.Length)
		{
			list.Add(pointToAdd);
		}
		else
		{
			list.Insert(indexToAddAfter + 1, pointToAdd);
		}
		return new Polygon(list.ToArray());
	}

	// Token: 0x060011BA RID: 4538 RVA: 0x000822CE File Offset: 0x000804CE
	private static bool PointsOverlap(Vector2 pointA, Vector2 pointB)
	{
		return Vector2.Distance(pointA, pointB) < 1.0000001E-06f;
	}

	// Token: 0x060011BB RID: 4539 RVA: 0x000822E0 File Offset: 0x000804E0
	private static void AddPointUniquelyToList(Vector2 thisPoint, List<Vector2> listOfPoints)
	{
		for (int i = 0; i < listOfPoints.Count; i++)
		{
			if (Polygon.PointsOverlap(listOfPoints[i], thisPoint))
			{
				return;
			}
		}
		listOfPoints.Add(thisPoint);
	}

	// Token: 0x060011BC RID: 4540 RVA: 0x00082315 File Offset: 0x00080515
	public bool IntersectsWithPolygon(Polygon otherPolygon)
	{
		return Polygon.AddIntersectionPointsOfPoly(this, otherPolygon).indexList.Count > 0;
	}

	// Token: 0x060011BD RID: 4541 RVA: 0x0008232B File Offset: 0x0008052B
	public bool WhollyContainsOtherPolygon(Polygon otherPolygon)
	{
		return !this.IntersectsWithPolygon(otherPolygon) && this.ContainsPoint(otherPolygon.vertices[0]);
	}

	// Token: 0x060011BE RID: 4542 RVA: 0x0008234C File Offset: 0x0008054C
	private static Polygon.PolygonWithSubsetOfPoints AddIntersectionPointsOfPoly(Polygon addToPoly, Polygon intersectingPoly)
	{
		Polygon.PolygonWithSubsetOfPoints polygonWithSubsetOfPoints = default(Polygon.PolygonWithSubsetOfPoints);
		polygonWithSubsetOfPoints.polygon = addToPoly;
		List<global::Line> list = intersectingPoly.GetLines().ToList<global::Line>();
		List<Vector2> list2 = new List<Vector2>();
		for (int i = addToPoly.vertices.Length - 1; i >= 0; i--)
		{
			Vector2 linePoint = Polygon.PointInPolyFromIndex(polygonWithSubsetOfPoints.polygon, i);
			global::Line line = new global::Line(linePoint, Polygon.PointInPolyFromIndex(polygonWithSubsetOfPoints.polygon, i + 1));
			List<Vector2> list3 = new List<Vector2>();
			for (int j = 0; j < list.Count; j++)
			{
				Vector2 vector;
				if (global::Line.LineIntersectionPoint(line, list[j], out vector, true, true))
				{
					Polygon.AddPointUniquelyToList(vector, list3);
				}
			}
			list3.Sort((Vector2 v1, Vector2 v2) => (v1 - linePoint).sqrMagnitude.CompareTo((v2 - linePoint).sqrMagnitude));
			for (int k = list3.Count - 1; k >= 0; k--)
			{
				Vector2 vector2 = list3[k];
				if (!polygonWithSubsetOfPoints.polygon.vertices.Contains(vector2))
				{
					polygonWithSubsetOfPoints.polygon = polygonWithSubsetOfPoints.polygon.AddVert(i, vector2);
				}
				Polygon.AddPointUniquelyToList(vector2, list2);
			}
		}
		polygonWithSubsetOfPoints.indexList = new List<int>();
		for (int l = 0; l < list2.Count; l++)
		{
			polygonWithSubsetOfPoints.indexList.Add(Polygon.GetIndexInPolyAtPoint(polygonWithSubsetOfPoints.polygon, list2[l]));
		}
		return polygonWithSubsetOfPoints;
	}

	// Token: 0x060011BF RID: 4543 RVA: 0x000824AE File Offset: 0x000806AE
	private int IndexOfIndexInOtherPoly(int index, Polygon sourcePoly)
	{
		return Polygon.GetIndexInPolyAtPoint(this, Polygon.PointInPolyFromIndex(sourcePoly, index));
	}

	// Token: 0x060011C0 RID: 4544 RVA: 0x000824BD File Offset: 0x000806BD
	private bool EncompassesPointOfIndexInOtherPoly(int index, Polygon sourcePoly)
	{
		return this.ContainsPoint(Polygon.PointInPolyFromIndex(sourcePoly, index));
	}

	// Token: 0x060011C1 RID: 4545 RVA: 0x000824CC File Offset: 0x000806CC
	public bool RayPolygonIntersection(Vector2 rayOrigin, Vector2 ray, out Polygon.PolygonRaycastHit hit)
	{
		bool flag = false;
		Vector2 normalized = ray.normalized;
		hit = default(Polygon.PolygonRaycastHit);
		hit.distance = float.MaxValue;
		hit.point = rayOrigin;
		float num = ray.magnitude;
		int num2 = this._vertices.Length - 1;
		for (int i = 0; i < this._vertices.Length; i++)
		{
			float num3;
			if (new global::Line(this._vertices[num2], this._vertices[i]).RayLineIntersect(rayOrigin, normalized, out num3) && num3 < num)
			{
				flag = true;
				num = (hit.distance = num3);
				hit.point = rayOrigin + normalized * hit.distance;
				Vector2 vector = this._vertices[num2] - this._vertices[i];
				hit.normal = new Vector2(-vector.y, vector.x).normalized;
			}
			num2 = i;
		}
		return flag;
	}

	// Token: 0x060011C2 RID: 4546 RVA: 0x000825CC File Offset: 0x000807CC
	public List<Polygon.PolygonRaycastHit> RayPolygonIntersections(Vector2 rayOrigin, Vector2 rayDirection)
	{
		List<Polygon.PolygonRaycastHit> list = new List<Polygon.PolygonRaycastHit>();
		this.RayPolygonIntersectionsNonAlloc(rayOrigin, rayDirection, ref list);
		return list;
	}

	// Token: 0x060011C3 RID: 4547 RVA: 0x000825EC File Offset: 0x000807EC
	public void RayPolygonIntersectionsNonAlloc(Vector2 rayOrigin, Vector2 rayDirection, ref List<Polygon.PolygonRaycastHit> hits)
	{
		if (hits == null)
		{
			hits = new List<Polygon.PolygonRaycastHit>();
		}
		hits.Clear();
		int num = this._vertices.Length - 1;
		for (int i = 0; i < this._vertices.Length; i++)
		{
			float num2;
			if (new global::Line(this._vertices[num], this._vertices[i]).RayLineIntersect(rayOrigin, rayDirection, out num2))
			{
				Polygon.PolygonRaycastHit polygonRaycastHit = default(Polygon.PolygonRaycastHit);
				polygonRaycastHit.distance = num2;
				polygonRaycastHit.point = rayOrigin + rayDirection * polygonRaycastHit.distance;
				hits.Add(polygonRaycastHit);
			}
			num = i;
		}
	}

	// Token: 0x1700041B RID: 1051
	public Vector2 this[int key]
	{
		get
		{
			return this._vertices[key];
		}
		set
		{
			this._vertices[key] = value;
		}
	}

	// Token: 0x060011C6 RID: 4550 RVA: 0x000826A6 File Offset: 0x000808A6
	public float GetRepeatingVertexIndex(float vertexIndex)
	{
		return Mathf.Repeat(vertexIndex, (float)this.VertCount);
	}

	// Token: 0x060011C7 RID: 4551 RVA: 0x000826B5 File Offset: 0x000808B5
	public Vector2 FindClosestVertex(Vector2 point)
	{
		return Polygon.Closest(point, this._vertices);
	}

	// Token: 0x060011C8 RID: 4552 RVA: 0x000826C3 File Offset: 0x000808C3
	public int FindClosestVertexIndex(Vector2 point)
	{
		return Polygon.ClosestIndex(point, this._vertices);
	}

	// Token: 0x060011C9 RID: 4553 RVA: 0x000826D4 File Offset: 0x000808D4
	public void FindClosestEdgeIndices(Vector2 point, ref int bestIndex1, ref int bestIndex2)
	{
		float num = float.PositiveInfinity;
		Vector2 zero = Vector2.zero;
		bestIndex1 = 0;
		bestIndex2 = 0;
		for (int i = 0; i < this._vertices.Length; i++)
		{
			Vector2 vector = Vector2.zero;
			if (i < this._vertices.Length - 1)
			{
				vector = global::Line.GetClosestPointOnLine(this._vertices[i], this._vertices[i + 1], point, true);
			}
			else
			{
				vector = global::Line.GetClosestPointOnLine(this._vertices[i], this._vertices[0], point, true);
			}
			float num2 = Polygon.SqrDistance(point, vector);
			if (num2 < num)
			{
				num = num2;
				bestIndex1 = i;
				bestIndex2 = ((i < this._vertices.Length - 1) ? (i + 1) : 0);
			}
		}
	}

	// Token: 0x060011CA RID: 4554 RVA: 0x00082789 File Offset: 0x00080989
	private static float SqrDistance(Vector2 a, Vector2 b)
	{
		return (a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y);
	}

	// Token: 0x060011CB RID: 4555 RVA: 0x000827C2 File Offset: 0x000809C2
	public Vector2 FindClosestPointInPolygon(Vector2 point)
	{
		if (this.ContainsPoint(point))
		{
			return point;
		}
		return this.FindClosestPointOnPolygon(point, true);
	}

	// Token: 0x060011CC RID: 4556 RVA: 0x000827D7 File Offset: 0x000809D7
	public Vector2 FindClosestPointOnPolygon(Vector2 point, bool closed = true)
	{
		return Polygon.FindClosestPointOnPolygon(this._vertices, point, closed);
	}

	// Token: 0x060011CD RID: 4557 RVA: 0x000827E8 File Offset: 0x000809E8
	public static Vector2 FindClosestPointOnPolygon(Vector2[] verts, Vector2 point, bool closed = true)
	{
		float num = float.PositiveInfinity;
		Vector2 vector = Vector2.zero;
		int i = 0;
		while (i < verts.Length)
		{
			Vector2 vector2;
			if (i < verts.Length - 1)
			{
				vector2 = global::Line.GetClosestPointOnLine(verts[i], verts[i + 1], point, true);
				goto IL_004B;
			}
			if (closed)
			{
				vector2 = global::Line.GetClosestPointOnLine(verts[i], verts[0], point, true);
				goto IL_004B;
			}
			IL_005E:
			i++;
			continue;
			IL_004B:
			float num2 = Polygon.SqrDistance(point, vector2);
			if (num2 < num)
			{
				num = num2;
				vector = vector2;
				goto IL_005E;
			}
			goto IL_005E;
		}
		return vector;
	}

	// Token: 0x060011CE RID: 4558 RVA: 0x0008285E File Offset: 0x00080A5E
	public bool ContainsPoint(Vector2 testPoint)
	{
		return Polygon.ContainsPoint(this._vertices, testPoint);
	}

	// Token: 0x060011CF RID: 4559 RVA: 0x0008286C File Offset: 0x00080A6C
	public static bool ContainsPoint(Vector2[] polyPoints, Vector2 testPoint)
	{
		bool flag = false;
		int num = polyPoints.Length - 1;
		for (int i = 0; i < polyPoints.Length; i++)
		{
			if (((polyPoints[i].y < testPoint.y && polyPoints[num].y >= testPoint.y) || (polyPoints[num].y < testPoint.y && polyPoints[i].y >= testPoint.y)) && polyPoints[i].x + (testPoint.y - polyPoints[i].y) / (polyPoints[num].y - polyPoints[i].y) * (polyPoints[num].x - polyPoints[i].x) < testPoint.x)
			{
				flag = !flag;
			}
			num = i;
		}
		return flag;
	}

	// Token: 0x060011D0 RID: 4560 RVA: 0x0008294C File Offset: 0x00080B4C
	public static bool ContainsPoint(List<Vector2> polyPoints, Vector2 testPoint)
	{
		bool flag = false;
		int num = polyPoints.Count - 1;
		for (int i = 0; i < polyPoints.Count; i++)
		{
			if (((polyPoints[i].y < testPoint.y && polyPoints[num].y >= testPoint.y) || (polyPoints[num].y < testPoint.y && polyPoints[i].y >= testPoint.y)) && polyPoints[i].x + (testPoint.y - polyPoints[i].y) / (polyPoints[num].y - polyPoints[i].y) * (polyPoints[num].x - polyPoints[i].x) < testPoint.x)
			{
				flag = !flag;
			}
			num = i;
		}
		return flag;
	}

	// Token: 0x060011D1 RID: 4561 RVA: 0x00082A2F File Offset: 0x00080C2F
	public float GetArea()
	{
		if (!this.IsValid())
		{
			return 0f;
		}
		return Triangulator.Area(this._vertices);
	}

	// Token: 0x060011D2 RID: 4562 RVA: 0x00082A4C File Offset: 0x00080C4C
	public void CopyFrom(Polygon src)
	{
		this.vertices = new Vector2[src.vertices.Length];
		for (int i = 0; i < this._vertices.Length; i++)
		{
			this._vertices[i] = src[i];
		}
	}

	// Token: 0x060011D3 RID: 4563 RVA: 0x00082A94 File Offset: 0x00080C94
	public override string ToString()
	{
		string text = string.Concat(new string[]
		{
			"Polygon: VertCount=",
			this.VertCount.ToString(),
			" Connected=",
			this.connected.ToString(),
			" Valid=",
			this.IsValid().ToString()
		});
		for (int i = 0; i < this._vertices.Length; i++)
		{
			string[] array = new string[5];
			array[0] = text;
			array[1] = "\n";
			array[2] = i.ToString();
			array[3] = ": ";
			int num = 4;
			Vector2 vector = this._vertices[i];
			array[num] = vector.ToString();
			text = string.Concat(array);
		}
		return text;
	}

	// Token: 0x060011D4 RID: 4564 RVA: 0x00082B54 File Offset: 0x00080D54
	private static int ClosestIndex(Vector2 v, IList<Vector2> values)
	{
		int num = 0;
		float num2 = Polygon.SqrDistance(v, values[num]);
		for (int i = 1; i < values.Count; i++)
		{
			float num3 = Polygon.SqrDistance(v, values[i]);
			if (num3 < num2)
			{
				num2 = num3;
				num = i;
			}
		}
		return num;
	}

	// Token: 0x060011D5 RID: 4565 RVA: 0x00082B99 File Offset: 0x00080D99
	public static Vector2 Closest(Vector2 v, IList<Vector2> values)
	{
		return values[Polygon.ClosestIndex(v, values)];
	}

	// Token: 0x060011D6 RID: 4566 RVA: 0x00082BA8 File Offset: 0x00080DA8
	public static int FurthestIndex(Vector2 v, IList<Vector2> values)
	{
		if (values.Count == 0)
		{
			Debug.LogError("Values is empty!");
			return -1;
		}
		int num = 0;
		float num2 = Polygon.SqrDistance(v, values[num]);
		for (int i = 1; i < values.Count; i++)
		{
			float num3 = Polygon.SqrDistance(v, values[i]);
			if (num3 > num2)
			{
				num2 = num3;
				num = i;
			}
		}
		return num;
	}

	// Token: 0x060011D7 RID: 4567 RVA: 0x00082C01 File Offset: 0x00080E01
	public static Vector2 Furthest(Vector2 v, IList<Vector2> values)
	{
		return values[Polygon.FurthestIndex(v, values)];
	}

	// Token: 0x060011D8 RID: 4568 RVA: 0x00082C10 File Offset: 0x00080E10
	public int PrevVertexIndex(int i)
	{
		return (i + this._vertices.Length - 1) % this._vertices.Length;
	}

	// Token: 0x060011D9 RID: 4569 RVA: 0x00082C27 File Offset: 0x00080E27
	public int NextVertexIndex(int i)
	{
		return (i + 1) % this._vertices.Length;
	}

	// Token: 0x060011DA RID: 4570 RVA: 0x00082C38 File Offset: 0x00080E38
	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		Polygon polygon = (Polygon)obj;
		return polygon != null && this.Equals(polygon);
	}

	// Token: 0x060011DB RID: 4571 RVA: 0x00082C5D File Offset: 0x00080E5D
	public bool Equals(Polygon p)
	{
		return p != null && this._vertices.Length == p._vertices.Length && this._vertices.SequenceEqual(p._vertices);
	}

	// Token: 0x060011DC RID: 4572 RVA: 0x00082C89 File Offset: 0x00080E89
	public override int GetHashCode()
	{
		return this._vertices.GetHashCode();
	}

	// Token: 0x060011DD RID: 4573 RVA: 0x00082C96 File Offset: 0x00080E96
	public static bool operator ==(Polygon left, Polygon right)
	{
		return left == right || (left != null && right != null && left.Equals(right));
	}

	// Token: 0x060011DE RID: 4574 RVA: 0x00082CAD File Offset: 0x00080EAD
	public static bool operator !=(Polygon left, Polygon right)
	{
		return !(left == right);
	}

	// Token: 0x060011DF RID: 4575 RVA: 0x00082CB9 File Offset: 0x00080EB9
	public static Polygon Subtract(Polygon initialPoly, Polygon subtractionPoly)
	{
		return Polygon.CombinePolygons(initialPoly, subtractionPoly, false);
	}

	// Token: 0x060011E0 RID: 4576 RVA: 0x00082CC3 File Offset: 0x00080EC3
	public static Polygon Add(Polygon initialPoly, Polygon additionPoly)
	{
		return Polygon.CombinePolygons(initialPoly, additionPoly, true);
	}

	// Token: 0x060011E1 RID: 4577 RVA: 0x00082CCD File Offset: 0x00080ECD
	public bool intersectsWithPolygon(Polygon otherPolygon)
	{
		return Polygon.AddIntersectionPointsOfPoly(this, otherPolygon).indexList.Count > 0;
	}

	// Token: 0x060011E2 RID: 4578 RVA: 0x00082CE3 File Offset: 0x00080EE3
	public bool whollyContainsOtherPolygon(Polygon otherPolygon)
	{
		return !this.intersectsWithPolygon(otherPolygon) && this.ContainsPoint(otherPolygon.vertices[0]);
	}

	// Token: 0x060011E3 RID: 4579 RVA: 0x00082D04 File Offset: 0x00080F04
	public static Polygon CombinePolygons(Polygon initialPoly, Polygon secondPoly, bool doingAddition)
	{
		secondPoly = Polygon.AddIntersectionPointsOfPoly(secondPoly, initialPoly).polygon;
		Polygon.PolygonWithSubsetOfPoints polygonWithSubsetOfPoints = Polygon.AddIntersectionPointsOfPoly(initialPoly, secondPoly);
		initialPoly = polygonWithSubsetOfPoints.polygon;
		List<int> indexList = polygonWithSubsetOfPoints.indexList;
		if (indexList.Count != 0)
		{
			List<Vector2> list = new List<Vector2>();
			int num = 0;
			int num2 = -1;
			int num3 = 0;
			while (num3 < initialPoly.vertices.Length && (secondPoly.EncompassesPointOfIndexInOtherPoly(num3, initialPoly) || indexList.Contains(num3)))
			{
				num3++;
			}
			Polygon.AddPointUniquelyToList(Polygon.PointInPolyFromIndex(initialPoly, num3), list);
			bool flag = true;
			for (int i = num3 + 1; i < num3 + initialPoly.vertices.Length; i++)
			{
				if (indexList.Contains(i % initialPoly.vertices.Length))
				{
					if (flag)
					{
						num2 = secondPoly.IndexOfIndexInOtherPoly(i, initialPoly);
						flag = false;
						if (num == 0)
						{
							if (initialPoly.ContainsPoint((secondPoly[(num2 + 1) % secondPoly.vertices.Length] - secondPoly[num2]).normalized * 0.001f + secondPoly[num2]))
							{
								num = -1;
							}
							else
							{
								num = 1;
							}
							if (!doingAddition)
							{
								num *= -1;
							}
						}
					}
					else
					{
						flag = true;
						int num4 = secondPoly.IndexOfIndexInOtherPoly(i, initialPoly);
						while ((num2 - num4) % secondPoly.vertices.Length != 0)
						{
							Polygon.AddPointUniquelyToList(Polygon.PointInPolyFromIndex(secondPoly, num2), list);
							num2 += num;
						}
					}
					Polygon.AddPointUniquelyToList(Polygon.PointInPolyFromIndex(initialPoly, i), list);
				}
				else if (flag)
				{
					Polygon.AddPointUniquelyToList(Polygon.PointInPolyFromIndex(initialPoly, i), list);
				}
			}
			return new Polygon(list.ToArray());
		}
		if (doingAddition)
		{
			if (initialPoly.whollyContainsOtherPolygon(secondPoly))
			{
				return new Polygon(initialPoly);
			}
			if (secondPoly.whollyContainsOtherPolygon(initialPoly))
			{
				return new Polygon(secondPoly);
			}
			return Polygon.BridgePolys(initialPoly, secondPoly);
		}
		else
		{
			if (!initialPoly.whollyContainsOtherPolygon(secondPoly))
			{
				return new Polygon(initialPoly);
			}
			if (secondPoly.whollyContainsOtherPolygon(initialPoly))
			{
				return new Polygon(Array.Empty<Vector2>());
			}
			Debug.LogError("Tried to subtract a polygon and leave a hole. We can't support this.");
			return null;
		}
	}

	// Token: 0x060011E4 RID: 4580 RVA: 0x00082EE4 File Offset: 0x000810E4
	public static Polygon BridgePolys(Polygon polyOne, Polygon polyTwo)
	{
		int num = -1;
		int num2 = -1;
		float num3 = Vector2.Distance(polyOne.center, polyTwo.center);
		for (int i = 0; i < polyOne.vertices.Length; i++)
		{
			for (int j = 0; j < polyTwo.vertices.Length; j++)
			{
				float num4 = Vector2.Distance(polyOne.vertices[i], polyTwo.vertices[j]);
				bool flag = polyOne.ContainsPoint((polyTwo.vertices[j] - polyOne.vertices[i]) * 0.001f + polyOne.vertices[i]);
				bool flag2 = polyTwo.ContainsPoint((polyOne.vertices[i] - polyTwo.vertices[j]) * 0.001f + polyTwo.vertices[j]);
				if (!flag && !flag2 && num4 < num3)
				{
					num3 = num4;
					num = i;
					num2 = j;
				}
			}
		}
		float num5 = float.MaxValue;
		int num6 = -1;
		int num7 = -1;
		for (int k = -1; k <= 1; k += 2)
		{
			Vector2 vertex = polyOne.GetVertex(num + k);
			for (int l = -1; l <= 1; l += 2)
			{
				Vector2 vertex2 = polyTwo.GetVertex(num2 + l);
				bool flag3 = polyOne.ContainsPoint((polyTwo.GetVertex(num2) - vertex).normalized * 0.001f + vertex);
				bool flag4 = polyTwo.ContainsPoint((polyOne.GetVertex(num) - vertex2).normalized * 0.001f + vertex2);
				bool flag5 = polyOne.ContainsPoint((vertex2 - vertex).normalized * 0.001f + vertex);
				bool flag6 = polyTwo.ContainsPoint((vertex - vertex2).normalized * 0.001f + vertex2);
				if (!flag5 && !flag6 && (!flag3 || !flag4))
				{
					float num8 = Vector2.Distance(vertex, vertex2);
					if (num8 < num5)
					{
						num5 = num8;
						num6 = k;
						num7 = l;
					}
				}
			}
		}
		List<Vector2> list = new List<Vector2>();
		for (int m = 0; m < polyOne.VertCount; m++)
		{
			list.Add(polyOne.GetVertex(num + m * -1 * num6));
		}
		for (int n = 0; n < polyTwo.VertCount; n++)
		{
			list.Add(polyTwo.GetVertex(num2 + num7 + n * num7));
		}
		return new Polygon(list.ToArray());
	}

	// Token: 0x04001273 RID: 4723
	[SerializeField]
	private Vector2[] _vertices;

	// Token: 0x04001274 RID: 4724
	private static List<int> tris = new List<int>();

	// Token: 0x04001275 RID: 4725
	private Vector2 _poleOfInaccessibility = new Vector2(0f, 0f);

	// Token: 0x04001276 RID: 4726
	private const float epsilon = 0.001f;

	// Token: 0x02000402 RID: 1026
	private struct QuadCell
	{
		// Token: 0x04001AD4 RID: 6868
		public Vector2 centre;

		// Token: 0x04001AD5 RID: 6869
		public float halfSize;

		// Token: 0x04001AD6 RID: 6870
		public float distanceFromPoly;

		// Token: 0x04001AD7 RID: 6871
		public float maxDistanceFromPoly;
	}

	// Token: 0x02000403 RID: 1027
	private struct PolygonWithSubsetOfPoints
	{
		// Token: 0x04001AD8 RID: 6872
		public Polygon polygon;

		// Token: 0x04001AD9 RID: 6873
		public List<int> indexList;
	}

	// Token: 0x02000404 RID: 1028
	public struct PolygonRaycastHit
	{
		// Token: 0x04001ADA RID: 6874
		public float distance;

		// Token: 0x04001ADB RID: 6875
		public Vector2 point;

		// Token: 0x04001ADC RID: 6876
		public Vector2 normal;
	}

	// Token: 0x02000405 RID: 1029
	public static class SutherlandHodgman
	{
		// Token: 0x060018F5 RID: 6389 RVA: 0x0009F4BC File Offset: 0x0009D6BC
		public static void GetIntersectedPolygon(Vector2[] subjectPoly, Vector2[] clipPoly, ref List<Vector2> outputList)
		{
			if (subjectPoly.Length < 3 || clipPoly.Length < 3)
			{
				Debug.LogError(string.Format("The polygons passed in must have at least 3 Vector2s: subject={0}, clip={1}", subjectPoly.Length.ToString(), clipPoly.Length.ToString()));
			}
			if (outputList == null)
			{
				outputList = new List<Vector2>();
			}
			else
			{
				outputList.Clear();
				outputList.AddRange(subjectPoly);
			}
			bool isClockwise = Polygon.GetIsClockwise(subjectPoly);
			if (!isClockwise)
			{
				outputList.Reverse();
			}
			List<Vector2> list = new List<Vector2>();
			foreach (Polygon.SutherlandHodgman.Edge edge in Polygon.SutherlandHodgman.IterateEdgesClockwise(clipPoly))
			{
				list.Clear();
				list.AddRange(outputList);
				outputList.Clear();
				if (list.Count == 0)
				{
					break;
				}
				Vector2 vector = list[list.Count - 1];
				foreach (Vector2 vector2 in list)
				{
					if (Polygon.SutherlandHodgman.IsInside(edge, vector2))
					{
						if (!Polygon.SutherlandHodgman.IsInside(edge, vector))
						{
							Vector2? intersect = Polygon.SutherlandHodgman.GetIntersect(vector, vector2, edge.From, edge.To);
							if (intersect == null)
							{
								Debug.LogError("Line segments don't intersect");
							}
							else
							{
								outputList.Add(intersect.Value);
							}
						}
						outputList.Add(vector2);
					}
					else if (Polygon.SutherlandHodgman.IsInside(edge, vector))
					{
						Vector2? intersect2 = Polygon.SutherlandHodgman.GetIntersect(vector, vector2, edge.From, edge.To);
						if (intersect2 == null)
						{
							Debug.LogError("Line segments don't intersect");
						}
						else
						{
							outputList.Add(intersect2.Value);
						}
					}
					vector = vector2;
				}
			}
			if (!isClockwise)
			{
				outputList.Reverse();
			}
		}

		// Token: 0x060018F6 RID: 6390 RVA: 0x0009F6A8 File Offset: 0x0009D8A8
		private static IEnumerable<Polygon.SutherlandHodgman.Edge> IterateEdgesClockwise(Vector2[] polygon)
		{
			if (Polygon.GetIsClockwise(polygon))
			{
				int num;
				for (int cntr = 0; cntr < polygon.Length - 1; cntr = num + 1)
				{
					yield return new Polygon.SutherlandHodgman.Edge(polygon[cntr], polygon[cntr + 1]);
					num = cntr;
				}
				yield return new Polygon.SutherlandHodgman.Edge(polygon[polygon.Length - 1], polygon[0]);
			}
			else
			{
				int num;
				for (int cntr = polygon.Length - 1; cntr > 0; cntr = num - 1)
				{
					yield return new Polygon.SutherlandHodgman.Edge(polygon[cntr], polygon[cntr - 1]);
					num = cntr;
				}
				yield return new Polygon.SutherlandHodgman.Edge(polygon[0], polygon[polygon.Length - 1]);
			}
			yield break;
		}

		// Token: 0x060018F7 RID: 6391 RVA: 0x0009F6B8 File Offset: 0x0009D8B8
		private static Vector2? GetIntersect(Vector2 line1From, Vector2 line1To, Vector2 line2From, Vector2 line2To)
		{
			Vector2 vector = line1To - line1From;
			Vector2 vector2 = line2To - line2From;
			float num = vector.x * vector2.y - vector.y * vector2.x;
			if (Mathf.Abs(num) <= 1E-05f)
			{
				return null;
			}
			Vector2 vector3 = line2From - line1From;
			float num2 = (vector3.x * vector2.y - vector3.y * vector2.x) / num;
			return new Vector2?(line1From + num2 * vector);
		}

		// Token: 0x060018F8 RID: 6392 RVA: 0x0009F744 File Offset: 0x0009D944
		private static bool IsInside(Polygon.SutherlandHodgman.Edge edge, Vector2 test)
		{
			bool? flag = Polygon.SutherlandHodgman.IsLeftOf(edge, test);
			return flag == null || !flag.Value;
		}

		// Token: 0x060018F9 RID: 6393 RVA: 0x0009F770 File Offset: 0x0009D970
		private static bool? IsLeftOf(Polygon.SutherlandHodgman.Edge edge, Vector2 test)
		{
			Vector2 vector = edge.To - edge.From;
			Vector2 vector2 = test - edge.To;
			double num = (double)(vector.x * vector2.y - vector.y * vector2.x);
			if (num < 0.0)
			{
				return new bool?(false);
			}
			if (num > 0.0)
			{
				return new bool?(true);
			}
			return null;
		}

		// Token: 0x02000436 RID: 1078
		private class Edge
		{
			// Token: 0x06001980 RID: 6528 RVA: 0x000A0F45 File Offset: 0x0009F145
			public Edge(Vector2 from, Vector2 to)
			{
				this.From = from;
				this.To = to;
			}

			// Token: 0x04001B98 RID: 7064
			public readonly Vector2 From;

			// Token: 0x04001B99 RID: 7065
			public readonly Vector2 To;
		}
	}
}
