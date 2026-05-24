using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000179 RID: 377
	[Preserve]
	[NativeInputDeviceProfile]
	public class MadCatzMicroConControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x060007E6 RID: 2022 RVA: 0x0003B80C File Offset: 0x00039A0C
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Mad Catz MicroCon Controller";
			base.DeviceNotes = "Mad Catz MicroCon Controller on Mac";
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
