using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

// Token: 0x02000125 RID: 293
public class MapsViewController : MonoSingleton<MapsViewController>
{
	// Token: 0x1700026C RID: 620
	// (get) Token: 0x060009BF RID: 2495 RVA: 0x000519A4 File Offset: 0x0004FBA4
	public bool isBusy
	{
		get
		{
			return this.gettingMaps || this.confirmingMap;
		}
	}

	// Token: 0x1700026D RID: 621
	// (get) Token: 0x060009C0 RID: 2496 RVA: 0x000519B6 File Offset: 0x0004FBB6
	// (set) Token: 0x060009C1 RID: 2497 RVA: 0x000519BE File Offset: 0x0004FBBE
	public bool gettingMaps { get; private set; }

	// Token: 0x1700026E RID: 622
	// (get) Token: 0x060009C2 RID: 2498 RVA: 0x000519C7 File Offset: 0x0004FBC7
	// (set) Token: 0x060009C3 RID: 2499 RVA: 0x000519CF File Offset: 0x0004FBCF
	public bool confirmingMap { get; private set; }

	// Token: 0x1700026F RID: 623
	// (get) Token: 0x060009C4 RID: 2500 RVA: 0x000519D8 File Offset: 0x0004FBD8
	public bool maximised
	{
		get
		{
			return this._visibleAndMaximised;
		}
	}

	// Token: 0x17000270 RID: 624
	// (get) Token: 0x060009C5 RID: 2501 RVA: 0x000519E0 File Offset: 0x0004FBE0
	public float maximisedNorm
	{
		get
		{
			return this._maximiseNorm;
		}
	}

	// Token: 0x17000271 RID: 625
	// (get) Token: 0x060009C6 RID: 2502 RVA: 0x000519E8 File Offset: 0x0004FBE8
	public bool mapIsSelected
	{
		get
		{
			return this._visibleAndMaximised && this.selectedMap != null;
		}
	}

	// Token: 0x17000272 RID: 626
	// (get) Token: 0x060009C7 RID: 2503 RVA: 0x00051A00 File Offset: 0x0004FC00
	public bool cameraViewportIsOffsetForMaps
	{
		get
		{
			return (this.mapIsSelected && MonoSingleton<PeakStateController>.instance.active) || this.confirmingMap || (this.gettingMaps && MonoSingleton<PeakStateController>.instance.active && MonoSingleton<PeakStateController>.instance.transition >= 1f);
		}
	}

	// Token: 0x17000273 RID: 627
	// (get) Token: 0x060009C8 RID: 2504 RVA: 0x00051A55 File Offset: 0x0004FC55
	public float cameraViewportOffsetNorm
	{
		get
		{
			return Mathf.SmoothStep(0f, 1f, this._cameraViewportOffsetNorm);
		}
	}

	// Token: 0x17000274 RID: 628
	// (get) Token: 0x060009C9 RID: 2505 RVA: 0x00051A6C File Offset: 0x0004FC6C
	public bool targettingBeyondCurrentLevel
	{
		get
		{
			if (this._focussingOnOccludedMarkerPastCurrentLevel)
			{
				return true;
			}
			if (this._previewMarker == null)
			{
				return false;
			}
			float num = Level.currentZ + 250f;
			return this._previewMarker.worldPos.z > num;
		}
	}

	// Token: 0x17000275 RID: 629
	// (get) Token: 0x060009CA RID: 2506 RVA: 0x00051AB2 File Offset: 0x0004FCB2
	private bool allowInput
	{
		get
		{
			return this._visibleAndMaximised && !this.isBusy && !MonoSingleton<JournalController>.instance.visible && !PhotoMode.visible;
		}
	}

	// Token: 0x17000276 RID: 630
	// (get) Token: 0x060009CB RID: 2507 RVA: 0x00051ADA File Offset: 0x0004FCDA
	private bool allowShowMapsInput
	{
		get
		{
			return this.visibleAtAll && !this.isBusy && !MonoSingleton<JournalController>.instance.visible && !PhotoMode.visible;
		}
	}

	// Token: 0x17000277 RID: 631
	// (get) Token: 0x060009CC RID: 2508 RVA: 0x00051B02 File Offset: 0x0004FD02
	private bool visibleAtAll
	{
		get
		{
			return this._visibleAndMaximised || this.showReason > MapsViewController.ShowReason.None;
		}
	}

	// Token: 0x17000278 RID: 632
	// (get) Token: 0x060009CD RID: 2509 RVA: 0x00051B17 File Offset: 0x0004FD17
	private CanvasGroup canvasGroup
	{
		get
		{
			if (this._canvasGroup == null)
			{
				this._canvasGroup = base.GetComponent<CanvasGroup>();
			}
			return this._canvasGroup;
		}
	}

	// Token: 0x17000279 RID: 633
	// (get) Token: 0x060009CE RID: 2510 RVA: 0x00051B39 File Offset: 0x0004FD39
	// (set) Token: 0x060009CF RID: 2511 RVA: 0x00051B41 File Offset: 0x0004FD41
	public Map nearbyMarkerWithMap { get; private set; }

	// Token: 0x1700027A RID: 634
	// (get) Token: 0x060009D0 RID: 2512 RVA: 0x00051B4C File Offset: 0x0004FD4C
	public Vector2 reticulePosNorm
	{
		get
		{
			Vector2 center = this._spottingReticule.center;
			Vector2 canvasSize = this._spottingReticule.canvasSize;
			return new Vector2(center.x / canvasSize.x, center.y / canvasSize.y);
		}
	}

	// Token: 0x1700027B RID: 635
	// (get) Token: 0x060009D1 RID: 2513 RVA: 0x00051B90 File Offset: 0x0004FD90
	public Map inVicinityPromptableMap
	{
		get
		{
			if (this._nearbyPromptableMap_lastFrameIdxRun == Time.frameCount)
			{
				return this._nearbyPromptableMap;
			}
			float num = float.MaxValue;
			this._nearbyPromptableMap = null;
			Prop prop = Prop.NearestMajorPeak(Runner.instance.physicalPosition3d);
			if (prop != null && !PropsController.instance.visitedPeaks.Contains(prop.inkListItemName))
			{
				Vector3 position = prop.transform.position;
				Vector3 physicalPosition3d = Runner.instance.physicalPosition3d;
				if (Vector2.Distance(position, physicalPosition3d) < 80f && Mathf.Abs(position.z - physicalPosition3d.z) < 10f)
				{
					goto IL_0194;
				}
			}
			foreach (Map map in MonoSingleton<Inventory>.instance.incompleteForwardFacingMaps)
			{
				if (!map.neverPromptDirectly)
				{
					bool flag = MonoSingleton<MapsViewController>.instance.mapMarkerPositions.ContainsKey(map);
					if (!flag || !MonoSingleton<MapsViewController>.instance.MarkerIsCorrectlyPlaced(map))
					{
						ValueTuple<bool, Prop, bool> valueTuple = this.PositionIsWithinMapTarget(Runner.instance.physicalPosition3d, map, map.nearbyRadius, map.nearbyZ, false);
						bool item = valueTuple.Item1;
						Prop item2 = valueTuple.Item2;
						bool item3 = valueTuple.Item3;
						if (item && item3)
						{
							float num2 = Vector2.Distance(Runner.instance.position, item2.transform.position);
							if (num2 <= num && (!flag || Narrative.instance.MapWantsToBeFound(map.targetInkPropName)))
							{
								this._nearbyPromptableMap = map;
								num = num2;
							}
						}
					}
				}
			}
			IL_0194:
			this._nearbyPromptableMap_lastFrameIdxRun = Time.frameCount;
			return this._nearbyPromptableMap;
		}
	}

	// Token: 0x1700027C RID: 636
	// (get) Token: 0x060009D2 RID: 2514 RVA: 0x00051D54 File Offset: 0x0004FF54
	private Map defaultMapToSelect
	{
		get
		{
			Map map = null;
			if (this.nearbyMarkerWithMap != null)
			{
				map = this.nearbyMarkerWithMap;
			}
			else if (this._selectedMap != null)
			{
				map = this._selectedMap;
			}
			else
			{
				List<Map> incompleteForwardFacingMaps = MonoSingleton<Inventory>.instance.incompleteForwardFacingMaps;
				if (incompleteForwardFacingMaps.Count > 0)
				{
					map = incompleteForwardFacingMaps[0];
				}
			}
			return map;
		}
	}

	// Token: 0x1700027D RID: 637
	// (get) Token: 0x060009D3 RID: 2515 RVA: 0x00051DAE File Offset: 0x0004FFAE
	private SLayout layout
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

	// Token: 0x060009D4 RID: 2516 RVA: 0x00051DD0 File Offset: 0x0004FFD0
	public void Clear(bool visualsOnly)
	{
		this.nearbyMarkerWithMap = null;
		if (!visualsOnly)
		{
			this.mapMarkerPositions.Clear();
			foreach (MapMarkerWorld mapMarkerWorld in this._worldMapMarkers.Values)
			{
				mapMarkerWorld.ReturnToPool(true);
			}
			this._worldMapMarkers.Clear();
		}
	}

	// Token: 0x060009D5 RID: 2517 RVA: 0x00051E48 File Offset: 0x00050048
	public void ShowMinimised(MapsViewController.ShowReason reason)
	{
		if (MonoSingleton<Inventory>.instance.incompleteForwardFacingMaps.Count == 0)
		{
			Debug.LogError("Can't show maps because you don't have any (that aren't already found)!");
			return;
		}
		bool flag = this.showReason != MapsViewController.ShowReason.None;
		this.showReason |= reason;
		if (flag)
		{
			return;
		}
		this.selectedMap = this.defaultMapToSelect;
		MapView mapView = this._mapViewPrototype.Instantiate<MapView>(null);
		mapView.map = this.selectedMap;
		mapView.layout.size = this._settings.mapAreaSize;
		mapView.foundTick.groupAlpha = 0f;
		this._mapViews.Add(mapView);
		this._selectedSmoothIdx = (float)this.selectedMapIndex;
		this.UpdateLayout(null, null);
		this.RefreshPromptsUI(false);
	}

