using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020001B6 RID: 438
public class WorldManager : MonoBehaviour
{
	// Token: 0x1700036E RID: 878
	// (get) Token: 0x06000E6E RID: 3694 RVA: 0x000721C2 File Offset: 0x000703C2
	public static WorldManager instance
	{
		get
		{
			return GSR.WorldManager;
		}
	}

	// Token: 0x1700036F RID: 879
	// (get) Token: 0x06000E6F RID: 3695 RVA: 0x000721C9 File Offset: 0x000703C9
	// (set) Token: 0x06000E70 RID: 3696 RVA: 0x000721D1 File Offset: 0x000703D1
	public World currentWorld
	{
		get
		{
			return this._currentWorld;
		}
		private set
		{
			this._currentWorld = value;
		}
	}

	// Token: 0x17000370 RID: 880
	// (get) Token: 0x06000E71 RID: 3697 RVA: 0x000721DA File Offset: 0x000703DA
	// (set) Token: 0x06000E72 RID: 3698 RVA: 0x000721E2 File Offset: 0x000703E2
	public bool loading { get; private set; }

	// Token: 0x06000E73 RID: 3699 RVA: 0x000721EB File Offset: 0x000703EB
	private void OnDisable()
	{
		Level.ClearAll();
	}

	// Token: 0x06000E74 RID: 3700 RVA: 0x000721F4 File Offset: 0x000703F4
	public Level GetLevelAtDepth(float depth)
	{
		int num = Level.DepthToIndex(depth);
		if (num < 0 || num >= this.currentWorld.levels.Count)
		{
			return null;
		}
		return this.currentWorld.levels[num];
	}

