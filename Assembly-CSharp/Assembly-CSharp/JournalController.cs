using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000117 RID: 279
public class JournalController : MonoSingleton<JournalController>
{
	// Token: 0x0600094C RID: 2380 RVA: 0x0004DCE8 File Offset: 0x0004BEE8
	public void SetLayoutDirty(JournalSection section)
	{
		this._sectionLayouts.Remove(section);
		if (this.visible && this._section == section)
		{
			this.CalculateLayoutIfNecessary(section);
			this.DestroyPageContent(this._activePageContentBuffer);
			this.CreateDoublePageContent(this._currentLeftPageIdx, this._section, this._activePageContentBuffer);
		}
	}

	// Token: 0x0600094D RID: 2381 RVA: 0x0004DD40 File Offset: 0x0004BF40
	private JournalController.SectionLayout CalculateLayoutIfNecessary(JournalSection section)
	{
		JournalController.SectionLayout sectionLayout;
		if (this._sectionLayouts.TryGetValue(section, out sectionLayout))
		{
			return sectionLayout;
		}
		sectionLayout.pageLayouts = new List<JournalController.PageLayout>();
		this._sectionLayouts[section] = sectionLayout;
		switch (section)
		{
		case JournalSection.Discoveries:
			this.CalculateLayout_Discoveries(sectionLayout);
			break;
		case JournalSection.Peaks:
			this.CalculateLayout_Peaks(sectionLayout);
			break;
		case JournalSection.Inventory:
			this.CalculateLayout_Inventory(sectionLayout);
			break;
		case JournalSection.Maps:
			this.CalculateLayout_Maps(sectionLayout);
			break;
		case JournalSection.System:
			this.CalculateLayout_System(sectionLayout);
			break;
		}
		return sectionLayout;
	}

	// Token: 0x0600094E RID: 2382 RVA: 0x0004DDC0 File Offset: 0x0004BFC0
	private void CalculateLayout_Discoveries(JournalController.SectionLayout sectionLayout)
	{
		JournalController.<>c__DisplayClass2_0 CS$<>8__locals1;
		CS$<>8__locals1.<>4__this = this;
		List<Narrative.InkItemDescription> list = Narrative.instance.ProduceAllDiscoveries();
		float num = this._settings.pageTopMargin;
		float num2 = this._leftPage.rectTransform.rect.size.y - this._settings.pageBottomMargin;
		CS$<>8__locals1.isLeft = true;
		int num3 = 0;
		sectionLayout.AddPage();
		sectionLayout.AddItemToLastPage(new JournalController.ItemLayout
		{
			prototype = this._titleItemPrototype,
			text1 = "Discoveries",
			pos = new Vector2(this.<CalculateLayout_Discoveries>g__X|2_0(ref CS$<>8__locals1) - 5f, num + this._settings.titleYOffset)
		});
		num += this._settings.titleHeight;
		foreach (Narrative.InkItemDescription inkItemDescription in list)
		{
			string text = "<line-indent=80>" + inkItemDescription.description;
			float y = this._discoveryTestItem.text1.textMeshPro.GetPreferredValues(text, this._discoveryTestItem.layout.width, float.MaxValue).y;
			if (num + y > num2)
			{
				num = this._settings.pageTopMargin;
				sectionLayout.AddPage();
				CS$<>8__locals1.isLeft = !CS$<>8__locals1.isLeft;
			}
			sectionLayout.AddItemToLastPage(new JournalController.ItemLayout
			{
				prototype = this._discoveryItemPrototype,
				text1 = "<line-indent=80>" + inkItemDescription.description,
				sprite = this._settings.discoveriesSprites[num3 % this._settings.discoveriesSprites.Length],
				spriteAngle = (float)num3 * 69f,
				pos = new Vector2(this.<CalculateLayout_Discoveries>g__X|2_0(ref CS$<>8__locals1), num),
				height = y,
				inkName = inkItemDescription.inkItemName
			});
			num += y + this._settings.discoveryBetweenPadding;
			num3++;
		}
	}

	// Token: 0x0600094F RID: 2383 RVA: 0x0004DFEC File Offset: 0x0004C1EC
	private void CalculateLayout_Peaks(JournalController.SectionLayout sectionLayout)
	{
		JournalController.<>c__DisplayClass3_0 CS$<>8__locals1;
		CS$<>8__locals1.<>4__this = this;
		List<Narrative.PeakKnowledge> list = Narrative.instance.ProduceAllPeakKnowledge();
		float num = this._settings.pageTopMargin;
		float num2 = this._leftPage.rectTransform.rect.size.y - this._settings.pageBottomMargin;
		CS$<>8__locals1.isLeft = true;
		int num3 = 0;
		sectionLayout.AddPage();
		JournalController.ItemLayout itemLayout = new JournalController.ItemLayout
		{
			prototype = this._titleItemPrototype,
			text1 = "Peaks",
			pos = new Vector2(this.<CalculateLayout_Peaks>g__X|3_0(ref CS$<>8__locals1) + 20f, num + this._settings.titleYOffset)
		};
		sectionLayout.AddItemToLastPage(itemLayout);
		num = this._settings.peaksStartYFirstPage;
		foreach (Narrative.PeakKnowledge peakKnowledge in list)
		{
			if (num + this._settings.peaksItemHeight > num2)
			{
				sectionLayout.AddPage();
				num = ((sectionLayout.pageLayouts.Count >= 3) ? this._settings.peaksStartYSubsequentPages : this._settings.peaksStartYFirstPage);
				CS$<>8__locals1.isLeft = !CS$<>8__locals1.isLeft;
			}
			bool blessed = peakKnowledge.blessed;
			string englishName = peakKnowledge.englishName;
			itemLayout = new JournalController.ItemLayout
			{
				prototype = (CS$<>8__locals1.isLeft ? this._peakLeftItemPrototype : this._peakRightItemPrototype),
				text1 = englishName,
				text2 = peakKnowledge.gaelicName,
				sprite = this._settings.peakIconDatabase.SpriteWithName(peakKnowledge.inkItemName),
				pos = new Vector2(this.<CalculateLayout_Peaks>g__X|3_0(ref CS$<>8__locals1), num),
				height = this._settings.peaksItemHeight,
				ticked = peakKnowledge.correctlyNamed,
				lit = blessed,
				imageFaded = !peakKnowledge.visited,
				inkName = peakKnowledge.inkItemName
			};
			sectionLayout.AddItemToLastPage(itemLayout);
			num += this._settings.peaksItemHeight;
			num3++;
		}
	}

	// Token: 0x06000950 RID: 2384 RVA: 0x0004E230 File Offset: 0x0004C430
	public Sprite IconForInventoryItemNamed(string itemName)
	{
		Sprite sprite = Resources.Load<Sprite>(itemName);
		if (sprite != null)
		{
			return sprite;
		}
		return this._settings.fallbackInventoryIcon;
	}

	// Token: 0x06000951 RID: 2385 RVA: 0x0004E25C File Offset: 0x0004C45C
	private void CalculateLayout_Inventory(JournalController.SectionLayout sectionLayout)
	{
		JournalController.<>c__DisplayClass5_0 CS$<>8__locals1;
		CS$<>8__locals1.<>4__this = this;
		List<Narrative.InkItemDescription> list = Narrative.instance.ProduceAllInventory();
		float num = this._settings.pageTopMargin;
		float num2 = this._leftPage.rectTransform.rect.size.y - this._settings.pageBottomMargin;
		CS$<>8__locals1.isLeft = true;
		int num3 = 0;
		sectionLayout.AddPage();
		sectionLayout.AddItemToLastPage(new JournalController.ItemLayout
		{
			prototype = this._titleItemPrototype,
			text1 = "Items",
			pos = new Vector2(this.<CalculateLayout_Inventory>g__X|5_0(ref CS$<>8__locals1) + 20f, num + this._settings.titleYOffset)
		});
		num += this._settings.titleHeight;
		foreach (Narrative.InkItemDescription inkItemDescription in list)
		{
			if (num + this._settings.inventoryItemHeight > num2)
			{
				num = this._settings.pageTopMargin;
				sectionLayout.AddPage();
				CS$<>8__locals1.isLeft = !CS$<>8__locals1.isLeft;
			}
			sectionLayout.AddItemToLastPage(new JournalController.ItemLayout
			{
				prototype = this._inventoryItemPrototype,
				text1 = inkItemDescription.description,
				sprite = this.IconForInventoryItemNamed(inkItemDescription.inkItemName),
				pos = new Vector2(this.<CalculateLayout_Inventory>g__X|5_0(ref CS$<>8__locals1), num),
				inkName = inkItemDescription.inkItemName
			});
			num += this._settings.inventoryItemHeight;
			num3++;
		}
	}

	// Token: 0x06000952 RID: 2386 RVA: 0x0004E40C File Offset: 0x0004C60C
	private void CalculateMapsOrder()
	{
		this._mapsInOrder.Clear();
		List<Map> maps = MonoSingleton<Inventory>.instance.maps;
		for (int i = 0; i < maps.Count; i++)
		{
			this._mapsInOrder.Add(new JournalController.SortedMap
			{
				map = maps[i],
				index = i,
				playerCompleted = Narrative.instance.PlayerHasCompletedMapWithPropName(maps[i].targetInkPropName)
			});
		}
		this._mapsInOrder.Sort(delegate(JournalController.SortedMap m1, JournalController.SortedMap m2)
		{
			if (m1.playerCompleted == m2.playerCompleted)
			{
				return m1.index - m2.index;
			}
			if (!m1.playerCompleted)
			{
				return -1;
			}
			return 1;
		});
		this.SetLayoutDirty(JournalSection.Maps);
	}

