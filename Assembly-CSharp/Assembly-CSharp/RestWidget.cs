using System;
using UnityEngine;

// Token: 0x02000141 RID: 321
public class RestWidget : PromptWithVisibility
{
	// Token: 0x06000AE0 RID: 2784 RVA: 0x00058921 File Offset: 0x00056B21
	protected override bool ShouldShow()
	{
		return Runner.instance != null && MonoSingleton<RestStateController>.instance.active && !MonoSingleton<RestStateController>.instance.sleeping && !MonoSingleton<RestStateController>.instance.sleepPending;
	}

	// Token: 0x06000AE1 RID: 2785 RVA: 0x00058957 File Offset: 0x00056B57
	protected override void OnEnable()
	{
		base.OnEnable();
		Game.onUIPositionUpdate += this.OnUIPositionUpdate;
		this.RefreshLabels(true);
	}

	// Token: 0x06000AE2 RID: 2786 RVA: 0x00058977 File Offset: 0x00056B77
	private void OnDisable()
	{
		Game.onUIPositionUpdate -= this.OnUIPositionUpdate;
	}

	// Token: 0x06000AE3 RID: 2787 RVA: 0x0005898C File Offset: 0x00056B8C
	private void RefreshLabels(bool immediate)
	{
		string text;
		if (MonoSingleton<RestStateController>.instance.resting)
		{
			text = "Resting...";
		}
		else if (MonoSingleton<RestStateController>.instance.wantsSleepNotRest)
		{
			text = "Sleep";
		}
		else
		{
			text = "Rest";
		}
		if (this._activeLabelText != text)
		{
			this._visibleAvailableHeal = AvailableHeal.RefreshingLabel;
			SLayout oldLabel = this._activeLabel;
			if (oldLabel != null)
			{
				oldLabel.Animate(immediate ? 0f : this.labelCrossFadeDuration, delegate
				{
					oldLabel.alpha = 0f;
				}).Then(delegate
				{
					oldLabel.GetComponent<Prototype>().ReturnToPool();
				});
			}
			this._activeLabel = null;
			this._activeLabelText = text;
			if (this._activeLabelText != null)
			{
				SLayout newLabel = this._labelProto.Instantiate<SLayout>(null);
				newLabel.textMeshPro.text = this._activeLabelText;
				newLabel.alpha = 0f;
				newLabel.Animate(immediate ? 0f : this.labelCrossFadeDuration, delegate
				{
					newLabel.alpha = 1f;
				});
				this._activeLabel = newLabel;
			}
			bool activeSelf = this._holdLabel.gameObject.activeSelf;
			bool flag = !MonoSingleton<RestStateController>.instance.wantsSleepNotRest;
			if (!activeSelf && flag)
			{
				this._holdLabel.gameObject.SetActive(true);
				this._activeLabel.centerY = 0.5f * this._activeLabel.parentRect.height + 6f;
			}
			else if (activeSelf && !flag)
			{
				this._holdLabel.gameObject.SetActive(false);
				this._activeLabel.centerY = 0.5f * this._activeLabel.parentRect.height;
			}
		}
		Prop activeRestProp = PropsController.instance.activeRestProp;
		string text2 = ((activeRestProp != null) ? activeRestProp.inkListItemName : null);
		AvailableHeal availableHeal = ((Runner.instance == null) ? AvailableHeal.None : (MonoSingleton<RestStateController>.instance.wantsSleepNotRest ? Runner.instance.health.GetAvailableSleepComfort(text2) : Runner.instance.health.availableHeal));
		if (this._visibleAvailableHeal != availableHeal)
		{
			string text3 = "";
			string text4 = "";
			if (MonoSingleton<RestStateController>.instance.wantsSleepNotRest)
			{
				switch (availableHeal)
				{
				case AvailableHeal.None:
					text3 = "Exposed";
					text4 = "Maximum health will <color=#51D2FF>plummet</color>";
					break;
				case AvailableHeal.VerySlow:
					text3 = "Unpleasant";
					text4 = "Maximum health will <color=#51D2FF>suffer</color>";
					break;
				case AvailableHeal.Slow:
					text3 = "Sheltered";
					text4 = "Maximum health will <color=#51D2FF>decrease</color>";
					break;
				case AvailableHeal.Medium:
					text3 = "Comfortable";
					text4 = "Maximum health will <color=#36FF00>benefit</color>";
					break;
				case AvailableHeal.Full:
					text3 = "Excellent";
					text4 = "Maximum health will <color=#36FF00>improve</color>";
					break;
				case AvailableHeal.Magical:
					text3 = "Blessed";
					text4 = "Maximum health will <color=#36FF00>improve</color>";
					break;
				}
			}
			else
			{
				text3 = ((availableHeal == AvailableHeal.Magical) ? "Protected" : Narrative.instance.GetWeatherDescription(text2));
				if (availableHeal == AvailableHeal.Unnecessary)
				{
					text4 = "Health is full";
				}
				else if (availableHeal == AvailableHeal.None)
				{
					text4 = "Health regain <color=#51D2FF>impossible</color>";
				}
				else if (availableHeal == AvailableHeal.VerySlow)
				{
					text4 = "Health regain <color=#51D2FF>feeble</color>";
				}
				else if (availableHeal == AvailableHeal.Slow)
				{
					text4 = "Health regain will be <color=#51D2FF>slow</color>";
				}
				else if (availableHeal == AvailableHeal.Medium)
				{
					text4 = "Health regain will be normal";
				}
				else if (availableHeal == AvailableHeal.Magical)
				{
					text4 = "Health regain will be <color=#36FF00>blessed</color>";
				}
				else
				{
					text4 = "Health regain will be <color=#36FF00>fast</color>";
				}
			}
			this._temperatureTitle.textMeshPro.text = text3;
			this._temperatureDescription.textMeshPro.text = text4;
			this._visibleAvailableHeal = availableHeal;
		}
	}

