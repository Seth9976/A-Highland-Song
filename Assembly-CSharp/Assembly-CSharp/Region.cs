using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020001C8 RID: 456
public class Region : MonoBehaviour
{
	// Token: 0x06000F11 RID: 3857 RVA: 0x00074094 File Offset: 0x00072294
	public static Region GetRegionAtPosition(Vector3 position)
	{
		foreach (Region region in Region.activeRegions)
		{
			if (region.ContainsPoint(position))
			{
				return region;
			}
		}
		return null;
	}

	// Token: 0x06000F12 RID: 3858 RVA: 0x000740F0 File Offset: 0x000722F0
	public static List<Region> GetRegionsAtPosition(Vector3 position)
	{
		List<Region> list = new List<Region>();
		foreach (Region region in Region.activeRegions)
		{
			if (region.ContainsPoint(position))
			{
				list.Add(region);
			}
		}
		return list;
	}

	// Token: 0x17000380 RID: 896
	// (get) Token: 0x06000F13 RID: 3859 RVA: 0x00074154 File Offset: 0x00072354
	// (set) Token: 0x06000F14 RID: 3860 RVA: 0x0007415C File Offset: 0x0007235C
	public Polygon polygon
	{
		get
		{
			return this._polygon;
		}
		set
		{
			if (this._polygon == value)
			{
				return;
			}
			this._polygon = value;
			this.OnPolygonPropertiesChanged();
		}
	}

	// Token: 0x17000381 RID: 897
	// (get) Token: 0x06000F15 RID: 3861 RVA: 0x0007417A File Offset: 0x0007237A
	// (set) Token: 0x06000F16 RID: 3862 RVA: 0x00074182 File Offset: 0x00072382
	public float height
	{
		get
		{
			return this._height;
		}
		set
		{
			this._height = value;
			this.OnPolygonPropertiesChanged();
		}
	}

	// Token: 0x06000F17 RID: 3863 RVA: 0x00074191 File Offset: 0x00072391
	private void OnPolygonPropertiesChanged()
	{
		if (base.enabled)
		{
			this.OnPropertiesChanged();
			return;
		}
		this.rebuildPropertiesOnEnable = true;
	}

	// Token: 0x17000382 RID: 898
	// (get) Token: 0x06000F18 RID: 3864 RVA: 0x000741A9 File Offset: 0x000723A9
	public bool in2DMode
	{
		get
		{
			return this.height <= 0f || this.height == float.PositiveInfinity;
		}
	}

	// Token: 0x17000383 RID: 899
	// (get) Token: 0x06000F19 RID: 3865 RVA: 0x000741C8 File Offset: 0x000723C8
	public Vector3 worldNormal
	{
		get
		{
			return this.matrix.MultiplyVector(Vector3.forward).normalized;
		}
	}

	// Token: 0x17000384 RID: 900
	// (get) Token: 0x06000F1A RID: 3866 RVA: 0x000741F0 File Offset: 0x000723F0
	public Plane floorPlane
	{
		get
		{
			return new Plane(this.worldNormal, this.matrix.MultiplyPoint3x4(Vector3.zero));
		}
	}

	// Token: 0x17000385 RID: 901
	// (get) Token: 0x06000F1B RID: 3867 RVA: 0x0007421C File Offset: 0x0007241C
	public Plane frontPlane
	{
		get
		{
			Vector3 worldNormal = this.worldNormal;
			return new Plane(worldNormal, this.matrix.MultiplyPoint3x4(Vector3.zero) + worldNormal * this.height * 0.5f);
		}
	}

	// Token: 0x17000386 RID: 902
	// (get) Token: 0x06000F1C RID: 3868 RVA: 0x00074264 File Offset: 0x00072464
	public Plane backPlane
	{
		get
		{
			Vector3 worldNormal = this.worldNormal;
			return new Plane(worldNormal, this.matrix.MultiplyPoint3x4(Vector3.zero) + worldNormal * -this.height * 0.5f);
		}
	}

	// Token: 0x17000387 RID: 903
	// (get) Token: 0x06000F1D RID: 3869 RVA: 0x000742B0 File Offset: 0x000724B0
	public Plane[] edgePlanes
	{
		get
		{
			Vector3[] verts3D = this.verts3D;
			Plane[] array = new Plane[verts3D.Length];
			Vector3 worldNormal = this.worldNormal;
			for (int i = 0; i < verts3D.Length; i++)
			{
				Vector3 vector = verts3D[i];
				Vector3 vector2 = verts3D[(i == verts3D.Length - 1) ? 0 : (i + 1)];
				Vector3 vector3 = Vector3.Cross(Vector3.Normalize(vector2 - vector), worldNormal);
				array[i] = new Plane(vector3, Vector3.Lerp(vector, vector2, 0.5f));
			}
			return array;
		}
	}

