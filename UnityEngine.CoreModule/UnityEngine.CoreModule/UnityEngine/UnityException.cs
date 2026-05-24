using System;
using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000219 RID: 537
	[RequiredByNativeCode]
	[Serializable]
	public class UnityException : SystemException
	{
		// Token: 0x0600175F RID: 5983 RVA: 0x00025D11 File Offset: 0x00023F11
		public UnityException()
			: base("A Unity Runtime error occurred!")
		{
			base.HResult = -2147467261;
		}

		// Token: 0x06001760 RID: 5984 RVA: 0x00025D2C File Offset: 0x00023F2C
		public UnityException(string message)
			: base(message)
		{
			base.HResult = -2147467261;
		}

		// Token: 0x06001761 RID: 5985 RVA: 0x00025D43 File Offset: 0x00023F43
		public UnityException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.HResult = -2147467261;
		}

		// Token: 0x06001762 RID: 5986 RVA: 0x00025D5B File Offset: 0x00023F5B
		protected UnityException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x04000803 RID: 2051
		private const int Result = -2147467261;

		// Token: 0x04000804 RID: 2052
		private string unityStackTrace;
	}
}
