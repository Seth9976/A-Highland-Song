using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000173 RID: 371
	[Preserve]
	[NativeInputDeviceProfile]
	public class MadCatzFightStickTESPlusMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x060007DA RID: 2010 RVA: 0x0003B524 File Offset: 0x00039724
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Mad Catz Fight Stick TES Plus";
			base.DeviceNotes = "Mad Catz Fight Stick TES Plus on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 7085,
					ProductID = 61506
				}
			};
		}
	}
}
