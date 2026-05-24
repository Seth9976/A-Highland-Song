using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000067 RID: 103
public class Launcher : MonoSingleton<Launcher>
{
	// Token: 0x170000C9 RID: 201
	// (get) Token: 0x060002D2 RID: 722 RVA: 0x0001714A File Offset: 0x0001534A
	// (set) Token: 0x060002D3 RID: 723 RVA: 0x0001714D File Offset: 0x0001534D
	public static LaunchMode launchMode
	{
		get
		{
			return LaunchMode.Splash;
		}
		set
		{
		}
	}

	// Token: 0x060002D4 RID: 724 RVA: 0x00017150 File Offset: 0x00015350
	public void LaunchOnApplicationLoad()
	{
		if (Launcher.logLaunchTimes)
		{
			Debug.Log(Time.realtimeSinceStartup.ToString() + " LaunchOnApplicationLoad");
		}
		Game.loaded = false;
		base.StartCoroutine(this.Launch_Coroutine());
	}

	// Token: 0x060002D5 RID: 725 RVA: 0x00017193 File Offset: 0x00015393
	public void ReturnToTitleScreen()
	{
		Game.loaded = false;
		base.StartCoroutine(this.LoadOrSetupNewGame_Coroutine(LaunchMode.ReturnToTitleScreen, null, SaveLoadType.None));
	}

	// Token: 0x060002D6 RID: 726 RVA: 0x000171AB File Offset: 0x000153AB
	public void DemoReturnToTitleScreenAndRestart()
	{
		Game.loaded = false;
		base.StartCoroutine(this.LoadOrSetupNewGame_Coroutine(LaunchMode.DemoReturnToTitleScreenAndRestart, null, SaveLoadType.None));
	}

	// Token: 0x060002D7 RID: 727 RVA: 0x000171C3 File Offset: 0x000153C3
	public void RestartAndBegin()
	{
		Game.loaded = false;
		base.StartCoroutine(this.LoadOrSetupNewGame_Coroutine(LaunchMode.RestartGame, null, SaveLoadType.None));
	}

	// Token: 0x060002D8 RID: 728 RVA: 0x000171DB File Offset: 0x000153DB
	public void RestartFromEndOfGameForNewGamePlus()
	{
		Game.loaded = false;
		base.StartCoroutine(this.LoadOrSetupNewGame_Coroutine(LaunchMode.NewGamePlus, null, SaveLoadType.None));
	}

	// Token: 0x060002D9 RID: 729 RVA: 0x000171F4 File Offset: 0x000153F4
	public void ResetCompletely()
	{
		Game.loaded = false;
		base.StartCoroutine(this.LoadOrSetupNewGame_Coroutine(LaunchMode.ResetCompletely, null, SaveLoadType.None));
	}

	// Token: 0x060002DA RID: 730 RVA: 0x0001720D File Offset: 0x0001540D
	public void LoadSaveType(SaveLoadType type)
	{
		Game.loaded = false;
		base.StartCoroutine(this.LoadOrSetupNewGame_Coroutine(LaunchMode.LoadSaveOfType, null, type));
	}

	// Token: 0x060002DB RID: 731 RVA: 0x00017226 File Offset: 0x00015426
	public void LoadGameWithSaveState(string saveStateJSON)
	{
		Game.loaded = false;
		if (string.IsNullOrWhiteSpace(saveStateJSON))
		{
			saveStateJSON = null;
		}
		base.StartCoroutine(this.LoadOrSetupNewGame_Coroutine(LaunchMode.LoadSpecificSave, saveStateJSON, SaveLoadType.None));
	}

