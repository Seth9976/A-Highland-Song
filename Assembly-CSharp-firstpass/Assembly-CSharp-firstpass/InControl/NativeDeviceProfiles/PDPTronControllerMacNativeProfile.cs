using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000198 RID: 408
	[Preserve]
	[NativeInputDeviceProfile]
	public class PDPTronControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000824 RID: 2084 RVA: 0x0003CE04 File Offset: 0x0003B004
		public override void Define()
		{
			base.Define();
			base.DeviceName = "PDP Tron Controller";
			base.DeviceNotes = "PDP Tron Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 7085,
					ProductID = 63747
				}
			};
		}
	}
}
