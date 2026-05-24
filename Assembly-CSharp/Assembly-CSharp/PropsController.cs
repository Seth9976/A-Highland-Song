using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000038 RID: 56
public class PropsController : MonoBehaviour
{
	// Token: 0x17000098 RID: 152
	// (get) Token: 0x060001B0 RID: 432 RVA: 0x0000FB29 File Offset: 0x0000DD29
	public static PropsController instance
	{
		get
		{
			return GSR.PropsController;
		}
	}

	// Token: 0x17000099 RID: 153
	// (get) Token: 0x060001B1 RID: 433 RVA: 0x0000FB30 File Offset: 0x0000DD30
	public static Runner runner
	{
		get
		{
			return GSR.Runner;
		}
	}

	// Token: 0x1700009A RID: 154
	// (get) Token: 0x060001B2 RID: 434 RVA: 0x0000FB37 File Offset: 0x0000DD37
	public bool isInTriggerZone
	{
		get
		{
			return this._activeTriggerZones.Count > 0;
		}
	}

	// Token: 0x1700009B RID: 155
	// (get) Token: 0x060001B3 RID: 435 RVA: 0x0000FB47 File Offset: 0x0000DD47
	public bool isInAttractZone
	{
		get
		{
			return this._attractRangeProps.Count > 0;
		}
	}

	// Token: 0x1700009C RID: 156
	// (get) Token: 0x060001B4 RID: 436 RVA: 0x0000FB57 File Offset: 0x0000DD57
	public Prop lastInteractedPath
	{
		get
		{
			if (!(this.lastInteractedProp == null) && this.lastInteractedProp.isPathOut)
			{
				return this.lastInteractedProp;
			}
			return null;
		}
	}

	// Token: 0x1700009D RID: 157
	// (get) Token: 0x060001B5 RID: 437 RVA: 0x0000FB7C File Offset: 0x0000DD7C
	// (set) Token: 0x060001B6 RID: 438 RVA: 0x0000FB84 File Offset: 0x0000DD84
	public Prop lastInteractedProp { get; private set; }

	// Token: 0x1700009E RID: 158
	// (get) Token: 0x060001B7 RID: 439 RVA: 0x0000FB8D File Offset: 0x0000DD8D
	// (set) Token: 0x060001B8 RID: 440 RVA: 0x0000FB95 File Offset: 0x0000DD95
	public TriggerFlags triggerFlags { get; private set; }

	// Token: 0x1700009F RID: 159
	// (get) Token: 0x060001B9 RID: 441 RVA: 0x0000FB9E File Offset: 0x0000DD9E
	// (set) Token: 0x060001BA RID: 442 RVA: 0x0000FBA6 File Offset: 0x0000DDA6
	public HashSet<string> completedAutoRunZones { get; private set; } = new HashSet<string>();

	// Token: 0x170000A0 RID: 160
	// (get) Token: 0x060001BB RID: 443 RVA: 0x0000FBAF File Offset: 0x0000DDAF
	// (set) Token: 0x060001BC RID: 444 RVA: 0x0000FBB7 File Offset: 0x0000DDB7
	public HashSet<string> visitedPeaks { get; private set; } = new HashSet<string>();

	// Token: 0x170000A1 RID: 161
	// (get) Token: 0x060001BD RID: 445 RVA: 0x0000FBC0 File Offset: 0x0000DDC0
	// (set) Token: 0x060001BE RID: 446 RVA: 0x0000FBC8 File Offset: 0x0000DDC8
	public Prop currentActiveProp { get; private set; }

	// Token: 0x170000A2 RID: 162
	// (get) Token: 0x060001BF RID: 447 RVA: 0x0000FBD1 File Offset: 0x0000DDD1
	// (set) Token: 0x060001C0 RID: 448 RVA: 0x0000FBD9 File Offset: 0x0000DDD9
	public bool inRestPreventionZone { get; private set; }

	// Token: 0x170000A3 RID: 163
	// (get) Token: 0x060001C1 RID: 449 RVA: 0x0000FBE2 File Offset: 0x0000DDE2
	public List<Prop> triggerRangeProps
	{
		get
		{
			return this._triggerRangeProps;
		}
	}

	// Token: 0x060001C2 RID: 450 RVA: 0x0000FBEA File Offset: 0x0000DDEA
	private void OnEnable()
	{
		Narrative.onDidRefreshInteractables += this.OnNarrativeRefreshedInteractables;
		NarrativePresenter.onWillChooseChoiceOnProp = (NarrativePresenter.WillChooseChoiceOnPropDelegate)Delegate.Combine(NarrativePresenter.onWillChooseChoiceOnProp, new NarrativePresenter.WillChooseChoiceOnPropDelegate(this.OnWillChooseChoiceOnProp));
	}

