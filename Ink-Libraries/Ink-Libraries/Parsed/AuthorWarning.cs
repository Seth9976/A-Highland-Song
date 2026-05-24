using System;
using Ink.Runtime;

namespace Ink.Parsed
{
	// Token: 0x0200003E RID: 62
	public class AuthorWarning : Object
	{
		// Token: 0x0600038F RID: 911 RVA: 0x00013E96 File Offset: 0x00012096
		public AuthorWarning(string message)
		{
			this.warningMessage = message;
		}

		// Token: 0x06000390 RID: 912 RVA: 0x00013EA5 File Offset: 0x000120A5
		public override Object GenerateRuntimeObject()
		{
			base.Warning(this.warningMessage, null);
			return null;
		}

		// Token: 0x04000112 RID: 274
		public string warningMessage;
	}
}
