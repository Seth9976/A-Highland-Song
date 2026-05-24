using System;

namespace InControl.NativeDeviceProfiles
{
	// Token: 0x0200013D RID: 317
	[Preserve]
	[NativeInputDeviceProfile]
	public class ElecomControllerMacNativeProfile : Xbox360DriverMacNativeProfile
	{
		// Token: 0x0600076E RID: 1902 RVA: 0x000395D4 File Offset: 0x000377D4
		public override void Define()
		{
			base.Define();
			base.DeviceName = "Elecom Controller";
			base.DeviceNotes = "Elecom Controller on Mac";
			base.Matchers = new InputDeviceMatcher[]
			{
				new InputDeviceMatcher
				{
					DriverType = InputDeviceDriverType.HID,
					VendorID = 1390,
					ProductID = 8196
				}
			};
		}
	}
}
