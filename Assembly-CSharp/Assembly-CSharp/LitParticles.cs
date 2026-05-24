using System;
using UnityEngine;

// Token: 0x02000194 RID: 404
public class LitParticles : MonoInstancer<LitParticles>
{
	// Token: 0x1700030F RID: 783
	// (get) Token: 0x06000D3A RID: 3386 RVA: 0x00069D8F File Offset: 0x00067F8F
	// (set) Token: 0x06000D3B RID: 3387 RVA: 0x00069D97 File Offset: 0x00067F97
	public Color lightingColor
	{
		get
		{
			return this._lightingColor;
		}
		set
		{
			if (this._lightingColor != value)
			{
				this._lightingColor = value;
				this.RefreshColor();
			}
		}
	}

	// Token: 0x06000D3C RID: 3388 RVA: 0x00069DB4 File Offset: 0x00067FB4
	public void RefreshColor()
	{
		if (this._properties == null)
		{
			return;
		}
		Color color = Color.Lerp(Color.white, this.lightingColor, this.lightingStrength);
		color.a *= this._levelFadeAlpha;
		this._properties.SetColor(LitParticles._ColorId, color);
		this.renderer.SetPropertyBlock(this._properties);
	}

	// Token: 0x06000D3D RID: 3389 RVA: 0x00069E14 File Offset: 0x00068014
	public void SetLevelFadeAlpha(float levelFadeAlpha)
	{
		if (this._levelFadeAlpha != levelFadeAlpha)
		{
			this._levelFadeAlpha = levelFadeAlpha;
			this.RefreshColor();
		}
	}

	// Token: 0x17000310 RID: 784
	// (get) Token: 0x06000D3E RID: 3390 RVA: 0x00069E2C File Offset: 0x0006802C
	private Renderer renderer
	{
		get
		{
			if (this._renderer == null)
			{
				this._renderer = base.GetComponent<Renderer>();
			}
			return this._renderer;
		}
	}

	// Token: 0x06000D3F RID: 3391 RVA: 0x00069E4E File Offset: 0x0006804E
	private void Awake()
	{
		if (LitParticles._ColorId == 0)
		{
			LitParticles._ColorId = Shader.PropertyToID("_Color");
		}
		this._properties = new MaterialPropertyBlock();
	}

	// Token: 0x06000D40 RID: 3392 RVA: 0x00069E71 File Offset: 0x00068071
	protected override void OnEnable()
	{
		base.OnEnable();
		this.RefreshColor();
	}

	// Token: 0x0400101A RID: 4122
	[SerializeField]
	private Color _lightingColor = Color.white;

	// Token: 0x0400101B RID: 4123
	[Range(0f, 1f)]
	public float lightingStrength = 0.7f;

	// Token: 0x0400101C RID: 4124
	private Renderer _renderer;

	// Token: 0x0400101D RID: 4125
	private MaterialPropertyBlock _properties;

	// Token: 0x0400101E RID: 4126
	private float _levelFadeAlpha = 1f;

	// Token: 0x0400101F RID: 4127
	private static int _ColorId;
}
