using System;
using ActionIcon;
using InControl;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000101 RID: 257
[RequireComponent(typeof(SLayout))]
public class ButtonPrompt : MonoBehaviour, ILayoutSelfController, ILayoutController, ILayoutGroup, ILayoutElement
{
	// Token: 0x14000008 RID: 8
	// (add) Token: 0x0600086C RID: 2156 RVA: 0x000485AC File Offset: 0x000467AC
	// (remove) Token: 0x0600086D RID: 2157 RVA: 0x000485E4 File Offset: 0x000467E4
	public event Action<ButtonPrompt> onPress;

	// Token: 0x14000009 RID: 9
	// (add) Token: 0x0600086E RID: 2158 RVA: 0x0004861C File Offset: 0x0004681C
	// (remove) Token: 0x0600086F RID: 2159 RVA: 0x00048654 File Offset: 0x00046854
	public event Action<ButtonPrompt> onPointerEnter;

	// Token: 0x1400000A RID: 10
	// (add) Token: 0x06000870 RID: 2160 RVA: 0x0004868C File Offset: 0x0004688C
	// (remove) Token: 0x06000871 RID: 2161 RVA: 0x000486C4 File Offset: 0x000468C4
	public event Action<ButtonPrompt> onPointerExit;

	// Token: 0x1700021A RID: 538
	// (get) Token: 0x06000872 RID: 2162 RVA: 0x000486F9 File Offset: 0x000468F9
	public float pressValue
	{
		get
		{
			if (this.action != null && this._visible)
			{
				return this.action.Value;
			}
			return 0f;
		}
	}

	// Token: 0x1700021B RID: 539
	// (get) Token: 0x06000873 RID: 2163 RVA: 0x0004871C File Offset: 0x0004691C
	public ActionSetsManager.ActionSetType actionSetType
	{
		get
		{
			return this._actionSetType;
		}
	}

	// Token: 0x1700021C RID: 540
	// (get) Token: 0x06000874 RID: 2164 RVA: 0x00048724 File Offset: 0x00046924
	public string actionName
	{
		get
		{
			return this._actionName;
		}
	}

	// Token: 0x1700021D RID: 541
	// (get) Token: 0x06000875 RID: 2165 RVA: 0x0004872C File Offset: 0x0004692C
	// (set) Token: 0x06000876 RID: 2166 RVA: 0x000487B9 File Offset: 0x000469B9
	public PlayerAction action
	{
		get
		{
			if (!this.hasAction)
			{
				return null;
			}
			if (this._action == null)
			{
				if (string.IsNullOrWhiteSpace(this.actionName))
				{
					Debug.LogWarning("Action name is empty!", this);
					return null;
				}
				this._action = ActionSetsManager.GetAction(this.actionSetType, this.actionName);
				if (this._action == null)
				{
					Debug.LogWarning("No action found in set " + this.actionSetType.ToString() + " matching name " + this.actionName, this);
				}
			}
			return this._action;
		}
		set
		{
			if (this._action != value)
			{
				this._action = value;
				this.hasAction = this._action != null;
				this.RefreshActionIcon();
			}
		}
	}

	// Token: 0x1700021E RID: 542
	// (get) Token: 0x06000877 RID: 2167 RVA: 0x000487E0 File Offset: 0x000469E0
	public ActionIconView actionIconView
	{
		get
		{
			if (this._actionIconView == null && this.iconLayout != null)
			{
				this._actionIconView = this.iconLayout.GetComponent<ActionIconView>();
			}
			return this._actionIconView;
		}
	}

	// Token: 0x1700021F RID: 543
	// (get) Token: 0x06000878 RID: 2168 RVA: 0x00048815 File Offset: 0x00046A15
	// (set) Token: 0x06000879 RID: 2169 RVA: 0x00048836 File Offset: 0x00046A36
	public string text
	{
		get
		{
			if (this.label)
			{
				return this.label.textMeshPro.text;
			}
			return null;
		}
		set
		{
			if (this.label == null)
			{
				return;
			}
			this.label.textMeshPro.text = value;
		}
	}

	// Token: 0x17000220 RID: 544
	// (get) Token: 0x0600087A RID: 2170 RVA: 0x00048858 File Offset: 0x00046A58
	// (set) Token: 0x0600087B RID: 2171 RVA: 0x00048860 File Offset: 0x00046A60
	public bool visible
	{
		get
		{
			return this._visible;
		}
		set
		{
			if (this._visible != value)
			{
				this._visible = value;
				this.RefreshVisibility();
				this.RefreshClickableButtonSetup();
			}
		}
	}

