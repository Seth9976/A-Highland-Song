using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000164 RID: 356
	[Preserve]
	[NativeInputDeviceProfile]
	public class LogitechControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x060007BC RID: 1980 RVA: 0x0003ACA0 File Offset: 0x00038EA0
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Logitech Controller";
			base.DeviceNotes = "Logitech Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 1133,
					ProductID = 62209
				}
			};
		}
	}
}
