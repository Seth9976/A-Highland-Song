using System;

namespace UnityEngine.SceneManagement
{
	// Token: 0x020002E3 RID: 739
	public class SceneManagerAPI
	{
		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x06001E26 RID: 7718 RVA: 0x00030F52 File Offset: 0x0002F152
		internal static SceneManagerAPI ActiveAPI
		{
			get
			{
				return SceneManagerAPI.overrideAPI ?? SceneManagerAPI.s_DefaultAPI;
			}
		}

		// Token: 0x170005E1 RID: 1505
		// (get) Token: 0x06001E27 RID: 7719 RVA: 0x00030F62 File Offset: 0x0002F162
		// (set) Token: 0x06001E28 RID: 7720 RVA: 0x00030F69 File Offset: 0x0002F169
		public static SceneManagerAPI overrideAPI { get; set; }

		// Token: 0x06001E29 RID: 7721 RVA: 0x00008CAF File Offset: 0x00006EAF
		protected internal SceneManagerAPI()
		{
		}

		// Token: 0x06001E2A RID: 7722 RVA: 0x00030F71 File Offset: 0x0002F171
		protected internal virtual int GetNumScenesInBuildSettings()
		{
			return SceneManagerAPIInternal.GetNumScenesInBuildSettings();
		}

		// Token: 0x06001E2B RID: 7723 RVA: 0x00030F78 File Offset: 0x0002F178
		protected internal virtual Scene GetSceneByBuildIndex(int buildIndex)
		{
			return SceneManagerAPIInternal.GetSceneByBuildIndex(buildIndex);
		}

		// Token: 0x06001E2C RID: 7724 RVA: 0x00030F80 File Offset: 0x0002F180
		protected internal virtual AsyncOperation LoadSceneAsyncByNameOrIndex(string sceneName, int sceneBuildIndex, LoadSceneParameters parameters, bool mustCompleteNextFrame)
		{
			return SceneManagerAPIInternal.LoadSceneAsyncNameIndexInternal(sceneName, sceneBuildIndex, parameters, mustCompleteNextFrame);
		}

		// Token: 0x06001E2D RID: 7725 RVA: 0x00030F8C File Offset: 0x0002F18C
		protected internal virtual AsyncOperation UnloadSceneAsyncByNameOrIndex(string sceneName, int sceneBuildIndex, bool immediately, UnloadSceneOptions options, out bool outSuccess)
		{
			return SceneManagerAPIInternal.UnloadSceneNameIndexInternal(sceneName, sceneBuildIndex, immediately, options, out outSuccess);
		}

		// Token: 0x06001E2E RID: 7726 RVA: 0x00030F9A File Offset: 0x0002F19A
		protected internal virtual AsyncOperation LoadFirstScene(bool mustLoadAsync)
		{
			return null;
		}

		// Token: 0x040009E1 RID: 2529
		private static SceneManagerAPI s_DefaultAPI = new SceneManagerAPI();
	}
}
