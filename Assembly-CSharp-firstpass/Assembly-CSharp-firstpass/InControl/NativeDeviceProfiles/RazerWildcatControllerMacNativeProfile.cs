using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x020001AB RID: 427
	[Preserve]
	[NativeInputDeviceProfile]
	public class RazerWildcatControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x0600084A RID: 2122 RVA: 0x0003DCB0 File Offset: 0x0003BEB0
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Razer Wildcat Controller";
			base.DeviceNotes = "Razer Wildcat Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 5426,
					ProductID = 2563
				}
			};
		}
	}
}
