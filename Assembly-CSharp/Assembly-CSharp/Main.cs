using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Ink.Runtime;
using UnityEngine;

// Token: 0x02000068 RID: 104
public class Main : MonoSingleton<Main>
{
	// Token: 0x170000CA RID: 202
	// (get) Token: 0x060002E0 RID: 736 RVA: 0x0001728B File Offset: 0x0001548B
	public bool isAtStart
	{
		get
		{
			return this.saveState == null || this.saveState.playTime < 1f;
		}
	}

	// Token: 0x170000CB RID: 203
	// (get) Token: 0x060002E1 RID: 737 RVA: 0x000172A9 File Offset: 0x000154A9
	public static bool saveThreadIsActive
	{
		get
		{
			return Main._saveThread != null;
		}
	}

	// Token: 0x060002E2 RID: 738 RVA: 0x000172B3 File Offset: 0x000154B3
	private void Awake()
	{
		Main.timeOnStart = DateTime.Now;
	}

	// Token: 0x060002E3 RID: 739 RVA: 0x000172BF File Offset: 0x000154BF
	private void OnEnable()
	{
		this.timeSinceLastSave = float.PositiveInfinity;
	}

	// Token: 0x060002E4 RID: 740 RVA: 0x000172CC File Offset: 0x000154CC
	private void OnDisable()
	{
		this.timeSinceLastSave = float.PositiveInfinity;
	}

	// Token: 0x060002E5 RID: 741 RVA: 0x000172D9 File Offset: 0x000154D9
	private void Start()
	{
		MonoSingleton<Launcher>.instance.LaunchOnApplicationLoad();
	}

	// Token: 0x060002E6 RID: 742 RVA: 0x000172E8 File Offset: 0x000154E8
	private void Update()
	{
		if (!Game.loaded)
		{
			return;
		}
		this.CheckForCompletedSaveThread();
		if (Game.gameplayPaused)
		{
			return;
		}
		if (Game.instance.inActiveGameplay || Game.instance.isPathFollowing || Game.instance.inPeakState)
		{
			this.saveState.playTime += Time.deltaTime;
		}
		this.timeSinceLastSave += Time.unscaledDeltaTime;
		if (this.timeSinceLastSave > 15f)
		{
			this.TrySave(false);
		}
	}

	// Token: 0x060002E7 RID: 743 RVA: 0x0001736C File Offset: 0x0001556C
	private void OnApplicationQuit()
	{
		Main.isQuitting = true;
	}

	// Token: 0x060002E8 RID: 744 RVA: 0x00017374 File Offset: 0x00015574
	public void ResetSaveStateCompletely()
	{
		Game.instance.playthroughIdx = 0;
		Tutorial.ResetAll();
		SaveLoadManager.DeleteAll();
		this.CreateNewSaveState();
	}