	// Token: 0x17000221 RID: 545
	// (get) Token: 0x0600087C RID: 2172 RVA: 0x0004887E File Offset: 0x00046A7E
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

	// Token: 0x17000222 RID: 546
	// (get) Token: 0x0600087D RID: 2173 RVA: 0x000488A0 File Offset: 0x00046AA0
	private float standardAlpha
	{
		get
		{
			if (!this.brightenNotGlow)
			{
				return 1f;
			}
			return this.darkenedAlpha;
		}
	}

	// Token: 0x17000223 RID: 547
	// (get) Token: 0x0600087E RID: 2174 RVA: 0x000488B6 File Offset: 0x00046AB6
	private bool currentlyClickableButton
	{
		get
		{
			return GameInput.activeInputType != GameInput.InputType.Gamepad && this.clickable && this._visible;
		}
	}

	// Token: 0x0600087F RID: 2175 RVA: 0x000488D2 File Offset: 0x00046AD2
	private void OnEnable()
	{
		GameInput.onInputTypeChanged = (Action<GameInput.InputType>)Delegate.Combine(GameInput.onInputTypeChanged, new Action<GameInput.InputType>(this.OnChangeInputType));
		this.CreateGlowHighlightIfNecessary();
		this.RefreshVisibility();
		this.RefreshClickableButtonSetup();
		this.RefreshActionIcon();
	}

	// Token: 0x06000880 RID: 2176 RVA: 0x0004890C File Offset: 0x00046B0C
	private void OnDisable()
	{
		GameInput.onInputTypeChanged = (Action<GameInput.InputType>)Delegate.Remove(GameInput.onInputTypeChanged, new Action<GameInput.InputType>(this.OnChangeInputType));
		this.SetButtonEnabled(false);
	}

	// Token: 0x06000881 RID: 2177 RVA: 0x00048938 File Offset: 0x00046B38
	private void Update()
	{
		if (this.action != null && this._visible)
		{
			bool wasPressed = this.action.WasPressed;
			if (wasPressed)
			{
				if (this.onPress != null)
				{
					this.action.ClearInputState();
				}
				if (!this.dontHighlight)
				{
					this.CreateGlowHighlightIfNecessary();
					if (this.brightenNotGlow)
					{
						this.layout.groupAlpha = 1f;
					}
					else
					{
						this._glowHighlight.alpha = this.settings.glowOpacity;
					}
				}
				if (this.onPress != null)
				{
					this.onPress(this);
				}
			}
			if (((this.holdable && this.action.WasReleased) || (!this.holdable && wasPressed)) && !this.dontHighlight)
			{
				if (this.brightenNotGlow)
				{
					this.layout.Animate(0.4f, delegate
					{
						this.layout.groupAlpha = this.standardAlpha;
					});
					return;
				}
				this._glowHighlight.Animate(0.4f, delegate
				{
					this._glowHighlight.alpha = 0f;
				});
			}
		}
	}

	// Token: 0x06000882 RID: 2178 RVA: 0x00048A3C File Offset: 0x00046C3C
	private void OnChangeInputType(GameInput.InputType newInputType)
	{
		this.PerformLayout();
	}