	// Token: 0x060002DC RID: 732 RVA: 0x00017249 File Offset: 0x00015449
	private IEnumerator Launch_Coroutine()
	{
		if (Launcher.logLaunchTimes)
		{
			Debug.Log(Time.realtimeSinceStartup.ToString() + " SetUp_Coroutine");
		}
		Blackout.Show();
		Time.maximumDeltaTime = 0.06666667f;
		Cursor.visible = false;
		if (Launcher.launchMode == LaunchMode.Splash)
		{
			MonoSingleton<SplashSequence>.instance.Begin();
			int num;
			for (int i = 0; i < 10; i = num + 1)
			{
				yield return null;
				num = i;
			}
		}
		SettingsScreen.SetupDefaultSettingsIfNecessary();
		if (!SceneManager.GetSceneByName("Game").isLoaded)
		{
			yield return SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);
		}
		if (Game.instance != null)
		{
			GSR.timeOfDayEffects.enabled = false;
			GSR.WeatherSystem.gameObject.SetActive(false);
		}
		if (GameClock.instance != null)
		{
			GameClock.instance.enabled = false;
		}
		if (Runner.instance != null)
		{
			Runner.instance.enabled = false;
		}
		if (MonoSingleton<TrackBuilder>.instance != null)
		{
			MonoSingleton<TrackBuilder>.instance.enabled = false;
		}
		if (MonoSingleton<RunTrack>.instance != null)
		{
			MonoSingleton<RunTrack>.instance.enabled = false;
		}
		if (GameCamera.instance != null)
		{
			GameCamera.instance.enabled = false;
		}
		if (Narrative.instance != null)
		{
			Narrative.instance.enabled = false;
		}
		Narrative.instance.LoadInk();
		MonoSingleton<PeakClimbedBanner>.instance.LoadAllIcons();
		Narrative.instance.preventBackgroundRemarks = Narrative.PreventBackgroundRemarksReason.Loading;
		if (!SceneManager.GetSceneByName("GlobalWorld").isLoaded)
		{
			yield return SceneManager.LoadSceneAsync("GlobalWorld", LoadSceneMode.Additive);
		}
		if (!SceneManager.GetSceneByName("Runner").isLoaded)
		{
			yield return SceneManager.LoadSceneAsync("Runner", LoadSceneMode.Additive);
			Runner.instance.enabled = false;
		}
		if (!SceneManager.GetSceneByName("TrackBuilderAndLibrary").isLoaded)
		{
			yield return SceneManager.LoadSceneAsync("TrackBuilderAndLibrary", LoadSceneMode.Additive);
			MonoSingleton<TrackBuilder>.instance.enabled = false;
		}
		SceneManager.SetActiveScene(MonoSingleton<Main>.instance.gameObject.scene);
		Level.ResetLevelFadeAlpha();
		Map.LoadAll();
		Launcher.launching = false;
		yield return this.LoadOrSetupNewGame_Coroutine(Launcher.launchMode, null, SaveLoadType.None);
		yield break;
	}

	// Token: 0x060002DD RID: 733 RVA: 0x00017258 File Offset: 0x00015458
	private IEnumerator LoadOrSetupNewGame_Coroutine(LaunchMode mode, string editorExplicitSaveJSON = null, SaveLoadType specificSaveLoadType = SaveLoadType.None)
	{
		if (Launcher.logLaunchTimes)
		{
			Debug.Log(Time.realtimeSinceStartup.ToString() + " Load " + mode.ToString());
		}
		History.enabled = false;
		if (mode == LaunchMode.NewGamePlus)
		{
			MonoSingleton<SplashSequence>.instance.BeginLoadingAnimOnly();
		}
		if (mode != LaunchMode.RestartGame && mode != LaunchMode.NewGamePlus)
		{
			Game.instance.Clear(Game.ClearType.FullState);
		}
		if (mode == LaunchMode.ResetCompletely || mode == LaunchMode.DemoReturnToTitleScreenAndRestart)
		{
			MonoSingleton<Main>.instance.ResetSaveStateCompletely();
		}
		if (mode == LaunchMode.NewGamePlus)
		{
			Game.instance.playthroughIdx++;
		}
		if ((mode == LaunchMode.RestartGame || mode == LaunchMode.ResetCompletely) && !WorldManager.instance.currentWorld.levels[0].isSetup)
		{
			Blackout.FadeOut(null);
			while (!Blackout.isFullyVisible)
			{
				yield return null;
			}
			MonoSingleton<SplashSequence>.instance.BeginLoadingAnimOnly();
		}
		bool loadedSave = false;
		bool demoNeverLoadOnTitleScreen = MonoSingleton<BuildSetupManager>.instance.setup.demoNeverLoadOnTitleScreen;
		if (mode == LaunchMode.LoadCurrentSave || mode == LaunchMode.LoadSpecificSave || (mode == LaunchMode.TitleScreen && !demoNeverLoadOnTitleScreen) || (mode == LaunchMode.Splash && !demoNeverLoadOnTitleScreen) || (mode == LaunchMode.RestartGame && !demoNeverLoadOnTitleScreen) || (mode == LaunchMode.ReturnToTitleScreen && !demoNeverLoadOnTitleScreen) || mode == LaunchMode.LoadSaveOfType)
		{
			bool flag = mode == LaunchMode.LoadCurrentSave || mode == LaunchMode.LoadSpecificSave;
			SaveLoadType saveLoadType = SaveLoadType.Latest;
			if (mode == LaunchMode.RestartGame)
			{
				saveLoadType = SaveLoadType.GameStart;
			}
			else if (flag && editorExplicitSaveJSON != null)
			{
				saveLoadType = SaveLoadType.ExplicitEditorSaveData;
			}
			else if (mode == LaunchMode.LoadSaveOfType)
			{
				saveLoadType = specificSaveLoadType;
			}
			if (!SaveLoadManager.Load(saveLoadType, editorExplicitSaveJSON))
			{
				if (flag)
				{
					mode = LaunchMode.NewGame;
				}
			}
			else
			{
				yield return Game.instance.TeleportPlayerTo3DCR(MonoSingleton<Main>.instance.saveState.runnerPosition, "First load", 0, false, null, false, true, null, null);
				MonoSingleton<Main>.instance.ApplySaveStateToGame();
				Narrative.instance.RefreshInteractablesChoices(true, false);
				loadedSave = true;
			}
		}
		if (mode == LaunchMode.EditorDebugFlow)
		{
			MonoSingleton<Main>.instance.CreateNewSaveState();
			yield return Game.instance.TeleportPlayerTo3DCR(Runner.instance.transform.position, "Editor debug flow", 0, false, null, false, false, null, null);
		}
		if (mode == LaunchMode.NewGame || mode == LaunchMode.RestartGame || mode == LaunchMode.ResetCompletely || mode == LaunchMode.DemoReturnToTitleScreenAndRestart || mode == LaunchMode.NewGamePlus || (mode == LaunchMode.TitleScreen && !loadedSave) || (mode == LaunchMode.Splash && !loadedSave))
		{
			MonoSingleton<Main>.instance.CreateNewSaveState();
			if (mode == LaunchMode.NewGamePlus || mode == LaunchMode.RestartGame)
			{
				Game.instance.Clear(Game.ClearType.FullStatePreservingLoopVariables);
			}
			if (Game.instance.inTitleScreenAndIntroState)
			{
				GameCamera.IntroCameraState introCameraState = GameCamera.instance.introCameraState;
				introCameraState.active = true;
				introCameraState.strength = 1f;
			}
			yield return MonoSingleton<BuildSetupManager>.instance.setup.newGameSetup.RunCR(null, false);
		}
		if (MonoSingleton<BuildSetupManager>.instance.setup.fastIntro)
		{
			GameClock.instance.daysNorm = 0.75f;
		}
		MonoSingleton<SplashSequence>.instance.CompleteLoadingAnimOnly();
		Game.instance.OnCompleteLoad();
		if (mode == LaunchMode.LoadCurrentSave || mode == LaunchMode.LoadSpecificSave || mode == LaunchMode.EditorDebugFlow)
		{
			Blackout.Hide();
		}
		else if (mode != LaunchMode.Splash)
		{
			Blackout.FadeIn(0f, null);
		}
		if (mode == LaunchMode.TitleScreen || mode == LaunchMode.Splash || mode == LaunchMode.ReturnToTitleScreen || mode == LaunchMode.DemoReturnToTitleScreenAndRestart || mode == LaunchMode.NewGamePlus)
		{
			Game.instance.ShowTitleScreen(false, mode == LaunchMode.NewGamePlus);
		}
		else if (mode == LaunchMode.ResetCompletely)
		{
			if (!MonoSingleton<TitleScreen>.instance.visible)
			{
				Game.instance.ShowTitleScreen(false, false);
			}
			else
			{
				MonoSingleton<TitleScreen>.instance.RefreshOptions(false);
			}
		}
		else if (mode == LaunchMode.RestartGame)
		{
			Game.instance.IntroSwoopAfterTitleScreen();
		}
		else
		{
			if (mode == LaunchMode.EditorDebugFlow)
			{
				Narrative.instance.RunFunctionAndParseOutput("chooseNewWeatherPattern", new object[] { Level.currentIndex + 1 });
			}
			Game.instance.BeginGameplay();
		}
		if (mode == LaunchMode.NewGamePlus)
		{
			MonoSingleton<Main>.instance.TrySave(true);
		}
		yield break;
	}

	// Token: 0x04000415 RID: 1045
	public static bool logLaunchTimes = false;

	// Token: 0x04000416 RID: 1046
	public Camera stupidCameraThatFixesCanvasResolution;

	// Token: 0x04000417 RID: 1047
	public static bool launching = true;
}
