using System;
using UnityEngine;

// Token: 0x02000143 RID: 323
public class SettingButtonWithLabels : SettingControl<bool>
{
	// Token: 0x06000AEA RID: 2794 RVA: 0x00058F34 File Offset: 0x00057134
	public void Setup(string label, string extraLabel, Action onTrigger)
	{
		this._extraLabel.textMeshPro.text = extraLabel;
		base.Setup(label, true, null, delegate(bool _)
		{
			if (onTrigger != null)
			{
				onTrigger();
			}
		});
	}

	// Token: 0x06000AEB RID: 2795 RVA: 0x00058F74 File Offset: 0x00057174
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

	// Token: 0x06000AEC RID: 2796 RVA: 0x00058FAC File Offset: 0x000571AC
	public override void RefreshLayout()
	{
		this._buttonLabel.color = (base.highlighted ? this.settings.buttonLabelHighlightColor : this.settings.buttonLabelNormalColor);
		this._label.color = (base.highlighted ? this.settings.highlightColor : this.settings.labelColor);
		if (this._buttonBg != null)
		{
			this._buttonBg.color = (this._highlighted ? this.settings.highlightColor : this.settings.buttonBackgroundColor);
		}
		if (this._extraLabel.textMeshPro.text != "")
		{
			this._label.centerY = 0.4f * this._label.parentRect.height;
			this._extraLabel.centerY = 0.7f * this._label.parentRect.height;
			return;
		}
		this._label.centerY = 0.5f * this._label.parentRect.height;
	}

	// Token: 0x04000D11 RID: 3345
	[SerializeField]
	private SLayout _buttonBg;

	// Token: 0x04000D12 RID: 3346
	[SerializeField]
	private SLayout _buttonLabel;

	// Token: 0x04000D13 RID: 3347
	[SerializeField]
	private SLayout _extraLabel;
}
