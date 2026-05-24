using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Scripting;

namespace UnityEngine.Events
{
	// Token: 0x020002C4 RID: 708
	[Serializable]
	public class UnityEvent : UnityEventBase
	{
		// Token: 0x06001D91 RID: 7569 RVA: 0x0002FFAE File Offset: 0x0002E1AE
		[RequiredByNativeCode]
		public UnityEvent()
		{
		}

		// Token: 0x06001D92 RID: 7570 RVA: 0x0002FFBF File Offset: 0x0002E1BF
		public void AddListener(UnityAction call)
		{
			base.AddCall(UnityEvent.GetDelegate(call));
		}

		// Token: 0x06001D93 RID: 7571 RVA: 0x0002FFCF File Offset: 0x0002E1CF
		public void RemoveListener(UnityAction call)
		{
			base.RemoveListener(call.Target, call.Method);
		}

		// Token: 0x06001D94 RID: 7572 RVA: 0x0002FFE8 File Offset: 0x0002E1E8
		protected override MethodInfo FindMethod_Impl(string name, Type targetObjType)
		{
			return UnityEventBase.GetValidMethodInfo(targetObjType, name, new Type[0]);
		}

		// Token: 0x06001D95 RID: 7573 RVA: 0x00030008 File Offset: 0x0002E208
		internal override BaseInvokableCall GetDelegate(object target, MethodInfo theFunction)
		{
			return new InvokableCall(target, theFunction);
		}

		// Token: 0x06001D96 RID: 7574 RVA: 0x00030024 File Offset: 0x0002E224
		private static BaseInvokableCall GetDelegate(UnityAction action)
		{
			return new InvokableCall(action);
		}

		// Token: 0x06001D97 RID: 7575 RVA: 0x0003003C File Offset: 0x0002E23C
		public void Invoke()
		{
			List<BaseInvokableCall> list = base.PrepareInvoke();
			for (int i = 0; i < list.Count; i++)
			{
				InvokableCall invokableCall = list[i] as InvokableCall;
				bool flag = invokableCall != null;
				if (flag)
				{
					invokableCall.Invoke();
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
							this.m_InvokeArray = new object[0];
						}
						baseInvokableCall.Invoke(this.m_InvokeArray);
					}
				}
			}
		}

		// Token: 0x040009AC RID: 2476
		private object[] m_InvokeArray = null;
	}
}
