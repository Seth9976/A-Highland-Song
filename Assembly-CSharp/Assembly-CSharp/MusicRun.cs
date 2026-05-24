using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000198 RID: 408
[NullableContext(1)]
[Nullable(0)]
public class MusicRun : MonoBehaviour
{
	// Token: 0x17000311 RID: 785
	// (get) Token: 0x06000D44 RID: 3396 RVA: 0x00069EC8 File Offset: 0x000680C8
	// (set) Token: 0x06000D45 RID: 3397 RVA: 0x00069F30 File Offset: 0x00068130
	public Range range
	{
		get
		{
			float x = base.transform.TransformPoint(new Vector2(this._localRange.min, 0f)).x;
			float x2 = base.transform.TransformPoint(new Vector2(this._localRange.max, 0f)).x;
			return new Range(x, x2);
		}
		set
		{
			float y = base.transform.position.y;
			this._localRange = new Range(base.transform.InverseTransformPoint(new Vector2(value.min, y)).x, base.transform.InverseTransformPoint(new Vector2(value.max, y)).x);
		}
	}

	// Token: 0x17000312 RID: 786
	// (get) Token: 0x06000D46 RID: 3398 RVA: 0x00069F9D File Offset: 0x0006819D
	[Nullable(2)]
	public Chunk westChunk
	{
		[NullableContext(2)]
		get
		{
			if (this.chunks.Count <= 0)
			{
				return null;
			}
			return this.chunks[0];
		}
	}

	// Token: 0x17000313 RID: 787
	// (get) Token: 0x06000D47 RID: 3399 RVA: 0x00069FBB File Offset: 0x000681BB
	[Nullable(2)]
	public Chunk eastChunk
	{
		[NullableContext(2)]
		get
		{
			if (this.chunks.Count <= 0)
			{
				return null;
			}
			return this.chunks[this.chunks.Count - 1];
		}
	}

	// Token: 0x17000314 RID: 788
	// (get) Token: 0x06000D48 RID: 3400 RVA: 0x00069FE5 File Offset: 0x000681E5
	[Nullable(2)]
	public Slope westSlopeToJoin
	{
		[NullableContext(2)]
		get
		{
			if (this.slope == null)
			{
				return null;
			}
			if (this.slope.leftSlopes.Count == 0)
			{
				return null;
			}
			return this.slope.leftSlopes[0];
		}
	}

	// Token: 0x17000315 RID: 789
	// (get) Token: 0x06000D49 RID: 3401 RVA: 0x0006A01C File Offset: 0x0006821C
	[Nullable(2)]
	public Slope eastSlopeToJoin
	{
		[NullableContext(2)]
		get
		{
			if (this.slope == null)
			{
				return null;
			}
			if (this.slope.rightSlopes.Count == 0)
			{
				return null;
			}
			return this.slope.rightSlopes[0];
		}
	}

	// Token: 0x17000316 RID: 790
	// (get) Token: 0x06000D4A RID: 3402 RVA: 0x0006A053 File Offset: 0x00068253
	public Poly poly
	{
		get
		{
			if (this._poly == null)
			{
				this._poly = base.GetComponent<Poly>();
			}
			return this._poly;
		}
	}

	// Token: 0x17000317 RID: 791
	// (get) Token: 0x06000D4B RID: 3403 RVA: 0x0006A075 File Offset: 0x00068275
	// (set) Token: 0x06000D4C RID: 3404 RVA: 0x0006A07D File Offset: 0x0006827D
	[Nullable(new byte[] { 2, 1 })]
	public MusicRunFlock[] musicRunFlocks
	{
		[return: Nullable(new byte[] { 2, 1 })]
		get;
		[param: Nullable(new byte[] { 2, 1 })]
		private set;
	}

	// Token: 0x06000D4D RID: 3405 RVA: 0x0006A088 File Offset: 0x00068288
	public void MarkAllChunksNonLive()
	{
		foreach (Chunk chunk in this.chunks)
		{
			chunk.liveForMusic = false;
		}
	}

