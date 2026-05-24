using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x020001A0 RID: 416
	[Preserve]
	[NativeInputDeviceProfile]
	public class PowerAMiniProExControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000834 RID: 2100 RVA: 0x0003D5E0 File Offset: 0x0003B7E0
		public override void Define()
		{
			base.Define();
			base.DeviceName = "PowerA Mini Pro Ex Controller";
			base.DeviceNotes = "PowerA Mini Pro Ex Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 5604,
					ProductID = 16128
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 9414,
					ProductID = 21274
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 9414,
					ProductID = 21248
				}
			};
		}
	}
}
