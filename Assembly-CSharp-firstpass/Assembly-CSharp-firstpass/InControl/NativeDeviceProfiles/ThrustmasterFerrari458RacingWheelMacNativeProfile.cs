using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x020001B7 RID: 439
	[Preserve]
	[NativeInputDeviceProfile]
	public class ThrustmasterFerrari458RacingWheelMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000862 RID: 2146 RVA: 0x0003E3F8 File Offset: 0x0003C5F8
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Thrustmaster Ferrari 458 Racing Wheel";
			base.DeviceNotes = "Thrustmaster Ferrari 458 Racing Wheel on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 9414,
					ProductID = 23296
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 9414,
					ProductID = 23299
				}
			};
		}
	}
}
