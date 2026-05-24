using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000166 RID: 358
	[Preserve]
	[NativeInputDeviceProfile]
	public class LogitechF310ControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x060007C0 RID: 1984 RVA: 0x0003AD98 File Offset: 0x00038F98
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Logitech F310 Controller";
			base.DeviceNotes = "Logitech F310 Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 1133,
					ProductID = 49693
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 1133,
					ProductID = 49686
				}
			};
		}
	}
}
