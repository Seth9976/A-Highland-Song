using System;

namespace UnityEngine.UIElements.StyleSheets
{
	// Token: 0x0200036C RID: 876
	internal struct MatchResult
	{
		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x06001BE9 RID: 7145 RVA: 0x00082590 File Offset: 0x00080790
		public bool success
		{
			get
			{
				return this.errorCode == MatchResultErrorCode.None;
			}
		}

		// Token: 0x04000DEB RID: 3563
		public MatchResultErrorCode errorCode;

		// Token: 0x04000DEC RID: 3564
		public string errorValue;
	}
}
