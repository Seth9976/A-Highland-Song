using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x020001B8 RID: 440
	[Preserve]
	[NativeInputDeviceProfile]
	public class ThrustMasterFerrari458SpiderRacingWheelMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000864 RID: 2148 RVA: 0x0003E4B4 File Offset: 0x0003C6B4
		public override void Define()
		{
			base.Define();
			base.DeviceName = "ThrustMaster Ferrari 458 Spider Racing Wheel";
			base.DeviceNotes = "ThrustMaster Ferrari 458 Spider Racing Wheel on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 1103,
					ProductID = 46705
				}
			};
		}
	}
}
