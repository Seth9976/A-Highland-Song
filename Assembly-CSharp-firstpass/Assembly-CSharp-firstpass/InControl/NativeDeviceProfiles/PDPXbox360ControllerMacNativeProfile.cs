using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x0200019A RID: 410
	[Preserve]
	[NativeInputDeviceProfile]
	public class PDPXbox360ControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000828 RID: 2088 RVA: 0x0003CEFC File Offset: 0x0003B0FC
		public override void Define()
		{
			base.Define();
			base.DeviceName = "PDP Xbox 360 Controller";
			base.DeviceNotes = "PDP Xbox 360 Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 3695,
					ProductID = 1281
				}
			};
		}
	}
}