	// Token: 0x06000883 RID: 2179 RVA: 0x00048A44 File Offset: 0x00046C44
	private void RefreshClickableButtonSetup()
	{
		if (this.currentlyClickableButton)
		{
			if (this._background == null)
			{
				GameObject gameObject = new GameObject("Background");
				gameObject.transform.SetParent(base.transform, false);
				RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
				rectTransform.anchorMin = Vector2.zero;
				rectTransform.anchorMax = Vector2.one;
				rectTransform.sizeDelta = Vector2.zero;
				this._background = gameObject.AddComponent<SLayout>();
				gameObject.AddComponent<Image>().type = Image.Type.Sliced;
				this._background.transform.SetAsFirstSibling();
			}
			if (this._button == null)
			{
				this._button = base.gameObject.GetComponent<ExtendedButton>() ?? base.gameObject.AddComponent<ExtendedButton>();
				this._button.targetGraphic = this._background.image;
				this._button.transition = Selectable.Transition.ColorTint;
				this._button.navigation = new Navigation
				{
					mode = Navigation.Mode.None
				};
				this._button.enabled = false;
			}
			this.SetButtonEnabled(true);
			ColorBlock defaultColorBlock = ColorBlock.defaultColorBlock;
			defaultColorBlock.normalColor = Color.gray;
			defaultColorBlock.disabledColor = defaultColorBlock.normalColor;
			defaultColorBlock.highlightedColor = this.settings.highlightColor;
			defaultColorBlock.pressedColor = this.settings.pressedColor;
			defaultColorBlock.fadeDuration = 0f;
			defaultColorBlock.colorMultiplier = 2f;
			this._button.colors = defaultColorBlock;
			if (this.label && !this.isIconOnly)
			{
				this.label.textMeshPro.raycastTarget = false;
			}
			if (this.iconLayout)
			{
				this.iconLayout.gameObject.SetActive(this.iconShouldBeVisible);
			}
			SLayout.WithoutAnimating(new Action(this.RefreshClickableButtonStyle));
			return;
		}
		this.SetButtonEnabled(false);
		if (this._button != null)
		{
			this._button.enabled = false;
		}
		SLayout.WithoutAnimating(delegate
		{
			if (this._background)
			{
				this._background.color = Color.clear;
			}
			if (this.label)
			{
				this.label.color = (this.isIconOnly ? Color.clear : Color.white);
			}
		});
		if (this.iconLayout)
		{
			this.iconLayout.gameObject.SetActive(true);
		}
	}

	// Token: 0x06000884 RID: 2180 RVA: 0x00048C68 File Offset: 0x00046E68
	private void RefreshClickableButtonStyle()
	{
		if (!this.currentlyClickableButton)
		{
			Debug.LogError("Shouldn't be calling RefreshClickableButtonStyle when the ButtonPrompt isn't currentlyClickableButton");
		}
		if (this._background == null)
		{
			return;
		}
		if (this.isIconOnly)
		{
			this._background.image.sprite = null;
		}
		else
		{
			this._background.image.sprite = (this.isOutline ? this.settings.outlineBackgroundSprite : this.settings.backgroundSprite);
		}
		if (this.isOutline)
		{
			this._background.color = this.settings.outlineColor;
		}
		else if (this.isPrimary)
		{
			this._background.color = this.settings.primaryColor;
		}
		else if (this.isIconOnly)
		{
			this._background.color = Color.clear;
		}
		else
		{
			this._background.color = this.settings.secondaryColor;
		}
		if (this.label)
		{
			if (this.isPrimary || this.isOutline)
			{
				this.label.color = Color.white;
				return;
			}
			if (this.isIconOnly)
			{
				this.label.color = Color.clear;
				return;
			}
			this.label.color = this.settings.secondaryLabelColor;
		}
	}

	// Token: 0x06000885 RID: 2181 RVA: 0x00048DAE File Offset: 0x00046FAE
	private void RefreshVisibility()
	{
		this.layout.groupAlpha = (this._visible ? this.standardAlpha : 0f);
	}

	// Token: 0x06000886 RID: 2182 RVA: 0x00048DD0 File Offset: 0x00046FD0
	private void RefreshActionIcon()
	{
		if (Application.isPlaying && this.hasAction && this.actionIconView != null && this.actionIconView.mode == ActionIconView.Mode.Action)
		{
			this.actionIconView.SetAction(this.action, BackgroundType.AutoButton);
		}
	}

	// Token: 0x06000887 RID: 2183 RVA: 0x00048E10 File Offset: 0x00047010
	private void SetButtonEnabled(bool enabled)
	{
		if (this._button == null)
		{
			return;
		}
		if (enabled && !this._button.enabled)
		{
			this._button.enabled = true;
			this._button.onEnter.AddListener(new UnityAction<PointerEventData>(this.OnPointerEnter));
			this._button.onExit.AddListener(new UnityAction<PointerEventData>(this.OnPointerExit));
			this._button.onClick.AddListener(new UnityAction(this.OnDesktopClick));
			return;
		}
		if (!enabled && this._button.enabled)
		{
			this._button.enabled = false;
			this._button.onEnter.RemoveListener(new UnityAction<PointerEventData>(this.OnPointerEnter));
			this._button.onExit.RemoveListener(new UnityAction<PointerEventData>(this.OnPointerExit));
			this._button.onClick.RemoveListener(new UnityAction(this.OnDesktopClick));
		}
	}

