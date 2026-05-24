using System;
using System.Collections.Generic;

namespace UnityEngine.Windows.Speech
{
	// Token: 0x0200029A RID: 666
	public sealed class KeywordRecognizer : PhraseRecognizer
	{
		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x06001C7B RID: 7291 RVA: 0x0002DA92 File Offset: 0x0002BC92
		// (set) Token: 0x06001C7C RID: 7292 RVA: 0x0002DA9A File Offset: 0x0002BC9A
		public IEnumerable<string> Keywords { get; private set; }

		// Token: 0x06001C7D RID: 7293 RVA: 0x0002DAA3 File Offset: 0x0002BCA3
		public KeywordRecognizer(string[] keywords)
			: this(keywords, ConfidenceLevel.Medium)
		{
		}

		// Token: 0x06001C7E RID: 7294 RVA: 0x0002DAB0 File Offset: 0x0002BCB0
		public KeywordRecognizer(string[] keywords, ConfidenceLevel minimumConfidence)
		{
			bool flag = keywords == null;
			if (flag)
			{
				throw new ArgumentNullException("keywords");
			}
			bool flag2 = keywords.Length == 0;
			if (flag2)
			{
				throw new ArgumentException("At least one keyword must be specified.", "keywords");
			}
			int num = keywords.Length;
			for (int i = 0; i < num; i++)
			{
				bool flag3 = keywords[i] == null;
				if (flag3)
				{
					throw new ArgumentNullException(string.Format("Keyword at index {0} is null.", i));
				}
			}
			this.Keywords = keywords;
			this.m_Recognizer = PhraseRecognizer.CreateFromKeywords(this, keywords, minimumConfidence);
		}
	}
}
