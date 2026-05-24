using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x020001B2 RID: 434
	[Preserve]
	[NativeInputDeviceProfile]
	public class RockCandyPS3ControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000858 RID: 2136 RVA: 0x0003E04C File Offset: 0x0003C24C
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Rock Candy PS3 Controller";
			base.DeviceNotes = "Rock Candy PS3 Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 3695,
					ProductID = 286
				}
			};
		}
	}
}
