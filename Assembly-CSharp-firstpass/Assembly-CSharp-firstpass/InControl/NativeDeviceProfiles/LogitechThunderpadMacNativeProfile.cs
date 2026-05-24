using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x0200016A RID: 362
	[Preserve]
	[NativeInputDeviceProfile]
	public class LogitechThunderpadMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x060007C8 RID: 1992 RVA: 0x0003AFC8 File Offset: 0x000391C8
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Logitech Thunderpad";
			base.DeviceNotes = "Logitech Thunderpad on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 1133,
					ProductID = 51848
				}
			};
		}
	}
}