	// Token: 0x06000888 RID: 2184 RVA: 0x00048F10 File Offset: 0x00047110
	private void CreateGlowHighlightIfNecessary()
	{
		if (this.dontHighlight || this._glowHighlight != null || this.brightenNotGlow)
		{
			return;
		}
		GameObject gameObject = new GameObject("GlowHighlight");
		gameObject.transform.SetParent(base.transform, false);
		gameObject.transform.SetSiblingIndex((this._background != null) ? 1 : 0);
		gameObject.AddComponent<Image>();
		this._glowHighlight = gameObject.AddComponent<SLayout>();
		this._glowHighlight.image.sprite = this.settings.glowSprite;
		this._glowHighlight.image.type = Image.Type.Sliced;
		this._glowHighlight.rectTransform.anchorMin = Vector2.zero;
		this._glowHighlight.rectTransform.anchorMax = Vector2.one;
		this._glowHighlight.rectTransform.sizeDelta = new Vector2(20f, 20f);
		this._glowHighlight.alpha = 0f;
	}

	// Token: 0x06000889 RID: 2185 RVA: 0x0004900E File Offset: 0x0004720E
	private void OnDesktopClick()
	{
		if (this.onPress != null)
		{
			this.onPress(this);
		}
	}

	// Token: 0x0600088A RID: 2186 RVA: 0x00049024 File Offset: 0x00047224
	private void OnPointerEnter(PointerEventData p)
	{
		if (this.onPointerEnter != null)
		{
			this.onPointerEnter(this);
		}
	}

	// Token: 0x0600088B RID: 2187 RVA: 0x0004903A File Offset: 0x0004723A
	private void OnPointerExit(PointerEventData p)
	{
		if (this.onPointerExit != null)
		{
			this.onPointerExit(this);
		}
	}

	// Token: 0x0600088C RID: 2188 RVA: 0x00049050 File Offset: 0x00047250
	[ContextMenu("Refresh layout")]
	public void PerformLayout()
	{
		this._performingManualLayout = true;
		if (Application.isPlaying)
		{
			this.RefreshClickableButtonSetup();
		}
		this.RefreshActionIcon();
		this.SetLayoutHorizontal();
		this.SetLayoutVertical();
		this._performingManualLayout = false;
	}

	// Token: 0x17000224 RID: 548
	// (get) Token: 0x0600088D RID: 2189 RVA: 0x0004907F File Offset: 0x0004727F
	private bool iconShouldBeVisible
	{
		get
		{
			return (GameInput.activeInputType != GameInput.InputType.KeyboardAndMouse || !this.clickable || !this.noIconOnClickable) && this.iconLayout != null;
		}
	}

	// Token: 0x17000225 RID: 549
	// (get) Token: 0x0600088E RID: 2190 RVA: 0x000490A9 File Offset: 0x000472A9
	private bool labelShouldBeVisible
	{
		get
		{
			return !this.isIconOnly && this.label != null;
		}
	}

	// Token: 0x17000226 RID: 550
	// (get) Token: 0x0600088F RID: 2191 RVA: 0x000490C1 File Offset: 0x000472C1
	private float outerHeight
	{
		get
		{
			if (this.isIconOnly && this.iconLayout)
			{
				return LayoutUtility.GetPreferredHeight(this.iconLayout.rectTransform);
			}
			return this.settings.outerHeight;
		}
	}

	// Token: 0x17000227 RID: 551
	// (get) Token: 0x06000890 RID: 2192 RVA: 0x000490F4 File Offset: 0x000472F4
	private float iconWidth
	{
		get
		{
			if (!this.iconShouldBeVisible)
			{
				return 0f;
			}
			return LayoutUtility.GetPreferredWidth(this.iconLayout.rectTransform);
		}
	}

	// Token: 0x17000228 RID: 552
	// (get) Token: 0x06000891 RID: 2193 RVA: 0x00049114 File Offset: 0x00047314
	private float labelWidth
	{
		get
		{
			if (!this.labelShouldBeVisible)
			{
				return 0f;
			}
			return LayoutUtility.GetPreferredWidth(this.label.rectTransform);
		}
	}

