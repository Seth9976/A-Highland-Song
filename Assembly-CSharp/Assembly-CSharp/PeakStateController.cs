using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000035 RID: 53
public class PeakStateController : MonoSingleton<PeakStateController>
{
	// Token: 0x1700008E RID: 142
	// (get) Token: 0x0600019B RID: 411 RVA: 0x0000F0AD File Offset: 0x0000D2AD
	public bool allowInput
	{
		get
		{
			return this.active && Game.instance.stateTimer > 2f && !MonoSingleton<JournalController>.instance.visible && !PhotoMode.visible;
		}
	}

	// Token: 0x1700008F RID: 143
	// (get) Token: 0x0600019C RID: 412 RVA: 0x0000F0E0 File Offset: 0x0000D2E0
	public bool allowExit
	{
		get
		{
			return this.allowInput && this._anyInitialNarrativeComplete && !MonoSingleton<MapsViewController>.instance.isBusy && GameInput.HasControl(MonoSingleton<PeakStateController>.instance) && (MonoSingleton<Tutorial>.instance.HasDone(TutorialId.PlaceCorrectMarker) || Level.currentIndex >= 1 || Game.instance.newGamePlus);
		}
	}

	// Token: 0x17000090 RID: 144
	// (get) Token: 0x0600019D RID: 413 RVA: 0x0000F138 File Offset: 0x0000D338
	public float targetFieldOfView
	{
		get
		{
			if (this.currentPeakProp != null && this.currentPeakProp.inkListItemName == "HILL_BEHIND_HOUSE_MINOR_PEAK")
			{
				return this._settings.camera.fieldOfViewFirstMiniPeak;
			}
			return this._settings.camera.fieldOfView;
		}
	}

	// Token: 0x17000091 RID: 145
	// (get) Token: 0x0600019E RID: 414 RVA: 0x0000F18B File Offset: 0x0000D38B
	public float zoomTransitionSmoothed
	{
		get
		{
			return Mathf.SmoothStep(0f, 1f, this._zoomTransition);
		}
	}

	// Token: 0x17000092 RID: 146
	// (get) Token: 0x0600019F RID: 415 RVA: 0x0000F1A2 File Offset: 0x0000D3A2
	public bool zoomedPastCurrentLevel
	{
		get
		{
			return this.active && this._zoomTransition > this.settings.camera.zoomNormToHideCurrentLevel;
		}
	}

	// Token: 0x17000093 RID: 147
	// (get) Token: 0x060001A0 RID: 416 RVA: 0x0000F1C8 File Offset: 0x0000D3C8
	public float peakCloudsAlpha
	{
		get
		{
			if (!this.active)
			{
				return 1f;
			}
			float num = Mathf.InverseLerp(this.settings.cloudsFadeStartZoom, 1f, this._zoomTransition);
			return Mathf.Lerp(1f, this.settings.minPeakCloudsAlpha, num);
		}
	}

	// Token: 0x17000094 RID: 148
	// (get) Token: 0x060001A1 RID: 417 RVA: 0x0000F215 File Offset: 0x0000D415
	// (set) Token: 0x060001A2 RID: 418 RVA: 0x0000F21D File Offset: 0x0000D41D
	public bool active { get; private set; }

	// Token: 0x17000095 RID: 149
	// (get) Token: 0x060001A3 RID: 419 RVA: 0x0000F226 File Offset: 0x0000D426
	// (set) Token: 0x060001A4 RID: 420 RVA: 0x0000F22E File Offset: 0x0000D42E
	public float transition { get; private set; }

	// Token: 0x17000096 RID: 150
	// (get) Token: 0x060001A5 RID: 421 RVA: 0x0000F238 File Offset: 0x0000D438
	public bool wantsDirectionalArrows
	{
		get
		{
			return this.allowInput && this._stateTimer > 4f && !MonoSingleton<MapsViewController>.instance.isBusy && (!Narrative.instance.isBusy || MonoSingleton<MapsViewController>.instance.mapIsSelected) && !this._hasPannedAtThisPeak;
		}
	}

