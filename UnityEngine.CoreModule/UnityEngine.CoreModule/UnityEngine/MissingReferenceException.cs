using System;
using System.Runtime.Serialization;

namespace UnityEngine
{
	// Token: 0x0200021C RID: 540
	[Serializable]
	public class MissingReferenceException : SystemException
	{
		// Token: 0x0600176B RID: 5995 RVA: 0x00025D11 File Offset: 0x00023F11
		public MissingReferenceException()
			: base("A Unity Runtime error occurred!")
		{
			base.HResult = -2147467261;
		}

		// Token: 0x0600176C RID: 5996 RVA: 0x00025D2C File Offset: 0x00023F2C
		public MissingReferenceException(string message)
			: base(message)
		{
			base.HResult = -2147467261;
		}

		// Token: 0x0600176D RID: 5997 RVA: 0x00025D43 File Offset: 0x00023F43
		public MissingReferenceException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.HResult = -2147467261;
		}

		// Token: 0x0600176E RID: 5998 RVA: 0x00025D5B File Offset: 0x00023F5B
		protected MissingReferenceException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x04000809 RID: 2057
		private const int Result = -2147467261;

		// Token: 0x0400080A RID: 2058
		private string unityStackTrace;
	}
}
