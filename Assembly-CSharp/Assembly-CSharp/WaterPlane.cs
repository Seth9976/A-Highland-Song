using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200005F RID: 95
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(GuidComponent))]
[ExecuteInEditMode]
[DisallowMultipleComponent]
public class WaterPlane : MonoInstancer<WaterPlane>
{
	// Token: 0x170000BE RID: 190
	// (get) Token: 0x0600028B RID: 651 RVA: 0x0001578E File Offset: 0x0001398E
	// (set) Token: 0x0600028C RID: 652 RVA: 0x00015796 File Offset: 0x00013996
	public Polygon polygon
	{
		get
		{
			return this._polygon;
		}
		set
		{
			this._polygon = value;
			this.Refresh();
		}
	}

	// Token: 0x0600028D RID: 653 RVA: 0x000157A8 File Offset: 0x000139A8
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

	// Token: 0x170000BF RID: 191
	// (get) Token: 0x0600028E RID: 654 RVA: 0x00015824 File Offset: 0x00013A24
	private static Material waterMaterial
	{
		get
		{
			if (WaterPlane._waterMaterial == null)
			{
				if (Application.isPlaying)
				{
					WaterPlane._waterMaterial = Resources.Load<Material>("WaterMaterial");
				}
				if (WaterPlane._waterMaterial == null)
				{
					Debug.LogError("WaterMaterial not found!");
				}
			}
			return WaterPlane._waterMaterial;
		}
	}

	// Token: 0x170000C0 RID: 192
	// (get) Token: 0x0600028F RID: 655 RVA: 0x00015870 File Offset: 0x00013A70
	public MeshRenderer meshRenderer
	{
		get
		{
			return this._meshRenderer;
		}
	}

	// Token: 0x170000C1 RID: 193
	// (get) Token: 0x06000290 RID: 656 RVA: 0x00015878 File Offset: 0x00013A78
	public MeshFilter MeshFilter
	{
		get
		{
			return this.meshFilter;
		}
	}

	// Token: 0x06000291 RID: 657 RVA: 0x00015880 File Offset: 0x00013A80
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

	// Token: 0x06000292 RID: 658 RVA: 0x000158E8 File Offset: 0x00013AE8
	private void OnSetGUID()
	{
		this.InitialSetUp();
	}

	// Token: 0x06000293 RID: 659 RVA: 0x000158F0 File Offset: 0x00013AF0
	private void InitialSetUp()
	{
		this.clearSharedMeshOnAwake = true;
		base.gameObject.layer = LayerMask.NameToLayer("Water");
		this.guidComponent.hideFlags = HideFlags.HideInInspector;
		this.meshFilter = base.GetComponent<MeshFilter>();
		this.meshFilter.hideFlags = HideFlags.HideInInspector;
		this._meshRenderer = base.GetComponent<MeshRenderer>();
		this._meshRenderer.hideFlags = HideFlags.HideInInspector;
		this._meshRenderer.material = WaterPlane.waterMaterial;
	}

	// Token: 0x06000294 RID: 660 RVA: 0x00015965 File Offset: 0x00013B65
	protected override void OnEnable()
	{
		base.OnEnable();
		this.ValidateGUID();
		this.InitMesh();
		this.RemoveLowQualityReflectionProperties();
	}

	// Token: 0x06000295 RID: 661 RVA: 0x0001597F File Offset: 0x00013B7F
	protected override void OnDisable()
	{
		this._properties = null;
		this._mesh = null;
	}

	// Token: 0x06000296 RID: 662 RVA: 0x00015990 File Offset: 0x00013B90
	private void InitMesh()
	{
		if (this.clearSharedMeshOnAwake || this.meshFilter.sharedMesh == null)
		{
			this.clearSharedMeshOnAwake = false;
			this._mesh = new Mesh();
			this._mesh.name = string.Format("Water Plane Mesh {0}", this.guidComponent.GetGuid().ToString());
			this.meshFilter.mesh = this._mesh;
			this.Refresh();
		}
		else
		{
			this._mesh = this.meshFilter.sharedMesh;
		}
		this.UpdateVerticesScratch();
	}

