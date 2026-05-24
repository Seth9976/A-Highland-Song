using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000050 RID: 80
public class Tutorial : MonoSingleton<Tutorial>
{
	// Token: 0x170000B4 RID: 180
	// (get) Token: 0x0600022B RID: 555 RVA: 0x00013754 File Offset: 0x00011954
	public bool visible
	{
		get
		{
			return this._activeTutorial > TutorialId.None;
		}
	}

	// Token: 0x170000B5 RID: 181
	// (get) Token: 0x0600022C RID: 556 RVA: 0x0001375F File Offset: 0x0001195F
	public TutorialId activeTutorial
	{
		get
		{
			return this._activeTutorial;
		}
	}

	// Token: 0x170000B6 RID: 182
	// (get) Token: 0x0600022D RID: 557 RVA: 0x00013767 File Offset: 0x00011967
	public SLayout mainPanelLayout
	{
		get
		{
			return this._mainPanelLayout;
		}
	}

	// Token: 0x0600022E RID: 558 RVA: 0x0001376F File Offset: 0x0001196F
	private static string PlayerPrefName(TutorialId id)
	{
		return "Tutorial__" + Enum.GetName(typeof(TutorialId), id);
	}

	// Token: 0x170000B7 RID: 183
	// (get) Token: 0x0600022F RID: 559 RVA: 0x00013790 File Offset: 0x00011990
	// (set) Token: 0x06000230 RID: 560 RVA: 0x0001379D File Offset: 0x0001199D
	private int lowStaminaTutorialShowCount
	{
		get
		{
			return PlayerPrefsX.GetInt("lowStaminaTutorialCount", 0);
		}
		set
		{
			PlayerPrefsX.SetInt("lowStaminaTutorialCount", value);
		}
	}

	// Token: 0x06000231 RID: 561 RVA: 0x000137AC File Offset: 0x000119AC
	public static void ResetAll()
	{
		if (MonoSingleton<Tutorial>.isInstantiated)
		{
			MonoSingleton<Tutorial>.instance.ResetCache();
		}
		for (TutorialId tutorialId = TutorialId.BasicMovement; tutorialId < TutorialId.TOTAL; tutorialId++)
		{
			PlayerPrefsX.DeleteKey(Tutorial.PlayerPrefName(tutorialId));
		}
		PlayerPrefsX.DeleteKey("lowStaminaTutorialCount");
		ClimbPrompt.ResetUpwardClimbCounter();
	}

	// Token: 0x06000232 RID: 562 RVA: 0x000137F4 File Offset: 0x000119F4
	private void ResetCache()
	{
		for (int i = 0; i < this._completedTutorials.Length; i++)
		{
			this._completedTutorials[i] = false;
		}
	}

	// Token: 0x06000233 RID: 563 RVA: 0x0001381D File Offset: 0x00011A1D
	public void Clear()
	{
		this.RemoveTutorial(true);
		this._lastAudioConfigChangeTime = double.MinValue;
	}

	// Token: 0x06000234 RID: 564 RVA: 0x00013835 File Offset: 0x00011A35
	public void JournalWasOpened()
	{
		this._lastAudioConfigChangeTime = double.MinValue;
	}

	// Token: 0x06000235 RID: 565 RVA: 0x00013848 File Offset: 0x00011A48
	private void Start()
	{
		this._mainPanelLayout.groupAlpha = 0f;
		this._tooltipLayout.groupAlpha = 0f;
		this._moveLeftIcon.gameObject.SetActive(false);
		this._moveRightIcon.gameObject.SetActive(false);
		this._lStickWiggle.gameObject.SetActive(false);
		this._jumpIcon.gameObject.SetActive(false);
		this._markerIcon.gameObject.SetActive(false);
		this._holdDownIcon.gameObject.SetActive(false);
		this._journalIcon.gameObject.SetActive(false);
		this._staminaIcon.gameObject.SetActive(false);
		this._altJumpButton.gameObject.SetActive(false);
		this._musicCalibIcon.gameObject.SetActive(false);
		if (DebugOptions.opts.resetTutorialOnLoad)
		{
			Tutorial.ResetAll();
			return;
		}
		this.LoadCacheIfNecessary();
	}

