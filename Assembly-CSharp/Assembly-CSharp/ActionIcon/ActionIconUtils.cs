using System;
using System.Collections.Generic;
using System.Linq;
using InControl;
using UnityEngine;

namespace ActionIcon
{
	// Token: 0x0200023A RID: 570
	public static class ActionIconUtils
	{
		// Token: 0x06001454 RID: 5204 RVA: 0x0008CDDC File Offset: 0x0008AFDC
		public static ActionIconViewData GetDataFromInputControlType(InputControlType controlType, BackgroundType backgroundType = BackgroundType.Auto, IconVariant variant = IconVariant.Standard, ActionIconControllerType controllerType = ActionIconControllerType.Auto)
		{
			ActionIconViewData templateForVariantType = ActionIconUtils.GetTemplateForVariantType(variant);
			ActionIconUtils.ApplyActionIconViewDataForController(controllerType, controlType, backgroundType, variant, ref templateForVariantType);
			return templateForVariantType;
		}

		// Token: 0x06001455 RID: 5205 RVA: 0x0008CDFC File Offset: 0x0008AFFC
		public static ActionIconViewData GetDataFromKey(Key key, BackgroundType backgroundType = BackgroundType.Auto, IconVariant variant = IconVariant.Standard)
		{
			ActionIconViewData templateForVariantType = ActionIconUtils.GetTemplateForVariantType(variant);
			ActionIconUtils.ApplyActionIconViewDataForKeyboard(key, backgroundType, variant, ref templateForVariantType);
			return templateForVariantType;
		}

		// Token: 0x06001456 RID: 5206 RVA: 0x0008CE1C File Offset: 0x0008B01C
		public static ActionIconViewData GetDataFromMouse(Mouse mouse, BackgroundType backgroundType = BackgroundType.Auto, IconVariant variant = IconVariant.Standard)
		{
			ActionIconViewData templateForVariantType = ActionIconUtils.GetTemplateForVariantType(variant);
			ActionIconUtils.ApplyActionIconViewDataForMouse(mouse, backgroundType, variant, ref templateForVariantType);
			return templateForVariantType;
		}

		// Token: 0x06001457 RID: 5207 RVA: 0x0008CE3C File Offset: 0x0008B03C
		public static ActionIconViewData GetDataFromAction(PlayerAction action, BackgroundType backgroundType = BackgroundType.Auto, IconVariant variant = IconVariant.Standard, ActionIconControllerType controllerType = ActionIconControllerType.Auto, List<BindingSourceType> bindingSourceTypesOverride = null)
		{
			BindingSource bestBindingSource = ActionIconUtils.GetBestBindingSource(action, bindingSourceTypesOverride);
			if (bestBindingSource == null)
			{
				return ActionIconViewData.blank;
			}
			ActionIconViewData templateForVariantType = ActionIconUtils.GetTemplateForVariantType(variant);
			if (ActionIconUtils.debugFakeSwitchController)
			{
				InputControlType inputControlType = ((DeviceBindingSource)action.UnfilteredBindings.FirstOrDefault((BindingSource b) => b is DeviceBindingSource)).Control;
				if (inputControlType == InputControlType.Action1)
				{
					inputControlType = InputControlType.Action2;
				}
				else if (inputControlType == InputControlType.Action2)
				{
					inputControlType = InputControlType.Action1;
				}
				if (inputControlType == InputControlType.Action3)
				{
					inputControlType = InputControlType.Action4;
				}
				else if (inputControlType == InputControlType.Action4)
				{
					inputControlType = InputControlType.Action3;
				}
				ActionIconUtils.ApplyActionIconViewDataForController(ActionIconControllerType.Switch, inputControlType, backgroundType, variant, ref templateForVariantType);
				return templateForVariantType;
			}
			ActionIconUtils.ApplyDataFromBindingSource(bestBindingSource, backgroundType, variant, controllerType, ref templateForVariantType);
			return templateForVariantType;
		}

