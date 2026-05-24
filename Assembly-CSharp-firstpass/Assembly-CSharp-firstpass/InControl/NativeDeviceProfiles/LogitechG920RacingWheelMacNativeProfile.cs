using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000169 RID: 361
	[Preserve]
	[NativeInputDeviceProfile]
	public class LogitechG920RacingWheelMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x060007C6 RID: 1990 RVA: 0x0003AF4C File Offset: 0x0003914C
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Logitech G920 Racing Wheel";
			base.DeviceNotes = "Logitech G920 Racing Wheel on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 1133,
					ProductID = 49761
				}
			};
		}
	}
}
