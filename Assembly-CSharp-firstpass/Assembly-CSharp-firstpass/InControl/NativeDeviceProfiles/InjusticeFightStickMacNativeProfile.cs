using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x0200015F RID: 351
	[Preserve]
	[NativeInputDeviceProfile]
	public class InjusticeFightStickMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x060007B2 RID: 1970 RVA: 0x0003AA38 File Offset: 0x00038C38
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Injustice Fight Stick";
			base.DeviceNotes = "Injustice Fight Stick on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 3695,
					ProductID = 293
				}
			};
		}
	}
}