	// Token: 0x06000892 RID: 2194 RVA: 0x00049134 File Offset: 0x00047334
	public void SetLayoutHorizontal()
	{
		if (Application.isPlaying && this.layout.isAnimating && !this._performingManualLayout)
		{
			return;
		}
		bool iconShouldBeVisible = this.iconShouldBeVisible;
		bool labelShouldBeVisible = this.labelShouldBeVisible;
		float rightX = this.layout.rightX;
		if (iconShouldBeVisible)
		{
			RectTransform rectTransform = this.iconLayout.rectTransform;
			RectTransform rectTransform2 = this.iconLayout.rectTransform;
			Vector2 vector = new Vector2((this.align == ButtonPrompt.Align.Left) ? 0f : 1f, 0.5f);
			rectTransform2.anchorMax = vector;
			rectTransform.anchorMin = vector;
		}
		if (iconShouldBeVisible)
		{
			this.iconLayout.groupAlpha = (this.currentlyClickableButton ? 0.4f : 1f);
		}
		if (this.label)
		{
			this.label.gameObject.SetActive(labelShouldBeVisible);
		}
		if (labelShouldBeVisible)
		{
			this.label.textMeshPro.alignment = ((this.align == ButtonPrompt.Align.Left) ? TextAlignmentOptions.Left : TextAlignmentOptions.Right);
		}
		this.CalculateLayoutInputHorizontal();
		this.layout.width = this._calculatedWidth;
		if (this.align == ButtonPrompt.Align.Right)
		{
			this.layout.rightX = rightX;
		}
		float iconWidth = this.iconWidth;
		float labelWidth = this.labelWidth;
		if (iconShouldBeVisible)
		{
			this.iconLayout.width = iconWidth;
		}
		if (labelShouldBeVisible)
		{
			this.label.width = labelWidth;
		}
		float num = this._calculatedEdgeMargin;
		for (int i = 0; i < 2; i++)
		{
			if ((i == 0 && this.align == ButtonPrompt.Align.Left) || (i == 1 && this.align == ButtonPrompt.Align.Right))
			{
				if (iconShouldBeVisible)
				{
					this.iconLayout.x = num;
					num += iconWidth;
				}
			}
			else if (labelShouldBeVisible)
			{
				this.label.x = num;
				num += labelWidth;
			}
			if (labelShouldBeVisible && iconShouldBeVisible)
			{
				num += this.settings.padding;
			}
		}
		if (this.currentlyClickableButton)
		{
			this.RefreshClickableButtonStyle();
		}
	}

	// Token: 0x06000893 RID: 2195 RVA: 0x00049304 File Offset: 0x00047504
	public void SetLayoutVertical()
	{
		if (Application.isPlaying && this.layout.isAnimating && !this._performingManualLayout)
		{
			return;
		}
		float outerHeight = this.outerHeight;
		this.layout.height = outerHeight;
		float num = 0.5f * outerHeight;
		if (this.labelShouldBeVisible)
		{
			this.label.height = LayoutUtility.GetPreferredHeight(this.label.rectTransform);
			this.label.centerY = num;
		}
		if (this.iconShouldBeVisible)
		{
			this.iconLayout.height = LayoutUtility.GetPreferredHeight(this.iconLayout.rectTransform);
			this.iconLayout.centerY = num;
		}
	}

	// Token: 0x06000894 RID: 2196 RVA: 0x000493A8 File Offset: 0x000475A8
	public virtual void CalculateLayoutInputHorizontal()
	{
		bool iconShouldBeVisible = this.iconShouldBeVisible;
		bool labelShouldBeVisible = this.labelShouldBeVisible;
		float num = ((this.clickable && GameInput.activeInputType != GameInput.InputType.Gamepad) ? this.settings.horizontalMargin : 0f);
		if (this.isIconOnly)
		{
			num = 0f;
		}
		float num2 = 0f;
		if (iconShouldBeVisible && labelShouldBeVisible)
		{
			num2 = num + this.iconWidth + this.settings.padding + this.labelWidth + num;
		}
		else if (iconShouldBeVisible)
		{
			num2 = num + this.iconWidth + num;
		}
		else if (labelShouldBeVisible)
		{
			num2 = num + this.labelWidth + num;
		}
		float outerHeight = this.outerHeight;
		if (GameInput.activeInputType == GameInput.InputType.KeyboardAndMouse && this.clickable && num2 < outerHeight)
		{
			float num3 = num2;
			num2 = outerHeight;
			num += (num2 - num3) / 2f;
		}
		this._calculatedWidth = num2;
		this._calculatedEdgeMargin = num;
	}

	// Token: 0x06000895 RID: 2197 RVA: 0x00049482 File Offset: 0x00047682
	public virtual void CalculateLayoutInputVertical()
	{
		this._calculatedHeight = this.outerHeight;
	}

