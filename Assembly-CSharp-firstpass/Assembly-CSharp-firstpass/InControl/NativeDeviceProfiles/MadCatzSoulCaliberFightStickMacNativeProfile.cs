using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000183 RID: 387
	[Preserve]
	[NativeInputDeviceProfile]
	public class MadCatzSoulCaliberFightStickMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x060007FA RID: 2042 RVA: 0x0003BCE4 File Offset: 0x00039EE4
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Mad Catz Soul Caliber Fight Stick";
			base.DeviceNotes = "Mad Catz Soul Caliber Fight Stick on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 7085,
					ProductID = 61503
				}
			};
		}
	}
}
