using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x020001A8 RID: 424
	[Preserve]
	[NativeInputDeviceProfile]
	public class RazerOnzaTEControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000844 RID: 2116 RVA: 0x0003DAC0 File Offset: 0x0003BCC0
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Razer Onza TE Controller";
			base.DeviceNotes = "Razer Onza TE Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 7085,
					ProductID = 64768
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 5769,
					ProductID = 64768
				}
			};
		}
	}
}
