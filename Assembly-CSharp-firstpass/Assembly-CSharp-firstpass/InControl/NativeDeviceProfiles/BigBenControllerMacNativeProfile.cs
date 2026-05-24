using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000138 RID: 312
	[Preserve]
	[NativeInputDeviceProfile]
	public class BigBenControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000764 RID: 1892 RVA: 0x00039368 File Offset: 0x00037568
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Big Ben Controller";
			base.DeviceNotes = "Big Ben Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 5227,
					ProductID = 1537
				}
			};
		}
	}
}
