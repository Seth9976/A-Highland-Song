using System;
using Ink.Runtime;

namespace Ink.Parsed
{
	// Token: 0x02000050 RID: 80
	public class Identifier
	{
		// Token: 0x06000449 RID: 1097 RVA: 0x0001759F File Offset: 0x0001579F
		public override string ToString()
		{
			return this.name;
		}

		// Token: 0x04000161 RID: 353
		public string name;

		// Token: 0x04000162 RID: 354
		public DebugMetadata debugMetadata;

		// Token: 0x04000163 RID: 355
		public static Identifier Done = new Identifier
		{
			name = "DONE",
			debugMetadata = null
		};
	}
}