	// Token: 0x06000953 RID: 2387 RVA: 0x0004E4B8 File Offset: 0x0004C6B8
	private void CalculateLayout_Maps(JournalController.SectionLayout sectionLayout)
	{
		JournalController.<>c__DisplayClass7_0 CS$<>8__locals1;
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.isLeft = true;
		sectionLayout.AddPage();
		sectionLayout.AddItemToLastPage(new JournalController.ItemLayout
		{
			prototype = this._titleItemPrototype,
			text1 = "Maps",
			pos = new Vector2(this.<CalculateLayout_Maps>g__X|7_0(ref CS$<>8__locals1) + 20f, this._settings.pageTopMargin + this._settings.titleYOffset)
		});
		bool flag = false;
		foreach (JournalController.SortedMap sortedMap in this._mapsInOrder)
		{
			Map map = sortedMap.map;
			if (flag)
			{
				sectionLayout.AddPage();
			}
			float num = ((sectionLayout.pageLayouts.Count == 1) ? this._settings.mapsFirstPageY : this._settings.mapsY);
			sectionLayout.AddItemToLastPage(new JournalController.ItemLayout
			{
				prototype = this._mapItemPrototype,
				text1 = Narrative.instance.MapDescriptionForJournal(map.targetInkPropName),
				sprite = map.sprite,
				pos = new Vector2(this.<CalculateLayout_Maps>g__X|7_0(ref CS$<>8__locals1), num),
				ticked = Narrative.instance.PlayerHasCompletedMapWithPropName(map.targetInkPropName),
				isMap = true
			});
			CS$<>8__locals1.isLeft = !CS$<>8__locals1.isLeft;
			flag = true;
		}
	}

	// Token: 0x06000954 RID: 2388 RVA: 0x0004E640 File Offset: 0x0004C840
	private void OnSystemItemHighlightChanged(JournalItemView itemView, bool highlighted)
	{
		itemView.text1.textMeshPro.fontSharedMaterial = (highlighted ? this._settings.systemItemHighlightedFontMaterial : this._settings.systemItemHighlightedDefaultFontMaterial);
	}

	// Token: 0x06000955 RID: 2389 RVA: 0x0004E670 File Offset: 0x0004C870
	private void CalculateLayout_System(JournalController.SectionLayout sectionLayout)
	{
		JournalController.<>c__DisplayClass9_0 CS$<>8__locals1 = new JournalController.<>c__DisplayClass9_0();
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.isLeft = true;
		float num = this._settings.pageTopMargin;
		sectionLayout.AddPage();
		JournalController.ItemLayout itemLayout = new JournalController.ItemLayout
		{
			prototype = this._logoPrototype,
			pos = new Vector2(CS$<>8__locals1.<CalculateLayout_System>g__X|0(), this._settings.logoY)
		};
		sectionLayout.AddItemToLastPage(itemLayout);
		num = this._settings.systemOptionsStartY;
		float systemOptionHeight = this._settings.systemOptionHeight;
		if (MonoSingleton<BuildSetupManager>.instance.setup.timeoutAfterInactivity)
		{
			itemLayout = new JournalController.ItemLayout
			{
				prototype = this._systemOptionPrototype,
				text1 = "Restart demo",
				pos = new Vector2(CS$<>8__locals1.<CalculateLayout_System>g__X|0(), num),
				onHighlightChanged = new JournalItemView.OnHighlightChanged(this.OnSystemItemHighlightChanged),
				onSelect = delegate
				{
					Dialogue instance = MonoSingleton<Dialogue>.instance;
					string text2 = "Are you sure you want to restart the demo?";
					string text3 = null;
					string text4 = "Restart";
					string text5 = "Cancel";
					bool flag2 = false;
					Dialogue.OnComplete onComplete;
					if ((onComplete = CS$<>8__locals1.<>9__2) == null)
					{
						onComplete = (CS$<>8__locals1.<>9__2 = delegate(Dialogue.Result result)
						{
							if (result == Dialogue.Result.Primary)
							{
								CS$<>8__locals1.<>4__this.Close();
								Blackout.FadeOut(delegate
								{
									MonoSingleton<Launcher>.instance.DemoReturnToTitleScreenAndRestart();
								});
							}
						});
					}
					instance.Show(text2, text3, text4, text5, flag2, onComplete);
				}
			};
			sectionLayout.AddItemToLastPage(itemLayout);
		}
		else
		{
			itemLayout = new JournalController.ItemLayout
			{
				prototype = this._systemOptionPrototype,
				text1 = "Exit to title screen",
				pos = new Vector2(CS$<>8__locals1.<CalculateLayout_System>g__X|0(), num),
				onHighlightChanged = new JournalItemView.OnHighlightChanged(this.OnSystemItemHighlightChanged),
				onSelect = delegate
				{
					Dialogue instance2 = MonoSingleton<Dialogue>.instance;
					string text6 = "Are you sure you wish to exit?";
					string exitDialogueSaveString = CS$<>8__locals1.<>4__this.exitDialogueSaveString;
					string text7 = "Exit";
					string text8 = "Cancel";
					bool flag3 = false;
					Dialogue.OnComplete onComplete2;
					if ((onComplete2 = CS$<>8__locals1.<>9__5) == null)
					{
						onComplete2 = (CS$<>8__locals1.<>9__5 = delegate(Dialogue.Result result)
						{
							if (result == Dialogue.Result.Primary)
							{
								CS$<>8__locals1.<>4__this.Close();
								Blackout.FadeOut(delegate
								{
									MonoSingleton<Launcher>.instance.ReturnToTitleScreen();
								});
							}
						});
					}
					instance2.Show(text6, exitDialogueSaveString, text7, text8, flag3, onComplete2);
				}
			};
			sectionLayout.AddItemToLastPage(itemLayout);
		}
		num += systemOptionHeight;
		itemLayout = default(JournalController.ItemLayout);
		itemLayout.prototype = this._systemOptionPrototype;
		itemLayout.text1 = "Calibrate music timing";
		itemLayout.pos = new Vector2(CS$<>8__locals1.<CalculateLayout_System>g__X|0(), num);
		itemLayout.onHighlightChanged = new JournalItemView.OnHighlightChanged(this.OnSystemItemHighlightChanged);
		itemLayout.onSelect = delegate
		{
			MonoSingleton<LatencyCalibrator>.instance.Show(LatencyCalibrator.Context.Manual);
		};
		sectionLayout.AddItemToLastPage(itemLayout);
		num += systemOptionHeight;
		bool flag = (Game.instance.inActiveGameplay || Game.instance.inPeakState) && !MonoSingleton<MapsViewController>.instance.isBusy;
		itemLayout = new JournalController.ItemLayout
		{
			prototype = this._systemOptionPrototype,
			text1 = "Photo Mode",
			pos = new Vector2(CS$<>8__locals1.<CalculateLayout_System>g__X|0(), num),
			disabled = !flag,
			onHighlightChanged = new JournalItemView.OnHighlightChanged(this.OnSystemItemHighlightChanged),
			onSelect = delegate
			{
				CS$<>8__locals1.<>4__this.Close();
				MonoSingleton<PhotoMode>.instance.Show();
			}
		};
		sectionLayout.AddItemToLastPage(itemLayout);
		num += systemOptionHeight;
		itemLayout = default(JournalController.ItemLayout);
		itemLayout.prototype = this._systemOptionPrototype;
		itemLayout.text1 = "Settings";
		itemLayout.pos = new Vector2(CS$<>8__locals1.<CalculateLayout_System>g__X|0(), num);
		itemLayout.onHighlightChanged = new JournalItemView.OnHighlightChanged(this.OnSystemItemHighlightChanged);
		itemLayout.onSelect = delegate
		{
			MonoSingleton<SettingsScreen>.instance.Show();
		};
		sectionLayout.AddItemToLastPage(itemLayout);
		num += systemOptionHeight;
		itemLayout = new JournalController.ItemLayout
		{
			prototype = this._systemOptionPrototype,
			text1 = "Credits",
			pos = new Vector2(CS$<>8__locals1.<CalculateLayout_System>g__X|0(), num),
			onHighlightChanged = new JournalItemView.OnHighlightChanged(this.OnSystemItemHighlightChanged),
			onSelect = delegate
			{
				CS$<>8__locals1.<>4__this.Close();
				Credits instance3 = MonoSingleton<Credits>.instance;
				CreditsContext creditsContext = CreditsContext.JournalInGame;
				Action action;
				if ((action = CS$<>8__locals1.<>9__11) == null)
				{
					action = (CS$<>8__locals1.<>9__11 = delegate
					{
						CS$<>8__locals1.<>4__this.Open(true);
					});
				}
				instance3.Show(creditsContext, action);
			}
		};
		sectionLayout.AddItemToLastPage(itemLayout);
		num += systemOptionHeight;
		if (SystemInfo.deviceType == DeviceType.Desktop)
		{
			itemLayout = new JournalController.ItemLayout
			{
				prototype = this._systemOptionPrototype,
				text1 = "Quit",
				pos = new Vector2(CS$<>8__locals1.<CalculateLayout_System>g__X|0(), num),
				onHighlightChanged = new JournalItemView.OnHighlightChanged(this.OnSystemItemHighlightChanged),
				onSelect = delegate
				{
					MonoSingleton<Dialogue>.instance.Show("Are you sure you wish to quit?", CS$<>8__locals1.<>4__this.exitDialogueSaveString, "Quit", "Cancel", false, delegate(Dialogue.Result result)
					{
						if (result == Dialogue.Result.Primary)
						{
							Application.Quit();
						}
					});
				}
			};
			sectionLayout.AddItemToLastPage(itemLayout);
		}
		num += systemOptionHeight;
		string text;
		if (float.IsInfinity(MonoSingleton<Main>.instance.timeSinceLastSave))
		{
			text = "Not yet saved";
		}
		else
		{
			text = "Last saved " + this.timeSinceLastSaveString;
		}
		itemLayout = new JournalController.ItemLayout
		{
			prototype = this._lastSavedPrototype,
			text1 = text,
			pos = new Vector2(CS$<>8__locals1.<CalculateLayout_System>g__X|0(), this._settings.lastSavedY)
		};
		sectionLayout.AddItemToLastPage(itemLayout);
		sectionLayout.AddPage();
		itemLayout = new JournalController.ItemLayout
		{
			prototype = this._fullPageSketchPrototype,
			pos = new Vector2(0f, 50f)
		};
		sectionLayout.AddItemToLastPage(itemLayout);
	}

	// Token: 0x06000956 RID: 2390 RVA: 0x0004EAE4 File Offset: 0x0004CCE4
	private void ClearAllLayouts()
	{
		this._sectionLayouts.Clear();
		this._mapsInOrder.Clear();
	}