		// Token: 0x06001458 RID: 5208 RVA: 0x0008CEE4 File Offset: 0x0008B0E4
		public static ActionIconViewData GetDataFromIconType(ActionIconView.IconType iconType, IconVariant variant)
		{
			ActionIconViewData templateForVariantType = ActionIconUtils.GetTemplateForVariantType(variant);
			switch (iconType)
			{
			case ActionIconView.IconType.NavTriangleWithDot:
				templateForVariantType.background = ActionIconUtils.iconDatabase.navTriangle;
				templateForVariantType.icon = ActionIconUtils.iconDatabase.dotIcon;
				break;
			case ActionIconView.IconType.DotInCircle:
				templateForVariantType.background = ActionIconUtils.iconDatabase.circleBackground;
				templateForVariantType.icon = ActionIconUtils.iconDatabase.dotIcon;
				break;
			case ActionIconView.IconType.BackChevron:
				templateForVariantType.background = ActionIconBackground.blank;
				templateForVariantType.icon = ActionIconUtils.iconDatabase.backChevron;
				break;
			default:
				Debug.LogWarning("ActionIconView.IconType " + iconType.ToString() + " not handled!");
				break;
			}
			return templateForVariantType;
		}

		// Token: 0x06001459 RID: 5209 RVA: 0x0008CF98 File Offset: 0x0008B198
		public static ActionIconViewData GetDataFromText(string text, BackgroundType backgroundType = BackgroundType.Auto, IconVariant variant = IconVariant.Standard)
		{
			ActionIconViewData templateForVariantType = ActionIconUtils.GetTemplateForVariantType(variant);
			ActionIconUtils.SetDataFromText(text, backgroundType, variant, ref templateForVariantType);
			return templateForVariantType;
		}

		// Token: 0x0600145A RID: 5210 RVA: 0x0008CFB7 File Offset: 0x0008B1B7
		private static string ApplyCapitalization(string text, ActionIconDatabase.CapitalizationMode capitalizationMode)
		{
			if (capitalizationMode == ActionIconDatabase.CapitalizationMode.UpperCase)
			{
				return text.ToUpper();
			}
			if (capitalizationMode != ActionIconDatabase.CapitalizationMode.LowerCase)
			{
				return text;
			}
			return text.ToLower();
		}

		// Token: 0x0600145B RID: 5211 RVA: 0x0008CFD4 File Offset: 0x0008B1D4
		private static void SetDataFromText(string text, BackgroundType backgroundType, IconVariant variant, ref ActionIconViewData viewData)
		{
			viewData.iconText = text;
			if (backgroundType != BackgroundType.Auto && backgroundType != BackgroundType.AutoButton)
			{
				viewData.background = ActionIconUtils._iconDatabase.GetBackgroundFromBackgroundType(backgroundType);
				return;
			}
			if (backgroundType == BackgroundType.AutoButton && viewData.iconText.Length == 1)
			{
				viewData.background = ActionIconUtils.iconDatabase.keyboardSingleCharacter;
				return;
			}
			viewData.background = ActionIconUtils.iconDatabase.keyboardFullName;
		}

		// Token: 0x0600145C RID: 5212 RVA: 0x0008D034 File Offset: 0x0008B234
		public static ActionIconViewData GetDataFromSprite(Sprite sprite, BackgroundType backgroundType = BackgroundType.Auto, IconVariant variant = IconVariant.Standard)
		{
			ActionIconViewData templateForVariantType = ActionIconUtils.GetTemplateForVariantType(variant);
			templateForVariantType.icon = sprite;
			if (backgroundType == BackgroundType.Auto || backgroundType == BackgroundType.AutoButton)
			{
				templateForVariantType.background = ActionIconBackground.blank;
			}
			else
			{
				templateForVariantType.background = ActionIconUtils._iconDatabase.GetBackgroundFromBackgroundType(backgroundType);
			}
			return templateForVariantType;
		}

