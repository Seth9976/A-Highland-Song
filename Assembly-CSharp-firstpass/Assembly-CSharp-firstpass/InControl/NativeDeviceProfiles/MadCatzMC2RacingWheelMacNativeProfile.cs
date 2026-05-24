using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000178 RID: 376
	[Preserve]
	[NativeInputDeviceProfile]
	public class MadCatzMC2RacingWheelMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x060007E4 RID: 2020 RVA: 0x0003B790 File Offset: 0x00039990
		public override void Define()
		{
			base.Define();
			base.DeviceName = "MadCatz MC2 Racing Wheel";
			base.DeviceNotes = "MadCatz MC2 Racing Wheel on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 7085,
					ProductID = 61472
				}
			};
		}
	}
}
