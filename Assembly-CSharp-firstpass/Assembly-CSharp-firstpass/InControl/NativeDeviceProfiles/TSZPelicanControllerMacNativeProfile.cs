using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x020001BD RID: 445
	[Preserve]
	[NativeInputDeviceProfile]
	public class TSZPelicanControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x0600086E RID: 2158 RVA: 0x0003E75C File Offset: 0x0003C95C
		public override void Define()
		{
			base.Define();
			base.DeviceName = "TSZ Pelican Controller";
			base.DeviceNotes = "TSZ Pelican Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 3695,
					ProductID = 513
				}
			};
		}
	}
}
