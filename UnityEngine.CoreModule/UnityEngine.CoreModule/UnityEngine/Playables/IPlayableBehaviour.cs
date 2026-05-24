using System;
using UnityEngine.Scripting;

namespace UnityEngine.Playables
{
	// Token: 0x02000435 RID: 1077
	public interface IPlayableBehaviour
	{
		// Token: 0x0600256F RID: 9583
		[RequiredByNativeCode]
		void OnGraphStart(Playable playable);

		// Token: 0x06002570 RID: 9584
		[RequiredByNativeCode]
		void OnGraphStop(Playable playable);

		// Token: 0x06002571 RID: 9585
		[RequiredByNativeCode]
		void OnPlayableCreate(Playable playable);

		// Token: 0x06002572 RID: 9586
		[RequiredByNativeCode]
		void OnPlayableDestroy(Playable playable);

		// Token: 0x06002573 RID: 9587
		[RequiredByNativeCode]
		void OnBehaviourPlay(Playable playable, FrameData info);

		// Token: 0x06002574 RID: 9588
		[RequiredByNativeCode]
		void OnBehaviourPause(Playable playable, FrameData info);

		// Token: 0x06002575 RID: 9589
		[RequiredByNativeCode]
		void PrepareFrame(Playable playable, FrameData info);

		// Token: 0x06002576 RID: 9590
		[RequiredByNativeCode]
		void ProcessFrame(Playable playable, FrameData info, object playerData);
	}
}
