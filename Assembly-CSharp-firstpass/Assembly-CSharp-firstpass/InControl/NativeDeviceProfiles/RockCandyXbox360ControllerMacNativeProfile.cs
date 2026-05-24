using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x020001B3 RID: 435
	[Preserve]
	[NativeInputDeviceProfile]
	public class RockCandyXbox360ControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x0600085A RID: 2138 RVA: 0x0003E0C8 File Offset: 0x0003C2C8
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Rock Candy Xbox 360 Controller";
			base.DeviceNotes = "Rock Candy Xbox 360 Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 3695,
					ProductID = 543
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 9414,
					ProductID = 64254
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 3695,
					ProductID = 338
				}
			};
		}
	}
}
