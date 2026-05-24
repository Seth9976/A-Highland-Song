using System;
using System.Collections.Generic;
using ActionIcon;
using InControl;
using Steamworks;
using UnityEngine;

// Token: 0x02000030 RID: 48
public class GameInput : MonoSingleton<GameInput>
{
	// Token: 0x17000055 RID: 85
	// (get) Token: 0x06000145 RID: 325 RVA: 0x0000D501 File Offset: 0x0000B701
	// (set) Token: 0x06000146 RID: 326 RVA: 0x0000D508 File Offset: 0x0000B708
	public static bool altControls { get; private set; }

	// Token: 0x17000056 RID: 86
	// (get) Token: 0x06000147 RID: 327 RVA: 0x0000D510 File Offset: 0x0000B710
	public static float moveLeftRight
	{
		get
		{
			Vector2 value = MonoSingleton<GameInput>.instance.mapping.move.Value;
			float num = value.magnitude;
			if (num < 0.02f)
			{
				num = 0f;
			}
			if (num > 0.95f)
			{
				num = 1f;
			}
			if (num > 0f)
			{
				Vector2 vector = value;
				if (vector.x < 0f)
				{
					vector.x *= -1f;
				}
				if (Vector2.Angle(vector, Vector2.right) > 45f)
				{
					num = 0f;
				}
			}
			return Mathf.Sign(value.x) * num;
		}
	}

	// Token: 0x17000057 RID: 87
	// (get) Token: 0x06000148 RID: 328 RVA: 0x0000D5A2 File Offset: 0x0000B7A2
	public static float moveUpDown
	{
		get
		{
			return MonoSingleton<GameInput>.instance.mapping.moveUpDown.Value;
		}
	}

	// Token: 0x17000058 RID: 88
	// (get) Token: 0x06000149 RID: 329 RVA: 0x0000D5B8 File Offset: 0x0000B7B8
	public static Vector2 move2d
	{
		get
		{
			return MonoSingleton<GameInput>.instance.mapping.move.Value;
		}
	}

	// Token: 0x17000059 RID: 89
	// (get) Token: 0x0600014A RID: 330 RVA: 0x0000D5CE File Offset: 0x0000B7CE
	public static float upDownPressed
	{
		get
		{
			if (MonoSingleton<GameInput>.instance.mapping.moveUp.WasPressed)
			{
				return 1f;
			}
			if (MonoSingleton<GameInput>.instance.mapping.moveDown.WasPressed)
			{
				return -1f;
			}
			return 0f;
		}
	}

	// Token: 0x1700005A RID: 90
	// (get) Token: 0x0600014B RID: 331 RVA: 0x0000D60D File Offset: 0x0000B80D
	public static bool pressedLeft
	{
		get
		{
			return MonoSingleton<GameInput>.instance.mapping.moveLeft.WasPressed;
		}
	}

	// Token: 0x1700005B RID: 91
	// (get) Token: 0x0600014C RID: 332 RVA: 0x0000D623 File Offset: 0x0000B823
	public static bool pressedRight
	{
		get
		{
			return MonoSingleton<GameInput>.instance.mapping.moveRight.WasPressed;
		}
	}

	// Token: 0x1700005C RID: 92
	// (get) Token: 0x0600014D RID: 333 RVA: 0x0000D639 File Offset: 0x0000B839
	public static bool jumped
	{
		get
		{
			return MonoSingleton<GameInput>.instance.mapping.jump.WasPressed;
		}
	}

	// Token: 0x1700005D RID: 93
	// (get) Token: 0x0600014E RID: 334 RVA: 0x0000D64F File Offset: 0x0000B84F
	public static bool jumpedSpecial
	{
		get
		{
			return MonoSingleton<GameInput>.instance.mapping.jumpSpecial.WasPressed;
		}
	}

	// Token: 0x1700005E RID: 94
	// (get) Token: 0x0600014F RID: 335 RVA: 0x0000D665 File Offset: 0x0000B865
	public static bool jumpHeld
	{
		get
		{
			return MonoSingleton<GameInput>.instance.mapping.jump.IsPressed;
		}
	}

	// Token: 0x1700005F RID: 95
	// (get) Token: 0x06000150 RID: 336 RVA: 0x0000D67B File Offset: 0x0000B87B
	public static bool sprintPressed
	{
		get
		{
			return MonoSingleton<GameInput>.instance.mapping.sprint.WasPressed && !GameInput.debugFast;
		}
	}

