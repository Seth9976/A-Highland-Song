using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Scripting;

namespace UnityEngine.Events
{
	// Token: 0x020002C6 RID: 710
	[Serializable]
	public class UnityEvent<T0> : UnityEventBase
	{
		// Token: 0x06001D9C RID: 7580 RVA: 0x000300E4 File Offset: 0x0002E2E4
		[RequiredByNativeCode]
		public UnityEvent()
		{
		}

		// Token: 0x06001D9D RID: 7581 RVA: 0x000300F5 File Offset: 0x0002E2F5
		public void AddListener(UnityAction<T0> call)
		{
			base.AddCall(UnityEvent<T0>.GetDelegate(call));
		}

		// Token: 0x06001D9E RID: 7582 RVA: 0x0002FFCF File Offset: 0x0002E1CF
		public void RemoveListener(UnityAction<T0> call)
		{
			base.RemoveListener(call.Target, call.Method);
		}

		// Token: 0x06001D9F RID: 7583 RVA: 0x00030108 File Offset: 0x0002E308
		protected override MethodInfo FindMethod_Impl(string name, Type targetObjType)
		{
			return UnityEventBase.GetValidMethodInfo(targetObjType, name, new Type[] { typeof(T0) });
		}

		// Token: 0x06001DA0 RID: 7584 RVA: 0x00030134 File Offset: 0x0002E334
		internal override BaseInvokableCall GetDelegate(object target, MethodInfo theFunction)
		{
			return new InvokableCall<T0>(target, theFunction);
		}

		// Token: 0x06001DA1 RID: 7585 RVA: 0x00030150 File Offset: 0x0002E350
		private static BaseInvokableCall GetDelegate(UnityAction<T0> action)
		{
			return new InvokableCall<T0>(action);
		}

		// Token: 0x06001DA2 RID: 7586 RVA: 0x00030168 File Offset: 0x0002E368
		public void Invoke(T0 arg0)
		{
			List<BaseInvokableCall> list = base.PrepareInvoke();
			for (int i = 0; i < list.Count; i++)
			{
				InvokableCall<T0> invokableCall = list[i] as InvokableCall<T0>;
				bool flag = invokableCall != null;
				if (flag)
				{
					invokableCall.Invoke(arg0);
				}
				else
				{
					InvokableCall invokableCall2 = list[i] as InvokableCall;
					bool flag2 = invokableCall2 != null;
					if (flag2)
					{
						invokableCall2.Invoke();
					}
					else
					{
						BaseInvokableCall baseInvokableCall = list[i];
						bool flag3 = this.m_InvokeArray == null;
						if (flag3)
						{
							this.m_InvokeArray = new object[1];
						}
						this.m_InvokeArray[0] = arg0;
						baseInvokableCall.Invoke(this.m_InvokeArray);
					}
				}
			}
		}

		// Token: 0x040009AD RID: 2477
		private object[] m_InvokeArray = null;
	}
}
