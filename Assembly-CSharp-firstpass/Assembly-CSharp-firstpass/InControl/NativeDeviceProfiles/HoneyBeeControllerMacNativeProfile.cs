using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000144 RID: 324
	[Preserve]
	[NativeInputDeviceProfile]
	public class HoneyBeeControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x0600077C RID: 1916 RVA: 0x00039A38 File Offset: 0x00037C38
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Honey Bee Controller";
			base.DeviceNotes = "Honey Bee Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 4779,
					ProductID = 21760
				}
			};
		}
	}
}
