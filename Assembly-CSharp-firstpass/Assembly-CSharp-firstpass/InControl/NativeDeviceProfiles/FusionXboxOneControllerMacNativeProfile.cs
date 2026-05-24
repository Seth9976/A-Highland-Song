using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x0200013E RID: 318
	[Preserve]
	[NativeInputDeviceProfile]
	public class FusionXboxOneControllerMacNativeProfile : XboxOneDriverMacNativeProfile
	{
		// Token: 0x06000770 RID: 1904 RVA: 0x00039650 File Offset: 0x00037850
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Fusion Xbox One Controller";
			base.DeviceNotes = "Fusion Xbox One Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 9414,
					ProductID = 21786
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 9414,
					ProductID = 22042
				}
			};
		}
	}
}
