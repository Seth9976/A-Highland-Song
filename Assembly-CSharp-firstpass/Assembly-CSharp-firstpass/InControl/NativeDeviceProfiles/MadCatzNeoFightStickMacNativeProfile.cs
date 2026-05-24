using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x0200017C RID: 380
	[Preserve]
	[NativeInputDeviceProfile]
	public class MadCatzNeoFightStickMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x060007EC RID: 2028 RVA: 0x0003B980 File Offset: 0x00039B80
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Mad Catz Neo Fight Stick";
			base.DeviceNotes = "Mad Catz Neo Fight Stick on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 7085,
					ProductID = 61498
				}
			};
		}
	}
}