	// Token: 0x06000236 RID: 566 RVA: 0x00013937 File Offset: 0x00011B37
	private void OnEnable()
	{
		AudioSettings.OnAudioConfigurationChanged += this.OnAudioConfigurationChanged;
	}

	// Token: 0x06000237 RID: 567 RVA: 0x0001394A File Offset: 0x00011B4A
	private void OnDisable()
	{
		AudioSettings.OnAudioConfigurationChanged -= this.OnAudioConfigurationChanged;
	}

	// Token: 0x06000238 RID: 568 RVA: 0x0001395D File Offset: 0x00011B5D
	private void OnAudioConfigurationChanged(bool deviceWasChanged)
	{
		this._lastAudioConfigChangeTime = Time.unscaledTimeAsDouble;
	}

	// Token: 0x06000239 RID: 569 RVA: 0x0001396C File Offset: 0x00011B6C
	public bool HasDone(TutorialId id)
	{
		if (id < TutorialId.None || id >= (TutorialId)this._completedTutorials.Length)
		{
			return false;
		}
		this.LoadCacheIfNecessary();
		return this._completedTutorials[(int)id];
	}

	// Token: 0x0600023A RID: 570 RVA: 0x0001399A File Offset: 0x00011B9A
	public void AddTriggerZone(TutorialTrigger trigger)
	{
		if (!this._triggeredZones.Contains(trigger))
		{
			this._triggeredZones.Add(trigger);
		}
	}

	// Token: 0x0600023B RID: 571 RVA: 0x000139B8 File Offset: 0x00011BB8
	public void RemoveTriggerWithZone(TriggerZone zone)
	{
		this._triggeredZones.RemoveAll((TutorialTrigger t) => t.zone == zone);
	}

