using System;

namespace InControl
{
	// Token: 0x0200003B RID: 59
	[Serializable]
	public class InControlException : Exception
	{
		// Token: 0x06000233 RID: 563 RVA: 0x0000850B File Offset: 0x0000670B
		public InControlException()
		{
		}

		// Token: 0x06000234 RID: 564 RVA: 0x00008513 File Offset: 0x00006713
		public InControlException(string message)
			: base(message)
		{
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000851C File Offset: 0x0000671C
		public InControlException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
