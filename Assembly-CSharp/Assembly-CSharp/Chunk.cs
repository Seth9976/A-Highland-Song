using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

// Token: 0x0200006E RID: 110
[ExecuteInEditMode]
[SelectionBase]
public class Chunk : MonoBehaviour
{
	// Token: 0x170000CF RID: 207
	// (get) Token: 0x06000313 RID: 787 RVA: 0x00018E1E File Offset: 0x0001701E
	public bool canBeUsedEastwards
	{
		get
		{
			return (this.direction & Chunk.Direction.Eastwards) > (Chunk.Direction)0;
		}
	}

	// Token: 0x170000D0 RID: 208
	// (get) Token: 0x06000314 RID: 788 RVA: 0x00018E2B File Offset: 0x0001702B
	public bool canBeUsedWestwards
	{
		get
		{
			return (this.direction & Chunk.Direction.Westwards) > (Chunk.Direction)0;
		}
	}

	// Token: 0x170000D1 RID: 209
	// (get) Token: 0x06000315 RID: 789 RVA: 0x00018E38 File Offset: 0x00017038
	public bool canBeUsedBothWays
	{
		get
		{
			return (this.direction & Chunk.Direction.BothEither) == Chunk.Direction.BothEither;
		}
	}

	// Token: 0x170000D2 RID: 210
	// (get) Token: 0x06000316 RID: 790 RVA: 0x00018E45 File Offset: 0x00017045
	public float validatedAuthoredDuration
	{
		get
		{
			if (this.authoredDuration <= 0f)
			{
				Debug.LogWarning(string.Format("Authored duration for {0} was {1}, so returning 2.0 instead. You should be able to make the authoredDuration get updated in editor by simply selecting the Chunk.", base.name, this.authoredDuration));
				return 2f;
			}
			return this.authoredDuration;
		}
	}

	// Token: 0x170000D3 RID: 211
	// (get) Token: 0x06000317 RID: 791 RVA: 0x00018E80 File Offset: 0x00017080
	public List<Poly> polys
	{
		get
		{
			if (this._polys == null || !Application.isPlaying)
			{
				if (this._polys != null)
				{
					this._polys.Clear();
				}
				else
				{
					this._polys = new List<Poly>(32);
				}
				base.GetComponentsInChildren<Poly>(this._polys);
				this._polys.RemoveAll((Poly p) => p.sceneryOnly);
			}
			return this._polys;
		}
	}

	// Token: 0x170000D4 RID: 212
	// (get) Token: 0x06000318 RID: 792 RVA: 0x00018EFC File Offset: 0x000170FC
	public List<Splat> splats
	{
		get
		{
			if (this._splats == null || !Application.isPlaying)
			{
				if (this._splats != null)
				{
					this._splats.Clear();
				}
				else
				{
					this._splats = new List<Splat>(128);
				}
				base.GetComponentsInChildren<Splat>(this._splats);
			}
			return this._splats;
		}
	}

	// Token: 0x170000D5 RID: 213
	// (get) Token: 0x06000319 RID: 793 RVA: 0x00018F4F File Offset: 0x0001714F
	public TripHazard[] trips
	{
		get
		{
			if (!this._tripsSet || !Application.isPlaying)
			{
				this._trips = base.GetComponentsInChildren<TripHazard>();
				this._tripsSet = true;
			}
			return this._trips;
		}
	}

	// Token: 0x170000D6 RID: 214
	// (get) Token: 0x0600031A RID: 794 RVA: 0x00018F79 File Offset: 0x00017179
	public BalancePoint[] balancePoints
	{
		get
		{
			if (!this._balancePointsSet || !Application.isPlaying)
			{
				this._balancePoints = base.GetComponentsInChildren<BalancePoint>();
				this._balancePointsSet = true;
			}
			return this._balancePoints;
		}
	}

	// Token: 0x170000D7 RID: 215
	// (get) Token: 0x0600031B RID: 795 RVA: 0x00018FA3 File Offset: 0x000171A3
	public ChunkGapHint[] gapHints
	{
		get
		{
			if (!this._gapHintsSet || !Application.isPlaying)
			{
				this._gapHints = base.GetComponentsInChildren<ChunkGapHint>();
				this._gapHintsSet = true;
			}
			return this._gapHints;
		}
	}

