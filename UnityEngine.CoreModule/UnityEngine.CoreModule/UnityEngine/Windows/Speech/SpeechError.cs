using System;

namespace UnityEngine.Windows.Speech
{
	// Token: 0x02000295 RID: 661
	public enum SpeechError
	{
		// Token: 0x04000938 RID: 2360
		NoError,
		// Token: 0x04000939 RID: 2361
		TopicLanguageNotSupported,
		// Token: 0x0400093A RID: 2362
		GrammarLanguageMismatch,
		// Token: 0x0400093B RID: 2363
		GrammarCompilationFailure,
		// Token: 0x0400093C RID: 2364
		AudioQualityFailure,
		// Token: 0x0400093D RID: 2365
		PauseLimitExceeded,
		// Token: 0x0400093E RID: 2366
		TimeoutExceeded,
		// Token: 0x0400093F RID: 2367
		NetworkFailure,
		// Token: 0x04000940 RID: 2368
		MicrophoneUnavailable,
		// Token: 0x04000941 RID: 2369
		UnknownError
	}
}
