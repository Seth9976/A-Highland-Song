using System;
using System.Reflection;

namespace UnityEngine.Events
{
	// Token: 0x020002BD RID: 701
	internal class CachedInvokableCall<T> : InvokableCall<T>
	{
		// Token: 0x06001D48 RID: 7496 RVA: 0x0002F1AC File Offset: 0x0002D3AC
		public CachedInvokableCall(Object target, MethodInfo theFunction, T argument)
			: base(target, theFunction)
		{
			this.m_Arg1 = argument;
		}

		// Token: 0x06001D49 RID: 7497 RVA: 0x0002F1BF File Offset: 0x0002D3BF
		public override void Invoke(object[] args)
		{
			base.Invoke(this.m_Arg1);
		}

		// Token: 0x06001D4A RID: 7498 RVA: 0x0002F1BF File Offset: 0x0002D3BF
		public override void Invoke(T arg0)
		{
			base.Invoke(this.m_Arg1);
		}

		// Token: 0x04000999 RID: 2457
		private readonly T m_Arg1;
	}
}