	// Token: 0x060001C3 RID: 451 RVA: 0x0000FC20 File Offset: 0x0000DE20
	private void OnDisable()
	{
		Narrative.onDidRefreshInteractables -= this.OnNarrativeRefreshedInteractables;
		NarrativePresenter.onWillChooseChoiceOnProp = (NarrativePresenter.WillChooseChoiceOnPropDelegate)Delegate.Remove(NarrativePresenter.onWillChooseChoiceOnProp, new NarrativePresenter.WillChooseChoiceOnPropDelegate(this.OnWillChooseChoiceOnProp));
		this.lastInteractedProp = null;
		if (Narrative.instance != null)
		{
			Narrative.instance.preventBackgroundRemarks &= ~Narrative.PreventBackgroundRemarksReason.PropHighlighted;
		}
	}

	// Token: 0x060001C4 RID: 452 RVA: 0x0000FC88 File Offset: 0x0000DE88
	public void Refresh()
	{
		if (Game.gameplayPaused || !PropsController.runner.enabled)
		{
			return;
		}
		PropsController.runner.inSlowdownTrigger = false;
		Narrative.instance.PreventBackgroundRemarks(false, Narrative.PreventBackgroundRemarksReason.RemarkBlockerZone);
		this.UpdateTriggerZones();
		this.UpdateProps();
	}

	// Token: 0x060001C5 RID: 453 RVA: 0x0000FCC2 File Offset: 0x0000DEC2
	public void SetPropDotsAllowedByInk(bool allowed)
	{
		if (this._propDotsAllowedByInk != allowed)
		{
			this._propDotsAllowedByInk = allowed;
			this.Refresh();
		}
	}

	// Token: 0x060001C6 RID: 454 RVA: 0x0000FCDC File Offset: 0x0000DEDC
	public void Clear(bool visualsOnly)
	{
		this._activeTriggerZones.Clear();
		this._availableRestProps.Clear();
		this._attractRangeProps.Clear();
		this._triggerRangeProps.Clear();
		this._activeInkZonesAndPeaks.Clear();
		this.lastInteractedProp = null;
		this.explicitlyHiddenProps.Clear();
		this.triggerFlags = TriggerFlags.None;
		this.nearbyPeakProp = null;
		this.currentActiveProp = null;
		this.activeRestProp = null;
		this.inRestPreventionZone = false;
		this._activeLockingPropZone = null;
		this._zonesThatWantToLockPlayer.Clear();
		foreach (TriggerZone triggerZone in MonoInstancer<TriggerZone>.all)
		{
			triggerZone.triggering = false;
		}
		if (!visualsOnly)
		{
			this.completedAutoRunZones.Clear();
			this.visitedPeaks.Clear();
			this.passedProps.Clear();
			this.enabledProps.Clear();
			this.disabledProps.Clear();
		}
	}

