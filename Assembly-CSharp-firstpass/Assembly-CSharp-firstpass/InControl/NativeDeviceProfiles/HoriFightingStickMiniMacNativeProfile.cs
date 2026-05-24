using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x0200014E RID: 334
	[Preserve]
	[NativeInputDeviceProfile]
	public class HoriFightingStickMiniMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000790 RID: 1936 RVA: 0x0003A140 File Offset: 0x00038340
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Hori Fighting Stick Mini";
			base.DeviceNotes = "Hori Fighting Stick Mini on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 3853,
					ProductID = 237
				}
			};
		}
	}
}
