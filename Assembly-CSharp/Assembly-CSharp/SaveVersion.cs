using System;

// Token: 0x020000E8 RID: 232
[Serializable]
public struct SaveVersion
{
	// Token: 0x060007AC RID: 1964 RVA: 0x0004538F File Offset: 0x0004358F
	public SaveVersion(int version)
	{
		this.version = version;
	}

	// Token: 0x060007AD RID: 1965 RVA: 0x00045398 File Offset: 0x00043598
	public bool IsCompatableWithBuild()
	{
		return this.version >= 1 && 1 >= this.version;
	}

	// Token: 0x060007AE RID: 1966 RVA: 0x000453B1 File Offset: 0x000435B1
	public override string ToString()
	{
		return string.Format("[{0}] Version:{1}", base.GetType().Name, this.version);
	}

	// Token: 0x0400099E RID: 2462
	public const int buildSaveVersion = 1;

	// Token: 0x0400099F RID: 2463
	public const int minCompatableSaveVersion = 1;

	// Token: 0x040009A0 RID: 2464
	public int version;
}
