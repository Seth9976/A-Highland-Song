using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001CC RID: 460
[ExecuteInEditMode]
[RequireComponent(typeof(CanvasRenderer))]
public class RoundRect : Graphic
{
	// Token: 0x17000390 RID: 912
	// (get) Token: 0x06000F5A RID: 3930 RVA: 0x00075D1D File Offset: 0x00073F1D
	// (set) Token: 0x06000F5B RID: 3931 RVA: 0x00075D25 File Offset: 0x00073F25
	public float cornerRadius
	{
		get
		{
			return this._cornerRadius;
		}
		set
		{
			if (this._cornerRadius == value)
			{
				return;
			}
			this._cornerRadius = value;
			this._roundRectOutlineParamsDirty = true;
			this.SetVerticesDirty();
		}
	}

	// Token: 0x17000391 RID: 913
	// (get) Token: 0x06000F5C RID: 3932 RVA: 0x00075D45 File Offset: 0x00073F45
	// (set) Token: 0x06000F5D RID: 3933 RVA: 0x00075D4D File Offset: 0x00073F4D
	public Color fillColor
	{
		get
		{
			return this._fillColor;
		}
		set
		{
			if (this._fillColor == value)
			{
				return;
			}
			this._fillColor = value;
			this.SetVerticesDirty();
		}
	}

	// Token: 0x17000392 RID: 914
	// (get) Token: 0x06000F5E RID: 3934 RVA: 0x00075D6B File Offset: 0x00073F6B
	// (set) Token: 0x06000F5F RID: 3935 RVA: 0x00075D73 File Offset: 0x00073F73
	public Color outlineColor
	{
		get
		{
			return this._outlineColor;
		}
		set
		{
			if (this._outlineColor == value)
			{
				return;
			}
			this._outlineColor = value;
			this.SetVerticesDirty();
		}
	}

	// Token: 0x17000393 RID: 915
	// (get) Token: 0x06000F60 RID: 3936 RVA: 0x00075D91 File Offset: 0x00073F91
	// (set) Token: 0x06000F61 RID: 3937 RVA: 0x00075D99 File Offset: 0x00073F99
	public RoundRect.OutlineMode outlineMode
	{
		get
		{
			return this._outlineMode;
		}
		set
		{
			if (this._outlineMode == value)
			{
				return;
			}
			this._outlineMode = value;
			this._roundRectOutlineParamsDirty = true;
			this.SetVerticesDirty();
		}
	}

	// Token: 0x17000394 RID: 916
	// (get) Token: 0x06000F62 RID: 3938 RVA: 0x00075DB9 File Offset: 0x00073FB9
	// (set) Token: 0x06000F63 RID: 3939 RVA: 0x00075DC1 File Offset: 0x00073FC1
	public float antiAliasWidth
	{
		get
		{
			return this._antiAliasWidth;
		}
		set
		{
			if (this._antiAliasWidth == value)
			{
				return;
			}
			this._antiAliasWidth = value;
			this._roundRectOutlineParamsDirty = true;
			this.SetVerticesDirty();
		}
	}

	// Token: 0x17000395 RID: 917
	// (get) Token: 0x06000F64 RID: 3940 RVA: 0x00075DE1 File Offset: 0x00073FE1
	// (set) Token: 0x06000F65 RID: 3941 RVA: 0x00075DE9 File Offset: 0x00073FE9
	public float outlineWidth
	{
		get
		{
			return this._outlineWidth;
		}
		set
		{
			if (this._outlineWidth == value)
			{
				return;
			}
			this._outlineWidth = value;
			this._roundRectOutlineParamsDirty = true;
			this.SetVerticesDirty();
		}
	}

	// Token: 0x06000F66 RID: 3942 RVA: 0x00075E0C File Offset: 0x0007400C
	private float RenderedRadius(float radius, Vector2 rectSize)
	{
		if (radius < 0f)
		{
			radius = 0f;
		}
		if (radius * 2f > rectSize.y)
		{
			radius = rectSize.y / 2f;
		}
		if (radius * 2f > rectSize.x)
		{
			radius = rectSize.x / 2f;
		}
		return radius;
	}

