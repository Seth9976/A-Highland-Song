using System;
using UnityEngine;

// Token: 0x0200010C RID: 268
public class DialogueButton : MonoBehaviour
{
	// Token: 0x17000246 RID: 582
	// (get) Token: 0x060008F6 RID: 2294 RVA: 0x0004B3A5 File Offset: 0x000495A5
	public bool highlighted
	{
		get
		{
			return this.state == DialogueButton.State.Highlighted;
		}
	}

	// Token: 0x17000247 RID: 583
	// (get) Token: 0x060008F7 RID: 2295 RVA: 0x0004B3B0 File Offset: 0x000495B0
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

	// Token: 0x060008F8 RID: 2296 RVA: 0x0004B3D2 File Offset: 0x000495D2
	public void Setup(string labelText)
	{
		this._label.textMeshPro.text = labelText;
		this.state = DialogueButton.State.Normal;
	}

	// Token: 0x060008F9 RID: 2297 RVA: 0x0004B3EC File Offset: 0x000495EC
	private void Update()
	{
		DialogueButtonSettings.Theme theme;
		switch (this.type)
		{
		case DialogueButton.Type.Primary:
			theme = this._settings.primaryTheme;
			break;
		case DialogueButton.Type.Secondary:
			theme = this._settings.secondaryTheme;
			break;
		case DialogueButton.Type.Danger:
			theme = this._settings.dangerTheme;
			break;
		default:
			throw new Exception("Unhandled theme type");
		}
		DialogueButtonSettings.Theme theme2 = theme;
		float num = (((float)this.state > this._interpolatedStateIdx) ? 0.05f : 0.3f);
		this._interpolatedStateIdx = Mathf.MoveTowards(this._interpolatedStateIdx, (float)this.state, Time.unscaledDeltaTime / num);
		float num2 = 0.5f * (Mathf.Sin(this._settings.pulseSpeed * Time.unscaledTime) + 1f);
		Color color = Color.Lerp(theme2.highlightedColor1, theme2.highlightedColor2, num2);
		if (this._interpolatedStateIdx <= 1f)
		{
			this._bg.color = Color.Lerp(theme2.normalColor, color, this._interpolatedStateIdx);
		}
		else if (this._interpolatedStateIdx >= 1f && this._interpolatedStateIdx <= 2f)
		{
			this._bg.color = Color.Lerp(color, theme2.pressedColor, this._interpolatedStateIdx - 1f);
		}
		float num3 = Mathf.Clamp01(this._interpolatedStateIdx);
		this._outline.outlineColor = theme2.outlineColor.WithAlpha(num3);
		float num4 = Mathf.Lerp(0.6f, 1f, num3);
		this._label.alpha = num4;
		if (this._baseCornerRadius == -1f)
		{
			this._baseCornerRadius = this._outline.roundRect.cornerRadius;
		}
		float num5 = this._settings.outlineMarginPulseRange.Lerp(num2);
		this._outline.rect = new Rect(-num5, -num5, this.layout.width + 2f * num5, this.layout.height + 2f * num5);
		this._outline.roundRect.cornerRadius = this._settings.outlinePulseCornerRadius.Lerp(num2);
	}

	// Token: 0x04000AC1 RID: 2753
	public DialogueButton.Type type;

	// Token: 0x04000AC2 RID: 2754
	[Disable]
	public DialogueButton.State state;

	// Token: 0x04000AC3 RID: 2755
	private SLayout _layout;

	// Token: 0x04000AC4 RID: 2756
	private float _interpolatedStateIdx;

	// Token: 0x04000AC5 RID: 2757
	private float _baseCornerRadius;

	// Token: 0x04000AC6 RID: 2758
	[SerializeField]
	private SLayout _bg;

	// Token: 0x04000AC7 RID: 2759
	[SerializeField]
	private SLayout _outline;

	// Token: 0x04000AC8 RID: 2760
	[SerializeField]
	private SLayout _label;

	// Token: 0x04000AC9 RID: 2761
	[SerializeField]
	private DialogueButtonSettings _settings;

	// Token: 0x0200032D RID: 813
	public enum Type
	{
		// Token: 0x04001815 RID: 6165
		Primary,
		// Token: 0x04001816 RID: 6166
		Secondary,
		// Token: 0x04001817 RID: 6167
		Danger
	}

	// Token: 0x0200032E RID: 814
	public enum State
	{
		// Token: 0x04001819 RID: 6169
		Normal,
		// Token: 0x0400181A RID: 6170
		Highlighted,
		// Token: 0x0400181B RID: 6171
		Pressing
	}
}
