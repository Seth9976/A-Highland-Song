using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000151 RID: 337
	[Preserve]
	[NativeInputDeviceProfile]
	public class HoriPadEXTurboControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000796 RID: 1942 RVA: 0x0003A2F4 File Offset: 0x000384F4
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Hori Pad EX Turbo Controller";
			base.DeviceNotes = "Hori Pad EX Turbo Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 3853,
					ProductID = 12
				}
			};
		}
	}
}
