using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x020001A6 RID: 422
	[Preserve]
	[NativeInputDeviceProfile]
	public class RazerAtroxArcadeStickMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000840 RID: 2112 RVA: 0x0003D948 File Offset: 0x0003BB48
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Razer Atrox Arcade Stick";
			base.DeviceNotes = "Razer Atrox Arcade Stick on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 5426,
					ProductID = 2560
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 9414,
					ProductID = 20480
				}
			};
		}
	}
}
