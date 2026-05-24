using System;
using UnityEngine;

// Token: 0x02000142 RID: 322
public class SettingButton : SettingControl<bool>
{
	// Token: 0x06000AE6 RID: 2790 RVA: 0x00058E54 File Offset: 0x00057054
	public void Setup(string label, Action onTrigger)
	{
		base.Setup(label, true, null, delegate(bool _)
		{
			if (onTrigger != null)
			{
				onTrigger();
			}
		});
	}

	// Token: 0x06000AE7 RID: 2791 RVA: 0x00058E83 File Offset: 0x00057083
	public override void Trigger()
	{
		this.RefreshLayout();
		if (this.onChange != null)
		{
			this.onChange(this.currentValue);
		}
		if (this.onTrigger != null)
		{
			this.onTrigger(true);
		}
	}

	// Token: 0x06000AE8 RID: 2792 RVA: 0x00058EB8 File Offset: 0x000570B8
	public override void RefreshLayout()
	{
		this._label.color = (base.highlighted ? this.settings.buttonLabelHighlightColor : this.settings.buttonLabelNormalColor);
		if (this._bg != null)
		{
			this._bg.color = (this._highlighted ? this.settings.highlightColor : this.settings.buttonBackgroundColor);
		}
	}

	// Token: 0x04000D10 RID: 3344
	[SerializeField]
	private SLayout _bg;
}
