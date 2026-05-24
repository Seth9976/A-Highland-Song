using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000165 RID: 357
	[Preserve]
	[NativeInputDeviceProfile]
	public class LogitechDriveFXRacingWheelMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x060007BE RID: 1982 RVA: 0x0003AD1C File Offset: 0x00038F1C
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Logitech DriveFX Racing Wheel";
			base.DeviceNotes = "Logitech DriveFX Racing Wheel on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 1133,
					ProductID = 51875
				}
			};
		}
	}
}
