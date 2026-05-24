using System;
using UnityEngine;

// Token: 0x020001D8 RID: 472
internal static class SetPropertyUtility
{
	// Token: 0x0600104F RID: 4175 RVA: 0x00079384 File Offset: 0x00077584
	public static bool SetColor(ref Color currentValue, Color newValue)
	{
		if (currentValue.r == newValue.r && currentValue.g == newValue.g && currentValue.b == newValue.b && currentValue.a == newValue.a)
		{
			return false;
		}
		currentValue = newValue;
		return true;
	}

	// Token: 0x06001050 RID: 4176 RVA: 0x000793D3 File Offset: 0x000775D3
	public static bool SetStruct<T>(ref T currentValue, T newValue) where T : struct
	{
		if (currentValue.Equals(newValue))
		{
			return false;
		}
		currentValue = newValue;
		return true;
	}

	// Token: 0x06001051 RID: 4177 RVA: 0x000793F4 File Offset: 0x000775F4
	public static bool SetClass<T>(ref T currentValue, T newValue) where T : class
	{
		if ((currentValue == null && newValue == null) || (currentValue != null && currentValue.Equals(newValue)))
		{
			return false;
		}
		currentValue = newValue;
		return true;
	}
}
