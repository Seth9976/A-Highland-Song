using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000189 RID: 393
	[Preserve]
	[NativeInputDeviceProfile]
	public class MicrosoftAdaptiveControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000806 RID: 2054 RVA: 0x0003BFCC File Offset: 0x0003A1CC
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Microsoft Adaptive Controller";
			base.DeviceNotes = "Microsoft Adaptive Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 1118,
					ProductID = 2826
				}
			};
		}
	}
}
