using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200019D RID: 413
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(GuidComponent))]
[ExecuteInEditMode]
[DisallowMultipleComponent]
[SelectionBase]
public class Poly : BasePolyAndSlope
{
	// Token: 0x1700031A RID: 794
	// (get) Token: 0x06000D64 RID: 3428 RVA: 0x0006AE42 File Offset: 0x00069042
	// (set) Token: 0x06000D65 RID: 3429 RVA: 0x0006AE4A File Offset: 0x0006904A
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
				this.SetWorldPoints(this.polygon.vertices);
			}
		}
	}

	// Token: 0x1700031B RID: 795
	// (get) Token: 0x06000D66 RID: 3430 RVA: 0x0006AE77 File Offset: 0x00069077
	public int levelDepthIdx
	{
		get
		{
			if (this._hasLevelDepthIdx)
			{
				return this._levelDepthIdx;
			}
			return Level.DepthToIndex(base.transform.position.z);
		}
	}

	// Token: 0x1700031C RID: 796
	// (get) Token: 0x06000D67 RID: 3431 RVA: 0x0006AE9D File Offset: 0x0006909D
	// (set) Token: 0x06000D68 RID: 3432 RVA: 0x0006AEA5 File Offset: 0x000690A5
	public Polygon polygon
	{
		get
		{
			return this._polygon;
		}
		set
		{
			this.SetPolygon(value, true);
		}
	}

	// Token: 0x06000D69 RID: 3433 RVA: 0x0006AEAF File Offset: 0x000690AF
	public void SetPolygon(Polygon newPoly, bool refresh)
	{
		this._polygon = newPoly;
		if (this._isStatic)
		{
			this.SetWorldPoints(this._polygon.vertices);
		}
		if (refresh)
		{
			this.FullRefresh();
		}
	}

	// Token: 0x06000D6A RID: 3434 RVA: 0x0006AEDC File Offset: 0x000690DC
	public List<Vector3> SetWorldPoints(Vector2[] localPoints)
	{
		int num = localPoints.Length;
		if (this._worldPoints == null)
		{
			this._worldPoints = new List<Vector3>(num);
		}
		else if (this._worldPoints.Capacity < num)
		{
			this._worldPoints.Capacity = num;
		}
		foreach (Vector2 vector in localPoints)
		{
			this._worldPoints.Add(base.transform.TransformPoint(vector));
		}
		return this._worldPoints;
	}

	// Token: 0x1700031D RID: 797
	// (get) Token: 0x06000D6B RID: 3435 RVA: 0x0006AF57 File Offset: 0x00069157
	// (set) Token: 0x06000D6C RID: 3436 RVA: 0x0006AF5F File Offset: 0x0006915F
	public Color color
	{
		get
		{
			return this._color;
		}
		set
		{
			this._color = value;
			this.RefreshPropertyBlock();
		}
	}

	// Token: 0x1700031E RID: 798
	// (get) Token: 0x06000D6D RID: 3437 RVA: 0x0006AF6E File Offset: 0x0006916E
	public Mesh mesh
	{
		get
		{
			return this._mesh;
		}
	}

	// Token: 0x1700031F RID: 799
	// (get) Token: 0x06000D6E RID: 3438 RVA: 0x0006AF78 File Offset: 0x00069178
	public static Material material
	{
		get
		{
			if (Poly._material == null)
			{
				if (Application.isPlaying)
				{
					Poly._material = Resources.Load<Material>("PolyMaterial");
				}
				if (Poly._material == null)
				{
					Debug.LogError("PolyMaterial not found!");
				}
			}
			return Poly._material;
		}
	}

	// Token: 0x17000320 RID: 800
	// (get) Token: 0x06000D6F RID: 3439 RVA: 0x0006AFC4 File Offset: 0x000691C4
	public MusicRun musicRun
	{
		get
		{
			if (!this._musicRunSet || !Application.isPlaying)
			{
				this._musicRun = base.GetComponent<MusicRun>();
				this._musicRunSet = true;
			}
			return this._musicRun;
		}
	}

	// Token: 0x06000D70 RID: 3440 RVA: 0x0006AFEE File Offset: 0x000691EE
	public bool EdgeIsClimbable(int i)
	{
		return !this.sceneryOnly && (this.edgeFlags != null && i < this.edgeFlags.Count) && (this.edgeFlags[i] & Poly.EdgeFlags.Climbable) > Poly.EdgeFlags.None;
	}

	// Token: 0x06000D71 RID: 3441 RVA: 0x0006B023 File Offset: 0x00069223
	public bool EdgeIsCollidable(int i)
	{
		return !this.sceneryOnly && (this.edgeFlags == null || i >= this.edgeFlags.Count || (this.edgeFlags[i] & Poly.EdgeFlags.Collidable) > Poly.EdgeFlags.None);
	}

	// Token: 0x06000D72 RID: 3442 RVA: 0x0006B058 File Offset: 0x00069258
	public bool EdgeIsUnreachable(int i)
	{
		return !this.sceneryOnly && (this.edgeFlags == null || i >= this.edgeFlags.Count || (this.edgeFlags[i] & Poly.EdgeFlags.Unreachable) > Poly.EdgeFlags.None);
	}

	// Token: 0x06000D73 RID: 3443 RVA: 0x0006B08D File Offset: 0x0006928D
	public bool EdgeIsSlideable(int i)
	{
		return !this.sceneryOnly && this.edgeFlags != null && i < this.edgeFlags.Count && (this.edgeFlags[i] & Poly.EdgeFlags.Slideable) > Poly.EdgeFlags.None;
	}

	// Token: 0x06000D74 RID: 3444 RVA: 0x0006B0C4 File Offset: 0x000692C4
	public static bool EdgeRunnableOrSlidable(Vector2 v0, Vector2 v1, bool slideable, bool clockwise, Poly context = null)
	{
		bool flag = false;
		if ((v1.x > v0.x) ^ !clockwise)
		{
			float num = Slope.AngleWithVector(clockwise ? (v1 - v0) : (v0 - v1), false, context);
			float num2 = (slideable ? Poly.runnerSettings.run.maxSlideGroundAngle : Poly.runnerSettings.run.maxGroundAngle);
			flag = Mathf.Abs(num) < num2;
		}
		return flag;
	}

	// Token: 0x06000D75 RID: 3445 RVA: 0x0006B134 File Offset: 0x00069334
	public void SetEdgeIsClimbable(int i, bool climbable)
	{
		this.ValidateEdgeFlagsListSize();
		List<Poly.EdgeFlags> list;
		if (climbable)
		{
			list = this.edgeFlags;
			list[i] |= Poly.EdgeFlags.Climbable;
			return;
		}
		list = this.edgeFlags;
		list[i] &= ~Poly.EdgeFlags.Climbable;
	}

	// Token: 0x06000D76 RID: 3446 RVA: 0x0006B180 File Offset: 0x00069380
	public void SetEdgeIsUnreachable(int i, bool unreachable)
	{
		this.ValidateEdgeFlagsListSize();
		List<Poly.EdgeFlags> list;
		if (unreachable)
		{
			list = this.edgeFlags;
			list[i] |= Poly.EdgeFlags.Unreachable;
			return;
		}
		list = this.edgeFlags;
		list[i] &= ~Poly.EdgeFlags.Unreachable;
	}

	// Token: 0x06000D77 RID: 3447 RVA: 0x0006B1CC File Offset: 0x000693CC
	public void SetEdgeIsCollidable(int i, bool collidable)
	{
		this.ValidateEdgeFlagsListSize();
		List<Poly.EdgeFlags> list;
		if (collidable)
		{
			list = this.edgeFlags;
			list[i] |= Poly.EdgeFlags.Collidable;
			return;
		}
		list = this.edgeFlags;
		list[i] &= ~Poly.EdgeFlags.Collidable;
	}

	// Token: 0x06000D78 RID: 3448 RVA: 0x0006B218 File Offset: 0x00069418
	public void SetEdgeIsSlideable(int i, bool slideable)
	{
		this.ValidateEdgeFlagsListSize();
		List<Poly.EdgeFlags> list;
		if (slideable)
		{
			list = this.edgeFlags;
			list[i] |= Poly.EdgeFlags.Slideable;
			return;
		}
		list = this.edgeFlags;
		list[i] &= ~Poly.EdgeFlags.Slideable;
	}

	// Token: 0x06000D79 RID: 3449 RVA: 0x0006B264 File Offset: 0x00069464
	private void ValidateEdgeFlagsListSize()
	{
		if (this.edgeFlags == null)
		{
			this.edgeFlags = new List<Poly.EdgeFlags>(this.vertexCount);
		}
		while (this.edgeFlags.Count < this.vertexCount)
		{
			this.edgeFlags.Add(Poly.EdgeFlags.Collidable);
		}
		if (this.edgeFlags.Count > this.vertexCount)
		{
			this.edgeFlags.RemoveRange(this.vertexCount, this.edgeFlags.Count - this.vertexCount);
		}
	}

	// Token: 0x17000321 RID: 801
	// (get) Token: 0x06000D7A RID: 3450 RVA: 0x0006B2E1 File Offset: 0x000694E1
	public bool invisible
	{
		get
		{
			return this._invisible;
		}
	}

	// Token: 0x17000322 RID: 802
	// (get) Token: 0x06000D7B RID: 3451 RVA: 0x0006B2E9 File Offset: 0x000694E9
	public MeshRenderer meshRenderer
	{
		get
		{
			return this._meshRenderer;
		}
	}

	// Token: 0x17000323 RID: 803
	// (get) Token: 0x06000D7C RID: 3452 RVA: 0x0006B2F1 File Offset: 0x000694F1
	public MeshFilter MeshFilter
	{
		get
		{
			return this.meshFilter;
		}
	}

	// Token: 0x17000324 RID: 804
	// (get) Token: 0x06000D7D RID: 3453 RVA: 0x0006B2F9 File Offset: 0x000694F9
	public bool isCutawayWall
	{
		get
		{
			return this._isCutawayWall;
		}
	}

	// Token: 0x17000325 RID: 805
	// (get) Token: 0x06000D7E RID: 3454 RVA: 0x0006B301 File Offset: 0x00069501
	public bool cutawayWallFullyHidden
	{
		get
		{
			return this._isCutawayWall && this._cutawayAlpha <= 0f;
		}
	}

	// Token: 0x17000326 RID: 806
	// (get) Token: 0x06000D7F RID: 3455 RVA: 0x0006B31D File Offset: 0x0006951D
	// (set) Token: 0x06000D80 RID: 3456 RVA: 0x0006B328 File Offset: 0x00069528
	public float cutawayAlpha
	{
		get
		{
			return this._cutawayAlpha;
		}
		set
		{
			if (this._cutawayAlpha == value)
			{
				return;
			}
			this._cutawayAlpha = value;
			this.UpdateVisibility();
			this.RefreshPropertyBlock();
			foreach (Splat splat in this.splats)
			{
				splat.RefreshFadeDueToPolyCutaway();
			}
		}
	}

	// Token: 0x06000D81 RID: 3457 RVA: 0x0006B398 File Offset: 0x00069598
	public Vector3 PointIdx(int idx)
	{
		if (this._isStatic)
		{
			return this._worldPoints[idx];
		}
		return base.transform.TransformPoint(this.polygon.vertices[idx]);
	}

	// Token: 0x17000327 RID: 807
	// (get) Token: 0x06000D82 RID: 3458 RVA: 0x0006B3D0 File Offset: 0x000695D0
	public int vertexCount
	{
		get
		{
			return this.polygon.VertCount;
		}
	}

	// Token: 0x17000328 RID: 808
	// (get) Token: 0x06000D83 RID: 3459 RVA: 0x0006B3E0 File Offset: 0x000695E0
	public Range xRange
	{
		get
		{
			Range range = new Range(float.MaxValue, float.MinValue);
			for (int i = 0; i < this.vertexCount; i++)
			{
				Vector3 vector = this.PointIdx(i);
				if (vector.x < range.min)
				{
					range.min = vector.x;
				}
				if (vector.x > range.max)
				{
					range.max = vector.x;
				}
			}
			return range;
		}
	}

	// Token: 0x06000D84 RID: 3460 RVA: 0x0006B450 File Offset: 0x00069650
	private void ValidateGUID()
	{
		bool flag = false;
		if (this.guidComponent == null)
		{
			flag = true;
			this.guidComponent = base.GetComponent<GuidComponent>();
		}
		if (this.guidComponent.didJustAssignNewGUID || flag)
		{
			this.OnSetGUID();
			return;
		}
		GuidComponent guidComponent = this.guidComponent;
		guidComponent.OnSetGUID = (Action)Delegate.Combine(guidComponent.OnSetGUID, new Action(this.OnSetGUID));
	}

	// Token: 0x06000D85 RID: 3461 RVA: 0x0006B4B8 File Offset: 0x000696B8
	private void OnSetGUID()
	{
		this.InitialSetUp();
	}

	// Token: 0x06000D86 RID: 3462 RVA: 0x0006B4C0 File Offset: 0x000696C0
	private void InitialSetUp()
	{
		this.clearSharedMeshOnAwake = true;
		this.guidComponent.hideFlags = HideFlags.HideInInspector;
		this.meshFilter = base.GetComponent<MeshFilter>();
		this.meshFilter.hideFlags = HideFlags.HideInInspector;
		this._meshRenderer = base.GetComponent<MeshRenderer>();
		this._meshRenderer.hideFlags = HideFlags.HideInInspector;
		if (this._meshRenderer.sharedMaterial == null)
		{
			this._meshRenderer.material = Poly.material;
		}
	}

	// Token: 0x06000D87 RID: 3463 RVA: 0x0006B533 File Offset: 0x00069733
	public void OnAddedToLevel(Level level, bool isStatic)
	{
		this._levelDepthIdx = level.levelIdx;
		this._hasLevelDepthIdx = true;
		this.isStatic = isStatic;
	}

	// Token: 0x06000D88 RID: 3464 RVA: 0x0006B54F File Offset: 0x0006974F
	private void OnEnable()
	{
		this.ValidateGUID();
		this.InitMesh();
		this.UpdateVisibility();
		this.RefreshPropertyBlock();
		this.SetAllChildSplats();
	}

	// Token: 0x06000D89 RID: 3465 RVA: 0x0006B570 File Offset: 0x00069770
	private void OnDisable()
	{
		this._hasLevelDepthIdx = false;
		this._musicRun = null;
		this._cutawayAlpha = 1f;
		this._properties = null;
		this._mesh = null;
	}

	// Token: 0x06000D8A RID: 3466 RVA: 0x0006B59C File Offset: 0x0006979C
	public void SetAllChildSplats()
	{
		this.splats.Clear();
		base.GetComponentsInChildren<Splat>(true, this.splats);
		foreach (Splat splat in this.splats)
		{
			if (splat.GetComponentInParent<Poly>() == this)
			{
				splat.OnAddedToPoly(this);
			}
		}
	}

	// Token: 0x06000D8B RID: 3467 RVA: 0x0006B618 File Offset: 0x00069818
	private void InitMesh()
	{
		if (this.clearSharedMeshOnAwake || this.meshFilter.sharedMesh == null)
		{
			this.clearSharedMeshOnAwake = false;
			this._mesh = new Mesh();
			this._mesh.name = string.Format("Poly Mesh {0}", this.guidComponent.GetGuid().ToString());
			this.meshFilter.mesh = this._mesh;
			this.FullRefresh();
		}
		else
		{
			this._mesh = this.meshFilter.sharedMesh;
		}
		this.UpdateVerticesScratch();
	}

	// Token: 0x06000D8C RID: 3468 RVA: 0x0006B6B0 File Offset: 0x000698B0
	public void UpdateCutawayAlpha()
	{
		float num = 1f;
		float num2 = 1f;
		Vector3 position = Runner.instance.transform.position;
		Vector2 vector = position;
		Bounds bounds = this.meshRenderer.bounds;
		Vector3 min = bounds.min;
		Vector3 max = bounds.max;
		Rect rect = Rect.MinMaxRect(min.x - num2, min.y - num2, max.x + num2, max.y + num2 + 0.01f);
		Range range = new Range(-0.5f, 15f) + base.transform.position.z;
		if (rect.Contains(position) && range.Contains(position.z) && (!Game.instance.looking || CaveRegion.inCave) && !Game.instance.lookingFurther)
		{
			Vector3 vector2 = base.transform.worldToLocalMatrix.MultiplyPoint(position);
			bool flag = this.polygon.ContainsPoint(vector2);
			vector2 = this.polygon.FindClosestPointOnPolygon(vector2, true);
			Vector3 vector3 = base.transform.localToWorldMatrix.MultiplyPoint3x4(vector2);
			float num3 = Vector2.Distance(vector, vector3);
			if (flag)
			{
				num3 = -num3;
			}
			if (num3 < num2)
			{
				num = 0f;
			}
		}
		if (this._cutawayAlpha != num)
		{
			bool cutawayWallFullyHidden = this.cutawayWallFullyHidden;
			this.cutawayAlpha = Mathf.MoveTowards(this._cutawayAlpha, num, 2f * Time.deltaTime);
			if (this.cutawayWallFullyHidden != cutawayWallFullyHidden && this._additionalObjectsToHideWithCutaway != null)
			{
				foreach (GameObject gameObject in this._additionalObjectsToHideWithCutaway)
				{
					gameObject.SetActive(!this.cutawayWallFullyHidden);
				}
			}
		}
	}

	// Token: 0x06000D8D RID: 3469 RVA: 0x0006B8A0 File Offset: 0x00069AA0
	public void FullRefresh()
	{
		this.FastRefresh();
		this.RefreshSlideables();
		this.RefreshClimbables();
		foreach (Splat splat in this.splats)
		{
			if (splat.enabled)
			{
				splat.OnPolyRefresh();
			}
		}
	}

	// Token: 0x06000D8E RID: 3470 RVA: 0x0006B90C File Offset: 0x00069B0C
	public void FastRefresh()
	{
		if (this._polygon == null)
		{
			return;
		}
		if (this._mesh == null)
		{
			Debug.LogError("Poly: Mesh shouldn't be null here! Fixing. " + base.gameObject.name, this);
			this.InitMesh();
			return;
		}
		this.UpdateVerticesScratch();
		this._mesh.Clear();
		this._mesh.SetVertices(this._verticesScratch);
		if (this._trianglesScratch == null)
		{
			this._trianglesScratch = new List<int>(32);
		}
		else
		{
			this._trianglesScratch.Clear();
		}
		Triangulator.GenerateIndices(this._verticesScratch, this._trianglesScratch);
		this._mesh.SetTriangles(this._trianglesScratch, 0);
		this._mesh.RecalculateNormals();
		this._mesh.RecalculateBounds();
		this.UpdateVisibility();
		this.RefreshPropertyBlock();
	}

	// Token: 0x17000329 RID: 809
	// (get) Token: 0x06000D8F RID: 3471 RVA: 0x0006B9E2 File Offset: 0x00069BE2
	private static ClimbSettings climbSettings
	{
		get
		{
			if (Poly._climbSettings == null && Runner.instance != null)
			{
				Poly._climbSettings = Runner.instance.settings.climb;
			}
			return Poly._climbSettings;
		}
	}

	// Token: 0x06000D90 RID: 3472 RVA: 0x0006BA18 File Offset: 0x00069C18
	public void RefreshClimbables()
	{
		if (this.sceneryOnly)
		{
			for (int i = 0; i < this.polygon.vertices.Length; i++)
			{
				this.SetEdgeIsClimbable(i, false);
				this.SetEdgeIsUnreachable(i, false);
			}
			return;
		}
		Climbable[] componentsInChildren = base.GetComponentsInChildren<Climbable>();
		bool isClockwise = this.polygon.isClockwise;
		ClimbSettings climbSettings = Poly.climbSettings;
		Vector3 vector = this.PointIdx(0);
		Vector3 vector2 = vector;
		for (int j = 0; j < this.polygon.vertices.Length; j++)
		{
			vector = vector2;
			this.SetEdgeIsClimbable(j, false);
			this.SetEdgeIsUnreachable(j, false);
			int num = (j + 1) % this.polygon.vertices.Length;
			vector2 = this.PointIdx(num);
			if (!Poly.EdgeRunnableOrSlidable(vector, vector2, this.EdgeIsSlideable(j), isClockwise, this))
			{
				bool flag = this.allClimbable;
				bool flag2 = false;
				foreach (Climbable climbable in componentsInChildren)
				{
					Vector3 vector3 = climbable.transform.InverseTransformPoint(vector);
					Vector3 vector4 = climbable.transform.InverseTransformPoint(vector2);
					if (Line.GetClosestDistanceFromLine(vector3, vector4, Vector2.zero) < 1f)
					{
						flag = climbable.climbable && !climbable.unreachable;
						flag2 = climbable.unreachable;
						break;
					}
				}
				this.SetEdgeIsUnreachable(j, flag2);
				if (flag && Util.ClimbAngleFromSurfacePolyEdge(vector2 - vector) <= climbSettings.maxOverhangAngle)
				{
					this.SetEdgeIsClimbable(j, true);
				}
			}
		}
	}

	// Token: 0x06000D91 RID: 3473 RVA: 0x0006BBAC File Offset: 0x00069DAC
	public void RefreshSlideables()
	{
		for (int i = 0; i < this.polygon.vertices.Length; i++)
		{
			this.SetEdgeIsSlideable(i, false);
		}
		if (this.sceneryOnly)
		{
			return;
		}
		Slideable[] componentsInChildren = base.transform.GetComponentsInChildren<Slideable>();
		bool[] array = new bool[this.polygon.vertices.Length];
		for (int j = 0; j < this.polygon.vertices.Length; j++)
		{
			array[j] = false;
			Vector3 vector = this.PointIdx(j);
			Slideable[] array2 = componentsInChildren;
			for (int k = 0; k < array2.Length; k++)
			{
				if (array2[k].transform.InverseTransformPoint(vector).magnitude < 1f)
				{
					array[j] = true;
					break;
				}
			}
		}
		for (int l = 0; l < this.polygon.vertices.Length; l++)
		{
			int num = (l + 1) % this.polygon.vertices.Length;
			Vector3 vector2 = this.PointIdx(l);
			Vector3 vector3 = this.PointIdx(num);
			bool flag = array[l] && array[num];
			bool flag2 = Poly.EdgeRunnableOrSlidable(vector2, vector3, flag, true, this);
			this.SetEdgeIsSlideable(l, flag2 && flag);
		}
	}

	// Token: 0x06000D92 RID: 3474 RVA: 0x0006BCE0 File Offset: 0x00069EE0
	private void UpdateVerticesScratch()
	{
		Vector2[] vertices = this.polygon.vertices;
		if (this._verticesScratch == null)
		{
			this._verticesScratch = new List<Vector3>(vertices.Length);
		}
		else
		{
			this._verticesScratch.Clear();
		}
		for (int i = 0; i < vertices.Length; i++)
		{
			this._verticesScratch.Add(vertices[i]);
		}
	}

	// Token: 0x06000D93 RID: 3475 RVA: 0x0006BD44 File Offset: 0x00069F44
	public void RefreshMeshVerticesWithoutTriangulation()
	{
		if (this._verticesScratch == null)
		{
			Debug.LogError("Poly has null _verticesScratch?", this);
		}
		this.UpdateVerticesScratch();
		this._mesh.SetVertices(this._verticesScratch);
		this._mesh.RecalculateNormals();
		this._mesh.RecalculateBounds();
	}

	// Token: 0x1700032A RID: 810
	// (get) Token: 0x06000D94 RID: 3476 RVA: 0x0006BD91 File Offset: 0x00069F91
	// (set) Token: 0x06000D95 RID: 3477 RVA: 0x0006BD99 File Offset: 0x00069F99
	public bool flattenedAndHidden
	{
		get
		{
			return this._flattendAndHidden;
		}
		set
		{
			if (this._flattendAndHidden != value)
			{
				this._flattendAndHidden = value;
				this.UpdateVisibility();
			}
		}
	}

	// Token: 0x06000D96 RID: 3478 RVA: 0x0006BDB4 File Offset: 0x00069FB4
	public bool UpdateVisibility()
	{
		bool flag = !this._invisible;
		if (this._flattendAndHidden)
		{
			flag = false;
		}
		else if (Application.isPlaying && this.cutawayWallFullyHidden)
		{
			flag = false;
		}
		if (this._meshRenderer.enabled != flag)
		{
			this._meshRenderer.enabled = flag;
			return flag;
		}
		return false;
	}

	// Token: 0x06000D97 RID: 3479 RVA: 0x0006BE08 File Offset: 0x0006A008
	public void RefreshPropertyBlock()
	{
		if (!this.meshRenderer.enabled)
		{
			return;
		}
		if (!Poly._generatedIDs)
		{
			Poly._colorId = Shader.PropertyToID("_Color");
			Poly._mainTexId = Shader.PropertyToID("_MainTex");
			Poly._mainTexSTId = Shader.PropertyToID("_MainTex_ST");
			Poly._fadeId = Shader.PropertyToID("_Fade");
			Poly._generatedIDs = true;
		}
		if (this._properties == null)
		{
			this._properties = new MaterialPropertyBlock();
		}
		this._properties.Clear();
		this._properties.SetColor(Poly._colorId, this._color.WithAlpha(this._color.a * this._cutawayAlpha));
		if (this.baseTexture != null)
		{
			this._properties.SetTexture(Poly._mainTexId, this.baseTexture);
			this._properties.SetVector(Poly._mainTexSTId, new Vector4(1f / this.textureScale, 1f / this.textureScale, -this.textureOffset.x, -this.textureOffset.y));
		}
		else if (!this.useDebugTexture)
		{
			this._properties.SetTexture(Poly._mainTexId, Texture2D.whiteTexture);
		}
		this._properties.SetFloat(Poly._fadeId, 1f - this.cutawayAlpha);
		this.meshRenderer.SetPropertyBlock(this._properties);
	}

	// Token: 0x06000D98 RID: 3480 RVA: 0x0006BF6C File Offset: 0x0006A16C
	private void OnDestroy()
	{
		if (this._mesh != null)
		{
			Poly.DestroyAutomatic(this._mesh);
			this._mesh = null;
		}
	}

	// Token: 0x06000D99 RID: 3481 RVA: 0x0006BF8E File Offset: 0x0006A18E
	private static void DestroyAutomatic(Object o)
	{
		Object.Destroy(o);
	}

	// Token: 0x1700032B RID: 811
	// (get) Token: 0x06000D9A RID: 3482 RVA: 0x0006BF96 File Offset: 0x0006A196
	private static RunnerSettings runnerSettings
	{
		get
		{
			if (!Poly._hasRunnerSettings && Runner.instance != null)
			{
				Poly._runnerSettings = Runner.instance.settings;
				Poly._hasRunnerSettings = Poly._runnerSettings != null;
			}
			return Poly._runnerSettings;
		}
	}

	// Token: 0x04001046 RID: 4166
	[NonSerialized]
	private bool _isStatic;

	// Token: 0x04001047 RID: 4167
	private int _levelDepthIdx;

	// Token: 0x04001048 RID: 4168
	private bool _hasLevelDepthIdx;

	// Token: 0x04001049 RID: 4169
	public bool lockTransform;

	// Token: 0x0400104A RID: 4170
	[SerializeField]
	private Polygon _polygon = new Polygon(new Vector2[]
	{
		new Vector2(-5f, 5f),
		new Vector2(5f, 5f),
		new Vector2(5f, -5f),
		new Vector2(-5f, -5f)
	});

	// Token: 0x0400104B RID: 4171
	[NonSerialized]
	private List<Vector3> _worldPoints;

	// Token: 0x0400104C RID: 4172
	[SerializeField]
	private Color _color = Color.gray;

	// Token: 0x0400104D RID: 4173
	private static Material _material;

	// Token: 0x0400104E RID: 4174
	private MusicRun _musicRun;

	// Token: 0x0400104F RID: 4175
	private bool _musicRunSet;

	// Token: 0x04001050 RID: 4176
	public bool useDebugTexture = true;

	// Token: 0x04001051 RID: 4177
	public Texture2D baseTexture;

	// Token: 0x04001052 RID: 4178
	public float textureScale = 100f;

	// Token: 0x04001053 RID: 4179
	public Vector2 textureOffset = Vector2.zero;

	// Token: 0x04001054 RID: 4180
	public List<Poly.EdgeFlags> edgeFlags;

	// Token: 0x04001055 RID: 4181
	public bool sceneryOnly;

	// Token: 0x04001056 RID: 4182
	[SerializeField]
	private bool _invisible;

	// Token: 0x04001057 RID: 4183
	public bool passThroughWalls;

	// Token: 0x04001058 RID: 4184
	public bool allClimbable = true;

	// Token: 0x04001059 RID: 4185
	[SerializeField]
	private GuidComponent guidComponent;

	// Token: 0x0400105A RID: 4186
	[SerializeField]
	private MeshRenderer _meshRenderer;

	// Token: 0x0400105B RID: 4187
	[SerializeField]
	private MeshFilter meshFilter;

	// Token: 0x0400105C RID: 4188
	[SerializeField]
	private bool _isCutawayWall;

	// Token: 0x0400105D RID: 4189
	private float _cutawayAlpha = 1f;

	// Token: 0x0400105E RID: 4190
	[SerializeField]
	private List<GameObject> _additionalObjectsToHideWithCutaway;

	// Token: 0x0400105F RID: 4191
	private bool clearSharedMeshOnAwake;

	// Token: 0x04001060 RID: 4192
	private static ClimbSettings _climbSettings;

	// Token: 0x04001061 RID: 4193
	private List<Vector3> _verticesScratch;

	// Token: 0x04001062 RID: 4194
	private List<int> _trianglesScratch;

	// Token: 0x04001063 RID: 4195
	private bool _flattendAndHidden;

	// Token: 0x04001064 RID: 4196
	private static RunnerSettings _runnerSettings;

	// Token: 0x04001065 RID: 4197
	private static bool _hasRunnerSettings;

	// Token: 0x04001066 RID: 4198
	private Mesh _mesh;

	// Token: 0x04001067 RID: 4199
	private MaterialPropertyBlock _properties;

	// Token: 0x04001068 RID: 4200
	private static bool _generatedIDs;

	// Token: 0x04001069 RID: 4201
	private static int _colorId;

	// Token: 0x0400106A RID: 4202
	private static int _mainTexId;

	// Token: 0x0400106B RID: 4203
	private static int _mainTexSTId;

	// Token: 0x0400106C RID: 4204
	private static int _fadeId;

	// Token: 0x020003B1 RID: 945
	[Flags]
	public enum EdgeFlags
	{
		// Token: 0x040019C6 RID: 6598
		None = 0,
		// Token: 0x040019C7 RID: 6599
		Collidable = 1,
		// Token: 0x040019C8 RID: 6600
		Climbable = 2,
		// Token: 0x040019C9 RID: 6601
		Unreachable = 4,
		// Token: 0x040019CA RID: 6602
		Slideable = 8,
		// Token: 0x040019CB RID: 6603
		Default = 1
	}
}
