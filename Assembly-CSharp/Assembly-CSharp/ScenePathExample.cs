using System;
using UnityEngine;

// Token: 0x020001FD RID: 509
public class ScenePathExample : MonoBehaviour
{
	// Token: 0x040012A4 RID: 4772
	[ScenePath(ScenePathAttribute.SceneFindMethod.EnabledInBuild, true)]
	public string sceneName;

	// Token: 0x040012A5 RID: 4773
	[ScenePath(ScenePathAttribute.SceneFindMethod.EnabledInBuild, true)]
	public string sceneName2;
}