		// Token: 0x0600145D RID: 5213 RVA: 0x0008D078 File Offset: 0x0008B278
		public static ActionIconViewData GetTemplateForVariantType(IconVariant variant)
		{
			if (variant == IconVariant.Standard)
			{
				return ActionIconUtils.standardActionIconViewData;
			}
			if (variant == IconVariant.Highlight)
			{
				return ActionIconUtils.highlightActionIconViewData;
			}
			return ActionIconUtils.standardActionIconViewData;
		}

		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x0600145E RID: 5214 RVA: 0x0008D092 File Offset: 0x0008B292
		private static ActionIconViewData standardActionIconViewData
		{
			get
			{
				return new ActionIconViewData(ActionIconUtils.iconDatabase.circleBackground, null, "", ActionIconUtils.iconDatabase.defaultIconColor, ActionIconUtils.iconDatabase.defaultBackgroundColor, 1f);
			}
		}

		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x0600145F RID: 5215 RVA: 0x0008D0C2 File Offset: 0x0008B2C2
		private static ActionIconViewData highlightActionIconViewData
		{
			get
			{
				return new ActionIconViewData(ActionIconUtils.iconDatabase.circleBackground, null, "", ActionIconUtils.iconDatabase.highlightedIconColor, ActionIconUtils.iconDatabase.highlightedBackgroundColor, 1f);
			}
		}

		// Token: 0x06001460 RID: 5216 RVA: 0x0008D0F4 File Offset: 0x0008B2F4
		private static string KeyToText(Key key)
		{
			if (key <= Key.Escape)
			{
				switch (key)
				{
				case Key.None:
					Debug.LogWarning("Key is mapped to Key.None!");
					return "???";
				case Key.Shift:
				case Key.Alt:
					break;
				case Key.Command:
					return "CMD";
				case Key.Control:
					return "CTRL";
				default:
					if (key == Key.Escape)
					{
						return "ESC";
					}
					break;
				}
			}
			else
			{
				switch (key)
				{
				case Key.Backquote:
					return "`";
				case Key.Minus:
				case Key.Tab:
				case Key.Period:
				case Key.Insert:
				case Key.Home:
				case Key.End:
				case Key.PageUp:
				case Key.PageDown:
					break;
				case Key.Equals:
					return "=";
				case Key.Backspace:
					return "Backspace";
				case Key.LeftBracket:
					return "[";
				case Key.RightBracket:
					return "]";
				case Key.Backslash:
					return "\\";
				case Key.Semicolon:
					return ";";
				case Key.Quote:
					return "'";
				case Key.Return:
					return "Enter";
				case Key.Comma:
					return ",";
				case Key.Slash:
					return "/";
				case Key.Space:
					return "Space";
				case Key.Delete:
					return "DEL";
				case Key.LeftArrow:
					return "Left";
				case Key.RightArrow:
					return "Right";
				case Key.UpArrow:
					return "Up";
				case Key.DownArrow:
					return "Down";
				default:
					if (key == Key.CapsLock)
					{
						return "CAPS";
					}
					break;
				}
			}
			return key.ToString().ToUpper();
		}

		// Token: 0x06001461 RID: 5217 RVA: 0x0008D244 File Offset: 0x0008B444
		private static BindingSource GetBestBindingSource(PlayerAction action, List<BindingSourceType> bindingSourceTypesOverride = null)
		{
			if (action == null)
			{
				return null;
			}
			if (action.Bindings.Count == 0)
			{
				Debug.LogWarning("Action '" + action.Name + "' has no bindings!");
				return null;
			}
			List<BindingSourceType> list;
			if (bindingSourceTypesOverride != null && bindingSourceTypesOverride.Count > 0)
			{
				list = bindingSourceTypesOverride;
			}
			else
			{
				list = ActionIconUtils.targetBindingSourceTypes;
			}
			foreach (BindingSourceType bindingSourceType in list)
			{
				foreach (BindingSource bindingSource3 in action.Bindings)
				{
					if (bindingSourceType == bindingSource3.BindingSourceType)
					{
						return bindingSource3;
					}
				}
			}
			foreach (BindingSourceType bindingSourceType2 in list)
			{
				foreach (BindingSource bindingSource2 in action.UnfilteredBindings)
				{
					if (bindingSourceType2 == bindingSource2.BindingSourceType)
					{
						return bindingSource2;
					}
				}
			}
			string[] array = new string[8];
			array[0] = "No binding source found for action '";
			array[1] = action.Name;
			array[2] = "'\nTarget binding sources ";
			array[3] = DebugX.ListAsString<BindingSourceType>(list, null, true, true);
			array[4] = "\nAction bindings ";
			int num = 5;
			string text;
			if (action.Bindings != null)
			{
				text = DebugX.ListAsString<string>(action.Bindings.Select((BindingSource bindingSource) => bindingSource.Name + " (" + bindingSource.BindingSourceType.ToString() + ")"), null, true, true);
			}
			else
			{
				text = "NULL";
			}
			array[num] = text;
			array[6] = "\nUnfiltered Action bindings ";
			int num2 = 7;
			string text2;
			if (action.UnfilteredBindings != null)
			{
				text2 = DebugX.ListAsString<string>(action.UnfilteredBindings.Select((BindingSource bindingSource) => bindingSource.Name + " (" + bindingSource.BindingSourceType.ToString() + ")"), null, true, true);
			}
			else
			{
				text2 = "NULL";
			}
			array[num2] = text2;
			Debug.LogWarning(string.Concat(array));
			return null;
		}

