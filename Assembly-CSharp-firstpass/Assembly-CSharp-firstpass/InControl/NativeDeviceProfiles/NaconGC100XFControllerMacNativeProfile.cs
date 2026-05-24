using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000191 RID: 401
	[Preserve]
	[NativeInputDeviceProfile]
	public class NaconGC100XFControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000816 RID: 2070 RVA: 0x0003C724 File Offset: 0x0003A924
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Nacon GC-100XF Controller";
			base.DeviceNotes = "Nacon GC-100XF Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 4553,
					ProductID = 22000
				}
			};
		}
	}
}
