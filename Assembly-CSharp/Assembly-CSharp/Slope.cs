using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001A9 RID: 425
[SelectionBase]
public class Slope : BasePolyAndSlope
{
	// Token: 0x17000353 RID: 851
	// (get) Token: 0x06000DF6 RID: 3574 RVA: 0x0006E2FF File Offset: 0x0006C4FF
	public RunPredictHint predictModeHint
	{
		get
		{
			if (this._predictModeHint == null)
			{
				this._predictModeHint = base.GetComponentInChildren<RunPredictHint>();
			}
			return this._predictModeHint;
		}
	}

	// Token: 0x17000354 RID: 852
	// (get) Token: 0x06000DF7 RID: 3575 RVA: 0x0006E321 File Offset: 0x0006C521
	public float leftEdge
	{
		get
		{
			return this.PointIdx(0).x;
		}
	}

	// Token: 0x17000355 RID: 853
	// (get) Token: 0x06000DF8 RID: 3576 RVA: 0x0006E32F File Offset: 0x0006C52F
	public float rightEdge
	{
		get
		{
			return this.PointIdx(this.localPoints.Count - 1).x;
		}
	}

	// Token: 0x17000356 RID: 854
	// (get) Token: 0x06000DF9 RID: 3577 RVA: 0x0006E349 File Offset: 0x0006C549
	public Range range
	{
		get
		{
			return new Range(this.leftEdge, this.rightEdge);
		}
	}

	// Token: 0x17000357 RID: 855
	// (get) Token: 0x06000DFA RID: 3578 RVA: 0x0006E35C File Offset: 0x0006C55C
	// (set) Token: 0x06000DFB RID: 3579 RVA: 0x0006E364 File Offset: 0x0006C564
	public bool isStatic
	{
		get
		{
			return this._isStatic;
		}
		set
		{
			if (this._isStatic == value)
			{
				return;
			}
			this._isStatic = value;
			if (this._isStatic)
			{
				this.CalculateStaticWorldPointsFromLocal();
				return;
			}
			if (this._worldPoints != null)
			{
				this._worldPoints.Clear();
			}
		}
	}

	// Token: 0x06000DFC RID: 3580 RVA: 0x0006E399 File Offset: 0x0006C599
	public void OnAddedToLevel(bool isStatic)
	{
		this.isStatic = isStatic;
	}

	// Token: 0x06000DFD RID: 3581 RVA: 0x0006E3A2 File Offset: 0x0006C5A2
	public Vector3 PointIdx(int idx)
	{
		if (this._isStatic)
		{
			return this._worldPoints[idx];
		}
		return base.transform.TransformPoint(this.localPoints[idx]);
	}

	// Token: 0x06000DFE RID: 3582 RVA: 0x0006E3D5 File Offset: 0x0006C5D5
	public void SetPointIdx(int idx, Vector3 worldPoint)
	{
		this.localPoints[idx] = base.transform.InverseTransformPoint(worldPoint);
		if (this._isStatic)
		{
			this._worldPoints[idx] = worldPoint;
		}
	}

	// Token: 0x17000358 RID: 856
	// (get) Token: 0x06000DFF RID: 3583 RVA: 0x0006E409 File Offset: 0x0006C609
	public int numberOfPoints
	{
		get
		{
			return this.localPoints.Count;
		}
	}

	// Token: 0x17000359 RID: 857
	// (get) Token: 0x06000E00 RID: 3584 RVA: 0x0006E416 File Offset: 0x0006C616
	public SlopeSample leftSample
	{
		get
		{
			return this.SampleBetweenPointsNorm(0, 0f);
		}
	}

	// Token: 0x1700035A RID: 858
	// (get) Token: 0x06000E01 RID: 3585 RVA: 0x0006E424 File Offset: 0x0006C624
	public SlopeSample rightSample
	{
		get
		{
			return this.SampleBetweenPointsNorm(this.localPoints.Count - 2, 1f);
		}
	}

