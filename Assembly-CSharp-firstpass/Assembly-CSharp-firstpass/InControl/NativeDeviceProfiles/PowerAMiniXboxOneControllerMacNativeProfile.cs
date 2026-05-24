using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x020001A1 RID: 417
	[Preserve]
	[NativeInputDeviceProfile]
	public class PowerAMiniXboxOneControllerMacNativeProfile : XboxOneDriverMacNativeProfile
	{
		// Token: 0x06000836 RID: 2102 RVA: 0x0003D6DC File Offset: 0x0003B8DC
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Power A Mini Xbox One Controller";
			base.DeviceNotes = "Power A Mini Xbox One Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 9414,
					ProductID = 21562
				}
			};
		}
	}
}
