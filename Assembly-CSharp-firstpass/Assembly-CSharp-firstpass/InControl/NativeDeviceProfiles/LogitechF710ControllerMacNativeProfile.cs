using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000168 RID: 360
	[Preserve]
	[NativeInputDeviceProfile]
	public class LogitechF710ControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x060007C4 RID: 1988 RVA: 0x0003AED0 File Offset: 0x000390D0
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Logitech F710 Controller";
			base.DeviceNotes = "Logitech F710 Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 1133,
					ProductID = 49695
				}
			};
		}
	}
}
