using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x0200017D RID: 381
	[Preserve]
	[NativeInputDeviceProfile]
	public class MadCatzPortableDrumMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x060007EE RID: 2030 RVA: 0x0003B9FC File Offset: 0x00039BFC
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Mad Catz Portable Drum";
			base.DeviceNotes = "Mad Catz Portable Drum on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 1848,
					ProductID = 39025
				}
			};
		}
	}
}
