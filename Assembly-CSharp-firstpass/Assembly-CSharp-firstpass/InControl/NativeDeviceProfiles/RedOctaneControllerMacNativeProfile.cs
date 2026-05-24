using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x020001AD RID: 429
	[Preserve]
	[NativeInputDeviceProfile]
	public class RedOctaneControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x0600084E RID: 2126 RVA: 0x0003DDA8 File Offset: 0x0003BFA8
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Red Octane Controller";
			base.DeviceNotes = "Red Octane Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 5168,
					ProductID = 63489
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 5168,
					ProductID = 672
				}
			};
		}
	}
}
