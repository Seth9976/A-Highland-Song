using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x020001A2 RID: 418
	[Preserve]
	[NativeInputDeviceProfile]
	public class PowerASpectraIlluminatedControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000838 RID: 2104 RVA: 0x0003D758 File Offset: 0x0003B958
		public override void Define()
		{
			base.Define();
			base.DeviceName = "PowerA Spectra Illuminated Controller";
			base.DeviceNotes = "PowerA Spectra Illuminated Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 9414,
					ProductID = 21546
				}
			};
		}
	}
}
