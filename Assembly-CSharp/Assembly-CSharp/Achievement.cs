using System;

// Token: 0x020000EA RID: 234
[Serializable]
public class Achievement
{
	// Token: 0x060007B1 RID: 1969 RVA: 0x000453E7 File Offset: 0x000435E7
	public Achievement(string achievementID)
	{
		this.ID = achievementID;
	}

	// Token: 0x060007B2 RID: 1970 RVA: 0x000453F6 File Offset: 0x000435F6
	public override string ToString()
	{
		return string.Format("[{0}] ID={1} unlocked={2}", base.GetType().Name, this.ID, this.unlocked);
	}

	// Token: 0x040009A1 RID: 2465
	public string ID;

	// Token: 0x040009A2 RID: 2466
	public bool unlocked;
}