	// Token: 0x17000388 RID: 904
	// (get) Token: 0x06000F1E RID: 3870 RVA: 0x00074334 File Offset: 0x00072534
	// (set) Token: 0x06000F1F RID: 3871 RVA: 0x0007433C File Offset: 0x0007253C
	public Rect polygonRect
	{
		get
		{
			return this._polygonRect;
		}
		private set
		{
			this._polygonRect = value;
		}
	}

	// Token: 0x17000389 RID: 905
	// (get) Token: 0x06000F20 RID: 3872 RVA: 0x00074348 File Offset: 0x00072548
	public Bounds bounds
	{
		get
		{
			Matrix4x4 matrix = this.matrix;
			Vector3[] array = new Vector3[8];
			Vector3 vector = this.localBounds.min;
			Vector3 vector2 = this.localBounds.max;
			array[0] = matrix.MultiplyPoint3x4(vector);
			array[1] = matrix.MultiplyPoint3x4(vector2);
			array[2] = matrix.MultiplyPoint3x4(new Vector3(vector.x, vector.y, vector2.z));
			array[3] = matrix.MultiplyPoint3x4(new Vector3(vector.x, vector2.y, vector.z));
			array[4] = matrix.MultiplyPoint3x4(new Vector3(vector2.x, vector.y, vector.z));
			array[5] = matrix.MultiplyPoint3x4(new Vector3(vector.x, vector2.y, vector2.z));
			array[6] = matrix.MultiplyPoint3x4(new Vector3(vector2.x, vector.y, vector2.z));
			array[7] = matrix.MultiplyPoint3x4(new Vector3(vector2.x, vector2.y, vector.z));
			Bounds bounds = new Bounds(array[0], Vector3.zero);
			vector = bounds.min;
			vector2 = bounds.max;
			foreach (Vector3 vector3 in array)
			{
				if (vector3.x < vector.x)
				{
					vector.x = vector3.x;
				}
				else if (vector3.x > vector2.x)
				{
					vector2.x = vector3.x;
				}
				if (vector3.y < vector.y)
				{
					vector.y = vector3.y;
				}
				else if (vector3.y > vector2.y)
				{
					vector2.y = vector3.y;
				}
				if (vector3.z < vector.z)
				{
					vector.z = vector3.z;
				}
				else if (vector3.z > vector2.z)
				{
					vector2.z = vector3.z;
				}
			}
			bounds.SetMinMax(vector, vector2);
			return bounds;
		}
	}

	// Token: 0x1700038A RID: 906
	// (get) Token: 0x06000F21 RID: 3873 RVA: 0x00074588 File Offset: 0x00072788
	// (set) Token: 0x06000F22 RID: 3874 RVA: 0x00074590 File Offset: 0x00072790
	public Bounds localBounds
	{
		get
		{
			return this._localBounds;
		}
		private set
		{
			this._localBounds = value;
		}
	}

	// Token: 0x1700038B RID: 907
	// (get) Token: 0x06000F23 RID: 3875 RVA: 0x0007459C File Offset: 0x0007279C
	public Vector3[] verts3D
	{
		get
		{
			Matrix4x4 matrix = this.matrix;
			Vector3[] array = new Vector3[this.polygon.vertices.Length];
			for (int i = 0; i < this.polygon.vertices.Length; i++)
			{
				array[i] = matrix.MultiplyPoint3x4(this.polygon.vertices[i]);
			}
			return array;
		}
	}

	// Token: 0x06000F24 RID: 3876 RVA: 0x00074600 File Offset: 0x00072800
	public void GetVerts3DNonAlloc(ref Vector3[] verts)
	{
		Matrix4x4 matrix = this.matrix;
		if (verts == null)
		{
			verts = new Vector3[this.polygon.vertices.Length];
		}
		else if (verts.Length != this.polygon.vertices.Length)
		{
			Array.Resize<Vector3>(ref verts, this.polygon.vertices.Length);
		}
		for (int i = 0; i < this.polygon.vertices.Length; i++)
		{
			verts[i] = matrix.MultiplyPoint3x4(this.polygon.vertices[i]);
		}
	}