	// Token: 0x170000D8 RID: 216
	// (get) Token: 0x0600031C RID: 796 RVA: 0x00018FD0 File Offset: 0x000171D0
	private List<Transform> objectsToAlign
	{
		get
		{
			if (!this._objectsToAlignSet || !Application.isPlaying)
			{
				if (this._objectsToAlign == null)
				{
					this._objectsToAlign = new List<Transform>();
				}
				else
				{
					this._objectsToAlign.Clear();
				}
				this._objectsToAlignSet = true;
				Chunk.CollectObjectsToAlign(base.transform, this._objectsToAlign);
			}
			return this._objectsToAlign;
		}
	}

	// Token: 0x170000D9 RID: 217
	// (get) Token: 0x0600031D RID: 797 RVA: 0x0001902A File Offset: 0x0001722A
	public ChunkStartEnd startMarker
	{
		get
		{
			this.FindStartEndMarkersIfNecessary();
			return this._startMarker;
		}
	}

	// Token: 0x170000DA RID: 218
	// (get) Token: 0x0600031E RID: 798 RVA: 0x00019038 File Offset: 0x00017238
	public ChunkStartEnd endMarker
	{
		get
		{
			this.FindStartEndMarkersIfNecessary();
			return this._endMarker;
		}
	}

	// Token: 0x170000DB RID: 219
	// (get) Token: 0x0600031F RID: 799 RVA: 0x00019048 File Offset: 0x00017248
	public Alignment currentAlignment
	{
		get
		{
			if (this.startMarker != null && this.endMarker != null)
			{
				Alignment alignment = new Alignment
				{
					left = this.startMarker.point,
					right = this.endMarker.point
				};
				if (alignment.left.x > alignment.right.x)
				{
					alignment = new Alignment
					{
						left = alignment.right,
						right = alignment.left
					};
				}
				return alignment;
			}
			if (this.leftSlope != null && this.rightSlope != null)
			{
				return new Alignment
				{
					left = this.leftSlope.leftPoint,
					right = this.rightSlope.rightPoint
				};
			}
			Debug.LogError("Chunk doesn't have markers or slopes to calculate alignment", this);
			return new Alignment
			{
				left = new Vector2(-10f, 0f),
				right = new Vector2(10f, 0f)
			};
		}
	}

	// Token: 0x170000DC RID: 220
	// (get) Token: 0x06000320 RID: 800 RVA: 0x00019180 File Offset: 0x00017380
	private Alignment originalAlignment
	{
		get
		{
			if (!this._originalAlignmentFound || !Application.isPlaying)
			{
				if (this.prototype.isOriginalPrototype)
				{
					if (!this._originalAlignmentFound)
					{
						this._originalAlignment = this.currentAlignment;
					}
				}
				else
				{
					this._originalAlignment = this.originalPrototypeChunk.originalAlignment;
				}
				this._originalAlignmentFound = true;
			}
			return this._originalAlignment;
		}
	}

	// Token: 0x170000DD RID: 221
	// (get) Token: 0x06000321 RID: 801 RVA: 0x000191DD File Offset: 0x000173DD
	public Prototype prototype
	{
		get
		{
			if (this._prototype == null)
			{
				this._prototype = base.GetComponent<Prototype>();
			}
			return this._prototype;
		}
	}

	// Token: 0x170000DE RID: 222
	// (get) Token: 0x06000322 RID: 802 RVA: 0x00019200 File Offset: 0x00017400
	private Chunk originalPrototypeChunk
	{
		get
		{
			if (this._originalPrototypeChunk == null)
			{
				if (this.prototype.isOriginalPrototype)
				{
					this._originalPrototypeChunk = this;
				}
				else
				{
					this._originalPrototypeChunk = this.prototype.originalPrototype.GetComponent<Chunk>();
				}
			}
			return this._originalPrototypeChunk;
		}
	}