	// Token: 0x06000297 RID: 663 RVA: 0x00015A28 File Offset: 0x00013C28
	public void Refresh()
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
		Triangulator.GenerateIndices(this.polygon.vertices, this._trianglesScratch);
		this._mesh.SetTriangles(this._trianglesScratch, 0);
		this.bounds = new Bounds(this.meshRenderer.bounds.center, new Vector3(this.meshRenderer.bounds.size.x, 0.01f, this.meshRenderer.bounds.size.z));
	}

	// Token: 0x06000298 RID: 664 RVA: 0x00015B38 File Offset: 0x00013D38
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
			this._verticesScratch.Add(new Vector3(vertices[i].x, 0f, vertices[i].y));
		}
	}

	// Token: 0x06000299 RID: 665 RVA: 0x00015BB0 File Offset: 0x00013DB0
	public void SetLowQualityWaterProperties(Vector2 offset, float yScale, RenderTexture tex)
	{
		if (this._properties == null)
		{
			this._properties = new MaterialPropertyBlock();
		}
		this._properties.SetFloat("_ReflectionYScale", yScale);
		this._properties.SetVector("_ScreenspaceReflectionOffset", offset);
		if (tex != null)
		{
			this._properties.SetTexture("_ReflectionTex", tex);
		}
		this.meshRenderer.SetPropertyBlock(this._properties);
	}

	// Token: 0x0600029A RID: 666 RVA: 0x00015C22 File Offset: 0x00013E22
	public void SetHighQualityReflectionTex(RenderTexture tex)
	{
		if (this._properties == null)
		{
			this._properties = new MaterialPropertyBlock();
		}
		this._properties.SetTexture("_ReflectionTex", tex);
		this.meshRenderer.SetPropertyBlock(this._properties);
	}

	// Token: 0x0600029B RID: 667 RVA: 0x00015C5C File Offset: 0x00013E5C
	public void RemoveLowQualityReflectionProperties()
	{
		if (this._properties == null)
		{
			this._properties = new MaterialPropertyBlock();
		}
		this._properties.SetFloat("_ReflectionYScale", 1f);
		this._properties.SetVector("_ScreenspaceReflectionOffset", Vector2.zero);
		this.meshRenderer.SetPropertyBlock(this._properties);
	}

	// Token: 0x0600029C RID: 668 RVA: 0x00015CBC File Offset: 0x00013EBC
	public void SetDistanceFadeScalar(float v)
	{
		if (this._properties == null)
		{
			this._properties = new MaterialPropertyBlock();
		}
		this._properties.SetFloat("_DistanceFadeScalar", v);
		this.meshRenderer.SetPropertyBlock(this._properties);
	}

	// Token: 0x0600029D RID: 669 RVA: 0x00015CF3 File Offset: 0x00013EF3
	private void OnDestroy()
	{
		if (this._mesh != null)
		{
			WaterPlane.DestroyAutomatic(this._mesh);
			this._mesh = null;
		}
	}

	// Token: 0x0600029E RID: 670 RVA: 0x00015D15 File Offset: 0x00013F15
	private static void DestroyAutomatic(Object o)
	{
		Object.Destroy(o);
	}

	// Token: 0x040003CD RID: 973
	[SerializeField]
	private Polygon _polygon = new Polygon(new Vector2[]
	{
		new Vector2(-50f, 50f),
		new Vector2(50f, 50f),
		new Vector2(50f, -50f),
		new Vector2(-50f, -50f)
	});

	// Token: 0x040003CE RID: 974
	[NonSerialized]
	private List<Vector3> _worldPoints;

	// Token: 0x040003CF RID: 975
	private static Material _waterMaterial;

	// Token: 0x040003D0 RID: 976
	[SerializeField]
	private GuidComponent guidComponent;

	// Token: 0x040003D1 RID: 977
	[SerializeField]
	private MeshRenderer _meshRenderer;

	// Token: 0x040003D2 RID: 978
	[SerializeField]
	private MeshFilter meshFilter;

	// Token: 0x040003D3 RID: 979
	public Bounds bounds;

	// Token: 0x040003D4 RID: 980
	private Mesh _mesh;

	// Token: 0x040003D5 RID: 981
	private MaterialPropertyBlock _properties;

	// Token: 0x040003D6 RID: 982
	private bool clearSharedMeshOnAwake;

	// Token: 0x040003D7 RID: 983
	private List<Vector3> _verticesScratch;

	// Token: 0x040003D8 RID: 984
	private List<int> _trianglesScratch;
}
