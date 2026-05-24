using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000143 RID: 323
	[Preserve]
	[NativeInputDeviceProfile]
	public class HarmonixKeyboardMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x0600077A RID: 1914 RVA: 0x000399BC File Offset: 0x00037BBC
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Harmonix Keyboard";
			base.DeviceNotes = "Harmonix Keyboard on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 7085,
					ProductID = 4920
				}
			};
		}
	}
}
