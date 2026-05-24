using System;
using System.Diagnostics;
using System.Reflection;

namespace UnityEngine.Events
{
	// Token: 0x020002B8 RID: 696
	internal class InvokableCall : BaseInvokableCall
	{
		// Token: 0x14000022 RID: 34
		// (add) Token: 0x06001D25 RID: 7461 RVA: 0x0002EAA0 File Offset: 0x0002CCA0
		// (remove) Token: 0x06001D26 RID: 7462 RVA: 0x0002EAD8 File Offset: 0x0002CCD8
		[field: DebuggerBrowsable(0)]
		private event UnityAction Delegate;

		// Token: 0x06001D27 RID: 7463 RVA: 0x0002EB0D File Offset: 0x0002CD0D
		public InvokableCall(object target, MethodInfo theFunction)
			: base(target, theFunction)
		{
			this.Delegate += (UnityAction)global::System.Delegate.CreateDelegate(typeof(UnityAction), target, theFunction);
		}

		// Token: 0x06001D28 RID: 7464 RVA: 0x0002EB36 File Offset: 0x0002CD36
		public InvokableCall(UnityAction action)
		{
			this.Delegate += action;
		}

		// Token: 0x06001D29 RID: 7465 RVA: 0x0002EB48 File Offset: 0x0002CD48
		public override void Invoke(object[] args)
		{
			bool flag = BaseInvokableCall.AllowInvoke(this.Delegate);
			if (flag)
			{
				this.Delegate();
			}
		}

		// Token: 0x06001D2A RID: 7466 RVA: 0x0002EB74 File Offset: 0x0002CD74
		public void Invoke()
		{
			bool flag = BaseInvokableCall.AllowInvoke(this.Delegate);
			if (flag)
			{
				this.Delegate();
			}
		}

		// Token: 0x06001D2B RID: 7467 RVA: 0x0002EBA0 File Offset: 0x0002CDA0
		public override bool Find(object targetObj, MethodInfo method)
		{
			return this.Delegate.Target == targetObj && this.Delegate.Method.Equals(method);
		}
	}
}