	// Token: 0x17000060 RID: 96
	// (get) Token: 0x06000151 RID: 337 RVA: 0x0000D69D File Offset: 0x0000B89D
	public static bool sprintHeld
	{
		get
		{
			return MonoSingleton<GameInput>.instance.mapping.sprint.IsPressed && !GameInput.debugFast;
		}
	}

	// Token: 0x17000061 RID: 97
	// (get) Token: 0x06000152 RID: 338 RVA: 0x0000D6BF File Offset: 0x0000B8BF
	public static bool restPressed
	{
		get
		{
			return MonoSingleton<GameInput>.instance.mapping.rest.WasPressed && (!Application.isEditor || (!Input.GetKey(KeyCode.LeftMeta) && !Input.GetKey(KeyCode.LeftControl)));
		}
	}

	// Token: 0x17000062 RID: 98
	// (get) Token: 0x06000153 RID: 339 RVA: 0x0000D6FD File Offset: 0x0000B8FD
	public static bool slipRecover
	{
		get
		{
			return MonoSingleton<GameInput>.instance.mapping.slipRecover.WasPressed;
		}
	}

	// Token: 0x17000063 RID: 99
	// (get) Token: 0x06000154 RID: 340 RVA: 0x0000D713 File Offset: 0x0000B913
	public static bool telescope
	{
		get
		{
			return MonoSingleton<GameInput>.instance.mapping.telescope.WasPressed;
		}
	}

	// Token: 0x17000064 RID: 100
	// (get) Token: 0x06000155 RID: 341 RVA: 0x0000D729 File Offset: 0x0000B929
	public static bool zoomOutHeld
	{
		get
		{
			return MonoSingleton<GameInput>.instance.mapping.zoomOut.IsPressed;
		}
	}

	// Token: 0x17000065 RID: 101
	// (get) Token: 0x06000156 RID: 342 RVA: 0x0000D73F File Offset: 0x0000B93F
	public static bool zoomInHeld
	{
		get
		{
			return MonoSingleton<GameInput>.instance.mapping.zoomIn.IsPressed;
		}
	}

	// Token: 0x17000066 RID: 102
	// (get) Token: 0x06000157 RID: 343 RVA: 0x0000D755 File Offset: 0x0000B955
	public static bool zoomInPressed
	{
		get
		{
			return MonoSingleton<GameInput>.instance.mapping.zoomIn.WasPressed;
		}
	}

	// Token: 0x17000067 RID: 103
	// (get) Token: 0x06000158 RID: 344 RVA: 0x0000D76B File Offset: 0x0000B96B
	public static bool zoomFurtherPressed
	{
		get
		{
			return MonoSingleton<GameInput>.instance.mapping.lookFurther.WasPressed;
		}
	}

	// Token: 0x17000068 RID: 104
	// (get) Token: 0x06000159 RID: 345 RVA: 0x0000D781 File Offset: 0x0000B981
	public static Vector2 pan
	{
		get
		{
			return MonoSingleton<GameInput>.instance.mapping.pan.Value;
		}
	}

	// Token: 0x17000069 RID: 105
	// (get) Token: 0x0600015A RID: 346 RVA: 0x0000D797 File Offset: 0x0000B997
	public static bool resetCameraToPlayer
	{
		get
		{
			return MonoSingleton<GameInput>.instance.mapping.resetCameraToPlayer.WasPressed;
		}
	}

	// Token: 0x1700006A RID: 106
	// (get) Token: 0x0600015B RID: 347 RVA: 0x0000D7AD File Offset: 0x0000B9AD
	public static bool selectUp
	{
		get
		{
			return MonoSingleton<GameInput>.instance.mapping.selectUp.WasPressed || MonoSingleton<GameInput>.instance.mapping.selectUp.WasRepeated;
		}
	}

	// Token: 0x1700006B RID: 107
	// (get) Token: 0x0600015C RID: 348 RVA: 0x0000D7DB File Offset: 0x0000B9DB
	public static bool selectDown
	{
		get
		{
			return MonoSingleton<GameInput>.instance.mapping.selectDown.WasPressed || MonoSingleton<GameInput>.instance.mapping.selectDown.WasRepeated;
		}
	}

	// Token: 0x1700006C RID: 108
	// (get) Token: 0x0600015D RID: 349 RVA: 0x0000D809 File Offset: 0x0000BA09
	public static bool selectLeft
	{
		get
		{
			return MonoSingleton<GameInput>.instance.mapping.selectLeft.WasPressed || MonoSingleton<GameInput>.instance.mapping.selectLeft.WasRepeated;
		}
	}

