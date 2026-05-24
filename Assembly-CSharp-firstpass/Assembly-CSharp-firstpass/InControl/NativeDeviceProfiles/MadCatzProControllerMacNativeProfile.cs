using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x0200017E RID: 382
	[Preserve]
	[NativeInputDeviceProfile]
	public class MadCatzProControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x060007F0 RID: 2032 RVA: 0x0003BA78 File Offset: 0x00039C78
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Mad Catz Pro Controller";
			base.DeviceNotes = "Mad Catz Pro Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 1848,
					ProductID = 18214
				}
			};
		}
	}
}
