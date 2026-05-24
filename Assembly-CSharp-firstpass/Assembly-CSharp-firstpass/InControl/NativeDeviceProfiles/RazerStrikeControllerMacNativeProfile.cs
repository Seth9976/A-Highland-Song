using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x020001AA RID: 426
	[Preserve]
	[NativeInputDeviceProfile]
	public class RazerStrikeControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000848 RID: 2120 RVA: 0x0003DC38 File Offset: 0x0003BE38
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Razer Strike Controller";
			base.DeviceNotes = "Razer Strike Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 5769,
					ProductID = 1
				}
			};
		}
	}
}