	// Token: 0x170000DF RID: 223
	// (get) Token: 0x06000323 RID: 803 RVA: 0x00019250 File Offset: 0x00017450
	public Range range
	{
		get
		{
			this.FindStartEndMarkersIfNecessary();
			if (this._startMarker != null && this._endMarker != null)
			{
				return Range.Auto(this._startMarker.point.x, this._endMarker.point.x);
			}
			if (this.leftSlope != null && this.rightSlope != null)
			{
				return new Range(this.leftSlope.leftEdge, this.rightSlope.rightEdge);
			}
			Debug.LogError("Chunk " + base.name + " doesn't have start/end markers or left/right slopes, can't calc range", this);
			return new Range(-10f, 10f);
		}
	}

	// Token: 0x06000324 RID: 804 RVA: 0x00019308 File Offset: 0x00017508
	public void CalculateUnderbellyRangeIfNecessary()
	{
		if (this.floorPoly == null)
		{
			Debug.LogError("Chunk doesn't have a floor poly, should it?", this);
			return;
		}
		Chunk._underbellyEdgesScratch.Clear();
		Vector3 vector = this.floorPoly.PointIdx(0);
		int num = -1;
		for (int i = 0; i <= this.floorPoly.polygon.VertCount; i++)
		{
			int num2 = i % this.floorPoly.polygon.VertCount;
			int num3 = (i + this.floorPoly.polygon.VertCount + 1) % this.floorPoly.polygon.VertCount;
			Vector3 vector2 = this.floorPoly.PointIdx(num3);
			bool flag = vector2.x < vector.x;
			float num4 = -1f;
			if (flag)
			{
				num4 = Vector2.Distance(vector, vector2);
			}
			else
			{
				num = num2;
			}
			Chunk._underbellyEdgesScratch.Add(new Chunk.UnderbellyEdge
			{
				startIdx = num2,
				downward = flag,
				length = num4
			});
			vector = vector2;
		}
		int num5 = -1;
		int num6 = 0;
		float num7 = 0f;
		int num8 = -1;
		int num9 = 0;
		float num10 = 0f;
		for (int j = 0; j <= this.floorPoly.polygon.VertCount; j++)
		{
			int num11 = (num + j) % this.floorPoly.polygon.VertCount;
			Chunk.UnderbellyEdge underbellyEdge = Chunk._underbellyEdgesScratch[num11];
			if (underbellyEdge.downward)
			{
				if (num8 == -1)
				{
					num8 = num11;
					num10 = 0f;
				}
				num10 += underbellyEdge.length;
				num9++;
				if (num10 > num7)
				{
					num7 = num10;
					num5 = num8;
					num6 = num9;
				}
			}
			else
			{
				num8 = -1;
				num10 = 0f;
				num9 = 0;
			}
		}
		if (num5 == -1 || num6 == 0)
		{
			Debug.LogError("Underbelly start/end point search failed on " + base.name + "? Is there something weird about this poly? No verts? Wrong clockwise?", this);
			return;
		}
		this.underbellyFirstVertexIdx = num5;
		this.underbellyLastVertexIdx = (num5 + num6) % this.floorPoly.polygon.VertCount;
	}

	// Token: 0x170000E0 RID: 224
	// (get) Token: 0x06000325 RID: 805 RVA: 0x00019510 File Offset: 0x00017710
	public List<Vector2> underbellyVerticesLocalToMusicRun
	{
		get
		{
			if (this._underbellyVerticesLocalToMusicRun.Count == 0)
			{
				Chunk chunk = (this.prototype.isOriginalPrototype ? this : this.prototype.originalPrototype.GetComponent<Chunk>());
				chunk.CalculateUnderbellyRangeIfNecessary();
				if (chunk.underbellyFirstVertexIdx != -1)
				{
					int vertexCount = chunk.floorPoly.vertexCount;
					int num = chunk.underbellyLastVertexIdx;
					for (;;)
					{
						Vector3 vector = this.floorPoly.PointIdx(num);
						Vector3 vector2 = this.musicRun.transform.InverseTransformPoint(vector);
						this._underbellyVerticesLocalToMusicRun.Add(vector2);
						if (num == chunk.underbellyFirstVertexIdx)
						{
							break;
						}
						num = (num - 1 + vertexCount) % vertexCount;
					}
				}
				else
				{
					Vector3 point = this.GetConnectorEast(false).point;
					Vector3 point2 = this.GetConnectorEast(true).point;
					this._underbellyVerticesLocalToMusicRun.Add(this.musicRun.transform.InverseTransformPoint(point));
					this._underbellyVerticesLocalToMusicRun.Add(this.musicRun.transform.InverseTransformPoint(point2));
				}
			}
			return this._underbellyVerticesLocalToMusicRun;
		}
	}

