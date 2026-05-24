using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

// Token: 0x02000204 RID: 516
public static class DebugX
{
	// Token: 0x060012F1 RID: 4849 RVA: 0x00086FF0 File Offset: 0x000851F0
	public static string ListAsString<T>(IEnumerable<T> list, Func<T, string> toString = null, bool showTypeAndCount = true, bool lineSeparated = true)
	{
		if (list == null)
		{
			return "NULL";
		}
		int num = 0;
		foreach (T t in list)
		{
			num++;
		}
		StringBuilder stringBuilder = new StringBuilder();
		if (showTypeAndCount)
		{
			stringBuilder.AppendLine(string.Concat(new string[]
			{
				"Displaying list of ",
				typeof(T).Name,
				" with ",
				num.ToString(),
				" values:"
			}));
		}
		bool flag = true;
		foreach (T t2 in list)
		{
			string text = ((t2 == null) ? "NULL" : ((toString == null) ? t2.ToString() : toString(t2)));
			if (lineSeparated)
			{
				stringBuilder.AppendLine(text);
			}
			else
			{
				if (!flag)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append(text);
			}
			if (flag)
			{
				flag = false;
			}
		}
		return stringBuilder.ToString();
	}

	// Token: 0x060012F2 RID: 4850 RVA: 0x00087120 File Offset: 0x00085320
	public static string DictionaryAsString<TKey, TValue>(IDictionary<TKey, TValue> dictionary, Func<KeyValuePair<TKey, TValue>, string> toString = null, bool showTypeAndCount = true)
	{
		if (dictionary == null)
		{
			return "NULL";
		}
		int num = 0;
		foreach (KeyValuePair<TKey, TValue> keyValuePair in dictionary)
		{
			num++;
		}
		StringBuilder stringBuilder = new StringBuilder();
		if (showTypeAndCount)
		{
			stringBuilder.AppendLine(string.Concat(new string[]
			{
				"Displaying dictionary with key ",
				typeof(TKey).Name,
				" and value ",
				typeof(TValue).Name,
				" with ",
				num.ToString(),
				" values:"
			}));
		}
		bool flag = true;
		foreach (KeyValuePair<TKey, TValue> keyValuePair2 in dictionary)
		{
			if (toString != null)
			{
				stringBuilder.AppendLine(toString(keyValuePair2));
			}
			else
			{
				stringBuilder.Append("KEY: ");
				StringBuilder stringBuilder2 = stringBuilder;
				TKey key = keyValuePair2.Key;
				stringBuilder2.Append(key.ToString());
				stringBuilder.Append(", VALUE: ");
				StringBuilder stringBuilder3 = stringBuilder;
				TValue value = keyValuePair2.Value;
				stringBuilder3.AppendLine(value.ToString());
			}
			if (flag)
			{
				flag = false;
			}
		}
		return stringBuilder.ToString();
	}

	// Token: 0x060012F3 RID: 4851 RVA: 0x00087280 File Offset: 0x00085480
	public static void LogList<T>(IEnumerable<T> list, Func<T, string> toString = null, bool showTypeAndCount = true, bool lineSeparated = true)
	{
		if (list == null || !list.Any<T>())
		{
			Debug.Log("List is null or empty");
			return;
		}
		Debug.Log(DebugX.ListAsString<T>(list, toString, showTypeAndCount, lineSeparated));
	}

	// Token: 0x060012F4 RID: 4852 RVA: 0x000872A8 File Offset: 0x000854A8
	public static void LogList<T>(string log, IEnumerable<T> list, Func<T, string> toString = null, bool showTypeAndCount = true, bool lineSeparated = true)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine(log);
		if (list == null || !list.Any<T>())
		{
			stringBuilder.AppendLine("List is null or empty");
		}
		else
		{
			stringBuilder.AppendLine(DebugX.ListAsString<T>(list, toString, showTypeAndCount, lineSeparated));
		}
		Debug.Log(stringBuilder.ToString());
	}

	// Token: 0x060012F5 RID: 4853 RVA: 0x000872F8 File Offset: 0x000854F8
	public static void LogDictionary<TKey, TValue>(IDictionary<TKey, TValue> dictionary, Func<KeyValuePair<TKey, TValue>, string> toString = null, bool showTypeAndCount = true)
	{
		if (dictionary == null || dictionary.Count == 0)
		{
			Debug.Log("Dictionary is null or empty");
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("Dictionary (" + dictionary.Count.ToString() + ") : ");
		stringBuilder.AppendLine(DebugX.DictionaryAsString<TKey, TValue>(dictionary, toString, showTypeAndCount));
		Debug.Log(stringBuilder.ToString());
	}

	// Token: 0x060012F6 RID: 4854 RVA: 0x00087360 File Offset: 0x00085560
	public static void LogDictionary<TKey, TValue>(string log, Dictionary<TKey, TValue> dictionary, Func<KeyValuePair<TKey, TValue>, string> toString = null, bool showTypeAndCount = true)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine(log);
		if (dictionary == null || !dictionary.Any<KeyValuePair<TKey, TValue>>())
		{
			stringBuilder.AppendLine("Dictionary is null or empty");
		}
		else
		{
			stringBuilder.AppendLine(DebugX.DictionaryAsString<TKey, TValue>(dictionary, toString, showTypeAndCount));
		}
		Debug.Log(stringBuilder.ToString());
	}

	// Token: 0x060012F7 RID: 4855 RVA: 0x000873AE File Offset: 0x000855AE
	public static void DrawCross(Vector3 mid, float diameter)
	{
		DebugX.DrawCross(mid, diameter, Color.white, 0f);
	}

	// Token: 0x060012F8 RID: 4856 RVA: 0x000873C4 File Offset: 0x000855C4
	public static void DrawCross(Vector3 mid, float diameter, Color color, float duration = 0f)
	{
		float num = 0.5f * diameter;
		Debug.DrawLine(mid - num * Vector3.up, mid + num * Vector3.up, color, duration);
		Debug.DrawLine(mid - num * Vector3.right, mid + num * Vector3.right, color, duration);
	}
}
