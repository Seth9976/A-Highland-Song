using System;

namespace InControl
{
	// Token: 0x0200004D RID: 77
	public interface IKeyboardProvider
	{
		// Token: 0x0600039C RID: 924
		void Setup();

		// Token: 0x0600039D RID: 925
		void Reset();

		// Token: 0x0600039E RID: 926
		void Update();

		// Token: 0x0600039F RID: 927
		bool AnyKeyIsPressed();

		// Token: 0x060003A0 RID: 928
		bool GetKeyIsPressed(Key control);

		// Token: 0x060003A1 RID: 929
		string GetNameForKey(Key control);
	}
}
