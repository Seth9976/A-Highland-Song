using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x020000E5 RID: 229
public static class SaveLoadManager
{
	// Token: 0x170001FD RID: 509
	// (get) Token: 0x06000797 RID: 1943 RVA: 0x00044B74 File Offset: 0x00042D74
	public static string saveDirectory
	{
		get
		{
			if (SaveLoadManager._saveDirectory == null)
			{
				SaveLoadManager._saveDirectory = Application.persistentDataPath;
			}
			return SaveLoadManager._saveDirectory;
		}
	}

	// Token: 0x170001FE RID: 510
	// (get) Token: 0x06000798 RID: 1944 RVA: 0x00044B8C File Offset: 0x00042D8C
	public static string projectPath
	{
		get
		{
			if (SaveLoadManager._projectPath == null)
			{
				SaveLoadManager._projectPath = Path.Combine(Application.dataPath, "../../");
			}
			return SaveLoadManager._projectPath;
		}
	}

	// Token: 0x170001FF RID: 511
	// (get) Token: 0x06000799 RID: 1945 RVA: 0x00044BB0 File Offset: 0x00042DB0
	public static string autoDevSaveDirectory
	{
		get
		{
			if (SaveLoadManager._autoDevSaveDirectory == null)
			{
				if (Application.isEditor)
				{
					SaveLoadManager._autoDevSaveDirectory = Path.GetFullPath(Path.Combine(SaveLoadManager.projectPath, "Auto Dev Saves"));
				}
				else
				{
					SaveLoadManager._autoDevSaveDirectory = Path.GetFullPath(Path.Combine(SaveLoadManager.saveDirectory, "Auto Dev Saves"));
				}
			}
			return SaveLoadManager._autoDevSaveDirectory;
		}
	}

	// Token: 0x0600079A RID: 1946 RVA: 0x00044C04 File Offset: 0x00042E04
	public static string FullSavePath(SaveLoadType saveType = SaveLoadType.Latest)
	{
		return Path.Combine(SaveLoadManager.saveDirectory, "HighlandSave_" + saveType.ToString() + ".json");
	}

	// Token: 0x0600079B RID: 1947 RVA: 0x00044C2C File Offset: 0x00042E2C
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	private static void SetupSavePaths()
	{
		string saveDirectory = SaveLoadManager.saveDirectory;
		string projectPath = SaveLoadManager.projectPath;
		string autoDevSaveDirectory = SaveLoadManager.autoDevSaveDirectory;
	}

	// Token: 0x0600079C RID: 1948 RVA: 0x00044C40 File Offset: 0x00042E40
	public static SaveInfo? TryGetSaveInfo(SaveLoadType saveType)
	{
		string text = SaveLoadManager.FullSavePath(saveType);
		if (!File.Exists(text))
		{
			return null;
		}
		return new SaveInfo?(new SaveInfo
		{
			type = saveType,
			dateTime = File.GetLastWriteTime(text)
		});
	}

	// Token: 0x17000200 RID: 512
	// (get) Token: 0x0600079D RID: 1949 RVA: 0x00044C8C File Offset: 0x00042E8C
	public static List<SaveInfo> backupSavesInfo
	{
		get
		{
			SaveLoadManager._backupSavesInfo.Clear();
			SaveLoadType[] backupTypes = SaveLoadManager._backupTypes;
			for (int i = 0; i < backupTypes.Length; i++)
			{
				SaveInfo? saveInfo = SaveLoadManager.TryGetSaveInfo(backupTypes[i]);
				if (saveInfo != null)
				{
					SaveLoadManager._backupSavesInfo.Add(saveInfo.Value);
				}
			}
			SaveLoadManager._backupSavesInfo.Sort((SaveInfo i1, SaveInfo i2) => -i1.dateTime.CompareTo(i2.dateTime));
			return SaveLoadManager._backupSavesInfo;
		}
	}

