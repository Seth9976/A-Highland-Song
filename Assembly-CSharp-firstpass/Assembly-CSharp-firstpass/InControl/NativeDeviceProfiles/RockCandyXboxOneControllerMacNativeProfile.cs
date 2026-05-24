using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x020001B4 RID: 436
	[Preserve]
	[NativeInputDeviceProfile]
	public class RockCandyXboxOneControllerMacNativeProfile : XboxOneDriverMacNativeProfile
	{
		// Token: 0x0600085C RID: 2140 RVA: 0x0003E1C4 File Offset: 0x0003C3C4
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Rock Candy Xbox One Controller";
			base.DeviceNotes = "Rock Candy Xbox One Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 3695,
					ProductID = 326
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 3695,
					ProductID = 582
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 3695,
					ProductID = 838
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 3695,
					ProductID = 719
				}
			};
		}
	}
}