	// Token: 0x1700038C RID: 908
	// (get) Token: 0x06000F25 RID: 3877 RVA: 0x00074691 File Offset: 0x00072891
	// (set) Token: 0x06000F26 RID: 3878 RVA: 0x00074699 File Offset: 0x00072899
	public Matrix4x4 offsetMatrix
	{
		get
		{
			return this._offsetMatrix;
		}
		set
		{
			if (this._offsetMatrix == value)
			{
				return;
			}
			this._offsetMatrix = value;
			this._matrixDirty = true;
		}
	}

	// Token: 0x1700038D RID: 909
	// (get) Token: 0x06000F27 RID: 3879 RVA: 0x000746B8 File Offset: 0x000728B8
	public Matrix4x4 matrix
	{
		get
		{
			if (!this._matrixDirty && base.transform.localToWorldMatrix != this.cachedLocalToWorldMatrix)
			{
				this._matrixDirty = true;
			}
			if (this._matrixDirty)
			{
				this.RefreshMatrix();
			}
			return this._matrix;
		}
	}

	// Token: 0x1700038E RID: 910
	// (get) Token: 0x06000F28 RID: 3880 RVA: 0x000746F5 File Offset: 0x000728F5
	public Matrix4x4 inverseMatrix
	{
		get
		{
			if (!this._matrixDirty && base.transform.localToWorldMatrix != this.cachedLocalToWorldMatrix)
			{
				this._matrixDirty = true;
			}
			if (this._matrixDirty)
			{
				this.RefreshMatrix();
			}
			return this._inverseMatrix;
		}
	}

	// Token: 0x06000F29 RID: 3881 RVA: 0x00074734 File Offset: 0x00072934
	private void RefreshMatrix()
	{
		this._matrix = base.transform.localToWorldMatrix * this.offsetMatrix;
		this._inverseMatrix = this._matrix.inverse;
		this.cachedLocalToWorldMatrix = base.transform.localToWorldMatrix;
		this._matrixDirty = false;
	}

	// Token: 0x06000F2A RID: 3882 RVA: 0x00074786 File Offset: 0x00072986
	private void OnEnable()
	{
		if (this.rebuildPropertiesOnEnable)
		{
			this.OnPropertiesChanged();
		}
	}

	// Token: 0x06000F2B RID: 3883 RVA: 0x00074798 File Offset: 0x00072998
	private void Reset()
	{
		this.polygon = new Polygon(new Vector2[]
		{
			new Vector2(-0.5f, 0.5f),
			new Vector2(0.5f, 0.5f),
			new Vector2(0.5f, -0.5f),
			new Vector2(-0.5f, -0.5f)
		});
		this.height = 1f;
	}

	// Token: 0x06000F2C RID: 3884 RVA: 0x0007481C File Offset: 0x00072A1C
	public Vector3 PolygonSpaceToWorldPoint(Vector3 worldPoint)
	{
		return this.matrix.MultiplyPoint3x4(worldPoint);
	}

	// Token: 0x06000F2D RID: 3885 RVA: 0x00074838 File Offset: 0x00072A38
	public Vector3 WorldToPolygonSpacePoint(Vector3 worldPoint)
	{
		return this.inverseMatrix.MultiplyPoint3x4(worldPoint);
	}

	// Token: 0x06000F2E RID: 3886 RVA: 0x00074854 File Offset: 0x00072A54
	public Vector3 PolygonSpaceToWorldVector(Vector3 worldPoint)
	{
		return this.matrix.MultiplyVector(worldPoint);
	}

	// Token: 0x06000F2F RID: 3887 RVA: 0x00074870 File Offset: 0x00072A70
	public Vector3 WorldToPolygonSpaceVector(Vector3 worldPoint)
	{
		return this.inverseMatrix.MultiplyVector(worldPoint);
	}

	// Token: 0x06000F30 RID: 3888 RVA: 0x0007488C File Offset: 0x00072A8C
	public bool ContainsPoint(Vector3 point)
	{
		Vector3 vector = this.WorldToPolygonSpacePoint(point);
		return this.ContainsPolygonSpacePoint(vector);
	}

