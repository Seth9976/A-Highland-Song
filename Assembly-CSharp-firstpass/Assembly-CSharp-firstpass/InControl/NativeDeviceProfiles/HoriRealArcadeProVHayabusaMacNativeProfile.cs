using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000159 RID: 345
	[Preserve]
	[NativeInputDeviceProfile]
	public class HoriRealArcadeProVHayabusaMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x060007A6 RID: 1958 RVA: 0x0003A6D4 File Offset: 0x000388D4
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Hori Real Arcade Pro V Hayabusa";
			base.DeviceNotes = "Hori Real Arcade Pro V Hayabusa on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 3853,
					ProductID = 216
				}
			};
		}
	}
}
