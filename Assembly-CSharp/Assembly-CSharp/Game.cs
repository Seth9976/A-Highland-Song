using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000028 RID: 40
public class Game : StateFuncMachine
{
	// Token: 0x14000001 RID: 1
	// (add) Token: 0x060000C3 RID: 195 RVA: 0x0000AA3C File Offset: 0x00008C3C
	// (remove) Token: 0x060000C4 RID: 196 RVA: 0x0000AA70 File Offset: 0x00008C70
	public static event Action<GameUI> onUIPositionUpdate;

	// Token: 0x1700002B RID: 43
	// (get) Token: 0x060000C5 RID: 197 RVA: 0x0000AAA3 File Offset: 0x00008CA3
	public static Game instance
	{
		get
		{
			return GSR.Game;
		}
	}

	// Token: 0x1700002C RID: 44
	// (get) Token: 0x060000C6 RID: 198 RVA: 0x0000AAAC File Offset: 0x00008CAC
	public static bool gameplayPaused
	{
		get
		{
			return !Game.loaded || JournalController.wantsGameplayPaused || TitleScreen.wantsGameplayPaused || SplashSequence.wantsGameplayPaused || Dialogue.wantsGameplayPaused || FeedbackController.wantsGameplayPaused || Credits.wantsGameplayPaused || Game.stateWantsGameplayPaused || TestMenu.wantsGameplayPaused || PhotoMode.wantsGameplayPaused;
		}
	}

	// Token: 0x1700002D RID: 45
	// (get) Token: 0x060000C7 RID: 199 RVA: 0x0000AAFF File Offset: 0x00008CFF
	public bool inActiveGameplay
	{
		get
		{
			return base.state == this.activeGameplayState;
		}
	}

	// Token: 0x1700002E RID: 46
	// (get) Token: 0x060000C8 RID: 200 RVA: 0x0000AB12 File Offset: 0x00008D12
	public bool inPeakState
	{
		get
		{
			return base.state == this.peakState;
		}
	}

	// Token: 0x1700002F RID: 47
	// (get) Token: 0x060000C9 RID: 201 RVA: 0x0000AB25 File Offset: 0x00008D25
	public bool inTitleScreenAndIntroState
	{
		get
		{
			return base.state == this.titleScreenAndIntroState;
		}
	}

	// Token: 0x17000030 RID: 48
	// (get) Token: 0x060000CA RID: 202 RVA: 0x0000AB38 File Offset: 0x00008D38
	public bool inEndGameSequence
	{
		get
		{
			return base.state == this.endOfGameState;
		}
	}

	// Token: 0x17000031 RID: 49
	// (get) Token: 0x060000CB RID: 203 RVA: 0x0000AB4B File Offset: 0x00008D4B
	// (set) Token: 0x060000CC RID: 204 RVA: 0x0000AB53 File Offset: 0x00008D53
	public bool showingIntro { get; private set; }

	// Token: 0x17000032 RID: 50
	// (get) Token: 0x060000CD RID: 205 RVA: 0x0000AB5C File Offset: 0x00008D5C
	public bool looking
	{
		get
		{
			return this._lookModeActive;
		}
	}

	// Token: 0x17000033 RID: 51
	// (get) Token: 0x060000CE RID: 206 RVA: 0x0000AB64 File Offset: 0x00008D64
	public bool lookingFurther
	{
		get
		{
			return this._lookFurtherActive;
		}
	}

	// Token: 0x17000034 RID: 52
	// (get) Token: 0x060000CF RID: 207 RVA: 0x0000AB6C File Offset: 0x00008D6C
	public bool canLookFurther
	{
		get
		{
			return (PropsController.instance.triggerFlags & (TriggerFlags.VeryShortZoomOut | TriggerFlags.ShortZoomOut | TriggerFlags.MediumZoomOut)) <= TriggerFlags.None && !CaveRegion.inCave && !MonoSingleton<RunTrack>.instance.playingOrAboutTo;
		}
	}

	// Token: 0x17000035 RID: 53
	// (get) Token: 0x060000D0 RID: 208 RVA: 0x0000AB96 File Offset: 0x00008D96
	public Boat activeBoat
	{
		get
		{
			return this._activeBoat;
		}
	}

	// Token: 0x17000036 RID: 54
	// (get) Token: 0x060000D1 RID: 209 RVA: 0x0000AB9E File Offset: 0x00008D9E
	// (set) Token: 0x060000D2 RID: 210 RVA: 0x0000ABA6 File Offset: 0x00008DA6
	public bool deathSequenceActive { get; private set; }

	// Token: 0x17000037 RID: 55
	// (get) Token: 0x060000D3 RID: 211 RVA: 0x0000ABAF File Offset: 0x00008DAF
	// (set) Token: 0x060000D4 RID: 212 RVA: 0x0000ABB7 File Offset: 0x00008DB7
	public bool startedFreshNewGame { get; private set; }

	// Token: 0x17000038 RID: 56
	// (get) Token: 0x060000D5 RID: 213 RVA: 0x0000ABC0 File Offset: 0x00008DC0
	public bool newGamePlus
	{
		get
		{
			return this.playthroughIdx >= 1;
		}
	}

	// Token: 0x17000039 RID: 57
	// (get) Token: 0x060000D6 RID: 214 RVA: 0x0000ABCE File Offset: 0x00008DCE
	public float playthroughSeed
	{
		get
		{
			return (float)(this.playthroughIdx + 1) * 33.153f;
		}
	}

	// Token: 0x060000D7 RID: 215 RVA: 0x0000ABDF File Offset: 0x00008DDF
	public void StartPeakState()
	{
		if (this.inPeakState)
		{
			return;
		}
		base.state = this.peakState;
	}

	// Token: 0x1700003A RID: 58
	// (get) Token: 0x060000D8 RID: 216 RVA: 0x0000ABF6 File Offset: 0x00008DF6
	private Runner runner
	{
		get
		{
			return Runner.instance;
		}
	}

	// Token: 0x1700003B RID: 59
	// (get) Token: 0x060000D9 RID: 217 RVA: 0x0000ABFD File Offset: 0x00008DFD
	private GameCamera gameCam
	{
		get
		{
			return GameCamera.instance;
		}
	}

	// Token: 0x1700003C RID: 60
	// (get) Token: 0x060000DA RID: 218 RVA: 0x0000AC04 File Offset: 0x00008E04
	private PropsController propsController
	{
		get
		{
			return PropsController.instance;
		}
	}

	// Token: 0x1700003D RID: 61
	// (get) Token: 0x060000DB RID: 219 RVA: 0x0000AC0B File Offset: 0x00008E0B
	private Narrative narrative
	{
		get
		{
			return Narrative.instance;
		}
	}