	// Token: 0x06000F67 RID: 3943 RVA: 0x00075E63 File Offset: 0x00074063
	protected override void OnRectTransformDimensionsChange()
	{
		base.OnRectTransformDimensionsChange();
		this._roundRectOutlineParamsDirty = true;
	}

	// Token: 0x06000F68 RID: 3944 RVA: 0x00075E72 File Offset: 0x00074072
	protected override void OnCanvasHierarchyChanged()
	{
		base.OnCanvasHierarchyChanged();
		if (base.canvas != null)
		{
			this.SetVerticesDirty();
			this.SetLayoutDirty();
		}
	}

	// Token: 0x06000F69 RID: 3945 RVA: 0x00075E94 File Offset: 0x00074094
	protected override void Start()
	{
		base.Start();
		this.SetupMaterialIfNecessary();
	}

	// Token: 0x06000F6A RID: 3946 RVA: 0x00075EA4 File Offset: 0x000740A4
	private void SetupMaterialIfNecessary()
	{
		if (this.material == null || RoundRect._sharedMaterial == null || this.material != RoundRect._sharedMaterial)
		{
			if (RoundRect._sharedMaterial == null)
			{
				RoundRect._sharedMaterial = new Material(Resources.Load<Shader>("RoundRectShader"));
				RoundRect._sharedMaterial.hideFlags = HideFlags.HideAndDontSave;
				RoundRect._sharedMaterial.name = "RoundRectShader";
			}
			this.material = RoundRect._sharedMaterial;
		}
	}

	// Token: 0x06000F6B RID: 3947 RVA: 0x00075F28 File Offset: 0x00074128
	private RoundRect.RoundRectOuterGeom CalcOuterGeom(float radius, float sizeAdjust)
	{
		Vector2 zero = Vector2.zero;
		Vector2 zero2 = Vector2.zero;
		Rect rect = base.rectTransform.rect;
		Vector2 vector = sizeAdjust * Vector2.one;
		rect.min -= vector;
		rect.max += vector;
		Vector2 pivot = base.rectTransform.pivot;
		zero.x = -rect.width * pivot.x;
		zero.y = -rect.height * pivot.y;
		zero2.x = rect.width * (1f - pivot.x);
		zero2.y = rect.height * (1f - pivot.y);
		float num = this.RenderedRadius(radius, rect.size);
		return new RoundRect.RoundRectOuterGeom
		{
			rect = rect,
			radius = num
		};
	}

