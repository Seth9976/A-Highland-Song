using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000161 RID: 353
	[Preserve]
	[NativeInputDeviceProfile]
	public class JoytekXbox360ControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x060007B6 RID: 1974 RVA: 0x0003AB30 File Offset: 0x00038D30
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Joytek Xbox 360 Controller";
			base.DeviceNotes = "Joytek Xbox 360 Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 5678,
					ProductID = 48879
				}
			};
		}
	}
}