	// Token: 0x1700006D RID: 109
	// (get) Token: 0x0600015E RID: 350 RVA: 0x0000D837 File Offset: 0x0000BA37
	public static bool selectRight
	{
		get
		{
			return MonoSingleton<GameInput>.instance.mapping.selectRight.WasPressed || MonoSingleton<GameInput>.instance.mapping.selectRight.WasRepeated;
		}
	}

	// Token: 0x1700006E RID: 110
	// (get) Token: 0x0600015F RID: 351 RVA: 0x0000D865 File Offset: 0x0000BA65
	public static bool showMaps
	{
		get
		{
			return MonoSingleton<GameInput>.instance.mapping.showMaps.WasPressed;
		}
	}

	// Token: 0x1700006F RID: 111
	// (get) Token: 0x06000160 RID: 352 RVA: 0x0000D87B File Offset: 0x0000BA7B
	public static bool showJournal
	{
		get
		{
			return MonoSingleton<GameInput>.instance.mapping.showJournal.WasPressed;
		}
	}

	// Token: 0x17000070 RID: 112
	// (get) Token: 0x06000161 RID: 353 RVA: 0x0000D891 File Offset: 0x0000BA91
	public static bool prevMap
	{
		get
		{
			return MonoSingleton<GameInput>.instance.mapping.prevMap.WasPressed || MonoSingleton<GameInput>.instance.mapping.prevMap.WasRepeated;
		}
	}

	// Token: 0x17000071 RID: 113
	// (get) Token: 0x06000162 RID: 354 RVA: 0x0000D8BF File Offset: 0x0000BABF
	public static bool nextMap
	{
		get
		{
			return MonoSingleton<GameInput>.instance.mapping.nextMap.WasPressed || MonoSingleton<GameInput>.instance.mapping.nextMap.WasRepeated;
		}
	}

	// Token: 0x17000072 RID: 114
	// (get) Token: 0x06000163 RID: 355 RVA: 0x0000D8ED File Offset: 0x0000BAED
	public static bool prevSection
	{
		get
		{
			return MonoSingleton<GameInput>.instance.mapping.prevSection.WasPressed || MonoSingleton<GameInput>.instance.mapping.prevSection.WasRepeated;
		}
	}

	// Token: 0x17000073 RID: 115
	// (get) Token: 0x06000164 RID: 356 RVA: 0x0000D91B File Offset: 0x0000BB1B
	public static bool nextSection
	{
		get
		{
			return MonoSingleton<GameInput>.instance.mapping.nextSection.WasPressed || MonoSingleton<GameInput>.instance.mapping.nextSection.WasRepeated;
		}
	}

	// Token: 0x17000074 RID: 116
	// (get) Token: 0x06000165 RID: 357 RVA: 0x0000D949 File Offset: 0x0000BB49
	public static bool selectChoice
	{
		get
		{
			return MonoSingleton<GameInput>.instance.mapping.selectChoice.WasPressed;
		}
	}

	// Token: 0x17000075 RID: 117
	// (get) Token: 0x06000166 RID: 358 RVA: 0x0000D95F File Offset: 0x0000BB5F
	public static bool skipDialogue
	{
		get
		{
			return MonoSingleton<GameInput>.instance.mapping.skipDialogue.WasPressed;
		}
	}

	// Token: 0x17000076 RID: 118
	// (get) Token: 0x06000167 RID: 359 RVA: 0x0000D975 File Offset: 0x0000BB75
	public static bool skipDialogueHeld
	{
		get
		{
			return MonoSingleton<GameInput>.instance.mapping.skipDialogue.IsPressed;
		}
	}

	// Token: 0x17000077 RID: 119
	// (get) Token: 0x06000168 RID: 360 RVA: 0x0000D98B File Offset: 0x0000BB8B
	public static bool hideChoices
	{
		get
		{
			return MonoSingleton<GameInput>.instance.mapping.hideChoices.WasPressed;
		}
	}

	// Token: 0x17000078 RID: 120
	// (get) Token: 0x06000169 RID: 361 RVA: 0x0000D9A1 File Offset: 0x0000BBA1
	public static bool selectMenuItem
	{
		get
		{
			return MonoSingleton<GameInput>.instance.mapping.selectMenuItem.WasPressed;
		}
	}

