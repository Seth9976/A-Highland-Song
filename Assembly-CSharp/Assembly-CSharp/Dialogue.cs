using System;
using UnityEngine;

// Token: 0x0200010B RID: 267
public class Dialogue : MonoSingleton<Dialogue>
{
	// Token: 0x17000244 RID: 580
	// (get) Token: 0x060008E9 RID: 2281 RVA: 0x0004AC03 File Offset: 0x00048E03
	private bool allowInput
	{
		get
		{
			return this.visible && GameInput.HasControl(this) && !this.layout.isAnimating;
		}
	}

	// Token: 0x17000245 RID: 581
	// (get) Token: 0x060008EA RID: 2282 RVA: 0x0004AC25 File Offset: 0x00048E25
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

	// Token: 0x060008EB RID: 2283 RVA: 0x0004AC48 File Offset: 0x00048E48
	public void Show(string title, string subtitle, string buttonPrimaryText, string buttonSecondaryText = null, bool primaryIsDanger = false, Dialogue.OnComplete onComplete = null)
	{
		if (this.visible)
		{
			return;
		}
		base.gameObject.SetActive(true);
		this.visible = true;
		Dialogue.wantsGameplayPaused = true;
		this.layout.canvas.enabled = true;
		this._title.textMeshPro.text = title;
		if (!string.IsNullOrWhiteSpace(subtitle))
		{
			this._subtitle.enabled = true;
			this._subtitle.color = Color.white;
			this._subtitle.textMeshPro.text = subtitle;
		}
		else
		{
			this._subtitle.enabled = false;
			this._subtitle.color = Color.clear;
		}
		this._buttonPrimary.type = (primaryIsDanger ? DialogueButton.Type.Danger : DialogueButton.Type.Primary);
		this._buttonPrimary.Setup(buttonPrimaryText);
		this._buttonPrimary.state = DialogueButton.State.Highlighted;
		if (!string.IsNullOrWhiteSpace(buttonSecondaryText))
		{
			this._buttonSecondary.gameObject.SetActive(true);
			this._buttonSecondary.Setup(buttonSecondaryText);
		}
		else
		{
			this._buttonSecondary.gameObject.SetActive(false);
		}
		if (this.layout.groupAlpha == 0f)
		{
			this._dialogueLayout.originY = this._originalY - 200f;
			this._dialogueLayout.scale = 0.6f;
			this._title.alpha = 0f;
			this._subtitle.alpha = 0f;
			this._buttonPrimary.layout.groupAlpha = 0f;
			this._buttonPrimary.layout.scale = 0.8f;
			if (this._buttonSecondary.isActiveAndEnabled)
			{
				this._buttonSecondary.layout.groupAlpha = 0f;
				this._buttonSecondary.layout.scale = 0.8f;
			}
		}
		this.RefreshLayout();
		this.layout.Animate(0.4f, 0f, SLayout.popCurve, delegate
		{
			this.layout.groupAlpha = 1f;
			this._dialogueLayout.originY = this._originalY;
			this._dialogueLayout.scale = 1f;
		});
		this.layout.Animate(0.2f, 0.15f, SLayout.popCurve, delegate
		{
			this._title.alpha = 1f;
			this.layout.AddDelay(0.1f);
			this._subtitle.alpha = 1f;
			this.layout.AddDelay(0.1f);
			if (this._buttonSecondary.isActiveAndEnabled)
			{
				this._buttonSecondary.layout.groupAlpha = 1f;
				this._buttonSecondary.layout.scale = 1f;
				this.layout.AddDelay(0.1f);
			}
			this._buttonPrimary.layout.groupAlpha = 1f;
			this._buttonPrimary.layout.scale = 1f;
		});
		GameInput.PushControlStack(this);
		this._onComplete = onComplete;
	}

	// Token: 0x060008EC RID: 2284 RVA: 0x0004AE70 File Offset: 0x00049070
	public void Hide()
	{
		if (!this.visible)
		{
			return;
		}
		this.visible = false;
		Dialogue.wantsGameplayPaused = false;
		GameInput.PopControlStack(this, true);
		this.layout.Animate(0.2f, 0f, SLayout.reversePopCurve, delegate
		{
			this.layout.groupAlpha = 0f;
			this._dialogueLayout.originY = this._originalY - 200f;
			this._dialogueLayout.scale = 0.6f;
		}).Then(delegate
		{
			base.gameObject.SetActive(false);
		});
	}

	// Token: 0x060008ED RID: 2285 RVA: 0x0004AED4 File Offset: 0x000490D4
	private void Start()
	{
		this.visible = false;
		Dialogue.wantsGameplayPaused = false;
		this.layout.groupAlpha = 0f;
		this.layout.canvas.enabled = false;
		this._originalY = this._dialogueLayout.originY;
		this._buttonsGap = this._buttonPrimary.layout.x - this._buttonSecondary.layout.rightX;
		this._bottomMargin = this._dialogueLayout.height - this._buttonPrimary.layout.bottomY;
		this._subtitleToButtonGap = this._buttonPrimary.layout.y - this._subtitle.bottomY;
		base.gameObject.SetActive(false);
	}

