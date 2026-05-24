using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000171 RID: 369
	[Preserve]
	[NativeInputDeviceProfile]
	public class MadCatzFightPadMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x060007D6 RID: 2006 RVA: 0x0003B42C File Offset: 0x0003962C
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Mad Catz FightPad";
			base.DeviceNotes = "Mad Catz FightPad on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 7085,
					ProductID = 61486
				}
			};
		}
	}
}