	// Token: 0x060001C7 RID: 455 RVA: 0x0000FDE4 File Offset: 0x0000DFE4
	private void UpdateTriggerZones()
	{
		Narrative instance = Narrative.instance;
		Vector2 runnerPos = PropsController.runner.position;
		int physicalDepth = PropsController.runner.physicalDepthLayerIdx;
		this.nearbyPeakProp = null;
		foreach (TriggerZone triggerZone3 in Level.current.triggerZones.Nearby(runnerPos, Range.Centered(0f, 100000f), 40f, null))
		{
			if (triggerZone3.prop != null && triggerZone3.prop.isPeak && Vector2.Distance(triggerZone3.transform.position, runnerPos) < 60f && Range.Centered(triggerZone3.transform.position.z, 8f).Contains((float)physicalDepth))
			{
				this.nearbyPeakProp = triggerZone3.prop;
				break;
			}
		}
		this.inRestPreventionZone = false;
		this.triggerFlags = TriggerFlags.None;
		FinalJumpZone.activeZone = null;
		foreach (TriggerZone triggerZone2 in Level.current.triggerZones.Nearby(runnerPos, Range.Centered(0f, 100000f), 0f, null))
		{
			bool flag = triggerZone2.InsideTriggerDist(runnerPos, (float)physicalDepth, 1f);
			if (flag)
			{
				this.triggerFlags |= triggerZone2.flags;
			}
			if (flag && !triggerZone2.triggering)
			{
				triggerZone2.triggering = true;
				this._activeTriggerZones.Add(triggerZone2);
			}
			if (triggerZone2.triggering)
			{
				MusicControlPoint component = triggerZone2.GetComponent<MusicControlPoint>();
				if (PropsController.runner.running && component)
				{
					MonoSingleton<RunTrack>.instance.ChangeTrack(component.changeTrack, component.forceStopMusic, component.barIdx);
				}
				SlowdownTrigger component2 = triggerZone2.GetComponent<SlowdownTrigger>();
				if (PropsController.runner.running && component2)
				{
					PropsController.runner.inSlowdownTrigger = true;
				}
				if (triggerZone2.GetComponent<RemarkBlocker>() != null)
				{
					instance.PreventBackgroundRemarks(true, Narrative.PreventBackgroundRemarksReason.RemarkBlockerZone);
				}
				TutorialTrigger component3 = triggerZone2.GetComponent<TutorialTrigger>();
				if (component3 != null)
				{
					MonoSingleton<Tutorial>.instance.AddTriggerZone(component3);
				}
				FinalJumpZone component4 = triggerZone2.GetComponent<FinalJumpZone>();
				if (component4 != null)
				{
					FinalJumpZone.activeZone = component4;
				}
				Prop prop = triggerZone2.prop;
				if (prop != null)
				{
					if (prop.preventRest)
					{
						this.inRestPreventionZone = true;
					}
					GameChoice gameChoice;
					bool flag2 = (prop.isInkZone || prop.isPeak || (prop.isInkInteractableLike && PropsController.runner.isStationary)) && (instance.TryGetZoneLikeChoice(prop.inkListItemName, out gameChoice) || this._zonesThatWantToLockPlayer.Contains(prop));
					float num = -1f;
					if (!this._activeInkZonesAndPeaks.Contains(triggerZone2) && flag2 && (PropsController.runner.running || PropsController.runner.jumping || PropsController.runner.bellyWriggling || PropsController.runner.hasInkPose) && (!Narrative.instance.isBusy || (this._zonesThatWantToLockPlayer.Contains(prop) && this._activeLockingPropZone == null && PropsController.CanAutoRunToProp(prop, out num))))
					{
						this._activeInkZonesAndPeaks.Add(triggerZone2);
						this.lastInteractedProp = prop;
						if (!Narrative.instance.isBusy)
						{
							this.TryMakeChoiceForPeakOrZone(prop);
						}
						else
						{
							this._activeLockingPropZone = prop;
							PropsController.runner.SetAutoRunTarget(triggerZone2.transform.position, num * 1.5f, false);
						}
					}
				}
			}
		}
		this._activeTriggerZones.RemoveAll(delegate(TriggerZone triggerZone)
		{
			if (triggerZone == null)
			{
				return true;
			}
			if (triggerZone.InsideTriggerDist(runnerPos, (float)physicalDepth, 1.1f))
			{
				return false;
			}
			triggerZone.triggering = false;
			this._activeInkZonesAndPeaks.Remove(triggerZone);
			MonoSingleton<Tutorial>.instance.RemoveTriggerWithZone(triggerZone);
			Prop prop2 = triggerZone.prop;
			if (prop2 != null)
			{
				this.explicitlyHiddenProps.Remove(prop2);
			}
			if (this.lastInteractedProp != null && this.lastInteractedProp == prop2)
			{
				this.lastInteractedProp = null;
			}
			return true;
		});
		this.explicitlyHiddenProps.RemoveAll((Prop p) => p == null);
	}