	// Token: 0x17000254 RID: 596
	// (get) Token: 0x06000957 RID: 2391 RVA: 0x0004EAFC File Offset: 0x0004CCFC
	private string timeSinceLastSaveString
	{
		get
		{
			float timeSinceLastSave = MonoSingleton<Main>.instance.timeSinceLastSave;
			if (float.IsInfinity(timeSinceLastSave))
			{
				return "never";
			}
			if (timeSinceLastSave < 2f)
			{
				return "just now";
			}
			if (timeSinceLastSave < 30f)
			{
				return "a few seconds ago";
			}
			if (timeSinceLastSave < 90f)
			{
				return "a minute ago";
			}
			if (timeSinceLastSave < 2700f)
			{
				return Mathf.RoundToInt(timeSinceLastSave / 60f).ToString() + " minutes ago";
			}
			return "hours ago";
		}
	}

	// Token: 0x17000255 RID: 597
	// (get) Token: 0x06000958 RID: 2392 RVA: 0x0004EB78 File Offset: 0x0004CD78
	private string exitDialogueSaveString
	{
		get
		{
			if (float.IsInfinity(MonoSingleton<Main>.instance.timeSinceLastSave))
			{
				return "Your game hasn't been saved yet.";
			}
			return "Your game was saved " + this.timeSinceLastSaveString + ".";
		}
	}

	// Token: 0x17000256 RID: 598
	// (get) Token: 0x06000959 RID: 2393 RVA: 0x0004EBA8 File Offset: 0x0004CDA8
	public bool canShowAndNotAlreadyVisible
	{
		get
		{
			return !this.visible && (Game.instance.inActiveGameplay || Game.instance.inPeakState) && (GameInput.controlStack.Count == 0 || this.controlStackItemAllowsPause) && !this._layout.isAnimating && !MonoSingleton<TitleScreen>.instance.visible && !Game.instance.HasTimeScalar(Game.TimeScalar.Tutorial) && !Blackout.showing && !MonoSingleton<MapsViewController>.instance.isBusy && !Runner.instance.inFinalJump && !Game.instance.deathSequenceActive;
		}
	}

	// Token: 0x17000257 RID: 599
	// (get) Token: 0x0600095A RID: 2394 RVA: 0x0004EC3D File Offset: 0x0004CE3D
	public bool controlStackItemAllowsPause
	{
		get
		{
			return GameInput.HasControl(MonoSingleton<RestStateController>.instance) || (GameInput.HasControl(MonoSingleton<PeakStateController>.instance) && !MonoSingleton<PeakStateController>.instance.allowExit);
		}
	}

	// Token: 0x17000258 RID: 600
	// (get) Token: 0x0600095B RID: 2395 RVA: 0x0004EC68 File Offset: 0x0004CE68
	public bool canExit
	{
		get
		{
			return this.visible && this.allowInput;
		}
	}

	// Token: 0x17000259 RID: 601
	// (get) Token: 0x0600095C RID: 2396 RVA: 0x0004EC7A File Offset: 0x0004CE7A
	// (set) Token: 0x0600095D RID: 2397 RVA: 0x0004EC82 File Offset: 0x0004CE82
	public bool visible { get; private set; }

	// Token: 0x1700025A RID: 602
	// (get) Token: 0x0600095E RID: 2398 RVA: 0x0004EC8B File Offset: 0x0004CE8B
	// (set) Token: 0x0600095F RID: 2399 RVA: 0x0004EC93 File Offset: 0x0004CE93
	public float slideOverNorm { get; private set; }

	// Token: 0x1700025B RID: 603
	// (get) Token: 0x06000960 RID: 2400 RVA: 0x0004EC9C File Offset: 0x0004CE9C
	public bool mapZoomingActive
	{
		get
		{
			return this._mapsZoomingActive;
		}
	}

	// Token: 0x1700025C RID: 604
	// (get) Token: 0x06000961 RID: 2401 RVA: 0x0004ECA4 File Offset: 0x0004CEA4
	private bool allowInput
	{
		get
		{
			return this.visible && !this._pendingClose && !this.mapConfirmActive && this._slideOverDir == 0 && GameInput.HasControl(this) && !this._mapsZoomingActive;
		}
	}

	// Token: 0x1700025D RID: 605
	// (get) Token: 0x06000962 RID: 2402 RVA: 0x0004ECD9 File Offset: 0x0004CED9
	private CanvasScaler canvasScaler
	{
		get
		{
			if (this._canvasScaler == null)
			{
				this._canvasScaler = this._layout.canvas.GetComponent<CanvasScaler>();
			}
			return this._canvasScaler;
		}
	}

	// Token: 0x06000963 RID: 2403 RVA: 0x0004ED05 File Offset: 0x0004CF05
	private static int LeftPageIdx(int pageIdx)
	{
		if (pageIdx % 2 == 1)
		{
			return pageIdx - 1;
		}
		return pageIdx;
	}

	// Token: 0x06000964 RID: 2404 RVA: 0x0004ED14 File Offset: 0x0004CF14
	public void Open(bool pressedEscToOpen)
	{
		if (MonoSingleton<RunTrack>.instance.playingOrAboutTo)
		{
			MonoSingleton<RunTrack>.instance.SetPaused(true, RunTrack.PauseReason.Journal);
		}
		Narrative.instance.SetPaused(Narrative.PauseReason.Journal, true);
		MonoSingleton<Main>.instance.TrySave(false);
		PlayerPrefsX.Save();
		this.CalculateMapsOrder();
		this.visible = true;
		JournalController.wantsGameplayPaused = true;
		this.ChooseAutoSectionAndPage(pressedEscToOpen);
		Game.instance.SetTimeScalar(Game.TimeScalar.Journal, 0f);
		float originY = this._defaultOriginYNorm * this._layout.parentRect.height;
		if (!this._layout.isAnimating)
		{
			this._layout.canvas.enabled = true;
			this._layout.scale = this._settings.openCloseMinScale;
			this._layout.originY = originY + this._settings.closeOffsetY;
			this.openCloseHingeAngle = this._settings.maxOpenCloseHingeAngle;
		}
		this._layout.Animate(0.3f, delegate
		{
			this._layout.groupAlpha = 1f;
		});
		this._layout.Animate(0.5f, delegate
		{
			this._layout.scale = 1f;
			this._layout.originY = originY;
			this.openCloseHingeAngle = 0f;
		});
		this._layout.Animate(0.3f, 0.3f, delegate
		{
			this._prompts.groupAlpha = 1f;
		});
		GameInput.PushControlStack(this);
		this._pageTurnActive = false;
		this._timeScalarCoolDownTimer = 0f;
		this._layout.timeScale = 1f;
		this._targetSection = this._section;
		this._slideOverDir = 0;
		this._mapConfirmWasPreviouslyCancelled = false;
		this._leftPage.gameObject.SetActive(true);
		this._rightPage.gameObject.SetActive(true);
		this.CalculateLayoutIfNecessary(this._section);
		this.RefreshTabs();
		if (this._currentLeftPageIdx >= this._sectionLayouts[this._section].pageLayouts.Count)
		{
			this._currentLeftPageIdx = 0;
		}
		this.CreateDoublePageContent(this._currentLeftPageIdx, this._section, this._activePageContentBuffer);
		AudioController.instance.PlayUI(UISound.BookOpenClose);
		Game.onUIPositionUpdate += this.OnUIPositionUpdate;
		MonoSingleton<Tutorial>.instance.JournalWasOpened();
	}

	// Token: 0x06000965 RID: 2405 RVA: 0x0004EF4C File Offset: 0x0004D14C
	public void Close()
	{
		this.visible = false;
		JournalController.wantsGameplayPaused = false;
		GameInput.PopControlStack(this, true);
		this._pageTurnActive = false;
		this._timeScalarCoolDownTimer = 0f;
		this._layout.timeScale = 1f;
		this._targetSection = this._section;
		this._mapConfirmWasPreviouslyCancelled = false;
		this._popMapWhenPossible = null;
		this._layout.CompleteAnimations();
		this._layout.Animate(0.3f, delegate
		{
			this._prompts.groupAlpha = 0f;
		});
		this._layout.Animate(0.3f, 0.2f, delegate
		{
			this._layout.groupAlpha = 0f;
		});
		this._layout.Animate(0.5f, delegate
		{
			this.openCloseHingeAngle = this._settings.maxOpenCloseHingeAngle;
			this._layout.scale = this._settings.openCloseMinScale;
			SLayout.Animatable(delegate(float t)
			{
				this.slideOverNorm = (float)this._slideOverDir * (1f - t);
			});
			if (this._slideOverDir > 0)
			{
				this._layout.x = this._layout.parentRect.width + 100f;
				return;
			}
			if (this._slideOverDir < 0)
			{
				this._layout.rightX = -100f;
				return;
			}
			this._layout.originY = this._defaultOriginYNorm * this._layout.parentRect.height + this._settings.closeOffsetY;
		}).Then(delegate
		{
			this._layout.centerX = 0.5f * this._layout.parentRect.width;
			this._layout.originY = this._defaultOriginYNorm * this._layout.parentRect.height + this._settings.closeOffsetY;
			this._slideOverDir = 0;
			this.slideOverNorm = 0f;
			this.DestroyPageContent(this._activePageContentBuffer);
			this.DestroyPageContent(this._pendingPageContentBuffer);
			this._layout.canvas.enabled = false;
			this._layout.After(0.3f, delegate
			{
				if (!this.visible)
				{
					Game.instance.RemoveTimeScalar(Game.TimeScalar.Journal);
					Narrative.instance.SetPaused(Narrative.PauseReason.Journal, false);
					if (MonoSingleton<RunTrack>.instance.playingOrAboutTo)
					{
						MonoSingleton<RunTrack>.instance.SetPaused(false, RunTrack.PauseReason.Journal);
					}
				}
			});
		});
		this.ClearAllLayouts();
		AudioController.instance.PlayUI(UISound.BookOpenClose);
		Game.onUIPositionUpdate -= this.OnUIPositionUpdate;
		if (!MonoSingleton<RunTrack>.instance.playingOrAboutTo)
		{
			Game.instance.RemoveTimeScalar(Game.TimeScalar.Journal);
		}
		Runner.instance.playerControlDisabled &= ~PlayerControlDisableReason.JournalMapConfirm;
		if (JournalController.onClose != null)
		{
			JournalController.onClose();
		}
	}

