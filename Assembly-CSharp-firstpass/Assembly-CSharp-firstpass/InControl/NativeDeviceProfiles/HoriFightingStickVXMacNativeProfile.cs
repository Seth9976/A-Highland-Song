using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x0200014F RID: 335
	[Preserve]
	[NativeInputDeviceProfile]
	public class HoriFightingStickVXMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000792 RID: 1938 RVA: 0x0003A1BC File Offset: 0x000383BC
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Hori Fighting Stick VX";
			base.DeviceNotes = "Hori Fighting Stick VX on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 7085,
					ProductID = 62723
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 9414,
					ProductID = 21762
				}
			};
		}
	}
}