	// Token: 0x06000AE4 RID: 2788 RVA: 0x00058D14 File Offset: 0x00056F14
	private void OnUIPositionUpdate(GameUI ui)
	{
		if (base.layout.groupAlpha == 0f)
		{
			return;
		}
		Vector2 vector = ui.WorldToCanvas(Runner.instance.transform.position + this.worldOffsetY * Vector3.up, default(Vector2));
		Vector2 size = base.layout.size;
		Rect rect = new Rect(vector.x - 0.5f * size.x, vector.y - size.y, size.x, size.y);
		rect = ui.ContstrainWithinSafeZones(rect);
		base.layout.position = rect.position;
		if (MonoSingleton<RestStateController>.instance.resting)
		{
			this._buttonBg.fillColor = this.restingFillColor;
		}
		else
		{
			this._buttonBg.fillColor = this.defaultFillColor;
		}
		this.RefreshLabels(false);
	}

	// Token: 0x04000D04 RID: 3332
	public float worldOffsetY = -5f;

	// Token: 0x04000D05 RID: 3333
	public Color restingFillColor = Color.blue.WithAlpha(0.5f);

	// Token: 0x04000D06 RID: 3334
	public Color defaultFillColor = Color.black.WithAlpha(0.5f);

	// Token: 0x04000D07 RID: 3335
	public float labelCrossFadeDuration = 0.3f;

	// Token: 0x04000D08 RID: 3336
	private string _activeLabelText;

	// Token: 0x04000D09 RID: 3337
	private SLayout _activeLabel;

	// Token: 0x04000D0A RID: 3338
	private AvailableHeal _visibleAvailableHeal = (AvailableHeal)(-1);

	// Token: 0x04000D0B RID: 3339
	[SerializeField]
	private Prototype _labelProto;

	// Token: 0x04000D0C RID: 3340
	[SerializeField]
	private SLayout _holdLabel;

	// Token: 0x04000D0D RID: 3341
	[SerializeField]
	private SLayout _temperatureTitle;

	// Token: 0x04000D0E RID: 3342
	[SerializeField]
	private SLayout _temperatureDescription;

	// Token: 0x04000D0F RID: 3343
	[SerializeField]
	private SLayout _buttonBg;
}
