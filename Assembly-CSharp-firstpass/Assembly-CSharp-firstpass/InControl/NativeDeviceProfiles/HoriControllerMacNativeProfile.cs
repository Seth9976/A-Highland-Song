using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000146 RID: 326
	[Preserve]
	[NativeInputDeviceProfile]
	public class HoriControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000780 RID: 1920 RVA: 0x00039B30 File Offset: 0x00037D30
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Hori Controller";
			base.DeviceNotes = "Hori Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 3853,
					ProductID = 220
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 3853,
					ProductID = 103
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 3853,
					ProductID = 256
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 7085,
					ProductID = 21760
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 7085,
					ProductID = 654
				}
			};
		}
	}
}
