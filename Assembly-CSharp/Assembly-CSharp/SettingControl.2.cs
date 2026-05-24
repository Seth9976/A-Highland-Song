using System;
using UnityEngine;

// Token: 0x02000145 RID: 325
public class SettingControl<T> : SettingControl
{
	// Token: 0x06000AF8 RID: 2808 RVA: 0x00059168 File Offset: 0x00057368
	public void Setup(string label, T initialValue, Action<T> onChange, Action<T> onTrigger)
	{
		string text = LineBreaker.InsertNewlines(label, (float)this.settings.maxLabelCharactersPerLine);
		this._label.textMeshPro.text = text;
		this.onChange = onChange;
		this.onTrigger = onTrigger;
		this.currentValue = initialValue;
		this._highlighted = false;
		SLayout.WithoutAnimating(delegate
		{
			this.RefreshLayout();
		});
	}

	// Token: 0x06000AF9 RID: 2809 RVA: 0x000591C7 File Offset: 0x000573C7
	public override void RefreshLayout()
	{
		this._label.color = (base.highlighted ? this.settings.highlightColor : this.settings.labelColor);
	}

	// Token: 0x06000AFA RID: 2810 RVA: 0x000591F4 File Offset: 0x000573F4
	public override void Trigger()
	{
		if (this.onTrigger != null)
		{
			this.onTrigger(this.currentValue);
		}
	}

	// Token: 0x06000AFB RID: 2811 RVA: 0x0005920F File Offset: 0x0005740F
	public override void LeftRight(int amount)
	{
	}

	// Token: 0x04000D19 RID: 3353
	public T currentValue;

	// Token: 0x04000D1A RID: 3354
	protected Action<T> onChange;

	// Token: 0x04000D1B RID: 3355
	protected Action<T> onTrigger;

	// Token: 0x04000D1C RID: 3356
	[SerializeField]
	protected SLayout _label;
}
