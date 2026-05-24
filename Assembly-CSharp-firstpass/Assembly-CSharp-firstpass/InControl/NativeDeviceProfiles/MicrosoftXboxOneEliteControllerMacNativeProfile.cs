using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x0200018D RID: 397
	[Preserve]
	[NativeInputDeviceProfile]
	public class MicrosoftXboxOneEliteControllerMacNativeProfile : XboxOneDriverMacNativeProfile
	{
		// Token: 0x0600080E RID: 2062 RVA: 0x0003C4F4 File Offset: 0x0003A6F4
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Microsoft Xbox One Elite Controller";
			base.DeviceNotes = "Microsoft Xbox One Elite Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 1118,
					ProductID = 739
				}
			};
		}
	}
}
