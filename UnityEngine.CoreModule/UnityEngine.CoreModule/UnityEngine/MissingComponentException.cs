using System;
using System.Runtime.Serialization;

namespace UnityEngine
{
	// Token: 0x0200021A RID: 538
	[Serializable]
	public class MissingComponentException : SystemException
	{
		// Token: 0x06001763 RID: 5987 RVA: 0x00025D11 File Offset: 0x00023F11
		public MissingComponentException()
			: base("A Unity Runtime error occurred!")
		{
			base.HResult = -2147467261;
		}

		// Token: 0x06001764 RID: 5988 RVA: 0x00025D2C File Offset: 0x00023F2C
		public MissingComponentException(string message)
			: base(message)
		{
			base.HResult = -2147467261;
		}

		// Token: 0x06001765 RID: 5989 RVA: 0x00025D43 File Offset: 0x00023F43
		public MissingComponentException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.HResult = -2147467261;
		}

		// Token: 0x06001766 RID: 5990 RVA: 0x00025D5B File Offset: 0x00023F5B
		protected MissingComponentException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x04000805 RID: 2053
		private const int Result = -2147467261;

		// Token: 0x04000806 RID: 2054
		private string unityStackTrace;
	}
}
