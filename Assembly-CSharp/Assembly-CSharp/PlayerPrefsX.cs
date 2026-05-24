using System;
using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;

// Token: 0x02000217 RID: 535
public static class PlayerPrefsX
{
	// Token: 0x06001386 RID: 4998 RVA: 0x000898F0 File Offset: 0x00087AF0
	public static int GetInt(string prefName, int defaultValue = 0)
	{
		return PlayerPrefs.GetInt(prefName, defaultValue);
	}

	// Token: 0x06001387 RID: 4999 RVA: 0x000898F9 File Offset: 0x00087AF9
	public static void SetInt(string prefName, int newValue)
	{
		PlayerPrefs.SetInt(prefName, newValue);
	}

	// Token: 0x06001388 RID: 5000 RVA: 0x00089902 File Offset: 0x00087B02
	public static string GetString(string prefName, string defaultValue = null)
	{
		return PlayerPrefs.GetString(prefName, defaultValue);
	}

	// Token: 0x06001389 RID: 5001 RVA: 0x0008990B File Offset: 0x00087B0B
	public static void SetString(string prefName, string newValue)
	{
		PlayerPrefs.SetString(prefName, newValue);
	}

	// Token: 0x0600138A RID: 5002 RVA: 0x00089914 File Offset: 0x00087B14
	public static float GetFloat(string prefName, float defaultValue = 0f)
	{
		return PlayerPrefs.GetFloat(prefName, defaultValue);
	}

	// Token: 0x0600138B RID: 5003 RVA: 0x0008991D File Offset: 0x00087B1D
	public static void SetFloat(string prefName, float newValue)
	{
		PlayerPrefs.SetFloat(prefName, newValue);
	}

	// Token: 0x0600138C RID: 5004 RVA: 0x00089926 File Offset: 0x00087B26
	public static bool HasKey(string prefName)
	{
		return PlayerPrefs.HasKey(prefName);
	}

	// Token: 0x0600138D RID: 5005 RVA: 0x0008992E File Offset: 0x00087B2E
	public static void DeleteKey(string prefName)
	{
		PlayerPrefs.DeleteKey(prefName);
	}

	// Token: 0x0600138E RID: 5006 RVA: 0x00089936 File Offset: 0x00087B36
	public static void LoadIfNecessary()
	{
	}

	// Token: 0x0600138F RID: 5007 RVA: 0x00089938 File Offset: 0x00087B38
	private static string GetAllPrefsJson()
	{
		PlayerPrefsX.LoadIfNecessary();
		string text = null;
		object obj = PlayerPrefsX.threadLock;
		lock (obj)
		{
			if (PlayerPrefsX._jsonWriter == null)
			{
				PlayerPrefsX._jsonWriter = new SimpleJson.Writer();
			}
			else
			{
				PlayerPrefsX._jsonWriter.Clear();
			}
			PlayerPrefsX._jsonWriter.WriteObjectStart();
			using (Dictionary<string, object>.Enumerator enumerator = PlayerPrefsX._prefs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, object> kv = enumerator.Current;
					PlayerPrefsX._jsonWriter.WriteProperty(kv.Key, delegate(SimpleJson.Writer val)
					{
						if (kv.Value == null)
						{
							return;
						}
						if (kv.Value is int)
						{
							PlayerPrefsX._jsonWriter.Write((int)kv.Value);
							return;
						}
						if (kv.Value is float)
						{
							PlayerPrefsX._jsonWriter.Write((float)kv.Value);
							return;
						}
						if (kv.Value is string)
						{
							PlayerPrefsX._jsonWriter.Write((string)kv.Value, true);
							return;
						}
						Debug.LogError(string.Format("Unexpected PlayerPrefs type: {0} for ({1}: {2})", val.GetType(), kv.Key, kv.Value));
					});
				}
			}
			PlayerPrefsX._jsonWriter.WriteObjectEnd();
			text = PlayerPrefsX._jsonWriter.ToString();
			PlayerPrefsX._jsonWriter.Clear();
		}
		return text;
	}

	// Token: 0x06001390 RID: 5008 RVA: 0x00089A2C File Offset: 0x00087C2C
	public static void Save()
	{
		PlayerPrefs.Save();
	}

	// Token: 0x040012C9 RID: 4809
	private static Dictionary<string, object> _prefs = new Dictionary<string, object>();

	// Token: 0x040012CA RID: 4810
	private static SimpleJson.Writer _jsonWriter;

	// Token: 0x040012CB RID: 4811
	private static object threadLock = new object();
}
