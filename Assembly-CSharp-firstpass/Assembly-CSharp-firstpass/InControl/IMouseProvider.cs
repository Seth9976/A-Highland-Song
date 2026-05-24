using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x0200004F RID: 79
	public interface IMouseProvider
	{
		// Token: 0x060003AA RID: 938
		void Setup();

		// Token: 0x060003AB RID: 939
		void Reset();

		// Token: 0x060003AC RID: 940
		void Update();

		// Token: 0x060003AD RID: 941
		Vector2 GetPosition();

		// Token: 0x060003AE RID: 942
		float GetDeltaX();

		// Token: 0x060003AF RID: 943
		float GetDeltaY();

		// Token: 0x060003B0 RID: 944
		float GetDeltaScroll();

		// Token: 0x060003B1 RID: 945
		bool GetButtonIsPressed(Mouse control);

		// Token: 0x060003B2 RID: 946
		bool GetButtonWasPressed(Mouse control);

		// Token: 0x060003B3 RID: 947
		bool GetButtonWasReleased(Mouse control);

		// Token: 0x060003B4 RID: 948
		bool HasMousePresent();
	}
}