	// Token: 0x06000326 RID: 806 RVA: 0x0001961C File Offset: 0x0001781C
	public bool CanMatchObstacles(Obstacles obstacles, int beatCount)
	{
		if (this.matchType == Chunk.CanMatch.Never)
		{
			return false;
		}
		if (beatCount < this.minBeatCount || beatCount > this.maxBeatCount)
		{
			return false;
		}
		if (this.matchType == Chunk.CanMatch.Specific)
		{
			return obstacles.HasHopPattern(this.specificObstaclesMatch, 2 * beatCount);
		}
		if (this.matchType == Chunk.CanMatch.AnyWholeBeatHopCombo)
		{
			bool flag = false;
			for (int i = 0; i < 2 * beatCount; i++)
			{
				if (obstacles[i] != ObstacleType.None && obstacles[i] != ObstacleType.Hop)
				{
					return false;
				}
				bool flag2 = obstacles[i] == ObstacleType.Hop;
				if (flag2 && flag)
				{
					return false;
				}
				flag = flag2;
			}
			return true;
		}
		return false;
	}

	// Token: 0x06000327 RID: 807 RVA: 0x000196AC File Offset: 0x000178AC
	public void SetupWithObstacles(Obstacles obstacles, int beatCount)
	{
		foreach (PatternOptional patternOptional in this._patternOptionalObjects)
		{
			bool flag = obstacles.Has(patternOptional.optionalElementIndex, patternOptional.obstacleType);
			bool flag2 = (flag && !patternOptional.invert) || (!flag && patternOptional.invert);
			patternOptional.gameObject.SetActive(flag2);
		}
	}

	// Token: 0x06000328 RID: 808 RVA: 0x00019710 File Offset: 0x00017910
	public Chunk.Connector GetConnector(bool start)
	{
		this.FindStartEndMarkersIfNecessary();
		Chunk.Connector connector = default(Chunk.Connector);
		connector.isStart = start;
		bool flag = this.direction == Chunk.Direction.BothEither && Application.isPlaying && !this.instanceCreatedEastwards;
		ChunkStartEnd chunkStartEnd = ((start ^ flag) ? this._startMarker : this._endMarker);
		if (chunkStartEnd)
		{
			connector.rightwards = chunkStartEnd.rightwards ^ flag;
			connector.slope = chunkStartEnd.slope;
			connector.point = chunkStartEnd.point;
			connector.isOnLeft = chunkStartEnd.pointOnLeftOfSlope;
			connector.marker = chunkStartEnd;
		}
		else
		{
			if (this.direction == Chunk.Direction.BothEither)
			{
				connector.rightwards = !Application.isPlaying || this.instanceCreatedEastwards;
			}
			else
			{
				connector.rightwards = this.canBeUsedEastwards;
			}
			connector.isOnLeft = connector.rightwards ^ !start;
			connector.slope = (connector.isOnLeft ? this.leftSlope : this.rightSlope);
			if (connector.slope == null)
			{
				return connector;
			}
			connector.point = (connector.isOnLeft ? connector.slope.leftPoint : connector.slope.rightPoint);
		}
		return connector;
	}

	// Token: 0x06000329 RID: 809 RVA: 0x00019848 File Offset: 0x00017A48
	public Chunk.Connector GetConnectorEast(bool east)
	{
		return this.GetConnector(east ^ this.instanceCreatedEastwards);
	}

	// Token: 0x170000E1 RID: 225
	// (get) Token: 0x0600032A RID: 810 RVA: 0x00019858 File Offset: 0x00017A58
	public Range boundsRange
	{
		get
		{
			if (!this._calculatedBoundsRange)
			{
				this._boundsRange = new Range(float.MaxValue, float.MinValue);
				foreach (Slope slope in this.slopes)
				{
					this._boundsRange.min = Mathf.Min(this._boundsRange.min, slope.leftEdge);
					this._boundsRange.max = Mathf.Max(this._boundsRange.max, slope.rightEdge);
				}
				this._calculatedBoundsRange = true;
			}
			return this._boundsRange;
		}
	}

