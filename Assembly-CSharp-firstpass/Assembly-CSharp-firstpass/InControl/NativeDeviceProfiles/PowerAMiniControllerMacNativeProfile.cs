using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x0200019F RID: 415
	[Preserve]
	[NativeInputDeviceProfile]
	public class PowerAMiniControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000832 RID: 2098 RVA: 0x0003D564 File Offset: 0x0003B764
		public override void Define()
		{
			base.Define();
			base.DeviceName = "PowerA Mini Controller";
			base.DeviceNotes = "PowerA Mini Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 9414,
					ProductID = 21530
				}
			};
		}
	}
}
