using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x0200015B RID: 347
	[Preserve]
	[NativeInputDeviceProfile]
	public class HoriRealArcadeProVXMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x060007AA RID: 1962 RVA: 0x0003A808 File Offset: 0x00038A08
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Hori Real Arcade Pro VX";
			base.DeviceNotes = "Hori Real Arcade Pro VX on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 3853,
					ProductID = 27
				}
			};
		}
	}
}