	// Token: 0x060002E9 RID: 745 RVA: 0x00017394 File Offset: 0x00015594
	public void UpdateSaveState(bool immediateSync)
	{
		this.saveState.saveMetaInfo.saveDateTime = DateTime.Now;
		if (immediateSync)
		{
			this.saveState.storyJSON = Narrative.instance.inkStory.state.ToJson();
		}
		else
		{
			this.inkStateSnapshot = Narrative.instance.inkStory.CopyStateForBackgroundThreadSave();
		}
		Runner instance = Runner.instance;
		Health health = instance.health;
		this.saveState.playthroughIdx = Game.instance.playthroughIdx;
		this.saveState.runnerPosition = instance.transform.position;
		this.saveState.levelIdx = Level.currentIndex;
		this.saveState.runnerXRange = instance.levelXRange;
		this.saveState.runnerHealth = instance.health.currentHealth;
		this.saveState.runnerMaxHealth = health.currentMaxHealth;
		this.saveState.runnerMaxStamina = instance.maxStamina;
		this.saveState.lastMusicRunHealthBoostDayIdx = health.lastMusicRunHealthBoostDayIdx;
		this.saveState.lastWeatherHealthImpact = health.lastWeatherHealthImpact;
		this.saveState.weatherRecoverTimeRemaining = health.weatherRecoverDaysNormRemaining;
		this.saveState.lastNightfallRemarkDayIdx = Narrative.instance.lastNightApproachRemarkDayIdx;
		this.saveState.activeAutoCutZoneName = Narrative.instance.activeAutoCutZoneName;
		this.saveState.hourOfDay = GameClock.instance.hourOfDay;
		this.saveState.dayIdx = GameClock.instance.dayIdx;
		this.saveState.firstDayPauseEnabled = GameClock.instance.firstDayPauseEnabled;
		this.saveState.inventoryMapNames.Clear();
		foreach (Map map in MonoSingleton<Inventory>.instance.maps)
		{
			if (!string.IsNullOrWhiteSpace(map.targetInkPropName))
			{
				this.saveState.inventoryMapNames.Add(map.targetInkPropName);
				if (map == MonoSingleton<MapsViewController>.instance.selectedMap)
				{
					this.saveState.selectedMapIdx = this.saveState.inventoryMapNames.Count - 1;
				}
			}
		}
		if (MonoSingleton<MapsViewController>.instance.selectedMap == null)
		{
			this.saveState.selectedMapIdx = -1;
		}
		this.saveState.orderOfAquisitionIdx.Clear();
		foreach (KeyValuePair<string, int> keyValuePair in Narrative.instance.orderOfAquisitionIdx)
		{
			this.saveState.orderOfAquisitionIdx.Add(new SaveState.OrderOfAquisition
			{
				inkName = keyValuePair.Key,
				idx = keyValuePair.Value
			});
		}
		this.saveState.nextAquisitionIdx = Narrative.instance.nextAquisitionIdx;
		WeatherSystem.instance.UpdateSaveState(this.saveState.weatherSystemSaveState);
		this.saveState.mapMarkerPositions.Clear();
		foreach (KeyValuePair<Map, Vector3> keyValuePair2 in MonoSingleton<MapsViewController>.instance.mapMarkerPositions)
		{
			this.saveState.mapMarkerPositions.Add(new SaveState.MapMarkerPos
			{
				inkName = keyValuePair2.Key.targetInkPropName,
				pos = keyValuePair2.Value
			});
		}
		Main.<UpdateSaveState>g__CopyPropNameList|17_0(PropsController.instance.passedProps, this.saveState.passedProps);
		Main.<UpdateSaveState>g__CopyPropNameList|17_0(PropsController.instance.disabledProps, this.saveState.disabledProps);
		Main.<UpdateSaveState>g__CopyPropNameList|17_0(PropsController.instance.enabledProps, this.saveState.enabledProps);
		Main.<UpdateSaveState>g__CopyPropNameList|17_0(PropsController.instance.completedAutoRunZones, this.saveState.completedAutoRunZones);
		Main.<UpdateSaveState>g__CopyPropNameList|17_0(PropsController.instance.visitedPeaks, this.saveState.visitedPeaks);
		Main.<UpdateSaveState>g__CopyPropNameList|17_0(PropsController.instance.foundPaths, this.saveState.foundPaths);
		this.saveState.activeCreatures.Clear();
		foreach (Creature creature in Level.current.creatures)
		{
			if (creature.shouldSaveState && creature.lastValidSaveState.isValid)
			{
				this.saveState.activeCreatures.Add(creature.lastValidSaveState);
			}
		}
		List<SaveState.AnimatorState> prevAnimStateList = this._prevAnimStateList;
		this._prevAnimStateList = this.saveState.animatorStates;
		this.saveState.animatorStates = prevAnimStateList;
		this.saveState.animatorStates.Clear();
		foreach (KeyValuePair<string, InkAnimation> keyValuePair3 in InkAnimation.all)
		{
			string key = keyValuePair3.Key;
			InkAnimation value = keyValuePair3.Value;
			if (value.animPlayOrder.Count != 0)
			{
				this.saveState.animatorStates.Add(new SaveState.AnimatorState
				{
					animatorName = key,
					animPlayOrder = value.animPlayOrder.ToArray()
				});
			}
		}
		foreach (SaveState.AnimatorState animatorState in this._prevAnimStateList)
		{
			if (!InkAnimation.all.ContainsKey(animatorState.animatorName))
			{
				this.saveState.animatorStates.Add(animatorState);
			}
		}
		this._prevAnimStateList.Clear();
		this.saveState.castleCrowsFlown = Crow.allFlownAway;
		if (this.saveState.walkerStates == null)
		{
			this.saveState.walkerStates = new List<SaveState.InkWalkerState>();
		}
		else
		{
			this.saveState.walkerStates.Clear();
		}
		foreach (InkWalker inkWalker in MonoInstancer<InkWalker>.all)
		{
			this.saveState.walkerStates.Add(new SaveState.InkWalkerState
			{
				guid = inkWalker.guid,
				pos = inkWalker.transform.position,
				dir = Mathf.RoundToInt(Mathf.Sign(inkWalker.transform.localScale.x))
			});
		}
		if (MonoSingleton<SkiLift>.isInstantiated && MonoSingleton<SkiLift>.instance != null)
		{
			this.saveState.chairLiftRunning = MonoSingleton<SkiLift>.instance.running;
		}
		this.saveState.goshawksFlying.Clear();
		foreach (Goshawk goshawk in MonoInstancer<Goshawk>.all)
		{
			if (goshawk.bird.state == Bird.State.Flying)
			{
				this.saveState.goshawksFlying.Add(goshawk.guid);
			}
		}
	}

