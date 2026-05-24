using System;
using UnityEngine;

// Token: 0x020001F2 RID: 498
public class SerializedScriptableSingleton<T> : ScriptableObject where T : ScriptableObject
{
	// Token: 0x17000442 RID: 1090
	// (get) Token: 0x0600128B RID: 4747 RVA: 0x000850F7 File Offset: 0x000832F7
	private static string settingsPrefsKey
	{
		get
		{
			if (SerializedScriptableSingleton<T>._settingsPrefsKey == null)
			{
				SerializedScriptableSingleton<T>._settingsPrefsKey = string.Format("{0} Settings ({1})", typeof(T).Name, Application.productName);
			}
			return SerializedScriptableSingleton<T>._settingsPrefsKey;
		}
	}

	// Token: 0x14000019 RID: 25
	// (add) Token: 0x0600128C RID: 4748 RVA: 0x00085128 File Offset: 0x00083328
	// (remove) Token: 0x0600128D RID: 4749 RVA: 0x0008515C File Offset: 0x0008335C
	public static event Action OnCreateOrLoad;

	// Token: 0x17000443 RID: 1091
	// (get) Token: 0x0600128E RID: 4750 RVA: 0x0008518F File Offset: 0x0008338F
	public static T Instance
	{
		get
		{
			if (SerializedScriptableSingleton<T>._Instance == null)
			{
				SerializedScriptableSingleton<T>.LoadOrCreateAndSave();
			}
			return SerializedScriptableSingleton<T>._Instance;
		}
	}

	// Token: 0x0600128F RID: 4751 RVA: 0x000851AE File Offset: 0x000833AE
	private static T LoadOrCreateAndSave()
	{
		SerializedScriptableSingleton<T>.Load();
		if (SerializedScriptableSingleton<T>._Instance == null)
		{
			SerializedScriptableSingleton<T>.CreateAndSave();
		}
		return SerializedScriptableSingleton<T>._Instance;
	}

	// Token: 0x06001290 RID: 4752 RVA: 0x000851D1 File Offset: 0x000833D1
	public static void CreateAndSave()
	{
		SerializedScriptableSingleton<T>._Instance = ScriptableObject.CreateInstance<T>();
		SerializedScriptableSingleton<T>.Save(SerializedScriptableSingleton<T>._Instance);
		if (SerializedScriptableSingleton<T>.OnCreateOrLoad != null)
		{
			SerializedScriptableSingleton<T>.OnCreateOrLoad();
		}
	}

	// Token: 0x06001291 RID: 4753 RVA: 0x000851F8 File Offset: 0x000833F8
	public static void Save()
	{
		SerializedScriptableSingleton<T>.Save(SerializedScriptableSingleton<T>._Instance);
	}

	// Token: 0x06001292 RID: 4754 RVA: 0x00085204 File Offset: 0x00083404
	public static void Save(T settings)
	{
		string text = JsonUtility.ToJson(settings);
		if (!Application.isEditor)
		{
			PlayerPrefsX.SetString(SerializedScriptableSingleton<T>.settingsPrefsKey, text);
		}
	}

	// Token: 0x06001293 RID: 4755 RVA: 0x00085230 File Offset: 0x00083430
	private static void Load()
	{
		string text = null;
		if (!Application.isEditor)
		{
			if (!PlayerPrefsX.HasKey(SerializedScriptableSingleton<T>.settingsPrefsKey))
			{
				return;
			}
			text = PlayerPrefsX.GetString(SerializedScriptableSingleton<T>.settingsPrefsKey, null);
		}
		SerializedScriptableSingleton<T>._Instance = ScriptableObject.CreateInstance<T>();
		try
		{
			JsonUtility.FromJsonOverwrite(text, SerializedScriptableSingleton<T>._Instance);
			if (SerializedScriptableSingleton<T>._Instance != null && SerializedScriptableSingleton<T>.OnCreateOrLoad != null)
			{
				SerializedScriptableSingleton<T>.OnCreateOrLoad();
			}
		}
		catch
		{
			Debug.LogError("Save Data was corrupt and could not be parsed. New data created. Old data was:\n" + text);
			SerializedScriptableSingleton<T>.CreateAndSave();
		}
	}

	// Token: 0x06001294 RID: 4756 RVA: 0x000852C8 File Offset: 0x000834C8
	public static void Delete()
	{
		if (!Application.isEditor)
		{
			PlayerPrefsX.DeleteKey(SerializedScriptableSingleton<T>.settingsPrefsKey);
		}
		if (Application.isPlaying)
		{
			Object.Destroy(SerializedScriptableSingleton<T>._Instance);
			return;
		}
		Object.DestroyImmediate(SerializedScriptableSingleton<T>._Instance);
	}

	// Token: 0x04001299 RID: 4761
	private static string _settingsPrefsKey;

	// Token: 0x0400129B RID: 4763
	private static T _Instance;
}
