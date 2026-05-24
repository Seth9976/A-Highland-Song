using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000182 RID: 386
	[Preserve]
	[NativeInputDeviceProfile]
	public class MadCatzSF4FightStickTEMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x060007F8 RID: 2040 RVA: 0x0003BC68 File Offset: 0x00039E68
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Mad Catz SF4 Fight Stick TE";
			base.DeviceNotes = "Mad Catz SF4 Fight Stick TE on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 1848,
					ProductID = 18232
				}
			};
		}
	}
}
