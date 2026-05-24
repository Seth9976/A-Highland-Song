using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000155 RID: 341
	[Preserve]
	[NativeInputDeviceProfile]
	public class HoriRealArcadeProEXPremiumVLXMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x0600079E RID: 1950 RVA: 0x0003A4E4 File Offset: 0x000386E4
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Hori Real Arcade Pro EX Premium VLX";
			base.DeviceNotes = "Hori Real Arcade Pro EX Premium VLX on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 7085,
					ProductID = 62726
				}
			};
		}
	}
}
