using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000188 RID: 392
	[Preserve]
	[NativeInputDeviceProfile]
	public class McbazelAdapterMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000804 RID: 2052 RVA: 0x0003BF50 File Offset: 0x0003A150
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Mcbazel Adapter";
			base.DeviceNotes = "Mcbazel Adapter on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 3695,
					ProductID = 654
				}
			};
		}
	}
}