	// Token: 0x060001A6 RID: 422 RVA: 0x0000F28C File Offset: 0x0000D48C
	public void Enter()
	{
		this.currentPeakProp = PropsController.instance.nearbyPeakProp;
		if (this.currentPeakProp == null)
		{
			this.currentPeakProp = Prop.NearestMajorOrMinorPeak(Runner.instance.physicalPosition3d);
		}
		this._visitingForFirstTime = !PropsController.instance.visitedPeaks.Contains(this.currentPeakProp.inkListItemName);
		PropsController.instance.visitedPeaks.Add(this.currentPeakProp.inkListItemName);
		int num = 0;
		float num2 = this._settings.maxAssumedSpottableDistance;
		if (this.currentPeakProp.isMinorPeak)
		{
			num2 = this._settings.maxAssumedSpottableDistanceMinor;
		}
		foreach (Map map in MonoSingleton<Inventory>.instance.incompleteForwardFacingMaps)
		{
			List<Prop> list = Level.current.PropsWithName(map.targetInkPropName);
			if (list.Count != 0)
			{
				using (List<Prop>.Enumerator enumerator2 = list.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (Mathf.Abs(enumerator2.Current.transform.position.x - Runner.instance.position.x) < num2)
						{
							num++;
						}
					}
				}
			}
		}
		Narrative.instance.SetNumberOfSpottableMapsOnCurrentPeak(num);
		this._zoomTransition = 0f;
		this._anyInitialNarrativeComplete = false;
		this._mapsAppearTimePassed = false;
		this._holeUI.scale = this._settings.vignette.inactiveScale;
		this._holeUI.groupAlpha = 0f;
		Runner.instance.playerControlDisabled |= PlayerControlDisableReason.Peak;
		this._stateTimer = 0f;
		this.active = true;
		GameInput.PushControlStack(this);
	}

	// Token: 0x060001A7 RID: 423 RVA: 0x0000F470 File Offset: 0x0000D670
	public void Exit()
	{
		this.active = false;
		MonoSingleton<BlackBars>.instance.SetVisible(false, BlackBarsReason.Peak);
		MonoSingleton<MapsViewController>.instance.Hide(MapsViewController.ShowReason.PeakView);
		MonoSingleton<PeakWidgetController>.instance.HideWidgets();
		this._holeUI.Animate(0.3f, delegate
		{
			this._holeUI.scale = this._settings.vignette.inactiveScale;
			this._holeUI.groupAlpha = 0f;
		});
		Prop nearbyPeakProp = PropsController.instance.nearbyPeakProp;
		string text = ((nearbyPeakProp != null) ? nearbyPeakProp.inkListItemName : null);
		PropsController.instance.nearbyPeakProp = null;
		this.currentPeakProp = null;
		GameCamera.instance.peakState.Exit();
		Runner.instance.playerControlDisabled &= ~PlayerControlDisableReason.Peak;
		GameInput.PopControlStack(this, true);
		if (Narrative.instance.PeakKnown(text))
		{
			Narrative.instance.NotifyPeakIfNecessary(text);
		}
		if (this._visitingForFirstTime && !string.IsNullOrWhiteSpace(text))
		{
			Narrative.instance.LeavePeak(text);
		}
	}

	// Token: 0x060001A8 RID: 424 RVA: 0x0000F548 File Offset: 0x0000D748
	public void UpdateState(float stateTimer)
	{
		MonoSingleton<BlackBars>.instance.SetVisible(true, BlackBarsReason.Peak);
		GameCamera instance = GameCamera.instance;
		Vector2 vector = default(Vector2);
		float num = 0f;
		if (stateTimer > 1f && !instance.peakState.active)
		{
			instance.peakState.Enter();
			instance.peakState.isMinorPeak = this.currentPeakProp.isMinorPeak;
			instance.peakState.isTutorialPeak = this.currentPeakProp.inkListItemName == "HILL_BEHIND_HOUSE_MINOR_PEAK";
			instance.peakState.peakViewExtentOverride = this.currentPeakProp.GetComponent<PeakViewExtentOverride>();
		}
		if (this.allowInput)
		{
			vector += GameInput.pan;
			if (vector.magnitude > 0.05f)
			{
				this._hasPannedAtThisPeak = true;
			}
			if (this.allowExit && GameInput.Back(this))
			{
				GameInput.ClearInputState();
				if (PeakStateController.onRequestExit != null)
				{
					PeakStateController.onRequestExit();
				}
				return;
			}
			if (GameInput.zoomOutHeld)
			{
				num -= 1f;
			}
			if (GameInput.zoomInHeld)
			{
				num += 1f;
			}
			if (GameInput.resetCameraToPlayer)
			{
				GameCamera.instance.peakState.ResetViewToPlayer();
				this._resettingZoom = true;
			}
		}
		float num2 = 1f;
		if (this._resettingZoom)
		{
			if (this._zoomTransition == 0f)
			{
				this._resettingZoom = false;
			}
			else
			{
				num = -1f;
				num2 = 3f;
			}
		}
		Ray ray = Raycast.ViewportPointToRay(new Vector2(0.5f, 0.5f));
		float num3 = Level.IndexToDepth(Level.currentIndex + 1);
		this._currentMaxZoom = ((Raycast.RayIntersectionWithZPlane(ray, num3).y > 0f) ? 1f : this.settings.camera.zoomNormToHideCurrentLevel);
		if (this._zoomTransition > this._currentMaxZoom && num > 0f)
		{
			num = 0f;
		}
		float num4 = 1f;
		bool targettingBeyondCurrentLevel = MonoSingleton<MapsViewController>.instance.targettingBeyondCurrentLevel;
		if (this._zoomTransition < this.settings.camera.zoomNormToHideCurrentLevel && num > 0f && !targettingBeyondCurrentLevel)
		{
			float num5 = Mathf.InverseLerp(this.settings.camera.zoomNormToHideCurrentLevel - 0.3f, this.settings.camera.zoomNormToHideCurrentLevel, this._zoomTransition);
			num4 = Mathf.Lerp(1f, this.settings.camera.cameraMaxSlowdownAtSwitchPoint, num5);
		}
		this._zoomTransition = Mathf.Clamp01(this._zoomTransition + this.settings.camera.zoomSpeed * num * num4 * num2 * Time.deltaTime);
		if (this._zoomTransition > this._currentMaxZoom)
		{
			this._zoomTransition = Mathf.MoveTowards(this._zoomTransition, this._currentMaxZoom, 2f * Time.deltaTime);
		}
		this.UpdateHoleUI(stateTimer);
		GameCamera.instance.peakState.playerPanInput = vector;
		GameCamera.instance.peakState.playerZoomInput = this.zoomTransitionSmoothed;
	}

	// Token: 0x060001A9 RID: 425 RVA: 0x0000F828 File Offset: 0x0000DA28
	private void Update()
	{
		if (!Game.loaded)
		{
			return;
		}
		if (this.active)
		{
			this._stateTimer += Time.deltaTime;
		}
		else
		{
			this._hasPannedAtThisPeak = false;
		}
		this.transition = Mathf.MoveTowards(this.transition, (float)((this.active && this._stateTimer > 1f) ? 1 : 0), Time.deltaTime);
		PeakScalingGroup.UpdateAll(this.transition);
		TimeOfDayEffects.peakFogTransition = this.transition;
		int num = (DebugOptions.opts.peakViewStartsWithMinimisedMaps ? 1 : 3);
		if (this.active && !this._mapsAppearTimePassed && this._stateTimer > (float)num && !Narrative.instance.isBusy)
		{
			this._mapsAppearTimePassed = true;
			if (MonoSingleton<Inventory>.instance.incompleteForwardFacingMaps.Count > 0)
			{
				if (DebugOptions.opts.peakViewStartsWithMinimisedMaps)
				{
					MonoSingleton<MapsViewController>.instance.ShowMinimised(MapsViewController.ShowReason.PeakView);
				}
				else
				{
					MonoSingleton<MapsViewController>.instance.Show(MapsViewController.ShowReason.PeakView);
				}
			}
			MonoSingleton<PeakWidgetController>.instance.ShowWidgets();
		}
		if (this.active && this._stateTimer > 2f && !Narrative.instance.isBusy)
		{
			this._anyInitialNarrativeComplete = true;
		}
		int num2 = (MonoSingleton<PeakStateController>.instance.wantsDirectionalArrows ? 1 : 0);
		this._zoomPrompt.groupAlpha = Mathf.MoveTowards(this._zoomPrompt.groupAlpha, (float)num2, Time.unscaledDeltaTime);
		if (MonoSingleton<MapsViewController>.isInstantiated)
		{
			this._zoomPrompt.centerX = MonoSingleton<MapsViewController>.instance.reticulePosNorm.x * this._zoomPrompt.parentRect.width;
		}
		int num3 = ((this.active && this.zoomedPastCurrentLevel) ? 0 : 1);
		float num4 = Mathf.MoveTowards(Level.current.fadeAlpha, (float)num3, Time.unscaledDeltaTime / this.levelFadeTime);
		Level.current.SetLevelFadeAlpha(num4);
	}

	// Token: 0x060001AA RID: 426 RVA: 0x0000F9F4 File Offset: 0x0000DBF4
	private void UpdateHoleUI(float stateTimer)
	{
		this._holeUI.scale = Mathf.Lerp(this._settings.vignette.inactiveScale, 1f, this.zoomTransitionSmoothed);
		this._holeUI.groupAlpha = this._zoomTransition;
		float num = Mathf.Lerp(0.5f, 0.25f, MonoSingleton<MapsViewController>.instance.cameraViewportOffsetNorm);
		this._holeUI.centerX = num * this._holeUI.parentRect.width;
	}

	// Token: 0x17000097 RID: 151
	// (get) Token: 0x060001AB RID: 427 RVA: 0x0000FA77 File Offset: 0x0000DC77
	public PeakStateSettings settings
	{
		get
		{
			return this._settings;
		}
	}

	// Token: 0x0400025F RID: 607
	private const string tutorialPeakInkName = "HILL_BEHIND_HOUSE_MINOR_PEAK";

	// Token: 0x04000260 RID: 608
	public static Action onRequestExit;

	// Token: 0x04000263 RID: 611
	public float levelFadeTime = 0.5f;

	// Token: 0x04000264 RID: 612
	[Disable]
	public Prop currentPeakProp;

	// Token: 0x04000265 RID: 613
	private float _zoomTransition;

	// Token: 0x04000266 RID: 614
	private float _targetZoomTransition;

	// Token: 0x04000267 RID: 615
	private float _currentMaxZoom;

	// Token: 0x04000268 RID: 616
	private bool _resettingZoom;

	// Token: 0x04000269 RID: 617
	private bool _hasPannedAtThisPeak;

	// Token: 0x0400026A RID: 618
	private bool _mapsAppearTimePassed;

	// Token: 0x0400026B RID: 619
	private float _dirArrowsAlpha;

	// Token: 0x0400026C RID: 620
	public object _telescopeBackStackItem = new object();

	// Token: 0x0400026D RID: 621
	private float _stateTimer;

	// Token: 0x0400026E RID: 622
	private bool _visitingForFirstTime;

	// Token: 0x0400026F RID: 623
	private bool _anyInitialNarrativeComplete;

	// Token: 0x04000270 RID: 624
	[SerializeField]
	private PeakStateSettings _settings;

	// Token: 0x04000271 RID: 625
	[SerializeField]
	private SLayout _holeUI;

	// Token: 0x04000272 RID: 626
	[SerializeField]
	private SLayout _zoomPrompt;
}
