using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000174 RID: 372
	[Preserve]
	[NativeInputDeviceProfile]
	public class MadCatzFPSProMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x060007DC RID: 2012 RVA: 0x0003B5A0 File Offset: 0x000397A0
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Mad Catz FPS Pro";
			base.DeviceNotes = "Mad Catz FPS Pro on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 7085,
					ProductID = 61479
				}
			};
		}
	}
}