	// Token: 0x060002EA RID: 746 RVA: 0x00017AE4 File Offset: 0x00015CE4
	public void TrySave(bool force = false)
	{
		this.lastCantSaveReason = SaveLoadManager.CantSaveReason(force);
		if (this.lastCantSaveReason != null)
		{
			return;
		}
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		int levelIdx = this.saveState.levelIdx;
		this.UpdateSaveState(false);
		bool createGameStartSave = float.IsInfinity(this.timeSinceLastSave);
		bool createLevelStartSave = this.saveState.levelIdx != levelIdx;
		bool shuffleBackups = this.saveState.playTime - this.saveState.lastBackupTime > 300f;
		if (shuffleBackups)
		{
			this.saveState.lastBackupTime = this.saveState.playTime;
		}
		stopwatch.Stop();
		Main._saveMainThreadDurationMs = (float)((double)(1000L * stopwatch.ElapsedTicks) / (double)Stopwatch.Frequency);
		Main._saveThread = new Thread(delegate
		{
			Main.SaveThreadFunction(this.saveState, this.inkStateSnapshot, createGameStartSave, createLevelStartSave, shuffleBackups);
		});
		Main._saveThread.Start();
		this.timeSinceLastSave = 0f;
	}

	// Token: 0x060002EB RID: 747 RVA: 0x00017BE8 File Offset: 0x00015DE8
	private static void SaveThreadFunction(SaveState saveState, StoryState inkStateSnapshot, bool createGameStartSave, bool createLevelStartSave, bool shuffleBackups)
	{
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		try
		{
			saveState.storyJSON = inkStateSnapshot.ToJson();
			string text = JsonUtility.ToJson(saveState);
			SaveLoadManager.Save(SaveLoadType.Latest, text);
			if (createGameStartSave)
			{
				SaveLoadManager.TryCopySave(SaveLoadType.Latest, SaveLoadType.GameStart, true);
			}
			else if (createLevelStartSave)
			{
				SaveLoadManager.TryCopySave(SaveLoadType.Latest, SaveLoadType.LevelStart, true);
			}
			if (shuffleBackups)
			{
				SaveLoadManager.TryCopySave(SaveLoadType.Backup3, SaveLoadType.Backup4, false);
				SaveLoadManager.TryCopySave(SaveLoadType.Backup2, SaveLoadType.Backup3, false);
				SaveLoadManager.TryCopySave(SaveLoadType.Backup1, SaveLoadType.Backup2, false);
				SaveLoadManager.TryCopySave(SaveLoadType.Latest, SaveLoadType.Backup1, false);
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("EXCEPTION IN SAVE THREAD: " + ex.Message + " " + ex.StackTrace);
		}
		stopwatch.Stop();
		Main._saveBackgroundThreadDurationMs = (float)((double)(1000L * stopwatch.ElapsedTicks) / (double)Stopwatch.Frequency);
		Main._saveThreadComplete = true;
	}

	// Token: 0x060002EC RID: 748 RVA: 0x00017CBC File Offset: 0x00015EBC
	public void CheckForCompletedSaveThread()
	{
		if (Main._saveThreadComplete)
		{
			this.inkStateSnapshot = null;
			Narrative.instance.inkStory.BackgroundSaveComplete();
			Main._saveThread = null;
			Main._saveThreadComplete = false;
		}
	}

	// Token: 0x060002ED RID: 749 RVA: 0x00017CE7 File Offset: 0x00015EE7
	public void Load(string saveStateJSON)
	{
		this.timeSinceLastSave = 0f;
		MonoSingleton<Launcher>.instance.LoadGameWithSaveState(saveStateJSON);
	}

	// Token: 0x060002EE RID: 750 RVA: 0x00017CFF File Offset: 0x00015EFF
	public void CreateNewSaveState()
	{
		this.timeSinceLastSave = float.PositiveInfinity;
		this.saveState = SaveState.Create();
	}

	// Token: 0x060002EF RID: 751 RVA: 0x00017D18 File Offset: 0x00015F18
	public void ApplySaveStateToGame()
	{
		this.timeSinceLastSave = 0f;
		Runner.instance.health.ApplyFromSave(this.saveState.runnerHealth, this.saveState.runnerMaxHealth, this.saveState.lastWeatherHealthImpact, this.saveState.weatherRecoverTimeRemaining);
		if (this.saveState.runnerMaxStamina != 0f)
		{
			Runner.instance.maxStamina = this.saveState.runnerMaxStamina;
		}
		else
		{
			Runner.instance.maxStamina = 1.01f;
		}
		Runner.instance.levelXRange = this.saveState.runnerXRange;
		Runner.instance.SetSafeResetPoint();
		Game.instance.playthroughIdx = this.saveState.playthroughIdx;
		GameClock.instance.SetDayAndHourWithoutTimePassing(this.saveState.dayIdx, this.saveState.hourOfDay);
		Narrative.instance.lastNightApproachRemarkDayIdx = this.saveState.lastNightfallRemarkDayIdx;
		Narrative.instance.activeAutoCutZoneName = this.saveState.activeAutoCutZoneName;
		Narrative.instance.ResetLullRemarkTime();
		WeatherSystem.instance.LoadSaveState(this.saveState.weatherSystemSaveState);
		MonoSingleton<MapsViewController>.instance.mapMarkerPositions.Clear();
		foreach (SaveState.MapMarkerPos mapMarkerPos in this.saveState.mapMarkerPositions)
		{
			Map map;
			if (Map.allByPropName.TryGetValue(mapMarkerPos.inkName, out map))
			{
				MonoSingleton<MapsViewController>.instance.mapMarkerPositions[map] = mapMarkerPos.pos;
			}
		}
		MonoSingleton<MapsViewController>.instance.RefreshAllWorldMapMarkers();
		MonoSingleton<MapsViewController>.instance.SyncMapMarkersToNarrative();
		MonoSingleton<Inventory>.instance.maps.Clear();
		for (int i = 0; i < this.saveState.inventoryMapNames.Count; i++)
		{
			string text = this.saveState.inventoryMapNames[i];
			Map map2;
			if (!Map.allByPropName.TryGetValue(text, out map2))
			{
				Debug.LogError("Map named " + text + " from save data was not found in game? Has it been renamed/removed?");
			}
			else
			{
				MonoSingleton<Inventory>.instance.maps.Add(map2);
				if (i == this.saveState.selectedMapIdx)
				{
					MonoSingleton<MapsViewController>.instance.selectedMap = map2;
				}
			}
		}
		if (this.saveState.selectedMapIdx == -1)
		{
			MonoSingleton<MapsViewController>.instance.selectedMap = null;
		}
		Narrative.instance.nextAquisitionIdx = this.saveState.nextAquisitionIdx;
		Narrative.instance.orderOfAquisitionIdx.Clear();
		foreach (SaveState.OrderOfAquisition orderOfAquisition in this.saveState.orderOfAquisitionIdx)
		{
			Narrative.instance.orderOfAquisitionIdx[orderOfAquisition.inkName] = orderOfAquisition.idx;
		}
		GameClock.instance.firstDayPauseEnabled = this.saveState.firstDayPauseEnabled;
		Main.<ApplySaveStateToGame>g__CopyPropNameList|24_0(this.saveState.passedProps, PropsController.instance.passedProps);
		Main.<ApplySaveStateToGame>g__CopyPropNameList|24_0(this.saveState.disabledProps, PropsController.instance.disabledProps);
		Main.<ApplySaveStateToGame>g__CopyPropNameList|24_0(this.saveState.enabledProps, PropsController.instance.enabledProps);
		Main.<ApplySaveStateToGame>g__CopyPropNameList|24_0(this.saveState.completedAutoRunZones, PropsController.instance.completedAutoRunZones);
		Main.<ApplySaveStateToGame>g__CopyPropNameList|24_0(this.saveState.visitedPeaks, PropsController.instance.visitedPeaks);
		Main.<ApplySaveStateToGame>g__CopyPropNameList|24_0(this.saveState.foundPaths, PropsController.instance.foundPaths);
		if (this.saveState.foundPaths.Count == 0)
		{
			foreach (string text2 in Narrative.instance.GetNonMapPathsTakenByPlayerInThePast())
			{
				PropsController.instance.foundPaths.Add(text2);
			}
		}
		PropsController.instance.RefreshPropsEnabledState();
		foreach (SaveState.CreatureState creatureState in this.saveState.activeCreatures)
		{
			GameObject gameObject = GuidManager.ResolveGuid(new Guid(creatureState.guid));
			if (gameObject == null)
			{
				Debug.LogError(string.Concat(new string[] { "Creature named ", creatureState.name, " with GUID ", creatureState.guid, " couldn't be found when loading creatures state. Skipping." }));
			}
			else
			{
				Creature component = gameObject.GetComponent<Creature>();
				if (component == null)
				{
					Debug.LogError("Creature named " + creatureState.name + " didn't appear to have a Creature component when loading state!?", gameObject);
				}
				else
				{
					component.ApplySaveState(creatureState);
				}
			}
		}
		foreach (SaveState.AnimatorState animatorState in this.saveState.animatorStates)
		{
			InkAnimation inkAnimation;
			if (InkAnimation.all.TryGetValue(animatorState.animatorName, out inkAnimation))
			{
				inkAnimation.StopAndReset(animatorState.animPlayOrder);
			}
		}
		Crow.allFlownAway = this.saveState.castleCrowsFlown;
		if (this.saveState.walkerStates != null)
		{
			using (List<SaveState.InkWalkerState>.Enumerator enumerator6 = this.saveState.walkerStates.GetEnumerator())
			{
				while (enumerator6.MoveNext())
				{
					SaveState.InkWalkerState walkerState = enumerator6.Current;
					InkWalker inkWalker = MonoInstancer<InkWalker>.all.FirstOrDefault((InkWalker w) => w.guid == walkerState.guid);
					if (inkWalker != null)
					{
						inkWalker.transform.position = walkerState.pos;
						inkWalker.FaceDir((float)walkerState.dir);
					}
				}
			}
		}
		if (MonoSingleton<SkiLift>.instance != null)
		{
			MonoSingleton<SkiLift>.instance.running = this.saveState.chairLiftRunning;
			MonoSingleton<SkiLift>.instance.ResetAfterLoad();
		}
		foreach (Goshawk goshawk in MonoInstancer<Goshawk>.all)
		{
			if (this.saveState.goshawksFlying.Contains(goshawk.guid))
			{
				goshawk.bird.TakeOff(false);
			}
			else
			{
				goshawk.flock.ResetGoshawk();
			}
		}
	}

	// Token: 0x060002F1 RID: 753 RVA: 0x000183E0 File Offset: 0x000165E0
	[CompilerGenerated]
	internal static void <UpdateSaveState>g__CopyPropNameList|17_0(HashSet<string> sourceNames, List<string> to)
	{
		to.Clear();
		foreach (string text in sourceNames)
		{
			to.Add(text);
		}
	}

	// Token: 0x060002F2 RID: 754 RVA: 0x00018434 File Offset: 0x00016634
	[CompilerGenerated]
	internal static void <ApplySaveStateToGame>g__CopyPropNameList|24_0(List<string> names, HashSet<string> to)
	{
		to.Clear();
		foreach (string text in names)
		{
			to.Add(text);
		}
	}

	// Token: 0x04000418 RID: 1048
	public static bool isQuitting;

	// Token: 0x04000419 RID: 1049
	public static DateTime timeOnStart;

	// Token: 0x0400041A RID: 1050
	[Space]
	public SaveState saveState;

	// Token: 0x0400041B RID: 1051
	public StoryState inkStateSnapshot;

	// Token: 0x0400041C RID: 1052
	[Disable]
	public float timeSinceLastSave = float.PositiveInfinity;

	// Token: 0x0400041D RID: 1053
	[Disable]
	public string lastCantSaveReason;

	// Token: 0x0400041E RID: 1054
	private List<SaveState.AnimatorState> _prevAnimStateList = new List<SaveState.AnimatorState>(128);

	// Token: 0x0400041F RID: 1055
	private static Thread _saveThread;

	// Token: 0x04000420 RID: 1056
	private static bool _saveThreadComplete;

	// Token: 0x04000421 RID: 1057
	private static float _saveMainThreadDurationMs;

	// Token: 0x04000422 RID: 1058
	private static float _saveBackgroundThreadDurationMs;
}
