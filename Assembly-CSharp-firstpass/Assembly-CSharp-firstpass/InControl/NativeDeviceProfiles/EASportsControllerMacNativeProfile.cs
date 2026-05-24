using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x0200013C RID: 316
	[Preserve]
	[NativeInputDeviceProfile]
	public class EASportsControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x0600076C RID: 1900 RVA: 0x00039558 File Offset: 0x00037758
		public override void Define()
		{
			base.Define();
			base.DeviceName = "EA Sports Controller";
			base.DeviceNotes = "EA Sports Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 3695,
					ProductID = 305
				}
			};
		}
	}
}