	// Token: 0x1700035B RID: 859
	// (get) Token: 0x06000E02 RID: 3586 RVA: 0x0006E43E File Offset: 0x0006C63E
	// (set) Token: 0x06000E03 RID: 3587 RVA: 0x0006E447 File Offset: 0x0006C647
	public Vector3 leftPoint
	{
		get
		{
			return this.PointIdx(0);
		}
		set
		{
			this.SetPointIdx(0, value);
		}
	}

	// Token: 0x1700035C RID: 860
	// (get) Token: 0x06000E04 RID: 3588 RVA: 0x0006E451 File Offset: 0x0006C651
	// (set) Token: 0x06000E05 RID: 3589 RVA: 0x0006E461 File Offset: 0x0006C661
	public Vector3 rightPoint
	{
		get
		{
			return this.PointIdx(this.numberOfPoints - 1);
		}
		set
		{
			this.SetPointIdx(this.numberOfPoints - 1, value);
		}
	}

	// Token: 0x06000E06 RID: 3590 RVA: 0x0006E474 File Offset: 0x0006C674
	public SlopeSample SampleBetweenPointsNorm(int i0, float t)
	{
		SlopeSample slopeSample = new SlopeSample
		{
			slope = this,
			depth = base.transform.position.z
		};
		slopeSample.i0 = i0;
		slopeSample.i1 = slopeSample.i0 + 1;
		Vector2 vector;
		Vector2 vector2;
		if (this._isStatic)
		{
			vector = this.PointIdx(slopeSample.i0);
			vector2 = this.PointIdx(slopeSample.i0 + 1);
		}
		else
		{
			vector = base.transform.TransformPoint(this.localPoints[slopeSample.i0]);
			vector2 = base.transform.TransformPoint(this.localPoints[slopeSample.i0 + 1]);
		}
		slopeSample.t = t;
		slopeSample.point = (slopeSample.clampedPoint = Vector2.Lerp(vector, vector2, t));
		Vector2 vector3 = vector2 - vector;
		if (vector3.x < 0f)
		{
			vector3 = -vector3;
		}
		slopeSample.normal = new Vector2(-vector3.y, vector3.x).normalized;
		slopeSample.angle = Slope.AngleWithVector(vector3, false, this);
		return slopeSample;
	}

	// Token: 0x06000E07 RID: 3591 RVA: 0x0006E5B4 File Offset: 0x0006C7B4
	public SlopeSample SampleAt(float x, bool clampIfVeryClose = false)
	{
		Range range = new Range(this.leftPoint.x, this.rightPoint.x);
		if (range.Contains(x))
		{
			Vector3 vector = this.PointIdx(0);
			for (int i = 1; i < this.numberOfPoints; i++)
			{
				Vector3 vector2 = this.PointIdx(i);
				if (vector2.x < vector.x)
				{
					Debug.LogError("Slope went backwards. Should always be left -> right for this type of sampling.", this);
				}
				if (x <= vector2.x)
				{
					return this.SampleBetweenPointsNorm(i - 1, Mathf.InverseLerp(vector.x, vector2.x, x));
				}
				vector = vector2;
			}
			throw new Exception("Shouldn't be able to get here");
		}
		SlopeSample slopeSample = ((x < range.min) ? this.leftSample : this.rightSample);
		if (clampIfVeryClose && Mathf.Abs(slopeSample.point.x - x) < 0.05f)
		{
			return slopeSample;
		}
		slopeSample.point.x = x;
		slopeSample.normal = Vector2.up;
		slopeSample.angle = 0f;
		slopeSample.outOfRange = true;
		return slopeSample;
	}

	// Token: 0x1700035D RID: 861
	// (get) Token: 0x06000E08 RID: 3592 RVA: 0x0006E6BD File Offset: 0x0006C8BD
	// (set) Token: 0x06000E09 RID: 3593 RVA: 0x0006E6C5 File Offset: 0x0006C8C5
	public List<Vector2> localPoints
	{
		get
		{
			return this._localPoints;
		}
		set
		{
			this._localPoints = value;
		}
	}