	// Token: 0x06000E75 RID: 3701 RVA: 0x00072232 File Offset: 0x00070432
	public IEnumerator ActivateLoadedScenes()
	{
		List<AsyncOperation> loadedScenes = new List<AsyncOperation>();
		foreach (AsyncOperation asyncOperation in this._sceneLoadOperations)
		{
			if (asyncOperation.progress >= 0.9f)
			{
				loadedScenes.Add(asyncOperation);
			}
		}
		using (List<AsyncOperation>.Enumerator enumerator = loadedScenes.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				AsyncOperation asyncOperation2 = enumerator.Current;
				asyncOperation2.allowSceneActivation = true;
			}
			goto IL_00BF;
		}
		IL_00A8:
		yield return null;
		IL_00BF:
		if (!loadedScenes.Any((AsyncOperation op) => !op.isDone))
		{
			yield break;
		}
		goto IL_00A8;
	}

	// Token: 0x06000E76 RID: 3702 RVA: 0x00072241 File Offset: 0x00070441
	public IEnumerator AwaitSceneLoadCompletion()
	{
		while (this._sceneLoadOperations.Count > 0)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000E77 RID: 3703 RVA: 0x00072250 File Offset: 0x00070450
	public void LoadLevelsFrom(int firstLevelIndex, World specificWorld = null)
	{
		base.StartCoroutine(this.LoadLevelsFromCR(firstLevelIndex, null));
	}

	// Token: 0x06000E78 RID: 3704 RVA: 0x00072261 File Offset: 0x00070461
	public IEnumerator LoadLevelsFromCR(int firstLevelIndex, World specificWorld = null)
	{
		this.loading = true;
		if (Launcher.logLaunchTimes)
		{
			Debug.Log(Time.realtimeSinceStartup.ToString() + " LoadLevelsFromCR BEGIN");
		}
		if (specificWorld != null)
		{
			this.currentWorld = specificWorld;
		}
		else if (this.currentWorld == null)
		{
			this.currentWorld = this.mainWorld;
		}
		int lastLevelIndex = Mathf.Min(firstLevelIndex + 20, this.currentWorld.levels.Count) - 1;
		List<LevelSection> list = MonoInstancer<LevelSection>.all.ToList<LevelSection>();
		for (int i = firstLevelIndex; i <= lastLevelIndex; i++)
		{
			Level level = this.currentWorld.levels[i];
			list.RemoveAll((LevelSection levelSection) => levelSection.level == level);
		}
		List<string> alreadyLoadedSceneNames = MonoInstancer<LevelSection>.all.Select((LevelSection levelSection) => levelSection.gameObject.scene.name).ToList<string>();
		bool flag = true;
		foreach (string text in this.currentWorld.levels[firstLevelIndex].scenePaths)
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text);
			if (!alreadyLoadedSceneNames.Contains(fileNameWithoutExtension))
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			Level.SetCurrentLevelIdx(firstLevelIndex);
		}
		if (list.Count > 0)
		{
			NarrativePresenter.instance.ClearChoiceAndPropWidgets();
			GameCamera.instance.modifierState.ClearCameraVolumes();
		}
		foreach (LevelSection levelSection2 in list)
		{
			string name = levelSection2.gameObject.scene.name;
			if (Application.isPlaying)
			{
				yield return this.UnloadSingleLevelSectionSceneCR(name);
			}
			else
			{
				this.UnloadSingleLevelSectionSceneCR(name).MoveNext();
			}
		}
		List<LevelSection>.Enumerator enumerator2 = default(List<LevelSection>.Enumerator);
		Level.ClearUnused(this.currentWorld, firstLevelIndex, lastLevelIndex);
		for (int j = firstLevelIndex; j <= lastLevelIndex; j++)
		{
			foreach (string text2 in this.currentWorld.levels[j].scenePaths)
			{
				string fileNameWithoutExtension2 = Path.GetFileNameWithoutExtension(text2);
				if (!alreadyLoadedSceneNames.Contains(fileNameWithoutExtension2))
				{
					this.LoadSingleLevelSectionScene(fileNameWithoutExtension2);
				}
			}
		}
		for (;;)
		{
			if (!this._sceneLoadOperations.Any((AsyncOperation op) => !op.isDone))
			{
				break;
			}
			if (WorldManager.allowSceneActivations)
			{
				foreach (AsyncOperation asyncOperation in this._sceneLoadOperations)
				{
					asyncOperation.allowSceneActivation = true;
				}
			}
			yield return null;
		}
		this._sceneLoadOperations.Clear();
		SceneManager.SetActiveScene(MonoSingleton<Main>.instance.gameObject.scene);
		if (Application.isPlaying)
		{
			for (int k = firstLevelIndex; k <= lastLevelIndex; k++)
			{
				Level level2 = this.currentWorld.levels[k];
				bool flag2 = false;
				bool flag3 = false;
				using (List<LevelSection>.Enumerator enumerator4 = level2.loadedLevelSections.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						if (enumerator4.Current.crossReferencedSlopesHaveBeenConnected)
						{
							flag3 = true;
						}
						else
						{
							flag2 = true;
						}
					}
				}
				if (flag3 && flag2)
				{
					Debug.LogError("Some LevelSections have had their cross referenced slopes connected, some not!");
				}
				else if (flag2)
				{
					Build.ConnectCrossReferencedSlopes(level2.loadedLevelSections);
				}
			}
		}
		Level.SetupLevelRange(this.currentWorld, firstLevelIndex, lastLevelIndex);
		Level.SetCurrentLevelIdx(firstLevelIndex);
		if (Application.isPlaying)
		{
			Level.SetupLODs(this.currentWorld, firstLevelIndex);
			Level.ResetCutawayAndMusicRunSplats(this.currentWorld, firstLevelIndex);
			Prop.RefreshLoadedProps();
			PropsController.instance.RefreshPropsEnabledState();
			PropsController.instance.OnLevelChanged();
			MonoSingleton<MapsViewController>.instance.RefreshAllWorldMapMarkers();
			GameCamera.instance.modifierState.SetupForCurrentLevel();
			MonoSingleton<GodRayParticlesManager>.instance.FadeOutGodRaysBeforeLevelIndex(firstLevelIndex);
		}
		if (Launcher.logLaunchTimes)
		{
			Debug.Log(Time.realtimeSinceStartup.ToString() + " LoadLevelsFromCR COMPLETE");
		}
		this.loading = false;
		yield break;
		yield break;
	}

	// Token: 0x06000E79 RID: 3705 RVA: 0x00072280 File Offset: 0x00070480
	public void LoadSingleLevelSectionScene(string sceneName)
	{
		Scene sceneByName = SceneManager.GetSceneByName(sceneName);
		if ((!sceneByName.IsValid() || !sceneByName.isLoaded) && Application.isPlaying)
		{
			AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
			asyncOperation.allowSceneActivation = WorldManager.allowSceneActivations;
			this._sceneLoadOperations.Add(asyncOperation);
		}
	}

	// Token: 0x06000E7A RID: 3706 RVA: 0x000722CC File Offset: 0x000704CC
	private void CheckLoadedScene(string sceneName)
	{
		Scene sceneByName = SceneManager.GetSceneByName(sceneName);
		if (!sceneByName.IsValid() || !sceneByName.isLoaded)
		{
			Debug.LogError("Failed to load scene with name '" + sceneName + "'. Does it exist?");
			return;
		}
		if (this.FindLevelSectionInScene(sceneByName) == null)
		{
			Debug.LogError("LevelSection not found in newly loaded scene " + sceneName);
		}
	}

	// Token: 0x06000E7B RID: 3707 RVA: 0x00072327 File Offset: 0x00070527
	public IEnumerator UnloadSingleLevelSectionSceneCR(string sceneName)
	{
		Scene sceneByName = SceneManager.GetSceneByName(sceneName);
		if (sceneByName.IsValid() && sceneByName.isLoaded)
		{
			this.FindLevelSectionInScene(sceneByName);
			if (Application.isPlaying)
			{
				AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(sceneName);
				yield return asyncOperation;
			}
		}
		yield break;
	}

	// Token: 0x06000E7C RID: 3708 RVA: 0x00072340 File Offset: 0x00070540
	private LevelSection FindLevelSectionInScene(Scene scene)
	{
		GameObject[] rootGameObjects = scene.GetRootGameObjects();
		for (int i = 0; i < rootGameObjects.Length; i++)
		{
			LevelSection component = rootGameObjects[i].GetComponent<LevelSection>();
			if (component != null)
			{
				return component;
			}
		}
		return null;
	}

	// Token: 0x06000E7D RID: 3709 RVA: 0x0007237C File Offset: 0x0007057C
	private string FindScenePathFromName(string sceneName)
	{
		World[] array = WorldManager.instance.worlds;
		for (int i = 0; i < array.Length; i++)
		{
			foreach (Level level in array[i].levels)
			{
				foreach (string text in level.scenePaths)
				{
					if (sceneName == Path.GetFileNameWithoutExtension(text))
					{
						return text;
					}
				}
			}
		}
		return null;
	}

	// Token: 0x0400114F RID: 4431
	public const int numLevelsToLoad = 20;

	// Token: 0x04001150 RID: 4432
	public static bool allowSceneActivations = true;

	// Token: 0x04001151 RID: 4433
	[SerializeField]
	private World[] worlds;

	// Token: 0x04001152 RID: 4434
	public World mainWorld;

	// Token: 0x04001153 RID: 4435
	private World _currentWorld;

	// Token: 0x04001155 RID: 4437
	private List<AsyncOperation> _sceneLoadOperations = new List<AsyncOperation>(16);
}
