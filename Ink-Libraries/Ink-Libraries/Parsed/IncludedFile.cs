using System;
using Ink.Runtime;

namespace Ink.Parsed
{
	// Token: 0x02000052 RID: 82
	public class IncludedFile : Object
	{
		// Token: 0x17000108 RID: 264
		// (get) Token: 0x0600044D RID: 1101 RVA: 0x000175CD File Offset: 0x000157CD
		// (set) Token: 0x0600044E RID: 1102 RVA: 0x000175D5 File Offset: 0x000157D5
		public Story includedStory { get; private set; }

		// Token: 0x0600044F RID: 1103 RVA: 0x000175DE File Offset: 0x000157DE
		public IncludedFile(Story includedStory)
		{
			this.includedStory = includedStory;
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x000175ED File Offset: 0x000157ED
		public override Object GenerateRuntimeObject()
		{
			return null;
		}
	}
}