	// Token: 0x17000229 RID: 553
	// (get) Token: 0x06000896 RID: 2198 RVA: 0x00049490 File Offset: 0x00047690
	public virtual float minWidth
	{
		get
		{
			return this._calculatedWidth;
		}
	}

	// Token: 0x1700022A RID: 554
	// (get) Token: 0x06000897 RID: 2199 RVA: 0x00049498 File Offset: 0x00047698
	public virtual float preferredWidth
	{
		get
		{
			return this._calculatedWidth;
		}
	}

	// Token: 0x1700022B RID: 555
	// (get) Token: 0x06000898 RID: 2200 RVA: 0x000494A0 File Offset: 0x000476A0
	public virtual float flexibleWidth
	{
		get
		{
			return -1f;
		}
	}

	// Token: 0x1700022C RID: 556
	// (get) Token: 0x06000899 RID: 2201 RVA: 0x000494A7 File Offset: 0x000476A7
	public virtual float minHeight
	{
		get
		{
			return this._calculatedHeight;
		}
	}

	// Token: 0x1700022D RID: 557
	// (get) Token: 0x0600089A RID: 2202 RVA: 0x000494AF File Offset: 0x000476AF
	public virtual float preferredHeight
	{
		get
		{
			return this._calculatedHeight;
		}
	}

	// Token: 0x1700022E RID: 558
	// (get) Token: 0x0600089B RID: 2203 RVA: 0x000494B7 File Offset: 0x000476B7
	public virtual float flexibleHeight
	{
		get
		{
			return -1f;
		}
	}

	// Token: 0x1700022F RID: 559
	// (get) Token: 0x0600089C RID: 2204 RVA: 0x000494BE File Offset: 0x000476BE
	public virtual int layoutPriority
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x04000A26 RID: 2598
	public ButtonPrompt.Align align;

	// Token: 0x04000A27 RID: 2599
	public bool clickable;

	// Token: 0x04000A28 RID: 2600
	public bool holdable;

	// Token: 0x04000A29 RID: 2601
	public bool noIconOnClickable;

	// Token: 0x04000A2A RID: 2602
	public bool isPrimary;

	// Token: 0x04000A2B RID: 2603
	public bool isOutline;

	// Token: 0x04000A2C RID: 2604
	public bool isIconOnly;

	// Token: 0x04000A2D RID: 2605
	public bool dontHighlight;

	// Token: 0x04000A2E RID: 2606
	public bool brightenNotGlow;

	// Token: 0x04000A2F RID: 2607
	public float darkenedAlpha = 0.4f;

	// Token: 0x04000A30 RID: 2608
	public bool hasAction;

	// Token: 0x04000A31 RID: 2609
	[SerializeField]
	private ActionSetsManager.ActionSetType _actionSetType = ActionSetsManager.ActionSetType.Game;

	// Token: 0x04000A32 RID: 2610
	[SerializeField]
	private string _actionName = string.Empty;

	// Token: 0x04000A33 RID: 2611
	private PlayerAction _action;

	// Token: 0x04000A34 RID: 2612
	public ButtonPromptSettings settings;

	// Token: 0x04000A35 RID: 2613
	public SLayout iconLayout;

	// Token: 0x04000A36 RID: 2614
	public SLayout label;

	// Token: 0x04000A37 RID: 2615
	private ActionIconView _actionIconView;

	// Token: 0x04000A38 RID: 2616
	private bool _visible = true;

	// Token: 0x04000A39 RID: 2617
	private SLayout _layout;

	// Token: 0x04000A3A RID: 2618
	private float _calculatedWidth;

	// Token: 0x04000A3B RID: 2619
	private float _calculatedHeight;

	// Token: 0x04000A3C RID: 2620
	private float _calculatedEdgeMargin;

	// Token: 0x04000A3D RID: 2621
	[SerializeField]
	[HideInInspector]
	private ExtendedButton _button;

	// Token: 0x04000A3E RID: 2622
	[SerializeField]
	[HideInInspector]
	private SLayout _background;

	// Token: 0x04000A3F RID: 2623
	[SerializeField]
	[HideInInspector]
	private SLayout _glowHighlight;

	// Token: 0x04000A40 RID: 2624
	private bool _performingManualLayout;

	// Token: 0x02000326 RID: 806
	public enum Align
	{
		// Token: 0x04001806 RID: 6150
		Left,
		// Token: 0x04001807 RID: 6151
		Right
	}
}