	// Token: 0x0600023C RID: 572 RVA: 0x000139EC File Offset: 0x00011BEC
	private void Update()
	{
		if (!Game.loaded || WorldManager.instance.loading)
		{
			return;
		}
		TutorialId tutorialId = TutorialId.None;
		SLayout slayout = null;
		SLayout slayout2 = null;
		string text = null;
		float num = 1f;
		bool flag = false;
		bool flag2 = false;
		Vector2 vector = default(Vector2);
		bool flag3 = true;
		Runner instance = Runner.instance;
		if (instance.staminaIsLow)
		{
			if (instance.running)
			{
				this._timeSpentRunningWithLowStamina += Time.deltaTime;
			}
		}
		else
		{
			this._timeSpentRunningWithLowStamina = 0f;
		}
		float num2 = Tutorial._lowStaminaThresholdSeconds[Mathf.Min(this.lowStaminaTutorialShowCount, Tutorial._lowStaminaThresholdSeconds.Length - 1)];
		if (!this.HasDone(TutorialId.Jumping) && this.HasDone(TutorialId.BasicMovement))
		{
			if (this._triggeredZones.Exists((TutorialTrigger t) => t.tutorialId == TutorialId.Jumping) && instance.running)
			{
				tutorialId = TutorialId.Jumping;
				text = "Jump";
				slayout = this._jumpIcon;
				goto IL_0589;
			}
		}
		if (!this.HasDone(TutorialId.DropDown))
		{
			if (this._triggeredZones.Exists((TutorialTrigger t) => t.tutorialId == TutorialId.DropDown) && instance.running)
			{
				tutorialId = TutorialId.DropDown;
				text = "Hold <color=#DDDD55>down</color> and press <color=#DDDD55>jump</color> to drop down.";
				slayout = this._holdDownIcon;
				slayout2 = this._jumpIcon;
				goto IL_0589;
			}
		}
		if (!this.HasDone(TutorialId.MusicRunFirstJump) && MonoSingleton<RunTrack>.instance.playing && instance.running)
		{
			RhythmActionMarker nearestMarker = MonoSingleton<TrackBuilder>.instance.nearestMarker;
			if (nearestMarker != null && this._settings.musicRunAllowObsPauseRange.Contains(instance.position.x - nearestMarker.transform.position.x))
			{
				tutorialId = TutorialId.MusicRunFirstJump;
				text = "Jump to the music!";
				slayout = this._jumpIcon;
				num = 0f;
			}
		}
		else if (!this.HasDone(TutorialId.MusicRunningSpecialJump) && MonoSingleton<RunTrack>.instance.playing && instance.running)
		{
			RhythmActionMarker nearestMarker2 = MonoSingleton<TrackBuilder>.instance.nearestMarker;
			if (nearestMarker2 != null && nearestMarker2.obstacleRef.special && this._settings.musicRunAllowObsPauseRange.Contains(instance.position.x - nearestMarker2.transform.position.x))
			{
				tutorialId = TutorialId.MusicRunningSpecialJump;
				if (GameInput.activeInputType == GameInput.InputType.Gamepad)
				{
					text = "At the <color=#12D4CC>RINGED MARKERS</color> press:";
					slayout2 = this._altJumpButton;
				}
				else
				{
					text = "At the <color=#12D4CC>RINGED MARKERS</color>\npress <color=#12D4CC>UP</color> to jump";
					slayout = this._altJumpButton;
				}
				num = 0f;
			}
		}
		else if (!this.HasDone(TutorialId.PlaceMarker) && MonoSingleton<PeakStateController>.instance.active && MonoSingleton<MapsViewController>.instance.mapIsSelected)
		{
			tutorialId = TutorialId.PlaceMarker;
			text = "<color=#DDDD55>Mark the spot</color> where you\nthink the <color=#DDDD55>map is pointing</color> to.";
			slayout = this._markerIcon;
		}
		else if (!this.HasDone(TutorialId.JournalMapMechanic) && !instance.isMusicRunning && MonoSingleton<MapsViewController>.instance.inVicinityPromptableMap != null && !MonoSingleton<MapsViewController>.instance.isBusy && Game.instance.inActiveGameplay && !Game.gameplayPaused)
		{
			Map inVicinityPromptableMap = MonoSingleton<MapsViewController>.instance.inVicinityPromptableMap;
			if (MonoSingleton<MapsViewController>.instance.PlayerIsInCorrectPrimaryLocationForMap(inVicinityPromptableMap))
			{
				tutorialId = TutorialId.JournalMapMechanic;
				text = "To <color=#DDDD55>confirm a map</color> of this location, open the <color=#DDDD55>Journal</color>.";
				slayout = this._journalIcon;
			}
		}
		else if (Time.unscaledTimeAsDouble < this._lastAudioConfigChangeTime + 10.0)
		{
			tutorialId = TutorialId.AudioDeviceChanged;
			text = "<b>Audio configuration changed</b>\nYou can <color=#DDDD55>calibrate musical timing</color> from the main menu.";
			slayout = this._musicCalibIcon;
			num = (float)(MonoSingleton<RunTrack>.instance.playing ? 0 : 1);
		}
		else if (!this.HasDone(TutorialId.Stamina) && this._timeSpentRunningWithLowStamina > Tutorial._lowStaminaThresholdSeconds[Mathf.Min(this.lowStaminaTutorialShowCount, Tutorial._lowStaminaThresholdSeconds.Length - 1)] && !instance.catchingBreath)
		{
			tutorialId = TutorialId.Stamina;
			text = "<color=#DDDD55>Stop</color> for a moment to <color=#DDDD55>catch your breath</color>";
			slayout = this._staminaIcon;
		}
		else if ((!this.HasDone(TutorialId.MaxHealthReduce) && instance.health.shelterComfort != Health.ShelterComfort.NONE && instance.health.activeHealthEffect.maxHealthPerDay < 0f) || this._activeTutorial == TutorialId.MaxHealthReduce)
		{
			tutorialId = TutorialId.MaxHealthReduce;
			text = "When sleep is <color=#DD5555>uncomfortable</color>, <color=#DD5555>maximum health</color> is reduced.";
			flag2 = true;
			flag3 = true;
			vector = MonoSingleton<GameUI>.instance.healthUI.topMaxHealthPosInCanvas + new Vector2(10f, 0f);
		}
		else if ((!this.HasDone(TutorialId.MaxHealthIncrease) && instance.health.shelterComfort != Health.ShelterComfort.NONE && instance.health.activeHealthEffect.maxHealthPerDay > 0f) || this._activeTutorial == TutorialId.MaxHealthIncrease)
		{
			tutorialId = TutorialId.MaxHealthIncrease;
			text = "When sleep is <color=#55DD55>comfortable</color>, <color=#DDDD55>maximum health</color> is increased.";
			flag2 = true;
			flag3 = true;
			vector = MonoSingleton<GameUI>.instance.healthUI.topMaxHealthPosInCanvas + new Vector2(10f, 0f);
		}
		else if (!this.HasDone(TutorialId.RestOutside) && instance.health.availableHeal != AvailableHeal.None && instance.health.currentHealth < 0.7f * instance.health.currentMaxHealth && GameClock.instance.hourOfDay > 6f && GameClock.instance.hourOfDay < 14f && MonoSingleton<RestPromptUI>.instance.visibleAndEnabled && !NarrativePresenter.showingSubtitles)
		{
			tutorialId = TutorialId.RestOutside;
			text = "You can sit down and rest to recover health.";
			flag2 = true;
			flag3 = false;
			vector = MonoSingleton<RestPromptUI>.instance.leftMidEdgeInCanvasSpace;
		}
		else if ((!this.HasDone(TutorialId.WeatherEffects) && instance.health.availableHeal == AvailableHeal.None && instance.health.currentHealth < instance.health.currentMaxHealth && MonoSingleton<RestStateController>.instance.sitting) || this._activeTutorial == TutorialId.WeatherEffects)
		{
			tutorialId = TutorialId.WeatherEffects;
			text = "<color=#5555DD>Wind and rain</color> prevents you from resting to recover health. <color=#DDDD55>Find shelter!</color>";
			flag2 = true;
			flag3 = true;
			vector = MonoSingleton<GameUI>.instance.healthUI.activeWeatherIconInCanvas;
		}
		IL_0589:
		if ((tutorialId == TutorialId.None && MonoSingleton<RunTrack>.instance.playing && instance.stumbleCountInRow >= Runner.instance.settings.run.stumbleCountMax) || this._activeTutorial == TutorialId.MusicRunJumpReminder)
		{
			tutorialId = TutorialId.MusicRunJumpReminder;
			if (this.HasDone(TutorialId.MusicRunningSpecialJump))
			{
				if (GameInput.activeInputType == GameInput.InputType.Gamepad)
				{
					text = "At light markers press the indicated jump buttons";
				}
				else
				{
					text = "At <color=#DDDD55>light markers</color> press jump.\nAt <color=#12D4CC>RINGED MARKERS</color> press <color=#12D4CC>UP</color> only.";
				}
				slayout = this._jumpIcon;
				slayout2 = this._altJumpButton;
			}
			else
			{
				text = "Jump at light markers!";
				slayout = this._jumpIcon;
			}
		}
		if (DebugOptions.opts.noTutorial)
		{
			tutorialId = TutorialId.None;
		}
		if (!this._mainPanelLayout.isAnimating && !this._tooltipLayout.isAnimating)
		{
			if (this._activeTutorial == TutorialId.None && tutorialId != TutorialId.None)
			{
				if (flag2)
				{
					this.ShowTooltip(tutorialId, text, flag3, vector);
				}
				else
				{
					this.ShowTutorial(tutorialId, text, slayout, slayout2, num, flag);
				}
				if (tutorialId == TutorialId.MusicRunFirstJump || tutorialId == TutorialId.MusicRunningSpecialJump)
				{
					MonoSingleton<RunTrack>.instance.SetPaused(true, RunTrack.PauseReason.Tutorial);
				}
			}
			else if (this._activeTutorial != TutorialId.None && this._activeTutorial != tutorialId)
			{
				if (this._activeTutorial == TutorialId.Stamina && !instance.staminaIsLow && this._tutorialShownFor > 3f)
				{
					int lowStaminaTutorialShowCount = this.lowStaminaTutorialShowCount;
					this.lowStaminaTutorialShowCount = lowStaminaTutorialShowCount + 1;
				}
				this.RemoveTutorial(false);
			}
			else if (tutorialId != TutorialId.None && this._activeTutorial == tutorialId && flag2)
			{
				this.UpdateTooltipPos(flag3, vector);
			}
		}
		if (GameInput.moveLeftRight != 0f)
		{
			this.CompleteTutorial(TutorialId.BasicMovement);
		}
		if (instance.jumping)
		{
			this.CompleteTutorial(TutorialId.Jumping);
		}
		if (instance.droppingDown)
		{
			this.CompleteTutorial(TutorialId.DropDown);
		}
		if (MonoSingleton<RunTrack>.instance.playing && !this.HasDone(TutorialId.MusicRunFirstJump) && instance.jumping)
		{
			if (MonoSingleton<RunTrack>.instance.paused)
			{
				MonoSingleton<RunTrack>.instance.SetPaused(false, RunTrack.PauseReason.Tutorial);
			}
			this.CompleteTutorial(TutorialId.MusicRunFirstJump);
		}
		if (MonoSingleton<RunTrack>.instance.playing && !this.HasDone(TutorialId.MusicRunningSpecialJump) && instance.jumping && instance.jumpIsSpecial)
		{
			if (MonoSingleton<RunTrack>.instance.paused)
			{
				MonoSingleton<RunTrack>.instance.SetPaused(false, RunTrack.PauseReason.Tutorial);
			}
			this.CompleteTutorial(TutorialId.MusicRunningSpecialJump);
		}
		if (MonoSingleton<PeakStateController>.instance.active && MonoSingleton<MapsViewController>.instance.maximised)
		{
			if (MonoSingleton<RunTrack>.instance.paused)
			{
				MonoSingleton<RunTrack>.instance.SetPaused(false, RunTrack.PauseReason.Tutorial);
			}
			this.CompleteTutorial(TutorialId.MapsViewedAtPeak);
		}
		if (MonoSingleton<JournalController>.instance.mapConfirmActive)
		{
			this.CompleteTutorial(TutorialId.JournalMapMechanic);
		}
		if (this.lowStaminaTutorialShowCount > 6)
		{
			this.CompleteTutorial(TutorialId.Stamina);
		}
		if (this._activeTutorial == TutorialId.MaxHealthReduce && this._tutorialShownFor > 12f)
		{
			this.CompleteTutorial(this._activeTutorial);
		}
		if (this._activeTutorial == TutorialId.MaxHealthIncrease && this._tutorialShownFor > 12f)
		{
			this.CompleteTutorial(this._activeTutorial);
		}
		if (this._activeTutorial == TutorialId.MusicRunJumpReminder && this._tutorialShownFor > 8f)
		{
			this.RemoveTutorial(false);
		}
		if ((MonoSingleton<RestStateController>.instance.resting && instance.health.activeHealthEffect.healthPerDay > 0f) || (this._activeTutorial == TutorialId.RestOutside && this._tutorialShownFor > 12f))
		{
			this.CompleteTutorial(TutorialId.RestOutside);
		}
		if (this._activeTutorial == TutorialId.WeatherEffects && this._tutorialShownFor > 12f)
		{
			this.CompleteTutorial(this._activeTutorial);
		}
		if (this._timeScale == 1f)
		{
			Game.instance.RemoveTimeScalar(Game.TimeScalar.Tutorial);
		}
		else
		{
			Game.instance.SetTimeScalar(Game.TimeScalar.Tutorial, this._timeScale);
		}
		if (this._activeTutorial != TutorialId.None)
		{
			this._tutorialShownFor += Time.unscaledDeltaTime;
		}
	}

