using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x0200015C RID: 348
	[Preserve]
	[NativeInputDeviceProfile]
	public class HoriRealArcadeProVXSAMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x060007AC RID: 1964 RVA: 0x0003A884 File Offset: 0x00038A84
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Hori Real Arcade Pro VX SA";
			base.DeviceNotes = "Hori Real Arcade Pro VX SA on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 7085,
					ProductID = 62722
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 9414,
					ProductID = 21761
				}
			};
		}
	}
}