	// Token: 0x1700003E RID: 62
	// (get) Token: 0x060000DC RID: 220 RVA: 0x0000AC12 File Offset: 0x00008E12
	private PeakStateController peakStateController
	{
		get
		{
			if (this._peakStateController == null)
			{
				this._peakStateController = base.GetComponentInChildren<PeakStateController>();
			}
			return this._peakStateController;
		}
	}

	// Token: 0x060000DD RID: 221 RVA: 0x0000AC34 File Offset: 0x00008E34
	public void ShowTitleScreen(bool midGame, bool newGamePlus)
	{
		base.StartCoroutine(this.ShowTitleScreen_Coroutine(midGame, newGamePlus));
	}

	// Token: 0x060000DE RID: 222 RVA: 0x0000AC45 File Offset: 0x00008E45
	private IEnumerator ShowTitleScreen_Coroutine(bool midGame, bool newGamePlus)
	{
		base.state = this.titleScreenAndIntroState;
		MonoSingleton<BlackBars>.instance.SetVisible(true, BlackBarsReason.TitleScreen);
		Narrative.NewGamePlusStrings ngPlusStrings = default(Narrative.NewGamePlusStrings);
		if (newGamePlus)
		{
			ngPlusStrings = Narrative.instance.NewGamePlusText();
		}
		this.Clear(Game.ClearType.VisualsOnly);
		if (midGame)
		{
			Blackout.FadeOut(null);
			while (Blackout.isAnimating)
			{
				yield return null;
			}
		}
		GameCamera.IntroCameraState introCameraState = GameCamera.instance.introCameraState;
		introCameraState.active = true;
		introCameraState.strength = 1f;
		GameCamera.instance.ResetDynamicWaterQuality();
		if (newGamePlus)
		{
			MonoSingleton<Dialogue>.instance.Show(ngPlusStrings.title, ngPlusStrings.subtitle, ngPlusStrings.buttonText, null, false, null);
			while (MonoSingleton<Dialogue>.instance.visible)
			{
				yield return null;
			}
		}
		while (MonoSingleton<SplashSequence>.instance.visible)
		{
			yield return null;
		}
		if (Blackout.showing)
		{
			Blackout.FadeIn(0f, null);
		}
		yield return new WaitForSecondsRealtime(2f);
		MonoSingleton<TitleScreen>.instance.Show(newGamePlus);
		yield break;
	}

	// Token: 0x060000DF RID: 223 RVA: 0x0000AC62 File Offset: 0x00008E62
	public void IntroSwoopAfterTitleScreen()
	{
		base.StartCoroutine(this.Intro_Coroutine(MonoSingleton<Main>.instance.isAtStart));
	}

	// Token: 0x060000E0 RID: 224 RVA: 0x0000AC7C File Offset: 0x00008E7C
	public void Clear(Game.ClearType clearType)
	{
		GameCamera.instance.Clear();
		MonoSingleton<RunTrack>.instance.Clear();
		MonoSingleton<RestStateController>.instance.Clear();
		PropsController.instance.Clear(clearType == Game.ClearType.VisualsOnly);
		MonoSingleton<Tutorial>.instance.Clear();
		Narrative instance = Narrative.instance;
		Narrative.ClearType clearType2;
		switch (clearType)
		{
		case Game.ClearType.VisualsOnly:
			clearType2 = Narrative.ClearType.Visual;
			break;
		case Game.ClearType.FullState:
			clearType2 = Narrative.ClearType.FullState;
			break;
		case Game.ClearType.FullStatePreservingLoopVariables:
			clearType2 = Narrative.ClearType.FullStatePreservingLoopVariables;
			break;
		default:
			throw new Exception("Unsupported ClearType");
		}
		instance.Clear(clearType2);
		NarrativePresenter.instance.Clear(false, false);
		GameClock.instance.CancelTimePassing();
		MonoSingleton<MapsViewController>.instance.Clear(clearType == Game.ClearType.VisualsOnly);
		AudioController.instance.Clear(clearType == Game.ClearType.FullStatePreservingLoopVariables || clearType == Game.ClearType.VisualsOnly);
		MonoSingleton<JournalController>.instance.Clear();
		MonoSingleton<ProgressBanner>.instance.Clear();
		MonoSingleton<NowPlaying>.instance.Clear();
		MonoSingleton<BlackBars>.instance.SetVisible(false, BlackBarsReason.FinalJump);
		MonoSingleton<BlackBars>.instance.SetVisible(false, BlackBarsReason.Ink);
		MonoSingleton<GodRayParticlesManager>.instance.Clear();
		this._timeScalars.Clear();
		if (clearType != Game.ClearType.VisualsOnly)
		{
			InkAnimation.StopAndResetAll();
			GameClock.instance.firstDayPauseEnabled = true;
			Runner.instance.ResetForGameStart();
			WeatherSystem.instance.Clear();
			MonoSingleton<Inventory>.instance.ClearAndGiveInitialMaps();
			Narrative.instance.SyncMapsInInventory();
			Crow.ResetAll();
			InkWalker.ResetAll();
			MonoSingleton<Eagle>.instance.Clear();
			Boat.ResetAll();
			this._activeBoat = null;
		}
	}

	// Token: 0x060000E1 RID: 225 RVA: 0x0000ADD7 File Offset: 0x00008FD7
	private void OnDestroy()
	{
		this.UnsubscribeFromStaticEvents();
	}

	// Token: 0x060000E2 RID: 226 RVA: 0x0000ADE0 File Offset: 0x00008FE0
	private void SubscribeToStaticEventsIfNecessary()
	{
		if (this.subscribedToStaticEvents)
		{
			return;
		}
		Runner.onTrip = (Action<float>)Delegate.Combine(Runner.onTrip, new Action<float>(this.OnTrip));
		PeakStateController.onRequestExit = (Action)Delegate.Combine(PeakStateController.onRequestExit, new Action(this.OnPeakStateRequestsExit));
		this.subscribedToStaticEvents = true;
	}

	// Token: 0x060000E3 RID: 227 RVA: 0x0000AE40 File Offset: 0x00009040
	private void UnsubscribeFromStaticEvents()
	{
		if (!this.subscribedToStaticEvents)
		{
			return;
		}
		Runner.onTrip = (Action<float>)Delegate.Remove(Runner.onTrip, new Action<float>(this.OnTrip));
		PeakStateController.onRequestExit = (Action)Delegate.Remove(PeakStateController.onRequestExit, new Action(this.OnPeakStateRequestsExit));
		this.subscribedToStaticEvents = false;
	}

