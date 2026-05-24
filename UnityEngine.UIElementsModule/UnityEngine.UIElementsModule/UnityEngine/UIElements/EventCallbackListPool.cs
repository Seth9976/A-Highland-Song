using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x020001D6 RID: 470
	internal class EventCallbackListPool
	{
		// Token: 0x06000EAE RID: 3758 RVA: 0x0003B6D0 File Offset: 0x000398D0
		public EventCallbackList Get(EventCallbackList initializer)
		{
			bool flag = this.m_Stack.Count == 0;
			EventCallbackList eventCallbackList;
			if (flag)
			{
				bool flag2 = initializer != null;
				if (flag2)
				{
					eventCallbackList = new EventCallbackList(initializer);
				}
				else
				{
					eventCallbackList = new EventCallbackList();
				}
			}
			else
			{
				eventCallbackList = this.m_Stack.Pop();
				bool flag3 = initializer != null;
				if (flag3)
				{
					eventCallbackList.AddRange(initializer);
				}
			}
			return eventCallbackList;
		}

		// Token: 0x06000EAF RID: 3759 RVA: 0x0003B730 File Offset: 0x00039930
		public void Release(EventCallbackList element)
		{
			element.Clear();
			this.m_Stack.Push(element);
		}

		// Token: 0x040006BB RID: 1723
		private readonly Stack<EventCallbackList> m_Stack = new Stack<EventCallbackList>();
	}
}
