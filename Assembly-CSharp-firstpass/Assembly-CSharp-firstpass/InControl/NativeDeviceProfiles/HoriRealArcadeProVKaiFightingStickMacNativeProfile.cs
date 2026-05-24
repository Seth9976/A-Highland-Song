using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x0200015A RID: 346
	[Preserve]
	[NativeInputDeviceProfile]
	public class HoriRealArcadeProVKaiFightingStickMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x060007A8 RID: 1960 RVA: 0x0003A750 File Offset: 0x00038950
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Hori Real Arcade Pro V Kai Fighting Stick";
			base.DeviceNotes = "Hori Real Arcade Pro V Kai Fighting Stick on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 9414,
					ProductID = 21774
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 3853,
					ProductID = 120
				}
			};
		}
	}
}
