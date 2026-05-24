using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x020001A3 RID: 419
	[Preserve]
	[NativeInputDeviceProfile]
	public class ProEXXbox360ControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x0600083A RID: 2106 RVA: 0x0003D7D4 File Offset: 0x0003B9D4
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Pro EX Xbox 360 Controller";
			base.DeviceNotes = "Pro EX Xbox 360 Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 9414,
					ProductID = 21258
				}
			};
		}
	}
}
