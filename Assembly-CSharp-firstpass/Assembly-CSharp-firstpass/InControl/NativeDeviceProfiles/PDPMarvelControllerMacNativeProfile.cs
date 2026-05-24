using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000195 RID: 405
	[Preserve]
	[NativeInputDeviceProfile]
	public class PDPMarvelControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x0600081E RID: 2078 RVA: 0x0003CC90 File Offset: 0x0003AE90
		public override void Define()
		{
			base.Define();
			base.DeviceName = "PDP Marvel Controller";
			base.DeviceNotes = "PDP Marvel Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 3695,
					ProductID = 327
				}
			};
		}
	}
}