	// Token: 0x17000079 RID: 121
	// (get) Token: 0x0600016A RID: 362 RVA: 0x0000D9B7 File Offset: 0x0000BBB7
	public static bool selectMenuitemHeld
	{
		get
		{
			return MonoSingleton<GameInput>.instance.mapping.selectMenuItem.IsPressed;
		}
	}

	// Token: 0x1700007A RID: 122
	// (get) Token: 0x0600016B RID: 363 RVA: 0x0000D9CD File Offset: 0x0000BBCD
	public static bool togglePhotoMode
	{
		get
		{
			return MonoSingleton<GameInput>.instance.mapping.togglePhotoMode.WasPressed;
		}
	}

	// Token: 0x1700007B RID: 123
	// (get) Token: 0x0600016C RID: 364 RVA: 0x0000D9E3 File Offset: 0x0000BBE3
	public static bool photoModeOptions
	{
		get
		{
			return MonoSingleton<GameInput>.instance.mapping.photoModeOptions.WasPressed;
		}
	}

	// Token: 0x1700007C RID: 124
	// (get) Token: 0x0600016D RID: 365 RVA: 0x0000D9F9 File Offset: 0x0000BBF9
	public static bool takePhoto
	{
		get
		{
			return MonoSingleton<GameInput>.instance.mapping.takePhoto.WasPressed;
		}
	}

	// Token: 0x1700007D RID: 125
	// (get) Token: 0x0600016E RID: 366 RVA: 0x0000DA0F File Offset: 0x0000BC0F
	public static bool photoModeToggleUI
	{
		get
		{
			return MonoSingleton<GameInput>.instance.mapping.photoModeToggleUI.WasPressed;
		}
	}

	// Token: 0x1700007E RID: 126
	// (get) Token: 0x0600016F RID: 367 RVA: 0x0000DA25 File Offset: 0x0000BC25
	public static bool revealPhoto
	{
		get
		{
			return MonoSingleton<GameInput>.instance.mapping.revealPhoto.WasPressed;
		}
	}

	// Token: 0x1700007F RID: 127
	// (get) Token: 0x06000170 RID: 368 RVA: 0x0000DA3B File Offset: 0x0000BC3B
	public static bool debugFlyToggle
	{
		get
		{
			return MonoSingleton<GameInput>.instance.mapping.debugFlyToggle.WasPressed;
		}
	}

	// Token: 0x17000080 RID: 128
	// (get) Token: 0x06000171 RID: 369 RVA: 0x0000DA51 File Offset: 0x0000BC51
	public static Vector2 debugFlyMove
	{
		get
		{
			return MonoSingleton<GameInput>.instance.mapping.debugFlyMove.Value;
		}
	}

	// Token: 0x17000081 RID: 129
	// (get) Token: 0x06000172 RID: 370 RVA: 0x0000DA67 File Offset: 0x0000BC67
	public static float debugFlyInOut
	{
		get
		{
			return MonoSingleton<GameInput>.instance.mapping.debugFlyInOut.Value;
		}
	}

	// Token: 0x17000082 RID: 130
	// (get) Token: 0x06000173 RID: 371 RVA: 0x0000DA7D File Offset: 0x0000BC7D
	public static bool debugFast
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000083 RID: 131
	// (get) Token: 0x06000174 RID: 372 RVA: 0x0000DA80 File Offset: 0x0000BC80
	public static bool debugReset
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000084 RID: 132
	// (get) Token: 0x06000175 RID: 373 RVA: 0x0000DA83 File Offset: 0x0000BC83
	public static bool debugToggleTestMenu
	{
		get
		{
			return MonoSingleton<GameInput>.instance.mapping.debugTestMenu.WasPressed || Input.GetKeyDown(KeyCode.Backslash);
		}
	}

	// Token: 0x17000085 RID: 133
	// (get) Token: 0x06000176 RID: 374 RVA: 0x0000DAA4 File Offset: 0x0000BCA4
	public static bool rawBackWasPressed
	{
		get
		{
			return MonoSingleton<GameInput>.instance.mapping.back.WasPressed;
		}
	}

	// Token: 0x06000177 RID: 375 RVA: 0x0000DABA File Offset: 0x0000BCBA
	public static bool SelectNumIdx(int i)
	{
		return MonoSingleton<GameInput>.instance.mapping.nums[i].WasPressed;
	}