	// Token: 0x0600023D RID: 573 RVA: 0x000142F4 File Offset: 0x000124F4
	private void ShowTutorial(TutorialId tutorialId, string text, SLayout leftIcon, SLayout rightIcon, float timeScale = 1f, bool lowPosition = false)
	{
		this._activeTutorial = tutorialId;
		float num = this._settings.marginX;
		if (leftIcon != null)
		{
			leftIcon.gameObject.SetActive(true);
			leftIcon.groupAlpha = 1f;
			leftIcon.x = num;
			num += leftIcon.width + this._settings.innerPadding;
			this._activeIcons.Add(leftIcon);
		}
		Vector2 preferredValues = this._text.textMeshPro.GetPreferredValues(text, this._settings.maxTextWidth, 500f);
		if (preferredValues.x > this._settings.maxTextWidth)
		{
			preferredValues.x = this._settings.maxTextWidth;
		}
		this._text.textMeshPro.text = text;
		this._text.x = num;
		this._text.width = preferredValues.x;
		this._text.height = preferredValues.y;
		this._text.textMeshPro.ForceMeshUpdate(false, false);
		num += this._text.width;
		if (rightIcon != null)
		{
			rightIcon.gameObject.SetActive(true);
			num += this._settings.innerPadding;
			rightIcon.x = num;
			num += rightIcon.width;
			this._activeIcons.Add(rightIcon);
		}
		num += this._settings.marginX;
		this._mainPanelLayout.width = num;
		this._mainPanelLayout.height = preferredValues.y + 2f * this._settings.marginY;
		this._mainPanelLayout.centerX = 0.5f * this._mainPanelLayout.parentRect.width;
		this._text.centerY = this._mainPanelLayout.middleY;
		if (this._mainPanelLayout.groupAlpha == 0f)
		{
			this._mainPanelLayout.centerY = 0f;
		}
		this._mainPanelLayout.Animate(0.5f, delegate
		{
			this._mainPanelLayout.groupAlpha = 1f;
			this._mainPanelLayout.centerY = (lowPosition ? this._settings.lowY : this._settings.highY);
		});
		this._timeScale = timeScale;
	}

