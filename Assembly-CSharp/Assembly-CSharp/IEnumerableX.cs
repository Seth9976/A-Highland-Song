using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000207 RID: 519
public static class IEnumerableX
{
	// Token: 0x0600131C RID: 4892 RVA: 0x00087AA0 File Offset: 0x00085CA0
	public static T Random<T>(this IEnumerable<T> source)
	{
		if (source.Count<T>() == 0)
		{
			return default(T);
		}
		return source.ElementAt(source.RandomIndex<T>());
	}

	// Token: 0x0600131D RID: 4893 RVA: 0x00087ACB File Offset: 0x00085CCB
	public static int RandomIndex<T>(this IEnumerable<T> source)
	{
		return global::UnityEngine.Random.Range(0, source.Count<T>());
	}

	// Token: 0x0600131E RID: 4894 RVA: 0x00087AD9 File Offset: 0x00085CD9
	public static bool IsEmpty<T>(this ICollection<T> coll)
	{
		return coll.Count == 0;
	}

	// Token: 0x0600131F RID: 4895 RVA: 0x00087AE4 File Offset: 0x00085CE4
	public static int BestIndex<T>(this IEnumerable<T> source, IEnumerableX.GetScore<T> selector, IEnumerableX.ChooseBest comparer, float defaultBestScore = 0f)
	{
		int num = 0;
		int num2 = -1;
		float num3 = defaultBestScore;
		foreach (T t in source)
		{
			float num4 = selector(t);
			if (comparer(num4, num3))
			{
				num3 = num4;
				num2 = num;
			}
			num++;
		}
		return num2;
	}

	// Token: 0x06001320 RID: 4896 RVA: 0x00087B4C File Offset: 0x00085D4C
	public static T Best<T>(this IEnumerable<T> source, IEnumerableX.GetScore<T> selector, IEnumerableX.ChooseBest comparer, float defaultBestScore = 0f, T defaultValue = default(T))
	{
		int num = source.BestIndex(selector, comparer, defaultBestScore);
		if (num == -1)
		{
			return defaultValue;
		}
		return source.ElementAt(num);
	}

	// Token: 0x06001321 RID: 4897 RVA: 0x00087B74 File Offset: 0x00085D74
	public static T Best<T>(this IEnumerable<T> source, IEnumerableX.GetScore<T> selector, IEnumerableX.ChooseBest comparer, ref float defaultBestScore, T defaultValue = default(T))
	{
		int num = source.BestIndex(selector, comparer, defaultBestScore);
		if (num == -1)
		{
			return defaultValue;
		}
		return source.ElementAt(num);
	}

	// Token: 0x06001322 RID: 4898 RVA: 0x00087B9A File Offset: 0x00085D9A
	public static T WithMin<T, U>(this IEnumerable<T> enumerable, Func<T, U> selectedVal) where U : IComparable
	{
		return enumerable.WithMinMax(selectedVal, true);
	}

	// Token: 0x06001323 RID: 4899 RVA: 0x00087BA4 File Offset: 0x00085DA4
	public static T WithMax<T, U>(this IEnumerable<T> enumerable, Func<T, U> selectedVal) where U : IComparable
	{
		return enumerable.WithMinMax(selectedVal, false);
	}

	// Token: 0x06001324 RID: 4900 RVA: 0x00087BB0 File Offset: 0x00085DB0
	public static T WithMinMax<T, U>(this IEnumerable<T> enumerable, Func<T, U> selectedVal, bool isMin) where U : IComparable
	{
		T t = default(T);
		U u = default(U);
		int num = (isMin ? 1 : (-1));
		bool flag = true;
		foreach (T t2 in enumerable)
		{
			U u2 = selectedVal(t2);
			if (flag || num * u2.CompareTo(u) < 0)
			{
				u = u2;
				t = t2;
				flag = false;
			}
		}
		return t;
	}

	// Token: 0x06001325 RID: 4901 RVA: 0x00087C40 File Offset: 0x00085E40
	public static bool GetChanges<T>(IEnumerable<T> oldList, IEnumerable<T> newList, ref List<T> itemsRemoved, ref List<T> itemsAdded)
	{
		if (itemsRemoved == null)
		{
			itemsRemoved = new List<T>();
		}
		if (itemsAdded == null)
		{
			itemsAdded = new List<T>();
		}
		IEnumerableX.GetRemovedNonAlloc<T>(oldList, newList, itemsRemoved);
		IEnumerableX.GetAddedNonAlloc<T>(oldList, newList, itemsAdded);
		return itemsRemoved.Count > 0 || itemsAdded.Count > 0;
	}

