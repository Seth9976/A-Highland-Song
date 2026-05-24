using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.SceneManagement
{
	// Token: 0x020002E2 RID: 738
	[StaticAccessor("SceneManagerBindings", StaticAccessorType.DoubleColon)]
	[NativeHeader("Runtime/SceneManager/SceneManager.h")]
	[NativeHeader("Runtime/Export/SceneManager/SceneManager.bindings.h")]
	internal static class SceneManagerAPIInternal
	{
		// Token: 0x06001E20 RID: 7712
		[MethodImpl(4096)]
		public static extern int GetNumScenesInBuildSettings();

		// Token: 0x06001E21 RID: 7713 RVA: 0x00030F30 File Offset: 0x0002F130
		[NativeThrows]
		public static Scene GetSceneByBuildIndex(int buildIndex)
		{
			Scene scene;
			SceneManagerAPIInternal.GetSceneByBuildIndex_Injected(buildIndex, out scene);
			return scene;
		}

		// Token: 0x06001E22 RID: 7714 RVA: 0x00030F46 File Offset: 0x0002F146
		[NativeThrows]
		public static AsyncOperation LoadSceneAsyncNameIndexInternal(string sceneName, int sceneBuildIndex, LoadSceneParameters parameters, bool mustCompleteNextFrame)
		{
			return SceneManagerAPIInternal.LoadSceneAsyncNameIndexInternal_Injected(sceneName, sceneBuildIndex, ref parameters, mustCompleteNextFrame);
		}

		// Token: 0x06001E23 RID: 7715
		[NativeThrows]
		[MethodImpl(4096)]
		public static extern AsyncOperation UnloadSceneNameIndexInternal(string sceneName, int sceneBuildIndex, bool immediately, UnloadSceneOptions options, out bool outSuccess);

		// Token: 0x06001E24 RID: 7716
		[MethodImpl(4096)]
		private static extern void GetSceneByBuildIndex_Injected(int buildIndex, out Scene ret);

		// Token: 0x06001E25 RID: 7717
		[MethodImpl(4096)]
		private static extern AsyncOperation LoadSceneAsyncNameIndexInternal_Injected(string sceneName, int sceneBuildIndex, ref LoadSceneParameters parameters, bool mustCompleteNextFrame);
	}
}
