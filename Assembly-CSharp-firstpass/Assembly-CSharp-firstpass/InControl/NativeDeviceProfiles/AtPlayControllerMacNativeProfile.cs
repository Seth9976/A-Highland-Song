using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000135 RID: 309
	[Preserve]
	[NativeInputDeviceProfile]
	public class AtPlayControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x0600075E RID: 1886 RVA: 0x00039174 File Offset: 0x00037374
		public override void Define()
		{
			base.Define();
			base.DeviceName = "At Play Controller";
			base.DeviceNotes = "At Play Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 9414,
					ProductID = 64250
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 9414,
					ProductID = 64251
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 3695,
					ProductID = 690
				}
			};
		}
	}
}
