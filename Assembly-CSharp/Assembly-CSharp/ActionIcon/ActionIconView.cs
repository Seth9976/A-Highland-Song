using System;
using System.Collections.Generic;
using InControl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ActionIcon
{
	// Token: 0x0200023B RID: 571
	[RequireComponent(typeof(Image))]
	public class ActionIconView : MonoBehaviour
	{
		// Token: 0x06001470 RID: 5232 RVA: 0x0008DC20 File Offset: 0x0008BE20
		public static void RefreshAllImmediate()
		{
			foreach (ActionIconView actionIconView in ActionIconView.all)
			{
				actionIconView.RefreshImmediate();
			}
		}

		// Token: 0x06001471 RID: 5233 RVA: 0x0008DC70 File Offset: 0x0008BE70
		public void SetAction(PlayerAction action, BackgroundType backgroundType = BackgroundType.Auto)
		{
			this._mode = ActionIconView.Mode.Action;
			this._action = action;
			this._actionName = action.Name;
			this._backgroundType = backgroundType;
			this.RefreshImmediate();
		}

		// Token: 0x06001472 RID: 5234 RVA: 0x0008DC99 File Offset: 0x0008BE99
		public void SetAction(ActionSetsManager.ActionSetType actionSetType, string actionName)
		{
			if (this._actionSetType == actionSetType && this._actionName == actionName)
			{
				return;
			}
			this._mode = ActionIconView.Mode.Action;
			this._actionSetType = actionSetType;
			this._actionName = actionName;
			this.RefreshImmediate();
		}

		// Token: 0x06001473 RID: 5235 RVA: 0x0008DCCE File Offset: 0x0008BECE
		public void SetIconType(ActionIconView.IconType icon)
		{
			this._mode = ActionIconView.Mode.Icon;
			this._iconType = icon;
			this.RefreshImmediate();
		}

		// Token: 0x06001474 RID: 5236 RVA: 0x0008DCE4 File Offset: 0x0008BEE4
		public void SetText(string text, BackgroundType backgroundType = BackgroundType.Auto)
		{
			this._mode = ActionIconView.Mode.Text;
			this._text = text;
			this.RefreshImmediate();
		}

		// Token: 0x06001475 RID: 5237 RVA: 0x0008DCFA File Offset: 0x0008BEFA
		public void SetSprite(Sprite sprite, BackgroundType backgroundType = BackgroundType.Auto)
		{
			this._mode = ActionIconView.Mode.Sprite;
			this._sprite = sprite;
			this._backgroundType = backgroundType;
			this.RefreshImmediate();
		}

		// Token: 0x06001476 RID: 5238 RVA: 0x0008DD17 File Offset: 0x0008BF17
		public void SetInputControlType(InputControlType inputControlType, BackgroundType backgroundType = BackgroundType.Auto)
		{
			this._mode = ActionIconView.Mode.InputControlType;
			this._inputControlType = inputControlType;
			this._backgroundType = backgroundType;
			this.RefreshImmediate();
		}

		// Token: 0x06001477 RID: 5239 RVA: 0x0008DD34 File Offset: 0x0008BF34
		public void TweenVariant(IconVariant targetVariant, bool evenIfInStateAlready = false)
		{
			if (!evenIfInStateAlready && this._variant == targetVariant)
			{
				return;
			}
			ActionIconViewData currentValue = this._viewDataTween.currentValue;
			IconVariant variant = this._variant;
			this._variant = targetVariant;
			ActionIconViewData targetData = this.GetTargetData(ref currentValue);
			float num = 0.1f;
			if (this._variant == IconVariant.Standard && variant == IconVariant.Highlight)
			{
				num = 1f;
			}
			this._viewDataTween.Tween(currentValue, targetData, num);
		}

		// Token: 0x06001478 RID: 5240 RVA: 0x0008DD98 File Offset: 0x0008BF98
		private void TweenToHighlightState()
		{
			if (this._variant == IconVariant.Highlight)
			{
				return;
			}
			ActionIconViewData currentValue = this._viewDataTween.currentValue;
			this._variant = IconVariant.Highlight;
			ActionIconViewData targetData = this.GetTargetData(ref currentValue);
			this._viewDataTween.Tween(currentValue, targetData, 0.1f);
		}

		// Token: 0x06001479 RID: 5241 RVA: 0x0008DDDD File Offset: 0x0008BFDD
		[ContextMenu("Force refresh")]
		public void ForceRefreshAction()
		{
			this._action = null;
			this.RefreshImmediate();
		}

		// Token: 0x0600147A RID: 5242 RVA: 0x0008DDEC File Offset: 0x0008BFEC
		public static void ForceRefreshAll()
		{
			ActionSetsManager.ResetActionSets();
			ActionIconView[] array = Object.FindObjectsOfType<ActionIconView>(true);
			for (int i = 0; i < array.Length; i++)
			{
				array[i].ForceRefreshAction();
			}
		}

		// Token: 0x0600147B RID: 5243 RVA: 0x0008DE1B File Offset: 0x0008C01B
		public void RefreshImmediate()
		{
			this._viewDataTween.Reset(this.GetTargetData());
		}

		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x0600147C RID: 5244 RVA: 0x0008DE2E File Offset: 0x0008C02E
		public ActionIconView.ScalingMode scalingMode
		{
			get
			{
				return this._scalingMode;
			}
		}

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x0600147D RID: 5245 RVA: 0x0008DE36 File Offset: 0x0008C036
		public ActionIconView.Mode mode
		{
			get
			{
				return this._mode;
			}
		}

		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x0600147E RID: 5246 RVA: 0x0008DE3E File Offset: 0x0008C03E
		public ActionSetsManager.ActionSetType actionSetType
		{
			get
			{
				return this._actionSetType;
			}
		}

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x0600147F RID: 5247 RVA: 0x0008DE46 File Offset: 0x0008C046
		// (set) Token: 0x06001480 RID: 5248 RVA: 0x0008DE4E File Offset: 0x0008C04E
		public string actionName
		{
			get
			{
				return this._actionName;
			}
			set
			{
				if (this._actionName != value)
				{
					this._actionName = value;
					this.ForceRefreshAction();
				}
			}
		}

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x06001481 RID: 5249 RVA: 0x0008DE6C File Offset: 0x0008C06C
		public PlayerAction action
		{
			get
			{
				if (this._action == null)
				{
					if (string.IsNullOrWhiteSpace(this.actionName))
					{
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
		}

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x06001482 RID: 5250 RVA: 0x0008DEE4 File Offset: 0x0008C0E4
		public InputControlType inputControlType
		{
			get
			{
				return this._inputControlType;
			}
		}

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x06001483 RID: 5251 RVA: 0x0008DEEC File Offset: 0x0008C0EC
		// (set) Token: 0x06001484 RID: 5252 RVA: 0x0008DEF4 File Offset: 0x0008C0F4
		public List<BindingSourceType> bindingSourceTypesOverride { get; set; }

		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x06001485 RID: 5253 RVA: 0x0008DEFD File Offset: 0x0008C0FD
		public Key key
		{
			get
			{
				return this._key;
			}
		}

		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x06001486 RID: 5254 RVA: 0x0008DF05 File Offset: 0x0008C105
		public Mouse mouse
		{
			get
			{
				return this._mouse;
			}
		}

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x06001487 RID: 5255 RVA: 0x0008DF0D File Offset: 0x0008C10D
		public Sprite sprite
		{
			get
			{
				return this._sprite;
			}
		}

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x06001488 RID: 5256 RVA: 0x0008DF15 File Offset: 0x0008C115
		public BackgroundType backgroundType
		{
			get
			{
				return this._backgroundType;
			}
		}

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x06001489 RID: 5257 RVA: 0x0008DF1D File Offset: 0x0008C11D
		// (set) Token: 0x0600148A RID: 5258 RVA: 0x0008DF25 File Offset: 0x0008C125
		public IconVariant variant
		{
			get
			{
				return this._variant;
			}
			set
			{
				this._variant = value;
				this.RefreshImmediate();
			}
		}

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x0600148B RID: 5259 RVA: 0x0008DF34 File Offset: 0x0008C134
		public ActionIconControllerType controllerTypeOverride
		{
			get
			{
				return this._controllerTypeOverride;
			}
		}

		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x0600148C RID: 5260 RVA: 0x0008DF3C File Offset: 0x0008C13C
		public Image backgroundImage
		{
			get
			{
				if (this._backgroundImage == null)
				{
					this._backgroundImage = base.gameObject.GetComponent<Image>();
					if (this._backgroundImage == null)
					{
						this._backgroundImage = base.gameObject.AddComponent<Image>();
					}
					this._backgroundImage.type = Image.Type.Sliced;
				}
				return this._backgroundImage;
			}
		}

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x0600148D RID: 5261 RVA: 0x0008DF9C File Offset: 0x0008C19C
		private Image arrowImage
		{
			get
			{
				if (this._arrowImage == null)
				{
					GameObject gameObject = new GameObject("Auto Icon View Arrow");
					gameObject.transform.SetParent(base.transform, false);
					RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
					rectTransform.anchoredPosition = Vector2.zero;
					rectTransform.SetAsFirstSibling();
					this._arrowImage = rectTransform.gameObject.AddComponent<Image>();
					this._arrowImage.color = Color.clear;
				}
				return this._arrowImage;
			}
		}

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x0600148E RID: 5262 RVA: 0x0008E014 File Offset: 0x0008C214
		private Image highlightImage
		{
			get
			{
				if (this._highlightImage == null)
				{
					GameObject gameObject = new GameObject("Auto Icon View Highlight");
					gameObject.transform.SetParent(base.transform, false);
					RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
					rectTransform.anchoredPosition = Vector2.zero;
					rectTransform.SetAsFirstSibling();
					this._highlightImage = rectTransform.gameObject.AddComponent<Image>();
					this._highlightImage.color = Color.clear;
				}
				return this._highlightImage;
			}
		}

		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x0600148F RID: 5263 RVA: 0x0008E08C File Offset: 0x0008C28C
		private RectTransform iconContainer
		{
			get
			{
				if (this._iconContainer == null)
				{
					GameObject gameObject = new GameObject("Auto Icon View Icon");
					gameObject.transform.SetParent(base.transform, false);
					this._iconContainer = gameObject.AddComponent<RectTransform>();
					this._iconContainer.anchoredPosition = Vector2.zero;
					this._iconContainer.anchorMin = Vector2.zero;
					this._iconContainer.anchorMax = Vector2.one;
					this._iconContainer.sizeDelta = Vector2.zero;
					this._iconContainer.SetAsLastSibling();
				}
				return this._iconContainer;
			}
		}

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x06001490 RID: 5264 RVA: 0x0008E124 File Offset: 0x0008C324
		public TextMeshProUGUI iconTextMesh
		{
			get
			{
				if (this._iconTextMesh == null)
				{
					GameObject gameObject = new GameObject("Auto Icon View Text");
					gameObject.transform.SetParent(this.iconContainer, false);
					RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
					rectTransform.anchoredPosition = Vector2.zero;
					rectTransform.anchorMin = Vector2.zero;
					rectTransform.anchorMax = Vector2.one;
					rectTransform.sizeDelta = Vector2.zero;
					this._iconTextMesh = gameObject.gameObject.AddComponent<TextMeshProUGUI>();
					this._iconTextMesh.font = ActionIconUtils.iconDatabase.font;
					this._iconTextMesh.color = Color.clear;
					this._iconTextMesh.enableAutoSizing = true;
					this._iconTextMesh.fontSizeMin = 6f;
					this._iconTextMesh.alignment = TextAlignmentOptions.Center;
					this._iconTextMesh.enableWordWrapping = false;
					rectTransform.sizeDelta = Vector2.zero;
					this._iconTextMesh.enabled = false;
				}
				return this._iconTextMesh;
			}
		}

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x06001491 RID: 5265 RVA: 0x0008E21C File Offset: 0x0008C41C
		public Image iconImage
		{
			get
			{
				if (this._iconImage == null)
				{
					GameObject gameObject = new GameObject("Auto Icon View Image");
					gameObject.transform.SetParent(this.iconContainer, false);
					gameObject.AddComponent<RectTransform>().anchoredPosition = Vector2.zero;
					this._iconImage = gameObject.gameObject.AddComponent<Image>();
					this._iconImage.color = Color.clear;
					this._iconImage.enabled = false;
				}
				return this._iconImage;
			}
		}

		// Token: 0x06001492 RID: 5266 RVA: 0x0008E297 File Offset: 0x0008C497
		private void OnEnable()
		{
			if (this._viewDataTween == null)
			{
				this._viewDataTween = new ActionIconViewDataTween();
			}
			this._viewDataTween.OnChange += this.OnChangeViewData;
			ActionIconView.all.Add(this);
			this.Init();
		}

		// Token: 0x06001493 RID: 5267 RVA: 0x0008E2D4 File Offset: 0x0008C4D4
		private void OnDisable()
		{
			this._viewDataTween.OnChange -= this.OnChangeViewData;
			ActionIconView.all.Remove(this);
		}

		// Token: 0x06001494 RID: 5268 RVA: 0x0008E2F9 File Offset: 0x0008C4F9
		private void Init()
		{
			this._viewDataTween.Reset(this.GetTargetData());
		}

		// Token: 0x06001495 RID: 5269 RVA: 0x0008E30C File Offset: 0x0008C50C
		private void Update()
		{
			this._viewDataTween.Update();
		}

		// Token: 0x06001496 RID: 5270 RVA: 0x0008E31A File Offset: 0x0008C51A
		private void OnChangeViewData(ActionIconViewData viewData)
		{
			this.ApplyData(viewData);
		}

		// Token: 0x06001497 RID: 5271 RVA: 0x0008E324 File Offset: 0x0008C524
		private ActionIconViewData GetTargetData()
		{
			if (this._mode == ActionIconView.Mode.Action)
			{
				return ActionIconUtils.GetDataFromAction(this.action, this.backgroundType, this.variant, (this.controllerTypeOverride == ActionIconControllerType.Auto) ? GameInput.GetActionIconControllerType() : this.controllerTypeOverride, this.bindingSourceTypesOverride);
			}
			if (this._mode == ActionIconView.Mode.InputControlType)
			{
				return ActionIconUtils.GetDataFromInputControlType(this.inputControlType, this.backgroundType, this.variant, (this.controllerTypeOverride == ActionIconControllerType.Auto) ? GameInput.GetActionIconControllerType() : this.controllerTypeOverride);
			}
			if (this._mode == ActionIconView.Mode.Key)
			{
				return ActionIconUtils.GetDataFromKey(this.key, this.backgroundType, this.variant);
			}
			if (this._mode == ActionIconView.Mode.Mouse)
			{
				return ActionIconUtils.GetDataFromMouse(this.mouse, this.backgroundType, this.variant);
			}
			if (this._mode == ActionIconView.Mode.Icon)
			{
				return ActionIconUtils.GetDataFromIconType(this._iconType, this.variant);
			}
			if (this._mode == ActionIconView.Mode.Text)
			{
				return ActionIconUtils.GetDataFromText(this._text, this.backgroundType, this.variant);
			}
			return ActionIconUtils.GetDataFromSprite(this._sprite, this.backgroundType, this.variant);
		}

		// Token: 0x06001498 RID: 5272 RVA: 0x0008E437 File Offset: 0x0008C637
		private ActionIconViewData GetTargetData(ref ActionIconViewData currentData)
		{
			return this.GetTargetData();
		}

		// Token: 0x06001499 RID: 5273 RVA: 0x0008E43F File Offset: 0x0008C63F
		private void CorrectColors(ref ActionIconViewData currentData, ref ActionIconViewData targetData)
		{
			this.CorrectColors(ref currentData.highlightColor, ref targetData.highlightColor);
			this.CorrectColors(ref currentData.iconColor, ref targetData.iconColor);
		}

		// Token: 0x0600149A RID: 5274 RVA: 0x0008E468 File Offset: 0x0008C668
		private void CorrectColors(ref Color currentData, ref Color targetData)
		{
			if (currentData == Color.clear)
			{
				currentData = targetData.WithAlpha(0f);
			}
			if (targetData == Color.clear)
			{
				targetData = currentData.WithAlpha(0f);
			}
		}

		// Token: 0x0600149B RID: 5275 RVA: 0x0008E4C8 File Offset: 0x0008C6C8
		private void ApplyData(ActionIconViewData data)
		{
			if (data.background.backgroundSprite != null)
			{
				this.backgroundImage.sprite = data.background.backgroundSprite;
				this.backgroundImage.enabled = true;
				if (this.scalingMode == ActionIconView.ScalingMode.Auto)
				{
					if (data.background.size != Vector2.zero)
					{
						this.backgroundImage.rectTransform.anchorMax = this.backgroundImage.rectTransform.anchorMin;
						this.backgroundImage.rectTransform.sizeDelta = data.background.size;
						this.backgroundImage.SetAllDirty();
					}
					else
					{
						this.backgroundImage.SetNativeSize();
					}
				}
			}
			else if (this._backgroundImage != null)
			{
				this.backgroundImage.enabled = false;
			}
			if (data.background.highlightSprite != null)
			{
				this.highlightImage.sprite = data.background.highlightSprite;
				this.highlightImage.color = data.highlightColor;
				this.highlightImage.enabled = true;
				if (this.scalingMode == ActionIconView.ScalingMode.Auto)
				{
					this.highlightImage.SetNativeSize();
				}
			}
			else if (this._highlightImage != null)
			{
				this.highlightImage.enabled = false;
			}
			if (!string.IsNullOrEmpty(data.iconText))
			{
				this.iconTextMesh.text = data.iconText;
				this.iconTextMesh.color = data.iconColor;
				this.iconTextMesh.enabled = true;
			}
			else if (this._iconTextMesh != null)
			{
				this.iconTextMesh.enabled = false;
			}
			if (data.icon != null)
			{
				this.iconImage.sprite = data.icon;
				this.iconImage.color = data.iconColor;
				this.iconImage.enabled = true;
				this.iconImage.SetNativeSize();
			}
			else if (this._iconImage != null)
			{
				this.iconImage.enabled = false;
			}
			if (this._iconContainer != null)
			{
				this.iconContainer.anchorMin = data.background.textAnchorMin;
				this.iconContainer.anchorMax = data.background.textAnchorMax;
			}
			base.transform.localScale = Vector3.one * data.scale;
		}

		// Token: 0x0400137C RID: 4988
		private static List<ActionIconView> all = new List<ActionIconView>();

		// Token: 0x0400137D RID: 4989
		private const float coloredTweenTime = 0.1f;

		// Token: 0x0400137E RID: 4990
		private const float highlightedToColoredTweenTime = 1f;

		// Token: 0x0400137F RID: 4991
		[SerializeField]
		private ActionIconView.ScalingMode _scalingMode;

		// Token: 0x04001380 RID: 4992
		[SerializeField]
		private ActionIconView.Mode _mode;

		// Token: 0x04001381 RID: 4993
		[SerializeField]
		private ActionSetsManager.ActionSetType _actionSetType;

		// Token: 0x04001382 RID: 4994
		[SerializeField]
		private string _actionName;

		// Token: 0x04001383 RID: 4995
		private PlayerAction _action;

		// Token: 0x04001384 RID: 4996
		[SerializeField]
		private InputControlType _inputControlType;

		// Token: 0x04001386 RID: 4998
		[SerializeField]
		private Key _key;

		// Token: 0x04001387 RID: 4999
		[SerializeField]
		private Mouse _mouse;

		// Token: 0x04001388 RID: 5000
		[SerializeField]
		private ActionIconView.IconType _iconType;

		// Token: 0x04001389 RID: 5001
		[SerializeField]
		private string _text = string.Empty;

		// Token: 0x0400138A RID: 5002
		[SerializeField]
		private Sprite _sprite;

		// Token: 0x0400138B RID: 5003
		[SerializeField]
		private BackgroundType _backgroundType;

		// Token: 0x0400138C RID: 5004
		[SerializeField]
		private IconVariant _variant;

		// Token: 0x0400138D RID: 5005
		[SerializeField]
		private ActionIconControllerType _controllerTypeOverride;

		// Token: 0x0400138E RID: 5006
		[SerializeField]
		private Image _backgroundImage;

		// Token: 0x0400138F RID: 5007
		[SerializeField]
		private Image _arrowImage;

		// Token: 0x04001390 RID: 5008
		[SerializeField]
		private Image _highlightImage;

		// Token: 0x04001391 RID: 5009
		[SerializeField]
		private RectTransform _iconContainer;

		// Token: 0x04001392 RID: 5010
		[SerializeField]
		private TextMeshProUGUI _iconTextMesh;

		// Token: 0x04001393 RID: 5011
		[SerializeField]
		private Image _iconImage;

		// Token: 0x04001394 RID: 5012
		[SerializeField]
		private ActionIconViewDataTween _viewDataTween = new ActionIconViewDataTween();

		// Token: 0x02000424 RID: 1060
		public enum ScalingMode
		{
			// Token: 0x04001B74 RID: 7028
			Auto,
			// Token: 0x04001B75 RID: 7029
			Manual
		}

		// Token: 0x02000425 RID: 1061
		public enum Mode
		{
			// Token: 0x04001B77 RID: 7031
			Action,
			// Token: 0x04001B78 RID: 7032
			InputControlType,
			// Token: 0x04001B79 RID: 7033
			Key,
			// Token: 0x04001B7A RID: 7034
			Mouse,
			// Token: 0x04001B7B RID: 7035
			Text,
			// Token: 0x04001B7C RID: 7036
			Icon,
			// Token: 0x04001B7D RID: 7037
			Sprite
		}

		// Token: 0x02000426 RID: 1062
		public enum IconType
		{
			// Token: 0x04001B7F RID: 7039
			NavTriangleWithDot,
			// Token: 0x04001B80 RID: 7040
			DotInCircle,
			// Token: 0x04001B81 RID: 7041
			BackChevron
		}
	}
}