	// Token: 0x06000178 RID: 376 RVA: 0x0000DAD2 File Offset: 0x0000BCD2
	public static bool Back(object controller)
	{
		return GameInput.HasControl(controller) && MonoSingleton<GameInput>.instance.mapping.back.WasPressed;
	}

	// Token: 0x06000179 RID: 377 RVA: 0x0000DAF2 File Offset: 0x0000BCF2
	public static void PushControlStack(object item)
	{
		MonoSingleton<GameInput>.instance._controlStack.Add(item);
	}

	// Token: 0x0600017A RID: 378 RVA: 0x0000DB04 File Offset: 0x0000BD04
	public static void PopControlStack(object item, bool warnIfNotAtTop = true)
	{
		if (MonoSingleton<GameInput>.instance._controlStack.Count == 0)
		{
			Debug.LogError(string.Format("GameInput's control stack EMPTY when tried to pop {0}", item));
			return;
		}
		int num = MonoSingleton<GameInput>.instance._controlStack.Count - 1;
		object obj = MonoSingleton<GameInput>.instance._controlStack[num];
		if (MonoSingleton<GameInput>.instance._controlStack[num] != item)
		{
			int num2 = MonoSingleton<GameInput>.instance._controlStack.IndexOf(item);
			if (num2 != -1)
			{
				if (warnIfNotAtTop)
				{
					Debug.LogWarning(string.Format("GameInput's control stack had mismatched item: wanted to pop {0} but saw {1}. Found at {2} places from the top and removing from there", item, obj, num - num2));
				}
				MonoSingleton<GameInput>.instance._controlStack.RemoveAt(num2);
				return;
			}
			if (warnIfNotAtTop)
			{
				Debug.LogWarning(string.Format("GameInput's control stack had mismatched item: wanted to pop {0} but saw {1}. Expected item wasn't in control stack at all, so skipping.", item, obj));
				return;
			}
		}
		else
		{
			MonoSingleton<GameInput>.instance._controlStack.RemoveAt(num);
		}
	}

	// Token: 0x0600017B RID: 379 RVA: 0x0000DBD0 File Offset: 0x0000BDD0
	public static void RemoveControlStackItemEvenIfNotOnTop(object itemToRemove)
	{
		if (!MonoSingleton<GameInput>.instance._controlStack.Contains(itemToRemove))
		{
			return;
		}
		GameInput.PopControlStack(itemToRemove, false);
	}

	// Token: 0x0600017C RID: 380 RVA: 0x0000DBEC File Offset: 0x0000BDEC
	public static bool HasControl(object controller)
	{
		if (MonoSingleton<GameInput>.instance._controlStack.Count != 0)
		{
			return MonoSingleton<GameInput>.instance._controlStack[MonoSingleton<GameInput>.instance._controlStack.Count - 1] == controller;
		}
		return controller == null;
	}

	// Token: 0x17000086 RID: 134
	// (get) Token: 0x0600017D RID: 381 RVA: 0x0000DC27 File Offset: 0x0000BE27
	public static List<object> controlStack
	{
		get
		{
			return MonoSingleton<GameInput>.instance._controlStack;
		}
	}

	// Token: 0x17000087 RID: 135
	// (get) Token: 0x0600017E RID: 382 RVA: 0x0000DC33 File Offset: 0x0000BE33
	public GameInputMapping mapping
	{
		get
		{
			if (this._mapping == null)
			{
				this._mapping = new GameInputMapping(false, false);
				GameInput.LoadCustomMapping();
			}
			return this._mapping;
		}
	}

	// Token: 0x0600017F RID: 383 RVA: 0x0000DC55 File Offset: 0x0000BE55
	public void SetUseAltControls(bool useAltControls)
	{
		if (useAltControls != GameInput.altControls)
		{
			GameInput.altControls = useAltControls;
			this.RefreshMapping();
		}
	}

	// Token: 0x06000180 RID: 384 RVA: 0x0000DC6B File Offset: 0x0000BE6B
	private void RefreshMapping()
	{
		this._mapping = new GameInputMapping(GameInput.altControls, false);
		GameInput.LoadCustomMapping();
		ActionIconView.ForceRefreshAll();
	}

	// Token: 0x06000181 RID: 385 RVA: 0x0000DC88 File Offset: 0x0000BE88
	protected void Awake()
	{
		GameInput.RefreshActiveDevice(false);
		InputManager.OnActiveDeviceChanged += GameInput.OnActiveDeviceChanged;
		InputManager.OnDeviceAttached += GameInput.OnDeviceAttached;
		InputManager.OnDeviceDetached += GameInput.OnDeviceDetached;
		GameInput.altControls = PlayerPrefsX.GetInt("altControls", 0) != 0;
		this.RefreshMapping();
	}

