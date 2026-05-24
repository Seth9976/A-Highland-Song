using System;
using UnityEngine;

// Token: 0x02000156 RID: 342
public class TwinkleUI : MonoBehaviour
{
	// Token: 0x170002B5 RID: 693
	// (get) Token: 0x06000B9C RID: 2972 RVA: 0x0005D289 File Offset: 0x0005B489
	public SLayout layout
	{
		get
		{
			if (!this._layoutSet)
			{
				this._layout = base.GetComponent<SLayout>();
				this._layoutSet = true;
			}
			return this._layout;
		}
	}

	// Token: 0x06000B9D RID: 2973 RVA: 0x0005D2AC File Offset: 0x0005B4AC
	private void OnEnable()
	{
		this.StartNewPeriod();
	}

	// Token: 0x06000B9E RID: 2974 RVA: 0x0005D2B4 File Offset: 0x0005B4B4
	private void StartNewPeriod()
	{
		this._time = 0f;
		this._duration = this.settings.duration.Random();
		this._pauseDuration = this.settings.pauseDuration.Random();
		this._rotOffset = (float)Random.Range(0, 360);
	}

	// Token: 0x06000B9F RID: 2975 RVA: 0x0005D30C File Offset: 0x0005B50C
	private void Update()
	{
		if (this.settings == null)
		{
			return;
		}
		this._time += Time.deltaTime;
		if (this._time > this._pauseDuration + this._duration)
		{
			this.StartNewPeriod();
		}
		if (this._time < this._pauseDuration)
		{
			this.layout.rotation = this._rotOffset;
			this.layout.color = this.settings.colourOverTime.Evaluate(0f);
			return;
		}
		float num = (this._time - this._pauseDuration) / this._duration;
		this.layout.rotation = -(this._rotOffset + 360f * this.settings.rotationOverTime.Evaluate(num));
		this.layout.color = this.settings.colourOverTime.Evaluate(num);
	}

	// Token: 0x04000DC8 RID: 3528
	public TwinkleSettings settings;

	// Token: 0x04000DC9 RID: 3529
	private bool _layoutSet;

	// Token: 0x04000DCA RID: 3530
	private SLayout _layout;

	// Token: 0x04000DCB RID: 3531
	private float _duration;

	// Token: 0x04000DCC RID: 3532
	private float _pauseDuration;

	// Token: 0x04000DCD RID: 3533
	private float _time;

	// Token: 0x04000DCE RID: 3534
	private float _rotOffset;
}