	// Token: 0x06000966 RID: 2406 RVA: 0x0004F084 File Offset: 0x0004D284
	public void Clear()
	{
		if (this._mapConfirmCoroutine != null)
		{
			base.StopCoroutine(this._mapConfirmCoroutine);
			this._mapConfirmCoroutine = null;
			GameInput.RemoveControlStackItemEvenIfNotOnTop(this._confirmMapBackStackItem);
			if (this._mapConfirmChoicesWidget != null)
			{
				this._mapConfirmChoicesWidget.prototype.ReturnToPool();
				this._mapConfirmChoicesWidget = null;
			}
			this.mapConfirmActive = false;
		}
		this._popMapWhenPossible = null;
		if (this._slideOverDir != 0)
		{
			this.SetSlideOver(0, -1, null);
		}
		if (this.visible)
		{
			this.Close();
		}
	}

	// Token: 0x06000967 RID: 2407 RVA: 0x0004F10C File Offset: 0x0004D30C
	private void ChooseAutoSectionAndPage(bool pressedEscToOpen)
	{
		this._itemToHighlightOnOpenInkName = null;
		if (pressedEscToOpen)
		{
			this._section = JournalSection.System;
			return;
		}
		if (MonoSingleton<Notifications>.instance.activeNotification != null)
		{
			bool flag = false;
			Notification activeNotification = MonoSingleton<Notifications>.instance.activeNotification;
			if (activeNotification.type == NotificationType.Discovery)
			{
				this._section = JournalSection.Discoveries;
				flag = true;
			}
			else if (activeNotification.type == NotificationType.Item)
			{
				this._section = JournalSection.Inventory;
				flag = true;
			}
			else if (activeNotification.type == NotificationType.Peak)
			{
				this._section = JournalSection.Peaks;
				flag = true;
			}
			else if (activeNotification.type == NotificationType.Stamina)
			{
				this._section = JournalSection.Discoveries;
				flag = true;
			}
			else
			{
				Debug.LogError("Unhandled notification type when opening Journal: " + activeNotification.type.ToString());
			}
			this.CalculateLayoutIfNecessary(this._section);
			if (flag)
			{
				List<JournalController.PageLayout> pageLayouts = this._sectionLayouts[this._section].pageLayouts;
				int num = -1;
				for (int i = 0; i < pageLayouts.Count; i++)
				{
					foreach (JournalController.ItemLayout itemLayout in pageLayouts[i].items)
					{
						if (itemLayout.inkName == activeNotification.inkName)
						{
							this._itemToHighlightOnOpenInkName = itemLayout.inkName;
							num = i;
							break;
						}
					}
					if (num != -1)
					{
						break;
					}
				}
				if (num == -1)
				{
					Debug.LogError("Ink item " + activeNotification.inkName + " not found in Journal, so it's not possible to turn to the correct page");
				}
				else
				{
					this._currentLeftPageIdx = JournalController.LeftPageIdx(num);
				}
				MonoSingleton<Notifications>.instance.CompleteCurrent();
				return;
			}
		}
		else if (MonoSingleton<PeakWidgetController>.instance.focussedWidget != null)
		{
			PeakWidget widget = MonoSingleton<PeakWidgetController>.instance.focussedWidget;
			if (widget.prop.isPathOut)
			{
				if (widget.map != null)
				{
					this._section = JournalSection.Maps;
					this._currentLeftPageIdx = JournalController.LeftPageIdx(this._mapsInOrder.FindIndex((JournalController.SortedMap sortedMap) => sortedMap.map == widget.map));
					this._popMapWhenPossible = widget.map;
					return;
				}
			}
			else if (widget.prop.isPeak)
			{
				this.CalculateLayoutIfNecessary(JournalSection.Peaks);
				this._section = JournalSection.Peaks;
				string inkListItemName = widget.prop.inkListItemName;
				this._currentLeftPageIdx = 0;
				List<JournalController.PageLayout> pageLayouts2 = this._sectionLayouts[JournalSection.Peaks].pageLayouts;
				for (int j = 0; j < pageLayouts2.Count; j++)
				{
					using (List<JournalController.ItemLayout>.Enumerator enumerator = pageLayouts2[j].items.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.inkName == inkListItemName)
							{
								this._currentLeftPageIdx = JournalController.LeftPageIdx(j);
								this._itemToHighlightOnOpenInkName = inkListItemName;
								return;
							}
						}
					}
				}
				return;
			}
		}
		else if (!Narrative.instance.isBusy || Narrative.instance.canInterrupt)
		{
			Map nearbyMap = MonoSingleton<MapsViewController>.instance.nearbyMarkerWithMap;
			if (nearbyMap == null)
			{
				Map inVicinityPromptableMap = MonoSingleton<MapsViewController>.instance.inVicinityPromptableMap;
				if (inVicinityPromptableMap != null && MonoSingleton<MapsViewController>.instance.PlayerIsInCorrectPrimaryLocationForMap(inVicinityPromptableMap))
				{
					nearbyMap = inVicinityPromptableMap;
				}
			}
			if (nearbyMap != null)
			{
				this._section = JournalSection.Maps;
				this._currentLeftPageIdx = JournalController.LeftPageIdx(this._mapsInOrder.FindIndex((JournalController.SortedMap sortedMap) => sortedMap.map == nearbyMap));
			}
		}
	}

	// Token: 0x06000968 RID: 2408 RVA: 0x0004F4B8 File Offset: 0x0004D6B8
	private void Start()
	{
		this._layout.canvas.enabled = false;
		this._layout.groupAlpha = 0f;
		this._prompts.groupAlpha = 0f;
		this._activePageContentBuffer = this._contentBufferPrototype.Instantiate<JournalContentBuffer>(null);
		this._pendingPageContentBuffer = this._contentBufferPrototype.Instantiate<JournalContentBuffer>(null);
		this._leftPage.texture = this._activePageContentBuffer.renderTexture;
		this._rightPage.texture = this._activePageContentBuffer.renderTexture;
		this._pendingLeftPage.texture = this._pendingPageContentBuffer.renderTexture;
		this._pendingRightPage.texture = this._pendingPageContentBuffer.renderTexture;
		this.SetPageRotation(true, 0f, true);
		this._pendingLeftPage.gameObject.SetActive(false);
		this._pendingRightPage.gameObject.SetActive(false);
		this._discoveryTestItem = this._discoveryItemPrototype.Instantiate<JournalItemView>(null);
		this._discoveryTestItem.gameObject.SetActive(false);
		this._defaultOriginYNorm = this._layout.originY / this._layout.parentRect.height;
		this._section = (this._targetSection = JournalSection.Discoveries);
		this._currentLeftPageIdx = 0;
		this._discoveriesTabOriginalY = this._discoveriesTab.y;
		this._peaksTabOriginalY = this._peaksTab.y;
		this._itemsTabOriginalY = this._itemsTab.y;
		this._mapsTabOriginalY = this._mapsTab.y;
		this._systemTabOriginalX = this._systemTab.x;
		this._darkenBackgroundForMapZooming.gameObject.SetActive(false);
		this._lookCloserPrompt.groupAlpha = 0f;
	}

	// Token: 0x06000969 RID: 2409 RVA: 0x0004F678 File Offset: 0x0004D878
	private void Update()
	{
		if (this.canShowAndNotAlreadyVisible)
		{
			bool showJournal = GameInput.showJournal;
			bool flag = GameInput.activeInputType == GameInput.InputType.KeyboardAndMouse && (GameInput.Back(null) || GameInput.Back(MonoSingleton<PeakStateController>.instance)) && !NarrativePresenter.instance.showingHideableChoiceWidget;
			if (GameInput.showJournal || flag)
			{
				GameInput.ClearInputState();
				this.Open(flag);
			}
		}
		if (this.allowInput)
		{
			if ((GameInput.selectMenuItem || GameInput.zoomInPressed) && this._section == JournalSection.Maps && this._mapsInOrder.Count > 0 && !this._pageTurnActive && !this._mapsZoomingActive)
			{
				GameInput.ClearInputState();
				base.StartCoroutine(this.SetMapsZoomed_Coroutine());
			}
			else if (GameInput.Back(this) || GameInput.showJournal)
			{
				GameInput.ClearInputState();
				this._pendingClose = true;
				this._pendingPageTurnDir = 0;
				this._targetSection = this._section;
				if (this._pageTurnActive && this._layout.timeScale < 2f)
				{
					this._layout.timeScale = 2f;
					this._timeScalarCoolDownTimer = 2f;
				}
			}
			if (GameInput.selectLeft)
			{
				GameInput.ClearInputState();
				this.TurnPage(-1);
			}
			else if (GameInput.selectRight)
			{
				GameInput.ClearInputState();
				this.TurnPage(1);
			}
			else if (GameInput.selectUp)
			{
				GameInput.ClearInputState();
				this.SetItemHighlighted(-1);
			}
			else if (GameInput.selectDown)
			{
				GameInput.ClearInputState();
				this.SetItemHighlighted(1);
			}
			else if (GameInput.selectMenuItem && this._selectedItem != null)
			{
				GameInput.ClearInputState();
				AudioController.instance.PlayUI(UISound.MajorClick);
				this._selectedItem.onSelect();
			}
			else if (GameInput.nextSection)
			{
				GameInput.ClearInputState();
				this.TryChangeSection(this._targetSection + 1);
			}
			else if (GameInput.prevSection)
			{
				GameInput.ClearInputState();
				this.TryChangeSection(this._targetSection - 1);
			}
			else
			{
				for (int i = 0; i <= 4; i++)
				{
					if (i != (int)this._section && GameInput.SelectNumIdx(i))
					{
						GameInput.ClearInputState();
						this.TryChangeSection((JournalSection)i);
					}
				}
			}
			if (this._targetSection != this._section && !this._pageTurnActive)
			{
				this._pendingPageTurnDir = 0;
				this.TurnTo(this._targetSection, 0);
			}
			if (!this._pageTurnActive && this._pendingPageTurnDir != 0)
			{
				int pendingPageTurnDir = this._pendingPageTurnDir;
				this._pendingPageTurnDir = 0;
				this.TurnPage(pendingPageTurnDir);
			}
		}
		else
		{
			this._targetSection = this._section;
		}
		if (this._timeScalarCoolDownTimer > 0f)
		{
			this._timeScalarCoolDownTimer -= Time.unscaledDeltaTime;
			if (this._timeScalarCoolDownTimer <= 0f)
			{
				this._layout.timeScale = 1f;
				this._timeScalarCoolDownTimer = 0f;
			}
		}
		if (this._pendingClose && !this._pageTurnActive && !this._layout.isAnimating)
		{
			this._pendingClose = false;
			this.Close();
		}
		Vector2 canvasSize = this._layout.canvasSize;
		this.canvasScaler.matchWidthOrHeight = (float)((canvasSize.x / canvasSize.y < 1.3333334f) ? 0 : 1);
		if (this.visible && this._section == JournalSection.Maps && !this._pageTurnActive && !this.mapConfirmActive && this._slideOverDir == 0 && !this._mapConfirmWasPreviouslyCancelled && this._mapsInOrder.Count > 0 && (!Narrative.instance.isBusy || Narrative.instance.canInterrupt) && Runner.instance.running)
		{
			int currentLeftPageIdx = this._currentLeftPageIdx;
			Map map = this._mapsInOrder[currentLeftPageIdx].map;
			int num = this._currentLeftPageIdx + 1;
			Map map2 = ((num < this._sectionLayouts[this._section].pageLayouts.Count) ? this._mapsInOrder[num].map : null);
			if (MonoSingleton<MapsViewController>.instance.PlayerIsInCorrectPrimaryLocationForMap(map) && !Narrative.instance.PlayerHasCompletedMapWithPropName(map.targetInkPropName))
			{
				this.SetSlideOver(1, this._currentLeftPageIdx, map);
			}
			else if (map2 != null && MonoSingleton<MapsViewController>.instance.PlayerIsInCorrectPrimaryLocationForMap(map2) && !Narrative.instance.PlayerHasCompletedMapWithPropName(map2.targetInkPropName))
			{
				this.SetSlideOver(-1, this._currentLeftPageIdx + 1, map2);
			}
		}
		if (this._popMapWhenPossible != null && this.visible && !this._layout.isAnimating)
		{
			JournalItemView mapItemView = null;
			foreach (JournalItemView journalItemView in this._activePageContentBuffer.content)
			{
				SLayout image = journalItemView.image;
				Object @object;
				if (image == null)
				{
					@object = null;
				}
				else
				{
					Image image2 = image.image;
					@object = ((image2 != null) ? image2.sprite : null);
				}
				if (@object == this._popMapWhenPossible.sprite)
				{
					mapItemView = journalItemView;
					break;
				}
			}
			if (mapItemView != null)
			{
				mapItemView.image.Animate(0.3f, 0f, SLayout.thereAndBack, delegate
				{
					mapItemView.image.scale = 1.1f;
				});
			}
			this._popMapWhenPossible = null;
		}
	}

	// Token: 0x0600096A RID: 2410 RVA: 0x0004FBCC File Offset: 0x0004DDCC
	private void SetSlideOver(int dir, int pageIdx, Map map)
	{
		this._slideOverDir = dir;
		Action<float> <>9__1;
		this._layout.Animate(0.5f, (dir == 0) ? 0f : 0.5f, delegate
		{
			float num = 0.5f * this._layout.parentRect.width;
			if (dir == 0)
			{
				this._layout.centerX = num;
				this._prompts.groupAlpha = 1f;
			}
			else if (dir > 0)
			{
				this._layout.centerX = this._layout.parentRect.width - this._settings.slideOverDistFromEdge;
				this._prompts.groupAlpha = 0f;
			}
			else
			{
				this._layout.centerX = this._settings.slideOverDistFromEdge;
				this._prompts.groupAlpha = 0f;
			}
			Action<float> action;
			if ((action = <>9__1) == null)
			{
				action = (<>9__1 = delegate(float t)
				{
					this.slideOverNorm = (float)this._slideOverDir * t;
				});
			}
			SLayout.Animatable(action);
		});
		Game.instance.SetTimeScalar(Game.TimeScalar.Journal, (float)((this._slideOverDir == 0) ? 0 : 1));
		Runner.instance.playerControlDisabled |= PlayerControlDisableReason.JournalMapConfirm;
		if (this._slideOverDir == 0)
		{
			Runner.instance.playerControlDisabled &= ~PlayerControlDisableReason.JournalMapConfirm;
			return;
		}
		Runner.instance.momentum = 0f;
		Runner.instance.animator.SetAnimation("Idle", FrameAnimator.PosMatch.None);
		this._mapConfirmCoroutine = this.ConfirmMapDuringSlideOver(map, pageIdx);
		base.StartCoroutine(this._mapConfirmCoroutine);
	}

	// Token: 0x0600096B RID: 2411 RVA: 0x0004FCB6 File Offset: 0x0004DEB6
	private IEnumerator SetMapsZoomed_Coroutine()
	{
		this._mapsZoomingActive = true;
		GameInput.PushControlStack(this._mapsZoomedBackItem);
		SLayout leftMap = null;
		SLayout rightMap = null;
		bool leftMapIsFirst = this._currentLeftPageIdx == 0;
		JournalController.PageLayout pageLayout = this._sectionLayouts[this._section].pageLayouts[this._currentLeftPageIdx];
		foreach (JournalItemView journalItemView in this._activePageContentBuffer.content)
		{
			if (journalItemView.isMap)
			{
				journalItemView.image.transform.GetSiblingIndex();
				if (leftMap == null)
				{
					leftMap = journalItemView.image;
				}
				else
				{
					rightMap = journalItemView.image;
				}
			}
		}
		float midX = 0.5f * this._layout.width;
		SLayout leftMapZoomed = null;
		SLayout rightMapZoomed = null;
		if (leftMap != null)
		{
			leftMapZoomed = this._mapPrototypeForZooming.Instantiate<SLayout>(null);
			leftMapZoomed.image.sprite = leftMap.image.sprite;
			leftMapZoomed.center = new Vector2(midX - this._settings.mapZoomLeftX, leftMapIsFirst ? this._settings.mapZoomLeftFirstY : this._settings.mapZoomStartY);
			leftMap.gameObject.SetActive(false);
		}
		if (rightMap != null)
		{
			rightMapZoomed = this._mapPrototypeForZooming.Instantiate<SLayout>(null);
			rightMapZoomed.image.sprite = rightMap.image.sprite;
			rightMapZoomed.center = new Vector2(midX + this._settings.mapZoomRightX, this._settings.mapZoomStartY);
			rightMap.gameObject.SetActive(false);
		}
		this._darkenBackgroundForMapZooming.gameObject.SetActive(true);
		this._darkenBackgroundForMapZooming.alpha = 0f;
		this._layout.Animate(0.3f, delegate
		{
			if (leftMapZoomed != null)
			{
				leftMapZoomed.scale = 2f;
				if (rightMapZoomed != null)
				{
					leftMapZoomed.centerX -= this._settings.mapZoomOffsetX;
				}
				else
				{
					leftMapZoomed.centerX = midX;
				}
				leftMapZoomed.centerY = this._settings.mapZoomY;
			}
			if (rightMapZoomed != null)
			{
				rightMapZoomed.scale = 2f;
				rightMapZoomed.centerX += this._settings.mapZoomOffsetX;
				rightMapZoomed.centerY = this._settings.mapZoomY;
			}
			this._darkenBackgroundForMapZooming.alpha = 0.45f;
			this._prompts.groupAlpha = 0f;
			this._lookCloserPrompt.groupAlpha = 0f;
		});
		while (!GameInput.Back(this._mapsZoomedBackItem) && !GameInput.selectMenuItem)
		{
			yield return null;
		}
		GameInput.ClearInputState();
		midX = 0.5f * this._layout.width;
		SLayoutAnimation anim = this._layout.Animate(0.3f, delegate
		{
			if (leftMapZoomed != null)
			{
				leftMapZoomed.scale = 1f;
				leftMapZoomed.center = new Vector2(midX - this._settings.mapZoomLeftX, leftMapIsFirst ? this._settings.mapZoomLeftFirstY : this._settings.mapZoomStartY);
			}
			if (rightMapZoomed != null)
			{
				rightMapZoomed.scale = 1f;
				rightMapZoomed.center = new Vector2(midX + this._settings.mapZoomRightX, this._settings.mapZoomStartY);
			}
			this._darkenBackgroundForMapZooming.alpha = 0f;
			this._prompts.groupAlpha = 1f;
			this._lookCloserPrompt.groupAlpha = 1f;
		}).Then(delegate
		{
			if (leftMapZoomed != null)
			{
				leftMapZoomed.GetComponent<Prototype>().ReturnToPool();
			}
			if (rightMapZoomed != null)
			{
				rightMapZoomed.GetComponent<Prototype>().ReturnToPool();
			}
			this._darkenBackgroundForMapZooming.gameObject.SetActive(false);
		});
		while (!anim.isComplete)
		{
			yield return null;
		}
		if (leftMap != null)
		{
			leftMap.gameObject.SetActive(true);
		}
		if (rightMap != null)
		{
			rightMap.gameObject.SetActive(true);
		}
		GameInput.PopControlStack(this._mapsZoomedBackItem, true);
		this._mapsZoomingActive = false;
		yield break;
	}

	// Token: 0x0600096C RID: 2412 RVA: 0x0004FCC5 File Offset: 0x0004DEC5
	public void EndMapConfirm()
	{
		this.mapConfirmActive = false;
	}

	// Token: 0x0600096D RID: 2413 RVA: 0x0004FCCE File Offset: 0x0004DECE
	private IEnumerator ConfirmMapDuringSlideOver(Map map, int pageIdx)
	{
		JournalController.<>c__DisplayClass56_0 CS$<>8__locals1 = new JournalController.<>c__DisplayClass56_0();
		CS$<>8__locals1.<>4__this = this;
		this.mapConfirmActive = true;
		GameInput.PushControlStack(this._confirmMapBackStackItem);
		if (!Narrative.instance.isBusy)
		{
			NarrativePresenter.instance.Clear(false, false);
		}
		else
		{
			Narrative.instance.ClearForInterrupt();
		}
		GameChoice gameChoice;
		if (!Narrative.instance.TryGetMapConfirmChoice(map.targetInkPropName, true, out gameChoice))
		{
			Narrative.instance.RefreshInteractablesChoices(true, false);
		}
		Narrative.instance.SetPaused(Narrative.PauseReason.Journal, false);
		Narrative.instance.blockedForJournalChoice = true;
		yield return new WaitForSeconds(1f);
		JournalController.PageLayout pageLayout = this._sectionLayouts[this._section].pageLayouts[pageIdx];
		CS$<>8__locals1.mapItemView = null;
		foreach (JournalItemView journalItemView in this._activePageContentBuffer.content)
		{
			SLayout image = journalItemView.image;
			Object @object;
			if (image == null)
			{
				@object = null;
			}
			else
			{
				Image image2 = image.image;
				@object = ((image2 != null) ? image2.sprite : null);
			}
			if (@object == map.sprite)
			{
				CS$<>8__locals1.mapItemView = journalItemView;
				break;
			}
		}
		CS$<>8__locals1.originalMapViewSiblingIdx = CS$<>8__locals1.mapItemView.image.transform.GetSiblingIndex();
		CS$<>8__locals1.mapItemView.image.transform.SetAsLastSibling();
		CS$<>8__locals1.mapItemView.image.Animate(1f, delegate
		{
			CS$<>8__locals1.mapItemView.image.scale = 1.5f;
		});
		GameChoice choice;
		if (!Narrative.instance.TryGetMapConfirmChoice(map.targetInkPropName, true, out choice))
		{
			Debug.LogError("Expected map confirm choice not found!");
			CS$<>8__locals1.<ConfirmMapDuringSlideOver>g__CancelMapConfirm|1();
			yield break;
		}
		this._mapConfirmChoicesWidget = this._mapConfirmChoicesPrototype.Instantiate<ChoicesWidget>(null);
		this._mapConfirmChoicesWidget.SetChoices(new List<GameChoice> { choice }, 1, false, false);
		this._mapConfirmChoicesWidget.layout.groupAlpha = 0f;
		this._mapConfirmChoicesWidget.layout.Animate(0.3f, delegate
		{
			CS$<>8__locals1.<>4__this._mapConfirmChoicesWidget.layout.groupAlpha = 1f;
		});
		while (!GameInput.Back(this._confirmMapBackStackItem))
		{
			if (GameInput.selectChoice)
			{
				MonoSingleton<MapsViewController>.instance.RemoveMarkerForMap(map);
				Narrative.instance.blockedForJournalChoice = false;
				Narrative.instance.SetConfirmedMap(map.targetInkPropName);
				Narrative.instance.ChooseMapConfirmChoice(choice);
				CS$<>8__locals1.<ConfirmMapDuringSlideOver>g__RemoveConfirmChoiceWidget|2();
				while (this.mapConfirmActive)
				{
					yield return null;
				}
				CS$<>8__locals1.mapItemView.SetTicked(true);
				CS$<>8__locals1.mapItemView.tick.transform.SetAsLastSibling();
				AudioController.instance.PlaySoundEffect(SoundEffect.Ting);
				yield return new WaitForSeconds(0.5f);
				CS$<>8__locals1.mapItemView.image.Animate(0.5f, delegate
				{
					CS$<>8__locals1.mapItemView.image.scale = 1f;
				});
				while (this._layout.isAnimating)
				{
					yield return null;
				}
				CS$<>8__locals1.mapItemView.image.transform.SetSiblingIndex(CS$<>8__locals1.originalMapViewSiblingIdx);
				GameInput.PopControlStack(this._confirmMapBackStackItem, true);
				this.Close();
				yield break;
			}
			yield return null;
		}
		CS$<>8__locals1.<ConfirmMapDuringSlideOver>g__CancelMapConfirm|1();
		yield break;
	}

	// Token: 0x0600096E RID: 2414 RVA: 0x0004FCEB File Offset: 0x0004DEEB
	private void TryChangeSection(JournalSection newSection)
	{
		if (newSection < JournalSection.Discoveries || newSection > JournalSection.System)
		{
			if (!this._pageTurnActive)
			{
				this.FailedPageTurnEffect(newSection - this._section);
			}
			return;
		}
		this._targetSection = newSection;
		this.RefreshTabs();
		if (this._pageTurnActive)
		{
			this.TurnFaster();
		}
	}

	// Token: 0x0600096F RID: 2415 RVA: 0x0004FD28 File Offset: 0x0004DF28
	private void TurnFaster()
	{
		float num = this._layout.timeScale + 0.5f;
		if (num <= 6f)
		{
			this._layout.timeScale = num;
		}
		this._timeScalarCoolDownTimer = 1f;
	}

	// Token: 0x06000970 RID: 2416 RVA: 0x0004FD68 File Offset: 0x0004DF68
	private void FailedPageTurnEffect(int dir)
	{
		this._pageTurnActive = true;
		this._layout.timeScale = 1f;
		this._timeScalarCoolDownTimer = 0f;
		this._layout.AnimateCustom(0.3f, delegate(float t)
		{
			this.SetPageRotation(dir > 0, 0.1f * Mathf.Sin(t * 3.1415927f), true);
		}).Then(delegate
		{
			this._pageTurnActive = false;
		});
	}

	// Token: 0x06000971 RID: 2417 RVA: 0x0004FDDC File Offset: 0x0004DFDC
	private void TurnPage(int dir)
	{
		if (this._pageTurnActive)
		{
			this.TurnFaster();
			this._pendingPageTurnDir = dir;
			return;
		}
		int num = this._sectionLayouts[this._section].pageLayouts.Count;
		int num2 = this._currentLeftPageIdx + 2 * dir;
		JournalSection journalSection = this._section;
		if ((num2 < 0 && this._section == JournalSection.Discoveries) || (num2 >= num && this._section == JournalSection.System))
		{
			this.FailedPageTurnEffect(dir);
			return;
		}
		if (num2 < 0 && this._section > JournalSection.Discoveries)
		{
			journalSection = this._section - 1;
			this.CalculateLayoutIfNecessary(journalSection);
			num = this._sectionLayouts[journalSection].pageLayouts.Count;
			if (num % 2 == 0)
			{
				num2 = Mathf.Max(num - 2, 0);
			}
			else
			{
				num2 = Mathf.Max(num - 1, 0);
			}
		}
		else if (num2 >= num && this._section < JournalSection.System)
		{
			journalSection = this._section + 1;
			num2 = 0;
		}
		this.TurnTo(journalSection, num2);
	}

	// Token: 0x06000972 RID: 2418 RVA: 0x0004FEC0 File Offset: 0x0004E0C0
	private void SetItemHighlighted(int upDownDir)
	{
		int num = -1;
		if (this._selectedItem != null)
		{
			num = this._activePageContentBuffer.content.IndexOf(this._selectedItem);
		}
		int num2 = -1;
		int num3 = num + upDownDir;
		while (num3 >= 0 && num3 < this._activePageContentBuffer.content.Count)
		{
			if (this._activePageContentBuffer.content[num3].onHighlightChanged != null && !this._activePageContentBuffer.content[num3].disabled)
			{
				num2 = num3;
				break;
			}
			num3 += upDownDir;
		}
		if (num2 != num && num2 != -1 && this._selectedItem != null)
		{
			AudioController.instance.PlayUI(UISound.MinorClick);
			this._selectedItem.onHighlightChanged(this._selectedItem, false);
			this._selectedItem = this._activePageContentBuffer.content[num2];
			this._selectedItem.onHighlightChanged(this._selectedItem, true);
		}
	}

	// Token: 0x06000973 RID: 2419 RVA: 0x0004FFB4 File Offset: 0x0004E1B4
	private void SetTabHighlighted(SLayout tab, float originalPos, bool highlighted, bool moveXRatherThanY)
	{
		if (tab.targetOutlineColor.a > 0.1f != highlighted)
		{
			tab.Animate(highlighted ? 0.15f : 0.3f, 0f, highlighted ? SLayout.popCurve : null, delegate
			{
				if (highlighted)
				{
					tab.outlineColor = tab.outlineColor.WithAlpha(1f);
					if (moveXRatherThanY)
					{
						tab.x = originalPos + 10f;
					}
					else
					{
						tab.y = originalPos + 10f;
					}
					tab.scale = 1.1f;
					return;
				}
				tab.outlineColor = tab.outlineColor.WithAlpha(0f);
				if (moveXRatherThanY)
				{
					tab.x = originalPos;
				}
				else
				{
					tab.y = originalPos;
				}
				tab.scale = 1f;
			});
		}
	}

	// Token: 0x06000974 RID: 2420 RVA: 0x00050044 File Offset: 0x0004E244
	private void RefreshTabs()
	{
		this.SetTabHighlighted(this._discoveriesTab, this._discoveriesTabOriginalY, this._targetSection == JournalSection.Discoveries, false);
		this.SetTabHighlighted(this._peaksTab, this._peaksTabOriginalY, this._targetSection == JournalSection.Peaks, false);
		this.SetTabHighlighted(this._itemsTab, this._itemsTabOriginalY, this._targetSection == JournalSection.Inventory, false);
		this.SetTabHighlighted(this._mapsTab, this._mapsTabOriginalY, this._targetSection == JournalSection.Maps, false);
		this.SetTabHighlighted(this._systemTab, this._systemTabOriginalX, this._targetSection == JournalSection.System, true);
		float num = 0.4f;
		this._prevTabPrompt.groupAlpha = ((this._targetSection == JournalSection.Discoveries) ? num : 1f);
		this._nextTabPrompt.groupAlpha = ((this._targetSection == JournalSection.System) ? num : 1f);
		this._lookCloserPrompt.groupAlpha = (float)((this._targetSection == JournalSection.Maps && this._mapsInOrder.Count > 0) ? 1 : 0);
	}

	// Token: 0x06000975 RID: 2421 RVA: 0x00050144 File Offset: 0x0004E344
	private void TurnTo(JournalSection newSection, int newLeftPageIdx)
	{
		this._pageTurnActive = true;
		bool rightwardTurn = false;
		if (newSection == this._section)
		{
			rightwardTurn = newLeftPageIdx > this._currentLeftPageIdx;
		}
		else
		{
			rightwardTurn = newSection > this._section;
		}
		this._section = newSection;
		this._targetSection = this._section;
		this.CalculateLayoutIfNecessary(this._section);
		this.RefreshTabs();
		this._pendingLeftPage.gameObject.SetActive(true);
		this._pendingRightPage.gameObject.SetActive(true);
		this.CreateDoublePageContent(newLeftPageIdx, this._section, this._pendingPageContentBuffer);
		this.SetPageRotation(rightwardTurn, 0.01f, false);
		this._layout.AnimateCustom(0.5f, delegate(float t)
		{
			this.SetPageRotation(rightwardTurn, Mathf.Lerp(0.01f, 1f, t), false);
		}).Then(delegate
		{
			this._currentLeftPageIdx = newLeftPageIdx;
			ValueTuple<JournalPage, JournalPage> valueTuple = new ValueTuple<JournalPage, JournalPage>(this._leftPage, this._rightPage);
			JournalPage pendingLeftPage = this._pendingLeftPage;
			JournalPage pendingRightPage = this._pendingRightPage;
			this._leftPage = pendingLeftPage;
			this._rightPage = pendingRightPage;
			ValueTuple<JournalPage, JournalPage> valueTuple2 = valueTuple;
			this._pendingLeftPage = valueTuple2.Item1;
			this._pendingRightPage = valueTuple2.Item2;
			JournalContentBuffer activePageContentBuffer = this._activePageContentBuffer;
			this._activePageContentBuffer = this._pendingPageContentBuffer;
			this._pendingPageContentBuffer = activePageContentBuffer;
			this.DestroyPageContent(activePageContentBuffer);
			this._pendingLeftPage.gameObject.SetActive(false);
			this._pendingRightPage.gameObject.SetActive(false);
			this._pageTurnActive = false;
		});
	}

	// Token: 0x06000976 RID: 2422 RVA: 0x00050244 File Offset: 0x0004E444
	private void SetPageRotation(bool pendingIsOnRight, float turnNorm, bool onlyCurrent = false)
	{
		if (pendingIsOnRight)
		{
			this._leftPage.transform.eulerAngles = Vector3.zero;
			this._rightPage.transform.eulerAngles = new Vector3(0f, turnNorm * 180f, 0f);
			if (!onlyCurrent)
			{
				this._pendingLeftPage.transform.eulerAngles = new Vector3(0f, (1f - turnNorm) * -180f, 0f);
				this._pendingRightPage.transform.eulerAngles = Vector3.zero;
				return;
			}
		}
		else
		{
			this._leftPage.transform.eulerAngles = new Vector3(0f, turnNorm * -180f, 0f);
			this._rightPage.transform.eulerAngles = Vector3.zero;
			if (!onlyCurrent)
			{
				this._pendingLeftPage.transform.eulerAngles = Vector3.zero;
				this._pendingRightPage.transform.eulerAngles = new Vector3(0f, (1f - turnNorm) * 180f, 0f);
			}
		}
	}

	// Token: 0x06000977 RID: 2423 RVA: 0x0005035C File Offset: 0x0004E55C
	private void CreateDoublePageContent(int leftPageIdx, JournalSection section, JournalContentBuffer contentBuffer)
	{
		JournalController.SectionLayout sectionLayout = this._sectionLayouts[section];
		if (leftPageIdx < 0 || leftPageIdx >= sectionLayout.pageLayouts.Count)
		{
			Debug.LogError("Out of range!");
		}
		JournalController.PageLayout pageLayout = sectionLayout.pageLayouts[leftPageIdx];
		JournalController.PageLayout pageLayout2 = default(JournalController.PageLayout);
		int num = leftPageIdx + 1;
		if (num < sectionLayout.pageLayouts.Count)
		{
			pageLayout2 = sectionLayout.pageLayouts[num];
		}
		this.CreateSinglePageContent(pageLayout, false, contentBuffer);
		this.CreateSinglePageContent(pageLayout2, true, contentBuffer);
		int num2 = leftPageIdx / 2 + 1;
		int num3 = Mathf.CeilToInt((float)sectionLayout.pageLayouts.Count / 2f);
		contentBuffer.SetPageNumber(num2, num3);
		this._selectedItem = null;
		foreach (JournalItemView journalItemView in contentBuffer.content)
		{
			if (journalItemView.onHighlightChanged != null)
			{
				if (this._selectedItem == null)
				{
					this._selectedItem = journalItemView;
					this._selectedItem.onHighlightChanged(journalItemView, true);
				}
				else
				{
					this._selectedItem.onHighlightChanged(journalItemView, false);
				}
			}
		}
	}

	// Token: 0x06000978 RID: 2424 RVA: 0x00050494 File Offset: 0x0004E694
	private void CreateSinglePageContent(JournalController.PageLayout pageLayout, bool isRight, JournalContentBuffer contentBuffer)
	{
		if (pageLayout.items == null || pageLayout.items.Count == 0)
		{
			return;
		}
		float middleX = contentBuffer.layout.middleX;
		foreach (JournalController.ItemLayout itemLayout in pageLayout.items)
		{
			Vector2 pos = itemLayout.pos;
			if (isRight)
			{
				pos.x += middleX;
			}
			JournalItemView itemView = itemLayout.prototype.Instantiate<JournalItemView>(contentBuffer.layout.transform);
			itemView.layout.position = pos;
			itemView.layout.scale = 1f;
			if (itemLayout.height != 0f)
			{
				itemView.layout.height = itemLayout.height;
			}
			else
			{
				itemView.layout.height = itemView.originalHeight;
			}
			if (itemView.text1 != null && itemLayout.text1 != null)
			{
				itemView.text1.textMeshPro.text = itemLayout.text1;
			}
			else if (itemView.text1 != null)
			{
				itemView.text1.textMeshPro.text = "";
			}
			if (itemView.text2 != null && itemLayout.text2 != null)
			{
				itemView.text2.textMeshPro.text = itemLayout.text2;
			}
			else if (itemView.text2 != null)
			{
				itemView.text2.textMeshPro.text = "";
			}
			if (itemView.image != null && itemLayout.sprite != null)
			{
				itemView.image.image.sprite = itemLayout.sprite;
				itemView.image.rotation = itemLayout.spriteAngle;
			}
			else if (itemView.image != null)
			{
				itemView.image.image.sprite = null;
				itemView.image.rotation = 0f;
			}
			itemView.SetTicked(itemLayout.ticked);
			itemView.SetImageFaded(itemLayout.imageFaded);
			itemView.SetLit(itemLayout.lit);
			itemView.SetDisabled(itemLayout.disabled);
			itemView.isMap = itemLayout.isMap;
			itemView.onHighlightChanged = itemLayout.onHighlightChanged;
			itemView.onSelect = itemLayout.onSelect;
			if (itemView.highlight != null)
			{
				itemView.highlight.alpha = 0f;
			}
			if (this._itemToHighlightOnOpenInkName != null && itemLayout.inkName == this._itemToHighlightOnOpenInkName)
			{
				itemView.layout.Animate(0.2f, 0.5f, delegate
				{
					if (itemView.highlight != null)
					{
						itemView.highlight.alpha = 1f;
					}
					itemView.layout.scale = 1.1f;
				}).ThenAnimate(0.6f, delegate
				{
					if (itemView.highlight != null)
					{
						itemView.highlight.alpha = 0f;
					}
					itemView.layout.scale = 1f;
				});
				this._itemToHighlightOnOpenInkName = null;
			}
			contentBuffer.content.Add(itemView);
		}
	}

	// Token: 0x06000979 RID: 2425 RVA: 0x00050820 File Offset: 0x0004EA20
	private void DestroyPageContent(JournalContentBuffer buffer)
	{
		foreach (JournalItemView journalItemView in buffer.content)
		{
			if (journalItemView.onHighlightChanged != null)
			{
				journalItemView.onHighlightChanged(journalItemView, false);
			}
			journalItemView.layout.CancelAnimations();
			journalItemView.prototype.ReturnToPool();
		}
		buffer.content.Clear();
	}

	// Token: 0x1700025E RID: 606
	// (get) Token: 0x0600097A RID: 2426 RVA: 0x000508A4 File Offset: 0x0004EAA4
	// (set) Token: 0x0600097B RID: 2427 RVA: 0x000508B7 File Offset: 0x0004EAB7
	private float openCloseHingeAngle
	{
		get
		{
			this.InitOpenCloseHingeAngle();
			return this._openCloseHingeAngle.value;
		}
		set
		{
			this.InitOpenCloseHingeAngle();
			this._openCloseHingeAngle.value = value;
		}
	}

	// Token: 0x0600097C RID: 2428 RVA: 0x000508CB File Offset: 0x0004EACB
	private void InitOpenCloseHingeAngle()
	{
		if (this._openCloseHingeAngle == null)
		{
			this._openCloseHingeAngle = new SLayoutAngleProperty
			{
				getter = () => this._rightHalf.rotation.eulerAngles.y,
				setter = delegate(float r)
				{
					this._rightHalf.rotation = Quaternion.Euler(0f, r, 0f);
					this._leftHalf.rotation = Quaternion.Euler(0f, -r, 0f);
				}
			};
		}
	}

	// Token: 0x0600097D RID: 2429 RVA: 0x00050904 File Offset: 0x0004EB04
	private void OnUIPositionUpdate(GameUI ui)
	{
		if (this._mapConfirmChoicesWidget != null)
		{
			Vector2 vector = ui.WorldToCanvas(Runner.instance.physicalPosition3d, default(Vector2));
			this._mapConfirmChoicesWidget.layout.centerX = vector.x;
			this._mapConfirmChoicesWidget.layout.topY = vector.y - 50f;
		}
	}

	// Token: 0x0600097F RID: 2431 RVA: 0x000509A4 File Offset: 0x0004EBA4
	[CompilerGenerated]
	private float <CalculateLayout_Discoveries>g__X|2_0(ref JournalController.<>c__DisplayClass2_0 A_1)
	{
		if (!A_1.isLeft)
		{
			return this._settings.pageRightMargin;
		}
		return this._settings.pageLeftMargin;
	}

	// Token: 0x06000980 RID: 2432 RVA: 0x000509C5 File Offset: 0x0004EBC5
	[CompilerGenerated]
	private float <CalculateLayout_Peaks>g__X|3_0(ref JournalController.<>c__DisplayClass3_0 A_1)
	{
		if (!A_1.isLeft)
		{
			return this._settings.pageRightMargin;
		}
		return this._settings.pageLeftMargin;
	}

	// Token: 0x06000981 RID: 2433 RVA: 0x000509E6 File Offset: 0x0004EBE6
	[CompilerGenerated]
	private float <CalculateLayout_Inventory>g__X|5_0(ref JournalController.<>c__DisplayClass5_0 A_1)
	{
		if (!A_1.isLeft)
		{
			return this._settings.pageRightMargin;
		}
		return this._settings.pageLeftMargin;
	}

	// Token: 0x06000982 RID: 2434 RVA: 0x00050A07 File Offset: 0x0004EC07
	[CompilerGenerated]
	private float <CalculateLayout_Maps>g__X|7_0(ref JournalController.<>c__DisplayClass7_0 A_1)
	{
		if (!A_1.isLeft)
		{
			return this._settings.pageRightMargin;
		}
		return this._settings.pageLeftMargin;
	}

	// Token: 0x04000B2E RID: 2862
	private JournalItemView _discoveryTestItem;

	// Token: 0x04000B2F RID: 2863
	private List<JournalController.SortedMap> _mapsInOrder = new List<JournalController.SortedMap>(256);

	// Token: 0x04000B30 RID: 2864
	private Dictionary<JournalSection, JournalController.SectionLayout> _sectionLayouts = new Dictionary<JournalSection, JournalController.SectionLayout>();

	// Token: 0x04000B31 RID: 2865
	public static Action onClose;

	// Token: 0x04000B32 RID: 2866
	public static bool wantsGameplayPaused;

	// Token: 0x04000B34 RID: 2868
	public bool mapConfirmActive;

	// Token: 0x04000B36 RID: 2870
	private CanvasScaler _canvasScaler;

	// Token: 0x04000B37 RID: 2871
	[SerializeField]
	private SLayout _layout;

	// Token: 0x04000B38 RID: 2872
	private SLayoutAngleProperty _openCloseHingeAngle;

	// Token: 0x04000B39 RID: 2873
	[SerializeField]
	private Transform _leftHalf;

	// Token: 0x04000B3A RID: 2874
	[SerializeField]
	private Transform _rightHalf;

	// Token: 0x04000B3B RID: 2875
	[SerializeField]
	private JournalPage _leftPage;

	// Token: 0x04000B3C RID: 2876
	[SerializeField]
	private JournalPage _rightPage;

	// Token: 0x04000B3D RID: 2877
	[SerializeField]
	private JournalPage _pendingLeftPage;

	// Token: 0x04000B3E RID: 2878
	[SerializeField]
	private JournalPage _pendingRightPage;

	// Token: 0x04000B3F RID: 2879
	[SerializeField]
	private SLayout _discoveriesTab;

	// Token: 0x04000B40 RID: 2880
	[SerializeField]
	private SLayout _peaksTab;

	// Token: 0x04000B41 RID: 2881
	[SerializeField]
	private SLayout _itemsTab;

	// Token: 0x04000B42 RID: 2882
	[SerializeField]
	private SLayout _mapsTab;

	// Token: 0x04000B43 RID: 2883
	[SerializeField]
	private SLayout _systemTab;

	// Token: 0x04000B44 RID: 2884
	[SerializeField]
	private SLayout _prompts;

	// Token: 0x04000B45 RID: 2885
	[SerializeField]
	private SLayout _prevTabPrompt;

	// Token: 0x04000B46 RID: 2886
	[SerializeField]
	private SLayout _nextTabPrompt;

	// Token: 0x04000B47 RID: 2887
	[SerializeField]
	private Prototype _contentBufferPrototype;

	// Token: 0x04000B48 RID: 2888
	[SerializeField]
	private Prototype _titleItemPrototype;

	// Token: 0x04000B49 RID: 2889
	[SerializeField]
	private Prototype _discoveryItemPrototype;

	// Token: 0x04000B4A RID: 2890
	[SerializeField]
	private Prototype _peakLeftItemPrototype;

	// Token: 0x04000B4B RID: 2891
	[SerializeField]
	private Prototype _peakRightItemPrototype;

	// Token: 0x04000B4C RID: 2892
	[SerializeField]
	private Prototype _inventoryItemPrototype;

	// Token: 0x04000B4D RID: 2893
	[SerializeField]
	private Prototype _mapItemPrototype;

	// Token: 0x04000B4E RID: 2894
	[SerializeField]
	private Prototype _systemOptionPrototype;

	// Token: 0x04000B4F RID: 2895
	[SerializeField]
	private Prototype _logoPrototype;

	// Token: 0x04000B50 RID: 2896
	[SerializeField]
	private Prototype _lastSavedPrototype;

	// Token: 0x04000B51 RID: 2897
	[SerializeField]
	private Prototype _mapConfirmChoicesPrototype;

	// Token: 0x04000B52 RID: 2898
	[SerializeField]
	private Prototype _fullPageSketchPrototype;

	// Token: 0x04000B53 RID: 2899
	[SerializeField]
	private Prototype _mapPrototypeForZooming;

	// Token: 0x04000B54 RID: 2900
	[SerializeField]
	private SLayout _darkenBackgroundForMapZooming;

	// Token: 0x04000B55 RID: 2901
	[SerializeField]
	private SLayout _lookCloserPrompt;

	// Token: 0x04000B56 RID: 2902
	[SerializeField]
	private JournalSettings _settings;

	// Token: 0x04000B57 RID: 2903
	private JournalContentBuffer _activePageContentBuffer;

	// Token: 0x04000B58 RID: 2904
	private JournalContentBuffer _pendingPageContentBuffer;

	// Token: 0x04000B59 RID: 2905
	private int _currentLeftPageIdx;

	// Token: 0x04000B5A RID: 2906
	private JournalItemView _selectedItem;

	// Token: 0x04000B5B RID: 2907
	private string _itemToHighlightOnOpenInkName;

	// Token: 0x04000B5C RID: 2908
	private JournalSection _section;

	// Token: 0x04000B5D RID: 2909
	private JournalSection _targetSection;

	// Token: 0x04000B5E RID: 2910
	private bool _pageTurnActive;

	// Token: 0x04000B5F RID: 2911
	private int _pendingPageTurnDir;

	// Token: 0x04000B60 RID: 2912
	private bool _pendingClose;

	// Token: 0x04000B61 RID: 2913
	private float _defaultOriginYNorm;

	// Token: 0x04000B62 RID: 2914
	private float _discoveriesTabOriginalY;

	// Token: 0x04000B63 RID: 2915
	private float _peaksTabOriginalY;

	// Token: 0x04000B64 RID: 2916
	private float _itemsTabOriginalY;

	// Token: 0x04000B65 RID: 2917
	private float _mapsTabOriginalY;

	// Token: 0x04000B66 RID: 2918
	private float _systemTabOriginalX;

	// Token: 0x04000B67 RID: 2919
	private float _timeScalarCoolDownTimer;

	// Token: 0x04000B68 RID: 2920
	private int _slideOverDir;

	// Token: 0x04000B69 RID: 2921
	private bool _mapConfirmWasPreviouslyCancelled;

	// Token: 0x04000B6A RID: 2922
	private bool _mapsZoomingActive;

	// Token: 0x04000B6B RID: 2923
	private object _mapsZoomedBackItem = new object();

	// Token: 0x04000B6C RID: 2924
	private Map _popMapWhenPossible;

	// Token: 0x04000B6D RID: 2925
	private JournalItemView _popPeakItemWhenPossible;

	// Token: 0x04000B6E RID: 2926
	private ChoicesWidget _mapConfirmChoicesWidget;

	// Token: 0x04000B6F RID: 2927
	private IEnumerator _mapConfirmCoroutine;

	// Token: 0x04000B70 RID: 2928
	private object _confirmMapBackStackItem = new object();

	// Token: 0x04000B71 RID: 2929
	private const JournalSection _lastSection = JournalSection.System;

	// Token: 0x0200033F RID: 831
	private struct ItemLayout
	{
		// Token: 0x0400183E RID: 6206
		public Prototype prototype;

		// Token: 0x0400183F RID: 6207
		public Vector2 pos;

		// Token: 0x04001840 RID: 6208
		public float height;

		// Token: 0x04001841 RID: 6209
		public string text1;

		// Token: 0x04001842 RID: 6210
		public string text2;

		// Token: 0x04001843 RID: 6211
		public Sprite sprite;

		// Token: 0x04001844 RID: 6212
		public float spriteAngle;

		// Token: 0x04001845 RID: 6213
		public bool ticked;

		// Token: 0x04001846 RID: 6214
		public bool isMap;

		// Token: 0x04001847 RID: 6215
		public bool lit;

		// Token: 0x04001848 RID: 6216
		public bool imageFaded;

		// Token: 0x04001849 RID: 6217
		public bool disabled;

		// Token: 0x0400184A RID: 6218
		public JournalItemView.OnHighlightChanged onHighlightChanged;

		// Token: 0x0400184B RID: 6219
		public Action onSelect;

		// Token: 0x0400184C RID: 6220
		public string inkName;
	}

	// Token: 0x02000340 RID: 832
	private struct PageLayout
	{
		// Token: 0x0400184D RID: 6221
		public List<JournalController.ItemLayout> items;
	}

	// Token: 0x02000341 RID: 833
	private struct SectionLayout
	{
		// Token: 0x060016EF RID: 5871 RVA: 0x00099914 File Offset: 0x00097B14
		public void AddPage()
		{
			this.pageLayouts.Add(new JournalController.PageLayout
			{
				items = new List<JournalController.ItemLayout>()
			});
		}

		// Token: 0x060016F0 RID: 5872 RVA: 0x00099941 File Offset: 0x00097B41
		public void AddItemToLastPage(JournalController.ItemLayout itemLayout)
		{
			this.pageLayouts[this.pageLayouts.Count - 1].items.Add(itemLayout);
		}

		// Token: 0x0400184E RID: 6222
		public List<JournalController.PageLayout> pageLayouts;
	}

	// Token: 0x02000342 RID: 834
	private struct SortedMap
	{
		// Token: 0x0400184F RID: 6223
		public Map map;

		// Token: 0x04001850 RID: 6224
		public int index;

		// Token: 0x04001851 RID: 6225
		public bool playerCompleted;
	}
}
