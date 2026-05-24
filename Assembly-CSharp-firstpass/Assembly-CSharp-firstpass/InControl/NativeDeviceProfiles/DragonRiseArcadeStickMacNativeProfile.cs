using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x0200013B RID: 315
	[Preserve]
	[NativeInputDeviceProfile]
	public class DragonRiseArcadeStickMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x0600076A RID: 1898 RVA: 0x000394DC File Offset: 0x000376DC
		public override void Define()
		{
			base.Define();
			base.DeviceName = "DragonRise Arcade Stick";
			base.DeviceNotes = "DragonRise Arcade Stick on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 121,
					ProductID = 6268
				}
			};
		}
	}
}