	// Token: 0x06000F6C RID: 3948 RVA: 0x00076020 File Offset: 0x00074220
	protected override void OnPopulateMesh(VertexHelper vh)
	{
		if (base.rectTransform == null)
		{
			return;
		}
		vh.Clear();
		this.vertexBuffer.Clear();
		this.indexBuffer.Clear();
		Color color = this._fillColor * this.color;
		Color color2 = this._outlineColor * this.color;
		if (color.a == 0f && color2.a == 0f)
		{
			return;
		}
		float num = this.antiAliasWidth;
		if (this._outlineMode == RoundRect.OutlineMode.Inner && num > this.cornerRadius)
		{
			num = this.cornerRadius;
		}
		float num2 = 0f;
		float num3 = -Mathf.Min(1.5f * num, this.outlineWidth);
		float num4 = this.cornerRadius;
		float num5 = this.cornerRadius - 0.5f * num;
		if (this._outlineMode == RoundRect.OutlineMode.Center)
		{
			num2 = 0.5f * this.outlineWidth;
			num3 = 0f;
			num5 = this.cornerRadius;
			num4 = this.cornerRadius + 0.5f * this.outlineWidth;
		}
		else if (this._outlineMode == RoundRect.OutlineMode.Outer)
		{
			num2 = this.outlineWidth;
			num3 = num;
			num5 = this.cornerRadius + 0.5f * num;
			num4 = this.cornerRadius + this.outlineWidth;
		}
		RoundRect.RoundRectOuterGeom roundRectOuterGeom = this.CalcOuterGeom(num4, num2);
		RoundRect.RoundRectOuterGeom roundRectOuterGeom2 = this.CalcOuterGeom(num5, num3);
		if (this._roundRectOutlineParamsDirty)
		{
			float num6 = 0f;
			if (base.canvas != null)
			{
				float width = base.canvas.rootCanvas.pixelRect.width;
				RectTransform rectTransform = (RectTransform)base.canvas.rootCanvas.transform;
				num6 = num * rectTransform.rect.width / width;
			}
			float num7 = this.RenderedRadius(num4, roundRectOuterGeom.rect.size);
			float num8 = 0f;
			float num9 = 0f;
			if (num7 > 0f)
			{
				num8 = num6 / num7;
				num9 = this._outlineWidth / num7;
			}
			float num10 = 0f;
			if (this._outlineWidth < 1f)
			{
				num10 = 1.5f * num8 * (1f - Mathf.InverseLerp(0f, 1f, this._outlineWidth));
			}
			this._roundRectParamsOutline = new RoundRect.RoundRectParams
			{
				outlineStartNorm = 1f - num9 + num10 - 0.5f * num8,
				antialiasWidth = num8
			};
			this._roundRectOutlineParamsDirty = false;
		}
		RoundRect.RoundRectParams roundRectParams = new RoundRect.RoundRectParams
		{
			outlineStartNorm = -1f,
			antialiasWidth = this._roundRectParamsOutline.antialiasWidth
		};
		if (color.a > 0f)
		{
			this.MakeRoundRectOutlineGeometry(this.vertexBuffer, this.indexBuffer, roundRectOuterGeom2, roundRectParams, color);
		}
		if (color2.a > 0f && this.outlineWidth != 0f)
		{
			this.MakeRoundRectOutlineGeometry(this.vertexBuffer, this.indexBuffer, roundRectOuterGeom, this._roundRectParamsOutline, color2);
		}
		Rect rect = roundRectOuterGeom2.rect;
		rect.min += roundRectOuterGeom2.radius * Vector2.one;
		rect.max -= roundRectOuterGeom2.radius * Vector2.one;
		RoundRect.RoundRectParams roundRectParams2 = new RoundRect.RoundRectParams
		{
			outlineStartNorm = -1f,
			antialiasWidth = 0f
		};
		if (rect.size.x > 0f && rect.size.y > 0f && color.a > 0f)
		{
			this.MakeHardQuad(this.vertexBuffer, this.indexBuffer, rect, roundRectParams2, color);
		}
		if (this.outlineWidth >= roundRectOuterGeom.radius + 1.5f * num && color2.a > 0f)
		{
			float num11 = this.outlineWidth - roundRectOuterGeom.radius - 1.5f * num;
			Rect rect2 = rect;
			rect2.min += num11 * Vector2.one;
			rect2.max -= num11 * Vector2.one;
			if (rect2.size.x > 0f && rect2.size.y > 0f)
			{
				this.MakeHardQuad(this.vertexBuffer, this.indexBuffer, new Rect(rect.x, rect.y, rect.width, num11), roundRectParams2, color2);
				this.MakeHardQuad(this.vertexBuffer, this.indexBuffer, new Rect(rect.x, rect.yMax - num11, rect.width, num11), roundRectParams2, color2);
				this.MakeHardQuad(this.vertexBuffer, this.indexBuffer, new Rect(rect.x, rect.y + num11, num11, rect.height - 2f * num11), roundRectParams2, color2);
				this.MakeHardQuad(this.vertexBuffer, this.indexBuffer, new Rect(rect.xMax - num11, rect.y + num11, num11, rect.height - 2f * num11), roundRectParams2, color2);
			}
			else if (rect.size.x > 0f && rect.size.y > 0f)
			{
				this.MakeHardQuad(this.vertexBuffer, this.indexBuffer, rect, roundRectParams2, color2);
			}
		}
		vh.AddUIVertexStream(this.vertexBuffer, this.indexBuffer);
	}

