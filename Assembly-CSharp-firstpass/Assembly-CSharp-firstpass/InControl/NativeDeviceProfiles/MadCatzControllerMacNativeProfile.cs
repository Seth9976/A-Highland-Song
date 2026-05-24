using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x0200016F RID: 367
	[Preserve]
	[NativeInputDeviceProfile]
	public class MadCatzControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x060007D2 RID: 2002 RVA: 0x0003B234 File Offset: 0x00039434
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Mad Catz Controller";
			base.DeviceNotes = "Mad Catz Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 1848,
					ProductID = 18198
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 7085,
					ProductID = 63746
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 7085,
					ProductID = 61642
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 1848,
					ProductID = 672
				}
			};
		}
	}
}