	// Token: 0x06000E0A RID: 3594 RVA: 0x0006E6D0 File Offset: 0x0006C8D0
	public void CopyLocalPoints(IList<Vector2> newLocalPoints)
	{
		if (this._localPoints == null)
		{
			this._localPoints = new List<Vector2>(newLocalPoints.Count);
		}
		else
		{
			this._localPoints.Clear();
			if (this._localPoints.Capacity < newLocalPoints.Count)
			{
				this._localPoints.Capacity = newLocalPoints.Count;
			}
		}
		this._localPoints.AddRange(newLocalPoints);
	}

	// Token: 0x06000E0B RID: 3595 RVA: 0x0006E733 File Offset: 0x0006C933
	private void ResetWorldPoints(int capacity)
	{
		if (this._worldPoints == null)
		{
			this._worldPoints = new List<Vector3>(capacity);
			return;
		}
		this._worldPoints.Clear();
		if (this._worldPoints.Capacity < capacity)
		{
			this._worldPoints.Capacity = capacity;
		}
	}

	// Token: 0x06000E0C RID: 3596 RVA: 0x0006E770 File Offset: 0x0006C970
	private void CalculateStaticWorldPointsFromLocal()
	{
		bool flag = false;
		this.ResetWorldPoints(this._localPoints.Capacity);
		foreach (Vector2 vector in this._localPoints)
		{
			Vector3 vector2 = base.transform.TransformPoint(vector);
			float z = vector2.z;
			if (flag && Mathf.Abs(z - (float)Mathf.RoundToInt(z)) > 0.001f)
			{
				string text = "Slope has non-integer z coordinate: ";
				Poly poly = this.originPoly;
				Debug.LogError(text + ((poly != null) ? poly.name : null), this);
			}
			this._worldPoints.Add(vector2);
		}
	}

	// Token: 0x06000E0D RID: 3597 RVA: 0x0006E834 File Offset: 0x0006CA34
	public void SetWorldPoints(List<Vector2> worldPoints)
	{
		this.ResetWorldPoints(worldPoints.Capacity);
		float z = base.transform.position.z;
		foreach (Vector2 vector in worldPoints)
		{
			Vector3 vector2 = vector;
			vector2.z = z;
			this._worldPoints.Add(vector2);
		}
		this._isStatic = true;
		if (this._localPoints == null)
		{
			this._localPoints = new List<Vector2>(worldPoints.Capacity);
		}
		else
		{
			this._localPoints.Clear();
			if (this._localPoints.Capacity < worldPoints.Capacity)
			{
				this._localPoints.Capacity = worldPoints.Capacity;
			}
		}
		for (int i = 0; i < worldPoints.Count; i++)
		{
			this._localPoints.Add(base.transform.InverseTransformPoint(worldPoints[i]));
		}
	}

	// Token: 0x06000E0E RID: 3598 RVA: 0x0006E93C File Offset: 0x0006CB3C
	public void ReCalculateWorldPoints()
	{
		this.ResetWorldPoints(this._localPoints.Count);
		this._isStatic = false;
		this._worldPoints.Clear();
		for (int i = 0; i < this._localPoints.Count; i++)
		{
			this._worldPoints.Add(this.PointIdx(i));
		}
		this._isStatic = true;
	}

	// Token: 0x06000E0F RID: 3599 RVA: 0x0006E99C File Offset: 0x0006CB9C
	public void ReverseIfNecessary()
	{
		if (this._localPoints[0].x > this._localPoints[this._localPoints.Count - 1].x)
		{
			this._localPoints.Reverse();
			if (this._worldPoints != null)
			{
				this._worldPoints.Reverse();
			}
		}
	}

	// Token: 0x06000E10 RID: 3600 RVA: 0x0006E9F8 File Offset: 0x0006CBF8
	public Alignment AlignmentForRange(Range range)
	{
		range.min = Mathf.Max(this.leftPoint.x, range.min);
		range.max = Mathf.Min(this.rightPoint.x, range.max);
		SlopeSample slopeSample = this.SampleAt(range.min, false);
		SlopeSample slopeSample2 = this.SampleAt(range.max, false);
		return new Alignment
		{
			left = slopeSample.point,
			right = slopeSample2.point
		};
	}

