using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x020001B1 RID: 433
	[Preserve]
	[NativeInputDeviceProfile]
	public class RockCandyControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000856 RID: 2134 RVA: 0x0003DFD0 File Offset: 0x0003C1D0
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Rock Candy Controller";
			base.DeviceNotes = "Rock Candy Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 3695,
					ProductID = 287
				}
			};
		}
	}
}