		// Token: 0x06001462 RID: 5218 RVA: 0x0008D46C File Offset: 0x0008B66C
		private static void ApplyDataFromBindingSource(BindingSource bindingSource, BackgroundType backgroundType, IconVariant variant, ActionIconControllerType controllerType, ref ActionIconViewData viewData)
		{
			if (bindingSource == null)
			{
				Debug.LogError("BINDING SOURCE IS NULL!");
				return;
			}
			if (bindingSource.BindingSourceType == BindingSourceType.KeyBindingSource)
			{
				ActionIconUtils.ApplyActionIconViewDataForKeyboard(((KeyBindingSource)bindingSource).Control.GetInclude(0), backgroundType, variant, ref viewData);
				return;
			}
			if (bindingSource.BindingSourceType == BindingSourceType.DeviceBindingSource)
			{
				InputControlType control = ((DeviceBindingSource)bindingSource).Control;
				ActionIconUtils.ApplyActionIconViewDataForController(controllerType, control, backgroundType, variant, ref viewData);
				return;
			}
			if (bindingSource.BindingSourceType == BindingSourceType.MouseBindingSource)
			{
				ActionIconUtils.ApplyActionIconViewDataForMouse(((MouseBindingSource)bindingSource).Control, backgroundType, variant, ref viewData);
				return;
			}
			Debug.LogWarning("No type found from " + ((bindingSource != null) ? bindingSource.ToString() : null));
		}

		// Token: 0x06001463 RID: 5219 RVA: 0x0008D510 File Offset: 0x0008B710
		private static void ApplyActionIconViewDataForKeyboard(Key key, BackgroundType backgroundType, IconVariant variant, ref ActionIconViewData viewData)
		{
			if (ActionIconUtils.iconDatabase.keyboardIcons != null)
			{
				ActionIconUtils.ApplyPlatformActionIcon(ActionIconUtils.iconDatabase.keyboardIcons.GetPlatformActionIcon(key), variant, ref viewData);
			}
			if (viewData.icon == null)
			{
				viewData.iconText = ActionIconUtils.ApplyCapitalization(ActionIconUtils.KeyToText(key), ActionIconUtils.iconDatabase.keyNameCapitalization);
			}
			if (backgroundType != BackgroundType.Auto && backgroundType != BackgroundType.AutoButton)
			{
				viewData.background = ActionIconUtils._iconDatabase.GetBackgroundFromBackgroundType(backgroundType);
				return;
			}
			if (viewData.iconText == null || viewData.iconText.Length <= 1 || viewData.icon != null)
			{
				viewData.background = ActionIconUtils.iconDatabase.keyboardSingleCharacter;
				return;
			}
			viewData.background = ActionIconUtils.iconDatabase.keyboardFullName;
		}

