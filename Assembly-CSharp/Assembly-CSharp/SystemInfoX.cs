using System;
using UnityEngine;

// Token: 0x0200020F RID: 527
public static class SystemInfoX
{
	// Token: 0x1700044C RID: 1100
	// (get) Token: 0x0600136A RID: 4970 RVA: 0x00088CB0 File Offset: 0x00086EB0
	public static bool IsMacOS
	{
		get
		{
			return SystemInfo.operatingSystem.Contains("Mac OS");
		}
	}

	// Token: 0x1700044D RID: 1101
	// (get) Token: 0x0600136B RID: 4971 RVA: 0x00088CC1 File Offset: 0x00086EC1
	public static bool IsWinOS
	{
		get
		{
			return SystemInfo.operatingSystem.Contains("Windows");
		}
	}
}
