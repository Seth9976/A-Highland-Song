using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200004C RID: 76
[Serializable]
public struct TimeOfDayPicker
{
	// Token: 0x0600021A RID: 538 RVA: 0x000134A0 File Offset: 0x000116A0
	public TimeOfDayPicker(params int[] times)
	{
		this.flagsValue = 0;
		foreach (int num in times)
		{
			this.SetAtTime(num);
		}
	}

	// Token: 0x0600021B RID: 539 RVA: 0x000134CF File Offset: 0x000116CF
	public bool IsSetAtTime(float hour)
	{
		return TimeOfDayPicker.IsSetAtTime(this.flagsValue, Mathf.FloorToInt(hour));
	}

	// Token: 0x0600021C RID: 540 RVA: 0x000134E2 File Offset: 0x000116E2
	public static bool IsSetAtTime(int flagsValue, int hour)
	{
		hour = TimeOfDayPicker.Mod(hour, 24);
		return (flagsValue & (1 << hour)) != 0;
	}

	// Token: 0x0600021D RID: 541 RVA: 0x000134F9 File Offset: 0x000116F9
	public void SetAtTime(int hour)
	{
		this.flagsValue = TimeOfDayPicker.SetAtTime(this.flagsValue, hour);
	}

	// Token: 0x0600021E RID: 542 RVA: 0x0001350D File Offset: 0x0001170D
	public static int SetAtTime(int flagsValue, int hour)
	{
		hour = TimeOfDayPicker.Mod(hour, 24);
		return flagsValue | (1 << hour);
	}

	// Token: 0x0600021F RID: 543 RVA: 0x00013521 File Offset: 0x00011721
	public void UnsetAtTime(int hour)
	{
		this.flagsValue = TimeOfDayPicker.UnsetAtTime(this.flagsValue, hour);
	}

	// Token: 0x06000220 RID: 544 RVA: 0x00013535 File Offset: 0x00011735
	public static int UnsetAtTime(int flagsValue, int hour)
	{
		hour = TimeOfDayPicker.Mod(hour, 24);
		return flagsValue & ~(1 << hour);
	}

	// Token: 0x06000221 RID: 545 RVA: 0x0001354A File Offset: 0x0001174A
	public void ToggleAtTime(int hour)
	{
		this.flagsValue = TimeOfDayPicker.ToggleAtTime(this.flagsValue, hour);
	}

	// Token: 0x06000222 RID: 546 RVA: 0x0001355E File Offset: 0x0001175E
	public static int ToggleAtTime(int flagsValue, int hour)
	{
		hour = TimeOfDayPicker.Mod(hour, 24);
		return flagsValue ^ (1 << hour);
	}

	// Token: 0x06000223 RID: 547 RVA: 0x00013572 File Offset: 0x00011772
	public IEnumerable<int> Times()
	{
		int num;
		for (int hour = 0; hour < 24; hour = num + 1)
		{
			if (TimeOfDayPicker.IsSetAtTime(this.flagsValue, hour))
			{
				yield return hour;
			}
			num = hour;
		}
		yield break;
	}

	// Token: 0x06000224 RID: 548 RVA: 0x00013587 File Offset: 0x00011787
	public static IEnumerable<int> Times(int flagsValue)
	{
		int num;
		for (int hour = 0; hour < 24; hour = num + 1)
		{
			if (TimeOfDayPicker.IsSetAtTime(flagsValue, hour))
			{
				yield return hour;
			}
			num = hour;
		}
		yield break;
	}

	// Token: 0x06000225 RID: 549 RVA: 0x00013598 File Offset: 0x00011798
	private static int Mod(int a, int n)
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

	// Token: 0x04000335 RID: 821
	public static TimeOfDayPicker none = default(TimeOfDayPicker);

	// Token: 0x04000336 RID: 822
	public static TimeOfDayPicker all = new TimeOfDayPicker(new int[]
	{
		0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
		10, 11, 12, 13, 14, 15, 16, 17, 18, 19,
		20, 21, 22, 23
	});

	// Token: 0x04000337 RID: 823
	public static TimeOfDayPicker torchTimes = new TimeOfDayPicker(new int[]
	{
		18, 19, 20, 21, 22, 23, 0, 1, 2, 3,
		4, 5
	});

	// Token: 0x04000338 RID: 824
	public static TimeOfDayPicker morning = new TimeOfDayPicker(new int[] { 6, 7, 8 });

	// Token: 0x04000339 RID: 825
	public static TimeOfDayPicker evening = new TimeOfDayPicker(new int[] { 17, 18, 19 });

	// Token: 0x0400033A RID: 826
	public static TimeOfDayPicker day = new TimeOfDayPicker(new int[]
	{
		6, 7, 8, 9, 10, 11, 12, 13, 14, 15,
		16, 17, 18, 19
	});

	// Token: 0x0400033B RID: 827
	public static TimeOfDayPicker night = new TimeOfDayPicker(new int[] { 0, 1, 2, 3, 4, 5, 20, 21, 22, 23 });

	// Token: 0x0400033C RID: 828
	public int flagsValue;
}
