using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x020001BF RID: 447
	[Preserve]
	[NativeInputDeviceProfile]
	public class Xbox360ProEXControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000872 RID: 2162 RVA: 0x0003E854 File Offset: 0x0003CA54
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Xbox 360 Pro EX Controller";
			base.DeviceNotes = "Xbox 360 Pro EX Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 8406,
					ProductID = 10271
				}
			};
		}
	}
}