	// Token: 0x0600023E RID: 574 RVA: 0x0001451C File Offset: 0x0001271C
	private void ShowTooltip(TutorialId tutorialId, string text, bool tooltipPointsLeft, Vector2 tooltipAttach)
	{
		this._activeTutorial = tutorialId;
		Vector2 preferredValues = this._tooltipText.textMeshPro.GetPreferredValues(text, this._settings.tooltipMaxTextWidth, 500f);
		if (preferredValues.x > this._settings.tooltipMaxTextWidth)
		{
			preferredValues.x = this._settings.tooltipMaxTextWidth;
		}
		this._tooltipText.textMeshPro.text = text;
		this._tooltipText.width = preferredValues.x;
		this._tooltipText.height = preferredValues.y;
		this._tooltipText.textMeshPro.ForceMeshUpdate(false, false);
		float x = this._tooltipText.x;
		float y = this._tooltipText.y;
		float num = preferredValues.x + 2f * x;
		this._tooltipLayout.width = num;
		this._tooltipLayout.height = preferredValues.y + 2f * y;
		RectTransform rectTransform = this._tooltipNubbin.rectTransform;
		Vector2 vector = new Vector2((float)(tooltipPointsLeft ? 0 : 1), 0.5f);
		rectTransform.anchorMax = vector;
		rectTransform.anchorMin = vector;
		this._tooltipNubbin.x = (tooltipPointsLeft ? 1f : (num - 1f));
		this._tooltipNubbin.transform.localScale = new Vector3((float)(tooltipPointsLeft ? 1 : (-1)), 1f, 1f);
		this._tooltipLayout.pivot = new Vector2((float)(tooltipPointsLeft ? 0 : 1), 0.5f);
		this.UpdateTooltipPos(tooltipPointsLeft, tooltipAttach);
		this._tooltipLayout.Animate(0.5f, delegate
		{
			this._tooltipLayout.groupAlpha = 1f;
		});
		this._tooltipLayout.scale = 0.2f;
		this._tooltipLayout.Animate(0.4f, 0f, SLayout.popCurve, delegate
		{
			this._tooltipLayout.scale = 1f;
		});
	}

