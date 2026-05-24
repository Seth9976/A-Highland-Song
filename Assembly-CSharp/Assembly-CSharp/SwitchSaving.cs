using System;
using System.Collections.Generic;

// Token: 0x020000A8 RID: 168
public static class SwitchSaving
{
	// Token: 0x17000162 RID: 354
	// (get) Token: 0x06000576 RID: 1398 RVA: 0x0002B830 File Offset: 0x00029A30
	private static Dictionary<SaveLoadType, DateTime> switchSaveTimes
	{
		get
		{
			Dictionary<SaveLoadType, DateTime> switchSaveTimes = SwitchSaving._switchSaveTimes;
			Dictionary<SaveLoadType, DateTime> switchSaveTimes2;
			lock (switchSaveTimes)
			{
				switchSaveTimes2 = SwitchSaving._switchSaveTimes;
			}
			return switchSaveTimes2;
		}
	}

	// Token: 0x0400063C RID: 1596
	private static bool _savingOnBackgroundThread;

	// Token: 0x0400063D RID: 1597
	private static Dictionary<SaveLoadType, DateTime> _switchSaveTimes = new Dictionary<SaveLoadType, DateTime>();

	// Token: 0x0400063E RID: 1598
	private static object _playerPrefsFileLock = new object();
}
