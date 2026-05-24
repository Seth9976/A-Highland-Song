using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x0200016B RID: 363
	[Preserve]
	[NativeInputDeviceProfile]
	public class MadCatzArcadeStickMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x060007CA RID: 1994 RVA: 0x0003B044 File Offset: 0x00039244
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Mad Catz Arcade Stick";
			base.DeviceNotes = "Mad Catz Arcade Stick on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 1848,
					ProductID = 18264
				}
			};
		}
	}
}
