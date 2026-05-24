using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200011A RID: 282
[ExecuteInEditMode]
[RequireComponent(typeof(CanvasRenderer))]
public class JournalPage : Graphic
{
	// Token: 0x17000261 RID: 609
	// (get) Token: 0x0600099A RID: 2458 RVA: 0x00050F52 File Offset: 0x0004F152
	public override Texture mainTexture
	{
		get
		{
			if (!(this._texture == null))
			{
				return this._texture;
			}
			return Graphic.s_WhiteTexture;
		}
	}

	// Token: 0x17000262 RID: 610
	// (get) Token: 0x0600099B RID: 2459 RVA: 0x00050F6E File Offset: 0x0004F16E
	// (set) Token: 0x0600099C RID: 2460 RVA: 0x00050F76 File Offset: 0x0004F176
	public Texture texture
	{
		get
		{
			return this._texture;
		}
		set
		{
			if (this._texture == value)
			{
				return;
			}
			this._texture = value;
			this.SetMaterialDirty();
		}
	}

	// Token: 0x0600099D RID: 2461 RVA: 0x00050F94 File Offset: 0x0004F194
	protected override void OnPopulateMesh(VertexHelper vh)
	{
		RectTransform rectTransform = base.rectTransform;
		if (rectTransform == null)
		{
			return;
		}
		Vector2 pivot = rectTransform.pivot;
		Rect rect = rectTransform.rect;
		vh.Clear();
		this._vertexBuffer.Clear();
		UIVertex simpleVert = UIVertex.simpleVert;
		Rect rect2 = new Rect(-pivot * rect.size, rect.size);
		float num = (this.isRightPage ? 0.5f : 0f);
		simpleVert.position = new Vector2(rect2.xMin, rect2.yMin);
		simpleVert.uv0 = new Vector2(0f + num, 0f);
		simpleVert.color = this.color;
		this._vertexBuffer.Add(simpleVert);
		simpleVert.position = new Vector2(rect2.xMax, rect2.yMin);
		simpleVert.uv0 = new Vector2(0.5f + num, 0f);
		simpleVert.color = this.color;
		this._vertexBuffer.Add(simpleVert);
		simpleVert.position = new Vector2(rect2.xMax, rect2.yMax);
		simpleVert.uv0 = new Vector2(0.5f + num, 1f);
		simpleVert.color = this.color;
		this._vertexBuffer.Add(simpleVert);
		simpleVert.position = new Vector2(rect2.xMin, rect2.yMax);
		simpleVert.uv0 = new Vector2(0f + num, 1f);
		simpleVert.color = this.color;
		this._vertexBuffer.Add(simpleVert);
		vh.AddUIVertexStream(this._vertexBuffer, this._indexBuffer);
	}

	// Token: 0x04000B85 RID: 2949
	public bool isRightPage;

	// Token: 0x04000B86 RID: 2950
	[SerializeField]
	private Texture _texture;

	// Token: 0x04000B87 RID: 2951
	private List<UIVertex> _vertexBuffer = new List<UIVertex>();

	// Token: 0x04000B88 RID: 2952
	private List<int> _indexBuffer = new List<int> { 2, 1, 0, 0, 3, 2 };
}