	// Token: 0x060001C8 RID: 456 RVA: 0x0001023C File Offset: 0x0000E43C
	private void UpdateProps()
	{
		Runner instance = Runner.instance;
		Vector2 position = instance.position;
		int physicalDepthLayerIdx = instance.physicalDepthLayerIdx;
		Narrative instance2 = Narrative.instance;
		this._attractRangeProps.Clear();
		this._triggerRangeProps.Clear();
		bool flag = GameClock.instance.isNight && (instance.playerControlDisabled & PlayerControlDisableReason.Ink) > PlayerControlDisableReason.None;
		bool flag2 = (instance.playerControlDisabled & PlayerControlDisableReason.NarrativeChoicesWithNoExit) > PlayerControlDisableReason.None;
		bool flag3 = Game.instance.inActiveGameplay && !instance.stoneSkimming && !instance.dead && !instance.inFinalJumpAndLeftLand && !instance.falling && !instance.hardLanding && !instance.onPath && !instance.hidden && !flag && this._propDotsAllowedByInk;
		bool flag4 = flag3 && !NarrativePresenter.presenting;
		Prop prop = null;
		float num = float.MaxValue;
		if (flag3)
		{
			foreach (Prop prop2 in Level.current.peaks)
			{
				bool flag5 = !this.visitedPeaks.Contains(prop2.inkListItemName);
				if (prop2.inkListItemName != "" && flag5 && prop2.triggerZone.InsideRadiusOf(position, (float)physicalDepthLayerIdx, 200f, 100f))
				{
					this._attractRangeProps.Add(prop2);
				}
			}
			foreach (TriggerZone triggerZone in Level.current.triggerZones.Nearby(position, new Range(-10000f, 100000f), 0f, null))
			{
				Prop prop3 = triggerZone.prop;
				if (!(prop3 == null) && prop3.interactive && triggerZone.InsideAttractDist(position, (float)physicalDepthLayerIdx))
				{
					string inkListItemName = prop3.inkListItemName;
					MarkerState markerState = MarkerState.unplacedOrNotAMarker;
					bool flag6 = inkListItemName == "MAP_MARKER";
					if (flag6)
					{
						string targetInkPropName = prop3.GetComponentInParent<MapMarkerWorld>().map.targetInkPropName;
						markerState = MonoSingleton<MapsViewController>.instance.GetMarkerState(targetInkPropName);
					}
					if (prop3.isInkInteractableLike)
					{
						bool flag7 = flag4 && instance2.HasInteractableChoice(inkListItemName, markerState);
						bool flag8 = !flag4 && !flag2 && instance2.DidHaveInteractableChoice(inkListItemName);
						if (flag7 || flag8)
						{
							if (flag7)
							{
								prop3.hasRestChoice = instance2.HasRestChoice(inkListItemName);
								prop3.hasTakePathChoice = instance2.HasTakePathChoice(inkListItemName);
								if (prop3.isPathOut && !prop3.pathIsLocal && prop3.hasTakePathChoice)
								{
									this.foundPaths.Add(prop3.inkListItemName);
								}
							}
							if (!triggerZone.InsideTriggerDist(position, (float)physicalDepthLayerIdx, 1f) || this.explicitlyHiddenProps.Contains(prop3) || !flag4)
							{
								this._attractRangeProps.Add(prop3);
							}
							else
							{
								this._triggerRangeProps.Add(prop3);
								if (triggerZone.InsideTriggerDist(position, (float)physicalDepthLayerIdx, 1f))
								{
									float num2 = Vector2.Distance(position, triggerZone.transform.position);
									if (flag6)
									{
										num2 = Mathf.Max(num2 - 5f, 0f);
									}
									if (num2 < num)
									{
										prop = prop3;
										num = num2;
									}
								}
							}
						}
					}
				}
			}
		}
		this.currentActiveProp = prop;
		List<GameChoice> list = null;
		if (this.currentActiveProp != null)
		{
			MarkerState markerState2 = MarkerState.unplacedOrNotAMarker;
			MapMarkerWorld componentInParent = this.currentActiveProp.GetComponentInParent<MapMarkerWorld>();
			if (componentInParent != null)
			{
				string targetInkPropName2 = componentInParent.map.targetInkPropName;
				markerState2 = MonoSingleton<MapsViewController>.instance.GetMarkerState(targetInkPropName2);
			}
			list = Narrative.instance.GetPropTextChoices(this.currentActiveProp.inkListItemName, markerState2, true, false);
			PropsController.ReplacePathChoiceTextForPathDirection(this.currentActiveProp, list);
			if (!this.currentActiveProp.isInkZone && Narrative.instance.HasZoneLikeChoice(this.currentActiveProp.inkListItemName))
			{
				list.Clear();
			}
		}
		NarrativePresenter.instance.SetPropsState(this._attractRangeProps, this._triggerRangeProps, this.currentActiveProp, list);
		if (this.currentActiveProp != null)
		{
			Narrative.instance.preventBackgroundRemarks |= Narrative.PreventBackgroundRemarksReason.PropHighlighted;
		}
		else
		{
			Narrative.instance.preventBackgroundRemarks &= ~Narrative.PreventBackgroundRemarksReason.PropHighlighted;
		}
		this.activeRestProp = null;
		foreach (Prop prop4 in this._availableRestProps)
		{
			if (prop4 != null && prop4.interactive && prop4.triggerZone.InsideTriggerDist(position, (float)physicalDepthLayerIdx, 1f))
			{
				this.activeRestProp = prop4;
				break;
			}
		}
	}

