using System;
using TMPro;
using UnityEngine;

// Token: 0x0200013A RID: 314
public class ProgressBanner : MonoSingleton<ProgressBanner>
{
	// Token: 0x1700029D RID: 669
	// (get) Token: 0x06000A9D RID: 2717 RVA: 0x00057BDE File Offset: 0x00055DDE
	// (set) Token: 0x06000A9E RID: 2718 RVA: 0x00057BE6 File Offset: 0x00055DE6
	public bool visible { get; private set; }

	// Token: 0x1700029E RID: 670
	// (get) Token: 0x06000A9F RID: 2719 RVA: 0x00057BEF File Offset: 0x00055DEF
	private SLayout layout
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

	// Token: 0x06000AA0 RID: 2720 RVA: 0x00057C11 File Offset: 0x00055E11
	private void Start()
	{
		this.ResetState();
	}

	// Token: 0x06000AA1 RID: 2721 RVA: 0x00057C19 File Offset: 0x00055E19
	private void SetGlow(float glowNorm)
	{
		this._text.textMeshPro.fontSharedMaterial.SetFloat(ShaderUtilities.ID_GlowPower, glowNorm * this._settings.glowMaxPower);
	}

	// Token: 0x06000AA2 RID: 2722 RVA: 0x00057C44 File Offset: 0x00055E44
	private void ResetState()
	{
		this.layout.groupAlpha = 0f;
		this.SetGlow(0f);
		this._leftIcon.alpha = 0f;
		this._rightIcon.alpha = 0f;
		this._underline.alpha = 0f;
	}

	// Token: 0x06000AA3 RID: 2723 RVA: 0x00057C9C File Offset: 0x00055E9C
	private void OnDisable()
	{
		this.SetGlow(0f);
	}

	// Token: 0x06000AA4 RID: 2724 RVA: 0x00057CAC File Offset: 0x00055EAC
	public void Show(string text)
	{
		if (this.visible)
		{
			Debug.LogError(string.Concat(new string[]
			{
				"Couldn't show PROGRESS banner with '",
				text,
				"' because it was already visible with '",
				this._text.textMeshPro.text,
				"'"
			}));
			return;
		}
		this.visible = true;
		this.ResetState();
		this._text.textMeshPro.text = text;
		this._text.textMeshPro.ForceMeshUpdate(false, false);
		this._text.width = this._text.textMeshPro.preferredWidth;
		this._text.centerX = this.layout.middleX;
		this._leftIcon.rightX = this._text.x - this._settings.iconMargin;
		this._rightIcon.x = this._text.rightX + this._settings.iconMargin;
		this.layout.scale = this._settings.startScale;
		this.layout.Animate(this._settings.transitionInDuration + this._settings.waitTime + this._settings.transitionOutDuration, delegate
		{
			this.layout.scale = this._settings.endScale;
		});
		this.layout.Animate(this._settings.transitionInDuration, 0f, this._settings.transitionInCurve, delegate
		{
			this.layout.groupAlpha = 1f;
		}).ThenAnimate(this._settings.transitionOutDuration, this._settings.waitTime, delegate
		{
			this.layout.groupAlpha = 0f;
		}).Then(delegate
		{
			this.visible = false;
		});
		this.layout.AnimateCustom(this._settings.glowDuration, this._settings.glowDelay, delegate(float t)
		{
			this.SetGlow(this._settings.glowCurve.Evaluate(t));
		});
		this.layout.Animate(this._settings.additionalElementFadeTime, this._settings.leftIconDelay, delegate
		{
			this._leftIcon.alpha = 1f;
		});
		this.layout.Animate(this._settings.additionalElementFadeTime, this._settings.rightIconDelay, delegate
		{
			this._rightIcon.alpha = 1f;
		});
		this.layout.Animate(this._settings.additionalElementFadeTime, this._settings.underlineDelay, delegate
		{
			this._underline.alpha = 1f;
		});
	}

	// Token: 0x06000AA5 RID: 2725 RVA: 0x00057F1A File Offset: 0x0005611A
	public void Clear()
	{
		this.layout.CancelAnimations();
		this.layout.groupAlpha = 0f;
		this.visible = false;
	}

	// Token: 0x04000CD6 RID: 3286
	private SLayout _layout;

	// Token: 0x04000CD7 RID: 3287
	[SerializeField]
	private SLayout _text;

	// Token: 0x04000CD8 RID: 3288
	[SerializeField]
	private SLayout _leftIcon;

	// Token: 0x04000CD9 RID: 3289
	[SerializeField]
	private SLayout _rightIcon;

	// Token: 0x04000CDA RID: 3290
	[SerializeField]
	private SLayout _underline;

	// Token: 0x04000CDB RID: 3291
	[SerializeField]
	private ProgressBannerSettings _settings;
}