	// Token: 0x060000E4 RID: 228 RVA: 0x0000AEA0 File Offset: 0x000090A0
	public void OnCompleteLoad()
	{
		this.SubscribeToStaticEventsIfNecessary();
		GameInput.RefreshActiveDevice(false);
		GSR.timeOfDayEffects.enabled = true;
		GSR.WeatherSystem.gameObject.SetActive(true);
		GameClock.instance.enabled = true;
		this.runner.enabled = true;
		MonoSingleton<TrackBuilder>.instance.enabled = true;
		MonoSingleton<RunTrack>.instance.enabled = true;
		this.gameCam.enabled = true;
		this.narrative.enabled = true;
		this.gameCam.Refresh(true);
		WeatherSystem.instance.OnCompleteLoad();
		if (MonoSingleton<BetweenLayersSeparator>.instance != null)
		{
			MonoSingleton<BetweenLayersSeparator>.instance.RefreshImmediate();
		}
		Narrative.instance.ResetLullRemarkTime();
		Narrative.instance.RefreshPeakWeatherModifierZones();
		Narrative.instance.ReCheckAchievements();
		NarrativePresenter.instance.recentMoiraDialogue.Clear();
		MonoSingleton<TestMenu>.instance.SetupOptions();
		this.RefreshChristmas();
		Game.loaded = true;
	}

	// Token: 0x060000E5 RID: 229 RVA: 0x0000AF8C File Offset: 0x0000918C
	public void BeginGameplay()
	{
		if (this.narrative.isInkBusy)
		{
			Debug.LogError("Ink was already active during game setup?! (" + this.narrative.debugInkEvaluationStartPoint + ") Could maybe change this to: if( !narrative.isInkBusy ) narrative.RefreshInteractablesChoices;");
		}
		this.narrative.OnCompleteLoad();
		PropsController.instance.Refresh();
		GameCamera.instance.introCameraState.active = false;
		base.state = this.activeGameplayState;
	}

	// Token: 0x060000E6 RID: 230 RVA: 0x0000AFF8 File Offset: 0x000091F8
	public void TeleportPlayerWithoutLevelChange(Vector3 playerPos, string placeContext, int lookDirection = 0, bool dontChangeState = false, bool immmediateCameraUpdate = true)
	{
		this.runner.transform.position = playerPos;
		this.runner.physicalDepthLayerIdx = Mathf.RoundToInt(playerPos.z);
		this.runner.momentum = 0f;
		if (lookDirection != 0)
		{
			this.runner.direction = (float)((lookDirection > 0) ? 1 : (-1));
		}
		if (Application.isPlaying)
		{
			this.PlaceRunnerOnGroundAndStartTrackBuilding(placeContext, immmediateCameraUpdate, dontChangeState);
			this.gameCam.Refresh(true);
			if (!this.narrative.isBusy)
			{
				this.narrative.RefreshInteractablesChoices(true, false);
			}
		}
	}

	// Token: 0x060000E7 RID: 231 RVA: 0x0000B08C File Offset: 0x0000928C
	public IEnumerator TeleportPlayerTo3DCR(Vector3 playerPos, string placeContext, int lookDirection = 0, bool fade = true, World specificWorld = null, bool dontChangeState = false, bool dontRefreshInteractables = false, Action onComplete = null, Action onPlaceOnGround = null)
	{
		int targetLevelIndex = Level.DepthToIndex(playerPos.z);
		if (Application.isPlaying)
		{
			this.runner.enabled = false;
			if (fade)
			{
				Blackout.FadeOut(null);
				yield return new WaitForSeconds(Blackout.fadeOutTime);
			}
		}
		yield return base.StartCoroutine(WorldManager.instance.LoadLevelsFromCR(targetLevelIndex, specificWorld));
		this.runner.transform.position = playerPos;
		this.runner.physicalDepthLayerIdx = Mathf.RoundToInt(playerPos.z);
		if (lookDirection != 0)
		{
			this.runner.direction = (float)((lookDirection > 0) ? 1 : (-1));
		}
		if (Application.isPlaying)
		{
			this.PlaceRunnerOnGroundAndStartTrackBuilding(placeContext, true, dontChangeState);
			if (onPlaceOnGround != null)
			{
				onPlaceOnGround();
			}
			this.gameCam.Refresh(true);
			if (!dontRefreshInteractables)
			{
				if (this.narrative.isBusy)
				{
					Debug.LogWarning("Could not refresh Narrative while teleporting since ink or narrative presentation was still busy. It'll refresh automatically when ink and narrative finishes though.");
				}
				else
				{
					this.narrative.RefreshInteractablesChoices(true, false);
				}
			}
			this.runner.enabled = true;
			if (fade)
			{
				Blackout.FadeIn(0f, null);
				yield return new WaitForSeconds(Blackout.fadeInTime);
			}
		}
		if (onComplete != null)
		{
			onComplete();
		}
		yield break;
	}

	// Token: 0x060000E8 RID: 232 RVA: 0x0000B0EC File Offset: 0x000092EC
	public void PlaceRunnerOnGroundAndStartTrackBuilding(string context, bool immediateCameraUpdate = true, bool dontChangeState = false)
	{
		Vector3 position = this.runner.transform.position;
		SlopeSample slopeSample = Raycast.FindBestNearbySlopeSample(Level.current, position, true, 3f);
		if (slopeSample.slope == null)
		{
			Debug.LogError(string.Format("PlaceRunnerOnGroundAndStartTrackBuilding ({0}) didn't find any slope at all any where near the Runner (at {1})", context, Runner.instance.transform.position));
			return;
		}
		this.runner.ResetToSlopeSample(slopeSample, dontChangeState);
		MusicRun componentInParent = this.runner.currentSlope.GetComponentInParent<MusicRun>();
		if (componentInParent != null)
		{
			MonoSingleton<TrackBuilder>.instance.UpdateBuilding();
			slopeSample = Raycast.FindBestNearbySlopeSample(Level.current, position, false, 3f);
			if (slopeSample.slope == null)
			{
				Debug.LogError(string.Concat(new string[] { "PlaceRunnerOnGroundAndStartTrackBuilding (", context, ") found a music run (", componentInParent.name, ") as the closest slope but failed to find a true slope for player to be placed on" }), componentInParent);
				return;
			}
			this.runner.currentSlope = slopeSample.slope;
			this.runner.position = slopeSample.point;
		}
		Vector3 vector = this.runner.transform.position - position;
		if (Mathf.Abs(vector.z) > 5f || vector.magnitude > 15f)
		{
			Debug.LogWarning(string.Format("PlaceRunnerOnGroundAndStartTrackBuilding ({0}) had to player quite far from original expected location. Distance from expected location: {1}", context, vector));
		}
		this.runner.RefreshImmediate();
		if (immediateCameraUpdate)
		{
			this.gameCam.Refresh(true);
		}
	}

	// Token: 0x060000E9 RID: 233 RVA: 0x0000B26B File Offset: 0x0000946B
	public void PlayerReachedEndOfGame()
	{
		base.state = this.endOfGameState;
	}

