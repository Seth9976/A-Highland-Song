using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000142 RID: 322
	[Preserve]
	[NativeInputDeviceProfile]
	public class HarmonixGuitarMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000778 RID: 1912 RVA: 0x00039940 File Offset: 0x00037B40
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Harmonix Guitar";
			base.DeviceNotes = "Harmonix Guitar on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 7085,
					ProductID = 5432
				}
			};
		}
	}
}
