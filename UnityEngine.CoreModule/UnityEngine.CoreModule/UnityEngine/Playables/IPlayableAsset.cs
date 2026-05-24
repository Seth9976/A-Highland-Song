using System;
using System.Collections.Generic;

namespace UnityEngine.Playables
{
	// Token: 0x0200043A RID: 1082
	public interface IPlayableAsset
	{
		// Token: 0x06002582 RID: 9602
		Playable CreatePlayable(PlayableGraph graph, GameObject owner);

		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x06002583 RID: 9603
		double duration { get; }

		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x06002584 RID: 9604
		IEnumerable<PlayableBinding> outputs { get; }
	}
}