	// Token: 0x060000EA RID: 234 RVA: 0x0000B279 File Offset: 0x00009479
	public void SetTimeScalar(Game.TimeScalar timeScalar, float val)
	{
		this._timeScalars[timeScalar] = val;
	}

	// Token: 0x060000EB RID: 235 RVA: 0x0000B288 File Offset: 0x00009488
	public void RemoveTimeScalar(Game.TimeScalar timeScalar)
	{
		this._timeScalars.Remove(timeScalar);
	}

	// Token: 0x060000EC RID: 236 RVA: 0x0000B297 File Offset: 0x00009497
	public bool HasTimeScalar(Game.TimeScalar timeScalar)
	{
		return this._timeScalars.ContainsKey(timeScalar);
	}

	// Token: 0x1700003F RID: 63
	// (get) Token: 0x060000ED RID: 237 RVA: 0x0000B2A5 File Offset: 0x000094A5
	public float debugFastTimeScalar
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x060000EE RID: 238 RVA: 0x0000B2AC File Offset: 0x000094AC
	private void OnApplicationFocus(bool hasFocus)
	{
		this.RefreshChristmas();
	}

	// Token: 0x060000EF RID: 239 RVA: 0x0000B2B4 File Offset: 0x000094B4
	private void OnApplicationPause(bool pauseStatus)
	{
		this.RefreshChristmas();
	}

	// Token: 0x060000F0 RID: 240 RVA: 0x0000B2BC File Offset: 0x000094BC
	private void RefreshChristmas()
	{
		DateTime now = DateTime.Now;
		Game.isChristmas = now.Day >= 24 && now.Day <= 26 && now.Month == 12;
	}

	// Token: 0x060000F1 RID: 241 RVA: 0x0000B2F8 File Offset: 0x000094F8
	private void Update()
	{
		if (!Game.loaded)
		{
			return;
		}
		if (GameInput.debugFast)
		{
			float num = (float)((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) ? 100 : 10);
			this.SetTimeScalar(Game.TimeScalar.DebugFast, num);
		}
		else
		{
			this.RemoveTimeScalar(Game.TimeScalar.DebugFast);
		}
		float num2 = 1f;
		for (Game.TimeScalar timeScalar = Game.TimeScalar.FinalJump; timeScalar < Game.TimeScalar.TOTAL; timeScalar++)
		{
			float num3;
			if (this._timeScalars.TryGetValue(timeScalar, out num3))
			{
				num2 = num3;
			}
		}
		Time.timeScale = num2;
		if (MonoSingleton<JournalController>.instance.visible)
		{
			return;
		}
		this.narrative.PreventBackgroundRemarks(false, Narrative.PreventBackgroundRemarksReason.Loading);
		this.narrative.PreventBackgroundRemarks(this.runner.isMusicRunning, Narrative.PreventBackgroundRemarksReason.MusicRunning);
		this.narrative.PreventBackgroundRemarks(base.state == this.peakState, Narrative.PreventBackgroundRemarksReason.InPeakState);
		this.UpdateLook();
		PropsController.instance.Refresh();
		base.UpdateStateMachine(Time.deltaTime);
		CaveRegion.UpdateAll();
		ParticlesX.UpdateParticlesAwaitingReturnToPool();
		if (!MonoSingleton<BuildSetupManager>.instance.setup.trailerFeatures || !(MonoSingleton<TestMenu>.instance != null) || !MonoSingleton<TestMenu>.instance.cameraSwoopOrMoveActive || !MonoSingleton<TestMenu>.instance.allHighLOD)
		{
			if ((MonoSingleton<PeakStateController>.instance.active || Game.instance.inActiveGameplay) && !GameCamera.instance.freeCameraState.active)
			{
				if (MonoSingleton<PeakStateController>.instance.zoomedPastCurrentLevel || Level.current.fadeAlpha < 1f)
				{
					Level.current.SetFlattenedLODLevel(LODLevel.Medium);
				}
				else
				{
					Vector3 vector = GameCamera.instance.camera.WorldToViewportPoint(Runner.instance.physicalPosition3d);
					Vector3 vector2 = GameCamera.instance.camera.WorldToViewportPoint(Runner.instance.physicalPosition3d + 10f * Vector3.right);
					float num4 = Vector2.Distance(vector, vector2);
					float num5 = this.flattenedLODViewportThreshold;
					if (num4 < this.flattenedLODViewportThreshold)
					{
						Level.current.SetFlattenedLODLevel(LODLevel.Medium);
					}
					else if (DebugOptions.opts.allowDynamicMediumLODs)
					{
						Level.current.SetDynamicMediumAndHighLODLevel();
					}
					else
					{
						Level.current.SetFlattenedLODLevel(LODLevel.High);
					}
				}
			}
			else if (Game.instance.inTitleScreenAndIntroState || GameCamera.instance.freeCameraState.active)
			{
				int num6 = Level.DepthToIndex(GameCamera.instance.transform.position.z);
				Level.SetupLODs(WorldManager.instance.currentWorld, num6);
			}
		}
		if (this.showingIntro && this._canCancelIntro && Input.GetKeyDown(KeyCode.Escape) && GameInput.HasControl(null))
		{
			this.SetTimeScalar(Game.TimeScalar.IntroPause, 0f);
			Narrative.instance.SetPaused(Narrative.PauseReason.Intro, true);
			MonoSingleton<Dialogue>.instance.Show("Return to title screen?", "Are you sure you want to exit the game?", "Okay", "Cancel", false, delegate(Dialogue.Result result)
			{
				this.RemoveTimeScalar(Game.TimeScalar.IntroPause);
				if (result == Dialogue.Result.Primary)
				{
					this._cancelIntro = true;
					return;
				}
				Narrative.instance.SetPaused(Narrative.PauseReason.Intro, false);
			});
		}
		if (Level.currentIndex == 8)
		{
			WeatherSystem.instance.BeginGlobalInkOverride(WeatherType.Clear);
		}
		if (!Game.gameplayPaused)
		{
			float deltaTime = Time.deltaTime;
			foreach (Creature creature in Level.current.creatures)
			{
				creature.ManualUpdate(deltaTime);
			}
		}
		if (!Game.gameplayPaused && this._activeBoat != null)
		{
			this._activeBoat.ManualUpdate();
			if (this._activeBoat.arrived && !Runner.instance.isBoating)
			{
				this._activeBoat = null;
			}
		}
	}

