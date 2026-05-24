using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000163 RID: 355
	[Preserve]
	[NativeInputDeviceProfile]
	public class LogitechChillStreamControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x060007BA RID: 1978 RVA: 0x0003AC24 File Offset: 0x00038E24
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Logitech Chill Stream Controller";
			base.DeviceNotes = "Logitech Chill Stream Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 1133,
					ProductID = 49730
				}
			};
		}
	}
}
