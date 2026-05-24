using System;

namespace InControl
{
	// Token: 0x02000061 RID: 97
	public class TouchInputDevice : InputDevice
	{
		// Token: 0x0600046A RID: 1130 RVA: 0x0001085C File Offset: 0x0000EA5C
		public TouchInputDevice()
			: base("Touch Input Device", true)
		{
			base.DeviceClass = InputDeviceClass.TouchScreen;
		}
	}
}
