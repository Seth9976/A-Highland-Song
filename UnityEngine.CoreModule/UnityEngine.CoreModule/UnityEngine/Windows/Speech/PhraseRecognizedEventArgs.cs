using System;

namespace UnityEngine.Windows.Speech
{
	// Token: 0x02000299 RID: 665
	public struct PhraseRecognizedEventArgs
	{
		// Token: 0x06001C7A RID: 7290 RVA: 0x0002DA6A File Offset: 0x0002BC6A
		internal PhraseRecognizedEventArgs(string text, ConfidenceLevel confidence, SemanticMeaning[] semanticMeanings, DateTime phraseStartTime, TimeSpan phraseDuration)
		{
			this.text = text;
			this.confidence = confidence;
			this.semanticMeanings = semanticMeanings;
			this.phraseStartTime = phraseStartTime;
			this.phraseDuration = phraseDuration;
		}

		// Token: 0x04000951 RID: 2385
		public readonly ConfidenceLevel confidence;

		// Token: 0x04000952 RID: 2386
		public readonly SemanticMeaning[] semanticMeanings;

		// Token: 0x04000953 RID: 2387
		public readonly string text;

		// Token: 0x04000954 RID: 2388
		public readonly DateTime phraseStartTime;

		// Token: 0x04000955 RID: 2389
		public readonly TimeSpan phraseDuration;
	}
}