		// Token: 0x06001464 RID: 5220 RVA: 0x0008D5D0 File Offset: 0x0008B7D0
		private static void ApplyActionIconViewDataForController(ActionIconControllerType consoleIconType, InputControlType controlType, BackgroundType backgroundType, IconVariant variant, ref ActionIconViewData viewData)
		{
			viewData.icon = null;
			viewData.iconText = null;
			bool flag = ActionIconUtils.InputControlTypeIsLeftStick(controlType);
			bool flag2 = ActionIconUtils.InputControlTypeIsRightStick(controlType);
			if (backgroundType == BackgroundType.Auto || backgroundType == BackgroundType.AutoButton)
			{
				if (controlType - InputControlType.LeftTrigger > 1)
				{
					if (controlType - InputControlType.LeftBumper > 1)
					{
						if (controlType != InputControlType.Select)
						{
							if (flag || flag2)
							{
								viewData.background = ActionIconUtils.iconDatabase.stickBackground;
							}
							else
							{
								viewData.background = ActionIconUtils.iconDatabase.circleBackground;
							}
						}
						else
						{
							viewData.background = ActionIconUtils.iconDatabase.wideRoundBackground;
						}
					}
					else
					{
						viewData.background = ActionIconUtils.iconDatabase.bumperBackground;
					}
				}
				else
				{
					viewData.background = ActionIconUtils.iconDatabase.triggerBackground;
				}
			}
			else
			{
				viewData.background = ActionIconUtils.iconDatabase.GetBackgroundFromBackgroundType(backgroundType);
			}
			PlatformActionIconSet iconSetFromControllerType = ActionIconUtils.iconDatabase.GetIconSetFromControllerType(consoleIconType);
			string text = controlType.ToString();
			if (text.StartsWith("Action"))
			{
				viewData.iconText = text.Substring(6);
			}
			else
			{
				if (controlType <= InputControlType.Plus)
				{
					switch (controlType)
					{
					case InputControlType.LeftStickButton:
						viewData.iconText = ActionIconUtils.GetButtonDisplayStringForControllerType(controlType, consoleIconType);
						goto IL_0293;
					case InputControlType.RightStickUp:
					case InputControlType.RightStickDown:
					case InputControlType.RightStickLeft:
					case InputControlType.RightStickRight:
						break;
					case InputControlType.RightStickButton:
						viewData.iconText = ActionIconUtils.GetButtonDisplayStringForControllerType(controlType, consoleIconType);
						goto IL_0293;
					case InputControlType.DPadUp:
						viewData.background.backgroundSprite = iconSetFromControllerType.dpadUp;
						goto IL_0293;
					case InputControlType.DPadDown:
						viewData.background.backgroundSprite = iconSetFromControllerType.dpadDown;
						goto IL_0293;
					case InputControlType.DPadLeft:
						viewData.background.backgroundSprite = iconSetFromControllerType.dpadLeft;
						goto IL_0293;
					case InputControlType.DPadRight:
						viewData.background.backgroundSprite = iconSetFromControllerType.dpadRight;
						goto IL_0293;
					case InputControlType.LeftTrigger:
						viewData.iconText = ActionIconUtils.GetButtonDisplayStringForControllerType(controlType, consoleIconType);
						goto IL_0293;
					case InputControlType.RightTrigger:
						viewData.iconText = ActionIconUtils.GetButtonDisplayStringForControllerType(controlType, consoleIconType);
						goto IL_0293;
					case InputControlType.LeftBumper:
						viewData.iconText = ActionIconUtils.GetButtonDisplayStringForControllerType(controlType, consoleIconType);
						goto IL_0293;
					case InputControlType.RightBumper:
						viewData.iconText = ActionIconUtils.GetButtonDisplayStringForControllerType(controlType, consoleIconType);
						goto IL_0293;
					default:
						if (controlType == InputControlType.Plus)
						{
							viewData.background.backgroundSprite = iconSetFromControllerType.switchPlus;
							goto IL_0293;
						}
						break;
					}
				}
				else
				{
					if (controlType == InputControlType.Minus)
					{
						viewData.background.backgroundSprite = iconSetFromControllerType.switchMinus;
						goto IL_0293;
					}
					if (controlType == InputControlType.TouchPadButton)
					{
						viewData.background.backgroundSprite = iconSetFromControllerType.touchPad;
						goto IL_0293;
					}
				}
				if (flag)
				{
					viewData.iconText = "L";
				}
				else if (flag2)
				{
					viewData.iconText = "R";
				}
				else
				{
					viewData.iconText = controlType.ToString().ToUpper();
				}
			}
			IL_0293:
			ActionIconUtils.ApplyPlatformActionIcon(iconSetFromControllerType.GetPlatformActionIcon(controlType), variant, ref viewData);
		}

