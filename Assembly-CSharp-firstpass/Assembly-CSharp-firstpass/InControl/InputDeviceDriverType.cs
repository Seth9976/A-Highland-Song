using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000041 RID: 65
	public enum InputDeviceDriverType : ushort
	{
		// Token: 0x040002EA RID: 746
		Unknown,
		// Token: 0x040002EB RID: 747
		HID,
		// Token: 0x040002EC RID: 748
		USB,
		// Token: 0x040002ED RID: 749
		Bluetooth,
		// Token: 0x040002EE RID: 750
		[InspectorName("XInput")]
		XInput,
		// Token: 0x040002EF RID: 751
		[InspectorName("DirectInput")]
		DirectInput,
		// Token: 0x040002F0 RID: 752
		[InspectorName("RawInput")]
		RawInput,
		// Token: 0x040002F1 RID: 753
		[InspectorName("AppleGameController")]
		AppleGameController,
		// Token: 0x040002F2 RID: 754
		[InspectorName("SDLJoystick")]
		SDLJoystick,
		// Token: 0x040002F3 RID: 755
		[InspectorName("SDLController")]
		SDLController
	}
}
