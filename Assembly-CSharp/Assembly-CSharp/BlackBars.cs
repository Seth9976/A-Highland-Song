using System;
using UnityEngine;

// Token: 0x020000FF RID: 255
public class BlackBars : MonoSingleton<BlackBars>
{
	// Token: 0x17000213 RID: 531
	// (get) Token: 0x06000858 RID: 2136 RVA: 0x00048287 File Offset: 0x00046487
	public bool visible
	{
		get
		{
			return this._visible;
		}
	}

	// Token: 0x06000859 RID: 2137 RVA: 0x00048290 File Offset: 0x00046490
	public void SetVisible(bool visible, BlackBarsReason reason)
	{
		BlackBarsReason visibleReason = this._visibleReason;
		if (visible)
		{
			this._visibleReason |= reason;
		}
		else
		{
			this._visibleReason &= ~reason;
		}
		if (this._visibleReason == visibleReason)
		{
			return;
		}
		bool flag = visibleReason > BlackBarsReason.None;
		bool flag2 = this._visibleReason > BlackBarsReason.None;
		if (flag2 != flag)
		{
			this.SetVisible_Internal(flag2, true);
		}
	}

	// Token: 0x0600085A RID: 2138 RVA: 0x000482EB File Offset: 0x000464EB
	private void Start()
	{
		this.SetVisible_Internal(false, false);
	}

	// Token: 0x0600085B RID: 2139 RVA: 0x000482F5 File Offset: 0x000464F5
	private void Update()
	{
	}

	// Token: 0x0600085C RID: 2140 RVA: 0x000482F8 File Offset: 0x000464F8
	private void SetVisible_Internal(bool visible, bool animated)
	{
		this._visible = visible;
		if (this._visible)
		{
			this.top.image.enabled = (this.bottom.image.enabled = true);
		}
		this.top.Animate(animated ? this.duration : 0f, delegate
		{
			if (visible)
			{
				this.top.topY = this.top.canvasHeight;
				this.bottom.bottomY = 0f;
				return;
			}
			this.top.bottomY = this.top.canvasHeight;
			this.bottom.topY = 0f;
		}).Then(delegate
		{
			if (!this._visible)
			{
				this.top.image.enabled = (this.bottom.image.enabled = false);
			}
		});
	}

	// Token: 0x04000A1B RID: 2587
	public float duration = 2f;

	// Token: 0x04000A1C RID: 2588
	public SLayout top;

	// Token: 0x04000A1D RID: 2589
	public SLayout bottom;

	// Token: 0x04000A1E RID: 2590
	private bool _visible;

	// Token: 0x04000A1F RID: 2591
	[SerializeField]
	[Disable]
	private BlackBarsReason _visibleReason;
}
