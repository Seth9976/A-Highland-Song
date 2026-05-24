using System;

namespace UnityEngine
{
	// Token: 0x020001EF RID: 495
	public sealed class AddComponentMenu : Attribute
	{
		// Token: 0x0600164D RID: 5709 RVA: 0x00023CA6 File Offset: 0x00021EA6
		public AddComponentMenu(string menuName)
		{
			this.m_AddComponentMenu = menuName;
			this.m_Ordering = 0;
		}

		// Token: 0x0600164E RID: 5710 RVA: 0x00023CBE File Offset: 0x00021EBE
		public AddComponentMenu(string menuName, int order)
		{
			this.m_AddComponentMenu = menuName;
			this.m_Ordering = order;
		}

		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x0600164F RID: 5711 RVA: 0x00023CD8 File Offset: 0x00021ED8
		public string componentMenu
		{
			get
			{
				return this.m_AddComponentMenu;
			}
		}

		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x06001650 RID: 5712 RVA: 0x00023CF0 File Offset: 0x00021EF0
		public int componentOrder
		{
			get
			{
				return this.m_Ordering;
			}
		}

		// Token: 0x040007CE RID: 1998
		private string m_AddComponentMenu;

		// Token: 0x040007CF RID: 1999
		private int m_Ordering;
	}
}
