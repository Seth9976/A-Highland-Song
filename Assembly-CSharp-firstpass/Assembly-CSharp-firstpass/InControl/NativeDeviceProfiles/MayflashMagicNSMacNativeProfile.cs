using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000187 RID: 391
	[Preserve]
	[NativeInputDeviceProfile]
	public class MayflashMagicNSMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000802 RID: 2050 RVA: 0x0003BED4 File Offset: 0x0003A0D4
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Mayflash Magic-NS";
			base.DeviceNotes = "Mayflash Magic-NS on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 121,
					ProductID = 6355
				}
			};
		}
	}
}
