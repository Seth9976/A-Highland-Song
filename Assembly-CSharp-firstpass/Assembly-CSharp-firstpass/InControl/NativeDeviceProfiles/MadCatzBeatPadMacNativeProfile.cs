using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x0200016C RID: 364
	[Preserve]
	[NativeInputDeviceProfile]
	public class MadCatzBeatPadMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x060007CC RID: 1996 RVA: 0x0003B0C0 File Offset: 0x000392C0
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Mad Catz Beat Pad";
			base.DeviceNotes = "Mad Catz Beat Pad on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 1848,
					ProductID = 18240
				}
			};
		}
	}
}
