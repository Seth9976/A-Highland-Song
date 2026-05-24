using System;
using UnityEngine;

// Token: 0x02000133 RID: 307
public abstract class PromptWithVisibility : MonoBehaviour
{
	// Token: 0x06000A5A RID: 2650
	protected abstract bool ShouldShow();

	// Token: 0x17000294 RID: 660
	// (get) Token: 0x06000A5B RID: 2651 RVA: 0x0005658A File Offset: 0x0005478A
	public bool shownOrShowing
	{
		get
		{
			return this._shownOrShowing;
		}
	}

	// Token: 0x17000295 RID: 661
	// (get) Token: 0x06000A5C RID: 2652 RVA: 0x00056592 File Offset: 0x00054792
	public SLayout layout
	{
		get
		{
			if (this._layout == null)
			{
				this._layout = base.GetComponent<SLayout>();
			}
			return this._layout;
		}
	}

	// Token: 0x06000A5D RID: 2653 RVA: 0x000565B4 File Offset: 0x000547B4
	protected virtual void OnEnable()
	{
		this._shownOrShowing = this.ShouldShow();
		this.Layout();
	}

	// Token: 0x06000A5E RID: 2654 RVA: 0x000565C8 File Offset: 0x000547C8
	protected virtual void Update()
	{
		if (this.ShouldShow() && !this._shownOrShowing)
		{
			this._shownOrShowing = true;
			this.layout.Animate(this.fadeInDuration, new Action(this.Layout));
			return;
		}
		if (!this.ShouldShow() && this._shownOrShowing)
		{
			this._shownOrShowing = false;
			this.layout.Animate(this.fadeOutDuration, new Action(this.Layout));
		}
	}

	// Token: 0x06000A5F RID: 2655 RVA: 0x00056640 File Offset: 0x00054840
	private void Layout()
	{
		this.layout.groupAlpha = (float)(this._shownOrShowing ? 1 : 0);
	}

	// Token: 0x04000C8A RID: 3210
	public float fadeInDuration = 0.2f;

	// Token: 0x04000C8B RID: 3211
	public float fadeOutDuration = 0.2f;

	// Token: 0x04000C8C RID: 3212
	private SLayout _layout;

	// Token: 0x04000C8D RID: 3213
	protected bool _shownOrShowing;
}
