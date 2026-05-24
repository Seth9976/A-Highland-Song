using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x0200015D RID: 349
	[Preserve]
	[NativeInputDeviceProfile]
	public class HoriXbox360GemPadExMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x060007AE RID: 1966 RVA: 0x0003A940 File Offset: 0x00038B40
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Hori Xbox 360 Gem Pad Ex";
			base.DeviceNotes = "Hori Xbox 360 Gem Pad Ex on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 9414,
					ProductID = 21773
				}
			};
		}
	}
}
