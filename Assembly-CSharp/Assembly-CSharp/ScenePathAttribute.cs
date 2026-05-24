using System;
using UnityEngine;

// Token: 0x020001FC RID: 508
public class ScenePathAttribute : PropertyAttribute
{
	// Token: 0x060012BF RID: 4799 RVA: 0x0008618B File Offset: 0x0008438B
	public ScenePathAttribute(ScenePathAttribute.SceneFindMethod findMethod = ScenePathAttribute.SceneFindMethod.EnabledInBuild, bool useFullPath = true)
	{
		this.findMethod = findMethod;
		this.useFullPath = useFullPath;
	}

	// Token: 0x040012A2 RID: 4770
	public ScenePathAttribute.SceneFindMethod findMethod;

	// Token: 0x040012A3 RID: 4771
	public bool useFullPath;

	// Token: 0x0200040A RID: 1034
	public enum SceneFindMethod
	{
		// Token: 0x04001AE9 RID: 6889
		EnabledInBuild,
		// Token: 0x04001AEA RID: 6890
		AllInBuild,
		// Token: 0x04001AEB RID: 6891
		AllInProject
	}
}
