using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000205 RID: 517
public static class FlagsX
{
	// Token: 0x060012F9 RID: 4857 RVA: 0x0008742B File Offset: 0x0008562B
	public static bool IsSet(int flagsValue, int flagValue)
	{
		return (flagsValue & flagValue) != 0;
	}

	// Token: 0x060012FA RID: 4858 RVA: 0x00087434 File Offset: 0x00085634
	public static bool AnySet(int flagsValue, params int[] flagValues)
	{
		return flagValues.Any((int x) => FlagsX.IsSet(flagsValue, x));
	}

	// Token: 0x060012FB RID: 4859 RVA: 0x00087460 File Offset: 0x00085660
	public static bool AllSet(int flagsValue, params int[] flagValues)
	{
		return flagValues.All((int x) => FlagsX.IsSet(flagsValue, x));
	}

	// Token: 0x060012FC RID: 4860 RVA: 0x0008748C File Offset: 0x0008568C
	private static int SetSingle(int flagsValue, int flagValue)
	{
		return flagsValue | flagValue;
	}

	// Token: 0x060012FD RID: 4861 RVA: 0x00087494 File Offset: 0x00085694
	public static int Set(int flagsValue, params int[] flagValues)
	{
		foreach (int num in flagValues)
		{
			flagsValue = FlagsX.SetSingle(flagsValue, num);
		}
		return flagsValue;
	}

	// Token: 0x060012FE RID: 4862 RVA: 0x000874BF File Offset: 0x000856BF
	public static int Unset(int flagsValue, int flagValue)
	{
		return flagsValue & ~flagValue;
	}

	// Token: 0x060012FF RID: 4863 RVA: 0x000874C8 File Offset: 0x000856C8
	public static T Create<T>(params T[] flags) where T : struct
	{
		int[] array = new int[flags.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = (int)((object)flags[i]);
		}
		return (T)((object)FlagsX.Create(array));
	}

	// Token: 0x06001300 RID: 4864 RVA: 0x00087510 File Offset: 0x00085710
	private static int Create(params int[] flags)
	{
		int num = 0;
		foreach (int num2 in flags)
		{
			if (!FlagsX.IsSet(num, num2))
			{
				num = FlagsX.Set(num, new int[] { num2 });
			}
		}
		return num;
	}

	// Token: 0x06001301 RID: 4865 RVA: 0x0008754E File Offset: 0x0008574E
	public static T CreateEverything<T>() where T : struct
	{
		return (T)((object)(-1));
	}

	// Token: 0x06001302 RID: 4866 RVA: 0x0008755B File Offset: 0x0008575B
	public static int LinearToFlagValue(int indexValue)
	{
		return (int)Mathf.Pow(2f, (float)indexValue);
	}

	// Token: 0x06001303 RID: 4867 RVA: 0x0008756A File Offset: 0x0008576A
	public static int LinearToFlagValue<T>(T flags) where T : struct
	{
		return FlagsX.LinearToFlagValue((int)((object)flags));
	}

	// Token: 0x06001304 RID: 4868 RVA: 0x0008757C File Offset: 0x0008577C
	public static bool Intersects(int flagsA, int flagsB)
	{
		return FlagsX.Intersection(flagsA, flagsB) != 0;
	}

	// Token: 0x06001305 RID: 4869 RVA: 0x00087588 File Offset: 0x00085788
	public static int Intersection(int flagsA, int flagsB)
	{
		return flagsA & flagsB;
	}

	// Token: 0x06001306 RID: 4870 RVA: 0x0008758D File Offset: 0x0008578D
	public static int Union(int flagsA, int flagsB)
	{
		return flagsA | flagsB;
	}

	// Token: 0x06001307 RID: 4871 RVA: 0x00087592 File Offset: 0x00085792
	public static int Invert<T>(int flags) where T : struct
	{
		return (int)((object)FlagsX.CreateEverything<T>()) & ~flags;
	}

	// Token: 0x06001308 RID: 4872 RVA: 0x000875A8 File Offset: 0x000857A8
	public static IEnumerable<Enum> GetIndividualFlags(this Enum value)
	{
		Type type = value.GetType();
		Enum[] array = null;
		if (!FlagsX.individualFlagsCache.TryGetValue(type, out array))
		{
			array = (FlagsX.individualFlagsCache[type] = FlagsX.GetFlagValues(type).ToArray<Enum>());
		}
		return FlagsX.GetFlags(value, array);
	}

	// Token: 0x06001309 RID: 4873 RVA: 0x000875F0 File Offset: 0x000857F0
	private static IEnumerable<Enum> GetFlags(Enum value, Enum[] values)
	{
		ulong num = Convert.ToUInt64(value);
		List<Enum> list = new List<Enum>();
		for (int i = values.Length - 1; i >= 0; i--)
		{
			ulong num2 = Convert.ToUInt64(values[i]);
			if (i == 0 && num2 == 0UL)
			{
				break;
			}
			if ((num & num2) == num2)
			{
				list.Add(values[i]);
				num -= num2;
			}
		}
		if (num != 0UL)
		{
			return Enumerable.Empty<Enum>();
		}
		if (Convert.ToUInt64(value) != 0UL)
		{
			return list.Reverse<Enum>();
		}
		if (num == Convert.ToUInt64(value) && values.Length != 0 && Convert.ToUInt64(values[0]) == 0UL)
		{
			return values.Take(1);
		}
		return Enumerable.Empty<Enum>();
	}

	// Token: 0x0600130A RID: 4874 RVA: 0x00087678 File Offset: 0x00085878
	private static IEnumerable<Enum> GetFlagValues(Type enumType)
	{
		ulong flag = 1UL;
		foreach (Enum @enum in Enum.GetValues(enumType).Cast<Enum>())
		{
			ulong num = Convert.ToUInt64(@enum);
			if (num != 0UL)
			{
				while (flag < num)
				{
					flag <<= 1;
				}
				if (flag == num)
				{
					yield return @enum;
				}
			}
		}
		IEnumerator<Enum> enumerator = null;
		yield break;
		yield break;
	}

	// Token: 0x0600130B RID: 4875 RVA: 0x00087688 File Offset: 0x00085888
	public static int FlagToEnumValue(int flagValue)
	{
		if (flagValue == 0)
		{
			return 0;
		}
		if (flagValue < 0)
		{
			return -1;
		}
		int num = 1;
		while (flagValue != 1)
		{
			flagValue >>= 1;
			num++;
		}
		return num;
	}

	// Token: 0x0600130C RID: 4876 RVA: 0x000876B2 File Offset: 0x000858B2
	public static int EnumToFlagValue(int enumValue)
	{
		if (enumValue != 0)
		{
			return 1 << enumValue - 1;
		}
		return 0;
	}

	// Token: 0x040012A7 RID: 4775
	private static Dictionary<Type, Enum[]> individualFlagsCache = new Dictionary<Type, Enum[]>();
}
