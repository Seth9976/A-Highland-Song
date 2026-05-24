using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000162 RID: 354
	[Preserve]
	[NativeInputDeviceProfile]
	public class KonamiDancePadMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x060007B8 RID: 1976 RVA: 0x0003ABAC File Offset: 0x00038DAC
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Konami Dance Pad";
			base.DeviceNotes = "Konami Dance Pad on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 4779,
					ProductID = 4
				}
			};
		}
	}
}
