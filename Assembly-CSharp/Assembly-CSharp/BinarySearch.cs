using System;
using System.Collections.Generic;

// Token: 0x020000F4 RID: 244
public static class BinarySearch
{
	// Token: 0x060007E0 RID: 2016 RVA: 0x0004606C File Offset: 0x0004426C
	public static int SearchRoundDown<T>(IList<T> list, float val, Func<T, float> lookup)
	{
		if (list.Count == 0)
		{
			return 0;
		}
		if (val < lookup(list[0]))
		{
			return 0;
		}
		int num = list.Count - 1;
		if (val > lookup(list[num]))
		{
			return num;
		}
		int i = 0;
		int num2 = num;
		while (i < num2)
		{
			int num3 = (i + num2) / 2 + 1;
			if (val < lookup(list[num3]))
			{
				num2 = num3 - 1;
			}
			else
			{
				i = num3;
			}
		}
		return i;
	}

	// Token: 0x060007E1 RID: 2017 RVA: 0x000460DC File Offset: 0x000442DC
	public static int SearchNext<T>(IList<T> list, float val, Func<T, float> lookup)
	{
		if (list.Count == 0)
		{
			return -1;
		}
		if (val < lookup(list[0]))
		{
			return 0;
		}
		int num = list.Count - 1;
		if (val > lookup(list[num]))
		{
			return -1;
		}
		return BinarySearch.SearchRoundDown<T>(list, val, lookup) + 1;
	}

	// Token: 0x060007E2 RID: 2018 RVA: 0x0004612C File Offset: 0x0004432C
	public static int SearchNearest<T>(IList<T> list, float val, Func<T, float> lookup)
	{
		int num = BinarySearch.SearchRoundDown<T>(list, val, lookup);
		if (num >= list.Count - 1)
		{
			return num;
		}
		float num2 = lookup(list[num]);
		float num3 = lookup(list[num + 1]);
		if (val - num2 <= num3 - val)
		{
			return num;
		}
		return num + 1;
	}
}