	// Token: 0x06000182 RID: 386 RVA: 0x0000DCE7 File Offset: 0x0000BEE7
	protected override void OnDestroy()
	{
		base.OnDestroy();
		InputManager.OnActiveDeviceChanged -= GameInput.OnActiveDeviceChanged;
		InputManager.OnDeviceAttached -= GameInput.OnDeviceAttached;
		InputManager.OnDeviceDetached -= GameInput.OnDeviceDetached;
	}

	// Token: 0x17000088 RID: 136
	// (get) Token: 0x06000183 RID: 387 RVA: 0x0000DD22 File Offset: 0x0000BF22
	public static GameInput.InputType activeInputType
	{
		get
		{
			return GameInput._activeInputType;
		}
	}

	// Token: 0x06000184 RID: 388 RVA: 0x0000DD29 File Offset: 0x0000BF29
	private static void SetActiveInputType(GameInput.InputType newActiveInputType, bool forceEvent = false)
	{
		if (newActiveInputType == GameInput.activeInputType && !forceEvent)
		{
			return;
		}
		GameInput._activeInputType = newActiveInputType;
		InputManager.ClearInputState();
		if (GameInput.onInputTypeChanged != null)
		{
			GameInput.onInputTypeChanged(GameInput.activeInputType);
		}
	}

	// Token: 0x17000089 RID: 137
	// (get) Token: 0x06000185 RID: 389 RVA: 0x0000DD58 File Offset: 0x0000BF58
	public static GameInput.ControllerType activeControllerType
	{
		get
		{
			return GameInput._activeControllerType;
		}
	}

	// Token: 0x06000186 RID: 390 RVA: 0x0000DD5F File Offset: 0x0000BF5F
	private static void SetActiveControllerType(GameInput.ControllerType newActiveControllerType, bool forceEvent = false)
	{
		if (GameInput._activeControllerType == newActiveControllerType && !forceEvent)
		{
			return;
		}
		GameInput._activeControllerType = newActiveControllerType;
		if (GameInput.onChangeControllerType != null)
		{
			GameInput.onChangeControllerType(GameInput._activeControllerType);
		}
	}

	// Token: 0x06000187 RID: 391 RVA: 0x0000DD89 File Offset: 0x0000BF89
	public static ActionIconControllerType GetActionIconControllerType()
	{
		if (GameInput._activeControllerType == GameInput.ControllerType.PlayStation)
		{
			return ActionIconControllerType.PlayStation;
		}
		if (GameInput._activeControllerType == GameInput.ControllerType.Xbox)
		{
			return ActionIconControllerType.XboxOne;
		}
		if (GameInput._activeControllerType == GameInput.ControllerType.Switch)
		{
			return ActionIconControllerType.Switch;
		}
		if (GameInput._activeControllerType == GameInput.ControllerType.Deck)
		{
			return ActionIconControllerType.Deck;
		}
		if (GameInput._activeControllerType == GameInput.ControllerType.MFI)
		{
			return ActionIconControllerType.MFI;
		}
		return ActionIconControllerType.Auto;
	}

	// Token: 0x06000188 RID: 392 RVA: 0x0000DDBD File Offset: 0x0000BFBD
	private static void OnActiveDeviceChanged(InputDevice device)
	{
		GameInput.RefreshActiveDevice(false);
	}

	// Token: 0x06000189 RID: 393 RVA: 0x0000DDC5 File Offset: 0x0000BFC5
	private static void OnDeviceAttached(InputDevice device)
	{
		GameInput.RefreshActiveDevice(false);
	}

	// Token: 0x0600018A RID: 394 RVA: 0x0000DDCD File Offset: 0x0000BFCD
	private static void OnDeviceDetached(InputDevice device)
	{
		GameInput.RefreshActiveDevice(false);
	}

