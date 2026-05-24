using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Ink;
using Ink.Runtime;
using UnityEngine;

// Token: 0x0200008E RID: 142
public class Narrative : MonoBehaviour
{
	// Token: 0x1700011E RID: 286
	// (get) Token: 0x06000407 RID: 1031 RVA: 0x000216B0 File Offset: 0x0001F8B0
	public static Narrative instance
	{
		get
		{
			return GSR.Narrative;
		}
	}

	// Token: 0x14000002 RID: 2
	// (add) Token: 0x06000408 RID: 1032 RVA: 0x000216B8 File Offset: 0x0001F8B8
	// (remove) Token: 0x06000409 RID: 1033 RVA: 0x000216EC File Offset: 0x0001F8EC
	public static event Action<Story> onCreateStory;

	// Token: 0x14000003 RID: 3
	// (add) Token: 0x0600040A RID: 1034 RVA: 0x00021720 File Offset: 0x0001F920
	// (remove) Token: 0x0600040B RID: 1035 RVA: 0x00021754 File Offset: 0x0001F954
	public static event Action<Story> onLoadState;

	// Token: 0x14000004 RID: 4
	// (add) Token: 0x0600040C RID: 1036 RVA: 0x00021788 File Offset: 0x0001F988
	// (remove) Token: 0x0600040D RID: 1037 RVA: 0x000217BC File Offset: 0x0001F9BC
	public static event Action onDidRefreshInteractables;

	// Token: 0x14000005 RID: 5
	// (add) Token: 0x0600040E RID: 1038 RVA: 0x000217F0 File Offset: 0x0001F9F0
	// (remove) Token: 0x0600040F RID: 1039 RVA: 0x00021824 File Offset: 0x0001FA24
	public static event Action onRefreshedWeather;

	// Token: 0x14000006 RID: 6
	// (add) Token: 0x06000410 RID: 1040 RVA: 0x00021858 File Offset: 0x0001FA58
	// (remove) Token: 0x06000411 RID: 1041 RVA: 0x0002188C File Offset: 0x0001FA8C
	public static event Action<string> onEventDidFire;

	// Token: 0x1700011F RID: 287
	// (get) Token: 0x06000412 RID: 1042 RVA: 0x000218BF File Offset: 0x0001FABF
	// (set) Token: 0x06000413 RID: 1043 RVA: 0x000218E6 File Offset: 0x0001FAE6
	public static bool profanity
	{
		get
		{
			if (!Narrative._gotCachedProfanitySetting)
			{
				Narrative._profanity = PlayerPrefsX.GetInt("Profanity", 1) != 0;
				Narrative._gotCachedProfanitySetting = true;
			}
			return Narrative._profanity;
		}
		set
		{
			if (Narrative._profanity != value || !Narrative._gotCachedProfanitySetting)
			{
				Narrative._profanity = value;
				PlayerPrefsX.SetInt("Profanity", value ? 1 : 0);
				Narrative._gotCachedProfanitySetting = true;
			}
		}
	}

	// Token: 0x17000120 RID: 288
	// (get) Token: 0x06000414 RID: 1044 RVA: 0x00021914 File Offset: 0x0001FB14
	// (set) Token: 0x06000415 RID: 1045 RVA: 0x0002191C File Offset: 0x0001FB1C
	public Story inkStory
	{
		get
		{
			return this._inkStory;
		}
		private set
		{
			this._inkStory = value;
		}
	}

	// Token: 0x17000121 RID: 289
	// (get) Token: 0x06000416 RID: 1046 RVA: 0x00021925 File Offset: 0x0001FB25
	public List<string> debugLog
	{
		get
		{
			return this._debugLog;
		}
	}

	// Token: 0x06000417 RID: 1047 RVA: 0x0002192D File Offset: 0x0001FB2D
	public void PreventBackgroundRemarks(bool prevent, Narrative.PreventBackgroundRemarksReason reason)
	{
		if (prevent)
		{
			this.preventBackgroundRemarks |= reason;
			return;
		}
		this.preventBackgroundRemarks &= ~reason;
	}

	// Token: 0x17000122 RID: 290
	// (get) Token: 0x06000418 RID: 1048 RVA: 0x00021950 File Offset: 0x0001FB50
	private bool allowLullRemarksAtAll
	{
		get
		{
			return !Runner.instance.resting && !Runner.instance.dead && !Runner.instance.hidden && !Runner.instance.inFinalJump;
		}
	}

	// Token: 0x17000123 RID: 291
	// (get) Token: 0x06000419 RID: 1049 RVA: 0x00021985 File Offset: 0x0001FB85
	public bool allowLullRemarks
	{
		get
		{
			return this.allowLullRemarksAtAll && this.preventBackgroundRemarks == Narrative.PreventBackgroundRemarksReason.None;
		}
	}

	// Token: 0x17000124 RID: 292
	// (get) Token: 0x0600041A RID: 1050 RVA: 0x0002199A File Offset: 0x0001FB9A
	public bool resetLullRemarkTime
	{
		get
		{
			return !this.allowLullRemarksAtAll || (this.preventBackgroundRemarks != Narrative.PreventBackgroundRemarksReason.None && this.preventBackgroundRemarks != Narrative.PreventBackgroundRemarksReason.PoorRunnerState);
		}
	}

	// Token: 0x0600041B RID: 1051 RVA: 0x000219BC File Offset: 0x0001FBBC
	public void ResetLullRemarkTime()
	{
		this._lullRemarkStartWaitingFromTime = Mathf.Max(Time.time - 5f, this._lullRemarkStartWaitingFromTime);
	}

	// Token: 0x17000125 RID: 293
	// (get) Token: 0x0600041C RID: 1052 RVA: 0x000219DA File Offset: 0x0001FBDA
	public bool allowRecoveryRemark
	{
		get
		{
			return this.allowLullRemarksAtAll && this.preventBackgroundRemarks == Narrative.PreventBackgroundRemarksReason.FeelingExhausted;
		}
	}

	// Token: 0x17000126 RID: 294
	// (get) Token: 0x0600041D RID: 1053 RVA: 0x000219F3 File Offset: 0x0001FBF3
	public bool isBusy
	{
		get
		{
			return this.isInkBusy || NarrativePresenter.presenting || this.blockedForJournalChoice;
		}
	}

	// Token: 0x17000127 RID: 295
	// (get) Token: 0x0600041E RID: 1054 RVA: 0x00021A0C File Offset: 0x0001FC0C
	public bool isInkBusy
	{
		get
		{
			return this._inkEvaluationCoroutine != null;
		}
	}

	// Token: 0x17000128 RID: 296
	// (get) Token: 0x0600041F RID: 1055 RVA: 0x00021A17 File Offset: 0x0001FC17
	public bool canInterrupt
	{
		get
		{
			return (bool)this.inkStory.variablesState["interruptAllowed"];
		}
	}

	// Token: 0x17000129 RID: 297
	// (get) Token: 0x06000420 RID: 1056 RVA: 0x00021A33 File Offset: 0x0001FC33
	public int knotQueueCount
	{
		get
		{
			return this._queuedKnots.Count;
		}
	}

	// Token: 0x1700012A RID: 298
	// (get) Token: 0x06000421 RID: 1057 RVA: 0x00021A40 File Offset: 0x0001FC40
	public bool hasAutoCut
	{
		get
		{
			return this.activeAutoCutZoneName != null;
		}
	}

	// Token: 0x1700012B RID: 299
	// (get) Token: 0x06000422 RID: 1058 RVA: 0x00021A4B File Offset: 0x0001FC4B
	public bool canRunOff
	{
		get
		{
			return (Narrative.instance.hasAutoCut || NarrativePresenter.instance.hasExitTriggerZoneChoices) && Runner.instance.playerControlDisabled == PlayerControlDisableReason.None;
		}
	}

	// Token: 0x1700012C RID: 300
	// (get) Token: 0x06000423 RID: 1059 RVA: 0x00021A74 File Offset: 0x0001FC74
	// (set) Token: 0x06000424 RID: 1060 RVA: 0x00021A7C File Offset: 0x0001FC7C
	public string customRunOffTutorialText { get; private set; }

	// Token: 0x1700012D RID: 301
	// (get) Token: 0x06000425 RID: 1061 RVA: 0x00021A85 File Offset: 0x0001FC85
	public bool hasCustomRunOffTutorialText
	{
		get
		{
			return this.customRunOffTutorialText != null && this.customRunOffTutorialText.Length > 0;
		}
	}

	// Token: 0x1700012E RID: 302
	// (get) Token: 0x06000426 RID: 1062 RVA: 0x00021A9F File Offset: 0x0001FC9F
	// (set) Token: 0x06000427 RID: 1063 RVA: 0x00021AA7 File Offset: 0x0001FCA7
	public bool runOffTutorialIsSticky { get; private set; }

	// Token: 0x06000428 RID: 1064 RVA: 0x00021AB0 File Offset: 0x0001FCB0
	public void NotifyPeakIfNecessary(string peakInkName)
	{
		if (!this.orderOfAquisitionIdx.ContainsKey(peakInkName))
		{
			this.orderOfAquisitionIdx[peakInkName] = this.nextAquisitionIdx;
			this.nextAquisitionIdx++;
			if (!this._clearing)
			{
				MonoSingleton<JournalController>.instance.SetLayoutDirty(JournalSection.Peaks);
				MonoSingleton<Notifications>.instance.Notify(NotificationType.Peak, peakInkName);
			}
		}
	}

	// Token: 0x06000429 RID: 1065 RVA: 0x00021B0A File Offset: 0x0001FD0A
	public bool PeakKnown(string peakInkName)
	{
		return ((InkList)this.inkStory.variablesState["KnownPeaks"]).ContainsItemNamed(peakInkName);
	}