	// Token: 0x06000F31 RID: 3889 RVA: 0x000748A8 File Offset: 0x00072AA8
	public bool ContainsPolygonSpacePoint(Vector3 polygonSpace)
	{
		if (this.in2DMode)
		{
			return this.ContainsPolygonSpacePoint2D(polygonSpace);
		}
		return this.ContainsPolygonSpacePoint3D(polygonSpace);
	}

	// Token: 0x06000F32 RID: 3890 RVA: 0x000748C8 File Offset: 0x00072AC8
	private bool ContainsPolygonSpacePoint3D(Vector3 polygonSpace)
	{
		return Region.<ContainsPolygonSpacePoint3D>g__SqrDistance|61_0(this.localBounds.center, polygonSpace) <= this.localBoundsSqrRadius && this.localBounds.Contains(polygonSpace) && polygonSpace.z <= this.height * 0.5f && this.polygon.ContainsPoint(polygonSpace);
	}

	// Token: 0x06000F33 RID: 3891 RVA: 0x00074930 File Offset: 0x00072B30
	private bool ContainsPolygonSpacePoint2D(Vector2 polygonSpace)
	{
		return this.polygonRect.Contains(polygonSpace) && this.polygon.ContainsPoint(polygonSpace);
	}

	// Token: 0x06000F34 RID: 3892 RVA: 0x0007495C File Offset: 0x00072B5C
	public Vector3 ClosestPointInRegion(Vector3 point)
	{
		Vector3 vector = this.WorldToPolygonSpacePoint(point);
		vector = this.polygon.FindClosestPointInPolygon(vector);
		Vector3 vector2 = this.matrix.MultiplyPoint3x4(vector);
		if (!this.in2DMode)
		{
			float num = this.floorPlane.GetDistanceToPoint(point);
			num = Mathf.Clamp(num, -this.height, this.height);
			vector2 += this.floorPlane.normal * num;
		}
		return vector2;
	}

	// Token: 0x06000F35 RID: 3893 RVA: 0x000749E4 File Offset: 0x00072BE4
	public Vector3 ClosestPointInRegionIgnoringHeight(Vector3 point)
	{
		Vector3 vector = this.WorldToPolygonSpacePoint(point);
		vector = this.polygon.FindClosestPointInPolygon(vector);
		Vector3 vector2 = this.matrix.MultiplyPoint3x4(vector);
		float distanceToPoint = this.floorPlane.GetDistanceToPoint(point);
		return vector2 + this.floorPlane.normal * distanceToPoint;
	}

	// Token: 0x06000F36 RID: 3894 RVA: 0x00074A48 File Offset: 0x00072C48
	public Vector3 ClosestPointOnRegionEdge(Vector3 point)
	{
		Vector3 vector = this.WorldToPolygonSpacePoint(point);
		vector = this.polygon.FindClosestPointOnPolygon(vector, true);
		Vector3 vector2 = this.matrix.MultiplyPoint3x4(vector);
		if (!this.in2DMode)
		{
			float num = this.floorPlane.GetDistanceToPoint(point);
			num = Mathf.Clamp(num, -this.height, this.height);
			vector2 += this.floorPlane.normal * num;
		}
		return vector2;
	}

	// Token: 0x06000F37 RID: 3895 RVA: 0x00074AD0 File Offset: 0x00072CD0
	public float SignedDistanceFromPoint(Vector3 position)
	{
		Vector3 vector = this.ClosestPointOnRegionEdge(position);
		float num;
		if (this.in2DMode)
		{
			num = Vector3.ProjectOnPlane(vector - position, this.floorPlane.normal).magnitude;
		}
		else
		{
			num = Vector3.Distance(position, vector);
		}
		if (this.ContainsPoint(position))
		{
			num = -num;
		}
		return num;
	}

	// Token: 0x06000F38 RID: 3896 RVA: 0x00074B30 File Offset: 0x00072D30
	public float GetVolume()
	{
		float num = this.polygon.GetArea();
		if (!this.in2DMode)
		{
			num *= this.height;
		}
		return num;
	}

	// Token: 0x06000F39 RID: 3897 RVA: 0x00074B5C File Offset: 0x00072D5C
	public Vector3 GetRandomPointInRegion()
	{
		Vector3 vector = this.PolygonSpaceToWorldPoint(this.polygon.GetRandomPointInPolygon());
		if (!this.in2DMode)
		{
			vector += this.floorPlane.normal * Random.Range(-this.height, this.height) * 0.5f;
		}
		return vector;
	}