	// Token: 0x06001326 RID: 4902 RVA: 0x00087C80 File Offset: 0x00085E80
	public static bool GetChanges<T>(IEnumerable<T> oldList, IEnumerable<T> newList, ref List<T> itemsRemoved, ref List<T> itemsAdded, ref List<T> itemsUnchanged)
	{
		if (itemsRemoved == null)
		{
			itemsRemoved = new List<T>();
		}
		if (itemsAdded == null)
		{
			itemsAdded = new List<T>();
		}
		if (itemsUnchanged == null)
		{
			itemsUnchanged = new List<T>();
		}
		IEnumerableX.GetRemovedNonAlloc<T>(oldList, newList, itemsRemoved);
		IEnumerableX.GetAddedNonAlloc<T>(oldList, newList, itemsAdded);
		IEnumerableX.GetInBothNonAlloc<T>(oldList, newList, itemsUnchanged);
		return itemsRemoved.Count > 0 || itemsAdded.Count > 0;
	}

	// Token: 0x06001327 RID: 4903 RVA: 0x00087CE2 File Offset: 0x00085EE2
	public static IEnumerable<T> GetInBoth<T>(IEnumerable<T> oldList, IEnumerable<T> newList)
	{
		if (oldList == null)
		{
			yield break;
		}
		foreach (T t in oldList)
		{
			if (newList != null && newList.Contains(t))
			{
				yield return t;
			}
		}
		IEnumerator<T> enumerator = null;
		yield break;
		yield break;
	}

	// Token: 0x06001328 RID: 4904 RVA: 0x00087CFC File Offset: 0x00085EFC
	public static void GetInBothNonAlloc<T>(IEnumerable<T> oldList, IEnumerable<T> newList, List<T> unchangedListToFill)
	{
		unchangedListToFill.Clear();
		if (oldList == null)
		{
			return;
		}
		foreach (T t in oldList)
		{
			if (newList != null && newList.Contains(t))
			{
				unchangedListToFill.Add(t);
			}
		}
	}

	// Token: 0x06001329 RID: 4905 RVA: 0x00087D5C File Offset: 0x00085F5C
	public static IEnumerable<T> GetRemoved<T>(IEnumerable<T> oldList, IEnumerable<T> newList)
	{
		if (oldList == null)
		{
			yield break;
		}
		foreach (T t in oldList)
		{
			if (newList == null || !newList.Contains(t))
			{
				yield return t;
			}
		}
		IEnumerator<T> enumerator = null;
		yield break;
		yield break;
	}

	// Token: 0x0600132A RID: 4906 RVA: 0x00087D74 File Offset: 0x00085F74
	public static void GetRemovedNonAlloc<T>(IEnumerable<T> oldList, IEnumerable<T> newList, List<T> removedListToFill)
	{
		removedListToFill.Clear();
		if (oldList == null)
		{
			return;
		}
		foreach (T t in oldList)
		{
			if (newList == null || !newList.Contains(t))
			{
				removedListToFill.Add(t);
			}
		}
	}

	// Token: 0x0600132B RID: 4907 RVA: 0x00087DD4 File Offset: 0x00085FD4
	public static IEnumerable<T> GetAdded<T>(IEnumerable<T> oldList, IEnumerable<T> newList)
	{
		if (newList == null)
		{
			yield break;
		}
		foreach (T t in newList)
		{
			if (oldList == null || !oldList.Contains(t))
			{
				yield return t;
			}
		}
		IEnumerator<T> enumerator = null;
		yield break;
		yield break;
	}

	// Token: 0x0600132C RID: 4908 RVA: 0x00087DEC File Offset: 0x00085FEC
	public static void GetAddedNonAlloc<T>(IEnumerable<T> oldList, IEnumerable<T> newList, List<T> addedListToFill)
	{
		addedListToFill.Clear();
		if (newList == null)
		{
			return;
		}
		foreach (T t in newList)
		{
			if (oldList == null || !oldList.Contains(t))
			{
				addedListToFill.Add(t);
			}
		}
	}

	// Token: 0x0600132D RID: 4909 RVA: 0x00087E4C File Offset: 0x0008604C
	public static bool ScrambledEquals<T>(IEnumerable<T> list1, IEnumerable<T> list2)
	{
		Dictionary<T, int> dictionary = new Dictionary<T, int>();
		foreach (T t in list1)
		{
			if (dictionary.ContainsKey(t))
			{
				Dictionary<T, int> dictionary2 = dictionary;
				T t2 = t;
				int num = dictionary2[t2];
				dictionary2[t2] = num + 1;
			}
			else
			{
				dictionary.Add(t, 1);
			}
		}
		foreach (T t3 in list2)
		{
			if (!dictionary.ContainsKey(t3))
			{
				return false;
			}
			Dictionary<T, int> dictionary3 = dictionary;
			T t2 = t3;
			int num = dictionary3[t2];
			dictionary3[t2] = num - 1;
		}
		return dictionary.Values.All((int c) => c == 0);
	}

	// Token: 0x0200040E RID: 1038
	// (Invoke) Token: 0x06001916 RID: 6422
	public delegate float GetScore<T>(T value);

	// Token: 0x0200040F RID: 1039
	// (Invoke) Token: 0x0600191A RID: 6426
	public delegate bool ChooseBest(float other, float currentBest);
}