	// Token: 0x06000D4E RID: 3406 RVA: 0x0006A0DC File Offset: 0x000682DC
	public MusicRun.Extents FindExtents()
	{
		MusicRun.Extents extents = new MusicRun.Extents
		{
			leftPoint = new Vector2(float.MaxValue, 0f),
			rightPoint = new Vector2(float.MinValue, 0f),
			vertexIndexBeforeRun = -1,
			vertexIndexAfterRun = -1
		};
		bool flag = false;
		bool flag2 = false;
		Vector3 vector = new Vector3(float.MaxValue, 0f, 0f);
		Vector3 vector2 = new Vector3(float.MinValue, 0f, 0f);
		Range range = this.range;
		Poly poly = this.poly;
		if (poly == null || poly.vertexCount == 0)
		{
			return extents;
		}
		bool isClockwise = poly.polygon.isClockwise;
		for (int i = 0; i < poly.polygon.VertCount; i++)
		{
			int num = (i + poly.polygon.VertCount + 1) % poly.polygon.VertCount;
			Vector3 vector3 = poly.PointIdx(i);
			Vector3 vector4 = poly.PointIdx(num);
			if (Poly.EdgeRunnableOrSlidable(vector3, vector4, poly.EdgeIsSlideable(i), true, poly))
			{
				Vector3 vector5 = vector3;
				Vector3 vector6 = vector4;
				if (vector3.x > vector4.x)
				{
					vector5 = vector4;
					vector6 = vector3;
				}
				Range range2 = new Range(vector5.x, vector6.x);
				if (range2.Contains(range.min))
				{
					float num2 = range2.InverseLerp(range.min);
					Vector3 vector7 = Vector3.Lerp(vector5, vector6, num2);
					extents.leftPoint = vector7;
					extents.vertexIndexBeforeRun = i;
					flag = true;
				}
				if (range2.Contains(range.max))
				{
					float num3 = range2.InverseLerp(range.max);
					Vector3 vector8 = Vector3.Lerp(vector5, vector6, num3);
					extents.rightPoint = vector8;
					extents.vertexIndexAfterRun = num;
					flag2 = true;
				}
				if (vector5.x < vector.x)
				{
					vector = vector5;
				}
				if (vector6.x > vector2.x)
				{
					vector2 = vector6;
				}
			}
		}
		if (!flag2)
		{
			extents.rightPoint = vector2;
		}
		if (!flag)
		{
			extents.leftPoint = vector;
		}
		return extents;
	}

	// Token: 0x06000D4F RID: 3407 RVA: 0x0006A318 File Offset: 0x00068518
	private void CheckAndRemoveDuplicateVert()
	{
		int count = MusicRun._polyVerticesBuildList.Count;
		if (count < 2)
		{
			return;
		}
		Vector2 vector = MusicRun._polyVerticesBuildList[count - 2];
		Vector2 vector2 = MusicRun._polyVerticesBuildList[count - 1];
		if (Mathf.Abs(vector.x - vector2.x) < 0.01f && Mathf.Abs(vector.y - vector2.y) < 0.01f)
		{
			MusicRun._polyVerticesBuildList.RemoveAt(count - 1);
		}
	}

	// Token: 0x06000D50 RID: 3408 RVA: 0x0006A390 File Offset: 0x00068590
	private void AddLocalRunVerticesInRange(Range worldRange)
	{
		Vector3 position = base.transform.position;
		Vector3 vector = base.transform.InverseTransformPoint(new Vector3(worldRange.min, position.y, position.z));
		Vector3 vector2 = base.transform.InverseTransformPoint(new Vector3(worldRange.max, position.y, position.z));
		for (int i = 0; i < this._originalRunLocalVertices.Count; i++)
		{
			Vector2 vector3 = this._originalRunLocalVertices[i];
			if (vector3.x >= vector.x)
			{
				if (vector3.x > vector2.x)
				{
					break;
				}
				MusicRun._polyVerticesBuildList.Add(vector3);
				if (i == 0)
				{
					this.CheckAndRemoveDuplicateVert();
				}
			}
		}
	}

