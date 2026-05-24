using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x0200014A RID: 330
	[Preserve]
	[NativeInputDeviceProfile]
	public class HORIFightingCommanderControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000788 RID: 1928 RVA: 0x00039E98 File Offset: 0x00038098
		public override void Define()
		{
			base.Define();
			base.DeviceName = "HORI Fighting Commander Controller";
			base.DeviceNotes = "HORI Fighting Commander Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 3853,
					ProductID = 134
				}
			};
		}
	}
}