	// Token: 0x0600042A RID: 1066 RVA: 0x00021B2C File Offset: 0x0001FD2C
	private bool TryGetItemNamed(string itemName, out InkListItem item)
	{
		item = default(InkListItem);
		ListValue listValue = this.inkStory.listDefinitions.FindSingleItemListWithName(itemName);
		if (listValue == null)
		{
			return false;
		}
		using (Dictionary<InkListItem, int>.Enumerator enumerator = listValue.value.GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				KeyValuePair<InkListItem, int> keyValuePair = enumerator.Current;
				item = keyValuePair.Key;
			}
		}
		return true;
	}

	// Token: 0x0600042B RID: 1067 RVA: 0x00021BAC File Offset: 0x0001FDAC
	public void LoadInk()
	{
		base.StopAllCoroutines();
		this.NullCoroutine();
		this.inkStory = new Story(this.ink.text);
		this.RebuildCachedPairStore(true);
		this.inkStory.ObserveVariable("health", delegate(string healthVarName, object newHealthVal)
		{
			Runner.instance.health.SetFromInk(float.Parse(newHealthVal.ToString()));
		});
		this.inkStory.ObserveVariable("currentMaxHealth", delegate(string healthVarName, object newMaxHealthVal)
		{
			float num;
			if (newMaxHealthVal is int)
			{
				num = (float)((int)newMaxHealthVal);
			}
			else
			{
				num = (float)newMaxHealthVal;
			}
			Runner.instance.health.currentMaxHealth = num;
		});
		this.inkStory.ObserveVariable("RestingComfort", delegate(string varName, object inkListVal)
		{
			KeyValuePair<InkListItem, int> minItem = ((InkList)inkListVal).minItem;
			Runner.instance.health.SetShelterComfortFromInk(minItem.Key.itemName);
		});
		this.inkStory.ObserveVariable("Inventory", delegate(string varName, object inkListVal)
		{
			InkList inkList = (InkList)inkListVal;
			Runner.instance.torch.hasTorch = inkList.ContainsItemNamed("Torch");
			foreach (KeyValuePair<InkListItem, int> keyValuePair in inkList)
			{
				if (!this.orderOfAquisitionIdx.ContainsKey(keyValuePair.Key.itemName))
				{
					this.orderOfAquisitionIdx[keyValuePair.Key.itemName] = this.nextAquisitionIdx;
					this.nextAquisitionIdx++;
					if (!this._clearing)
					{
						MonoSingleton<JournalController>.instance.SetLayoutDirty(JournalSection.Inventory);
						MonoSingleton<Notifications>.instance.Notify(NotificationType.Item, keyValuePair.Key.itemName);
					}
				}
			}
			if (!this._clearing)
			{
				Narrative._removedInventoryScratch.Clear();
				foreach (string text in this.orderOfAquisitionIdx.Keys)
				{
					ListValue listValue = this.inkStory.listDefinitions.FindSingleItemListWithName(text);
					if (!(listValue == null) && listValue.value.singleItem.originName == "Inventory" && !inkList.ContainsItemNamed(text))
					{
						Narrative._removedInventoryScratch.Add(text);
					}
				}
				foreach (string text2 in Narrative._removedInventoryScratch)
				{
					this.orderOfAquisitionIdx.Remove(text2);
				}
				if (Narrative._removedInventoryScratch.Count > 0)
				{
					MonoSingleton<JournalController>.instance.SetLayoutDirty(JournalSection.Inventory);
				}
				Narrative._removedInventoryScratch.Clear();
			}
		});
		this.inkStory.ObserveVariable("DiscoveredFacts", delegate(string varName, object inkListVal)
		{
			foreach (KeyValuePair<InkListItem, int> keyValuePair2 in ((InkList)inkListVal))
			{
				if (!this.orderOfAquisitionIdx.ContainsKey(keyValuePair2.Key.itemName))
				{
					this.orderOfAquisitionIdx[keyValuePair2.Key.itemName] = this.nextAquisitionIdx;
					this.nextAquisitionIdx++;
					if (!this._clearing)
					{
						MonoSingleton<JournalController>.instance.SetLayoutDirty(JournalSection.Discoveries);
						if (keyValuePair2.Key.itemName != "RUNNING_MAKES_ME_STRONGER")
						{
							MonoSingleton<Notifications>.instance.Notify(NotificationType.Discovery, keyValuePair2.Key.itemName);
						}
					}
				}
			}
		});
		this.inkStory.ObserveVariable("SuspectedPeaks", delegate(string varName, object inkListVal)
		{
			if (this._clearing)
			{
				return;
			}
			foreach (KeyValuePair<InkListItem, int> keyValuePair3 in ((InkList)inkListVal))
			{
				this.NotifyPeakIfNecessary(keyValuePair3.Key.itemName);
			}
		});
		this.inkStory.ObserveVariable("TorchBatteryLevel", delegate(string varName, object newBatteryLevel)
		{
			Runner.instance.torch.batteryLevel = (float)newBatteryLevel;
		});
		this.inkStory.ObserveVariable("autoCutZone", delegate(string varName, object newAutoCutZoneVal)
		{
			InkList inkList2 = (InkList)newAutoCutZoneVal;
			if (inkList2.Count == 0)
			{
				this.activeAutoCutZoneName = null;
				return;
			}
			this.activeAutoCutZoneName = inkList2.First<KeyValuePair<InkListItem, int>>().Key.itemName;
		});
		this.inkStory.ObserveVariable("customRunOffTutorialText", delegate(string varName, object newText)
		{
			this.customRunOffTutorialText = (string)newText;
		});
		this.inkStory.ObserveVariable("runOffTutorialIsSticky", delegate(string varName, object newBool)
		{
			this.runOffTutorialIsSticky = (bool)newBool;
		});
		this.inkStory.BindExternalFunction<InkList, InkList>("HAS_ANY", delegate(InkList largeListToSearch, InkList smallNumberOfItems)
		{
			if (smallNumberOfItems == null || smallNumberOfItems.Count == 0)
			{
				return false;
			}
			foreach (InkListItem inkListItem in smallNumberOfItems.Keys)
			{
				if (largeListToSearch.ContainsKey(inkListItem))
				{
					return true;
				}
			}
			return false;
		}, false);
		this.inkStory.BindExternalFunction("resetLullRemarkTimer", delegate
		{
			this.ResetLullRemarkTime();
		}, false);
		this.inkStory.BindExternalFunction<InkList>("enable", delegate(InkList propListItem)
		{
			foreach (KeyValuePair<InkListItem, int> keyValuePair4 in propListItem)
			{
				PropsController.instance.SetPropEnabled(keyValuePair4.Key.itemName, true);
			}
		}, false);
		this.inkStory.BindExternalFunction<InkList>("disable", delegate(InkList propListItem)
		{
			foreach (KeyValuePair<InkListItem, int> keyValuePair5 in propListItem)
			{
				if (Level.current != null)
				{
					foreach (Creature creature in Level.current.creatures)
					{
						if (creature.inkName == keyValuePair5.Key.itemName)
						{
							creature.StopAndHide();
						}
					}
				}
				PropsController.instance.SetPropEnabled(keyValuePair5.Key.itemName, false);
			}
		}, false);
		this.inkStory.BindExternalFunction<float>("alterHealth", delegate(float delta)
		{
			if (delta < 0f)
			{
				Runner.instance.health.ApplyDamage(DamageType.Ink, -1f * delta);
				return;
			}
			if (delta > 0f)
			{
				Runner.instance.health.ApplyHealing(delta);
			}
		}, false);
		this.inkStory.BindExternalFunction<InkList>("isEnabled", delegate(InkList propListItem)
		{
			if (propListItem.Count == 0)
			{
				return false;
			}
			string itemName = propListItem.First<KeyValuePair<InkListItem, int>>().Key.itemName;
			return PropsController.instance.IsPropEnabled(itemName);
		}, false);
		this.inkStory.BindExternalFunction<InkList, bool>("enableDisablePeakWeatherMod", delegate(InkList peakID, bool doEnable)
		{
			if (peakID.Count == 0)
			{
				return;
			}
			string itemName2 = peakID.singleItem.itemName;
			List<Prop> loadedPropsByInkName = Prop.GetLoadedPropsByInkName(itemName2);
			if (loadedPropsByInkName.Count == 0)
			{
				return;
			}
			Prop prop = loadedPropsByInkName[0];
			if (prop == null || !prop.enabled)
			{
				return;
			}
			WeatherModifierZone weatherModifierZone = prop.weatherModifierZone;
			if (weatherModifierZone != null)
			{
				weatherModifierZone.enabled = doEnable;
				MonoSingleton<GodRayParticlesManager>.instance.SetPeakGodRayActive(itemName2, doEnable, prop.transform.position);
			}
		}, false);
		this.inkStory.BindExternalFunction("tunnelDepth", () => this.inkStory.state.callstackDepth, false);
		this.inkStory.BindExternalFunction<InkList, InkList, InkList>("_relate", delegate(InkList list1, InkList list2, InkList relation)
		{
			string itemName3 = relation.First<KeyValuePair<InkListItem, int>>().Key.itemName;
			string text3 = "";
			foreach (KeyValuePair<InkListItem, int> keyValuePair6 in list1)
			{
				foreach (KeyValuePair<InkListItem, int> keyValuePair7 in list2)
				{
					if (!this.IsRelatedPairInPairStoreString(keyValuePair6.Key, keyValuePair7.Key, itemName3))
					{
						text3 += this.PairStoreString(keyValuePair6.Key.itemName, keyValuePair7.Key.itemName, itemName3, true);
						this.LinkInRelationCache(keyValuePair6.Key.itemName, itemName3, keyValuePair7.Key.itemName);
					}
				}
			}
			VariablesState variablesState = this.inkStory.variablesState;
			VariablesState variablesState2 = variablesState;
			string text4 = "pairstore";
			object obj = variablesState["pairstore"];
			variablesState2[text4] = ((obj != null) ? obj.ToString() : null) + text3;
			return !string.IsNullOrWhiteSpace(text3);
		}, false);
		this.inkStory.BindExternalFunction<InkList, InkList, InkList>("_unrelate", delegate(InkList list1, InkList relation, InkList list2)
		{
			string itemName4 = relation.First<KeyValuePair<InkListItem, int>>().Key.itemName;
			List<string> removeKeys = new List<string>();
			foreach (KeyValuePair<InkListItem, int> keyValuePair8 in list1)
			{
				foreach (KeyValuePair<InkListItem, int> keyValuePair9 in list2)
				{
					if (this.IsRelatedPairInPairStoreString(keyValuePair8.Key, keyValuePair9.Key, itemName4))
					{
						removeKeys.Add(this.PairStoreString(keyValuePair8.Key.itemName, keyValuePair9.Key.itemName, itemName4, false));
						this._cachedPairStore[itemName4][keyValuePair8.Key.itemName].Remove(keyValuePair9.Key.itemName);
						this._reversedCachedPairStore[itemName4][keyValuePair9.Key.itemName].Remove(keyValuePair8.Key.itemName);
					}
				}
			}
			if (removeKeys.Count == 0)
			{
				return false;
			}
			IEnumerable<string> enumerable = from t in ((string)this.inkStory.variablesState["pairstore"]).Split(Narrative._pairStoreStringDelimiter, StringSplitOptions.RemoveEmptyEntries)
				where !removeKeys.Contains(t)
				select t;
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string text5 in enumerable)
			{
				stringBuilder.Append(text5 + Narrative._pairStoreStringDelimiter[0].ToString());
			}
			this.inkStory.variablesState["pairstore"] = stringBuilder.ToString();
			return true;
		}, false);
		this.inkStory.BindExternalFunction<InkList, InkList, string>("_isRelated", delegate(InkList list1, InkList list2, string relationName)
		{
			foreach (KeyValuePair<InkListItem, int> keyValuePair10 in list1)
			{
				foreach (KeyValuePair<InkListItem, int> keyValuePair11 in list2)
				{
					if (!this.IsRelatedPairInPairStoreString(keyValuePair10.Key, keyValuePair11.Key, relationName))
					{
						return false;
					}
				}
			}
			return true;
		}, true);
		this.inkStory.BindExternalFunction<InkList, InkList>("_getRelatesTo", (InkList list1, InkList relation) => this.GetRelatedElementsInPairStoreString(list1, relation.First<KeyValuePair<InkListItem, int>>().Key.itemName, false), true);
		this.inkStory.BindExternalFunction<InkList, InkList>("_getRelatedFrom", (InkList list2, InkList relation) => this.GetRelatedElementsInPairStoreString(list2, relation.First<KeyValuePair<InkListItem, int>>().Key.itemName, true), true);
		this.inkStory.BindExternalFunction<InkList, InkList>("list_item_is_member_of", delegate(InkList k, InkList list)
		{
			if (k.origins == null)
			{
				return false;
			}
			foreach (ListDefinition listDefinition2 in k.origins)
			{
				if (!list.origins.Contains(listDefinition2))
				{
					return false;
				}
			}
			return true;
		}, true);
		this.inkStory.BindExternalFunction<string>("STRING_TO_LIST", delegate(string itemKey)
		{
			object obj2;
			try
			{
				obj2 = InkList.FromString(itemKey, this.inkStory);
			}
			catch
			{
				obj2 = new InkList();
			}
			return obj2;
		}, true);
		this.inkStory.BindExternalFunction<InkList>("not_reached", (InkList stateList) => !this.InkListsContain(Narrative.knownStateLists, stateList, true), true);
		this.inkStory.BindExternalFunction<InkList>("not_reached_any", (InkList stateList) => !this.InkListsContain(Narrative.knownStateLists, stateList, false), true);
		this.inkStory.BindExternalFunction<InkList>("reached", (InkList stateList) => this.InkListsContain(((bool)this.inkStory.variablesState["testUsingRecentOnly"]) ? Narrative.recentStateLists : Narrative.knownStateLists, stateList, true), true);
		this.inkStory.BindExternalFunction<InkList>("reached_any", (InkList stateList) => this.InkListsContain(((bool)this.inkStory.variablesState["testUsingRecentOnly"]) ? Narrative.recentStateLists : Narrative.knownStateLists, stateList, false), true);
		this.inkStory.BindExternalFunction<InkList>("IS_IT_POSSIBLE_TO_REACH_PEAK", delegate(InkList peakList)
		{
			KeyValuePair<InkListItem, int> keyValuePair12 = peakList.First<KeyValuePair<InkListItem, int>>();
			if (PropsController.instance.passedProps.Contains(keyValuePair12.Key.itemName))
			{
				return false;
			}
			return true;
		}, false);
		this.inkStory.BindExternalFunction<InkList>("IS_IT_POSSIBLE_TO_REACH_INTERACTERABLE", delegate(InkList interactableID)
		{
			KeyValuePair<InkListItem, int> keyValuePair13 = interactableID.First<KeyValuePair<InkListItem, int>>();
			if (PropsController.instance.passedProps.Contains(keyValuePair13.Key.itemName))
			{
				return false;
			}
			return true;
		}, false);
		this.inkStory.BindExternalFunction("PASSED_PROPS_LIST", delegate
		{
			InkList inkList3 = new InkList();
			foreach (string text6 in PropsController.instance.passedProps)
			{
				InkListItem inkListItem2;
				if (this.TryGetItemNamed(text6, out inkListItem2))
				{
					inkList3.AddItem(inkListItem2.itemName, this.inkStory);
				}
			}
			return inkList3;
		}, false);
		this.inkStory.BindExternalFunction("playthroughIdx", () => Game.instance.playthroughIdx, false);
		this.inkStory.BindExternalFunction("RESET_LEVEL_TRACKING", delegate
		{
			Runner.instance.levelXRange.min = Runner.instance.position.x;
			Runner.instance.levelXRange.max = Runner.instance.position.x;
		}, false);
		this.inkStory.BindExternalFunction<InkList, float, int, int>("GET_NEAREST_PROP_ON_LEVEL", (InkList propsToConsider, float maxRadius, int numToReturn, int allowedLevelsAhead) => this.getNearestPropsFromList(propsToConsider, maxRadius, numToReturn, allowedLevelsAhead, false, false), false);
		this.inkStory.BindExternalFunction<InkList, float, int, int>("GET_NEAREST_UNEXPLORED_PROP_ON_LEVEL", (InkList propsToConsider, float maxRadius, int numToReturn, int allowedLevelsAhead) => this.getNearestPropsFromList(propsToConsider, maxRadius, numToReturn, allowedLevelsAhead, true, false), false);
		this.inkStory.BindExternalFunction<InkList, float, int, int>("GET_NEAREST_EXPLORED_PROP_ON_LEVEL", (InkList propsToConsider, float maxRadius, int numToReturn, int allowedLevelsAhead) => this.getNearestPropsFromList(propsToConsider, maxRadius, numToReturn, allowedLevelsAhead, false, true), false);
		this.inkStory.BindExternalFunction("NEAREST_PEAK", delegate
		{
			Prop prop2 = Prop.NearestMajorPeak(Runner.instance.physicalPosition3d);
			InkList inkList4 = null;
			if (prop2 != null)
			{
				inkList4 = prop2.inkListVar;
			}
			if (inkList4 == null)
			{
				inkList4 = new InkList();
			}
			return inkList4;
		}, false);
		this.inkStory.BindExternalFunction("NEAREST_MAJOR_OR_MINOR_PEAK", delegate
		{
			Prop prop3 = Prop.NearestMajorOrMinorPeak(Runner.instance.physicalPosition3d);
			InkList inkList5 = null;
			if (prop3 != null)
			{
				inkList5 = prop3.inkListVar;
			}
			if (inkList5 == null)
			{
				inkList5 = new InkList();
			}
			return inkList5;
		}, false);
		this.inkStory.BindExternalFunction<InkList>("ON_THE_SLOPES_OF_PEAK", delegate(InkList peakList)
		{
			Prop prop4 = Prop.GetLoadedPropsByInkName(peakList.First<KeyValuePair<InkListItem, int>>().Key.itemName).FirstOrDefault<Prop>();
			if (prop4 == null)
			{
				return false;
			}
			return Vector2.Distance(Runner.instance.position, prop4.transform.position) < 250f;
		}, false);
		this.inkStory.BindExternalFunction<string>("IS_MINOR_PEAK", delegate(string peakListItemName)
		{
			List<Prop> loadedPropsByInkName2 = Prop.GetLoadedPropsByInkName(peakListItemName);
			int num2 = loadedPropsByInkName2.Count<Prop>();
			if (num2 > 0)
			{
				if (num2 > 1)
				{
					Debug.LogWarning("Several props with ink name " + peakListItemName + ". Returning IS_MINOR_PEAK at the first one found.");
				}
				return loadedPropsByInkName2.First<Prop>().isMinorPeak;
			}
			return false;
		}, false);
		this.inkStory.BindExternalFunction<InkList>("getPathDuration", (InkList pathID) => Prop.FindNearestByInkName(Runner.instance.transform.position, pathID.singleItem.itemName).pathTravelTimeHours, false);
		this.inkStory.BindExternalFunction<InkList>("pathIsForward", delegate(InkList pathID)
		{
			Prop prop5 = Prop.FindNearestByInkName(Runner.instance.transform.position, pathID.singleItem.itemName);
			GameObject gameObject = prop5.pathDestination.gameObject;
			if (gameObject == null)
			{
				Debug.LogError("Error when running ink external function 'pathIsForward' - path '" + prop5.name + "' has no destination?", prop5);
				return false;
			}
			return prop5.transform.position.z < gameObject.transform.position.z;
		}, false);
		this.inkStory.BindExternalFunction<InkList>("PEAK_LEVEL", delegate(InkList peak)
		{
			if (peak.Count == 0)
			{
				Debug.LogWarning("Ink tried to call PEAK_LEVEL with an empty peak variable at " + this.inkStory.state.callStack.callStackTrace);
				return -1;
			}
			Prop prop6 = Prop.GetLoadedPropsByInkName(peak.First<KeyValuePair<InkListItem, int>>().Key.itemName).FirstOrDefault<Prop>();
			if (prop6 == null)
			{
				return -1;
			}
			return Level.DepthToIndex(prop6.transform.position.z) + 1;
		}, false);
		this.inkStory.BindExternalFunction<InkList>("PLAYER_IN_ZONE", delegate(InkList inkZone)
		{
			if (inkZone.IsEmpty<KeyValuePair<InkListItem, int>>())
			{
				return false;
			}
			Runner runner = Runner.instance;
			List<Prop> loadedPropsByInkName3 = Prop.GetLoadedPropsByInkName(inkZone.First<KeyValuePair<InkListItem, int>>().Key.itemName);
			if (loadedPropsByInkName3.Count<Prop>() == 0)
			{
				return false;
			}
			return loadedPropsByInkName3.Any((Prop t) => t.triggerZone.InsideTriggerDist(runner.position, (float)runner.physicalDepthLayerIdx, 1f));
		}, false);
		this.inkStory.BindExternalFunction<InkList>("GET_SECONDARY_MAP_TARGET", delegate(InkList mapList)
		{
			if (mapList.Count == 0)
			{
				return new InkList();
			}
			string mapInkName = mapList.First<KeyValuePair<InkListItem, int>>().Key.itemName;
			Map map = Map.all.FirstOrDefault((Map t) => t.targetInkPropName == mapInkName);
			if (map == null)
			{
				Debug.LogError("GET_SECONDARY_MAP_TARGET: Map not found with name: " + mapInkName);
				return new InkList();
			}
			string secondaryTargetInkPropName = map.secondaryTargetInkPropName;
			if (string.IsNullOrEmpty(secondaryTargetInkPropName))
			{
				return new InkList();
			}
			Prop prop7 = Prop.GetLoadedPropsByInkName(secondaryTargetInkPropName).FirstOrDefault<Prop>();
			if (prop7 == null)
			{
				return new InkList();
			}
			if (prop7.inkListVar == null)
			{
				Debug.LogError(string.Concat(new string[] { "GET_SECONDARY_MAP_TARGET: Secondary target Prop named ", secondaryTargetInkPropName, " exists but its ink list item ", prop7.inkListItemName, " doesn't exist in ink" }));
				return new InkList();
			}
			return prop7.inkListVar;
		}, false);
		this.inkStory.BindExternalFunction("exhausted", () => Runner.instance.staminaIsVeryLow, false);
		this.inkStory.BindExternalFunction("dayNumber", () => GameClock.instance.dayNumber, false);
		this.inkStory.BindExternalFunction<int>("rewindDays", delegate(int daysToSubtract)
		{
			GameClock.instance.daysNorm -= (float)daysToSubtract;
			GameClock.instance.hourOfDay = Mathf.Max(7f, GameClock.instance.hourOfDay - 2f);
		}, false);
		this.inkStory.BindExternalFunction<InkList>("isEasterEgg", delegate(InkList propNameList)
		{
			if (propNameList.IsEmpty<KeyValuePair<InkListItem, int>>())
			{
				return false;
			}
			string itemName5 = propNameList.First<KeyValuePair<InkListItem, int>>().Key.itemName;
			return this.MapIsEasterEgg(itemName5);
		}, false);
		this.inkStory.BindExternalFunction("TryJumpOnChairLift", () => Runner.instance.TryJumpOnChairLift(), false);
		this.inkStory.onError += this.OnInkError;
		this._inCave = CaveRegion.inCave;
		ListDefinition listDefinition;
		if (this.inkStory.listDefinitions.TryListGetDefinition("Achievements", out listDefinition))
		{
			MonoSingleton<AchievementsManager>.instance.SetupAchievementsFromInk(listDefinition);
		}
		else
		{
			Debug.LogError("No 'Achievements' list found in ink?");
		}
		if (Narrative.onCreateStory != null)
		{
			Narrative.onCreateStory(this.inkStory);
		}
	}

	// Token: 0x1700012F RID: 303
	// (get) Token: 0x0600042C RID: 1068 RVA: 0x00022409 File Offset: 0x00020609
	private bool paused
	{
		get
		{
			return this._pauseReason > Narrative.PauseReason.None;
		}
	}

	// Token: 0x0600042D RID: 1069 RVA: 0x00022414 File Offset: 0x00020614
	public void SetPaused(Narrative.PauseReason reason, bool wantsPause)
	{
		bool paused = this.paused;
		if (wantsPause)
		{
			this._pauseReason |= reason;
		}
		else
		{
			this._pauseReason &= ~reason;
		}
		if (this.paused == paused)
		{
			return;
		}
		NarrativePresenter.instance.SetPaused(this.paused);
	}

	// Token: 0x0600042E RID: 1070 RVA: 0x00022464 File Offset: 0x00020664
	private InkList getNearestPropsFromList(InkList propsToConsider, float maxRadius, int numToReturn, int allowedLevelsAhead, bool preferUnexploredXRange = false, bool onlyExploredRange = false)
	{
		float runnerX = Runner.instance.transform.position.x;
		Range runnerLevelX = Runner.instance.levelXRange;
		IEnumerable<Prop> enumerable = from kv in propsToConsider.Keys
			select Prop.FindNearestByInkName(Runner.instance.transform.position, kv.itemName) into x
			where x != null
			select x into t
			where !onlyExploredRange || runnerLevelX.Contains(t.transform.position.x)
			where Math.Abs(runnerX - t.transform.position.x) <= maxRadius
			select t into x
			where x.levelIdx <= Runner.instance.levelIdx + allowedLevelsAhead
			select x into t
			orderby Vector3.Distance(Runner.instance.transform.position, t.transform.position) * base.<getNearestPropsFromList>g__searchCostFunction|0(t)
			select t;
		InkList inkList = new InkList();
		foreach (Prop prop in enumerable)
		{
			if (numToReturn == 0)
			{
				break;
			}
			if ((prop.levelIdx != 1 && prop.levelIdx != 2) || Runner.instance.levelIdx > 2 || ((runnerX >= -44f || prop.transform.position.x <= -44f) && (prop.transform.position.x >= -44f || runnerX <= -44f)))
			{
				inkList.AddItem(prop.inkListItemName, this.inkStory);
				numToReturn--;
			}
		}
		return inkList;
	}

	// Token: 0x0600042F RID: 1071 RVA: 0x00022618 File Offset: 0x00020818
	public void SetFallDamageInInk(string fallDamageName)
	{
		this.inkStory.variablesState["HarmLevel"] = InkList.FromString(fallDamageName, this.inkStory);
	}

	// Token: 0x06000430 RID: 1072 RVA: 0x0002263C File Offset: 0x0002083C
	private string PairStoreString(string x1, string x2, string relationName, bool appendFinalDelimiter = true)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(x1);
		stringBuilder.Append(Narrative._pairStoreElementDelimiter[0]);
		stringBuilder.Append(relationName);
		stringBuilder.Append(Narrative._pairStoreElementDelimiter[0]);
		stringBuilder.Append(x2);
		if (appendFinalDelimiter)
		{
			stringBuilder.Append(Narrative._pairStoreStringDelimiter[0]);
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06000431 RID: 1073 RVA: 0x0002269C File Offset: 0x0002089C
	private void RebuildCachedPairStore(bool clearAndRebuild = false)
	{
		if (clearAndRebuild)
		{
			this._cachedPairStore.Clear();
			this._reversedCachedPairStore.Clear();
		}
		if (this._cachedPairStore.IsEmpty<KeyValuePair<string, Dictionary<string, List<string>>>>())
		{
			string[] array = ((string)this.inkStory.variablesState["pairstore"]).Split(Narrative._pairStoreStringDelimiter, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split(Narrative._pairStoreElementDelimiter, StringSplitOptions.RemoveEmptyEntries);
				this.LinkInRelationCache(array2[0], array2[1], array2[2]);
			}
		}
	}

	// Token: 0x06000432 RID: 1074 RVA: 0x00022724 File Offset: 0x00020924
	private void LinkInRelationCache(string x1, string relationName, string x2)
	{
		if (!this._cachedPairStore.ContainsKey(relationName))
		{
			this._cachedPairStore[relationName] = new Dictionary<string, List<string>>();
			this._reversedCachedPairStore[relationName] = new Dictionary<string, List<string>>();
		}
		if (!this._cachedPairStore[relationName].ContainsKey(x1))
		{
			this._cachedPairStore[relationName][x1] = new List<string>();
		}
		this._cachedPairStore[relationName][x1].Add(x2);
		if (!this._reversedCachedPairStore[relationName].ContainsKey(x2))
		{
			this._reversedCachedPairStore[relationName][x2] = new List<string>();
		}
		this._reversedCachedPairStore[relationName][x2].Add(x1);
	}

	// Token: 0x06000433 RID: 1075 RVA: 0x000227E8 File Offset: 0x000209E8
	private bool IsRelatedPairInPairStoreString(InkListItem x1, InkListItem x2, string relationName)
	{
		this.RebuildCachedPairStore(false);
		return this._cachedPairStore.ContainsKey(relationName) && this._cachedPairStore[relationName].ContainsKey(x1.itemName) && this._cachedPairStore[relationName][x1.itemName].Contains(x2.itemName);
	}

	// Token: 0x06000434 RID: 1076 RVA: 0x0002284C File Offset: 0x00020A4C
	private InkList GetRelatedElementsInPairStoreString(InkList keyList, string relationName, bool returnLHS)
	{
		new List<string>();
		InkList inkList = new InkList();
		this.RebuildCachedPairStore(false);
		Dictionary<string, Dictionary<string, List<string>>> dictionary = (returnLHS ? this._reversedCachedPairStore : this._cachedPairStore);
		if (dictionary.ContainsKey(relationName))
		{
			foreach (KeyValuePair<InkListItem, int> keyValuePair in keyList)
			{
				if (dictionary[relationName].ContainsKey(keyValuePair.Key.itemName))
				{
					foreach (string text in dictionary[relationName][keyValuePair.Key.itemName])
					{
						inkList.AddItem(text, this.inkStory);
					}
				}
			}
		}
		return inkList;
	}

	// Token: 0x06000435 RID: 1077 RVA: 0x0002293C File Offset: 0x00020B3C
	public bool MapWantsToBeFound(string mapTargetInkName)
	{
		InkList inkList = this.InkListWithItemNamed(mapTargetInkName);
		this.FinishAsyncIfNecessary();
		return (bool)this.inkStory.EvaluateFunction("mapWantsToBeFound", new object[] { inkList });
	}

	// Token: 0x06000436 RID: 1078 RVA: 0x00022978 File Offset: 0x00020B78
	public string GetWeatherDescription(string propIDName)
	{
		if (this.inkStory == null)
		{
			return "";
		}
		this.FinishAsyncIfNecessary();
		string text;
		this.inkStory.EvaluateFunction("getWeatherDescription", out text, new object[] { propIDName ?? "" });
		return text.Trim();
	}

	// Token: 0x06000437 RID: 1079 RVA: 0x000229CC File Offset: 0x00020BCC
	public string GetSleepComfortLevel(string propIDName)
	{
		if (this.inkStory == null)
		{
			return "";
		}
		this.FinishAsyncIfNecessary();
		string text;
		this.inkStory.EvaluateFunction("getSleepComfortLevelDescription", out text, new object[] { propIDName ?? "" });
		return text.Trim();
	}

	// Token: 0x06000438 RID: 1080 RVA: 0x00022A1F File Offset: 0x00020C1F
	public string ChooseBeatTrack(string runName, float normalisedTrackDuration)
	{
		this.FinishAsyncIfNecessary();
		return ((InkList)this.inkStory.EvaluateFunction("jukebox", new object[]
		{
			InkList.FromString(runName, this.inkStory),
			normalisedTrackDuration
		})).ToString();
	}

	// Token: 0x06000439 RID: 1081 RVA: 0x00022A60 File Offset: 0x00020C60
	public bool TryLoadState(string storyStateJSON)
	{
		if (storyStateJSON == null)
		{
			return false;
		}
		bool flag;
		try
		{
			this.inkStory.state.LoadJson(storyStateJSON);
			if (Narrative.onLoadState != null)
			{
				Narrative.onLoadState(this.inkStory);
			}
			this.RebuildCachedPairStore(true);
			flag = true;
		}
		catch
		{
			Debug.LogWarning("Failed loading story state! Resetting state.");
			this.inkStory.ResetState();
			flag = false;
		}
		return flag;
	}

	// Token: 0x0600043A RID: 1082 RVA: 0x00022AD4 File Offset: 0x00020CD4
	private void PrepareInkState(string optionalInteractableName)
	{
		this.inkStory.variablesState["hour"] = Mathf.FloorToInt(GameClock.instance.hourOfDay);
		this.inkStory.variablesState["level"] = Level.currentIndex + 1;
		this.inkStory.variablesState["health"] = Runner.instance.health.currentHealth;
		this.inkStory.variablesState["currentMaxHealth"] = Runner.instance.health.currentMaxHealth;
		this.inkStory.variablesState["maxHealth"] = 16f;
		this.inkStory.variablesState["TorchBatteryLevel"] = Runner.instance.torch.batteryLevel;
		this.inkStory.variablesState["inCave"] = CaveRegion.inCave;
		this.inkStory.variablesState["inWater"] = Runner.instance.position.y < 0f;
		this.inkStory.variablesState["profanity"] = Narrative.profanity;
		this.inkStory.variablesState["christmas"] = Game.isChristmas;
		this.inkStory.variablesState["DEMO_TRIBECA_EXHIBITION"] = MonoSingleton<BuildSetupManager>.instance.setup.inkTribecaExhibitionSetup;
		this.inkStory.variablesState["DEMO_SHORT_INTRO"] = MonoSingleton<BuildSetupManager>.instance.setup.fastIntro;
		this.inkStory.variablesState["DEBUG_GAMEPLAY_DEMO_MAX_LEVEL"] = MonoSingleton<BuildSetupManager>.instance.setup.levelLimit;
		this.inkStory.variablesState["interruptAllowed"] = false;
		this.SyncMapsInInventory();
		InkList inkList = new InkList();
		if (optionalInteractableName != null)
		{
			ListValue listValue = this.inkStory.listDefinitions.FindSingleItemListWithName(optionalInteractableName);
			if (listValue == null)
			{
				Debug.LogError("Ink list item '" + optionalInteractableName + "' not found");
				return;
			}
			foreach (KeyValuePair<InkListItem, int> keyValuePair in listValue.value)
			{
				inkList.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}
		this.inkStory.variablesState["gameChosenInteractable"] = inkList;
		this._activeInteractableChoices.Clear();
	}

	// Token: 0x0600043B RID: 1083 RVA: 0x00022D9C File Offset: 0x00020F9C
	public void RunInkWithChoiceIndex(int choiceIdx, string interactableName = null)
	{
		if (this._inkEvaluationCoroutine != null)
		{
			Debug.LogError("Expected ink evaluation to be inactive in order to begin a choice!");
		}
		this.PrepareInkState(interactableName);
		this._inkEvaluationCoroutineContext = "Running Ink with choice " + this.inkStory.currentChoices[choiceIdx].text;
		this.ChooseChoiceIndex(choiceIdx);
		this._inkEvaluationCoroutine = this.InkEvaluationCoroutine(false);
		base.StartCoroutine(this._inkEvaluationCoroutine);
	}

	// Token: 0x0600043C RID: 1084 RVA: 0x00022E09 File Offset: 0x00021009
	private void ContinueInkWithChoiceIndex(int choiceIdx, string interactableName = null)
	{
		if (this._inkEvaluationCoroutine == null)
		{
			Debug.LogError("Expected ink evaluation to be active in order to choose a choice!");
		}
		this.PrepareInkState(interactableName);
		this.ChooseChoiceIndex(choiceIdx);
	}

	// Token: 0x0600043D RID: 1085 RVA: 0x00022E2C File Offset: 0x0002102C
	private void ChooseChoiceIndex(int choiceIdx)
	{
		Choice choice = this.inkStory.currentChoices[choiceIdx];
		this.debugInkEvaluationStartPoint = "CHOICE: " + choice.text + " -> " + choice.pathStringOnChoice;
		this.inkStory.ChooseChoiceIndex(choiceIdx);
	}

	// Token: 0x0600043E RID: 1086 RVA: 0x00022E78 File Offset: 0x00021078
	private void OnInkError(string message, ErrorType type)
	{
		string text = ((type == ErrorType.Warning) ? "INK WARNING: " : "INK ERROR: ") + message;
		if (!string.IsNullOrWhiteSpace(this.debugInkEvaluationStartPoint))
		{
			text = text + " while running: " + this.debugInkEvaluationStartPoint;
		}
		string text2 = this.inkStory.state.currentPathString;
		if (string.IsNullOrWhiteSpace(text2))
		{
			text2 = this.inkStory.state.previousPathString;
		}
		if (!string.IsNullOrWhiteSpace(text2))
		{
			text = text + " error at: " + text2;
		}
		if (type == ErrorType.Error)
		{
			Debug.LogError(text);
		}
		this._hadError = true;
		this._ranOutOfContent = message.Contains("unexpectedly reached end of content");
	}

	// Token: 0x0600043F RID: 1087 RVA: 0x00022F1C File Offset: 0x0002111C
	public void RunFunctionAndParseOutput(string functionName, params object[] arguments)
	{
		this.FinishAsyncIfNecessary();
		if (!this._clearing)
		{
			this.PrepareInkState(null);
		}
		string text;
		Narrative.instance.inkStory.EvaluateFunction(functionName, out text, arguments);
		if (!string.IsNullOrWhiteSpace(text))
		{
			string[] array = InkInstructionParserUtility.SplitTextIntoLines(text);
			if (array.Length != 0)
			{
				if (this._inkEvaluationCoroutine != null)
				{
					Debug.LogError(string.Concat(new string[] { "Ink was already active when RunFunctionAndParseOutput was called, returning lines to read! Was previously '", this.debugInkEvaluationStartPoint, "' when wanted ", functionName, ". Can you check narrative.isBusy first?" }));
					return;
				}
				Narrative.RunCoroutineSync(this.ProcessInkLines(array));
			}
		}
	}

	// Token: 0x06000440 RID: 1088 RVA: 0x00022FB0 File Offset: 0x000211B0
	public void RunKnot(string inkPath, string interactableName = null, bool refreshingInteractables = false, bool allowInterject = false, params object[] arguments)
	{
		if (!allowInterject && this._inkEvaluationCoroutine != null)
		{
			Debug.LogError(string.Concat(new string[] { "Ink was already active when RunKnot was called! Was previously '", this.debugInkEvaluationStartPoint, "' when wanted ", inkPath, ". Can you check narrative.isBusy first?" }));
			return;
		}
		if (allowInterject && this._inkEvaluationCoroutine != null && NarrativePresenter.instance.hasAnyChoices)
		{
			NarrativePresenter.instance.ClearChoiceAndPropWidgets();
		}
		this.PrepareInkState(interactableName);
		this.inkStory.ChoosePathString(inkPath, true, arguments);
		this.debugInkEvaluationStartPoint = inkPath;
		if (!this._inkStory.asyncContinueComplete)
		{
			this._inkStory.Continue();
		}
		if (this._inkEvaluationCoroutine != null)
		{
			return;
		}
		this._inkEvaluationCoroutineContext = "Running knot " + inkPath;
		this._inkEvaluationCoroutine = this.InkEvaluationCoroutine(refreshingInteractables);
		base.StartCoroutine(this._inkEvaluationCoroutine);
	}

	// Token: 0x06000441 RID: 1089 RVA: 0x0002308B File Offset: 0x0002128B
	public void RunKnotInterject(string inkPath)
	{
		this.RunKnot(inkPath, null, false, true, Array.Empty<object>());
	}

	// Token: 0x06000442 RID: 1090 RVA: 0x0002309C File Offset: 0x0002129C
	public void ForceComplete()
	{
		this._forcingCoroutineSyncComplete = true;
		Narrative.RunCoroutineSync(this._inkEvaluationCoroutine);
		this._forcingCoroutineSyncComplete = false;
	}

	// Token: 0x06000443 RID: 1091 RVA: 0x000230B8 File Offset: 0x000212B8
	private static Coroutine RunCoroutineSync(IEnumerator func)
	{
		while (func.MoveNext())
		{
			if (func.Current != null)
			{
				Narrative.RunCoroutineSync((IEnumerator)func.Current);
			}
		}
		return null;
	}

	// Token: 0x06000444 RID: 1092 RVA: 0x000230E0 File Offset: 0x000212E0
	private string SingleItemDescription(string functionName, string listItemName)
	{
		InkList inkList = this.InkListWithItemNamed(listItemName);
		this.FinishAsyncIfNecessary();
		string text;
		this.inkStory.EvaluateFunction(functionName, out text, new object[] { inkList });
		if (string.IsNullOrWhiteSpace(text))
		{
			return null;
		}
		return InkStylingUtility.ProcessText(text, true, false);
	}

	// Token: 0x06000445 RID: 1093 RVA: 0x00023126 File Offset: 0x00021326
	private void FinishAsyncIfNecessary()
	{
		if (!this.inkStory.asyncContinueComplete)
		{
			this.inkStory.Continue();
		}
	}

	// Token: 0x06000446 RID: 1094 RVA: 0x00023141 File Offset: 0x00021341
	public string DiscoveryDescription(string discoveryInkId)
	{
		return this.SingleItemDescription("DiscoveryDescription", discoveryInkId);
	}

	// Token: 0x06000447 RID: 1095 RVA: 0x0002314F File Offset: 0x0002134F
	public string ItemDescription(string itemInkId)
	{
		return this.SingleItemDescription("a", itemInkId);
	}

	// Token: 0x06000448 RID: 1096 RVA: 0x0002315D File Offset: 0x0002135D
	public List<Narrative.InkItemDescription> ProduceAllDiscoveries()
	{
		return this.ProduceDescriptions("DiscoveredFacts", "DiscoveredFacts", "DiscoveryDescription");
	}

	// Token: 0x06000449 RID: 1097 RVA: 0x00023174 File Offset: 0x00021374
	public List<Narrative.InkItemDescription> ProduceAllInventory()
	{
		return this.ProduceDescriptions("Inventory", "Inventory", "a");
	}

	// Token: 0x0600044A RID: 1098 RVA: 0x0002318C File Offset: 0x0002138C
	private List<Narrative.InkItemDescription> ProduceDescriptions(string inkListName, string listOriginName, string inkDescriptionFunction)
	{
		this.FinishAsyncIfNecessary();
		this._inkItemDescriptionList.Clear();
		Dictionary<InkListItem, int> dictionary = (InkList)this.inkStory.variablesState[inkListName];
		InkList inkList = new InkList(listOriginName, this.inkStory);
		foreach (KeyValuePair<InkListItem, int> keyValuePair in dictionary)
		{
			inkList.Add(keyValuePair.Key, keyValuePair.Value);
			string text;
			this.inkStory.EvaluateFunction(inkDescriptionFunction, out text, new object[] { inkList });
			if (!string.IsNullOrWhiteSpace(text))
			{
				text = InkStylingUtility.ProcessText(text, true, false);
				text = InkStylingUtility.ParseStyling(text, true, "#FFE473");
				this._inkItemDescriptionList.Add(new Narrative.InkItemDescription
				{
					inkItemName = keyValuePair.Key.itemName,
					description = text,
					inkListValue = keyValuePair.Value
				});
			}
			inkList.Clear();
		}
		this._inkItemDescriptionList.OrderBy(delegate(Narrative.InkItemDescription item)
		{
			int num;
			if (this.orderOfAquisitionIdx.TryGetValue(item.inkItemName, out num))
			{
				return num;
			}
			return item.inkListValue - 100000;
		});
		return this._inkItemDescriptionList;
	}

	// Token: 0x0600044B RID: 1099 RVA: 0x000232BC File Offset: 0x000214BC
	public string MapDescriptionForJournal(string mapPropName)
	{
		ListValue listValue = this.inkStory.listDefinitions.FindSingleItemListWithName(mapPropName);
		this.FinishAsyncIfNecessary();
		string text;
		this.inkStory.EvaluateFunction("TEXT_MapJournalBlurb", out text, new object[] { listValue.value, true });
		text = InkStylingUtility.ProcessText(text, true, false);
		text = InkStylingUtility.ParseStyling(text, true, "#FFE473");
		return text;
	}

	// Token: 0x0600044C RID: 1100 RVA: 0x00023324 File Offset: 0x00021524
	public List<Narrative.NamedPeakWidget> ProducePeakWidgetContent()
	{
		this._namedPeakWidgetScratchList.Clear();
		InkList inkList = new InkList("Paths", Narrative.instance.inkStory);
		foreach (KeyValuePair<InkListItem, int> keyValuePair in ((InkList)this._inkStory.variablesState["mapsConfirmed"]))
		{
			string itemName = keyValuePair.Key.itemName;
			Prop prop3 = Prop.FindNearestByInkName(Runner.instance.physicalPosition3d, itemName);
			if (prop3 != null && prop3.isPathOut && !prop3.pathIsLocal)
			{
				inkList.Add(keyValuePair.Key, keyValuePair.Value);
				string text;
				this.inkStory.EvaluateFunction("PathNameForPathSpotting", out text, new object[] { inkList });
				text = InkStylingUtility.ProcessText(text, false, false);
				inkList.Clear();
				this._namedPeakWidgetScratchList.Add(new Narrative.NamedPeakWidget
				{
					prop = prop3,
					name = text,
					hasMap = true
				});
			}
		}
		foreach (string text2 in PropsController.instance.foundPaths)
		{
			Prop prop = Prop.FindNearestByInkName(Runner.instance.physicalPosition3d, text2);
			if (prop != null && prop.isPathOut && !prop.pathIsLocal && !this._namedPeakWidgetScratchList.Exists((Narrative.NamedPeakWidget widget) => widget.prop == prop))
			{
				foreach (KeyValuePair<InkListItem, int> keyValuePair2 in this.inkStory.listDefinitions.FindSingleItemListWithName(text2).value)
				{
					inkList.Add(keyValuePair2.Key, keyValuePair2.Value);
				}
				string text3;
				this.inkStory.EvaluateFunction("PathNameForPathSpotting", out text3, new object[] { inkList });
				text3 = InkStylingUtility.ProcessText(text3, false, false);
				inkList.Clear();
				this._namedPeakWidgetScratchList.Add(new Narrative.NamedPeakWidget
				{
					prop = prop,
					name = text3,
					hasMap = false
				});
			}
		}
		Dictionary<InkListItem, int> dictionary = (InkList)this.inkStory.variablesState["KnownPeaks"];
		InkList inkList2 = (InkList)this.inkStory.variablesState["PeaksWithoutNames"];
		InkList inkList3 = new InkList("Peaks", this.inkStory);
		foreach (KeyValuePair<InkListItem, int> keyValuePair3 in dictionary)
		{
			Prop prop2 = Prop.FindNearestByInkName(Runner.instance.physicalPosition3d, keyValuePair3.Key.itemName);
			if (!(prop2 == null) && !inkList2.ContainsItemNamed(keyValuePair3.Key.itemName))
			{
				inkList3.Add(keyValuePair3.Key, keyValuePair3.Value);
				string text4;
				this.inkStory.EvaluateFunction("getPeakName", out text4, new object[] { inkList3, true });
				text4 = InkStylingUtility.ProcessText(text4, false, false);
				this._namedPeakWidgetScratchList.Add(new Narrative.NamedPeakWidget
				{
					prop = prop2,
					name = text4.Trim(),
					hasMap = false
				});
				inkList3.Clear();
			}
		}
		return this._namedPeakWidgetScratchList;
	}

	// Token: 0x0600044D RID: 1101 RVA: 0x0002374C File Offset: 0x0002194C
	public List<string> GetNonMapPathsTakenByPlayerInThePast()
	{
		List<string> list = new List<string>();
		ListDefinition listDefinition;
		this.inkStory.listDefinitions.TryListGetDefinition("Paths", out listDefinition);
		foreach (KeyValuePair<InkListItem, int> keyValuePair in ((InkList)this.inkStory.variablesState["RedundantMaps"]))
		{
			InkListItem key = keyValuePair.Key;
			if (listDefinition.items.ContainsKey(key))
			{
				list.Add(key.itemName);
			}
		}
		return list;
	}

	// Token: 0x0600044E RID: 1102 RVA: 0x000237F0 File Offset: 0x000219F0
	public List<Narrative.PeakKnowledge> ProduceAllPeakKnowledge()
	{
		return this.ProducePeakKnowledge_Internal(null);
	}

	// Token: 0x0600044F RID: 1103 RVA: 0x000237FC File Offset: 0x000219FC
	public Narrative.PeakKnowledge ProducePeakKnowledge(string peakInkName)
	{
		List<Narrative.PeakKnowledge> list = this.ProducePeakKnowledge_Internal(peakInkName);
		if (list.Count > 0)
		{
			return list[0];
		}
		return default(Narrative.PeakKnowledge);
	}

	// Token: 0x06000450 RID: 1104 RVA: 0x0002382C File Offset: 0x00021A2C
	private List<Narrative.PeakKnowledge> ProducePeakKnowledge_Internal(string specificPeakNameOnly = null)
	{
		this.FinishAsyncIfNecessary();
		this._peakKnowledgeList.Clear();
		Dictionary<InkListItem, int> dictionary;
		if (specificPeakNameOnly == null)
		{
			if (this._allPeaks == null)
			{
				ListDefinition listDefinition;
				this.inkStory.listDefinitions.TryListGetDefinition("Peaks", out listDefinition);
				this._allPeaks = listDefinition.items;
			}
			dictionary = this._allPeaks;
		}
		else
		{
			dictionary = this.inkStory.listDefinitions.FindSingleItemListWithName(specificPeakNameOnly).value;
		}
		InkList inkList = (InkList)this.inkStory.variablesState["KnownPeaks"];
		InkList inkList2 = (InkList)this.inkStory.variablesState["BaggedPeaks"];
		InkList inkList3 = (InkList)this.inkStory.variablesState["PeaksNotToJournal"];
		InkList inkList4 = (InkList)this.inkStory.variablesState["BlessedPeaks"];
		InkList inkList5 = new InkList("Peaks", this.inkStory);
		foreach (KeyValuePair<InkListItem, int> keyValuePair in dictionary)
		{
			inkList5.Add(keyValuePair.Key, keyValuePair.Value);
			if (!inkList3.ContainsKey(keyValuePair.Key))
			{
				string text;
				this.inkStory.EvaluateFunction("getPeakName", out text, new object[] { inkList5, true });
				string text2;
				this.inkStory.EvaluateFunction("getPeakName", out text2, new object[] { inkList5, false });
				text = InkStylingUtility.ProcessText(text, true, false);
				text2 = InkStylingUtility.ProcessText(text2, true, false);
				this._peakKnowledgeList.Add(new Narrative.PeakKnowledge
				{
					visited = inkList2.ContainsKey(keyValuePair.Key),
					inkItemName = keyValuePair.Key.itemName,
					englishName = text.Trim(),
					gaelicName = text2.Trim(),
					correctlyNamed = (inkList.ContainsKey(keyValuePair.Key) && !string.IsNullOrWhiteSpace(text)),
					blessed = inkList4.ContainsKey(keyValuePair.Key)
				});
			}
			inkList5.Clear();
		}
		return this._peakKnowledgeList;
	}

	// Token: 0x06000451 RID: 1105 RVA: 0x00023A94 File Offset: 0x00021C94
	private bool RunKnotWithNameListItemAllowQueue(string knotName, string listItemName, bool itemIsOptional = false, params object[] arguments)
	{
		string[] array = new string[] { listItemName };
		return this.RunKnotWithNameListItemAllowQueue(knotName, array, itemIsOptional, arguments);
	}

	// Token: 0x06000452 RID: 1106 RVA: 0x00023AB7 File Offset: 0x00021CB7
	private bool RunKnotAllowQueue(string knotName, params object[] arguments)
	{
		return this.RunKnotWithNameListItemAllowQueue(knotName, Narrative._emptyListItemNames, true, arguments);
	}

	// Token: 0x06000453 RID: 1107 RVA: 0x00023AC8 File Offset: 0x00021CC8
	private bool RunKnotWithNameListItemAllowQueue(string knotName, string[] listItemNames, bool itemIsOptional = false, params object[] arguments)
	{
		InkList inkList = new InkList();
		if (listItemNames.IsEmpty<string>())
		{
			if (!itemIsOptional)
			{
				Debug.LogError("List item is blank but it's non-optional for the knot '" + knotName + "'");
				return false;
			}
		}
		else
		{
			foreach (string text in listItemNames)
			{
				ListValue listValue = this.inkStory.listDefinitions.FindSingleItemListWithName(text);
				if (listValue == null)
				{
					if (!string.IsNullOrWhiteSpace(text))
					{
						Debug.LogError(string.Concat(new string[] { "List item not found in ink: ", text, " when running knot '", knotName, "'" }));
					}
					if (!itemIsOptional)
					{
						return false;
					}
				}
				else
				{
					inkList = inkList.Union(listValue.value);
				}
			}
		}
		object[] array = new object[arguments.Length + 1];
		array[0] = inkList;
		for (int j = 0; j < arguments.Length; j++)
		{
			array[j + 1] = arguments[j];
		}
		Narrative.QueuedKnot knotWithArgs = new Narrative.QueuedKnot
		{
			knotName = knotName,
			arguments = array
		};
		if (this.isBusy)
		{
			if (!this.canInterrupt)
			{
				if (!knotWithArgs.Equals(this._activeQueueableKnot) && !this._queuedKnots.Exists((Narrative.QueuedKnot k) => k.Equals(knotWithArgs)))
				{
					this._queuedKnots.Add(new Narrative.QueuedKnot
					{
						knotName = knotName,
						arguments = array
					});
				}
				return true;
			}
			this.ClearForInterrupt();
		}
		this._activeQueueableKnot = knotWithArgs;
		this.RunKnot(knotName, null, false, false, array);
		return true;
	}

	// Token: 0x06000454 RID: 1108 RVA: 0x00023C5E File Offset: 0x00021E5E
	public void ClearForInterrupt()
	{
		this.CancelCoroutine();
		this.inkStory.ResetCallstack();
		this._activeInteractableChoices.Clear();
		this._lastInteractableNames.Clear();
		NarrativePresenter.instance.Clear(false, true);
	}

	// Token: 0x06000455 RID: 1109 RVA: 0x00023C93 File Offset: 0x00021E93
	public void OnCompleteLoad()
	{
		this.UpdateWeather(true);
		this.inkStory.variablesState["justLoaded"] = true;
		this.RefreshInteractablesChoices(true, false);
		this._inCave = CaveRegion.inCave;
	}

	// Token: 0x06000456 RID: 1110 RVA: 0x00023CCA File Offset: 0x00021ECA
	public void RefreshPeakWeatherModifierZones()
	{
		this.RunFunctionAndParseOutput("mollifyAllPeaks", Array.Empty<object>());
	}

	// Token: 0x06000457 RID: 1111 RVA: 0x00023CDC File Offset: 0x00021EDC
	public void ReCheckAchievements()
	{
		foreach (InkListItem inkListItem in ((InkList)this.inkStory.variablesState["Achievements"]).Keys)
		{
			string itemName = inkListItem.itemName;
			MonoSingleton<AchievementsManager>.instance.UnlockWithInkID(itemName);
		}
	}

	// Token: 0x06000458 RID: 1112 RVA: 0x00023D54 File Offset: 0x00021F54
	public void RefreshInteractablesChoices(bool immediate, bool nearbyPropListHasAlreadyJustBeenUpdated = false)
	{
		if (this.isInkBusy)
		{
			Debug.LogError("Could not refresh interactables because ink was still busy. Should check !narrative.isBusy before calling RefreshInteractablesChoices.");
			return;
		}
		if (MonoSingleton<RestStateController>.instance.active)
		{
			return;
		}
		this._activeInteractableChoices.Clear();
		this._lastInteractableNames.Clear();
		this._lastInteractablesRefreshPropList.Clear();
		if (!nearbyPropListHasAlreadyJustBeenUpdated)
		{
			this.RefreshNearbyInteractablePropList();
		}
		if (this._availableInteractableInkItems_Scratch == null)
		{
			this._availableInteractableInkItems_Scratch = new InkList();
		}
		else
		{
			this._availableInteractableInkItems_Scratch.Clear();
		}
		foreach (Prop prop in this._nearbyPropsScratch)
		{
			ListValue listValue = this.inkStory.listDefinitions.FindSingleItemListWithName(prop.inkListItemName);
			if (listValue == null)
			{
				if (!string.IsNullOrWhiteSpace(prop.inkListItemName))
				{
					Debug.LogError(string.Concat(new string[]
					{
						"Ink list item '",
						prop.inkListItemName,
						"' not found for prop '",
						prop.gameObject.name,
						"'"
					}), prop);
				}
			}
			else
			{
				foreach (KeyValuePair<InkListItem, int> keyValuePair in listValue.value)
				{
					this._availableInteractableInkItems_Scratch[keyValuePair.Key] = keyValuePair.Value;
				}
			}
		}
		if (this._availableInteractableInkItems_Scratch.Count == 0)
		{
			if (Narrative.onDidRefreshInteractables != null)
			{
				Narrative.onDidRefreshInteractables();
			}
			return;
		}
		this.RunKnot("HUB_interactable", null, true, false, new object[] { this._availableInteractableInkItems_Scratch });
		if (immediate)
		{
			while (this._inkEvaluationCoroutine != null && this.inkStory.canContinue && this._inkEvaluationCoroutine.MoveNext())
			{
			}
			if (this._inkEvaluationCoroutine != null)
			{
				base.StopCoroutine(this._inkEvaluationCoroutine);
				this.NullCoroutine();
			}
		}
	}

	// Token: 0x06000459 RID: 1113 RVA: 0x00023F58 File Offset: 0x00022158
	private void ParseInteractablesChoicesAndComplete()
	{
		this._availableInteractableInkItems_Scratch.Clear();
		foreach (Choice choice in this.inkStory.currentChoices)
		{
			string text = null;
			int num = choice.text.IndexOf(" - ");
			string text2;
			if (num != -1)
			{
				text2 = choice.text.Substring(0, num).Trim();
				text = choice.text.Substring(num + " - ".Length).Trim();
			}
			else
			{
				text2 = choice.text.Trim();
			}
			if (text2.StartsWith("EXIT "))
			{
				Debug.Log("RefreshInteractablesChoices found UNSUPPORTED choice trigger zone EXIT choice for interactable(s): " + text2);
			}
			else
			{
				GameChoiceSpecialType gameChoiceSpecialType = GameChoiceSpecialType.None;
				string text3 = null;
				int num2 = text2.IndexOf("/");
				if (num2 != -1)
				{
					string text4 = text2.Substring(num2 + 1).Trim();
					if (text4 == "REST")
					{
						gameChoiceSpecialType = GameChoiceSpecialType.Rest;
					}
					else if (text4.StartsWith("CONFIRM MAP SUCCESS"))
					{
						gameChoiceSpecialType = GameChoiceSpecialType.ConfirmMapSuccess;
					}
					else if (text4.StartsWith("CONFIRM MAP FAIL"))
					{
						gameChoiceSpecialType = GameChoiceSpecialType.ConfirmMapFail;
					}
					else
					{
						Debug.LogError("Ink had choice with unrecognised special type: " + text2);
					}
					if (gameChoiceSpecialType == GameChoiceSpecialType.ConfirmMapSuccess || gameChoiceSpecialType == GameChoiceSpecialType.ConfirmMapFail)
					{
						int num3 = text4.IndexOf("(");
						int num4 = text4.IndexOf(")");
						if (num3 != -1 && num4 != -1)
						{
							text3 = text4.Substring(num3 + 1, num4 - num3 - 1).Trim();
						}
					}
					text2 = text2.Substring(0, num2).Trim();
				}
				GameChoice gameChoice = new GameChoice
				{
					interactableNames = this.InteractableNames(text2),
					text = text,
					inkChoiceIdx = choice.index,
					type = ((text != null) ? GameChoiceType.PropText : GameChoiceType.ZoneLike),
					specialType = gameChoiceSpecialType,
					specificMapInkName = text3
				};
				if (gameChoice.type == GameChoiceType.PropText)
				{
					gameChoice = this.AddIconToChoice(gameChoice);
				}
				else
				{
					gameChoice.icon = ChoiceIcon.None;
				}
				if (!this.ChoiceListContains(this._activeInteractableChoices, gameChoice))
				{
					this._activeInteractableChoices.Add(gameChoice);
					if (gameChoice.type == GameChoiceType.PropText && !gameChoice.isMapChoice)
					{
						foreach (string text5 in gameChoice.interactableNames)
						{
							this._lastInteractableNames.Add(text5);
						}
					}
				}
			}
		}
		this._lastInteractablesRefreshPropList.AddRange(this._nearbyPropsScratch);
		if (Narrative.onDidRefreshInteractables != null)
		{
			Narrative.onDidRefreshInteractables();
		}
	}

	// Token: 0x0600045A RID: 1114 RVA: 0x00024228 File Offset: 0x00022428
	private bool ChoiceListContains(List<GameChoice> choiceList, GameChoice choice)
	{
		foreach (GameChoice gameChoice in choiceList)
		{
			if (gameChoice.IsEquivalentTo(choice))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600045B RID: 1115 RVA: 0x00024280 File Offset: 0x00022480
	private List<string> InteractableNames(string interactableNamesStr)
	{
		List<string> list = interactableNamesStr.Split(',', StringSplitOptions.None).ToList<string>();
		for (int i = 0; i < list.Count; i++)
		{
			list[i] = list[i].Trim();
		}
		return list;
	}

	// Token: 0x0600045C RID: 1116 RVA: 0x000242C4 File Offset: 0x000224C4
	private GameChoice AddIconToChoice(GameChoice choice)
	{
		choice.icon = ChoiceIcon.Hand;
		int num = choice.text.IndexOf(" - ");
		if (num != -1)
		{
			string text = choice.text.Substring(0, num).Trim();
			string text2 = choice.text.Substring(num + " - ".Length).Trim();
			uint num2 = <PrivateImplementationDetails>.ComputeStringHash(text2);
			ChoiceIcon choiceIcon;
			if (num2 <= 1581801795U)
			{
				if (num2 <= 1069749053U)
				{
					if (num2 != 455361754U)
					{
						if (num2 != 496873500U)
						{
							if (num2 == 1069749053U)
							{
								if (text2 == "ICON_FOOTPRINTS")
								{
									choiceIcon = ChoiceIcon.Footprints;
									goto IL_0219;
								}
							}
						}
						else if (text2 == "ICON_TIME")
						{
							choiceIcon = ChoiceIcon.Time;
							goto IL_0219;
						}
					}
					else if (text2 == "ICON_SHELTER")
					{
						choiceIcon = ChoiceIcon.Shelter;
						goto IL_0219;
					}
				}
				else if (num2 != 1208101458U)
				{
					if (num2 != 1217977069U)
					{
						if (num2 == 1581801795U)
						{
							if (text2 == "ICON_BACKPACK")
							{
								choiceIcon = ChoiceIcon.Backpack;
								goto IL_0219;
							}
						}
					}
					else if (text2 == "ICON_BOOT")
					{
						choiceIcon = ChoiceIcon.Boot;
						goto IL_0219;
					}
				}
				else if (text2 == "ICON_EYE")
				{
					choiceIcon = ChoiceIcon.Eye;
					goto IL_0219;
				}
			}
			else if (num2 <= 3115070199U)
			{
				if (num2 != 2751890662U)
				{
					if (num2 != 2914622121U)
					{
						if (num2 == 3115070199U)
						{
							if (text2 == "ICON_BUBBLE")
							{
								choiceIcon = ChoiceIcon.Bubble;
								goto IL_0219;
							}
						}
					}
					else if (text2 == "ICON_MAP")
					{
						choiceIcon = ChoiceIcon.Map;
						goto IL_0219;
					}
				}
				else if (text2 == "ICON_MOON_STARS")
				{
					choiceIcon = ChoiceIcon.MoonStars;
					goto IL_0219;
				}
			}
			else if (num2 != 3127987256U)
			{
				if (num2 != 3868605772U)
				{
					if (num2 == 4249224696U)
					{
						if (text2 == "ICON_WATER")
						{
							choiceIcon = ChoiceIcon.Water;
							goto IL_0219;
						}
					}
				}
				else if (text2 == "ICON_NAME_PEAK")
				{
					choiceIcon = ChoiceIcon.NamePeak;
					goto IL_0219;
				}
			}
			else if (text2 == "ICON_HAND")
			{
				choiceIcon = ChoiceIcon.Hand;
				goto IL_0219;
			}
			choiceIcon = ChoiceIcon.Hand;
			IL_0219:
			choice.icon = choiceIcon;
			choice.text = text;
		}
		else if (choice.specialType == GameChoiceSpecialType.Rest)
		{
			choice.icon = ChoiceIcon.Time;
		}
		else if (choice.specialType == GameChoiceSpecialType.ConfirmMapSuccess || choice.specialType == GameChoiceSpecialType.ConfirmMapFail)
		{
			choice.icon = ChoiceIcon.Map;
		}
		else
		{
			string text3 = choice.text.ToLowerInvariant();
			if (text3.Contains("sleep"))
			{
				choice.icon = ChoiceIcon.MoonStars;
			}
			else if (text3.Contains("look "))
			{
				choice.icon = ChoiceIcon.Eye;
			}
			else if (text3.Contains("wait"))
			{
				choice.icon = ChoiceIcon.MoonStars;
			}
			else if (text3.Contains("nightfall"))
			{
				choice.icon = ChoiceIcon.MoonStars;
			}
			else if (text3.Contains("tell "))
			{
				choice.icon = ChoiceIcon.Bubble;
			}
			else if (text3.Contains("\""))
			{
				choice.icon = ChoiceIcon.Bubble;
			}
			else if (text3.Contains("ask "))
			{
				choice.icon = ChoiceIcon.Bubble;
			}
			else if (text3.Contains("talk"))
			{
				choice.icon = ChoiceIcon.Bubble;
			}
			else if (text3.StartsWith("say"))
			{
				choice.icon = ChoiceIcon.Bubble;
			}
			else if (text3.Contains("kick"))
			{
				choice.icon = ChoiceIcon.Boot;
			}
			else if (text3.Contains("shelter"))
			{
				choice.icon = ChoiceIcon.Shelter;
			}
			else if (text3.Contains("swim"))
			{
				choice.icon = ChoiceIcon.Water;
			}
		}
		return choice;
	}

	// Token: 0x0600045D RID: 1117 RVA: 0x00024674 File Offset: 0x00022874
	public void NotePeak(string inkListNameOfPeak)
	{
		ListValue listValue = this.inkStory.listDefinitions.FindSingleItemListWithName(inkListNameOfPeak);
		if (listValue != null)
		{
			this.inkStory.variablesState["currentPeak"] = listValue.value;
			return;
		}
		this.inkStory.variablesState["currentPeak"] = new InkList();
	}

	// Token: 0x0600045E RID: 1118 RVA: 0x000246D2 File Offset: 0x000228D2
	public void SetNumberOfSpottableMapsOnCurrentPeak(int numSpottableMaps)
	{
		this.inkStory.variablesState["NumberOfSpottableMapsFromThisPeak"] = numSpottableMaps;
	}

	// Token: 0x0600045F RID: 1119 RVA: 0x000246EF File Offset: 0x000228EF
	public bool ReachPeak(string peakListItemName)
	{
		return this.RunKnotWithNameListItemAllowQueue("TRIGGER_reaching_peak", peakListItemName, true, Array.Empty<object>());
	}

	// Token: 0x06000460 RID: 1120 RVA: 0x00024703 File Offset: 0x00022903
	public void LeavePeak(string peakListItemName)
	{
		this.RunKnotWithNameListItemAllowQueue("TRIGGER_leave_peak", peakListItemName, false, Array.Empty<object>());
	}

	// Token: 0x06000461 RID: 1121 RVA: 0x00024718 File Offset: 0x00022918
	public void StartMusicRunning()
	{
		this.RunKnot("TRIGGER_start_music_running", null, false, false, Array.Empty<object>());
	}

	// Token: 0x06000462 RID: 1122 RVA: 0x0002472D File Offset: 0x0002292D
	public void FailMusicRunning()
	{
		this.RunKnot("TRIGGER_fail_music_running", null, false, false, Array.Empty<object>());
	}

	// Token: 0x06000463 RID: 1123 RVA: 0x00024744 File Offset: 0x00022944
	public void CompleteMusicRunning(bool flawless)
	{
		this.RunKnot("TRIGGER_complete_music_running", null, false, false, new object[] { flawless });
	}

	// Token: 0x06000464 RID: 1124 RVA: 0x0002476E File Offset: 0x0002296E
	public void Fall()
	{
		this.RunKnotAllowQueue("TRIGGER_fall", Array.Empty<object>());
	}

	// Token: 0x06000465 RID: 1125 RVA: 0x00024781 File Offset: 0x00022981
	public void FallSoftLanding()
	{
		this.RunKnotAllowQueue("TRIGGER_fallSoftLanding", Array.Empty<object>());
	}

	// Token: 0x06000466 RID: 1126 RVA: 0x00024794 File Offset: 0x00022994
	public void SitDown(string propName)
	{
		this.RunKnot("TRIGGER_sitDown", propName, false, false, Array.Empty<object>());
	}

	// Token: 0x06000467 RID: 1127 RVA: 0x000247AC File Offset: 0x000229AC
	public void StartRest(string propName)
	{
		this.RunKnot("TRIGGER_start_rest", propName, false, false, new object[] { true });
	}

	// Token: 0x06000468 RID: 1128 RVA: 0x000247D6 File Offset: 0x000229D6
	public void MidRest(string propName)
	{
		this.RunKnotWithNameListItemAllowQueue("TRIGGER_mid_rest", propName, false, new object[] { false });
	}

	// Token: 0x06000469 RID: 1129 RVA: 0x000247F5 File Offset: 0x000229F5
	public void NightFallsWhileResting()
	{
		this.RunKnotAllowQueue("TRIGGER_nightFallsWhileResting", Array.Empty<object>());
	}

	// Token: 0x0600046A RID: 1130 RVA: 0x00024808 File Offset: 0x00022A08
	public void Sleep(string propName)
	{
		this.RunKnot("TRIGGER_sleep", propName, false, false, Array.Empty<object>());
	}

	// Token: 0x0600046B RID: 1131 RVA: 0x0002481D File Offset: 0x00022A1D
	public void PlayerAddedMapMarker(string mapPropName)
	{
		this.RunKnotWithNameListItemAllowQueue("TRIGGER_placedMapMarker", mapPropName, false, Array.Empty<object>());
	}

	// Token: 0x0600046C RID: 1132 RVA: 0x00024832 File Offset: 0x00022A32
	public void PlayerMarkedAnotherPeak(string mapPropName, string peakID, bool correctly)
	{
		this.RunKnotWithNameListItemAllowQueue("TRIGGER_placedMapMarkerOnAnotherPeak", mapPropName, false, new object[] { peakID, correctly });
	}

	// Token: 0x0600046D RID: 1133 RVA: 0x00024855 File Offset: 0x00022A55
	public void PlayerMarkedSelf(string mapPropName, bool correctly)
	{
		this.RunKnotWithNameListItemAllowQueue("TRIGGER_placedMapMarkerOnTopOfPlayer", mapPropName, false, new object[] { correctly });
	}

	// Token: 0x0600046E RID: 1134 RVA: 0x00024874 File Offset: 0x00022A74
	public void BellyWriggleDidExit()
	{
		this.RunKnotAllowQueue("TRIGGER_wriggleExit", Array.Empty<object>());
	}

	// Token: 0x0600046F RID: 1135 RVA: 0x00024887 File Offset: 0x00022A87
	public void BellyWriggleTriedMoveButStuck()
	{
		this.RunKnot("TRIGGER_wriggleTriedMoveButStuck", null, false, false, Array.Empty<object>());
	}

	// Token: 0x06000470 RID: 1136 RVA: 0x0002489C File Offset: 0x00022A9C
	public void BellyWriggleMovedOtherWayWhenStuck()
	{
		this.RunKnot("TRIGGER_wriggleMoveOtherWayWhenStuck", null, false, false, Array.Empty<object>());
	}

	// Token: 0x06000471 RID: 1137 RVA: 0x000248B1 File Offset: 0x00022AB1
	public bool CanBellyWriggle(bool forwards)
	{
		return (bool)this.inkStory.variablesState[forwards ? "canWriggleForwards" : "canWriggleBackwards"];
	}

	// Token: 0x06000472 RID: 1138 RVA: 0x000248D7 File Offset: 0x00022AD7
	public void PlayerAlarmedBird(string propName)
	{
		this.RunKnotWithNameListItemAllowQueue("TRIGGER_alarmBird", propName, false, Array.Empty<object>());
	}

	// Token: 0x06000473 RID: 1139 RVA: 0x000248EC File Offset: 0x00022AEC
	public void FinalJumpStarted()
	{
		this.RunKnot("TRIGGER_final_jump", null, false, true, Array.Empty<object>());
	}

	// Token: 0x06000474 RID: 1140 RVA: 0x00024904 File Offset: 0x00022B04
	public void RunCreatureKnot(string creatureName, string stateName)
	{
		InkList inkList = this.InkListWithItemNamed(stateName);
		this.RunKnotWithNameListItemAllowQueue("TRIGGER_creatureInteraction", creatureName, false, new object[] { inkList });
	}

	// Token: 0x06000475 RID: 1141 RVA: 0x00024934 File Offset: 0x00022B34
	public void LeftAutoCutZone()
	{
		this.activeAutoCutZoneName = null;
		object obj = this.inkStory.variablesState["autoCutTarget"];
		string text = null;
		if (obj is string)
		{
			text = (string)obj;
		}
		else if (obj is Path)
		{
			text = obj.ToString();
		}
		if (text != null && text.Length > 0)
		{
			this.inkStory.variablesState["autoCutTarget"] = 0;
			this.RunKnotInterject(text);
		}
	}

	// Token: 0x06000476 RID: 1142 RVA: 0x000249AE File Offset: 0x00022BAE
	public void EndEvent(string eventName)
	{
		this._eventsEnded.Add(eventName);
	}

	// Token: 0x06000477 RID: 1143 RVA: 0x000249BC File Offset: 0x00022BBC
	private InkList InkListWithItemNamed(string singleItemName)
	{
		InkList inkList = new InkList();
		ListValue listValue = this.inkStory.listDefinitions.FindSingleItemListWithName(singleItemName);
		if (listValue == null)
		{
			return inkList;
		}
		KeyValuePair<InkListItem, int> keyValuePair = listValue.value.First<KeyValuePair<InkListItem, int>>();
		inkList.Add(keyValuePair.Key, keyValuePair.Value);
		return inkList;
	}

	// Token: 0x06000478 RID: 1144 RVA: 0x00024A10 File Offset: 0x00022C10
	private InkList InkListWithItemsNamed(List<string> names)
	{
		InkList inkList = new InkList();
		foreach (string text in names)
		{
			ListValue listValue = this.inkStory.listDefinitions.FindSingleItemListWithName(text);
			if (!(listValue == null))
			{
				KeyValuePair<InkListItem, int> keyValuePair = listValue.value.First<KeyValuePair<InkListItem, int>>();
				inkList.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}
		return inkList;
	}

	// Token: 0x06000479 RID: 1145 RVA: 0x00024A9C File Offset: 0x00022C9C
	public void SetCorrectlyPlacedMapMarkerNames(List<string> mapMarkerNames)
	{
		this.inkStory.variablesState["mapsCorrectlyMarked"] = this.InkListWithItemsNamed(mapMarkerNames);
	}

	// Token: 0x0600047A RID: 1146 RVA: 0x00024ABA File Offset: 0x00022CBA
	public void SetPlacedMapMarkerNames(List<string> mapMarkerNames)
	{
		this.inkStory.variablesState["mapsMarked"] = this.InkListWithItemsNamed(mapMarkerNames);
	}

	// Token: 0x0600047B RID: 1147 RVA: 0x00024AD8 File Offset: 0x00022CD8
	public void SetConfirmedMap(string mapName)
	{
		this.inkStory.variablesState["mapConfirmTarget"] = this.InkListWithItemsNamed(new List<string> { mapName });
	}

	// Token: 0x0600047C RID: 1148 RVA: 0x00024B01 File Offset: 0x00022D01
	public bool PlayerHasCompletedMapWithPropName(string propName)
	{
		return ((InkList)this._inkStory.variablesState["mapsConfirmed"]).ContainsItemNamed(propName);
	}

	// Token: 0x0600047D RID: 1149 RVA: 0x00024B24 File Offset: 0x00022D24
	public bool MapIsEasterEgg(string propName)
	{
		foreach (Map map in MonoSingleton<Inventory>.instance.maps)
		{
			if (map.targetInkPropName == propName)
			{
				return map.neverPromptDirectly;
			}
		}
		return false;
	}

	// Token: 0x0600047E RID: 1150 RVA: 0x00024B90 File Offset: 0x00022D90
	public void SyncMapsInInventory()
	{
		InkList inkList = (InkList)this.inkStory.variablesState["MapsYouveGot"];
		InkList inkList2 = null;
		foreach (Map map in MonoSingleton<Inventory>.instance.maps)
		{
			if (!string.IsNullOrWhiteSpace(map.targetInkPropName) && !inkList.ContainsItemNamed(map.targetInkPropName))
			{
				if (inkList2 == null)
				{
					inkList2 = new InkList(inkList);
				}
				try
				{
					inkList2.AddItem(map.targetInkPropName, this.inkStory);
				}
				catch (Exception ex)
				{
					Debug.LogError(ex.Message);
				}
			}
		}
		if (inkList2 != null)
		{
			this.inkStory.variablesState["MapsYouveGot"] = inkList2;
		}
		foreach (InkListItem inkListItem in inkList.Keys)
		{
			string mapName = inkListItem.itemName;
			Map map2;
			if (!MonoSingleton<Inventory>.instance.maps.Exists((Map m) => m.targetInkPropName == mapName) && Map.allByPropName.TryGetValue(mapName, out map2))
			{
				MonoSingleton<Inventory>.instance.maps.Insert(0, map2);
			}
		}
	}

	// Token: 0x0600047F RID: 1151 RVA: 0x00024D00 File Offset: 0x00022F00
	public void Death(bool fast)
	{
		this.RunKnotAllowQueue("TRIGGER_onDeath", new object[] { fast });
	}

	// Token: 0x06000480 RID: 1152 RVA: 0x00024D20 File Offset: 0x00022F20
	public bool TryChooseZoneLikeChoice(string interactableName)
	{
		GameChoice gameChoice;
		if (!this.TryGetZoneLikeChoice(interactableName, out gameChoice))
		{
			return false;
		}
		this.RunInkWithChoiceIndex(gameChoice.inkChoiceIdx, interactableName);
		return true;
	}

	// Token: 0x06000481 RID: 1153 RVA: 0x00024D48 File Offset: 0x00022F48
	public bool HasInteractableChoice(string interactableName, MarkerState markerState)
	{
		return this.GetPropTextChoices(interactableName, markerState, false, true).Count > 0;
	}

	// Token: 0x06000482 RID: 1154 RVA: 0x00024D5C File Offset: 0x00022F5C
	public bool HasZoneLikeChoice(string interactableName)
	{
		return this._activeInteractableChoices.Exists((GameChoice choice) => choice.interactableNames.Contains(interactableName) && choice.type == GameChoiceType.ZoneLike);
	}

	// Token: 0x06000483 RID: 1155 RVA: 0x00024D90 File Offset: 0x00022F90
	public bool HasRestChoice(string interactableName)
	{
		return this._activeInteractableChoices.Exists((GameChoice choice) => choice.specialType == GameChoiceSpecialType.Rest && choice.interactableNames.Contains(interactableName));
	}

	// Token: 0x06000484 RID: 1156 RVA: 0x00024DC4 File Offset: 0x00022FC4
	public bool HasTakePathChoice(string interactableName)
	{
		return this._activeInteractableChoices.Exists((GameChoice choice) => choice.icon == ChoiceIcon.Footprints && choice.interactableNames.Contains(interactableName));
	}

	// Token: 0x06000485 RID: 1157 RVA: 0x00024DF5 File Offset: 0x00022FF5
	public bool DidHaveInteractableChoice(string interactableName)
	{
		return this._lastInteractableNames.Contains(interactableName);
	}

	// Token: 0x06000486 RID: 1158 RVA: 0x00024E04 File Offset: 0x00023004
	public List<GameChoice> GetPropTextChoices(string interactableName, MarkerState markerState, bool textChoiceOnly, bool justOneIsFine = false)
	{
		Narrative._choicesScratch.Clear();
		bool flag = false;
		foreach (GameChoice gameChoice in this._activeInteractableChoices)
		{
			if ((!textChoiceOnly || gameChoice.type == GameChoiceType.PropText || gameChoice.isMapChoice) && (!gameChoice.isMapChoice || (!flag && markerState.isValidPlacedMarker && (gameChoice.specialType != GameChoiceSpecialType.ConfirmMapSuccess || markerState.correct) && (gameChoice.specialType != GameChoiceSpecialType.ConfirmMapFail || !markerState.correct) && (gameChoice.specificMapInkName == null || !(gameChoice.specificMapInkName != markerState.inkName)))) && gameChoice.interactableNames.Contains(interactableName))
			{
				Narrative._choicesScratch.Add(gameChoice);
				if (gameChoice.isMapChoice)
				{
					flag = true;
				}
				if (justOneIsFine)
				{
					return Narrative._choicesScratch;
				}
			}
		}
		return Narrative._choicesScratch;
	}

	// Token: 0x06000487 RID: 1159 RVA: 0x00024F04 File Offset: 0x00023104
	public bool TryGetMapConfirmChoice(string interactableName, bool isSuccess, out GameChoice mapChoice)
	{
		mapChoice = default(GameChoice);
		List<GameChoice> propTextChoices = this.GetPropTextChoices("MAP_MARKER", new MarkerState
		{
			inkName = interactableName,
			correct = isSuccess
		}, true, true);
		if (propTextChoices.Count > 0)
		{
			mapChoice = propTextChoices[0];
			return true;
		}
		return false;
	}

	// Token: 0x06000488 RID: 1160 RVA: 0x00024F58 File Offset: 0x00023158
	public void ChooseMapConfirmChoice(GameChoice choice)
	{
		this.RunInkWithChoiceIndex(choice.inkChoiceIdx, choice.interactableNames[0]);
	}

	// Token: 0x06000489 RID: 1161 RVA: 0x00024F74 File Offset: 0x00023174
	public List<GameChoice> GetRestChoices()
	{
		Narrative._choicesScratch.Clear();
		foreach (GameChoice gameChoice in this._activeInteractableChoices)
		{
			if (gameChoice.specialType == GameChoiceSpecialType.Rest)
			{
				Narrative._choicesScratch.Add(gameChoice);
			}
		}
		return Narrative._choicesScratch;
	}

	// Token: 0x0600048A RID: 1162 RVA: 0x00024FE4 File Offset: 0x000231E4
	public bool TryGetZoneLikeChoice(string interactableName, out GameChoice foundChoice)
	{
		foundChoice = default(GameChoice);
		foreach (GameChoice gameChoice in this._activeInteractableChoices)
		{
			if (gameChoice.type == GameChoiceType.ZoneLike && gameChoice.interactableNames.Contains(interactableName))
			{
				foundChoice = gameChoice;
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600048B RID: 1163 RVA: 0x0002505C File Offset: 0x0002325C
	public List<GameChoice> GetZoneLikeChoices()
	{
		Narrative._choicesScratch.Clear();
		foreach (GameChoice gameChoice in this._activeInteractableChoices)
		{
			if (gameChoice.type == GameChoiceType.ZoneLike)
			{
				Narrative._choicesScratch.Add(gameChoice);
			}
		}
		return Narrative._choicesScratch;
	}

	// Token: 0x0600048C RID: 1164 RVA: 0x000250CC File Offset: 0x000232CC
	private bool InkListsContain(string[] inkListVariableNames, InkList stateList, bool requireFullContainment)
	{
		if (stateList.IsEmpty<KeyValuePair<InkListItem, int>>())
		{
			return false;
		}
		requireFullContainment |= stateList.Count<KeyValuePair<InkListItem, int>>() == 1;
		this._keysToFind.Clear();
		if (requireFullContainment)
		{
			this._keysToFind = stateList.Select((KeyValuePair<InkListItem, int> t) => t.Key.itemName).ToList<string>();
		}
		foreach (string text in inkListVariableNames)
		{
			InkList inkList = (InkList)this.inkStory.variablesState[text];
			if (!requireFullContainment)
			{
				if (stateList.HasIntersection(inkList))
				{
					return true;
				}
			}
			else
			{
				foreach (KeyValuePair<InkListItem, int> keyValuePair in inkList)
				{
					if (this._keysToFind.Contains(keyValuePair.Key.itemName))
					{
						this._keysToFind.Remove(keyValuePair.Key.itemName);
					}
				}
				if (this._keysToFind.IsEmpty<string>())
				{
					break;
				}
			}
		}
		return requireFullContainment && this._keysToFind.IsEmpty<string>();
	}

	// Token: 0x0600048D RID: 1165 RVA: 0x000251F8 File Offset: 0x000233F8
	public void Clear(Narrative.ClearType clearType)
	{
		this._clearing = true;
		this.CancelCoroutine();
		if (!this.inkStory.asyncContinueComplete)
		{
			this.inkStory.Continue();
		}
		this.inkStory.ResetCallstack();
		this._activeInteractableChoices.Clear();
		this._lastInteractableNames.Clear();
		this._nearbyPropsScratch.Clear();
		this._lastInteractablesRefreshPropList.Clear();
		this._pauseReason = Narrative.PauseReason.None;
		this.DisableAllInkCameras(true);
		GameCamera.instance.inkModifierState.Reset();
		if (clearType == Narrative.ClearType.Visual)
		{
			this._clearing = false;
			return;
		}
		if (clearType == Narrative.ClearType.FullState || clearType == Narrative.ClearType.FullStatePreservingLoopVariables)
		{
			if (clearType == Narrative.ClearType.FullState)
			{
				this.orderOfAquisitionIdx.Clear();
				this.nextAquisitionIdx = 0;
			}
			Dictionary<string, object> dictionary = null;
			if (clearType == Narrative.ClearType.FullStatePreservingLoopVariables)
			{
				dictionary = new Dictionary<string, object>();
				foreach (string text in Narrative.loopableVariables)
				{
					dictionary[text] = this.inkStory.variablesState[text];
				}
			}
			this.inkStory.ResetState();
			if (clearType == Narrative.ClearType.FullStatePreservingLoopVariables)
			{
				foreach (KeyValuePair<string, object> keyValuePair in dictionary)
				{
					string key = keyValuePair.Key;
					if (this.inkStory.variablesState.GlobalVariableExistsWithName(key))
					{
						this.inkStory.variablesState[key] = keyValuePair.Value;
					}
				}
			}
			this.RunFunctionAndParseOutput("setUpInitialData", Array.Empty<object>());
			this.RebuildCachedPairStore(true);
		}
		this.lastNightApproachRemarkDayIdx = -1;
		this._lullRemarkStartWaitingFromTime = 0f;
		this._lastRecoveryBarkTime = 0f;
		this._lastRunToProp = null;
		this._lastInkKnownWeatherType = WeatherType.Clear;
		this._lastInkKnownRawWeatherEffect = WeatherHealthEffect.None;
		this._lastInkKnownProtectedWeatherEffect = WeatherHealthEffect.None;
		this._recentlyRemarkedNearbyMap = null;
		this._recentlyRemarkedNearbyMapTime = 0f;
		this._lastLookFurtherRemarkTime = float.MinValue;
		this._hasRemarkedDuringCurrentLookFurther = false;
		this._enabledCameraVolumes.Clear();
		this.preventBackgroundRemarks = Narrative.PreventBackgroundRemarksReason.None;
		NarrativePresenter.instance.Clear(clearType == Narrative.ClearType.Death, false);
		this._debugLog.Add("--- NARRATIVE STATE CLEARED, THOUGH PRESERVING LOG ---");
		this._clearing = false;
	}

	// Token: 0x0600048E RID: 1166 RVA: 0x00025414 File Offset: 0x00023614
	private void Update()
	{
		if (this.inkStory == null)
		{
			return;
		}
		if (!Game.loaded || Game.gameplayPaused)
		{
			return;
		}
		this.UpdateWeather(false);
		this.UpdateRemarks();
		if (!this.isInkBusy && !Runner.instance.isMusicRunning && !GameClock.instance.isWaitingForTimeToPass)
		{
			this.RefreshNearbyInteractablePropList();
			foreach (Prop prop in this._nearbyPropsScratch)
			{
				if (!this._lastInteractablesRefreshPropList.Contains(prop))
				{
					this.RefreshInteractablesChoices(false, true);
					break;
				}
			}
		}
	}

	// Token: 0x0600048F RID: 1167 RVA: 0x000254CC File Offset: 0x000236CC
	private void RefreshNearbyInteractablePropList()
	{
		Level.current.props.Nearby(Runner.instance.position, Range.infinity, 0f, this._nearbyPropsScratch);
		this._nearbyPropsScratch.RemoveAllAnd((Prop prop) => !prop.IsCurrentlyAvailableAsNarrativeChoice() || !prop.triggerZone.InsideAttractDist(Runner.instance.position, (float)Runner.instance.physicalDepthLayerIdx), null);
	}

	// Token: 0x06000490 RID: 1168 RVA: 0x00025530 File Offset: 0x00023730
	private void UpdateRemarks()
	{
		Runner instance = Runner.instance;
		bool flag = instance.balancing || instance.climbing || instance.falling || instance.hardLanding || instance.climbSlipping || instance.inFinalJump || instance.dead || instance.bellyWriggling;
		if (!Game.instance.lookingFurther)
		{
			this._hasRemarkedDuringCurrentLookFurther = false;
		}
		this.PreventBackgroundRemarks(Runner.instance.staminaIsLow, Narrative.PreventBackgroundRemarksReason.FeelingExhausted);
		this.PreventBackgroundRemarks(flag, Narrative.PreventBackgroundRemarksReason.PoorRunnerState);
		this.PreventBackgroundRemarks(GameClock.instance.isLate, Narrative.PreventBackgroundRemarksReason.LateNight);
		if (GameClock.instance.isNight && Game.instance.inActiveGameplay && Level.currentIndex + 1 < 9 && !Runner.instance.isMusicRunning && Runner.instance.running && !PropsController.instance.isInAttractZone && !PropsController.instance.isInTriggerZone && !this.isBusy && !GameCamera.instance.introCameraState.active)
		{
			this.RunKnot("TRIGGER_nightfall", null, false, false, Array.Empty<object>());
			return;
		}
		int num = ((GameClock.instance.hourOfDay >= 19f) ? GameClock.instance.dayIdx : (GameClock.instance.dayIdx - 1));
		if (GameClock.instance.isLate && num > this.lastNightApproachRemarkDayIdx)
		{
			bool flag2 = Game.instance.inActiveGameplay && !Runner.instance.isMusicRunning && Runner.instance.running && !Runner.instance.climbing && !PropsController.instance.isInAttractZone && !PropsController.instance.isInTriggerZone && !this.isBusy && !GameCamera.instance.introCameraState.active;
			if (MonoSingleton<RestStateController>.instance.active || GameClock.instance.isNight)
			{
				this.lastNightApproachRemarkDayIdx = num;
			}
			else if (flag2)
			{
				this.lastNightApproachRemarkDayIdx = GameClock.instance.dayIdx;
				this.RunKnot("TRIGGER_nightApproaching", null, false, false, Array.Empty<object>());
				return;
			}
		}
		bool flag3 = !DebugOptions.opts.dontLullRemark && this.allowLullRemarks && !this.isBusy;
		bool flag4 = this.allowRecoveryRemark && !this.isBusy;
		if (!flag3 && this.resetLullRemarkTime)
		{
			this.ResetLullRemarkTime();
		}
		if (flag3)
		{
			Map inVicinityPromptableMap = MonoSingleton<MapsViewController>.instance.inVicinityPromptableMap;
			if (inVicinityPromptableMap != null && !inVicinityPromptableMap.isFirstTutorialMap && (this._recentlyRemarkedNearbyMap != inVicinityPromptableMap || Time.time > this._recentlyRemarkedNearbyMapTime + 40f))
			{
				this._recentlyRemarkedNearbyMap = inVicinityPromptableMap;
				this._recentlyRemarkedNearbyMapTime = Time.time;
				this.RunKnotWithNameListItemAllowQueue("TRIGGER_mapNearbyUnmarkerOrIncorrect", inVicinityPromptableMap.targetInkPropName, false, Array.Empty<object>());
				return;
			}
		}
		if (flag3 && Game.instance.lookingFurther && !this._hasRemarkedDuringCurrentLookFurther && Time.time > this._lastLookFurtherRemarkTime + 30f)
		{
			this._hasRemarkedDuringCurrentLookFurther = true;
			this._lastLookFurtherRemarkTime = Time.time;
			this.RunKnot("TRIGGER_lookingFurther", null, false, false, Array.Empty<object>());
		}
		if (flag4 && instance.stamina < 0.3f && instance.stoppedAndPaused && Time.time > this._lastRecoveryBarkTime + 40f)
		{
			this._lullRemarkStartWaitingFromTime = Time.time;
			this._lastRecoveryBarkTime = Time.time;
			this.RunKnot("recovery_bark", null, false, false, Array.Empty<object>());
			return;
		}
		if (flag3 && Time.time > this._lullRemarkStartWaitingFromTime + 20f)
		{
			this._lullRemarkStartWaitingFromTime = Time.time + 40f;
			this.RunKnot("lull_remarks", null, false, false, Array.Empty<object>());
			return;
		}
		bool inCave = CaveRegion.inCave;
		if (inCave != this._inCave && !this.isBusy && !flag)
		{
			this._inCave = inCave;
			if (!this.isBusy && !GameClock.instance.isLate)
			{
				this.RunKnot("describeCaveDarknessChange", null, false, false, new object[] { this._inCave });
				return;
			}
		}
	}

	// Token: 0x06000491 RID: 1169 RVA: 0x00025940 File Offset: 0x00023B40
	public void UpdateWeather(bool withoutComment = false)
	{
		WeatherType currentWeather = WeatherSystem.instance.currentWeather;
		WeatherHealthEffect weatherHealthEffects = Runner.instance.health.GetWeatherHealthEffects(false);
		WeatherHealthEffect weatherHealthEffects2 = Runner.instance.health.GetWeatherHealthEffects(true);
		if (this._lastInkKnownWeatherType != currentWeather || this._lastInkKnownRawWeatherEffect != weatherHealthEffects || this._lastInkKnownProtectedWeatherEffect != weatherHealthEffects2)
		{
			InkList inkList = Narrative.<UpdateWeather>g__WeatherTypeToInkList|195_0(currentWeather, weatherHealthEffects);
			InkList inkList2 = Narrative.<UpdateWeather>g__WeatherTypeToInkList|195_0(currentWeather, weatherHealthEffects2);
			Narrative.instance.inkStory.variablesState["CurrentWeather"] = inkList;
			Narrative.instance.inkStory.variablesState["CurrentWeatherWithProtection"] = inkList2;
			if (!Narrative.instance.isBusy && Narrative.instance.allowLullRemarks && !withoutComment)
			{
				InkList inkList3 = Narrative.<UpdateWeather>g__WeatherTypeToInkList|195_0(this._lastInkKnownWeatherType, this._lastInkKnownRawWeatherEffect);
				InkList inkList4 = Narrative.<UpdateWeather>g__WeatherTypeToInkList|195_0(currentWeather, weatherHealthEffects2);
				WeatherModifierZone currentWeatherModifierZone = Runner.instance.health.currentWeatherModifierZone;
				InkList inkList5 = null;
				if (currentWeatherModifierZone != null && currentWeatherModifierZone.optionalInkName != null)
				{
					string text = currentWeatherModifierZone.optionalInkName.Trim();
					if (text.Length > 0)
					{
						ListValue listValue = this.inkStory.listDefinitions.FindSingleItemListWithName(text);
						if (listValue != null)
						{
							inkList5 = listValue.value;
						}
						else
						{
							Debug.LogError("WeatherModifierZone: Ink list item name '" + text + "' couldn't be found to pass to describeWeatherChange in zone " + currentWeatherModifierZone.name);
						}
					}
				}
				if (inkList5 == null)
				{
					inkList5 = new InkList();
				}
				Narrative.instance.RunKnot("describeWeatherChange", null, false, false, new object[] { inkList3, inkList, inkList4, inkList5 });
			}
			this._lastInkKnownWeatherType = currentWeather;
			this._lastInkKnownRawWeatherEffect = weatherHealthEffects;
			this._lastInkKnownProtectedWeatherEffect = weatherHealthEffects2;
			if (Narrative.onRefreshedWeather != null)
			{
				Narrative.onRefreshedWeather();
			}
		}
	}

	// Token: 0x06000492 RID: 1170 RVA: 0x00025AFF File Offset: 0x00023CFF
	private IEnumerator ProcessInkLines(string[] lines)
	{
		string[] array = lines;
		for (int i = 0; i < array.Length; i++)
		{
			string text = array[i].Trim();
			if (text.Length > 0)
			{
				yield return this.ProcessInkLine(text);
			}
		}
		array = null;
		yield break;
	}

	// Token: 0x06000493 RID: 1171 RVA: 0x00025B15 File Offset: 0x00023D15
	private void CancelCoroutine()
	{
		if (this._inkEvaluationCoroutine != null)
		{
			base.StopCoroutine(this._inkEvaluationCoroutine);
			this.NullCoroutine();
		}
	}

	// Token: 0x06000494 RID: 1172 RVA: 0x00025B31 File Offset: 0x00023D31
	private void NullCoroutine()
	{
		this._inkEvaluationCoroutine = null;
		this._inkEvaluationCoroutineContext = null;
		this._inkEvaluationCoroutineLine = null;
		this.debugInkEvaluationStartPoint = null;
	}

	// Token: 0x06000495 RID: 1173 RVA: 0x00025B4F File Offset: 0x00023D4F
	private IEnumerator ProcessInkLine(string line)
	{
		if (line.Length == 0)
		{
			yield break;
		}
		this._inkEvaluationCoroutineLine = line;
		BaseContentInstruction baseContentInstruction;
		if (BaseContentInstruction.TryParse(line, this.inkStory.currentTags, out baseContentInstruction))
		{
			yield return this.RunStoryContentCoroutine(baseContentInstruction);
		}
		else
		{
			Debug.LogError("Could not parse story content line: " + line);
		}
		yield break;
	}

	// Token: 0x06000496 RID: 1174 RVA: 0x00025B68 File Offset: 0x00023D68
	private List<GameChoice> ParseChoices()
	{
		List<GameChoice> list = new List<GameChoice>();
		List<Choice> currentChoices = this.inkStory.currentChoices;
		for (int i = 0; i < currentChoices.Count; i++)
		{
			Choice choice = currentChoices[i];
			GameChoice gameChoice = new GameChoice
			{
				inkChoiceIdx = i
			};
			int num = choice.text.IndexOf(':');
			if (num != -1)
			{
				string text = choice.text.Substring(num + 1).Trim();
				gameChoice.text = "\"" + text + "\"";
				gameChoice.type = GameChoiceType.Dialogue;
				gameChoice.icon = ChoiceIcon.Bubble;
			}
			else if (choice.text.StartsWith("EXIT "))
			{
				gameChoice.type = GameChoiceType.ExitZone;
				gameChoice.interactableNames = this.InteractableNames(choice.text.Substring("EXIT ".Length));
			}
			else
			{
				gameChoice.text = choice.text;
				gameChoice.type = GameChoiceType.Normal;
				gameChoice = this.AddIconToChoice(gameChoice);
			}
			gameChoice.text = InkStylingUtility.ProcessText(gameChoice.text, false, true);
			if (!this.ChoiceListContains(list, gameChoice))
			{
				list.Add(gameChoice);
			}
		}
		return list;
	}

	// Token: 0x06000497 RID: 1175 RVA: 0x00025C94 File Offset: 0x00023E94
	private IEnumerator InkEvaluationCoroutine(bool refreshingInteractables)
	{
		while (this.inkStory.canContinue)
		{
			while (this.paused && !refreshingInteractables && !this._forcingCoroutineSyncComplete)
			{
				yield return null;
			}
			int inkEvalFrames = 1;
			this._stopwatch.Reset();
			this._stopwatch.Start();
			for (;;)
			{
				this.inkStory.ContinueAsync(this.asyncMillisecsBudget);
				if (this.inkStory.asyncContinueComplete)
				{
					break;
				}
				yield return null;
				if (this.inkStory.asyncContinueComplete)
				{
					break;
				}
				int num = inkEvalFrames;
				inkEvalFrames = num + 1;
			}
			this._stopwatch.Stop();
			string line = this.inkStory.currentText.Trim();
			if (line.Length > 0)
			{
				if (refreshingInteractables && !line.StartsWith("["))
				{
				}
				while (this.paused && !refreshingInteractables && !this._forcingCoroutineSyncComplete)
				{
					yield return null;
				}
				yield return this.ProcessInkLine(line);
			}
			while (this.paused && !refreshingInteractables && !this._forcingCoroutineSyncComplete)
			{
				yield return null;
			}
			if (this.inkStory.currentChoices.Count != 0)
			{
				if (refreshingInteractables)
				{
					break;
				}
				List<GameChoice> list = this.ParseChoices();
				yield return NarrativePresenter.Choices(list);
			}
			while (this.paused && !refreshingInteractables && !this._forcingCoroutineSyncComplete)
			{
				yield return null;
			}
			line = null;
		}
		this.NullCoroutine();
		this.inkStory.variablesState["interruptAllowed"] = false;
		this.SyncMapsInInventory();
		Runner.instance.playerControlDisabled &= ~PlayerControlDisableReason.Ink;
		Runner.instance.playerControlDisabled &= ~PlayerControlDisableReason.InkPose;
		Runner.instance.playerControlDisabled &= ~PlayerControlDisableReason.AutoRunToProp;
		Runner.instance.playerControlDisabled &= ~PlayerControlDisableReason.NarrativeChoicesWithNoExit;
		Runner.instance.health.SetInvincible(false, Health.InvincibleReason.InkScene);
		Runner.instance.RemoveStrongPoses();
		this.DisableAllInkCameras(false);
		GameCamera.InkModifierState inkModifierState = GameCamera.instance.inkModifierState;
		inkModifierState.inkBlendTime = 1f;
		inkModifierState.inkEnabled = false;
		AudioController.instance.SetAmbientMusicAllowedByInk(true);
		this._activeQueueableKnot = default(Narrative.QueuedKnot);
		if (this._hadError)
		{
			this._hadError = false;
			if (this._ranOutOfContent)
			{
				this._ranOutOfContent = false;
				this.RunKnot("recover_after_unexpectedly_running_out_of_content", null, false, false, Array.Empty<object>());
				yield break;
			}
		}
		if (refreshingInteractables)
		{
			this.ParseInteractablesChoicesAndComplete();
		}
		if (this._queuedKnots.Count > 0)
		{
			Narrative.QueuedKnot queuedKnot = this._queuedKnots[0];
			this._queuedKnots.RemoveAt(0);
			this.RunKnot(queuedKnot.knotName, null, false, false, queuedKnot.arguments);
		}
		else if (!refreshingInteractables)
		{
			this.RefreshInteractablesChoices(false, false);
		}
		if (!this.isInkBusy)
		{
			PropsController.instance.SetPropDotsAllowedByInk(true);
		}
		yield break;
	}

	// Token: 0x06000498 RID: 1176 RVA: 0x00025CAC File Offset: 0x00023EAC
	private string[] SplitContentByComma(string content)
	{
		string[] array = content.Split(",", StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = array[i].Trim();
		}
		return array;
	}

	// Token: 0x06000499 RID: 1177 RVA: 0x00025CE0 File Offset: 0x00023EE0
	private IEnumerator RunStoryContentCoroutine(BaseContentInstruction baseContentInstruction)
	{
		Runner runner = Runner.instance;
		if (baseContentInstruction is DebugLogInstruction)
		{
			Debug.Log(((DebugLogInstruction)baseContentInstruction).log);
		}
		else if (baseContentInstruction is PauseInstruction)
		{
			yield return new WaitForSeconds(((PauseInstruction)baseContentInstruction).pauseLength);
		}
		else if (baseContentInstruction is HealthInstruction)
		{
			HealthInstruction healthInstruction = (HealthInstruction)baseContentInstruction;
			if (healthInstruction.healing > 0)
			{
				runner.health.ApplyHealing((float)healthInstruction.healing);
			}
			else
			{
				runner.health.ApplyDamage(healthInstruction.damageType, healthInstruction.damage);
			}
		}
		else if (baseContentInstruction is AudioInstruction)
		{
			AudioInstruction audioInstruction = (AudioInstruction)baseContentInstruction;
			yield return AudioController.instance.PlayNarrativeAudioWithName(audioInstruction.audioPath);
		}
		else if (baseContentInstruction is LegacyInstruction)
		{
			LegacyInstruction cont = (LegacyInstruction)baseContentInstruction;
			switch (cont.type)
			{
			case LegacyInstruction.Type.NarratorDialogue:
				yield return NarrativePresenter.Narrate(cont.content, cont.audioFilename);
				break;
			case LegacyInstruction.Type.CharacterDialogue:
				yield return NarrativePresenter.DialogueBubble(cont.characterName, cont.content, cont.audioFilename);
				break;
			case LegacyInstruction.Type.Run:
				if (!cont.end)
				{
					string[] array = this.SplitContentByComma(cont.content);
					string text = array[0];
					bool flag = false;
					if (array.Length > 1)
					{
						flag = array[1] == "PRECISELY";
					}
					List<Prop> loadedPropsByInkName = Prop.GetLoadedPropsByInkName(text);
					int num = loadedPropsByInkName.Count<Prop>();
					if (num > 0)
					{
						if (num > 1)
						{
							Debug.LogWarning("Several props with ink name " + cont.content + ". Returning IS_MINOR_PEAK at the first one found.");
						}
						float num2;
						if (PropsController.CanAutoRunToProp(loadedPropsByInkName.First<Prop>(), out num2))
						{
							runner.SetAutoRunTarget(loadedPropsByInkName.First<Prop>().transform.position, num2 * 2f + 0.5f, flag);
						}
						else
						{
							Debug.LogWarning("Could not reach prop to run to " + loadedPropsByInkName.First<Prop>().gameObject.name, loadedPropsByInkName.First<Prop>());
						}
					}
					else
					{
						Debug.LogError("Prop not found to >>> RUN to: '" + cont.content);
					}
				}
				if (!cont.start)
				{
					while (runner.hasAutoRunTarget && !runner.isAtAutoRunTargetAndStopped)
					{
						yield return null;
					}
					if (this._lastRunToProp != null && this._lastRunToProp.preferredLookDirection != 0)
					{
						runner.direction = (float)this._lastRunToProp.preferredLookDirection;
					}
				}
				break;
			case LegacyInstruction.Type.Camera:
			{
				CameraVolume cameraVolume = null;
				if (cont.content != null && cont.content.Length > 0)
				{
					cameraVolume = CameraVolume.WithName(cont.content);
				}
				if (!cont.end && cameraVolume)
				{
					cameraVolume.inkHasEnabled = true;
					if (!this._enabledCameraVolumes.Contains(cameraVolume))
					{
						this._enabledCameraVolumes.Add(cameraVolume);
					}
				}
				if (cont.end)
				{
					if (cameraVolume)
					{
						cameraVolume.inkHasEnabled = false;
					}
					else
					{
						this.DisableAllInkCameras(true);
					}
				}
				break;
			}
			case LegacyInstruction.Type.CameraShake:
			{
				CameraShakeName cameraShakeName = CameraShakeName.Major;
				if (!string.IsNullOrWhiteSpace(cont.content) && !Enum.TryParse<CameraShakeName>(cont.content, out cameraShakeName))
				{
					Debug.LogError(">>> CAMERA SHAKE: Unrecognised camera shake name: " + cont.content);
				}
				GameCamera.instance.cameraShakeState.StartShake(cameraShakeName);
				break;
			}
			case LegacyInstruction.Type.Zoom:
			{
				float num3 = 30f;
				float num4 = 1f;
				string[] array2 = this.SplitContentByComma(cont.content);
				for (int i = 0; i < array2.Length; i++)
				{
					string text2 = array2[i];
					bool flag2 = false;
					if (text2.EndsWith("s"))
					{
						text2 = text2.Substring(0, text2.Length - 1);
						flag2 = true;
					}
					else if (i > 0)
					{
						flag2 = true;
					}
					float num5;
					if (float.TryParse(text2, out num5))
					{
						if (flag2)
						{
							num4 = num5;
						}
						else
						{
							num3 = num5;
						}
					}
				}
				GameCamera.InkModifierState inkModifierState = GameCamera.instance.inkModifierState;
				if (!cont.end)
				{
					inkModifierState.inkEnabled = true;
					inkModifierState.inkTargetDistance = num3;
					inkModifierState.inkBlendTime = num4;
				}
				else
				{
					inkModifierState.inkEnabled = false;
					inkModifierState.inkBlendTime = num4;
				}
				break;
			}
			case LegacyInstruction.Type.Audio:
				yield return AudioController.instance.PlayNarrativeAudioWithName(cont.content);
				break;
			case LegacyInstruction.Type.Sting:
			{
				string text3 = cont.content.ToLowerInvariant();
				uint num6 = <PrivateImplementationDetails>.ComputeStringHash(text3);
				Sting sting;
				if (num6 <= 1322331238U)
				{
					if (num6 <= 563675566U)
					{
						if (num6 != 327445277U)
						{
							if (num6 != 563675566U)
							{
								goto IL_0802;
							}
							if (!(text3 == "danger"))
							{
								goto IL_0802;
							}
							sting = Sting.Danger;
							goto IL_0805;
						}
						else
						{
							if (!(text3 == "triumphant"))
							{
								goto IL_0802;
							}
							sting = Sting.Triumphant;
							goto IL_0805;
						}
					}
					else if (num6 != 1282981772U)
					{
						if (num6 != 1322331238U)
						{
							goto IL_0802;
						}
						if (!(text3 == "worry"))
						{
							goto IL_0802;
						}
						sting = Sting.Worry;
						goto IL_0805;
					}
					else if (!(text3 == "longdistance"))
					{
						goto IL_0802;
					}
				}
				else if (num6 <= 2268106109U)
				{
					if (num6 != 1357725606U)
					{
						if (num6 != 2268106109U)
						{
							goto IL_0802;
						}
						if (!(text3 == "minitriumph"))
						{
							goto IL_0802;
						}
						sting = Sting.MiniTriumph;
						goto IL_0805;
					}
					else if (!(text3 == "long distance"))
					{
						goto IL_0802;
					}
				}
				else if (num6 != 2561280663U)
				{
					if (num6 != 3873349473U)
					{
						goto IL_0802;
					}
					if (!(text3 == "night"))
					{
						goto IL_0802;
					}
					sting = Sting.Night;
					goto IL_0805;
				}
				else
				{
					if (!(text3 == "miniworry"))
					{
						goto IL_0802;
					}
					sting = Sting.MiniWorry;
					goto IL_0805;
				}
				sting = Sting.LongDistance;
				goto IL_0805;
				IL_0802:
				sting = Sting.Triumphant;
				IL_0805:
				AudioController.instance.PlaySting(sting, -1);
				break;
			}
			case LegacyInstruction.Type.BackgroundMusic:
				AudioController.instance.SetAmbientMusicAllowedByInk(!cont.end);
				break;
			case LegacyInstruction.Type.StoneSkimming:
				if (cont.end)
				{
					if (runner.stoneSkimming)
					{
						runner.EndStoneSkimming();
					}
				}
				else if (cont.content.StartsWith("Throw at"))
				{
					string text4 = cont.content.Substring(9).Trim();
					Prop prop = Prop.FindNearestByInkName(Runner.instance.physicalPosition3d, text4);
					if (prop == null)
					{
						Debug.LogError("Couldn't find stone throw target Prop " + text4);
					}
					runner.StartStoneThrowAtTarget(prop);
				}
				else
				{
					runner.StartStoneSkimming(PropsController.instance.lastInteractedProp);
				}
				break;
			case LegacyInstruction.Type.BellyWriggle:
				if (cont.end)
				{
					runner.EndBellyWriggle();
				}
				else
				{
					runner.StartBellyWriggle();
				}
				break;
			case LegacyInstruction.Type.BlackBars:
				MonoSingleton<BlackBars>.instance.SetVisible(!cont.end, BlackBarsReason.Ink);
				break;
			case LegacyInstruction.Type.Viewpoint:
				if (!cont.end)
				{
					Game.instance.StartPeakState();
				}
				else
				{
					while (Game.instance.inPeakState)
					{
						yield return null;
					}
				}
				break;
			case LegacyInstruction.Type.Progress:
				if (!cont.end)
				{
					string text5 = InkStylingUtility.ProcessText(cont.content, true, false);
					MonoSingleton<ProgressBanner>.instance.Show(text5);
				}
				if (!cont.start)
				{
					while (MonoSingleton<ProgressBanner>.instance.visible)
					{
						yield return null;
					}
				}
				break;
			case LegacyInstruction.Type.ShelterCam:
				if (!cont.end)
				{
					GameCamera.instance.shelterState.targetStrength = 1f;
				}
				else
				{
					GameCamera.instance.shelterState.targetStrength = 0f;
				}
				break;
			case LegacyInstruction.Type.PlayerControl:
				if (cont.end)
				{
					runner.playerControlDisabled |= PlayerControlDisableReason.Ink;
				}
				else
				{
					runner.playerControlDisabled &= ~PlayerControlDisableReason.Ink;
					runner.playerControlDisabled &= ~PlayerControlDisableReason.AutoRunToProp;
					runner.playerControlDisabled &= ~PlayerControlDisableReason.InkPose;
					runner.playerControlDisabled &= ~PlayerControlDisableReason.NarrativeChoicesWithNoExit;
				}
				break;
			case LegacyInstruction.Type.HidePlayer:
			{
				bool flag3 = !cont.end;
				runner.SetHidden(flag3, HideReason.Ink);
				break;
			}
			case LegacyInstruction.Type.EnterExitDoor:
			{
				while (runner.state == Runner.State.EnterExitDoor)
				{
					yield return null;
				}
				bool start = cont.start;
				runner.EnterExitDoor(start);
				break;
			}
			case LegacyInstruction.Type.WaitUntil:
			case LegacyInstruction.Type.WaitFor:
			{
				GameClock clock = GameClock.instance;
				if (!cont.end)
				{
					float num8;
					if (cont.type == LegacyInstruction.Type.WaitUntil)
					{
						float floatContent = cont.floatContent;
						int num7;
						if (clock.hourOfDay < floatContent)
						{
							num7 = clock.dayIdx;
						}
						else
						{
							num7 = clock.dayIdx + 1;
						}
						num8 = (float)num7 + floatContent / 24f;
					}
					else
					{
						num8 = clock.daysNorm + cont.floatContent / 24f;
					}
					base.StartCoroutine(clock.WaitUntilDaysNorm(num8));
				}
				if (!cont.start)
				{
					while (clock.isWaitingForTimeToPass)
					{
						yield return null;
					}
				}
				break;
			}
			case LegacyInstruction.Type.FollowPath:
				if (!cont.end)
				{
					Game.instance.FollowPath();
				}
				else
				{
					yield return Game.instance.AwaitFollowPathComplete();
				}
				break;
			case LegacyInstruction.Type.Animate:
			{
				string text6 = cont.content;
				bool flag4 = false;
				int num9 = text6.IndexOf("(queue)");
				if (num9 != -1)
				{
					flag4 = true;
					text6 = text6.Substring(0, num9);
				}
				string[] array3 = text6.Split(',', StringSplitOptions.None);
				string text7 = array3[0].Trim();
				if (!cont.end)
				{
					string text8 = null;
					if (array3.Length > 1)
					{
						text8 = array3[1].Trim();
					}
					InkAnimation inkAnimation;
					if (!InkAnimation.all.TryGetValue(text7, out inkAnimation))
					{
						Debug.LogError("Ink animation not found: " + text7);
						break;
					}
					inkAnimation.Begin(text8, flag4);
				}
				if (!cont.start)
				{
					yield return InkAnimation.AwaitCompletion(text7);
				}
				break;
			}
			case LegacyInstruction.Type.Pose:
				if (!cont.end)
				{
					string text9 = null;
					string[] array4 = this.SplitContentByComma(cont.content);
					string text10;
					if (array4.Length > 1)
					{
						text9 = array4[0];
						text10 = array4[1];
					}
					else
					{
						text10 = array4[0];
					}
					if (text9 == null && !runner.hidden)
					{
						Prop lastInteractedProp = PropsController.instance.lastInteractedProp;
						if (lastInteractedProp != null)
						{
							runner.TurnToFace(lastInteractedProp);
						}
						runner.SetPoseFromInk(cont.content);
					}
					else if (text9 != null)
					{
						StoryCharacter storyCharacter = StoryCharacter.WithName(text9);
						if (storyCharacter != null)
						{
							string text11 = null;
							if (text10.Contains(" THEN "))
							{
								string[] array5 = text10.Split(" THEN ", StringSplitOptions.None);
								text11 = array5[0].Trim();
								text10 = array5[1].Trim();
							}
							storyCharacter.SetCustomPose(text11, text10);
						}
						else
						{
							Debug.LogError(">>> START POSE -- the character '" + text9 + "' couldn't be found");
						}
					}
				}
				if (cont.end)
				{
					if (string.IsNullOrWhiteSpace(cont.content))
					{
						runner.RemovePose();
					}
					else
					{
						StoryCharacter storyCharacter2 = StoryCharacter.WithName(cont.content);
						if (storyCharacter2 != null)
						{
							storyCharacter2.SetCustomPose(null, null);
						}
						else
						{
							Debug.LogError(">>> END POSE -- the character '" + cont.content + "' couldn't be found");
						}
					}
				}
				break;
			case LegacyInstruction.Type.Face:
				if (cont.content == "LEFT")
				{
					runner.direction = -1f;
				}
				else if (cont.content == "RIGHT")
				{
					runner.direction = 1f;
				}
				else
				{
					StoryCharacter storyCharacter3 = StoryCharacter.WithName(cont.content);
					if (storyCharacter3 != null)
					{
						runner.TurnToFace(storyCharacter3);
					}
					else
					{
						List<Prop> loadedPropsByInkName2 = Prop.GetLoadedPropsByInkName(cont.content);
						int num10 = loadedPropsByInkName2.Count<Prop>();
						if (num10 > 0)
						{
							if (num10 > 1)
							{
								Debug.LogWarning("Several props with ink name " + cont.content + ". Looking at the first one found.");
							}
							runner.TurnToFace(loadedPropsByInkName2.First<Prop>());
						}
					}
				}
				break;
			case LegacyInstruction.Type.Teleport:
			{
				string text12 = cont.content;
				bool flag5 = true;
				int num11 = cont.content.IndexOf(",");
				if (num11 != -1)
				{
					text12 = cont.content.Substring(0, num11).Trim();
					string text13 = cont.content.Substring(num11 + 1).Trim();
					if (text13 == "NO FADE")
					{
						flag5 = false;
					}
					else if (text13 == "FADE")
					{
						flag5 = true;
					}
					else
					{
						Debug.LogError(">>> TELEPORT: Unexpected extra content: '" + text13 + "' - only expected option is 'NO FADE' or 'FADE', otherwise please leave it out.");
					}
				}
				Prop prop2 = Prop.FindNearestByInkName(Runner.instance.transform.position, text12);
				if (prop2 != null)
				{
					bool dead = Runner.instance.dead;
					yield return Game.instance.TeleportPlayerTo3DCR(prop2.transform.position + Vector3.up, "Ink teleport to " + text12, 0, flag5, null, dead, false, null, null);
				}
				else
				{
					Debug.LogError("Ink TELEPORT couldn't find destination prop: " + text12);
				}
				break;
			}
			case LegacyInstruction.Type.Fade:
				if (cont.end)
				{
					while (Blackout.isAnimating)
					{
						yield return null;
					}
				}
				else if (cont.content == "OUT")
				{
					Blackout.FadeOut(null);
					if (!cont.start)
					{
						yield return new WaitForSeconds(Blackout.fadeOutTime);
					}
				}
				else
				{
					Blackout.FadeIn(0f, null);
					if (!cont.start)
					{
						yield return new WaitForSeconds(Blackout.fadeInTime);
					}
				}
				break;
			case LegacyInstruction.Type.GetMaps:
				foreach (string text14 in cont.content.Split(',', StringSplitOptions.None))
				{
					MonoSingleton<MapsViewController>.instance.GetMapWithPropName(text14.Trim());
				}
				while (MonoSingleton<MapsViewController>.instance.gettingMaps)
				{
					yield return null;
				}
				break;
			case LegacyInstruction.Type.MapConfirm:
				if (cont.end)
				{
					if (MonoSingleton<JournalController>.instance.mapConfirmActive)
					{
						MonoSingleton<JournalController>.instance.EndMapConfirm();
					}
					else
					{
						yield return MonoSingleton<MapsViewController>.instance.EndMapConfirm(cont.content == "success");
					}
				}
				else
				{
					Debug.LogError("MapConfirm instruction is only designed to work with >>> END MAP CONFIRM: success/fail because start is handled by NarrativePresenter as soon as you make the choice");
				}
				break;
			case LegacyInstruction.Type.Invincible:
				if (cont.end)
				{
					Runner.instance.health.SetInvincible(false, Health.InvincibleReason.InkScene);
				}
				else
				{
					Runner.instance.health.SetInvincible(true, Health.InvincibleReason.InkScene);
				}
				break;
			case LegacyInstruction.Type.Resurrect:
				Runner.instance.Resurrect();
				break;
			case LegacyInstruction.Type.PeakBanner:
				yield return MonoSingleton<PeakClimbedBanner>.instance.Play_Coroutine(cont.content);
				break;
			case LegacyInstruction.Type.Weather:
			{
				string[] array7 = cont.content.Split(",", StringSplitOptions.None);
				string text15 = null;
				string text16;
				if (array7.Length == 2)
				{
					text15 = array7[0].Trim();
					text16 = array7[1].Trim();
				}
				else
				{
					if (array7.Length != 1)
					{
						Debug.LogError("Unexpected number of arguments to >>> WEATHER instruction. Expected 1 or 2, but got: " + cont.content);
						break;
					}
					text16 = array7[0];
				}
				WeatherType weatherType;
				if (!Enum.TryParse<WeatherType>(text16, out weatherType))
				{
					Debug.LogError(">>> WEATHER instruction: Didn't recognise weather type " + text16);
				}
				else if (text15 == null)
				{
					WeatherSystem.instance.BeginGlobalInkOverride(weatherType);
				}
				else
				{
					WeatherSystem.instance.BeginLocalInkOverride(text15, weatherType);
				}
				break;
			}
			case LegacyInstruction.Type.WeatherPattern:
				WeatherSystem.instance.SetWeatherPatternFromInk(cont.content);
				break;
			case LegacyInstruction.Type.Eagle:
				if (!cont.end)
				{
					MonoSingleton<Eagle>.instance.StartPickupAndFlyTo(cont.content);
				}
				if (cont.end)
				{
					while (!MonoSingleton<Eagle>.instance.complete)
					{
						yield return null;
					}
				}
				break;
			case LegacyInstruction.Type.ChairLift:
				if (cont.start)
				{
					MonoSingleton<SkiLift>.instance.running = true;
				}
				if (cont.end)
				{
					MonoSingleton<SkiLift>.instance.running = false;
				}
				break;
			case LegacyInstruction.Type.Boat:
				if (cont.start)
				{
					Game.instance.StartBoat();
				}
				if (cont.end)
				{
					while (Game.instance.activeBoat != null)
					{
						yield return null;
					}
				}
				break;
			case LegacyInstruction.Type.ZipLine:
				if (!cont.end)
				{
					Runner.instance.StartZipLine();
				}
				if (!cont.start)
				{
					while (Runner.instance.isZipLining)
					{
						yield return null;
					}
				}
				break;
			case LegacyInstruction.Type.FinalMusic:
				if (cont.start)
				{
					if (cont.content == "blessing")
					{
						AudioController.instance.StartBlessingMusic();
					}
					else if (cont.content == "super")
					{
						AudioController.instance.StartFinalJumpMusic();
					}
					else if (cont.content == "good")
					{
						AudioController.instance.StartEndingMusic();
					}
					else
					{
						AudioController.instance.StartEndingMusic();
					}
				}
				else if (cont.content.ToLowerInvariant().Trim() == "enter final loop")
				{
					AudioController.instance.StartFinalJumpMusic();
					while (!AudioController.instance.doubleSyncTrack.playing)
					{
						yield return null;
					}
					AudioController.instance.SyncMusicToFinalLoop();
				}
				break;
			case LegacyInstruction.Type.EndGame:
				Game.instance.PlayerReachedEndOfGame();
				break;
			case LegacyInstruction.Type.Achievement:
				MonoSingleton<AchievementsManager>.instance.UnlockWithInkID(cont.content);
				break;
			case LegacyInstruction.Type.Particles:
			{
				string[] array8 = this.SplitContentByComma(cont.content);
				if (array8.Length == 0)
				{
					Debug.LogError(">>> PARTICLES instruction specified no Prop");
				}
				else
				{
					Prop prop3 = Prop.FindNearestByInkName(Runner.instance.physicalPosition3d, array8[0]);
					if (prop3 == null)
					{
						Debug.LogError("Ink >>> PARTICLES instructions looked for particles attached to a Prop called " + cont.content + " but that Prop couldn't be found.");
					}
					else
					{
						ParticleSystem componentInChildren = prop3.GetComponentInChildren<ParticleSystem>();
						if (componentInChildren == null)
						{
							Debug.LogError("Ink >>> PARTICLES instruction looked for particles attached to a Prop called " + cont.content + ". The Prop exists but doesn't appear to have a ParticleSystem in its hierarchy.", prop3);
						}
						else if (cont.start)
						{
							ParticlesX.SetActive(componentInChildren, true);
						}
						else if (cont.end)
						{
							ParticlesX.SetActive(componentInChildren, false);
						}
						else if (!cont.start && !cont.end)
						{
							int num12 = 10;
							if (array8.Length > 1 && !int.TryParse(array8[1], out num12))
							{
								Debug.LogError(">>> PARTICLES instruction with content 'cont.content' expected number of particles as second parameter but saw " + array8[1]);
							}
							componentInChildren.Emit(num12);
						}
					}
				}
				break;
			}
			case LegacyInstruction.Type.PlayerExplosion:
				if (!cont.end)
				{
					Runner.instance.Explode(cont.content);
				}
				if (!cont.start)
				{
					while (Runner.instance.exploded)
					{
						yield return null;
					}
				}
				break;
			case LegacyInstruction.Type.PropDots:
				if (cont.start)
				{
					PropsController.instance.SetPropDotsAllowedByInk(true);
				}
				else if (cont.end)
				{
					PropsController.instance.SetPropDotsAllowedByInk(false);
				}
				break;
			case LegacyInstruction.Type.DeathAudio:
				if (cont.start)
				{
					AudioController.instance.deathAudioWantedByInk = true;
				}
				else if (cont.end)
				{
					AudioController.instance.deathAudioWantedByInk = false;
				}
				break;
			case LegacyInstruction.Type.Event:
				if (!cont.end && Narrative.onEventDidFire != null)
				{
					Narrative.onEventDidFire(cont.content);
				}
				if (!cont.start)
				{
					while (!this._eventsEnded.Contains(cont.content))
					{
						yield return null;
					}
					this._eventsEnded.Remove(cont.content);
				}
				break;
			case LegacyInstruction.Type.Error:
				Debug.LogError(">>> ERROR: " + cont.content);
				break;
			case LegacyInstruction.Type.Log:
				Debug.LogError(">>> LOG: " + cont.content);
				break;
			default:
				Debug.LogError("Story command parsed correctly but unhandled: " + cont.type.ToString());
				break;
			}
			cont = null;
		}
		yield break;
	}

	// Token: 0x0600049A RID: 1178 RVA: 0x00025CF8 File Offset: 0x00023EF8
	private void DisableAllInkCameras(bool forceAll)
	{
		List<CameraVolume> list = new List<CameraVolume>();
		foreach (CameraVolume cameraVolume in this._enabledCameraVolumes)
		{
			if (!cameraVolume.persistAfterInkCompletes || forceAll)
			{
				cameraVolume.inkHasEnabled = false;
			}
			else
			{
				list.Add(cameraVolume);
			}
		}
		this._enabledCameraVolumes = list;
	}

	// Token: 0x0600049B RID: 1179 RVA: 0x00025D70 File Offset: 0x00023F70
	public Narrative.NewGamePlusStrings NewGamePlusText()
	{
		this.FinishAsyncIfNecessary();
		string text;
		this.inkStory.EvaluateFunction("newGamePlusText", out text, Array.Empty<object>());
		string[] array = text.Split("\n", StringSplitOptions.RemoveEmptyEntries);
		return new Narrative.NewGamePlusStrings
		{
			title = array[0],
			subtitle = string.Join("\n", array, 1, array.Length - 2),
			buttonText = array[array.Length - 1]
		};
	}

	// Token: 0x0600049C RID: 1180 RVA: 0x00025DE1 File Offset: 0x00023FE1
	private void Awake()
	{
		this.inkStory = null;
		NarrativePresenter.onChooseChoice = (NarrativePresenter.ChooseChoiceDelegate)Delegate.Combine(NarrativePresenter.onChooseChoice, new NarrativePresenter.ChooseChoiceDelegate(this.OnPresenterChoseChoice));
	}

	// Token: 0x0600049D RID: 1181 RVA: 0x00025E0A File Offset: 0x0002400A
	private void OnEnable()
	{
		GameClock.onInkIntervalDidPass = (Action)Delegate.Combine(GameClock.onInkIntervalDidPass, new Action(this.OnGameClockIntervalDidPass));
	}

	// Token: 0x0600049E RID: 1182 RVA: 0x00025E2C File Offset: 0x0002402C
	private void OnDisable()
	{
		GameClock.onInkIntervalDidPass = (Action)Delegate.Remove(GameClock.onInkIntervalDidPass, new Action(this.OnGameClockIntervalDidPass));
	}

	// Token: 0x0600049F RID: 1183 RVA: 0x00025E4E File Offset: 0x0002404E
	private void OnDestroy()
	{
		NarrativePresenter.onChooseChoice = (NarrativePresenter.ChooseChoiceDelegate)Delegate.Remove(NarrativePresenter.onChooseChoice, new NarrativePresenter.ChooseChoiceDelegate(this.OnPresenterChoseChoice));
	}

	// Token: 0x060004A0 RID: 1184 RVA: 0x00025E70 File Offset: 0x00024070
	private void OnPresenterChoseChoice(GameChoice choice, string optionalInteractableName)
	{
		if (this._inkEvaluationCoroutine == null)
		{
			this.RunInkWithChoiceIndex(choice.inkChoiceIdx, optionalInteractableName);
			return;
		}
		this.ContinueInkWithChoiceIndex(choice.inkChoiceIdx, optionalInteractableName);
	}

	// Token: 0x060004A1 RID: 1185 RVA: 0x00025E95 File Offset: 0x00024095
	private void OnGameClockIntervalDidPass()
	{
		this.inkStory.variablesState["hour"] = Mathf.FloorToInt(GameClock.instance.hourOfDay);
	}

	// Token: 0x060004BD RID: 1213 RVA: 0x00026BB0 File Offset: 0x00024DB0
	[CompilerGenerated]
	internal static InkList <UpdateWeather>g__WeatherTypeToInkList|195_0(WeatherType weatherType, WeatherHealthEffect effects)
	{
		InkList inkList = new InkList("Weathers", Narrative.instance.inkStory);
		if (weatherType == WeatherType.Clear)
		{
			inkList.AddItem("Clear", null);
		}
		if ((weatherType & WeatherType.Cloudy) > WeatherType.Clear)
		{
			inkList.AddItem("Cloudy", null);
		}
		if ((weatherType & WeatherType.Foggy) > WeatherType.Clear)
		{
			inkList.AddItem("Foggy", null);
		}
		if ((effects & WeatherHealthEffect.Rain) > WeatherHealthEffect.None)
		{
			inkList.AddItem("Raining", null);
		}
		if ((effects & WeatherHealthEffect.Storm) > WeatherHealthEffect.None)
		{
			inkList.AddItem("Raining", null);
			inkList.AddItem("Storm", null);
		}
		if ((effects & WeatherHealthEffect.Snow) > WeatherHealthEffect.None)
		{
			inkList.AddItem("Snow", null);
		}
		if ((effects & WeatherHealthEffect.Wind) > WeatherHealthEffect.None)
		{
			inkList.AddItem("Windy", null);
		}
		if ((effects & WeatherHealthEffect.StrongWind) > WeatherHealthEffect.None)
		{
			inkList.AddItem("Windy", null);
			inkList.AddItem("StrongWindy", null);
		}
		if ((effects & WeatherHealthEffect.Chilly) > WeatherHealthEffect.None)
		{
			inkList.AddItem("Chilly", null);
		}
		if ((effects & WeatherHealthEffect.Cold) > WeatherHealthEffect.None)
		{
			inkList.AddItem("Chilly", null);
			inkList.AddItem("Cold", null);
		}
		if ((effects & WeatherHealthEffect.Freezing) > WeatherHealthEffect.None)
		{
			inkList.AddItem("Chilly", null);
			inkList.AddItem("Cold", null);
			inkList.AddItem("Freezing", null);
		}
		return inkList;
	}

	// Token: 0x04000559 RID: 1369
	public const string profanityPrefName = "Profanity";

	// Token: 0x0400055A RID: 1370
	private static bool _gotCachedProfanitySetting;

	// Token: 0x0400055B RID: 1371
	private static bool _profanity;

	// Token: 0x0400055C RID: 1372
	public TextAsset ink;

	// Token: 0x0400055D RID: 1373
	public float asyncMillisecsBudget = 4f;

	// Token: 0x0400055E RID: 1374
	public const string runningMakesMeStrongerFact = "RUNNING_MAKES_ME_STRONGER";

	// Token: 0x0400055F RID: 1375
	private Story _inkStory;

	// Token: 0x04000560 RID: 1376
	private const bool shouldDebugLog = false;

	// Token: 0x04000561 RID: 1377
	[Disable]
	public Narrative.PreventBackgroundRemarksReason preventBackgroundRemarks;

	// Token: 0x04000562 RID: 1378
	public bool blockedForJournalChoice;

	// Token: 0x04000563 RID: 1379
	public int lastNightApproachRemarkDayIdx = -1;

	// Token: 0x04000564 RID: 1380
	public string activeAutoCutZoneName;

	// Token: 0x04000565 RID: 1381
	private static string[] loopableVariables = new string[]
	{
		"BackstoryRemarks", "DiscoveredFacts", "KnownPeaks", "LoopedStates", "WomansStory", "pairstore", "HamishAndMotherChunks", "Hawks", "NaturalPickupZones", "MapsYouveGot",
		"mapsConfirmed", "RedundantMaps", "BadEndings", "Inventory", "Achievements", "MusicTracks", "PeaksForStoryPrompts", "BaggedPeaks", "ScriptChunkBothies", "fastestJourneyTimeInDays",
		"PeaksMentionedByName", "PeaksMentioned", "numberOfMapsConfirmedThisGame", "reachLightHouseOnDay"
	};

	// Token: 0x04000566 RID: 1382
	private static string[] recentStateLists = new string[] { "recentStates1", "recentStates2", "recentStates3" };

	// Token: 0x04000567 RID: 1383
	private static string[] knownStateLists = new string[] { "AllStates", "DiscoveredFacts", "PeaksWeveVisitedThisGame", "LoopedStates", "PeaksMentionedByName" };

	// Token: 0x04000568 RID: 1384
	public Dictionary<string, int> orderOfAquisitionIdx = new Dictionary<string, int>();

	// Token: 0x04000569 RID: 1385
	public int nextAquisitionIdx;

	// Token: 0x0400056C RID: 1388
	private static readonly char[] _pairStoreStringDelimiter = new char[] { ';' };

	// Token: 0x0400056D RID: 1389
	private static readonly char[] _pairStoreElementDelimiter = new char[] { '>' };

	// Token: 0x0400056E RID: 1390
	private Dictionary<string, Dictionary<string, List<string>>> _cachedPairStore = new Dictionary<string, Dictionary<string, List<string>>>();

	// Token: 0x0400056F RID: 1391
	private Dictionary<string, Dictionary<string, List<string>>> _reversedCachedPairStore = new Dictionary<string, Dictionary<string, List<string>>>();

	// Token: 0x04000570 RID: 1392
	private List<Narrative.InkItemDescription> _inkItemDescriptionList = new List<Narrative.InkItemDescription>(128);

	// Token: 0x04000571 RID: 1393
	private List<Narrative.NamedPeakWidget> _namedPeakWidgetScratchList = new List<Narrative.NamedPeakWidget>(128);

	// Token: 0x04000572 RID: 1394
	private List<Narrative.PeakKnowledge> _peakKnowledgeList = new List<Narrative.PeakKnowledge>(128);

	// Token: 0x04000573 RID: 1395
	private static string[] _emptyListItemNames = new string[0];

	// Token: 0x04000574 RID: 1396
	private List<string> _keysToFind = new List<string>();

	// Token: 0x04000575 RID: 1397
	public string _inkEvaluationCoroutineContext;

	// Token: 0x04000576 RID: 1398
	public string _inkEvaluationCoroutineLine;

	// Token: 0x04000577 RID: 1399
	private IEnumerator _inkEvaluationCoroutine;

	// Token: 0x04000578 RID: 1400
	public string debugInkEvaluationStartPoint;

	// Token: 0x04000579 RID: 1401
	private bool _forcingCoroutineSyncComplete;

	// Token: 0x0400057A RID: 1402
	private bool _clearing;

	// Token: 0x0400057B RID: 1403
	private List<Prop> _nearbyPropsScratch = new List<Prop>(256);

	// Token: 0x0400057C RID: 1404
	private List<Prop> _lastInteractablesRefreshPropList = new List<Prop>(256);

	// Token: 0x0400057D RID: 1405
	private InkList _availableInteractableInkItems_Scratch;

	// Token: 0x0400057E RID: 1406
	private List<GameChoice> _activeInteractableChoices = new List<GameChoice>();

	// Token: 0x0400057F RID: 1407
	private HashSet<string> _lastInteractableNames = new HashSet<string>();

	// Token: 0x04000580 RID: 1408
	private List<Narrative.QueuedKnot> _queuedKnots = new List<Narrative.QueuedKnot>();

	// Token: 0x04000581 RID: 1409
	private Narrative.QueuedKnot _activeQueueableKnot;

	// Token: 0x04000582 RID: 1410
	private float _lullRemarkStartWaitingFromTime;

	// Token: 0x04000583 RID: 1411
	private float _lastRecoveryBarkTime;

	// Token: 0x04000584 RID: 1412
	private bool _hasRemarkedDuringCurrentLookFurther;

	// Token: 0x04000585 RID: 1413
	private float _lastLookFurtherRemarkTime = float.MinValue;

	// Token: 0x04000586 RID: 1414
	private Prop _lastRunToProp;

	// Token: 0x04000587 RID: 1415
	private WeatherType _lastInkKnownWeatherType;

	// Token: 0x04000588 RID: 1416
	private WeatherHealthEffect _lastInkKnownRawWeatherEffect;

	// Token: 0x04000589 RID: 1417
	private WeatherHealthEffect _lastInkKnownProtectedWeatherEffect;

	// Token: 0x0400058A RID: 1418
	private List<CameraVolume> _enabledCameraVolumes = new List<CameraVolume>();

	// Token: 0x0400058B RID: 1419
	private Dictionary<InkListItem, int> _allPeaks;

	// Token: 0x0400058C RID: 1420
	private bool _inCave;

	// Token: 0x0400058D RID: 1421
	private Map _recentlyRemarkedNearbyMap;

	// Token: 0x0400058E RID: 1422
	private float _recentlyRemarkedNearbyMapTime;

	// Token: 0x0400058F RID: 1423
	private const string exitChoicePrefix = "EXIT ";

	// Token: 0x04000590 RID: 1424
	private List<string> _eventsEnded = new List<string>();

	// Token: 0x04000591 RID: 1425
	private List<string> _debugLog = new List<string>();

	// Token: 0x04000592 RID: 1426
	private StringBuilder _sb = new StringBuilder();

	// Token: 0x04000593 RID: 1427
	private bool _hadError;

	// Token: 0x04000594 RID: 1428
	private bool _ranOutOfContent;

	// Token: 0x04000595 RID: 1429
	private Narrative.PauseReason _pauseReason;

	// Token: 0x04000596 RID: 1430
	private Stopwatch _stopwatch = new Stopwatch();

	// Token: 0x04000597 RID: 1431
	private static List<GameChoice> _choicesScratch = new List<GameChoice>();

	// Token: 0x04000598 RID: 1432
	private static List<string> _removedInventoryScratch = new List<string>(4);

	// Token: 0x020002A8 RID: 680
	[Flags]
	public enum PreventBackgroundRemarksReason
	{
		// Token: 0x040015A8 RID: 5544
		None = 0,
		// Token: 0x040015A9 RID: 5545
		LateNight = 1,
		// Token: 0x040015AA RID: 5546
		PoorRunnerState = 2,
		// Token: 0x040015AB RID: 5547
		MusicRunning = 4,
		// Token: 0x040015AC RID: 5548
		InPeakState = 8,
		// Token: 0x040015AD RID: 5549
		RemarkBlockerZone = 16,
		// Token: 0x040015AE RID: 5550
		Loading = 64,
		// Token: 0x040015AF RID: 5551
		PropHighlighted = 128,
		// Token: 0x040015B0 RID: 5552
		FeelingExhausted = 256
	}

	// Token: 0x020002A9 RID: 681
	[Flags]
	public enum PauseReason
	{
		// Token: 0x040015B2 RID: 5554
		None = 0,
		// Token: 0x040015B3 RID: 5555
		Intro = 1,
		// Token: 0x040015B4 RID: 5556
		Journal = 2,
		// Token: 0x040015B5 RID: 5557
		PhotoMode = 3
	}

	// Token: 0x020002AA RID: 682
	public struct InkItemDescription
	{
		// Token: 0x040015B6 RID: 5558
		public string inkItemName;

		// Token: 0x040015B7 RID: 5559
		public string description;

		// Token: 0x040015B8 RID: 5560
		public int inkListValue;
	}

	// Token: 0x020002AB RID: 683
	public struct NamedPeakWidget
	{
		// Token: 0x040015B9 RID: 5561
		public Prop prop;

		// Token: 0x040015BA RID: 5562
		public string name;

		// Token: 0x040015BB RID: 5563
		public bool hasMap;
	}

	// Token: 0x020002AC RID: 684
	public struct PeakKnowledge
	{
		// Token: 0x040015BC RID: 5564
		public bool visited;

		// Token: 0x040015BD RID: 5565
		public string inkItemName;

		// Token: 0x040015BE RID: 5566
		public string englishName;

		// Token: 0x040015BF RID: 5567
		public string gaelicName;

		// Token: 0x040015C0 RID: 5568
		public bool correctlyNamed;

		// Token: 0x040015C1 RID: 5569
		public bool blessed;
	}

	// Token: 0x020002AD RID: 685
	public enum ClearType
	{
		// Token: 0x040015C3 RID: 5571
		Visual,
		// Token: 0x040015C4 RID: 5572
		Death,
		// Token: 0x040015C5 RID: 5573
		FullState,
		// Token: 0x040015C6 RID: 5574
		FullStatePreservingLoopVariables
	}

	// Token: 0x020002AE RID: 686
	public struct NewGamePlusStrings
	{
		// Token: 0x040015C7 RID: 5575
		public string title;

		// Token: 0x040015C8 RID: 5576
		public string subtitle;

		// Token: 0x040015C9 RID: 5577
		public string buttonText;
	}

	// Token: 0x020002AF RID: 687
	private struct QueuedKnot
	{
		// Token: 0x060015C1 RID: 5569 RVA: 0x00094BDD File Offset: 0x00092DDD
		public bool Equals(Narrative.QueuedKnot k)
		{
			return this.knotName == k.knotName && k.arguments.SequenceEqual(this.arguments);
		}

		// Token: 0x040015CA RID: 5578
		public string knotName;

		// Token: 0x040015CB RID: 5579
		public object[] arguments;
	}
}
