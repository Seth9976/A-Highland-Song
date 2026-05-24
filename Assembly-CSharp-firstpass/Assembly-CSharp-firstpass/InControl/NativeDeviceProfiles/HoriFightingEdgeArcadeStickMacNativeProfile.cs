using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x0200014C RID: 332
	[Preserve]
	[NativeInputDeviceProfile]
	public class HoriFightingEdgeArcadeStickMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x0600078C RID: 1932 RVA: 0x00039FD0 File Offset: 0x000381D0
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Hori Fighting Edge Arcade Stick";
			base.DeviceNotes = "Hori Fighting Edge Arcade Stick on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 9414,
					ProductID = 21763
				}
			};
		}
	}
}