	// Token: 0x06000D51 RID: 3409 RVA: 0x0006A448 File Offset: 0x00068648
	public void RefreshPolyAroundChunks()
	{
		MusicRun._polyVerticesBuildList.Clear();
		MusicRun._polyVerticesBuildList.Add(base.transform.InverseTransformPoint(this._extentLeftPoint));
		if (this.chunks.Count > 0 && !this.connectedToTrackWest)
		{
			float x = this.westChunk.GetConnectorEast(false).point.x;
			this.AddLocalRunVerticesInRange(new Range(this._extentLeftPoint.x, x));
		}
		for (int i = 0; i < this.chunks.Count; i++)
		{
			List<Vector2> underbellyVerticesLocalToMusicRun = this.chunks[i].underbellyVerticesLocalToMusicRun;
			int num = 0;
			int num2 = Mathf.Max(MusicRun._polyVerticesBuildList.Count - 4, 0);
			int num3 = Mathf.Min(underbellyVerticesLocalToMusicRun.Count - 1, 3);
			int num4 = -1;
			int num5 = -1;
			Vector2 vector = default(Vector2);
			Vector2 vector2 = default(Vector2);
			for (int j = num2; j < MusicRun._polyVerticesBuildList.Count - 1; j++)
			{
				int num6 = j + 1;
				Line line = new Line(MusicRun._polyVerticesBuildList[j], MusicRun._polyVerticesBuildList[num6]);
				for (int k = 0; k < num3; k++)
				{
					int num7 = k + 1;
					Line line2 = new Line(underbellyVerticesLocalToMusicRun[k], underbellyVerticesLocalToMusicRun[num7]);
					Vector2 vector3;
					if (Line.LineIntersectionPoint(line, line2, out vector3, true, true))
					{
						if (num4 == -1 || j < num4)
						{
							num4 = j;
							vector = vector3;
						}
						if (num5 == -1 || num7 > num5)
						{
							num5 = num7;
							vector2 = vector3;
						}
					}
				}
			}
			if (num4 != -1)
			{
				Vector3 vector4 = base.transform.TransformPoint(vector);
				Debug.DrawLine(vector4, vector4 + Vector3.up, Color.red, 5f);
				int num8 = MusicRun._polyVerticesBuildList.Count - num4 - 1;
				MusicRun._polyVerticesBuildList.RemoveRange(num4 + 1, num8);
				MusicRun._polyVerticesBuildList.Add(vector);
				this.CheckAndRemoveDuplicateVert();
			}
			if (num5 != -1)
			{
				Vector3 vector5 = base.transform.TransformPoint(vector2);
				Debug.DrawLine(vector5, vector5 + Vector3.up, Color.magenta, 5f);
				num = num5;
				MusicRun._polyVerticesBuildList.Add(vector2);
				this.CheckAndRemoveDuplicateVert();
			}
			for (int l = num; l < underbellyVerticesLocalToMusicRun.Count; l++)
			{
				Vector2 vector6 = underbellyVerticesLocalToMusicRun[l];
				MusicRun._polyVerticesBuildList.Add(vector6);
				if (l == num)
				{
					this.CheckAndRemoveDuplicateVert();
				}
			}
		}
		if (this.chunks.Count > 0 && !this.connectedToTrackEast)
		{
			float x2 = this.eastChunk.GetConnectorEast(true).point.x;
			this.AddLocalRunVerticesInRange(new Range(x2, this._extentRightPoint.x));
		}
		if (this.chunks.Count == 0)
		{
			MusicRun._polyVerticesBuildList.AddRange(this._originalRunLocalVertices);
		}
		MusicRun._polyVerticesBuildList.Add(base.transform.InverseTransformPoint(this._extentRightPoint));
		MusicRun._polyVerticesBuildList.AddRange(this._originalBackgroundLocalVertices);
		this.poly.polygon.vertices = MusicRun._polyVerticesBuildList.ToArray();
		this.poly.FastRefresh();
		MusicRun._polyVerticesBuildList.Clear();
	}

	// Token: 0x06000D52 RID: 3410 RVA: 0x0006A780 File Offset: 0x00068980
	public Vector3 FindPositionOnSurfaceNorm(float posNorm)
	{
		float num = this._localRange.Lerp(posNorm);
		for (int i = 0; i < this._originalRunLocalVertices.Count - 1; i++)
		{
			Vector2 vector = this._originalRunLocalVertices[i];
			Vector2 vector2 = this._originalRunLocalVertices[i + 1];
			if (num >= vector.x && num <= vector2.x)
			{
				float num2 = Mathf.InverseLerp(vector.x, vector2.x, num);
				Vector2 vector3 = Vector2.Lerp(vector, vector2, num2);
				return base.transform.TransformPoint(vector3);
			}
		}
		return base.transform.TransformPoint(this._originalRunLocalVertices[this._originalRunLocalVertices.Count - 1]);
	}

	// Token: 0x17000318 RID: 792
	// (get) Token: 0x06000D53 RID: 3411 RVA: 0x0006A83A File Offset: 0x00068A3A
	public List<Splat> staticFadingSplats
	{
		get
		{
			this.FindStaticFadingSplatsIfNecessary();
			return this._staticFadingSplats;
		}
	}

