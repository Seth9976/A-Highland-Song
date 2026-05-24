using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000149 RID: 329
	[Preserve]
	[NativeInputDeviceProfile]
	public class HoriEX2ControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000786 RID: 1926 RVA: 0x00039DA0 File Offset: 0x00037FA0
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Hori EX2 Controller";
			base.DeviceNotes = "Hori EX2 Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 3853,
					ProductID = 13
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 7085,
					ProductID = 62721
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 9414,
					ProductID = 21760
				}
			};
		}
	}
}
