using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000167 RID: 359
	[Preserve]
	[NativeInputDeviceProfile]
	public class LogitechF510ControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x060007C2 RID: 1986 RVA: 0x0003AE54 File Offset: 0x00039054
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Logitech F510 Controller";
			base.DeviceNotes = "Logitech F510 Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 1133,
					ProductID = 49694
				}
			};
		}
	}
}
