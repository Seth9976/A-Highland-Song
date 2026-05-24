using System;

namespace UnityEngine.Windows.Speech
{
	// Token: 0x0200029B RID: 667
	public sealed class GrammarRecognizer : PhraseRecognizer
	{
		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x06001C7F RID: 7295 RVA: 0x0002DB41 File Offset: 0x0002BD41
		// (set) Token: 0x06001C80 RID: 7296 RVA: 0x0002DB49 File Offset: 0x0002BD49
		public string GrammarFilePath { get; private set; }

		// Token: 0x06001C81 RID: 7297 RVA: 0x0002DB52 File Offset: 0x0002BD52
		public GrammarRecognizer(string grammarFilePath)
			: this(grammarFilePath, ConfidenceLevel.Medium)
		{
		}

		// Token: 0x06001C82 RID: 7298 RVA: 0x0002DB60 File Offset: 0x0002BD60
		public GrammarRecognizer(string grammarFilePath, ConfidenceLevel minimumConfidence)
		{
			bool flag = grammarFilePath == null;
			if (flag)
			{
				throw new ArgumentNullException("grammarFilePath");
			}
			bool flag2 = grammarFilePath.Length == 0;
			if (flag2)
			{
				throw new ArgumentException("Grammar file path cannot be empty.");
			}
			this.GrammarFilePath = grammarFilePath;
			this.m_Recognizer = PhraseRecognizer.CreateFromGrammarFile(this, grammarFilePath, minimumConfidence);
		}
	}
}
