using System;
using UnityEngine;

// Token: 0x020000AB RID: 171
[ExecuteInEditMode]
public class BlurEffect : MonoBehaviour
{
	// Token: 0x17000164 RID: 356
	// (get) Token: 0x0600057C RID: 1404 RVA: 0x0002BBA7 File Offset: 0x00029DA7
	private Camera camera
	{
		get
		{
			if (this._camera == null)
			{
				this._camera = base.GetComponent<Camera>();
			}
			return this._camera;
		}
	}

	// Token: 0x0600057D RID: 1405 RVA: 0x0002BBCC File Offset: 0x00029DCC
	private void OnDisable()
	{
		this._camera = null;
		if (this.renderTexture != null)
		{
			RenderTexture.ReleaseTemporary(this.renderTexture);
			this.renderTexture = null;
		}
		this.blurEffectMaterial.SetFloat("_BlurSize", this.settings.maxBlurSize);
	}

	// Token: 0x0600057E RID: 1406 RVA: 0x0002BC1C File Offset: 0x00029E1C
	private void OnPreRender()
	{
		if (this.renderTexture == null || this.renderTexture.width != this.settings.renderWidth)
		{
			if (this.renderTexture != null)
			{
				RenderTexture.ReleaseTemporary(this.renderTexture);
				this.renderTexture = null;
			}
			int num = Mathf.RoundToInt((float)this.settings.renderWidth / this.camera.aspect);
			this.renderTexture = RenderTexture.GetTemporary(this.settings.renderWidth, num, 16);
		}
		this.camera.targetTexture = this.renderTexture;
	}

	// Token: 0x0600057F RID: 1407 RVA: 0x0002BCB8 File Offset: 0x00029EB8
	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		if (this.strength == 0f)
		{
			Graphics.Blit(src, dest);
			return;
		}
		this.blurEffectMaterial.SetFloat("_BlurSize", this.strength * this.settings.maxBlurSize);
		int num = Mathf.RoundToInt((float)this.settings.renderWidth / this.camera.aspect);
		RenderTexture temporary = RenderTexture.GetTemporary(this.settings.renderWidth, num, 16);
		Graphics.Blit(src, temporary, this.blurEffectMaterial, 0);
		Graphics.Blit(temporary, dest, this.blurEffectMaterial, 1);
		RenderTexture.ReleaseTemporary(temporary);
	}

	// Token: 0x06000580 RID: 1408 RVA: 0x0002BD50 File Offset: 0x00029F50
	private void OnPostRender()
	{
		this.camera.targetTexture = null;
		Graphics.Blit(this.renderTexture, null);
	}

	// Token: 0x0400064F RID: 1615
	public BlurEffectSettings settings;

	// Token: 0x04000650 RID: 1616
	[Range(0f, 1f)]
	public float strength = 1f;

	// Token: 0x04000651 RID: 1617
	public Material blurEffectMaterial;

	// Token: 0x04000652 RID: 1618
	private Camera _camera;

	// Token: 0x04000653 RID: 1619
	private RenderTexture renderTexture;
}
