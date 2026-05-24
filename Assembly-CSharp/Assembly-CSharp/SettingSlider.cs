using System;
using UnityEngine;

// Token: 0x02000149 RID: 329
public class SettingSlider : SettingControl<float>
{
	// Token: 0x06000B11 RID: 2833 RVA: 0x0005987C File Offset: 0x00057A7C
	public override void LeftRight(int amount)
	{
		float num = Mathf.Clamp01(this.currentValue + this.leftRightIncrements * (float)amount);
		if (num != this.currentValue)
		{
			this.currentValue = num;
			this.RefreshLayout();
			this.onChange(this.currentValue);
		}
	}

	// Token: 0x06000B12 RID: 2834 RVA: 0x000598C8 File Offset: 0x00057AC8
	public override void RefreshLayout()
	{
		base.RefreshLayout();
		this._nubbin.Animate(0.2f, delegate
		{
			this._nubbin.color = (base.highlighted ? this.settings.highlightColor : Color.white);
		});
		this._nubbin.Animate(0.3f, 0f, SLayout.popCurve, delegate
		{
			this._nubbin.scale = (base.highlighted ? 1.4f : 1f);
		});
	}

	// Token: 0x06000B13 RID: 2835 RVA: 0x00059920 File Offset: 0x00057B20
	private void Update()
	{
		this._nubbin.centerX = Mathf.SmoothDamp(this._nubbin.centerX, this.currentValue * this._bar.width, ref this._speed, this.settings.sliderSmoothTime, float.MaxValue, Time.unscaledDeltaTime);
	}

	// Token: 0x04000D35 RID: 3381
	public float leftRightIncrements = 0.05f;

	// Token: 0x04000D36 RID: 3382
	private float _speed;

	// Token: 0x04000D37 RID: 3383
	[SerializeField]
	private SLayout _bar;

	// Token: 0x04000D38 RID: 3384
	[SerializeField]
	private SLayout _nubbin;
}
