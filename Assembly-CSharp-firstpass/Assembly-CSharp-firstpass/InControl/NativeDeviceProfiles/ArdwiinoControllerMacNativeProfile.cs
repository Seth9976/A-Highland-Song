using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x02000134 RID: 308
	[Preserve]
	[NativeInputDeviceProfile]
	public class ArdwiinoControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x0600075C RID: 1884 RVA: 0x000390F8 File Offset: 0x000372F8
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Ardwiino Controller";
			base.DeviceNotes = "Ardwiino Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 4617,
					ProductID = 10370
				}
			};
		}
	}
}
