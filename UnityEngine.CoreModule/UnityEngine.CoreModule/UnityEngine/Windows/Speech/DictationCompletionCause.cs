using System;

namespace UnityEngine.Windows.Speech
{
	// Token: 0x02000297 RID: 663
	public enum DictationCompletionCause
	{
		// Token: 0x04000947 RID: 2375
		Complete,
		// Token: 0x04000948 RID: 2376
		AudioQualityFailure,
		// Token: 0x04000949 RID: 2377
		Canceled,
		// Token: 0x0400094A RID: 2378
		TimeoutExceeded,
		// Token: 0x0400094B RID: 2379
		PauseLimitExceeded,
		// Token: 0x0400094C RID: 2380
		NetworkFailure,
		// Token: 0x0400094D RID: 2381
		MicrophoneUnavailable,
		// Token: 0x0400094E RID: 2382
		UnknownError
	}
}
