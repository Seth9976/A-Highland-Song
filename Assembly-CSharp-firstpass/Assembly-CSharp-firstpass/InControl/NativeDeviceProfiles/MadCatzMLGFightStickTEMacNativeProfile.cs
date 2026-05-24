using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x0200017B RID: 379
	[Preserve]
	[NativeInputDeviceProfile]
	public class MadCatzMLGFightStickTEMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x060007EA RID: 2026 RVA: 0x0003B904 File Offset: 0x00039B04
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Mad Catz MLG Fight Stick TE";
			base.DeviceNotes = "Mad Catz MLG Fight Stick TE on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 7085,
					ProductID = 61502
				}
			};
		}
	}
}
