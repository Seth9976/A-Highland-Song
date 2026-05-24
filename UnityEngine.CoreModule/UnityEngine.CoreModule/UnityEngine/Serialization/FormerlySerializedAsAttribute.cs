using System;
using UnityEngine.Scripting;

namespace UnityEngine.Serialization
{
	// Token: 0x020002CD RID: 717
	[AttributeUsage(256, AllowMultiple = true, Inherited = false)]
	[RequiredByNativeCode]
	public class FormerlySerializedAsAttribute : Attribute
	{
		// Token: 0x06001DC4 RID: 7620 RVA: 0x0003068C File Offset: 0x0002E88C
		public FormerlySerializedAsAttribute(string oldName)
		{
			this.m_oldName = oldName;
		}

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x06001DC5 RID: 7621 RVA: 0x000306A0 File Offset: 0x0002E8A0
		public string oldName
		{
			get
			{
				return this.m_oldName;
			}
		}

		// Token: 0x040009B1 RID: 2481
		private string m_oldName;
	}
}