	// Token: 0x060001C9 RID: 457 RVA: 0x00010740 File Offset: 0x0000E940
	private static void ReplacePathChoiceTextForPathDirection(Prop activeProp, List<GameChoice> choices)
	{
		if (activeProp.isPathOut)
		{
			bool flag = true;
			GameObject gameObject = activeProp.pathDestination.gameObject;
			if (gameObject != null)
			{
				float num = gameObject.transform.position.z - activeProp.transform.position.z;
				flag = num > 0.5f || (num >= -0.5f && gameObject.transform.position.y > activeProp.transform.position.y);
			}
			for (int i = 0; i < choices.Count; i++)
			{
				GameChoice gameChoice = choices[i];
				string text = "PATH TEXT";
				if (gameChoice.text.StartsWith(text))
				{
					int num2 = gameChoice.text.IndexOf("/");
					if (flag)
					{
						gameChoice.text = gameChoice.text.Substring(text.Length, num2 - text.Length).Trim();
					}
					else
					{
						gameChoice.text = gameChoice.text.Substring(num2 + 1).Trim();
					}
					choices[i] = gameChoice;
				}
			}
		}
	}

	// Token: 0x060001CA RID: 458 RVA: 0x0001086C File Offset: 0x0000EA6C
	private void UpdateZonesThatWantToLockPlayer()
	{
		if (!Narrative.instance.isBusy)
		{
			this._zonesThatWantToLockPlayer.Clear();
			foreach (GameChoice gameChoice in Narrative.instance.GetZoneLikeChoices())
			{
				foreach (string text in gameChoice.interactableNames)
				{
					if (!this.completedAutoRunZones.Contains(text))
					{
						foreach (Prop prop in Level.current.PropsWithName(text))
						{
							if (prop.isInkZone && prop.autoRunToZone)
							{
								this._zonesThatWantToLockPlayer.Add(prop);
							}
						}
					}
				}
			}
			foreach (Prop prop2 in Level.current.props.all)
			{
				if (prop2.isPeak && !this.visitedPeaks.Contains(prop2.inkListItemName))
				{
					this._zonesThatWantToLockPlayer.Add(prop2);
				}
			}
		}
	}

	// Token: 0x060001CB RID: 459 RVA: 0x000109F4 File Offset: 0x0000EBF4
	public static bool CanAutoRunToProp(Prop prop, out float estTimeToReach)
	{
		Runner instance = Runner.instance;
		estTimeToReach = float.MaxValue;
		if (!instance.running)
		{
			return false;
		}
		SlopeSample slopeSample;
		Raycast.SampleWithDepthRange(prop.transform.position + Vector2.up * 0.5f * 10f, prop.transform.position + Vector2.down * 10f, prop.transform.position.z, instance.raycastNearbyRange, out slopeSample, default(Color));
		if (instance.currentSlope == slopeSample.slope)
		{
			Simulate.FindResult findResult = Simulate.FindTimeFromTo(instance.currentSlope, instance.position.x, slopeSample.point.x, Simulate.FindOptions.standardPredict, instance.settings);
			if (findResult.duration >= 0f)
			{
				estTimeToReach = findResult.duration;
				return true;
			}
		}
		return false;
	}

	// Token: 0x060001CC RID: 460 RVA: 0x00010AEC File Offset: 0x0000ECEC
	public void SetPropEnabled(string inkPropName, bool propEnabled)
	{
		if (propEnabled)
		{
			this.disabledProps.Remove(inkPropName);
			this.enabledProps.Add(inkPropName);
		}
		else
		{
			this.disabledProps.Add(inkPropName);
			this.enabledProps.Remove(inkPropName);
		}
		foreach (Prop prop in Prop.GetLoadedPropsByInkName(inkPropName))
		{
			prop.SetEnabledByInk(propEnabled, false);
		}
	}

	// Token: 0x060001CD RID: 461 RVA: 0x00010B78 File Offset: 0x0000ED78
	public bool IsPropEnabled(string inkPropName)
	{
		if (PropsController.instance.enabledProps.Contains(inkPropName))
		{
			return true;
		}
		if (PropsController.instance.disabledProps.Contains(inkPropName))
		{
			return false;
		}
		List<Prop> loadedPropsByInkName = Prop.GetLoadedPropsByInkName(inkPropName);
		return loadedPropsByInkName.Count <= 0 || loadedPropsByInkName[0].interactive;
	}