	// Token: 0x060000F2 RID: 242 RVA: 0x0000B66C File Offset: 0x0000986C
	private void LateUpdate()
	{
		if (!Game.loaded)
		{
			return;
		}
		if (this.gameCam != null && this.gameCam.enabled)
		{
			this.gameCam.Refresh(false);
		}
		foreach (Poly poly in Level.current.cutawayPolys)
		{
			poly.UpdateCutawayAlpha();
		}
		float z = GameCamera.instance.transform.position.z;
		foreach (MusicRun musicRun in Level.current.musicRuns)
		{
			Range range = new Range(float.MinValue, float.MinValue);
			if (musicRun.chunks.Count > 0)
			{
				range.min = musicRun.westChunk.boundsRange.min;
				range.max = musicRun.eastChunk.boundsRange.max;
			}
			foreach (Splat splat in musicRun.staticFadingSplats)
			{
				splat.RefreshMusicRunSplatAlpha(z, true, range);
			}
		}
		foreach (MusicRun musicRun2 in Level.current.musicRuns)
		{
			foreach (Chunk chunk in musicRun2.chunks)
			{
				foreach (Splat splat2 in chunk.splats)
				{
					splat2.RefreshMusicRunSplatAlpha(z, false, default(Range));
				}
			}
		}
		foreach (Creature creature in Level.current.creatures)
		{
			creature.UpdateHat();
		}
		if (Game.onUIPositionUpdate != null)
		{
			Game.onUIPositionUpdate(MonoSingleton<GameUI>.instance);
		}
	}

	// Token: 0x060000F3 RID: 243 RVA: 0x0000B8F8 File Offset: 0x00009AF8
	private void UpdateLook()
	{
		if (!this.runner.enabled || (this.runner.playerControlDisabled != PlayerControlDisableReason.None && this.runner.playerControlDisabled != PlayerControlDisableReason.LookFurther) || !this.runner.gameObject.activeInHierarchy || (!this.runner.running && !this.runner.climbing && !this.runner.cliffEdgeWobbling && !this.runner.sitting && !this.runner.jumping) || this.runner.isMusicRunning || this.inPeakState || this.inTitleScreenAndIntroState || this.gameCam.followPathState.strength != 0f || this.gameCam.caughtCameraState.strength != 0f || this.gameCam.shelterState.strength != 0f || this.gameCam.animatedCameraState.strength != 0f || MonoSingleton<RestStateController>.instance.active || (PropsController.instance.triggerFlags & TriggerFlags.PreventZoomOut) != TriggerFlags.None)
		{
			this.maxPlayerZoomOut = MaxZoom.None;
		}
		else if (this.runner.isOnRidge)
		{
			this.maxPlayerZoomOut = MaxZoom.OnRidge;
		}
		else
		{
			this.maxPlayerZoomOut = MaxZoom.Limited;
		}
		bool flag = false;
		if (this.maxPlayerZoomOut == MaxZoom.None || (!GameInput.zoomOutHeld && !GameInput.zoomInHeld && !this._lookFurtherActive))
		{
			this._lookModeActive = false;
			this.gameCam.playerZoomState.activeZoomDir = 0;
			flag = true;
		}
		else
		{
			this.gameCam.playerZoomState.maxZoom = this.maxPlayerZoomOut;
			this.gameCam.playerZoomState.activeZoomDir = ((GameInput.zoomOutHeld || this._lookFurtherActive) ? (-1) : 1);
			this._lookModeActive = true;
		}
		if (this._lookModeActive && !this._lookFurtherActive && GameInput.zoomFurtherPressed && this.gameCam.playerZoomState.activeZoomDir == -1 && this.canLookFurther)
		{
			this._lookFurtherActive = true;
			MonoSingleton<PeakWidgetController>.instance.ShowWidgets();
			this.gameCam.playerZoomState.lookFurther = true;
			Runner.instance.playerControlDisabled |= PlayerControlDisableReason.LookFurther;
			GameInput.PushControlStack(this._lookFurtherControlItem);
		}
		else if (this._lookFurtherActive && (GameInput.zoomFurtherPressed || GameInput.Back(this._lookFurtherControlItem) || GameInput.zoomInPressed || !this.canLookFurther))
		{
			flag = true;
		}
		if (this._lookFurtherActive && flag)
		{
			this._lookFurtherActive = false;
			MonoSingleton<PeakWidgetController>.instance.HideWidgets();
			this.gameCam.playerZoomState.lookFurther = false;
			Runner.instance.playerControlDisabled &= ~PlayerControlDisableReason.LookFurther;
			GameInput.PopControlStack(this._lookFurtherControlItem, true);
		}
	}

	// Token: 0x060000F4 RID: 244 RVA: 0x0000BBC4 File Offset: 0x00009DC4
	public void StartBoat()
	{
		Prop lastInteractedProp = PropsController.instance.lastInteractedProp;
		if (lastInteractedProp == null)
		{
			Debug.LogError("Expected PropsController.instance.lastInteractedProp to be valid");
		}
		this._activeBoat = lastInteractedProp.GetComponentInParent<Boat>();
		if (this._activeBoat == null)
		{
			Debug.LogError("Expected PropsController.instance.lastInteractedProp to have a Boat in its parent hierarchy");
		}
		this._activeBoat.Begin();
		Runner.instance.StartBoating();
	}

	// Token: 0x060000F5 RID: 245 RVA: 0x0000BC28 File Offset: 0x00009E28
	private IEnumerator Intro_Coroutine(bool freshNewGame)
	{
		this.showingIntro = true;
		this._canCancelIntro = false;
		this.startedFreshNewGame = freshNewGame;
		if (freshNewGame)
		{
			Narrative.instance.RunKnot("TRIGGER_new_game_pre_intro_setup", null, false, false, Array.Empty<object>());
			Narrative.instance.ForceComplete();
		}
		MonoSingleton<BlackBars>.instance.SetVisible(true, BlackBarsReason.Intro);
		GameCamera.instance.introCameraState.BeginIntro(!freshNewGame || MonoSingleton<BuildSetupManager>.instance.setup.fastIntro);
		if (freshNewGame)
		{
			GameSetup newGameSetup = MonoSingleton<BuildSetupManager>.instance.setup.newGameSetup;
			float waitUntilDaysNorm = newGameSetup.clockTimeHour / 24f;
			if (MonoSingleton<BuildSetupManager>.instance.setup.fastIntro)
			{
				GameClock.instance.dayIdx = 0;
				if (waitUntilDaysNorm < GameClock.instance.daysNorm)
				{
					GameClock.instance.dayIdx = -1;
				}
			}
			else
			{
				GameClock.instance.dayIdx = -1;
			}
			yield return new WaitForSecondsRealtime(2f);
			this._canCancelIntro = true;
			Narrative.instance.RunKnot("intro_vo", null, false, false, Array.Empty<object>());
			base.StartCoroutine(GameClock.instance.WaitUntilDaysNorm(waitUntilDaysNorm));
			while (GameClock.instance.isWaitingForTimeToPass)
			{
				if (this.PlayerWantsToCancelIntroAndExit())
				{
					yield break;
				}
				yield return null;
			}
		}
		else
		{
			yield return new WaitForSecondsRealtime(1f);
			AudioController.instance.PlaySting(Sting.LongDistance, -1);
		}
		while (GameCamera.instance.introCameraState.active)
		{
			if (this.PlayerWantsToCancelIntroAndExit())
			{
				yield break;
			}
			yield return null;
		}
		while (Narrative.instance.isBusy)
		{
			if (this.PlayerWantsToCancelIntroAndExit())
			{
				yield break;
			}
			yield return null;
		}
		MonoSingleton<BlackBars>.instance.SetVisible(false, BlackBarsReason.Intro);
		this.showingIntro = false;
		this._canCancelIntro = false;
		this._cancelIntro = false;
		this.BeginGameplay();
		yield break;
	}