	// Token: 0x0600018B RID: 395 RVA: 0x0000DDD8 File Offset: 0x0000BFD8
	public static void RefreshActiveDevice(bool forceEvent = false)
	{
		InputDevice inputDevice = null;
		if (InputManager.ActiveDevice != null)
		{
			if (InputManager.ActiveDevice.DeviceClass == InputDeviceClass.Controller || InputManager.ActiveDevice.DeviceClass == InputDeviceClass.Keyboard)
			{
				inputDevice = InputManager.ActiveDevice;
			}
		}
		else if (InputManager.Devices != null)
		{
			foreach (InputDevice inputDevice2 in InputManager.Devices)
			{
				if (inputDevice2.DeviceClass == InputDeviceClass.Controller)
				{
					inputDevice = inputDevice2;
					break;
				}
			}
		}
		GameInput.InputType inputType = ((inputDevice != null || SystemInfo.deviceType == DeviceType.Console || DebugOptions.opts.forceGamepadUI) ? GameInput.InputType.Gamepad : GameInput.InputType.KeyboardAndMouse);
		string text = ((inputDevice != null) ? inputDevice.Name : null);
		GameInput.ControllerType controllerType = GameInput._activeControllerType;
		if (text != null)
		{
			if (text == "PlayStation 4 Controller" || text == "PlayStation 3 Controller")
			{
				controllerType = GameInput.ControllerType.PlayStation;
			}
			else if (text.IndexOf("Xbox", StringComparison.OrdinalIgnoreCase) != -1)
			{
				controllerType = GameInput.ControllerType.Xbox;
			}
			else if (text.IndexOf("Switch", StringComparison.OrdinalIgnoreCase) != -1)
			{
				controllerType = GameInput.ControllerType.Switch;
			}
			else if (inputDevice.DeviceStyle == InputDeviceStyle.AppleMFi)
			{
				controllerType = GameInput.ControllerType.MFI;
			}
			if (SteamUtils.IsSteamRunningOnSteamDeck())
			{
				controllerType = GameInput.ControllerType.Deck;
			}
			else if (SystemInfo.deviceModel.Contains("The Wine Project"))
			{
				controllerType = GameInput.ControllerType.Deck;
			}
		}
		if (MonoSingleton<TestMenu>.isInstantiated && MonoSingleton<TestMenu>.instance.forceSwitchController)
		{
			inputType = GameInput.InputType.Gamepad;
			controllerType = GameInput.ControllerType.Switch;
		}
		if (inputType != GameInput.activeInputType)
		{
			GameInput.SetActiveInputType(inputType, forceEvent);
		}
		if (controllerType != GameInput._activeControllerType)
		{
			GameInput.SetActiveControllerType(controllerType, forceEvent);
		}
	}

	// Token: 0x0600018C RID: 396 RVA: 0x0000DF3C File Offset: 0x0000C13C
	private void Update()
	{
		this.UpdateInputType();
	}

	// Token: 0x1700008A RID: 138
	// (get) Token: 0x0600018D RID: 397 RVA: 0x0000DF44 File Offset: 0x0000C144
	public bool hoveringOverLegacyGUI
	{
		get
		{
			return GUIUtility.hotControl != 0;
		}
	}

	// Token: 0x1700008B RID: 139
	// (get) Token: 0x0600018E RID: 398 RVA: 0x0000DF4E File Offset: 0x0000C14E
	public bool hoveringOverUI
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700008C RID: 140
	// (get) Token: 0x0600018F RID: 399 RVA: 0x0000DF54 File Offset: 0x0000C154
	public bool hoveringOverGameView
	{
		get
		{
			return Input.mousePosition.x >= 0f && Input.mousePosition.x <= (float)Screen.width && Input.mousePosition.y >= 0f && Input.mousePosition.y <= (float)Screen.height;
		}
	}

	// Token: 0x06000190 RID: 400 RVA: 0x0000DFAC File Offset: 0x0000C1AC
	private void UpdateInputType()
	{
		if (Input.GetMouseButtonDown(0))
		{
			this.lastClickStartedOverLegacyGUI = this.hoveringOverLegacyGUI;
			this.lastClickStartedOverUI = this.hoveringOverUI;
		}
		if (Input.GetMouseButtonUp(0))
		{
			this.lastClickStartedOverLegacyGUI = false;
			this.lastClickStartedOverUI = false;
		}
		Vector2 vector = this.lastMousePosition - Input.mousePosition;
		this.lastMousePosition = Input.mousePosition;
		this.mouseMovedThisFrame = vector.magnitude > 3f;
		if (DebugOptions.opts.forceGamepadUI)
		{
			GameInput.SetActiveInputType(GameInput.InputType.Gamepad, false);
			return;
		}
		this.mouseDownThisFrame = Input.GetMouseButtonDown(0);
		if ((this.mouseMovedThisFrame || this.mouseDownThisFrame) && this.hoveringOverGameView)
		{
			GameInput.SetActiveInputType(GameInput.InputType.KeyboardAndMouse, false);
		}
		foreach (PlayerAction playerAction in this.mapping.Actions)
		{
			if (playerAction != this.mapping.move && playerAction != this.mapping.jump && this.GetStateEventIsFromKeyboard(playerAction))
			{
				GameInput.SetActiveInputType(GameInput.InputType.KeyboardAndMouse, false);
			}
		}
		if (InputManager.ActiveDevice != null && (InputManager.ActiveDevice.AnyButtonIsPressed || InputManager.ActiveDevice.LeftStick.State || InputManager.ActiveDevice.RightStick.State))
		{
			GameInput.SetActiveInputType(GameInput.InputType.Gamepad, false);
		}
	}

