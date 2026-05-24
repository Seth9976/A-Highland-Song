using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x0200016E RID: 366
	[Preserve]
	[NativeInputDeviceProfile]
	public class MadCatzCODControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x060007D0 RID: 2000 RVA: 0x0003B1B8 File Offset: 0x000393B8
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Mad Catz COD Controller";
			base.DeviceNotes = "Mad Catz COD Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 7085,
					ProductID = 61477
				}
			};
		}
	}
}