	// Token: 0x06000D54 RID: 3412 RVA: 0x0006A848 File Offset: 0x00068A48
	public void FindStaticFadingSplatsIfNecessary()
	{
		if (this._staticFadingSplats != null && this._staticFadingSplats.Count > 0)
		{
			return;
		}
		this._staticFadingSplats = new List<Splat>(256);
		base.GetComponentsInChildren<Splat>(this._staticFadingSplats);
		if (this._staticFadingSplats.Count == 0)
		{
			return;
		}
		this._staticFadingSplats.RemoveAllAnd((Splat splat) => !splat.musicRunFadeSplat, null);
		if (this._staticFadingSplats.Count == 0)
		{
			return;
		}
		foreach (Splat splat2 in this._staticFadingSplats)
		{
			splat2.CalculateXRangeForMusicRunFading();
		}
	}

	// Token: 0x06000D55 RID: 3413 RVA: 0x0006A914 File Offset: 0x00068B14
	private void Start()
	{
		this.CacheOriginalVertices();
		this.musicRunFlocks = base.GetComponentsInChildren<MusicRunFlock>();
	}

	// Token: 0x06000D56 RID: 3414 RVA: 0x0006A928 File Offset: 0x00068B28
	private void CacheOriginalVertices()
	{
		MusicRun.Extents extents = this.FindExtents();
		this._extentLeftPoint = extents.leftPoint;
		this._extentRightPoint = extents.rightPoint;
		this._originalRunLocalVertices.Clear();
		this._originalBackgroundLocalVertices.Clear();
		int num;
		if (extents.vertexIndexBeforeRun < extents.vertexIndexAfterRun)
		{
			num = extents.vertexIndexAfterRun - extents.vertexIndexBeforeRun - 1;
		}
		else
		{
			int num2 = extents.vertexIndexBeforeRun - extents.vertexIndexAfterRun + 1;
			num = this.poly.vertexCount - num2;
		}
		for (int i = 0; i < this.poly.vertexCount; i++)
		{
			int num3 = (extents.vertexIndexBeforeRun + i + 1 + this.poly.vertexCount) % this.poly.vertexCount;
			Vector3 vector = this.poly.PointIdx(num3);
			if (Vector2.Distance(vector, this._extentLeftPoint) >= 0.1f && Vector2.Distance(vector, this._extentRightPoint) >= 0.1f)
			{
				Vector2 vector2 = this.poly.polygon.vertices[num3];
				if (i < num)
				{
					this._originalRunLocalVertices.Add(vector2);
				}
				else
				{
					this._originalBackgroundLocalVertices.Add(vector2);
				}
			}
		}
	}

	// Token: 0x04001029 RID: 4137
	[Disable]
	public Slope slope = Presume<Slope>.NonNull("C:\\Users\\josep\\Desktop\\highland-song\\Highland Run\\Assets\\Scripts\\World\\MusicRun.cs", 16);

	// Token: 0x0400102A RID: 4138
	public string inkName = "";

	// Token: 0x0400102B RID: 4139
	[SerializeField]
	private Range _localRange = new Range(-100f, 100f);

	// Token: 0x0400102C RID: 4140
	public bool musicContinuesEast;

	// Token: 0x0400102D RID: 4141
	public bool musicContinuesWest;

	// Token: 0x0400102E RID: 4142
	[NonSerialized]
	public bool connectedToTrackEast;

	// Token: 0x0400102F RID: 4143
	[NonSerialized]
	public bool connectedToTrackWest;

	// Token: 0x04001030 RID: 4144
	[NonSerialized]
	public bool chunkTestingMode;

	// Token: 0x04001031 RID: 4145
	public List<Chunk> chunks = new List<Chunk>(32);

	// Token: 0x04001032 RID: 4146
	[Nullable(2)]
	private Poly _poly;

	// Token: 0x04001034 RID: 4148
	[Nullable(new byte[] { 2, 1 })]
	private List<Splat> _staticFadingSplats;

	// Token: 0x04001035 RID: 4149
	private List<Vector2> _originalRunLocalVertices = new List<Vector2>();

	// Token: 0x04001036 RID: 4150
	private List<Vector2> _originalBackgroundLocalVertices = new List<Vector2>();

	// Token: 0x04001037 RID: 4151
	private static List<Vector2> _polyVerticesBuildList = new List<Vector2>();

	// Token: 0x04001038 RID: 4152
	private Vector2 _extentLeftPoint;

	// Token: 0x04001039 RID: 4153
	private Vector2 _extentRightPoint;

	// Token: 0x020003AE RID: 942
	[NullableContext(0)]
	public struct Extents
	{
		// Token: 0x040019BD RID: 6589
		public Vector2 leftPoint;

		// Token: 0x040019BE RID: 6590
		public Vector2 rightPoint;

		// Token: 0x040019BF RID: 6591
		public int vertexIndexBeforeRun;

		// Token: 0x040019C0 RID: 6592
		public int vertexIndexAfterRun;
	}
}
