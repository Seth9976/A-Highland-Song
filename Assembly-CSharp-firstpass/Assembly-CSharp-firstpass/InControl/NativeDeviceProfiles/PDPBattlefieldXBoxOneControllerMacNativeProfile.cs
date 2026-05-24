using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000194 RID: 404
	[Preserve]
	[NativeInputDeviceProfile]
	public class PDPBattlefieldXBoxOneControllerMacNativeProfile : XboxOneDriverMacNativeProfile
	{
		// Token: 0x0600081C RID: 2076 RVA: 0x0003CC14 File Offset: 0x0003AE14
		public override void Define()
		{
			base.Define();
			base.DeviceName = "PDP Battlefield XBox One Controller";
			base.DeviceNotes = "PDP Battlefield XBox One Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 3695,
					ProductID = 356
				}
			};
		}
	}
}
