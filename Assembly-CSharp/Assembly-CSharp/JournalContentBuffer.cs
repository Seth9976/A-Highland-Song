using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000116 RID: 278
public class JournalContentBuffer : MonoBehaviour
{
	// Token: 0x17000253 RID: 595
	// (get) Token: 0x06000946 RID: 2374 RVA: 0x0004DBF1 File Offset: 0x0004BDF1
	public RenderTexture renderTexture
	{
		get
		{
			this.UpdateRenderTextureIfNecessary();
			return this._renderTexture;
		}
	}

	// Token: 0x06000947 RID: 2375 RVA: 0x0004DBFF File Offset: 0x0004BDFF
	public void SetPageNumber(int curr, int total)
	{
		this._pageNumberLayout.textMeshPro.text = curr.ToString() + " / " + total.ToString();
	}

	// Token: 0x06000948 RID: 2376 RVA: 0x0004DC29 File Offset: 0x0004BE29
	private void Start()
	{
		this.UpdateRenderTextureIfNecessary();
		this._camera.enabled = true;
	}

	// Token: 0x06000949 RID: 2377 RVA: 0x0004DC3D File Offset: 0x0004BE3D
	private void Update()
	{
		this.UpdateRenderTextureIfNecessary();
	}

	// Token: 0x0600094A RID: 2378 RVA: 0x0004DC48 File Offset: 0x0004BE48
	private void UpdateRenderTextureIfNecessary()
	{
		bool flag = true;
		if (this._renderTexture == null || !flag)
		{
			if (this._renderTexture != null)
			{
				this._camera.targetTexture = null;
				Object.Destroy(this._renderTexture);
				this._renderTexture = null;
			}
			this._renderTexture = new RenderTexture(this.renderTextureWidth, this.renderTextureHeight, 24, RenderTextureFormat.ARGB32);
			this._camera.targetTexture = this._renderTexture;
		}
	}

	// Token: 0x04000B27 RID: 2855
	public int renderTextureWidth = 1200;

	// Token: 0x04000B28 RID: 2856
	public int renderTextureHeight = 900;

	// Token: 0x04000B29 RID: 2857
	private RenderTexture _renderTexture;

	// Token: 0x04000B2A RID: 2858
	public SLayout layout;

	// Token: 0x04000B2B RID: 2859
	public List<JournalItemView> content = new List<JournalItemView>();

	// Token: 0x04000B2C RID: 2860
	[SerializeField]
	private Camera _camera;

	// Token: 0x04000B2D RID: 2861
	[SerializeField]
	private SLayout _pageNumberLayout;
}
