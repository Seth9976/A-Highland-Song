using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x0200014D RID: 333
	[Preserve]
	[NativeInputDeviceProfile]
	public class HoriFightingStickEX2MacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x0600078E RID: 1934 RVA: 0x0003A04C File Offset: 0x0003824C
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Hori Fighting Stick EX2";
			base.DeviceNotes = "Hori Fighting Stick EX2 on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 3853,
					ProductID = 10
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 7085,
					ProductID = 62725
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 3853,
					ProductID = 13
				}
			};
		}
	}
}