	// Token: 0x06000F6D RID: 3949 RVA: 0x000765D4 File Offset: 0x000747D4
	private void MakeRoundRectOutlineGeometry(List<UIVertex> vertices, List<int> indices, RoundRect.RoundRectOuterGeom geom, RoundRect.RoundRectParams roundRectParams, Color32 color)
	{
		if (geom.radius > 0f)
		{
			this.AddCorner(this.vertexBuffer, this.indexBuffer, geom.rect.min, new Vector2(geom.radius, geom.radius), roundRectParams, color, false);
			this.AddCorner(this.vertexBuffer, this.indexBuffer, new Vector2(geom.rect.xMax, geom.rect.yMin), new Vector2(-geom.radius, geom.radius), roundRectParams, color, true);
			this.AddCorner(this.vertexBuffer, this.indexBuffer, new Vector2(geom.rect.xMin, geom.rect.yMax), new Vector2(geom.radius, -geom.radius), roundRectParams, color, true);
			this.AddCorner(this.vertexBuffer, this.indexBuffer, new Vector2(geom.rect.xMax, geom.rect.yMax), new Vector2(-geom.radius, -geom.radius), roundRectParams, color, false);
		}
		float num = geom.rect.width - 2f * geom.radius;
		if (num > 0f)
		{
			this.AddEdge(this.vertexBuffer, this.indexBuffer, new Vector2(geom.rect.xMin + geom.radius, geom.rect.yMax), new Vector2(num, -geom.radius), roundRectParams, color, false);
			this.AddEdge(this.vertexBuffer, this.indexBuffer, new Vector2(geom.rect.xMin + geom.radius + num, geom.rect.yMin), new Vector2(-num, geom.radius), roundRectParams, color, false);
		}
		float num2 = geom.rect.height - 2f * geom.radius;
		if (num2 > 0f)
		{
			this.AddEdge(this.vertexBuffer, this.indexBuffer, new Vector2(geom.rect.xMin, geom.rect.yMin + geom.radius), new Vector2(geom.radius, num2), roundRectParams, color, true);
			this.AddEdge(this.vertexBuffer, this.indexBuffer, new Vector2(geom.rect.xMax, geom.rect.yMax - geom.radius), new Vector2(-geom.radius, -num2), roundRectParams, color, true);
		}
	}

	// Token: 0x06000F6E RID: 3950 RVA: 0x00076864 File Offset: 0x00074A64
	private void AddCorner(List<UIVertex> vertices, List<int> indices, Vector2 corner, Vector2 toCurveOrigin, RoundRect.RoundRectParams roundRectParams, Color32 color, bool flipWinding)
	{
		UIVertex simpleVert = UIVertex.simpleVert;
		simpleVert.color = color;
		int count = vertices.Count;
		simpleVert.position = new Vector3(corner.x, corner.y);
		simpleVert.uv0 = this.RoundRectUV0(1f, 1f, roundRectParams);
		this.verts[0] = simpleVert;
		simpleVert.position = new Vector3(corner.x, corner.y + toCurveOrigin.y);
		simpleVert.uv0 = this.RoundRectUV0(1f, 0f, roundRectParams);
		this.verts[1] = simpleVert;
		simpleVert.position = new Vector3(corner.x + toCurveOrigin.x, corner.y + toCurveOrigin.y);
		simpleVert.uv0 = this.RoundRectUV0(0f, 0f, roundRectParams);
		this.verts[2] = simpleVert;
		simpleVert.position = new Vector3(corner.x + toCurveOrigin.x, corner.y);
		simpleVert.uv0 = this.RoundRectUV0(0f, 1f, roundRectParams);
		this.verts[3] = simpleVert;
		if (flipWinding)
		{
			this.inds[0] = count + 2;
			this.inds[1] = count + 1;
			this.inds[2] = count;
			this.inds[3] = count;
			this.inds[4] = count + 3;
			this.inds[5] = count + 2;
		}
		else
		{
			this.inds[0] = count;
			this.inds[1] = count + 1;
			this.inds[2] = count + 2;
			this.inds[3] = count + 2;
			this.inds[4] = count + 3;
			this.inds[5] = count;
		}
		vertices.AddRange(this.verts);
		indices.AddRange(this.inds);
	}