		// Token: 0x06001465 RID: 5221 RVA: 0x0008D880 File Offset: 0x0008BA80
		private static string GetButtonDisplayStringForControllerType(InputControlType controlType, ActionIconControllerType controllerType)
		{
			if (controlType != InputControlType.LeftStickButton)
			{
				switch (controlType)
				{
				case InputControlType.RightStickButton:
					switch (controllerType)
					{
					case ActionIconControllerType.XboxOne:
						return "R";
					case ActionIconControllerType.PlayStation:
					case ActionIconControllerType.Deck:
					case ActionIconControllerType.MFI:
						return "R3";
					case ActionIconControllerType.Switch:
						return "R";
					default:
						return "R3";
					}
					break;
				case InputControlType.LeftTrigger:
					switch (controllerType)
					{
					case ActionIconControllerType.XboxOne:
						return "LT";
					case ActionIconControllerType.PlayStation:
					case ActionIconControllerType.Deck:
					case ActionIconControllerType.MFI:
						return "L2";
					case ActionIconControllerType.Switch:
						return "ZL";
					default:
						return "LB";
					}
					break;
				case InputControlType.RightTrigger:
					switch (controllerType)
					{
					case ActionIconControllerType.XboxOne:
						return "RT";
					case ActionIconControllerType.PlayStation:
					case ActionIconControllerType.Deck:
					case ActionIconControllerType.MFI:
						return "R2";
					case ActionIconControllerType.Switch:
						return "ZR";
					default:
						return "RB";
					}
					break;
				case InputControlType.LeftBumper:
					switch (controllerType)
					{
					case ActionIconControllerType.XboxOne:
						return "LB";
					case ActionIconControllerType.PlayStation:
					case ActionIconControllerType.Deck:
					case ActionIconControllerType.MFI:
						return "L1";
					case ActionIconControllerType.Switch:
						return "L";
					default:
						return "LB";
					}
					break;
				case InputControlType.RightBumper:
					switch (controllerType)
					{
					case ActionIconControllerType.XboxOne:
						return "RB";
					case ActionIconControllerType.PlayStation:
					case ActionIconControllerType.Deck:
					case ActionIconControllerType.MFI:
						return "R1";
					case ActionIconControllerType.Switch:
						return "R";
					default:
						return "RB";
					}
					break;
				}
				Debug.LogWarning("No button name for " + controlType.ToString());
				return "";
			}
			switch (controllerType)
			{
			case ActionIconControllerType.XboxOne:
				return "L";
			case ActionIconControllerType.PlayStation:
			case ActionIconControllerType.Deck:
			case ActionIconControllerType.MFI:
				return "L3";
			case ActionIconControllerType.Switch:
				return "L";
			default:
				return "L3";
			}
		}

		// Token: 0x06001466 RID: 5222 RVA: 0x0008DA2C File Offset: 0x0008BC2C
		private static void ApplyActionIconViewDataForMouse(Mouse mouse, BackgroundType backgroundType, IconVariant variant, ref ActionIconViewData data)
		{
			if (ActionIconUtils.iconDatabase.mouseIcons == null)
			{
				return;
			}
			if (mouse == Mouse.LeftButton)
			{
				data.icon = ActionIconUtils.iconDatabase.mouseIcons.leftMouse;
				return;
			}
			if (mouse != Mouse.RightButton)
			{
				return;
			}
			data.icon = ActionIconUtils.iconDatabase.mouseIcons.rightMouse;
		}

		// Token: 0x06001467 RID: 5223 RVA: 0x0008DA80 File Offset: 0x0008BC80
		private static void ApplyPlatformActionIcon(PlatformActionIcon actionIconOverrides, IconVariant variant, ref ActionIconViewData viewData)
		{
			if (actionIconOverrides == null)
			{
				return;
			}
			if (variant == IconVariant.Standard)
			{
				viewData.iconColor = actionIconOverrides.iconColor;
			}
			else if (variant == IconVariant.Highlight)
			{
				viewData.highlightColor = actionIconOverrides.highlightColor;
			}
			if (actionIconOverrides.icon != null)
			{
				viewData.iconText = null;
				viewData.icon = actionIconOverrides.icon;
				return;
			}
			viewData.iconText = actionIconOverrides.label;
			viewData.icon = null;
		}

