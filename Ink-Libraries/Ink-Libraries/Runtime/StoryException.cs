using System;

namespace Ink.Runtime
{
	// Token: 0x0200002C RID: 44
	public class StoryException : Exception
	{
		// Token: 0x060002BD RID: 701 RVA: 0x0001103E File Offset: 0x0000F23E
		public StoryException()
		{
		}

		// Token: 0x060002BE RID: 702 RVA: 0x00011046 File Offset: 0x0000F246
		public StoryException(string message)
			: base(message)
		{
		}

		// Token: 0x040000DF RID: 223
		public bool useEndLineNumber;
	}
}
