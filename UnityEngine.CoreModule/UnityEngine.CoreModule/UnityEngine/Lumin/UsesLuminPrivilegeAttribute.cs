using System;

namespace UnityEngine.Lumin
{
	// Token: 0x02000395 RID: 917
	[AttributeUsage(4, AllowMultiple = true)]
	public sealed class UsesLuminPrivilegeAttribute : Attribute
	{
		// Token: 0x06001EF0 RID: 7920 RVA: 0x000325F8 File Offset: 0x000307F8
		public UsesLuminPrivilegeAttribute(string privilege)
		{
			this.m_Privilege = privilege;
		}

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x06001EF1 RID: 7921 RVA: 0x0003260C File Offset: 0x0003080C
		public string privilege
		{
			get
			{
				return this.m_Privilege;
			}
		}

		// Token: 0x04000A31 RID: 2609
		private readonly string m_Privilege;
	}
}