	// Token: 0x0600079E RID: 1950 RVA: 0x00044D08 File Offset: 0x00042F08
	public static string CantSaveReason(bool force = false)
	{
		if (Main.saveThreadIsActive)
		{
			return "Main.saveThreadIsActive";
		}
		if (!Application.isPlaying)
		{
			return "!Application.isPlaying";
		}
		if (SaveLoadManager._loading)
		{
			return "_loading";
		}
		if (!Game.loaded)
		{
			return "!Game.loaded";
		}
		if (Game.instance == null)
		{
			return "Game.instance == null";
		}
		if (!Game.instance.inActiveGameplay && (!force || !Game.instance.inTitleScreenAndIntroState))
		{
			return "!Game.instance.inActiveGameplay";
		}
		if (MonoSingleton<Main>.instance.timeSinceLastSave < 2f && !force)
		{
			return "Main.instance.timeSinceLastSave < 2.0f";
		}
		if (Narrative.instance == null)
		{
			return "Narrative.instance == null";
		}
		if (Narrative.instance.inkStory == null)
		{
			return "Narrative.instance.inkStory == null";
		}
		if (Narrative.instance.isBusy)
		{
			return "Narrative.instance.isBusy";
		}
		if (!force)
		{
			if (!Runner.instance.running)
			{
				return "!Runner.instance.running";
			}
			if (Runner.instance.inMusicRunningArea)
			{
				return "Runner.instance.inMusicRunningArea";
			}
			if (Runner.instance.momentumAbs > 0.1f)
			{
				return "Runner.instance.momentumAbs > 0.1f";
			}
			if (AudioController.instance.playingFinalJumpMusic)
			{
				return "AudioController.instance.playingFinalJumpMusic";
			}
			if (Level.currentIndex == 8)
			{
				return "Level.currentIndex == 8";
			}
			if (InkAnimation.anyNonLoopingActive)
			{
				return "InkAnimation.anyNonLoopingActive";
			}
			if (!InkWalker.canSave)
			{
				return "!InkWalker.canSave";
			}
			if (!MonoSingleton<Eagle>.instance.complete)
			{
				return "!Eagle.instance.complete";
			}
		}
		return null;
	}

	// Token: 0x17000201 RID: 513
	// (get) Token: 0x0600079F RID: 1951 RVA: 0x00044E62 File Offset: 0x00043062
	public static bool canSave
	{
		get
		{
			return SaveLoadManager.CantSaveReason(false) == null;
		}
	}

	// Token: 0x060007A0 RID: 1952 RVA: 0x00044E70 File Offset: 0x00043070
	public static void Save(SaveLoadType saveLoadType, string saveStateJSON)
	{
		string text = SaveLoadManager.FullSavePath(saveLoadType);
		string text2 = Path.Combine(SaveLoadManager.saveDirectory, "HighlandSave_Temp") + ".json";
		if (File.Exists(text2))
		{
			File.Delete(text2);
		}
		File.WriteAllText(text2, saveStateJSON);
		if (File.Exists(text))
		{
			File.Delete(text);
		}
		File.Move(text2, text);
	}

	// Token: 0x060007A1 RID: 1953 RVA: 0x00044EC8 File Offset: 0x000430C8
	public static bool TryCopySave(SaveLoadType from, SaveLoadType to, bool errorIfDoesntExist = true)
	{
		string text;
		if (to == SaveLoadType.DevAutoSave)
		{
			text = SaveLoadManager.NextDevAutoSavePath();
		}
		else
		{
			text = SaveLoadManager.FullSavePath(to);
		}
		return SaveLoadManager.TryCopySave(from, text, errorIfDoesntExist);
	}

	// Token: 0x060007A2 RID: 1954 RVA: 0x00044EF4 File Offset: 0x000430F4
	public static bool TryCopySave(SaveLoadType from, string toPath, bool errorIfDoesntExist = true)
	{
		string text = SaveLoadManager.FullSavePath(from);
		if (!File.Exists(text))
		{
			if (errorIfDoesntExist)
			{
				Debug.LogError(string.Format("Cannot copy save from {0} since it doesn't exist at: ", from) + text);
			}
			return false;
		}
		if (File.Exists(toPath))
		{
			try
			{
				File.Delete(toPath);
			}
			catch (Exception ex)
			{
				Debug.LogError("Failed to delete save file at path " + toPath + " -- " + ex.Message);
				return false;
			}
		}
		File.Copy(text, toPath);
		return true;
	}

	// Token: 0x060007A3 RID: 1955 RVA: 0x00044F7C File Offset: 0x0004317C
	private static string NextDevAutoSavePath()
	{
		DateTime now = DateTime.Now;
		string text = "AutoSave";
		string text2 = string.Concat(new string[]
		{
			now.Year.ToString(),
			"_",
			now.Month.ToString(),
			"_",
			now.Day.ToString(),
			"_",
			now.Hour.ToString("00"),
			"_",
			now.Minute.ToString("00"),
			"_",
			now.Second.ToString("00")
		});
		string text3 = string.Format("{0}_{1}.txt", text, text2);
		if (!Directory.Exists(SaveLoadManager.autoDevSaveDirectory))
		{
			Directory.CreateDirectory(SaveLoadManager.autoDevSaveDirectory);
		}
		return Path.Combine(SaveLoadManager.autoDevSaveDirectory, Path.GetFileNameWithoutExtension(text3) + ".json");
	}