	// Token: 0x060000F6 RID: 246 RVA: 0x0000BC3E File Offset: 0x00009E3E
	private bool PlayerWantsToCancelIntroAndExit()
	{
		if (this._canCancelIntro && this._cancelIntro)
		{
			this._canCancelIntro = false;
			this._cancelIntro = false;
			Narrative.instance.Clear(Narrative.ClearType.Visual);
			Blackout.FadeOut(delegate
			{
				this.showingIntro = false;
				GameCamera.instance.introCameraState.Reset();
				MonoSingleton<Launcher>.instance.ReturnToTitleScreen();
			});
			return true;
		}
		return false;
	}

	// Token: 0x060000F7 RID: 247 RVA: 0x0000BC80 File Offset: 0x00009E80
	private void State_TitleScreenAndIntro(bool start, bool end)
	{
		if (start)
		{
			this.runner.playerControlDisabled |= PlayerControlDisableReason.TitleScreenAndIntro;
			Game.stateWantsGameplayPaused = true;
			return;
		}
		if (end)
		{
			this.runner.playerControlDisabled &= ~PlayerControlDisableReason.TitleScreenAndIntro;
			Game.stateWantsGameplayPaused = false;
			return;
		}
	}

	// Token: 0x060000F8 RID: 248 RVA: 0x0000BCCF File Offset: 0x00009ECF
	private void State_ActiveGameplay(bool start, bool end)
	{
		if (start)
		{
			this.runner.ResetAutoRunTarget();
			this.runner.playerControlDisabled &= ~PlayerControlDisableReason.AutoRunToProp;
			return;
		}
	}

	// Token: 0x060000F9 RID: 249 RVA: 0x0000BCF6 File Offset: 0x00009EF6
	private void State_PeakState(bool start, bool end)
	{
		if (start)
		{
			this.peakStateController.Enter();
			return;
		}
		if (end)
		{
			this.peakStateController.Exit();
			return;
		}
		this.peakStateController.UpdateState(base.stateTimer);
	}

	// Token: 0x060000FA RID: 250 RVA: 0x0000BD28 File Offset: 0x00009F28
	private void State_FollowPath(bool start, bool end)
	{
		if (start)
		{
			Prop lastInteractedPath = this.propsController.lastInteractedPath;
			GameObject gameObject = lastInteractedPath.pathDestination.gameObject;
			if (lastInteractedPath == null)
			{
				Debug.LogError("Could not follow path because propsController.activePathInteractable was null", this.propsController.lastInteractedProp);
				base.state = this.activeGameplayState;
				return;
			}
			if (gameObject == null)
			{
				Debug.LogError("Could not follow path because path's destination object was not valid or couldn't be found. Could it be in an unloaded scene?", this.propsController.lastInteractedProp);
				base.state = this.activeGameplayState;
				return;
			}
			this._followPathCoroutine = this.FollowPathState_Coroutine(lastInteractedPath, gameObject);
			base.StartCoroutine(this._followPathCoroutine);
			return;
		}
		else
		{
			if (end)
			{
				return;
			}
			if (this._followPathCoroutine == null)
			{
				base.state = this.activeGameplayState;
			}
			return;
		}
	}

	// Token: 0x060000FB RID: 251 RVA: 0x0000BDDD File Offset: 0x00009FDD
	private IEnumerator FollowPathState_Coroutine(Prop pathEntryProp, GameObject pathExitGO)
	{
		int levelsChange = pathEntryProp.pathLevelsChange;
		this._pathFollowIsBetweenLevels = levelsChange > 0;
		if (this._pathFollowIsBetweenLevels)
		{
			GameClock.instance.firstDayPauseEnabled = false;
		}
		Vector3 pathStartPoint = pathEntryProp.transform.position;
		Vector3 pathExitPoint = pathExitGO.transform.position;
		int targetLevelIdx = Level.GetForTransform(pathExitGO.transform).levelIdx;
		int currentPathExitDir = ((pathEntryProp.pathDestinationExitDir == Prop.PathExitDirection.Left) ? (-1) : 1);
		bool currentPathIsForwards = pathExitPoint.z > pathStartPoint.z;
		Prop componentInChildren = pathExitGO.GetComponentInChildren<Prop>();
		if (componentInChildren != null)
		{
			GameObject gameObject = componentInChildren.pathDestination.gameObject;
			if (gameObject != null && gameObject.GetComponentInChildren<Prop>() == pathEntryProp)
			{
				PropsController.instance.explicitlyHiddenProps.Add(componentInChildren);
			}
		}
		this.runner.EnterPath(pathExitPoint.z > pathStartPoint.z, currentPathExitDir, pathStartPoint, pathEntryProp.pathAnimType);
		float pathTravelTimeHours = pathEntryProp.pathTravelTimeHours;
		bool flag = targetLevelIdx >= 8;
		if (pathTravelTimeHours > 0f && !flag)
		{
			float num = GameClock.instance.daysNorm + pathTravelTimeHours / 24f;
			base.StartCoroutine(GameClock.instance.WaitUntilDaysNorm(num));
		}
		yield return new WaitForSeconds(1f);
		GameCamera.FollowPathCameraState.PathLength pathLength = GameCamera.FollowPathCameraState.PathLength.Standard;
		if (levelsChange == 0)
		{
			pathLength = GameCamera.FollowPathCameraState.PathLength.Local;
		}
		else if (levelsChange > 1)
		{
			pathLength = GameCamera.FollowPathCameraState.PathLength.Long;
		}
		this.gameCam.followPathState.Enter(pathStartPoint, pathExitPoint, pathLength);
		if (levelsChange > 0)
		{
			AudioController.instance.PlaySting(Sting.LongDistance, -1);
			foreach (Prop prop in Level.current.props.all)
			{
				if (!string.IsNullOrWhiteSpace(prop.inkListItemName) && !PropsController.instance.passedProps.Contains(prop.inkListItemName))
				{
					PropsController.instance.passedProps.Add(prop.inkListItemName);
				}
			}
		}
		float layerSwitchTime = 0.65f * this.gameCam.followPathState.pathChangeDuration;
		while (base.stateTimer < layerSwitchTime)
		{
			yield return null;
		}
		WorldManager.instance.LoadLevelsFrom(targetLevelIdx, null);
		this.runner.transform.position = pathExitPoint;
		this.runner.physicalDepthLayerIdx = Mathf.RoundToInt(this.runner.transform.position.z);
		this.runner.direction = (float)currentPathExitDir;
		HighlandCameraProperties @default = HighlandCameraProperties.@default;
		GameCamera.instance.runningState.Update(true, ref @default);
		while (Level.currentIndex != targetLevelIdx)
		{
			yield return null;
		}
		this.PlaceRunnerOnGroundAndStartTrackBuilding("Follow path exit", false, false);
		this.runner.SetHidden(true, HideReason.FollowPath);
		this.gameCam.followPathState.ReadyForBlendToExit();
		float num2 = 1f + this.gameCam.followPathState.pathChangeDuration;
		layerSwitchTime = num2 - 0.5f;
		while (base.stateTimer < layerSwitchTime)
		{
			yield return null;
		}
		this.runner.SetHidden(false, HideReason.FollowPath);
		this.runner.ExitPath(currentPathIsForwards, currentPathExitDir);
		while (this.gameCam.followPathState.active || GameClock.instance.isWaitingForTimeToPass)
		{
			yield return null;
		}
		this.runner.playerControlDisabled &= ~PlayerControlDisableReason.FollowPath;
		this._followPathCoroutine = null;
		yield break;
	}

