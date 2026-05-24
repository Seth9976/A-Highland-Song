using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x0200016D RID: 365
	[Preserve]
	[NativeInputDeviceProfile]
	public class MadCatzBrawlStickMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x060007CE RID: 1998 RVA: 0x0003B13C File Offset: 0x0003933C
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Mad Catz Brawl Stick";
			base.DeviceNotes = "Mad Catz Brawl Stick on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 7085,
					ProductID = 61465
				}
			};
		}
	}
}