	// Token: 0x060007A4 RID: 1956 RVA: 0x0004508C File Offset: 0x0004328C
	public static bool DeleteSave(SaveLoadType saveType)
	{
		string text = SaveLoadManager.FullSavePath(saveType);
		if (File.Exists(text))
		{
			try
			{
				File.Delete(text);
				return true;
			}
			catch (Exception ex)
			{
				Debug.LogError("Failed to delete save file at path " + text + " -- " + ex.Message);
			}
			return false;
		}
		return false;
	}

	// Token: 0x060007A5 RID: 1957 RVA: 0x000450E4 File Offset: 0x000432E4
	public static void DeleteAll()
	{
		SaveLoadManager.DeleteSave(SaveLoadType.GameStart);
		SaveLoadManager.DeleteSave(SaveLoadType.LevelStart);
		SaveLoadManager.DeleteSave(SaveLoadType.Backup1);
		SaveLoadManager.DeleteSave(SaveLoadType.Backup2);
		SaveLoadManager.DeleteSave(SaveLoadType.Backup3);
		SaveLoadManager.DeleteSave(SaveLoadType.Backup4);
		SaveLoadManager.DeleteSave(SaveLoadType.Latest);
	}

	// Token: 0x060007A6 RID: 1958 RVA: 0x00045118 File Offset: 0x00043318
	public static bool Load(SaveLoadType saveType, string editorExplicitSaveJSON)
	{
		string text = null;
		if (saveType == SaveLoadType.ExplicitEditorSaveData)
		{
			if (editorExplicitSaveJSON != null)
			{
				text = editorExplicitSaveJSON;
			}
			else
			{
				saveType = SaveLoadType.Latest;
			}
		}
		if (text == null)
		{
			string text2 = SaveLoadManager.FullSavePath(saveType);
			if (File.Exists(text2))
			{
				text = File.ReadAllText(text2);
			}
			if (text == null)
			{
				Debug.Log("No save file exists");
				return false;
			}
		}
		if (!SaveLoadManager.LoadSaveState(text, ref MonoSingleton<Main>.instance.saveState))
		{
			Debug.LogError("SaveLoadManager: Save state JSON was invalid!");
			return false;
		}
		if (!Narrative.instance.TryLoadState(MonoSingleton<Main>.instance.saveState.storyJSON))
		{
			Debug.LogError("SaveLoadManager: Ink state was invalid!");
			return false;
		}
		return true;
	}

	// Token: 0x060007A7 RID: 1959 RVA: 0x000451A8 File Offset: 0x000433A8
	public static bool LoadSaveState(string saveStateJSON, ref SaveState saveState)
	{
		if (!Application.isPlaying)
		{
			Debug.LogError("Can't LoadSaveState because we're not in play mode");
			return false;
		}
		if (SaveLoadManager._loading)
		{
			Debug.LogError("Can't LoadSaveState because we're already loading");
			return false;
		}
		SaveLoadManager._loading = true;
		bool flag = false;
		try
		{
			JsonUtility.FromJsonOverwrite(saveStateJSON, saveState);
			if (saveState.saveMetaInfo.saveVersion.version < 1)
			{
				Debug.LogError("Can't load save because save version " + saveState.saveMetaInfo.saveVersion.version.ToString() + " is less than minimum compatible version " + 1.ToString());
			}
			else
			{
				flag = true;
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("Couldn't load save state: " + ex.ToString());
		}
		SaveLoadManager._loading = false;
		return flag;
	}

	// Token: 0x0400096E RID: 2414
	public const string fileName = "HighlandSave";

	// Token: 0x0400096F RID: 2415
	public const string fileExtension = "json";

	// Token: 0x04000970 RID: 2416
	private static string _saveDirectory;

	// Token: 0x04000971 RID: 2417
	private static string _projectPath;

	// Token: 0x04000972 RID: 2418
	private static string _autoDevSaveDirectory;

	// Token: 0x04000973 RID: 2419
	private static List<SaveInfo> _backupSavesInfo = new List<SaveInfo>(4);

	// Token: 0x04000974 RID: 2420
	private static SaveLoadType[] _backupTypes = new SaveLoadType[]
	{
		SaveLoadType.LevelStart,
		SaveLoadType.Backup1,
		SaveLoadType.Backup2,
		SaveLoadType.Backup3,
		SaveLoadType.Backup4
	};

	// Token: 0x04000975 RID: 2421
	private static bool _loading;
}
