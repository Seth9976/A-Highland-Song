using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x020001A7 RID: 423
	[Preserve]
	[NativeInputDeviceProfile]
	public class RazerOnzaControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000842 RID: 2114 RVA: 0x0003DA04 File Offset: 0x0003BC04
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Razer Onza Controller";
			base.DeviceNotes = "Razer Onza Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 7085,
					ProductID = 64769
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 5769,
					ProductID = 64769
				}
			};
		}
	}
}
