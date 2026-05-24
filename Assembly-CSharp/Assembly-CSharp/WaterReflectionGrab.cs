using System;
using UnityEngine;

// Token: 0x02000026 RID: 38
[ExecuteInEditMode]
public class WaterReflectionGrab : MonoBehaviour
{
	// Token: 0x060000BE RID: 190 RVA: 0x0000A81E File Offset: 0x00008A1E
	private void OnDisable()
	{
		this.DestoryPrevFrameRenderTex();
	}

	// Token: 0x060000BF RID: 191 RVA: 0x0000A826 File Offset: 0x00008A26
	private void DestoryPrevFrameRenderTex()
	{
		if (this.renderTex == null)
		{
			return;
		}
		if (Application.isPlaying)
		{
			Object.Destroy(this.renderTex);
		}
		else
		{
			Object.DestroyImmediate(this.renderTex);
		}
		this.renderTex = null;
	}

	// Token: 0x060000C0 RID: 192 RVA: 0x0000A860 File Offset: 0x00008A60
	public void RefreshReflectionOffset()
	{
		Vector2 zero = Vector2.zero;
		float y = GameCamera.instance.camera.WorldToViewportPoint(GameCamera.instance.waterReflectionReferencePosition).y;
		float num = 2f * (y - 0.5f);
		zero.y = num;
		Vector3 vector = GameCamera.instance.transform.position - this.prevFrameCameraPos;
		Vector3 position = Runner.instance.transform.position;
		Vector3 vector2 = GameCamera.instance.camera.WorldToViewportPoint(position - vector);
		Vector3 vector3 = GameCamera.instance.camera.WorldToViewportPoint(position) - vector2;
		zero.x = -this.viewportMovementScalar * vector3.x;
		foreach (WaterPlane waterPlane in MonoInstancer<WaterPlane>.all)
		{
			waterPlane.SetLowQualityWaterProperties(zero + new Vector2(0f, 1f), -1f, this.renderTex);
		}
	}

	// Token: 0x060000C1 RID: 193 RVA: 0x0000A980 File Offset: 0x00008B80
	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		int num = dest.width / this.resolutionScaleFactor;
		int num2 = dest.height / this.resolutionScaleFactor;
		if (this.renderTex == null || this.renderTex.width != num || this.renderTex.height != num2)
		{
			this.DestoryPrevFrameRenderTex();
			this.renderTex = new RenderTexture(num, num2, 24, RenderTextureFormat.ARGB32);
		}
		Graphics.Blit(src, dest);
		Graphics.Blit(src, this.renderTex);
		this.prevFrameCameraPos = this._currFrameCameraPos;
		this._currFrameCameraPos = base.transform.position;
		RenderTexture.active = dest;
	}

	// Token: 0x04000199 RID: 409
	public RenderTexture renderTex;

	// Token: 0x0400019A RID: 410
	public Vector3 prevFrameCameraPos;

	// Token: 0x0400019B RID: 411
	public float viewportMovementScalar = 0.5f;

	// Token: 0x0400019C RID: 412
	public int resolutionScaleFactor = 4;

	// Token: 0x0400019D RID: 413
	private Vector3 _currFrameCameraPos;
}