	// Token: 0x06000F6F RID: 3951 RVA: 0x00076A3C File Offset: 0x00074C3C
	private void AddEdge(List<UIVertex> vertices, List<int> indices, Vector2 onEdgeLeft, Vector2 opEdgeCorner, RoundRect.RoundRectParams roundRectParams, Color32 color, bool isSideEdge)
	{
		UIVertex simpleVert = UIVertex.simpleVert;
		simpleVert.color = color;
		int count = vertices.Count;
		simpleVert.position = new Vector3(onEdgeLeft.x, onEdgeLeft.y);
		simpleVert.uv0 = (isSideEdge ? this.RoundRectUV0(0f, 1f, roundRectParams) : this.RoundRectUV0(0f, 1f, roundRectParams));
		this.verts[0] = simpleVert;
		simpleVert.position = new Vector3(onEdgeLeft.x, onEdgeLeft.y + opEdgeCorner.y);
		simpleVert.uv0 = (isSideEdge ? this.RoundRectUV0(0f, 1f, roundRectParams) : this.RoundRectUV0(0f, 0f, roundRectParams));
		this.verts[1] = simpleVert;
		simpleVert.position = new Vector3(onEdgeLeft.x + opEdgeCorner.x, onEdgeLeft.y + opEdgeCorner.y);
		simpleVert.uv0 = (isSideEdge ? this.RoundRectUV0(0f, 0f, roundRectParams) : this.RoundRectUV0(0f, 0f, roundRectParams));
		this.verts[2] = simpleVert;
		simpleVert.position = new Vector3(onEdgeLeft.x + opEdgeCorner.x, onEdgeLeft.y);
		simpleVert.uv0 = (isSideEdge ? this.RoundRectUV0(0f, 0f, roundRectParams) : this.RoundRectUV0(0f, 1f, roundRectParams));
		this.verts[3] = simpleVert;
		if (!isSideEdge)
		{
			this.inds[0] = count + 2;
			this.inds[1] = count + 1;
			this.inds[2] = count;
			this.inds[3] = count;
			this.inds[4] = count + 3;
			this.inds[5] = count + 2;
		}
		else
		{
			this.inds[0] = count;
			this.inds[1] = count + 1;
			this.inds[2] = count + 2;
			this.inds[3] = count + 2;
			this.inds[4] = count + 3;
			this.inds[5] = count;
		}
		vertices.AddRange(this.verts);
		indices.AddRange(this.inds);
	}

	// Token: 0x06000F70 RID: 3952 RVA: 0x00076C74 File Offset: 0x00074E74
	private void AddMiddle(List<UIVertex> vertices, List<int> indices, Vector2 corner1, Vector2 corner2, RoundRect.RoundRectParams roundRectParams, Color32 color, float renderedRadius)
	{
		UIVertex simpleVert = UIVertex.simpleVert;
		simpleVert.color = color;
		int count = vertices.Count;
		simpleVert.position = new Vector3(corner1.x + renderedRadius, corner1.y + renderedRadius);
		simpleVert.uv0 = this.RoundRectUV0(0f, 0f, roundRectParams);
		this.verts[0] = simpleVert;
		simpleVert.position = new Vector3(corner1.x + renderedRadius, corner2.y - renderedRadius);
		simpleVert.uv0 = this.RoundRectUV0(0f, 0f, roundRectParams);
		this.verts[1] = simpleVert;
		simpleVert.position = new Vector3(corner2.x - renderedRadius, corner2.y - renderedRadius);
		simpleVert.uv0 = this.RoundRectUV0(0f, 0f, roundRectParams);
		this.verts[2] = simpleVert;
		simpleVert.position = new Vector3(corner2.x - renderedRadius, corner1.y + renderedRadius);
		simpleVert.uv0 = this.RoundRectUV0(0f, 0f, roundRectParams);
		this.verts[3] = simpleVert;
		this.inds[0] = count + 2;
		this.inds[1] = count + 1;
		this.inds[2] = count;
		this.inds[3] = count;
		this.inds[4] = count + 3;
		this.inds[5] = count + 2;
		vertices.AddRange(this.verts);
		indices.AddRange(this.inds);
	}

