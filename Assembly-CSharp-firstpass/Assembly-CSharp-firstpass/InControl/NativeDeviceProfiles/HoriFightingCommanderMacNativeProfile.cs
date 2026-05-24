using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x0200014B RID: 331
	[Preserve]
	[NativeInputDeviceProfile]
	public class HoriFightingCommanderMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x0600078A RID: 1930 RVA: 0x00039F14 File Offset: 0x00038114
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Hori Fighting Commander";
			base.DeviceNotes = "Hori Fighting Commander on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 3853,
					ProductID = 197
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 9414,
					ProductID = 21776
				}
			};
		}
	}
}
