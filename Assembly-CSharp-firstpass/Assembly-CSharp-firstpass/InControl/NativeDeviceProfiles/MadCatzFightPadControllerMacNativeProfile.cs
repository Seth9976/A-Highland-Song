using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000170 RID: 368
	[Preserve]
	[NativeInputDeviceProfile]
	public class MadCatzFightPadControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x060007D4 RID: 2004 RVA: 0x0003B370 File Offset: 0x00039570
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Mad Catz FightPad Controller";
			base.DeviceNotes = "Mad Catz FightPad Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 7085,
					ProductID = 61480
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 1848,
					ProductID = 18216
				}
			};
		}
	}
}
