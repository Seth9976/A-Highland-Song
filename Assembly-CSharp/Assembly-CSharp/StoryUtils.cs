using System;
using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;

// Token: 0x020000A2 RID: 162
public static class StoryUtils
{
	// Token: 0x06000547 RID: 1351 RVA: 0x0002A11C File Offset: 0x0002831C
	public static float GetEstimatedTimeToRead(string textStr, TextReadSettings textReadSettings)
	{
		string text = textStr.Trim();
		int num = 0;
		int num2 = 0;
		bool flag = false;
		for (int i = 0; i < text.Length; i++)
		{
			bool flag2 = text[i].isTerminator();
			if (flag2 && !flag)
			{
				num2++;
			}
			if (!flag2)
			{
				num++;
			}
			flag = flag2;
		}
		if (flag)
		{
			num2--;
		}
		return Mathf.Clamp(textReadSettings.extraTimeConstant + (float)num * textReadSettings.characterReadDuration + (float)num2 * textReadSettings.terminatorDuration, textReadSettings.minDuration, textReadSettings.maxDuration);
	}

	// Token: 0x06000548 RID: 1352 RVA: 0x0002A19C File Offset: 0x0002839C
	public static int[] GetValuesFromInkListVariable(InkList listVar)
	{
		if (listVar == null)
		{
			return new int[0];
		}
		int[] array = new int[listVar.Count];
		int num = 0;
		foreach (KeyValuePair<InkListItem, int> keyValuePair in listVar)
		{
			array[num] = keyValuePair.Value;
			num++;
		}
		return array;
	}

	// Token: 0x06000549 RID: 1353 RVA: 0x0002A20C File Offset: 0x0002840C
	public static string[] GetKeysFromInkList(InkList listVar)
	{
		if (listVar == null)
		{
			return new string[0];
		}
		string[] array = new string[listVar.Count];
		int num = 0;
		foreach (KeyValuePair<InkListItem, int> keyValuePair in listVar)
		{
			array[num] = keyValuePair.Key.itemName;
			num++;
		}
		return array;
	}

	// Token: 0x0600054A RID: 1354 RVA: 0x0002A280 File Offset: 0x00028480
	public static T RunInkFunction<T>(this Story story, string inkFunctionName, params object[] args)
	{
		object obj = null;
		T t = default(T);
		try
		{
			string text = null;
			obj = story.EvaluateFunction(inkFunctionName, out text, args);
			if (obj == null)
			{
				return (T)((object)text);
			}
			if (typeof(T) == typeof(bool) && obj is int)
			{
				t = (T)((object)((int)obj != 0));
			}
			else if (typeof(T) == typeof(bool) && obj is float)
			{
				t = (T)((object)((float)obj == 1f));
			}
			else if (typeof(T) == typeof(int) && obj is float)
			{
				t = (T)((object)Mathf.RoundToInt((float)obj));
			}
			else
			{
				t = (T)((object)obj);
			}
		}
		catch (Exception ex)
		{
			string name = typeof(T).Name;
			string text2 = DebugX.ListAsString<object>(args, delegate(object o)
			{
				if (o == null)
				{
					return "NULL";
				}
				return o.ToString() + " (" + o.GetType().Name + ")";
			}, false, true);
			string text3 = DebugX.ListAsString<string>(story.currentErrors, null, false, true);
			if (obj == null)
			{
				string[] array = new string[10];
				array[0] = "Ink function error at: ";
				array[1] = inkFunctionName;
				array[2] = "(<";
				array[3] = name;
				array[4] = ">)\nArgs:\n";
				array[5] = text2;
				array[6] = "\nInk Errors\n";
				array[7] = text3;
				array[8] = "\n\nErr";
				int num = 9;
				Exception ex2 = ex;
				array[num] = ((ex2 != null) ? ex2.ToString() : null);
				Debug.LogError(string.Concat(array));
			}
			else
			{
				string text6;
				if (obj != null)
				{
					string text4 = obj.ToString();
					string text5 = "(";
					Type type = obj.GetType();
					text6 = text4 + text5 + ((type != null) ? type.ToString() : null) + ")";
				}
				else
				{
					text6 = "NULL";
				}
				string text7 = text6;
				string text10;
				if (t != null)
				{
					string text8 = t.ToString();
					string text9 = "(";
					Type type2 = t.GetType();
					text10 = text8 + text9 + ((type2 != null) ? type2.ToString() : null) + ")";
				}
				else
				{
					text10 = "NULL";
				}
				string text11 = text10;
				string[] array2 = new string[14];
				array2[0] = "Ink function error at: ";
				array2[1] = inkFunctionName;
				array2[2] = "(<";
				array2[3] = name;
				array2[4] = ">)\nArgs:\n ";
				array2[5] = text2;
				array2[6] = "\n\nRawOutput ";
				array2[7] = text7;
				array2[8] = "\nOutput ";
				array2[9] = text11;
				array2[10] = "\nInk Errors:\n";
				array2[11] = text3;
				array2[12] = "\nErr";
				int num2 = 13;
				Exception ex3 = ex;
				array2[num2] = ((ex3 != null) ? ex3.ToString() : null);
				Debug.LogError(string.Concat(array2));
			}
		}
		return t;
	}
}
