using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x0200017F RID: 383
	[Preserve]
	[NativeInputDeviceProfile]
	public class MadCatzSaitekAV8R02MacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x060007F2 RID: 2034 RVA: 0x0003BAF4 File Offset: 0x00039CF4
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Mad Catz Saitek AV8R02";
			base.DeviceNotes = "Mad Catz Saitek AV8R02 on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 1848,
					ProductID = 52009
				}
			};
		}
	}
}