	// Token: 0x060000FC RID: 252 RVA: 0x0000BDFA File Offset: 0x00009FFA
	private void State_EndOfGame(bool start, bool end)
	{
		if (start)
		{
			base.StartCoroutine(this.EndOfGameSequence_Coroutine());
			Game.stateWantsGameplayPaused = true;
			return;
		}
		if (end)
		{
			Game.stateWantsGameplayPaused = false;
			return;
		}
	}

	// Token: 0x060000FD RID: 253 RVA: 0x0000BE1D File Offset: 0x0000A01D
	private IEnumerator EndOfGameSequence_Coroutine()
	{
		Runner.instance.playerControlDisabled |= PlayerControlDisableReason.EndOfGameSequence;
		if (MonoSingleton<BuildSetupManager>.instance.setup.demoEndGameSequence)
		{
			Runner.instance.playerControlDisabled &= ~PlayerControlDisableReason.EndOfGameSequence;
			MonoSingleton<Launcher>.instance.DemoReturnToTitleScreenAndRestart();
			yield break;
		}
		MonoSingleton<Credits>.instance.Show(CreditsContext.GameEnd, null);
		while (MonoSingleton<Credits>.instance.visible)
		{
			if (MonoSingleton<Credits>.instance.nearlyBlackedOutOrHiding && !Blackout.isShownOrShowing)
			{
				Blackout.FadeOut(null);
			}
			yield return null;
		}
		if (!Blackout.isShownOrShowing)
		{
			Blackout.FadeOut(null);
		}
		while (Blackout.isAnimating)
		{
			yield return null;
		}
		Runner.instance.playerControlDisabled &= ~PlayerControlDisableReason.EndOfGameSequence;
		MonoSingleton<Launcher>.instance.RestartFromEndOfGameForNewGamePlus();
		yield break;
	}

	// Token: 0x17000040 RID: 64
	// (get) Token: 0x060000FE RID: 254 RVA: 0x0000BE25 File Offset: 0x0000A025
	public bool isPathFollowing
	{
		get
		{
			return base.state == this.followPathState;
		}
	}

	// Token: 0x17000041 RID: 65
	// (get) Token: 0x060000FF RID: 255 RVA: 0x0000BE38 File Offset: 0x0000A038
	public bool isPathFollowingBetweenLevels
	{
		get
		{
			return this.isPathFollowing && this._pathFollowIsBetweenLevels;
		}
	}

	// Token: 0x06000100 RID: 256 RVA: 0x0000BE4A File Offset: 0x0000A04A
	public void FollowPath()
	{
		base.state = this.followPathState;
	}

