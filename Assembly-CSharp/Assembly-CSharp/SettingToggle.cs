using System;
using UnityEngine;

// Token: 0x0200014C RID: 332
public class SettingToggle : SettingControl<bool>
{
	// Token: 0x06000B41 RID: 2881 RVA: 0x0005B398 File Offset: 0x00059598
	public override void Trigger()
	{
		this.currentValue = !this.currentValue;
		this._tick.Animate(0.1f, delegate
		{
			this._tick.scale = 1.5f;
		}).ThenAnimate(0.1f, delegate
		{
			this._tick.scale = 1f;
		});
		this.RefreshLayout();
		if (this.onChange != null)
		{
			this.onChange(this.currentValue);
		}
		if (this.onTrigger != null)
		{
			this.onTrigger(this.currentValue);
		}
	}

	// Token: 0x06000B42 RID: 2882 RVA: 0x0005B41F File Offset: 0x0005961F
	public override void RefreshLayout()
	{
		base.RefreshLayout();
		this._tick.Animate(0.2f, delegate
		{
			this._tick.alpha = (float)(this.currentValue ? 1 : 0);
			this._outline.color = (base.highlighted ? this.settings.highlightColor : Color.white);
			base.layout.groupAlpha = (this._disabled ? 0.3f : 1f);
		});
	}

	// Token: 0x04000D63 RID: 3427
	[SerializeField]
	private SLayout _tick;

	// Token: 0x04000D64 RID: 3428
	[SerializeField]
	private SLayout _outline;
}