	// Token: 0x06000191 RID: 401 RVA: 0x0000E118 File Offset: 0x0000C318
	public bool GetStateEventIsFromKeyboard(PlayerAction action)
	{
		if (!action.State)
		{
			return false;
		}
		foreach (BindingSource bindingSource in action.Bindings)
		{
			if (bindingSource.DeviceName == "Keyboard" && bindingSource.GetState(null) == action.State)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000192 RID: 402 RVA: 0x0000E190 File Offset: 0x0000C390
	public static void ClearInputState()
	{
		Input.ResetInputAxes();
		InputManager.ClearInputState();
	}

	// Token: 0x06000193 RID: 403 RVA: 0x0000E19C File Offset: 0x0000C39C
	public static void SaveCustomMapping()
	{
		PlayerPrefsX.SetString(GameInput.customMappingPlayerPrefName, MonoSingleton<GameInput>.instance.mapping.Save());
	}

	// Token: 0x06000194 RID: 404 RVA: 0x0000E1B8 File Offset: 0x0000C3B8
	public static void LoadCustomMapping()
	{
		if (PlayerPrefsX.HasKey(GameInput.customMappingPlayerPrefName))
		{
			string @string = PlayerPrefsX.GetString(GameInput.customMappingPlayerPrefName, null);
			MonoSingleton<GameInput>.instance.mapping.Load(@string);
		}
	}

	// Token: 0x04000212 RID: 530
	public const string altControlsPrefName = "altControls";

	// Token: 0x04000213 RID: 531
	public const string wasdControlsPrefName = "wasdControls";

	// Token: 0x04000215 RID: 533
	private GameInputMapping _mapping;

	// Token: 0x04000216 RID: 534
	private static GameInput.InputType _activeInputType = GameInput.InputType.KeyboardAndMouse;

	// Token: 0x04000217 RID: 535
	public static Action<GameInput.InputType> onInputTypeChanged;

	// Token: 0x04000218 RID: 536
	private static GameInput.ControllerType _activeControllerType = GameInput.ControllerType.Xbox;

	// Token: 0x04000219 RID: 537
	public static Action<GameInput.ControllerType> onChangeControllerType;

	// Token: 0x0400021A RID: 538
	[NonSerialized]
	public bool mouseMovedThisFrame;

	// Token: 0x0400021B RID: 539
	[NonSerialized]
	public bool mouseDownThisFrame;

	// Token: 0x0400021C RID: 540
	[NonSerialized]
	public Vector2 lastMousePosition;

	// Token: 0x0400021D RID: 541
	[Disable]
	public bool lastClickStartedOverLegacyGUI;

	// Token: 0x0400021E RID: 542
	[Disable]
	public bool lastClickStartedOverUI;

	// Token: 0x0400021F RID: 543
	public static string customMappingPlayerPrefName = "InControlCustomMapping";

	// Token: 0x04000220 RID: 544
	private List<object> _controlStack = new List<object>();

	// Token: 0x0200026D RID: 621
	public enum InputType
	{
		// Token: 0x040014A5 RID: 5285
		KeyboardAndMouse,
		// Token: 0x040014A6 RID: 5286
		Gamepad
	}

	// Token: 0x0200026E RID: 622
	public enum ControllerType
	{
		// Token: 0x040014A8 RID: 5288
		Xbox,
		// Token: 0x040014A9 RID: 5289
		PlayStation,
		// Token: 0x040014AA RID: 5290
		Switch,
		// Token: 0x040014AB RID: 5291
		Deck,
		// Token: 0x040014AC RID: 5292
		MFI
	}
}
