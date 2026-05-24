using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Token: 0x0200020A RID: 522
public static class ListX
{
	// Token: 0x06001354 RID: 4948 RVA: 0x000886C1 File Offset: 0x000868C1
	public static bool ContainsIndex<T>(this IList<T> list, int index)
	{
		return list != null && list.Count != 0 && index >= 0 && index <= list.Count - 1;
	}

	// Token: 0x06001355 RID: 4949 RVA: 0x000886E4 File Offset: 0x000868E4
	public static void UpdateAndRemoveIf<T>(this IList<T> list, Predicate<T> update)
	{
		int i = 0;
		while (i < list.Count)
		{
			T t = list[i];
			if (update(t))
			{
				int num = list.Count - 1;
				list[i] = list[num];
				list.RemoveAt(num);
			}
			else
			{
				i++;
			}
		}
	}

	// Token: 0x06001356 RID: 4950 RVA: 0x00088734 File Offset: 0x00086934
	public static void RemoveAllAnd<T>(this IList<T> list, Predicate<T> pred, Action<T> and = null)
	{
		int i = 0;
		while (i < list.Count)
		{
			T t = list[i];
			if (pred(t))
			{
				if (and != null)
				{
					and(t);
				}
				int num = list.Count - 1;
				list[i] = list[num];
				list.RemoveAt(num);
			}
			else
			{
				i++;
			}
		}
	}

	// Token: 0x06001357 RID: 4951 RVA: 0x00088790 File Offset: 0x00086990
	public static void RemoveAllOrderedAnd<T>(this IList<T> list, Predicate<T> pred, Action<T> and = null)
	{
		int num = 0;
		for (int i = 0; i < list.Count; i++)
		{
			T t = list[i];
			if (pred(t))
			{
				if (and != null)
				{
					and(t);
				}
				num++;
			}
			else if (num > 0)
			{
				list[i - num] = list[i];
			}
		}
		for (int j = 0; j < num; j++)
		{
			list.RemoveAt(list.Count - 1);
		}
	}

	// Token: 0x06001358 RID: 4952 RVA: 0x000887FF File Offset: 0x000869FF
	public static int GetRepeatingIndex<T>(this IList<T> list, int index)
	{
		return ListX.<GetRepeatingIndex>g__Mod|4_0<T>(index, list.Count);
	}

	// Token: 0x06001359 RID: 4953 RVA: 0x0008880D File Offset: 0x00086A0D
	public static T GetRepeating<T>(this IList<T> list, int index)
	{
		return list[list.GetRepeatingIndex(index)];
	}

	// Token: 0x0600135A RID: 4954 RVA: 0x0008881C File Offset: 0x00086A1C
	[CompilerGenerated]
	internal static int <GetRepeatingIndex>g__Mod|4_0<T>(int a, int n)
	{
		if (n == 0)
		{
			throw new ArgumentOutOfRangeException("n", "(a mod 0) is undefined.");
		}
		int num = a % n;
		if ((n > 0 && num < 0) || (n < 0 && num > 0))
		{
			return num + n;
		}
		return num;
	}
}