	// Token: 0x0600032B RID: 811 RVA: 0x00019914 File Offset: 0x00017B14
	public void FindStartEndMarkersIfNecessary()
	{
		if (!this._searchedForStartEndMarkers || !Application.isPlaying)
		{
			foreach (ChunkStartEnd chunkStartEnd in base.GetComponentsInChildren<ChunkStartEnd>())
			{
				if (chunkStartEnd.isStart)
				{
					this._startMarker = chunkStartEnd;
				}
				else
				{
					this._endMarker = chunkStartEnd;
				}
			}
			this._searchedForStartEndMarkers = true;
		}
	}

	// Token: 0x0600032C RID: 812 RVA: 0x00019968 File Offset: 0x00017B68
	private void Awake()
	{
		if (Application.isPlaying)
		{
			this._objectsToAlign = this.objectsToAlign;
			this._trips = this.trips;
			this.prototype.OnReturnToPool += this.OnReturnToPool;
			this._patternOptionalObjects = base.GetComponentsInChildren<PatternOptional>();
		}
	}

	// Token: 0x0600032D RID: 813 RVA: 0x000199B7 File Offset: 0x00017BB7
	private void OnEnable()
	{
		if (!this.prototype.isOriginalPrototype)
		{
			this.prototype.originalPrototype.GetComponent<Chunk>().originalPrototypeRefCount++;
		}
	}

	// Token: 0x0600032E RID: 814 RVA: 0x000199E3 File Offset: 0x00017BE3
	private void OnDisable()
	{
		if (!this.prototype.isOriginalPrototype)
		{
			this.prototype.originalPrototype.GetComponent<Chunk>().originalPrototypeRefCount--;
		}
	}

	// Token: 0x0600032F RID: 815 RVA: 0x00019A10 File Offset: 0x00017C10
	private void OnReturnToPool()
	{
		List<Transform> objectsToAlign = this.objectsToAlign;
		List<Transform> objectsToAlign2 = this.prototype.originalPrototype.GetComponent<Chunk>().objectsToAlign;
		for (int i = 0; i < objectsToAlign.Count; i++)
		{
			objectsToAlign[i].localPosition = objectsToAlign2[i].localPosition;
		}
		this._underbellyVerticesLocalToMusicRun.Clear();
		if (this.onRecycle != null)
		{
			this.onRecycle(this);
		}
		this.RecycleSlopesAndRemoveRefs();
		this.instanceCreatedEastwards = false;
		this._calculatedBoundsRange = false;
		this.decision.ReturnToPool();
		this.decision = default(TrackBuilder.MatchDecision);
	}

	// Token: 0x06000330 RID: 816 RVA: 0x00019AB0 File Offset: 0x00017CB0
	public void RecycleSlopesAndRemoveRefs()
	{
		SlopeFactory.Recycle(this.slopes);
		this.leftSlope = null;
		this.rightSlope = null;
		if (this.startMarker)
		{
			this.startMarker.slope = null;
		}
		if (this.endMarker)
		{
			this.endMarker.slope = null;
		}
	}

	// Token: 0x06000331 RID: 817 RVA: 0x00019B08 File Offset: 0x00017D08
	private static Matrix4x4 BasisWorldMatrixFromPoints(Alignment align, Transform relativeTransform = null)
	{
		Vector2 vector = align.left;
		Vector2 vector2 = align.right;
		if (relativeTransform)
		{
			vector = relativeTransform.TransformPoint(align.left);
			vector2 = relativeTransform.TransformPoint(align.right);
		}
		Vector2 vector3 = vector2 - vector;
		Vector2 normalized = new Vector2(-vector3.y, vector3.x).normalized;
		Vector3 vector4 = Vector3.Lerp(vector, vector2, 0.5f);
		return new Matrix4x4(vector3, normalized, Vector3.forward, new Vector4(vector4.x, vector4.y, vector4.z, 1f));
	}

