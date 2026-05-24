using System;
using UnityEngine;

// Token: 0x020001A6 RID: 422
public class SkiLiftChair : MonoBehaviour
{
	// Token: 0x17000351 RID: 849
	// (get) Token: 0x06000DF0 RID: 3568 RVA: 0x0006E1FA File Offset: 0x0006C3FA
	private SpriteRenderer spriteRenderer
	{
		get
		{
			if (this._spriteRenderer == null)
			{
				this._spriteRenderer = base.GetComponentInChildren<SpriteRenderer>();
			}
			return this._spriteRenderer;
		}
	}

	// Token: 0x17000352 RID: 850
	// (get) Token: 0x06000DF1 RID: 3569 RVA: 0x0006E21C File Offset: 0x0006C41C
	// (set) Token: 0x06000DF2 RID: 3570 RVA: 0x0006E224 File Offset: 0x0006C424
	public float alpha
	{
		get
		{
			return this._alpha;
		}
		set
		{
			this._alpha = value;
			Color color = this.spriteRenderer.color;
			color.a = this._alpha;
			this.spriteRenderer.color = color;
		}
	}

	// Token: 0x040010C8 RID: 4296
	public Transform sitTransform;

	// Token: 0x040010C9 RID: 4297
	[NonSerialized]
	public float positionOnLine;

	// Token: 0x040010CA RID: 4298
	[NonSerialized]
	public int currentBasePosIdx;

	// Token: 0x040010CB RID: 4299
	[NonSerialized]
	public float swingMaxAngle;

	// Token: 0x040010CC RID: 4300
	[NonSerialized]
	public float swingVariation;

	// Token: 0x040010CD RID: 4301
	private SpriteRenderer _spriteRenderer;

	// Token: 0x040010CE RID: 4302
	private float _alpha = 1f;
}
