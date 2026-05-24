using System;
using UnityEngine;

// Token: 0x0200021E RID: 542
public class FolderPathAttribute : PropertyAttribute
{
	// Token: 0x0600139A RID: 5018 RVA: 0x00089A9E File Offset: 0x00087C9E
	public FolderPathAttribute()
	{
	}

	// Token: 0x0600139B RID: 5019 RVA: 0x00089AA6 File Offset: 0x00087CA6
	public FolderPathAttribute(FolderPathAttribute.RelativeTo relativeTo)
	{
		this.relativeTo = relativeTo;
	}

	// Token: 0x040012CE RID: 4814
	public FolderPathAttribute.RelativeTo relativeTo;

	// Token: 0x0200041A RID: 1050
	public enum RelativeTo
	{
		// Token: 0x04001B1F RID: 6943
		Root,
		// Token: 0x04001B20 RID: 6944
		Project,
		// Token: 0x04001B21 RID: 6945
		Assets,
		// Token: 0x04001B22 RID: 6946
		Desktop,
		// Token: 0x04001B23 RID: 6947
		PersistentDataPath
	}
}
