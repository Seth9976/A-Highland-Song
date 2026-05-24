using System;
using System.Collections.Generic;

namespace InControl
{
	// Token: 0x02000043 RID: 67
	public abstract class InputDeviceManager
	{
		// Token: 0x060002C3 RID: 707
		public abstract void Update(ulong updateTick, float deltaTime);

		// Token: 0x060002C4 RID: 708 RVA: 0x0000A143 File Offset: 0x00008343
		public virtual void Destroy()
		{
		}

		// Token: 0x040002FE RID: 766
		protected readonly List<InputDevice> devices = new List<InputDevice>();
	}
}
