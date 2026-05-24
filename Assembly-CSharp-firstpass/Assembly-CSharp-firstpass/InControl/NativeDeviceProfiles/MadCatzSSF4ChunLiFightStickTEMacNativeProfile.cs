using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000184 RID: 388
	[Preserve]
	[NativeInputDeviceProfile]
	public class MadCatzSSF4ChunLiFightStickTEMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x060007FC RID: 2044 RVA: 0x0003BD60 File Offset: 0x00039F60
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Mad Catz SSF4 Chun-Li Fight Stick TE";
			base.DeviceNotes = "Mad Catz SSF4 Chun-Li Fight Stick TE on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 7085,
					ProductID = 61501
				}
			};
		}
	}
}
