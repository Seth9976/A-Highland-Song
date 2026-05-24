using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000172 RID: 370
	[Preserve]
	[NativeInputDeviceProfile]
	public class MadCatzFightStickTE2MacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x060007D8 RID: 2008 RVA: 0x0003B4A8 File Offset: 0x000396A8
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Mad Catz Fight Stick TE2";
			base.DeviceNotes = "Mad Catz Fight Stick TE2 on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 7085,
					ProductID = 61568
				}
			};
		}
	}
}
