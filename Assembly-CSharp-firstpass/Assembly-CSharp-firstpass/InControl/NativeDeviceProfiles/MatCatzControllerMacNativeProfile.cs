using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000186 RID: 390
	[Preserve]
	[NativeInputDeviceProfile]
	public class MatCatzControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000800 RID: 2048 RVA: 0x0003BE58 File Offset: 0x0003A058
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Mat Catz Controller";
			base.DeviceNotes = "Mat Catz Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 7085,
					ProductID = 61462
				}
			};
		}
	}
}
