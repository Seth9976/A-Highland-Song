using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x020001B5 RID: 437
	[Preserve]
	[NativeInputDeviceProfile]
	public class SaitekXbox360ControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x0600085E RID: 2142 RVA: 0x0003E300 File Offset: 0x0003C500
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Saitek Xbox 360 Controller";
			base.DeviceNotes = "Saitek Xbox 360 Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 1848,
					ProductID = 51970
				}
			};
		}
	}
}