	// Token: 0x06000332 RID: 818 RVA: 0x00019BD0 File Offset: 0x00017DD0
	private static Vector2 InterpolatePointFromList(List<Vector2> points, float x)
	{
		int num = BinarySearch.SearchRoundDown<Vector2>(points, x, (Vector2 p) => p.x);
		if (num >= points.Count - 1)
		{
			num = points.Count - 2;
		}
		Vector2 vector = points[num];
		Vector2 vector2 = points[num + 1];
		float num2 = Mathf.InverseLerp(vector.x, vector2.x, x);
		return Vector2.Lerp(vector, vector2, num2);
	}

	// Token: 0x06000333 RID: 819 RVA: 0x00019C44 File Offset: 0x00017E44
	private bool OrderedRightwards(IList<Vector2> points)
	{
		for (int i = 0; i < points.Count - 1; i++)
		{
			if (points[i].x > points[i + 1].x)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06000334 RID: 820 RVA: 0x00019C83 File Offset: 0x00017E83
	public void AlignTo(Alignment to)
	{
		this.AlignBetween(this.originalAlignment, to, this.originalPrototypeChunk.objectsToAlign, this.objectsToAlign);
	}

	// Token: 0x06000335 RID: 821 RVA: 0x00019CA4 File Offset: 0x00017EA4
	private void AlignBetween(Alignment from, Alignment to, IList<Transform> originalObjects, IList<Transform> targetObjects)
	{
		if (originalObjects.Count != targetObjects.Count)
		{
			Debug.LogError(string.Format("Couldn't perform alignment since the object counts aren't the same ({0} vs {1})", originalObjects.Count, targetObjects.Count));
			return;
		}
		float num = Vector2.SignedAngle(from.vector, to.vector);
		float num2 = to.vector.magnitude / from.vector.magnitude;
		Matrix4x4 matrix4x = Matrix4x4.Translate(-from.left);
		Matrix4x4 matrix4x2 = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, num), num2 * Vector3.one);
		Matrix4x4 matrix4x3 = Matrix4x4.Translate(to.left) * matrix4x2 * matrix4x;
		this.PerformRealignment(matrix4x3, originalObjects, targetObjects);
	}

	// Token: 0x06000336 RID: 822 RVA: 0x00019D84 File Offset: 0x00017F84
	public void RealignUsingScale(Vector3 pivot, float scale)
	{
		Matrix4x4 matrix4x = Matrix4x4.Translate(-pivot);
		Matrix4x4 matrix4x2 = Matrix4x4.Scale(new Vector3(scale, scale, scale));
		Matrix4x4 matrix4x3 = Matrix4x4.Translate(pivot) * matrix4x2 * matrix4x;
		this.PerformRealignment(matrix4x3, this.objectsToAlign, this.objectsToAlign);
	}

	// Token: 0x06000337 RID: 823 RVA: 0x00019DD4 File Offset: 0x00017FD4
	private void PerformRealignment(Matrix4x4 realignMatrix, IList<Transform> originalObjects, IList<Transform> targetObjects)
	{
		for (int i = 0; i < targetObjects.Count; i++)
		{
			Vector3 vector = realignMatrix.MultiplyPoint3x4(originalObjects[i].position);
			Quaternion quaternion = Quaternion.identity;
			Vector3 vector2 = Vector3.one;
			Poly component = originalObjects[i].GetComponent<Poly>();
			bool flag = originalObjects[i].GetComponent<Splat>() != null;
			ChunkAlignSettings component2 = originalObjects[i].GetComponent<ChunkAlignSettings>();
			ChunkAlignMode chunkAlignMode;
			if (component2 != null)
			{
				chunkAlignMode = component2.mode;
			}
			else if (component != null)
			{
				chunkAlignMode = ChunkAlignMode.All;
			}
			else if (flag)
			{
				chunkAlignMode = ChunkAlignMode.All;
			}
			else
			{
				chunkAlignMode = ChunkAlignMode.Move;
			}
			if ((chunkAlignMode & ChunkAlignMode.Rotate) != ChunkAlignMode.None)
			{
				quaternion = Quaternion.AngleAxis(Slope.AngleWithVector((realignMatrix.MultiplyPoint3x4(originalObjects[i].position + 0.5f * Vector3.right) - vector).normalized, false, originalObjects[i]), Vector3.forward) * originalObjects[i].localRotation;
			}
			if ((chunkAlignMode & ChunkAlignMode.Scale) != ChunkAlignMode.None)
			{
				Vector3 vector3 = realignMatrix.MultiplyPoint3x4(originalObjects[i].position + 1f * Vector3.right);
				Vector3 localScale = originalObjects[i].localScale;
				vector2 = (vector3 - vector).magnitude * localScale;
			}
			if ((chunkAlignMode & ChunkAlignMode.Move) != ChunkAlignMode.None)
			{
				vector.z = targetObjects[i].position.z;
				targetObjects[i].position = vector;
			}
			if ((chunkAlignMode & ChunkAlignMode.Rotate) != ChunkAlignMode.None)
			{
				targetObjects[i].localRotation = quaternion;
			}
			if ((chunkAlignMode & ChunkAlignMode.Scale) != ChunkAlignMode.None)
			{
				targetObjects[i].localScale = vector2;
			}
		}
		foreach (Slope slope in this.slopes)
		{
			slope.ReCalculateWorldPoints();
		}
	}

