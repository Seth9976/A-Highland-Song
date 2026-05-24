using System;

namespace UnityEngine.Assertions
{
	// Token: 0x02000485 RID: 1157
	public class AssertionException : Exception
	{
		// Token: 0x06002901 RID: 10497 RVA: 0x00043BA7 File Offset: 0x00041DA7
		public AssertionException(string message, string userMessage)
			: base(message)
		{
			this.m_UserMessage = userMessage;
		}

		// Token: 0x170007C7 RID: 1991
		// (get) Token: 0x06002902 RID: 10498 RVA: 0x00043BBC File Offset: 0x00041DBC
		public override string Message
		{
			get
			{
				string text = base.Message;
				bool flag = this.m_UserMessage != null;
				if (flag)
				{
					text = this.m_UserMessage + "\n" + text;
				}
				return text;
			}
		}

		// Token: 0x04000F97 RID: 3991
		private string m_UserMessage;
	}
}