	// Token: 0x06000F3A RID: 3898 RVA: 0x00074BC0 File Offset: 0x00072DC0
	public bool Linecast(Line3D line)
	{
		if (!this.in2DMode)
		{
			float num = 0f;
			if (Region.PlaneLineIntersectionPoint(this.backPlane, line, out num))
			{
				Vector3 vector = line.AtDistance(num);
				Vector3 vector2 = this.WorldToPolygonSpacePoint(vector);
				if (this.ContainsPolygonSpacePoint2D(vector2))
				{
					return true;
				}
			}
			if (Region.PlaneLineIntersectionPoint(this.frontPlane, line, out num))
			{
				Vector3 vector3 = line.AtDistance(num);
				Vector3 vector4 = this.WorldToPolygonSpacePoint(vector3);
				if (this.ContainsPolygonSpacePoint2D(vector4))
				{
					return true;
				}
			}
		}
		Vector3 vector5 = this.WorldToPolygonSpacePoint(line.start);
		Vector3 vector6 = this.WorldToPolygonSpacePoint(line.end);
		Line line2 = new Line(vector5, vector6);
		Line3D line3D = new Line3D(vector5, vector6);
		using (IEnumerator<Line> enumerator = this.polygon.GetLines().GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Vector2 vector7;
				if (Line.LineIntersectionPoint(enumerator.Current, line2, out vector7, true, true))
				{
					float normalizedDistanceOnLine = Line.GetNormalizedDistanceOnLine(line2.start, line2.end, vector7, true);
					if (this.in2DMode)
					{
						return true;
					}
					if (Mathf.Abs(line3D.AtDistance(normalizedDistanceOnLine * line.length).z) < this.height * 0.5f)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06000F3B RID: 3899 RVA: 0x00074D24 File Offset: 0x00072F24
	public bool LineIntersectionPoints(Line3D line, ref List<Vector3> intersectionPoints)
	{
		intersectionPoints.Clear();
		if (!this.in2DMode)
		{
			float num = 0f;
			if (Region.PlaneLineIntersectionPoint(this.backPlane, line, out num))
			{
				Vector3 vector = line.AtDistance(num);
				Vector3 vector2 = this.WorldToPolygonSpacePoint(vector);
				if (this.ContainsPolygonSpacePoint2D(vector2))
				{
					intersectionPoints.Add(vector);
				}
			}
			if (Region.PlaneLineIntersectionPoint(this.frontPlane, line, out num))
			{
				Vector3 vector3 = line.AtDistance(num);
				Vector3 vector4 = this.WorldToPolygonSpacePoint(vector3);
				if (this.ContainsPolygonSpacePoint2D(vector4))
				{
					intersectionPoints.Add(vector3);
				}
			}
		}
		Vector3 vector5 = this.WorldToPolygonSpacePoint(line.start);
		Vector3 vector6 = this.WorldToPolygonSpacePoint(line.end);
		Line line2 = new Line(vector5, vector6);
		Line3D line3D = new Line3D(vector5, vector6);
		using (IEnumerator<Line> enumerator = this.polygon.GetLines().GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Vector2 vector7;
				if (Line.LineIntersectionPoint(enumerator.Current, line2, out vector7, true, true))
				{
					float normalizedDistanceOnLine = Line.GetNormalizedDistanceOnLine(line2.start, line2.end, vector7, true);
					if (this.in2DMode)
					{
						intersectionPoints.Add(line.AtDistance(normalizedDistanceOnLine * line.length));
					}
					else if (Mathf.Abs(line3D.AtDistance(normalizedDistanceOnLine * line.length).z) < this.height * 0.5f)
					{
						intersectionPoints.Add(line.AtDistance(normalizedDistanceOnLine * line.length));
					}
				}
			}
		}
		return intersectionPoints.Count > 0;
	}

	// Token: 0x06000F3C RID: 3900 RVA: 0x00074ED4 File Offset: 0x000730D4
	private static bool PlaneLineIntersectionPoint(Plane plane, Line3D line, out float intersectionLineDistance)
	{
		Vector3 vector = Vector3.Normalize(line.end - line.start);
		float num = Vector3.Dot(plane.normal, vector);
		if (Mathf.Abs(num) > Mathf.Epsilon)
		{
			Vector3 vector2 = -plane.normal * plane.distance;
			Vector3 vector3 = line.start - vector2;
			intersectionLineDistance = -Vector3.Dot(plane.normal, vector3) / num;
			return intersectionLineDistance >= 0f && intersectionLineDistance <= line.length;
		}
		intersectionLineDistance = 0f;
		return false;
	}

	// Token: 0x06000F3D RID: 3901 RVA: 0x00074F6C File Offset: 0x0007316C
	public static int LineIntersectionNormalizedDistances(Matrix4x4 inverseMatrix, IEnumerable<Line> polygonLines, Line3D line, ref List<float> intersectionDistances)
	{
		Debug.LogWarning("TODO - integrate region height! See LineIntersectionPoints, although this that function is unsorted");
		intersectionDistances.Clear();
		Line line2 = new Line(inverseMatrix.MultiplyPoint3x4(line.start), inverseMatrix.MultiplyPoint3x4(line.end));
		int num = 0;
		int num2 = 0;
		using (IEnumerator<Line> enumerator = polygonLines.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Vector2 vector;
				if (Line.LineIntersectionPoint(enumerator.Current, line2, out vector, true, true))
				{
					float normalizedDistanceOnLine = Line.GetNormalizedDistanceOnLine(line2.start, line2.end, vector, true);
					if (num2 >= intersectionDistances.Count)
					{
						intersectionDistances.Add(normalizedDistanceOnLine);
					}
					else
					{
						intersectionDistances[num2] = normalizedDistanceOnLine;
					}
					num++;
				}
				num2++;
			}
		}
		return num;
	}

	// Token: 0x06000F3E RID: 3902 RVA: 0x0007503C File Offset: 0x0007323C
	public List<Region.RegionRaycastHit> RayRegionIntersections(Vector3 rayOrigin, Vector3 rayDirection)
	{
		Vector3 vector = this.WorldToPolygonSpacePoint(rayOrigin);
		Vector3 vector2 = this.WorldToPolygonSpaceVector(rayDirection);
		List<Region.RegionRaycastHit> list = new List<Region.RegionRaycastHit>();
		Region.RegionRaycastHit regionRaycastHit = default(Region.RegionRaycastHit);
		if (!this.in2DMode)
		{
			regionRaycastHit.distance = Region.<RayRegionIntersections>g__GetDistanceToPointInDirection|74_0(this.backPlane, new Ray(rayOrigin, rayDirection));
			if (regionRaycastHit.distance > 0f)
			{
				regionRaycastHit.point = rayOrigin + rayDirection * regionRaycastHit.distance;
				Vector3 vector3 = this.WorldToPolygonSpacePoint(regionRaycastHit.point);
				if (this.ContainsPolygonSpacePoint2D(vector3))
				{
					list.Add(regionRaycastHit);
				}
			}
			regionRaycastHit.distance = Region.<RayRegionIntersections>g__GetDistanceToPointInDirection|74_0(this.frontPlane, new Ray(rayOrigin, rayDirection));
			if (regionRaycastHit.distance > 0f)
			{
				regionRaycastHit.point = rayOrigin + rayDirection * regionRaycastHit.distance;
				Vector3 vector4 = this.WorldToPolygonSpacePoint(regionRaycastHit.point);
				if (this.ContainsPolygonSpacePoint2D(vector4))
				{
					list.Add(regionRaycastHit);
				}
			}
		}
		foreach (Polygon.PolygonRaycastHit polygonRaycastHit in this.polygon.RayPolygonIntersections(vector, vector2))
		{
			regionRaycastHit.distance = polygonRaycastHit.distance;
			regionRaycastHit.point = rayOrigin + rayDirection * polygonRaycastHit.distance;
			bool flag = false;
			if (this.in2DMode)
			{
				flag = true;
			}
			else if (Mathf.Abs((vector + vector2 * polygonRaycastHit.distance).z) < this.height * 0.5f)
			{
				flag = true;
			}
			if (flag)
			{
				for (int i = 0; i < list.Count; i++)
				{
					if (regionRaycastHit.distance < list[i].distance)
					{
						list.Insert(i, regionRaycastHit);
						flag = false;
						break;
					}
				}
				if (flag)
				{
					list.Add(regionRaycastHit);
				}
			}
		}
		return list;
	}

	// Token: 0x06000F3F RID: 3903 RVA: 0x00075240 File Offset: 0x00073440
	public Vector3 GetCenter()
	{
		return this.matrix.MultiplyPoint3x4(this.polygon.center);
	}

	// Token: 0x06000F40 RID: 3904 RVA: 0x0007526C File Offset: 0x0007346C
	public void CreatePolygonMesh(ref Mesh mesh, bool drawFront = true, bool drawBack = false)
	{
		mesh.Clear();
		Vector2[] vertices = this.polygon.vertices;
		if (vertices == null || vertices.Length == 0)
		{
			return;
		}
		if (!drawFront && !drawBack)
		{
			return;
		}
		List<int> list = new List<int>();
		Triangulator.GenerateIndices(this.polygon.vertices, list);
		int num = list.Count - 1;
		int num2 = (drawFront ? 1 : 0) + (drawBack ? 1 : 0);
		Vector3[] array = new Vector3[num2 * 6 * vertices.Length + this.polygon.vertices.Length * (num2 * 2)];
		int[] array2 = new int[num2 * 6 * vertices.Length + list.Count * (num2 * 2)];
		int num3 = 0;
		int num4 = 0;
		Vector3 vector = Vector3.forward * this.height * 0.5f;
		Vector3 vector2 = Vector3.zero;
		Vector3 vector3 = vertices[vertices.Length - 1];
		Vector3 vector4 = -vector + vector3;
		Vector3 vector5 = vector + vector3;
		for (int i = 0; i < vertices.Length; i++)
		{
			vector3 = vertices[i];
			Vector3 vector6 = -vector + vector3;
			vector2 = vector + vector3;
			if (drawFront)
			{
				array[num3] = vector4;
				num3++;
				array[num3] = vector6;
				num3++;
				array[num3] = vector5;
				num3++;
				array[num3] = vector6;
				num3++;
				array[num3] = vector2;
				num3++;
				array[num3] = vector5;
				num3++;
				for (int j = 0; j < 6; j++)
				{
					array2[num4 + j] = num4 + j;
				}
				num4 += 6;
			}
			if (drawBack)
			{
				array[num3] = vector5;
				num3++;
				array[num3] = vector6;
				num3++;
				array[num3] = vector4;
				num3++;
				array[num3] = vector5;
				num3++;
				array[num3] = vector2;
				num3++;
				array[num3] = vector6;
				num3++;
				for (int k = 0; k < 6; k++)
				{
					array2[num4 + k] = num4 + k;
				}
				num4 += 6;
			}
			vector4 = vector6;
			vector5 = vector2;
		}
		if (drawFront)
		{
			for (int l = 0; l < vertices.Length; l++)
			{
				array[num3 + l] = vertices[l] - vector;
			}
			for (int m = 0; m < list.Count; m++)
			{
				array2[num4 + m] = list[m] + num3;
			}
			num3 += vertices.Length;
			num4 += list.Count;
			for (int n = 0; n < vertices.Length; n++)
			{
				array[num3 + n] = vertices[n] + vector;
			}
			for (int num5 = 0; num5 < list.Count; num5++)
			{
				array2[num4 + num5] = list[num - num5] + num3;
			}
			num3 += vertices.Length;
			num4 += list.Count;
		}
		if (drawBack)
		{
			for (int num6 = 0; num6 < vertices.Length; num6++)
			{
				array[num3 + num6] = vertices[num6] - vector;
			}
			for (int num7 = 0; num7 < list.Count; num7++)
			{
				array2[num4 + num7] = list[num - num7] + num3;
			}
			num3 += vertices.Length;
			num4 += list.Count;
			for (int num8 = 0; num8 < vertices.Length; num8++)
			{
				array[num3 + num8] = vertices[num8] + vector;
			}
			for (int num9 = 0; num9 < list.Count; num9++)
			{
				array2[num4 + num9] = list[num9] + num3;
			}
			num3 += vertices.Length;
			num4 += list.Count;
		}
		mesh.vertices = array;
		mesh.triangles = array2;
		mesh.RecalculateNormals();
	}

	// Token: 0x06000F41 RID: 3905 RVA: 0x000756A0 File Offset: 0x000738A0
	public Mesh CreatePolygonMesh(bool drawFront = true, bool drawBack = false)
	{
		Mesh mesh = new Mesh();
		this.CreatePolygonMesh(ref mesh, drawFront, drawBack);
		return mesh;
	}

	// Token: 0x06000F42 RID: 3906 RVA: 0x000756BE File Offset: 0x000738BE
	[ContextMenu("Refresh")]
	public void OnPropertiesChanged()
	{
		this.RebuildProperties();
		if (this.OnChange != null)
		{
			this.OnChange(this);
		}
	}

	// Token: 0x06000F43 RID: 3907 RVA: 0x000756DC File Offset: 0x000738DC
	private void RebuildProperties()
	{
		this.polygonRect = this.polygon.GetRect();
		this.localBounds = Region.<RebuildProperties>g__CreateEncapsulating|79_0(new Vector3[]
		{
			new Vector3(this.polygonRect.x, this.polygonRect.y, -this.height * 0.5f),
			new Vector3(this.polygonRect.x, this.polygonRect.y, this.height * 0.5f),
			new Vector3(this.polygonRect.xMax, this.polygonRect.yMax, -this.height * 0.5f),
			new Vector3(this.polygonRect.xMax, this.polygonRect.yMax, this.height * 0.5f)
		});
		this.localBoundsSqrRadius = this.localBounds.extents.sqrMagnitude;
		this.rebuildPropertiesOnEnable = false;
	}

	// Token: 0x06000F46 RID: 3910 RVA: 0x00075840 File Offset: 0x00073A40
	[CompilerGenerated]
	internal static float <ContainsPolygonSpacePoint3D>g__SqrDistance|61_0(Vector3 a, Vector3 b)
	{
		return (a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y) + (a.z - b.z) * (a.z - b.z);
	}

	// Token: 0x06000F47 RID: 3911 RVA: 0x000758A0 File Offset: 0x00073AA0
	[CompilerGenerated]
	internal static float <RayRegionIntersections>g__GetDistanceToPointInDirection|74_0(Plane plane, Ray ray)
	{
		float num = 0f;
		plane.Raycast(ray, out num);
		return num;
	}

	// Token: 0x06000F48 RID: 3912 RVA: 0x000758C0 File Offset: 0x00073AC0
	[CompilerGenerated]
	internal static Bounds <RebuildProperties>g__CreateEncapsulating|79_0(Vector3[] vectors)
	{
		if (vectors == null || vectors.Length == 0)
		{
			return new Bounds(Vector3.zero, Vector3.zero);
		}
		Bounds bounds = new Bounds(vectors[0], Vector3.zero);
		Vector3 min = bounds.min;
		Vector3 max = bounds.max;
		foreach (Vector3 vector in vectors)
		{
			if (vector.x < min.x)
			{
				min.x = vector.x;
			}
			else if (vector.x > max.x)
			{
				max.x = vector.x;
			}
			if (vector.y < min.y)
			{
				min.y = vector.y;
			}
			else if (vector.y > max.y)
			{
				max.y = vector.y;
			}
			if (vector.z < min.z)
			{
				min.z = vector.z;
			}
			else if (vector.z > max.z)
			{
				max.z = vector.z;
			}
		}
		bounds.SetMinMax(min, max);
		return bounds;
	}

	// Token: 0x040011BC RID: 4540
	public static List<Region> activeRegions = new List<Region>();

	// Token: 0x040011BD RID: 4541
	[SerializeField]
	private Polygon _polygon;

	// Token: 0x040011BE RID: 4542
	[SerializeField]
	private float _height = 1f;

	// Token: 0x040011BF RID: 4543
	private bool rebuildPropertiesOnEnable;

	// Token: 0x040011C0 RID: 4544
	[SerializeField]
	private float localBoundsSqrRadius;

	// Token: 0x040011C1 RID: 4545
	[SerializeField]
	private Rect _polygonRect;

	// Token: 0x040011C2 RID: 4546
	[SerializeField]
	private Bounds _localBounds;

	// Token: 0x040011C3 RID: 4547
	[SerializeField]
	private Matrix4x4 _offsetMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);

	// Token: 0x040011C4 RID: 4548
	private Matrix4x4 _matrix;

	// Token: 0x040011C5 RID: 4549
	private Matrix4x4 _inverseMatrix;

	// Token: 0x040011C6 RID: 4550
	private bool _matrixDirty = true;

	// Token: 0x040011C7 RID: 4551
	private Matrix4x4 cachedLocalToWorldMatrix;

	// Token: 0x040011C8 RID: 4552
	public Action<Region> OnChange;

	// Token: 0x020003D5 RID: 981
	[Serializable]
	public struct RegionRaycastHit
	{
		// Token: 0x04001A40 RID: 6720
		public float distance;

		// Token: 0x04001A41 RID: 6721
		public Vector3 point;
	}
}