	// Token: 0x0600023F RID: 575 RVA: 0x000146F4 File Offset: 0x000128F4
	private void UpdateTooltipPos(bool tooltipPointsLeft, Vector2 tooltipAttach)
	{
		if (tooltipPointsLeft)
		{
			this._tooltipLayout.x = tooltipAttach.x + this._settings.tooltipXOffsetFromAnchor;
		}
		else
		{
			this._tooltipLayout.rightX = tooltipAttach.x - this._settings.tooltipXOffsetFromAnchor;
		}
		this._tooltipLayout.centerY = tooltipAttach.y;
	}

	// Token: 0x06000240 RID: 576 RVA: 0x00014751 File Offset: 0x00012951
	public void CompleteTutorial(TutorialId completedId)
	{
		if (this.HasDone(completedId))
		{
			return;
		}
		this.MakeCompletedInCache(completedId);
		PlayerPrefsX.SetInt(Tutorial.PlayerPrefName(completedId), 1);
		if (completedId == this._activeTutorial)
		{
			this.RemoveTutorial(false);
		}
	}

	// Token: 0x06000241 RID: 577 RVA: 0x00014780 File Offset: 0x00012980
	private void MakeCompletedInCache(TutorialId id)
	{
		if (id <= TutorialId.None)
		{
			return;
		}
		this.LoadCacheIfNecessary();
		this._completedTutorials[(int)id] = true;
	}