	// Token: 0x060009D6 RID: 2518 RVA: 0x00051EFC File Offset: 0x000500FC
	public void Show(MapsViewController.ShowReason reason)
	{
		if (MonoSingleton<Inventory>.instance.incompleteForwardFacingMaps.Count == 0)
		{
			Debug.LogError("Can't show maps because you don't have any (that aren't already found)!");
			return;
		}
		this.showReason |= reason;
		if (!this.visibleAtAll)
		{
			this.selectedMap = this.defaultMapToSelect;
		}
		this.Maximise();
	}

	// Token: 0x060009D7 RID: 2519 RVA: 0x00051F50 File Offset: 0x00050150
	private void Maximise()
	{
		if (this._visibleAndMaximised)
		{
			return;
		}
		this._visibleAndMaximised = true;
		if (this.selectedMap == null)
		{
			this.selectedMap = this.defaultMapToSelect;
		}
		this._oldMapViews.Clear();
		this._oldMapViews.AddRange(this._mapViews);
		this._mapViews.Clear();
		int num = int.MaxValue;
		List<Map> incompleteForwardFacingMaps = MonoSingleton<Inventory>.instance.incompleteForwardFacingMaps;
		using (List<Map>.Enumerator enumerator = incompleteForwardFacingMaps.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Map map = enumerator.Current;
				if (map.sprite == null)
				{
					Debug.LogError("Map " + map.name + " doesn't have a sprite! It won't show up in the UI. Click here to select the map asset.", map);
				}
				else
				{
					MapView mapView = null;
					for (int i = 0; i < this._oldMapViews.Count; i++)
					{
						MapView mapView2 = this._oldMapViews[i];
						if (mapView2.map == map)
						{
							mapView = mapView2;
							this._oldMapViews.RemoveAt(i);
							break;
						}
					}
					if (mapView == null)
					{
						mapView = this._mapViewPrototype.Instantiate<MapView>(null);
						mapView.map = map;
						mapView.layout.size = this._settings.mapAreaSize;
						mapView.foundTick.groupAlpha = 0f;
					}
					if (this._mapViews.Exists((MapView v) => v.map == map))
					{
						Debug.LogError("Already exists");
					}
					this._mapViews.Add(mapView);
					num = Mathf.Min(num, mapView.transform.GetSiblingIndex());
				}
			}
		}
		foreach (MapView mapView3 in this._oldMapViews)
		{
			Debug.LogError("Unexpected spare map view needed to be deleted here.");
			mapView3.ReturnToPool();
		}
		this._oldMapViews.Clear();
		for (int j = 0; j < this._mapViews.Count; j++)
		{
			int num2 = this._mapViews.Count - 1 - j;
			this._mapViews[num2].transform.SetSiblingIndex(num + j);
		}
		if (!incompleteForwardFacingMaps.Contains(this.selectedMap))
		{
			this._selectedMap = ((incompleteForwardFacingMaps.Count > 0) ? incompleteForwardFacingMaps[0] : null);
		}
		this._selectedSmoothIdx = (float)this.selectedMapIndex;
		this.UpdateLayout(null, null);
		this.RefreshPromptsUI(false);
		if (MonoSingleton<PeakStateController>.instance.active)
		{
			foreach (KeyValuePair<Map, Vector3> keyValuePair in this.mapMarkerPositions)
			{
				Map key = keyValuePair.Key;
				Vector3 value = keyValuePair.Value;
				if (WorldManager.instance.GetLevelAtDepth(value.z).isSetup)
				{
					MapMarker mapMarker = this.CreateMarker(keyValuePair.Value, keyValuePair.Key);
					this._markers.Add(mapMarker);
				}
			}
		}
		this._hasMovedReticuleAwayFromSelf = false;
	}

	// Token: 0x060009D8 RID: 2520 RVA: 0x000522CC File Offset: 0x000504CC
	private void ExitMaximised()
	{
		if (!this._visibleAndMaximised)
		{
			return;
		}
		this._visibleAndMaximised = false;
		if (this.showReason == MapsViewController.ShowReason.ByMapMarker && this.nearbyMarkerWithMap != null)
		{
			this.selectedMap = this.nearbyMarkerWithMap;
		}
		foreach (MapMarker mapMarker in this._markers)
		{
			mapMarker.beingRemoved = true;
		}
		this._previewMarker = null;
		this._confirmMarkerPrompt.Animate(0.5f, delegate
		{
			this._confirmMarkerPrompt.groupAlpha = 0f;
		});
		this._focussingOnOccludedMarkerPastCurrentLevel = false;
		this.RefreshPromptsUI(false);
	}

	// Token: 0x060009D9 RID: 2521 RVA: 0x00052384 File Offset: 0x00050584
	public void Hide(MapsViewController.ShowReason reason)
	{
		if (this.showReason == MapsViewController.ShowReason.None && !this._visibleAndMaximised)
		{
			return;
		}
		this.showReason &= ~reason;
		if (this._visibleAndMaximised)
		{
			this.ExitMaximised();
		}
		else
		{
			this.RefreshPromptsUI(false);
		}
		if (this.showReason == MapsViewController.ShowReason.ByMapMarker && (this.selectedMap == null || this.selectedMap != this.nearbyMarkerWithMap))
		{
			this.selectedMap = this.nearbyMarkerWithMap;
			if (!this._mapViews.Exists((MapView v) => v.map == this.nearbyMarkerWithMap))
			{
				MapView mapView = this._mapViewPrototype.Instantiate<MapView>(null);
				mapView.map = this.nearbyMarkerWithMap;
				mapView.layout.size = this._settings.mapAreaSize;
				mapView.foundTick.groupAlpha = 0f;
				this._mapViews.Add(mapView);
			}
		}
	}

	// Token: 0x060009DA RID: 2522 RVA: 0x00052468 File Offset: 0x00050668
	public void GetMapWithPropName(string propName)
	{
		if (string.IsNullOrWhiteSpace(propName))
		{
			Debug.LogError("GetMapWithPropName: propName was null or empty");
			return;
		}
		using (List<Map>.Enumerator enumerator = MonoSingleton<Inventory>.instance.maps.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.targetInkPropName == propName)
				{
					return;
				}
			}
		}
		Map map = null;
		foreach (Map map2 in Map.all)
		{
			if (map2.targetInkPropName == propName)
			{
				map = map2;
				break;
			}
		}
		if (map == null)
		{
			Debug.LogError("Tried to get a Map that doesn't exist that targets a prop with the name: " + propName);
			return;
		}
		this._getMapQueue.Add(map);
		if (this._getMapQueue.Count <= 1)
		{
			base.StartCoroutine(this.GetMaps_Coroutine());
		}
	}

	// Token: 0x060009DB RID: 2523 RVA: 0x0005254C File Offset: 0x0005074C
	private IEnumerator GetMaps_Coroutine()
	{
		this.gettingMaps = true;
		Runner.instance.playerControlDisabled |= PlayerControlDisableReason.MapsTransition;
		this.layout.Animate(0.5f, delegate
		{
			this.UpdateLayout(null, null);
		});
		this.selectedMap = null;
		this._darkenFullscreenBG.CancelAnimations();
		this._darkenFullscreenBG.gameObject.SetActive(true);
		this._darkenFullscreenBG.alpha = 0f;
		this._darkenFullscreenBG.Animate(0.5f, delegate
		{
			this._darkenFullscreenBG.alpha = this._settings.getMapBGDarkness;
		});
		bool hasPlayedSting = false;
		while (this._getMapQueue.Count > 0)
		{
			MapsViewController.<>c__DisplayClass56_0 CS$<>8__locals1 = new MapsViewController.<>c__DisplayClass56_0();
			CS$<>8__locals1.<>4__this = this;
			Map nextMapToReveal = this._getMapQueue[0];
			CS$<>8__locals1.view = this._mapViewPrototype.Instantiate<MapView>(null);
			CS$<>8__locals1.view.map = nextMapToReveal;
			CS$<>8__locals1.view.transform.SetAsLastSibling();
			CS$<>8__locals1.view.layout.groupAlpha = 0f;
			CS$<>8__locals1.view.layout.size = this._settings.mapAreaSize;
			CS$<>8__locals1.view.layout.scale = this._settings.getMapStartScale;
			CS$<>8__locals1.view.layout.center = CS$<>8__locals1.view.layout.parent.middle + new Vector2(0f, this._settings.getMapStartOffsetY);
			CS$<>8__locals1.view.layout.rotation = 0f;
			CS$<>8__locals1.view.foundTick.groupAlpha = 0f;
			SLayoutAnimation mapAppearAnim = CS$<>8__locals1.view.layout.Animate(this._settings.getMapAppearDuration, 0f, this._settings.getMapAppearCurve, delegate
			{
				CS$<>8__locals1.view.layout.groupAlpha = 1f;
				CS$<>8__locals1.view.layout.scale = 1f;
				CS$<>8__locals1.view.layout.center = CS$<>8__locals1.view.layout.parent.middle;
			});
			if (!hasPlayedSting)
			{
				AudioController.instance.PlaySting(Sting.MiniTriumph, -1);
				hasPlayedSting = true;
			}
			CS$<>8__locals1.view.transform.SetAsLastSibling();
			do
			{
				yield return null;
				if (mapAppearAnim.isComplete && !this._getMapContinuePrompt.gameObject.activeSelf)
				{
					this._getMapContinuePrompt.gameObject.SetActive(true);
					this._getMapContinuePrompt.Animate(0.3f, delegate
					{
						this._getMapContinuePrompt.groupAlpha = 1f;
					});
				}
			}
			while (!GameInput.skipDialogue && !GameInput.selectChoice && !GameInput.selectMenuItem && !GameInput.rawBackWasPressed);
			if (!mapAppearAnim.isComplete)
			{
				mapAppearAnim.CompleteImmediate();
			}
			this._getMapContinuePrompt.Animate(0.3f, delegate
			{
				this._getMapContinuePrompt.groupAlpha = 0f;
			}).Then(delegate
			{
				this._getMapContinuePrompt.gameObject.SetActive(false);
			});
			MonoSingleton<Inventory>.instance.maps.Insert(0, nextMapToReveal);
			this._getMapQueue.RemoveAt(0);
			if (this._getMapQueue.Count == 0)
			{
				this.gettingMaps = false;
			}
			Runner.instance.playerControlDisabled &= ~PlayerControlDisableReason.MapsTransition;
			this.selectedMap = nextMapToReveal;
			this._selectedSmoothIdx = 0f;
			bool flag = this.visibleAtAll || MonoSingleton<PeakStateController>.instance.active;
			if (this._getMapQueue.Count > 0 || !flag)
			{
				CS$<>8__locals1.view.layout.Animate(0.5f, delegate
				{
					CS$<>8__locals1.view.layout.rightX = CS$<>8__locals1.view.layout.parentRect.width - CS$<>8__locals1.<>4__this._settings.minimisedMapOffsetFromTopRight.x;
					CS$<>8__locals1.view.layout.bottomY = CS$<>8__locals1.<>4__this._settings.mapAreaOffsetFromTopRight.y + CS$<>8__locals1.<>4__this._settings.minimisedMapOffsetFromTopRight.y;
					CS$<>8__locals1.view.layout.rotation = CS$<>8__locals1.<>4__this._settings.minimisedMapAngle;
					CS$<>8__locals1.view.layout.scale = CS$<>8__locals1.<>4__this._settings.deselectedScalar;
				}).ThenAnimate(0.5f, 1f, delegate
				{
					CS$<>8__locals1.view.layout.y -= 500f;
					CS$<>8__locals1.view.layout.groupAlpha = 0f;
				}).Then(delegate
				{
					CS$<>8__locals1.view.ReturnToPool();
				});
			}
			else
			{
				this._mapViews.Insert(0, CS$<>8__locals1.view);
				if (MonoSingleton<PeakStateController>.instance.active)
				{
					this._maximiseNorm = 1f;
					this._visibleAtAllNorm = 1f;
				}
				CS$<>8__locals1.view.layout.Animate(0.5f, delegate
				{
					CS$<>8__locals1.<>4__this.UpdateLayout(CS$<>8__locals1.view, null);
				});
				if (MonoSingleton<PeakStateController>.instance.active)
				{
					this.Show(MapsViewController.ShowReason.PeakView);
				}
				this.RefreshPromptsUI(false);
			}
			CS$<>8__locals1 = null;
			nextMapToReveal = null;
			mapAppearAnim = null;
		}
		this._darkenFullscreenBG.Animate(0.5f, delegate
		{
			this._darkenFullscreenBG.alpha = 0f;
		}).Then(delegate
		{
			this._darkenFullscreenBG.gameObject.SetActive(false);
		});
		yield break;
	}

	// Token: 0x060009DC RID: 2524 RVA: 0x0005255C File Offset: 0x0005075C
	private void Start()
	{
		this._leftRightPrompts.groupAlpha = 0f;
		this._defaultShadowAlpha = this._backgroundShadow.alpha;
		this._backgroundShadow.alpha = 0f;
		this._toggleMapsPrompt.groupAlpha = 0f;
		this._resetViewPrompt.groupAlpha = 0f;
		this._minimisedMapGlow.alpha = 0f;
		this._bottomGradientUnderPrompts.alpha = 0f;
		this._darkenFullscreenBG.gameObject.SetActive(false);
		this._getMapContinuePrompt.gameObject.SetActive(false);
	}

	// Token: 0x060009DD RID: 2525 RVA: 0x000525FC File Offset: 0x000507FC
	private void OnEnable()
	{
		Game.onUIPositionUpdate += this.OnUIPositionUpdate;
	}

	// Token: 0x060009DE RID: 2526 RVA: 0x00052610 File Offset: 0x00050810
	private void OnDisable()
	{
		this.Hide((MapsViewController.ShowReason)(-1));
		this.nearbyMarkerWithMap = null;
		foreach (MapMarker mapMarker in this._markers)
		{
			mapMarker.ReturnToPool();
		}
		this._markers.Clear();
		foreach (MapMarkerWorld mapMarkerWorld in this._worldMapMarkers.Values)
		{
			if (mapMarkerWorld != null)
			{
				mapMarkerWorld.ReturnToPool(true);
			}
		}
		this._worldMapMarkers.Clear();
		Game.onUIPositionUpdate -= this.OnUIPositionUpdate;
	}

	// Token: 0x060009DF RID: 2527 RVA: 0x000526E8 File Offset: 0x000508E8
	private void Update()
	{
		if (!Game.loaded)
		{
			return;
		}
		int num = (MonoSingleton<JournalController>.instance.visible ? 0 : 1);
		this.canvasGroup.alpha = Mathf.MoveTowards(this.canvasGroup.alpha, (float)num, 2f * Time.unscaledDeltaTime);
		if (this.allowInput && GameInput.prevMap)
		{
			this.SelectPrevNextMap(-1);
		}
		if (this.allowInput && GameInput.nextMap)
		{
			this.SelectPrevNextMap(1);
		}
		if (this.allowShowMapsInput && GameInput.showMaps && !this.isBusy && MonoSingleton<PeakStateController>.instance.active)
		{
			GameInput.ClearInputState();
			if (!this._visibleAndMaximised)
			{
				this.Maximise();
			}
			else
			{
				this.ExitMaximised();
			}
		}
		this._selectedSmoothIdx = Mathf.SmoothDamp(this._selectedSmoothIdx, (float)this.selectedMapIndex, ref this._selectedSmoothIdxSpeed, this._settings.smoothTime, this._settings.maxSpeed);
		this._maximiseNorm = Mathf.MoveTowards(this._maximiseNorm, (float)((this._visibleAndMaximised || this.confirmingMap) ? 1 : 0), Time.deltaTime / this._settings.onOffScreenDuration);
		this._visibleAtAllNorm = Mathf.MoveTowards(this._visibleAtAllNorm, (float)(this.visibleAtAll ? 1 : 0), Time.deltaTime / this._settings.onOffScreenDuration);
		float num2 = 0.5f * (this._leftAreaLayout.canvasWidth - this._leftAreaLayout.width);
		this._leftAreaLayout.x = Mathf.Lerp(num2, 0f, this._maximiseNorm);
		if (!this.layout.isAnimating && !this.confirmingMap)
		{
			this.UpdateLayout(null, null);
		}
		if (this.visibleAtAll && this._leftRightPrompts.groupAlpha > 0f)
		{
			this.<Update>g__UpdatePromptScale|60_0(this._prevMapPrompt);
			this.<Update>g__UpdatePromptScale|60_0(this._nextMapPrompt);
		}
		this.UpdateMinimisedMapGlow();
		this.UpdateSpottingAndMarkerPlacing();
		this._cameraViewportOffsetNorm = Mathf.MoveTowards(this._cameraViewportOffsetNorm, (float)(this.cameraViewportIsOffsetForMaps ? 1 : 0), Time.deltaTime / this._settings.onOffScreenDuration);
		this.nearbyMarkerWithMap = null;
		Prop mapMarkerPropWithVisibleChoices = NarrativePresenter.instance.mapMarkerPropWithVisibleChoices;
		if (mapMarkerPropWithVisibleChoices != null)
		{
			MapMarkerWorld componentInParent = mapMarkerPropWithVisibleChoices.GetComponentInParent<MapMarkerWorld>();
			this.nearbyMarkerWithMap = componentInParent.map;
		}
		if (!this._visibleAndMaximised)
		{
			if (this.nearbyMarkerWithMap != null)
			{
				this.ShowMinimised(MapsViewController.ShowReason.ByMapMarker);
				return;
			}
			this.Hide(MapsViewController.ShowReason.ByMapMarker);
		}
	}

	// Token: 0x060009E0 RID: 2528 RVA: 0x00052954 File Offset: 0x00050B54
	private void UpdateSpottingAndMarkerPlacing()
	{
		bool flag = this.selectedMap != null && Narrative.instance.PlayerHasCompletedMapWithPropName(this.selectedMap.targetInkPropName);
		PeakWidget focussedWidget = MonoSingleton<PeakWidgetController>.instance.focussedWidget;
		bool flag2 = focussedWidget != null && focussedWidget.isDisabled;
		bool flag3 = this._visibleAndMaximised && !this.isBusy && this.selectedMap != null && !flag && MonoSingleton<PeakStateController>.instance.allowInput && MonoSingleton<PeakStateController>.instance.transition == 1f && this._cameraViewportOffsetNorm == 1f;
		float num = (flag2 ? 0.3f : 1f);
		this._confirmMarkerPrompt.groupAlpha = Mathf.MoveTowards(this._confirmMarkerPrompt.groupAlpha, flag3 ? num : 0f, Time.deltaTime / 0.25f);
		bool flag4 = !this.isBusy && MonoSingleton<PeakStateController>.instance.allowInput;
		this._spottingReticule.groupAlpha = Mathf.MoveTowards(this._spottingReticule.groupAlpha, (float)(flag4 ? 1 : 0), Time.deltaTime / 0.25f);
		this._focussingOnOccludedMarkerPastCurrentLevel = false;
		if (flag3)
		{
			if (this._correctlyPlacedMapMarkerNames.Contains(this.selectedMap.targetInkPropName) && this.selectedMap.targetInkPropName == MonoSingleton<PeakStateController>.instance.currentPeakProp.inkListItemName && !Narrative.instance.isBusy)
			{
				GameCamera.instance.peakState.ResetViewToPlayer();
				this.StartMapConfirm(null);
				this.RemoveMarkerForMap(this.selectedMap);
				Narrative.instance.PlayerMarkedSelf(this.selectedMap.targetInkPropName, true);
				return;
			}
			ValueTuple<Vector3, Slope> valueTuple = this.ProjectSpottingMarker();
			Vector3 vector = valueTuple.Item1;
			Object item = valueTuple.Item2;
			if (this._previewMarker == null)
			{
				this._previewMarker = this.CreateMarker(vector, null);
			}
			if (item != null)
			{
				this._previewMarker.worldPos = vector;
			}
			MapsViewController.SpottingTarget spottingTarget = this.ChooseBestSpottingTarget(vector, this.selectedMap);
			if (spottingTarget.correctTargetInReticule || spottingTarget.markingPeak)
			{
				vector = spottingTarget.pos;
			}
			if (spottingTarget.markingPeak)
			{
				this._previewMarker.worldPos = vector;
			}
			float num2 = this._previewMarker.worldPos.z - GameCamera.instance.transform.position.z;
			float num3 = Mathf.InverseLerp(200f, 1500f, num2);
			this._previewMarker.layoutForDistanceScale.scale = Mathf.Lerp(1.2f, 0.35f, num3);
			if (!spottingTarget.playerInReticule && !spottingTarget.markingPeak)
			{
				this._hasMovedReticuleAwayFromSelf = true;
			}
			bool flag5 = false;
			PeakWidget focussedWidget2 = MonoSingleton<PeakWidgetController>.instance.focussedWidget;
			string text;
			if (focussedWidget2 != null)
			{
				if (focussedWidget2.prop.isPathOut && focussedWidget2.map == null)
				{
					text = "Unmapped";
				}
				else if (focussedWidget2.prop.isPeak)
				{
					text = "View in journal";
				}
				else
				{
					text = "View path map";
				}
				this._previewMarkerAlphaScalar = 0f;
				flag5 = true;
			}
			else if (spottingTarget.playerInReticule && this._hasMovedReticuleAwayFromSelf)
			{
				text = "Right here?";
				this._previewMarkerAlphaScalar = 0f;
				flag5 = true;
			}
			else if (spottingTarget.markingPeak && this._hasMovedReticuleAwayFromSelf)
			{
				text = "That peak?";
				this._previewMarkerAlphaScalar = 1f;
				flag5 = true;
			}
			else if (spottingTarget.removeExistingMarker)
			{
				text = "Remove";
				flag5 = true;
				this._previewMarkerAlphaScalar = 0f;
			}
			else
			{
				text = (this._worldMapMarkers.ContainsKey(this.selectedMap) ? "Here instead?" : "Mark spot");
				this._previewMarkerAlphaScalar = 1f;
			}
			if (text != this._confirmMarkerPromptText.text)
			{
				this._confirmMarkerPromptText.text = text;
			}
			float num4 = (flag5 ? 1.1f : 1f);
			this._spottingReticule.scale = Mathf.MoveTowards(this._spottingReticule.scale, num4, Time.deltaTime / 2f);
			float num5 = (flag5 ? 0.25f : 0.1f);
			Color outlineColor = this._spottingReticule.roundRect.outlineColor;
			outlineColor.a = Mathf.MoveTowards(outlineColor.a, num5, Time.deltaTime);
			this._spottingReticule.roundRect.outlineColor = outlineColor;
			if (spottingTarget.removeExistingMarker)
			{
				Vector3 position = GameCamera.instance.transform.position;
				Vector3 normalized = (spottingTarget.pos - position).normalized;
				if (Raycast.RayHitAnyPolyAnyLevel(new Ray(position, normalized), spottingTarget.pos.z - 5f, Level.currentIndex, null) != null)
				{
					this._focussingOnOccludedMarkerPastCurrentLevel = true;
				}
			}
			if (this.allowInput && GameInput.selectMenuItem && focussedWidget2 != null)
			{
				GameInput.ClearInputState();
				if (!flag2)
				{
					MonoSingleton<PeakWidgetController>.instance.ViewWidgetDetail();
				}
			}
			else if (this.allowInput && GameInput.selectMenuItem && !string.IsNullOrEmpty(this.selectedMap.targetInkPropName))
			{
				GameInput.ClearInputState();
				if (!spottingTarget.removeExistingMarker)
				{
					if (spottingTarget.correctTargetInReticule)
					{
						this._previewMarker.worldPos = vector;
					}
					if (spottingTarget.playerInReticule)
					{
						if (spottingTarget.correctTargetInReticule && spottingTarget.isPrimaryTarget)
						{
							this.StartMapConfirm(null);
						}
						Narrative.instance.PlayerMarkedSelf(this.selectedMap.targetInkPropName, spottingTarget.correctTargetInReticule);
						return;
					}
					if (spottingTarget.markingPeak)
					{
						Narrative.instance.PlayerMarkedAnotherPeak(this.selectedMap.targetInkPropName, spottingTarget.peak.inkListItemName, spottingTarget.correctTargetInReticule);
						if (!spottingTarget.correctTargetInReticule || !string.IsNullOrWhiteSpace(this.selectedMap.secondaryTargetInkPropName))
						{
							return;
						}
					}
				}
				this.RemoveMarkerForMap(this.selectedMap);
				if (spottingTarget.removeExistingMarker)
				{
					this.SyncMapMarkersToNarrative();
					return;
				}
				this._previewMarker.map = this.selectedMap;
				this._previewMarker = null;
				this.mapMarkerPositions[this.selectedMap] = vector;
				MapMarkerWorld mapMarkerWorld = this._mapMarkerWorldPrototype.Instantiate<MapMarkerWorld>(null);
				mapMarkerWorld.Setup(vector, this.selectedMap);
				this._worldMapMarkers[this.selectedMap] = mapMarkerWorld;
				this.SyncMapMarkersToNarrative();
				MonoSingleton<Tutorial>.instance.CompleteTutorial(TutorialId.PlaceMarker);
				Narrative.instance.PlayerAddedMapMarker(this.selectedMap.targetInkPropName);
				return;
			}
		}
		if (!flag3 && this._previewMarker != null)
		{
			this._previewMarker.beingRemoved = true;
			this._previewMarker.map = null;
			this._previewMarker = null;
		}
	}

	// Token: 0x060009E1 RID: 2529 RVA: 0x00053008 File Offset: 0x00051208
	private MapsViewController.SpottingTarget ChooseBestSpottingTarget(Vector3 targettingPoint, Map selectedMap)
	{
		MapsViewController.<>c__DisplayClass63_0 CS$<>8__locals1;
		CS$<>8__locals1.<>4__this = this;
		MapsViewController.SpottingTarget spottingTarget = default(MapsViewController.SpottingTarget);
		CS$<>8__locals1.cam = GameCamera.instance.camera;
		CS$<>8__locals1.reticulePosNorm = this.reticulePosNorm;
		float num = float.MaxValue;
		CS$<>8__locals1.aspectRatio = (float)Screen.width / (float)Screen.height;
		int num2 = -1;
		CS$<>8__locals1.minLevelIdx = Level.currentIndex;
		if (MonoSingleton<PeakStateController>.instance.zoomedPastCurrentLevel)
		{
			num2 = Level.currentIndex;
			CS$<>8__locals1.minLevelIdx = Level.currentIndex + 1;
		}
		foreach (Prop prop in Prop.allLoadedPeaks)
		{
			float num3;
			if (!prop.isMinorPeak && prop.levelIdx != num2 && this.<ChooseBestSpottingTarget>g__SpottingTargetScore|63_0(prop.transform.position, out num3, false, ref CS$<>8__locals1))
			{
				num3 *= 0.9f;
				if (num3 < num)
				{
					num = num3;
					spottingTarget = new MapsViewController.SpottingTarget
					{
						markingPeak = true,
						peak = prop,
						pos = prop.transform.position
					};
					if (selectedMap.targetInkPropName == prop.inkListItemName)
					{
						spottingTarget.correctTargetInReticule = true;
						spottingTarget.isPrimaryTarget = true;
					}
				}
			}
		}
		if (!spottingTarget.correctTargetInReticule)
		{
			foreach (Prop prop2 in this.ValidPropTargets(selectedMap))
			{
				if (prop2.levelIdx != num2)
				{
					Vector3 position = prop2.transform.position;
					float num4;
					if (this.<ChooseBestSpottingTarget>g__SpottingTargetScore|63_0(position, out num4, false, ref CS$<>8__locals1))
					{
						num4 *= 0.7f;
						if (num4 < num)
						{
							num = num4;
							spottingTarget = new MapsViewController.SpottingTarget
							{
								correctTargetInReticule = true,
								isPrimaryTarget = (prop2.inkListItemName == selectedMap.targetInkPropName),
								pos = position
							};
							if (prop2.isPeak)
							{
								spottingTarget.markingPeak = true;
								spottingTarget.peak = prop2;
							}
						}
					}
				}
			}
		}
		Vector3 vector;
		float num5;
		if (this.mapMarkerPositions.TryGetValue(selectedMap, out vector) && this.<ChooseBestSpottingTarget>g__SpottingTargetScore|63_0(vector, out num5, true, ref CS$<>8__locals1))
		{
			num5 *= 0.5f;
			if (num5 < num)
			{
				num = num5;
				spottingTarget = new MapsViewController.SpottingTarget
				{
					removeExistingMarker = true,
					pos = vector
				};
			}
		}
		float num6;
		if (!MonoSingleton<PeakStateController>.instance.zoomedPastCurrentLevel && this.<ChooseBestSpottingTarget>g__SpottingTargetScore|63_0(Runner.instance.physicalPosition3d, out num6, false, ref CS$<>8__locals1))
		{
			num6 *= 0.8f;
			if (num6 < num)
			{
				num = num6;
				ValueTuple<bool, Prop, bool> valueTuple = this.PositionIsWithinMapTarget(Runner.instance.physicalPosition3d, selectedMap, 0f, 0f, false);
				bool item = valueTuple.Item1;
				bool item2 = valueTuple.Item3;
				spottingTarget = new MapsViewController.SpottingTarget
				{
					playerInReticule = true,
					pos = Runner.instance.physicalPosition3d,
					correctTargetInReticule = item,
					isPrimaryTarget = item2
				};
			}
		}
		if (!spottingTarget.playerInReticule && !spottingTarget.markingPeak && !spottingTarget.removeExistingMarker && !spottingTarget.correctTargetInReticule)
		{
			spottingTarget = new MapsViewController.SpottingTarget
			{
				pos = targettingPoint
			};
		}
		return spottingTarget;
	}

	// Token: 0x060009E2 RID: 2530 RVA: 0x00053358 File Offset: 0x00051558
	public void SyncMapMarkersToNarrative()
	{
		this._placedMapMarkerNames.Clear();
		this._correctlyPlacedMapMarkerNames.Clear();
		foreach (KeyValuePair<Map, Vector3> keyValuePair in this.mapMarkerPositions)
		{
			Map key = keyValuePair.Key;
			if (!string.IsNullOrEmpty(key.targetInkPropName))
			{
				this._placedMapMarkerNames.Add(key.targetInkPropName);
				if (this.PositionIsWithinMapTarget(keyValuePair.Value, key, 0f, 0f, false).Item1)
				{
					this._correctlyPlacedMapMarkerNames.Add(key.targetInkPropName);
					MonoSingleton<Tutorial>.instance.CompleteTutorial(TutorialId.PlaceCorrectMarker);
				}
			}
		}
		foreach (Map map in Map.all)
		{
			if (map.highlightLookPromptWithoutSpotting && !this._placedMapMarkerNames.Contains(map.targetInkPropName))
			{
				this._placedMapMarkerNames.Add(map.targetInkPropName);
			}
		}
		Narrative.instance.SetPlacedMapMarkerNames(this._placedMapMarkerNames);
		Narrative.instance.SetCorrectlyPlacedMapMarkerNames(this._correctlyPlacedMapMarkerNames);
	}

	// Token: 0x060009E3 RID: 2531 RVA: 0x0005348C File Offset: 0x0005168C
	private ValueTuple<Vector3, Slope> ProjectSpottingMarker()
	{
		Ray ray = Raycast.ViewportPointToRay(this.reticulePosNorm);
		float num = float.MaxValue;
		Vector3 vector = Vector3.zero;
		Slope slope = null;
		float num2 = Mathf.Tan(0.017453292f * GameCamera.instance.fov);
		int num3 = Level.currentIndex;
		if (MonoSingleton<PeakStateController>.instance.zoomedPastCurrentLevel)
		{
			num3 = Level.currentIndex + 1;
		}
		foreach (Level level in Level.activeLevels)
		{
			if (level != Level.current || !MonoSingleton<PeakStateController>.instance.zoomedPastCurrentLevel)
			{
				ValueTuple<Vector2, float> nearbyParamsForRay = Raycast.GetNearbyParamsForRay(level, ray, level.slopes.zRange);
				Vector2 item = nearbyParamsForRay.Item1;
				float item2 = nearbyParamsForRay.Item2;
				List<Slope> list = level.slopes.Nearby(item, Range.infinity, item2, null);
				foreach (MusicRun musicRun in level.musicRuns)
				{
					list.Add(musicRun.slope);
				}
				foreach (Slope slope2 in list)
				{
					int num4 = (int)slope2.transform.position.z;
					float num5 = (float)num4 - ray.origin.z;
					Vector3 vector2 = Raycast.RayIntersectionWithZPlane(ray, (float)num4);
					float num6 = this._settings.markerProjectionMaxRelativeRadius * num5 * num2;
					if (vector2.x >= slope2.leftEdge - num6 && vector2.x <= slope2.rightEdge + num6)
					{
						Vector3 vector3 = slope2.PointIdx(0);
						for (int i = 0; i < slope2.localPoints.Count - 1; i++)
						{
							Vector3 vector4 = slope2.PointIdx(i + 1);
							Vector2 closestPointOnLine = Line.GetClosestPointOnLine(vector3, vector4, vector2, true);
							float num7 = Vector2.Distance(closestPointOnLine, vector2);
							vector3 = vector4;
							if (num7 <= num6)
							{
								float num8 = num7 + this._settings.markerProjectionZWeight * num5;
								if (num8 < num)
								{
									Vector3 vector5 = new Vector3(closestPointOnLine.x, closestPointOnLine.y, (float)num4);
									if (!(Raycast.RayHitAnyPolyAnyLevel(new Ray(ray.origin, (vector5 - ray.origin).normalized), vector5.z, num3, slope2.originPoly) != null))
									{
										num = num8;
										vector = new Vector3(closestPointOnLine.x, closestPointOnLine.y, (float)num4);
										slope = slope2;
									}
								}
							}
						}
					}
				}
			}
		}
		return new ValueTuple<Vector3, Slope>(vector, slope);
	}

	// Token: 0x060009E4 RID: 2532 RVA: 0x000537A8 File Offset: 0x000519A8
	private List<Prop> ValidPropTargets(Map map)
	{
		if (map.targetInkPropName == null || string.IsNullOrWhiteSpace(map.targetInkPropName))
		{
			string[] array = new string[5];
			array[0] = "Map '";
			array[1] = map.name;
			array[2] = "' with sprite '";
			int num = 3;
			Sprite sprite = map.sprite;
			array[num] = ((sprite != null) ? sprite.ToString() : null);
			array[4] = "' had an empty ink prop name so we can't decide whether player found it.";
			Debug.LogError(string.Concat(array), map);
			return null;
		}
		this._validPropsScratch.Clear();
		this._validPropsScratch.AddRange(Prop.GetLoadedPropsByInkName(map.targetInkPropName));
		if (!string.IsNullOrWhiteSpace(map.secondaryTargetInkPropName))
		{
			this._validPropsScratch.AddRange(Prop.GetLoadedPropsByInkName(map.secondaryTargetInkPropName));
		}
		return this._validPropsScratch;
	}

	// Token: 0x060009E5 RID: 2533 RVA: 0x00053860 File Offset: 0x00051A60
	[return: TupleElementNames(new string[] { "found", "prop", "isPrimary" })]
	private ValueTuple<bool, Prop, bool> PositionIsWithinMapTarget(Vector3 position, Map map, float minimumRadius = 0f, float minimumZOffset = 0f, bool primaryOnly = false)
	{
		List<Prop> list = this.ValidPropTargets(map);
		if (list.Count == 0)
		{
			return new ValueTuple<bool, Prop, bool>(false, null, false);
		}
		float num = float.MaxValue;
		Prop prop = null;
		foreach (Prop prop2 in list)
		{
			Vector3 position2 = prop2.transform.position;
			float num2 = Mathf.Max(minimumRadius, map.triggerRadius);
			float num3 = Mathf.Max(Mathf.Max(0.8f, minimumZOffset), prop2.triggerZone.triggerDepthDiff);
			float num4 = Vector2.Distance(position2, position);
			float num5 = Mathf.Abs(position2.z - position.z);
			if (prop2.isActiveAndEnabled && num4 < num2 && num5 < num3 && num4 < num)
			{
				prop = prop2;
				num = num4;
			}
		}
		if (prop == null)
		{
			return new ValueTuple<bool, Prop, bool>(false, null, false);
		}
		return new ValueTuple<bool, Prop, bool>(true, prop, prop.inkListItemName == map.targetInkPropName);
	}

	// Token: 0x060009E6 RID: 2534 RVA: 0x00053978 File Offset: 0x00051B78
	public void RemoveMarkerForMap(Map map)
	{
		MapMarkerWorld mapMarkerWorld;
		if (this._worldMapMarkers.TryGetValue(map, out mapMarkerWorld))
		{
			mapMarkerWorld.ReturnToPool(false);
			this._worldMapMarkers.Remove(map);
		}
		int num = this._markers.FindIndex((MapMarker marker) => marker.map == map);
		if (num != -1)
		{
			this._markers[num].ReturnToPool();
			this._markers.RemoveAt(num);
		}
		this.mapMarkerPositions.Remove(map);
	}

	// Token: 0x060009E7 RID: 2535 RVA: 0x00053A0C File Offset: 0x00051C0C
	public Map StartMapConfirm(Prop mapMarkerProp)
	{
		this.confirmingMap = true;
		Runner.instance.playerControlDisabled |= PlayerControlDisableReason.MapsTransition;
		if (mapMarkerProp != null)
		{
			MapMarkerWorld componentInParent = mapMarkerProp.GetComponentInParent<MapMarkerWorld>();
			this.RemoveMarkerForMap(componentInParent.map);
		}
		this.nearbyMarkerWithMap = null;
		this.SyncMapMarkersToNarrative();
		this.RefreshPromptsUI(false);
		MapView confirmedMapView = this._mapViews[this.selectedMapIndex];
		this._darkenFullscreenBG.CancelAnimations();
		this._darkenFullscreenBG.gameObject.SetActive(true);
		this._darkenFullscreenBG.alpha = 0f;
		this._darkenFullscreenBG.Animate(0.3f, delegate
		{
			this._darkenFullscreenBG.alpha = 0.2f;
		}).ThenAnimate(1f, 3.5f, delegate
		{
			this._darkenFullscreenBG.alpha = 0f;
		}).Then(delegate
		{
			this._darkenFullscreenBG.gameObject.SetActive(false);
		});
		this.layout.Animate(0.5f, delegate
		{
			confirmedMapView.layout.rightX = confirmedMapView.layout.parentRect.width - this._settings.mapAreaOffsetFromTopRight.x;
			confirmedMapView.layout.topY = this._settings.mapAreaOffsetFromTopRight.y;
			confirmedMapView.layout.rotation = 0f;
			this.UpdateLayout(null, confirmedMapView);
		});
		this.layout.Animate(2f, delegate
		{
			confirmedMapView.layout.scale = 1.2f;
		});
		return confirmedMapView.map;
	}

	// Token: 0x060009E8 RID: 2536 RVA: 0x00053B44 File Offset: 0x00051D44
	public IEnumerator EndMapConfirm(bool success)
	{
		if (!this.confirmingMap)
		{
			Debug.LogWarning(string.Format("EndMapConfirm called though we weren't confirming map. MapsViewController.showReason = {0}. selectedMap = {1}", this.showReason, this.selectedMap));
			yield break;
		}
		bool flag = this.selectedMapIndex != -1;
		if (success && flag)
		{
			MapsViewController.<>c__DisplayClass73_0 CS$<>8__locals1 = new MapsViewController.<>c__DisplayClass73_0();
			CS$<>8__locals1.<>4__this = this;
			AudioController.instance.PlaySoundEffect(SoundEffect.Ting);
			CS$<>8__locals1.confirmedMapView = ((this.selectedMapIndex != -1) ? this._mapViews[this.selectedMapIndex] : this._mapViews[0]);
			CS$<>8__locals1.confirmedMapView.foundTick.Animate(0.1f, 0.4f, delegate
			{
				CS$<>8__locals1.confirmedMapView.foundTick.scale = 1.2f;
				CS$<>8__locals1.confirmedMapView.foundTick.groupAlpha = 1f;
			}).ThenAnimate(0.3f, delegate
			{
				CS$<>8__locals1.confirmedMapView.foundTick.scale = 1f;
			});
			AudioController.instance.PlaySting(Sting.Triumphant, -1);
			MonoSingleton<Tutorial>.instance.CompleteTutorial(TutorialId.MapConfirmation);
			yield return new WaitForSeconds(2f);
			int selectedMapIndex = this.selectedMapIndex;
			this._mapViews.Remove(CS$<>8__locals1.confirmedMapView);
			CS$<>8__locals1.confirmedMapView.layout.Animate(0.5f, delegate
			{
				float width = CS$<>8__locals1.confirmedMapView.layout.parentRect.width;
				CS$<>8__locals1.confirmedMapView.layout.rightX = width - CS$<>8__locals1.<>4__this._settings.minimisedMapOffsetFromTopRight.x;
				CS$<>8__locals1.confirmedMapView.layout.topY = CS$<>8__locals1.<>4__this._settings.mapAreaOffsetFromTopRight.y + CS$<>8__locals1.<>4__this._settings.offscreenOffset;
				CS$<>8__locals1.confirmedMapView.layout.rotation = CS$<>8__locals1.<>4__this._settings.hiddenMapAngle;
				CS$<>8__locals1.confirmedMapView.layout.scale = CS$<>8__locals1.<>4__this._settings.hiddenScale;
			}).Then(delegate
			{
				CS$<>8__locals1.confirmedMapView.ReturnToPool();
			});
			if (this._mapViews.Count > 0)
			{
				int num = Mathf.Clamp(selectedMapIndex, 0, this._mapViews.Count - 1);
				this.selectedMap = this._mapViews[num].map;
				this._selectedSmoothIdx = (float)num;
				this.layout.Animate(0.5f, delegate
				{
					this.UpdateLayout(null, null);
				});
			}
			yield return new WaitForSeconds(1f);
			Runner.instance.playerControlDisabled &= ~PlayerControlDisableReason.MapsTransition;
			if (this._mapViews.Count == 0)
			{
				this.selectedMap = null;
			}
			CS$<>8__locals1 = null;
		}
		else
		{
			Runner.instance.playerControlDisabled &= ~PlayerControlDisableReason.MapsTransition;
		}
		this.confirmingMap = false;
		if ((this.showReason & MapsViewController.ShowReason.ByMapMarker) > MapsViewController.ShowReason.None)
		{
			this.Hide(MapsViewController.ShowReason.ByMapMarker);
		}
		if ((this.showReason & MapsViewController.ShowReason.PeakView) > MapsViewController.ShowReason.None && this._mapViews.Count == 0)
		{
			this.Hide(MapsViewController.ShowReason.PeakView);
		}
		yield break;
	}

	// Token: 0x060009E9 RID: 2537 RVA: 0x00053B5C File Offset: 0x00051D5C
	private void SelectPrevNextMap(int direction)
	{
		int num = Mathf.Clamp(this.selectedMapIndex + direction, 0, this._mapViews.Count - 1);
		this.selectedMap = this._mapViews[num].map;
		if (this.selectedMap.isFirstTutorialMap)
		{
			this.RefreshPromptsUI(false);
		}
	}

	// Token: 0x1700027E RID: 638
	// (get) Token: 0x060009EA RID: 2538 RVA: 0x00053BB0 File Offset: 0x00051DB0
	// (set) Token: 0x060009EB RID: 2539 RVA: 0x00053BB8 File Offset: 0x00051DB8
	public Map selectedMap
	{
		get
		{
			return this._selectedMap;
		}
		set
		{
			if (value != this._selectedMap)
			{
				Map selectedMap = this._selectedMap;
				this._selectedMap = value;
				if (this._visibleAndMaximised)
				{
					this.RefreshPromptsUI(false);
				}
			}
		}
	}

	// Token: 0x1700027F RID: 639
	// (get) Token: 0x060009EC RID: 2540 RVA: 0x00053BE8 File Offset: 0x00051DE8
	private int selectedMapIndex
	{
		get
		{
			int num = -1;
			if (this.selectedMap != null)
			{
				for (int i = 0; i < this._mapViews.Count; i++)
				{
					if (this._mapViews[i].map == this.selectedMap)
					{
						num = i;
						break;
					}
				}
			}
			return num;
		}
	}

	// Token: 0x060009ED RID: 2541 RVA: 0x00053C40 File Offset: 0x00051E40
	private void UpdateLayout(MapView restrictToMapView = null, MapView excludeMapView = null)
	{
		if (this._mapViews.Count == 0)
		{
			return;
		}
		float num = this._selectedSmoothIdx;
		if (num < 0f)
		{
			num = 0f;
		}
		int selectedMapIndex = this.selectedMapIndex;
		float zoomTransitionSmoothed = MonoSingleton<PeakStateController>.instance.zoomTransitionSmoothed;
		float width = this._mapViews[0].layout.parent.width;
		float num2 = 1f - Mathf.InverseLerp(1f, 1.6888889f, (float)Screen.width / (float)Screen.height);
		float num3 = Mathf.Lerp(1f, this._settings.minimumSquareAspectScale, num2);
		this._leftAreaLayout.width = 0.5317708f * this._leftAreaLayout.canvasWidth;
		this._mapsStackContainer.scale = num3;
		this._oldMapViews.Clear();
		for (int i = 0; i < this._mapViews.Count; i++)
		{
			MapView mapView = this._mapViews[i];
			if ((!mapView.layout.isAnimating || !(mapView != restrictToMapView)) && (!(restrictToMapView != null) || !(restrictToMapView != mapView)) && (!(excludeMapView != null) || !(excludeMapView == mapView)))
			{
				float num4 = (float)i - num;
				Vector2 vector = Vector2.zero;
				if (num4 != 0f)
				{
					if (num4 >= -1f && num4 < 0f)
					{
						vector = Vector2.Lerp(this._settings.leftFanOffsetStart, Vector2.zero, num4 + 1f);
						float num5 = num4 + 1f;
						Vector2 vector2 = this._settings.tuckOffset * this._settings.tuckCurve.Evaluate(num5);
						vector += vector2;
					}
					else if (num4 > 0f && num4 <= 1f)
					{
						vector = Vector2.Lerp(Vector2.zero, this._settings.rightFanOffsetStart, num4);
					}
					else
					{
						float num6 = Mathf.Log(0.3f * (Mathf.Abs(num4) - 1f) + 1f);
						if (num4 < 0f)
						{
							vector = Vector2.LerpUnclamped(this._settings.leftFanOffsetStart, this._settings.leftFanOffsetEnd, num6);
						}
						else
						{
							vector = Vector2.LerpUnclamped(this._settings.rightFanOffsetStart, this._settings.rightFanOffsetEnd, num6);
						}
					}
				}
				float num7 = 1f;
				if (i == selectedMapIndex)
				{
					num7 = Mathf.Lerp(1f, 1.1f, zoomTransitionSmoothed);
				}
				float num8 = Mathf.Clamp01(1f - Mathf.Abs(num4));
				float num9 = Mathf.Lerp(1f, this._maximiseNorm, num8);
				float num10 = Mathf.Min(this._visibleAtAllNorm, Mathf.Lerp(num8, 1f, this._maximiseNorm));
				float num11 = (float)((this.gettingMaps || (this.confirmingMap && i != selectedMapIndex)) ? 0 : 1);
				float num12 = 1f - Mathf.InverseLerp(3f, 4f, Mathf.Abs(num4));
				if (num10 * num11 * num12 == 0f)
				{
					mapView.gameObject.SetActive(false);
				}
				else
				{
					mapView.gameObject.SetActive(true);
					mapView.layout.groupAlpha = num10 * num11 * num12;
					float num13 = Mathf.Abs(num4);
					if (num13 < 1f)
					{
						mapView.tint = Color.Lerp(Color.white, this._settings.colourTintStart, num13);
					}
					else
					{
						mapView.tint = Color.Lerp(this._settings.colourTintStart, this._settings.colourTintEnd, Mathf.Clamp(num13, 1f, 2f) - 1f);
					}
					float num14 = width - this._settings.minimisedMapOffsetFromTopRight.x;
					float num15 = num14;
					float num16 = width - this._settings.mapAreaOffsetFromTopRight.x + vector.x;
					float num17 = Mathf.SmoothStep(num14, num16, num9);
					num17 = Mathf.SmoothStep(num15, num17, num10);
					mapView.layout.rightX = num17;
					float y = vector.y;
					float y2 = this._settings.minimisedMapOffsetFromTopRight.y;
					float offscreenOffset = this._settings.offscreenOffset;
					float num18 = Mathf.SmoothStep(y2, y, num9);
					num18 = Mathf.SmoothStep(offscreenOffset, num18, num10);
					mapView.layout.topY = this._settings.mapAreaOffsetFromTopRight.y + num18;
					float hiddenMapAngle = this._settings.hiddenMapAngle;
					float minimisedMapAngle = this._settings.minimisedMapAngle;
					float num19 = num4 * this._settings.fanAnglePerMap;
					float num20 = Mathf.SmoothStep(minimisedMapAngle, num19, num9);
					num20 = Mathf.SmoothStep(hiddenMapAngle, num20, num10);
					mapView.layout.rotation = num20;
					float hiddenScale = this._settings.hiddenScale;
					float deselectedScalar = this._settings.deselectedScalar;
					float num21 = 1f * Mathf.Pow(this._settings.fanScalePerMap, Mathf.Abs(num4)) * num7;
					float num22 = Mathf.SmoothStep(deselectedScalar, num21, num9);
					num22 = Mathf.SmoothStep(hiddenScale, num22, num10);
					mapView.layout.scale = num22;
				}
				if ((!this.visibleAtAll || (i != selectedMapIndex && !this._visibleAndMaximised)) && num10 == 0f)
				{
					this._oldMapViews.Add(mapView);
				}
			}
		}
		float num23 = this._selectedSmoothIdx - (float)selectedMapIndex;
		foreach (MapView mapView2 in this._oldMapViews)
		{
			mapView2.ReturnToPool();
			this._mapViews.Remove(mapView2);
		}
		this._oldMapViews.Clear();
		this._selectedSmoothIdx = Mathf.Clamp((float)this.selectedMapIndex + num23, 0f, (float)(this._mapViews.Count - 1));
		if (this._selectedSmoothIdx > 0f)
		{
			int num24 = Mathf.Clamp(Mathf.RoundToInt(this._selectedSmoothIdx + this._settings.mapLayeringZIndexOffset), 0, this._mapViews.Count);
			for (int j = 0; j < num24; j++)
			{
				this._mapViews[j].transform.SetAsLastSibling();
			}
			for (int k = this._mapViews.Count - 1; k >= num24; k--)
			{
				this._mapViews[k].transform.SetAsLastSibling();
			}
		}
	}

	// Token: 0x060009EE RID: 2542 RVA: 0x000542B0 File Offset: 0x000524B0
	private void UpdateMinimisedMapGlow()
	{
		bool flag = (this.showReason & MapsViewController.ShowReason.ByMapMarker) > MapsViewController.ShowReason.None && !this._visibleAndMaximised && this.visibleAtAll && this.selectedMap != null && !this.confirmingMap && this.selectedMap != null;
		if (!flag && MonoSingleton<PeakStateController>.instance.active && !this._visibleAndMaximised && !this.isBusy && !Narrative.instance.isBusy && !MonoSingleton<Tutorial>.instance.HasDone(TutorialId.MapsViewedAtPeak))
		{
			flag = true;
		}
		float num = this._settings.glowBaseAlpha + this._settings.glowPulseAlphaAmount * Mathf.Sin(this._settings.glowPulseSpeed * Time.time);
		float num2 = 1f + this._settings.glowPulseScaleAmount * Mathf.Sin(this._settings.glowPulseSpeed * Time.time);
		float num3 = (float)(flag ? 1 : 0);
		this._glowOpacity = Mathf.MoveTowards(this._glowOpacity, num3, Time.deltaTime);
		num2 = Mathf.Lerp(1f, num2, this._glowOpacity);
		this._minimisedMapGlow.alpha = Mathf.Clamp01(this._settings.glowBaseAlpha * num * this._glowOpacity * this._visibleAtAllNorm);
		this._minimisedMapGlow.scale = num2;
		if (this.selectedMap != null)
		{
			MapView mapView = this._mapViews.Find((MapView v) => v.map == this.selectedMap);
			if (mapView != null)
			{
				this._minimisedMapGlow.center = mapView.layout.ConvertPositionToTarget(mapView.layout.middle, this._minimisedMapGlow.parent);
			}
		}
	}

	// Token: 0x060009EF RID: 2543 RVA: 0x00054458 File Offset: 0x00052658
	private void RefreshPromptsUI(bool immediate = false)
	{
		if (this.selectedMap != null && this.selectedMap.targetInkPropName != null)
		{
			Narrative.instance.PlayerHasCompletedMapWithPropName(this.selectedMap.targetInkPropName);
		}
		int selectedMapIndex = this.selectedMapIndex;
		MapsViewController.<RefreshPromptsUI>g__UpdateLayoutVisible|83_0(this._prevMapPrompt, selectedMapIndex > 0, immediate, 0.2f);
		MapsViewController.<RefreshPromptsUI>g__UpdateLayoutVisible|83_0(this._nextMapPrompt, selectedMapIndex < this._mapViews.Count - 1, immediate, 0.2f);
		bool flag = this.showReason == MapsViewController.ShowReason.ByMapMarker && !this._visibleAndMaximised;
		MapsViewController.<RefreshPromptsUI>g__UpdateLayoutVisible|83_0(this._toggleMapsPrompt, this.visibleAtAll && !flag && !this.isBusy, immediate, 0f);
		MapsViewController.<RefreshPromptsUI>g__UpdateLayoutVisible|83_0(this._resetViewPrompt, this.visibleAtAll && !flag && !this.isBusy, immediate, 0f);
		bool flag2 = this._visibleAndMaximised && !this.isBusy;
		if (this._toggleMapsPromptIsHide != flag2)
		{
			this._toggleMapsPromptIsHide = flag2;
			this._toggleMapsLabel.text = (this._toggleMapsPromptIsHide ? "Hide Maps" : "Show Maps");
			this._promptContainer.Animate(immediate ? 0f : 0.4f, delegate
			{
				if (this._toggleMapsPromptIsHide)
				{
					this._promptContainer.width = this._settings.visiblePromptsContainerWidth;
					this._promptContainer.rightX = this._promptContainer.parentRect.width - this._settings.visiblePromptsOffsetFromRight;
					return;
				}
				this._promptContainer.width = this._settings.hiddenPromptsContainerWidth;
				this._promptContainer.rightX = this._promptContainer.parentRect.width - this._settings.hiddenPromptsOffsetFromRight;
			});
		}
		bool wantLeftRightPrompts = this._toggleMapsPromptIsHide && this._mapViews.Count > 1;
		if ((wantLeftRightPrompts && this._leftRightPrompts.targetGroupAlpha < 1f) || (!wantLeftRightPrompts && this._leftRightPrompts.targetGroupAlpha > 0f))
		{
			this._promptContainer.Animate(immediate ? 0f : 0.4f, delegate
			{
				if (wantLeftRightPrompts)
				{
					this._leftRightPrompts.groupAlpha = 1f;
					return;
				}
				this._leftRightPrompts.groupAlpha = 0f;
			});
		}
		MapsViewController.<RefreshPromptsUI>g__UpdateLayoutVisible|83_0(this._bottomGradientUnderPrompts, this.visibleAtAll, immediate, 0f);
	}

	// Token: 0x060009F0 RID: 2544 RVA: 0x00054644 File Offset: 0x00052844
	public bool PlayerIsInCorrectPrimaryLocationForMap(Map map)
	{
		ValueTuple<bool, Prop, bool> valueTuple = this.PositionIsWithinMapTarget(Runner.instance.physicalPosition3d, map, 0f, 0f, false);
		bool item = valueTuple.Item1;
		bool item2 = valueTuple.Item3;
		return item && item2;
	}

	// Token: 0x060009F1 RID: 2545 RVA: 0x00054680 File Offset: 0x00052880
	public MarkerState GetMarkerState(string inkPropName)
	{
		if (Narrative.instance.PlayerHasCompletedMapWithPropName(inkPropName))
		{
			return MarkerState.unplacedOrNotAMarker;
		}
		if (this._correctlyPlacedMapMarkerNames.Contains(inkPropName))
		{
			return new MarkerState
			{
				inkName = inkPropName,
				correct = true
			};
		}
		if (this._placedMapMarkerNames.Contains(inkPropName))
		{
			return new MarkerState
			{
				inkName = inkPropName,
				correct = false
			};
		}
		return MarkerState.unplacedOrNotAMarker;
	}

	// Token: 0x060009F2 RID: 2546 RVA: 0x000546F5 File Offset: 0x000528F5
	public bool MarkerIsCorrectlyPlaced(Map map)
	{
		return this._correctlyPlacedMapMarkerNames.Contains(map.targetInkPropName);
	}

	// Token: 0x060009F3 RID: 2547 RVA: 0x00054708 File Offset: 0x00052908
	private void OnUIPositionUpdate(GameUI ui)
	{
		this._markers.UpdateAndRemoveIf(delegate(MapMarker marker)
		{
			marker.layout.origin = ui.WorldToCanvas(marker.worldPos, default(Vector2));
			float num = marker.layout.groupAlpha;
			float num2;
			if (marker.beingRemoved || this.confirmingMap)
			{
				num2 = 0f;
			}
			else if (marker == this._previewMarker)
			{
				float num3 = this._settings.previewMarkerAppearZoomRange.InverseLerp(MonoSingleton<PeakStateController>.instance.zoomTransitionSmoothed);
				num2 = 0.5f * num3 * this._previewMarkerAlphaScalar;
			}
			else
			{
				num2 = 1f;
			}
			if (marker == this._previewMarker || (this.selectedMap != null && marker.map == this.selectedMap))
			{
				marker.color = Color.yellow;
			}
			else
			{
				marker.color = Color.white;
			}
			num = Mathf.MoveTowards(num, num2, Time.deltaTime / 0.3f);
			marker.layout.groupAlpha = num;
			if (!marker.layout.isAnimating)
			{
				if (this.selectedMap != null && marker.map == this.selectedMap && marker != this._previewMarker)
				{
					float num4 = 0.5f * (Mathf.Sin(this._settings.markerSelectedBobSpeed * Time.time) + 1f);
					float num5 = this._settings.markerSelectedBobScale - 1f;
					marker.layout.scale = Mathf.Lerp(1f - num5, 1f + num5, num4);
				}
				else
				{
					marker.layout.scale = 1f;
				}
			}
			if (num == 0f && marker.beingRemoved)
			{
				marker.ReturnToPool();
				return true;
			}
			return false;
		});
		this._runnerCanvasPos = ui.WorldToCanvas(Runner.instance.physicalPosition3d, default(Vector2));
	}

	// Token: 0x060009F4 RID: 2548 RVA: 0x00054764 File Offset: 0x00052964
	private MapMarker CreateMarker(Vector3 worldPos, Map map)
	{
		MapMarker marker = this._markerPrototype.Instantiate<MapMarker>(null);
		marker.map = map;
		marker.worldPos = worldPos;
		marker.layout.origin = MonoSingleton<GameUI>.instance.WorldToCanvas(worldPos, default(Vector2));
		marker.layout.groupAlpha = 0f;
		marker.layout.scale = 0.5f;
		marker.layout.Animate(0.3f, 0f, SLayout.popCurve, delegate
		{
			marker.layout.scale = 1f;
		});
		this._markers.Add(marker);
		return marker;
	}

	// Token: 0x060009F5 RID: 2549 RVA: 0x00054834 File Offset: 0x00052A34
	public void RefreshAllWorldMapMarkers()
	{
		foreach (MapMarkerWorld mapMarkerWorld in this._worldMapMarkers.Values)
		{
			mapMarkerWorld.ReturnToPool(true);
		}
		this._worldMapMarkers.Clear();
		foreach (KeyValuePair<Map, Vector3> keyValuePair in this.mapMarkerPositions)
		{
			Map key = keyValuePair.Key;
			Vector3 value = keyValuePair.Value;
			if (WorldManager.instance.GetLevelAtDepth(value.z).isSetup)
			{
				MapMarkerWorld mapMarkerWorld2 = this._mapMarkerWorldPrototype.Instantiate<MapMarkerWorld>(null);
				mapMarkerWorld2.Setup(value, key);
				this._worldMapMarkers[key] = mapMarkerWorld2;
			}
		}
	}

	// Token: 0x06000A00 RID: 2560 RVA: 0x00054A58 File Offset: 0x00052C58
	[CompilerGenerated]
	private void <Update>g__UpdatePromptScale|60_0(SLayout prevNextPrompt)
	{
		float num = 0.5f * (Mathf.Sin(Time.time * this._settings.prevNextPulseSpeed) + 1f);
		if (prevNextPrompt.targetGroupAlpha > 0.5f)
		{
			prevNextPrompt.scale = this._settings.prevNextPulseScale.Lerp(num);
			return;
		}
		prevNextPrompt.scale = Mathf.MoveTowards(prevNextPrompt.scale, 1f, 5f * Time.deltaTime);
	}

	// Token: 0x06000A01 RID: 2561 RVA: 0x00054AD0 File Offset: 0x00052CD0
	[CompilerGenerated]
	private bool <ChooseBestSpottingTarget>g__SpottingTargetScore|63_0(Vector3 pos, out float score, bool ignoreOcclusion, ref MapsViewController.<>c__DisplayClass63_0 A_4)
	{
		score = float.MaxValue;
		Vector3 vector = A_4.cam.WorldToViewportPoint(pos);
		if (vector.x < 0f || vector.x > 1f || vector.y < 0f || vector.y > 1f || vector.z < 0f)
		{
			return false;
		}
		Vector2 vector2 = vector - A_4.reticulePosNorm;
		Vector2 vector3 = new Vector2(vector2.x * 1080f * A_4.aspectRatio, vector2.y * 1080f);
		float magnitude = vector3.magnitude;
		if (magnitude > 125f)
		{
			return false;
		}
		if (!ignoreOcclusion && Raycast.RayHitAnyPolyAnyLevel(Raycast.ViewportPointToRay(A_4.reticulePosNorm), pos.z - 5f, A_4.minLevelIdx, null) != null)
		{
			return false;
		}
		float z = vector.z;
		score = magnitude + this._settings.markerProjectionZWeight * z;
		return true;
	}

	// Token: 0x06000A04 RID: 2564 RVA: 0x00054BEC File Offset: 0x00052DEC
	[CompilerGenerated]
	internal static void <RefreshPromptsUI>g__UpdateLayoutVisible|83_0(SLayout prompt, bool shouldBeVisible, bool immediate, float minAlpha)
	{
		if (shouldBeVisible && prompt.targetGroupAlpha < 1f)
		{
			prompt.Animate(immediate ? 0f : 0.2f, delegate
			{
				prompt.groupAlpha = 1f;
			});
			return;
		}
		if (!shouldBeVisible && prompt.targetGroupAlpha > 0f)
		{
			prompt.Animate(immediate ? 0f : 0.2f, delegate
			{
				prompt.groupAlpha = minAlpha;
			});
		}
	}

	// Token: 0x04000BD2 RID: 3026
	public Dictionary<Map, Vector3> mapMarkerPositions = new Dictionary<Map, Vector3>();

	// Token: 0x04000BD3 RID: 3027
	private CanvasGroup _canvasGroup;

	// Token: 0x04000BD5 RID: 3029
	private Map _nearbyPromptableMap;

	// Token: 0x04000BD6 RID: 3030
	private int _nearbyPromptableMap_lastFrameIdxRun;

	// Token: 0x04000BD7 RID: 3031
	public MapsViewController.ShowReason showReason;

	// Token: 0x04000BD8 RID: 3032
	private SLayout _layout;

	// Token: 0x04000BD9 RID: 3033
	private List<string> _placedMapMarkerNames = new List<string>();

	// Token: 0x04000BDA RID: 3034
	private List<string> _correctlyPlacedMapMarkerNames = new List<string>();

	// Token: 0x04000BDB RID: 3035
	private List<Prop> _validPropsScratch = new List<Prop>(32);

	// Token: 0x04000BDC RID: 3036
	private Map _selectedMap;

	// Token: 0x04000BDD RID: 3037
	private float _selectedSmoothIdx;

	// Token: 0x04000BDE RID: 3038
	private float _selectedSmoothIdxSpeed;

	// Token: 0x04000BDF RID: 3039
	private float _maximiseNorm;

	// Token: 0x04000BE0 RID: 3040
	private float _visibleAtAllNorm;

	// Token: 0x04000BE1 RID: 3041
	private float _cameraViewportOffsetNorm;

	// Token: 0x04000BE2 RID: 3042
	private List<MapView> _mapViews = new List<MapView>(256);

	// Token: 0x04000BE3 RID: 3043
	private List<MapView> _oldMapViews = new List<MapView>(256);

	// Token: 0x04000BE4 RID: 3044
	private bool _visibleAndMaximised;

	// Token: 0x04000BE5 RID: 3045
	private List<Map> _getMapQueue = new List<Map>();

	// Token: 0x04000BE6 RID: 3046
	private float _defaultShadowAlpha;

	// Token: 0x04000BE7 RID: 3047
	private bool _toggleMapsPromptIsHide = true;

	// Token: 0x04000BE8 RID: 3048
	private List<MapMarker> _markers = new List<MapMarker>();

	// Token: 0x04000BE9 RID: 3049
	private Dictionary<Map, MapMarkerWorld> _worldMapMarkers = new Dictionary<Map, MapMarkerWorld>();

	// Token: 0x04000BEA RID: 3050
	private MapMarker _previewMarker;

	// Token: 0x04000BEB RID: 3051
	private float _previewMarkerAlphaScalar = 1f;

	// Token: 0x04000BEC RID: 3052
	private bool _hasMovedReticuleAwayFromSelf;

	// Token: 0x04000BED RID: 3053
	private Vector2 _runnerCanvasPos;

	// Token: 0x04000BEE RID: 3054
	private float _glowOpacity;

	// Token: 0x04000BEF RID: 3055
	private bool _focussingOnOccludedMarkerPastCurrentLevel;

	// Token: 0x04000BF0 RID: 3056
	[SerializeField]
	private Prototype _mapViewPrototype;

	// Token: 0x04000BF1 RID: 3057
	[SerializeField]
	private MapsViewSettings _settings;

	// Token: 0x04000BF2 RID: 3058
	[SerializeField]
	private SLayout _leftRightPrompts;

	// Token: 0x04000BF3 RID: 3059
	[SerializeField]
	private SLayout _mapsStackContainer;

	// Token: 0x04000BF4 RID: 3060
	[SerializeField]
	private SLayout _leftAreaLayout;

	// Token: 0x04000BF5 RID: 3061
	[SerializeField]
	private SLayout _promptContainer;

	// Token: 0x04000BF6 RID: 3062
	[SerializeField]
	private SLayout _prevMapPrompt;

	// Token: 0x04000BF7 RID: 3063
	[SerializeField]
	private SLayout _nextMapPrompt;

	// Token: 0x04000BF8 RID: 3064
	[SerializeField]
	private SLayout _confirmMarkerPrompt;

	// Token: 0x04000BF9 RID: 3065
	[SerializeField]
	private TextMeshProUGUI _confirmMarkerPromptText;

	// Token: 0x04000BFA RID: 3066
	[SerializeField]
	private SLayout _spottingReticule;

	// Token: 0x04000BFB RID: 3067
	[SerializeField]
	private SLayout _toggleMapsPrompt;

	// Token: 0x04000BFC RID: 3068
	[SerializeField]
	private SLayout _resetViewPrompt;

	// Token: 0x04000BFD RID: 3069
	[SerializeField]
	private TextMeshProUGUI _toggleMapsLabel;

	// Token: 0x04000BFE RID: 3070
	[SerializeField]
	private SLayout _backgroundShadow;

	// Token: 0x04000BFF RID: 3071
	[SerializeField]
	private SLayout _darkenFullscreenBG;

	// Token: 0x04000C00 RID: 3072
	[SerializeField]
	private SLayout _bottomGradientUnderPrompts;

	// Token: 0x04000C01 RID: 3073
	[SerializeField]
	private SLayout _minimisedMapGlow;

	// Token: 0x04000C02 RID: 3074
	[SerializeField]
	private SLayout _getMapContinuePrompt;

	// Token: 0x04000C03 RID: 3075
	[SerializeField]
	private Prototype _markerPrototype;

	// Token: 0x04000C04 RID: 3076
	[SerializeField]
	private Prototype _mapMarkerWorldPrototype;

	// Token: 0x04000C05 RID: 3077
	[SerializeField]
	private Prototype _mapConfirmChoicesPrototype;

	// Token: 0x0200035B RID: 859
	[Flags]
	public enum ShowReason
	{
		// Token: 0x0400189E RID: 6302
		None = 0,
		// Token: 0x0400189F RID: 6303
		PeakView = 2,
		// Token: 0x040018A0 RID: 6304
		ByMapMarker = 4
	}

	// Token: 0x0200035C RID: 860
	private struct SpottingTarget
	{
		// Token: 0x040018A1 RID: 6305
		public Prop peak;

		// Token: 0x040018A2 RID: 6306
		public Vector3 pos;

		// Token: 0x040018A3 RID: 6307
		public bool markingPeak;

		// Token: 0x040018A4 RID: 6308
		public bool playerInReticule;

		// Token: 0x040018A5 RID: 6309
		public bool removeExistingMarker;

		// Token: 0x040018A6 RID: 6310
		public bool correctTargetInReticule;

		// Token: 0x040018A7 RID: 6311
		public bool isPrimaryTarget;
	}
}
