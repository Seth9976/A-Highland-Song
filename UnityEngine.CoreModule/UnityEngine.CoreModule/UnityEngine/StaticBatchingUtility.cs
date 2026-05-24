using System;
using Unity.Profiling;

namespace UnityEngine
{
	// Token: 0x0200023F RID: 575
	public sealed class StaticBatchingUtility
	{
		// Token: 0x0600189C RID: 6300 RVA: 0x000280D0 File Offset: 0x000262D0
		public static void Combine(GameObject staticBatchRoot)
		{
			using (StaticBatchingUtility.s_CombineMarker.Auto())
			{
				InternalStaticBatchingUtility.CombineRoot(staticBatchRoot, null);
			}
		}

		// Token: 0x0600189D RID: 6301 RVA: 0x00028114 File Offset: 0x00026314
		public static void Combine(GameObject[] gos, GameObject staticBatchRoot)
		{
			using (StaticBatchingUtility.s_CombineMarker.Auto())
			{
				InternalStaticBatchingUtility.CombineGameObjects(gos, staticBatchRoot, false, null);
			}
		}

		// Token: 0x04000849 RID: 2121
		internal static ProfilerMarker s_CombineMarker = new ProfilerMarker("StaticBatching.Combine");

		// Token: 0x0400084A RID: 2122
		internal static ProfilerMarker s_SortMarker = new ProfilerMarker("StaticBatching.SortObjects");

		// Token: 0x0400084B RID: 2123
		internal static ProfilerMarker s_MakeBatchMarker = new ProfilerMarker("StaticBatching.MakeBatch");
	}
}
