using System;

namespace UnityEngine.Playables
{
	// Token: 0x02000437 RID: 1079
	public class Notification : INotification
	{
		// Token: 0x06002578 RID: 9592 RVA: 0x0003F315 File Offset: 0x0003D515
		public Notification(string name)
		{
			this.id = new PropertyName(name);
		}

		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x06002579 RID: 9593 RVA: 0x0003F32B File Offset: 0x0003D52B
		public PropertyName id { get; }
	}
}
