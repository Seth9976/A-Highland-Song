using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x020001B6 RID: 438
	[Preserve]
	[NativeInputDeviceProfile]
	public class ThrustMasterFerrari430RacingWheelMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000860 RID: 2144 RVA: 0x0003E37C File Offset: 0x0003C57C
		public override void Define()
		{
			base.Define();
			base.DeviceName = "ThrustMaster Ferrari 430 Racing Wheel";
			base.DeviceNotes = "ThrustMaster Ferrari 430 Racing Wheel on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 1103,
					ProductID = 46683
				}
			};
		}
	}
}
