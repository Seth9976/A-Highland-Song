using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x0200018F RID: 399
	[Preserve]
	[NativeInputDeviceProfile]
	public class MLGControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000812 RID: 2066 RVA: 0x0003C5EC File Offset: 0x0003A7EC
		public override void Define()
		{
			base.Define();
			base.DeviceName = "MLG Controller";
			base.DeviceNotes = "MLG Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 7085,
					ProductID = 61475
				}
			};
		}
	}
}
