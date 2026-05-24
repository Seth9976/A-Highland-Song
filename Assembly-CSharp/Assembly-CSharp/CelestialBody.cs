using System;
using UnityEngine;

// Token: 0x02000174 RID: 372
public class CelestialBody : MonoBehaviour
{
	// Token: 0x06000C52 RID: 3154 RVA: 0x00062884 File Offset: 0x00060A84
	private void OnEnable()
	{
		this._renderer = base.GetComponentInChildren<Renderer>();
	}

	// Token: 0x06000C53 RID: 3155 RVA: 0x00062894 File Offset: 0x00060A94
	private void LateUpdate()
	{
		float num = 0.017453292f * this.theta;
		base.transform.localPosition = new Vector3(this.radiusX * Mathf.Sin(num), this.radiusY * Mathf.Cos(num) + this.offsetY, base.transform.localPosition.z);
		if (this.rotate)
		{
			base.transform.localRotation = Quaternion.Euler(0f, 0f, -this.theta);
		}
		if (this._renderer is SpriteRenderer)
		{
			((SpriteRenderer)this._renderer).color = this.color;
			return;
		}
		if (this._propertyBlock == null)
		{
			this._propertyBlock = new MaterialPropertyBlock();
			this._colorId = Shader.PropertyToID("_Color");
		}
		this._propertyBlock.SetColor(this._colorId, this.color);
		((MeshRenderer)this._renderer).SetPropertyBlock(this._propertyBlock);
	}

	// Token: 0x04000EBD RID: 3773
	public float theta;

	// Token: 0x04000EBE RID: 3774
	public float radiusX = 1000f;

	// Token: 0x04000EBF RID: 3775
	public float radiusY = 1000f;

	// Token: 0x04000EC0 RID: 3776
	public float offsetY;

	// Token: 0x04000EC1 RID: 3777
	public bool rotate;

	// Token: 0x04000EC2 RID: 3778
	public Color color = Color.white;

	// Token: 0x04000EC3 RID: 3779
	public Transform backlightFlare;

	// Token: 0x04000EC4 RID: 3780
	private Renderer _renderer;

	// Token: 0x04000EC5 RID: 3781
	private MaterialPropertyBlock _propertyBlock;

	// Token: 0x04000EC6 RID: 3782
	private int _colorId;
}
