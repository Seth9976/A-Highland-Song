using System;
using System.Runtime.Serialization;

namespace UnityEngine
{
	// Token: 0x0200021B RID: 539
	[Serializable]
	public class UnassignedReferenceException : SystemException
	{
		// Token: 0x06001767 RID: 5991 RVA: 0x00025D11 File Offset: 0x00023F11
		public UnassignedReferenceException()
			: base("A Unity Runtime error occurred!")
		{
			base.HResult = -2147467261;
		}

		// Token: 0x06001768 RID: 5992 RVA: 0x00025D2C File Offset: 0x00023F2C
		public UnassignedReferenceException(string message)
			: base(message)
		{
			base.HResult = -2147467261;
		}

		// Token: 0x06001769 RID: 5993 RVA: 0x00025D43 File Offset: 0x00023F43
		public UnassignedReferenceException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.HResult = -2147467261;
		}

		// Token: 0x0600176A RID: 5994 RVA: 0x00025D5B File Offset: 0x00023F5B
		protected UnassignedReferenceException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x04000807 RID: 2055
		private const int Result = -2147467261;

		// Token: 0x04000808 RID: 2056
		private string unityStackTrace;
	}
}
