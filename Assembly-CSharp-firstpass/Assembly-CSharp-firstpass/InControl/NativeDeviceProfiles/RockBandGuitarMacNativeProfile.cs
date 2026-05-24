using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x020001B0 RID: 432
	[Preserve]
	[NativeInputDeviceProfile]
	public class RockBandGuitarMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000854 RID: 2132 RVA: 0x0003DF58 File Offset: 0x0003C158
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Rock Band Guitar";
			base.DeviceNotes = "Rock Band Guitar on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 7085,
					ProductID = 2
				}
			};
		}
	}
}
