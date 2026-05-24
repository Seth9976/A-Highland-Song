using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000150 RID: 336
	[Preserve]
	[NativeInputDeviceProfile]
	public class HoriFightStickMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000794 RID: 1940 RVA: 0x0003A278 File Offset: 0x00038478
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Hori Fight Stick";
			base.DeviceNotes = "Hori Fight Stick on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 3853,
					ProductID = 13
				}
			};
		}
	}
}
