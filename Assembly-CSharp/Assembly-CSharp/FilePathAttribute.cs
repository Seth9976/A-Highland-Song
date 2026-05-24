using System;
using UnityEngine;

// Token: 0x0200021D RID: 541
public class FilePathAttribute : PropertyAttribute
{
	// Token: 0x06001397 RID: 5015 RVA: 0x00089A71 File Offset: 0x00087C71
	public FilePathAttribute()
	{
	}

	// Token: 0x06001398 RID: 5016 RVA: 0x00089A79 File Offset: 0x00087C79
	public FilePathAttribute(FilePathAttribute.RelativeTo relativeTo)
	{
		this.relativeTo = relativeTo;
	}

	// Token: 0x06001399 RID: 5017 RVA: 0x00089A88 File Offset: 0x00087C88
	public FilePathAttribute(FilePathAttribute.RelativeTo relativeTo, bool allowScrolling)
	{
		this.relativeTo = relativeTo;
		this.showPrevNextFileControls = allowScrolling;
	}

	// Token: 0x040012CC RID: 4812
	public FilePathAttribute.RelativeTo relativeTo;

	// Token: 0x040012CD RID: 4813
	public bool showPrevNextFileControls;

	// Token: 0x02000419 RID: 1049
	public enum RelativeTo
	{
		// Token: 0x04001B19 RID: 6937
		Root,
		// Token: 0x04001B1A RID: 6938
		Project,
		// Token: 0x04001B1B RID: 6939
		Assets,
		// Token: 0x04001B1C RID: 6940
		Resources,
		// Token: 0x04001B1D RID: 6941
		PersistentDataPath
	}
}
