using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000141 RID: 321
	[Preserve]
	[NativeInputDeviceProfile]
	public class HarmonixDrumKitMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000776 RID: 1910 RVA: 0x000398C4 File Offset: 0x00037AC4
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Harmonix Drum Kit";
			base.DeviceNotes = "Harmonix Drum Kit on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 7085,
					ProductID = 4408
				}
			};
		}
	}
}
