using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000156 RID: 342
	[Preserve]
	[NativeInputDeviceProfile]
	public class HoriRealArcadeProEXSEMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x060007A0 RID: 1952 RVA: 0x0003A560 File Offset: 0x00038760
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Hori Real Arcade Pro EX SE";
			base.DeviceNotes = "Hori Real Arcade Pro EX SE on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 3853,
					ProductID = 22
				}
			};
		}
	}
}
