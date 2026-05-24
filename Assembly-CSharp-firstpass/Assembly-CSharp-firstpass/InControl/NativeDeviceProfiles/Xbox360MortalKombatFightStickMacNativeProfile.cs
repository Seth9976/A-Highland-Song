using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x020001BE RID: 446
	[Preserve]
	[NativeInputDeviceProfile]
	public class Xbox360MortalKombatFightStickMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000870 RID: 2160 RVA: 0x0003E7D8 File Offset: 0x0003C9D8
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Xbox 360 Mortal Kombat Fight Stick";
			base.DeviceNotes = "Xbox 360 Mortal Kombat Fight Stick on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 7085,
					ProductID = 63750
				}
			};
		}
	}
}
