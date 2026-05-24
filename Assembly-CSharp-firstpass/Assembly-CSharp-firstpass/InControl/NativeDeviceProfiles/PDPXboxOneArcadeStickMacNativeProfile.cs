using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x0200019B RID: 411
	[Preserve]
	[NativeInputDeviceProfile]
	public class PDPXboxOneArcadeStickMacNativeProfile : XboxOneDriverMacNativeProfile
	{
		// Token: 0x0600082A RID: 2090 RVA: 0x0003CF78 File Offset: 0x0003B178
		public override void Define()
		{
			base.Define();
			base.DeviceName = "PDP Xbox One Arcade Stick";
			base.DeviceNotes = "PDP Xbox One Arcade Stick on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 3695,
					ProductID = 348
				}
			};
		}
	}
}
