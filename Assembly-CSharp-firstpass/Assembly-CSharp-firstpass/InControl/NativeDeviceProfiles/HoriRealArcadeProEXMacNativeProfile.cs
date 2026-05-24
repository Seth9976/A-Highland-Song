using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000154 RID: 340
	[Preserve]
	[NativeInputDeviceProfile]
	public class HoriRealArcadeProEXMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x0600079C RID: 1948 RVA: 0x0003A468 File Offset: 0x00038668
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Hori Real Arcade Pro EX";
			base.DeviceNotes = "Hori Real Arcade Pro EX on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 7085,
					ProductID = 62724
				}
			};
		}
	}
}
