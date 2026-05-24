using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000176 RID: 374
	[Preserve]
	[NativeInputDeviceProfile]
	public class MadCatzInnoControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x060007E0 RID: 2016 RVA: 0x0003B698 File Offset: 0x00039898
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Mad Catz Inno Controller";
			base.DeviceNotes = "Mad Catz Inno Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 1848,
					ProductID = 62465
				}
			};
		}
	}
}
