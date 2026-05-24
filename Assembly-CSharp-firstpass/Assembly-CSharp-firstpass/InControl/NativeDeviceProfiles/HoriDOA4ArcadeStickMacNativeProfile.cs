using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000147 RID: 327
	[Preserve]
	[NativeInputDeviceProfile]
	public class HoriDOA4ArcadeStickMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000782 RID: 1922 RVA: 0x00039CA8 File Offset: 0x00037EA8
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Hori DOA4 Arcade Stick";
			base.DeviceNotes = "Hori DOA4 Arcade Stick on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 3853,
					ProductID = 10
				}
			};
		}
	}
}
