using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000196 RID: 406
	[Preserve]
	[NativeInputDeviceProfile]
	public class PDPMetallicsLEControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x06000820 RID: 2080 RVA: 0x0003CD0C File Offset: 0x0003AF0C
		public override void Define()
		{
			base.Define();
			base.DeviceName = "PDP Metallics LE Controller";
			base.DeviceNotes = "PDP Metallics LE Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 3695,
					ProductID = 345
				}
			};
		}
	}
}
