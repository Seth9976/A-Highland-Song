using System;

namespace InControl
{
	// Token: 0x02000019 RID: 25
	public interface BindingSourceListener
	{
		// Token: 0x0600008E RID: 142
		void Reset();

		// Token: 0x0600008F RID: 143
		BindingSource Listen(BindingListenOptions listenOptions, InputDevice device);
	}
}
