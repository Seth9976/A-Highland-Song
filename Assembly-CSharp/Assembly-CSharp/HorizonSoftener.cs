using System;
using UnityEngine;

// Token: 0x0200018D RID: 397
public class HorizonSoftener : MonoSingleton<HorizonSoftener>
{
	// Token: 0x170002FF RID: 767
	// (get) Token: 0x06000CF3 RID: 3315 RVA: 0x00067A80 File Offset: 0x00065C80
	private SpriteRenderer spriteRenderer
	{
		get
		{
			if (this._spriteRenderer == null)
			{
				this._spriteRenderer = base.GetComponent<SpriteRenderer>();
			}
			return this._spriteRenderer;
		}
	}

	// Token: 0x06000CF4 RID: 3316 RVA: 0x00067AA2 File Offset: 0x00065CA2
	public void SetColour(Color color)
	{
		this.spriteRenderer.color = color;
	}

	// Token: 0x06000CF5 RID: 3317 RVA: 0x00067AB0 File Offset: 0x00065CB0
	private void LateUpdate()
	{
		GameCamera instance = GameCamera.instance;
		Vector3 position = instance.transform.position;
		float farClipPlane = instance.camera.farClipPlane;
		float num = 57.29578f * Mathf.Atan2(position.y, farClipPlane);
		float num2 = num + 0.5f * this.verticalFOV;
		float num3 = Mathf.Sin(num2 * 0.017453292f);
		float num4 = Mathf.Cos(num2 * 0.017453292f);
		this.distToGroundIntersection = position.y / num3 * num4;
		float num5 = position.z + this.distToGroundIntersection;
		if (num5 < this.closestWorldZ)
		{
			num5 = this.closestWorldZ;
			this.distToGroundIntersection = num5 - position.z;
		}
		float num6 = new Vector2(this.distToGroundIntersection, position.y).magnitude / Mathf.Cos(0.008726646f * this.verticalFOV);
		Vector3 normalized = new Vector3(0f, -position.y, farClipPlane).normalized;
		Vector3 vector = position + 0.8f * new Vector3(0f, -position.y, farClipPlane);
		float num7 = 100f * instance.cameraProperties.shearFactor;
		base.transform.position = vector + num7 * Vector3.up;
		float num8 = num6 * Mathf.Tan(0.008726646f * this.verticalFOV);
		base.transform.localScale = new Vector3(100000f, 0.5f * num8, 1f);
		base.transform.rotation = Quaternion.Euler(-num, 0f, 0f);
	}

	// Token: 0x04000FC6 RID: 4038
	public float verticalFOV = 5f;

	// Token: 0x04000FC7 RID: 4039
	public float closestWorldZ = 4000f;

	// Token: 0x04000FC8 RID: 4040
	public float distToGroundIntersection;

	// Token: 0x04000FC9 RID: 4041
	private SpriteRenderer _spriteRenderer;
}