	// Token: 0x06000F71 RID: 3953 RVA: 0x00076E04 File Offset: 0x00075004
	private void MakeHardQuad(List<UIVertex> vertices, List<int> indices, Rect rect, RoundRect.RoundRectParams fillRoundRectParams, Color32 color)
	{
		UIVertex simpleVert = UIVertex.simpleVert;
		simpleVert.color = color;
		int count = vertices.Count;
		simpleVert.position = new Vector3(rect.xMin, rect.yMin);
		simpleVert.uv0 = this.RoundRectUV0(0f, 0f, fillRoundRectParams);
		this.verts[0] = simpleVert;
		simpleVert.position = new Vector3(rect.xMin, rect.yMax);
		simpleVert.uv0 = this.RoundRectUV0(0f, 0f, fillRoundRectParams);
		this.verts[1] = simpleVert;
		simpleVert.position = new Vector3(rect.xMax, rect.yMax);
		simpleVert.uv0 = this.RoundRectUV0(0f, 0f, fillRoundRectParams);
		this.verts[2] = simpleVert;
		simpleVert.position = new Vector3(rect.xMax, rect.yMin);
		simpleVert.uv0 = this.RoundRectUV0(0f, 0f, fillRoundRectParams);
		this.verts[3] = simpleVert;
		this.inds[0] = count + 2;
		this.inds[1] = count + 1;
		this.inds[2] = count;
		this.inds[3] = count;
		this.inds[4] = count + 3;
		this.inds[5] = count + 2;
		vertices.AddRange(this.verts);
		indices.AddRange(this.inds);
	}

	// Token: 0x06000F72 RID: 3954 RVA: 0x00076F7D File Offset: 0x0007517D
	private Vector4 RoundRectUV0(float x, float y, RoundRect.RoundRectParams roundRectParams)
	{
		return new Vector4(x, y, roundRectParams.outlineStartNorm, roundRectParams.antialiasWidth);
	}

	// Token: 0x040011D8 RID: 4568
	[SerializeField]
	private float _cornerRadius = 10f;

	// Token: 0x040011D9 RID: 4569
	[SerializeField]
	private Color _fillColor = Color.white;

	// Token: 0x040011DA RID: 4570
	[SerializeField]
	private Color _outlineColor = Color.black;

	// Token: 0x040011DB RID: 4571
	[SerializeField]
	private RoundRect.OutlineMode _outlineMode;

	// Token: 0x040011DC RID: 4572
	[Range(0f, 4f)]
	[SerializeField]
	private float _antiAliasWidth = 0.5f;

	// Token: 0x040011DD RID: 4573
	[SerializeField]
	private float _outlineWidth = 2f;

	// Token: 0x040011DE RID: 4574
	private List<UIVertex> vertexBuffer = new List<UIVertex>();

	// Token: 0x040011DF RID: 4575
	private List<int> indexBuffer = new List<int>();

	// Token: 0x040011E0 RID: 4576
	[SerializeField]
	private bool _debugBool;

	// Token: 0x040011E1 RID: 4577
	private UIVertex[] verts = new UIVertex[4];

	// Token: 0x040011E2 RID: 4578
	private int[] inds = new int[6];

	// Token: 0x040011E3 RID: 4579
	private RoundRect.RoundRectParams _roundRectParamsOutline;

	// Token: 0x040011E4 RID: 4580
	private bool _roundRectOutlineParamsDirty = true;

	// Token: 0x040011E5 RID: 4581
	private static Material _sharedMaterial;

	// Token: 0x020003E2 RID: 994
	public enum OutlineMode
	{
		// Token: 0x04001A43 RID: 6723
		Inner,
		// Token: 0x04001A44 RID: 6724
		Center,
		// Token: 0x04001A45 RID: 6725
		Outer
	}

	// Token: 0x020003E3 RID: 995
	private struct RoundRectOuterGeom
	{
		// Token: 0x04001A46 RID: 6726
		public Rect rect;

		// Token: 0x04001A47 RID: 6727
		public float radius;
	}

	// Token: 0x020003E4 RID: 996
	private struct RoundRectParams
	{
		// Token: 0x04001A48 RID: 6728
		public float outlineStartNorm;

		// Token: 0x04001A49 RID: 6729
		public float antialiasWidth;
	}
}
