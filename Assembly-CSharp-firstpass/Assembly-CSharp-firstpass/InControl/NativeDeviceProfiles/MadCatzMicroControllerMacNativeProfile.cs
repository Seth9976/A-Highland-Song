using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x0200017A RID: 378
	[Preserve]
	[NativeInputDeviceProfile]
	public class MadCatzMicroControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x060007E8 RID: 2024 RVA: 0x0003B888 File Offset: 0x00039A88
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Mad Catz Micro Controller";
			base.DeviceNotes = "Mad Catz Micro Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 1848,
					ProductID = 18230
				}
			};
		}
	}
}
