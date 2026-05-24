using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x0200018C RID: 396
	[Preserve]
	[NativeInputDeviceProfile]
	public class MicrosoftXboxOneControllerMacNativeProfile : XboxOneDriverMacNativeProfile
	{
		// Token: 0x0600080C RID: 2060 RVA: 0x0003C438 File Offset: 0x0003A638
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Microsoft Xbox One Controller";
			base.DeviceNotes = "Microsoft Xbox One Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 1118,
					ProductID = 721
				},
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 1118,
					ProductID = 733
				}
			};
		}
	}
}
