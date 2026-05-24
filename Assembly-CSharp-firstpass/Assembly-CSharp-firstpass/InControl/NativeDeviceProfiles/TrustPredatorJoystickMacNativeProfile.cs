using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x020001BC RID: 444
	[Preserve]
	[NativeInputDeviceProfile]
	public class TrustPredatorJoystickMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x0600086C RID: 2156 RVA: 0x0003E6E4 File Offset: 0x0003C8E4
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Trust Predator Joystick";
			base.DeviceNotes = "Trust Predator Joystick on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 2064,
					ProductID = 3
				}
			};
		}
	}
}
