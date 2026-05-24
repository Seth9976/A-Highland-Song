using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000185 RID: 389
	[Preserve]
	[NativeInputDeviceProfile]
	public class MadCatzSSF4FightStickTEMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x060007FE RID: 2046 RVA: 0x0003BDDC File Offset: 0x00039FDC
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Mad Catz SSF4 Fight Stick TE";
			base.DeviceNotes = "Mad Catz SSF4 Fight Stick TE on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 1848,
					ProductID = 63288
				}
			};
		}
	}
}