	// Token: 0x060008EE RID: 2286 RVA: 0x0004AF98 File Offset: 0x00049198
	private void Complete(Dialogue.Result result)
	{
		if (result != Dialogue.Result.Cancel)
		{
			DialogueButton dialogueButton = ((result == Dialogue.Result.Primary) ? this._buttonPrimary : this._buttonSecondary);
			dialogueButton.state = DialogueButton.State.Pressing;
			dialogueButton.layout.scale = 1.1f;
		}
		this.layout.After((result != Dialogue.Result.Cancel) ? 0.3f : 0f, delegate
		{
			this.Hide();
			if (this._onComplete != null)
			{
				this._onComplete(result);
			}
		});
	}

	// Token: 0x060008EF RID: 2287 RVA: 0x0004B01C File Offset: 0x0004921C
	private void Update()
	{
		if (!this.visible)
		{
			return;
		}
		if (this.allowInput)
		{
			if (GameInput.selectMenuItem)
			{
				GameInput.ClearInputState();
				AudioController.instance.PlayUI(UISound.MajorClick);
				if (this._buttonSecondary.isActiveAndEnabled && this._buttonSecondary.highlighted)
				{
					this.Complete(Dialogue.Result.Secondary);
					return;
				}
				this.Complete(Dialogue.Result.Primary);
				return;
			}
			else
			{
				if (GameInput.Back(this))
				{
					GameInput.ClearInputState();
					AudioController.instance.PlayUI(UISound.MinorClick);
					this.Complete(Dialogue.Result.Cancel);
					return;
				}
				if (GameInput.selectLeft)
				{
					if (this._buttonPrimary.highlighted && this._buttonSecondary.isActiveAndEnabled)
					{
						GameInput.ClearInputState();
						this._buttonPrimary.state = DialogueButton.State.Normal;
						this._buttonSecondary.state = DialogueButton.State.Highlighted;
						AudioController.instance.PlayUI(UISound.MinorClick);
						return;
					}
				}
				else if (GameInput.selectRight && this._buttonSecondary.highlighted)
				{
					GameInput.ClearInputState();
					this._buttonSecondary.state = DialogueButton.State.Normal;
					this._buttonPrimary.state = DialogueButton.State.Highlighted;
					AudioController.instance.PlayUI(UISound.MinorClick);
				}
			}
		}
	}

	// Token: 0x060008F0 RID: 2288 RVA: 0x0004B124 File Offset: 0x00049324
	private void RefreshLayout()
	{
		this._subtitle.textMeshPro.ForceMeshUpdate(false, false);
		this._subtitle.height = this._subtitle.textMeshPro.preferredHeight;
		this._buttonPrimary.layout.y = (this._buttonSecondary.layout.y = this._subtitle.bottomY + this._subtitleToButtonGap);
		float middleX = this._dialogueLayout.middleX;
		if (this._buttonSecondary.gameObject.activeSelf)
		{
			float num = this._buttonSecondary.layout.width + this._buttonsGap + this._buttonPrimary.layout.width;
			this._buttonSecondary.layout.x = middleX - 0.5f * num;
			this._buttonPrimary.layout.rightX = middleX + 0.5f * num;
		}
		else
		{
			this._buttonPrimary.layout.centerX = middleX;
		}
		this._dialogueLayout.height = this._buttonPrimary.layout.bottomY + this._bottomMargin;
		this._dialogueLayout.centerY = 0.5f * this._dialogueLayout.parentRect.height;
	}

	// Token: 0x04000AB4 RID: 2740
	[NonSerialized]
	public bool visible;

	// Token: 0x04000AB5 RID: 2741
	public static bool wantsGameplayPaused;

	// Token: 0x04000AB6 RID: 2742
	private SLayout _layout;

	// Token: 0x04000AB7 RID: 2743
	private Dialogue.OnComplete _onComplete;

	// Token: 0x04000AB8 RID: 2744
	private float _originalY;

	// Token: 0x04000AB9 RID: 2745
	private float _buttonsGap;

	// Token: 0x04000ABA RID: 2746
	private float _bottomMargin;

	// Token: 0x04000ABB RID: 2747
	private float _subtitleToButtonGap;

	// Token: 0x04000ABC RID: 2748
	[SerializeField]
	private SLayout _dialogueLayout;

	// Token: 0x04000ABD RID: 2749
	[SerializeField]
	private SLayout _title;

	// Token: 0x04000ABE RID: 2750
	[SerializeField]
	private SLayout _subtitle;

	// Token: 0x04000ABF RID: 2751
	[SerializeField]
	private DialogueButton _buttonPrimary;

	// Token: 0x04000AC0 RID: 2752
	[SerializeField]
	private DialogueButton _buttonSecondary;

	// Token: 0x0200032A RID: 810
	public enum Result
	{
		// Token: 0x0400180F RID: 6159
		Primary,
		// Token: 0x04001810 RID: 6160
		Secondary,
		// Token: 0x04001811 RID: 6161
		Cancel
	}

	// Token: 0x0200032B RID: 811
	// (Invoke) Token: 0x060016CF RID: 5839
	public delegate void OnComplete(Dialogue.Result result);
}
