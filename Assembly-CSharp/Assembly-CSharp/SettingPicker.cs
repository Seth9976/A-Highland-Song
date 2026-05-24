using System;
using UnityEngine;

// Token: 0x02000148 RID: 328
public class SettingPicker : SettingControl<string>
{
	// Token: 0x170002AB RID: 683
	// (get) Token: 0x06000B09 RID: 2825 RVA: 0x000595BF File Offset: 0x000577BF
	public bool requiresConfirmation
	{
		get
		{
			return this._newOptionIdx != this._originalOptionIdx;
		}
	}

	// Token: 0x06000B0A RID: 2826 RVA: 0x000595D4 File Offset: 0x000577D4
	public void Setup(string label, string[] options, string initialValue, Action<string> onChange, Action<string> onTrigger)
	{
		this._options = options;
		this._originalOptionIdx = (this._newOptionIdx = Mathf.Max(Array.IndexOf<string>(this._options, initialValue), 0));
		base.Setup(label, initialValue, onChange, onTrigger);
	}

	// Token: 0x06000B0B RID: 2827 RVA: 0x00059618 File Offset: 0x00057818
	public override void LeftRight(int amount)
	{
		int num = Mathf.Clamp(this._newOptionIdx + amount, 0, this._options.Length - 1);
		if (num != this._newOptionIdx)
		{
			this._newOptionIdx = num;
			if (this.onTrigger == null)
			{
				this._originalOptionIdx = this._newOptionIdx;
			}
			this.currentValue = this._options[this._newOptionIdx];
			this.RefreshLayout();
			if (this.onChange != null)
			{
				this.onChange(this.currentValue);
			}
		}
	}

	// Token: 0x06000B0C RID: 2828 RVA: 0x00059694 File Offset: 0x00057894
	public void Revert()
	{
		this._newOptionIdx = this._originalOptionIdx;
		this.currentValue = this._options[this._originalOptionIdx];
		this.RefreshLayout();
		if (this.onChange != null)
		{
			this.onChange(this.currentValue);
		}
	}

	// Token: 0x06000B0D RID: 2829 RVA: 0x000596D4 File Offset: 0x000578D4
	public override void Trigger()
	{
		this._originalOptionIdx = this._newOptionIdx;
		this.RefreshLayout();
		base.Trigger();
	}

	// Token: 0x06000B0E RID: 2830 RVA: 0x000596F0 File Offset: 0x000578F0
	public override void RefreshLayout()
	{
		base.RefreshLayout();
		this._pickText.textMeshPro.text = this.currentValue;
		base.layout.Animate(0.2f, 0f, SLayout.popCurve, delegate
		{
			if (this._originalOptionIdx != this._newOptionIdx)
			{
				this._pickBox.scale = 1.1f;
				this._pickBoxBG.alpha = 1f;
				this._pickButtonPrompt.groupAlpha = 1f;
				this._pickText.width = this._pickButtonPrompt.x;
				this._pickText.color = Color.white;
				return;
			}
			this._pickBox.scale = 1f;
			this._pickBoxBG.alpha = 0f;
			this._pickButtonPrompt.groupAlpha = 0f;
			this._pickText.width = this._pickBox.width;
			this._pickText.color = (base.highlighted ? this.settings.highlightColor : Color.white);
		});
		int num = Array.IndexOf<string>(this._options, this.currentValue);
		this._leftChevron.alpha = ((num > 0) ? 1f : 0.25f);
		this._rightChevron.alpha = ((num < this._options.Length - 1) ? 1f : 0.25f);
	}

	// Token: 0x04000D2C RID: 3372
	private string[] _options;

	// Token: 0x04000D2D RID: 3373
	private int _originalOptionIdx;

	// Token: 0x04000D2E RID: 3374
	private int _newOptionIdx;

	// Token: 0x04000D2F RID: 3375
	[SerializeField]
	private SLayout _pickBox;

	// Token: 0x04000D30 RID: 3376
	[SerializeField]
	private SLayout _pickBoxBG;

	// Token: 0x04000D31 RID: 3377
	[SerializeField]
	private SLayout _pickText;

	// Token: 0x04000D32 RID: 3378
	[SerializeField]
	private SLayout _pickButtonPrompt;

	// Token: 0x04000D33 RID: 3379
	[SerializeField]
	private SLayout _leftChevron;

	// Token: 0x04000D34 RID: 3380
	[SerializeField]
	private SLayout _rightChevron;
}
