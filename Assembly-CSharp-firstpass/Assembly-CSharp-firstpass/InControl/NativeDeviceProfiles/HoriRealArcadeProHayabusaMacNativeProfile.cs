using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000157 RID: 343
	[Preserve]
	[NativeInputDeviceProfile]
	public class HoriRealArcadeProHayabusaMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x060007A2 RID: 1954 RVA: 0x0003A5DC File Offset: 0x000387DC
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Hori Real Arcade Pro Hayabusa";
			base.DeviceNotes = "Hori Real Arcade Pro Hayabusa on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 3853,
					ProductID = 99
				}
			};
		}
	}
}
