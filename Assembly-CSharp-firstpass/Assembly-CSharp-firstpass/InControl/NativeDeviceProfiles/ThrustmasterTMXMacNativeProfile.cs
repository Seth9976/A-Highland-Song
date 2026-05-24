using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x020001BA RID: 442
	[Preserve]
	[NativeInputDeviceProfile]
	public class ThrustmasterTMXMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000868 RID: 2152 RVA: 0x0003E5EC File Offset: 0x0003C7EC
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Thrustmaster TMX";
			base.DeviceNotes = "Thrustmaster TMX on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 1103,
					ProductID = 46718
				}
			};
		}
	}
}
