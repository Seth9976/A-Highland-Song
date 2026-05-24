using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x020001EE RID: 494
	[AttributeUsage(4, AllowMultiple = true)]
	[RequiredByNativeCode]
	public sealed class RequireComponent : Attribute
	{
		// Token: 0x0600164A RID: 5706 RVA: 0x00023C5E File Offset: 0x00021E5E
		public RequireComponent(Type requiredComponent)
		{
			this.m_Type0 = requiredComponent;
		}

		// Token: 0x0600164B RID: 5707 RVA: 0x00023C6F File Offset: 0x00021E6F
		public RequireComponent(Type requiredComponent, Type requiredComponent2)
		{
			this.m_Type0 = requiredComponent;
			this.m_Type1 = requiredComponent2;
		}

		// Token: 0x0600164C RID: 5708 RVA: 0x00023C87 File Offset: 0x00021E87
		public RequireComponent(Type requiredComponent, Type requiredComponent2, Type requiredComponent3)
		{
			this.m_Type0 = requiredComponent;
			this.m_Type1 = requiredComponent2;
			this.m_Type2 = requiredComponent3;
		}

		// Token: 0x040007CB RID: 1995
		public Type m_Type0;

		// Token: 0x040007CC RID: 1996
		public Type m_Type1;

		// Token: 0x040007CD RID: 1997
		public Type m_Type2;
	}
}