	// Token: 0x06000E11 RID: 3601 RVA: 0x0006EA80 File Offset: 0x0006CC80
	public Range AngleRange(Range betweenXRange)
	{
		Range range = new Range(float.MaxValue, float.MinValue);
		float leftEdge = this.leftEdge;
		float rightEdge = this.rightEdge;
		Range range2 = new Range(Mathf.InverseLerp(leftEdge, rightEdge, betweenXRange.min), Mathf.InverseLerp(leftEdge, rightEdge, betweenXRange.max));
		float x = this.localPoints[0].x;
		float x2 = this.localPoints[this.localPoints.Count - 1].x;
		Range range3 = new Range(Mathf.Lerp(x, x2, range2.min), Mathf.Lerp(x, x2, range2.max));
		for (int i = BinarySearch.SearchRoundDown<Vector2>(this.localPoints, range3.min, (Vector2 p) => p.x); i < this.numberOfPoints - 1; i++)
		{
			Vector2 vector = this.localPoints[i];
			Vector2 vector2 = this.localPoints[i + 1];
			float num = Slope.AngleWithVector(vector2 - vector, false, this);
			range.min = Mathf.Min(range.min, num);
			range.max = Mathf.Max(range.max, num);
			if (vector2.x > range3.max)
			{
				break;
			}
		}
		return range;
	}

	// Token: 0x06000E12 RID: 3602 RVA: 0x0006EBD4 File Offset: 0x0006CDD4
	public float HeightOfPointAbove(Vector2 pointToSample)
	{
		Vector2 vector = base.transform.InverseTransformPoint(pointToSample);
		for (int i = 0; i < this._localPoints.Count - 1; i++)
		{
			Vector2 vector2 = this._localPoints[i];
			Vector2 vector3 = this._localPoints[i + 1];
			if (vector.x < vector2.x)
			{
				return float.MaxValue;
			}
			if (vector.x <= vector3.x)
			{
				float num = Mathf.InverseLerp(vector2.x, vector3.x, vector.x);
				float num2 = Mathf.Lerp(vector2.y, vector3.y, num);
				return vector.y - num2;
			}
		}
		return float.MaxValue;
	}

	// Token: 0x06000E13 RID: 3603 RVA: 0x0006EC8E File Offset: 0x0006CE8E
	public static float AngleWithVector(Vector2 vec, bool autoDir = false, Object context = null)
	{
		if (vec.x < 0f && autoDir)
		{
			vec = -vec;
		}
		return Mathf.Atan2(vec.y, vec.x) * 57.29578f;
	}

	// Token: 0x06000E14 RID: 3604 RVA: 0x0006ECBF File Offset: 0x0006CEBF
	public static Vector2 VectorWithAngle(float angle, float direction = 1f)
	{
		return new Vector2(direction * Mathf.Cos(angle * 0.017453292f), Mathf.Sin(angle * 0.017453292f));
	}

	// Token: 0x06000E15 RID: 3605 RVA: 0x0006ECE0 File Offset: 0x0006CEE0
	private void OnEnable()
	{
		this.isStatic = false;
		if (this.leftSlopes.Count > 0)
		{
			this._authoredLeftSlopes = new List<Slope>(this.leftSlopes.Count);
			this._authoredLeftSlopes.AddRange(this.leftSlopes);
		}
		else if (this._authoredLeftSlopes != null && this._authoredLeftSlopes.Count > 0)
		{
			this.leftSlopes.AddRange(this._authoredLeftSlopes);
		}
		if (this.rightSlopes.Count > 0)
		{
			this._authoredRightSlopes = new List<Slope>(this.rightSlopes.Count);
			this._authoredRightSlopes.AddRange(this.rightSlopes);
		}
		else if (this._authoredRightSlopes != null && this._authoredRightSlopes.Count > 0)
		{
			this.rightSlopes.AddRange(this._authoredRightSlopes);
		}
		this.reverseFlow = false;
		this.nextEastSlope = null;
		this.nextWestSlope = null;
	}

