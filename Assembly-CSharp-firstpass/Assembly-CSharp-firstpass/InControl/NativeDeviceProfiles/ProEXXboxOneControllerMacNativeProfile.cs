using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x020001A4 RID: 420
	[Preserve]
	[NativeInputDeviceProfile]
	public class ProEXXboxOneControllerMacNativeProfile : XboxOneDriverMacNativeProfile
	{
		// Token: 0x0600083C RID: 2108 RVA: 0x0003D850 File Offset: 0x0003BA50
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Pro EX Xbox One Controller";
			base.DeviceNotes = "Pro EX Xbox One Controller on Mac";
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