	// Token: 0x06000338 RID: 824 RVA: 0x00019FCC File Offset: 0x000181CC
	private static void CollectObjectsToAlign(Transform t, List<Transform> list)
	{
		foreach (object obj in t)
		{
			Transform transform = (Transform)obj;
			ChunkAlignSettings component = transform.GetComponent<ChunkAlignSettings>();
			SortingGroup component2 = transform.GetComponent<SortingGroup>();
			bool flag = true;
			if (component && component.alignChildren)
			{
				flag = false;
				Chunk.CollectObjectsToAlign(transform, list);
			}
			else if (component2 != null)
			{
				Chunk.CollectObjectsToAlign(transform, list);
			}
			if (component && component.alsoAlignSelf)
			{
				flag = true;
			}
			if (flag)
			{
				list.Add(transform);
			}
		}
	}

	// Token: 0x0400043F RID: 1087
	public Action<Chunk> onRecycle;

	// Token: 0x04000440 RID: 1088
	public Chunk.Direction direction;

	// Token: 0x04000441 RID: 1089
	public float baseLineOffset = -2f;

	// Token: 0x04000442 RID: 1090
	[FormerlySerializedAs("departsGuideSlope")]
	public bool departsMusicRun;

	// Token: 0x04000443 RID: 1091
	public bool dontAbsorbErrorInMargins;

	// Token: 0x04000444 RID: 1092
	public bool dontPatternMatch;

	// Token: 0x04000445 RID: 1093
	public bool debugFallbackOnly;

	// Token: 0x04000446 RID: 1094
	[HideInInspector]
	public bool instanceCreatedEastwards;

	// Token: 0x04000447 RID: 1095
	public TrackBuilder.MatchDecision decision;

	// Token: 0x04000448 RID: 1096
	[NonSerialized]
	public int originalPrototypeRefCount;

	// Token: 0x04000449 RID: 1097
	[Range(0f, 1f)]
	public float weight = 1f;

	// Token: 0x0400044A RID: 1098
	public bool debugSuperHighWeight;

	// Token: 0x0400044B RID: 1099
	public Range validSlopeAbsAngleRange;

	// Token: 0x0400044C RID: 1100
	[NonSerialized]
	public Slope leftSlope;

	// Token: 0x0400044D RID: 1101
	[NonSerialized]
	public Slope rightSlope;

	// Token: 0x0400044E RID: 1102
	[NonSerialized]
	public MusicRun musicRun;

	// Token: 0x0400044F RID: 1103
	[NonSerialized]
	public float instantiatedTime;

	// Token: 0x04000450 RID: 1104
	[NonSerialized]
	public Chunk flippedPrototype;

	// Token: 0x04000451 RID: 1105
	[NonSerialized]
	public bool liveForMusic;

	// Token: 0x04000452 RID: 1106
	public Poly floorPoly;

	// Token: 0x04000453 RID: 1107
	public ObstaclePlacement[] obstaclePlacements;

	// Token: 0x04000454 RID: 1108
	public Chunk.CanMatch matchType = Chunk.CanMatch.Specific;

	// Token: 0x04000455 RID: 1109
	public Obstacles specificObstaclesMatch;

