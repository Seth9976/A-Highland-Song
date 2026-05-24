using System;
using UnityEngine;

// Token: 0x020000AF RID: 175
public class Twinkle : MonoBehaviour
{
	// Token: 0x17000168 RID: 360
	// (get) Token: 0x0600058C RID: 1420 RVA: 0x0002BE77 File Offset: 0x0002A077
	public SpriteRenderer spriteRenderer
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

	// Token: 0x0600058D RID: 1421 RVA: 0x0002BE99 File Offset: 0x0002A099
	private void OnEnable()
	{
		this.StartNewPeriod();
	}

	// Token: 0x0600058E RID: 1422 RVA: 0x0002BEA4 File Offset: 0x0002A0A4
	private void StartNewPeriod()
	{
		this._time = 0f;
		this._duration = this.settings.duration.Random();
		this._pauseDuration = this.settings.pauseDuration.Random();
		this._rotOffset = (float)Random.Range(0, 360);
	}

	// Token: 0x0600058F RID: 1423 RVA: 0x0002BEFC File Offset: 0x0002A0FC
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
			base.transform.rotation = Quaternion.Euler(0f, 0f, this._rotOffset);
			this.spriteRenderer.color = this.settings.colourOverTime.Evaluate(0f);
			return;
		}
		float num = (this._time - this._pauseDuration) / this._duration;
		float num2 = this._rotOffset + 360f * this.settings.rotationOverTime.Evaluate(num);
		base.transform.rotation = Quaternion.Euler(0f, 0f, -num2);
		this.spriteRenderer.color = this.settings.colourOverTime.Evaluate(num);
	}

	// Token: 0x0400065B RID: 1627
	public TwinkleSettings settings;

	// Token: 0x0400065C RID: 1628
	private SpriteRenderer _spriteRenderer;

	// Token: 0x0400065D RID: 1629
	private float _duration;

	// Token: 0x0400065E RID: 1630
	private float _pauseDuration;

	// Token: 0x0400065F RID: 1631
	private float _time;

	// Token: 0x04000660 RID: 1632
	private float _rotOffset;
}
