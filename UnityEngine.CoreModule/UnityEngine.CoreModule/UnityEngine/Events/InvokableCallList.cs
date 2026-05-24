using System;
using System.Collections.Generic;
using System.Reflection;

namespace UnityEngine.Events
{
	// Token: 0x020002C1 RID: 705
	internal class InvokableCallList
	{
		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x06001D6D RID: 7533 RVA: 0x0002F8BC File Offset: 0x0002DABC
		public int Count
		{
			get
			{
				return this.m_PersistentCalls.Count + this.m_RuntimeCalls.Count;
			}
		}

		// Token: 0x06001D6E RID: 7534 RVA: 0x0002F8E5 File Offset: 0x0002DAE5
		public void AddPersistentInvokableCall(BaseInvokableCall call)
		{
			this.m_PersistentCalls.Add(call);
			this.m_NeedsUpdate = true;
		}

		// Token: 0x06001D6F RID: 7535 RVA: 0x0002F8FC File Offset: 0x0002DAFC
		public void AddListener(BaseInvokableCall call)
		{
			this.m_RuntimeCalls.Add(call);
			this.m_NeedsUpdate = true;
		}

		// Token: 0x06001D70 RID: 7536 RVA: 0x0002F914 File Offset: 0x0002DB14
		public void RemoveListener(object targetObj, MethodInfo method)
		{
			List<BaseInvokableCall> list = new List<BaseInvokableCall>();
			for (int i = 0; i < this.m_RuntimeCalls.Count; i++)
			{
				bool flag = this.m_RuntimeCalls[i].Find(targetObj, method);
				if (flag)
				{
					list.Add(this.m_RuntimeCalls[i]);
				}
			}
			this.m_RuntimeCalls.RemoveAll(new Predicate<BaseInvokableCall>(list.Contains));
			List<BaseInvokableCall> list2 = new List<BaseInvokableCall>(this.m_PersistentCalls.Count + this.m_RuntimeCalls.Count);
			list2.AddRange(this.m_PersistentCalls);
			list2.AddRange(this.m_RuntimeCalls);
			this.m_ExecutingCalls = list2;
			this.m_NeedsUpdate = false;
		}

		// Token: 0x06001D71 RID: 7537 RVA: 0x0002F9D0 File Offset: 0x0002DBD0
		public void Clear()
		{
			this.m_RuntimeCalls.Clear();
			List<BaseInvokableCall> list = new List<BaseInvokableCall>(this.m_PersistentCalls);
			this.m_ExecutingCalls = list;
			this.m_NeedsUpdate = false;
		}

		// Token: 0x06001D72 RID: 7538 RVA: 0x0002FA04 File Offset: 0x0002DC04
		public void ClearPersistent()
		{
			this.m_PersistentCalls.Clear();
			List<BaseInvokableCall> list = new List<BaseInvokableCall>(this.m_RuntimeCalls);
			this.m_ExecutingCalls = list;
			this.m_NeedsUpdate = false;
		}

		// Token: 0x06001D73 RID: 7539 RVA: 0x0002FA38 File Offset: 0x0002DC38
		public List<BaseInvokableCall> PrepareInvoke()
		{
			bool needsUpdate = this.m_NeedsUpdate;
			if (needsUpdate)
			{
				this.m_ExecutingCalls.Clear();
				this.m_ExecutingCalls.AddRange(this.m_PersistentCalls);
				this.m_ExecutingCalls.AddRange(this.m_RuntimeCalls);
				this.m_NeedsUpdate = false;
			}
			return this.m_ExecutingCalls;
		}

		// Token: 0x040009A5 RID: 2469
		private readonly List<BaseInvokableCall> m_PersistentCalls = new List<BaseInvokableCall>();

		// Token: 0x040009A6 RID: 2470
		private readonly List<BaseInvokableCall> m_RuntimeCalls = new List<BaseInvokableCall>();

		// Token: 0x040009A7 RID: 2471
		private List<BaseInvokableCall> m_ExecutingCalls = new List<BaseInvokableCall>();

		// Token: 0x040009A8 RID: 2472
		private bool m_NeedsUpdate = true;
	}
}