	// Token: 0x04000456 RID: 1110
	public int minBeatCount = 4;

	// Token: 0x04000457 RID: 1111
	public int maxBeatCount = 4;

	// Token: 0x04000458 RID: 1112
	public float authoredDuration;

	// Token: 0x04000459 RID: 1113
	[NonSerialized]
	public List<Slope> slopes = new List<Slope>(32);

	// Token: 0x0400045A RID: 1114
	private List<Poly> _polys;

	// Token: 0x0400045B RID: 1115
	private List<Splat> _splats;

	// Token: 0x0400045C RID: 1116
	private TripHazard[] _trips;

	// Token: 0x0400045D RID: 1117
	private bool _tripsSet;

	// Token: 0x0400045E RID: 1118
	private BalancePoint[] _balancePoints;

	// Token: 0x0400045F RID: 1119
	private bool _balancePointsSet;

	// Token: 0x04000460 RID: 1120
	private ChunkGapHint[] _gapHints;

	// Token: 0x04000461 RID: 1121
	private bool _gapHintsSet;

	// Token: 0x04000462 RID: 1122
	private List<Transform> _objectsToAlign;

	// Token: 0x04000463 RID: 1123
	private bool _objectsToAlignSet;

	// Token: 0x04000464 RID: 1124
	private Alignment _originalAlignment;

	// Token: 0x04000465 RID: 1125
	private bool _originalAlignmentFound;

	// Token: 0x04000466 RID: 1126
	private Prototype _prototype;

	// Token: 0x04000467 RID: 1127
	private Chunk _originalPrototypeChunk;

	// Token: 0x04000468 RID: 1128
	[NonSerialized]
	public int underbellyFirstVertexIdx = -1;

	// Token: 0x04000469 RID: 1129
	[NonSerialized]
	public int underbellyLastVertexIdx = -1;

	// Token: 0x0400046A RID: 1130
	private static List<Chunk.UnderbellyEdge> _underbellyEdgesScratch = new List<Chunk.UnderbellyEdge>();

	// Token: 0x0400046B RID: 1131
	private List<Vector2> _underbellyVerticesLocalToMusicRun = new List<Vector2>();

	// Token: 0x0400046C RID: 1132
	private Range _boundsRange;

	// Token: 0x0400046D RID: 1133
	private bool _calculatedBoundsRange;

	// Token: 0x0400046E RID: 1134
	private PatternOptional[] _patternOptionalObjects;

	// Token: 0x0400046F RID: 1135
	private bool _searchedForStartEndMarkers;

	// Token: 0x04000470 RID: 1136
	private ChunkStartEnd _startMarker;

	// Token: 0x04000471 RID: 1137
	private ChunkStartEnd _endMarker;

	// Token: 0x02000293 RID: 659
	[Flags]
	public enum Direction
	{
		// Token: 0x04001521 RID: 5409
		Eastwards = 1,
		// Token: 0x04001522 RID: 5410
		Westwards = 2,
		// Token: 0x04001523 RID: 5411
		BothEither = 3
	}

	// Token: 0x02000294 RID: 660
	public enum CanMatch
	{
		// Token: 0x04001525 RID: 5413
		Never,
		// Token: 0x04001526 RID: 5414
		Specific,
		// Token: 0x04001527 RID: 5415
		AnyWholeBeatHopCombo
	}

	// Token: 0x02000295 RID: 661
	private struct UnderbellyEdge
	{
		// Token: 0x04001528 RID: 5416
		public bool downward;

		// Token: 0x04001529 RID: 5417
		public float length;

		// Token: 0x0400152A RID: 5418
		public int startIdx;
	}

	// Token: 0x02000296 RID: 662
	public struct Connector
	{
		// Token: 0x0400152B RID: 5419
		public bool rightwards;

		// Token: 0x0400152C RID: 5420
		public Slope slope;

		// Token: 0x0400152D RID: 5421
		public Vector3 point;

		// Token: 0x0400152E RID: 5422
		public bool isStart;

		// Token: 0x0400152F RID: 5423
		public bool isOnLeft;

		// Token: 0x04001530 RID: 5424
		public ChunkStartEnd marker;
	}
}