	// Token: 0x060001CE RID: 462 RVA: 0x00010BCC File Offset: 0x0000EDCC
	public void RefreshPropsEnabledState()
	{
		SaveState saveState = MonoSingleton<Main>.instance.saveState;
		foreach (Prop prop in Prop.allLoadedProps)
		{
			bool flag = this.enabledProps.Contains(prop.inkListItemName) || (!this.disabledProps.Contains(prop.inkListItemName) && !prop.disabledAtStart);
			prop.SetEnabledByInk(flag, true);
		}
	}

	// Token: 0x060001CF RID: 463 RVA: 0x00010C64 File Offset: 0x0000EE64
	private void OnNarrativeRefreshedInteractables()
	{
		List<GameChoice> restChoices = Narrative.instance.GetRestChoices();
		this._availableRestProps.Clear();
		foreach (GameChoice gameChoice in restChoices)
		{
			foreach (string text in gameChoice.interactableNames)
			{
				List<Prop> list = Level.current.PropsWithName(text);
				this._availableRestProps.AddRange(list);
			}
		}
		if (this._activeLockingPropZone != null)
		{
			Prop activeLockingPropZone = this._activeLockingPropZone;
			this._activeLockingPropZone = null;
			this.TryMakeChoiceForPeakOrZone(activeLockingPropZone);
		}
		this.UpdateZonesThatWantToLockPlayer();
		this.UpdateProps();
	}

	// Token: 0x060001D0 RID: 464 RVA: 0x00010D44 File Offset: 0x0000EF44
	private bool TryMakeChoiceForPeakOrZone(Prop prop)
	{
		if (prop.isPeak)
		{
			Narrative.instance.NotePeak(prop.inkListItemName);
			if (this.visitedPeaks.Contains(prop.inkListItemName))
			{
				return false;
			}
			Narrative.instance.ReachPeak(prop.inkListItemName);
		}
		else if (Narrative.instance.TryChooseZoneLikeChoice(prop.inkListItemName))
		{
			this.completedAutoRunZones.Add(prop.inkListItemName);
		}
		return true;
	}

	// Token: 0x060001D1 RID: 465 RVA: 0x00010DB6 File Offset: 0x0000EFB6
	public void OnLevelChanged()
	{
		this._availableRestProps.Clear();
	}

	// Token: 0x060001D2 RID: 466 RVA: 0x00010DC3 File Offset: 0x0000EFC3
	private void OnWillChooseChoiceOnProp(Prop prop)
	{
		this.lastInteractedProp = prop;
	}

	// Token: 0x0400027D RID: 637
	public List<Prop> explicitlyHiddenProps = new List<Prop>();

	// Token: 0x0400027F RID: 639
	[Disable]
	public Prop nearbyPeakProp;

	// Token: 0x04000280 RID: 640
	private const float nearbyPeakRadius = 60f;

	// Token: 0x04000281 RID: 641
	public HashSet<string> passedProps = new HashSet<string>();

	// Token: 0x04000282 RID: 642
	public HashSet<string> disabledProps = new HashSet<string>();

	// Token: 0x04000283 RID: 643
	public HashSet<string> enabledProps = new HashSet<string>();

	// Token: 0x04000286 RID: 646
	public HashSet<string> foundPaths = new HashSet<string>();

	// Token: 0x04000288 RID: 648
	public Prop activeRestProp;

	// Token: 0x0400028A RID: 650
	private List<TriggerZone> _activeTriggerZones = new List<TriggerZone>();

	// Token: 0x0400028B RID: 651
	private List<TriggerZone> _activeInkZonesAndPeaks = new List<TriggerZone>();

	// Token: 0x0400028C RID: 652
	private List<Prop> _attractRangeProps = new List<Prop>();

	// Token: 0x0400028D RID: 653
	private List<Prop> _triggerRangeProps = new List<Prop>();

	// Token: 0x0400028E RID: 654
	private List<Prop> _availableRestProps = new List<Prop>();

	// Token: 0x0400028F RID: 655
	private HashSet<Prop> _zonesThatWantToLockPlayer = new HashSet<Prop>();

	// Token: 0x04000290 RID: 656
	private Prop _activeLockingPropZone;

	// Token: 0x04000291 RID: 657
	private bool _propDotsAllowedByInk = true;
}
