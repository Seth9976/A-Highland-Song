using System;

namespace UnityEngine
{
	// Token: 0x02000224 RID: 548
	[Flags]
	public enum HideFlags
	{
		// Token: 0x04000816 RID: 2070
		None = 0,
		// Token: 0x04000817 RID: 2071
		HideInHierarchy = 1,
		// Token: 0x04000818 RID: 2072
		HideInInspector = 2,
		// Token: 0x04000819 RID: 2073
		DontSaveInEditor = 4,
		// Token: 0x0400081A RID: 2074
		NotEditable = 8,
		// Token: 0x0400081B RID: 2075
		DontSaveInBuild = 16,
		// Token: 0x0400081C RID: 2076
		DontUnloadUnusedAsset = 32,
		// Token: 0x0400081D RID: 2077
		DontSave = 52,
		// Token: 0x0400081E RID: 2078
		HideAndDontSave = 61
	}
}
