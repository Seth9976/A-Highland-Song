using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x0200018E RID: 398
	[Preserve]
	[NativeInputDeviceProfile]
	public class MKKlassikFightStickMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000810 RID: 2064 RVA: 0x0003C570 File Offset: 0x0003A770
		public override void Define()
		{
			base.Define();
			base.DeviceName = "MK Klassik Fight Stick";
			base.DeviceNotes = "MK Klassik Fight Stick on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 4779,
					ProductID = 771
				}
			};
		}
	}
}