	// Token: 0x06000E16 RID: 3606 RVA: 0x0006EDC8 File Offset: 0x0006CFC8
	private void OnDisable()
	{
		foreach (Slope slope in this.leftSlopes)
		{
			if (slope != null)
			{
				slope.leftSlopes.Remove(this);
				slope.rightSlopes.Remove(this);
			}
		}
		foreach (Slope slope2 in this.rightSlopes)
		{
			if (slope2 != null)
			{
				slope2.leftSlopes.Remove(this);
				slope2.rightSlopes.Remove(this);
			}
		}
		this.leftSlopes.Clear();
		this.rightSlopes.Clear();
	}

	// Token: 0x1700035E RID: 862
	// (get) Token: 0x06000E17 RID: 3607 RVA: 0x0006EEAC File Offset: 0x0006D0AC
	public Bounds bounds
	{
		get
		{
			Slope.points3DForBounds.Clear();
			for (int i = 0; i < this.localPoints.Count; i++)
			{
				Slope.points3DForBounds.Add(this.PointIdx(i));
			}
			return BoundsX.CreateEncapsulating(Slope.points3DForBounds);
		}
	}

	// Token: 0x1700035F RID: 863
	// (get) Token: 0x06000E18 RID: 3608 RVA: 0x0006EEF4 File Offset: 0x0006D0F4
	private float roughRadius
	{
		get
		{
			Vector3 position = base.transform.position;
			float num = 0f;
			for (int i = 0; i < this.localPoints.Count; i++)
			{
				Vector3 vector = this.PointIdx(i);
				float num2 = Vector3.Distance(base.transform.position, vector);
				if (num2 > num)
				{
					num = num2;
				}
			}
			return num;
		}
	}

	// Token: 0x06000E19 RID: 3609 RVA: 0x0006EF4C File Offset: 0x0006D14C
	[ContextMenu("Copy points from edge collider")]
	private void CopyFromEdgeCollider()
	{
		EdgeCollider2D component = base.GetComponent<EdgeCollider2D>();
		this.CopyLocalPoints(component.points);
	}

	// Token: 0x040010D9 RID: 4313
	public Color defaultColor;

	// Token: 0x040010DA RID: 4314
	[NonSerialized]
	public Chunk chunk;

	// Token: 0x040010DB RID: 4315
	public Poly originPoly;

	// Token: 0x040010DC RID: 4316
	private RunPredictHint _predictModeHint;

	// Token: 0x040010DD RID: 4317
	public List<Slope> leftSlopes = new List<Slope>();

	// Token: 0x040010DE RID: 4318
	public List<Slope> rightSlopes = new List<Slope>();

	// Token: 0x040010DF RID: 4319
	public bool connectedLeft;

	// Token: 0x040010E0 RID: 4320
	public bool connectedRight;

	// Token: 0x040010E1 RID: 4321
	public Slope hintNextSlope;

	// Token: 0x040010E2 RID: 4322
	[Disable]
	public bool reverseFlow;

	// Token: 0x040010E3 RID: 4323
	[Disable]
	public Slope nextEastSlope;

	// Token: 0x040010E4 RID: 4324
	[Disable]
	public Slope nextWestSlope;

	// Token: 0x040010E5 RID: 4325
	public bool isSlide;

	// Token: 0x040010E6 RID: 4326
	[NonSerialized]
	private bool _isStatic;

	// Token: 0x040010E7 RID: 4327
	[SerializeField]
	private List<Vector2> _localPoints;

	// Token: 0x040010E8 RID: 4328
	[NonSerialized]
	private List<Vector3> _worldPoints;

	// Token: 0x040010E9 RID: 4329
	private static List<Vector3> points3DForBounds = new List<Vector3>();

	// Token: 0x040010EA RID: 4330
	[SerializeField]
	[HideInInspector]
	private List<Slope> _authoredLeftSlopes;

	// Token: 0x040010EB RID: 4331
	[SerializeField]
	[HideInInspector]
	private List<Slope> _authoredRightSlopes;
}