	// Token: 0x06000101 RID: 257 RVA: 0x0000BE58 File Offset: 0x0000A058
	public IEnumerator AwaitFollowPathComplete()
	{
		while (base.state == this.followPathState)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000102 RID: 258 RVA: 0x0000BE67 File Offset: 0x0000A067
	private void OnTrip(float magnitudeNorm)
	{
	}

	// Token: 0x06000103 RID: 259 RVA: 0x0000BE69 File Offset: 0x0000A069
	public void StartDeath(bool fast)
	{
		base.StartCoroutine(this.Death_Coroutine(fast));
	}

	// Token: 0x06000104 RID: 260 RVA: 0x0000BE79 File Offset: 0x0000A079
	private IEnumerator Death_Coroutine(bool fast)
	{
		this.deathSequenceActive = true;
		GameClock.instance.CancelTimePassing();
		if (MonoSingleton<RestStateController>.instance.active)
		{
			MonoSingleton<RestStateController>.instance.Exit();
		}
		Narrative.instance.Clear(Narrative.ClearType.Death);
		bool hitGround = this.runner.currentSlope != null && this.runner.position.y > 0f;
		bool fastDeathReset = (fast || DebugOptions.opts.fastDeathReset) && (!this.runner.healthBeforeFallAtCritical || !hitGround);
		yield return new WaitForSeconds(fastDeathReset ? 0.01f : 2f);
		AudioController.instance.PlaySting(fastDeathReset ? Sting.MiniWorry : Sting.Danger, -1);
		yield return new WaitForSeconds(fastDeathReset ? 0.5f : 2f);
		if (!fastDeathReset)
		{
			MonoSingleton<BlackBars>.instance.SetVisible(true, BlackBarsReason.Death);
			yield return new WaitForSeconds(2f);
		}
		if (fastDeathReset && hitGround)
		{
			yield return new WaitForSeconds(2f);
		}
		float blackoutStandardFadeOutTime = Blackout.fadeOutTime;
		float blackoutStandardFadeInTime = Blackout.fadeInTime;
		if (fastDeathReset)
		{
			Blackout.fadeOutTime = 0.5f;
			Blackout.fadeInTime = 0.5f;
		}
		Blackout.FadeOut(null);
		while (!Blackout.isFullyVisible)
		{
			yield return null;
		}
		this.runner.ResetToLastSafePosition(!fastDeathReset, fastDeathReset);
		if (fastDeathReset)
		{
			Blackout.FadeIn(0f, null);
			while (!Blackout.isFullyHidden)
			{
				yield return null;
			}
			Narrative.instance.Death(true);
			if (hitGround)
			{
				this.runner.health.ApplyDamage(DamageType.FastResurrection, Damage.MinorDamage);
				this.runner.SetSafeResetPoint();
			}
			Blackout.fadeOutTime = blackoutStandardFadeOutTime;
			Blackout.fadeInTime = blackoutStandardFadeInTime;
		}
		else
		{
			Narrative.instance.Death(false);
			while (AudioController.instance.IsPlayingSting(Sting.Danger))
			{
				yield return null;
			}
			AudioController.instance.PlaySting(Sting.Night, -1);
			while (Narrative.instance.isBusy && !Narrative.instance.canRunOff)
			{
				yield return null;
			}
			if (Blackout.showing)
			{
				Blackout.FadeIn(0f, null);
			}
			if (Runner.instance.dead)
			{
				Runner.instance.Resurrect();
			}
			MonoSingleton<BlackBars>.instance.SetVisible(false, BlackBarsReason.Death);
		}
		this.deathSequenceActive = false;
		yield break;
	}

	// Token: 0x06000105 RID: 261 RVA: 0x0000BE8F File Offset: 0x0000A08F
	private void OnPeakStateRequestsExit()
	{
		base.state = this.activeGameplayState;
	}

	// Token: 0x17000042 RID: 66
	// (get) Token: 0x06000106 RID: 262 RVA: 0x0000BE9D File Offset: 0x0000A09D
	private StateFuncMachine.StateFunc peakState
	{
		get
		{
			if (this._peakState == null)
			{
				this._peakState = new StateFuncMachine.StateFunc(this.State_PeakState);
			}
			return this._peakState;
		}
	}

	// Token: 0x17000043 RID: 67
	// (get) Token: 0x06000107 RID: 263 RVA: 0x0000BEBF File Offset: 0x0000A0BF
	private StateFuncMachine.StateFunc activeGameplayState
	{
		get
		{
			if (this._activeGameplayState == null)
			{
				this._activeGameplayState = new StateFuncMachine.StateFunc(this.State_ActiveGameplay);
			}
			return this._activeGameplayState;
		}
	}

	// Token: 0x17000044 RID: 68
	// (get) Token: 0x06000108 RID: 264 RVA: 0x0000BEE1 File Offset: 0x0000A0E1
	private StateFuncMachine.StateFunc followPathState
	{
		get
		{
			if (this._followPathState == null)
			{
				this._followPathState = new StateFuncMachine.StateFunc(this.State_FollowPath);
			}
			return this._followPathState;
		}
	}

	// Token: 0x17000045 RID: 69
	// (get) Token: 0x06000109 RID: 265 RVA: 0x0000BF03 File Offset: 0x0000A103
	private StateFuncMachine.StateFunc titleScreenAndIntroState
	{
		get
		{
			if (this._titleScreenAndIntroState == null)
			{
				this._titleScreenAndIntroState = new StateFuncMachine.StateFunc(this.State_TitleScreenAndIntro);
			}
			return this._titleScreenAndIntroState;
		}
	}

	// Token: 0x17000046 RID: 70
	// (get) Token: 0x0600010A RID: 266 RVA: 0x0000BF25 File Offset: 0x0000A125
	private StateFuncMachine.StateFunc endOfGameState
	{
		get
		{
			if (this._endOfGameState == null)
			{
				this._endOfGameState = new StateFuncMachine.StateFunc(this.State_EndOfGame);
			}
			return this._endOfGameState;
		}
	}

	// Token: 0x040001A3 RID: 419
	public static bool loaded;

	// Token: 0x040001A4 RID: 420
	public static bool stateWantsGameplayPaused;

	// Token: 0x040001A5 RID: 421
	public static bool isChristmas;

	// Token: 0x040001A9 RID: 425
	public int playthroughIdx;

	// Token: 0x040001AA RID: 426
	[Disable]
	public MaxZoom maxPlayerZoomOut;

	// Token: 0x040001AB RID: 427
	public float flattenedLODViewportThreshold = 0.02f;

	// Token: 0x040001AC RID: 428
	public float flattenedLODViewportThresholdSwitch = 0.02f;

	// Token: 0x040001AD RID: 429
	private PeakStateController _peakStateController;

	// Token: 0x040001AE RID: 430
	private bool subscribedToStaticEvents;

	// Token: 0x040001AF RID: 431
	private Dictionary<Game.TimeScalar, float> _timeScalars = new Dictionary<Game.TimeScalar, float>();

	// Token: 0x040001B0 RID: 432
	private bool _pathFollowIsBetweenLevels;

	// Token: 0x040001B1 RID: 433
	private IEnumerator _followPathCoroutine;

	// Token: 0x040001B2 RID: 434
	private bool _lookModeActive;

	// Token: 0x040001B3 RID: 435
	private bool _lookFurtherActive;

	// Token: 0x040001B4 RID: 436
	private object _lookFurtherControlItem = new object();

	// Token: 0x040001B5 RID: 437
	private bool _cancelIntro;

	// Token: 0x040001B6 RID: 438
	private bool _canCancelIntro;

	// Token: 0x040001B7 RID: 439
	private StateFuncMachine.StateFunc _peakState;

	// Token: 0x040001B8 RID: 440
	private StateFuncMachine.StateFunc _activeGameplayState;

	// Token: 0x040001B9 RID: 441
	private StateFuncMachine.StateFunc _followPathState;

	// Token: 0x040001BA RID: 442
	private StateFuncMachine.StateFunc _titleScreenAndIntroState;

	// Token: 0x040001BB RID: 443
	private StateFuncMachine.StateFunc _endOfGameState;

	// Token: 0x040001BC RID: 444
	private Boat _activeBoat;

	// Token: 0x0200025F RID: 607
	public enum ClearType
	{
		// Token: 0x04001440 RID: 5184
		VisualsOnly,
		// Token: 0x04001441 RID: 5185
		FullState,
		// Token: 0x04001442 RID: 5186
		FullStatePreservingLoopVariables
	}

	// Token: 0x02000260 RID: 608
	public enum TimeScalar
	{
		// Token: 0x04001444 RID: 5188
		FinalJump,
		// Token: 0x04001445 RID: 5189
		DebugFast,
		// Token: 0x04001446 RID: 5190
		ReadyToSprint,
		// Token: 0x04001447 RID: 5191
		EagleCatch,
		// Token: 0x04001448 RID: 5192
		MusicRunCatchup,
		// Token: 0x04001449 RID: 5193
		Journal,
		// Token: 0x0400144A RID: 5194
		Feedback,
		// Token: 0x0400144B RID: 5195
		Tutorial,
		// Token: 0x0400144C RID: 5196
		IntroPause,
		// Token: 0x0400144D RID: 5197
		TestMenuSequence,
		// Token: 0x0400144E RID: 5198
		TestMenuGameSpeed,
		// Token: 0x0400144F RID: 5199
		PhotoMode,
		// Token: 0x04001450 RID: 5200
		TOTAL
	}
}
