using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000148 RID: 328
	[Preserve]
	[NativeInputDeviceProfile]
	public class HoriEdgeFightingStickMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000784 RID: 1924 RVA: 0x00039D24 File Offset: 0x00037F24
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Hori Edge Fighting Stick";
			base.DeviceNotes = "Hori Edge Fighting Stick on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 3853,
					ProductID = 109
				}
			};
		}
	}
}