		// Token: 0x06001468 RID: 5224 RVA: 0x0008DAE7 File Offset: 0x0008BCE7
		private static bool InputControlTypeIsLeftStick(InputControlType inputControlType)
		{
			return inputControlType - InputControlType.LeftStickUp <= 3 || inputControlType - InputControlType.LeftStickX <= 1;
		}

		// Token: 0x06001469 RID: 5225 RVA: 0x0008DAFC File Offset: 0x0008BCFC
		private static bool InputControlTypeIsRightStick(InputControlType inputControlType)
		{
			return inputControlType - InputControlType.RightStickUp <= 3 || inputControlType - InputControlType.RightStickX <= 1;
		}

		// Token: 0x0600146A RID: 5226 RVA: 0x0008DB11 File Offset: 0x0008BD11
		public static void SetDirty()
		{
			ActionIconUtils._targetBindingSourceTypes = null;
			ActionIconView.RefreshAllImmediate();
		}

		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x0600146B RID: 5227 RVA: 0x0008DB20 File Offset: 0x0008BD20
		private static List<BindingSourceType> targetBindingSourceTypes
		{
			get
			{
				if (ActionIconUtils._targetBindingSourceTypes == null)
				{
					ActionIconUtils._targetBindingSourceTypes = new List<BindingSourceType>();
					if (GameInput.activeInputType == GameInput.InputType.Gamepad)
					{
						ActionIconUtils._targetBindingSourceTypes.Add(BindingSourceType.DeviceBindingSource);
					}
					else if (GameInput.activeInputType == GameInput.InputType.KeyboardAndMouse)
					{
						ActionIconUtils._targetBindingSourceTypes.Add(BindingSourceType.MouseBindingSource);
						ActionIconUtils._targetBindingSourceTypes.Add(BindingSourceType.KeyBindingSource);
					}
					else
					{
						Debug.LogWarning("Input type " + GameInput.activeInputType.ToString() + " not supported!");
					}
				}
				return ActionIconUtils._targetBindingSourceTypes;
			}
		}

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x0600146C RID: 5228 RVA: 0x0008DB9E File Offset: 0x0008BD9E
		public static ActionIconDatabase iconDatabase
		{
			get
			{
				if (!ActionIconUtils._searchedForIconDatabase)
				{
					ActionIconUtils._iconDatabase = Resources.Load<ActionIconDatabase>("IconDatabase");
					ActionIconUtils._searchedForIconDatabase = true;
				}
				return ActionIconUtils._iconDatabase;
			}
		}

		// Token: 0x0600146D RID: 5229 RVA: 0x0008DBC4 File Offset: 0x0008BDC4
		static ActionIconUtils()
		{
			GameInput.onInputTypeChanged = (Action<GameInput.InputType>)Delegate.Combine(GameInput.onInputTypeChanged, new Action<GameInput.InputType>(ActionIconUtils.OnChangeInputType));
			GameInput.onChangeControllerType = (Action<GameInput.ControllerType>)Delegate.Combine(GameInput.onChangeControllerType, new Action<GameInput.ControllerType>(ActionIconUtils.OnChangeControllerType));
		}

		// Token: 0x0600146E RID: 5230 RVA: 0x0008DC11 File Offset: 0x0008BE11
		private static void OnChangeInputType(GameInput.InputType inputType)
		{
			ActionIconUtils.SetDirty();
		}

		// Token: 0x0600146F RID: 5231 RVA: 0x0008DC18 File Offset: 0x0008BE18
		private static void OnChangeControllerType(GameInput.ControllerType controllerType)
		{
			ActionIconUtils.SetDirty();
		}

		// Token: 0x04001378 RID: 4984
		public static bool debugFakeSwitchController;

		// Token: 0x04001379 RID: 4985
		private static List<BindingSourceType> _targetBindingSourceTypes;

		// Token: 0x0400137A RID: 4986
		private static bool _searchedForIconDatabase;

		// Token: 0x0400137B RID: 4987
		private static ActionIconDatabase _iconDatabase;
	}
}
