using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x020001AF RID: 431
	[Preserve]
	[NativeInputDeviceProfile]
	public class RockBandDrumsMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000852 RID: 2130 RVA: 0x0003DEE0 File Offset: 0x0003C0E0
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Rock Band Drums";
			base.DeviceNotes = "Rock Band Drums on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 7085,
					ProductID = 3
				}
			};
		}
	}
}
