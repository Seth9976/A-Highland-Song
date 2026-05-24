using System;

namespace InControl
{
	// Token: 0x02000030 RID: 48
	public interface IInputControl
	{
		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600019A RID: 410
		bool HasChanged { get; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600019B RID: 411
		bool IsPressed { get; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600019C RID: 412
		bool WasPressed { get; }

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600019D RID: 413
		bool WasReleased { get; }

		// Token: 0x0600019E RID: 414
		void ClearInputState();
	}
}
