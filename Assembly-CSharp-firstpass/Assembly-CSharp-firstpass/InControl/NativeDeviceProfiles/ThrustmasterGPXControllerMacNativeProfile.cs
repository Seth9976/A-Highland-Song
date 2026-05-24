using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x020001B9 RID: 441
	[Preserve]
	[NativeInputDeviceProfile]
	public class ThrustmasterGPXControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000866 RID: 2150 RVA: 0x0003E530 File Offset: 0x0003C730
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Thrustmaster GPX Controller";
			base.DeviceNotes = "Thrustmaster GPX Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 1103,
					ProductID = 45862
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 9414,
					ProductID = 23298
				}
			};
		}
	}
}
