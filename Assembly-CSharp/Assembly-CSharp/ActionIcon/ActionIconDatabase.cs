using System;
using TMPro;
using UnityEngine;

namespace ActionIcon
{
	// Token: 0x02000239 RID: 569
	public class ActionIconDatabase : ScriptableObject
	{
		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x06001450 RID: 5200 RVA: 0x0008CC42 File Offset: 0x0008AE42
		public PlatformActionIconSet controllerAutoIconSet
		{
			get
			{
				return this.XBOneIcons;
			}
		}

		// Token: 0x06001451 RID: 5201 RVA: 0x0008CC4C File Offset: 0x0008AE4C
		public PlatformActionIconSet GetIconSetFromControllerType(ActionIconControllerType consoleIconType)
		{
			switch (consoleIconType)
			{
			case ActionIconControllerType.Auto:
				break;
			case ActionIconControllerType.XboxOne:
				return this.XBOneIcons;
			case ActionIconControllerType.PlayStation:
				return this.ps4Icons;
			case ActionIconControllerType.Switch:
				return this.switchIcons;
			case ActionIconControllerType.Deck:
				return this.deckIcons;
			case ActionIconControllerType.MFI:
				return this.mfiIcons;
			default:
				Debug.LogError("NO ICONS FOR PLATFORM " + consoleIconType.ToString());
				break;
			}
			return this.controllerAutoIconSet;
		}

		// Token: 0x06001452 RID: 5202 RVA: 0x0008CCC0 File Offset: 0x0008AEC0
		public ActionIconBackground GetBackgroundFromBackgroundType(BackgroundType backgroundType)
		{
			switch (backgroundType)
			{
			case BackgroundType.None:
				return ActionIconBackground.blank;
			case BackgroundType.Circle:
				return this.circleBackground;
			case BackgroundType.NavTriangle:
				return this.navTriangle;
			case BackgroundType.KeyboardFullName:
				return this.keyboardFullName;
			case BackgroundType.KeyboardSingleCharacter:
				return this.keyboardSingleCharacter;
			case BackgroundType.WideRound:
				return this.wideRoundBackground;
			case BackgroundType.Trigger:
				return this.triggerBackground;
			default:
				Debug.LogError("No background for BackgroundType " + backgroundType.ToString());
				return ActionIconBackground.blank;
			}
		}

		// Token: 0x04001361 RID: 4961
		public TMP_FontAsset font;

		// Token: 0x04001362 RID: 4962
		[Space]
		public ActionIconBackground circleBackground = ActionIconBackground.blank;

		// Token: 0x04001363 RID: 4963
		public ActionIconBackground navTriangle = ActionIconBackground.blank;

		// Token: 0x04001364 RID: 4964
		public ActionIconBackground keyboardFullName = ActionIconBackground.blank;

		// Token: 0x04001365 RID: 4965
		public ActionIconBackground keyboardSingleCharacter = ActionIconBackground.blank;

		// Token: 0x04001366 RID: 4966
		public ActionIconBackground wideRoundBackground = ActionIconBackground.blank;

		// Token: 0x04001367 RID: 4967
		public ActionIconBackground triggerBackground = ActionIconBackground.blank;

		// Token: 0x04001368 RID: 4968
		public ActionIconBackground bumperBackground = ActionIconBackground.blank;

		// Token: 0x04001369 RID: 4969
		public ActionIconBackground stickBackground = ActionIconBackground.blank;

		// Token: 0x0400136A RID: 4970
		[Space]
		public Color defaultIconColor = Color.green;

		// Token: 0x0400136B RID: 4971
		public Color defaultBackgroundColor = Color.clear;

		// Token: 0x0400136C RID: 4972
		[Space]
		public Color highlightedIconColor = Color.black;

		// Token: 0x0400136D RID: 4973
		public Color highlightedBackgroundColor = Color.green;

		// Token: 0x0400136E RID: 4974
		[Space]
		public Sprite dotIcon;

		// Token: 0x0400136F RID: 4975
		public Sprite backChevron;

		// Token: 0x04001370 RID: 4976
		[Space]
		public MouseActionIconSet mouseIcons;

		// Token: 0x04001371 RID: 4977
		public KeyboardActionIconSet keyboardIcons;

		// Token: 0x04001372 RID: 4978
		public PlatformActionIconSet ps4Icons;

		// Token: 0x04001373 RID: 4979
		public PlatformActionIconSet XBOneIcons;

		// Token: 0x04001374 RID: 4980
		public PlatformActionIconSet switchIcons;

		// Token: 0x04001375 RID: 4981
		public PlatformActionIconSet deckIcons;

		// Token: 0x04001376 RID: 4982
		public PlatformActionIconSet mfiIcons;

		// Token: 0x04001377 RID: 4983
		public ActionIconDatabase.CapitalizationMode keyNameCapitalization;

		// Token: 0x02000422 RID: 1058
		public enum CapitalizationMode
		{
			// Token: 0x04001B6C RID: 7020
			Capitalized,
			// Token: 0x04001B6D RID: 7021
			UpperCase,
			// Token: 0x04001B6E RID: 7022
			LowerCase
		}
	}
}
