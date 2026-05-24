using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.SceneManagement
{
	// Token: 0x020002EA RID: 746
	[NativeHeader("Runtime/Export/SceneManager/SceneUtility.bindings.h")]
	public static class SceneUtility
	{
		// Token: 0x06001E75 RID: 7797
		[StaticAccessor("SceneUtilityBindings", StaticAccessorType.DoubleColon)]
		[MethodImpl(4096)]
		public static extern string GetScenePathByBuildIndex(int buildIndex);

		// Token: 0x06001E76 RID: 7798
		[StaticAccessor("SceneUtilityBindings", StaticAccessorType.DoubleColon)]
		[MethodImpl(4096)]
		public static extern int GetBuildIndexByScenePath(string scenePath);
	}
}
