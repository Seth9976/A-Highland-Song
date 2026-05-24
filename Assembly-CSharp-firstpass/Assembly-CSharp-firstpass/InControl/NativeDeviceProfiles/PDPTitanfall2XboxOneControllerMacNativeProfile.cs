using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000197 RID: 407
	[Preserve]
	[NativeInputDeviceProfile]
	public class PDPTitanfall2XboxOneControllerMacNativeProfile : XboxOneDriverMacNativeProfile
	{
		// Token: 0x06000822 RID: 2082 RVA: 0x0003CD88 File Offset: 0x0003AF88
		public override void Define()
		{
			base.Define();
			base.DeviceName = "PDP Titanfall 2 Xbox One Controller";
			base.DeviceNotes = "PDP Titanfall 2 Xbox One Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 3695,
					ProductID = 357
				}
			};
		}
	}
}