	// Token: 0x06000242 RID: 578 RVA: 0x000147A4 File Offset: 0x000129A4
	private void LoadCacheIfNecessary()
	{
		if (this._hasCachedTutorials)
		{
			return;
		}
		for (int i = 0; i < 18; i++)
		{
			TutorialId tutorialId = (TutorialId)i;
			this._completedTutorials[i] = PlayerPrefsX.GetInt(Tutorial.PlayerPrefName(tutorialId), 0) != 0;
		}
		this._hasCachedTutorials = true;
	}

	// Token: 0x06000243 RID: 579 RVA: 0x000147E8 File Offset: 0x000129E8
	private void RemoveTutorial(bool immediate)
	{
		this._timeScale = 1f;
		this._tutorialShownFor = 0f;
		if (this._activeTutorial == TutorialId.None)
		{
			return;
		}
		if (this._mainPanelLayout.targetGroupAlpha > 0f)
		{
			this._mainPanelLayout.Animate(immediate ? 0f : 0.5f, delegate
			{
				this._mainPanelLayout.groupAlpha = 0f;
			}).Then(delegate
			{
				foreach (SLayout slayout in this._activeIcons)
				{
					slayout.gameObject.SetActive(false);
				}
				this._activeIcons.Clear();
				this._activeTutorial = TutorialId.None;
			});
			return;
		}
		this._tooltipLayout.Animate(immediate ? 0f : 0.5f, delegate
		{
			this._tooltipLayout.groupAlpha = 0f;
		}).Then(delegate
		{
			this._activeTutorial = TutorialId.None;
		});
	}

	// Token: 0x04000363 RID: 867
	private bool[] _completedTutorials = new bool[18];

	// Token: 0x04000364 RID: 868
	private bool _hasCachedTutorials;

	// Token: 0x04000365 RID: 869
	private float _timeScale = 1f;

	// Token: 0x04000366 RID: 870
	private TutorialId _activeTutorial;

	// Token: 0x04000367 RID: 871
	private List<SLayout> _activeIcons = new List<SLayout>(2);

	// Token: 0x04000368 RID: 872
	private float _tutorialShownFor;

	// Token: 0x04000369 RID: 873
	private float _timeSpentRunningWithLowStamina;

	// Token: 0x0400036A RID: 874
	private List<TutorialTrigger> _triggeredZones = new List<TutorialTrigger>(2);

	// Token: 0x0400036B RID: 875
	private double _lastAudioConfigChangeTime = double.MinValue;

	// Token: 0x0400036C RID: 876
	private static float[] _lowStaminaThresholdSeconds = new float[] { 1f, 2f, 4f, 8f };

	// Token: 0x0400036D RID: 877
	[SerializeField]
	private SLayout _mainPanelLayout;

	// Token: 0x0400036E RID: 878
	[SerializeField]
	private SLayout _tooltipLayout;

	// Token: 0x0400036F RID: 879
	[SerializeField]
	private SLayout _tooltipNubbin;

	// Token: 0x04000370 RID: 880
	[SerializeField]
	private SLayout _text;

	// Token: 0x04000371 RID: 881
	[SerializeField]
	private SLayout _tooltipText;

	// Token: 0x04000372 RID: 882
	[SerializeField]
	private SLayout _moveLeftIcon;

	// Token: 0x04000373 RID: 883
	[SerializeField]
	private SLayout _moveRightIcon;

	// Token: 0x04000374 RID: 884
	[SerializeField]
	private SLayout _lStickWiggle;

	// Token: 0x04000375 RID: 885
	[SerializeField]
	private SLayout _jumpIcon;

	// Token: 0x04000376 RID: 886
	[SerializeField]
	private SLayout _holdDownIcon;

	// Token: 0x04000377 RID: 887
	[SerializeField]
	private SLayout _markerIcon;

	// Token: 0x04000378 RID: 888
	[SerializeField]
	private SLayout _journalIcon;

	// Token: 0x04000379 RID: 889
	[SerializeField]
	private SLayout _staminaIcon;

	// Token: 0x0400037A RID: 890
	[SerializeField]
	private SLayout _altJumpButton;

	// Token: 0x0400037B RID: 891
	[SerializeField]
	private SLayout _musicCalibIcon;

	// Token: 0x0400037C RID: 892
	[SerializeField]
	private TutorialSettings _settings;
}
